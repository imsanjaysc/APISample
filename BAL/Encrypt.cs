using CCA.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class Encrypt
    {
        CCACrypto ccaCrypto = new CCACrypto();
        public string EncryptStr(string data)
        {
            string key = ConfigurationManager.AppSettings["EncryKey"];
            return ccaCrypto.Encrypt(data, key);

        }
        public string DecryptStr(string data)
        {
            string key = ConfigurationManager.AppSettings["EncryKey"];
            return ccaCrypto.Decrypt(data, key);

        }
    }
}
