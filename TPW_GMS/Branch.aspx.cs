using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Services;
using System.Threading.Tasks;

namespace TPW_GMS
{
    public partial class Branch : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!IsPostBack)
            {
                if (roleId == "1")
                {
                    bindUserGrid();
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
        protected void bindUserGrid()
        {
            var selectQuery = from l in db.Logins
                              join r in db.Roles on l.roleId equals r.roleId
                              orderby l.roleId
                              select new
                              {
                                  l.loginId,
                                  l.firstname,
                                  l.username,
                                  l.password,
                                  l.latitude,
                                  l.longitude,
                                  l.startBillNumber,
                                  l.currentBillNumber,
                                  r.role1
                              };
            GridUserMgmt.DataSource = selectQuery;
            GridUserMgmt.DataBind();
            db.Dispose();
        }
        protected void GridUserMgmt_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int tempId = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int index = row.RowIndex;
            if (e.CommandName == "editroww")
            {

                var selectQuery = (from p in db.Logins
                                   where p.loginId == tempId
                                   select p).SingleOrDefault();

                foreach (GridViewRow gr in GridUserMgmt.Rows)
                {
                    if (gr.RowIndex == index)
                    {
                        selectQuery.firstname = ((TextBox)gr.FindControl("txtFullName")).Text;                        
                        selectQuery.password = ((TextBox)gr.FindControl("txtPassword")).Text;
                        selectQuery.latitude = ((TextBox)gr.FindControl("txtLatitude")).Text;
                        selectQuery.longitude = ((TextBox)gr.FindControl("txtLongitude")).Text;
                        selectQuery.startBillNumber = ((TextBox)gr.FindControl("txtStartBillNumber")).Text;
                        selectQuery.currentBillNumber = ((TextBox)gr.FindControl("txtCurrentBillNumber")).Text;
                        db.SubmitChanges();
                        lblMessage.Text = "Information has been successfully Updated.";
                    }

                }

            }
            else if (e.CommandName == "deleteRow")
            {
                var selectQuery = (from p in db.Logins
                                   where p.loginId == tempId
                                   select p).SingleOrDefault();

                if (selectQuery.username != "admin" && selectQuery.username!="superadmin")
                {
                    db.Logins.DeleteOnSubmit(selectQuery);
                    db.SubmitChanges();
                }
                else
                    ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Admin cannot be deleted');", true);
            }
            bindUserGrid();
            db.Dispose();
        }
        protected void btnNewBranch_Click(object sender, EventArgs e)
        {
            try
            {
                Data.Login l = new Data.Login();
                l.firstname = txtSignInFullName.Text;
                l.username = txtSignInUserName.Text;
                l.password = txtSignInPassword.Text;
                l.longitude = txtLongitude.Text;
                l.latitude = txtLatitude.Text;
                l.startBillNumber =txtStartBillNumber.Text;
                l.currentBillNumber = txtCurrentBillNumber.Text;
                l.roleId = int.Parse(ddlUserRole.SelectedValue);

                db.Logins.InsertOnSubmit(l);
                db.SubmitChanges();
                new Task(() => { MailService.SendBroadCastEmail(txtSignInUserName.Text, "rozer.shrestha611@gmail.com", "New Branch Created"); }).Start();
                
                clearAll();
                bindUserGrid();
            }
            catch (Exception ex)
            {

                lblInfo.Text = ex.Message;
            }
            db.Dispose();
        }
        protected void clearAll()
        {
            txtSignInFullName.Text = "";
            txtSignInUserName.Text = "";
            txtSignInPassword.Text = "";
            txtLongitude.Text = "";
            txtLatitude.Text = "";
            txtStartBillNumber.Text = "";
            txtCurrentBillNumber.Text = "";
        }
    }
}