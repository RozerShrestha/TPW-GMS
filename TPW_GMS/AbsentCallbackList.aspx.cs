using Aspose.Cells.Utility;
using Aspose.Cells;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Repository;
using TPW_GMS.Services;
using System.IO;

namespace TPW_GMS
{
    public partial class AbsentCallbackList : System.Web.UI.Page
    {
        string roleId, loginUser, splitUser;
        public DbConFactory dbCon = new DbConFactory();
        private TPWDataContext db = new TPWDataContext();
        public static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!IsPostBack)
            {
                if (roleId == "1" || roleId == "4")
                {
                    btnSendReport.Visible = true;
                }
                if (roleId != "3")
                {
                    loadBranch();
                    LoadReportDate();
                }
                else
                {
                    Response.Redirect("AccessDenied.aspx");
                }
            }
        }

        private void LoadReportDate()
        {
            DateTime wednesdayDate= DateTime.Now.AddDays(-1);
            if (DayOfWeek.Wednesday == DateTime.Now.DayOfWeek)
            {
                wednesdayDate = DateTime.Now;
            }
            else
            {
                while (wednesdayDate.DayOfWeek != DayOfWeek.Wednesday)
                    wednesdayDate = wednesdayDate.AddDays(-1);
            }
            startDate.Text = NepaliDateService.EngToNep(wednesdayDate).ToString();
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
        protected void btnSendReport_Click(object sender, EventArgs e)
        {
            try
            {
                var reportDate = NepaliDateService.NepToEng(startDate.Text).ToShortDateString();
                reportDate = reportDate.Replace("/", "-");
                using (var conn = dbCon.CreateConnection())
                {
                    var result = conn.Query("AbsentcallbackList",
                        param: new { wedDate = reportDate, branch = "All" }
                        , commandType: System.Data.CommandType.StoredProcedure).ToList();
                    _logger.Info($"ReportDate: {reportDate}");
                    // Create a Workbook object
                    Workbook workbook = new Workbook();
                    Worksheet worksheet = workbook.Worksheets[0];
                    var jsonString = JsonConvert.SerializeObject(result);

                    // Set Styles
                    CellsFactory factory = new CellsFactory();
                    Aspose.Cells.Style style = factory.CreateStyle();
                    style.HorizontalAlignment = TextAlignmentType.Center;
                    style.Font.Color = System.Drawing.Color.BlueViolet;
                    style.Font.IsBold = true;

                    // Set JsonLayoutOptions
                    JsonLayoutOptions options = new JsonLayoutOptions();
                    options.TitleStyle = style;
                    options.ArrayAsTable = true;

                    // Import JSON Data
                    JsonUtility.ImportData(jsonString, worksheet.Cells, 0, 0, options);
                    // Save Excel file
                    string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, $"Files\\AbsentCallback\\absentcallback-{startDate.Text.Replace("/", "-")}.xlsx");
                    _logger.Info($"Filepath: {filePath}");
                    workbook.Save(filePath);
                    var emailresponse = MailService.SendEmailToOwner(filePath, "Absent Callback List");
                    if (emailresponse == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Email Send')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Something went wrong')", true);
                        _logger.Error($"Absent Callback CallBack:Error while sending Email due to {emailresponse}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Absent Callback Error:{ex}");
            }
        }

        protected void loadBranch()
        {
            using (var db = new TPWDataContext())
            {
                if (roleId == "1" || roleId == "4")
                {

                    var branchName = from p in db.Logins
                                     where !p.username.Contains("admin")
                                     select p.username;
                    branch.DataSource = branchName;
                    branch.DataBind();
                    branch.Items.Insert(0, new ListItem("ALL", "ALL"));
                }
                else
                {
                    var branchName = from p in db.Logins
                                     where p.username.Contains(splitUser) && !p.username.Contains("admin")
                                     select p.username;
                    branch.DataSource = branchName;
                    branch.DataBind();
                }
            }
        }
        
    }
}