<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AllMemberDev.aspx.cs" Inherits="TPW_GMS.AllMemberDev" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hidHeader" runat="server" Value="All Member" />
    <style>
    .iconDelete{
            width:15px;
            height:15px;
        }
        .iconView{
            width:15px;
            height:15px;
        }
        .iconEdit{
            width:13px;
            height:13px;
        }
        tr { 
            min-height: 20px 
        }
</style>
<script>
    function findElement(ele, charArr) {

    }
    $(document).ready(function () {
        //var ages = [];
        var table = $('#memberInformationTable').DataTable({
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
                url: 'api/GetAllMember?branch=<%=hidUserLogin.Value.ToString()%>',
                dataType: "JSON",
                dataSrc: "",
                headers: {
                    'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                },
            },
            rowCallback: function (row, data) {
                if (data["ActiveInActive"] == "InActive") {
                    $(row).css("color", "red");
                    $(row).addClass('highlight')
                }
                //if (ages.indexOf(data['fullname']) > 0) {
                //    $(row).css("background-color", "red");
                //    $(row).addClass('highlight')
                //}
            },
            columns: [
                { 'data': 'memberId' },
                { 'data': 'fullname' },
                //{ 'data': 'memberOption' },
                { 'data': 'branch' },
                { 'data': 'shift' },
                { 'data': 'memberCatagory' },
                { 'data': 'memberPaymentType' },
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
                { 'data': 'contactNo' },
                { 'data': 'receiptNo' },
                { 'data': 'ActiveInActive' },
                { 'data': 'finalAmount' },
                {'data':'email'},
                {
                    data: null
                },
            ],
            columnDefs: [{
                // puts a button in the last column
                data: "memberId",
                className: "center",
                targets: [-1], render: function (data, type, full, meta) {
                    if ('<%=hidUserLogin.Value.ToString()%>' == 'superadmin' || '<%=hidUserLogin.Value.ToString()%>' == 'admin') {
                        return '<a href="EditForm.aspx?id=' + data.memberId + "&" + "key=edit" + '" class="editAsset" target="_blank"><img src="Assets/Icon/edit.png" class="iconEdit"  /></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  <a href="EditForm.aspx?id=' + data.memberId + "&" + "key=view" + '" class="editAsset" target="_blank"><img src="Assets/Icon/view.png" class="iconView" /></a> &nbsp;&nbsp;&nbsp;&nbsp;<a href="AllMember.aspx?id=' + data.memberId + "&" + "key=delete" + '" class="editor_remove"><img src="Assets/Icon/delete.png" class="iconDelete" /></a>';
                    }
                    else {
                        if (data.branch == '<%=hidUserLogin.Value.ToString()%>' || data.branch == '') {
                            return '<a href="EditForm.aspx?id=' + data.memberId + "&" + "key=edit" + '" class="editAsset" target="_blank"><img src="Assets/Icon/edit.png" class="iconEdit"  /></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  <a href="EditForm.aspx?id=' + data.memberId + "&" + "key=view" + '" class="editAsset" target="_blank"><img src="Assets/Icon/view.png" class="iconView" /></a> &nbsp;&nbsp;&nbsp;&nbsp;';
                        }
                        else {
                            return '<a href="EditForm.aspx?id=' + data.memberId + "&" + "key=view" + '" class="editAsset" target="_blank"><img src="Assets/Icon/view.png" class="iconView" /></a>';
                        }
                    }
                }
            }],
        })
    });

</script>
<asp:HiddenField ID="hidUserLogin" runat="server" />
<div class="row">   
    <div class="col-xs-12">
        <div class="box box-info">
            <div class="box-body">
                <div class="table-responsive">
                    <table id="memberInformationTable" style="font-size: 12px; width:100%" class="table table-striped table-bordered table-sm">
                        <thead>
                            <tr class="border-bottom-0 tr-header header">
                                <th style="min-width:100px">MemberId</th>
                                <th style="min-width:100px">Name</th>
                                <%--<th>Membrship</th>--%>
                                <th>Branch</th>
                                <th>Shift</th>
                                <th>Catagory</th>
                                <th>Duration</th>
                                <th>Renew</th>
                                <th>Expire Date</th>
                                <th>Contact No</th>
                                <th>Receipt No</th>
                                <th>Stat</th>
                                <th>Fee</th>
                                <th>Email</th>
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
                    <asp:Label ID="lblPopupError" runat="server" ForeColor="Red" />
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
                    <asp:Button id="btnConfirmDelete" runat="server" Text="Yes" OnClick="btnConfirmDelete_Click" class="btn btn-danger btn-sm" />
                    <button type="button" class="btn btn-success btn-sm" data-dismiss="modal">No</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
