using Newtonsoft.Json;
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
    public partial class GuestList : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        static string tempId;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!IsPostBack)
            {
                try
                {
                    string key = Request.QueryString["key"];
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
            var error = validateField("new");
            if (error == "")
            {
                try
                {
                    Guest em = new Guest();
                    em.name = txtName.Text;
                    em.email = txtEmail.Text;
                    em.mobile = txtMobileNumber.Text;
                    if(txtFromDate.Text!="")
                        em.fromDate =NepaliDateService.NepToEng(txtFromDate.Text);
                    if (txtToDate.Text != "")
                        em.toDate =NepaliDateService.NepToEng(txtToDate.Text);
                    em.count = Convert.ToInt32(txtCount.Text);
                    em.attCount = 0;
                    em.created = DateTime.Now;
                    em.updated = DateTime.Now;
                    
                    db.Guests.InsertOnSubmit(em);
                    var jsonInfo = JsonConvert.SerializeObject(em);
                    lblInfo.Text = "Please wait, Sending Email....";
                    imgLoading.Visible = true;
                    GenerateQrImage(jsonInfo);
                    var emailSend = MailService.sendEmailToGuest(em.name,em.email);
                    if (emailSend == "")
                    {
                        imgLoading.Visible = false;
                        db.SubmitChanges();
                        lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                        lblInfo.Text = "Successfully Inserted Data and Email has been sent, Now Redirecting...";
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('GuestList.aspx') }, 1000);", true);
                        ClearFields();
                    }
                    else
                    {
                        lblInfo.ForeColor = ColorTranslator.FromHtml("#ff0000");
                        lblInfo.Text = "Error sending Email";
                        imgLoading.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#ff0000");
                    lblInfo.Text = ex.Message;
                    imgLoading.Visible = false;
                }
            }
            else
            {
                lblInfo.ForeColor = ColorTranslator.FromHtml("#ff0000");
                lblInfo.Text = error;
                imgLoading.Visible = false;
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
            var error = validateField("old");
            if (error == "")
            {
                try
                {
                    var em = (from p in db.Guests
                              where p.id == Convert.ToInt32(tempId)
                              select p).SingleOrDefault();
                    em.name = txtName.Text;
                    em.email = txtEmail.Text;
                    em.mobile = txtMobileNumber.Text;
                    em.fromDate = NepaliDateService.NepToEng(txtFromDate.Text);
                    em.toDate = NepaliDateService.NepToEng(txtToDate.Text);
                    em.count = Convert.ToInt32(txtCount.Text);
                    em.updated = DateTime.Now;

                    db.SubmitChanges();
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                    lblInfo.Text = "Successfully Updated Data, Now Redirecting...";
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('GuestList.aspx') }, 1000);", true);
                }
                catch (Exception ex)
                {
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#ff0000");
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
            var item = (from p in db.Guests
                        where p.id == Convert.ToInt32(tempId)
                        select p).SingleOrDefault();
            txtName.Text = item.name;
            txtEmail.Text = item.email;
            txtMobileNumber.Text = item.mobile;
            txtFromDate.Text = NepaliDateService.EngToNep(Convert.ToDateTime(item.fromDate)).ToString();
            txtToDate.Text = NepaliDateService.EngToNep(Convert.ToDateTime(item.toDate)).ToString();
            txtCount.Text =Convert.ToString(item.count);

            btnEdit.Enabled = true;
        }
        protected string validateField(string type)
        {
            txtName.Style.Remove("border-color");
            txtEmail.Style.Remove("border-color");
            txtMobileNumber.Style.Remove("border-color");
            txtFromDate.Style.Remove("border-color");
            txtToDate.Style.Remove("border-color");
            if (type == "new")
            {
                var isEmailMobile = db.Guests.Any(p => p.email == txtEmail.Text || p.mobile == txtMobileNumber.Text);
                if (isEmailMobile)
                {
                    return "Email or Mobile Number Already Exist";
                }
            }
            if (txtFromDate.Text != "")
            {
                if (txtToDate.Text == "")
                {
                    txtToDate.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                    return "To Date should not be Empty";
                }
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
            else if (string.IsNullOrEmpty(txtMobileNumber.Text))
            {
                txtMobileNumber.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Mobile Number should not be Empty";
            }
            else if (string.IsNullOrEmpty(txtCount.Text))
            {
                txtCount.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Count should not be Empty";
            }
            else
            {
                return "";
            }
            //else if (!string.IsNullOrEmpty(txtCount.Text))
            //{
            //    var returnData = "";
            //    if (string.IsNullOrEmpty(txtFromDate.Text))
            //    {
            //        txtFromDate.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
            //        returnData = "From Date should not be Empty";
            //    }
            //    else if (string.IsNullOrEmpty(txtToDate.Text))
            //    {
            //        txtToDate.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
            //        returnData = "To Date should not be Empty";
            //    }
            //    return returnData;
            //}



        }

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            int id =Convert.ToInt32(Request.QueryString["id"]);
            Delete(id); 
        }
        protected void Delete(int id)
        {
            var item = db.Guests.Where(p => p.id == id).SingleOrDefault();
            db.Guests.DeleteOnSubmit(item);
            db.SubmitChanges();
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
        public void GenerateQrImage(string qrText)
        {
            var qrTextEncrypted = Service.EncryptData(qrText);
            var em = JsonConvert.DeserializeObject<Guest>(qrText);
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
                    img.Save(Server.MapPath(@"~\Image\Guests\") + em.email + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
            }
        }
    }
}