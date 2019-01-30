using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SMS
    {
        public string sender { get; set; }
        public string recipient { get; set; }
        public string content { get; set; }
        public string type { get; set; }
        public string tag { get; set; }
    }

    public class EmailviaPostmarkapp
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
    }
}
