using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;

namespace TPW_GMS
{
    public partial class MemberTransfer : System.Web.UI.Page
    {
        //private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!Page.IsPostBack)
            {
                if (roleId == "1" || roleId == "2" || roleId=="4")
                {
                    loadCustomerDropDown(splitUser);
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
        protected void loadCustomerDropDown(string branch)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                if (branch.Equals("admin") || branch.Equals("superadmin"))
                {
                    var name = from p in db.MemberInformations
                               where p.memberOption != "Free User"
                               orderby p.fullname
                               select p.fullname;

                    ddlMemberName.DataSource = name;
                    ddlMemberName.DataBind();
                    ddlMemberName.Items.Insert(0, new ListItem("--Select--", "0"));
                }
                else
                {
                    var name = from p in db.MemberInformations
                               where p.memberOption != "Free User" && p.branch.Equals(branch)
                               orderby p.fullname
                               select p.fullname;
                    ddlMemberName.DataSource = name;
                    ddlMemberName.DataBind();
                    ddlMemberName.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
        }
        protected void loadBranch()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var branchName = (from p in db.Logins
                                  where !p.firstname.Contains("admin")
                                  select p.username);
                ddlDestinationBranch.DataSource = branchName;
                ddlDestinationBranch.DataBind();
                ddlDestinationBranch.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }
        protected void ddlMemberName_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                ddlMemberId.Items.Clear();
                var memberId = (from p in db.MemberInformations
                                where p.fullname == ddlMemberName.SelectedItem.ToString()
                                select p.memberId);
                var memberBranch = (from p in db.MemberInformations
                                    where p.fullname == ddlMemberName.SelectedItem.ToString()
                                    select p.branch);
                if (memberId != null)
                {
                    ddlMemberId.DataSource = memberId.ToList();
                    ddlMemberId.DataBind();
                    ddlCurrentBranch.DataSource = memberBranch.ToList();
                    ddlCurrentBranch.DataBind();
                    if (memberId.ToList().Count > 1)
                    {
                        lblMessage.Text = "Multiple Member ID and Branch, Please Choose from dropdown";
                        lblMessage.ForeColor = ColorTranslator.FromHtml("#d81910");
                    }
                    else
                    {
                        lblMessage.Text = "";
                    }
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var editItem = (from p in db.MemberInformations
                                where p.fullname == ddlMemberName.SelectedItem.Text && p.memberId == ddlMemberId.SelectedItem.Text
                                select p).SingleOrDefault();
                editItem.branch = ddlDestinationBranch.SelectedItem.Text;
                db.SubmitChanges();
                lblMessage.Text = "Member Successfully Transfered";
                lblMessage.ForeColor = ColorTranslator.FromHtml("#037203");
            }
        }
    }
}