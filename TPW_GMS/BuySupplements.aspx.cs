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
    public partial class BuySupplements : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!Page.IsPostBack)
            {
                string login = Session["userDb"].ToString();
                loadBranch();
                if (roleId == "1" || roleId == "2" || roleId == "4")
                {
                    try
                    {
                        string key = Request.QueryString["key"].ToString();
                        if (key == "edit")
                        {
                            btnAddSuplements.Enabled = false;
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
                }
                else
                {
                    Response.Redirect("AccessDenied.aspx");
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
        protected void deleteBuySuplement(string id)
        {
            var suplementBuy = (from s in db.Suplements
                                where s.suplementId.Equals(id)
                                select s).SingleOrDefault();
            db.Suplements.DeleteOnSubmit(suplementBuy);
            db.SubmitChanges();
            db.Dispose();
        }
        protected void btnEditSuplements_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(hidSnNo.Value);
            if (validateBuy() == "")
            {
                Suplement s = (from c in db.Suplements
                               where c.suplementId == id
                               select c).SingleOrDefault();
                s.nameOfSuplement = txtNameOfSuplement.Text;
                s.date = NepaliDateService.NepToEng(txtSuplementBuyingDate.Text);
                s.branch = ddlBranch.SelectedItem.Text;
                s.nameOfVender = txtNameOfVender.Text;
                s.quantity = Convert.ToDouble(txtQuantityOfSuplement.Text);
                s.perPrice = Convert.ToDouble(txtPerPrice.Text);
                s.totalPrice = Convert.ToDouble(txtTotalPrice.Text);
                s.discount = Convert.ToDouble(txtDiscount.Text);
                s.finalPrice = Convert.ToDouble(txtFinalPrice.Text);
                s.status = chkPaid.Checked;
                db.SubmitChanges();
                lblInfo.Text = "Successfully Updated Data, Now Redirecting...";
                lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('BuySupplements.aspx') }, 1000);", true);
            }
            db.Dispose();
        }
        protected void btnAddSuplements_Click(object sender, EventArgs e)   
        {
            var error = validateBuy();
            if (error == "")
            {
                Suplement suplementItem = new Suplement();
                suplementItem.nameOfSuplement = txtNameOfSuplement.Text;
                suplementItem.date = NepaliDateService.NepToEng(txtSuplementBuyingDate.Text);
                suplementItem.nameOfVender = txtNameOfVender.Text;
                suplementItem.branch = ddlBranch.SelectedItem.Text;
                suplementItem.quantity = Convert.ToDouble(txtQuantityOfSuplement.Text);
                suplementItem.perPrice = Convert.ToDouble(txtPerPrice.Text);
                suplementItem.totalPrice = Convert.ToDouble(txtTotalPrice.Text);
                suplementItem.discount = Convert.ToDouble(txtDiscount.Text);
                suplementItem.finalPrice = Convert.ToDouble(txtFinalPrice.Text);
                suplementItem.status = chkPaid.Checked;

                db.Suplements.InsertOnSubmit(suplementItem);
                db.SubmitChanges();
                lblInfo.Text = "Successfully Inserted Data, Now Redirecting...";
                lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('BuySupplements.aspx') }, 1000);", true);
            }
            else
            {
                lblInfo.Text = error;
                lblInfo.ForeColor = ColorTranslator.FromHtml("#E60D25");
                return;
            }
            db.Dispose();
        }
        protected string validateBuy()
        {
            txtNameOfSuplement.Style.Remove("border-color");
            txtSuplementBuyingDate.Style.Remove("border-color");
            txtNameOfVender.Style.Remove("border-color");
            txtQuantityOfSuplement.Style.Remove("border-color");
            txtPerPrice.Style.Remove("border-color");
            //txtNameOfSuplement.Style.Remove("border-color");
            if (string.IsNullOrEmpty(txtNameOfSuplement.Text))
            {
                txtNameOfSuplement.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Name of Suplement should not be empty";
            }
            else if (string.IsNullOrEmpty(txtSuplementBuyingDate.Text))
            {
                txtSuplementBuyingDate.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Date should not be empty";
            }
            else if (string.IsNullOrEmpty(txtNameOfVender.Text))
            {
                txtNameOfVender.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Name of Vender should not be empty";
            }
            else if (string.IsNullOrEmpty(txtQuantityOfSuplement.Text))
            {
                txtQuantityOfSuplement.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Quantity should not be Empty";
            }
            else if (string.IsNullOrEmpty(txtPerPrice.Text))
            {
                txtPerPrice.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Quantity should not be Empty";
            }
            else
            {
                return "";
            }
        }
        protected void loadData()
        {
            try
            {
                string tempId = Request.QueryString["ID"].ToString();

                var editQuery = (from c in db.Suplements
                                 where c.suplementId == Convert.ToInt32(tempId)
                                 select new
                                 {
                                     c.nameOfSuplement,
                                     c.date,
                                     c.nameOfVender,
                                     c.branch,
                                     c.quantity,
                                     c.perPrice,
                                     c.totalPrice,
                                     c.discount,
                                     c.finalPrice,
                                     c.suplementId,
                                     c.status
                                 }).SingleOrDefault();
                txtNameOfSuplement.Text = editQuery.nameOfSuplement;
                txtSuplementBuyingDate.Text = NepaliDateService.EngToNep(Convert.ToDateTime(editQuery.date)).ToString();
                txtNameOfVender.Text = editQuery.nameOfVender;
                ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByText(editQuery.branch));
                txtQuantityOfSuplement.Text = Convert.ToString(editQuery.quantity);
                txtPerPrice.Text = Convert.ToString(editQuery.perPrice);
                txtTotalPrice.Text = Convert.ToString(editQuery.totalPrice);
                txtDiscount.Text = Convert.ToString(editQuery.discount);
                txtFinalPrice.Text = Convert.ToString(editQuery.finalPrice);
                chkPaid.Checked = editQuery.status.Value;
                hidSnNo.Value = Convert.ToString(editQuery.suplementId);
                btnEditSuplements.Enabled = true;


            }
            catch (Exception)
            {

            }
        }
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["ID"].ToString();
            deleteBuySuplement(id);
        }
        protected void loadBranch()
        {
            if (roleId == "1" || roleId == "4")
            {
                var branchName = from p in db.Logins
                                 where !p.username.Contains("admin")
                                 select p.username;
                ddlBranch.DataSource = branchName;
                ddlBranch.DataBind();
                ddlBranch.Items.Insert(0, new ListItem("--Select--", "0"));
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