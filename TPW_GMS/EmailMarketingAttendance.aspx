<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmailMarketingAttendance.aspx.cs" Inherits="TPW_GMS.EmailMarketingAttendance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:HiddenField ID="hidHeader" runat="server" Value="Email Marketing Attendance" />
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
                                    <asp:BoundField HeaderText="Email" DataField="email" />
                                    <asp:BoundField HeaderText="Mobile" DataField="mobile" />
                                    <asp:BoundField HeaderText="Branch" DataField="branch" />
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
            <div class="modal fade" id="myModal">
                <div class="modal-dialog">
                    <div class="modal-content">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">INFORMATION</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            <p>Dear All,</p>
                            <p>There is a small modification on Attendance system.</p>
                            <p>Please login to the app with your Username(mobile number) and Password(see in Email) or ask to Admin</p>
                            <p>Scan the QR to login into an App</p>
                            <img src="Assets/Images/qr.png" />
                            <p>Thank you.</p>
                        </div>

                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
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
                        url: 'api/EMAttendance',
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
