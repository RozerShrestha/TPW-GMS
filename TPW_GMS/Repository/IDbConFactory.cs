using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPW_GMS.Repository
{
   public interface IDbConFactory
    {
        SqlConnection CreateConnection();
    }
}
