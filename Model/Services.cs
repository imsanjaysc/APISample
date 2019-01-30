using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ServiceList
    {
        public List<ServiceCategorylist> Categorylists { get; set; }
        public List<ProductServiceslist> productServiceslists { get; set; }
        public string Heading { get; set; }
        public string SubText { get; set; }
        public List<BannerImage> Banner { get; set; }
    }
    public class ServiceCategorylist
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Icons { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
       
    }

    public class BannerImage
    {
        public string ImageUrl { get; set; }
    }

    public class ProductServiceslist
    {
        public int CategoryID { get; set; }
        public int ServiceID { get; set; }        
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public string Icons { get; set; }
    }
}
