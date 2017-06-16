using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ODPDeepUnknown.Models
{
    public class OracleDBContext : DbContext
    {
        public OracleDBContext() : base("OracleDbContext") { }

    }
}