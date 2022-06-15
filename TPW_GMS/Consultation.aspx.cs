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
    public partial class Consultation : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!IsPostBack)
            {
                LoadCharge();
                LoadTrainer();
                LoadClient();
                try
                {
                    string key = Request.QueryString["key"].ToString();
                    string tempId = Request.QueryString["ID"].ToString();
                    if (key == "edit")
                    {
                        LoadData(tempId);
                        ddlTrainer.Enabled = false;
                        txtDateConsultation.Enabled = false;
                        ddlClientName.Enabled = false;
                        ddlClientId.Enabled = false;
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
        protected void LoadCharge()
        {
            var consulrationFee = (from p in db.ConsultationCharges where p.consChargeId == 1 select p).SingleOrDefault();
            txtFee.Text = consulrationFee.consCharge;
        }
        protected void LoadTrainer()
        {
            var trainer = (from c in db.Trainers
                           select c.fullName);
            List<String> trainerList = trainer.ToList();
            ddlTrainer.DataSource = trainerList;
            ddlTrainer.DataBind();
            ddlTrainer.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        protected void LoadClient()
        {
            var clientName = (from p in db.MemberInformations
                              select p.fullname);
            List<string> clientNameList = clientName.ToList();
            for (int i = 0; i < clientNameList.Count; i++)
            {
                ddlClientName.Items.Add(clientNameList[i].ToString());
            }
            ddlClientName.Items.Insert(0, new ListItem("--Select--", "0"));
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
        protected void LoadData(string id)
        {
            try
            {
                var items = (from c in db.ConsultationLogs
                             where c.consultationId == Convert.ToInt32(id)
                             select c).SingleOrDefault();
                hidSnNo.Value = Convert.ToString(items.consultationId);
                ddlTrainer.SelectedIndex = ddlTrainer.Items.IndexOf(ddlTrainer.Items.FindByText(items.trainerName));
                txtDateConsultation.Text = Convert.ToDateTime(items.date).ToShortDateString();
                ddlClientName.SelectedIndex = ddlClientName.Items.IndexOf(ddlClientName.Items.FindByText(items.memberName));
                ddlClientId.Items.Add(items.memberId);
                txtFee.Text = items.charge.ToString();
                txtDiscount.Text = items.discount.ToString();
                txtFinalFee.Text = items.finalCharge.ToString();
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(items.status));
                txtReceiptNo.Text = items.receiptNo;
                chkPaid.Checked = items.isPaid.Value;
                btnEditConsultation.Enabled = true;
                btnAddConsultation.Enabled = false;
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
            }
        }
        protected void btnAddConsultation_Click(object sender, EventArgs e)
        {
            try
            {
                var error = validateFIeld();
                if (error == "")
                {
                    ConsultationLog l = new ConsultationLog();
                    ComissionPaymentLog inf = new ComissionPaymentLog();
                    l.trainerName = ddlTrainer.SelectedItem.Text;
                    l.memberName = ddlClientName.Text;
                    l.memberId = ddlClientId.Text;
                    l.date = Convert.ToDateTime(txtDateConsultation.Text);
                    l.charge = Convert.ToInt32(txtFee.Text);
                    l.discount = Convert.ToInt32(txtDiscount.Text);
                    l.finalCharge = Convert.ToInt32(txtFinalFee.Text);
                    l.status = ddlStatus.SelectedItem.Text;
                    l.receiptNo = txtReceiptNo.Text;
                    l.isPaid = chkPaid.Checked;

                    if (ddlStatus.SelectedItem.Text == "Booked")
                    {
                        var trainerCommission = (from c in db.ConsultationCharges
                                                 select c.consFeeToTrainer).SingleOrDefault();
                        int commissionRate = trainerCommission.Value;

                        inf.date = DateTime.Now;
                        inf.name = ddlTrainer.SelectedItem.Text;
                        inf.commissionFor = "Trainer";
                        inf.discountCode = "N/A";
                        inf.comission = commissionRate;
                        inf.memberId = ddlClientId.SelectedItem.Text;
                        inf.memberName = ddlClientName.SelectedItem.Text;
                        inf.status = false;
                        db.ComissionPaymentLogs.InsertOnSubmit(inf);
                    }
                    db.ConsultationLogs.InsertOnSubmit(l);
                    db.SubmitChanges();
                    lblInfo.Text = "Successfully Inserted Data, Now Redirecting..";
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('Consultation.aspx') }, 500);", true);
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
                lblInfo.ForeColor = ColorTranslator.FromHtml("#E60D25");
            }
            db.Dispose();
        }
        protected void btnEditConsultation_Click(object sender, EventArgs e)
        {
            try
            {
                ComissionPaymentLog inf = new ComissionPaymentLog();
                int id = Convert.ToInt32(hidSnNo.Value);
                var error = validateFIeld();
                if (error == "")
                {
                    var l = (from c in db.ConsultationLogs
                             where c.consultationId == id
                             select c).SingleOrDefault();
                    l.trainerName = ddlTrainer.SelectedItem.Text;
                    l.memberName = ddlClientName.Text;
                    l.memberId = ddlClientId.Text;
                    l.date = Convert.ToDateTime(txtDateConsultation.Text);
                    l.charge = Convert.ToInt32(txtFee.Text);
                    l.discount = Convert.ToInt32(txtDiscount.Text);
                    l.finalCharge = Convert.ToInt32(txtFinalFee.Text);
                    l.receiptNo = txtReceiptNo.Text;
                    l.isPaid = chkPaid.Checked;

                    if (l.status != "Booked" && ddlStatus.SelectedItem.Text == "Booked")
                    {
                        var trainerCommission = (from c in db.ConsultationCharges
                                                 select c.consFeeToTrainer).SingleOrDefault();
                        int commissionRate = trainerCommission.Value;

                        inf.date = DateTime.Now;
                        inf.name = ddlTrainer.SelectedItem.Text;
                        inf.commissionFor = "Trainer";
                        inf.discountCode = "N/A";
                        inf.comission = commissionRate;
                        inf.memberId = ddlClientId.SelectedItem.Text;
                        inf.memberName = ddlClientName.SelectedItem.Text;
                        inf.status = false;
                        db.ComissionPaymentLogs.InsertOnSubmit(inf);
                    }
                    l.status = ddlStatus.SelectedItem.Text;
                    db.SubmitChanges();
                    lblInfo.Text = "Successfully Updated Data, Now Redirecting..";
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('Consultation.aspx') }, 500);", true);
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
                lblInfo.ForeColor = ColorTranslator.FromHtml("#E60D25");
            }
            db.Dispose();
        }
        protected string validateFIeld()
        {

            ddlTrainer.Style.Remove("border-color");
            txtDateConsultation.Style.Remove("border-color");
            ddlClientName.Style.Remove("border-color");
            txtFee.Style.Remove("border-color");
            txtDiscount.Style.Remove("border-color");
            ddlStatus.Style.Remove("border-color");
            txtReceiptNo.Style.Remove("border-color");
            if (ddlTrainer.SelectedIndex == 0)
            {
                ddlTrainer.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Select Trainer";
            }
            else if (txtDateConsultation.Text == "")
            {
                txtDateConsultation.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Choose Date";
            }
            else if (ddlClientName.SelectedIndex == 0)
            {
                ddlClientName.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Select Client Name";
            }
            else if (txtFee.Text == "")
            {
                txtFee.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Charge should not be Empty";
            }
            else if (txtDiscount.Text == "")
            {
                txtDiscount.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Discount Field should not be Empty";
            }
            else if (ddlStatus.SelectedIndex == 0)
            {
                ddlStatus.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Select one of the Status";
            }
            else if (txtReceiptNo.Text == "")
            {
                txtReceiptNo.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Receipt No should not be empty";
            }
            else
                return "";
        }
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["ID"].ToString();
            deleteConsultation(id);
        }
        protected void deleteConsultation(string id)
        {
            var consultation = (from c in db.ConsultationLogs
                                where c.consultationId.Equals(id)
                                select c).SingleOrDefault();
            db.ConsultationLogs.DeleteOnSubmit(consultation);
            db.SubmitChanges();
            db.Dispose();
        }
    }
}