<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrainerBooking.aspx.cs" Inherits="TPW_GMS.TrainerBooking" %>
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
                    url: 'api/GetAllTrainerBooking',
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                    success: function (data) {
                        var datatableVariable = $('#TrainerBookingTable').DataTable({
                            autoWidth: false,
                            data: data,
                            rowCallback: function (row, data) {
                                if (data["status"] == "Canceled") {
                                    $(row).css("color", "red");
                                    $(row).addClass('highlight')
                                }
                                else if (data["status"] == "Pending") {
                                    $(row).css("color", "rgb(0 114 255)");
                                    $(row).addClass('highlight')
                                }
                                else if (data["status"] == "Booked") {
                                    $(row).css("color", "green");
                                    $(row).addClass('highlight')
                                }

                                
                                //if (ages.indexOf(data['fullname']) > 0) {
                                //    $(row).css("background-color", "red");
                                //    $(row).addClass('highlight')
                                //}
                            },
                            columns: [
                                {
                                    'data': 'id',
                                    render: function (data, type, row, meta) {
                                        return meta.row + meta.settings._iDisplayStart + 1;
                                    }
                                },
                                { 'data': 'branch' },
                                { 'data': 'trainerName' },
                                { 'data': 'Class' },
                                {
                                    'data': 'from',
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
                                {
                                    'data': 'to',
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
                                { 'data': 'clientName' },
                                { 'data': 'charge' },
                                { 'data': 'discount' },
                                { 'data': 'finalCharge' },
                                { 'data': 'status' },
                                //{ 'data': 'TrainerCommission' },
                                {
                                    data: "trainerBookingId",
                                    className: "center",
                                    render: function (data, type, full, meta) {
                                        return '<a href="TrainerBooking.aspx?id=' + data + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a> &nbsp;&nbsp;&nbsp; <a href="TrainerBooking.aspx?id=' + data + "&" + "key=delete" + '" class="editor_remove"><img src="Assets/Icon/delete.png" class="iconDelete" /></a>';
                                    }
                                }
                            ]
                        });                       
                    }
                });
            });
            try {
                function calculate() {
                    var fee = document.getElementById("ContentPlaceHolder1_txtFee").value;
                    var discount = document.getElementById("ContentPlaceHolder1_txtDiscount").value;
                    var finalFee = document.getElementById("ContentPlaceHolder1_txtFinalFee");
                    finalFee.value = fee - discount;
                }
            } catch (error) {

            }
            
        </script>        
     <asp:HiddenField ID="hidHeader" runat="server" Value="Trainer Booking Request" />
    <asp:UpdatePanel ID="pnlTrainerBooking" runat="server">
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnAddTrainer" />
            <asp:PostBackTrigger ControlID="btnEditTrainer" />--%>
        </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-info">
                        <div class="box-title">
                        </div>
                        <div class="box-body">
                            <div class="col-sm-3">
                                Branch
                                <asp:DropDownList ID="branch" ClientIDMode="Static" runat="server" CssClass="form-control input-sm">
                            </asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                Trainer
                                <asp:DropDownList ID="ddlTrainer" runat="server" OnTextChanged="ddlTrainer_TextChanged" AutoPostBack="true" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-3">
                                Class
                                <asp:TextBox ID="txtClass" Enabled="false" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-3">
                                From
                                <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control input-sm nepCalendar" />
                            </div>
                            <div class="col-sm-3">
                                To
                                <asp:TextBox ID="txtTo" runat="server" CssClass="form-control input-sm nepCalendar" />
                            </div>
                            <div class="col-sm-3">
                                Client Name<br />
                                <asp:DropDownList ID="ddlClientName" OnTextChanged="ddlClientName_TextChanged" AutoPostBack="true" runat="server" CssClass="select2Example form-control input-sm"></asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                Client Id
                                <asp:DropDownList ID="ddlClientId" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                No of Trainee
                                  <asp:DropDownList ID="ddlNoOfTrainee" runat="server" OnSelectedIndexChanged="ddlNoOfTrainee_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control input-sm">
                                      <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                      <asp:ListItem Value="1" Text="1 Person"></asp:ListItem>
                                      <asp:ListItem Value="2" Text="2 or More"></asp:ListItem>
                                  </asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                Charge/Fee
                                <asp:TextBox ID="txtFee" runat="server" Enabled="false" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-3">
                                Discount
                                <asp:TextBox ID="txtDiscount" runat="server" Text="0" onkeyup="javascript:calculate();" CssClass="form-control input-sm" />
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
                            <div class="col-md-12">
                                <asp:Button ID="btnAddTrainerBooking" runat="server" Enabled="true" Text="Submit" OnClick="btnAddTrainerBooking_Click" CssClass="btn btn-sm btn-success" />
                                <asp:Button ID="btnEditTrainerBooking" runat="server" Enabled="false" Text="Edit" OnClick="btnEditTrainerBooking_Click" CssClass="btn btn-sm btn-danger btn-a" />
                                <asp:Label ID="lblInfo" Style="margin-left: 0px" runat="server"></asp:Label>
                                <asp:HiddenField ID="hidSnNo" runat="server" />
                                </td>
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
                        <table id="TrainerBookingTable" style="font-size: 12px" class="table table-bordered table-hover">
                            <thead>
                                <tr class="header">
                                    <th>Sn.</th>
                                    <th>Branch</th>
                                    <th>Trainer</th>
                                    <th>Class</th>
                                    <th>From</th>
                                    <th>To</th>
                                    <th>Cleint Name</th>
                                    <th>Charge/Fee</th>
                                    <th>Discount</th>
                                    <th>Final Charge</th>
                                    <th>Status</th>
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
