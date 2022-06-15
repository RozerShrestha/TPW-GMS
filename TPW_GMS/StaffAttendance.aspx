<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StaffAttendance.aspx.cs" Inherits="TPW_GMS.StaffAttendance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:HiddenField ID="hidHeader" runat="server" Value="Staff Attendance" />
        <asp:HiddenField ID="hidCurrentLoginBranch" runat="server" />
    <br />
    <asp:UpdatePanel ID="upnlAttendance" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="box box-info">
                    <div class="box-body">
                        <div class="col-sm-12">
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtQrCodeStaff" runat="server" CssClass="form-control input-sm" autofocus></asp:TextBox>
                            </div>
                            <div>
                                <input type="button" id="btnReload" name="Reload" style="margin-top: -7px" value="Reload" class="btn btn-danger" onclick="window.location.reload()" />
                            </div>
                        </div>
                        <br />
                        <div class="col-sm-12">
                           <div class="box box-info">
                        <div class="box-body">
                            <div class="table-responsive">
                                <table id="tblStaffAttendance" style="font-size: 12px; width: 100%" class="table table-striped table-bordered table-sm">
                                    <thead>
                                        <tr class="border-bottom-0 tr-header header">
                                            <th style="min-width: 100px">MemberId</th>
                                            <th style="min-width: 100px">Name</th>
                                            <th>Type</th>
                                            <th>CheckIn</th>
                                            <th>CheckOut</th>
                                            <th>Branch</th>
                                            <th>Checkin Branch</th>
                                            <th>Remark</th>
                                            <th>Late Flag</th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </div>
                    </div>
                        </div>
                    </div>
                </div>

            </div>
            <div class="modal fade" id="myModal">
                <div class="modal-dialog">
                    <div class="modal-content">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">INFORMATION</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            <p>Dear All,</p>
                            <p>There is a small modification on Attendance system.</p>
                            <p>Please login to the app with your Username(mobile number) and Password(see in Email) or ask to Admin</p>
                            <p>Scan the QR to login into an App</p>
                            <img src="Assets/Images/qr.png" />
                            <p>Thank you.</p>
                        </div>

                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </div>

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
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
            document.getElementById("ContentPlaceHolder1_txtQrCodeStaff").addEventListener("keyup", function (event) {

                if (event.getModifierState("CapsLock")) {
                    alert("Please Turn Off the Caps Lock");
                    return;
                }
            });
            //$('#myModal').modal('show');
            //$("#trId").hide();
            $('#ContentPlaceHolder1_txtQrCodeStaff').on('keyup', debounce(function () {
                var _msg = $get("ContentPlaceHolder1_txtQrCodeStaff").value;
                var _branch = document.getElementById("lblUserLogin").innerHTML;
                    $.ajax({
                        url: 'api/StaffAttendance',
                        headers: {
                            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                        },
                        type: "get",
                        async: false,
                        data: {
                            encryptedMemberId: _msg,
                            loginBranch: _branch
                        },
                        success: function (result) {
                            if (result != "CheckOut") {
                                alert(result);

                            }
                            else {
                                var r = confirm("You have Already Checked IN \n Do you want to Check Out?");
                                if (r) {
                                    CheckOut(_msg);
                                }  
                            }
                            location.reload();
                        },
                        error: function () {
                            alert("error");
                        }
                    });
            }, 250));
        });
        function loadAttRecord() {
            var loginUser = $("#lblUserLogin").text().split('-')[0];
            var table = $('#tblStaffAttendance').DataTable({
                fixedHeader: {
                    header: true,
                    headerOffset: 50,
                },
                ordering: false,
                autoWidth: true,
                language: {
                    sLoadingRecords: '<span style="width:100%;"><img src="Assets/Images/ajax-loader.gif"></span>'
                },
                lengthMenu: [[25, 50, -1], [25, 50, "All"]],
                ajax: {
                    url: `api/GetStaffAttendanceList?loginUser=${loginUser}`,
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
                    { 'data': 'remark' },
                    { 'data': 'lateFlag' },
                ],
            })
        }
        function CheckOut(id) {
            var _msg = $get("ContentPlaceHolder1_txtQrCodeStaff").value;
            $.ajax({
                url: 'api/CheckOut',
                headers: {
                    'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                },
                type: "get",
                async: true,
                data: {
                    encryptedMemberId: _msg,
                    type:'staff'
                },
                success: function (result) {
                    alert(result);
                    location.reload();
                }
            });
        }
    </script>
</asp:Content>

    
    
