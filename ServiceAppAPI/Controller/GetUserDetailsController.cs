using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataLayer;
using System.Security.Claims;
using DAL;

namespace WebApiRestService.Controllers
{
    public class GetUserDetailsController : ApiController
    {
        // GET: GetUserDetails
        //[Authorize]
        [Route("api/GetCategoriesList")]
        [HttpGet]
        public HttpResponseMessage GetCategoriesList()
        {
            try
            {
              
                    var response = new ServiceResponse<ServiceList>
                    {
                        ResponseObject = new ProductServices().GetCategoriesList()
                    };
                    if (response != null)
                        return Request.CreateResponse<ServiceResponse<ServiceList>>(HttpStatusCode.OK, response);
                    else
                        return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.NotFound, new ServiceResponse<string> { IsError = false, ResponseObject = "Data not Found" });
                //}
                //else
                //{
                //    return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.BadRequest, new ServiceResponse<string> { IsError = false, ResponseObject = "Parameteres Missing" });
                //}
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
            }
        }

        [Authorize]
        [Route("api/GetProductServiceslist/{CategoryID}")]
        [HttpGet]
        public HttpResponseMessage GetProductServiceslist(int CategoryID)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var clm = identity.Claims
                       .Where(c => c.Type == ClaimTypes.Sid)
                       .Select(c => c.Value);
                string Userid = clm.Single();
                if (Userid != "0" && CategoryID != 0)
                {
                    var response = new List<ProductServiceslist>();
                    response = new ProductServices().GetProductServiceslist(Convert.ToInt64(Userid), CategoryID);
                    if (response != null)
                        return Request.CreateResponse<ServiceResponse<List<ProductServiceslist>>>(HttpStatusCode.OK, new ServiceResponse<List<ProductServiceslist>> { IsError = false, ResponseObject = response });
                    else
                        return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.NotFound, new ServiceResponse<string> { IsError = false, ResponseObject = "Data not Found" });
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
        [Route("api/GetCustomerAddressList")]
        [HttpGet]
        public HttpResponseMessage GetCustomerAddressList()
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
                    var response = new List<CustAddressList>();
                    response = new UserData().GetAddresslist(Convert.ToInt64(Userid));
                    if (response != null)
                        return Request.CreateResponse<ServiceResponse<List<CustAddressList>>>(HttpStatusCode.OK, new ServiceResponse<List<CustAddressList>> { IsError = false, ResponseObject = response });
                    else
                        return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.NotFound, new ServiceResponse<string> { IsError = false, ResponseObject = "Data not Found" });
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
        [Route("api/GetProfileDetails")]
        [HttpGet]
        public HttpResponseMessage GetProfileDetails()
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
                    var response = new ServiceResponse<UserInfo>
                    {
                        ResponseObject = new UserData().GetProfileDetails(Convert.ToInt64(Userid), null, "userid")
                    };
                    if (response != null)
                        return Request.CreateResponse<ServiceResponse<UserInfo>>(HttpStatusCode.OK, response);
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

        [Route("api/GetProfileDetails/{Mobilenumber}")]
        [HttpGet]
        public HttpResponseMessage GetProfileDetails(string Mobilenumber)
        {
            try
            {
                if (Mobilenumber != null)
                {
                    var response = new ServiceResponse<UserInfo>
                    {
                        ResponseObject = new UserData().GetProfileDetails(0, Mobilenumber,"mobile")
                    };
                    if (response != null)
                        return Request.CreateResponse<ServiceResponse<UserInfo>>(HttpStatusCode.OK, response);
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