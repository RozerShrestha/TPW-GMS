﻿using Aspose.Cells.Utility;
using Aspose.Cells;
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
using Dapper;

namespace TPW_GMS
{
    public partial class PaymentReminderCallLIst : System.Web.UI.Page
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
                loadBranch();
                    LoadStartAndEndDate();
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

        protected void btnSendReport_Click(object sender, EventArgs e)
        {
            try
            {
                var itemList = (from m in db.MemberInformations
                                where m.memberOption != "Trainer" && m.memberOption != "Operation Manager" && m.memberOption != "Gym Admin"
                                where m.memberExpireDate >= NepaliDateService.NepToEng(startDate.Text) && m.memberExpireDate <= NepaliDateService.NepToEng(endDate.Text)
                                select new
                                {
                                    Branch = m.branch,
                                    FullName=m.fullname,
                                    Shift=m.shift,
                                    MemberExpiredDate=m.memberExpireDate,
                                    CallStatus = m.paymentReminerCallStatus,
                                    PaymentFeedback = m.paymentReminerPaymentFeedback,
                                    ProgressFeedback = m.pamentReminderProgressFeedback
                                }).ToList();
                itemList = itemList.Where(p => p.MemberExpiredDate >= NepaliDateService.NepToEng(startDate.Text) && p.MemberExpiredDate <= NepaliDateService.NepToEng(endDate.Text)).ToList();
                itemList = itemList.OrderBy(d => d.MemberExpiredDate).ToList();
                itemList = itemList.OrderBy(d => d.Branch).ToList();
                _logger.Info($"ReportDate: {startDate.Text}");

                // Create a Workbook object
                Workbook workbook = new Workbook();
                Worksheet worksheet = workbook.Worksheets[0];
                var jsonString = JsonConvert.SerializeObject(itemList);

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
                string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, $"Files\\PaymentReminerCallBack\\paymentremindercallback-{startDate.Text.Replace("/", "-")}-{endDate.Text.Replace("/", "-")}.xlsx");
                _logger.Info($"Filepath: {filePath}");
                workbook.Save(filePath);
                var emailresponse = MailService.SendEmailToOwner(filePath,"Payment Reminder Callback List");
                if (emailresponse == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Email Send')", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Something went wrong')", true);
                    _logger.Error($"Ex Client CallBack:Error while sending Email due to {emailresponse}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Ex Client Callback Error:{ex}");
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
        private void LoadStartAndEndDate()
        {
            try
            {
                DateTime saturdayDate = DateTime.Now.AddDays(-1);
                if (DayOfWeek.Saturday == DateTime.Now.DayOfWeek)
                {
                    saturdayDate = DateTime.Now;
                }
                else
                {
                    while (saturdayDate.DayOfWeek != DayOfWeek.Saturday)
                        saturdayDate = saturdayDate.AddDays(-1);
                }
                startDate.Text = NepaliDateService.EngToNep(saturdayDate.AddDays(1)).ToString();
                endDate.Text = NepaliDateService.EngToNep(saturdayDate.AddDays(7)).ToString();

            }
            catch (Exception ex)
            {

            }

        }
    }
}