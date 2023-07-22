using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Services;

namespace TPW_GMS
{
    public partial class MemberDashboard : System.Web.UI.Page
    {
        public string encryptedMemberId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetLoginCookies();
            }
        }
        protected void GetLoginCookies()
        {
            try
            {
                //HttpCookie loginCookie = Request.Cookies["LoginInformation"];
                //if (loginCookie != null)
                //{
                //    var todayDate = DateTime.Now;
                //    var todayEight = todayDate.ToString("yyyyMMdd");
                //    var extendedMemberId = loginCookie.Values["MemberId"] + "###" + todayEight;
                //    encryptedMemberId = Service.EncryptData(extendedMemberId);
                //}
                //HttpCookie reqCookies = Request.Cookies["LoginInformation"];
                HttpCookie loginCookie = Request.Cookies["LoginInfo"];
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("LoginInfo");
                if (loginCookie!= null)
                {
                    try
                    {
                        var loginType = loginCookie.Value.Split('#');
                        //For Client
                        if (loginType[0].Contains("Client"))
                        {
                            var memberId = loginType[1];
                            encryptedMemberId = memberId;
                        }
                        //For Staff
                        else if(loginType[0].Contains("Staff"))
                        {
                            var memberId = loginType[1];
                            var todayDate = DateTime.Now;
                            var todayEight = todayDate.ToString("yyyyMMdd");
                            var extendedMemberId = memberId + "//" + todayEight;
                            encryptedMemberId = Service.EncryptData(extendedMemberId);
                        }
                        //For Guest
                        else if (loginType[0].Contains("Guest"))
                        {
                            var memberId = loginType[1];
                            encryptedMemberId = Service.EncryptData(memberId);
                        }
                        else
                        {
                            encryptedMemberId="";
                            Response.Redirect("LoginMember.aspx");
                        }
                       
                    }
                    catch (Exception)
                    {
                        Response.Redirect("LoginMember.aspx");
                    }
                }
                else
                {
                    Response.Redirect("LoginMember.aspx");
                }
                //if (reqCookies != null)
                //{
                //    var memberId = Service.DecryptString(reqCookies["MemberId"].ToString());
                //    var todayDate = DateTime.Now;
                //    var todayEight = todayDate.ToString("yyyyMMdd");
                //    var extendedMemberId = memberId + "//" + todayEight;
                //    encryptedMemberId = Service.EncryptData(extendedMemberId);
                //}
            }
            catch (Exception)
            {

            }
        }
    }
}