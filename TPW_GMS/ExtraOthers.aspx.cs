using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Services;

namespace TPW_GMS
{
    public partial class ExtraOthers : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        ExtraInformation extra = new ExtraInformation();
        const string passphrase = "TPWP@ssw0rd123#";
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!IsPostBack)
            {
                if (roleId == "1" || roleId == "4")
                {
                    loadInfo();
                    LoadData();
                    db.Dispose();
                }
                else
                {
                    Response.Redirect("AccessDenied.aspx");
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
        protected void btnEmailSave_Click(object sender, EventArgs e)
        {
            var ex = (from c in db.ExtraInformations
                      where c.extraInformationId == 1
                      select c).SingleOrDefault();
            if (ex == null)
            {
                extra.extraInformationId = 1;
                extra.email = txtEmailAddress.Text;
                extra.password = txtEmailPassword.Text;
                extra.admission = Convert.ToInt32(txtAdmission.Text);
                extra.smtpClient = txtSmtpClient.Text;
                extra.port = Convert.ToInt32(txtPort.Text);
                extra.staffLateExtension=Convert.ToInt32(txtStaffIntimeExtenstion.Text);
                extra.currentNepaliDate = txtNepaliDate.Text;
                db.ExtraInformations.InsertOnSubmit(extra);
                db.SubmitChanges();
                lblMessage.Text = "Information successfully added";
            }
            else
            {
                ex.email = txtEmailAddress.Text;
                ex.admission = Convert.ToInt32(txtAdmission.Text);
                ex.smtpClient = txtSmtpClient.Text;
                ex.port = Convert.ToInt32(txtPort.Text);
                ex.password = txtEmailPassword.Text;
                ex.staffLateExtension = Convert.ToInt32(txtStaffIntimeExtenstion.Text);
                ex.currentNepaliDate = txtNepaliDate.Text;
                db.SubmitChanges();
                lblMessage.Text = "Information successfully updated";
            }
            db.Dispose();
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
        protected void loadInfo()
        {
            var ex = (from c in db.ExtraInformations
                      where c.extraInformationId == 1
                      select c).SingleOrDefault();
            if (ex != null)
            {
                txtEmailAddress.Text = ex.email;
                txtEmailPassword.Text = ex.password;
                txtAdmission.Text = ex.admission.ToString();
                txtSmtpClient.Text = ex.smtpClient;
                txtPort.Text = ex.port.ToString();
                txtStaffIntimeExtenstion.Text = ex.staffLateExtension.ToString();
                txtNepaliDate.Text = ex.currentNepaliDate;
                txtOneMonth.Text = ex.oneMonth.ToString();
                txtThreeMonth.Text = ex.threeMonth.ToString();
                txtSixMonth.Text = ex.sixMonth.ToString();
                txtTwelveMonth.Text = ex.twelveMonth.ToString();
            }
            else
            {
                txtEmailAddress.Text = "";
                txtEmailPassword.Text = "";
            }
        }
        protected void btnSaveStopStart_Click(object sender, EventArgs e)
        {
            var ex = (from c in db.ExtraInformations
                      where c.extraInformationId == 1
                      select c).SingleOrDefault();
            if (ex == null)
            {
                //ExtraInformation extra = new ExtraInformation();
                extra.oneMonth = Convert.ToInt32(txtOneMonth.Text);
                extra.threeMonth = Convert.ToInt32(txtThreeMonth.Text);
                extra.sixMonth = Convert.ToInt32(txtSixMonth.Text);
                extra.twelveMonth = Convert.ToInt32(txtTwelveMonth.Text);
                db.ExtraInformations.InsertOnSubmit(extra);
                db.SubmitChanges();
                lblMessage.Text = "Information successfully added";
            }
            else
            {
                ex.oneMonth = Convert.ToInt32(txtOneMonth.Text);
                ex.threeMonth = Convert.ToInt32(txtThreeMonth.Text);
                ex.sixMonth = Convert.ToInt32(txtSixMonth.Text);
                ex.twelveMonth = Convert.ToInt32(txtTwelveMonth.Text);
                db.SubmitChanges();
                lblMessage.Text = "Information successfully updated";
            }
            db.Dispose();
        }
        protected void gridInfluencer_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ViewState["CurrentTable1"] != null)
            {
                DataTable dt1 = (DataTable)ViewState["CurrentTable1"];
                DataRow dr1CurrentRow = null;
                int rowIndex = Convert.ToInt32(e.RowIndex);
                if (dt1.Rows.Count > 1)
                {
                    dt1.Rows.Remove(dt1.Rows[rowIndex]);
                    dr1CurrentRow = dt1.NewRow();
                    ViewState["CurrentTable1"] = dt1;
                    gridInfluencer.DataSource = dt1;
                    gridInfluencer.DataBind();

                    for (int i = 0; i < gridInfluencer.Rows.Count - 1; i++)
                    {
                        gridInfluencer.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                    }
                    SetPreviousDataInfluencer();
                }
            }
        }
        protected void btnAddMore_Click(object sender, EventArgs e)
        {
            AddNewRowToGridInfluencer();
        }
        protected bool deleteInfluencer()
        {
            db.Influencers.DeleteAllOnSubmit(db.Influencers);
            db.SubmitChanges();
            return true;
        }
        private void SetInitialRowInfluencer()
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
            dt1.Columns.Add(new DataColumn("Column7", typeof(bool)));



            dr1 = dt1.NewRow();
            //dr1["RowNumber"] = 1;
            dr1["Column1"] = string.Empty;
            dr1["Column2"] = string.Empty;
            dr1["Column3"] = string.Empty;
            dr1["Column4"] = string.Empty;
            dr1["Column5"] = string.Empty;
            dr1["Column6"] = string.Empty;
            dr1["Column7"] = bool.FalseString;


            dt1.Rows.Add(dr1);

            //Store the DataTable in ViewState
            ViewState["CurrentTable1"] = dt1;

            gridInfluencer.DataSource = dt1;
            gridInfluencer.DataBind();
        }
        private void AddNewRowToGridInfluencer()
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
                        TextBox box1 = (TextBox)gridInfluencer.Rows[rowIndex].Cells[1].FindControl("txtInfluencerName");
                        TextBox box2 = (TextBox)gridInfluencer.Rows[rowIndex].Cells[2].FindControl("txtFrom");
                        TextBox box3 = (TextBox)gridInfluencer.Rows[rowIndex].Cells[3].FindControl("txtTo");
                        TextBox box4 = (TextBox)gridInfluencer.Rows[rowIndex].Cells[4].FindControl("txtInfluencerCode");
                        TextBox box5 = (TextBox)gridInfluencer.Rows[rowIndex].Cells[5].FindControl("txtInfluencerCommision");
                        TextBox box6 = (TextBox)gridInfluencer.Rows[rowIndex].Cells[6].FindControl("txtContactNO");
                        CheckBox box7 = (CheckBox)gridInfluencer.Rows[rowIndex].Cells[7].FindControl("chkStatus");



                        dr1CurrentRow = dt1CurrentTable1.NewRow();
                        //dr1CurrentRow["RowNumber"] = i + 1;

                        dt1CurrentTable1.Rows[i - 1]["Column1"] = box1.Text;
                        dt1CurrentTable1.Rows[i - 1]["Column2"] = box2.Text;
                        dt1CurrentTable1.Rows[i - 1]["Column3"] = box3.Text;
                        dt1CurrentTable1.Rows[i - 1]["Column4"] = box4.Text;
                        dt1CurrentTable1.Rows[i - 1]["Column5"] = box5.Text;
                        dt1CurrentTable1.Rows[i - 1]["Column6"] = box6.Text;
                        dt1CurrentTable1.Rows[i - 1]["Column7"] = box7.Checked;


                        rowIndex++;
                    }
                    dt1CurrentTable1.Rows.Add(dr1CurrentRow);
                    ViewState["CurrentTable1"] = dt1CurrentTable1;

                    gridInfluencer.DataSource = dt1CurrentTable1;
                    gridInfluencer.DataBind();
                }
                else
                {
                    Response.Write("ViewState is null");
                }

                //Set Previous Data on Postbacks
                SetPreviousDataInfluencer();
            }
        }
        private void SetPreviousDataInfluencer()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable1"] != null)
            {
                DataTable dt1 = (DataTable)ViewState["CurrentTable1"];
                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)gridInfluencer.Rows[rowIndex].Cells[1].FindControl("txtInfluencerName");
                        TextBox box2 = (TextBox)gridInfluencer.Rows[rowIndex].Cells[2].FindControl("txtFrom");
                        TextBox box3 = (TextBox)gridInfluencer.Rows[rowIndex].Cells[3].FindControl("txtTo");
                        TextBox box4 = (TextBox)gridInfluencer.Rows[rowIndex].Cells[4].FindControl("txtInfluencerCode");
                        TextBox box5 = (TextBox)gridInfluencer.Rows[rowIndex].Cells[5].FindControl("txtInfluencerCommision");
                        TextBox box6 = (TextBox)gridInfluencer.Rows[rowIndex].Cells[6].FindControl("txtContactNO");
                        CheckBox box7 = (CheckBox)gridInfluencer.Rows[rowIndex].Cells[7].FindControl("chkStatus");


                        box1.Text = dt1.Rows[i]["Column1"].ToString();
                        box2.Text = dt1.Rows[i]["Column2"].ToString();
                        box3.Text = dt1.Rows[i]["Column3"].ToString();
                        box4.Text = dt1.Rows[i]["Column4"].ToString();
                        box5.Text = dt1.Rows[i]["Column5"].ToString();
                        box6.Text = dt1.Rows[i]["Column6"].ToString();
                        box7.Checked = dt1.Rows[i]["Column7"].Equals(true);
                        rowIndex++;
                    }
                }
            }
        }
        private void LoadData()
        {
            IQueryable<Influencer> influencerItem = db.Influencers;
            int count = influencerItem.Count();
            if (count > 0)
            {
                gridInfluencer.DataSource = influencerItem.ToList();
                gridInfluencer.DataBind();

                DataTable dt1 = new DataTable();
                dt1.Columns.Add(new DataColumn("Column1", typeof(string)));
                dt1.Columns.Add(new DataColumn("Column2", typeof(string)));
                dt1.Columns.Add(new DataColumn("Column3", typeof(string)));
                dt1.Columns.Add(new DataColumn("Column4", typeof(string)));
                dt1.Columns.Add(new DataColumn("Column5", typeof(string)));
                dt1.Columns.Add(new DataColumn("Column6", typeof(string)));
                dt1.Columns.Add(new DataColumn("Column7", typeof(bool)));

                for (int i = 0; i < gridInfluencer.Rows.Count; i++)
                {
                    TextBox box1 = (TextBox)gridInfluencer.Rows[i].FindControl("txtInfluencerName");
                    TextBox box2 = (TextBox)gridInfluencer.Rows[i].FindControl("txtFrom");
                    TextBox box3 = (TextBox)gridInfluencer.Rows[i].FindControl("txtTo");
                    TextBox box4 = (TextBox)gridInfluencer.Rows[i].FindControl("txtInfluencerCode");
                    TextBox box5 = (TextBox)gridInfluencer.Rows[i].FindControl("txtInfluencerCommision");
                    TextBox box6 = (TextBox)gridInfluencer.Rows[i].FindControl("txtContactNO");
                    CheckBox box7 = (CheckBox)gridInfluencer.Rows[i].FindControl("chkStatus");

                    DataRow dr1 = null;
                    dr1 = dt1.NewRow();
                    //dr1["RowNumber"] = 1;
                    dr1["Column1"] = box1.Text;
                    dr1["Column2"] = box2.Text;
                    dr1["Column3"] = box3.Text;
                    dr1["Column4"] = box4.Text;
                    dr1["Column5"] = box5.Text;
                    dr1["Column6"] = box6.Text;
                    dr1["Column7"] = box7.Checked;

                    dt1.Rows.Add(dr1);
                }
                ViewState["CurrentTable1"] = dt1;
            }
            else
            {
                SetInitialRowInfluencer();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (deleteInfluencer() == true)
            {
                foreach (GridViewRow gr in gridInfluencer.Rows)
                {
                    Influencer i = new Influencer();
                    i.influencerName = ((TextBox)gr.FindControl("txtInfluencerName")).Text;
                    i.startDate =NepaliDateService.NepToEng(((TextBox)gr.FindControl("txtFrom")).Text).ToString();
                    i.endDate = NepaliDateService.NepToEng(((TextBox)gr.FindControl("txtTo")).Text).ToString();
                    i.influencerCode = ((TextBox)gr.FindControl("txtInfluencerCode")).Text;
                    i.influencerComission = ((TextBox)gr.FindControl("txtInfluencerCommision")).Text;
                    i.contactNo = ((TextBox)gr.FindControl("txtContactNO")).Text;
                    i.status = ((CheckBox)gr.FindControl("chkStatus")).Checked;
                    db.Influencers.InsertOnSubmit(i);
                }
                db.SubmitChanges();
                lblInfo.Text = "Success";
            }
            db.Dispose();
        }
        protected void gridInfluencer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem.ToString() == "TPW_GMS.Data.Influencer")
            {
                Influencer items = (Influencer)e.Row.DataItem;

                ((TextBox)e.Row.FindControl("txtInfluencerName")).Text = items.influencerName;
                ((TextBox)e.Row.FindControl("txtFrom")).Text = NepaliDateService.EngToNep(Convert.ToDateTime(items.startDate)).ToString();
                ((TextBox)e.Row.FindControl("txtTo")).Text = NepaliDateService.EngToNep(Convert.ToDateTime(items.endDate)).ToString();
                ((TextBox)e.Row.FindControl("txtInfluencerCode")).Text = items.influencerCode;
                ((TextBox)e.Row.FindControl("txtInfluencerCommision")).Text = items.influencerComission;
                ((TextBox)e.Row.FindControl("txtContactNO")).Text = items.contactNo.ToString();
                ((CheckBox)e.Row.FindControl("chkStatus")).Checked = items.status.Value;
            }
        }
    }
}