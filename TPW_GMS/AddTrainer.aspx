<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddTrainer.aspx.cs" Inherits="TPW_GMS.AddTrainer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
            $(document).ready(function binddatatable() {
                $.ajax({
                    type: 'GET',
                    dataType: 'json',
                    url: '/api/GetAllTrainer',
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                    },
                    success: function (data) {
                        var datatableVariable = $('#TrainerTable').DataTable({
                            autoWidth: false,
                            data: data,
                            columns: [
                                {
                                    'data': 'id',
                                    render: function (data, type, row, meta) {
                                        return meta.row + meta.settings._iDisplayStart + 1;
                                    }
                                }, 
                                { 'data': 'fullName' },
                                { 'data': 'catagory' },
                                { 'data': 'contactNo' },
                                { 'data': 'address' },
                                { 'data': 'availableTime' },
                                { 'data': 'associateBranch' },
                                {
                                    "data": "joinDate",
                                    "render": function (data) {
                                        var date = new Date(data);
                                        var month = date.getMonth() + 1;
                                        return (month.length > 1 ? month : "0" + month) + "/" + date.getDate() + "/" + date.getFullYear();
                                    }
                                },
                                { 'data': 'discountCode' }, 
                                { 'data': 'status' }, 
                                {
                                    'data': null,
                                    render: function (data, type, full, meta) {
                                        if (data.image == "" || data.image == null) {
                                            return "<span style='color:red;font-weight: bolder;font-family: initial;'>No</span>";
                                        }
                                        else {

                                            return "<span style='color:green;font-weight: bolder;font-family: initial;'>Yes</span>";
                                        }
                                    }  
                                }, 
                                {
                                    data: "trainerId",
                                    className: "center",
                                    render: function (data, type, full, meta) {
                                        return '<a href="AddTrainer.aspx?id=' + data + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a> &nbsp;&nbsp;&nbsp; <a href="AddTrainer.aspx?id=' + data + "&" + "key=delete" + '" class="editor_remove"><img src="Assets/Icon/delete.png" class="iconDelete" /></a>';
                                    }
                                }
                            ]
                        });                       
                    }
                });

            });    
        
        </script>
    <asp:HiddenField ID="hidHeader" runat="server" Value="Trainer Management" />
    <asp:UpdatePanel ID="pnlSellItem" runat="server">
        <Triggers>
              <asp:PostBackTrigger ControlID="btnAddTrainer" />    
            <asp:PostBackTrigger ControlID="btnEditTrainer" />    
          </Triggers>
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-info">
                        <div class="box-title">
                        </div>
                        <div class="box-body">
                            <div class="col-sm-3">
                                Full Name<span class="asterik">*</span>
                                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-3">
                                Catagory<span class="asterik">*</span>
                                 <asp:DropDownList ID="ddlCatagory" runat="server" CssClass="form-control input-sm" >
                                     <asp:ListItem Value="0" Text="--select--"></asp:ListItem>
                                     <asp:ListItem Value="1" Text="Class A"></asp:ListItem>
                                     <asp:ListItem Value="2" Text="Class B"></asp:ListItem>
                                     <asp:ListItem Value="3" Text="Class C"></asp:ListItem>
                                 </asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                Contact NO<span class="asterik">*</span>
                                <asp:TextBox ID="txtContactNo" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-3">
                                Address<span class="asterik">*</span>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-3">
                                Available Time<span class="asterik">*</span>
                                <asp:TextBox ID="txtAvailableTime" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-3">
                                Associate Branch<span class="asterik">*</span>
                                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control input-sm" ></asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                TPW Join Date<span class="asterik">*</span>
                                <asp:TextBox ID="txtTPWJoinDate" runat="server" CssClass="form-control input-sm dateControl" />
                            </div>
                            <div class="col-sm-3">
                                Discount Code<span class="asterik">*</span>
                                <asp:TextBox ID="txtTrainerDiscountCode" runat="server" CssClass="form-control input-sm input-sm" />
                            </div>
                            <div class="col-sm-3">
                                commission(%)<span class="asterik">*</span>
                                <asp:TextBox ID="txtCommissionPercentage" runat="server" Text="5" CssClass="form-control input-sm input-sm" />
                            </div>
                            <div class="col-sm-3">
                                Image
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                            </div>
                            <div class="col-sm-3">
                                Status<br />
                                <asp:CheckBox ID="chkStatus" runat="server" />
                                <asp:HiddenField ID="hidSnNo" runat="server" />
                            </div>
                            <div class="col-sm-12">
                                Experience<span class="asterik">*</span>
                                <asp:TextBox ID="txtExperience" runat="server" TextMode="MultiLine" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-12">
                                <asp:Button ID="btnAddTrainer" runat="server" Enabled="true" Text="Submit" CssClass="btn btn-success" OnClick="btnAddTrainer_Click" />
                                <asp:Button ID="btnEditTrainer" runat="server" Enabled="false" Text="Edit" CssClass="btn btn-danger btn-a" OnClick="btnEditTrainer_Click" />
                                <asp:Label ID="lblInfo" Style="margin-left: 0px" runat="server"></asp:Label>
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
                        <table id="TrainerTable" style="font-size: 12px" class="table table-bordered table-hover">
                            <thead>
                                <tr class="header">
                                    <th>Sn.</th>
                                    <th>Full Name</th>
                                    <th>Catagory</th>
                                    <th>Contact No</th>
                                    <th>Address</th>
                                    <th>Available Time</th>
                                    <th>Associate Branch</th>
                                    <th>TPW Join Date</th>
                                    <th>Discount Code</th>
                                    <th>Status</th>
                                    <th>Image</th>
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
