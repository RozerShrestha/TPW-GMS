<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GuestAttendance.aspx.cs" Inherits="TPW_GMS.GuestAttendance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:HiddenField ID="hidHeader" runat="server" Value="Guest Attendance" />
        <asp:HiddenField ID="hidCurrentLoginBranch" runat="server" />
    <br />
    <asp:UpdatePanel ID="upnlAttendance" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="box box-info">
                    <div class="box-header">
                         <div class="col-sm-12">
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtQrCode" runat="server" CssClass="form-control input-sm" autofocus></asp:TextBox>
                            </div>
                            <div>
                                <input type="button" id="btnReload" name="Reload" style="margin-top: -7px" value="Reload" class="btn btn-danger" onclick="window.location.reload()" />
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="col-sm-12">
                            <asp:GridView ID="gridAttendance" Font-Size="Smaller" CssClass="table" runat="server" Style="padding: 3px" AutoGenerateColumns="false" Width="100%" AllowPaging="true" PageSize="35" OnPageIndexChanging="gridAttendance_PageIndexChanging">
                                <HeaderStyle BackColor="#a3b8c4" />
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sn.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Name" DataField="name" ItemStyle-ForeColor="red" />
                                    <asp:BoundField HeaderText="Checkin Branch" DataField="checkinBranch" />
                                    <asp:BoundField HeaderText="Check In" DataField="checkin" />
                                    <asp:BoundField HeaderText="Check Out" DataField="checkout" />
                                </Columns>
                                <EmptyDataTemplate>No Record Found!</EmptyDataTemplate>
                                <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        function debounce(fn, duration) {
            var timer;
            return function () {
                clearTimeout(timer);
                timer = setTimeout(fn, duration);
            }
        }
        $(function () {
            document.getElementById("ContentPlaceHolder1_txtQrCode").addEventListener("keyup", function (event) {
                if (event.getModifierState("CapsLock")) {
                    alert("Please Turn Off the Caps Lock");
                    return;
                }
            });
            //$('#myModal').modal('show');
            //$("#trId").hide();
            $('#ContentPlaceHolder1_txtQrCode').on('keyup', debounce(function () {
                var _msg = $get("ContentPlaceHolder1_txtQrCode").value;
                var _branch = document.getElementById("lblUserLogin").innerHTML;
                    $.ajax({
                        url: 'api/GuestAttendance',
                        headers: {
                            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                        },
                        type: "get",
                        async: false,
                        data: {
                            emJsonData: _msg,
                            loginBranch: _branch
                        },
                        success: function (result) {
                            alert(result.message);
                            location.reload();
                        },
                        error: function () {
                            alert("error");
                        }
                    });
            }, 250));
        });
    </script>
</asp:Content>
