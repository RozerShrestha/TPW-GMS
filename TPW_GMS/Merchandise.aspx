<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Merchandise.aspx.cs" Inherits="TPW_GMS.Merchandise" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
            $(document).ready(function binddatatable() {
                $.ajax({
                    type: 'GET',
                    dataType: 'json',
                    url: 'api/GetMerchan',
                    data: {
                        branch: $('#lblUserLogin').text().split('-')[0]
                    }, 
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                    success: function (data) {
                        var datatableVariable = $('#MerchanTable').DataTable({
                            autoWidth: false,
                            data: data,
                            columns: [
                                {
                                    'data': 'id',
                                    render: function (data, type, row, meta) {
                                        return meta.row + meta.settings._iDisplayStart + 1;
                                    }
                                },
                                { 'data': 'merchandiseName' },
                                {'data':'branch'},
                                { 'data': 'merchandiseType' },
                                { 'data': 'merchandiseQuantity' },
                                { 'data': 'merchandisePrice' },                 
                                {
                                    'data': null,
                                    render: function (data, type, JsonResultRow, meta) {
                                        if(data.image!=null)
                                            return '<img src="' + data.image + '" style="height:100px; width:100px;">';
                                        else
                                            return '<img src="Image/NoImage.jpg" style="height:100px; width:100px;">';
                                    }
                                },
                                { 'data': 'MerchandiseStatus' },
                                {
                                    data: "merchandiseId",
                                    className: "center",
                                    render: function (data, type, full, meta) {
                                        return '<a href="Merchandise.aspx?id=' + data + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a> &nbsp;&nbsp;&nbsp; <a href="Merchandise.aspx?id=' + data + "&" + "key=delete" + '" class="editor_remove"><img src="Assets/Icon/delete.png" class="iconDelete" /></a>';
                                    }
                                }
                            ]
                        });                       
                    }
                });
         });      
        </script>

    <asp:UpdatePanel runat="server">
        <Triggers>
            <%--File Upload problem if removed--%>
            <asp:PostBackTrigger ControlID="btnAddMerchanItem" />
            <asp:PostBackTrigger ControlID="btnEditMerchanItem" />
        </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="box box-info">
                    <div class="box-title">
                        <asp:HiddenField ID="hidHeader" runat="server" Value="Merchandise/Supplements Details" />
                    </div>
                    <div class="box-body">
                        <div class="col-xs-12">
                            <div class="col-sm-3">
                                Name
                                <asp:TextBox ID="txtMerchandiseName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Branch
                                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                Type
                                 <asp:DropDownList ID="ddlType" CssClass="form-control input-sm" runat="server">
                                     <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                     <asp:ListItem Value="1" Text="M"></asp:ListItem>
                                     <asp:ListItem Value="2" Text="S"></asp:ListItem>
                                 </asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                Quantity
                                <asp:TextBox ID="txtQuantity" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Price
                                <asp:TextBox ID="txtPrice" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                Image
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                            </div>
                            <div class="col-sm-3">
                                Status<br />
                                <asp:CheckBox ID="chkStatus" runat="server"></asp:CheckBox>
                            </div>       
                            <div class="col-sm-3">
                                <asp:HiddenField ID="hidSnNo" runat="server" />
                            </div>
                            <div class="col-md-12" style="margin-top: 10px">
                                <asp:Button ID="btnAddMerchanItem" runat="server" Enabled="true" Text="Submit" CssClass="btn btn-sm btn-success" OnClick="btnAddMerchanItem_Click" />
                                <asp:Button ID="btnEditMerchanItem" runat="server" Enabled="false" Text="Edit" CssClass="btn btn-sm btn-danger btn-a" OnClick="btnEditMerchanItem_Click" />
                                <asp:Label ID="lblInfo" Style="margin-left: 4px" runat="server"></asp:Label>
                                </td>
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
                        <table id="MerchanTable" style="font-size: 12px" class="table table-bordered table-hover">
                            <thead>
                                <tr class="header">
                                    <th>Sn.</th>
                                    <th>Name</th>
                                    <th>Branch</th>
                                    <th>Type</th>
                                    <th>Quantity</th>
                                    <th>Price</th>
                                    <th>Image</th>
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
   


    <div class="modal fade" id="errorModal" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Error</h4>
                </div>
                <div class="modal-body">
                    Error has occured:
                    <asp:Label ID="lblPopupErrorr" runat="server" ForeColor="Red" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Close</button>
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
