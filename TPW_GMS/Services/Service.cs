using NLog;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using TPW_GMS.Data;
using TPW_GMS.Models;


namespace TPW_GMS.Services
{
    public class Service
    {
        const string passphrase = "TPWP@ssw0rd123#";
        private static TPWDataContext db = new TPWDataContext();
        private static Logger _logger;
        public Service()
        {
            _logger = LogManager.GetLogger("f");
        }
        //private TPWDataContext db = new TPWDataContext();
        public static LoginUserInfo checkSession()
        {
            try
            {
                LoginUserInformation l = new LoginUserInformation();
                LoginUserInfo uInfo = new LoginUserInfo();
                var loginUser =(String)HttpContext.Current.Session["userDb"];
                if (loginUser != null)
                {
                    List<string> splitUser = new List<string>(loginUser.Split(new string[] { "-" }, StringSplitOptions.None));
                    int roleId = getLoginUSerRole(loginUser);
                    uInfo.loginUser = loginUser;
                    uInfo.roleId = roleId.ToString();
                    uInfo.splitUser = splitUser[0];
                    return uInfo;
                }
                else
                {
                    return uInfo = null;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }
        public static int getLoginUSerRole(string username)
        {
            var userRole = (from p in db.Logins
                            where p.username.Equals(username)
                            select p.roleId).SingleOrDefault();
            if (userRole != null)
                return Convert.ToInt16(userRole);
            else
                return 0;
        }
        public static string EncryptData(string Message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToEncrypt = UTF8.GetBytes(Message);
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return Convert.ToBase64String(Results);
        }
        public static string DecryptString(string Message)
        {
            //_logger.Info("##"+"Message for Decription: " + Message);
            byte[] Results;
            string returnResult;

            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            try
            {
                byte[] DataToDecrypt = Convert.FromBase64String(Message);
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                returnResult = UTF8.GetString(Results);
            }
            catch (Exception ex)
            {
                returnResult = ex.Message;
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return returnResult;
        }
        //for Member Login Only
        public static MemberInformation2 getMemberInfo(string encryptedMemberId)
        {
            string memberId = "";
            CultureInfo provider = CultureInfo.InvariantCulture;
           

            if (encryptedMemberId.Contains("TPW"))
            {
                memberId = encryptedMemberId;
            }
            else
            {
                var decryptedMemberId = Service.DecryptString(encryptedMemberId);
                var splitData = Regex.Split(decryptedMemberId, @"//");
                memberId = splitData[0];
                var todayDate8 = splitData[1];
                var todayDate = DateTime.ParseExact(todayDate8, "yyyyMMdd", provider);
            }
            var tday = DateTime.Now;
            var tt = DateTime.Today;
            var pday = tday.AddDays(-30);
            
            var memberInfo = (from p in db.MemberInformations
                              where p.memberId.Equals(memberId)
                              select new MemberInformation2
                              {
                                  memberId = p.memberId,
                                  branch = p.branch,
                                  fullname = p.fullname,
                                  contactNo = p.contactNo,
                                  email = p.email,
                                  dateOfBirth = p.dateOfBirth.ToString(),
                                  address = p.address,
                                  gender = p.gender,
                                  memberOption = p.memberOption,
                                  memberCatagory = p.memberCatagory,
                                  memberSubCatagory = p.memberSubCatagory,
                                  memberDate = p.memberDate.ToString(),
                                  memberBeginDate = p.memberBeginDate.ToString(),
                                  memberExpireDate = p.memberExpireDate.ToString(),
                                  shift = p.shift,
                                  imageLoc = p.imageLoc
                              }).SingleOrDefault();
            memberInfo.memberPaymentHistorys = (from c in db.Reports
                                                where c.memberId.Equals(memberId)
                                                select new MemberPaymentHistory
                                                {
                                                    memberId = c.memberId,
                                                    receiptNo = c.receiptNo,
                                                    memberBeginDate = c.memberBeginDate.ToString(),
                                                    memberExpireDate = c.memberExpireDate.ToString(),
                                                    memberOption = c.memberOption,
                                                    memberCatagory = c.memberCatagory,
                                                    memberPaymentType = c.memberPaymentType,
                                                    finalAmount = c.finalAmount.ToString()
                                                }).ToList();
            memberInfo.memberAttendances = (from m in db.MemberInformations
                                            join a in db.MemberAttandances on m.memberId equals a.memberId
                                            where a.checkin >= Convert.ToDateTime(pday) && a.checkin <= Convert.ToDateTime(tday) && m.memberId == memberId
                                            select new MemberAttendance
                                            {
                                                memberId = m.memberId,
                                                fullName = m.fullname,
                                                checkin = a.checkin == null ? "" : a.checkin.ToString(),
                                                checkout = a.checkout == null ? "" : a.checkout.ToString(),
                                                branch = a.branch,
                                                checkinBranch = a.checkinBranch
                                            }).ToList();
            memberInfo.staffAttendance = (from m in db.MemberInformations
                                          join a in db.StaffAttandances on m.memberId equals a.memberId
                                          where a.checkin >= Convert.ToDateTime(pday) && a.checkin <= Convert.ToDateTime(tday) && m.memberId == memberId
                                          select new StaffAtt
                                          {
                                              memberId = m.memberId,
                                              fullName = m.fullname,
                                              checkin = a.checkin == null ? "" : a.checkin.ToString(),
                                              checkout = a.checkout == null ? "" : a.checkout.ToString(),
                                              branch = a.branch,
                                              checkinBranch = a.checkinBranch,
                                              remark = a.remark,
                                              lateFlag=a.lateFlag

                                          }).ToList();


            return memberInfo;
        }
        public static string ResetPassword(MemberLoginCredential m)
        {
            try
            {
                var checkAuth = from mi in db.MemberInformations
                                join ml in db.MemberLogins on mi.memberId equals ml.memberId
                                where mi.contactNo.Equals(m.username) && ml.password.Equals(m.password)
                                select new
                                {
                                    mi.contactNo,
                                    ml.password
                                };

                var userInfo = (from p in db.MemberInformations
                                where p.contactNo.Equals(m.username)
                                select p).SingleOrDefault();
                var userLoginInfo = (from p in db.MemberLogins
                                     where p.memberId.Equals(userInfo.memberId)
                                     select p).SingleOrDefault();
                if (checkAuth.Any())
                {
                    userInfo.password = m.newPassword;
                    userLoginInfo.password = m.newPassword;
                    db.SubmitChanges();
                    return "Success";
                }
                else
                {
                    return "Username or Password is Incorrect";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
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
                    img.Save(HttpContext.Current.Server.MapPath(@"~\Image\QRCode\") + qrText + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
            }
        }
        //public static bool CheckReceiptNumberValidity(string receiptNumber, string splitUser)
        //{
        //    using (TPWDataContext db1 = new TPWDataContext())
        //    {
        //        int receiptNum = Convert.ToInt32(receiptNumber);
        //        var item = db1.Logins.Where(p => p.username == splitUser).SingleOrDefault();
        //        if (receiptNum - 1 == Convert.ToInt32(item.currentBillNumber))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}
        //public static string LoadReceiptNumber(string splitUser)
        //{
        //    using (TPWDataContext db1 = new TPWDataContext())
        //    {
        //        var itemBranch = db1.Logins.Where(p => p.username == splitUser).SingleOrDefault();
        //        var receiptNum = (Convert.ToInt32(itemBranch.currentBillNumber) + 1).ToString("D3");
        //        return receiptNum;
        //    }
        //} 
    }

}