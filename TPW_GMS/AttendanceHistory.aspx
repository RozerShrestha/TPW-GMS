<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AttendanceHistory.aspx.cs" Inherits="TPW_GMS.AttendanceHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:HiddenField ID="hidHeader" runat="server" Value="Attendance History" />
    <script>
        $(document).ready(function () {
            $('#btnSubmit').on('click', function () {
                loadData();
            });
        });
        function calculateLateCount(data) {
            let lateCount = data.filter(item => item.LateFlag == true).length;
            //let salDeduction = $("#ContentPlaceHolder1_membershipOption").val() == "Operation Manager" ? Math.floor(lateCount / 6) : Math.floor(lateCount / 3);
            let salDeduction =  Math.floor(lateCount / 3);
            $("#lateCount").text(lateCount);
            $("#salDeduction").text(salDeduction);

            
        }
        function loadData() {
            var memType = $("#ContentPlaceHolder1_membershipOption").val();
            $('#ContentPlaceHolder1_branch').removeAttr('disabled');
            var form = $('form#ctl01').serializeArray();
            $('#ContentPlaceHolder1_branch').attr('disabled', 'disabled');
            var formData = [];
            form.forEach(function (item) {
                if (item.name.split('$')[2] != null) {
                    formData[item.name.split('$')[2]] = item.value;
                }
            });
            formData['who'] = $('#lblUserLogin').text().split('-')[0] == 'admin' || $('#lblUserLogin').text().split('-')[0]=='superadmin' ? 'admin' : 'branch';
            var existingTbl = $('#dtTable').DataTable();
            if (existingTbl) {
                $('#dtTable').DataTable().destroy();
            }
            var table = $('#dtTable').dataTable({
                fixedHeader: {
                    header: true,
                    headerOffset: 50,
                },
                ordering:true,
                autoWidth: true,
                lengthMenu: [[25, 50, -1], [25, 50, "All"]],
                language: {
                    sLoadingRecords: '<span style="width:100%;"><img src="Assets/Images/ajax-loader.gif"></span>'
                },
                dom: 'Bfrtip',
                buttons: [
                    'excelHtml5','csv'
                ],
                ajax: {
                    url: 'api/GetAttendanceHistoryAdmin/',
                    data: formData,
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    type: "POST",
                    dataType: "JSON",
                    dataSrc: function (data) {
                        if (memType == "Trainer" || memType == "Gym Admin" || memType == "Operation Manager" || memType == "Intern") {
                            $("#lateCountInfo").show();
                            calculateLateCount(data);
                        }
                        else {
                            $("#lateCountInfo").hide();
                            if (memType == "Staff") {
                                $("#lateDetail").show();
                                loadLateCountDetails(formData);
                            }
                        }
                        return data;
                    },
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                },
                columns: [
                    { 'data': 'MemberId' },
                    { 'data': 'FullName' },
                    { 'data': 'MemberOption' },
                    {
                        "data": "CheckIn",
                        "render": function (data) {
                            try {
                                var dat = data != null ? (data.split('T')[0]).split('-').join('/') : '';
                                let objEng = NepaliFunctions.ConvertToDateObject(dat, "YYYY/MM/DD");
                                let objNep = NepaliFunctions.AD2BS(objEng);
                                let dt = NepaliFunctions.ConvertDateFormat(objNep, "YYYY/MM/DD");

                                return `${dt}-${data.split('T')[1]}`;
                            } catch (e) {
                                return '';
                            }
                        }
                    },
                    {
                        "data": "CheckOut",
                        "render": function (data) {
                            try {
                                var dat = data != null ? (data.split('T')[0]).split('-').join('/') : '';
                                let objEng = NepaliFunctions.ConvertToDateObject(dat, "YYYY/MM/DD");
                                let objNep = NepaliFunctions.AD2BS(objEng);
                                let dt = NepaliFunctions.ConvertDateFormat(objNep, "YYYY/MM/DD");

                                return `${dt}-${data.split('T')[1]}`;
                            } catch (e) {
                                return '';
                            }
                        }
                    },
                    { 'data': 'Branch' },
                    { 'data': 'CheckInBranch' },
                    { 'data': 'Remark' },
                    { 'data': 'LateFlag' },
                ],
            });
        }
        function loadLateCountDetails(formData) {
            var existingTbl = $('#lateDetailTable').DataTable();
            if (existingTbl) {
                $('#lateDetailTable').DataTable().destroy();
            }
            var table = $('#lateDetailTable').dataTable({
                fixedHeader: {
                    header: true,
                    headerOffset: 50,
                },
                ordering: true,
                autoWidth: true,
                lengthMenu: [[25, 50, -1], [25, 50, "All"]],
                language: {
                    sLoadingRecords: '<span style="width:100%;"><img src="Assets/Images/ajax-loader.gif"></span>'
                },
                dom: 'Bfrtip',
                buttons: [
                    'excelHtml5', 'csv'
                ],
                ajax: {
                    url: 'api/GetLateCountDetail',
                    data: formData,
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    type: "POST",
                    dataType: "JSON",
                    dataSrc: function (data) {
                        return data;
                    },
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                },
                columns: [
                    { 'data': 'branch' },
                    { 'data': 'memberOption' },
                    { 'data': 'fullname' },
                    { 'data': 'lateCount' },
                    { 'data': 'salDeduct' },
                ],
            });
        }
    </script>
        <asp:UpdatePanel ID="upnl" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-info">
                            <div class="box-body">
                                <div class="col-sm-2 col-md-2">
                                    From Date
                                    <asp:TextBox ID="startDate" placeholder="yyyy/mm/dd" CssClass="form-control nepCalendar" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-2 col-md-2">
                                    To Date
                                                <asp:TextBox ID="endDate" placeholder="yyyy/mm/dd" CssClass="form-control nepCalendar" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-2 col-md-2">
                                    Membership Type
                                    <asp:DropDownList ID="membershipOption" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="membershipOption_SelectedIndexChanged">
                                        <asp:ListItem Value="--Select--" Text="--Select--"></asp:ListItem>
                                        <asp:ListItem Value="Regular" Text="Regular"></asp:ListItem>
                                        <asp:ListItem Value="OffHour" Text="OffHour"></asp:ListItem>
                                        <asp:ListItem Value="Universal" Text="Universal"></asp:ListItem>
                                        <asp:ListItem Value="Trainer" Text="Trainer"></asp:ListItem>
                                        <asp:ListItem Value="Gym Admin" Text="Gym Admin"></asp:ListItem>
                                        <asp:ListItem Value="Operation Manager" Text="Operation Manager"></asp:ListItem>
                                        <asp:ListItem Value="Intern" Text="Intern"></asp:ListItem>
                                        <asp:ListItem Value="Free User" Text="Free User"></asp:ListItem>
                                        <asp:ListItem Value="Staff" Text="Staff"></asp:ListItem>
                                        <asp:ListItem Value="Members" Text="Members"></asp:ListItem>
                                        <asp:ListItem Value="Guest" Text="Guest"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-2 col-md-2">
                                    Members
                                    <asp:DropDownList ID="memberId" CssClass="form-control select2Example" runat="server"></asp:DropDownList>
                                </div>
                                <div class="col-sm-2 col-md-2">
                                    Branch
                                    <asp:DropDownList ID="branch" runat="server" Enabled="false"  CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-sm-2 col-md-2">
                                    <%--<input type="button" id="btnSubmit" style="margin-top: 22px" class="btn btn-success" name="Submit" value="Submit" />--%>
                                    <asp:Button ID="btnSubmitTest" runat="server" Style="margin-top: 22px" ClientIDMode="Static" OnClientClick="loadData()" CssClass="btn btn-success" Text="Submit" />
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    <div class="row">
        <div class="col-md-8">
        </div>
        <div id="lateCountInfo" class="col-md-4" style="display:none">
            <div class="col-sm-6">LateCount</div>
            <div class="col-sm-6">Salary Deduction</div>
            <div class="col-sm-6"><span id="lateCount"></span></div>
            <div class="col-sm-6"><span id="salDeduction"></span></div>
        </div>
        <div class="col-md-12">
            <div class="box">
                <div class="box-body">
                    <div class="table-responsive">
                        <table id="dtTable" class="table table-hover table-bordered" style="width: 100%">
                            <thead>
                                <tr class="border-bottom-0 tr-header subheader">
                                    <th>Member Id</th>
                                    <th>Full Name</th>
                                    <th>Membership Option</th>
                                    <th style="width: 160px">CheckIn</th>
                                    <th style="width: 160px">CheckOut</th>
                                    <th>Branch</th>
                                    <th>CheckInBranch</th>
                                    <th>Remark </th>
                                    <th>LateFlag</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
                <div class="box-body">
                    <div id="lateDetail" class="table-responsive" style="display:none">
                        <table id="lateDetailTable" class="table table-hover table-bordered" style="width: 100%">
                            <thead>
                                <tr class="border-bottom-0 tr-header subheader">
                                    <th>Branch</th>
                                    <th>Member Option</th>
                                    <th>Name</th>
                                    <th>Late Count</th>
                                    <th>Deduction </th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>      
    </div>
       
</asp:Content>
