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
    public partial class TrainerBooking : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!Page.IsPostBack)
            {
                if (roleId == "1" || roleId == "4")
                {
                    //branch.Enabled = true;
                    loadBranch();
                }
                else if (roleId == "2" || roleId == "3")
                {
                    branch.Items.Insert(0, new ListItem(splitUser));
                    branch.Enabled = false;
                    //LoadTrainer(branch.Text);
                    LoadTrainer();
                }
                //LoadClient(splitUser);
                LoadClient();
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
        }
        protected void loadBranch()
        {
            var branchName = from p in db.Logins
                             where !p.username.Contains("admin")
                             select p.username;
            branch.DataSource = branchName;
            branch.DataBind();
            branch.Items.Insert(0, new ListItem("ALL", "ALL"));
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
        protected void LoadTrainer()
        {
            var trainer = (from c in db.Staffs
                           where  c.status == true //&& c.associateBranch==branch
                           //where c.staffCatagory=="Trainer"
                           select c.staffName);
            List<String> trainerList = trainer.ToList();
            ddlTrainer.DataSource = trainerList;
            ddlTrainer.DataBind();
            ddlTrainer.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        protected void LoadClient()
        {
            var clientName = (from p in db.MemberInformations
                              //where p.branch==branch
                              select p.fullname);
            List<string> clientNameList = clientName.ToList();
            for (int i = 0; i < clientNameList.Count; i++)
            {
                ddlClientName.Items.Add(clientNameList[i].ToString());
            }
            ddlClientName.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        protected void ddlTrainer_TextChanged(object sender, EventArgs e)
        {
            var trainerInfo = (from p in db.Staffs
                               where p.staffName == ddlTrainer.SelectedItem.ToString()
                               select p).SingleOrDefault();
            txtClass.Text = trainerInfo.trainerCatagory.ToString();
            db.Dispose();
        }
        protected void ddlClientName_TextChanged(object sender, EventArgs e)
        {
            ddlClientId.Items.Clear();
            var customerId = (from p in db.MemberInformations
                              where p.fullname == ddlClientName.SelectedItem.ToString()
                              select p.memberId);

            List<String> customerIdList = customerId.ToList();
            for (int i = 0; i < customerIdList.Count; i++)
            {
                ddlClientId.Items.Add(customerIdList[i].ToString());
            }
            db.Dispose();
        }
        protected void ddlNoOfTrainee_SelectedIndexChanged(object sender, EventArgs e)
        {
            double differenceDate =  (NepaliDateService.NepToEng(txtTo.Text) - NepaliDateService.NepToEng(txtFrom.Text)).TotalDays;
            if (differenceDate >= 27 && differenceDate <= 31)
            {
                var charge = (from p in db.FeeTypes
                              where p.membershipType == ddlNoOfTrainee.SelectedItem.Text
                              select p).SingleOrDefault();
                txtFee.Text = charge.oneMonth.ToString();
                txtFinalFee.Text = charge.oneMonth.ToString();
            }
            else if (differenceDate >= 85 && differenceDate <= 95)
            {
                var charge = (from p in db.FeeTypes
                              where p.membershipType == ddlNoOfTrainee.SelectedItem.Text
                              select p).SingleOrDefault();
                txtFee.Text = charge.threeMonth.ToString();
                txtFinalFee.Text= charge.threeMonth.ToString();
            }
            else
            {
                lblInfo.Text = "Difference between From and To date should be 1 month(28-31 days) or 3 month(85-95 days)";
                lblInfo.ForeColor = ColorTranslator.FromHtml("#E60D25");
                txtFee.Text = "0";
                txtFinalFee.Text = "0";
            }
            db.Dispose();
        }
        protected void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            txtFinalFee.Text = (Convert.ToInt32(txtFee.Text) - Convert.ToInt32(txtDiscount.Text)).ToString();
        }
        protected string validateFIeld()
        {
            ddlTrainer.Style.Remove("border-color");
            txtFrom.Style.Remove("border-color");
            txtTo.Style.Remove("border-color");
            ddlClientName.Style.Remove("border-color");
            ddlNoOfTrainee.Style.Remove("border-color");
            txtDiscount.Style.Remove("border-color");
            ddlStatus.Style.Remove("border-color");
            if (ddlTrainer.SelectedIndex == 0)
            {
                ddlTrainer.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Select Trainer";
            }
            else if (txtFrom.Text == "")
            {
                txtFrom.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "From Field should not be Empty";
            }
            else if (txtTo.Text == "")
            {
                txtTo.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "To Field should not be Empty";
            }
            else if (ddlClientName.SelectedIndex == 0)
            {
                ddlClientName.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Select Client Name";
            }
            else if (ddlNoOfTrainee.SelectedIndex == 0)
            {
                ddlNoOfTrainee.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Select No of Trainee";
            }
            else if (txtDiscount.Text == "")
            {
                txtDiscount.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Discount Field should not be Empty, please input 0 if no discount";
            }
            else if (ddlStatus.SelectedIndex == 0)
            {
                ddlStatus.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Select one of the Status";
            }
            else
                return "";
        }
        protected void btnAddTrainerBooking_Click(object sender, EventArgs e)
        {
            try
            {
                var error = validateFIeld();
                if (error == "")
                {
                    TrainerBookingLog tbl = new TrainerBookingLog();
                    ComissionPaymentLog inf = new ComissionPaymentLog();
                    tbl.branch = branch.SelectedItem.Text;
                    tbl.trainerName = ddlTrainer.SelectedItem.Text;
                    tbl.Class = txtClass.Text;
                    tbl.from =NepaliDateService.NepToEng(txtFrom.Text);
                    tbl.to = NepaliDateService.NepToEng(txtTo.Text);
                    tbl.clientName = ddlClientName.SelectedItem.Text;
                    tbl.memberId = ddlClientId.SelectedItem.Text;
                    tbl.noOFTrainee = ddlNoOfTrainee.SelectedItem.Text;
                    tbl.charge = Convert.ToInt32(txtFee.Text);
                    tbl.discount = Convert.ToInt32(txtDiscount.Text);
                    tbl.finalCharge = Convert.ToInt32(txtFinalFee.Text);

                    tbl.status = ddlStatus.SelectedItem.Text;
                    if (ddlStatus.SelectedItem.Text == "Booked")
                    {
                        var trainerCommission = (from p in db.TrainerClassCommissions
                                                 where p.catagory == txtClass.Text
                                                 select p.commission).SingleOrDefault();
                        float commissionRate = trainerCommission.Value;
                        int Fee = Convert.ToInt32(txtFee.Text);

                        inf.date = DateTime.Now;
                        inf.name = ddlTrainer.SelectedItem.Text;
                        inf.commissionFor = "Trainer";
                        inf.discountCode = "N/A";
                        inf.comission = Convert.ToInt32(commissionRate / 100 * Fee);
                        inf.memberId = ddlClientId.SelectedItem.Text;
                        inf.memberName = ddlClientName.SelectedItem.Text;
                        inf.status = false;
                        db.ComissionPaymentLogs.InsertOnSubmit(inf);
                        //tbl.TrainerCommission = commissionRate / 100 * Fee;
                    }
                    db.TrainerBookingLogs.InsertOnSubmit(tbl);
                    db.SubmitChanges();
                    lblInfo.Text = "Successfully Inserted Data, Now Redirecting..";
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('TrainerBooking.aspx') }, 1000);", true);
                }
                else
                {
                    lblInfo.Text = error;
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#E60D25");
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorModal", "$('#errorModal').modal();", true);
                    return;
                }
            }
            catch (Exception ex)
            {

                lblInfo.Text = ex.Message;
            }
            finally
            {
                db.Dispose();
            }
            
        }
        protected void btnEditTrainerBooking_Click(object sender, EventArgs e)
        {
            ComissionPaymentLog inf = new ComissionPaymentLog();
            int id = Convert.ToInt32(hidSnNo.Value);
            if (validateFIeld() == "")
            {
                var tbl = (from p in db.TrainerBookingLogs
                           where p.trainerBookingId == id
                           select p).SingleOrDefault();
                tbl.branch = branch.SelectedItem.Text;
                tbl.trainerName = ddlTrainer.SelectedItem.Text;
                tbl.Class = txtClass.Text;
                tbl.from = NepaliDateService.NepToEng(txtFrom.Text);
                tbl.to = NepaliDateService.NepToEng(txtTo.Text);
                tbl.clientName = ddlClientName.SelectedItem.Text;
                tbl.memberId = ddlClientId.SelectedItem.Text;
                tbl.noOFTrainee = ddlNoOfTrainee.SelectedItem.Text;
                tbl.charge = Convert.ToInt32(txtFee.Text);
                tbl.discount = Convert.ToInt32(txtDiscount.Text);
                tbl.finalCharge = Convert.ToInt32(txtFinalFee.Text);

                if (tbl.status != "Booked" && ddlStatus.SelectedItem.Text == "Booked")
                {
                    var trainerCommission = (from p in db.TrainerClassCommissions
                                             where p.catagory == txtClass.Text
                                             select p.commission).SingleOrDefault();
                    float commissionRate = trainerCommission.Value;
                    int Fee = Convert.ToInt32(txtFee.Text);

                    inf.date = DateTime.Now;
                    inf.name = ddlTrainer.SelectedItem.Text;
                    inf.commissionFor = "Trainer";
                    inf.discountCode = "N/A";
                    inf.comission = Convert.ToInt32(commissionRate / 100 * Fee);
                    inf.memberId = ddlClientId.SelectedItem.Text;
                    inf.memberName = ddlClientName.SelectedItem.Text;
                    inf.status = false;
                    db.ComissionPaymentLogs.InsertOnSubmit(inf);
                }
                tbl.status = ddlStatus.SelectedItem.Text;
                db.SubmitChanges();
                lblInfo.Text = "Successfully Updated Data, Now Redirecting..";
                lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('TrainerBooking.aspx') }, 1000);", true);
            }
            else
            {
                //Label lbl = this.Master.FindControl("lblPopupErrorr") as Label;
                lblPopupErrorr.Text = validateFIeld();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorModal", "$('#errorModal').modal();", true);
                return;
            }
            db.Dispose();
        }
        protected void LoadData()
        {
            try
            {
                string tempId = Request.QueryString["ID"].ToString();
                var editQuery = (from c in db.TrainerBookingLogs
                                 where c.trainerBookingId == Convert.ToInt32(tempId)
                                 select new
                                 {
                                     c.trainerBookingId,
                                     c.branch,
                                     c.trainerName,
                                     c.Class,
                                     c.@from,
                                     c.to,
                                     c.clientName,
                                     c.memberId,
                                     c.noOFTrainee,
                                     c.charge,
                                     c.discount,
                                     c.finalCharge,
                                     c.status,
                                 }).SingleOrDefault();
                hidSnNo.Value = Convert.ToString(editQuery.trainerBookingId);
                branch.SelectedIndex = branch.Items.IndexOf(branch.Items.FindByText(editQuery.branch));
                ddlTrainer.SelectedIndex = ddlTrainer.Items.IndexOf(ddlTrainer.Items.FindByText(editQuery.trainerName));
                txtClass.Text = editQuery.Class;
                txtFrom.Text = NepaliDateService.EngToNep(Convert.ToDateTime(editQuery.from)).ToString();
                txtTo.Text = NepaliDateService.EngToNep(Convert.ToDateTime(editQuery.to)).ToString();
                ddlClientName.SelectedIndex = ddlClientName.Items.IndexOf(ddlClientName.Items.FindByText(editQuery.clientName));
                ddlClientId.Items.Add(editQuery.memberId);
                ddlNoOfTrainee.SelectedIndex = ddlNoOfTrainee.Items.IndexOf(ddlNoOfTrainee.Items.FindByText(editQuery.noOFTrainee));
                txtFee.Text = editQuery.charge.ToString();
                txtDiscount.Text = editQuery.discount.ToString();
                txtFinalFee.Text = editQuery.finalCharge.ToString();
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(editQuery.status));
                btnAddTrainerBooking.Enabled = false;
                btnEditTrainerBooking.Enabled = true;
            }
            catch (Exception)
            {

            }
        }
        protected void DeleteTrainerLog(string id)
        {
            var item = (from s in db.TrainerBookingLogs
                        where s.trainerBookingId.Equals(id)
                        select s).SingleOrDefault();
            db.TrainerBookingLogs.DeleteOnSubmit(item);
            db.SubmitChanges();
            db.Dispose();
        }
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["ID"].ToString();
            DeleteTrainerLog(id);
        }
    }
}