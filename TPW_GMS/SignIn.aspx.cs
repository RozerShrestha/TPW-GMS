using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using TPW_GMS.Data;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TPW_GMS
{
    public partial class SignIn : System.Web.UI.Page
    {
        const string passphrase = "TPWP@ssw0rd123#";
        private TPWDataContext db = new TPWDataContext();
        //private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        protected void Page_Load(object sender, EventArgs e)
        {
            checkPreRequiredTable();
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            String user = txtUserName.Text; 
            String pass = txtPassword.Text;
            var items = db.Logins;

            var Expiredate = (from c in items
                              where c.username.Equals("admin")
                              select c).SingleOrDefault();

            foreach (var item in items)
            {
                string userDb = item.username;
                string passDb = item.password;


                if (user.CompareTo(userDb) == 0 & pass.CompareTo(passDb) == 0)
                {
                    string a = EncryptData("2017-10-15");
                    DateTime expireDateValidate = Convert.ToDateTime(DecryptString(Expiredate.softwareExpireDate));
                    DateTime expireDateCheck = Convert.ToDateTime(DecryptString(Expiredate.softwareExpireCheck));
                    if (expireDateValidate.AddDays(5) == expireDateCheck)
                    {
                        DateTime dtSoftwareExpire = Convert.ToDateTime(DecryptString(Expiredate.softwareExpireDate));
                        DateTime dt = DateTime.Now;
                        //DateTime dt = GetNistTime();
                        if (dt > dtSoftwareExpire)
                        {
                            lblNotification.Text = "Software Expired!! Please Consult with Service Provider";
                            lblNotification.ForeColor = System.Drawing.Color.OrangeRed;
                            //logger.Warn("{branch}-Software Expired!! Please Consult with Service Provider", txtUserName.Text);
                            return;
                        }
                        else
                        {
                            Session["userDb"] = item.username;
                            Session["userType"] = "Employee";
                            //logger.Info("{branch}-Login Successful", txtUserName.Text);
                            Response.Redirect("Dashboard.aspx");
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallTokenFunction", "MyFunction()", true);
                        }
                    }
                    else
                    {
                        lblNotification.Text = "Expired Date modified";
                        //logger.Fatal("Expire Date Modified");
                        return;
                    }
                }
                else
                {
                    //lblNotification.Text = "Expired Date modified";
                    lblNotification.Text = "Wrong username or Password";
                    //logger.Warn("Wrong username {username} or password {password}", txtUserName.Text, txtPassword.Text);
                    lblNotification.ForeColor = System.Drawing.Color.OrangeRed;
                }
            }

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
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToDecrypt = Convert.FromBase64String(Message);
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return UTF8.GetString(Results);
        }
        public static DateTime GetNistTime()
        {
            DateTime dateTime = DateTime.MinValue;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://nist.time.gov/actualtime.cgi?lzbc=siqm9b");
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore); //No caching
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader stream = new StreamReader(response.GetResponseStream());
                string html = stream.ReadToEnd();//<timestamp time=\"1395772696469995\" delay=\"1395772696469995\"/>
                string time = Regex.Match(html, @"(?<=\btime="")[^""]*").Value;
                double milliseconds = Convert.ToInt64(time) / 1000.0;
                dateTime = new DateTime(1970, 1, 1).AddMilliseconds(milliseconds).ToLocalTime();
            }

            return dateTime;
        }
        protected void checkPreRequiredTable()
        {
            var loginItems = db.Logins.ToList();
            var roleItems = db.Roles.ToList();

            if(loginItems.Count==0 && roleItems.Count==0)
            {
                string[] roles = new string[] { "admin", "gymAdmin", "gymUser" };
                Login l = new Login();
                l.roleId = 1;
                l.firstname = "admin";
                l.username = "admin";
                l.password = "admin";
                l.softwareExpireDate = "9lxUzAwXazDEuNcsueQGEQ==";
                l.softwareExpireCheck = "M5DScU3JparEuNcsueQGEQ==";
                db.Logins.InsertOnSubmit(l);
                for (int i=1; i<=roles.Length;i++)
                {
                    Role r = new Role();
                    r.roleId = i;
                    r.role1 = roles[i - 1];
                    db.Roles.InsertOnSubmit(r);
                }
                db.SubmitChanges();
            }

        }
    }
}