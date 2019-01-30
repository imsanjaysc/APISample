using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Customer
    {
        public long CustomerID { get; set; }
    }
    public class UserInfo : Customer
    {
        public UserInfo()
        {
            ActiveStatus = true;
        }
        //public long CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string AltMobileNo { get; set; }
        public string Password { get; set; }
        public bool ActiveStatus { get; set; }
        public string OTP { get; set; }
        public string ProfilePicSmall { get; set; }
        public string ProfileImage { get; set; }
        public string Source { get; set; }
        public string LastLogin { get; set; }
        public bool IsEmailVerified { get; set; }
    }

    public class ServiceRequestID : UserInfo
    {
        public string Address { get; set; }
        public string RequestID { get; set; }
        public string ServicesName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public class UpdatePassword : Customer
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class UserProfilePic : Customer
    {
        public string FileName { get; set; }
        public FileStream ProfilePic { get; set; }
    }

    public class ImageModel
    {
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
    }

    public class Feedback
    {
        public string ToEmail { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string Message { get; set; }
    }

    public class CustAddress : Customer
    {
        public int AddressID { get; set; }
        public int TypeID { get; set; }
        public string Address { get; set; }
        public string Landmark { get; set; }
        public int? Pincode { get; set; }
    }

    public class CustAddressList : Customer
    {
        public int AddressID { get; set; }
        //public int TypeID { get; set; }
        public string Address { get; set; }
    }
    public class DelCustAddress
    {
        public int AddressID { get; set; }
    }
}
