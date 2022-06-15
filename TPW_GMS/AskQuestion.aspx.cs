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
    public partial class AskQuestion : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hidUserLogin.Value = splitUser;
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
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(hidSnNo.Value);
            try
            {
                var editQuery = (from p in db.AskQns
                                 where p.askId == id
                                 select p).SingleOrDefault();
                editQuery.date = txtDate.Text;
                editQuery.memberName = txtMemberName.Text;
                editQuery.memberId = txtMemberId.Text;
                editQuery.question = txtQuestion.Text;
                editQuery.status = chkStatus.Checked;
                db.SubmitChanges();
                lblInformation.Text = "Successfully Updated Data, Now Redirecting..";
                lblInformation.ForeColor = ColorTranslator.FromHtml("#037203");
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('AskQuestion.aspx') }, 1000);", true);
            }
            catch (Exception ex)
            {
                lblInformation.Text = ex.Message;
            }
            db.Dispose();
        }
        protected void DeleteAskQuestion(string id)
        {
            var item = (from s in db.AskQns
                        where s.askId.Equals(id)
                        select s).SingleOrDefault();
            db.AskQns.DeleteOnSubmit(item);
            db.SubmitChanges();
            Response.Redirect("AskQuestion.aspx");
            db.Dispose();
        }
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["ID"].ToString();
            DeleteAskQuestion(id);
        }
        public void LoadData()
        {
            try
            {
                string tempId = Request.QueryString["ID"].ToString();
                var editQuery = (from p in db.AskQns
                                 where p.askId == Convert.ToInt32(tempId)
                                 select p).SingleOrDefault();
                txtDate.Text = Convert.ToDateTime(editQuery.date).ToShortDateString();
                txtMemberId.Text = editQuery.memberId;
                txtMemberName.Text = editQuery.memberName;
                chkStatus.Checked = editQuery.status.Value;
                txtQuestion.Text = editQuery.question;
                hidSnNo.Value = Convert.ToString(editQuery.askId);
            }
            catch (Exception ex)
            {
                lblInformation.Text = ex.Message;
            }
            db.Dispose();
        }
    }
}