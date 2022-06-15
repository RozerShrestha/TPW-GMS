    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;

namespace TPW_GMS
{
    public partial class TPWReport : System.Web.UI.Page
    {
       public int sumNoOfMember;
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!IsPostBack)
            {
                if (roleId == "1")
                {
                    loadBranch();
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
        protected void loadBranch()
        {
            var branchName = from p in db.Logins
                             where !p.username.Contains("admin")
                             select p.username;
            ddlBranch.DataSource = branchName;
            ddlBranch.DataBind();
            ddlBranch.Items.Insert(0, new ListItem("--Select--", "0"));
            db.Dispose();
        }
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            Response.Redirect("TPWReport.aspx");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindData();
        }
        protected void gridViewReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string.IsNullOrEmpty(txtStartDateRenewed.Text);
            string.IsNullOrEmpty(txtEndDateRenewed.Text);

            
        }
        protected void bindData()
        {
            var a1 = txtStartDateNewMember.Text;
            var a2 = txtEndDateNewMember.Text;
            var a3 = ddlOption.SelectedIndex;
            var a4 = ddlCatagorySearch.SelectedIndex;
            var a5 = ddlMembershipPaymentType.SelectedIndex;
            var a6 = ddlShift.SelectedIndex;
            var at = a1 == "" && a2 == "" && a3==0 && a4==0 && a5==0 && a6==0 ? true : false;

            var b1 = txtStartDateRenewed.Text;
            var b2 = txtEndDateRenewed.Text;
            var b3 = ddlOptionRenewed.SelectedIndex;
            var b4 = ddlCatagorySearchRenewed.SelectedIndex;
            var b5 = ddlMembershipPaymentTypeRenewed.SelectedIndex;
            var b6 = ddlShiftRenewed.SelectedIndex;
            var bt = b1 == "" && b2 == "" && b3 == 0 && b4 == 0 && b5 == 0 && b6 == 0 ? true : false;

            var c1 = txtStartDateSuplements.Text;
            var c2 = txtEndDateSuplements.Text;
            var c12 = c1 == "" && c2 == "" ? true : false;

            var c3 = txtStartDateExpenditure.Text;
            var c4 = txtEndDateExpenditure.Text;
            var c34= c3== "" && c4 == "" ? true : false;

            var c5 = txtStartDateOverAllReport.Text;
            var c6 = txtEndDateOverAllReport.Text;
            var c56 = c5 == "" && c6 == "" ? true : false;

            if (CheckEmpty())
            { lblMessage.Text = "Start date and End date to be field first"; }
            else
            {
                //for New member Join Filter
                if (bt && c12 && c34 && c56)
                {
                    GridViewReportSearch.Visible = true;
                    pnlSuplementsReport.Visible = false;
                    GridExpenditure.Visible = false;
                    pnlOverAllReport.Visible = false;
                    //IQueryable<MemberInformation> items = db.MemberInformations;

                    var items = from m in db.MemberInformations
                                join p in db.PaymentInfos on m.memberId equals p.memberId
                                select new
                                {
                                    m.memberId,
                                    m.fullname,
                                    m.shift,
                                    m.memberDate,
                                    m.memberBeginDate,
                                    m.memberOption,
                                    m.memberCatagory,
                                    m.gender,
                                    m.memberPaymentType,
                                    m.branch,
                                    p.finalAmount,
                                };
                    if (ddlBranch.SelectedIndex != 0)
                        items = items.Where(c => c.branch == ddlBranch.SelectedItem.Text);
                    if (txtStartDateNewMember.Text != string.Empty && txtEndDateNewMember.Text != string.Empty)
                        items = items.Where(c => c.memberDate >= Convert.ToDateTime(txtStartDateNewMember.Text) && c.memberDate <= Convert.ToDateTime(txtEndDateNewMember.Text));
                    if (ddlCatagorySearch.SelectedIndex != 0 && ddlMembershipPaymentType.SelectedIndex != 0)
                        items = items.Where(c => c.memberCatagory == ddlCatagorySearch.SelectedItem.Text && c.memberPaymentType == ddlMembershipPaymentType.SelectedItem.Text);
                    if (ddlCatagorySearch.SelectedIndex != 0 && ddlMembershipPaymentType.SelectedIndex == 0)
                        items = items.Where(c => c.memberCatagory == ddlCatagorySearch.SelectedItem.Text);
                    if (ddlCatagorySearch.SelectedIndex == 0 && ddlMembershipPaymentType.SelectedIndex != 0)
                        items = items.Where(c => c.memberPaymentType == ddlMembershipPaymentType.SelectedItem.Text);
                    if (ddlOption.SelectedIndex != 0)
                        items = items.Where(c => c.memberOption == ddlOption.SelectedItem.Text);
                    if (ddlShift.SelectedIndex != 0)
                        items = items.Where(c => c.shift == ddlShift.SelectedItem.Text);

                    items = items.OrderBy(d => d.memberDate);

                     sumNoOfMember =Convert.ToInt32(items.Sum(x => x.finalAmount));
                    

                    GridViewReportSearch.DataSource = items;
                    GridViewReportSearch.DataBind();
                }
                //for renew members
                else if(at && c12 && c34 && c56)
                {
                    GridViewReportSearch.Visible = true;
                    pnlSuplementsReport.Visible = false;
                    GridExpenditure.Visible = false;
                    pnlOverAllReport.Visible = false;
                    //IQueryable<MemberInformation> items = db.MemberInformations;

                    var items = from m in db.MemberInformations
                                join p in db.PaymentInfos on m.memberId equals p.memberId
                                select new
                                {
                                    m.memberId,
                                    m.fullname,
                                    m.shift,
                                    m.memberDate,
                                    m.memberBeginDate,
                                    m.memberOption,
                                    m.memberCatagory,
                                    m.gender,
                                    m.memberPaymentType,
                                    m.branch,
                                    p.finalAmount,
                                };
                    if (ddlBranch.SelectedIndex != 0)
                        items = items.Where(c => c.branch == ddlBranch.SelectedItem.Text);
                    if (txtStartDateRenewed.Text != string.Empty && txtEndDateRenewed.Text != string.Empty)
                        items = items.Where(c => c.memberBeginDate >= Convert.ToDateTime(txtStartDateRenewed.Text) && c.memberBeginDate <= Convert.ToDateTime(txtEndDateRenewed.Text));
                    if (ddlCatagorySearchRenewed.SelectedIndex != 0 && ddlMembershipPaymentTypeRenewed.SelectedIndex != 0)
                        items = items.Where(c => c.memberCatagory == ddlCatagorySearchRenewed.SelectedItem.Text && c.memberPaymentType == ddlMembershipPaymentTypeRenewed.SelectedItem.Text);
                    if (ddlCatagorySearchRenewed.SelectedIndex != 0 && ddlMembershipPaymentTypeRenewed.SelectedIndex == 0)
                        items = items.Where(c => c.memberCatagory == ddlCatagorySearchRenewed.SelectedItem.Text);
                    if (ddlCatagorySearchRenewed.SelectedIndex == 0 && ddlMembershipPaymentTypeRenewed.SelectedIndex != 0)
                        items = items.Where(c => c.memberPaymentType == ddlMembershipPaymentTypeRenewed.SelectedItem.Text);
                    if (ddlOptionRenewed.SelectedIndex != 0)
                        items = items.Where(c => c.memberOption == ddlOptionRenewed.SelectedItem.Text);
                    if (ddlShiftRenewed.SelectedIndex != 0)
                        items = items.Where(c => c.shift == ddlShiftRenewed.SelectedItem.Text);

                    sumNoOfMember = Convert.ToInt32(items.Sum(x => x.finalAmount));
                    items = items.OrderBy(d => d.memberBeginDate);
                    GridViewReportSearch.DataSource = items;
                    GridViewReportSearch.DataBind();
                }
                //for suplements
                else if (at && bt && c34 && c56)
                {
                    GridViewReportSearch.Visible = false;
                    pnlSuplementsReport.Visible = true;
                    GridExpenditure.Visible = false;
                    pnlOverAllReport.Visible = false;
                    var itemsSuplementBuy = from sb in db.Suplements
                                            where (sb.date >= Convert.ToDateTime(txtStartDateSuplements.Text) && sb.date <= Convert.ToDateTime(txtEndDateSuplements.Text))
                                            select new
                                            {
                                                sb.date,
                                                sb.nameOfSuplement,
                                                sb.quantity,
                                                sb.perPrice,
                                                sb.totalPrice,
                                                sb.discount,
                                                sb.finalPrice,
                                                sb.branch
                                            };
                    var itemsSuplementSell = from ss in db.SuplementSellings
                                             where (ss.date_Sell >= Convert.ToDateTime(txtStartDateSuplements.Text) && ss.date_Sell <= Convert.ToDateTime(txtEndDateSuplements.Text))
                                             select new
                                             {
                                                 ss.date_Sell,
                                                 ss.nameOfSuplement_Sell,
                                                 ss.customer_Sell,
                                                 ss.quantity_Sell,
                                                 ss.perPrice_Sell,
                                                 ss.totalPrice_Sell,
                                                 ss.discount_Sell,
                                                 ss.finalPrice_Sell,
                                                 ss.branch
                                             };
                    if (ddlBranch.SelectedIndex != 0)
                    {
                        itemsSuplementBuy = itemsSuplementBuy.Where(c => c.branch == ddlBranch.SelectedItem.Text);
                        itemsSuplementSell = itemsSuplementSell.Where(a => a.branch == ddlBranch.SelectedItem.Text);
                    }

                    itemsSuplementBuy = itemsSuplementBuy.OrderBy(d => d.date);
                    itemsSuplementSell = itemsSuplementSell.OrderBy(dd => dd.date_Sell);

                    gridSuplementSellingReport.DataSource = itemsSuplementSell;
                    gridSuplementSellingReport.DataBind();
                    gridSuplementBuyingReport.DataSource = itemsSuplementBuy;
                    gridSuplementBuyingReport.DataBind();
                }
                //for Expenditures
                else if (at && bt && c12 && c56)
                {
                    GridViewReportSearch.Visible = false;
                    pnlSuplementsReport.Visible = false;
                    GridExpenditure.Visible = true;
                    pnlOverAllReport.Visible = false;

                    var itemExpenditures = from exp in db.Expenditures
                                           where (exp.expenditureDate >= Convert.ToDateTime(txtStartDateExpenditure.Text) && exp.expenditureDate <= Convert.ToDateTime(txtEndDateExpenditure.Text))
                                           select new
                                           {
                                               exp.expenditureDate,
                                               exp.branch,
                                               exp.expenditureType,
                                               exp.expenditureRate
                                           };
                    if (ddlBranch.SelectedIndex != 0)
                        itemExpenditures = itemExpenditures.Where(c => c.branch == ddlBranch.SelectedItem.Text);

                    itemExpenditures = itemExpenditures.OrderBy(d => d.expenditureDate);
                    GridExpenditure.DataSource = itemExpenditures;
                    GridExpenditure.DataBind();
                }
                //for OverAll Report
                else if (at && bt && c12 && c34)
                {
                    GridViewReportSearch.Visible = false;
                    pnlSuplementsReport.Visible = false;
                    GridExpenditure.Visible = false;
                    pnlOverAllReport.Visible = true;
                    if(ddlBranch.SelectedIndex==0)
                    {
                        //total number of member joined
                        var itemTotalMember = (from m in db.MemberInformations
                                               where (m.memberDate >= Convert.ToDateTime(txtStartDateOverAllReport.Text) && m.memberDate <= Convert.ToDateTime(txtEndDateOverAllReport.Text))
                                               select m);
                        int totalMember = itemTotalMember.Count();
                        lblTotalNoOfMemberJoined.Text = totalMember.ToString();

                        //total number of member renewed
                        var itemTotalMemberRenewed = (from m in db.MemberInformations
                                               where (m.memberBeginDate >= Convert.ToDateTime(txtStartDateOverAllReport.Text) && m.memberBeginDate <= Convert.ToDateTime(txtEndDateOverAllReport.Text))
                                               select m);
                        int totalMemberRenewed = itemTotalMemberRenewed.Count();
                        lblTotalNoOfMemberRenewed.Text = totalMemberRenewed.ToString();

                        //total earning                 
                        var itemTotalEarning = (from m in db.MemberInformations
                                                join p in db.PaymentInfos on m.memberId equals p.memberId
                                                where (m.memberBeginDate >= Convert.ToDateTime(txtStartDateOverAllReport.Text) && m.memberBeginDate <= Convert.ToDateTime(txtEndDateOverAllReport.Text))
                                                select new { p.finalAmount });
                        var totalEarning = itemTotalEarning.Sum(p => p.finalAmount);
                        lblEarningFromGym.Text = totalEarning.ToString();

                        //total suplements buying
                        var itemTotalSuplementBuying = from s in db.Suplements
                                                       where (s.date >= Convert.ToDateTime(txtStartDateOverAllReport.Text) && s.date <= Convert.ToDateTime(txtEndDateOverAllReport.Text))
                                                       select new
                                                       {
                                                           s.finalPrice
                                                       };
                        var totalSuplementBuying = itemTotalSuplementBuying.Sum(x => (x.finalPrice));
                        lblTotalSuplementsBought.Text = totalSuplementBuying.ToString();

                        //total suplements selling
                        var itemTotalSuplementSelling = from s in db.SuplementSellings
                                                        where (s.date_Sell >= Convert.ToDateTime(txtStartDateOverAllReport.Text) && s.date_Sell <= Convert.ToDateTime(txtEndDateOverAllReport.Text))
                                                        select new
                                                        {
                                                            s.finalPrice_Sell
                                                        };
                        var totalSuplementSelling = itemTotalSuplementSelling.Sum(x => (x.finalPrice_Sell));
                        lblTotalSuplementsSold.Text = totalSuplementSelling.ToString();


                        //total expenses
                        var itemTotalExpenses = from ex in db.Expenditures
                                                where (ex.expenditureDate >= Convert.ToDateTime(txtStartDateOverAllReport.Text) && ex.expenditureDate <= Convert.ToDateTime(txtEndDateOverAllReport.Text))
                                                select new
                                                {
                                                    ex.expenditureRate
                                                };
                        var totalExpenses = itemTotalExpenses.Sum(x => (Convert.ToDecimal(x.expenditureRate)));
                        lblTotalExpenses.Text = totalExpenses.ToString();
                    }
                    else
                    {
                        //total number of member joined
                        var itemTotalMember = (from m in db.MemberInformations
                                               where (m.branch==ddlBranch.SelectedItem.Text && m.memberDate >= Convert.ToDateTime(txtStartDateOverAllReport.Text) && m.memberDate <= Convert.ToDateTime(txtEndDateOverAllReport.Text))
                                               select m);
                        int totalMember = itemTotalMember.Count();
                        lblTotalNoOfMemberJoined.Text = totalMember.ToString();

                        //total earning                 
                        var itemTotalEarning = (from p in db.Reports
                                                join m in db.MemberInformations on p.memberId equals m.memberId
                                                where (m.branch == ddlBranch.SelectedItem.Text && p.memberBeginDate >= Convert.ToDateTime(txtStartDateOverAllReport.Text) && p.memberBeginDate <= Convert.ToDateTime(txtEndDateOverAllReport.Text))
                                                select new { p.finalAmount });
                        var totalEarning = itemTotalEarning.Sum(p => p.finalAmount);
                        lblEarningFromGym.Text = totalEarning.ToString();

                        //total suplements buying
                        var itemTotalSuplementBuying = from s in db.Suplements
                                                       where (s.branch==ddlBranch.SelectedItem.Text && s.date >= Convert.ToDateTime(txtStartDateOverAllReport.Text) && s.date <= Convert.ToDateTime(txtEndDateOverAllReport.Text))
                                                       select new
                                                       {
                                                           s.finalPrice
                                                       };
                        var totalSuplementBuying = itemTotalSuplementBuying.Sum(x => (x.finalPrice));
                        lblTotalSuplementsBought.Text = totalSuplementBuying.ToString();

                        //total suplements selling
                        var itemTotalSuplementSelling = from s in db.SuplementSellings
                                                        where (s.branch == ddlBranch.SelectedItem.Text && s.date_Sell >= Convert.ToDateTime(txtStartDateOverAllReport.Text) && s.date_Sell <= Convert.ToDateTime(txtEndDateOverAllReport.Text))
                                                        select new
                                                        {
                                                            s.finalPrice_Sell
                                                        };
                        var totalSuplementSelling = itemTotalSuplementSelling.Sum(x => (x.finalPrice_Sell));
                        lblTotalSuplementsSold.Text = totalSuplementSelling.ToString();


                        //total expenses
                        var itemTotalExpenses = from ex in db.Expenditures
                                                where (ex.branch==ddlBranch.SelectedItem.Text && ex.expenditureDate >= Convert.ToDateTime(txtStartDateOverAllReport.Text) && ex.expenditureDate <= Convert.ToDateTime(txtEndDateOverAllReport.Text))
                                                select new
                                                {
                                                    ex.expenditureRate
                                                };
                        var totalExpenses = itemTotalExpenses.Sum(x => (Convert.ToDecimal(x.expenditureRate)));
                        lblTotalExpenses.Text = totalExpenses.ToString();
                    }  
                }
                else
                {
                    GridViewReportSearch.Visible = false;
                    gridSuplementBuyingReport.Visible = false;
                    gridSuplementSellingReport.Visible = false;
                    GridExpenditure.Visible = false;
                }
            }
            db.Dispose();
        }
        protected void gridSuplementSellingReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                var itemsSuplementSell = from ss in db.SuplementSellings
                                         where (ss.date_Sell >= Convert.ToDateTime(txtStartDateSuplements.Text) && ss.date_Sell <= Convert.ToDateTime(txtEndDateSuplements.Text))
                                         select new
                                         {
                                             ss.date_Sell,
                                             ss.nameOfSuplement_Sell,
                                             ss.customer_Sell,
                                             ss.quantity_Sell,
                                             ss.perPrice_Sell,
                                             ss.totalPrice_Sell,
                                             ss.discount_Sell,
                                             ss.finalPrice_Sell
                                         };
                Label lblGrandQuantity = (Label)e.Row.FindControl("lblGrandQuantity");
                Label lblGrandPerPrice = (Label)e.Row.FindControl("lblGrandPerPrice");
                Label lblGrandTotalPrice = (Label)e.Row.FindControl("lblGrandTotalPrice");
                Label lblGrandDiscount = (Label)e.Row.FindControl("lblGrandDiscount");
                Label lblGrandFinalPrice = (Label)e.Row.FindControl("lblGrandFinalPrice");

