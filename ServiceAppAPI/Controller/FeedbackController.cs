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
using System.Net.Mail;
using System.IO;
using System.Configuration;
using System.Threading.Tasks;

namespace WebApiRestService.Controllers
{
    public class FeedbackController : ApiController
    {
        //private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        //ErrorLog error = new ErrorLog();
        [Authorize]
        [Route("api/Submitfeedback")]
        [HttpPost]
        public async Task<HttpResponseMessage> Submitfeedback(Feedback feedback)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var clm = identity.Claims
                       .Where(c => c.Type == ClaimTypes.Sid)
                       .Select(c => c.Value);
                string Userid = clm.Single();
                int val = 0;
                if (feedback != null && Userid != "")
                {
                    SendSMSEmail sse = new SendSMSEmail();
                    MailMessage msg = new MailMessage();
                    string Subject = "New Feedback Message Received";
                    string Body = "Hi, <br /><br /> <b>Name:</b> " + feedback.Name + ", <br /><b>Mobile Number:</b> " + feedback.MobileNo + ", <br /><b>Email ID:</b> " + feedback.EmailID + ", <br /><b>Message:</b> " + feedback.Message + "";
                    //msg.IsBodyHtml = true;
                    //msg.Body = Body;
                    val = await sse.SendEmailToAdminviaPostmarkapp(Subject, Body);
                }
                else
                {
                    val = 0;
                }
                return Request.CreateResponse<ServiceResponse<int>>(HttpStatusCode.OK, new ServiceResponse<int> { IsError = false, ResponseObject = val });
            }
            catch (Exception ex)
            {
                //await Task.WhenAll(Task.Run(() => error.Exceptionlog(ex)));
                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
            }
        }
    }
}
