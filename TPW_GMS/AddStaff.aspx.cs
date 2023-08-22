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
    public partial class AddStaff : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        public static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!IsPostBack)
            {
                if (roleId == "1" ||  roleId=="4")
                {
                    loadBranch();
                    try
                    {
                        string key = Request.QueryString["key"];
                        //new form mode
                        if (key == null)
                        {
                            ddlAssociateBranch.Enabled = false;
                        }
                        //edit mode
                        if (key == "edit")
                        {
                            //ddlStaffName.Enabled = false;
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
            }
        }
        protected void LoadData()
        {
            try
            {
                using (TPWDataContext db = new TPWDataContext())
                {
                    string tempId = Request.QueryString["ID"].ToString();
                    var editQuery = (from c in db.Staffs
                                     where c.id == Convert.ToInt32(tempId)
                                     select c).SingleOrDefault();
                    ddlStaffCatagory.SelectedIndex = ddlStaffCatagory.Items.IndexOf(ddlStaffCatagory.Items.FindByText(editQuery.staffCatagory));
                    ddlStaffName.Items.Add(editQuery.staffName);
                    ddlStaffName.SelectedIndex = ddlStaffName.Items.IndexOf(ddlStaffName.Items.FindByText(editQuery.staffName));
                    ddlCatagory.SelectedIndex = ddlCatagory.Items.IndexOf(ddlCatagory.Items.FindByText(editQuery.trainerCatagory));
                    txtContactNo.Text = editQuery.contactNo;
                    hidSnNo.Value = Convert.ToString(editQuery.id);
                    txtAddress.Text = editQuery.address;
                    ddlAssociateBranch.SelectedIndex = ddlAssociateBranch.Items.IndexOf(ddlAssociateBranch.Items.FindByText(editQuery.associateBranch));
                    //txtAssociateBranch.Text = editQuery.associateBranch;
                    txtTPWJoinDate.Text = DateTime.Parse(editQuery.JoinDate.ToString()).ToString("yyyy/MM/dd");
                    txtStaffDiscountCode.Text = editQuery.discountCode;
                    txtCommissionPercentage.Text = editQuery.commission.ToString();
                    txtExperience.Text = editQuery.experience;
                    chkStatus.Checked = editQuery.status.Value;
                    txtFrom1.Text = editQuery.from1;
                    txtTo1.Text = editQuery.to1;
                    txtFrom2.Text = editQuery.from2;
                    txtTo2.Text = editQuery.to2;
                    btnEditStaff.Enabled = true;
                    btnAddStaff.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            db.Dispose();
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
                              where !p.firstname.Contains("admin")
                              select p.username).ToList();
            ddlAssociateBranch.DataSource = branchName;
            ddlAssociateBranch.DataBind();
            ddlAssociateBranch.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        protected void btnAddEdit_Click(object sender, EventArgs e)
        {
            var btnType= (sender as Button).Text;
            var error = validateField();
            if (error == "")
            {
                try
                {
                    if (btnType == "Add")
                    {
                        Staff s = new Staff();
                        StaffSalaryDeduction sd = new StaffSalaryDeduction();
                        s.staffCatagory = ddlStaffCatagory.SelectedItem.Text;
                        s.memberId = ddlStaffName.SelectedValue;
                        s.staffName = ddlStaffName.SelectedItem.Text;
                        s.trainerCatagory = ddlCatagory.SelectedItem.Text;
                        s.contactNo = txtContactNo.Text;
                        s.address = txtAddress.Text;
                        s.associateBranch = ddlAssociateBranch.SelectedItem.Text;
                        s.JoinDate = Convert.ToDateTime(txtTPWJoinDate.Text);
                        s.discountCode = txtStaffDiscountCode.Text;
                        s.commission = Convert.ToInt32(txtCommissionPercentage.Text);
                        s.status = chkStatus.Checked;
                        s.from1 = txtFrom1.Text;
                        s.from2 = txtFrom2.Text;
                        s.to1 = txtTo1.Text;
                        s.to2 = txtTo2.Text;
                        s.experience = txtExperience.Text;
                        s.created = DateTime.Now;

                        sd.memberId = ddlStaffName.SelectedValue;
                        sd.count = 0;
                        sd.createdDate = DateTime.Now;
                        db.StaffSalaryDeductions.InsertOnSubmit(sd);
                        db.Staffs.InsertOnSubmit(s);
                        db.SubmitChanges();

                        lblInfo.Text = "Successfully Inserted Data , now redirecting...";
                        lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('AddStaff.aspx') }, 500);", true);
                    }
                    else if (btnType == "Edit")
                    {
                        int id = Convert.ToInt32(hidSnNo.Value);
                        var s = (from p in db.Staffs
                                 where p.id == id
                                 select p).SingleOrDefault();
                        s.staffCatagory = ddlStaffCatagory.SelectedItem.Text;
                        //s.memberId = ddlStaffName.SelectedValue;
                        //s.staffName = ddlStaffName.SelectedItem.Text;
                        s.trainerCatagory = ddlCatagory.SelectedItem.Text;
                        //s.contactNo = txtContactNo.Text;
                        //s.address = txtAddress.Text;
                        s.associateBranch = ddlAssociateBranch.SelectedItem.Text;
                        //s.JoinDate = Convert.ToDateTime(txtTPWJoinDate.Text);
                        s.discountCode = txtStaffDiscountCode.Text;
                        s.commission = Convert.ToInt32(txtCommissionPercentage.Text);
                        s.status = chkStatus.Checked;
                        if (!chkStatus.Checked)
                        {
                            var item = db.MemberInformations.Where(p => p.memberId == s.memberId).SingleOrDefault();
                            item.memberOption = "Regular";
                            item.memberCatagory = "Any1";
                            item.memberSubCatagory = "Gym";
                            item.ActiveInactive = "InActive";
                            item.memberExpireDate= DateTime.Now;
                            item.remark = "Auto Inactive and ExpiredDate set to today after Staff Resigned";
                        }
                        s.from1 = txtFrom1.Text;
                        s.from2 = txtFrom2.Text;
                        s.to1 = txtTo1.Text;
                        s.to2 = txtTo2.Text;
                        s.experience = txtExperience.Text;
                        s.modified = DateTime.Now;
                        db.SubmitChanges();

                        lblInfo.Text = "Successfully Inserted Data , now redirecting...";
                        lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('AddStaff.aspx') }, 500);", true);
                    }
                }
                catch (Exception ex)
                {
                    lblInfo.Text = ex.Message;
                    _logger.Error(ex);
                    return;
                }
                db.Dispose();
            }
            else
            {
                lblInfo.Text = error;
                lblInfo.ForeColor = ColorTranslator.FromHtml("#E60D25");
                return;
            }
        }
        protected void ddlStaffName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (from p in db.MemberInformations
                        where p.memberId == ddlStaffName.SelectedValue
                        select p).SingleOrDefault();
            txtContactNo.Text = item.contactNo;
            txtAddress.Text = item.address;
            ddlAssociateBranch.SelectedIndex = ddlAssociateBranch.Items.IndexOf(ddlAssociateBranch.Items.FindByText(item.branch));
            //txtAssociateBranch.Text = item.branch;
            txtTPWJoinDate.Text = item.memberDate.ToString();
            db.Dispose();
        }

        protected void btnEditStaff_Click(object sender, EventArgs e)
        {
            var error = validateField();
            if (error == "")
            {
                try
                {
                    int id = Convert.ToInt32(hidSnNo.Value);
                    var s = (from p in db.Staffs
                             where p.id == id
                             select p).SingleOrDefault();
                    s.staffCatagory = ddlStaffCatagory.SelectedItem.Text;
                    //s.memberId = ddlStaffName.SelectedValue;
                    //s.staffName = ddlStaffName.SelectedItem.Text;
                    s.trainerCatagory = ddlCatagory.SelectedItem.Text;
                    //s.contactNo = txtContactNo.Text;
                    //s.address = txtAddress.Text;
                    s.associateBranch = ddlAssociateBranch.SelectedItem.Text;
                    //s.JoinDate = Convert.ToDateTime(txtTPWJoinDate.Text);
                    s.discountCode = txtStaffDiscountCode.Text;
                    s.commission = Convert.ToInt32(txtCommissionPercentage.Text);
                    s.status = chkStatus.Checked;
                    s.from1 = txtFrom1.Text;
                    s.from2 = txtFrom2.Text;
                    s.to1 = txtTo1.Text;
                    s.to2 = txtTo2.Text;
                    s.experience = txtExperience.Text;
                    s.modified = DateTime.Now;
                    db.SubmitChanges();

                    lblInfo.Text = "Successfully Inserted Data , now redirecting...";
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('AddStaff.aspx') }, 500);", true);

                }
                catch (Exception ex)
                {
                    lblInfo.Text = ex.Message;
                    _logger.Error(ex);
                    return;
                }
                db.Dispose();
            }
            else
            {
                lblInfo.Text = error;
                lblInfo.ForeColor = ColorTranslator.FromHtml("#E60D25");
                return;
            }
        }
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(Request.QueryString["ID"]);
                var item = db.Staffs.Where(p => p.id == id).SingleOrDefault();
                var item1 = db.StaffSalaryDeductions.Where(p => p.memberId == item.memberId).SingleOrDefault();
                db.Staffs.DeleteOnSubmit(item);
                db.StaffSalaryDeductions.DeleteOnSubmit(item1);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
           
        }
        protected void ddlStaffCatagory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var items = (from p in db.MemberInformations
                         where p.memberOption.Equals(ddlStaffCatagory.SelectedItem.Text)
                         select p).ToList();
            ddlStaffName.DataSource = items;

            ddlStaffName.DataTextField = "fullname";
            ddlStaffName.DataValueField = "memberId";
            ddlStaffName.DataBind();
            ddlStaffName.Items.Insert(0, new ListItem("--Select--", "0"));

            if (ddlStaffCatagory.SelectedValue == "1")
            {
                ddlCatagory.Enabled = false;
                ddlCatagory.SelectedValue = "4";
            }
            else
            {
                ddlCatagory.Enabled = true;
                ddlCatagory.SelectedValue = "0";
            }
            db.Dispose();
        }
        protected string validateField()
        {
            ddlStaffCatagory.Style.Remove("border-color");
            ddlStaffName.Style.Remove("border-color");
            ddlCatagory.Style.Remove("border-color");
            txtContactNo.Style.Remove("border-color");
            txtAddress.Style.Remove("border-color");
            //txtAssociateBranch.Style.Remove("border-color");
            txtTPWJoinDate.Style.Remove("border-color");
            txtStaffDiscountCode.Style.Remove("border-color");
            txtCommissionPercentage.Style.Remove("border-color");
            txtFrom1.Style.Remove("border-color");
            txtTo1.Style.Remove("border-color");

            if (ddlStaffCatagory.SelectedIndex == 0)
            {
                ddlStaffCatagory.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Please select Staff Category";
            }
            if (ddlStaffName.SelectedItem.Text == "--Select--")
            {
                ddlStaffName.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Please select Staff Name";
            }
            if (ddlStaffCatagory.SelectedItem.Text == "Trainer" && ddlCatagory.SelectedIndex == 0)
            {
                ddlCatagory.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Please select Catagory";
            }
            if (!validateDiscountCode(txtStaffDiscountCode.Text))
            {
                txtStaffDiscountCode.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Invalid Discount Code, Please make sure the format is aaa$5";
            }
            if (txtFrom1.Text == "")
            {
                txtFrom1.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Invalid Entry";
            }
            if (txtTo1.Text == "")
            {
                txtTo1.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Invalid Entry";
            }
            else
            {
                return "";
            }


        }
        protected bool validateDiscountCode(string t)
        {
            try
            {
                var num = (t.Split('$')[1]).All(char.IsDigit);
                var isValid = num ? true : false;
                return isValid;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}