<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CommissionLog.aspx.cs" Inherits="TPW_GMS.CommissionLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function binddatatable() {
            $.ajax({
                type: "GET",
                dataType: "json",
                url: "/api/GetCommissionLog",
                headers: {
                    'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                },
                success: function (data) {
                    var datatableVariable = $('#ComissionPaymentLog').DataTable({
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
                                "data": "date",
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
                            { 'data': 'branch' },
                            { 'data': 'name' },
                            { 'data': 'commissionFor' },
                            { 'data': 'discountCode' },
                            { 'data': 'comission' },
                            { 'data': 'memberId' },
                            { 'data': 'memberName' },
                            //{
                            //    data: "suplementId",
                            //    className: "center",
                            //    render: function (data, type, full, meta) {
                            //        return '<a href="Suplements.aspx?id=' + data + "&" + "key=edit" + '" class="editAsset"><img src="Assets/Icon/edit.png" class="iconEdit" /></a> &nbsp;&nbsp;&nbsp; <a href="Suplements.aspx?id=' + data + "&" + "key=delete" + '" class="editor_remove"><img src="Assets/Icon/delete.png" class="iconDelete" /></a>';
                            //    }
                            //}
                        ]
                    });
                }
            });

        });
        </script>
    <asp:HiddenField ID="hidHeader" runat="server" Value="Commission Log" />
    <div class="row">
        <div class="col-xs-12">
            <div class="box box-solid">
                <div class="box-body">
                    <div class="table-responsive">
                        <table id="ComissionPaymentLog" style="font-size: 12px" class="table table-bordered table-hover">
                            <thead>
                                <tr class="subheader">
                                    <th>Sn.</th>
                                    <th>Date</th>
                                    <th>Branch</th>
                                    <th width="300px">Name</th>
                                    <th>Commission For</th>
                                    <th>Code</th>
                                    <th>Commission</th>
                                    <th>Member Id</th>
                                    <th>Member Name</th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
