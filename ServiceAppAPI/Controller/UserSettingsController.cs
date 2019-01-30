using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model;
using DataLayer;
using System.Security.Claims;
using DAL;

namespace WebApiRestService.Controllers
{
    public class UserSettingsController : ApiController
    {
        [Authorize]
        [Route("api/AddUserSettings")]
        [HttpPost]
        public HttpResponseMessage AddUserSettings(List<UserSettings> setting)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var clm = identity.Claims
                       .Where(c => c.Type == ClaimTypes.Sid)
                       .Select(c => c.Value);
                string Userid = clm.Single();
                if (Userid != "0")
                {
                    int val = 0;
                    val = new UserSettingData().UpdateUserSetting(setting, Convert.ToInt64(Userid));
                    return Request.CreateResponse<ServiceResponse<int>>(HttpStatusCode.OK, new ServiceResponse<int> { IsError = false, ResponseObject = val });
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

        //[Authorize]
        //[Route("api/GetUserSettings")]
        //[HttpGet]
        //public HttpResponseMessage GetUserSettings()
        //{
        //    try
        //    {
        //        var identity = (ClaimsIdentity)User.Identity;
        //        IEnumerable<Claim> claims = identity.Claims;
        //        var clm = identity.Claims
        //               .Where(c => c.Type == ClaimTypes.Sid)
        //               .Select(c => c.Value);
        //        string Userid = clm.Single();
        //        if (Userid != "0")
        //        {
        //            var response = new List<UserSettings>();
        //            response = new UserData().GetUserSettings(Convert.ToInt64(Userid));
        //            if (response != null)
        //                return Request.CreateResponse<ServiceResponse<List<UserSettings>>>(HttpStatusCode.OK, new ServiceResponse<List<UserSettings>> { IsError = false, ResponseObject = response });
        //            else
        //                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.NotFound, new ServiceResponse<string> { IsError = false, ResponseObject = "Data not Found" });
        //        }
        //        else
        //        {
        //            return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.BadRequest, new ServiceResponse<string> { IsError = false, ResponseObject = "Parameteres Missing" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
        //    }
        //}

    }
}
