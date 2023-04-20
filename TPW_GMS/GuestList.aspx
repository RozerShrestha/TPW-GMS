<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GuestList.aspx.cs" Inherits="TPW_GMS.GuestList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

        });
        function pageLoad(sender, args) {
            $(".nep").nepaliDatePicker({
                ndpYear: true,
                ndpMonth: true,
                ndpYearCount: 30,
                onChange: dateDiff,
                dateFormat: "YYYY/MM/DD"
            });
            loadData();
        }
        function loadData() {
            var existingTbl = $('#GuestTable').DataTable();
            if (existingTbl) {
                $('#GuestTable').DataTable().destroy();
            }
            var table = $('#GuestTable').DataTable({
                fixedHeader: {
                    header: true,
                    headerOffset: 50,
                },
                autoWidth: true,
                language: {
                    sLoadingRecords: '<span style="width:100%;"><img src="Assets/Images/ajax-loader.gif"></span>'
                },
                lengthMenu: [[25, 50, -1], [25, 50, "All"]],
                ajax: {
                    type: 'GET',
                    dataType: 'json',
                    url: 'api/GetGuest',
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                    dataSrc: function (data) {
                        expData = data;
                        return expData;
                    },
                },
                rowCallback: function (row, data) {
                    if (data["count"] === data["attCount"]) {
                        //$(row).css("background-color", "green");
                        $(row).css("color", "red");
                        $(row).addClass('highlight')
                    }
                    if (data["toDate"] != null && new Date() > new Date(data["toDate"])) {
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
                    { 'data': 'name' },
                    { 'data': 'email' },
                    { 'data': 'mobile' },
                    {
                        'data': 'fromDate',
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
                        'data': 'toDate',
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
                        'data': 'count',
                    },
                    {
                        'data': 'attCount',
                    },
                    {
                        data: "id",
                        className: "center",
                        render: function (data, type, full, meta) {
                            return `<a href="GuestList.aspx?id=' + data + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a> &nbsp;&nbsp;&nbsp; <a href="GuestList.aspx?id=' + data + "&" + "key=delete" + '" class="editor_remove"><img src="Assets/Icon/delete.png" class="iconDelete" /></a>     &nbsp;&nbsp;&nbsp; <a href="#" type="button" class="iconView" onclick="sendEmail('${full.name}','${full.email}')"><span></span><img src="Assets/Icon/email.png" class="iconView" /></a> `;
                        }
                    }
                ]
            });
        }
        function dateDiff() {
            let from = $("#txtFromDate").val();
            let to = $("#txtToDate").val()
            let fromObj = NepaliFunctions.ConvertToDateObject(from, "YYYY/MM/DD");
            let toObj = NepaliFunctions.ConvertToDateObject(to, "YYYY/MM/DD");
            let dateDiff = NepaliFunctions.BsDatesDiff(fromObj, toObj);
            $("#txtCount").val(dateDiff);
            //return dateDiff;
        }

        function sendEmail(name, email) {
            $("#imgLoading2").css("display", "block");
            $.ajax({
                url: `api/SendGuestEmail?name=${name}&email=${email}`,
                type: 'GET',
                async: true,
                headers: {
                    'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                },
                success: function (res) {
                    $("#imgLoading2").css("display", "none");
                    if (res) {
                        alert(res);
                    }
                    else {
                        alert(`Email Not Sent to`);
                    }
                },
                error: function (er) {
                    alert(er.responseJSON.Message);
                    $("#imgLoading2").css("display", "none");
                }
            });
        }

    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlEmailMarketing" runat="server">
                <div class="row">
                    <div class="box box-info">
                        <div class="box-title">
                            <asp:HiddenField ID="hidHeader" runat="server" Value="Guest List" />
                            <asp:HiddenField ID="hidSnNo" runat="server" />
                        </div>
                        <div class="box-body">
                            <div class="col-xs-12">
                                <div class="col-sm-2">
                                    Guest Name<span class="asterik">*</span>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    Email<span class="asterik">*</span>
                                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    Mobile Number
                                <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    From
                                <asp:TextBox ID="txtFromDate" ClientIDMode="Static" runat="server" CssClass="form-control input-sm nep"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    To
                                <asp:TextBox ID="txtToDate" ClientIDMode="Static" runat="server" CssClass="form-control input-sm nep"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    Count
                                <asp:TextBox ID="txtCount" ClientIDMode="Static" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </div>

                                <div class="col-md-12" style="margin-top: 10px">
                                    <asp:Button ID="btnAdd" runat="server" Enabled="true" Text="Submit" CssClass="btn btn-sm btn-success" OnClick="btnAdd_Click" />
                                    <asp:Button ID="btnEdit" runat="server" Enabled="false" Text="Edit" CssClass="btn btn-sm btn-danger btn-a" OnClick="btnEdit_Click" />
                                    <asp:Label ID="lblInfo" Style="margin-left: 4px" runat="server"></asp:Label>
                                    <asp:Image ID="imgLoading" Visible="false" runat="server" ImageUrl="~/Assets/Images/ajax-loader.gif" Style="width: 226px;" />
                                    <%--<img id="imgLoading" src="Assets/Images/ajax-loader.gif" style="width: 226px; display:none;" />--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="box-body">
        <div class="row">
            <img id="imgLoading2" src="Assets/Images/ajax-loader.gif" style="margin-top: 14px; width: 226px; display: none" />
        </div>
        <div class="row">
            <div class="col-xs-12">
                <div class="box box-solid">
                    <div class="box-body">
                        <div class="table-responsive">
                            <table id="GuestTable" style="font-size: 12px" class="table table-bordered table-hover">
                                <thead>
                                    <tr class="header">
                                        <th>Sn.</th>
                                        <th>Name</th>
                                        <th>Email</th>
                                        <th>Mobile</th>
                                        <th>From</th>
                                        <th>To</th>
                                        <th>Count</th>
                                        <th>Att Count</th>
                                        <th>Action</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
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
