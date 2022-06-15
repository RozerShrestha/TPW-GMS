using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Services;

namespace TPW_GMS
{
    public partial class Expenditures : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!Page.IsPostBack)
            {
                string login = splitUser;
                hidUserLogin.Value = splitUser;
                txtBranch.Text = login;                
                    try
                    {
                        string key = Request.QueryString["key"].ToString();
                        if (key == "edit")
                        {
                            loadData();
                        }
                        else if (key == "delete")
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "deleteConfirmModal", "$('#deleteConfirmModal').modal();", true);
                        }
                    }
                    catch (Exception)
                    {

                    }
                db.Dispose();
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
        protected void loadData()
        {
            try
            {
                string tempId = Request.QueryString["ID"].ToString();

                var editQuery = (from c in db.Expenditures
                                 where c.expenditureId == Convert.ToInt32(tempId)
                                 select new
                                 {
                                     c.expenditureId,
                                     c.expenditureDate,
                                     c.expenditureType,
                                     c.expenditureRate,
                                     c.branch
                                 }).SingleOrDefault();
                hid.Value = editQuery.expenditureId.ToString();
                txtExpenditureDate.Text = NepaliDateService.EngToNep(Convert.ToDateTime(editQuery.expenditureDate)).ToString();
                txtTypeOfExpenditure.Text = editQuery.expenditureType;
                txtExpenditureRate.Text = editQuery.expenditureRate;
                txtBranch.Text = editQuery.branch;
                btnEdit.Enabled = true;
                btnSubmit.Enabled = false;
            }
            catch
            {

            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var error = validateField();
            if (error == "")
            {
                Expenditure expenditureItem = new Expenditure();
                expenditureItem.expenditureDate = NepaliDateService.NepToEng(txtExpenditureDate.Text);
                expenditureItem.expenditureType = txtTypeOfExpenditure.Text;
                expenditureItem.expenditureRate = txtExpenditureRate.Text;
                expenditureItem.branch = txtBranch.Text;

                db.Expenditures.InsertOnSubmit(expenditureItem);
                db.SubmitChanges();
                lblInfo.Text = "Successfully Inserted Data, Now Redirecting...";
                lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('Expenditures.aspx') }, 1000);", true);
            }
            else
            {
                lblInfo.Text = error;
                lblInfo.ForeColor = ColorTranslator.FromHtml("#E60D25");
                return;
            }
            db.Dispose();
        }
        public string validateField()
        {
            txtExpenditureDate.Style.Remove("border-color");
            txtTypeOfExpenditure.Style.Remove("border-color");
            txtExpenditureRate.Style.Remove("border-color");
            if (string.IsNullOrWhiteSpace(txtExpenditureDate.Text))
            {
                txtExpenditureDate.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Date should not be Empty";
            }
            else if (string.IsNullOrWhiteSpace(txtTypeOfExpenditure.Text))
            {
                txtTypeOfExpenditure.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Type of Expenditure should not be Empty";
            }
            else if (string.IsNullOrWhiteSpace(txtExpenditureRate.Text))
            {
                txtExpenditureRate.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Rate should not be Empty";
            }
            else
                return "";
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Expenditure ex = (from c in db.Expenditures
                              where c.expenditureId.Equals(Convert.ToInt32(hid.Value))
                              select c).SingleOrDefault();
            ex.expenditureDate = NepaliDateService.NepToEng(txtExpenditureDate.Text);
            ex.expenditureType = txtTypeOfExpenditure.Text;
            ex.expenditureRate = txtExpenditureRate.Text;
            ex.branch = txtBranch.Text;
            db.SubmitChanges();
            lblInfo.Text = "Successfully Inserted Data, Now Redirecting...";
            lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('Expenditures.aspx') }, 1000);", true);
            db.Dispose();
        }
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["ID"].ToString();
            deleteBuySuplement(id);
        }
        protected void deleteBuySuplement(string id)
        {
            var expenditure = (from s in db.Expenditures
                               where s.expenditureId.Equals(id)
                               select s).SingleOrDefault();
            db.Expenditures.DeleteOnSubmit(expenditure);
            db.SubmitChanges();
            db.Dispose();
        }
    }
}