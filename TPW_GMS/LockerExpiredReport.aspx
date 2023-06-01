<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LockerExpiredReport.aspx.cs" Inherits="TPW_GMS.LockerExpiredReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var table = null;
        $(document).ready(function () {
            $('#btnUpdate').on('click', function () {
                updateData();
            });
            $('#btnSubmit').on('click', function () {
                loadData();
            });
            loadData();
        });

        function loadData() {
            var form = $('form#ctl01').serializeArray();
            var formData = [];
            form.forEach(function (item) {
                formData[item.name.split('$')[2]] = item.value;
            });
            if (table) {
                $('#dtTable').DataTable().destroy();
            }
            table = $('#dtTable').DataTable({
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
                    url: 'api/GetLockerExpiry/',
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
                    {
                        'data': 'id',
                        render: function (data, type, row, meta) {
                            return meta.row + meta.settings._iDisplayStart + 1;
                        }
                    },
                    { 'data': 'l.branch' },
                    {
                        'data': 'l.memberId',
                        "render": function (data) {
                            return data == null ? "" : data;
                        }
                    },
                    {
                        'data': 'm.fullname',
                        "render": function (data) {
                            return data == null ? "" : data;
                        } },
                    {
                        'data': 'm.contactNo',
                        "render": function (data) {
                            return data == null ? "" : data;
                        } },
                    { 'data': 'l.lockerNumber' },
                    {
                        'data': 'l.duration',
                        "render": function (data) {
                            return data == null ? "" : data;
                        } },
                    {
                        'data': 'l.amount',
                        "render": function (data) {
                            return data == null ? "" : data;
                        } },
                    {
                        "data": "l.renewDate",
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
                        "data": "l.expireDate",
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
                    { 'data': 'l.paymentMethod' },
                    {
                        'data': 'l.receiptNo',
                        "render": function (data) {
                            return data == null ? "" : data;
                        } },
                    {
                        "data": "l.created",
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
                        "data": "l.modified",
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
                        "data": "l.expireDate",
                        "render": function (data) {
                            let todayDate = new Date();
                            let rowDate = new Date(data);
                            let status = data == null ? "Unassigned":rowDate >= todayDate ? "Active" : rowDate < todayDate ? "Expired" : "Unknown";
                            return status == "Active" ? `<p class="text-success">${status}</p>` : status == "Expired" ? `<p class="text-danger">${status}</p>` : status == "Unassigned" ? `<p class="text-primary">${status}</p>` : `<p class="text-muted">${status}</p>`;
                        }
                    },
                    {
                        'data': null,
                        'className': 'center',
                        defaultContent: '<a href="#" class="editor_View">View</a>'
                    }
                ],
                rowCallback: function (row, data) {
                    if (data.l["callStatus"] == "Called") {
                        $(row).addClass('success')
                    }
                    else if (data.l["callStatus"] == "Called but didn't received") {
                        $(row).addClass('danger')
                    }

                },
            });

            $(document).on('click', '#dtTable a.editor_View', function () {
                var data = table.row($(this).closest('tr')).data();
                $('#txtMemberId').val(data.l['memberId']);
                $('#ddlCallStatus').val(data.l['callStatus']);
                $('#txtRemark').val(data.l['callRemark']);

                $('#modalRegister').modal("show");
            });
        }

        function updateData() {
            const params = new URLSearchParams();
            params.append('memberId', $('#txtMemberId').val());
            params.append('callStatus', $('#ddlCallStatus').val());
            params.append('callRemark', $('#txtRemark').val());
            axios({
                url: 'api/UpdateLockerInformation',
                method: 'post',
                data: params,
                dataType: "JSON",
                headers: {
                    'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                },
            }).then(response => {
                $('#lblMessage').text(`${response.data} now reloading...`);
                setTimeout(() => {
                    location.reload(true);
                }, 1000);


            }).catch(function (error) {
                $('#lblMessage').text(error);
            })
        }

        //data - target="#modalRegister"
    </script>
    <asp:HiddenField ID="hidHeader" runat="server" Value="Locker Report" />
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
                        <strong>Months</strong>
                        <asp:DropDownList ID="reportType" ClientIDMode="Static" runat="server" CssClass="form-control input-sm">
                            <asp:ListItem Value="0" Text="All"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Active"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Expired"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Expires within 7 Days"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <input type="button" id="btnSubmit" style="margin-top:17px" class="btn btn-sm btn-success" name="Submit" value="Submit" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="table-responsive">
                        <table id="dtTable" class="table table-striped table-bordered table-sm" style="width: 100%">
                            <thead>
                                <tr class="border-bottom-0 tr-header header">
                                    <th>Sn.</th>
                                    <th>Branch</th>
                                    <th>Member Id</th>
                                    <th>Full Name</th>
                                    <th>Mobile</th>
                                    <th>Locker No</th>
                                    <th>Duration</th>
                                    <th>Amount</th>
                                    <th>Renewed</th>
                                    <th>Expired</th>
                                    <th>Payment</th>
                                    <th>Receipt</th>
                                    <th>Created</th>
                                    <th>Modified</th>
                                    <th>Status</th>
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

    <div id="modalRegister" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title" style="text-align-last: center">Call Monitoring</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-12">
                        Member Id
                         <input type="text" id="txtMemberId" readonly name="txtMemberId" class="form-control input-sm" value=""/>
                    </div>
                    <div class="col-md-12">
                        Call Status
                         <select name="ddlCallStatus" id="ddlCallStatus" class="form-control input-sm">
                          <option value="Called">Called</option>
                          <option value="Called but didn't received">Called but didn't received</option>
                          <option value="Not Called">Not Called</option>
                             
                        </select>
                    </div>
                    <div class="col-md-12">
                        Remark
                         <textarea id="txtRemark" name="txtRemark" class="form-control input-sm"></textarea>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <label id="lblMessage" style="float:left"></label>
                <button type="button" id="btnUpdate" class="btn btn-success">Update</button>
                <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>
</asp:Content>