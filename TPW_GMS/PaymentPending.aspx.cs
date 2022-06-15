using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Services;

namespace TPW_GMS
{
    public partial class PaymentPending : System.Web.UI.Page 
    {
        private TPWDataContext db = new TPWDataContext();
        const string passphrase = "TPWP@ssw0rd123#";
        string roleId, loginUser, splitUser;
        private  Logger _logger;
        public PaymentPending()
        {
            _logger = LogManager.GetLogger("f");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!IsPostBack)
            {
                if (roleId == "1")
                {
                    ddlBranch.Enabled = true;
                    loadBranch();
                }
                else if (roleId == "2" || roleId =="3")
                {
                    ddlBranch.Items.Insert(0, new ListItem(splitUser, "0"));
                    ddlBranch.Enabled = false;
                }
                bindpaymentGrid(splitUser, roleId);
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
            var branchName = (from p in db.Logins
                              where !p.firstname.Contains("admin")
                              select p.username).ToList();
            ddlBranch.DataSource = branchName;
            ddlBranch.DataBind();
            ddlBranch.Items.Insert(0, new ListItem("ALL", "0"));
        }
        protected void btnSearchPayment_Click(object sender, EventArgs e)
        {
            bindpaymentGrid(splitUser, roleId);

        }
        protected void GridViewaPayment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
            GridViewaPayment.PageIndex = e.NewPageIndex;
            try
            {
                bindpaymentGrid(splitUser, roleId);
            }
            catch (Exception)
            {

            }
        }
        protected void bindpaymentGrid(string loginUser, string roleId)
        {
            int preCheckValue = Convert.ToInt16(ddlPrePendingCheck.SelectedValue);
                var item = from c in db.MemberInformations
                           join p in db.PaymentInfos on c.memberId equals p.memberId
                           select new
                           {
                               c.memberId,
                               c.fullname,
                               c.memberOption,
                               c.email,
                               c.emailStatus,
                               c.memberCatagory,
                               c.memberPaymentType,
                               c.memberBeginDate,
                               c.memberExpireDate,
                               c.contactNo,
                               c.branch,
                               c.ActiveInactive,
                               p.finalAmount
                           };

            //if (ddlBranch.SelectedItem.Text != "ALL")
            //    item = item.Where(k => k.branch == ddlBranch.SelectedItem.Text);
            if (ddlActiveInactive.SelectedItem.Text != "All")
                item = item.Where(k => k.ActiveInactive == ddlActiveInactive.SelectedItem.Text);
            if (preCheckValue == 0)
                item = item.Where(k => k.memberExpireDate < DateTime.Now.AddDays(preCheckValue));
            else
            {
                item = item.Where(k => k.memberExpireDate >= DateTime.Now && k.memberExpireDate <= DateTime.Now.AddDays(preCheckValue));
            }
            if (ddlFlag.SelectedValue != "0")
                item = item.Where(k => k.emailStatus == Convert.ToBoolean(ddlFlag.SelectedValue));     
            if (!string.IsNullOrWhiteSpace(txtCustomerIDPayment.Text))
                item = item.Where(c => c.memberId == txtCustomerIDPayment.Text.Trim());
            if (!string.IsNullOrWhiteSpace(txtCustomerNamePayment.Text))
                item = item.Where(c => c.fullname.Contains(txtCustomerNamePayment.Text.Trim()));
            if (!string.IsNullOrWhiteSpace(txtCustomerNumberPayment.Text))
                item = item.Where(c => c.contactNo == txtCustomerNumberPayment.Text.Trim());
            if (ddlBranch.SelectedIndex != 0)
                item = item.Where(c => c.branch == ddlBranch.SelectedItem.ToString());

            GridViewaPayment.DataSource = item;
            GridViewaPayment.DataBind();
        }
        protected void GridViewaPayment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String tempId = e.CommandArgument.ToString();
            if (e.CommandName == "email")
            {
                MemberInformation items = db.MemberInformations.Where(c => c.memberId == tempId).SingleOrDefault();
                if (SendEmail(items.fullname, items.memberCatagory, items.email, items.memberPaymentType, Convert.ToDateTime(items.memberExpireDate).ToShortDateString()))
                {
                    items.emailStatus = true;
                    db.SubmitChanges();
                    bindpaymentGrid(splitUser, roleId);
                }
            }
        }

        protected void btnSentEmailAll_Click(object sender, EventArgs e)
        {
            int preCheckValue = Convert.ToInt16(ddlPrePendingCheck.SelectedValue);
            var items = (from c in db.MemberInformations
                         where c.memberExpireDate < DateTime.Now.AddDays(preCheckValue) && c.ActiveInactive == "Active" && c.emailStatus == Convert.ToBoolean(ddlFlag.SelectedValue)
                         select c).ToList();


            if (!string.IsNullOrWhiteSpace(txtCustomerIDPayment.Text))
                items = items.Where(c => c.memberId == txtCustomerIDPayment.Text.Trim()).ToList();
            if (!string.IsNullOrWhiteSpace(txtCustomerNamePayment.Text))
                items = items.Where(c => c.fullname.Contains(txtCustomerNamePayment.Text.Trim())).ToList();
            if (!string.IsNullOrWhiteSpace(txtCustomerNumberPayment.Text))
                items = items.Where(c => c.contactNo == txtCustomerNumberPayment.Text.Trim()).ToList();
            if (ddlBranch.SelectedIndex != 0)
                items = items.Where(c => c.branch == ddlBranch.SelectedItem.ToString()).ToList(); ;


            var count = items.Count();
            ClientScript.RegisterStartupScript(GetType(), "confirm", "confirm('Bulk Email will be sent in background');", true);
            foreach (var item in items)
            {
                new Task(() =>
                {
                   if(SendEmail(item.fullname, item.memberCatagory, item.email, item.memberPaymentType, Convert.ToDateTime(item.memberExpireDate).ToShortDateString()))
                    {
                        item.emailStatus = true;
                        db.SubmitChanges();
                    }
                }).Start();
            }
        }

        protected void ddlFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFlag.SelectedValue == "false")
            {
                btnSentEmailAll.Enabled = true;
            }
            else
            {
                btnSentEmailAll.Enabled = false;
            }
        }

        protected bool SendEmail(string fullname, string package, string toEmail, string memberPaymentType, string expireDate)
        {
            try
            {
                var extraInfo = db.ExtraInformations;
                var email = (from c in extraInfo
                             where c.extraInformationId == 1
                             select c).SingleOrDefault();

                string txtEmail = MailService.EmailProvider();
                string password = email.password;
                string txtSubject = "";
                string message1 = "";
                string message2 = "";

                if (Convert.ToDateTime(expireDate) < DateTime.Now)
                {
                    txtSubject = "Gym Subcription Expired";
                    message1 = "has Expired on ";
                    message2 = "Please Renew as soon as possible";
                }
                else
                {
                    txtSubject = "Gym Subcription is going to Expire";
                    message1 = "will Expire on";
                    message2 = "Please Renew Early";
                }

                string txtBody = "Dear " + fullname + "," + Environment.NewLine + Environment.NewLine +
                "Your Subcription to the GYM for the package " + package + " duration " + memberPaymentType + " " + message1 + expireDate + ". " + message2 + Environment.NewLine +
                "Thank you." + Environment.NewLine + Environment.NewLine +
                "Regards," + Environment.NewLine +
                "The Physique Workshop";
                using (MailMessage mm = new MailMessage(txtEmail, toEmail))
                {
                    mm.Subject = txtSubject;
                    mm.Body = txtBody;
                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(txtEmail, password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    _logger.Info("##" + "Email send to: " + fullname + "with Email ID " + toEmail + " Message: " + txtBody);
                }
                
                return true;
                
            }
            catch (Exception)
            {
                _logger.Warn("##" + "Email Not Send to: " + fullname +"with Email ID "+toEmail);
                return false;
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
    }
}