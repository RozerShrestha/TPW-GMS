<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdmittedAndRenewed.aspx.cs" Inherits="TPW_GMS.AdmittedAndRenewed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {

            $('#endDate').val(ADToBS($.format.date(new Date(), 'yyyy/MM/dd')))
            $('#ddlDuration').change(() => {
                OnChangeReportDuration()
            });
            $('#btnSubmit').on('click', function () {
                loadData();
            });
        });
        function BSToAD(bs) {
            let bsObj = NepaliFunctions.ConvertToDateObject(bs, "YYYY/MM/DD");
            let adObj = NepaliFunctions.BS2AD(bsObj);
            let ad = NepaliFunctions.ConvertDateFormat(adObj, "YYYY/MM/DD");
            return ad;
        }
        function ADToBS(ad) {
            let adObj = NepaliFunctions.ConvertToDateObject(ad, "YYYY/MM/DD");
            let bsObj = NepaliFunctions.AD2BS(adObj);
            let bs = NepaliFunctions.ConvertDateFormat(bsObj, "YYYY/MM/DD");
            return bs;
        }
        function OnChangeReportDuration() {
            var todayDate = new Date();
            var reportDuration = $('#ddlDuration').val();
            var startDate = $('#startDate');
            if (reportDuration == "Daily") {
                startDate.val(ADToBS($.format.date(todayDate, 'yyyy/MM/dd')));

            }
            else if (reportDuration == "Weekly") {
                startDate.val(ADToBS($.format.date(todayDate.setDate(todayDate.getDate() - 7), 'yyyy/MM/dd')));
            }
            else if (reportDuration == "Monthly") {
                startDate.val(ADToBS($.format.date(todayDate.setMonth(todayDate.getMonth() - 1), 'yyyy/MM/dd')));
            }
        }
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
                ordering: false,
                autoWidth: true,
                lengthMenu: [[25, 50, -1], [25, 50, "All"]],
                language: {
                    sLoadingRecords: '<span style="width:100%;"><img src="Assets/Images/ajax-loader.gif"></span>'
                },
                dom: 'Bfrtip',
                buttons: [
                    'excelHtml5',
                ],
                ajax: {
                    url: 'api/GetAdmittedAndRenew/',
                    data: formData,
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    type: "POST",
                    dataType: "JSON",
                    dataSrc: "",
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                },
                columns: [
                    { 'data': 'memberId' },
                    { 'data': 'fullname' },
                    { 'data': 'contactNo' },
                    { 'data': 'shift' },
                    {
                        "data": "memberDate",
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
                        "data": "memberBeginDate",
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
                        "data": "memberExpireDate",
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
                        "data": "created",
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
                    { 'data': 'memberOption' },
                    { 'data': 'memberCatagory' },
                    { 'data': 'memberPaymentType' },
                   /* { 'data': 'email' },*/
                    { 'data': 'branch' },
                    { 'data': 'receiptNo' },
                    { 'data': 'paidAmount' },
                    { 'data': 'dueAmount' },
                    { 'data': 'dueClearAmount' },
                    { 'data': 'finalAmount' },
                    {'data': null},
                ],
                columnDefs: [{
                    // puts a button in the last column
                    data: "memberId",
                    className: "center",
                    targets: [-1], render: function (data, type, full, meta) {
                       return '<a href="EditForm.aspx?id=' + data.memberId + "&" + "key=view" + '" class="editAsset" target="_blank"><img src="Assets/Icon/view.png" class="iconView" /></a>';
                    }
                }],
            });
        }
    </script>

    <asp:HiddenField ID="hidHeader" runat="server" Value="Admitted And Renew Report" />
    <div class="box box-info">
        <div class="box-body">
            <div class="col-xs-12">
                <div class="row">
                    <div class="col-sm-2">
                        <strong>Branch</strong>
                        <asp:DropDownList ID="branch" ClientIDMode="Static" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <strong>Report Type</strong>
                        <asp:DropDownList ID="reportType" ClientIDMode="Static" runat="server" CssClass="form-control input-sm">
                            <asp:ListItem Value="newAdmitted" Text="New Admitted"></asp:ListItem>
                            <asp:ListItem Value="renewed" Text="Renewed"></asp:ListItem>
                            <asp:ListItem Value="extended" Text="Extended"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <strong>Report Duration</strong>
                        <asp:DropDownList ID="ddlDuration" runat="server" ClientIDMode="Static" CssClass="form-control input-sm">
                            <asp:ListItem Value="Select" Text="Select"></asp:ListItem>
                            <asp:ListItem Value="Daily" Text="Daily"></asp:ListItem>
                            <asp:ListItem Value="Weekly" Text="Weekly"></asp:ListItem>
                            <asp:ListItem Value="Monthly" Text="Monthly"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <strong>Start Date</strong>
                        <asp:TextBox ID="startDate" runat="server" ClientIDMode="Static" placeholder="start date" CssClass="form-control input-sm nepCalendar"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <strong>End Date</strong>
                        <asp:TextBox ID="endDate" runat="server" ClientIDMode="Static" placeholder="End date" CssClass="form-control input-sm nepCalendar"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2">
                        <strong>Membership Option</strong>
                        <asp:DropDownList ID="membershipOption" runat="server" Style="border-bottom-color: black; border-bottom-width: 1.5px" CssClass="form-control input-sm">
                            <asp:ListItem Value="ALL" Text="ALL"></asp:ListItem>
                            <asp:ListItem Value="Regular" Text="Regular"></asp:ListItem>
                            <asp:ListItem Value="OffHour" Text="OffHour"></asp:ListItem>
                            <asp:ListItem Value="Universal" Text="Universal"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <strong>Catagory</strong>
                        <asp:DropDownList ID="catagory" runat="server" Style="border-bottom-color: black; border-bottom-width: 1.5px" CssClass="form-control input-sm">
                            <asp:ListItem Value="ALL" Text="ALL"></asp:ListItem>
                            <asp:ListItem Value="Any1" Text="Any1"></asp:ListItem>
                            <asp:ListItem Value="Any2" Text="Any2"></asp:ListItem>
                            <asp:ListItem Value="Any3" Text="Any3"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <strong>Duration</strong>
                        <asp:DropDownList ID="duration" runat="server" Style="border-bottom-color: black; border-bottom-width: 1.5px" CssClass="form-control input-sm">
                            <asp:ListItem Value="ALL" Text="ALL"></asp:ListItem>
                            <asp:ListItem Value="1 Month" Text="1 Month"></asp:ListItem>
                            <asp:ListItem Value="3 Month" Text="3 Month"></asp:ListItem>
                            <asp:ListItem Value="6 Month" Text="6 Month"></asp:ListItem>
                            <asp:ListItem Value="12 Month" Text="12 Month"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <strong>Shift</strong>
                        <asp:DropDownList ID="shift" runat="server" Style="border-bottom-color: black; border-bottom-width: 1.5px" CssClass="form-control input-sm">
                            <asp:ListItem Value="ALL" Text="ALL"></asp:ListItem>
                            <asp:ListItem Value="Morning" Text="Morning"></asp:ListItem>
                            <asp:ListItem Value="Day" Text="Day"></asp:ListItem>
                            <asp:ListItem Value="Evening" Text="Evening"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <input type="button" id="btnSubmit" style="margin-top: 17px" class="btn btn-sm btn-success" name="Submit" value="Submit" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="table-responsive">
                        <table id="dtTable" class="table table-striped table-bordered table-sm" style="width: 100%">
                            <thead>
                                <tr class="border-bottom-0 tr-header header">
                                    <th>Member Id</th>
                                    <th>Full Name</th>
                                    <th>Mobile No</th>
                                    <th>Shift</th>
                                    <th>Membership Date</th>
                                    <th>Renew Date </th>
                                    <th>Expired Date </th>
                                    <th>Payment Date</th>
                                    <th>Membership Option </th>
                                    <th>Catagory </th>
                                    <th>Payment Type </th>
<%--                                    <th>Email</th>--%>
                                    <th>Branch </th>
                                    <th>Receipt</th>
                                    <th>Paid Amount</th>
                                    <th>Due Amount</th>
                                    <th>Due Clear Amount</th>
                                    <th>Fee</th>
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
</asp:Content>
