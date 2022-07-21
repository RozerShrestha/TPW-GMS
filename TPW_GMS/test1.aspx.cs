using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Services;

namespace TPW_GMS
{
    public partial class test1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //UpdateTable();
            //updatePassword();
            //CreateQRBULK();
            //SendEmail();
            //TestNepaliDate();
            //updateDate();
        }
        public void UpdateTable()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var pItems = db.PaymentInfos;
                var rItems = db.Reports;
                var memItem = db.MemberInformations;
                //foreach (var item in pItems)
                //{
                //    var r = (from a in rItems
                //             where a.memberId == item.memberId
                //             select a).ToList().LastOrDefault();
                //    if (r != null)
                //    {
                //        r.discount = item.discount;
                //        r.discountReason = item.discountReason;
                //        db.SubmitChanges();
                //    }
                //}
                foreach (var item in memItem)
                {
                    var r = pItems.Where(p => p.memberId == item.memberId).SingleOrDefault();
                    r.paymentMethod = item.paymentMethod;
                    r.receiptNo = item.receiptNo;
                    db.SubmitChanges();
                }
            }
        }
        public void updatePassword()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                string[] pass = { "GUG8thhB",
"e8cmtQZd",
"Sh8cZ9a5",
"FLLr8uyp",
"G54A3f5q",
"76PayPV7",
"8dQSZjEx",
"YBKG4kNy",
"96AY5Kya",
"T7KBLujx",
"u9G9KpGt",
"sbNmJM2S",
"NWdd4kxW",
"C2PBcR6R",
"yaFrT46L",
"SWLa2LpP",
"3avVE3Zt",
"LQTB3qEV",
"gpWB9tjG",
"69uuJMWZ",
"dNEtkp3B",
"GqAC4WtA",
"th3PmEMF",
"TvGa5dVy", };
                var items = (from p in db.MemberInformations
                             where p.memberOption.Equals("Trainer") || p.memberOption.Equals("Gym Admin") || p.memberOption.Equals("Super Admin")
                             select p);
                var itemLogin = db.MemberLogins;

                int i = 0;

                foreach (var item in items)
                {
                    var loginItem = itemLogin.Where(k => k.memberId == item.memberId).SingleOrDefault();
                    loginItem.password = pass[i];
                    item.password = pass[i];
                    db.SubmitChanges();
                    i++;
                }
            }
        }
        public void SendEmail()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var items = (from p in db.MemberInformations
                             where p.memberOption.Equals("Trainer") || p.memberOption.Equals("Gym Admin") || p.memberOption.Equals("Super Admin")
                             select p);
                foreach (var item in items)
                {
                    string message = "Dear" + item.fullname + "," + Environment.NewLine + Environment.NewLine +
                        "Your Login Credential is mention below:" + Environment.NewLine +
                        "User Name: " + item.contactNo + Environment.NewLine +
                        "Password: " + item.password + Environment.NewLine + Environment.NewLine +
                        "Regards" + Environment.NewLine +
                        "The Physique Workshop";
                    new Task(() =>
                    {
                        MailService.SendEmailStaffAttendence(message, item.email, "Login Credential");
                    }).Start();
                }
            }
        }
        public void CreateQRBULK()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var items = db.MemberInformations;
                foreach (var item in items)
                {
                    GenerateQrImage(item.memberId);
                }
            }
        }
        public void GenerateQrImage(string qrText)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            imgBarCode.Height = 150;
            imgBarCode.Width = 150;
            QRCode qrCode = new QRCode(qrCodeData);
            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                    img.Save(Server.MapPath(@"~\Image\QRCode\") + qrText + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
            }
        }
        public void TestNepaliDate()
        {
            var convertedNepToEng = NepaliDateService.NepToEng(2047, 11, 17).ToString("yyyy/MM/dd");
            var convertedEngToNep = NepaliDateService.EngToNep(1991, 03, 01);
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var item = (from p in db.ExtraInformations
                            where p.extraInformationId == 1
                            select p).SingleOrDefault();

                string str = CKEditor1.Text;
                string str1 = Server.HtmlDecode(str);
                string str2 = Server.HtmlEncode(str);
                lblText.Text = "Text After HtmlDecode : " + str1;
                lbl2.Text = "Text After HtmlEncode : " + str2;

                item.marketingEmailFormat = str1;
                item.subjectMaketing = "Em Subject";
                db.SubmitChanges();
            }


        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            int count1 = 1;
            //int count2 = 1;
            using (TPWDataContext db = new TPWDataContext())
            {

                var itemss = (from p in db.PaymentInfos.GroupBy(p => p.receiptNo)
                              where p.Count() > 1
                              select p).ToList();
                foreach(var item in itemss)
                {
                    foreach(var it in item)
                    {
                        it.receiptNo = it.receiptNo +"_dup"+ count1;
                        count1++;
                    }
                }
                db.SubmitChanges();

                
            }
        }

        protected void updateDate()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                string[] memberid = {
                    "TPW-01-06215758",
                    "TPW-02-18044390",
                    "TPW-03-08385078",
                    "TPW-03-08430739",
                    "TPW-03-15113119",
                    "TPW-03-15313293",
                    "TPW-03-16040753",
                    "TPW-03-16155787",
                    "TPW-03-17062414",
                    "TPW-03-17310296",
                    "TPW-03-18225541",
                    "TPW-04-06495808",
                    "TPW-04-10453540",
                    "TPW-04-13373564",
                    "TPW-04-13550184",
                    "TPW-04-13594624",
                    "TPW-04-14033746",
                    "TPW-04-14062877",
                    "TPW-04-14113920",
                    "TPW-04-14273606",
                    "TPW-04-14532463",
                    "TPW-04-15082523",
                    "TPW-04-18211435",
                    "TPW-05-08345630",
                    "TPW-05-09050783",
                    "TPW-05-09210710",
                    "TPW-05-09595496",
                    "TPW-05-10033620",
                    "TPW-05-12452117",
                    "TPW-05-18330908",
                    "TPW-05-18415699",
                    "TPW-06-18090591",
                    "TPW-07-17330086",
                    "TPW-08-11485317",
                    "TPW-09-09394524",
                    "TPW-09-17535568",
                    "TPW-10-07333061",
                    "TPW-10-07372702",
                    "TPW-10-16535134",
                    "TPW-10-17125536",
                    "TPW-11-08184991",
                    "TPW-11-19035887",
                    "TPW-12-07185503",
                    "TPW-12-18164599",
                    "TPW-12-18345482",
                    "TPW-12-18393391",
                    "TPW-13-10312571",
                    "TPW-15-18061323",
                    "TPW-15-18150176",
                    "TPW-16-18295554",
                    "TPW-16-19235955",
                    "TPW-16-20441792",
                    "TPW-17-16521771",
                    "TPW-17-18483711",
                    "TPW-18-07203412",
                    "TPW-18-13300227",
                    "TPW-18-13322941",
                    "TPW-18-13351376",
                    "TPW-18-13585962",
                    "TPW-18-14471298",
                    "TPW-18-14521649",
                    "TPW-18-14582153",
                    "TPW-18-15013028",
                    "TPW-18-15072546",
                    "TPW-18-17233026",
                    "TPW-18-17284684",
                    "TPW-18-18112384",
                    "TPW-18-18142491",
                    "TPW-19-11250765",
                    "TPW-19-13364911",
                    "TPW-19-18564191",
                    "TPW-20-09075534",
                    "TPW-20-10280388",
                    "TPW-20-18392761",
                    "TPW-21-11435316",
                    "TPW-21-17372095",
                    "TPW-22-06375905",
                    "TPW-23-15513924",
                    "TPW-24-07485415",
                    "TPW-24-10183689",
                    "TPW-24-10242006",
                    "TPW-24-15354362",
                    "TPW-24-18453972",
                    "TPW-25-06543937",
                    "TPW-25-18350919",
                    "TPW-26-07072747",
                    "TPW-26-08424126",
                    "TPW-26-17253838",
                    "TPW-26-17490196",
                    "TPW-27-07055693",
                    "TPW-27-17054739",
                    "TPW-27-17083555",
                    "TPW-29-06404508",
                    "TPW-29-07505145",
                    "TPW-29-14222585",
                    "TPW-30-08200535",
                    "TPW-30-08250447",
                    "TPW-31-17104768",
                    "TPW-31-17331428",
                    "TPW-31-18220959"
                };
                foreach(var i in memberid)
                {
                    var item = (from p in db.MemberInformations
                                where p.memberId == i
                                select p).SingleOrDefault();
                    var nepDate = Convert.ToDateTime(item.dateOfBirth).ToShortDateString();



                    var engDob = NepaliDateService.NepToEng(nepDate);
                    item.dateOfBirth = Convert.ToDateTime(engDob);
                    db.SubmitChanges();

                }

            }
        }
    }
}