using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TPW_API.Models;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Services;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using System.Globalization;
using System.Text.RegularExpressions;
using TPW_GMS.Repository;
using Dapper;
using System.Diagnostics;

namespace TPW_GMS.Controllers
{
    [Authorize]
    public class EmailController : ApiController
    {
        private TPWDataContext db = new TPWDataContext();
        public static string conString;
        public DbConFactory dbCon = new DbConFactory();
        private Logger _logger;

        public EmailController()
        {
            _logger = LogManager.GetLogger("f");
        }
        [Route("api/SendPendingEmail/")]
        [HttpGet]
        public IHttpActionResult SendPendingEmail(String memberId)
        {
            var response = MailService.SendEmail(memberId);
            return Ok(response);
        }

        [Route("api/SendBulkEmail")]
        [HttpPost]
        public IHttpActionResult SendBulkEmail(Email em)
        {
            try
            {
                if (em.type == "QR")
                {
                    var orgMessage = em.message;
                    foreach (string item in em.fullName_email)
                    {
                        List<string> splitData = new List<string>(item.Trim().Split(new string[] { "##" }, StringSplitOptions.None));
                        var alterMessage = orgMessage.Replace("$", splitData[0]);
                        //new Task(() =>
                        //{
                        var response = MailService.SendEmailQR(alterMessage, splitData[1], splitData[2], em.subject, em.path);
                        if (response == "")
                        {
                            _logger.Info("##" + "QR Email is Send to {0} email address {1}. ", splitData[0], splitData[2]);
                        }
                        else
                        {
                            _logger.Warn("##" + "QR Email is Not Send to {0} email address {1} due to {2} ", splitData[0], splitData[2], response);
                        }
                        //}).Start();
                    }
                    return Ok("Email are Queued to deliver to Selected GYM Members");
                }
                else if (em.type == "Normal")
                {
                    var orgMessage = em.message.Replace("<img", "<img style=\"width: 400px\"");
                    foreach (string item in em.fullName_email)
                    {
                        List<string> splitData = new List<string>(item.Trim().Split(new string[] { "##" }, StringSplitOptions.None));
                        var alterMessage = orgMessage.Replace("$", splitData[0]);
                        //new Task(() =>
                        //{
                        var response = MailService.SendBroadCastEmail(alterMessage, splitData[1], em.subject);
                        if (response == "")
                        {
                            _logger.Info("##" + "Broadcase Email is Send to {0} email address {1}. ", splitData[0], splitData[1]);
                        }
                        else
                        {
                            _logger.Warn("##" + "Broadcase Email is Not Send to {0} email address {1}. due to {2} ", splitData[0], splitData[1], response);
                        }
                        //}).Start();
                    }
                    return Ok("Email are Queued to deliver to Selected GYM Members");
                }
                else
                    return Ok("Notthing");
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }

        [Route("api/SendEmailMarketingBulk")]
        [HttpGet]
        public IHttpActionResult SendEmailMarketingBulk()
        {
            //list of people
            var items = (from p in db.EmMarketings
                         where p.flag == false && p.mailCount<5
                         select p).ToList();
            //string path = HttpContext.Current.Server.MapPath(@"~\Image\MarketingImages\");
            
            //DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            //FileInfo[] Files = d.GetFiles("*.jpg"); //Getting Text files

            foreach (var email in items)
            {
                string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, "Image\\QRMarketing\\" + email.mobile + ".jpeg");
                string emType ="em" + Convert.ToInt32(email.mailCount+1);
                var emailCon = db.EmailFormats.Where(p => p.Type == emType).SingleOrDefault();
                if (emailCon != null)
                {
                    var response = MailService.SendMarketingEmail(emailCon.message, email.email, email.name, emailCon.subject, filePath, emType);

                    //new Task(() =>
                    //{

                    if (response == "")
                    {
                        email.mailCount += 1;
                        db.SubmitChanges();
                        _logger.Info("##" + "Marketing Email Send to {0} email address {1}. ", email.name, email.email);
                    }
                    else
                    {
                        _logger.Warn("##" + "Marketing Email Not Send to {0} email address {1} due to {2}", email.name, email.email, response);
                    }
                }
                //}).Start();
            }
            return Ok("Emails are in queue to be delivered");
        }

        [Route("api/getEmailMarketingData")]
        public IEnumerable<EmMarketing> getEmailMarketingData(string branch)
        {

            return branch == "admin" || branch=="superadmin" ? db.EmMarketings : db.EmMarketings.Where(p => p.branch == branch);
        }

        [Route("api/SaveMarketingEmailFormat")]
        [HttpPatch]
        public IHttpActionResult SaveMarketingEmailFormat(Email em)
        {
            try
            {
                var item = (from p in db.ExtraInformations
                            where p.extraInformationId == 1
                            select p).SingleOrDefault();
                if (em.type == "write")
                {
                    item.marketingEmailFormat = em.message;
                    item.subjectMaketing = em.subject;
                    db.SubmitChanges();
                }
                return Json(new { status = "Success", message = new { emailFormat = item.marketingEmailFormat, subject = item.subjectMaketing } });
            }
            catch (Exception ex)
            {
                return Json(new { status = "Error", message = ex.Message });

            }

        }

        [Route("api/SaveMessageBroadCastEmailFormat")]
        [HttpPatch]
        public IHttpActionResult SaveMessageBroadCastEmailFormat(Email em)
        {
            try
            {
                var item = (from p in db.ExtraInformations
                            where p.extraInformationId == 1
                            select p).SingleOrDefault();
                if (em.type == "write")
                {
                    item.messageBroadcastEmailFormat = em.message;
                    item.subjectBroadcast = em.subject;
                    db.SubmitChanges();
                }
                return Json(new { status = "Broadcast Message is Saved", message = new { emailFormat = item.messageBroadcastEmailFormat, subject = item.subjectBroadcast } });
            }
            catch (Exception ex)
            {
                return Json(new { status = "Error", message = ex.Message });

            }

        }
    }
}
