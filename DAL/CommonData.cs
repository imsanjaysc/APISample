using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CommonData
    {
        //Get User Details
        public const string GetProfileDetailsByMobileNo = "GetProfileDetailsByMobileNo";
        public const string GetProfileDetails = "GetProfileDetails";
        public const string GetUserLocation = "GetUserLocation";
        public const string CheckExistMobileNo = "CheckExistMobileNo";
        public const string Forgotpassword = "Forgotpassword"; 
        public const string GetCustomerDataByMobileNo = "GetCustomerDataByMobileNo";

        //Search Users

        //Get User Details

        //Get Equipment/Brand/Model Details
        public const string GetCategoriesList = "GetServiceCategories";
        public const string GetServicesListByCategoryID = "GetServicesListByCategoryID";
        public const string GetCustomerAddresslist = "GetCustomerAddresslist";
        public const string GetServiceRequestHistoryList = "GetServiceRequestHistoryList";
        public const string EmailVerification = "EmailVerification"; 


        //Insert Procedures
        public const string InsertUserDetails = "InsertCustomerInfo";
        public const string InsertCustomerAddress = "InsertCustomerAddress";
        public const string AddtoServiceRequestDetails = "AddtoServiceRequestDetails";

        //Update User Profiles
        public const string UpdatePassword = "UpdatePassword";
        public const string UpdateUserDetails = "UpdateCustomerInfo";
        public const string UpdateUserLocations = "UpdateUserLocations";
        public const string UpdateUsersProfilepic = "UpdateUsersProfilepic";
        public const string UpdateCustomeraddress = "UpdateCustomeraddress";

        //Delete Customer Details
        public const string DeleteCustomerAddress = "DeleteCustomerAddress";
    }
}
