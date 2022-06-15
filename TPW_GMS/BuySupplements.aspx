<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuySupplements.aspx.cs" Inherits="TPW_GMS.BuySupplements" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .showHide {
            cursor: pointer;
        }
    </style>
    <script type="text/javascript">           
        $(document).ready(function binddatatable() {
            $.ajax({
                type: "GET",
                dataType: "json",
                url: "/api/GetBuySupplements",
                headers: {
                    'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                },
                success: function (data) {
                    var datatableVariable = $('#suplementTable').DataTable({
                        autoWidth: false,
                        data: data,
                        rowCallback: function (row, data) {
                            if (data["status"] == 0) { //I'm assuming you're using object JSON/ajax, if not,
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
                                "data": "date",
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
                            { 'data': 'nameOfSuplement' },
                            { 'data': 'branch' },
                            { 'data': 'nameOfVender' },
                            { 'data': 'quantity' },
                            { 'data': 'perPrice' },
                            { 'data': 'totalPrice' },
                            { 'data': 'discount' },
                            { 'data': 'finalPrice' },
                            {
                                data: "suplementId",
                                className: "center",
                                render: function (data, type, full, meta) {
                                    return '<a href="BuySupplements.aspx?id=' + data + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a> &nbsp;&nbsp;&nbsp; <a href="BuySupplements.aspx?id=' + data + "&" + "key=delete" + '" class="editor_remove"><img src="Assets/Icon/delete.png" class="iconDelete" /></a>';
                                }
                            }
                        ]
                    });
                }
            });

        });
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
                var quantity = document.getElementById("ContentPlaceHolder1_txtQuantityOfSuplement").value;
                var perPrice = document.getElementById("ContentPlaceHolder1_txtPerPrice").value;
                var totalPrice = document.getElementById("ContentPlaceHolder1_txtTotalPrice");
                var discountPercentage = document.getElementById("ContentPlaceHolder1_txtDiscountPercentage").value;
                var discount = document.getElementById("ContentPlaceHolder1_txtDiscount");
                var finalPrice = document.getElementById("ContentPlaceHolder1_txtFinalPrice");

                totalPrice.value = quantity * perPrice;
                if (GetParameterValues('key') != 'edit') {
                    document.getElementById('ContentPlaceHolder1_btnAddSuplements').removeAttribute('disabled');
                }
                if (discount != "") {
                    discount.value = (discountPercentage / 100) * totalPrice.value;

                    finalPrice.value = totalPrice.value - discount.value;

                }
            }
        }
        catch (error) {

        }
    </script> 

    <asp:UpdatePanel ID="upnl1" runat="server">
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="txtNameOfSuplement" />
     </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="box box-info">
                    <div class="box-title">
                        <asp:HiddenField ID="hidHeader" runat="server" Value="Supplements Buying Records" />
                    </div>
                    <div class="box-body">
                        <div class="col-xs-12">
                            <div class="col-sm-3">
                                Name of Suplement:
                                <asp:TextBox ID="txtNameOfSuplement" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                <asp:HiddenField ID="hidSnNo" runat="server" />
                            </div>
                            <div class="col-sm-3">
                                Date:
                                <asp:TextBox ID="txtSuplementBuyingDate" runat="server" CssClass="form-control input-sm nepCalendar"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Name of Vender/Supplier:
                                <asp:TextBox ID="txtNameOfVender" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Branch:
                                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                Quantity:
                                <asp:TextBox ID="txtQuantityOfSuplement" TextMode="Number" onkeyup="javascript:calculate();" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Per Price:
                                <asp:TextBox ID="txtPerPrice" TextMode="Number" onkeyup="javascript:calculate();" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Total Price:
                                <asp:TextBox ID="txtTotalPrice" Enabled="false" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Discount:<br />
                                <asp:TextBox ID="txtDiscountPercentage" TextMode="Number" runat="server" Width="49%" onkeyup="javascript:calculate();" Style="display: inline-block" placeholder="%" CssClass="form-control input-sm"></asp:TextBox>
                                <asp:TextBox ID="txtDiscount" runat="server" Width="49%" Enabled="false" onkeyup="javascript:calculate();" Text="0" Style="display: inline-block;" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Final Price:
                                <asp:TextBox ID="txtFinalPrice" runat="server" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Paid Status<br />
                                <asp:CheckBox ID="chkPaid" runat="server" />
                            </div>
                            <div class="col-sm-12">
                                <asp:Button ID="btnAddSuplements" runat="server" Text="Submit" CssClass="btn btn-sm btn-success" OnClick="btnAddSuplements_Click" />
                                <asp:Button ID="btnEditSuplements" runat="server" Enabled="false" Text="Edit" CssClass="btn btn-sm btn-danger btn-a" OnClick="btnEditSuplements_Click" />
                                <asp:Label ID="lblInfo" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="row">
        <div class="col-xs-12">
            <div class="box box-solid">
                <div class="box-body">
                    <div class="table-responsive">
                        <table id="suplementTable" style="font-size: 12px" class="table table-bordered table-hover">
                            <thead>
                                <tr class="header">
                                    <th>Sn.</th>
                                    <th>Date</th>
                                    <th width="300px">Name Of Suplement</th>
                                    <th>Branch</th>
                                    <th>Name Of Vender</th>
                                    <th>Quantity</th>
                                    <th>Per Price</th>
                                    <th>Total Price</th>
                                    <th>Discount</th>
                                    <th>Final Price</th>
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
                    <asp:Label ID="lblPopupError" runat="server"  ForeColor="Red" />
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
