using ActiveUp.Net.WhoIs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Repository;

namespace TPW_GMS
{
    public partial class ExClientCallBack : System.Web.UI.Page
    {
        string roleId, loginUser, splitUser;
        public DbConFactory dbCon = new DbConFactory();
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!IsPostBack)
            {
                loadBranch();
                LoadStartAndEndDate();
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
            using (var db = new TPWDataContext())
            {
                if (roleId == "1" || roleId == "4")
                {

                    var branchName = from p in db.Logins
                                     where !p.username.Contains("admin")
                                     select p.username;
                    branch.DataSource = branchName;
                    branch.DataBind();
                    branch.Items.Insert(0, new ListItem("--Select--", "0"));
                }
                else
                {
                    var branchName = from p in db.Logins
                                     where p.username.Contains(splitUser) && !p.username.Contains("admin")
                                     select p.username;
                    branch.DataSource = branchName;
                    branch.DataBind();
                }
            }
        }
        private void LoadStartAndEndDate()
        {
            try
            {
                using (var conn = dbCon.CreateConnection())
                {
                    var result = conn.QuerySingle("GetEngNepOneMonthBefore"
                        , commandType: System.Data.CommandType.StoredProcedure);
                    if (result != null)
                    {
                        startDate.Text = result.NepFirst;
                        endDate.Text = result.NepLast;
                    }
                }
                
            }
            catch (Exception ex)
            {

            }

        }
    }
}