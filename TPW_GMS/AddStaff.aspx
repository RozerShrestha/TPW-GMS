<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddStaff.aspx.cs" Inherits="TPW_GMS.AddStaff" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function binddatatable() {
            $.ajax({
                type: 'GET',
                dataType: 'json',
                url: '/api/GetAllStaff',
                headers: {
                    'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                },
                success: function (data) {
                    var datatableVariable = $('#StaffTable').DataTable({
                        autoWidth: false,
                        data: data,
                        rowCallback: function (row, data) {
                            if (data["status"] == false) {
                                $(row).css("color", "red");
                                $(row).addClass('highlight')
                            }
                            //if (ages.indexOf(data['fullname']) > 0) {
                            //    $(row).css("background-color", "red");
                            //    $(row).addClass('highlight')
                            //}
                        },
                        columns: [
                            {
                                'data': 'id',
                                render: function (data, type, row, meta) {
                                    return meta.row + meta.settings._iDisplayStart + 1;
                                }
                            },
                            { 'data': 'staffCatagory' },
                            { 'data': 'memberId' },
                            { 'data': 'staffName' },
                            { 'data': 'contactNo' },
                            { 'data': 'associateBranch' },
                            { 'data': 'address' },
                            {
                                "data": "JoinDate",
                                "render": function (data) {
                                    var dat = data != null ? (data.split('T')[0]).split('-').join('/') : '';
                                    return dat;
                                }
                            },
                            { 'data': 'discountCode' },
                            { 'data': 'status' },
                            //{
                            //    'data': null,
                            //    render: function (data, type, full, meta) {
                            //        if (data.image == "" || data.image == null) {
                            //            return "<span style='color:red;font-weight: bolder;font-family: initial;'>No</span>";
                            //        }
                            //        else {

                            //            return "<span style='color:green;font-weight: bolder;font-family: initial;'>Yes</span>";
                            //        }
                            //    }  
                            //}, 
                            {
                                data: "id",
                                className: "center",
                                render: function (data, type, full, meta) {
                                    return '<a href="AddStaff.aspx?id=' + data + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a> &nbsp;&nbsp;&nbsp; <a href="AddStaff.aspx?id=' + data + "&" + "key=delete" + '" class="editor_remove"><img src="Assets/Icon/delete.png" class="iconDelete" /></a>';
                                }
                            }
                        ]
                    });
                }
            });
        });

    </script>
    <asp:HiddenField ID="hidHeader" runat="server" Value="Staff Management" />
    <asp:UpdatePanel ID="upnlStaffManagement" runat="server">
        <Triggers>
            <%-- <asp:PostBackTrigger ControlID="btnAddStaff" />   --%>
            <%--<asp:PostBackTrigger ControlID="btnEditStaff" />--%>
        </Triggers>
        <ContentTemplate>
            <div class="box box-success">
                <div class="box-body">
                    <div class="row">
                        <div class="col-sm-3">
                            Staff Catagory<span class="asterik">*</span>
                            <asp:DropDownList ID="ddlStaffCatagory" runat="server" OnSelectedIndexChanged="ddlStaffCatagory_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control input-sm">
                                <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Gym Admin"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Trainer"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Operation Manager"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Intern"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-3">
                            Full Name<span class="asterik">*</span>
                            <asp:DropDownList ID="ddlStaffName" CssClass="form-control input-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlStaffName_SelectedIndexChanged" runat="server"></asp:DropDownList>
                        </div>
                        <div class="col-sm-3">
                            Catagory(Only for Trainer)
                                 <asp:DropDownList ID="ddlCatagory" runat="server" CssClass="form-control input-sm">
                                     <asp:ListItem Value="0" Text="--select--"></asp:ListItem>
                                     <asp:ListItem Value="1" Text="Class A"></asp:ListItem>
                                     <asp:ListItem Value="2" Text="Class B"></asp:ListItem>
                                     <asp:ListItem Value="3" Text="Class C"></asp:ListItem>
                                     <asp:ListItem Value="4" Text="N/A"></asp:ListItem>
                                 </asp:DropDownList>
                        </div>
                        <div class="col-sm-3">
                            Contact NO<span class="asterik">*</span>
                            <asp:TextBox ID="txtContactNo" runat="server" ReadOnly="true" CssClass="form-control input-sm" />
                        </div>
                        <div class="col-sm-3">
                            Address<span class="asterik">*</span>
                            <asp:TextBox ID="txtAddress" runat="server" ReadOnly="true" CssClass="form-control input-sm" />
                        </div>
                        <div class="col-sm-3">
                            Associate Branch<span class="asterik">*</span>
                            <asp:TextBox ID="txtAssociateBranch" runat="server" ReadOnly="true" CssClass="form-control input-sm"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            TPW Join Date<span class="asterik">*</span>
                            <asp:TextBox ID="txtTPWJoinDate" runat="server" ReadOnly="true" CssClass="form-control input-sm dateControl" />
                        </div>
                        <div class="col-sm-3">
                            Discount Code
                                <asp:TextBox ID="txtStaffDiscountCode" runat="server" placeholder="aaa$5" CssClass="form-control input-sm input-sm" />
                        </div>
                        <div class="col-sm-3">
                            commission(%)
                                <asp:TextBox ID="txtCommissionPercentage" runat="server" Text="5" CssClass="form-control input-sm input-sm" />
                        </div>
                        <div class="col-sm-3">
                            Status<br />
                            <asp:CheckBox ID="chkStatus" runat="server" />
                            <asp:HiddenField ID="hidSnNo" runat="server" />
                        </div>
                    </div>
                    <div class="row m-2">
                        <h4 class="card-title">Available Time</h4>
                        <span class="asterik">*</span>
                    </div>
                    <div class="row" m-2>
                        <div class="col-sm-3">
                            From<span class="asterik">*</span>
                            <asp:TextBox ID="txtFrom1" runat="server" CssClass="form-control input-sm timepicker"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            To<span class="asterik">*</span>
                            <asp:TextBox ID="txtTo1" runat="server" CssClass="form-control input-sm timepicker"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                            From
                                        <asp:TextBox ID="txtFrom2" runat="server" CssClass="form-control input-sm timepicker"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            To
                                        <asp:TextBox ID="txtTo2" runat="server" CssClass="form-control input-sm timepicker"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-md-12">
                            Experience<span class="asterik">*</span>
                            <asp:TextBox ID="txtExperience" runat="server" TextMode="MultiLine" CssClass="form-control input-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row ml-1 mt-2">
                        <asp:Button ID="btnAddStaff" runat="server" Enabled="true" Text="Submit" CssClass="btn btn-success" OnClick="btnAddStaff_Click" />
                        <asp:Button ID="btnEditStaff" runat="server" Enabled="false" Text="Edit" CssClass="btn btn-danger ml-2" OnClick="btnEditStaff_Click" />
                        <asp:Label ID="lblInfo" Style="margin-left: 0px" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="box box-success">
        <div class="box-body">
            <div class="row">
                <div class="table-responsive">
                    <table id="StaffTable" class="table table-bordered table-hover">
                        <thead>
                            <tr class="header">
                                <th>Sn.</th>
                                <th>Staff Catagory</th>
                                <th>Member ID</th>
                                <th>Staff Name</th>
                                <th>Contact No</th>
                                <th>Associate Branch</th>
                                <th>Address</th>
                                <th>TPW Join Date</th>
                                <th>Discount Code</th>
                                <th>Status</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                    </table>
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
