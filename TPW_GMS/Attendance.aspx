<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Attendance.aspx.cs" Inherits="TPW_GMS.Attendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        th, td {
            padding: 3px;
        }

        tr {
            padding: 3px;
        }
    </style>
    <asp:HiddenField ID="hidHeader" runat="server" Value="Attendance" />
    <asp:HiddenField ID="hidCurrentLoginBranch" runat="server" />
    <asp:UpdatePanel ID="upnlAttendance" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-5">
                    <div class="box box-info">
                        <div class="box-header header">
                            <span style="margin-left: 2px; font-size: 16px"><b>TPW-Attendance Record</b></span>
                            <span style="float: right">
                                <asp:Button ID="btnReload" runat="server" Style="margin-top: -7px" value="Reload" class="btn btn-danger" Text="Reload" OnClick="btnReload_Click" />
                            </span>
                        </div>
                        <div class="box-body">
                            <table class="table table-bordered" border="1" style="margin-left: 0px; margin-top: -6px;">
                                <tbody>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="qrCodeScan" CssClass="form-control" runat="server" autofocus></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td><b>Membership Option:</b>
                                        </td>
                                        <td>
                                            <input type="text" id="txtMembershipOption" disabled class="form-control input-sm" />
                                        </td>
                                    </tr>
                                    <tr hidden id="trId">
                                        <td>
                                            <asp:Button ID="btnSubmit" Text="submit" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-success" />
                                        </td>
                                        <td>
                                            <input type="text" id="txtOffHourAmount" placeholder="charge" class="form-control input-sm" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>First Name:</b></td>
                                        <td>
                                            <input type="text" id="txtFirstName" disabled class="form-control input-sm" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Last Name:</b></td>
                                        <td>
                                            <input type="text" id="txtLastName" disabled class="form-control input-sm" /></td>
                                    </tr>
                                    <tr>
                                        <td><b>Branch:</b></td>
                                        <td>
                                            <input type="text" id="txtBranch" runat="server" disabled class="form-control input-sm" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Universal Membership Limit:</b></td>
                                        <td>
                                            <input type="text" id="txtUniversalMembershipLimit" disabled class="form-control input-sm" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Membership Date:</b></td>
                                        <td>
                                            <input type="text" id="txtMembershipDate" disabled class="form-control input-sm dateControl" /></td>
                                    </tr>
                                    <tr>
                                        <td><b>Renew Date:</b></td>
                                        <td>
                                            <input type="text" id="txtRenewDate" disabled class="form-control input-sm" /></td>
                                    </tr>
                                    <tr>
                                        <td><b>Expire Date:</b></td>
                                        <td>
                                            <input type="text" id="txtExpireDate" disabled class="form-control input-sm" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><b>Message:</b>
                                            <textarea name="txtMessage" id="txtMessage" style="height: 120px; font-size:14px; color: red" disabled class="form-control input-sm"></textarea>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="col-md-7">
                    <div class="box box-info">
                        <div class="box-body">
                            <h4>Member Attendance Record</h4>
                            <div class="table-responsive">
                                <table id="tblMemberAttendance" style="font-size: 12px; width: 100%" class="table table-striped table-bordered table-sm">
                                    <thead>
                                        <tr class="border-bottom-0 tr-header header">
                                            <th style="min-width: 100px">MemberId</th>
                                            <th style="min-width: 100px">Name</th>
                                            <th>Member Option</th>
                                            <th>CheckIn</th>
                                            <th>CheckOut</th>
                                            <th>Branch</th>
                                            <th>Checkin Branch</th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="box box-info">
                        <div class="box-body">
                            <h4>Guest Attendance Record</h4>
                            <div class="table-responsive">
                                <table id="tblGuestAttendance" style="font-size: 12px; width: 100%" class="table table-striped table-bordered table-sm">
                                    <thead>
                                        <tr class="border-bottom-0 tr-header header">
                                            <th>Id</th> <%--should be auto--%>
                                            <th>Name</th> <%--should be auto--%>
                                            <th style="min-width: 100px">Checkin Branch</th>
                                            <th>CheckIn</th>
                                            <th>CheckOut</th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="box box-info">
                        <div class="box-body">
                            <h4>Email Marketing Attendance Record</h4>
                            <div class="table-responsive">
                                <table id="tblEmailMarketingAttendance" style="font-size: 12px; width: 100%" class="table table-striped table-bordered table-sm">
                                    <thead>
                                        <tr class="border-bottom-0 tr-header header">
                                            <th>Id</th> <%--should be auto--%>
                                            <th>Name</th> <%--should be auto--%>
                                            <th style="min-width: 100px">Checkin Branch</th>
                                            <th>CheckIn</th>
                                            <th>CheckOut</th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="errorModal" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Information</h4>
                </div>
                <div class="modal-body">
                    <span id="spnNote1"></span>
                    <br />
                    <div class="col-md-4" style="padding-left: 0px; padding-right: 0px;">
                        <asp:TextBox ID="txtStatic" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-md-8" style="padding-left: 0px; padding-right: 0px;">
                        <asp:TextBox ID="txtReceiptNo" placeholder="Receipt No" CssClass=" form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnActivate" runat="server" CssClass="btn btn-primary" disabled Text="Activate" OnClientClick="atten()" />
                    <asp:Button ID="btnSubmitModal" runat="server" CssClass="btn btn-primary" disabled Text="Yes" OnClick="btnSubmitModal_Click" />
                    <button type="button" class="btn btn-danger" data-dismiss="modal">No</button>
                </div>
            </div>
        </div>
    </div>
    <script>
        function debounce(fn, duration) {
            var timer;
            return function () {
                clearTimeout(timer);
                timer = setTimeout(fn, duration);
            }
        }
        $(function () {
            loadAttRecord();
            loadGuestAttRecord();
            loadEmailMarketingAttRecord();
            $('#txtReceiptNo').on('keyup', function () {
                if ($("#txtReceiptNo").val() != "")
                    $('#ContentPlaceHolder1_btnSubmitModal').attr('disabled', false);
                else
                    $('#ContentPlaceHolder1_btnSubmitModal').attr('disabled', true);
            });
            document.getElementById("ContentPlaceHolder1_qrCodeScan").addEventListener("keyup", function (event) {

                if (event.getModifierState("CapsLock")) {
                    alert("Please Turn Off the Caps Lock");
                    return;
                }
            });
            $('#ContentPlaceHolder1_qrCodeScan').on('keyup', debounce(function () {
                var _msg = $get("ContentPlaceHolder1_qrCodeScan").value;
                var _branch = document.getElementById("lblUserLogin").innerHTML;
                $.ajax({
                    url: 'api/Attendance',
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                    type: "get",
                    async: false,
                    data: {
                        memberId: _msg,
                        loginBranch: _branch,
                        type: 'normal'
                    },
                    success: function (result) {
                        loadDataToForm(result);
                    },
                    error: function () {
                        alert("error");
                    }
                });
            }, 200));
        });
        function loadAttRecord() {
            var loginUser = $("#lblUserLogin").text().split('-')[0];
            var table = $('#tblMemberAttendance').DataTable({
                fixedHeader: {
                    header: true,
                    headerOffset: 50,
                },
                ordering: false,
                autoWidth: true,
                language: {
                    sLoadingRecords: '<span style="width:100%;"><img src="Assets/Images/ajax-loader.gif"></span>'
                },
                lengthMenu: [[5,25, 50, -1], [5, 25, 50, "All"]],
                ajax: {
                    url: `api/GetMemberAttendanceList?loginUser=${loginUser}`,
                     dataType: "JSON",
                     dataSrc: "",
                     headers: {
                         'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                     },
                 },
                columns: [
                    { 'data': 'memberId' },
                    { 'data': 'fullname' },
                    { 'data': 'memberOption' },
                    {
                        'data': 'checkin',
                        //'render': function (data) {
                        //    try {
                        //        var dat = data != null ? (data.split('T')[0]).split('-').join('/') : '';
                        //        let objEng = NepaliFunctions.ConvertToDateObject(dat, "YYYY/MM/DD");
                        //        let objNep = NepaliFunctions.AD2BS(objEng);
                        //        let dt = NepaliFunctions.ConvertDateFormat(objNep, "YYYY/MM/DD");

                        //        return dt;
                        //    } catch (e) {
                        //        return '';
                        //    }
                        //}
                    },
                    {
                        'data': 'checkout',
                        //'render': function (data) {
                        //    try {
                        //        var dat = data != null ? (data.split('T')[0]).split('-').join('/') : '';
                        //        let objEng = NepaliFunctions.ConvertToDateObject(dat, "YYYY/MM/DD");
                        //        let objNep = NepaliFunctions.AD2BS(objEng);
                        //        let dt = NepaliFunctions.ConvertDateFormat(objNep, "YYYY/MM/DD");

                        //        return dt;
                        //    } catch (e) {
                        //        return '';
                        //    }
                          //}
                     },
                     { 'data': 'branch' },
                     { 'data': 'checkinBranch' },
                 ],
             })
        }
        function loadGuestAttRecord() {
            var loginUser = $("#lblUserLogin").text().split('-')[0];
            var table = $('#tblGuestAttendance').DataTable({
                fixedHeader: {
                    header: true,
                    headerOffset: 50,
                },
                ordering: false,
                autoWidth: true,
                language: {
                    sLoadingRecords: '<span style="width:100%;"><img src="Assets/Images/ajax-loader.gif"></span>'
                },
                lengthMenu: [[5, 25, 50, -1], [5, 25, 50, "All"]],
                ajax: {
                    url: `api/GetGuestAttendanceList?loginUser=${loginUser}`,
                    dataType: "JSON",
                    dataSrc: "",
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                },
                columns: [
                    {
                        'data': 'id',
                    },
                    { 'data': 'name' },
                    {
                        'data': 'checkin',
                        //'render': function (data) {
                        //    try {
                        //        var dat = data != null ? (data.split('T')[0]).split('-').join('/') : '';
                        //        let objEng = NepaliFunctions.ConvertToDateObject(dat, "YYYY/MM/DD");
                        //        let objNep = NepaliFunctions.AD2BS(objEng);
                        //        let dt = NepaliFunctions.ConvertDateFormat(objNep, "YYYY/MM/DD");

                        //        return dt;
                        //    } catch (e) {
                        //        return '';
                        //    }
                        //}
                    },
                    {
                        'data': 'checkout',
                        //'render': function (data) {
                        //    try {
                        //        var dat = data != null ? (data.split('T')[0]).split('-').join('/') : '';
                        //        let objEng = NepaliFunctions.ConvertToDateObject(dat, "YYYY/MM/DD");
                        //        let objNep = NepaliFunctions.AD2BS(objEng);
                        //        let dt = NepaliFunctions.ConvertDateFormat(objNep, "YYYY/MM/DD");

                        //        return dt;
                        //    } catch (e) {
                        //        return '';
                        //    }
                        //}
                    },
                    { 'data': 'checkinBranch' },
                ],
            })
        }
        function loadEmailMarketingAttRecord() {
            var loginUser = $("#lblUserLogin").text().split('-')[0];
            var table = $('#tblEmailMarketingAttendance').DataTable({
                fixedHeader: {
                    header: true,
                    headerOffset: 50,
                },
                ordering: false,
                autoWidth: true,
                language: {
                    sLoadingRecords: '<span style="width:100%;"><img src="Assets/Images/ajax-loader.gif"></span>'
                },
                lengthMenu: [[5, 25, 50, -1], [5, 25, 50, "All"]],
                ajax: {
                    url: `api/GetEmailMarketingAttendanceList?loginUser=${loginUser}`,
                    dataType: "JSON",
                    dataSrc: "",
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                },
                columns: [
                    {
                        'data': 'id',
                    },
                    { 'data': 'name' },
                    {
                        'data': 'checkin',
                    },
                    {
                        'data': 'checkout',
                    },
                    { 'data': 'checkinBranch' },
                ],
            })
        }
        function atten() {
            var _msg = $get("ContentPlaceHolder1_qrCodeScan").value;
            var _branch = document.getElementById("lblUserLogin").innerHTML;
            $.ajax({
                url: 'api/Attendance',
                headers: {
                    'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                },
                type: "get",
                async: false,
                data: {
                    memberId: _msg,
                    loginBranch: _branch,
                    type: 'activate'
                },
                success: function (result) {
                    loadDataToForm(result);
                },
                error: function () {
                    alert("error");
                }
            });
        }
        function loadDataToForm(result) {
            var finalMessage = "";
            var today = new Date(new Date().toLocaleDateString());
            var exp = new Date(new Date(result.membershipExpireDate).toLocaleDateString());
            $('#txtMembershipOption').val(result.membershipOption);
            $('#txtFirstName').val(result.firstName);
            $('#txtLastName').val(result.lastName);
            $('#ContentPlaceHolder1_txtBranch').val(result.branch);
            $('#txtUniversalMembershipLimit').val(result.universalMembershipLimit);
            $('#txtMembershipDate').val(result.membershipDate);
            $('#txtRenewDate').val(result.membershipBeginDate);
            $('#txtExpireDate').val(result.membershipExpireDate);

            finalMessage += result.universalMembershipLimit == 0 ? "Universal Limit: 0 \n" : "";
            finalMessage += result.attendanceCount != 0 ? "Checkin: Already Checked In \n" : "";
            finalMessage += result.membershipStatus == "InActive" ? "Membership Status: Inactive \n" : "";
            finalMessage += result.isValid == false ? "Valid: False \n" : "";
            finalMessage += result.pendingPayment != "" && result.pendingPayment!=null ? result.pendingPayment : "";
            finalMessage += result.membershipOption == "OffHour" && result.isValid == false ? "Membership option: OFFHOUR (CheckIn time is 10-4)\n" : "";
            finalMessage += today > exp && result.membershipOption != "Email Marketing Client" ? "Membership Expired" : "";
            finalMessage += result.message;
            $('#txtMessage').val(finalMessage);
            if (result.membershipOption == "OffHour" && result.isValid == false && result.membershipStatus == "Active") {
                $("#spnNote1").html("<i style='color: red'>Note:OFFHOUR Membership CheckIn time is only from 10 AM to 4 PM</i><br /><i style='color: green'>Do you want to Pay an Extra Charge of Rs 100? </i>");
                $('#errorModal').modal('show');
            }
            else if (result.universalMembershipLimit == 0 && result.isValid == false && result.membershipStatus == "Active") {
                $("#spnNote1").html("<i style='color: red'>Note:Universal Membership Limit is all used </i><br /><i style='color: green'>Do you want to Pay an Extra Charge of Rs 100? </i>");
                $('#errorModal').modal('show');
            }
            else if (result.membershipStatus == "InActive") {
                $("#spnNote1").html("<i style='color: red'>Note:Membership Status is INACTIVE</i><br /> Click Activate Button to Activate the Membership");
                $('#txtReceiptNo').hide();
                $('#ContentPlaceHolder1_btnSubmitModal').hide();
                $('#ContentPlaceHolder1_btnActivate').attr('disabled', false);
                $('#errorModal').modal('show');
            }
            else if (today > exp) {
                if (!result.membershipOption =="Email Marketing Client")
                        alert("Membership Expired");
            }

            if (!result.isValid) {
                alert(result.message)
            }

        }
        function CheckOut(id) {
            var _msg = $get("ContentPlaceHolder1_qrCodeScan").value;
            $.ajax({
                url: 'api/CheckOut',
                headers: {
                    'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                },
                type: "get",
                async: true,
                data: {
                    memberId: _msg,
                    type: 'member'
                },
                success: function (result) {
                    alert(result);
                }
            });
        }
    </script>
</asp:Content>
