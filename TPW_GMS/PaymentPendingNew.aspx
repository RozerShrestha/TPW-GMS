<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PaymentPendingNew.aspx.cs" Inherits="TPW_GMS.PaymentPendingNew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            $('#btnSubmit').on('click', function () {
                loadData();
            });
            $('#reportType').on('change', function () {
                var reportType = $('#reportType option:selected').text(); 
                if (reportType == "Active")
                    $('#postPendingCheck').prop("disabled", false);
                else
                    $('#postPendingCheck').prop("disabled", true);
            });
            loadData();
            if ($("#lblUserLogin").html()!=="admin")
                $(".dt-buttons").hide()
        });
        function loadData() {
            var form = $('form#ctl01').serializeArray();
            var formData = [];
            form.forEach(function (item) {
                formData[item.name.split('$')[2]] = item.value;
            });
            var existingTbl = $('#dtTable').DataTable();
            if (existingTbl) {
                $('#dtTable').DataTable().destroy();
            }
            var table = $('#dtTable').dataTable({
                fixedHeader: {
                    header: true,
                    headerOffset: 50,
                },
                autoWidth: true,
                order:false,
                lengthMenu: [[25, 50, -1], [25, 50, "All"]],
                language: {
                    sLoadingRecords: '<span style="width:100%;"><img src="Assets/Images/ajax-loader.gif"></span>'
                },
                ajax: {
                    url: 'api/GetPaymentPending/',
                    data: formData,
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    type: "POST",
                    dataType: "JSON",
                    dataSrc: "",
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                },
                rowCallback: function (row, data) {
                    if (data["ActiveInactive"] == "Active") {
                        $(row).css("color", "green");
                        $(row).addClass('highlight')
                    }
                    else {
                        $(row).css("color", "red");
                        $(row).addClass('highlight')
                    }
                    //if (ages.indexOf(data['fullname']) > 0) {
                    //    $(row).css("background-color", "red");
                    //    $(row).addClass('highlight')
                    //}
                },

                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        title: 'TPW-Payment Pending',
                        exportOptions: {
                            columns: ':visible'
                        },  
                    },
                ],
                columns: [
                    {
                        'data': 'memberId',
                        render: function (data, type, row) {
                                return '<input type="checkbox" onchange="handleChange(' + data + ');" class="editor-active">';
                        }
                    },
                    { 'data': 'memberId' },
                    { 'data': 'branch' },
                    { 'data': 'fullname' },
                    { 'data': 'email' },
                    { 'data': 'contactNo' },
                    {
                        "data": "memberBeginDate",
                        "render": function (data) {
                            try {
                                var dat = data != null ? (data.split('T')[0]).split('-').join('/') : '';
                                let objEng = NepaliFunctions.ConvertToDateObject(dat, "YYYY/MM/DD");
                                let objNep = NepaliFunctions.AD2BS(objEng);
                                let dt = NepaliFunctions.ConvertDateFormat(objNep, "YYYY/MM/DD");

                                return `${dt}`;
                            } catch (e) {
                                return '';
                            }
                        }
                    },
                    {
                        "data": "memberExpireDate",
                        "render": function (data) {
                            try {
                                var dat = data != null ? (data.split('T')[0]).split('-').join('/') : '';
                                let objEng = NepaliFunctions.ConvertToDateObject(dat, "YYYY/MM/DD");
                                let objNep = NepaliFunctions.AD2BS(objEng);
                                let dt = NepaliFunctions.ConvertDateFormat(objNep, "YYYY/MM/DD");

                                return `${dt}`;
                            } catch (e) {
                                return '';
                            }
                        }
                    },
                    { 'data': 'memberOption' },
                    { 'data': 'memberCatagory' },
                    { 'data': 'memberPaymentType' },
                    {
                        'data': 'emailStatus',
                        render: function (data, type, row) {
                            if(data)
                                return '<input type="checkbox" class="checkbox" disabled checked>'
                            else
                                return '<input type="checkbox" class="checkbox" disabled>'
                        }

                         
                    },
                    { 'data':'ActiveInactive'},
                    {'data':null}
                ],
                columnDefs: [{
                    data: 'memberId',
                    className: "dt-body-right",
                    targets: [-1], render: function (data, type, full, meta) {

                        return `<a href="EditForm.aspx?id=${data.memberId}&key=edit"  target="_blank"><img src="Assets/Icon/edit.png" class="iconView" /></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="#" type="button" class="iconView" onclick="sendEmail('${data.memberId}')"><span></span><img src="Assets/Icon/email.png" class="iconView" /></a>`;

                    }
                }]
            });
        }
        function sendEmail(memberid) {
            $("#imgLoading").css("display", "block");
            
            $.ajax({
                url: `api/SendPendingEmail`,
                type: 'GET',
                data: {
                    memberid: memberid,
                },
                async:false,
                headers: {
                    'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                },
               success: function (res) {
                   if (res) {
                       alert(`Email Sent to ${memberid}`);
                        $("#imgLoading").css("display", "none");
                    }
                   else {
                       alert(`Email Not Sent to ${memberid}`);
                        $("#imgLoading").css("display", "none");
                    }
                },
                error: function (er) {
                    alert(er);
                }
            });
        }
        function bulkEmail() {
            alert(`Email are in a Queue to send.Please do not close the browser.`);
            var memberids = "";
            $('#dtTable').find('tr').each(function () {
                var row = $(this);
                if (row.find('input[type="checkbox"]').is(':checked')) {
                    let memberId = row.find("td").eq(1).html();
                    setTimeout(function () { sendEmail(memberId); }, 5000);
                }
            });
        }

    </script>
    <asp:HiddenField ID="hidHeader" runat="server" Value="Payment Pending" />
            <div class="box box-info">
                <div class="box-body">
                    <div class="row">
                        <div class="col-sm-2">
                            <strong>Branch</strong>
                            <asp:DropDownList ID="branch" ClientIDMode="Static" runat="server" CssClass="form-control input-sm">
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-2">
                            <strong>Status</strong>
                            <asp:DropDownList ID="reportType" ClientIDMode="Static" runat="server" CssClass="form-control input-sm">
                                <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                                <asp:ListItem Text="Due(1 Month)" Selected="True" Value="Due"></asp:ListItem>
                                <asp:ListItem Text="Active Pending" Value="ActivePending"></asp:ListItem>
                                <asp:ListItem Text="Expired Over 1 Month" Value="expiredOverOne"></asp:ListItem>
                                <asp:ListItem Text="Paused Members" Value="paused"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-2">
                            <strong>Flag</strong>
                            <asp:DropDownList ID="flag" ClientIDMode="Static" runat="server" CssClass="form-control input-sm">
                                <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Email Sent" Value="true"></asp:ListItem>
                                <asp:ListItem Text="Email Not Sent" Value="false"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-2">
                            Post Pending    
                        <asp:DropDownList ID="postPendingCheck" Enabled="false" ClientIDMode="Static" runat="server" CssClass="form-control input-sm">
                            <asp:ListItem Value="1" Text="1 day after Expiry"></asp:ListItem>
                            <asp:ListItem Value="2" Text="2 days after Expiry"></asp:ListItem>
                            <asp:ListItem Value="3" Text="3 days after Expiry"></asp:ListItem>
                            <asp:ListItem Value="4" Text="4 days after Expiry"></asp:ListItem>
                            <asp:ListItem Value="5" Text="5 days after Expiry"></asp:ListItem>
                            <asp:ListItem Value="6" Text="6 days after Expiry"></asp:ListItem>
                            <asp:ListItem Value="7" Text="7 days after Expiry"></asp:ListItem>
                        </asp:DropDownList>
                        </div>
                        <div class="col-sm-2">
                            <input type="button" id="btnSubmit" style="margin-top: 17px" class="btn btn-sm btn-success" name="Submit" value="Submit" />
                            <input type="button" id="btnSendBulkEmail" onclick="bulkEmail()" style="margin-top: 17px" class="btn btn-sm btn-primary" name="Submit" value="Send Bulk Email" />
                        </div>
                    </div>
                </div>
                <div class="box-body">
                    <div class="row">
                        <img id="imgLoading" src="Assets/Images/ajax-loader.gif" style="margin-top: 14px; width: 226px; display:none" />

                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="table-responsive">
                                <table id="dtTable" class="table table-striped table-bordered table-sm" style="font-size: 12px; width: 100%">
                                    <thead>
                                        <tr class="border-bottom-0 tr-header header">
                                            <th>Check</th>
                                            <th>Member Id</th>
                                            <th>Branch </th>
                                            <th>Full Name</th>
                                            <th>Email</th>
                                            <th>Mobile No</th>
                                            <th>Renew Date</th>
                                            <th>Expired Date </th>
                                            <th>Membership Option </th>
                                            <th>Catagory </th>
                                            <th>Payment Type </th>
                                            <th>Email Flag</th>
                                            <th>Active</th>
                                            <th>Action</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    <div class="modal fade" id="emailConfirmBox" role="dialog">
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
                    
                    <button type="button" class="btn btn-success btn-sm" data-dismiss="modal">No</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
