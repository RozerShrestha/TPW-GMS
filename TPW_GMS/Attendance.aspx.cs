using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Services;

namespace TPW_GMS
{
    public partial class Attendance : System.Web.UI.Page
    {
        //static TPWDataContext db = new TPWDataContext();
        //private TPWDataContext db = new TPWDataContext();
        public static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            qrCodeScan.Enabled = loginUser.Contains("admin") ? false : true;
            hidCurrentLoginBranch.Value = splitUser;
            if (!IsPostBack)
            {
                loadInfo();
            }
        }
        protected void loadInfo()
        {
            //txtMemberId.Text = Utility.generateMemberId("TPW-"+txtBranch.Text);
            using (TPWDataContext db = new TPWDataContext())
            {
                var item = db.ExtraInformations.SingleOrDefault();
                txtStatic.Text = item.currentNepaliDate + splitUser;
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
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                MemberAttandance m = new MemberAttandance();

                //insert into Member Attendance
                m.memberId = qrCodeScan.Text.Trim();
                m.checkin = DateTime.Now;
                m.checkout = DateTime.Now.AddHours(2);
                m.branch = txtBranch.Value;
                m.checkinBranch = loginUser;
                db.MemberAttandances.InsertOnSubmit(m);

                //insert into Report

                db.SubmitChanges();
            }
        }
        protected void btnReload_Click(object sender, EventArgs e)
        {
            Response.Redirect("Attendance.aspx");
        }
        protected void btnSubmitModal_Click(object sender, EventArgs e)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                Report r = new Report();
                MemberAttandance m = new MemberAttandance();
                var trimQR = qrCodeScan.Text.Trim();
                var yesterdays = DateTime.Now.AddDays(-1).Date;
                var query = (from p in db.MemberInformations
                             where p.memberId == trimQR
                             select p).SingleOrDefault();
                var attendanceCount = (from a in db.MemberAttandances
                                       where a.memberId == trimQR && Convert.ToDateTime(a.checkin).Date > yesterdays
                                       select a);
                if (attendanceCount.Count() == 0)
                {

                    r.receiptNo = txtStatic.Text + "-" + txtReceiptNo.Text; ;
                    r.paymentMethod = "Cash";
                    r.memberId = trimQR;
                    r.memberOption = "OffHour/Universal";
                    r.finalAmount = 100;
                    r.created = DateTime.Now;
                    m.memberId = trimQR;
                    m.checkin = DateTime.Now;
                    m.checkout = DateTime.Now.AddHours(2);
                    m.branch = query.branch;
                    m.checkinBranch = splitUser;
                    query.universalMembershipLimit -= query.branch.Equals(splitUser) ? 0 : 1;
                    db.MemberAttandances.InsertOnSubmit(m);
                    db.Reports.InsertOnSubmit(r);
                    db.SubmitChanges();
                    Response.Redirect("Attendance.aspx");
                }
                else
                {
                    Response.Write("<script>alert('Already Checked In');</script>");
                }

            }
        }
    }
}