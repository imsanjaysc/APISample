using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Model;
using DataLayer;
using DAL;

namespace WebApiRestService.Controllers
{
    public class UserLocationController : ApiController
    {

        [Authorize]
        [Route("api/UpdateUserLocations")]
        [HttpPost]
        public HttpResponseMessage UpdateUserLocations(UserLocations locations)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var clm = identity.Claims
                       .Where(c => c.Type == ClaimTypes.Sid)
                       .Select(c => c.Value);
                string Userid = clm.Single();
                locations.CustomerID = Convert.ToInt64(Userid);
                int val;
                if (locations != null && locations.CustomerID != 0)
                {
                    val = new UserData().UpdateUserLocation(locations, Convert.ToInt64(Userid));
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
        [Route("api/GetUserLocation")]
        [HttpGet]
        public HttpResponseMessage GetUserLocation()
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
                    var response = new ServiceResponse<UserLocations>
                    {
                        ResponseObject = new UserData().GetUserLocation(Convert.ToInt64(Userid))
                    };
                    if (response != null)
                        return Request.CreateResponse<ServiceResponse<UserLocations>>(HttpStatusCode.OK, response);
                    else
                        return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.NotFound, new ServiceResponse<string> { IsError = false, ResponseObject = "Data not Found" });
                }
                else
                {
                    return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.BadRequest, new ServiceResponse<string> { IsError = true, ResponseObject = "Parameteres Missing" });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
            }
        }
    }
}
