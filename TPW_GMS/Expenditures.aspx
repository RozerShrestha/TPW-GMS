<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Expenditures.aspx.cs" Inherits="TPW_GMS.Expenditures" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <head>
        <style type="text/css">
            .showHide {
                cursor: pointer;
            }
        </style>  
        <script type="text/javascript">
            $(document).ready(function binddatatable() {
                $.ajax({
                    type: "GET",
                    dataType: "json",
                    url: "/api/GetAllExpenditures",
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                    success: function (data) {
                        var datatableVariable = $('#ExpenditureTable').DataTable({
                            autoWidth: false,
                            'data': data.filter(function (item) {
                                if ('<%=hidUserLogin.Value.ToString()%>' == 'admin')
                                    return true;
                                else if (item['branch'] == '<%=hidUserLogin.Value.ToString()%>')
                                    return true;
                                else
                                    return false;
                            }),
                            columns: [
                                {
                                    'data': 'id',
                                    render: function (data, type, row, meta) {
                                        return meta.row + meta.settings._iDisplayStart + 1;
                                    }
                                },
                                {
                                     "data": "expenditureDate",
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
                                { 'data': 'expenditureType' },
                                { 'data': 'expenditureRate' },
                                { 'data': 'branch' },
                                {
                                    data: "expenditureId",
                                    className: "center",
                                    render: function (data, type, full, meta) {
                                        return '<a href="Expenditures.aspx?id=' + data + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a> &nbsp;&nbsp;&nbsp; <a href="Expenditures.aspx?id=' + data + "&" + "key=delete" + '" class="editor_remove"><img src="Assets/Icon/delete.png" class="iconDelete" /></a>';
                                    }
                                }
                            ]
                        });
                    }
                });
            });
        </script>        
    </head>
    <asp:HiddenField ID="hid" runat="server" />
    <asp:HiddenField ID="hidUserLogin" runat="server" />
    <asp:HiddenField ID="hidHeader" runat="server" Value="Expenditures" />
    <asp:UpdatePanel ID="upnlExpenditure" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="box box-info">
                    <div class="box-title">      
                    </div>
                    <div class="box-body">
                        <div class="col-xs-12">
                            <div class="col-sm-3">
                                Date
                        <asp:TextBox ID="txtExpenditureDate" runat="server" ClientIDMode="Static" CssClass="form-control input-sm nepCalendar"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Type of Expenditure
                        <asp:TextBox ID="txtTypeOfExpenditure" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Rate
                        <asp:TextBox ID="txtExpenditureRate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Branch
                        <asp:TextBox ID="txtBranch" runat="server" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-sm-12">
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success" Text="Submit" OnClientClick="binddatatable()" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-danger" Text="Edit" Enabled="false" OnClientClick="binddatatable()" OnClick="btnEdit_Click" />
                                <asp:Label ID="lblInfo" runat="server"></asp:Label>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <div class="row">
        <div class="col-xs-12">
            <div class="box box-solid">
                <div class="box-body">
                    <div class="table-responsive">
                        <table id="ExpenditureTable" style="font-size: 12px" class="table table-bordered table-hover">
                            <thead>
                                <tr class="header">
                                    <th width="34px">Sn.</th>
                                    <th width="180px">Date</th>
                                    <th width="400px">Type of Expenditure</th>
                                    <th width="180px">Rate</th>
                                    <th width="180px">Branch</th>
                                    <th width="100px">Action</th>
                                </tr>
                            </thead>
                        </table>
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
