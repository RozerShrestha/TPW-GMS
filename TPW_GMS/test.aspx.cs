using QRCoder;
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
using TPW_GMS.Services;

namespace TPW_GMS
{
    public partial class test : System.Web.UI.Page
    {
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            //CreateNewGuestQR();
        }
        public LoginUserInfo checkSession()
        {
            LoginUserInformation l = new LoginUserInformation();
            LoginUserInfo uInfo = new LoginUserInfo();
            var loginUser = Session["userDb"].ToString();
            List<string> splitUser = new List<string>(loginUser.Split(new string[] { "-" }, StringSplitOptions.None));
            int roleId = l.getLoginUSerRole(loginUser);
            if (loginUser == null)
                Response.Redirect("SignIn.aspx");

            uInfo.loginUser = loginUser;
            uInfo.roleId = roleId.ToString();
            uInfo.splitUser = splitUser[0];

            return uInfo;
        }
        public void CreateNewGuestQR()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var guestList = db.Guests.ToList();
                foreach (var guest in guestList)
                {
                    var qrTextEncrypted = Service.EncryptData(guest.email);
                    //var em = JsonConvert.DeserializeObject<Guest>(qrText);
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrTextEncrypted, QRCodeGenerator.ECCLevel.Q);
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
                            img.Save(Server.MapPath(@"~\Image\Guests\") + guest.email + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                        }
                    }
                }
            }
        }
    }
}