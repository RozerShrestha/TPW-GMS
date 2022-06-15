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
    public partial class SellItems : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!Page.IsPostBack)
            {
                hidUserLogin.Value = splitUser;
                txtBranch.Text = splitUser;
                LoadItemType();
                loadCustomerDropDown();
                loadInfo();
                try{
                    string key = Request.QueryString["key"].ToString();
                    if (key == "edit"){
                        btnAddSellItem.Enabled = false;
                        LoadData();
                    }
                    else if (key == "delete"){
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "deleteConfirmModal", "$('#deleteConfirmModal').modal();", true);
                    }
                }
                catch (Exception){

                }
                db.Dispose();

            }
            txtTotalPrice.Attributes.Add("readonly", "readonly");
            txtFinalPrice.Attributes.Add("readonly", "readonly");

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
        protected void loadCustomerDropDown()
        {
            var name = (from p in db.MemberInformations
                        where p.branch == splitUser
                        select p.fullname);
            ddlCustomerName.DataSource = name;
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        protected void loadInfo()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var item = db.ExtraInformations.SingleOrDefault();
                txtStatic.Text = item.currentNepaliDate + splitUser;
            }
        }
        protected string validateItemSell()
        {
            ddlItemType.Style.Remove("border-color");
            ddlCustomerName.Style.Remove("border-color");
            txtDateSellItem.Style.Remove("border-color");
            txtQuantity.Style.Remove("border-color");
            txtReceiptNo.Style.Remove("border-color");
            
            if (ddlItemType.SelectedIndex == 0)
            {
                ddlItemType.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Item Type should not be Empty";
            }
            else if (ddlCustomerName.SelectedIndex == 0)
            {
                ddlCustomerName.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Please Select Customer Name";
            }
            else if (chkPaid.Checked && string.IsNullOrEmpty(txtDateSellItem.Text))
            {
                txtDateSellItem.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Date paid should not be empty";
            }
            else if (string.IsNullOrEmpty(txtQuantity.Text))
            {
                txtQuantity.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Quantity should not be empty";
            }
            else if (string.IsNullOrEmpty(txtReceiptNo.Text))
            {
                txtReceiptNo.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                return "Receipt No should not be empty";
            }
            else
            {
                return "";
            }
        }
        protected void deleteItemSell(string id)
        {
            var item = (from s in db.SellItems
                        where s.SellItemId.Equals(id)
                        select s).SingleOrDefault();
            db.SellItems.DeleteOnSubmit(item);
            db.SubmitChanges();
            db.Dispose();
        }
        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["ID"].ToString();
            deleteItemSell(id);

        }
        protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCustomerId.Items.Clear();
            var customerId = (from p in db.MemberInformations
                              where p.fullname == ddlCustomerName.SelectedItem.ToString()
                              select p.memberId);

            List<String> customerIdList = customerId.ToList();
            for (int i = 0; i < customerIdList.Count; i++)
            {
                ddlCustomerId.Items.Add(customerIdList[i].ToString());
            }
            db.Dispose();
        }
        protected void LoadData()
        {
            try
            {
                string tempId = Request.QueryString["ID"].ToString();
                var editQuery = (from c in db.SellItems
                                 where c.SellItemId == Convert.ToInt32(tempId)
                                 select c).SingleOrDefault();
                ddlItemType.SelectedIndex = ddlItemType.Items.IndexOf(ddlItemType.Items.FindByText(editQuery.itemTypeSell));
                ddlCustomerName.SelectedIndex = ddlCustomerName.Items.IndexOf(ddlCustomerName.Items.FindByText(editQuery.memberName));
                ddlCustomerId.Items.Add(editQuery.memberId);
                txtBranch.Text = editQuery.branch;
                if (editQuery.isPaidItemSell??false){
                    txtDateSellItem.Text = NepaliDateService.EngToNep(Convert.ToDateTime(editQuery.dateItemSell)).ToString();
                }
                txtQuantity.Text = editQuery.quantityItemSell.ToString();
                txtPerPrice.Text = editQuery.perPriceItemSell.ToString();
                txtTotalPrice.Text = editQuery.totalPriceItemSell.ToString();
                txtDiscount.Text = editQuery.discountItemSell.ToString();
                txtFinalPrice.Text = editQuery.finalPriceItemSell.ToString();
                txtReceiptNo.Text = editQuery.receiptNo.Split('-').Length == 2 ? editQuery.receiptNo.Split('-')[1] : editQuery.receiptNo.Split('-')[0];
                chkPaid.Checked = editQuery.isPaidItemSell.Value;
                hidSnNo.Value = Convert.ToString(editQuery.SellItemId);
                btnEditSellItem.Enabled = true;
                ddlItemType.Enabled = false;
                ddlCustomerName.Enabled = false;
                ddlCustomerId.Enabled = false;
                txtDateSellItem.Enabled = false;
                txtQuantity.Enabled = false;
                txtDiscount.Enabled = false;
                txtReceiptNo.Enabled = false;
                ddlPaymentMethod.SelectedIndex= ddlPaymentMethod.Items.IndexOf(ddlPaymentMethod.Items.FindByText(editQuery.paymentMethod));
            }
            catch (Exception)
            {

            }
            db.Dispose();
        }
        protected void LoadItemType()
        {
            var name = (from p in db.Merchans
                        where p.merchandiseType == "M" && p.MerchandiseStatus.Equals(1) && p.branch == splitUser
                        select p.merchandiseName);
            List<String> merchList = name.ToList();
            for (int i = 0; i < merchList.Count; i++)
            {
                ddlItemType.Items.Add(merchList[i].ToString());
            }
            ddlItemType.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        protected string UpdateMerchandiseQuantity(int q, string itemType)
        {
            var item = (from p in db.Merchans
                        where p.merchandiseName.Equals(itemType) && p.branch == hidUserLogin.Value
                        select p).SingleOrDefault();
            if (item.merchandiseQuantity > 0)
            {
                item.merchandiseQuantity -= q;
                db.SubmitChanges();
                return "";
            }
            else
            {
                return item.merchandiseName + " not available right now"; ;
            }
        }
        protected void btnAddSellItem_Click(object sender, EventArgs e)
        {
            var error = validateItemSell();
            if (error == "")
            {
                if (UpdateMerchandiseQuantity(Convert.ToInt32(txtQuantity.Text), ddlItemType.SelectedItem.Text) != "")
                {
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#ff0000");
                    lblInfo.Text = UpdateMerchandiseQuantity(Convert.ToInt32(txtQuantity.Text), ddlItemType.SelectedItem.Text);
                }
                else
                {
                    DateTime? dt = null;
                    
                    SellItem item = new SellItem();
                    item.itemTypeSell = ddlItemType.SelectedItem.Text;
                    item.memberName = ddlCustomerName.SelectedItem.Text;
                    item.memberId = ddlCustomerId.SelectedItem.Text;
                    item.branch = txtBranch.Text;
                    item.created = DateTime.Now;
                    item.quantityItemSell = Convert.ToInt32(txtQuantity.Text);
                    item.perPriceItemSell = Convert.ToInt32(txtPerPrice.Text);
                    item.totalPriceItemSell = Convert.ToInt32(txtTotalPrice.Text);
                    item.discountItemSell = Convert.ToInt32(txtDiscount.Text);
                    item.finalPriceItemSell = Convert.ToInt32(txtFinalPrice.Text);
                    item.receiptNo = txtStatic.Text + "-" + txtReceiptNo.Text;
                    item.isPaidItemSell = chkPaid.Checked;
                    item.dateItemSell = chkPaid.Checked ? Convert.ToDateTime(txtDateSellItem.Text) : dt;
                    if (chkPaid.Checked)
                    {
                        item.dateItemSell = NepaliDateService.NepToEng(txtDateSellItem.Text);
                    }
                    item.paymentMethod= ddlPaymentMethod.SelectedItem.Text;
                    db.SellItems.InsertOnSubmit(item);
                    db.SubmitChanges();
                    lblInfo.Text = "Successfully inserted Data, Now Redirecting..";
                    lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('SellItems.aspx') }, 1000);", true);
                    //Clear();
                }
            }
            else
            {
                lblInfo.Text = validateItemSell();
                lblInfo.ForeColor = ColorTranslator.FromHtml("#ff0000");
                //lblPopupErrorr.Text = validateItemSell();
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorModal", "$('#errorModal').modal();", true);
                //return;
            }
            db.Dispose();
        }

        protected void ddlPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void chkPaid_CheckedChanged(object sender, EventArgs e)
        {
            txtDateSellItem.Enabled = chkPaid.Checked ? true : false;
        }

        protected void btnEditSellItem_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(hidSnNo.Value);
            if (validateItemSell() == "")
            {
                var item = (from c in db.SellItems
                            where c.SellItemId == id
                            select c).SingleOrDefault();
                item.itemTypeSell = ddlItemType.SelectedItem.Text;
                item.memberName = ddlCustomerName.SelectedItem.Text;
                item.memberId = ddlCustomerId.SelectedItem.Text;
                item.branch = txtBranch.Text;
                
                item.quantityItemSell = Convert.ToInt32(txtQuantity.Text);
                item.perPriceItemSell = Convert.ToInt32(txtPerPrice.Text);
                item.totalPriceItemSell = Convert.ToInt32(txtTotalPrice.Text);
                item.discountItemSell = Convert.ToInt32(txtDiscount.Text);
                item.finalPriceItemSell = Convert.ToInt32(txtFinalPrice.Text);
                item.receiptNo = txtStatic.Text + "-" + txtReceiptNo.Text;
                item.isPaidItemSell = chkPaid.Checked;
                if (chkPaid.Checked)
                {
                    item.dateItemSell = NepaliDateService.NepToEng(txtDateSellItem.Text);
                }
                item.paymentMethod = ddlPaymentMethod.SelectedItem.Text;
                db.SubmitChanges();
                lblInfo.Text = "Successfully Updated Data, Now Redirecting..";
                lblInfo.ForeColor = ColorTranslator.FromHtml("#037203");
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('SellItems.aspx') }, 1000);", true);
                //Clear();
            }
            else
            {
                lblInfo.Text = validateItemSell();
            }
            db.Dispose();
        }
        protected void ddlItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var merchanItem = (from p in db.Merchans
                               where p.merchandiseName == ddlItemType.SelectedItem.ToString() && p.branch == hidUserLogin.Value
                               select p).SingleOrDefault();

            txtPerPrice.Text = merchanItem.merchandisePrice.ToString();
            lblQuantity.InnerText = merchanItem.merchandiseQuantity.ToString() + " Remaining";
            db.Dispose();
        }
    }
}