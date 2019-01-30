using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace DAL
{
    public class BaseClass
    {
        public readonly Database _db;
        public readonly Database _dbReadOnly;
        public BaseClass()
        {
            var dbFactory = new DatabaseProviderFactory();
            _db = dbFactory.Create("defaultConnection");
            _dbReadOnly = dbFactory.Create("defaultConnection_read");
        }
    }
}
