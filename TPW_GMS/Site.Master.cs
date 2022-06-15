using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Services;

namespace TPW_GMS
{
    public partial class SiteMaster : MasterPage
    {
        public string headerText;
        public int paymentPendingCount, lockerCount;
        public string userType;
        private readonly TPWDataContext db = new TPWDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblUserLogin.Text = Session["userDb"].ToString();
                hidUserType.Value = Session["userType"].ToString();
                userType = "Employee";
            }
            catch (Exception)
            {
                HttpCookie loginCookie = Request.Cookies["LoginMember"];
                if(loginCookie!=null)
                {
                    lblUserLogin.Text = loginCookie.Values["MemberName"];
                    hidUserType.Value= loginCookie.Values["UserType"];
                    userType = "Member";
                }  
            }
                      
            try
            {
                var a = (HiddenField)ContentPlaceHolder1.FindControl("hidHeader");
                headerText = a.Value.ToString();
                getTodayDate();
                //getListOfMembers();
            }
            catch (Exception)
            {

            }
            NotificationData();
        }
        public void getTodayDate()
        {
            var today = DateTime.Now;
            var nepToday = NepaliDateService.EngToNep(today);
            lblCurrentDateNew.Text ="Today's Date: "+ nepToday.ToString();
        }
        public void getListOfMembers()
        {
            //using (TPWDataContext db = new TPWDataContext())
            //{
            var d1 = DateTime.Now.AddMonths(-1);

                var items = db.MemberInformations.Where(k => k.ActiveInactive == "Active" && k.memberExpireDate < d1 &&( k.memberOption !="Trainer" && k.memberOption!="Super Admin" && k.memberOption!="Gym Admin")).ToList();
                foreach (var item in items)
                {
                    item.ActiveInactive = "InActive";
                }
                new Task(() =>
                    {
                        db.SubmitChanges();
                    }).Start();
                //}
        }
        protected void timer1_Tick(object sender, EventArgs e)
        {
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            if (userType=="Employee")
            { 
                Response.Redirect("SignIn.aspx");
            }
            else
            {
                Response.Redirect("LoginMember.aspx");
            }
            
        }
        protected void NotificationData()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                List<string> splitUser = new List<string>(lblUserLogin.Text.Split(new string[] { "-" }, StringSplitOptions.None));
                int value = 0;
                //try
                //{
                //    var a = (DropDownList)ContentPlaceHolder1.FindControl("ddlPrePendingCheck");
                //    value = Convert.ToInt16(a.SelectedValue.ToString());
                //}
                //catch (Exception)
                //{
                //    value = 0;
                //}

                if (lblUserLogin.Text == "admin" || lblUserLogin.Text=="superadmin")
                {
                    var paymentPending = (from p in db.MemberInformations
                                          where p.memberExpireDate < (DateTime.Now).AddDays(value) && p.ActiveInactive == "Active"
                                          select p).ToList();
                    paymentPendingCount = paymentPending.Count();

                    var lockerExpired = (from k in db.LockerMgs
                                         where k.expireDate < DateTime.Now
                                         select k);
                    lockerCount = lockerExpired.Count();
                }
                else
                {
                    var paymentPending = (from p in db.MemberInformations
                                          where p.memberExpireDate < (DateTime.Now).AddDays(value) && p.ActiveInactive == "Active" && p.branch == splitUser[0]
                                          select p).ToList();
                    paymentPendingCount = paymentPending.Count();

                    var lockerExpired = (from k in db.LockerMgs
                                         where k.expireDate < DateTime.Now && k.branch==splitUser[0]
                                         select k);
                    lockerCount = lockerExpired.Count();
                }
            }
        }
    }
}