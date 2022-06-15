<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AskQuestion.aspx.cs" Inherits="TPW_GMS.AskQuestion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">     
        $(document).ready(function () {
            $.ajax({
                type: "GET",
                dataType: "json",
                url: "api/AskQn",
                headers: {
                    'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                },
                success: function (data) {
                    var datatableVariable = $('#AskQuestionTable').DataTable({
                        autoWidth: false,
                        data: data,
                        columnDefs: [
                            {
                                targets: [10],
                                render: function (data, type, row) {
                                    return data['status'] == 'true' ? 'P' : 'NP'
                                }
                            }
                        ],
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
                            { 'data': 'memberName' },
                            { 'data': 'memberId' },
                            { 'data': 'question' },
                            {
                                'data': 'status',
                                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                                    if (sData) {
                                        $(nTd).addClass('text-success').text('Answered');
                                    } else {
                                        $(nTd).addClass('text-danger').text('Not Answered');
                                    }
                                }
                            },
                            {
                                'data': null,
                                render: function (data, type, row) {
                                    if (data.status == true) {
                                        return '<input type="checkbox" onchange="handleChange(' + data.askId + ');" checked class="editor-active">';
                                    }
                                    else
                                        return '<input type="checkbox" onchange="handleChange(' + data.askId + ');" class="editor-active">';
                                    
                                }
                            },
                            { 'data': null },
                        ],
                        columnDefs: [{
                            data: "askId",
                            className: "center",
                            targets: [-1],
                            render: function (data, type, full, meta) {

                                return '<a href="AskQuestion.aspx?id=' + data.askId + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a> &nbsp;&nbsp;&nbsp; <a href="AskQuestion.aspx?id=' + data.askId + "&" + "key=delete" + '" class="editor_remove"><img src="Assets/Icon/delete.png" class="iconDelete" /></a>';
                            }
                        }],
                    });
                }
            });
        });
        function handleChange(askId){
            $.ajax({
                type: "PATCH",
                dataType: "json",
                url: "api/AskQn",
                headers: {
                    'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                },
                data: {
                    id: askId,
                },
                success: function (data) {
                    
                }
            });
        }
    </script>
    <asp:HiddenField ID="hidHeader" runat="server" Value="Questions" />
     <asp:HiddenField ID="hidUserLogin" runat="server" />
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
                        Member Name
               <asp:TextBox ID="txtMemberName" Enabled="false" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-3">
                        Member Id
                <asp:TextBox ID="txtMemberId" runat="server" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-3">
                        Status
                <asp:CheckBox ID="chkStatus" runat="server" CssClass="checkbox" Style="margin-left: 20px"></asp:CheckBox>
                    </div>
                    <div class="col-sm-12">
                        Question
                <asp:TextBox ID="txtQuestion" TextMode="MultiLine" Enabled="false" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-12">
                        <%--<asp:Button ID="btnSubmit" runat="server" Style="margin-top: 19px;" CssClass="btn btn-success" Text="Submit" OnClientClick="binddatatable()" OnClick="btnSubmit_Click" />--%>
                        <asp:Button ID="btnEdit" runat="server" Style="margin-top: 19px;" CssClass="btn btn-danger" Text="Edit" OnClick="btnEdit_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:Label ID="lblInformation" runat="server"></asp:Label>
    <div class="row">
        <div class="box box-solid">
            <div class="box-body">
                <div class="col-xs-12">
                    <div class="table-responsive">
                        <div class="col-lg-12">
                            <table id="AskQuestionTable" style="font-size: 12px" class="table table-bordered table-hover">
                                <thead>
                                    <tr class="header">
                                        <th>Sn.</th>
                                        <th>Date</th>
                                        <th>Member Name</th>
                                        <th>member Id</th>
                                        <th>Question</th>
                                        <th>Status</th>
                                        <th>Test</th>
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
