using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ServiceResponse<T>
    {
        public int ResponseCode { get; set; }
        public int ResponseMessage { get; set; }
        public T ResponseObject { get; set; }
        public bool IsError { get; set; }
        public ExceptionModel ExceptionObject { get; set; }
    }

    public class ExceptionModel
    {
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public int Severity { get; set; }
        public string[] KeyParameter { get; set; }
        public string Source { get; set; }
    }

    public class Exceptionlog
    {
        public Exceptionlog()
        {
            ErrorDate = DateTime.Now;
        }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        //public int Severity { get; set; }
        public string KeyParameter { get; set; }
        public string Source { get; set; }
        public DateTime ErrorDate { get; set; }
    }
}
