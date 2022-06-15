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
    public partial class GuestAttendance : System.Web.UI.Page
    {
        public static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        string roleId, loginUser, splitUser;

        public string name { get; internal set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            txtQrCode.Enabled = loginUser.Contains("admin") ? false : true;
            hidCurrentLoginBranch.Value = splitUser;
            if (!IsPostBack)
            {
                LoadEMAttendance();
            }
        }
        protected void LoadEMAttendance()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                try
                {
                    var today = DateTime.Today;
                    var attendanceToday = (from p in db.GuestAttandances
                                           where DateTime.Compare(Convert.ToDateTime(p.checkin).Date, DateTime.Now.Date) == 0 && p.checkinBranch == loginUser
                                           orderby p.checkin descending
                                           select p).ToList();
                    gridAttendance.DataSource = attendanceToday;
                    gridAttendance.DataBind();
                }
                catch (Exception ex)
                {
                    _logger.Error("Attendance-{message}", ex.Message);
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
        protected void gridAttendance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
    }
}