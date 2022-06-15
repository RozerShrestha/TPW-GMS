using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;

namespace TPW_GMS
{
    public partial class Merchandise : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!Page.IsPostBack)
            {
                if (roleId == "1" || roleId == "2" || roleId == "4")
                {
                    try
                    {
                        loadBranch();
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
        protected void loadBranch()
        {
            if (roleId == "1" || roleId=="4")
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
                ddlBranch.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }
        protected void DeleteMerchans(string id)
        {
            var item = (from s in db.Merchans
                        where s.merchandiseId.Equals(id)
                        select s).SingleOrDefault();
            db.Merchans.DeleteOnSubmit(item);
            db.SubmitChanges();
            db.Dispose();
        }
        protected void btnAddMerchanItem_Click(object sender, EventArgs e)
        {
            var error = validateMerchan();
            if (error == "")
            {
                Merchan item = new Merchan();
                item.merchandiseName = txtMerchandiseName.Text;
                item.branch = ddlBranch.SelectedItem.Text;
                item.merchandiseType = ddlType.SelectedItem.Text;
                item.merchandiseQuantity = Convert.ToInt32(txtQuantity.Text);
                item.merchandisePrice = Convert.ToInt32(txtPrice.Text);
                item.MerchandiseStatus = chkStatus.Checked;
                if (FileUpload1.FileName != "")
                {
                    HttpPostedFile file = FileUpload1.PostedFile;
                    if (file.ContentLength < 524288)
                    {
                        string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                        string fileExtension = Path.GetExtension(filename);
                        FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Image/Merchan/") + txtMerchandiseName.Text + "_" + ddlType.SelectedItem.Text + fileExtension);
                        item.image = "Image/Merchan/" + txtMerchandiseName.Text + "_" + ddlType.SelectedItem.Text + fileExtension;
                    }
                    else
                    {
                        lblInfo.Text = "Image size is more that 512KB";
                        return;
                    }
                }
                item.modifiedDate = DateTime.Now;
                db.Merchans.InsertOnSubmit(item);
                db.SubmitChanges();
                lblInfo.Text = "Successfully Inserted Data , now redirecting...";
                lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('Merchandise.aspx') }, 1000);", true);
            }
            else
            {
                lblInfo.Text = error;
                lblInfo.ForeColor = ColorTranslator.FromHtml("#E60D25");
                return;
            }
            db.Dispose();
        }
        protected void btnEditMerchanItem_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(hidSnNo.Value);
            if (validateMerchan() == "")
            {
                var item = (from c in db.Merchans
                            where c.merchandiseId == id
                            select c).SingleOrDefault();
                item.merchandiseName = txtMerchandiseName.Text;
                item.branch = ddlBranch.SelectedItem.Text;
                item.merchandiseType = ddlType.SelectedItem.Text;
                item.merchandiseQuantity = Convert.ToInt32(txtQuantity.Text);
                item.merchandisePrice = Convert.ToInt32(txtPrice.Text);
                item.MerchandiseStatus = chkStatus.Checked;
                if (FileUpload1.FileName != "")
                {
                    HttpPostedFile file = FileUpload1.PostedFile;
                    if (file.ContentLength < 524288)
                    {
                        string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                        string fileExtension = Path.GetExtension(filename);
                        FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Image/Merchan/") + txtMerchandiseName.Text + "_" + ddlType.SelectedItem.Text + fileExtension);
                        item.image = "Image/Merchan/" + txtMerchandiseName.Text + "_" + ddlType.SelectedItem.Text + fileExtension;
                    }
                    else
                    {
                        lblPopupErrorr.Text = "Image size is more that 512KB";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorModal", "$('#errorModal').modal();", true);
                        //Response.Write("<script>alert('Image size is more that 512KB');</script>");
                        return;
                    }
                }
                item.modifiedDate = DateTime.Now;
                db.SubmitChanges();
                lblInfo.Text = "Successfully Updated Data , now redirecting...";
                lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('Merchandise.aspx') }, 200);", true);
            }
            else
            {
                lblInfo.Text = validateMerchan();
            }
            db.Dispose();
        }
        protected string validateMerchan()
        {
            txtMerchandiseName.Style.Remove("border-color");
            ddlBranch.Style.Remove("border-color");
            ddlType.Style.Remove("border-color");
            txtQuantity.Style.Remove("border-color");
            txtPrice.Style.Remove("border-color");

            if (string.IsNullOrEmpty(txtMerchandiseName.Text))
            {
                txtMerchandiseName.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Merchandise Name should not be Empty";
            }
            else if (ddlBranch.SelectedIndex==0)
            {
                ddlBranch.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Please Select Branch";
            }
            else if (ddlType.SelectedIndex == 0)
            {
                ddlType.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Please Select Merchandise Type";
            }
            else if (string.IsNullOrEmpty(txtQuantity.Text))
            {
                txtQuantity.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Quantity should not be empty";
            }
            else if (string.IsNullOrEmpty(txtPrice.Text))
            {
                txtPrice.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Price should not be empty";
            }
            else
            {
                return "";
            }
        }
        protected void LoadData()
        {
            try
            {
                string tempId = Request.QueryString["ID"].ToString();
                var editQuery = (from c in db.Merchans
                                 where c.merchandiseId == Convert.ToInt32(tempId)
                                 select c).SingleOrDefault();
                hidSnNo.Value = Convert.ToString(editQuery.merchandiseId);
                txtMerchandiseName.Text = editQuery.merchandiseName;
                ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByText(editQuery.branch));
                ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByText(editQuery.merchandiseType));
                txtQuantity.Text = editQuery.merchandiseQuantity.ToString();
                txtPrice.Text = editQuery.merchandisePrice.ToString();
                chkStatus.Checked = editQuery.MerchandiseStatus.Value;
                btnEditMerchanItem.Enabled = true;
                btnAddMerchanItem.Enabled = false;
            }
            catch (Exception)
            {

            }
        }
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["ID"].ToString();
            DeleteMerchans(id);
        }
    }
}