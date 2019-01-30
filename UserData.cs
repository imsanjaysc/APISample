using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Model;

namespace DAL
{
    public class UserData : BaseClass
    {
        readonly string constring = System.Configuration.ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
        public UserInfo GetProfileDetails(string MobileNO, string Password)
        {
            try
            {
                var res = new UserInfo();
                using (var reader = _dbReadOnly.ExecuteReader(CommonData.GetProfileDetailsByMobileNo, MobileNO, Password))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            res.CustomerID = reader["CustomerID"] == DBNull.Value ? 0 : Convert.ToInt64(reader["CustomerID"]);
                            res.FirstName = reader["FirstName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["FirstName"]);
                            res.LastName = reader["LastName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["LastName"]);
                            res.MobileNo = reader["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(reader["MobileNo"]);
                            res.AltMobileNo = reader["AlternateNumber"] == DBNull.Value ? string.Empty : Convert.ToString(reader["AlternateNumber"]);
                            res.Email = reader["EmailID"] == DBNull.Value ? string.Empty : Convert.ToString(reader["EmailID"]);
                            res.ProfileImage = reader["ProfilePicSmall"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ProfilePicSmall"]);
                            res.ProfileImage = reader["ProfilePic"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ProfilePic"]);
                            res.LastLogin = reader["LastLogin"] == DBNull.Value ? string.Empty : Convert.ToString(reader["LastLogin"]);
                        }
                    }

                }
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int CheckExistMobileNumber(string Mobileno)
        {
            try
            {
                int val = 0;
                using (var reader = _dbReadOnly.ExecuteReader(CommonData.CheckExistMobileNo, Mobileno))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            val = reader["MobileNo"] == DBNull.Value ? 0 : 1;
                        }
                    }

                }
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public string ForgotPassword(string Mobileno)
        {
            try
            {
                int val = 0;
                string pass = "";
                using (var reader = _dbReadOnly.ExecuteReader(CommonData.Forgotpassword, Mobileno))
                {
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            val = reader["IsExists"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IsExists"]);
                        }

                        if (val == 1)
                        {
                            pass = reader["Password"] == DBNull.Value ? null : Convert.ToString(reader["Password"]);
                        }
                    }

                }
                return pass;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserInfo GetProfileDetails(long CustomerID, string MobileNo, string Type)
        {
            var res = new UserInfo();
               
            var strName = ConfigurationManager.AppSettings["ImageUrl"];
            using (var reader = _dbReadOnly.ExecuteReader(CommonData.GetProfileDetails, CustomerID, MobileNo, Type))
            {
                if (reader.Read())
                {
                    res.CustomerID = Convert.ToInt32(CustomerID);
                    res.FirstName = reader["FirstName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["FirstName"]);
                    res.LastName = reader["LastName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["LastName"]);
                    res.MobileNo = reader["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(reader["MobileNo"]);
                    res.AltMobileNo = reader["AlternateNumber"] == DBNull.Value ? string.Empty : Convert.ToString(reader["AlternateNumber"]);
                    res.Email = reader["EmailID"] == DBNull.Value ? string.Empty : Convert.ToString(reader["EmailID"]);
                    res.ProfilePicSmall = reader["ProfilePicSmall"] == DBNull.Value ? string.Empty : (strName + "Profilepic/" + Convert.ToString(reader["ProfilePicSmall"]));
                    res.ProfileImage = reader["ProfilePic"] == DBNull.Value ? string.Empty : (strName + "Profilepic/" + Convert.ToString(reader["ProfilePic"]));
                    res.LastLogin = reader["LastLogin"] == DBNull.Value ? string.Empty : Convert.ToString(reader["LastLogin"]);
                    res.IsEmailVerified = Convert.ToBoolean(reader["IsEmailVerified"]);
                }
            }
            return res;
        }
        public int SubmitUserInfo(UserInfo user)
        {
            try
            {
                int val = 0;
                using (var reader = _dbReadOnly.ExecuteReader(CommonData.InsertUserDetails, user.FirstName, user.LastName, user.MobileNo, user.Email, user.Password, user.ActiveStatus, user.Source))
                    if(reader.Read())
                    {
                        val = reader["CustomerID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CustomerID"]);
                    }
                //var reader = _db.ExecuteNonQuery(CommonData.InsertUserDetails, user.FirstName, user.LastName, user.MobileNo, user.Email, user.Password, user.ActiveStatus, user.Source);
               // val = reader > 0 ? 1 : 0;
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int UpdateUserProfile(UserInfo profile)
        {
            try
            {
                int val = 0;
                var reader = _db.ExecuteNonQuery(CommonData.UpdateUserDetails, profile.CustomerID, profile.FirstName, profile.LastName, profile.Email, profile.MobileNo, profile.AltMobileNo, profile.ActiveStatus);
                val = reader > 0 ? 1 : 0;
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public UserInfo SearchByUserID(long UserId)
        {
            try
            {
                var res = new UserInfo();
                using (var reader = _dbReadOnly.ExecuteReader(CommonData.GetProfileDetails, UserId))
                {
                    if (reader != null)
                    {
                        var ImageUrl = ConfigurationManager.AppSettings["ImageUrl"];
                        while (reader.Read())
                        {
                            res.CustomerID = reader["UserId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["UserId"]);
                            res.FirstName = reader["FirstName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["FirstName"]);
                            res.LastName = reader["LastName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["LastName"]);
                            res.MobileNo = reader["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(reader["MobileNo"]);
                            res.Email = reader["Email"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Email"]);
                            res.Password = reader["Password"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Password"]);
                            res.ActiveStatus = reader["ActiveStatus"] == DBNull.Value ? false : Convert.ToBoolean(reader["ActiveStatus"]);
                            res.Password = reader["Password"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Password"]);
                            res.ProfileImage = reader["ProfilePic"] == DBNull.Value ? string.Empty : ImageUrl + "/ProfilePic/" + Convert.ToString(reader["ProfilePic"]);
                        }
                    }

                }
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int UpdateUserProfilePic(long UserID, string FilenName, string ResizedFile)
        {
            try
            {
                // var strName = ConfigurationManager.AppSettings["ImageUrl"];
                int val = 0;
                var reader = _db.ExecuteNonQuery(CommonData.UpdateUsersProfilepic, UserID, FilenName, ResizedFile);
                val = reader > 0 ? 1 : 0;
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int UpdatePassword(UpdatePassword pass)
        {
            try
            {
                int val = 0;
                var reader = _db.ExecuteNonQuery(CommonData.UpdatePassword, pass.CustomerID, pass.OldPassword, pass.NewPassword);
                val = reader > 0 ? 1 : 0;
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int UpdateUserLocation(UserLocations location, long CustomerID)
        {
            try
            {
                int val = 0;
                var reader = _db.ExecuteNonQuery(CommonData.UpdateUserLocations, CustomerID, location.Latitude, location.Longitude, location.CountryCode, location.CountryName,
                    location.FeatureName, location.PostalCode, location.SubLocality, location.Thoroughfare, location.SubThoroughfare, location.Locality, location.AdminArea, location.SubAdminArea);
                val = reader > 0 ? 1 : 0;
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int EmailVerification(int id)
        {
            try
            {
                int val = 0;
                var reader = _db.ExecuteNonQuery(CommonData.EmailVerification, id);
                val = reader > 0 ? 1 : 0;
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserLocations GetUserLocation(long CustomerID)
        {
            var res = new UserLocations();
            using (var reader = _dbReadOnly.ExecuteReader(CommonData.GetUserLocation, CustomerID))
            {
                if (reader.Read())
                {
                    res.CustomerID = Convert.ToInt32(CustomerID);
                    res.Latitude = reader["Latitude"] == DBNull.Value ? 0 : Convert.ToDouble(reader["Latitude"]);
                    res.Longitude = reader["Longitude"] == DBNull.Value ? 0 : Convert.ToDouble(reader["Longitude"]);
                    res.CountryCode = reader["CountryCode"] == DBNull.Value ? string.Empty : Convert.ToString(reader["CountryCode"]);
                    res.CountryName = reader["CountryName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["CountryName"]);
                    res.FeatureName = reader["FeatureName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["FeatureName"]);
                    res.PostalCode = reader["PostalCode"] == DBNull.Value ? string.Empty : Convert.ToString(reader["PostalCode"]);
                    res.SubLocality = reader["SubLocality"] == DBNull.Value ? string.Empty : Convert.ToString(reader["SubLocality"]);
                    res.Thoroughfare = reader["Thoroughfare"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Thoroughfare"]);
                    res.SubThoroughfare = reader["SubThoroughfare"] == DBNull.Value ? string.Empty : Convert.ToString(reader["SubThoroughfare"]);
                    res.Locality = reader["Locality"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Locality"]);
                    res.AdminArea = reader["AdminArea"] == DBNull.Value ? string.Empty : Convert.ToString(reader["AdminArea"]);
                    res.SubAdminArea = reader["SubAdminArea"] == DBNull.Value ? string.Empty : Convert.ToString(reader["SubAdminArea"]);

                }
            }
            return res;
        }

        public bool ValidateUser(string MobileNO, string Password)
        {
            UserInfo userInfo = new UserInfo();
            userInfo = GetProfileDetails(MobileNO, Password);
            if (userInfo != null)
            {
                bool val = userInfo.Password == Password ? true : false;
                return val;
            }
            else
            {
                return false;
            }
        }
        public int InsertCustomerAddress(CustAddress address)
        {
            try
            {
                int val = 0;
                var reader = _db.ExecuteNonQuery(CommonData.InsertCustomerAddress, address.CustomerID, address.TypeID, address.Address, address.Landmark, address.Pincode);
                val = reader > 0 ? 1 : 0;
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateCustomerAddress(CustAddress address)
        {
            try
            {
                int val = 0;
                var reader = _db.ExecuteNonQuery(CommonData.UpdateCustomeraddress, address.CustomerID, address.AddressID, address.TypeID, address.Address, address.Landmark, address.Pincode);
                val = reader > 0 ? 1 : 0;
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int DelCustomerAddress(List<DelCustAddress> add, long CustomerID)
        {

            int val = 0;
            DataTable dtValue = new DataTable();
            dtValue.Clear();
            dtValue.Columns.Add("CustomerID", typeof(long));
            dtValue.Columns.Add("AddressID", typeof(int));
            DataRow dr;
            foreach (var item in add)
            {
                dr = dtValue.NewRow();
                dr["CustomerID"] = CustomerID;
                dr["AddressID"] = item.AddressID;
                dtValue.Rows.Add(dr);
            }

            SqlConnection con = new SqlConnection(constring);
            SqlParameter[] del = new SqlParameter[1];
            del[0] = new SqlParameter("@tmpDelAddress", dtValue);
            del[0].SqlDbType = SqlDbType.Structured;
            val = SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, CommonData.DeleteCustomerAddress, del);
            val = val > 0 ? 1 : 0;
            return val;
        }

        public ServiceRequestID AddtoServiceRequests(ServiceRequests service, long CustomerID)
        {
            try
            {
                var res = new ServiceRequestID();
                DataTable dtValue = new DataTable();
                dtValue.Clear();
                dtValue.Columns.Add("ServiceID", typeof(int));
                DataRow dr;
                DataSet ds;
                foreach (var item in service.Services)
                {
                    dr = dtValue.NewRow();
                    dr["ServiceID"] = item.ServiceID;
                    dtValue.Rows.Add(dr);
                }
                SqlConnection con = new SqlConnection(constring);
                SqlParameter[] add = new SqlParameter[6];
                add[0] = new SqlParameter("@CustomerID", CustomerID);
                add[0].SqlDbType = SqlDbType.BigInt;
                add[1] = new SqlParameter("@AddressID", service.AddressID);
                add[1].SqlDbType = SqlDbType.Int;
                add[2] = new SqlParameter("@RequestTime", service.RequestTime);
                add[2].SqlDbType = SqlDbType.DateTime;
                add[3] = new SqlParameter("@Comments", service.Comments);
                add[3].SqlDbType = SqlDbType.NVarChar;
                add[4] = new SqlParameter("@RefferalCode", service.RefferalCode);
                add[4].SqlDbType = SqlDbType.NVarChar;
                add[5] = new SqlParameter("@tempServiceID", dtValue);
                add[5].SqlDbType = SqlDbType.Structured;
                ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, CommonData.AddtoServiceRequestDetails, add);
                if (ds.Tables[0].Rows.Count>0)
                {
                    res.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString() != null ? ds.Tables[0].Rows[0]["FirstName"].ToString(): null;
                    res.MobileNo = ds.Tables[0].Rows[0]["MobileNo"].ToString() != null ? ds.Tables[0].Rows[0]["MobileNo"].ToString() : null;
                    res.RequestID = ds.Tables[0].Rows[0]["ServiceReqID"].ToString() != null ? ds.Tables[0].Rows[0]["ServiceReqID"].ToString() : null;
                    res.Address = ds.Tables[0].Rows[0]["CustomerAddress"].ToString() != null ? ds.Tables[0].Rows[0]["CustomerAddress"].ToString() : null;
                    res.Email = ds.Tables[0].Rows[0]["Emailid"].ToString() != null ? ds.Tables[0].Rows[0]["Emailid"].ToString() : null;
                    res.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"]);
                    res.IsEmailVerified = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsEmailVerified"]);
                    
                }

                string textBody = " <table border=" + 1 + " cellpadding=" + 2 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#81bf38' color='white'><td><b>Sno</b></td> <td> <b> Service Name</b> </td></tr>";
                int count = 1;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int loopCount = 0; loopCount < ds.Tables[1].Rows.Count; loopCount++)
                    {
                        textBody += "<tr><td>" + count + "</td><td> " + ds.Tables[1].Rows[loopCount]["ServiceName"] + "</td> </tr>";
                        count++;
                    }

                }
                else
                {
                    textBody += "<tr><td>" + 0 + "</td><td> " + "None" + "</td> </tr>";
                }
                res.ServicesName = textBody;
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<ServiceHistory> GetServiceRequestHistory(long CustomerID)
        {
            var listofrequests = new List<ServiceHistory>();
            using (var reader = _dbReadOnly.ExecuteReader(CommonData.GetServiceRequestHistoryList, CustomerID))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        listofrequests.Add(new ServiceHistory
                        {
                            ServiceReqID = reader["ServiceReqID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ServiceReqID"]),
                            RequestDate = Convert.ToDateTime(reader["RequestTime"]),
                            Status = reader["Status"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Status"])
                        });
                    }
                }
            }
            return listofrequests;
        }

        public List<CustAddressList> GetAddresslist(long CustomerID)
        {
            var listofaddress = new List<CustAddressList>();
            using (var reader = _dbReadOnly.ExecuteReader(CommonData.GetCustomerAddresslist, CustomerID))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        listofaddress.Add(new CustAddressList
                        {
                            AddressID = reader["AddressID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["AddressID"]),
                            Address = reader["Address"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Address"])
                        });
                    }
                }
            }
            return listofaddress;
        }
    }
}
