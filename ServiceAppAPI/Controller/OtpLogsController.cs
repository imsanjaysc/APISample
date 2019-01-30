using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DAL;
using Model;

namespace WebApiRestService.Controllers
{
    public class OtpLogsController : ApiController
    {
        [HttpGet]
        [Route("api/otp/sendotp/{mobile}")]
        public HttpResponseMessage SendOTP(string mobile)
        {
            try
            {
                if (mobile == null)
                {
                    return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.BadRequest, new ServiceResponse<string> { IsError = true, ResponseObject = "Invalid Request!" });
                }
                int Val = new UserData().CheckExistMobileNumber(mobile);
                if (Val == 0)
                {
                    BAL.OTP otp = new BAL.OTP();
                    string code = otp.SendOTP(mobile);
                    return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.OK, new ServiceResponse<string> { IsError = false, ResponseObject = code });
                }
                else
                {
                    return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.Unauthorized, new ServiceResponse<string> { IsError = false, ResponseObject = "This mobile number is already registered. please try with other number." });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
            }
        }
    }
}