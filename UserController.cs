using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Drawing;
using System.Drawing.Imaging;
using Model;
using DAL;
using System.Net.Mail;
using BAL;
using System.Net.Http.Headers;

namespace WebApiRestService.Controllers
{
    public class UserController : ApiController
    {
        //[Authorize]
        [Route("api/SubmitUserInfo")]
        [HttpPost]
        public async Task<HttpResponseMessage> SubmitUserInfo(UserInfo user)
        {
            try
            {
                int val;
                if (user != null)
                {
                    val = new UserData().SubmitUserInfo(user);
                    if(user.Email!=null && val!=0)
                    {
                        SendSMSEmail sse = new SendSMSEmail();
                        MailMessage msg = new MailMessage();
                        string Subject = "Fixybee Email Verification";
                        string content = Encrypturl(val);
                        string Body = content;
                        //Task.Factory.StartNew(() => sse.SendEmail(user.FirstName, Subject, Body));
                       await sse.SendEmailToCustomerviaPostmarkapp(user.Email, Subject, Body);
                    }
                }
                else
                {
                    val = 0;
                }
                return Request.CreateResponse<ServiceResponse<int>>(HttpStatusCode.OK, new ServiceResponse<int> { IsError = false, ResponseObject = val});
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
            }
        }

        private string Encrypturl(int id)
        {
            Encrypt ee = new Encrypt();
            string key = ee.EncryptStr(Convert.ToString(id));
            string link = "http://fixybee.aphroecs.com/fixybee/emailverify/" + key;
            string content = string.Empty;
            //content = File.ReadAllText(Convert.ToString(ConfigurationManager.AppSettings["VerifyEmailBodyPath"]));
            //content = "http://fixybee.aphroecs.com/verifymail.htm";
            //string localpath = new Uri(content).LocalPath;
            using (WebClient client = new WebClient())
            {
                content = client.DownloadString(ConfigurationManager.AppSettings["VerifyEmailBodyPath"]);
            }
            content = content.Replace("{VerifyURL}", link);
            return content;
        }

