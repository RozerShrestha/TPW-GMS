<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Consultation.aspx.cs" Inherits="TPW_GMS.Consultation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <head>
        <script type="text/javascript">
            $(document).ready(function binddatatable() {
                $.ajax({
                    type: 'GET',
                    dataType: 'json',
                    url: '/api/GetConsultation',
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                    success: function (data) {
                        var datatableVariable = $('#ConsultationBookingTable').DataTable({
                            autoWidth:false,
                            data: data,
                            rowCallback: function (row, data) {
                                if (data["isPaid"] == 0) {
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
                                
                                { 'data': 'trainerName' },
                                {
                                    'data': 'date',
                                    "render": function (data) {
                                        var date = new Date(data);
                                        var month = date.getMonth() + 1;
                                        return (month.length > 1 ? month : "0" + month) + "/" + date.getDate() + "/" + date.getFullYear();
                                    }
                                },
                                { 'data': 'memberName' },
                                { 'data': 'memberId' },
                                { 'data': 'charge' },
                                { 'data': 'discount' },
                                { 'data': 'finalCharge' },
                                { 'data': 'status' },
                                {'data':'receiptNo'},
                                //{ 'data': 'TrainerCommission' },
                                {
                                    data: "consultationId",
                                    className: "center",
                                    render: function (data, type, full, meta) {
                                        return '<a href="Consultation.aspx?id=' + data + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a> &nbsp;&nbsp;&nbsp; <a href="Consultation.aspx?id=' + data + "&" + "key=delete" + '" class="editor_remove"><img src="Assets/Icon/delete.png" class="iconDelete" /></a>';
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
                    var charge = document.getElementById("ContentPlaceHolder1_txtFee");
                    var discount = document.getElementById("ContentPlaceHolder1_txtDiscount");
                    var finalPrice = document.getElementById("ContentPlaceHolder1_txtFinalFee");

                    finalPrice.value = charge.value - discount.value;
                    if (GetParameterValues('key') != 'edit') {
                        document.getElementById('ContentPlaceHolder1_btnAddConsultation').removeAttribute('disabled');
                    } 
                }
            }
            catch (error) {

            }
        </script>        
    </head>  
    <asp:HiddenField ID="hidHeader" runat="server" Value="Consultation" />
    <asp:HiddenField ID="hidSnNo" runat="server" />
     <asp:UpdatePanel ID="pnlConsulation" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="box box-solid">
                    <div class="box-body">
                        <div class="col-xs-12">
                            <div class="col-sm-3">
                                Trainer
                                <asp:DropDownList ID="ddlTrainer" runat="server" CssClass="select2Example form-control input-sm" />
                            </div>
                            <div class="col-sm-3">
                                Date
                                <asp:TextBox ID="txtDateConsultation" runat="server" CssClass="form-control input-sm dateControl" />
                            </div>
                            <div class="col-sm-3">
                                Client Name
                                <asp:DropDownList ID="ddlClientName" runat="server" OnTextChanged="ddlClientName_TextChanged" AutoPostBack="true" CssClass="select2Example form-control input-sm"></asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                Client Id
                                <asp:DropDownList ID="ddlClientId" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                Charge/Fee
                                <asp:TextBox ID="txtFee" runat="server" onkeyup="javascript:calculate();" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-3">
                                Discount
                                <asp:TextBox ID="txtDiscount" Text="0" runat="server" onkeyup="javascript:calculate();" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-3">
                                Final Charge/Fee
                                <asp:TextBox ID="txtFinalFee" runat="server" Enabled="false" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-3">
                                Status
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control input-sm">
                                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Pending"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Booked"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Canceled"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-2">
                                Receipt No:<span class="asterik">*</span>
                                <asp:TextBox ID="txtReceiptNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                Paid<br />
                                <asp:CheckBox ID="chkPaid" runat="server" />
                            </div>
                            <div class="col-sm-12">
                                <asp:Button ID="btnAddConsultation" runat="server" Enabled="true" Text="Submit" OnClick="btnAddConsultation_Click" CssClass="btn btn-sm btn-success" />
                                <asp:Button ID="btnEditConsultation" runat="server" Enabled="false" Text="Edit" OnClick="btnEditConsultation_Click" CssClass="btn btn-sm btn-danger btn-a" />
                                <asp:Label ID="lblInfo" Style="margin-left: 0px" runat="server"></asp:Label>
                                </td>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="box box-solid">
            <div class="box-body">
                <div class="col-xs-12">
                    <div class="table-responsive">
                        <table id="ConsultationBookingTable" style="font-size: 12px" class="table table-bordered table-hover">
                            <thead>
                                <tr class="header">
                                    <th>Sn.</th>
                                    <th>Trainer</th>
                                    <th>Date</th>
                                    <th>Client Name</th>
                                    <th width="110px">Client Id</th>
                                    <th>Charge/Fee</th>
                                    <th>Discount</th>
                                    <th>Final Charge</th>
                                    <th>Status</th>
                                    <th>Receipt No</th>
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
