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
    public partial class JoinTPWNow : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!Page.IsPostBack)
            {

                hidUserLogin.Value = splitUser;
                loadBranch();
                try
                {
                    string key = Request.QueryString["key"].ToString();
                    if (key == "edit")
                    {
                        LoadData();
                    }
                    else if (key == "delete")
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
        protected void loadBranch()
        {
            var branchName = (from p in db.Logins
                              where p.firstname != "admin"
                              select p.username).ToList();
            for (int i = 0; i < branchName.Count; i++)
            {
                ddlBranch.Items.Add(branchName[i].ToString());
            }
            ddlBranch.Items.Insert(0, new ListItem("--Select--", "0"));
            db.Dispose();
        }
        protected void LoadData()
        {
            try
            {
                string tempId = Request.QueryString["ID"].ToString();
                var editQuery = (from p in db.JoinTPWs
                                 where p.jId == Convert.ToInt32(tempId)
                                 select p).SingleOrDefault();
                txtDate.Text = Convert.ToDateTime(editQuery.date).ToShortDateString();
                txtMemberId.Text = editQuery.memberId;
                txtFirstName.Text = editQuery.firstName;
                txtLastName.Text = editQuery.lastName;
                txtAddress.Text = editQuery.address;
                txtMobileNumber.Text = editQuery.mobileNumber;
                ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByText(editQuery.branch));
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(editQuery.status));
                hidSnNo.Value = Convert.ToString(editQuery.jId);
            }
            catch (Exception ex)
            {
                lblInformation.Text = ex.Message;
            }
            db.Dispose();
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(hidSnNo.Value);
            var editQuery = (from p in db.JoinTPWs
                             where p.jId == id
                             select p).SingleOrDefault();
            editQuery.memberId = txtMemberId.Text;
            editQuery.date = Convert.ToDateTime(txtDate.Text);
            editQuery.firstName = txtFirstName.Text;
            editQuery.lastName = txtLastName.Text;
            editQuery.address = txtAddress.Text;
            editQuery.mobileNumber = txtMobileNumber.Text;
            editQuery.branch = ddlBranch.SelectedItem.Text;
            editQuery.status = ddlStatus.SelectedItem.Text;

            db.SubmitChanges();
            lblInformation.Text = "Successfully Updated Data, Now Redirecting...";
            lblInformation.ForeColor = ColorTranslator.FromHtml("#037203");
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('JoinTPWNow.aspx') }, 1000);", true);
            db.Dispose();
        }
        protected void DeleteJoinTPWNow(string id)
        {
            var item = (from s in db.JoinTPWs
                        where s.jId.Equals(id)
                        select s).SingleOrDefault();
            db.JoinTPWs.DeleteOnSubmit(item);
            db.SubmitChanges();
            Response.Redirect("JoinTPWNow.aspx");
            db.Dispose();
        }
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["ID"].ToString();
            DeleteJoinTPWNow(id);
        }
    }
}