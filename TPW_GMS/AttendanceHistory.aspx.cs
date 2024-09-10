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
    public partial class AttendanceHistory : System.Web.UI.Page
    {
        //static TPWDataContext db = new TPWDataContext();
        //private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "DropdownColor", "activeInactiveBGChange()", true);
            InitialCheck();
            if (!IsPostBack)
            {
                branch.Enabled = roleId == "1" || roleId=="4" ? true : false;
                loadBranch();
                loadFromAndToDate();
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
        protected void loadAttendanceCountPerstaff()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var items = (from sa in db.StaffAttandances
                             join s in db.Staffs on sa.memberId equals s.memberId
                             join ss in db.StaffSalaryDeductions on sa.memberId equals ss.memberId
                             where
                               sa.lateFlag == true &&
                               sa.checkin >= Convert.ToDateTime(startDate.Text) &&
                               sa.checkin <= Convert.ToDateTime(endDate.Text).AddDays(1)
                             group new { sa, s, ss } by new
                             {
                                 sa.memberId,
                                 s.staffName,
                                 ss.count
                             } into g
                             orderby
                               g.Count(p => p.sa.memberId != null),
                               g.Key.count
                             select new
                             {
                                 g.Key.memberId,
                                 g.Key.staffName,
                                 LateCount = g.Count(p => p.sa.memberId != null),
                                 SalDeduction = g.Key.count
                             });
                //gridPerStaff.DataSource = items;
                //gridPerStaff.DataBind();
            }

        }
        protected void loadBranch()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var branchName = (from p in db.Logins
                                  where !p.firstname.Contains("admin")
                                  select p.username);
                branch.DataSource = branchName;
                branch.DataBind();
                branch.Items.Insert(0, new ListItem("ALL", "0"));
            }
        }
        protected void loadFromAndToDate()
        {
            startDate.Text=NepaliDateService.EngToNep(DateTime.Now.AddDays(-1)).ToString();
            endDate.Text = NepaliDateService.EngToNep(DateTime.Now.AddDays(-1)).ToString();

        }
        protected void membershipOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                if (roleId == "1" || roleId=="4")
                {
                    btnSubmitTest.Enabled = true;
                    if (membershipOption.SelectedItem.Text == "Trainer" || membershipOption.SelectedItem.Text == "Gym Admin" || membershipOption.SelectedItem.Text == "Operation Manager" || membershipOption.SelectedItem.Text=="Intern")
                    {
                        var items = (from p in db.Staffs
                                     where p.staffCatagory == membershipOption.SelectedItem.Text && p.status==true
                                     select p).ToList();
                        memberId.DataSource = items;
                        memberId.DataTextField = "staffName";
                        memberId.DataValueField = "memberId";
                        memberId.DataBind();
                    }
                    else if(membershipOption.SelectedItem.Text == "Guest")
                    {
                        var items = (from p in db.Guests
                                     select p).ToList();
                        memberId.DataSource = items;
                        memberId.DataTextField = "name";
                        memberId.DataValueField = "id";
                        memberId.DataBind();
                    }
                    else
                    {
                        var items = (from p in db.MemberInformations
                                     where p.memberOption.Equals(membershipOption.SelectedItem.Text)
                                     select p).ToList();
                        memberId.DataSource = items;
                        memberId.DataTextField = "fullname";
                        memberId.DataValueField = "memberId";
                        memberId.DataBind();
                    }
                }
                else if (roleId == "2" || roleId == "3")
                {
                    if (membershipOption.SelectedItem.Text == "Trainer" || membershipOption.SelectedItem.Text == "Gym Admin" || membershipOption.SelectedItem.Text == "Operation Manager" || membershipOption.SelectedItem.Text == "Intern")
                    {
                        var items = (from p in db.Staffs
                                     where p.associateBranch == splitUser && p.staffCatagory == membershipOption.SelectedItem.Text && p.status == true
                                     select p).ToList();
                        memberId.DataSource = items;
                        memberId.DataTextField = "staffName";
                        memberId.DataValueField = "memberId";
                        memberId.DataBind();
                    }
                    else if (membershipOption.SelectedItem.Text == "Guest")
                    {
                        var items = (from p in db.Guests
                                     select p).ToList();
                        memberId.DataSource = items;
                        memberId.DataTextField = "name";
                        memberId.DataValueField = "name";
                        memberId.DataBind();
                    }
                    else
                    {
                        var items = (from p in db.MemberInformations
                                     where p.branch == splitUser && p.memberOption.Equals(membershipOption.SelectedItem.Text)
                                     select p).ToList();
                        memberId.DataSource = items;
                        memberId.DataTextField = "fullname";
                        memberId.DataValueField = "memberId";
                        memberId.DataBind();

                        branch.SelectedIndex = branch.Items.IndexOf(branch.Items.FindByText(splitUser));
                    }
                }
            }
        }
    }
}