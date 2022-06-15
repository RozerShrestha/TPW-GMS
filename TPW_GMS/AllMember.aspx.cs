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
    public partial class AllMember : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        const string passphrase = "TPWP@ssw0rd123#";
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!IsPostBack)
            {
                try
                {
                    hidUserLogin.Value = splitUser;
                    string id = Request.QueryString["ID"];
                    if (!string.IsNullOrEmpty(id))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "deleteConfirmModal", "$('#deleteConfirmModal').modal();", true);
                    }
                }
                catch (Exception)
                {

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
        protected void Delete(string id)
        {
            try
            {
                PaymentInfo p1 = (from p in db.PaymentInfos
                                  where p.memberId.Equals(id)
                                  select p).SingleOrDefault();
                db.PaymentInfos.DeleteOnSubmit(p1);
                StartStop s1 = (from s in db.StartStops
                                where s.memberId.Equals(id)
                                select s).SingleOrDefault();
                db.StartStops.DeleteOnSubmit(s1);
                IQueryable<Report> itemsReport = from p in db.Reports.Where(c => c.memberId == id)
                                                 select p;
                foreach (Report item in itemsReport)
                {
                    db.Reports.DeleteOnSubmit(item);
                }

                IQueryable<BodyMeasurement> itemsBody = from p in db.BodyMeasurements.Where(c => c.memberId == id)
                                                        select p;
                foreach (BodyMeasurement item in itemsBody)
                {
                    db.BodyMeasurements.DeleteOnSubmit(item);
                }

                MemberLogin ml = (from p in db.MemberLogins
                                  where p.memberId.Equals(id)
                                  select p).SingleOrDefault();
                db.MemberLogins.DeleteOnSubmit(ml);

                MemberInformation m1 = (from c in db.MemberInformations
                                        where c.memberId.Equals(id)
                                        select c).SingleOrDefault();
                db.MemberInformations.DeleteOnSubmit(m1);

                db.SubmitChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                lblPopupError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorModal", "$('#errorModal').modal();", true);
                return;
            }
        }
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["ID"].ToString();
            Delete(id);
        }
    }
}