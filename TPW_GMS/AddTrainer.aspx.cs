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
    public partial class AddTrainer : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        public static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!IsPostBack)
            {
                if (roleId == "1" || roleId=="2")
                {
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
                    db.Dispose();
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
        protected void LoadData()
        {
            try
            {
                string tempId = Request.QueryString["ID"].ToString();
                var editQuery = (from c in db.Trainers
                                 where c.trainerId == Convert.ToInt32(tempId)
                                 select c).SingleOrDefault();
                txtFullName.Text = editQuery.fullName;
                hidSnNo.Value = Convert.ToString(editQuery.trainerId);
                ddlCatagory.SelectedIndex = ddlCatagory.Items.IndexOf(ddlCatagory.Items.FindByText(editQuery.catagory));
                txtExperience.Text = editQuery.experience;
                txtContactNo.Text = editQuery.contactNo;
                txtAddress.Text = editQuery.address;
                txtAvailableTime.Text = editQuery.availableTime;
                ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByText(editQuery.associateBranch));
                txtTPWJoinDate.Text = Convert.ToDateTime(editQuery.joinDate).ToShortDateString();
                txtTrainerDiscountCode.Text = editQuery.discountCode;
                txtCommissionPercentage.Text = editQuery.commissionPercentage.ToString();
                chkStatus.Checked = editQuery.status.Value;
                btnEditTrainer.Enabled = true;
                btnAddTrainer.Enabled = false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            db.Dispose();
        }
        protected void btnAddTrainer_Click(object sender, EventArgs e)
        {
            var error = validateTrainer();
            try
            {
                if (error == "")
                {
                    Trainer item = new Trainer();
                    item.fullName = txtFullName.Text;
                    item.catagory = ddlCatagory.SelectedItem.Text;
                    item.experience = txtExperience.Text;
                    item.contactNo = txtContactNo.Text;
                    item.address = txtAddress.Text;
                    item.availableTime = txtAvailableTime.Text;
                    item.associateBranch = ddlBranch.SelectedItem.Text;
                    item.joinDate = Convert.ToDateTime(txtTPWJoinDate.Text);
                    item.discountCode = txtTrainerDiscountCode.Text;
                    item.commissionPercentage = Convert.ToInt32(txtCommissionPercentage.Text);
                    item.status = chkStatus.Checked;

                    if (FileUpload1.FileName != "")
                    {
                        HttpPostedFile file = FileUpload1.PostedFile;
                        if (file.ContentLength < 524288)
                        {
                            string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                            string fileExtension = Path.GetExtension(filename);
                            FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Image/Trainer/") + txtFullName.Text + "_" + txtContactNo.Text + fileExtension);
                            item.image = "Image/Trainer/" + txtFullName.Text + "_" + txtContactNo.Text + fileExtension;
                        }
                        else
                        {
                            lblInfo.Text = "Image size is more that 512KB";
                            //Response.Write("<script>alert('Image size is more that 512KB');</script>");
                            return;
                        }
                    }
                    db.Trainers.InsertOnSubmit(item);
                    db.SubmitChanges();
                    lblInfo.Text = "Successfully Inserted Data , now redirecting...";
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('AddTrainer.aspx') }, 1000);", true);
                }
                else
                {
                    lblInfo.Text = error;
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#E60D25");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            db.Dispose();
        }
        protected void btnEditTrainer_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(hidSnNo.Value);
            var error = validateTrainer();
            try
            {
                if (error == "")
                {
                    var item = (from c in db.Trainers
                                where c.trainerId == id
                                select c).SingleOrDefault();
                    item.fullName = txtFullName.Text;
                    item.catagory = ddlCatagory.SelectedItem.Text;
                    item.experience = txtExperience.Text;
                    item.contactNo = txtContactNo.Text;
                    item.address = txtAddress.Text;
                    item.availableTime = txtAvailableTime.Text;
                    item.associateBranch = ddlBranch.SelectedItem.Text;
                    item.joinDate = Convert.ToDateTime(txtTPWJoinDate.Text);
                    item.discountCode = txtTrainerDiscountCode.Text;
                    item.commissionPercentage = Convert.ToInt32(txtCommissionPercentage.Text);
                    item.status = chkStatus.Checked;

                    if (FileUpload1.FileName != "")
                    {
                        HttpPostedFile file = FileUpload1.PostedFile;
                        if (file.ContentLength < 524288)
                        {
                            string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                            string fileExtension = Path.GetExtension(filename);
                            FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Image/Trainer/") + txtFullName.Text + "_" + txtContactNo.Text + fileExtension);
                            item.image = "Image/Trainer/" + txtFullName.Text + "_" + txtContactNo.Text + fileExtension;
                        }
                        else
                        {
                            lblInfo.Text = "Image size is more that 512KB";
                            //Response.Write("<script>alert('Image size is more that 512KB');</script>");
                            return;
                        }
                    }

                    db.SubmitChanges();
                    lblInfo.Text = "Successfully Updated Data , now redirecting...";
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('AddTrainer.aspx') }, 1000);", true);
                }
                else
                {
                    lblInfo.Text = validateTrainer();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            db.Dispose();
        }
        protected string validateTrainer()
        {
            ddlCatagory.Style.Remove("border-color");
            ddlBranch.Style.Remove("border-color");
            txtFullName.Style.Remove("border-color");
            txtContactNo.Style.Remove("border-color");
            txtAddress.Style.Remove("border-color");
            txtAvailableTime.Style.Remove("border-color");
            txtTPWJoinDate.Style.Remove("border-color");
            txtTrainerDiscountCode.Style.Remove("border-color");

            if (string.IsNullOrEmpty(txtFullName.Text))
            {
                txtFullName.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Full Name should not be Empty";
            }
            else if (ddlCatagory.SelectedIndex == 0)
            {
                ddlCatagory.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Please Select Catagory";
            }
            else if (string.IsNullOrEmpty(txtContactNo.Text))
            {
                txtContactNo.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Contact No should not be empty";
            }
            else if (string.IsNullOrEmpty(txtAddress.Text))
            {
                txtAddress.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Address should not be empty";
            }
            else if (string.IsNullOrEmpty(txtAvailableTime.Text))
            {
                txtAvailableTime.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Available Time should not be empty";
            }
            else if (ddlBranch.SelectedIndex == 0)
            {
                ddlBranch.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Please Select Associate Branch";
            }
            else if (string.IsNullOrEmpty(txtTPWJoinDate.Text))
            {
                txtTPWJoinDate.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "TPW Join Date should not be empty";
            }
            else if(string.IsNullOrEmpty(txtTrainerDiscountCode.Text))
            {
                txtTrainerDiscountCode.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Discount Code should not be empty";
            }
            else if (Double.IsNaN(Convert.ToDouble(txtTrainerDiscountCode.Text.Split('$')[1])))
            {
                txtTrainerDiscountCode.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Discount Code Format didnot match. Sample (aaa$5)";
            }
            else
            {
                return "";
            }
        }
        protected void loadBranch()
        {
            
            if (roleId=="1")
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
        protected void DeleteTrainer(string id)
        {
            var item = (from s in db.Trainers
                        where s.trainerId.Equals(id)
                        select s).SingleOrDefault();
            db.Trainers.DeleteOnSubmit(item);
            db.SubmitChanges();
            db.Dispose();
        }
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["ID"].ToString();
            DeleteTrainer(id);
        }
    }
}