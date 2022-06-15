using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;

namespace TPW_GMS
{
    public partial class MemberExpiredReport : System.Web.UI.Page
    {
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!IsPostBack)
            {
                if (roleId == "1" || roleId=="4")
                {
                    loadBranch();
                }
                else
                {
                    Response.Redirect("AccessDenied.aspx");
                }
            }
        }
        public void InitialCheck()
        {
            LoginUserInfo l = Services.Service.checkSession();
            if (l == null)
                Response.Redirect("SignIn.aspx");
            else
            {
                roleId = l.roleId;
                loginUser = l.loginUser;
                splitUser = l.splitUser;
            }
        }
        protected void loadBranch()
        {
            using (var db = new TPWDataContext()) {
                var branchName = from p in db.Logins
                                 where !p.username.Contains("admin")
                                 select p.username;
                branch.DataSource = branchName;
                branch.DataBind();
                branch.Items.Insert(0, new ListItem("ALL", "ALL"));
            }
        }
    }
}