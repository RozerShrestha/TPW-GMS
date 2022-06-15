using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Services;

namespace TPW_GMS
{
    public partial class LoginMember : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool valid = false;
            valid = txtUserName.Text != "" && txtPassword.Text != "" ? true : false;

            if(valid)
            {
                string username = txtUserName.Text.Trim().ToLower();
                string password = txtPassword.Text.Trim();
                CheckLogin(username, password);
            }
        }
        private void CheckLogin(string username, string password)
        {
            var item = (from p in db.MemberInformations
                        where p.contactNo.Equals(username) && p.password.Equals(password)
                        select p).SingleOrDefault();
            if(item!=null)
            {
                var encryptedMemberId = Service.EncryptData(item.memberId);
                HttpCookie userExpireDate = new HttpCookie("ExpireDate");
                HttpCookie loginMember = new HttpCookie("LoginMember");

                userExpireDate["MemberId"] = encryptedMemberId;
                loginMember["MemberId"] = encryptedMemberId;
                Response.Cookies["ExpireDate"].Expires = DateTime.Now.AddDays(100);
                Response.Cookies["ExpireDate"].Value = encryptedMemberId;
                //Response.Cookies["ExpireDate"]. = item.fullname;
                Response.Redirect("MemberDashboard.aspx");
            }
            else
            {
                lblInfo.Text = "Wrong Username or Password";
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            MemberLoginCredential m = new MemberLoginCredential();
            m.username = txtResetUsername.Value;
            m.password = txtResetPassword.Value;
            m.newPassword = txtResetNewPassword.Value;
           lblInfo.Text= Service.ResetPassword(m);
        }       
    }
}