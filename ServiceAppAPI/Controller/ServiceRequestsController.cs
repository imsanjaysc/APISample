using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Model;
using DAL;
using BAL;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ServiceAppAPI.Controllers
{
    public class ServiceRequestsController : ApiController
    {
        [Authorize]
        [Route("api/AddtoServiceRequests")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddtoServiceRequests(ServiceRequests service)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var clm = identity.Claims
                       .Where(c => c.Type == ClaimTypes.Sid)
                       .Select(c => c.Value);
                string Userid = clm.Single();
                string val="";
                var res = new ServiceRequestID();
                if (service != null)
                {
                    res = new UserData().AddtoServiceRequests(service, Convert.ToInt64(Userid));
                    if (res!=null && res.FirstName !=null && res.MobileNo!=null && res.RequestID!= null)
                    {
                        OTP sendtext = new OTP();
                        sendtext.SendRequestDetailsToCustomer(res.MobileNo, res.RequestID);
                        sendtext.SendRequestDetailsToAdmin("9999916843", res.FirstName, res.MobileNo, res.Email, Convert.ToString(service.RequestTime), res.Address, res.RequestID);
                        SendSMSEmail sse = new SendSMSEmail();
                        MailMessage msg = new MailMessage();
                        string Subject = "New Service Request Received";
                        string Body = "Hi, <br /><br /> <b>Name:</b> " + res.FirstName + ", <br /><b>Mobile Number:</b> " + res.MobileNo + ", <br /><b>Book Date:</b> " + res.CreatedDate + ",  <br /><b>Request Date:</b> " + service.RequestTime + ", <br /><b>Address:</b> " + res.Address + ",<br /><b>Booking ID:</b> " + res.RequestID + ",<br /><b>Refferal Code:</b> " + service.RefferalCode + ",<br /><b>Comment:</b> " + service.Comments + ",<br /><b>Request Services:</b> " + res.ServicesName + "";
                        //msg.IsBodyHtml = true;
                        //msg.Body = Body;
                        //await sse.SendEmail("Fixybee", Subject, Body);
                        await sse.SendEmailToAdminviaPostmarkapp(Subject, Body);
                        val = res.RequestID.ToString().Trim();
                        if(res.IsEmailVerified && res.Email!=null)
                        {
                            Subject = "Fixybee Services";
                            Body = "Hi " + res.FirstName + ", <br /><br/> Your request has been submitted,  we will get back to you shortly, your Booking ID is "+ res.RequestID+".<br/>Thanks for choosing Fixybee";
                            await sse.SendEmailToCustomerviaPostmarkapp(res.Email, Subject, Body);
                        }
                    }
                }
                else
                {
                    val = "Null";
                }
                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.OK, new ServiceResponse<string> { IsError = false, ResponseObject = val , ResponseCode= 200 });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
            }
        }


        [Authorize]
        [Route("api/GetServiceRequestHistory")]
        [HttpGet]
        public HttpResponseMessage GetServiceRequestHistory()
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var clm = identity.Claims
                       .Where(c => c.Type == ClaimTypes.Sid)
                       .Select(c => c.Value);
                string Userid = clm.Single();
                if (Userid != "0" && Userid!=null)
                {
                    var response = new List<ServiceHistory>();
                    response = new UserData().GetServiceRequestHistory(Convert.ToInt64(Userid));
                    if (response != null)
                        return Request.CreateResponse<ServiceResponse<List<ServiceHistory>>>(HttpStatusCode.OK, new ServiceResponse<List<ServiceHistory>> { IsError = false, ResponseObject = response, ResponseCode=200 });
                    else
                        return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.NotFound, new ServiceResponse<string> { IsError = false, ResponseObject = "No previous requests found" });
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
    }
}