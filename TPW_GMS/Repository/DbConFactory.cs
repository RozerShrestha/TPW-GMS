using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TPW_GMS.Repository
{
    public class DbConFactory : IDbConFactory
    {
        private readonly string connString;
        public DbConFactory()
        {
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["TPW_GMSConnectionString"];
            connString = mySetting.ConnectionString;
        }
        public SqlConnection CreateConnection()
        {
            SqlMapper.Settings.CommandTimeout = 300;
            SqlConnection newConn = new SqlConnection(connString);
            newConn.Open();
            return newConn;
        }
    }
}