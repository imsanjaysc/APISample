using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ServiceRequests
    {
        public int AddressID { get; set; }
        public DateTime RequestTime { get; set; }
        public string Comments { get; set; }
        public string RefferalCode { get; set; }
        public List<ServicesList> Services { get; set; }
    }

    public class ServicesList
    {
        public int ServiceID { get; set; }
    }

    public class ServiceHistory
    {
        public long ServiceReqID { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
    }
}
