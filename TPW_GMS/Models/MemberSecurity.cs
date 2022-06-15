using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TPW_GMS.Data;

namespace TPW_API.Models
{
    public class MemberSecurity
    {
        public static bool Login(string username, string password)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                //bool isvalid = db.Logins.Any(user => user.username.Equals(username) && user.password == password);
                var uname = ConfigurationManager.AppSettings["uname"];
                var pass = ConfigurationManager.AppSettings["pass"];

                if (username == uname && password == pass)
                    return true;
                else
                    return false;
            }
        }
        public static bool LoginUserAuth(string Username, string Password)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var uname = ConfigurationManager.AppSettings["uname"];
                var pass = ConfigurationManager.AppSettings["pass"];
                if (Username.Equals("admin") && Username == uname && Password == pass)
                {
                    return true;
                }
                else
                {
                    var item = db.MemberLogins.Where(a => a.username.Equals(Username) && a.password.Equals(Password)).SingleOrDefault();

                    bool isvalid = item == null ? false : true;
                    return isvalid;
                }
                
            }
        }
    }
}