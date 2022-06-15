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
    public partial class Dashboard : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!Page.IsPostBack)
            {
                loadBranch();
                LoginUserInfo l = Services.Service.checkSession();
                if (l == null)
                    Response.Redirect("SignIn.aspx");
                else
                    loadDate();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Dash item = (from p in db.Dashes
                         select p).FirstOrDefault();
            item.startDate = DateTime.Parse(txtStartDate.Text);
            item.endDate = DateTime.Parse(txtEndDate.Text);
            db.SubmitChanges();
            loadDate();

        }
        public void loadDate()
        {
            var item = db.Dashes.Where(p => p.id == 1).SingleOrDefault();
            txtStartDate.Text = DateTime.Parse(item.startDate.ToString()).ToString("yyyy/MM/dd");
            txtEndDate.Text = DateTime.Parse(item.endDate.ToString()).ToString("yyyy/MM/dd");
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
            if (roleId == "4" || roleId=="1")
            {
                var branchName = from p in db.Logins
                                 where !p.username.Contains("admin")
                                 select p.username;
                ddlBranch.DataSource = branchName;
                ddlBranch.DataBind();
                ddlBranch.Items.Insert(0, new ListItem("All", "All"));
            }
            else
            {

                var branchName = from p in db.Logins
                                 where p.username.Contains(splitUser) && !p.username.Contains("admin")
                                 select p.username;
                ddlBranch.DataSource = branchName;
                ddlBranch.DataBind();
            }
        }
    }
}