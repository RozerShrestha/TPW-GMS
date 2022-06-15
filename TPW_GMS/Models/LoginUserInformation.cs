using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPW_GMS.Data;

namespace TPW_GMS
{
    public class LoginUserInformation
    {
        private TPWDataContext db = new TPWDataContext();
        public int getLoginUSerRole(string username)
        {
            var userRole = (from p in db.Logins
                            where p.username.Equals(username)
                            select p.roleId).SingleOrDefault();
            if (userRole != null)
                return Convert.ToInt16(userRole);
            else
                return 0;
        }
    }
}