                var grandquantity = itemsSuplementSell.Sum(x => (x.quantity_Sell));
                var grandPerPrice = itemsSuplementSell.Sum(x => (x.perPrice_Sell));
                var grandTotalPrice = itemsSuplementSell.Sum(x => (x.totalPrice_Sell));
                var grandDiscount = itemsSuplementSell.Sum(x => (x.discount_Sell));
                var grandFinalPrice = itemsSuplementSell.Sum(x => (x.finalPrice_Sell));

                lblGrandQuantity.Text = grandquantity.ToString();
                lblGrandPerPrice.Text = grandPerPrice.ToString();
                lblGrandTotalPrice.Text = grandTotalPrice.ToString();
                lblGrandDiscount.Text = grandDiscount.ToString();
                lblGrandFinalPrice.Text = grandFinalPrice.ToString();
            }
            db.Dispose();
        }
        protected void gridSuplementBuyingReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                var itemsSuplementBuy = from sb in db.Suplements
                                        where (sb.date >= Convert.ToDateTime(txtStartDateSuplements.Text) && sb.date <= Convert.ToDateTime(txtEndDateSuplements.Text))
                                        select new
                                        {
                                            sb.date,
                                            sb.nameOfSuplement,
                                            sb.quantity,
                                            sb.perPrice,
                                            sb.totalPrice,
                                            sb.discount,
                                            sb.finalPrice
                                        };
                Label lblGrandQuantity = (Label)e.Row.FindControl("lblGrandQuantity");
                Label lblGrandPerPrice = (Label)e.Row.FindControl("lblGrandPerPrice");
                Label lblGrandTotalPrice = (Label)e.Row.FindControl("lblGrandTotalPrice");
                Label lblGrandDiscount = (Label)e.Row.FindControl("lblGrandDiscount");
                Label lblGrandFinalPrice = (Label)e.Row.FindControl("lblGrandFinalPrice");

                var grandquantity = itemsSuplementBuy.Sum(x => (x.quantity));
                var grandPerPrice = itemsSuplementBuy.Sum(x => (x.perPrice));
                var grandTotalPrice = itemsSuplementBuy.Sum(x => (x.totalPrice));
                var grandDiscount = itemsSuplementBuy.Sum(x => (x.discount));
                var grandFinalPrice = itemsSuplementBuy.Sum(x => (x.finalPrice));

                lblGrandQuantity.Text = grandquantity.ToString();
                lblGrandPerPrice.Text = grandPerPrice.ToString();
                lblGrandTotalPrice.Text = grandTotalPrice.ToString();
                lblGrandDiscount.Text = grandDiscount.ToString();
                lblGrandFinalPrice.Text = grandFinalPrice.ToString();
            }
            db.Dispose();
        }
        protected void gridSuplementBuyingReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridSuplementBuyingReport.PageIndex = e.NewPageIndex;
            this.bindData();
        }
        protected void gridSuplementSellingReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridSuplementSellingReport.PageIndex = e.NewPageIndex;
            this.bindData();
        }
        protected void GridViewReportSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewReportSearch.PageIndex = e.NewPageIndex;
            this.bindData();
        }
        protected void GridExpenditure_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridExpenditure.PageIndex = e.NewPageIndex;
            this.bindData();
        }
        protected void GridViewReportSearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //Label lblSum = (Label)e.Row.FindControl("lblSum");
                //lblSum.Text = sumNoOfMemberJoined.ToString();
            }
        }
        protected void GridExpenditure_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                var itemExpenditures = from exp in db.Expenditures
                                       where (exp.expenditureDate >= Convert.ToDateTime(txtStartDateExpenditure.Text) && exp.expenditureDate <= Convert.ToDateTime(txtEndDateExpenditure.Text))
                                       select new
                                       {
                                           exp.expenditureDate,
                                           exp.expenditureType,
                                           exp.expenditureRate
                                       };
                Label lblTotalRate = (Label)e.Row.FindControl("lblTotalRate");
                var totalRate = itemExpenditures.Sum(x => (Convert.ToDecimal(x.expenditureRate)));
                lblTotalRate.Text = totalRate.ToString();
            }
            db.Dispose();
        }
        protected bool CheckEmpty()
        {
            //    if (string.IsNullOrEmpty(txtStartDateNewMember.Text) && string.IsNullOrEmpty(txtEndDateNewMember.Text) && ddlCatagorySearch.SelectedIndex == 0 && ddlMembershipPaymentType.SelectedIndex == 0 && ddlOption.SelectedIndex == 0 && ddlShift.SelectedIndex == 0 && string.IsNullOrEmpty(txtStartDateTotalEarning.Text) && string.IsNullOrEmpty(txtEndDateTotalEarning.Text))
            //    {
            //        return true;
            //    }
            //    else
            //        return false;
            return false;
        }
    }
}