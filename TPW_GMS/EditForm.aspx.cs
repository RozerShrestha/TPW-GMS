using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Services;

namespace TPW_GMS
{
    public partial class EditForm : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        //private TPWDataContext _db;

        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        static string a1, a3, a6, a12, z1, z3, z6, z12, gy1, gy3, gy6, gy12, c1, c3, c6, c12, d1, d3, d6, d12, s1, s3, s6, s12, g1, g3, g6, g12, p1, p3, p6, p12;
        public string roleId, loginUser, splitUser;
        static int btnclickStatus = 0;
        public string nepDate;
        public string mId;
        public bool qrshow = false;
        public static string preMemberExpDate;
        readonly string[] regular = new string[] { "--Select--", "Any1", "Any2", "Any3" };
        readonly string[] offhour = new string[] { "--Select--", "Any1", "Any2" };
        readonly string[] universal = new string[] { "--Select--", "Any1", "Any2" };
        readonly string[] Trainer = new string[] { "Any3" };
        readonly string[] Gym_Admin = new string[] { "Any3" };
        readonly string[] Super_Admin = new string[] { "Any3" };
        readonly string[] Free_User = new string[] { "N/A" };
        readonly string[] Staff = new string[] { "Any3" };
        readonly string[] select = new string[] { "" };
        readonly string[] any1 = new string[] { "Gym", "Zumba", "Cardio" };
        readonly string[] any2 = new string[] { "Gym Zumba", "Gym Cardio", "Zumba Cardio" };
        readonly string[] any3 = new string[] { "Gym Zumba Cardio" };
        public string state = "collapse";


        //public EditForm()
        //{
        //    _db = new TPWDataContext();
        //}
        protected void btnGenerateDate_Click(object sender, EventArgs e)
        {
            if (ddlMembershipPaymentType.SelectedItem.Text == "Select")
            {
                ddlMembershipPaymentType.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please select Payment Type first');", true);
            }
            else if (txtGenerateDate.Text == "")
            {
                txtGenerateDate.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Highlighted field sould not be empty');", true);
            }
            else
            {
                txtGenerateDate.Style.Remove("border-color");
                ddlMembershipPaymentType.Style.Remove("border-color");
                txtMembershipBeginDate.Text = txtGenerateDate.Text;
                try
                {
                    DateTime dt = Convert.ToDateTime(NepaliDateService.NepToEng(txtMembershipBeginDate.Text));
                    int i = ddlMemberOption.SelectedValue=="9"?3: Convert.ToInt32(ddlMembershipPaymentType.SelectedValue);
                    DateTime dt2 = (dt.AddMonths(i));
                    txtMembershipExpireDate.Text = NepaliDateService.EngToNep(dt2).ToString();
                }
                catch (Exception ex)
                {
                    _logger.Error("##" + "Edit Form-{0}", ex.Message);
                }
            }
        }

