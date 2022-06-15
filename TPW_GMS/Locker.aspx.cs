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
    public partial class Locker : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!Page.IsPostBack)
            {
                LoginUserInfo l = Services.Service.checkSession();
                if (l == null)
                    Response.Redirect("SignIn.aspx");
                LoadLockerNumber();
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
        public void LoadLockerNumber()
        {
            var lockerNum = (from p in db.LockerMgs
                             where p.branch == splitUser
                             select p.lockerNumber).ToList();
            for (int i = 1; i <= 100; i++)
            {
                if(!lockerNum.Contains(i))
                    ddlLockerNumber.Items.Add(i.ToString());
            }
        }
    }
}