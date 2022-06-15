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
    public partial class StaffAttendance : System.Web.UI.Page
    {
        //static TPWDataContext db = new TPWDataContext();
        //private TPWDataContext db = new TPWDataContext();
        public static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            txtQrCodeStaff.Enabled = loginUser.Contains("admin") ? false : true;
            hidCurrentLoginBranch.Value = splitUser;      
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
        protected void gridAttendance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
    }
}