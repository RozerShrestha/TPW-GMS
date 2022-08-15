using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using TPW_GMS.Data;
using TPW_GMS.Models;
using System.Drawing.Drawing2D;
using QRCoder;
using System.Net.Mime;
using TPW_GMS.Services;
using Newtonsoft.Json;

namespace TPW_GMS
{
    public partial class NewForm : System.Web.UI.Page
    {
        //private TPWDataContext db = new TPWDataContext();
        public static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        static string a1, a3, a6, a12, z1, z3, z6, z12, gy1, gy3, gy6, gy12, c1, c3, c6, c12, d1, d3, d6, d12, s1, s3, s6, s12, g1, g3, g6, g12, p1, p3, p6, p12;
        public string state = "collapse";
        string roleId, loginUser, splitUser;
        public string nepDate;
        string[] regular = new string[] { "--Select--", "Any1", "Any2", "Any3" };
        string[] offhour = new string[] { "--Select--", "Any1", "Any2" };
        string[] universal = new string[] { "--Select--", "Any1", "Any2" };
        //string[] Trainer = new string[] { "Any3" };
        //string[] Gym_Admin = new string[] { "Any3" };
        //string[] Super_Admin = new string[] { "Any3" };
        //string[] Intern = new string[] { "Any3" };
        string[] Free_User = new string[] { "N/A" };
        string[] Staff = new string[] { "Any3" };

        string[] select = new string[] { "" };

