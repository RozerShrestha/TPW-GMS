<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SellSupplements.aspx.cs" Inherits="TPW_GMS.SellSupplements" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <style type="text/css">
            .showHide {
                cursor: pointer;
            }
        </style>  
        <script type="text/javascript">
            $(document).ready(function binddatatable() {
                $.ajax({
                    type: 'GET',
                    dataType: 'json',
                    url: '/api/GetAllSellSupplements',
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                    success: function (data) {
                        var datatableVariable = $('#SellSuplementTable').DataTable({
                            autoWidth:false,
                            'data': data.filter(function (item) {
                                if ('<%=hidUserLogin.Value.ToString()%>' == 'admin')
                                    return true;
                                else if (item['branch'] == '<%=hidUserLogin.Value.ToString()%>')
                                    return true;
                                else
                                    return false;
                            }),
                            rowCallback: function (row, data) {
                                if (data["isPaidSuplementSell"] == 0) {
                                    $(row).css("color", "red");
                                    $(row).addClass('highlight')
                                }

                            },
                            columns: [
                                {
                                    'data': 'id',
                                    render: function (data, type, row, meta) {
                                        return meta.row + meta.settings._iDisplayStart + 1;
                                    }
                                },
                                {
                                    'data': 'date_Sell',
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
                                { 'data': 'nameOfSuplement_Sell' },
                                { 'data': 'customerIdSell' },
                                { 'data': 'customer_Sell' },
                                { 'data': 'branch' },
                                { 'data': 'quantity_Sell' },
                                { 'data': 'perPrice_Sell' },
                                { 'data': 'totalPrice_Sell' },
                                { 'data': 'discount_Sell' },
                                { 'data': 'finalPrice_Sell' },
                                { 'data': 'receiptNo' },
                                { 'data': 'paymentMethod' },

                                {
                                    'data': null
                                },
                            ],
                            columnDefs: [{
                                data: "suplementSellingId",
                                className: "center",
                                targets: [-1], render: function (data, type, full, meta) {
                                    if ('<%=hidUserLogin.Value.ToString()%>' == 'admin') {
                                        return '<a href="SellSupplements.aspx?id=' + data.suplementSellingId + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a> &nbsp;&nbsp;&nbsp; <a href="SellSupplements.aspx?id=' + data.suplementSellingId + "&" + "key=delete" + '" class="editor_remove"><img src="Assets/Icon/delete.png" class="iconDelete" /></a>';
                                    }
                                    else {
                                        if (data.branch == '<%=hidUserLogin.Value.ToString()%>') {
                                            return '<a href="SellSupplements.aspx?id=' + data.suplementSellingId + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a>';
                                        }
                                        else {
                                            return '<a href="SellSupplements.aspx?id=' + data.suplementSellingId + "&" + "key=edit" + '" class="editAsset"></a>';
                                        }
                                    }
                                }
                            }],
                        });
                    }
                });
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
                if (GetParameterValues('key') == 'edit' || GetParameterValues('key') == 'delete') {
                    document.getElementById('ContentPlaceHolder1_txtDiscountCode').disabled = true;
                    document.getElementById('ContentPlaceHolder1_btnAddSuplementsSell').setAttribute('disabled', true);
                }
            });
            function checkDisocuntCode() {
                let code = $('#ContentPlaceHolder1_txtDiscountCode').val()
                axios({
                    url: `api/CheckDiscountCodeStatus?code=${code}`,
                    method: 'get',
                    dataType: "JSON",
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                    }).then(response => {
                        var response = response.data;
                        if (response == "valid") {
                            $("#ContentPlaceHolder1_lblInfo").text("Discount Code Valid");
                            calculate();
                        }
                        else {
                            $("#ContentPlaceHolder1_lblInfo").text("Discount Code Invalid");
                        }
                    }).catch(function (error) {
                        console.log(error)
                    })
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
                    var quantity = document.getElementById("ContentPlaceHolder1_txtQuantitySell").value;
                    var perPrice = document.getElementById("ContentPlaceHolder1_txtPerPriceSell").value;
                    var totalPrice = document.getElementById("ContentPlaceHolder1_txtTotalPriceSell");
                    var discount = document.getElementById("ContentPlaceHolder1_txtDiscountSell").value;
                    var finalPrice = document.getElementById("ContentPlaceHolder1_txtFinalPriceSell");
                    var discountCode = 0;
                    var discountCodeCheck = $('#ContentPlaceHolder1_lblInfo').text();;
                    if (discountCodeCheck == "Discount Code Valid") {
                        var discountCodeFull = document.getElementById("ContentPlaceHolder1_txtDiscountCode").value;
                        var discountCodeArray = discountCodeFull.split('$');
                        discountCode = discountCodeArray[1];
                    }

                    totalPrice.value = (quantity * perPrice) - discountCode / 100 * (quantity * perPrice);
                    if (GetParameterValues('key') != 'edit') {
                        document.getElementById('ContentPlaceHolder1_btnAddSuplementsSell').removeAttribute('disabled');
                    }
                    if (discount != "") {
                        finalPrice.value = Math.round(totalPrice.value - discount);

                    }
                }
            }
            catch (error) {

            }
        </script>          
    
    <asp:HiddenField ID="hidUserLogin" runat="server" />
    <asp:UpdatePanel ID="upnlSuplementSell" runat="server">    
        <Triggers>
          </Triggers>
        <ContentTemplate>
            <section class="content">
                <div class="row">
                    <div class="box box-info">
                        <div class="box-title">
                            <asp:HiddenField ID="hidHeader" runat="server" Value="Supplements Selling Records" />
                        </div>
                        <div class="box-body">
                            <asp:Panel ID="pnl1" runat="server">
                                <div class="row">
                                    <div class="col-md-2">
                                        Name of Supplement:<span class="asterik">*</span>
                                        <asp:DropDownList ID="ddlNameOfSuplementSell" runat="server" OnSelectedIndexChanged="ddlNameOfSuplementSell_SelectedIndexChanged" AutoPostBack="true" CssClass="select2Example form-control input-sm"></asp:DropDownList>
                                        <asp:HiddenField ID="hidSnNo" runat="server" />
                                    </div>
                                    <div class="col-md-2">
                                        TPW-Member Name:<span class="asterik">*</span>
                                        <asp:DropDownList ID="ddlCustomerSell" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomerSell_SelectedIndexChanged" runat="server" CssClass="select2Example form-control input-sm"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        Member Id:
                                        <asp:DropDownList ID="ddlCustomerIdSell" AutoPostBack="true" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        Temp Customer Name:
                                        <asp:TextBox ID="txtTempCustomerName" runat="server" CssClass="form-control input-sm" placeholder="Temporary Customer Name"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        Date:<span class="asterik">*</span>
                                        <asp:TextBox ID="txtDateSell" runat="server" Enabled="false" CssClass="form-control input-sm nepCalendar"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        Branch
                                        <asp:TextBox ID="txtBranch" runat="server" ReadOnly="true" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        Discount's Code:
                                        <asp:TextBox ID="txtDiscountCode" runat="server" onchange="javascript:checkDisocuntCode();" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        Quantity:<span class="asterik">*</span><label class="asterik" id="lblQuantity" style="font-size: 12px" runat="server"></label>
                                        <asp:TextBox ID="txtQuantitySell" runat="server" onkeyup="javascript:calculate();" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        Per Price:
                                        <asp:TextBox ID="txtPerPriceSell" runat="server" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        Total Price:
                                        <asp:TextBox ID="txtTotalPriceSell" runat="server" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        Discount:<span class="asterik">*</span>
                                        <asp:TextBox ID="txtDiscountSell" runat="server" onkeyup="javascript:calculate();" Text="0" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        Final Price:
                                        <asp:TextBox ID="txtFinalPriceSell" runat="server" Enabled="false" Style="font-weight: bold" CssClass="form-control input-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
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
                                        <asp:Panel ID="pnlEBanking" runat="server" Visible="false">
                                            Reference ID
                                         <asp:TextBox ID="txtReferenceId" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                        </asp:Panel>
                                    </div>
                                    <div class="col-md-2">
                                        Receipt No<span class="asterik">*</span><br />
                                        <%--<asp:TextBox ID="txtReceiptNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>--%>
                                        <div class="col-md-6" style="padding-left: 0px; padding-right: 0px;">
                                            <asp:TextBox ID="txtStatic" CssClass="form-control input-sm" ClientIDMode="Static" runat="server" disabled></asp:TextBox>
                                        </div>
                                        <div class="col-md-6" style="padding-left: 0px; padding-right: 0px;">
                                            <asp:TextBox ID="txtReceiptNo" CssClass=" form-control input-sm" ClientIDMode="Static" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        Paid<br />
                                        <asp:CheckBox ID="chkPaid" OnCheckedChanged="chkPaid_CheckedChanged" AutoPostBack="true" runat="server" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Button ID="btnAddSuplementsSell" runat="server" Enabled="true" Text="Submit" CssClass="btn btn-sm btn-success" OnClick="btnAddSuplementsSell_Click" />
                                        <asp:Button ID="btnEditSuplementsSell" runat="server" Enabled="false" Text="Edit" CssClass="btn btn-sm btn-danger" OnClick="btnEditSuplementsSell_Click" />
                                        <asp:Label ID="lblInfo" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>  

    <div class="row">
        <div class="col-xs-12">
            <div class="box box-solid">
                <div class="box-body">
                    <div class="table-responsive">
                        <table id="SellSuplementTable" style="font-size: 12px" class="table table-bordered table-hover">
                            <thead>
                                <tr class="header">
                                    <th>Sn</th>
                                    <th width="69px">Date</th>
                                    <th>Name Of Suplement</th>
                                    <th>Customer Id</th>
                                    <th>Customer Name</th>
                                    <th>Branch</th>
                                    <th>Qty</th>
                                    <th>Per Price</th>
                                    <th>Total Price</th>
                                    <th>Discount</th>
                                    <th>Final Price</th>
                                    <th>Receipt No</th>
                                    <th>Payment Method</th>

                                    <th>Action</th>
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
                    <asp:Button id="btnConfirmDelete" runat="server" Text="Yes" OnClick="btnConfirmDelete_Click" class="btn btn-danger btn-sm" />
                    <button type="button" class="btn btn-success btn-sm" data-dismiss="modal">No</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
