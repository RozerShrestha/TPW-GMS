<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="JoinTPWNow.aspx.cs" Inherits="TPW_GMS.JoinTPWNow" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
            .showHide {
                cursor: pointer;
            }
        </style>  
        
 <script type="text/javascript">   
        $(document).ready(function () {
            $.ajax({
                type: "GET",
                dataType: "json",
                url: "api/GetJoinTPW",
                headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                success: function (data) {
                    var datatableVariable = $('#JoinTPWsTable').DataTable({
                        autoWidth:false,
                        data: data,
                        columns: [
                            {
                                'data': 'id',
                                render: function (data, type, row, meta) {
                                    return meta.row + meta.settings._iDisplayStart + 1;
                                }
                            },
                            {
                                'data': 'date',
                                "render": function (data) {
                                    var date = new Date(data);
                                    var month = date.getMonth() + 1;
                                    return (month.length > 1 ? month : "0" + month) + "/" + date.getDate() + "/" + date.getFullYear();
                                }
                            },  
                            { 'data': 'memberId' },  
                            { 'data': 'firstName' },
                            { 'data': 'lastName' },
                            {'data':'address'},
                            { 'data': 'mobileNumber' },
                            { 'data': 'branch' },                           
                            { 'data': 'status' },    
                            { 'data': null },
                        ],
                        columnDefs: [{
                            data: "jId",
                            className: "center",
                            targets: [-1], render: function (data, type, full, meta) {
                                if ('<%=hidUserLogin.Value.ToString()%>' == 'admin') {
                                    return '<a href="JoinTPWNow.aspx?id=' + data.jId + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a> &nbsp;&nbsp;&nbsp; <a href="JoinTPWNow.aspx?id=' + data.jId + "&" + "key=delete" + '" class="editor_remove"><img src="Assets/Icon/delete.png" class="iconDelete" /></a>';
                                    }
                                    else {
                                        if (data.branch == '<%=hidUserLogin.Value.ToString()%>') {
                                            return '<a href="JoinTPWNow.aspx?id=' + data.jId + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a>';
                                        }
                                        else {
                                            return '<a href="JoinTPWNow.aspx?id=' + data.jId + "&" + "key=edit" + '" class="editAsset"></a>';
                                        }
                                    }
                                }
                        }],
                    });
                }
            });
        });      
    </script>
    <asp:HiddenField ID="hidHeader" runat="server" Value="Join TPW" />
   <asp:HiddenField ID="hidUserLogin" runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="box box-solid">
                    <div class="box-body">
                        <div class="col-xs-12">
                            <div class="col-sm-3">
                                Date
                <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>
                                <asp:HiddenField ID="hidSnNo" runat="server" />
                            </div>
                            <div class="col-sm-3">
                                Member Id
                <asp:TextBox ID="txtMemberId" runat="server" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                First Name
               <asp:TextBox ID="txtFirstName" Enabled="false" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Last Name
               <asp:TextBox ID="txtLastName" Enabled="false" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Address
               <asp:TextBox ID="txtAddress" Enabled="false" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Mobile Number
                <asp:TextBox ID="txtMobileNumber" Enabled="false" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Branch
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control input-sm">
                </asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                Status
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control input-sm">
                    <asp:ListItem Value="0" Text="Not Joined"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Joined"></asp:ListItem>

                </asp:DropDownList>
                            </div>
                            <div class="col-sm-12">
                                <%--<asp:Button ID="btnSubmit" runat="server" Style="margin-top: 19px;" CssClass="btn btn-success" Text="Submit" OnClientClick="binddatatable()" OnClick="btnSubmit_Click" />--%>
                                <asp:Button ID="btnEdit" runat="server" Style="margin-top: 19px;" CssClass="btn btn-danger" Text="Edit" OnClientClick="binddatatable()" OnClick="btnEdit_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
           <asp:Label ID="lblInformation" runat="server"></asp:Label>
         </ContentTemplate>
        </asp:UpdatePanel>
            <div class="row">
                <div class="box box-solid">
                    <div class="box-body">
                        <div class="col-xs-12">
                            <div class="table-responsive">
                                <div class="col-lg-12">
                                    <table id="JoinTPWsTable" style="font-size: 12px" class="table table-bordered table-hover">
                                        <thead>
                                            <tr class="header">
                                                <th>Sn.</th>
                                                <th>Date</th>
                                                <th>Member ID</th>
                                                <th>First Name</th>
                                                <th>Last Name</th>
                                                <th>Address</th>
                                                <th>Mobile Number</th>
                                                <th>Branch</th>
                                                <th>Status</th>
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
