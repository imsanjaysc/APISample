using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using DAL;

namespace DataLayer
{
    public class ProductServices : BaseClass
    {
        public ServiceList GetCategoriesList()
        {
            var res = new ServiceList();
            var strName = ConfigurationManager.AppSettings["ImageUrl"];
            using (var reader = _dbReadOnly.ExecuteReader(CommonData.GetCategoriesList))
            {

                res.Categorylists = new List<ServiceCategorylist>();
                int i = 0;
                while (reader.Read())
                {
                    res.Categorylists.Add(new ServiceCategorylist
                    {
                        CategoryID = reader["CategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["CategoryName"]),
                        Description = reader["Description"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Description"]),
                        Icons = reader["Icons"] == DBNull.Value ? string.Empty : (strName + "icons/" + Convert.ToString(reader["Icons"])),
                        TotalCount = reader["TotalCounts"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TotalCounts"]),
                        Index = i,
                    });
                    i++;
                }

                reader.NextResult();
                res.productServiceslists = new List<ProductServiceslist>();
                while (reader.Read())
                {
                    res.productServiceslists.Add(new ProductServiceslist
                    {
                        CategoryID = reader["CategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CategoryID"]),
                        ServiceID = reader["ServiceID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ServiceID"]),
                        ServiceName = reader["ServiceName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ServiceName"]),
                        ServiceDescription = reader["ServiceDescription"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ServiceDescription"]),
                        Icons = reader["Icon"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Icon"])
                    });
                }

                List<BannerImage> bnimg = new List<BannerImage>();
                bnimg.Add(new BannerImage { ImageUrl = "http://fixybee.aphroecs.com/banner/banner1.png" });
                bnimg.Add(new BannerImage { ImageUrl = "http://fixybee.aphroecs.com/banner/banner2.png" });
                bnimg.Add(new BannerImage { ImageUrl = "http://fixybee.aphroecs.com/banner/banner3.png" });
                res.Heading = "Welcome to Fixybee";
                res.SubText = "Fixybee is your go-to app where you can connect with one professional for all your needs. Your premises will be inspected by a professional, who identifies the job that needs to be done. To begin, please select the suitable option from down below.";
                res.Banner = bnimg;
            }
            return res;
        }

        public List<ProductServiceslist> GetProductServiceslist(long CustomerID, int CategoryID)
        {
            var listofservices = new List<ProductServiceslist>();
            using (var reader = _dbReadOnly.ExecuteReader(CommonData.GetServicesListByCategoryID, CustomerID, CategoryID))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        listofservices.Add(new ProductServiceslist
                        {
                            ServiceID = reader["ServiceID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ServiceID"]),
                            ServiceName = reader["ServiceName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ServiceName"]),
                            ServiceDescription = reader["ServiceDescription"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ServiceDescription"]),
                            Icons = reader["Icon"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Icon"])
                        });
                    }
                }
            }
            return listofservices;
        }
    }
}
