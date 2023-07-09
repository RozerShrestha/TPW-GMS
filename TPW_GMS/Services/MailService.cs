using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TPW_GMS.Data;
using TPW_GMS.Models;

namespace TPW_GMS.Services
{
    public class MailService
    {
        private const string passphrase = "TPWP@ssw0rd123#";
        private static TPWDataContext db = new TPWDataContext();
        private static int i = 0;
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private static ExtraInformation extraInformation = (from c in db.ExtraInformations
                                 where c.extraInformationId == 1
                                 select c).SingleOrDefault();
        private static string senderEmail = MailService.EmailProvider();
        public static bool SendEmail(string memberId)
        {
            using (TPWDataContext _db = new TPWDataContext())
            {
                var memberInfo = _db.MemberInformations.Where(p => p.memberId == memberId).SingleOrDefault();
                string txtSubject = "";
                string message1 = "";
                string message2 = "";
                if (Convert.ToDateTime(memberInfo.memberExpireDate) < DateTime.Now)
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
                //creating body format dynamically
                string txtBody = "Dear " + memberInfo.fullname + "," + Environment.NewLine + Environment.NewLine +
                "Your Subcription to the GYM for the package " + memberInfo.memberCatagory + " duration " + memberInfo.memberPaymentType + " " + message1 + memberInfo.memberExpireDate + ". " + message2 + Environment.NewLine +
                "Thank you." + Environment.NewLine + Environment.NewLine +
                "Regards," + Environment.NewLine +
                "The Physique Workshop";
                //memberInfo.email = "rozer.shrestha611@gmail.com";
                bool emailStatus = GeneralEmailFormat(isThereAttachment: false, html: false, memberInformation: memberInfo, subject: txtSubject, body: txtBody);
                //Waits for 10 second after sending email
                Thread.Sleep(10000);
                if (emailStatus)
                {
                    memberInfo.emailStatus = true;
                    db.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static string SendEmailQR(string message, string memberId, string toEmail, string subject, string path)
        {
            var memberInfo = db.MemberInformations.Where(p => p.memberId == memberId).SingleOrDefault();
            string status = "";
            bool emailStatus = GeneralEmailFormat(isThereAttachment: true, html: false, memberInformation: memberInfo, subject: subject, body: message, path:path);
            if (emailStatus)
                return status;
            else
                return "";
        }
        public static string SendMarketingEmail(string message, string toEmail, string name, string subject, string filePath, string emType)
        {
            var altMessage = message.Replace("$", name);
            string status = "";
            try
            {
                var em = EmailProvider();
                    
                var fromEmailAdd = new MailAddress(em, "The Physique Workshop");
                var toEmailAdd = new MailAddress(toEmail);
                string txtSubject = subject;

                using (MailMessage mm = new MailMessage(fromEmailAdd, toEmailAdd))
                {
                    mm.Subject = txtSubject;
                    mm.IsBodyHtml = true;
                    if(emType=="em4")
                        mm.AlternateViews.Add(getEmbeddedImageForEmailMarketing(altMessage, filePath));
                    else
                        mm.Body = altMessage;

                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    NetworkCredential NetworkCred = new NetworkCredential(senderEmail, extraInformation.password);
                    smtp.Port = Convert.ToInt32(extraInformation.port);
                    smtp.Host = extraInformation.smtpClient;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Send(mm);

                    //update the mail count
                    status = "";
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            return status;
        }
        //this email is for message broadcast
        public static string SendBroadCastEmail(string message, string toEmail, string subject)
        {
            var memberInfo = db.MemberInformations.Where(p => p.email == toEmail).SingleOrDefault();
            bool emailStatus = GeneralEmailFormat(isThereAttachment: false, html: true, memberInformation: memberInfo, subject: subject, body: message);
            if (emailStatus)
                return "";
            else
                return "error"; 
        }
        //this Email sending is for Staff Attendance Module
        public static bool SendEmailStaffAttendence(string message, string memberId, string subject)
        {
            var memberInfo = db.MemberInformations.Where(p => p.memberId == memberId).SingleOrDefault();
            bool emailStatus = GeneralEmailFormat(isThereAttachment: false, html: false, memberInformation: memberInfo, subject: subject, body: message);
            return emailStatus;
        }
        public static void sendEmailNewMember(string memberid, string username, string branch, string password, string membershipOption, string catagoryType, string membershipDate, string membershipPaymentType, string membershipBeginDate, string membershipExpireDate, string email, string fullname, string contactNo, string dateOfBirth, string address, string discountCode, string finalAmount, string paidAmount, string dueAmount)
        {
           
            try
            {
                var fromEmailAdd = new MailAddress(senderEmail, "The Physique Workshop");
                var toEmailAdd = new MailAddress(email);
                string txtSubject = "New Membership";
                using (MailMessage mm = new MailMessage(fromEmailAdd, toEmailAdd))
                {
                    mm.Subject = txtSubject;
                    //mm.Body = txtBody;
                    mm.IsBodyHtml = true;
                    mm.AlternateViews.Add(getEmbeddedImageNewMember(memberid, username, branch, password, membershipOption, catagoryType, membershipDate, membershipPaymentType, membershipBeginDate, membershipExpireDate, email, fullname, contactNo, dateOfBirth, address, discountCode, finalAmount, paidAmount, dueAmount));
                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    NetworkCredential NetworkCred = new NetworkCredential(senderEmail, extraInformation.password);
                    smtp.Port = Convert.ToInt32(extraInformation.port);
                    smtp.Host = extraInformation.smtpClient;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Send(mm);
                }
                _logger.Info("##" + "New Form-{0} Mail send to new member", username);
            }
            catch (Exception)
            {
                _logger.Warn("##" + "New Form-{0} Mail Not send to new member", username);
            }
        }
        public static void sendEmailRenewMember(string memberId)
        {
            try
            {
                
                var memberItem = (from m in db.MemberInformations
                                join p in db.PaymentInfos
                                on m.memberId equals p.memberId
                                where m.memberId==memberId
                                select new
                                {
                                    m,
                                    p
                                }).SingleOrDefault();
                var fromEmailAdd = new MailAddress(senderEmail, "The Physique Workshop");
                var toEmailAdd = new MailAddress(memberItem.m.email);
                string txtSubject = "Membership Renewal";
                string txtBody = "Dear " + memberItem.m.fullname + "," + Environment.NewLine + Environment.NewLine +
                    "Your GYM membership has been renewed as per following: " + Environment.NewLine +
                    "Membership Option: " + memberItem.m.memberOption + Environment.NewLine +
                    "Membership Catagory: " + memberItem.m.memberCatagory + Environment.NewLine +
                    "Membership Payment Type: " + memberItem.m.memberPaymentType + Environment.NewLine +
                    "Membership Renew Date: " + memberItem.m.memberBeginDate + Environment.NewLine +
                    "Membership Expire Date: " + memberItem.m.memberExpireDate + Environment.NewLine +
                    "Final Amount:" + " " + memberItem.p.finalAmount + Environment.NewLine +
                    "Paid Amount:" + " " + memberItem.p.paidAmount + Environment.NewLine +
                    "Due Amount:" + " " + memberItem.p.dueAmount + Environment.NewLine + Environment.NewLine +

                   "Thank you." + Environment.NewLine + Environment.NewLine +
                    "Regards," + Environment.NewLine +
                    "The Physique Workshop";
                using (MailMessage mm = new MailMessage(fromEmailAdd, toEmailAdd))
                {
                    mm.Subject = txtSubject;
                    mm.Body = txtBody;
                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    NetworkCredential NetworkCred = new NetworkCredential(senderEmail, extraInformation.password);
                    smtp.Port = Convert.ToInt32(extraInformation.port);
                    smtp.Host = extraInformation.smtpClient;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Send(mm);
                }
                _logger.Info("##" + "Renew-{0} Mail send to renewed member {1}", memberItem.m.memberId, memberItem.m.fullname);
            }
            catch (Exception ex)
            {
                _logger.Error("##" + "Email send error to {0} during renewing membership due to {1}",memberId, ex.Message);
            }
        }
        public static string sendEmailToGuest(string name, string toEmail)
        {
                string status = "";
                try
                {
                    var fromEmailAdd = new MailAddress(senderEmail, "The Physique Workshop");
                    var toEmailAdd = new MailAddress(toEmail);
                    string pwd = extraInformation.password;
                    string txtSubject = "Attendance Information";

                    using (var mm = new MailMessage(fromEmailAdd, toEmailAdd))
                    {
                        mm.Subject = txtSubject;
                        mm.IsBodyHtml = true;
                        mm.AlternateViews.Add(getEmbeddedImageGuestQR(name, toEmail));

                        SmtpClient smtp = new SmtpClient();
                        smtp.UseDefaultCredentials = false;
                        NetworkCredential NetworkCred = new NetworkCredential(senderEmail, extraInformation.password);
                        smtp.Port = Convert.ToInt32(extraInformation.port);
                        smtp.Host = extraInformation.smtpClient;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = NetworkCred;
                        smtp.Send(mm);
                        status = "";
                    }
                }
                catch (Exception ex)
                {
                    status = ex.Message;
                }
                return status;
        }
        public static string SendEmailToOwner(string path, string subject)
        {
            string status = "";
            try
            {
                var fromEmailAdd = new MailAddress(senderEmail, "The Physique Workshop");
                string pwd = extraInformation.password;
                string txtSubject = subject;
                Attachment attachment = new Attachment(path);

                using (var mm = new MailMessage())
                {
                    mm.Subject = txtSubject;
                    mm.IsBodyHtml = true;
                    mm.From=fromEmailAdd;
                    mm.Body = "Please find attachements";
                    mm.To.Add("rozer.shrestha611@gmail.com, sushantpradhantpw@gmail.com, thephysiqueworkshop@gmail.com");
                    mm.Attachments.Add(attachment);
                    //mm.AlternateViews.Add(getEmbeddedExcel(path));
                   
                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    NetworkCredential NetworkCred = new NetworkCredential(senderEmail, extraInformation.password);
                    smtp.Port = Convert.ToInt32(extraInformation.port);
                    smtp.Host = extraInformation.smtpClient;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Send(mm);
                    status = "";
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            return status;
        }
        public static AlternateView getEmbeddedImageNewMember(string memberid, string username, string branch, string password, string membershipOption, string catagoryType, string membershipDate, string membershipPaymentType, string membershipBeginDate, string membershipExpireDate, string email, string fullname, string contactNo, string dateOfBirth, string address, string discountCode, string finalAmount, string paidAmount, string dueAmount)
        {
            try
            {
            string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, "Image\\QRCode\\" + memberid + ".jpeg");
            LinkedResource res = new LinkedResource(filePath);
            res.ContentId = Guid.NewGuid().ToString();
            var htmlBody = @"<div>Dear " + fullname + ",</div><br />" +
                            "<div>Welcome to our TPW Family,</div>" +
                            "<div>As the newest member of our family, we are here to take care of you in your fitness journey. " +
                            "We understand that your special needs require specially designed programs and guidance for the very special person that you are." +
                            "We are here to make sure that you always have a friend and that you are never alone in your fitness journey." +
                            "We are here with you in your beginning and we will cheer you on when you reach that finish line." +
                            "Just promise us that you will never hesitate to give us a nudge when you need someone in your transformation." +
                            "We’ll be your friend, your teacher, your motivator and most importantly your cheerleaders.</div><br />" +
                            "<div>Please use This QR Code for daily Attendance Purpose</div>" +
                            "<img src='cid:" + res.ContentId + @"' style='width:200px'/>" +
                            "<span style='color: red'><b><i>You can check into any of the Branch of TPW</i></b></span>" +
                            "<h3>Following are the Branch List:</h3>" +
                            "<table>" +
                            "<thead>" +
                            "<tr style='background - color: #dcdcdc;'>" +
                            "<td><b>Branch</b></td>" +
                            "<td><b>Location</b></td>" +
                            "<td><b>Contact</b></td>" +
                            "</tr>" +
                            "</thead>" +
                            "<tbody>" +
                            "<tr><td><b>Maitidevi<b></td><td>On Top of Nepal Banijya Bank, Near Seto pul Petrol Pump</td><td>01-4432716</td></tr>" +
                            "<tr><td><b>Maharajgunj<b></td><td>On Top of SBI Bank Building, opposite to Australian Embassy</td><td>01-4017606</td></tr>" +
                            "<tr><td><b>Kumaripati<b></td><td>On Top of Trimurti Gunasabhu Bhawan, Opposite to Korean Shop</td><td>01-5521190</td></tr>" +
                            "<tr><td><b>Baneshwor<b></td><td>On Top of LG Showroom, Opposite to Shantinagar gate</td><td>01-4106800</td></tr>" +
                            "</tbody>" +
                            "</table><br>" +
                            "<span><i>You can check in a total of 12 times every month to other branches</i></span><br>" +
                            "<table>" +
                            "<tr><td>MemberId: </td><td>" + memberid + "</td></tr>" +
                            //"<tr><td>UserName: </td><td>" + username + "</td></tr>" +
                            //"<tr><td>Password: </td><td>" + password + "</td></tr>" +
                            "<tr><td>Branch: </td><td>" + branch + "</td></tr>" +
                            "<tr><td>Membership Option: </td><td>" + membershipOption + "</td></tr>" +
                            "<tr><td>Membership Catagory: </td><td>" + catagoryType + "</td></tr>" +
                            "<tr><td>Membership Date: </td><td>" + membershipDate + "</td></tr>" +
                            "<tr><td>Membership Payment Type: </td><td>" + membershipPaymentType + "</td></tr>" +
                            "<tr><td>Membership Renew Date: </td><td>" + membershipBeginDate + "</td></tr>" +
                            "<tr><td>Membership Expire Date: </td><td>" + membershipExpireDate + "</td></tr>" +
                            "<tr><td>Email: </td><td>" + email + "</td></tr>" +
                            "<tr><td>Full Name: </td><td>" + fullname + "</td></tr>" +
                            "<tr><td>Contact No: </td><td>" + contactNo + "</td></tr>" +
                            "<tr><td>Date Of Brith: </td><td>" + dateOfBirth + "</td></tr>" +
                                "<tr><td>Address: </td><td>" + address + "</td></tr>" +
                            "<tr><td>Discount Code: </td><td>" + discountCode + "</td></tr>" +
                            "<tr><td>Final Amount: </td><td>" + finalAmount + "</td></tr>" +
                            "<tr><td>Paid Amount: </td><td>" + paidAmount + "</td></tr>" +
                            "<tr><td>Due Amount: </td><td>" + dueAmount + "</td></tr>" +
                            " </table><br />" +
                            "<div>Thank you.</div>" +
                            "<div>Regards,</div>" +
                            "<div>The Physique Workshop</div>";

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;

            }
            catch (Exception)
            {

                throw;
            }

        }
        private static AlternateView getEmbeddedImageQR(string memberId, string message, string path)
        {
            string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, "Image\\QRCode\\" + memberId + ".jpeg");
            LinkedResource res = new LinkedResource(filePath);
            res.ContentId = Guid.NewGuid().ToString();
            var htmlBody = @"" + message + "<br>" +
                "<div>Please use This QR Code for daily Attendance Purpose</div>" +
                "<img src='cid:" + res.ContentId + @"' style='width:200px'/><br>" +
                "<span style='color: red'><b><i>You can also checkin into any of the Branch of TPW</i></b></span>" +
                "<h3>Following are the Branch List:</h3>" +
                "<table>" +
                "<thead>" +
                "<tr style='background - color: #dcdcdc;'>" +
                "<td><b>Branch</b></td>" +
                "<td><b>Location</b></td>" +
                "<td><b>Contact</b></td>" +
                "</tr>" +
                "</thead>" +
                "<tbody>" +
                "<tr><td><b>Maitidevi<b></td><td>On Top of Nepal Banijya Bank, Near Seto pul Petrol Pump</td><td>01-4432716</td></tr>" +
                "<tr><td><b>Maharajgunj<b></td><td>On Top of SBI Bank Building, opposite to Australian Embassy</td><td>01-4017606</td></tr>" +
                "<tr><td><b>Kumaripati<b></td><td>On Top of Trimurti Gunasabhu Bhawan, Opposite to Korean Shop</td><td>01-5521190</td></tr>" +
                "<tr><td><b>Baneshwor<b></td><td>On Top of LG Showroom, Opposite to Shantinagar gate</td><td>01-4106800</td></tr>" +
                "</tbody>" +
                "</table><br>" +
                    "<span><i>You can check in a total of 12 times every month to other branches</i></span><br>" +
                "<b>Regards,<br>" +
                "The Physique Workshop";

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;
        }
        private static AlternateView getEmbeddedImageGuestQR(string name, string email)
        {
            string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, "Image\\Guests\\" + email + ".jpeg");
            //string filePath = newPath;
            LinkedResource res = new LinkedResource(filePath);
            res.ContentId = Guid.NewGuid().ToString();
            var htmlBody = @"" + "Dear "+ name + ",<br>" +
                "<div>Please use This QR Code for Attendance Purpose</div>" +
                "<img src='cid:" + res.ContentId + @"' style='width:200px'/><br>" +
                "<span style='color: red'><b><i>You can checkin into any of the Branch of TPW</i></b></span>" +
                "<h3>Following are the Branch List:</h3>" +
                "<table>" +
                "<thead>" +
                "<tr style='background - color: #dcdcdc;'>" +
                "<td><b>Branch</b></td>" +
                "<td><b>Location</b></td>" +
                "<td><b>Contact</b></td>" +
                "</tr>" +
                "</thead>" +
                "<tbody>" +
                "<tr><td><b>Maitidevi<b></td><td>On Top of Nepal Banijya Bank, Near Seto pul Petrol Pump</td><td>01-4432716</td></tr>" +
                "<tr><td><b>Maharajgunj<b></td><td>On Top of SBI Bank Building, opposite to Australian Embassy</td><td>01-4017606</td></tr>" +
                "<tr><td><b>Kumaripati<b></td><td>On Top of Trimurti Gunasabhu Bhawan, Opposite to Korean Shop</td><td>01-5521190</td></tr>" +
                "<tr><td><b>Baneshwor<b></td><td>On Top of LG Showroom, Opposite to Shantinagar gate</td><td>01-4106800</td></tr>" +
                "</tbody>" +
                "</table><br>" +
                "<b>Regards,<br>" +
                "The Physique Workshop";

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;
        }
       
        private static AlternateView getEmbeddedImageForEmailMarketing(string message, string filePath)
        {
            LinkedResource res = new LinkedResource(filePath);
            res.ContentId = Guid.NewGuid().ToString();
            var imgReplace = "<img src='cid:" + res.ContentId + @"' style='width:200px'/><br>";
            var htmlBody = message.Replace("{qr}", imgReplace);
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;

        }
        private static bool GeneralEmailFormat( bool isThereAttachment, bool html, MemberInformation memberInformation, string subject, string body, string path="", string emType="" )
        {
            try
            {
                var fromEmailAdd = new MailAddress(senderEmail, "The Physique Workshop");
                var toEmailAdd = new MailAddress(memberInformation.email);
                using (MailMessage mm = new MailMessage(fromEmailAdd, toEmailAdd))
                {
                    mm.Subject = subject;
                    if (!isThereAttachment)
                    {
                        mm.Body = body;
                    }
                    mm.IsBodyHtml = html;
                    if (isThereAttachment)
                    {
                        mm.AlternateViews.Add(getEmbeddedImageQR(memberInformation.memberId, body, path));
                    }
                    
                    SmtpClient smtp = new SmtpClient();
                    NetworkCredential NetworkCred = new NetworkCredential(senderEmail, extraInformation.password);
                    smtp.Port = Convert.ToInt32(extraInformation.port);
                    smtp.Host = extraInformation.smtpClient;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Send(mm);
                    _logger.Info("##" + "Email send to: " + memberInformation.fullname + " with Email ID " + memberInformation.email + " Message: " + body);
                    return true;
                }
            }
            catch (Exception ex)
             {
                _logger.Warn("##" + "Email Not Send to: " + memberInformation.fullname + " with Email ID " + memberInformation.email + " due to " + ex.Message);
                return false;
            }
            
        }
        public static string EmailProvider()
        {
            var em = (from p in db.ExtraInformations
                      where p.extraInformationId == 1
                      select p.email).SingleOrDefault();

            var emSelect = em.Split('#')[i];
            i++;
            i = i > 5 ? 0 : i;
            return emSelect;
        }
    }
}