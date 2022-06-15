using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;

namespace TPW_GMS
{
    public partial class EmailMarketingTemplate : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!Page.IsPostBack)
            {
                if (roleId == "1" || roleId == "4")
                {
                    loadCKEditorContent1();
                }
                else
                {
                    Response.Redirect("AccessDenied.aspx");
                }
            }
        }

        protected void btnCKEditor_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            var id = button.ID.Split('r')[1];
            using (TPWDataContext db = new TPWDataContext())
            {
                var item = db.EmailFormats.Where(c => c.Type == "em" + id).SingleOrDefault();
                
                var str = ((TextBox)pnlEmMarketing.FindControl("CKEditor" + id)).Text;
                var sub = ((TextBox)pnlEmMarketing.FindControl("txtSubject" + id)).Text;
                var htmlEditorContent = Server.HtmlDecode(str);
                item.message = htmlEditorContent;
                item.subject = sub;
                db.SubmitChanges();
            }
        }

        protected void loadCKEditorContent1()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var items = db.EmailFormats.Where(c=>c.Type.Contains("em"));
                foreach(var i in items)
                {
                    var id = i.Type.Substring(i.Type.Length - 1);
                    ((TextBox)pnlEmMarketing.FindControl("CKEditor" + id)).Text = i.message;
                    ((TextBox)pnlEmMarketing.FindControl("txtSubject" + id)).Text = i.subject;
                }
                
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
    }
}