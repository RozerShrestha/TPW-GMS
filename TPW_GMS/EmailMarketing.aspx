<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmailMarketing.aspx.cs" Inherits="TPW_GMS.EmailMarketing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            loadData();
            SendEmailButtonVisibility();
            $('#btnSubmit').click(function () {
                $("#imgLoading").css("display", "block");
                $.ajax({
                    url: 'api/SendEmailMarketingBulk',
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                    type: "get",
                    async: true,
                    success: function (response) {
                        alert(response);
                        loadData();
                        $("#imgLoading").css("display", "none");
                    },
                    error: function () {

                    }
                })
            });
            });  
        function loadData() {
            var existingTbl = $('#EmailMarketingTable').DataTable();
            if (existingTbl) {
                $('#EmailMarketingTable').DataTable().destroy();
            }
            var table = $('#EmailMarketingTable').DataTable({
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
                    url: 'api/GetEmailMarketingData',
                    data: {
                        branch: $('#lblUserLogin').text().split('-')[0]
                    },
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                    dataSrc: function (data) {
                        expData = data;
                        return expData;
                    },
                },
                rowCallback: function (row, data) {
                    if (data["flag"] == true) {
                        //$(row).css("background-color", "green");
                        $(row).css("color", "green");
                        $(row).addClass('highlight')
                    }
                    else if (data["flag"] == false && data["mailCount"]==4) {
                        $(row).css("color", "red");
                    }
                    //if (ages.indexOf(data['fullname']) > 0) {
                    //    $(row).css("background-color", "red");
                    //    $(row).addClass('highlight')
                    //}
                },
                columns: [
                    {
                        'data': 'id',
                    },
                    { 'data': 'branch' },
                    { 'data': 'name' },
                    { 'data': 'email' },
                    { 'data': 'mobile' },
                    {
                        'data': 'createdDate',
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
                        'data': 'emailSendDate',
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
                        'data': 'flag',
                    },
                    {
                        'data': 'attCount',
                    },
                    {
                        'data': 'mailCount',
                    },
                    {
                        data: "id",
                        className: "center",
                        render: function (data, type, full, meta) {
                            
                            return '<a href="EmailMarketing.aspx?id=' + data + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a> &nbsp;&nbsp;&nbsp; <a href="EmailMarketing.aspx?id=' + data + "&" + "key=delete" + '" class="editor_remove"><img src="Assets/Icon/delete.png" class="iconDelete" /></a>';
                        }
                    }
                ]
            });
        }
        function SendEmailButtonVisibility() {
            var loginUser = $('#lblUserLogin').text().split('-')[0];
            if (loginUser=='admin')
                $("#btnSubmit").css("display", "block")
            else
                $("#btnSubmit").css("display", "none");
        }
        
        </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlEmailMarketing" runat="server">
                <div class="row">
                    <div class="box box-info">
                        <div class="box-title">
                            <asp:HiddenField ID="hidHeader" runat="server" Value="Email Marketing/Walking Customer" />
                            <asp:HiddenField ID="hidSnNo" runat="server" />
                        </div>
                        <div class="box-body">
                            <div class="col-xs-12">
                                <div class="col-sm-3">
                                    Customer Name<span class="asterik">*</span>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                                <div class="col-sm-3">
                                    Email<span class="asterik">*</span>
                                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    Mobile Number
                                <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    Branch<span class="asterik">*</span>
                                    <%--<asp:TextBox ID="txtBranch" runat="server" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>--%>
                                     <asp:DropDownList ID="ddlBranch" runat="server"  CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-12" style="margin-top: 10px">
                                    <asp:Button ID="btnAdd" runat="server" Enabled="true" Text="Submit" CssClass="btn btn-sm btn-success" OnClick="btnAdd_Click" />
                                    <asp:Button ID="btnEdit" runat="server" Enabled="false" Text="Edit" CssClass="btn btn-sm btn-danger btn-a" OnClick="btnEdit_Click" />
                                    <asp:Label ID="lblInfo" Style="margin-left: 4px" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-xs-12">
            <%--<input type="button" id="btnSubmit" style="margin-top:17px" class="btn btn-sm btn-success" name="Send Marketing Email" value="Send Marketing Email" />--%>
            <img id="imgLoading" src="Assets/Images/ajax-loader.gif" style="margin-top: 14px; width: 226px; display:none" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <div class="box box-solid">
                <div class="box-body">
                    <div class="table-responsive">
                        <table id="EmailMarketingTable" style="font-size: 12px" class="table table-bordered table-hover">
                            <thead>
                                <tr class="header">
                                    <th>Sn.</th>
                                    <th>Branch</th>
                                    <th>Name</th>
                                    <th>Email</th>
                                    <th>Mobile</th>
                                    <th>Created</th>
                                    <th>Email Send Date</th>
                                    <th>Flag</th>
                                    <th>Att Count</th>
                                    <th>Mail Count</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
