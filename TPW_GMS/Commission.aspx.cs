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
    public partial class Commission : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!IsPostBack)
            {
                if (roleId == "1" || roleId=="4")
                {
                    loadBranch();
                }
                else
                {
                    Response.Redirect("AccessDenied.aspx");
                }
            }
        }
        protected void loadBranch()
        {
            var branchName = from p in db.Logins
                             where !p.username.Contains("admin")
                             select p.username;
            ddlBranch.DataSource = branchName;
            ddlBranch.DataBind();
            ddlBranch.Items.Insert(0, new ListItem("ALL", "ALL"));
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
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlType.SelectedValue == "1" || ddlType.SelectedValue == "3" || ddlType.SelectedValue == "4")
            {
                var items = (from p in db.Staffs
                                where p.status == true && p.staffCatagory == ddlType.SelectedItem.Text && p.associateBranch==ddlBranch.SelectedValue
                                select p.staffName).ToList();
                ddlName.DataSource = items;
                ddlName.DataBind();
            }
            else if (ddlType.SelectedValue == "2")
            {
                var influencers = (from p in db.Influencers
                                   where p.status == true
                                   select p.influencerName).ToList();
                ddlName.DataSource = influencers;
                ddlName.DataBind();
            }
            {
                ddlName.DataSource = null;
                ddlName.DataBind();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            bindCommissionGrid();
            //divGrid.Style.Add("display", "inline");
        }
        protected void bindCommissionGrid()
        {
            try
            {
                var error = validateField();
                if (error == "")
                {
                    lblInfo.Text = "";
                    string name = ddlName.SelectedItem.Text;
                    DateTime Fromdate = Convert.ToDateTime(NepaliDateService.NepToEng(txtDateCommissionFrom.Text));
                    DateTime ToDate = Convert.ToDateTime(NepaliDateService.NepToEng(txtDateCommissionTo.Text));
                    string pStatus = ddlPaidUnpaid.SelectedValue;

                    var items = (from p in db.ComissionPaymentLogs
                                 select p);

                    if (pStatus.Equals("2"))
                        items = items.Where(p => p.name.Equals(name) && p.date >= Fromdate && p.date <= ToDate);
                    else
                        items = items.Where(p => p.name.Equals(name) && p.date >= Fromdate && p.date <= ToDate && p.status.Equals(Convert.ToBoolean(pStatus)));

                    gridCommission.DataSource = items;
                    gridCommission.DataBind();
                }
                else
                {
                    lblInfo.Text = error;
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#E60D25");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
            }
        }
        protected string validateField()
        {
            ddlType.Style.Remove("border-color");
            txtDateCommissionFrom.Style.Remove("border-color");
            txtDateCommissionTo.Style.Remove("border-color");
            if (ddlType.SelectedIndex == 0)
            {
                ddlType.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Please select Type";
            }
            else if (string.IsNullOrEmpty(txtDateCommissionFrom.Text))
            {
                txtDateCommissionFrom.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Enter From date";
            }
            else if (string.IsNullOrEmpty(txtDateCommissionTo.Text))
            {
                txtDateCommissionTo.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Enter To Date";
            }
            else
            {
                
                return "";
            }
        }
        protected void gridCommission_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridCommission.PageIndex = e.NewPageIndex;

            try
            {
                bindCommissionGrid();
            }
            catch (Exception)
            {

            }
        }

        protected void lnkPayAll_Click(object sender, EventArgs e)
        {
            string name = ddlName.SelectedItem.Text;
            DateTime Fromdate = Convert.ToDateTime(txtDateCommissionFrom.Text);
            DateTime ToDate = Convert.ToDateTime(txtDateCommissionTo.Text);
            var items = (from p in db.ComissionPaymentLogs
                         where p.name.Equals(name) && p.date >= Fromdate && p.date <= ToDate
                         select p).ToList();
            foreach(var item in items)
            {
                item.status = !item.status;
            }
            db.SubmitChanges();
            bindCommissionGrid();
        }

        protected void gridCommission_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem.ToString() == "TPW_GMS.Data.ComissionPaymentLog")
            {
                ComissionPaymentLog items = (ComissionPaymentLog)e.Row.DataItem;
                var rd = NepaliDateService.EngToNep(Convert.ToDateTime(items.date));
                ((Label)e.Row.FindControl("lblDate")).Text = rd.ToString();
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                string name = ddlName.SelectedItem.Text;
                DateTime Fromdate =NepaliDateService.NepToEng(txtDateCommissionFrom.Text);
                DateTime ToDate = NepaliDateService.NepToEng(txtDateCommissionTo.Text);
                string pStatus = ddlPaidUnpaid.SelectedValue;
                var items = (from p in db.ComissionPaymentLogs
                             select p);

                if (pStatus.Equals("2"))
                    items = items.Where(p => p.name.Equals(name) && p.date >= Fromdate && p.date <= ToDate);
                else
                    items = items.Where(p => p.name.Equals(name) && p.date >= Fromdate && p.date <= ToDate && p.status.Equals(Convert.ToBoolean(pStatus)));

                Label lblTotalCommission = (Label)e.Row.FindControl("lblTotalCommission");

                var totalCommission = items.Sum(s => (s.comission));
                lblTotalCommission.Text = totalCommission.ToString();
            }
        }
        protected void gridCommission_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int tempId = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                if (e.CommandName == "editroww")
                {
                    var selectQuery = (from p in db.ComissionPaymentLogs
                                       where p.Id == tempId
                                       select p).SingleOrDefault();

                    foreach (GridViewRow gr in gridCommission.Rows)
                    {
                        if (gr.RowIndex == index)
                        {
                            CheckBox chkPaidStatus = (CheckBox)gr.FindControl("chkPaidStatus");
                            selectQuery.status = !selectQuery.status;
                            db.SubmitChanges();
                            bindCommissionGrid();
                            break;
                            //lblMessage.Text = "Payment Status has been successfully Updated.";
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}