using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;

namespace TPW_GMS
{
    public partial class MessageBroadcast : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //txtDateNewsletter.Text = DateTime.Now.ToShortDateString();
                //InitialCheck();
                //loadGymMember();
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
        public void loadGymMember()
        {
            ddlGymMember.Items.Clear();
            var items = (from p in db.MemberInformations
                         select p);
            if (ddlMemberOption.SelectedValue != "0")
            {
                items = items.Where(c => c.memberOption == ddlMemberOption.SelectedItem.Text);
            }
            if (txtDateNewsletter.Text != "")
            {
                items = items.Where(c => c.memberExpireDate < Convert.ToDateTime(txtDateNewsletter.Text));
            }
            //var items = (from p in db.MemberInformations
            //             where p.memberOption==ddlMemberOption.SelectedItem.Text && p.memberExpireDate < Convert.ToDateTime(txtDateNewsletter.Text)
            //             orderby p.fullname descending
            //             select p);
            //var cc = items.Count();
            items = items.OrderByDescending(p => p.fullname);
            foreach (var item in items)
            {
                //ddlGymMember.Items.Insert(0,new ListItem (item.fullname + " - " + Convert.ToDateTime(item.memberExpireDate).ToShortDateString(), item.email));
                ddlGymMember.Items.Insert(0, new ListItem(item.fullname, item.email));
            }
            db.Dispose();
        }
        protected void btnLoadMember_Click(object sender, EventArgs e)
        {
            loadGymMember();
        }
        protected bool SendEmail(string emailContent, string toEmail)
        {
            try
            {
                var extraInfo = db.ExtraInformations;
                var email = (from c in extraInfo
                             where c.extraInformationId == 1
                             select c).SingleOrDefault();
                string txtEmail = email.email;
                string password = email.password;
                string txtSubject = "";
                string txtBody = emailContent;

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
                    ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Email sent.');", true);
                    db.Dispose();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}