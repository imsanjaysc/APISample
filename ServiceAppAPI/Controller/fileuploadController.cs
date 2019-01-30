using DAL;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TinifyAPI;
using WebApiRestService.Models;

namespace WebApiRestService.Controllers
{
    public class fileuploadController : ApiController
    {
        [Authorize]
        [Route("api/Files/Upload/{source}")]
        [HttpPost]
        public async Task<string> Post(string source)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var clm = identity.Claims
                       .Where(c => c.Type == ClaimTypes.Sid)
                       .Select(c => c.Value);
                string Userid = clm.Single();
                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];

                        var fileName = postedFile.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();
                        string ext = System.IO.Path.GetExtension(fileName);
                        // var filePath = HttpContext.Current.Server.MapPath("~/Uploads/" + fileName);
                        var filePath = ConfigurationManager.AppSettings["ImagePath"];
                        //var filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/images/");
                        var ImageUrl = ConfigurationManager.AppSettings["ImageUrl"];
                        string name = DateTime.Now.ToString("ddmmyyyyMMsshhfff") + ext;
                        //string name_resized = DateTime.Now.ToString("ddmmyyyyMMsshhfff") + "_resized" + ext;
                        //string filepathResized = fileName + name_resized;
                        filePath = filePath + name;
                       // postedFile.SaveAs(filePath);
                        ImageHandler image = new ImageHandler();
                        Bitmap bmp = new Bitmap(postedFile.InputStream);
                        //Flip Image
                        if (source.ToUpper() == "IOS")
                        {
                            //bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            Image originalImage = Image.FromStream((postedFile.InputStream));

                            if (originalImage.PropertyIdList.Contains(0x0112))
                            {
                                int rotationValue = originalImage.GetPropertyItem(0x0112).Value[0];
                                switch (rotationValue)
                                {
                                    case 1: // landscape, do nothing
                                        break;

                                    case 8: // rotated 90 right
                                            // de-rotate:
                                        originalImage.RotateFlip(rotateFlipType: RotateFlipType.Rotate270FlipNone);
                                        break;

                                    case 3: // bottoms up
                                        originalImage.RotateFlip(rotateFlipType: RotateFlipType.Rotate180FlipNone);
                                        break;

                                    case 6: // rotated 90 left
                                        originalImage.RotateFlip(rotateFlipType: RotateFlipType.Rotate90FlipNone);
                                        break;
                                }
                            }
                            Bitmap bmp1 = new Bitmap(originalImage);
                            image.Save(bmp1, 300, 300, 70, filePath);
                        }
                        else
                        {
                            image.Save(bmp, 300, 300, 70, filePath);
                        }
                        int val = new UserData().UpdateUserProfilePic(Convert.ToInt64(Userid), name, name);
                        return ImageUrl + "Profilepic/" + name;
                    }
                }
            }
            catch (System.Exception exception)
            {
                return exception.Message;
            }

            return "no files";
        }
        public async void ResizeImage(string sFilePath, string sOptimizedFile)
        {
            String API_Key = ConfigurationManager.AppSettings["TinyfyApiKey"];
            Tinify.Key = API_Key; //TinyPNG Developer API KEY

            var source = Tinify.FromFile(sFilePath);
            var resized = source.Resize(new
            {
                method = "fit",
                width = 250,
                height = 250
            });
            await resized.ToFile(sOptimizedFile);
        }
    }
}


