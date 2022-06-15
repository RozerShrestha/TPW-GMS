using Newtonsoft.Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Services;

namespace TPW_GMS
{
    public partial class EmailMarketing : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        static string tempId;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!IsPostBack)
            {
                loadBranch();
                //pnlEmailMarketing.Enabled = roleId == "1" ? false : true;

                try
                {
                    string key = Request.QueryString["key"].ToString();
                    if (key == "edit")
                    {
                        btnAdd.Enabled = false;
                        loadData();
                    }
                    else if (key == "delete")
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "deleteConfirmModal", "$('#deleteConfirmModal').modal();", true);
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var error = validateField();
            if (error == "")
            {
                try
                {
                    EmMarketing em = new EmMarketing();
                    em.name = txtName.Text;
                    em.email = txtEmail.Text;
                    em.mobile = txtMobileNumber.Text;
                    em.branch = ddlBranch.SelectedItem.Text;
                    em.createdDate = DateTime.Now;
                    em.attCount = 0;
                    em.flag = false;
                    em.mailCount = 0;
                    db.EmMarketings.InsertOnSubmit(em);
                    db.SubmitChanges();

                    var jsonInfo = JsonConvert.SerializeObject(em);

                    GenerateQrImage(jsonInfo);
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                    lblInfo.Text = "Successfully Updated Data, Now Redirecting...";
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('EmailMarketing.aspx') }, 1000);", true);
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorModal11", "loadData()", true);
                    ClearFields();
                }
                catch (Exception ex)
                {
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#ff0000");
                    lblInfo.Text = ex.Message;
                }

            }
            else
            {
                lblInfo.ForeColor = ColorTranslator.FromHtml("#ff0000");
                lblInfo.Text = error;
            }
        }
        private void ClearFields()
        {
            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                tb.Text = string.Empty;

            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            var error = validateField();
            if (error == "")
            {
                try
                {
                    var em = (from p in db.EmMarketings
                              where p.id == Convert.ToInt32(tempId)
                              select p).SingleOrDefault();
                    em.name = txtName.Text;
                    em.email = txtEmail.Text;
                    em.mobile = txtMobileNumber.Text;
                    em.branch = ddlBranch.SelectedItem.Text;
                    em.modifiedDate = DateTime.Now;
                    db.SubmitChanges();
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                    lblInfo.Text = "Successfully Updated Data, Now Redirecting...";
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('EmailMarketing.aspx') }, 1000);", true);
                }
                catch (Exception ex)
                {

                    lblInfo.Text = ex.Message;
                }

            }
            else
            {
                lblInfo.Text = error;

            }
        }

        public void loadData()
        {
            tempId = Request.QueryString["ID"];
            var item = (from p in db.EmMarketings
                        where p.id == Convert.ToInt32(tempId)
                        select p).SingleOrDefault();
            txtName.Text = item.name;
            txtEmail.Text = item.email;
            txtMobileNumber.Text = item.mobile;
            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByText(item.branch));
            btnEdit.Enabled = true;
        }
        protected string validateField()
        {
            txtName.Style.Remove("border-color");
            txtEmail.Style.Remove("border-color");
            var isEmailExist = db.MemberInformations.Any(p => p.email == txtEmail.Text);
            if (isEmailExist)
            {
                return "Email Already Exist";
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Name should not be Empty";
            }
            else if (string.IsNullOrEmpty(txtEmail.Text))
            {
                txtEmail.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Email should not be Empty";
            }
            else
            {
                return "";
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
            using (TPWDataContext db = new TPWDataContext())
            {
                var branchName = (from p in db.Logins
                                  where !p.firstname.Contains("admin")
                                  select p.username);
                ddlBranch.DataSource = branchName;
                ddlBranch.DataBind();
                ddlBranch.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }
        public void GenerateQrImage(string qrText)
        {
            var em = JsonConvert.DeserializeObject<EmMarketing>(qrText);
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
                    img.Save(Server.MapPath(@"~\Image\QRMarketing\") + em.email + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
            }
        }

    }
}