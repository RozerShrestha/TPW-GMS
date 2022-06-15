using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Models;

namespace TPW_GMS
{
    public partial class test : System.Web.UI.Page
    {
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
             LoginUserInfo l = checkSession();
            roleId = l.roleId;
            loginUser = l.loginUser;
            splitUser = l.splitUser;
            if (!IsPostBack)
            {
                try
                {
                }
                catch (Exception)
                {

                }

            }
        }
        public LoginUserInfo checkSession()
        {
            LoginUserInformation l = new LoginUserInformation();
            LoginUserInfo uInfo = new LoginUserInfo();
            var loginUser = Session["userDb"].ToString();
            List<string> splitUser = new List<string>(loginUser.Split(new string[] { "-" }, StringSplitOptions.None));
            int roleId = l.getLoginUSerRole(loginUser);
            if (loginUser == null)
                Response.Redirect("SignIn.aspx");

            uInfo.loginUser = loginUser;
            uInfo.roleId = roleId.ToString();
            uInfo.splitUser = splitUser[0];

            return uInfo;
        }
    }
}