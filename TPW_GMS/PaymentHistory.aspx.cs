using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Services;

namespace TPW_GMS
{
    public partial class PaymentHistory : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!Page.IsPostBack)
            {
                loadCustomerDropDown(splitUser);
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
        protected void btnSearchReport_Click(object sender, EventArgs e)
        {
            int total = 0;
            IQueryable<Report> items = db.Reports;
            if (!string.IsNullOrWhiteSpace(txtCustomerReceiptNo.Text))
                items = items.Where(c => c.receiptNo.Contains(txtCustomerReceiptNo.Text.Trim()));
            if (ddlMemberName.SelectedItem.Text != "--Select--")
                items = items.Where(c => c.memberId == ddlMemberId.SelectedItem.Text);
            if (ddlMemberName.SelectedItem.Text== "--Select--" && string.IsNullOrWhiteSpace(txtCustomerReceiptNo.Text))
                items = null;

            GridViewReportSearch.DataSource = items;
            GridViewReportSearch.DataBind();
                
            foreach (GridViewRow gr in GridViewReportSearch.Rows)
            {
                Label lbl = (Label)gr.FindControl("lblTotalFee");
                total += Convert.ToInt32(lbl.Text);
            }

            //Label lblTotalmoneyPaid = (Label)GridViewReportSearch.FooterRow.FindControl("lblTotalmoneyPaid");
            lblTotalmoneyPaid.Text = total.ToString();
            lblTotalAmountPaymentHistory.Visible = true;
            lblTotalmoneyPaid.Visible = true;
            db.Dispose();
        }

        protected void GridViewReportSearch_RowDataBound(object sender, GridViewRowEventArgs e)
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
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void loadCustomerDropDown(string branch)
        {
            if (branch=="admin" || branch=="superadmin")
            {
                var name = (from p in db.MemberInformations
                            where p.memberOption != "Free User"
                            select p.fullname);
                ddlMemberName.DataSource = name;
                ddlMemberName.DataBind();
                ddlMemberName.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            else
            {
                var name = (from p in db.MemberInformations
                            where p.memberOption != "Free User" && p.branch.Equals(branch)
                            select p.fullname);
                ddlMemberName.DataSource = name;
                ddlMemberName.DataBind();
                ddlMemberName.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            db.Dispose();
        }
        protected void ddlMemberName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMemberId.Items.Clear();
            var memberId = (from p in db.MemberInformations
                            where p.fullname == ddlMemberName.SelectedItem.ToString()
                            select p.memberId);
            var memberBranch = (from p in db.MemberInformations
                                where p.fullname == ddlMemberName.SelectedItem.ToString()
                                select p.branch);
            if (memberId != null)
            {
                ddlMemberId.DataSource = memberId.ToList();
                ddlMemberId.DataBind();
                if (memberId.ToList().Count > 1)
                {
                    lblMessage.Text = "Multiple Member ID and Branch, Please Choose from dropdown";
                    lblMessage.ForeColor = ColorTranslator.FromHtml("#d81910");
                }
                else
                {
                    lblMessage.Text = "";
                }
            }
            db.Dispose();
        }
    }
}