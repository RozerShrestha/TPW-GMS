<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmailMarketingCSV.aspx.cs" Inherits="TPW_GMS.EmailMarketingCSV" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            $('#btnSubmit').on('click', function () {
                loadData();
            });
        });

        function loadData() {
            var form = $('form#ctl01').serializeArray();
            var formData = [];
            form.forEach(function (item) {
                if (item.name.split('$')[2] != null) {
                    formData[item.name.split('$')[2]] = item.value;
                }
            });
            formData['who'] = $('#lblUserLogin').text().split('-')[0] == 'admin' ? 'admin' : 'branch';
            var existingTbl = $('#dtTable').DataTable();
            if (existingTbl) {
                $('#dtTable').DataTable().destroy();
            }
            var table = $('#dtTable').dataTable({
                fixedHeader: {
                    header: true,
                    headerOffset: 50,
                },
                ordering:false,
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
                    url: 'api/GetEmailMarketingCSVList/',
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
                    { 'data': 'email' },
                    { 'data': 'firstName' },
                    { 'data': 'lastName' }, 
                    {
                        'data': 'memberExpireDate',
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
                ],
            });
        }
    </script>
    <asp:UpdatePanel ID="upnl" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info">
                        <div class="box-title">
                            <asp:HiddenField ID="hidHeader" runat="server" Value="Email Marketing CSV" />
                        </div>
                        <div class="box-body">
                            <div class="col-sm-2 col-md-2">
                                Branch
                                <asp:DropDownList ID="branch" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="col-sm-2 col-md-2">
                                Membership Type
                                <asp:DropDownList ID="membershipType" runat="server" CssClass="form-control" AutoPostBack="true">
                                    <asp:ListItem Value="ALL" Text="All"></asp:ListItem>
                                    <asp:ListItem Value="Active" Text="Active"></asp:ListItem>
                                    <asp:ListItem Value="InActive" Text="InActive"></asp:ListItem>
                                    <asp:ListItem Value="1_Month_Expired" Text="1 Month Expired"></asp:ListItem>
                                </asp:DropDownList>
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
        <div class="box box-solid">
            <div class="box-body">
                <div class="col-xs-12">
                    <div class="table-responsive">
                        <div class="col-lg-12">
                            <table id="dtTable" style="font-size: 12px" class="table table-bordered table-hover">
                                <thead>
                                    <tr class="header">
                                        <th>Email</th>
                                        <th>First Name</th>
                                        <th>Last Name</th>
                                        <th>Member Expired Date</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
