﻿using ActiveUp.Net.WhoIs;
using Aspose.Cells;
using Aspose.Cells.Utility;
using Dapper;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Repository;
using TPW_GMS.Services;

namespace TPW_GMS
{
    public partial class NewAdmissionCallback : System.Web.UI.Page
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
                if(roleId=="1" || roleId == "4")
                {
                    loadBranch();
                    LoadStartAndEndDate();
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

        protected void btnSendReport_Click(object sender, EventArgs e)
        {
            try
            {
                var itemList = (from m in db.MemberInformations
                            where m.memberOption != "Trainer" && m.memberOption != "Operation Manager" && m.memberOption != "Gym Admin"
                            where m.memberDate >= NepaliDateService.NepToEng(startDate.Text) && m.memberDate <= NepaliDateService.NepToEng(endDate.Text)
                            select new
                            {
                                Branch=m.branch,
                                FullName = m.fullname,
                                Shift = m.shift,
                                MemberExpiredDate = m.memberExpireDate,
                                CallStatus = m.newAdmissionCallStatus,
                                CallRemark = m.newAdmissionCallRemark
                            }).ToList();
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
                string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, $"Files\\NewAdmissionCallBack\\newadmissioncallback-{startDate.Text.Replace("/", "-")}-{endDate.Text.Replace("/", "-")}.xlsx");
                _logger.Info($"Filepath: {filePath}");
                workbook.Save(filePath);
                var emailresponse = MailService.SendEmailToOwner(filePath,"New Admission Callback");
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
                DateTime sundayDate = DateTime.Now.AddDays(-1);
                DateTime fridayDate=DateTime.Now.AddDays(-1);
                if (DayOfWeek.Sunday == DateTime.Now.DayOfWeek)
                {
                    sundayDate = DateTime.Now;
                }
                else
                {
                    while (sundayDate.DayOfWeek != DayOfWeek.Sunday)
                        sundayDate = sundayDate.AddDays(-1);
                }
                sundayDate = sundayDate.AddDays(-7);
                startDate.Text = NepaliDateService.EngToNep(sundayDate).ToString();
                endDate.Text = NepaliDateService.EngToNep(sundayDate.AddDays(5)).ToString();

            }
            catch (Exception ex)
            {

            }

        }

    }
}