        [Authorize]
        [Route("api/EmailVerification")]
        [HttpPost]
        public async Task<HttpResponseMessage> EmailVerification(UserInfo user)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var clm = identity.Claims
                       .Where(c => c.Type == ClaimTypes.Sid)
                       .Select(c => c.Value);
                string Userid = clm.Single();
                int val;
                SendSMSEmail sse = new SendSMSEmail();
                MailMessage msg = new MailMessage();
                string Subject = "Fixybee Email Verification";
                string content = Encrypturl(Convert.ToInt32(Userid));
                string Body = content;
                val = await sse.SendEmailToCustomerviaPostmarkapp(user.Email, Subject, Body);
                return Request.CreateResponse<ServiceResponse<int>>(HttpStatusCode.OK, new ServiceResponse<int> { IsError = false, ResponseObject = val });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
            }
        }

        [Route("fixybee/emailverify/{id}")]
        [HttpGet]
        public HttpResponseMessage EmailVerification(string id)
        {
            try
            {
                if (id != "")
                {
                    var response = new HttpResponseMessage();
                    Encrypt ee = new Encrypt();
                    string key = ee.DecryptStr(Convert.ToString(id));
                    int val;
                    val = new UserData().EmailVerification(Convert.ToInt32(key));
                    if (val != 0)
                    {
                        string content = string.Empty;
                        using (WebClient client = new WebClient())
                        {
                            content = client.DownloadString(ConfigurationManager.AppSettings["SuccessEmailVerifyBodyPath"]);
                        }
                        //content = File.ReadAllText(Convert.ToString(ConfigurationManager.AppSettings["SuccessEmailVerifyBodyPath"]));
                        response.Content= new StringContent(content);
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                        return response;
                        //return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.OK, new ServiceResponse<string> { IsError = false, ResponseObject = "Email Succesfully Verified!" });
                    }
                    else
                    {
                        response.Content = new StringContent("<html><body>Some error occured! Please try again...</body></html>");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                        return response;
                        //return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.OK, new ServiceResponse<string> { IsError = false, ResponseObject = "Some Error Occured!" });
                    }

                }
                else
                {
                    return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.BadRequest, new ServiceResponse<string> { IsError = false, ResponseObject = "Parameteres Missing" });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
            }
        }

        [Authorize]
        [Route("api/UpdateUserProfile")]
        [HttpPost]
        public HttpResponseMessage UpdateUserProfile(UserInfo profile)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var clm = identity.Claims
                       .Where(c => c.Type == ClaimTypes.Sid)
                       .Select(c => c.Value);
                string Userid = clm.Single();
                profile.CustomerID = Convert.ToInt64(Userid);
                int val;
                if (profile != null && profile.CustomerID!=0)
                {
                    val = new UserData().UpdateUserProfile(profile);
                }
                else
                {
                    val = 0;
                }
                return Request.CreateResponse<ServiceResponse<int>>(HttpStatusCode.OK, new ServiceResponse<int> { IsError = false, ResponseObject = val });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
            }
        }

        [Authorize]
        [Route("api/UpdatePassword")]
        [HttpPost]
        public HttpResponseMessage UpdatePassword(UpdatePassword pass)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var clm = identity.Claims
                       .Where(c => c.Type == ClaimTypes.Sid)
                       .Select(c => c.Value);
                string Userid = clm.Single();
                pass.CustomerID = Convert.ToInt64(Userid);
                int val;
                string str;
                if (pass != null && pass.CustomerID != 0)
                {
                    val = new UserData().UpdatePassword(pass);
                    if (val == 1)
                    {
                        str = "Your password has been changed successfully!";
                        return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.OK, new ServiceResponse<string> { IsError = false, ResponseObject = str });
                    }
                    else
                    {
                        str = "Old password does not match!";
                        return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.NotFound, new ServiceResponse<string> { IsError = false, ResponseObject = str });
                    }
                }
                else
                {
                    str = "Some error occurred!! please try again after sometime.";
                    return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.OK, new ServiceResponse<string> { IsError = false, ResponseObject = str });
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
            }
        }

        private string WriteImage(byte[] arr)
        {
            var filename = $@"images\{DateTime.Now.Ticks}.";

            using (var im = Image.FromStream(new MemoryStream(arr)))
            {
                ImageFormat frmt;
                if (ImageFormat.Png.Equals(im.RawFormat))
                {
                    filename += "png";
                    frmt = ImageFormat.Png;
                }
                else
                {
                    filename += "jpg";
                    frmt = ImageFormat.Jpeg;
                }
                string path = HttpContext.Current.Server.MapPath("~/") + filename;
                im.Save(path, frmt);
            }

            return $@"http:\\{Request.RequestUri.Host}\{filename}";
        }

        [Route("api/UploadImage")]
        [HttpPost]
        public IHttpActionResult UploadImage(ImageModel model)
        {
            var imgUrl = WriteImage(model.Bytes);
            return Ok(imgUrl);
            // Some code
        }

        [Route("api/Forgotpassword/{Mobileno}")]
        [HttpGet]
        public HttpResponseMessage ForgotPassword(string Mobileno)
        {
            try
            {
                if (Mobileno != "")
                {
                    string val = "";
                    string pass = "0";
                    val = new UserData().ForgotPassword(Mobileno);
                    if (val!="")
                    {
                        BAL.OTP otp = new BAL.OTP();
                        pass = otp.SendPassword(Mobileno, val);
                        return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.OK, new ServiceResponse<string> { IsError = false, ResponseObject = pass });
                    }
                    else
                    {
                        return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.OK, new ServiceResponse<string> { IsError = false, ResponseObject = pass });
                    }
                    
                }
                else
                {
                    return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.BadRequest, new ServiceResponse<string> { IsError = false, ResponseObject = "Parameteres Missing" });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
            }
        }

        [Authorize]
        [Route("api/InsertCustomerAddress")]
        [HttpPost]
        public HttpResponseMessage InsertCustomerAddress(CustAddress address)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var clm = identity.Claims
                       .Where(c => c.Type == ClaimTypes.Sid)
                       .Select(c => c.Value);
                string Userid = clm.Single();
                address.CustomerID = Convert.ToInt64(Userid);
                int val;
                if (address != null && address.CustomerID != 0)
                {
                    val = new UserData().InsertCustomerAddress(address);
                }
                else
                {
                    val = 0;
                }
                return Request.CreateResponse<ServiceResponse<int>>(HttpStatusCode.OK, new ServiceResponse<int> { IsError = false, ResponseObject = val });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
            }
        }

        [Authorize]
        [Route("api/UpdateCustomerAddress")]
        [HttpPost]
        public HttpResponseMessage UpdateCustomerAddress(CustAddress address)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var clm = identity.Claims
                       .Where(c => c.Type == ClaimTypes.Sid)
                       .Select(c => c.Value);
                string Userid = clm.Single();
                address.CustomerID = Convert.ToInt64(Userid);
                int val;
                if (address != null && address.CustomerID != 0 && address.AddressID !=0)
                {
                    val = new UserData().UpdateCustomerAddress(address);
                }
                else
                {
                    val = 0;
                }
                return Request.CreateResponse<ServiceResponse<int>>(HttpStatusCode.OK, new ServiceResponse<int> { IsError = false, ResponseObject = val });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
            }
        }

        [Authorize]
        [Route("api/DeleteCustomerAddress")]
        [HttpPost]
        public HttpResponseMessage DeleteCustomerAddress(List<DelCustAddress> address)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var clm = identity.Claims
                       .Where(c => c.Type == ClaimTypes.Sid)
                       .Select(c => c.Value);
                string Userid = clm.Single();
                int val;
                if (address != null && Convert.ToInt64(Userid) != 0)
                {
                    val = new UserData().DelCustomerAddress(address, Convert.ToInt64(Userid));
                }
                else
                {
                    val = 0;
                }
                return Request.CreateResponse<ServiceResponse<int>>(HttpStatusCode.OK, new ServiceResponse<int> { IsError = false, ResponseObject = val });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
            }
        }
    }
}
