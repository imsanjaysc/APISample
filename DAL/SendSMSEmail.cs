using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Model;
using Newtonsoft.Json;
/// <summary>
/// Summary description for SendSMSEmail
/// </summary>
/// 

namespace DAL
{
    public class SendSMSEmail
    {
        HttpClient _client;
        public async Task<int> SendSMS(string Mobileno, string msg)
        {
            try
            {
                using (_client = new HttpClient())
                {
                    HttpResponseMessage response = await _client.GetAsync("http://api.msg91.com/api/sendhttp.php?country=91&sender=fixybe&route=4&mobiles=" + Mobileno + "&authkey=239356AlghlwxUDNdI5bab30a7&message=" + msg);
                    return Convert.ToInt32(response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> SendEmailToAdminviaPostmarkapp(string Subject, string Body)
        {
            try
            {
                using (_client = new HttpClient())
                {
                    EmailviaPostmarkapp Mail = new EmailviaPostmarkapp();
                    Mail.From = Convert.ToString(ConfigurationManager.AppSettings["EmailSenderMailId"]);
                    Mail.To = Convert.ToString(ConfigurationManager.AppSettings["EmailToAdmin1"]) + "," + Convert.ToString(ConfigurationManager.AppSettings["EmailToAdmin2"]);
                    Mail.Subject = Subject;
                    Mail.HtmlBody = Body;
                    _client.DefaultRequestHeaders.Add("X-Postmark-Server-Token", Convert.ToString(ConfigurationManager.AppSettings["X-Postmark-Server-Token"]));
                    _client.DefaultRequestHeaders.Add("Accept", "application/json");
                    string data = JsonConvert.SerializeObject(Mail);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _client.PostAsync(Convert.ToString(ConfigurationManager.AppSettings["EmailUrl"]), content);
                    return Convert.ToInt32(response.StatusCode);
                }
            }
            catch
            {
                return 0;
                //throw ex;
            }
        }

        public async Task<int> SendEmailToCustomerviaPostmarkapp(string Email, string Subject, string Body)
        {
            try
            {
                using (_client = new HttpClient())
                {
                    EmailviaPostmarkapp Mail = new EmailviaPostmarkapp();
                    Mail.From = Convert.ToString(ConfigurationManager.AppSettings["EmailSenderMailId"]);
                    Mail.To = Email;
                    Mail.Subject = Subject;
                    Mail.HtmlBody = Body;
                    _client.DefaultRequestHeaders.Add("X-Postmark-Server-Token", Convert.ToString(ConfigurationManager.AppSettings["X-Postmark-Server-Token"]));
                    _client.DefaultRequestHeaders.Add("Accept", "application/json");
                    string data = JsonConvert.SerializeObject(Mail);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _client.PostAsync(Convert.ToString(ConfigurationManager.AppSettings["EmailUrl"]), content);
                    return Convert.ToInt32(response.StatusCode);
                }
            }
            catch
            {
                return 0;
                //throw ex;
            }
        }

       
    }
}