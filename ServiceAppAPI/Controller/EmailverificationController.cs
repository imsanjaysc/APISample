using BAL;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceAppAPI.Controllers
{
    public class EmailverificationController : Controller
    {
        // GET: Emailverification
        //[Route("fixybee/emailverify")]
        //[HttpGet]
        //public ActionResult Index(string id)
        //{
        //    try
        //    {
        //        if (id != "")
        //        {
        //            Encrypt ee = new Encrypt();
        //            string key = ee.DecryptStr(Convert.ToString(id));
        //            int val;
        //            val = new UserData().EmailVerification(Convert.ToInt32(key));
        //            if (val != 0)
        //            {
        //                return Content("Email Succesfully Verified!");
        //                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.OK, new ServiceResponse<string> { IsError = false, ResponseObject = "Email Succesfully Verified!" });
        //            }
        //            else
        //            {
        //                return Content("Some Error Occured!");
        //                return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.OK, new ServiceResponse<string> { IsError = false, ResponseObject = "Some Error Occured!" });
        //            }

        //        }
        //        else
        //        {
        //            return View();
        //            return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.BadRequest, new ServiceResponse<string> { IsError = false, ResponseObject = "Parameteres Missing" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return View();
        //        return Request.CreateResponse<ServiceResponse<string>>(HttpStatusCode.InternalServerError, new ServiceResponse<string> { IsError = true, ExceptionObject = new ExceptionModel { ErrorMessage = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source } });
        //    }

        //}

        public ActionResult Index1()
        {
            return Content("hi there!");
        }
    }
}