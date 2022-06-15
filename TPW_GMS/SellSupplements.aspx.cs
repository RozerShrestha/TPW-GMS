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
    public partial class SellSupplements : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            hidUserLogin.Value = splitUser;
            if (!Page.IsPostBack)
            {
                //if (roleId != "1")
                {
                    txtBranch.Text = splitUser;
                    loadCustomerDropDown(splitUser);
                    loadSuplementDropDown(splitUser);
                    loadInfo();
                    try
                    {
                        string key = Request.QueryString["key"].ToString();
                        if (key == "edit")
                        {
                            loadData();
                            disableField(roleId);
                        }
                        else if (key == "delete")
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "deleteConfirmModal", "$('#deleteConfirmModal').modal();", true);
                        }
                    }
                    catch (Exception)
                    { }
                    db.Dispose();
                }
            }
            try
            {
                string key = Request.QueryString["key"].ToString();
                if (key == "edit")
                    btnAddSuplementsSell.Enabled = false;
            }
            catch (Exception)
            { }

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
        protected void loadInfo()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var item = db.ExtraInformations.SingleOrDefault();
                txtStatic.Text = item.currentNepaliDate + splitUser;
            }
        }
        protected void loadCustomerDropDown(string branch)
        {
            var name = (from p in db.MemberInformations
                        where p.branch == branch
                        select p.fullname);
            ddlCustomerSell.DataSource = name;
            ddlCustomerSell.DataBind();
            ddlCustomerSell.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        protected void loadSuplementDropDown(string branch)
        {
            var SupName = (from p in db.Merchans
                           where p.MerchandiseStatus.Equals(1) && p.merchandiseType == "S" && p.branch == branch
                           select p.merchandiseName);

            ddlNameOfSuplementSell.DataSource = SupName;
            ddlNameOfSuplementSell.DataBind();
            ddlNameOfSuplementSell.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        protected string validateSell()
        {
            ddlNameOfSuplementSell.Style.Remove("border-color");
            ddlCustomerSell.Style.Remove("border-color");
            txtTempCustomerName.Style.Remove("border-color");
            txtDateSell.Style.Remove("border-color");
            txtQuantitySell.Style.Remove("border-color");
            txtReceiptNo.Style.Remove("border-color");
            if (ddlNameOfSuplementSell.SelectedIndex == 0)
            {
                ddlNameOfSuplementSell.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Please Select Suplement";
            }
            else if (lblQuantity.InnerText == "0")
            {
                return "Selected Supplement is Out of Stock";
            }
            else if (string.IsNullOrEmpty(txtTempCustomerName.Text) && ddlCustomerSell.SelectedIndex == 0)
            {
                ddlCustomerSell.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                txtTempCustomerName.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Please Select Customer";
            }
            else if (chkPaid.Checked && (string.IsNullOrEmpty(txtDateSell.Text) || txtDateSell.Text== "1/1/0001"))
            {
                txtDateSell.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Date is Invalid or should not be empty";
            }
            else if (string.IsNullOrEmpty(txtQuantitySell.Text))
            {
                txtQuantitySell.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Quantity should not be empty";
            }
            else if(ddlPaymentMethod.SelectedValue == "3" && txtChequeNumber.Text=="" && txtBankName.Text=="")
            {
                return "Bank Name and Cheque number should not be blank";
            }
            else if (ddlPaymentMethod.SelectedValue == "4" && txtReferenceId.Text == "")
            {
                return "Reference ID should not be blank";
            }
            else if (chkPaid.Checked && string.IsNullOrEmpty(txtReceiptNo.Text))
            {
                txtReceiptNo.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Receipt No should not be empty";
            }
            else
                return "";
        }
        protected string UpdateMerchandiseQuantity(int q, string itemType)
        {
            var item = (from p in db.Merchans
                        where p.merchandiseName.Equals(itemType) && p.branch == splitUser
                        select p).SingleOrDefault();
            if (item.merchandiseQuantity >= q)
            {
                item.merchandiseQuantity -= q;
                //db.SubmitChanges();
                return "";
            }
            else
            {
                return item.merchandiseName + " not available right now"; ;
            }
        }
        protected void btnAddSuplementsSell_Click(object sender, EventArgs e)
        {
            try
            {
                var error = validateSell();
                if(error=="")
                {
                    if (UpdateMerchandiseQuantity(Convert.ToInt32(txtQuantitySell.Text), ddlNameOfSuplementSell.SelectedItem.Text) != "")
                    {
                        lblInfo.ForeColor = ColorTranslator.FromHtml("#ff0000");
                        lblInfo.Text = UpdateMerchandiseQuantity(Convert.ToInt32(txtQuantitySell.Text), ddlNameOfSuplementSell.SelectedItem.Text);
                    }
                    else
                    {
                        SuplementSelling sItem = new SuplementSelling();
                        ComissionPaymentLog inf = new ComissionPaymentLog();
                        sItem.nameOfSuplement_Sell = ddlNameOfSuplementSell.SelectedItem.Text;
                        
                        if (ddlCustomerSell.SelectedItem.Text != "--Select--")
                            sItem.customer_Sell = ddlCustomerSell.SelectedItem.Text;
                        else
                            sItem.customer_Sell = txtTempCustomerName.Text;

                        try
                        {
                            sItem.customerIdSell = ddlCustomerIdSell.SelectedItem.Text;
                        }
                        catch (Exception)
                        {

                        }

                        sItem.branch = txtBranch.Text;
                        sItem.discountCode = txtDiscountCode.Text;
                        sItem.quantity_Sell = Convert.ToDouble(txtQuantitySell.Text);
                        sItem.perPrice_Sell = Convert.ToDouble(txtPerPriceSell.Text);
                        sItem.totalPrice_Sell = Convert.ToDouble(txtTotalPriceSell.Text);
                        sItem.discount_Sell = Convert.ToDouble(txtDiscountSell.Text);
                        sItem.finalPrice_Sell = Convert.ToDouble(txtFinalPriceSell.Text);
                        sItem.created = DateTime.Now;
                        sItem.isPaidSuplementSell = chkPaid.Checked;

                        sItem.paymentMethod = ddlPaymentMethod.SelectedItem.Text;
                        sItem.bank = txtBankName.Text;
                        sItem.chequeNumber = txtChequeNumber.Text;
                        sItem.referenceId = txtReferenceId.Text;

                        if (chkPaid.Checked)
                        {
                            sItem.date_Sell = Convert.ToDateTime(NepaliDateService.NepToEng(txtDateSell.Text));
                        }
                        sItem.receiptNo = txtStatic.Text + "-" + txtReceiptNo.Text;
                        db.SuplementSellings.InsertOnSubmit(sItem);
                        if (chkPaid.Checked)
                        {
                            var staffInfo = (from p in db.Staffs
                                             where p.discountCode == txtDiscountCode.Text.Trim()
                                             select p).SingleOrDefault();
                            if (staffInfo != null)
                            {
                                decimal a = Convert.ToDecimal(staffInfo.commission) / 100;
                                var b = (Convert.ToInt32(txtFinalPriceSell.Text));
                                var cc = a * b;

                                inf.date = DateTime.Now;
                                inf.branch = txtBranch.Text;
                                inf.name = staffInfo.staffName;
                                inf.commissionFor = staffInfo.staffCatagory;
                                inf.discountCode = txtDiscountCode.Text;
                                inf.comission = cc;
                                try
                                {
                                    inf.memberId = ddlCustomerIdSell.SelectedItem.Text;
                                    inf.memberName = ddlCustomerSell.SelectedItem.Text;
                                }
                                catch (Exception)
                                {
                                    inf.memberName = txtTempCustomerName.Text;
                                }
                                inf.status = false;
                                db.ComissionPaymentLogs.InsertOnSubmit(inf);
                            }
                        }

                        db.SubmitChanges();
                        lblInfo.Text = "Successfully Inserted Data, Now Redirecting..";
                        lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('SellSupplements.aspx') }, 500);", true);
                    }
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
            db.Dispose();
        }
        protected void btnEditSuplementsSell_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(hidSnNo.Value);
            if (validateSell() == "")
            {
                SuplementSelling sItem = (from c in db.SuplementSellings
                                          where c.suplementSellingId == id
                                          select c).SingleOrDefault();
                ComissionPaymentLog inf = new ComissionPaymentLog();
                sItem.nameOfSuplement_Sell = ddlNameOfSuplementSell.SelectedItem.Text;
                
                if (ddlCustomerSell.SelectedItem.Text != "--Select--")
                    sItem.customer_Sell = ddlCustomerSell.SelectedItem.Text;
                else
                    sItem.customer_Sell = txtTempCustomerName.Text;

                try
                {
                    sItem.customerIdSell = ddlCustomerIdSell.SelectedItem.Text;
                }
                catch (Exception)
                {

                }

                sItem.branch = txtBranch.Text;
                sItem.discountCode = txtDiscountCode.Text;
                sItem.quantity_Sell = Convert.ToDouble(txtQuantitySell.Text);
                sItem.perPrice_Sell = Convert.ToDouble(txtPerPriceSell.Text);
                sItem.totalPrice_Sell = Convert.ToDouble(txtTotalPriceSell.Text);
                sItem.discount_Sell = Convert.ToDouble(txtDiscountSell.Text);
                sItem.finalPrice_Sell = Convert.ToDouble(txtFinalPriceSell.Text);
                sItem.receiptNo = txtStatic.Text + "-" + txtReceiptNo.Text;
                
                sItem.paymentMethod = ddlPaymentMethod.SelectedItem.Text;
                sItem.bank = txtBankName.Text;
                sItem.chequeNumber = txtChequeNumber.Text;
                sItem.referenceId = txtReferenceId.Text;

                if (sItem.isPaidSuplementSell == false && txtDiscountCode.Text != "")
                {

                    var staffInfo = (from p in db.Staffs
                                     where p.discountCode == txtDiscountCode.Text.Trim()
                                     select p).SingleOrDefault();

                    if (staffInfo != null)
                    {
                        decimal a = Convert.ToDecimal(staffInfo.commission) / 100;
                        var b = (Convert.ToInt32(txtFinalPriceSell.Text));
                        var cc = a * b;

                        inf.date = DateTime.Now;
                        inf.name = staffInfo.staffName;
                        inf.commissionFor = staffInfo.staffCatagory;
                        inf.discountCode = txtDiscountCode.Text;
                        inf.comission = cc;
                        inf.branch = txtBranch.Text;
                        inf.memberId = ddlCustomerIdSell.SelectedItem.Text;
                        inf.memberName = ddlCustomerSell.SelectedItem.Text;
                        inf.status = false;
                        db.ComissionPaymentLogs.InsertOnSubmit(inf);
                    }
                }

                sItem.isPaidSuplementSell = chkPaid.Checked;
                if (chkPaid.Checked)
                {
                    sItem.date_Sell =  Convert.ToDateTime(NepaliDateService.NepToEng(txtDateSell.Text));
                }

                db.SubmitChanges();
                lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                lblInfo.Text = "Successfully Updated Data, Now Redirecting..";
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('SellSupplements.aspx') }, 500);", true);
                
            }
            else
            {
                //Label lbl = this.Master.FindControl("lblPopupErrorr") as Label;
                //lblPopupErrorr.Text = validateSell();
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorModal", "$('#errorModal').modal();", true);
                lblInfo.ForeColor = ColorTranslator.FromHtml("#f70e0e");
                lblInfo.Text = validateSell();
                return;
            }
            db.Dispose();
        }
        protected void ddlCustomerSell_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCustomerIdSell.Items.Clear();
            var customerId = (from p in db.MemberInformations
                              where p.fullname == ddlCustomerSell.SelectedItem.ToString()
                              select p.memberId);

            List<String> customerIdList = customerId.ToList();
            for (int i = 0; i < customerIdList.Count; i++)
            {
                ddlCustomerIdSell.Items.Add(customerIdList[i].ToString());
            }
            if(ddlCustomerSell.SelectedItem.Text!="--Select--")
                txtTempCustomerName.Enabled = false;
            else
                txtTempCustomerName.Enabled = true;
            db.Dispose();
        }
        //protected void txtQuantitySell_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        txtTotalPriceSell.Text = ((Convert.ToInt32(txtQuantitySell.Text)) * (Convert.ToInt32(txtPerPriceSell.Text))).ToString();
        //        txtFinalPriceSell.Text = ((Convert.ToDouble(txtTotalPriceSell.Text)) - (Convert.ToDouble(txtDiscountSell.Text))).ToString();
        //        btnAddSuplementsSell.Enabled = true;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //protected void txtPerPriceSell_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        txtTotalPriceSell.Text = ((Convert.ToInt32(txtQuantitySell.Text)) * (Convert.ToInt32(txtPerPriceSell.Text))).ToString();
        //        txtFinalPriceSell.Text = ((Convert.ToDouble(txtTotalPriceSell.Text)) - (Convert.ToDouble(txtDiscountSell.Text))).ToString();
        //        btnAddSuplementsSell.Enabled = true;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //protected void txtDiscountSell_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        txtFinalPriceSell.Text = (Convert.ToInt32(txtTotalPriceSell.Text) - Convert.ToInt32(txtDiscountSell.Text)).ToString();
        //        string tempId = Request.QueryString["ID"].ToString();
        //        if (string.IsNullOrEmpty(tempId))
        //            btnAddSuplementsSell.Enabled = true;
        //        else
        //            btnAddSuplementsSell.Enabled = false;
        //    }
        //    catch (Exception)
        //    {
        //        btnAddSuplementsSell.Enabled = true;
        //    }
        //}
        protected void loadData()
        {
            try
            {
                string tempId = Request.QueryString["ID"].ToString();

                var editQuery = (from c in db.SuplementSellings
                                 where c.suplementSellingId == Convert.ToInt32(tempId)
                                 select c).SingleOrDefault();
                ddlNameOfSuplementSell.SelectedIndex = ddlNameOfSuplementSell.Items.IndexOf(ddlNameOfSuplementSell.Items.FindByText(editQuery.nameOfSuplement_Sell));
                txtDateSell.Text = editQuery.date_Sell==null?"": NepaliDateService.EngToNep(Convert.ToDateTime(editQuery.date_Sell)).ToString();
                
                try
                {
                    ddlCustomerSell.SelectedIndex = ddlCustomerSell.Items.IndexOf(ddlCustomerSell.Items.FindByText(editQuery.customer_Sell));
                    ddlCustomerIdSell.Items.Add(editQuery.customerIdSell);
                    if (ddlCustomerSell.SelectedIndex == 0)
                    {
                        txtTempCustomerName.Text = editQuery.customer_Sell;
                    }
                }
                catch
                {

                }
                txtBranch.Text = editQuery.branch;
                txtQuantitySell.Text = editQuery.quantity_Sell.ToString();
                txtPerPriceSell.Text = editQuery.perPrice_Sell.ToString();
                txtTotalPriceSell.Text = editQuery.totalPrice_Sell.ToString();
                txtDiscountSell.Text = editQuery.discount_Sell.ToString();
                txtFinalPriceSell.Text = editQuery.finalPrice_Sell.ToString();
                txtDiscountCode.Text = editQuery.discountCode;
                hidSnNo.Value = Convert.ToString(editQuery.suplementSellingId);
                chkPaid.Checked = editQuery.isPaidSuplementSell.Value;
                ddlPaymentMethod.SelectedIndex = ddlPaymentMethod.Items.IndexOf(ddlPaymentMethod.Items.FindByText(editQuery.paymentMethod));
                txtReceiptNo.Text = editQuery.receiptNo.Split('-').Length == 2 ? editQuery.receiptNo.Split('-')[1] : editQuery.receiptNo.Split('-')[0];
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
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
            }
            db.Dispose();
        }
        protected void disableField(string roleId)
        {
            if (roleId == "3")
            {
                //ddlNameOfSuplementSell.Enabled = false;
                //ddlCustomerSell.Enabled = false;
                //ddlCustomerIdSell.Enabled = false;
                //txtTempCustomerName.Enabled = false;
                //txtDateSell.Enabled = false;
                //txtQuantitySell.Enabled = false;
                //txtDiscountCode.Enabled = false;
                //txtDiscountSell.Enabled = false;
                //txtReceiptNo.Enabled = false;
                //chkPaid.Enabled = false;
                //btnEditSuplementsSell.Enabled = false;
                pnl1.Enabled = false;
            }
            else
            {
                btnEditSuplementsSell.Enabled = true;
            }
        }
        protected void clear()
        {
            ddlNameOfSuplementSell.ClearSelection();
            txtDateSell.Text = "";
            ddlCustomerSell.Items.Clear();
            ddlCustomerIdSell.Items.Clear();
            txtQuantitySell.Text = "";
            txtPerPriceSell.Text = "";
            txtTotalPriceSell.Text = "";
            txtDiscountSell.Text = "";
            txtFinalPriceSell.Text = "";
        }
        protected void deleteSellSuplement(string id)
        {
            var supplementSell = (from s in db.SuplementSellings
                                  where s.suplementSellingId.Equals(id)
                                  select s).SingleOrDefault();
            db.SuplementSellings.DeleteOnSubmit(supplementSell);
            db.SubmitChanges();
            db.Dispose();
        }
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["ID"].ToString();
            deleteSellSuplement(id);
        }
        protected void chkPaid_CheckedChanged(object sender, EventArgs e)
        {
            txtDateSell.Enabled = chkPaid.Checked ? true : false;
        }
        protected void ddlNameOfSuplementSell_SelectedIndexChanged(object sender, EventArgs e)
        {
            var merchanItem = (from p in db.Merchans
                               where p.merchandiseName == ddlNameOfSuplementSell.SelectedItem.ToString() && p.branch == splitUser
                               select p).SingleOrDefault();
            if (merchanItem != null)
            {
                txtPerPriceSell.Text = merchanItem.merchandisePrice.ToString();
                lblQuantity.InnerText = merchanItem.merchandiseQuantity.ToString();
            }
            else
                txtPerPriceSell.Text = "";

            db.Dispose();
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