        const string passphrase = "TPWP@ssw0rd123#";
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "DropdownColor", "activeInactiveBGChange()", true);
            InitialCheck();
            txtMembershipBeginDate.Enabled = roleId == "1" ? true : false;
            txtMembershipExpireDate.Enabled = roleId == "2" ? true : false;
            if (roleId == "2") { ddlRenewExtendNormal.Items.Insert(2, new ListItem("Extend", "3")); }

            loadInfo();
            if (!IsPostBack)
            {
                txtChangeInStartStopDate.Enabled = roleId == "2" ? true : false;
                loadData();
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
        protected void FieldStatus(int roleId)
        {
            bool enab = true, disab = false;
            if (roleId == 1 || roleId == 2)
            {
                txtMembershipDate.Enabled = enab;
                txtMembershipBeginDate.Enabled = enab;
                //txtMembershipExpireDate.Enabled = enab;
                ddlShift.Enabled = enab;
                txtEmail.Enabled = enab;
                //txtFirstName.Enabled = enab;
                //txtLastName.Enabled = enab;
                txtContactNo.Enabled = enab;
                txtDateOfBirth.Enabled = enab;
                txtAddress.Enabled = enab;
                txtEmergencyContactPerson.Enabled = enab;
                ddlGender.Enabled = enab;
                txtEmergencyContactPhone.Enabled = enab;
                ddlGymAnytimeBefore.Enabled = enab;
                txtHowLong.Enabled = enab;
                txtAnyHealthIssue.Enabled = enab;
                txtPaymentAmount.Enabled = enab;
                txtAdmissionFee.Enabled = enab;
                txtFinalAmount.Enabled = enab;
                txtDueAmount.Enabled = enab;
                txtDiscountCode.Enabled = enab;
            }
            else
            {
                txtMembershipDate.Enabled = disab;
                txtMembershipBeginDate.Enabled = disab;
                txtMembershipExpireDate.Enabled = disab;
                ddlShift.Enabled = disab;
                txtEmail.Enabled = disab;
                //txtFirstName.Enabled = disab;
                //txtLastName.Enabled = disab;
                txtContactNo.Enabled = disab;
                txtDateOfBirth.Enabled = disab;
                txtAddress.Enabled = disab;
                txtEmergencyContactPerson.Enabled = disab;
                ddlGender.Enabled = disab;
                txtEmergencyContactPhone.Enabled = disab;
                ddlGymAnytimeBefore.Enabled = disab;
                txtHowLong.Enabled = disab;
                txtAnyHealthIssue.Enabled = disab;
                txtPaymentAmount.Enabled = disab;
                txtAdmissionFee.Enabled = disab;
                txtFinalAmount.Enabled = disab;
                txtDueAmount.Enabled = disab;
                txtDiscountCode.Enabled = disab;
            }
        }
        protected void loadInfo()
        {
            var item = db.ExtraInformations.SingleOrDefault();
            txtStatic.Text = item.currentNepaliDate + splitUser;
        }
        protected void loadDropdownValue()
        {
            if (ddlMemberOption.SelectedValue == "1")
            {
                ddlCatagoryType.DataSource = regular;
                ddlCatagoryType.DataBind();
            }
            else if (ddlMemberOption.SelectedValue == "2")
            {
                ddlCatagoryType.DataSource = offhour;
                ddlCatagoryType.DataBind();
            }
            else if (ddlMemberOption.SelectedValue == "3")
            {
                ddlCatagoryType.DataSource = universal;
                ddlCatagoryType.DataBind();
            }
            else if(ddlMemberOption.SelectedValue == "5" || ddlMemberOption.SelectedValue == "6" || ddlMemberOption.SelectedValue == "7" || ddlMemberOption.SelectedValue == "9")
            {
                ddlCatagoryType.DataSource = Staff;
                ddlCatagoryType.DataBind();
            }
            //else if (ddlMemberOption.SelectedValue == "Trainer")
            //{
            //    ddlCatagoryType.DataSource = Trainer;
            //    ddlCatagoryType.DataBind();

            //}
            //else if (ddlMemberOption.SelectedItem.Text == "Gym Admin")
            //{
            //    ddlCatagoryType.DataSource = Gym_Admin;
            //    ddlCatagoryType.DataBind();
            //}
            //else if (ddlMemberOption.SelectedItem.Text == "Super Admin")
            //{
            //    ddlCatagoryType.DataSource = Gym_Admin;
            //    ddlCatagoryType.DataBind();
            //}
            else if (ddlMemberOption.SelectedValue == "8")
            {
                ddlCatagoryType.DataSource = Free_User;
                ddlCatagoryType.DataBind();
            }

            else if (ddlMemberOption.SelectedValue == "0")
            {
                ddlCatagoryType.DataSource = select;
                ddlCatagoryType.DataBind();
            }
        }
        protected void loadDefaultImage()
        {
            image1.ImageUrl = "~/Assets/Images/sample.jpg?" + DateTime.Now.Ticks.ToString();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string filename = FileUpload1.FileName;
                string fileExt = Path.GetExtension(filename);
                if (!Directory.Exists(MapPath(@"~\Image\Members\")))
                {
                    Directory.CreateDirectory(MapPath(@"~\Image\Members\"));
                }
                // Specify the upload directory
                string directory = Server.MapPath(@"~\Image\Members\");
                // Create a bitmap of the content of the fileUpload control in memory
                Bitmap originalBMP = new Bitmap(FileUpload1.FileContent);
                // Calculate the new image dimensions
                int origWidth = originalBMP.Width;
                int origHeight = originalBMP.Height;
                int sngRatio = origWidth / origHeight;
                int newWidth = 0;
                int newHeight = 0;

                if (origWidth < 1000)
                {
                    newWidth = origWidth / 2;
                    newHeight = origHeight / 2;
                }
                else if (origWidth < 2000)
                {
                    newWidth = origWidth / 4;
                    newHeight = origHeight / 4;
                }
                else if (origWidth < 3000)
                {
                    newWidth = origWidth / 6;
                    newHeight = origHeight / 6;
                }

                // Create a new bitmap which will hold the previous resized bitmap
                Bitmap newBMP = new Bitmap(originalBMP, newWidth, newHeight);
                // Create a graphic based on the new bitmap
                Graphics oGraphics = Graphics.FromImage(newBMP);

                // Set the properties for the new graphic file
                oGraphics.SmoothingMode = SmoothingMode.AntiAlias; oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // Draw the new graphic based on the resized bitmap
                oGraphics.DrawImage(originalBMP, 0, 0, newWidth, newHeight);

                // Save the new graphic file to the server
                newBMP.Save(directory + txtMemberId.Text + fileExt);

                // Once finished with the bitmap objects, we deallocate them.
                originalBMP.Dispose();
                newBMP.Dispose();
                oGraphics.Dispose();

                // Write a message to inform the user all is OK
                lblFileUploadErrorMessage.Text = "File Name: <b style='color: red;'>" + filename + "</b><br>";
                lblFileUploadErrorMessage.Text += "Content Type: <b style='color: red;'>" + FileUpload1.PostedFile.ContentType + "</b><br>";
                lblFileUploadErrorMessage.Text += "File Size: <b style='color: red;'>" + FileUpload1.PostedFile.ContentLength.ToString() + "</b>";
                // Display the image to the user
                image1.Visible = true;
                image1.ImageUrl = @"~\Image\Members\" + txtMemberId.Text + fileExt;
                hidImage.Value = txtMemberId.Text + fileExt;
            }
        }
        protected void LoadFees()
        {
            var aerobicsQuery = (from p in db.Catagories
                                 where p.catagoryId.Equals(1)
                                 select p).SingleOrDefault();
            a1 = aerobicsQuery.oneMonth;
            a3 = aerobicsQuery.threeMonth;
            a6 = aerobicsQuery.sixMonth;
            a12 = aerobicsQuery.twelveMonth;

            var zumbaQuery = (from p in db.Catagories
                              where p.catagoryId.Equals(2)
                              select p).SingleOrDefault();
            z1 = zumbaQuery.oneMonth;
            z3 = zumbaQuery.threeMonth;
            z6 = zumbaQuery.sixMonth;
            z12 = zumbaQuery.twelveMonth;

            var silverQuery = (from p in db.Catagories
                               where p.catagoryId.Equals(3)
                               select p).SingleOrDefault();
            s1 = silverQuery.oneMonth;
            s3 = silverQuery.threeMonth;
            s6 = silverQuery.sixMonth;
            s12 = silverQuery.twelveMonth;

            var gymQuery = (from p in db.Catagories
                            where p.catagoryId.Equals(4)
                            select p).SingleOrDefault();
            gy1 = gymQuery.oneMonth;
            gy3 = gymQuery.threeMonth;
            gy6 = gymQuery.sixMonth;
            gy12 = gymQuery.twelveMonth;

            var cardioQuery = (from p in db.Catagories
                               where p.catagoryId.Equals(5)
                               select p).SingleOrDefault();
            c1 = cardioQuery.oneMonth;
            c3 = cardioQuery.threeMonth;
            c6 = cardioQuery.sixMonth;
            c12 = cardioQuery.twelveMonth;

            var goldQuery = (from p in db.Catagories
                             where p.catagoryId.Equals(6)
                             select p).SingleOrDefault();
            g1 = goldQuery.oneMonth;
            g3 = goldQuery.threeMonth;
            g6 = goldQuery.sixMonth;
            g12 = goldQuery.twelveMonth;

            var platinumQuery = (from p in db.Catagories
                                 where p.catagoryId.Equals(7)
                                 select p).SingleOrDefault();

            p1 = platinumQuery.oneMonth;
            p3 = platinumQuery.threeMonth;
            p6 = platinumQuery.sixMonth;
            p12 = platinumQuery.twelveMonth;

            var diamondQuery = (from p in db.Catagories
                                where p.catagoryId.Equals(8)
                                select p).SingleOrDefault();

            d1 = diamondQuery.oneMonth;
            d3 = diamondQuery.threeMonth;
            d6 = diamondQuery.sixMonth;
            d12 = diamondQuery.twelveMonth;

            db.Dispose();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string validationError = "";
            if (ddlMemberOption.SelectedValue == "1" || ddlMemberOption.SelectedValue == "2" || ddlMemberOption.SelectedValue == "3" || ddlMemberOption.SelectedValue == "4")
            {
                validationError = validateField();
            }
            else
            {
                validationError = validateNotPayingMember();
            }
            if (!string.IsNullOrEmpty(validationError))
            {
                lblPopupError.Text = validationError;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorModal", "$('#errorModal').modal();", true);
                return;
            }
            else
            {
                update();
            }

        }
        protected void ddlGymAnytimeBefore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGymAnytimeBefore.SelectedValue == "2")
            {
                txtHowLong.Visible = true;
                txtHowLong.Visible = true;
            }
            else if (ddlGymAnytimeBefore.SelectedValue == "3")
                txtHowLong.Visible = false;
        }
        protected void ddlCatagoryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCatagoryType.SelectedIndex == 1)
            {
                ddlSubCatagoryType.DataSource = any1;
                ddlSubCatagoryType.DataBind();
            }
            else if (ddlCatagoryType.SelectedIndex == 2)
            {
                ddlSubCatagoryType.DataSource = any2;
                ddlSubCatagoryType.DataBind();
            }
            else if (ddlCatagoryType.SelectedIndex == 3)
            {
                ddlSubCatagoryType.DataSource = any3;
                ddlSubCatagoryType.DataBind();
            }

            updatePrice();
        }
        protected void ddlMembershipPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            updatePrice();
        }
        protected void updatePrice()
        {
            if (ddlMemberOption.SelectedValue == "1" || ddlMemberOption.SelectedValue == "2" || ddlMemberOption.SelectedValue == "3" || ddlMemberOption.SelectedValue == "4")
            {
                for (int i = 1; i <= 4; i++)
                {
                    for (int j = 1; j <= 3; j++)
                    {
                        for (int k = 1; k <= 4; k++)
                        {
                            if (ddlMemberOption.SelectedIndex == i && ddlCatagoryType.SelectedIndex == j && ddlMembershipPaymentType.SelectedIndex == k)
                            {
                                //use index instead of item
                                if (k == 1)
                                {
                                    var fee = (from c in db.FeeTypes
                                               where (c.membershipOption == ddlMemberOption.SelectedItem.ToString() && c.membershipType == ddlCatagoryType.SelectedItem.Text.ToString())
                                               select c.oneMonth).SingleOrDefault();
                                    txtPaymentAmount.Text = fee.ToString();
                                }
                                else if (k == 2)
                                {
                                    var fee = (from c in db.FeeTypes
                                               where (c.membershipOption == ddlMemberOption.SelectedItem.ToString() && c.membershipType == ddlCatagoryType.SelectedItem.Text.ToString())
                                               select c.threeMonth).SingleOrDefault();
                                    txtPaymentAmount.Text = fee.ToString();
                                }
                                else if (k == 3)
                                {
                                    var fee = (from c in db.FeeTypes
                                               where (c.membershipOption == ddlMemberOption.SelectedItem.ToString() && c.membershipType == ddlCatagoryType.SelectedItem.Text.ToString())
                                               select c.sixMonth).SingleOrDefault();
                                    txtPaymentAmount.Text = fee.ToString();
                                }
                                else if (k == 4)
                                {
                                    var fee = (from c in db.FeeTypes
                                               where (c.membershipOption == ddlMemberOption.SelectedItem.ToString() && c.membershipType == ddlCatagoryType.SelectedItem.Text.ToString())
                                               select c.twelveMonth).SingleOrDefault();
                                    txtPaymentAmount.Text = fee.ToString();

                                }
                            }
                        }
                    }
                }
                try
                {
                    txtMembershipBeginDate.Text =NepaliDateService.EngToNep(Convert.ToDateTime(preMemberExpDate)).ToString();
                    DateTime dt = Convert.ToDateTime(preMemberExpDate);
                    int i = Convert.ToInt32(ddlMembershipPaymentType.SelectedValue);
                    DateTime dt2 = (dt.AddMonths(i));
                    txtMembershipExpireDate.Text = NepaliDateService.EngToNep(dt2).ToString();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                txtPaymentAmount.Text = Convert.ToString(0);
            }
            db.Dispose();
        }
        protected void ddlMemberOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMemberOption.SelectedValue == "1")
            {
                ddlCatagoryType.DataSource = regular;
                ddlCatagoryType.DataBind();
            }
            else if (ddlMemberOption.SelectedValue == "2")
            {
                ddlCatagoryType.DataSource = offhour;
                ddlCatagoryType.DataBind();
            }
            else if (ddlMemberOption.SelectedValue == "3")
            {
                ddlCatagoryType.DataSource = universal;
                ddlCatagoryType.DataBind();
            }
            else if (ddlMemberOption.SelectedValue == "5" || ddlMemberOption.SelectedValue == "6" || ddlMemberOption.SelectedValue == "7" || ddlMemberOption.SelectedValue == "9")
            {
                ddlCatagoryType.DataSource = Staff;
                ddlCatagoryType.DataBind();
                ddlSubCatagoryType.DataSource = any3;
                ddlSubCatagoryType.DataBind();
                ddlMembershipPaymentType.SelectedIndex = ddlMembershipPaymentType.Items.IndexOf(ddlMembershipPaymentType.Items.FindByText("N/A"));
                txtPaymentAmount.Text = "0";
                txtAdmissionFee.Text = "0";

            }
            else if (ddlMemberOption.SelectedValue == "8")
            {
                ddlCatagoryType.DataSource = Free_User;
                ddlCatagoryType.DataBind();
            }

            else if (ddlMemberOption.SelectedValue == "0")
            {
                ddlCatagoryType.DataSource = select;
                ddlCatagoryType.DataBind();
            }
        }
        protected void btnPriceCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                double discount = 0, finalAmount = 0, discountCampion = 0;
                double paymentAmount = Convert.ToInt32(txtPaymentAmount.Text);
                double admissionFee = Convert.ToInt32(txtAdmissionFee.Text);
                double dueClearAmount = Convert.ToInt32(txtDueClearAmount.Text);
                bool isRenew = false;
                int? da = 0;
                var paymentInfo = db.PaymentInfos.Where(b => b.memberId == txtMemberId.Text.ToString()).SingleOrDefault();
                var paymentHistory = db.Reports.Where(p => p.memberId == txtMemberId.Text.ToString()).ToList();
                var receiptNumberForm = txtStatic.Text + "-" + txtReceiptNo.Text;

                string discountCode = txtDiscountCode.Text;
                if (discountCode.Contains('%'))
                {
                    var item = (from c in db.Influencers
                                where c.influencerCode.Equals(discountCode) && c.status == true
                                select c).SingleOrDefault();
                    if (item != null)
                    {
                        discountCampion = Convert.ToDouble(item.influencerCode.Split('%')[1]) / 100;
                        txtDiscountReason.Text = "Offer " + "Discount Amount: " + (paymentAmount * discountCampion).ToString();
                    }
                }
                isRenew = receiptNumberForm == paymentInfo.receiptNo ? false : true;
                da = paymentInfo.dueAmount;
                discount = txtDiscount.Text == "" ? 0 : Convert.ToInt32(txtDiscount.Text);

                finalAmount =paymentHistory.Count>=1? paymentAmount - discount - paymentAmount * discountCampion: paymentAmount+ admissionFee - discount - paymentAmount * discountCampion;

                
                if (isRenew)
                {
                    txtFinalAmount.Text = finalAmount.ToString();
                    txtDueAmount.Text = (Convert.ToInt32(txtFinalAmount.Text) - Convert.ToInt32(txtpaidAmount.Text)-Convert.ToInt32(txtDueClearAmount.Text)).ToString();
                }
                else
                {
                    txtFinalAmount.Text = finalAmount.ToString(); /*paymentInfo.finalAmount.ToString();*/
                    txtDueAmount.Text = da==0?da.ToString(): (Convert.ToInt32(txtFinalAmount.Text) - Convert.ToInt32(txtpaidAmount.Text) - Convert.ToInt32(txtDueClearAmount.Text)).ToString();
                }
                
                btnEdit.Enabled = roleId == "1" || roleId == "4" ? false : true;
                lblInformation.Text = "";
            }
            catch (Exception ex)
            {
                _logger.Error("##" + "Edit Form-{0}", ex.Message);
                lblInformation.Text = ex.Message;
            }
            db.Dispose();
        }
        protected string validateField()
        {
            //var paymentInfoItem = db.PaymentInfos.Where(p => p.memberId == txtMemberId.Text).SingleOrDefault();
            //var previousReceiptNo = paymentInfoItem.receiptNo;
            if (ddlCatagoryType.SelectedIndex == 0)
            {
                return "Please select Membership Catagory";
            }
            //else if (uploadFileFlag == 0)
            //{
            //    return "Please uplaod Image";
            //}
            else if (string.IsNullOrEmpty(txtMembershipDate.Text))
            {
                return "Please Enter Membership Date";
            }
            else if (string.IsNullOrEmpty(txtMembershipBeginDate.Text))
            {
                return "Please enter Membership Begin Date";
            }
            else if (ddlRenewExtendNormal.SelectedIndex == 0)
            {
                return "Please select Reason for Update";
            }
            else if (string.IsNullOrEmpty(txtMembershipExpireDate.Text))
            {
                return "Please enter Membership Expire Date";
            }
            else if (string.IsNullOrEmpty(txtFirstName.Text))
            {
                return "Plese enter First Name";
            }
            else if (string.IsNullOrEmpty(txtLastName.Text))
            {
                return "Plese enter Last Name";
            }
            else if (txtContactNo.Text.Length != 10)
            {
                return "Field Contact No is empty or Invalid";
            }
            else if (string.IsNullOrEmpty(txtDateOfBirth.Text))
            {
                return "Please enter Date Of Birth";
            }
            else if (string.IsNullOrEmpty(txtAddress.Text))
            {
                return "Please enter Address";
            }
            else if (string.IsNullOrEmpty(txtEmergencyContactPerson.Text))
            {
                return "Please enter Emergency Contact Person";
            }
            else if (ddlGender.SelectedIndex == 0)
            {
                return "Please select Gender";
            }
            else if (txtEmergencyContactPhone.Text.Length != 10)
            {
                return "Emergency Contact No Field is Empty or Invalid";
            }
            else if (ddlGymAnytimeBefore.SelectedIndex == 0)
            {
                return "Please select Have you Been to Gym Before";
            }
            else if (string.IsNullOrEmpty(txtFinalAmount.Text))
            {
                return "Plese calculate the Fee";
            }
            else if (txtDiscount.Text.Length > 1 && string.IsNullOrWhiteSpace(txtDiscountReason.Text))
            {
                return "Please mention Reason for Discount";
            }
            else if (string.IsNullOrEmpty(txtReceiptNo.Text))
            {
                return "Receipt No is required";
            }
            else if (ddlPaymentMethod.SelectedIndex == 0)
            {
                return "Please Select Payment Method";
            }
            //else if ((txtStatic.Text + "-" + txtReceiptNo.Text) == (previousReceiptNo))
            //{
            //    return "Receipt No is same as of Previous, Please enter the new Receipt No";
            //}
            else
            {
                return "";
            }
        }
        protected string validateNotPayingMember()
        {
            if (string.IsNullOrEmpty(txtMembershipDate.Text))
            {
                return "Please Enter Membership Date";
            }
            else if (string.IsNullOrEmpty(ddlShift.SelectedItem.Text))
            {
                return "Please choose Shift";
            }
            else if (string.IsNullOrEmpty(txtEmail.Text))
            {
                return "Please Enter Email Address";
            }
            else if (string.IsNullOrEmpty(txtFirstName.Text))
            {
                return "Plese enter First Name";
            }
            else if (string.IsNullOrEmpty(txtLastName.Text))
            {
                return "Plese enter Last Name";
            }
            else if (txtContactNo.Text.Length != 10)
            {
                return "Field Contact No is empty or Invalid";
            }
            else if (string.IsNullOrEmpty(txtDateOfBirth.Text))
            {
                return "Please enter Date Of Birth";
            }
            else if (string.IsNullOrEmpty(txtAddress.Text))
            {
                return "Please enter Address";
            }
            else if (string.IsNullOrEmpty(txtEmergencyContactPerson.Text))
            {
                return "Please enter Emergency Contact Person";
            }
            else if (ddlGender.SelectedIndex == 0)
            {
                return "Please select Gender";
            }
            else if (txtEmergencyContactPhone.Text.Length != 10)
            {
                return "Emergency Contact No Field is Empty or Invalid";
            }
            else if (ddlGymAnytimeBefore.SelectedIndex == 0)
            {
                return "Please select Have you Been to Gym Before";
            }
            else if (string.IsNullOrEmpty(txtFinalAmount.Text))
            {
                return "Plese calculate the Fee";
            }
            else if (txtDiscount.Text.Length > 1 && string.IsNullOrWhiteSpace(txtDiscountReason.Text))
            {
                return "Please mention Reason for Discount";
            }
            else
            {
                return "";
            }
        }
        protected void loadData()
        {
            try
            {
                txtGenerateDate.Enabled = roleId == "3" ? false : true;
                btnGenerateDate.Enabled = roleId == "3" ? false : true;

                string id = Request.QueryString["ID"].ToString();
                string key = Request.QueryString["key"].ToString();
                mId = id;
                //string decryptId = DecryptString(id);
                var lockerItem = db.LockerMgs.Where(p => p.memberId == id).SingleOrDefault();
                if (lockerItem == null)
                {
                    lblLocker.Text = "No Locker";
                }
                else
                {
                    if (lockerItem.expireDate > DateTime.Now)
                    {
                        lblLocker.Attributes["class"] = "lockerExpired";
                        lblLocker.Text = "Yes -" + " " +  lockerItem.expireDate.ToString();
                    }
                    else
                    {
                        lblLocker.Attributes["class"] = "lockerNotExpired";
                        lblLocker.Text = "Yes -" + " " + lockerItem.expireDate.ToString();
                    }
                }
                lblLocker.Text = lockerItem == null ? "No" : "Yes -" + " " + lockerItem.expireDate.ToString();

                var editQuery = (from c in db.MemberInformations
                                 join p in db.PaymentInfos
                                 on c.memberId equals p.memberId
                                 join ml in db.MemberLogins
                                 on c.memberId equals ml.memberId
                                 join s in db.StartStops
                                 on c.memberId equals s.memberId
                                 where c.memberId == id
                                 select new
                                 {
                                     c.memberId,
                                     c.imageLoc,
                                     c.branch,
                                     c.ActiveInactive,
                                     c.memberOption,
                                     c.memberCatagory,
                                     c.memberSubCatagory,
                                     c.memberDate,
                                     c.memberBeginDate,
                                     c.admissionFee,
                                     c.memberPaymentType,
                                     c.memberExpireDate,
                                     c.shift,
                                     c.email,
                                     c.firstName,
                                     c.lastName,
                                     c.contactNo,
                                     c.dateOfBirth,
                                     c.address,
                                     c.emergencyContactPerson,
                                     c.gender,
                                     c.emergencyContactNo,
                                     c.haveBeenGymBefore,
                                     c.haveBeenGymBeforeText,
                                     c.anyhealthIssue,
                                     c.locker,
                                     c.lockerNo,
                                     c.universalMembershipLimit,
                                     c.remark,
                                     ml.username,
                                     ml.password,
                                     s.stopDate,
                                     s.startDate,
                                     s.stopDays,
                                     s.stopLimit,
                                     p.paymentAmount,
                                     p.discount,
                                     p.discountReason,
                                     p.disocuntCode,
                                     p.finalAmount,
                                     p.paidAmount,
                                     p.dueAmount,
                                     p.dueClearAmount,
                                     p.paymentMethod,
                                     p.bank,
                                     p.chequeNumber,
                                     p.referenceId,
                                     p.receiptNo,
                                     p.renewExtend
                                 }).SingleOrDefault();

                preMemberExpDate = Convert.ToDateTime(editQuery.memberExpireDate).ToShortDateString();
                lblPreExpireDate.Text =Convert.ToDateTime(preMemberExpDate).ToString("yyyy/MM/dd");
                txtMemberId.Text = editQuery.memberId.ToString();
                image1.ImageUrl = editQuery.imageLoc.ToString()=="Image/Members/" ? "Image/NoImage.jpg": editQuery.imageLoc.ToString();
                ddlActiveInactive.SelectedIndex = ddlActiveInactive.Items.IndexOf(ddlActiveInactive.Items.FindByText(editQuery.ActiveInactive));

                if (editQuery.branch != "")
                    txtBranch.Text = editQuery.branch;
                else
                    txtBranch.Text = Session["userDb"].ToString();
                txtuname.Text = editQuery.username;
                txtPwd.Attributes["value"] = editQuery.password;
                ddlMemberOption.SelectedIndex = ddlMemberOption.Items.IndexOf(ddlMemberOption.Items.FindByText(editQuery.memberOption));
                loadDropdownValue();
                ddlCatagoryType.SelectedIndex = ddlCatagoryType.Items.IndexOf(ddlCatagoryType.Items.FindByText(editQuery.memberCatagory));
                if (ddlCatagoryType.SelectedItem.Text == "Any1")
                {
                    ddlSubCatagoryType.DataSource = any1;
                    ddlSubCatagoryType.DataBind();
                }
                else if (ddlCatagoryType.SelectedItem.Text == "Any2")
                {
                    ddlSubCatagoryType.DataSource = any2;
                    ddlSubCatagoryType.DataBind();
                }
                else if (ddlCatagoryType.SelectedItem.Text == "Any3")
                {
                    ddlSubCatagoryType.DataSource = any3;
                    ddlSubCatagoryType.DataBind();
                }
                ddlSubCatagoryType.SelectedIndex = ddlSubCatagoryType.Items.IndexOf(ddlSubCatagoryType.Items.FindByText(editQuery.memberSubCatagory));

                var md = NepaliDateService.EngToNep(Convert.ToDateTime(editQuery.memberDate));
                txtMembershipDate.Text = md.ToString();

                var mbd = NepaliDateService.EngToNep(Convert.ToDateTime(editQuery.memberBeginDate));
                txtMembershipBeginDate.Text = mbd.ToString();

                var med= NepaliDateService.EngToNep(Convert.ToDateTime(editQuery.memberExpireDate));
                txtMembershipExpireDate.Text =med==null?"":med.ToString();

                lblPrePaymentType.Text = editQuery.memberPaymentType;
                ddlShift.SelectedIndex = ddlShift.Items.IndexOf(ddlShift.Items.FindByText(editQuery.shift));
                txtEmail.Text = editQuery.email;
                txtFirstName.Text = editQuery.firstName;
                txtLastName.Text = editQuery.lastName;
                txtContactNo.Text = editQuery.contactNo;
                var dob= NepaliDateService.EngToNep(Convert.ToDateTime(editQuery.dateOfBirth));
                txtDateOfBirth.Text = dob.ToString();
                txtAddress.Text = editQuery.address;
                txtEmergencyContactPerson.Text = editQuery.emergencyContactPerson;
                ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByText(editQuery.gender));
                txtEmergencyContactPhone.Text = editQuery.emergencyContactNo;
                ddlGymAnytimeBefore.SelectedIndex = ddlGymAnytimeBefore.Items.IndexOf(ddlGymAnytimeBefore.Items.FindByText(editQuery.haveBeenGymBefore));
                txtHowLong.Text = editQuery.haveBeenGymBeforeText;
                if (ddlGymAnytimeBefore.SelectedValue == "2") { txtHowLong.Visible = true; }
                txtAnyHealthIssue.Text = editQuery.anyhealthIssue;
                //chkIsRenewExtended.Checked = editQuery.renewExtend == "Renewed" ? true : false;
                txtPaymentAmount.Text = editQuery.paymentAmount.ToString();
                txtDiscount.Text = editQuery.discount.ToString();
                txtDiscountCode.Text = editQuery.disocuntCode;
                if(roleId=="1")
                    txtStatic.Text= editQuery.receiptNo.Split('-')[0];
                txtReceiptNo.Text = editQuery.receiptNo.Split('-').Length == 2 ? editQuery.receiptNo.Split('-')[1] : editQuery.receiptNo.Split('-')[0];
                txtRemark.Text = editQuery.remark;
                ddlPaymentMethod.SelectedIndex = ddlPaymentMethod.Items.IndexOf(ddlPaymentMethod.Items.FindByText(editQuery.paymentMethod));
                if (ddlPaymentMethod.SelectedItem.Text == "Cheque")
                {
                    pnlCheque.Visible = true;
                    txtBankName.Text = editQuery.bank;
                    txtChequeNumber.Text = editQuery.chequeNumber;
                }
                else if(ddlPaymentMethod.SelectedItem.Text == "E-Banking")
                {
                    pnlEBanking.Visible = true;
                    txtReferenceId.Text = editQuery.referenceId;
                }
                txtUnivershipMembershipLimit.Text = editQuery.universalMembershipLimit.ToString();
                txtFinalAmount.Text = editQuery.finalAmount.ToString();
                txtpaidAmount.Text = editQuery.paidAmount.ToString();
                txtDueAmount.Text = editQuery.dueAmount.ToString();
                txtDueClearAmount.Text = editQuery.dueClearAmount==null?"0": editQuery.dueClearAmount.ToString();

                if (editQuery.admissionFee != "")
                    txtAdmissionFee.Text = editQuery.admissionFee;
                else
                {
                    var query = (from ex in db.ExtraInformations
                                 where ex.extraInformationId == 1
                                 select ex.admission).SingleOrDefault();
                    txtAdmissionFee.Text = query.ToString();
                }
                if (editQuery.discountReason != "")
                {
                    txtDiscountReason.Visible = true;
                    txtDiscountReason.Text = editQuery.discountReason;
                }

                lblStop.Text =editQuery.stopDate.HasValue ? NepaliDateService.EngToNep(Convert.ToDateTime(editQuery.stopDate)).ToString():"";
                lblStart.Text = editQuery.startDate.HasValue? NepaliDateService.EngToNep(Convert.ToDateTime(editQuery.startDate)).ToString():"";
                lblStopDays.Text = editQuery.stopDays.HasValue? editQuery.stopDays.ToString():"";
                lblStopLimit.Text = editQuery.stopLimit.HasValue ? editQuery.stopLimit.ToString():"" ;

                if (string.IsNullOrEmpty(lblStop.Text) && string.IsNullOrEmpty(lblStart.Text))
                {
                    btnStop.Enabled = true;
                    btnStart.Enabled = false;
                }
                if (!string.IsNullOrEmpty(lblStop.Text) && string.IsNullOrEmpty(lblStart.Text))
                {
                    btnStop.Enabled = false;
                    btnStart.Enabled = true;
                }
                if ((!string.IsNullOrEmpty(lblStop.Text) && !string.IsNullOrEmpty(lblStart.Text)) && lblStopLimit.Text != "0")
                {
                    btnStop.Enabled = true;
                    btnStart.Enabled = false;
                }

                if (editQuery.stopLimit ==0)
                {
                    btnStop.Enabled = false;
                    btnStart.Enabled = false;
                }

                IQueryable<Report> reportItems = db.Reports;
                reportItems = reportItems.Where(c => c.memberId == id);
                gridReport.DataSource = reportItems.ToList();
                gridReport.DataBind();

                IQueryable<BodyMeasurement> items = db.BodyMeasurements;
                items = items.Where(c => c.memberId == id);
                int count = items.Count();
                if (count > 0)
                {
                    gridBodyMesurement.DataSource = items.ToList();
                    gridBodyMesurement.DataBind();

                    DataTable dt1 = new DataTable();
                    dt1.Columns.Add(new DataColumn("Column1", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Column2", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Column3", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Column4", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Column5", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Column6", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Column7", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Column8", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Column9", typeof(string)));

                    for (int i = 0; i < gridBodyMesurement.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)gridBodyMesurement.Rows[i].FindControl("txtDate");
                        TextBox box2 = (TextBox)gridBodyMesurement.Rows[i].FindControl("txtWeight");
                        TextBox box3 = (TextBox)gridBodyMesurement.Rows[i].FindControl("txtHeight");
                        TextBox box4 = (TextBox)gridBodyMesurement.Rows[i].FindControl("txtUpperArm");
                        TextBox box5 = (TextBox)gridBodyMesurement.Rows[i].FindControl("txtForeArm");
                        TextBox box6 = (TextBox)gridBodyMesurement.Rows[i].FindControl("txtChest");
                        TextBox box7 = (TextBox)gridBodyMesurement.Rows[i].FindControl("txtWaist");
                        TextBox box8 = (TextBox)gridBodyMesurement.Rows[i].FindControl("txtThighs");
                        TextBox box9 = (TextBox)gridBodyMesurement.Rows[i].FindControl("txtCalf");


                        DataRow dr1 = null;
                        dr1 = dt1.NewRow();
                        //dr1["RowNumber"] = 1;
                        dr1["Column1"] = box1.Text;
                        dr1["Column2"] = box2.Text;
                        dr1["Column3"] = box3.Text;
                        dr1["Column4"] = box4.Text;
                        dr1["Column5"] = box5.Text;
                        dr1["Column6"] = box6.Text;
                        dr1["Column7"] = box7.Text;
                        dr1["Column8"] = box8.Text;
                        dr1["Column9"] = box9.Text;

                        dt1.Rows.Add(dr1);


                    }
                    ViewState["CurrentTable1"] = dt1;
                }
                else
                {
                    SetInitialRowBodyMesurement();
                }

                if (key == "view")
                {
                    qrshow = true;
                    pnlEditForm.Enabled = false;
                }

                //if (roleId == 1 || roleId==2)
                //{
                //    if (key == "view")
                //        pnlEditForm.Enabled = false;
                //    if (key == "edit")
                //        pnlEditForm.Enabled = true;
                //}
                ////else if (key == "view")
                //else
                //    pnlEditForm.Enabled = false;
            }
            catch (Exception ex)
            {
                _logger.Error("Edit Form-{0}", ex.Message);
                lblInformation.Text = ex.ToString();
            }
            db.Dispose();
        }
        protected bool isRenewExpireDateChanged(MemberInformation m)
        {
            DateTime renewDateDB = Convert.ToDateTime(m.memberBeginDate);
            DateTime expireDateDB = Convert.ToDateTime(m.memberExpireDate);

            DateTime renewDateTxtField = Convert.ToDateTime(NepaliDateService.NepToEng(txtMembershipBeginDate.Text));
            DateTime expireDateTxtField = Convert.ToDateTime(NepaliDateService.NepToEng(txtMembershipExpireDate.Text));

            int result1 = DateTime.Compare(renewDateDB, renewDateTxtField);
            int result2 = DateTime.Compare(expireDateDB, expireDateTxtField);

            bool status= (result1 == 0 && result2 == 0) ? false : true;
            //if both result is 0, i.e. renew and expiry date is not modified
            return status;
        }

        protected bool isReceiptNumberChanged(PaymentInfo p)
        {
            bool status = p.receiptNo == (txtStatic.Text + "-" + txtReceiptNo.Text) ? false : true;
            return status;
        }
        public void update()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                btnEdit.Enabled = false;
                String tempId = txtMemberId.Text;
                try
                {
                    MemberInformation m1 = (from c in db.MemberInformations
                                            where c.memberId == tempId
                                            select c).SingleOrDefault();
                    PaymentInfo p1 = (from p in db.PaymentInfos
                                      where p.memberId == tempId
                                      select p).SingleOrDefault();
                    StartStop s1 = (from s in db.StartStops
                                    where s.memberId == tempId
                                    select s).SingleOrDefault();

                    Report r1 = new Report();

                    ExtraInformation ex = (from c in db.ExtraInformations
                                           where c.extraInformationId == 1
                                           select c).SingleOrDefault();

                    DateTime? dt = null;
                    Int32? num = null;

                    //var stat = isRenewExpireDateChanged(m1);
                    var stat1 = isReceiptNumberChanged(p1);

                    #region Update MemberInformation
                    try
                    {
                        if (m1.imageLoc == "Image/Members/")
                        {
                            m1.imageLoc = "Image/Members/" + hidImage.Value.ToString();
                        }
                    }
                    catch (Exception)
                    {

                    }
                    m1.branch = txtBranch.Text;
                    m1.ActiveInactive = ddlActiveInactive.SelectedItem.Text;
                    m1.username = txtuname.Text;
                    m1.password = txtPwd.Text;
                    m1.memberOption = ddlMemberOption.SelectedItem.Text;
                    m1.memberCatagory = ddlCatagoryType.SelectedItem.Text;
                    m1.memberSubCatagory = ddlSubCatagoryType.SelectedItem.Text;
                    var md = NepaliDateService.NepToEng(txtMembershipDate.Text);
                    m1.memberDate = Convert.ToDateTime(md);

                    //if (ddlMemberOption.SelectedValue == "1" || ddlMemberOption.SelectedValue == "2" || ddlMemberOption.SelectedValue == "3" || ddlMemberOption.SelectedValue == "4")
                    //{
                    var mbd = NepaliDateService.NepToEng(txtMembershipBeginDate.Text);
                    m1.memberBeginDate = Convert.ToDateTime(mbd);
                    var med = NepaliDateService.NepToEng(txtMembershipExpireDate.Text);
                    m1.memberExpireDate = Convert.ToDateTime(med);
                    //}

                    if (ddlMembershipPaymentType.SelectedItem.Text != "Select")
                    {
                        m1.memberPaymentType = ddlMembershipPaymentType.SelectedItem.Text;
                    }
                    m1.shift = ddlShift.SelectedItem.Text;
                    m1.email = txtEmail.Text;
                    //m1.emailStatus = Convert.ToBoolean(0);
                    m1.firstName = txtFirstName.Text;
                    m1.lastName = txtLastName.Text;
                    m1.fullname = txtFirstName.Text + " " + txtLastName.Text;
                    m1.contactNo = txtContactNo.Text;
                    var dob = NepaliDateService.NepToEng(txtDateOfBirth.Text);
                    m1.dateOfBirth = Convert.ToDateTime(dob);
                    m1.address = txtAddress.Text;
                    m1.emergencyContactPerson = txtEmergencyContactPerson.Text;
                    m1.gender = ddlGender.SelectedItem.Text;
                    m1.emergencyContactNo = txtEmergencyContactPhone.Text;
                    m1.haveBeenGymBefore = ddlGymAnytimeBefore.SelectedItem.Text;
                    m1.haveBeenGymBeforeText = txtHowLong.Text;
                    m1.anyhealthIssue = txtAnyHealthIssue.Text;
                    m1.remark = txtRemark.Text;
                    //m1.locker = chkLocker.Checked;
                    //m1.lockerNo = txtLockerNumber.Text;
                    m1.modifiedDate = DateTime.Now;
                    m1.modifiedBy = txtBranch.Text;
                    #endregion
                    #region Insert Body Measurement
                    if (deleteBodyMeasurement() == true)
                    {
                        foreach (GridViewRow gr in gridBodyMesurement.Rows)
                        {
                            BodyMeasurement disab = new BodyMeasurement();
                            disab.memberId = txtMemberId.Text;
                            var mdd = ((TextBox)gr.FindControl("txtDate")).Text;
                            disab.measurementDate = mdd == "" ? "" : Convert.ToString(NepaliDateService.NepToEng(mdd));
                            disab.weight = ((TextBox)gr.FindControl("txtWeight")).Text;
                            disab.height = ((TextBox)gr.FindControl("txtHeight")).Text;
                            disab.upperArm = ((TextBox)gr.FindControl("txtUpperArm")).Text;
                            disab.foreArm = ((TextBox)gr.FindControl("txtForeArm")).Text;
                            disab.chest = ((TextBox)gr.FindControl("txtChest")).Text;
                            disab.waist = ((TextBox)gr.FindControl("txtWaist")).Text;
                            disab.thigh = ((TextBox)gr.FindControl("txtThighs")).Text;
                            disab.calf = ((TextBox)gr.FindControl("txtCalf")).Text;
                            db.BodyMeasurements.InsertOnSubmit(disab);
                        }
                    }
                    #endregion
                    #region Update payment Info
                    p1.paymentAmount = Convert.ToInt32(txtPaymentAmount.Text);
                    if (txtDiscount.Text == "" || txtDiscount.Text == "0")
                    {
                        p1.discount = 0;
                        p1.discountReason = "";
                    }
                    else
                    {
                        p1.discount = Convert.ToInt32(txtDiscount.Text);
                        p1.discountReason = txtDiscountReason.Text;
                    }
                    p1.disocuntCode = txtDiscountCode.Text;
                    p1.finalAmount = Convert.ToInt32(txtFinalAmount.Text);
                    p1.paidAmount = Convert.ToInt32(txtpaidAmount.Text);
                    p1.dueAmount = Convert.ToInt32(txtDueAmount.Text);
                    p1.dueClearAmount = Convert.ToInt32(txtDueClearAmount.Text);

                    p1.paymentMethod = ddlPaymentMethod.SelectedItem.Text;
                    if (ddlPaymentMethod.SelectedItem.Text == "Cheque")
                    {
                        p1.bank = txtBankName.Text;
                        p1.chequeNumber = txtChequeNumber.Text;
                    }
                    else if (ddlPaymentMethod.SelectedItem.Text == "E-Banking")
                    {
                        p1.referenceId = txtReferenceId.Text;
                    }
                    p1.receiptNo = txtStatic.Text + "-" + txtReceiptNo.Text;
                    p1.updatedDate = DateTime.Now;
                    p1.due = false;
                    p1.renewExtend = "normalChanges";
                    #endregion
                    #region Startstop
                    s1.stopDate = dt;
                    s1.startDate = dt;
                    s1.stopDays = num;
                    if (ddlMembershipPaymentType.SelectedValue == "1")
                        s1.stopLimit = ex.oneMonth;
                    if (ddlMembershipPaymentType.SelectedValue == "3")
                        s1.stopLimit = ex.threeMonth;
                    else if (ddlMembershipPaymentType.SelectedValue == "6")
                        s1.stopLimit = ex.sixMonth;
                    else if (ddlMembershipPaymentType.SelectedValue == "12")
                        s1.stopLimit = ex.twelveMonth;
                    s1.memberId = txtMemberId.Text;
                    s1.stopDate = string.IsNullOrEmpty(lblStop.Text) ? dt : NepaliDateService.NepToEng(lblStop.Text);
                    s1.startDate = string.IsNullOrEmpty(lblStart.Text) ? dt : NepaliDateService.NepToEng(lblStart.Text);
                    s1.stopDays = string.IsNullOrEmpty(lblStopDays.Text) ? num : Convert.ToInt32(lblStopDays.Text);
                    s1.stopLimit = string.IsNullOrEmpty(lblStopLimit.Text) ? num : Convert.ToInt32(lblStopLimit.Text);
                    #endregion
                    //renew
                    if (ddlRenewExtendNormal.SelectedItem.Text== "Renew")
                    {
                        #region Report History
                        r1.memberId = txtMemberId.Text;
                        r1.memberBeginDate = Convert.ToDateTime(NepaliDateService.NepToEng(txtMembershipBeginDate.Text));
                        r1.memberExpireDate = Convert.ToDateTime(NepaliDateService.NepToEng(txtMembershipExpireDate.Text));
                        r1.memberOption = ddlMemberOption.SelectedItem.Text;
                        r1.memberCatagory = ddlCatagoryType.SelectedItem.Text;
                        r1.memberPaymentType = ddlMembershipPaymentType.SelectedItem.Text;
                        r1.discount = txtDiscount.Text == "" ? 0 : Convert.ToInt32(txtDiscount.Text);
                        r1.discountReason = txtDiscountReason.Text;
                        r1.discountCode = txtDiscountCode.Text;
                        r1.finalAmount = Convert.ToInt32(txtFinalAmount.Text);
                        r1.paidAmount = Convert.ToInt32(txtpaidAmount.Text);
                        r1.dueAmount = Convert.ToInt32(txtDueAmount.Text);
                        r1.dueClearAmount = Convert.ToInt32(txtDueClearAmount.Text);
                        r1.receiptNo = txtStatic.Text + "-" + txtReceiptNo.Text;
                        r1.paymentMethod = ddlPaymentMethod.SelectedItem.Text;
                        r1.created = DateTime.Now;
                        r1.renewExtend = "renewed";
                        db.Reports.InsertOnSubmit(r1);
                        #endregion
                        #region Member Information
                        m1.ActiveInactive = "Active";
                        m1.emailStatus = false;
                        m1.universalMembershipLimit = ddlMemberOption.SelectedValue == "1" ? 12 : ddlMemberOption.SelectedValue == "3" ? 365 : ddlMemberOption.SelectedValue == "2" ? 12 : ddlMemberOption.SelectedValue == "5" ? 365 : ddlMemberOption.SelectedValue == "6" ? 365 : ddlMemberOption.SelectedValue == "7" ? 365 : ddlMemberOption.SelectedValue == "8" ? 0 : 0;
                        #endregion
                        #region Send Email if Renewed
                        //new Task(() => {
                        //    sendEmail(txtMemberId.Text, txtuname.Text, txtBranch.Text, txtPwd.Text, ddlMemberOption.SelectedItem.Text, ddlCatagoryType.SelectedItem.Text, txtMembershipDate.Text, ddlMembershipPaymentType.SelectedItem.Text, txtMembershipBeginDate.Text, txtMembershipExpireDate.Text, txtEmail.Text, txtFirstName.Text + " " + txtLastName.Text, txtContactNo.Text, txtDateOfBirth.Text, txtAddress.Text, txtDiscountCode.Text, txtFinalAmount.Text, txtpaidAmount.Text, txtDueAmount.Text);
                        //}).Start();
                        #endregion
                        #region Payment Info
                        p1.updatedDate = DateTime.Now;
                        p1.renewExtend = "renewed";
                        #endregion
                    }
                    else if(ddlRenewExtendNormal.SelectedItem.Text == "Extend")
                    {
                        p1.updatedDate = DateTime.Now;
                        p1.renewExtend = "extended";
                        m1.ActiveInactive = "Active";
                       
                    }
                    
                    db.SubmitChanges();
                    lblInformation.Visible = true;
                    string key = Request.QueryString["key"].ToString();

                    lblInformation.Text = "sucessfully updated the record";
                    lblInformation.ForeColor = ColorTranslator.FromHtml("#037203");
                    Session["UseIsAuthenticated"] = "true";
                    if (key == "edit")
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('Dashboard.aspx') }, 1000);", true);
                    else if (key == "editP")
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('PaymentPending.aspx') }, 200);", true);
                }
                catch (Exception exx)
                {
                    lblInformation.Text = exx.Message;
                    _logger.Error("##" + "Edit Form-", exx.Message);
                }

            }
        }
        protected void sendEmail(string memberid, string username, string branch, string password, string membershipOption, string catagoryType, string membershipDate, string membershipPaymentType, string membershipBeginDate, string membershipExpireDate, string email, string fullname, string contactNo, string dateOfBirth, string address, string discountCode, string finalAmount, string paidAmount, string dueAmount)
        {
            try
            {
                var extraInfo = db.ExtraInformations;
                var emailInfo = (from c in extraInfo
                                 where c.extraInformationId == 1
                                 select c).SingleOrDefault();
                string txtEmail = emailInfo.email;
                string pwd = emailInfo.password;
                string txtSubject = "Membership Renewal";
                string txtBody = "Dear " + fullname + "," + Environment.NewLine + Environment.NewLine +
                    "Your GYM membership has been renewed as per following: " + Environment.NewLine +
                    "Membership Option: " + membershipOption + Environment.NewLine +
                    "Membership Catagory: " + catagoryType + Environment.NewLine +
                    "Membership Payment Type: " + membershipPaymentType + Environment.NewLine +
                    "Membership Renew Date: " + membershipBeginDate + Environment.NewLine +
                    "Membership Expire Date: " + membershipExpireDate + Environment.NewLine +
                    "Final Amount:" + " " + finalAmount + Environment.NewLine +
                    "Paid Amount:" + " " + paidAmount + Environment.NewLine +
                    "Due Amount:" + " " + dueAmount + Environment.NewLine + Environment.NewLine +

                   "Thank you." + Environment.NewLine + Environment.NewLine +
                    "Regards," + Environment.NewLine +
                    "The Physique Workshop";
                using (MailMessage mm = new MailMessage(txtEmail, email))
                {
                    mm.Subject = txtSubject;
                    mm.Body = txtBody;
                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(txtEmail, pwd);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Email sent.');", true);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("##" + "Edit Form-{0}", ex.Message);
            }
            db.Dispose();
        }
        protected void timerId_Tick(object sender, EventArgs e)
        {

        }
        protected bool deleteBodyMeasurement()
        {
            try
            {
                string tempId = txtMemberId.Text;
                IQueryable<BodyMeasurement> items = from p in db.BodyMeasurements.Where(c => c.memberId == tempId)
                                                    select p;
                foreach (BodyMeasurement item in items)
                {
                    db.BodyMeasurements.DeleteOnSubmit(item);
                }
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("##" + "Edit Form-{0}", ex.Message);
                return false;
            }

        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string validationError = validateField();
            if (!string.IsNullOrEmpty(validationError))
            {
                lblPopupError.Text = validationError;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorModal", "$('#errorModal').modal();", true);
                return;
            }
            else
            {
                update();
            }
        }
        private void SetInitialRowBodyMesurement()
        {
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            //dt1.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt1.Columns.Add(new DataColumn("Column1", typeof(string)));
            dt1.Columns.Add(new DataColumn("Column2", typeof(string)));
            dt1.Columns.Add(new DataColumn("Column3", typeof(string)));
            dt1.Columns.Add(new DataColumn("Column4", typeof(string)));
            dt1.Columns.Add(new DataColumn("Column5", typeof(string)));
            dt1.Columns.Add(new DataColumn("Column6", typeof(string)));
            dt1.Columns.Add(new DataColumn("Column7", typeof(string)));
            dt1.Columns.Add(new DataColumn("Column8", typeof(string)));
            dt1.Columns.Add(new DataColumn("Column9", typeof(string)));

            dr1 = dt1.NewRow();
            //dr1["RowNumber"] = 1;
            dr1["Column1"] = string.Empty;
            dr1["Column2"] = string.Empty;
            dr1["Column3"] = string.Empty;
            dr1["Column4"] = string.Empty;
            dr1["Column5"] = string.Empty;
            dr1["Column6"] = string.Empty;
            dr1["Column7"] = string.Empty;
            dr1["Column8"] = string.Empty;
            dr1["Column9"] = string.Empty;

            dt1.Rows.Add(dr1);

            //Store the DataTable in ViewState
            ViewState["CurrentTable1"] = dt1;

            gridBodyMesurement.DataSource = dt1;
            gridBodyMesurement.DataBind();
        }
        private void AddNewRowToGridBodyMesurement()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable1"] != null)
            {
                DataTable dt1CurrentTable1 = (DataTable)ViewState["CurrentTable1"];
                DataRow dr1CurrentRow = null;
                if (dt1CurrentTable1.Rows.Count > 0)
                {
                    for (int i = 1; i <= dt1CurrentTable1.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[1].FindControl("txtDate");
                        TextBox box2 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[2].FindControl("txtWeight");
                        TextBox box3 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[3].FindControl("txtHeight");
                        TextBox box4 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[4].FindControl("txtUpperArm");
                        TextBox box5 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[5].FindControl("txtForeArm");
                        TextBox box6 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[6].FindControl("txtChest");
                        TextBox box7 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[7].FindControl("txtWaist");
                        TextBox box8 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[8].FindControl("txtThighs");
                        TextBox box9 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[9].FindControl("txtCalf");



                        dr1CurrentRow = dt1CurrentTable1.NewRow();
                        //dr1CurrentRow["RowNumber"] = i + 1;

                        dt1CurrentTable1.Rows[i - 1]["Column1"] = box1.Text;
                        dt1CurrentTable1.Rows[i - 1]["Column2"] = box2.Text;
                        dt1CurrentTable1.Rows[i - 1]["Column3"] = box3.Text;
                        dt1CurrentTable1.Rows[i - 1]["Column4"] = box4.Text;
                        dt1CurrentTable1.Rows[i - 1]["Column5"] = box5.Text;
                        dt1CurrentTable1.Rows[i - 1]["Column6"] = box6.Text;
                        dt1CurrentTable1.Rows[i - 1]["Column7"] = box7.Text;
                        dt1CurrentTable1.Rows[i - 1]["Column8"] = box8.Text;
                        dt1CurrentTable1.Rows[i - 1]["Column9"] = box9.Text;

                        rowIndex++;
                    }
                    dt1CurrentTable1.Rows.Add(dr1CurrentRow);
                    ViewState["CurrentTable1"] = dt1CurrentTable1;

                    gridBodyMesurement.DataSource = dt1CurrentTable1;
                    gridBodyMesurement.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetPreviousDataBodyMesurement();
        }
        private void SetPreviousDataBodyMesurement()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable1"] != null)
            {
                DataTable dt1 = (DataTable)ViewState["CurrentTable1"];
                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[1].FindControl("txtDate");
                        TextBox box2 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[2].FindControl("txtWeight");
                        TextBox box3 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[3].FindControl("txtHeight");
                        TextBox box4 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[4].FindControl("txtUpperArm");
                        TextBox box5 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[5].FindControl("txtForeArm");
                        TextBox box6 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[6].FindControl("txtChest");
                        TextBox box7 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[7].FindControl("txtWaist");
                        TextBox box8 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[8].FindControl("txtThighs");
                        TextBox box9 = (TextBox)gridBodyMesurement.Rows[rowIndex].Cells[9].FindControl("txtCalf");

                        box1.Text = dt1.Rows[i]["Column1"].ToString();
                        box2.Text = dt1.Rows[i]["Column2"].ToString();
                        box3.Text = dt1.Rows[i]["Column3"].ToString();
                        box4.Text = dt1.Rows[i]["Column4"].ToString();
                        box5.Text = dt1.Rows[i]["Column5"].ToString();
                        box6.Text = dt1.Rows[i]["Column6"].ToString();
                        box7.Text = dt1.Rows[i]["Column7"].ToString();
                        box8.Text = dt1.Rows[i]["Column8"].ToString();
                        box9.Text = dt1.Rows[i]["Column9"].ToString();

                        rowIndex++;
                    }
                }
            }
        }
        protected void btnAddMore_Click(object sender, EventArgs e)
        {
            state = "expand";
            AddNewRowToGridBodyMesurement();
        }
        protected void txtMembershipBeginDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(txtMembershipBeginDate.Text);
                int i = Convert.ToInt32(ddlMembershipPaymentType.SelectedValue);
                DateTime dt2 = (dt.AddMonths(i));
                txtMembershipExpireDate.Text = DateTime.Parse(dt2.ToString()).ToString("yyyy/MM/dd");
                updatePrice();
            }
            catch (Exception)
            {

            }

        }
        protected void gridBodyMesurement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem.ToString() == "TPW_GMS.Data.BodyMeasurement")
            {
                BodyMeasurement items = (BodyMeasurement)e.Row.DataItem;

                if (items.measurementDate != "")
                {
                    var dt = NepaliDateService.EngToNep(Convert.ToDateTime(items.measurementDate));
                    ((TextBox)e.Row.FindControl("txtDate")).Text = dt.ToString();
                }
                TextBox txtWeight = (TextBox)e.Row.FindControl("txtWeight");
                txtWeight.Text = items.weight;
                TextBox txtHeight = (TextBox)e.Row.FindControl("txtHeight");
                txtHeight.Text = items.height;
                TextBox txtUpperArm = (TextBox)e.Row.FindControl("txtUpperArm");
                txtUpperArm.Text = items.upperArm;
                TextBox txtForeArm = (TextBox)e.Row.FindControl("txtForeArm");
                txtForeArm.Text = items.foreArm;

                TextBox txtChest = (TextBox)e.Row.FindControl("txtChest");
                txtChest.Text = items.chest;
                TextBox txtWaist = (TextBox)e.Row.FindControl("txtWaist");
                txtWaist.Text = items.waist;

                TextBox txtThighs = (TextBox)e.Row.FindControl("txtThighs");
                txtThighs.Text = items.thigh;
                TextBox txtCalf = (TextBox)e.Row.FindControl("txtCalf");
                txtCalf.Text = items.calf;

            }
        }
        protected void gridReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem.ToString() == "TPW_GMS.Data.Report")
                {
                    Report items = (Report)e.Row.DataItem;
                    var rd = NepaliDateService.EngToNep(Convert.ToDateTime(items.memberBeginDate));
                    ((Label)e.Row.FindControl("lblRenewDate")).Text = rd.ToString();
                    var ed = NepaliDateService.EngToNep(Convert.ToDateTime(items.memberExpireDate));
                    ((Label)e.Row.FindControl("lblExpiredDate")).Text = ed.ToString();
                    var created = NepaliDateService.EngToNep(Convert.ToDateTime(items.created));
                    ((Label)e.Row.FindControl("lblCreated")).Text = created.ToString();
                }
            }
            catch (Exception)
            {

            }
            
        }
        protected void gridBodyMesurement_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //SetRowDataLandAndBuilding();
            if (ViewState["CurrentTable1"] != null)
            {
                state = "expand";
                DataTable dt1 = (DataTable)ViewState["CurrentTable1"];
                DataRow dr1CurrentRow = null;
                int rowIndex = Convert.ToInt32(e.RowIndex);
                if (dt1.Rows.Count > 1)
                {
                    dt1.Rows.Remove(dt1.Rows[rowIndex]);
                    dr1CurrentRow = dt1.NewRow();
                    ViewState["CurrentTable1"] = dt1;
                    gridBodyMesurement.DataSource = dt1;
                    gridBodyMesurement.DataBind();

                    for (int i = 0; i < gridBodyMesurement.Rows.Count - 1; i++)
                    {
                        gridBodyMesurement.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                    }
                    SetPreviousDataBodyMesurement();
                }
            }
        }
        protected void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            if (txtDiscount.Text == "0" || txtDiscount.Text == "")
                txtDiscountReason.Visible = false;
            else
                txtDiscountReason.Visible = true;
        }
        //protected void btnConform_Click(object sender, EventArgs e)
        //{
        //    string login = Session["userDb"].ToString();
        //    txtDiscountReason.Text = "<disab>" + "Discount Reason:&nbsp;&nbsp;" + "</disab>" + "<disab>" + login + "</disab>" + "(" + txtConform.Text + ")";
        //}
        protected void btnStart_Click(object sender, EventArgs e)
        {
            btnclickStatus = 1;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "conformStopStart", "$('#conformStopStart').modal();", true);
        }
        protected void btnStop_Click(object sender, EventArgs e)
        {
            btnclickStatus = 2;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "conformStopStart", "$('#conformStopStart').modal();", true);
        }
        protected void btnStartStopModal_Click(object sender, EventArgs e)
        {
            if (btnclickStatus == 2)
            {
                lblStop.Text = txtChangeInStartStopDate.Text == "" ?NepaliDateService.EngToNep(DateTime.Now).ToString() : txtChangeInStartStopDate.Text;
                lblStart.Text = "";
                lblStopDays.Text = "";
                ddlActiveInactive.SelectedIndex = ddlActiveInactive.Items.IndexOf(ddlActiveInactive.Items.FindByText("InActive"));
            }
            if (btnclickStatus == 1)
            {
                int limit = Convert.ToInt32(lblStopLimit.Text);
                limit = limit - 1;
                var startDate = txtChangeInStartStopDate.Text == "" ? DateTime.Now.ToShortDateString() : NepaliDateService.NepToEng(txtChangeInStartStopDate.Text).ToShortDateString();

                var d1 = Convert.ToDateTime(startDate);
                var d2 = NepaliDateService.NepToEng(lblStop.Text);
                //double stopdays = (Convert.ToDateTime(lblStart.Text) - Convert.ToDateTime(lblStop.Text)).TotalDays;
                double stopdays = (d1-d2).TotalDays;
                if (stopdays > 14)
                {
                    lblStart.Text = NepaliDateService.EngToNep(Convert.ToDateTime(startDate)).ToString();
                    lblStopDays.Text = stopdays.ToString();
                    lblStopLimit.Text = limit.ToString();
                    DateTime dt11 = NepaliDateService.NepToEng(txtMembershipExpireDate.Text);
                    DateTime dt22 = (dt11.AddDays(Convert.ToInt32(stopdays)));
                    txtMembershipExpireDate.Text = NepaliDateService.EngToNep(dt22).ToString();
                    ddlActiveInactive.SelectedIndex = ddlActiveInactive.Items.IndexOf(ddlActiveInactive.Items.FindByText("Active"));
                }
                else
                {
                    btnReset.Visible = true;
                    lblStrtStopInfo.ForeColor = ColorTranslator.FromHtml("red");
                    lblStrtStopInfo.Text = "Only after 14 days of Stopping You can Re Start. ";
                }
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddlActiveInactive.SelectedIndex = ddlActiveInactive.Items.IndexOf(ddlActiveInactive.Items.FindByText("Active"));
            lblStart.Text = NepaliDateService.EngToNep(DateTime.Now).ToString();
            lblStopDays.Text = "0";
        }
        protected void ddlPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPaymentMethod.SelectedItem.Text == "Cheque")
            {
                pnlCheque.Visible = true;
                pnlEBanking.Visible = false;
                txtReferenceId.Text = "";
            }
            else if (ddlPaymentMethod.SelectedItem.Text == "E-Banking")
            {
                pnlEBanking.Visible = true;
                pnlCheque.Visible = false;
                txtBankName.Text = "";
                txtChequeNumber.Text = "";
            }
            else
            {
                pnlCheque.Visible = false;
                pnlEBanking.Visible = false;
                txtReferenceId.Text = "";
                txtBankName.Text = "";
                txtChequeNumber.Text = "";
            }
        }
    }
}