        string[] any1 = new string[] { "Gym", "Zumba", "Cardio" };
        string[] any2 = new string[] { "Gym Zumba", "Gym Cardio", "Zumba Cardio" };
        string[] any3 = new string[] { "Gym Zumba Cardio" };

        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            Page.MaintainScrollPositionOnPostBack = true;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "DropdownColor1", "activeInactiveBGChange()", true);
            if (!IsPostBack)
            {
                LoadFees();
                loadAdmissionFee();
                SetInitialRowBodyMesurement();
                loadInfo();
                LoadActiontaker();
                try
                {
                    if (loginUser.Contains("admin"))
                    {
                        pnlNewform.Enabled = false;
                        btnSubmit.Visible = false;
                    }
                    else
                    {
                        pnlNewform.Enabled = true;
                        btnSubmit.Visible = true;
                        txtBranch.Text = loginUser;
                    }

                }
                catch (Exception)
                {

                }

                Page.MaintainScrollPositionOnPostBack = true;
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
        private void LoadActiontaker()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                if (loginUser.Contains("admin"))
                {
                    var actionTakers = (from ex in db.Staffs
                                        where ex.associateBranch == loginUser && ex.staffCatagory == "Operation Manager"
                                        select ex.staffName).ToList();
                    ddlActionTaker.DataSource = actionTakers;
                    ddlActionTaker.DataBind();
                    ddlActionTaker.Items.Insert(0, new ListItem("--Select--", "0"));
                }
                else
                {
                    var actionTakers = (from ex in db.Staffs
                                        where ex.associateBranch == loginUser && ex.staffCatagory == "Gym Admin"
                                        select ex.staffName).ToList();
                    ddlActionTaker.DataSource = actionTakers;
                    ddlActionTaker.DataBind();
                    ddlActionTaker.Items.Insert(0, new ListItem("--Select--", "0"));
                }


            }
        }
        protected void loadAdmissionFee()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var query = (from ex in db.ExtraInformations
                             where ex.extraInformationId == 1
                             select ex.admission).SingleOrDefault();
                txtAdmissionFee.Text = query.ToString();
            }
        }
        protected void loadInfo()
        {

            //txtMemberId.Text = Utility.generateMemberId("TPW-"+txtBranch.Text);
            txtMemberId.Text = Utility.generateMemberId("TPW");
            image1.ImageUrl = "~/Assets/Images/sample.jpg?" + DateTime.Now.Ticks.ToString();
            txtPwd.Attributes["value"] = "TPW-12345";
            using (TPWDataContext db = new TPWDataContext())
            {
                var item = db.ExtraInformations.SingleOrDefault();
                txtStatic.Text = item.currentNepaliDate + splitUser;
            }
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
            using (TPWDataContext db = new TPWDataContext())
            {
                try
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
                }
                catch (Exception ex)
                {

                    lblInformation.Text = ex.Message;
                }
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
        protected void ddlMemberOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMemberOption.SelectedValue == "1" || ddlMemberOption.SelectedValue== "10")
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
            //else if (ddlMemberOption.SelectedValue == "Gym Admin")
            //{
            //    ddlCatagoryType.DataSource = Staff;
            //    ddlCatagoryType.DataBind();
            //    ddlSubCatagoryType.DataSource = any3;
            //    ddlSubCatagoryType.DataBind();
            //    ddlMembershipPaymentType.SelectedIndex = ddlMembershipPaymentType.Items.IndexOf(ddlMembershipPaymentType.Items.FindByText("N/A"));
            //    txtPaymentAmount.Text = "0";
            //    txtAdmissionFee.Text = "0";

            //}
            //else if (ddlMemberOption.SelectedItem.Text == "Super Admin")
            //{
            //    ddlCatagoryType.DataSource = Staff;
            //    ddlCatagoryType.DataBind();
            //    ddlSubCatagoryType.DataSource = any3;
            //    ddlSubCatagoryType.DataBind();
            //    ddlMembershipPaymentType.SelectedIndex = ddlMembershipPaymentType.Items.IndexOf(ddlMembershipPaymentType.Items.FindByText("N/A"));
            //    txtPaymentAmount.Text = "0";
            //    txtAdmissionFee.Text = "0";

            //}
            else if (ddlMemberOption.SelectedItem.Text == "Free User")
            {
                ddlCatagoryType.DataSource = Free_User;
                ddlCatagoryType.DataBind();
            }

            else if (ddlMemberOption.SelectedItem.Text == "--Select--")
            {
                ddlCatagoryType.DataSource = select;
                ddlCatagoryType.DataBind();
            }
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
            PaymentEvent();
        }
        protected void ddlMembershipPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaymentEvent();
            try
            {
                DateTime dt = Convert.ToDateTime(txtMembershipBeginDate.Text);
                DateTime? dt2;
                if (ddlMemberOption.SelectedValue == "10")
                {
                    if (ddlMembershipPaymentType.SelectedValue == "PerDay")
                    {
                        dt2 = (dt.AddDays(1));
                        txtMembershipExpireDate.Text = DateTime.Parse(dt2.ToString()).ToString("yyyy/MM/dd");
                    }
                    else if (ddlMembershipPaymentType.SelectedValue == "TenDays")
                    {
                        dt2 = (dt.AddDays(10));
                        txtMembershipExpireDate.Text = DateTime.Parse(dt2.ToString()).ToString("yyyy/MM/dd");
                    }
                    else
                    {
                        txtMembershipExpireDate.Text = "";
                    }
                    
                }
                else
                {
                    dt2 = (dt.AddMonths(Convert.ToInt32(ddlMembershipPaymentType.SelectedValue)));
                    txtMembershipExpireDate.Text = DateTime.Parse(dt2.ToString()).ToString("yyyy/MM/dd");
                }
                
            }
            catch (Exception)
            {
                txtMembershipExpireDate.Text = "";
            }
        }
        protected void PaymentEvent()
        {
            using (TPWDataContext db = new TPWDataContext())
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
                                        return;
                                    }
                                    else if (k == 2)
                                    {
                                        var fee = (from c in db.FeeTypes
                                                   where (c.membershipOption == ddlMemberOption.SelectedItem.ToString() && c.membershipType == ddlCatagoryType.SelectedItem.Text.ToString())
                                                   select c.threeMonth).SingleOrDefault();
                                        txtPaymentAmount.Text = fee.ToString();
                                        return;
                                    }
                                    else if (k == 3)
                                    {
                                        var fee = (from c in db.FeeTypes
                                                   where (c.membershipOption == ddlMemberOption.SelectedItem.ToString() && c.membershipType == ddlCatagoryType.SelectedItem.Text.ToString())
                                                   select c.sixMonth).SingleOrDefault();
                                        txtPaymentAmount.Text = fee.ToString();
                                        return;
                                    }
                                    else if (k == 4)
                                    {
                                        var fee = (from c in db.FeeTypes
                                                   where (c.membershipOption == ddlMemberOption.SelectedItem.ToString() && c.membershipType == ddlCatagoryType.SelectedItem.Text.ToString())
                                                   select c.twelveMonth).SingleOrDefault();
                                        txtPaymentAmount.Text = fee.ToString();
                                        return;
                                    }
                                }
                                else
                                {
                                    txtPaymentAmount.Text = Convert.ToString(0);
                                }
                            }
                        }
                    }
                }
                else if (ddlMemberOption.SelectedValue == "10")
                {
                    if (ddlMembershipPaymentType.SelectedValue == "PerDay" || ddlMembershipPaymentType.SelectedValue == "TenDays")
                    {
                        var fee = (from p in db.FeeTypes
                                   where (p.membershipOption == ddlMembershipPaymentType.SelectedValue && p.membershipType == ddlCatagoryType.SelectedValue)
                                   select p.oneTenDays).SingleOrDefault();
                        txtPaymentAmount.Text = fee.ToString();
                    }

                }
                else
                {
                    txtPaymentAmount.Text = Convert.ToString(0);
                }
                
            }
        }
        protected void txtMembershipBeginDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
               var cEngDate = NepaliDateService.NepToEng(txtMembershipBeginDate.Text);
                DateTime dt = Convert.ToDateTime(cEngDate);
                int i = Convert.ToInt32(ddlMembershipPaymentType.SelectedValue);
                DateTime dt2 = (dt.AddMonths(i));
                txtMembershipExpireDate.Text = DateTime.Parse(dt2.ToString()).ToString("yyyy/MM/dd");
            }
            catch (Exception)
            {

            }
        }
        protected void txtMemberId_TextChanged(object sender, EventArgs e)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var memberids = db.MemberInformations;
                foreach (var item in memberids)
                {
                    if (txtMemberId.Text.CompareTo(item.memberId) == 0)
                    {
                        lblmemberId.Visible = true;
                        lblmemberId.Text = "ID already exist";
                        break;
                    }
                    else
                    {
                        lblmemberId.Visible = false;
                    }
                }
            }
        }
        protected void txtuname_TextChanged(object sender, EventArgs e)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                bool userNameExist = db.MemberInformations.FirstOrDefault(p => p.username == txtuname.Text)==null?false:true;
                if (userNameExist)
                {
                    lblUserValid.ForeColor = ColorTranslator.FromHtml("red");
                    lblUserValid.Text = "Username already exist";
                }
                else
                {
                    lblUserValid.ForeColor = ColorTranslator.FromHtml("#037203");
                    lblUserValid.Text = "Username available";
                }

                //}
                //else
                //{
                //    lblUserValid.ForeColor = ColorTranslator.FromHtml("#037203");
                //    lblUserValid.Text = "Username available";
                //}
            }
        }
        protected void btnPriceCalculate_Click(object sender, EventArgs e)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                try
                {

                    double d = 0.0, f, commission, lockerCharge = 0.0;
                    double discountToClient;
                    int p = Convert.ToInt32(txtPaymentAmount.Text);

                    int admission = Convert.ToInt32(txtAdmissionFee.Text);
                    if (txtDiscount.Text == "")
                    {
                        d = 0;
                    }
                    else
                    {
                        d = Convert.ToInt32(txtDiscount.Text);
                    }
                    //Locker
                    //if (chkLocker.Checked)
                    //{
                    //    int lockersubcription = Convert.ToInt32(ddlMembershipPaymentType.SelectedValue);
                    //    var lockercrg = (from c in db.FeeTypes
                    //                     where c.membershipOption == "Locker"
                    //                     select c).SingleOrDefault();
                    //    if (lockersubcription == 1)
                    //        lockerCharge = Convert.ToInt32(lockercrg.oneMonth);
                    //    else if (lockersubcription == 3)
                    //        lockerCharge = Convert.ToInt32(lockercrg.threeMonth);
                    //    else if (lockersubcription == 6)
                    //        lockerCharge = Convert.ToInt32(lockercrg.sixMonth);
                    //    else if (lockersubcription == 12)
                    //        lockerCharge = Convert.ToInt32(lockercrg.twelveMonth);
                    //}

                    if (txtDiscountCode.Text == "")
                    {
                        f = p + admission + lockerCharge - d;

                        txtFinalAmount.Text = f.ToString();
                        btnSubmit.Enabled = true;
                    }
                    else
                    {
                        string discountCode = txtDiscountCode.Text;
                        var item = (from c in db.Influencers
                                    where c.influencerCode.Equals(discountCode) && c.status == true
                                    select c).SingleOrDefault();
                        if (item != null)
                        {
                            discountToClient = item.influencerCode.Contains('%') ? Convert.ToDouble(item.influencerCode.Split('%')[1]) : Convert.ToDouble(item.influencerCode.Split('$')[1]);

                            var finalDiscount = discountToClient < 100 ? (p * (discountToClient / 100)) : discountToClient;

                            commission = Convert.ToInt32(item.influencerComission);
                            hidCommission.Value = commission.ToString();
                            f = p + admission + lockerCharge - (d + finalDiscount);
                            txtFinalAmount.Text = f.ToString();
                            btnSubmit.Enabled = true;
                        }
                        else
                        {
                            lblPopupError.Text = "Discount Code Invalid, Please correct the discount code or empty the field";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorModal", "$('#errorModal').modal();", true);
                            return;
                        }
                    }
                    txtDueAmount.Text = (Convert.ToInt32(txtFinalAmount.Text) - Convert.ToInt32(txtpaidAmount.Text)).ToString();
                }
                catch (Exception ex)
                {
                    _logger.Error("##" + "New Form-{0}", ex.Message);
                    lblPopupError.Text = "Wrong Data Entry. Please correct and Re Calculate";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorModal11", "$('#errorModal').modal();", true);
                    return;
                }
            }
        }
        protected string validateField()
        {
            if (ddlCatagoryType.SelectedIndex == 0)
            {
                return "Please select Membership Catagory";
            }
            else if (string.IsNullOrEmpty(txtMembershipDate.Text))
            {
                return "Please Enter Membership Date";
            }
            else if (string.IsNullOrEmpty(txtMembershipBeginDate.Text))
            {
                return "Please enter Membership Begin Date";
            }
            else if (ddlMembershipPaymentType.SelectedIndex == 0)
            {
                return "Please select Membership payment type";
            }
            else if (string.IsNullOrEmpty(txtMembershipExpireDate.Text))
            {
                return "Please enter Membership Expire Date";
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
            else if (string.IsNullOrEmpty(txtReceiptNo.Text))
            {
                return "Receipt No is required";
            }
            else if (ddlPaymentMethod.SelectedIndex == 0)
            {
                return "Please Select Payment Method";
            }
            else if (ddlActionTaker.SelectedIndex == 0)
            {
                return "Please Select Your Name in Action Taker";
            }
            else if (ddlPaymentMethod.SelectedItem.Text == "Cheque")
            {
                return (txtBankName.Text != "" && txtChequeNumber.Text != "") ? "" : "Bank Name and Cheque Number is Required"; 
            }
            else if (!NepaliDateService.ValidateNepDate(txtMembershipDate.Text))
            {
                return "Membership Date is invalid";
            }
            else if (!NepaliDateService.ValidateNepDate(txtMembershipBeginDate.Text))
            {
                return "Renew Date is invalid";
            }
            else if (!NepaliDateService.ValidateNepDate(txtMembershipExpireDate.Text))
            {
                return "Expired Date is invalid";
            }
            else if (!NepaliDateService.ValidateNepDate(txtDateOfBirth.Text))
            {
                return "Date of Birth is invalid";
            }
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
            else
            {
                return "";
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtBranch.Text != "admin")
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
                    btnSubmit.Enabled = false;
                    insert();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorModal", "$('#errorModal').modal();", true);
                return;
            }

        }
        protected void insert()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                try
                {
                    MemberInformation m = new MemberInformation();
                    MemberInformationLog mLog = new MemberInformationLog();
                    MemberLogin ml = new MemberLogin();
                    PaymentInfo p = new PaymentInfo();
                    StartStop s = new StartStop();
                    Report r = new Report();
                    ComissionPaymentLog inf = new ComissionPaymentLog();
                    ExtraInformation ex = (from c in db.ExtraInformations
                                           where c.extraInformationId == 1
                                           select c).SingleOrDefault();

                    EmMarketing em = (from t in db.EmMarketings
                                      where t.email == txtEmail.Text
                                      select t).SingleOrDefault();

                    #region Insert Member Login
                    ml.memberId = txtMemberId.Text;
                    ml.username = txtuname.Text;
                    ml.password = txtPwd.Text;
                    db.MemberLogins.InsertOnSubmit(ml);
                    #endregion

                    #region Insert MemberInformationf
                    m.memberId = txtMemberId.Text;
                    m.ActiveInactive = ddlActiveInactive.SelectedItem.Text;
                    m.image = hidImage.Value.ToString();
                    m.imageLoc = "Image/Members/" + hidImage.Value.ToString();
                    //m.imageByte = (image1.HasAttributes) ? bytes : null;

                    m.branch = txtBranch.Text;
                    m.username = txtuname.Text;
                    m.password = txtPwd.Text;
                    m.memberOption = ddlMemberOption.SelectedItem.Text;
                    m.memberCatagory = ddlCatagoryType.SelectedItem.Text;
                    m.memberSubCatagory = ddlSubCatagoryType.SelectedItem.Text;
                    //m.memberDate = Convert.ToDateTime(txtMembershipDate.Text);
                    var md= NepaliDateService.NepToEng(txtMembershipDate.Text);
                    m.memberDate = Convert.ToDateTime(md);
                    //if (ddlMemberOption.SelectedValue == "1" || ddlMemberOption.SelectedValue == "2" || ddlMemberOption.SelectedValue == "3" || ddlMemberOption.SelectedValue == "4")
                    //{
                    var mbd = NepaliDateService.NepToEng(txtMembershipBeginDate.Text);
                    m.memberBeginDate = Convert.ToDateTime(mbd);
                    var med = NepaliDateService.NepToEng(txtMembershipExpireDate.Text);
                    m.memberExpireDate = Convert.ToDateTime(med);
                    //}
                    m.admissionFee = txtAdmissionFee.Text;
                    m.memberPaymentType = ddlMembershipPaymentType.SelectedItem.Text;

                    m.email = txtEmail.Text;
                    m.emailStatus = Convert.ToBoolean(0);
                    m.shift = ddlShift.SelectedItem.Text;
                    m.firstName = txtFirstName.Text;
                    m.lastName = txtLastName.Text;
                    m.fullname = txtFirstName.Text + " " + txtLastName.Text;
                    m.contactNo = txtContactNo.Text;
                    var dob = NepaliDateService.NepToEng(txtDateOfBirth.Text);
                    m.dateOfBirth = Convert.ToDateTime(dob);
                    m.address = txtAddress.Text;
                    m.emergencyContactPerson = txtEmergencyContactPerson.Text;
                    m.gender = ddlGender.SelectedItem.Text;
                    m.emergencyContactNo = txtEmergencyContactPhone.Text;
                    m.haveBeenGymBefore = ddlGymAnytimeBefore.SelectedItem.Text;
                    m.haveBeenGymBeforeText = txtHowLong.Text;
                    m.anyhealthIssue = txtAnyHealthIssue.Text;
                    //locker
                    //m.locker = chkLocker.Checked;
                    //m.lockerNo = txtLockerNumber.Text;
                    //m.discountCode = txtDiscountCode.Text;
                    //m.paymentMethod = ddlPaymentMethod.SelectedItem.Text;
                    //m.receiptNo = txtStatic.Text + "-" + txtReceiptNo.Text;
                    m.universalMembershipLimit = ddlMemberOption.SelectedValue == "1" ? 12 : ddlMemberOption.SelectedValue == "3" ? 365 : ddlMemberOption.SelectedValue == "2" ? 12 : ddlMemberOption.SelectedValue == "5" ? 365 : ddlMemberOption.SelectedValue == "6" ? 365 : ddlMemberOption.SelectedValue == "7" ? 365 : ddlMemberOption.SelectedValue == "8" ? 0 : 0;
                    m.remark = txtRemark.Text;
                    m.createdDate = DateTime.Now;
                    m.actionTaker = ddlActionTaker.SelectedItem.Text;
                    m.createdBy = txtBranch.Text;
                    db.MemberInformations.InsertOnSubmit(m);
                    mLog = JsonConvert.DeserializeObject<MemberInformationLog>(JsonConvert.SerializeObject(m));
                    db.MemberInformationLogs.InsertOnSubmit(mLog);

                    #endregion

                    #region Insert Body Measurement
                    foreach (GridViewRow gr in gridBodyMesurement.Rows)
                    {
                        BodyMeasurement b = new BodyMeasurement();
                        b.memberId = txtMemberId.Text;

                        var mdd = ((TextBox)gr.FindControl("txtDate")).Text;
                        b.measurementDate = mdd==""?"": NepaliDateService.NepToEng(mdd).ToString();
                        b.weight = ((TextBox)gr.FindControl("txtWeight")).Text;
                        b.height = ((TextBox)gr.FindControl("txtHeight")).Text;
                        b.upperArm = ((TextBox)gr.FindControl("txtUpperArm")).Text;
                        b.foreArm = ((TextBox)gr.FindControl("txtForeArm")).Text;
                        b.chest = ((TextBox)gr.FindControl("txtChest")).Text;
                        b.waist = ((TextBox)gr.FindControl("txtWaist")).Text;
                        b.thigh = ((TextBox)gr.FindControl("txtThighs")).Text;
                        b.calf = ((TextBox)gr.FindControl("txtCalf")).Text;
                        db.BodyMeasurements.InsertOnSubmit(b);
                    }
                    #endregion

                    #region Insert payment
                    p.memberId = txtMemberId.Text;
                    p.paymentAmount = Convert.ToInt32(txtPaymentAmount.Text);
                    if (txtDiscount.Text == "")
                    {
                        p.discount = 0;
                    }
                    else
                    {
                        p.discount = Convert.ToInt32(txtDiscount.Text);
                    }
                    p.discountReason = txtDiscountReason.Text;
                    p.disocuntCode= txtDiscountCode.Text;
                    p.finalAmount = Convert.ToInt32(txtFinalAmount.Text);
                    p.paidAmount = Convert.ToInt32(txtpaidAmount.Text);
                    p.dueAmount = Convert.ToInt32(txtDueAmount.Text);
                    p.paymentMethod= ddlPaymentMethod.SelectedItem.Text;
                    p.due = false;
                    if (ddlPaymentMethod.SelectedItem.Text == "Cheque")
                    {
                        p.bank = txtBankName.Text;
                        p.chequeNumber = txtChequeNumber.Text;
                    }
                    else if (ddlPaymentMethod.SelectedItem.Text == "E-Banking")
                    {
                        p.referenceId = txtReferenceId.Text;
                    }
                    p.receiptNo= txtStatic.Text + "-" + txtReceiptNo.Text;
                    p.updatedDate = DateTime.Now;
                    p.renewExtend = "newAdmitted";

                    db.PaymentInfos.InsertOnSubmit(p);
                    #endregion

                    #region Insert StartStop
                    s.memberId = txtMemberId.Text;
                    if (ddlMembershipPaymentType.SelectedValue == "1")
                        s.stopLimit = ex.oneMonth;
                    if (ddlMembershipPaymentType.SelectedValue == "3")
                        s.stopLimit = ex.threeMonth;
                    else if (ddlMembershipPaymentType.SelectedValue == "6")
                        s.stopLimit = ex.sixMonth;
                    else if (ddlMembershipPaymentType.SelectedValue == "12")
                        s.stopLimit = ex.twelveMonth;
                    db.StartStops.InsertOnSubmit(s);
                    #endregion

                    #region Insert Report
                    r.memberId = txtMemberId.Text;
                    //if (ddlMemberOption.SelectedValue == "1" || ddlMemberOption.SelectedValue == "2" || ddlMemberOption.SelectedValue == "3" || ddlMemberOption.SelectedValue == "4")
                    //{
                    //    r.memberBeginDate =Convert.ToDateTime(NepaliDateService.NepToEng(txtMembershipBeginDate.Text));
                    //    r.memberExpireDate = Convert.ToDateTime(NepaliDateService.NepToEng(txtMembershipExpireDate.Text));
                    //}
                    r.memberBeginDate = Convert.ToDateTime(NepaliDateService.NepToEng(txtMembershipBeginDate.Text));
                    r.memberExpireDate = Convert.ToDateTime(NepaliDateService.NepToEng(txtMembershipExpireDate.Text));

                    r.memberOption = ddlMemberOption.SelectedItem.Text;
                    r.memberCatagory = ddlCatagoryType.SelectedItem.Text;
                    r.memberPaymentType = ddlMembershipPaymentType.SelectedItem.Text;
                    r.discount = txtDiscount.Text == "" ? 0 : Convert.ToInt32(txtDiscount.Text);
                    r.discountReason = txtDiscountReason.Text;
                    r.discountCode = txtDiscountCode.Text;
                    r.finalAmount = Convert.ToInt32(txtFinalAmount.Text);
                    r.paidAmount = Convert.ToInt32(txtpaidAmount.Text);
                    r.dueAmount = Convert.ToInt32(txtDueAmount.Text);
                    r.receiptNo = txtStatic.Text + "-" + txtReceiptNo.Text;
                    r.paymentMethod = ddlPaymentMethod.SelectedItem.Text;
                    r.created = DateTime.Now;
                    r.actionTaker = ddlActionTaker.SelectedItem.Text;
                    r.renewExtend = "newAdmitted";
                    db.Reports.InsertOnSubmit(r);
                    #endregion

                    #region Insert into Influencerpayment Log
                    var influencerinfo = (from a in db.Influencers
                                          where a.influencerCode == txtDiscountCode.Text
                                          select a).SingleOrDefault();
                    if (influencerinfo != null)
                    {
                        inf.date = DateTime.Now;
                        inf.name = influencerinfo.influencerName;
                        inf.commissionFor = "Influencer";
                        inf.discountCode = txtDiscountCode.Text;
                        inf.comission = Convert.ToInt32(influencerinfo.influencerComission);
                        inf.memberId = txtMemberId.Text;
                        inf.memberName = txtFirstName.Text + " " + txtLastName.Text;
                        inf.status = false;
                        db.ComissionPaymentLogs.InsertOnSubmit(inf);
                    }
                    #endregion
                    //email marketing flag
                    if (em != null)
                    {
                        em.flag = true;
                    }
                    db.SubmitChanges();
                    GenerateQrImage(txtMemberId.Text);
                    lblInformation.Visible = true;
                    btnSubmit.Enabled = false;
                    lblInformation.Text = "Successfully Submitted , now redirecting...";
                    lblInformation.ForeColor = ColorTranslator.FromHtml("#037203");
                    Session["UseIsAuthenticated"] = "true";
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS",
                    "setTimeout(function() { window.location.replace('NewForm.aspx') }, 500);", true);
                    new Task(() =>
                    {
                      MailService.sendEmailNewMember(txtMemberId.Text, txtuname.Text, txtBranch.Text, txtPwd.Text, ddlMemberOption.SelectedItem.Text, ddlCatagoryType.SelectedItem.Text, txtMembershipDate.Text, ddlMembershipPaymentType.SelectedItem.Text, txtMembershipBeginDate.Text, txtMembershipExpireDate.Text, txtEmail.Text, txtFirstName.Text + " " + txtLastName.Text, txtContactNo.Text, txtDateOfBirth.Text, txtAddress.Text, txtDiscountCode.Text, txtFinalAmount.Text, txtpaidAmount.Text, txtDueAmount.Text);
                    }).Start();
                }
                catch (Exception ex)
                {
                    _logger.Error("##" + "New Form-{0}", ex.Message);
                    lblInformation.Text = ex.Message;
                }
            }
        }
        protected void timerId_Tick(object sender, EventArgs e)
        {

        }
        protected bool deleteBodyMeasurement()
        {
            using (TPWDataContext db = new TPWDataContext())
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
                catch (Exception)
                {

                    return false;
                }
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