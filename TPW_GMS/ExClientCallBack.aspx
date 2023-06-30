<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExClientCallBack.aspx.cs" Inherits="TPW_GMS.ExClientCallBack" %>
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
            formData["reportType"] = "ExCientCallBack";
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
                    url: 'api/GetMemberExpiry/',
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
                    { 'data': 'branch' },
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
                    { 'data': 'memberOption' },
                    { 'data': 'memberCatagory' },
                    { 'data': 'memberPaymentType' },
                    {
                        'data': null,
                        'className': 'center',
                        defaultContent: '<a href="#" class="editor_View">View</a>'
                    }
                ],
                rowCallback: function (row, data) {
                    if (data["callStatus"] == "Called") {
                        $(row).addClass('success')
                    }
                    else if (data["callStatus"] == "Called but didn't received") {
                        $(row).addClass('danger')
                    }

                },
            });

            $(document).on('click', '#dtTable a.editor_View', function () {
                var data = table.row($(this).closest('tr')).data();
                $('#txtMemberId').val(data['memberId']);
                $('#ddlCallStatus').val(data['callStatus']);
                $('#txtRemark').val(data['callRemark']);

                $('#modalRegister').modal("show");
            });
        }

        function updateData() {
            const params = new URLSearchParams();
            params.append('memberId', $('#txtMemberId').val());
            params.append('callStatus', $('#ddlCallStatus').val());
            params.append('callRemark', $('#txtRemark').val());
            axios({
                url: 'api/UpdateMemberInformation',
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
    <asp:HiddenField ID="hidHeader" runat="server" Value="Ex Client Call Back Report" />
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
                        <strong>StartDate</strong>
                        <asp:TextBox ID="startDate" ReadOnly runat="server" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                     <div class="col-sm-2">
                        <strong>EndDate</strong>
                        <asp:TextBox ID="endDate" ReadOnly runat="server" CssClass="form-control input-sm"></asp:TextBox>
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
                                    <th>Branch </th>
                                    <th>Member Id</th>
                                    <th>Full Name</th>
                                    <th>Mobile No</th>
                                    <th>Shift</th>
                                    <th>Membership Date</th>
                                    <th>Memebrship Expired Date</th>
                                    <th>Membership Option </th>
                                    <th>Catagory </th>
                                    <th>Payment Type </th>
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
