using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPW_GMS.Data;

namespace TPW_GMS.Repository
{
    public class UserMasterRepository : IDisposable
    {
        private TPWDataContext db = new TPWDataContext();
        public Login ValidateUser(string username, string password)
        {
            return db.Logins.FirstOrDefault(user => user.username.Equals(username) && user.password.Equals(password));
        }
        public MemberInformation ValidateStaff(string username, string password)
        {
            return db.MemberInformations.FirstOrDefault(user => user.contactNo.Equals(username) && user.password.Equals(password));

        }
        public void Dispose()
        {
            db.Dispose();
            //throw new NotImplementedException();
        }
    }
}