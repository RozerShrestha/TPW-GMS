using Microsoft.Ajax.Utilities;
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
            if (item != null)
            {
                //for staff
                if (item.memberOption =="Trainer" || item.memberOption == "Operation Manager" || item.memberOption == "Gym Admin")
                {
                    var encryptedMemberId = item.memberId;
                    HttpCookie httpCookie = new HttpCookie("LoginInfo");
                    httpCookie.Expires = DateTime.Now.AddDays(365);
                    httpCookie.Value = $"Staff#{encryptedMemberId}";
                    HttpContext.Current.Response.SetCookie(httpCookie);
                    Response.Redirect("MemberDashboard.aspx");

                    //Response.Cookies["ExpireDate"]. = item.fullname;
                    Response.Redirect("MemberDashboard.aspx");
                }
                //for Clients
                else
                {
                    var memberId = item.memberId;
                    HttpCookie httpCookie = new HttpCookie("LoginInfo");

                    httpCookie.Expires = DateTime.Now.AddDays(365);
                    httpCookie.Value = $"Client#{memberId}";
                    HttpContext.Current.Response.SetCookie(httpCookie);
                    Response.Redirect("MemberDashboard.aspx");
                }
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