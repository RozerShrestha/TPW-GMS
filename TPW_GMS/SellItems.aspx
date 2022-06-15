<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SellItems.aspx.cs" Inherits="TPW_GMS.SellItems" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
            $(document).ready(function binddatatable() {
                $.ajax({
                    type: 'GET',
                    dataType: 'json',
                    url: '/api/GetSellItems',
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                    success: function (data) {
                        var datatableVariable = $('#ItemSellsTable').DataTable({
                            autoWidth: false,
                            'data': data.filter(function (item) {
                                if ('<%=hidUserLogin.Value.ToString()%>' == 'admin')
                                    return true;
                                else if (item['branch'] == '<%=hidUserLogin.Value.ToString()%>')
                                    return true;
                                else
                                    return false;
                            }),
                            rowCallback: function (row, data) {
                                if (data["isPaidItemSell"] == 0) {
                                    $(row).css("color", "red");
                                    $(row).addClass('highlight')
                                }
                            },
                            columnDefs: [
                                {
                                    targets: [10],
                                    render: function (data, type, row) {
                                        return data == '1' ? 'P' : 'NP'
                                       
                                    }
                                }
                            ],
                            columns: [
                                {
                                    'data': 'id',
                                    render: function (data, type, row, meta) {
                                        return meta.row + meta.settings._iDisplayStart + 1;
                                    }
                                },
                                {
                                     "data": "dateItemSell",
                                    "render": function (data) {
                                        try {
                                            var dat = data != null ? (data.split('T')[0]).split('-').join('/') : '';
                                            let objEng = NepaliFunctions.ConvertToDateObject(dat, "YYYY/MM/DD");
                                            let objNep = NepaliFunctions.AD2BS(objEng);
                                            let dt = NepaliFunctions.ConvertDateFormat(objNep, "YYYY/MM/DD");

                                            return dt;
                                        } catch (e) {
                                            return '';
                                        }
                                    }
                                },
                                { 'data': 'branch' },
                                { 'data': 'itemTypeSell' },
                                { 'data': 'memberId' },
                                { 'data': 'memberName' },
                                { 'data': 'quantityItemSell' },
                                { 'data': 'perPriceItemSell' },
                                { 'data': 'totalPriceItemSell' },
                                { 'data': 'discountItemSell' },
                                { 'data': 'finalPriceItemSell' },
                                { 'data': 'receiptNo' },
                                {
                                    'data': null
                                },
                            ],
                            columnDefs: [{
                                data: "SellItemId",
                                className: "center",
                                targets: [-1], render: function (data, type, full, meta) {
                                    if ('<%=hidUserLogin.Value.ToString()%>' == 'admin') {
                                        return '<a href="SellItems.aspx?id=' + data.SellItemId + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a> &nbsp;&nbsp;&nbsp; <a href="SellItems.aspx?id=' + data.SellItemId + "&" + "key=delete" + '" class="editor_remove"><img src="Assets/Icon/delete.png" class="iconDelete" /></a>';
                                    }
                                    else {
                                        if (data.branch == '<%=hidUserLogin.Value.ToString()%>') {
                                            return '<a href="SellItems.aspx?id=' + data.SellItemId + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a>';
                                        }
                                        else {
                                            return '<a href="SellItems.aspx?id=' + data.SellItemId + "&" + "key=edit" + '" class="editAsset"></a>';
                                        }
                                    }
                                }
                            }],
                        });                       
                    }
                });
                
         });

         function disableField()
         {
             document.getElementById("ContentPlaceHolder1_txtTotalPrice").disabled = true;
         }

         try {  
             var key = GetParameterValues('key');
             function GetParameterValues(param) {
                 var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
                 for (var i = 0; i < url.length; i++) {
                     var urlparam = url[i].split('=');
                     if (urlparam[0] == param) {
                         return urlparam[1];
                     }
                 }
             }
             function calculate() {
                 var quantity = document.getElementById("ContentPlaceHolder1_txtQuantity").value;
                 var perPrice = document.getElementById("ContentPlaceHolder1_txtPerPrice").value;

                 var totalPrice = document.getElementById('<%= txtTotalPrice.ClientID %>');
                 var totalPrice = document.getElementById("ContentPlaceHolder1_txtTotalPrice");

                 var discount = document.getElementById("ContentPlaceHolder1_txtDiscount").value;
                 var finalPrice = document.getElementById("ContentPlaceHolder1_txtFinalPrice");

                 totalPrice.value = quantity * perPrice;
                 if (GetParameterValues('key') != 'edit') {
                    document.getElementById('ContentPlaceHolder1_btnAddSellItem').removeAttribute('disabled');
                 }
                 if (discount != "") {
                    finalPrice.value = totalPrice.value - discount;                        
                 }
             }
         }
         catch(error){
         
         }

     </script>    
    <asp:UpdatePanel ID="pnlSellItem" runat="server">
         <Triggers>         
          </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="box box-info">
                    <div class="box-title">
                        <asp:HiddenField ID="hidHeader" runat="server" Value="Item Selling Record" />
                        <asp:HiddenField ID="hidSnNo" runat="server" />
                    </div>
                    <div class="box-body">
                        <div class="col-xs-12">
                            <div class="col-sm-2">
                                Item Type<span class="asterik">*</span>
                                <asp:DropDownList ID="ddlItemType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlItemType_SelectedIndexChanged" CssClass="form-control input-sm select2Example"></asp:DropDownList>
                            </div>
                            <div class="col-sm-2">
                                Customer Name<span class="asterik">*</span>
                                <asp:DropDownList ID="ddlCustomerName" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomerName_SelectedIndexChanged" runat="server" CssClass="select2Example form-control input-sm" />
                            </div>
                            <div class="col-sm-2">
                                Customer Id
                                <asp:DropDownList ID="ddlCustomerId" AutoPostBack="true" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                            </div>
                            <div class="col-sm-2">
                                Branch
                                <asp:TextBox ID="txtBranch" runat="server" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                Item Selling Date
                                <asp:TextBox ID="txtDateSellItem" Enabled="false" runat="server" CssClass="form-control input-sm nepCalendar" />
                            </div>
                            <div class="col-sm-2">
                                Quantity<span class="asterik">*</span><label class="asterik" id="lblQuantity" runat="server"></label>
                                <asp:TextBox ID="txtQuantity" onkeyup="javascript:calculate();" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-1">
                                Per Price
                                <asp:TextBox ID="txtPerPrice" onkeyup="javascript:calculate();" ReadOnly="true" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-1">
                                Total Price
                                <asp:TextBox ID="txtTotalPrice" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-2">
                                Discount<span class="asterik">*</span>
                                <asp:TextBox ID="txtDiscount" Text="0" onkeyup="javascript:calculate();" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-2">
                                Final Price  
                                <asp:TextBox ID="txtFinalPrice" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-2">
                                Payment Method
                                    <asp:DropDownList ID="ddlPaymentMethod" runat="server" OnSelectedIndexChanged="ddlPaymentMethod_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control input-sm">
                                        <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Cash"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Card"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Cheque"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="E-Banking"></asp:ListItem>
                                    </asp:DropDownList>
                                <asp:Panel ID="pnlCheque" runat="server" Visible="false">
                                    Bank
                                        <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                    Cheque Number
                                        <asp:TextBox ID="txtChequeNumber" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </asp:Panel>

                            </div>
                            <div class="col-sm-2">
                                Receipt No<span class="asterik">*</span><br />
                                <%--<asp:TextBox ID="txtReceiptNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>--%>
                                <div class="col-md-6" style="padding-left: 0px; padding-right: 0px;">
                                    <asp:TextBox ID="txtStatic" CssClass="form-control input-sm" ClientIDMode="Static" runat="server" disabled></asp:TextBox>
                                </div>
                                <div class="col-md-6" style="padding-left: 0px; padding-right: 0px;">
                                    <asp:TextBox ID="txtReceiptNo" CssClass=" form-control input-sm" ClientIDMode="Static" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                Paid<br />
                                <asp:CheckBox ID="chkPaid" runat="server" OnCheckedChanged="chkPaid_CheckedChanged" AutoPostBack="true" />
                            </div>
                            <div class="col-sm-2">
                            </div>
                            
                            <div class="col-sm-12" style="margin-top:10px">
                                <asp:Button ID="btnAddSellItem" runat="server" Text="Submit" CssClass="btn btn-sm btn-success" OnClick="btnAddSellItem_Click" />
                                <asp:Button ID="btnEditSellItem" runat="server" Enabled="false" Text="Edit" CssClass="btn btn-sm btn-danger btn-a" OnClick="btnEditSellItem_Click" />
                                <asp:Label ID="lblInfo" Style="margin-left: 4px" runat="server"></asp:Label>
                                </td>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
        <asp:HiddenField ID="hidUserLogin" runat="server" />
    <div class="row">
        <div class="col-xs-12">
            <div class="box box-solid">
                <div class="box-body">
                    <div class="table-responsive">
                        <table id="ItemSellsTable" style="font-size: 12px" class="table table-bordered table-hover">
                            <thead>
                                <tr class="header">
                                    <th>Sn.</th>
                                    <th>Date</th>
                                    <th>Branch</th>
                                    <th>Item Type</th>
                                    <th>Customer Id</th>
                                    <th>Customer Name</th>
                                    <th>Qty</th>
                                    <th>Per Price</th>
                                    <th>Total Price</th>
                                    <th>Discount</th>
                                    <th>Final Price</th>
                                    <th>Receipt No</th>
                                    <th width="88px">Action</th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="errorModal" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Error</h4>
                </div>
                <div class="modal-body">
                    Error has occured:
                    <asp:Label ID="lblPopupErrorr" runat="server" ForeColor="Red" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="deleteConfirmModal" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Delete Confirm</h4>
                </div>
                <div class="modal-body">
                    Are you sure want to delete??
                    <%--<asp:Label ID="Label1" runat="server" ForeColor="Red" />--%>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnConfirmDelete" runat="server" Text="Yes" OnClick="btnConfirmDelete_Click" class="btn btn-danger btn-sm" />
                    <button type="button" class="btn btn-success btn-sm" data-dismiss="modal">No</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
