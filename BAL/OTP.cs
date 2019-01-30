using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class OTP
    {
        private Random _random = new Random();
        SendSMSEmail sms = new SendSMSEmail();
        private string GenerateRandomNo()
        {
            return _random.Next(0, 9999).ToString("D4");
        }

        public string SendOTP(string Mobileno)
        {
            string OTP = GenerateRandomNo();
            sms.SendSMS(Mobileno, "Your OTP for FIXYBEE is " + OTP + "\nOTP valid for 15 minutes.");
            return OTP;
        }

        public string SendRequestDetailsToCustomer(string Mobileno, string servicereqid)
        {
            sms.SendSMS(Mobileno, "Your request has been submitted, we will get back to you shortly. your Booking ID is " +servicereqid+ ". \nThanks for choosing FIXYBEE.");
            return "Success";
        }

        public string SendRequestDetailsToAdmin(string Mobileno, string Name, string CustMobileNo, string Email, string Date, string Address, string servicereqid)
        {
            string CustEmail = Email != "" ? " Email- " +Email + "," : "";
            sms.SendSMS(Mobileno, "Hi Fixybee, New Enquiry:\nName: " +Name+ "," +CustEmail+ " Mobile: "+ CustMobileNo + " for Date: "+Date+ ", Address: "+Address);
            return "Success";
        }
        public string SendPassword(string Mobileno, string Password)
        {
            //string OTP = GenerateRandomNo();
            sms.SendSMS(Mobileno, "Your Password for FIXYBEE is " + Password);
            return "Success";
        }

    }
}
