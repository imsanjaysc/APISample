using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;

namespace DataLayer
{
   public class UserSettingData : BaseClass
    {
        readonly string constring = System.Configuration.ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;

        public int UpdateUserSetting(List<UserSettings> settings, long UserID)
        {

            int val = 0;
            DataTable dtValue = new DataTable();
            dtValue.Clear();
            dtValue.Columns.Add("UserID", typeof(long));
            dtValue.Columns.Add("SID", typeof(int));
            dtValue.Columns.Add("IsActive", typeof(bool));
            DataRow dr;
            foreach (var item in settings)
            {
                dr = dtValue.NewRow();
                dr["UserID"] = UserID;
                dr["SID"] = item.SID;
                dr["IsActive"] = item.IsActive;
                dtValue.Rows.Add(dr);
            }

            SqlConnection con = new SqlConnection(constring);
            SqlParameter[] stng = new SqlParameter[1];
            stng[0] = new SqlParameter("@tmpSettingsMapping", dtValue);
            stng[0].SqlDbType = SqlDbType.Structured;
            val = SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, CommonData.UpdateUserDetails, stng);
            val = val > 0 ? 1 : 0;
            return val;
        }
    }
}
