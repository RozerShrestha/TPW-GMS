<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Branch.aspx.cs" Inherits="TPW_GMS.Branch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script>
        function UserDeleteConfirmation() {
            return confirm("Are you sure you want to delete this user?");
        }
        function UserEditConfirmation() {
            return confirm("Are you sure you want to Update password?");
        }
    </script>
    <asp:HiddenField ID="hidHeader" runat="server" Value="User Management" />
    <div class="row">
        <div class="col-xs-12">
            <div class="box box-info">
                <div class="box-title">
                </div>
                <div class="box-body">
                    <div class="col-sm-2">
                        Name
                        <asp:TextBox ID="txtSignInFullName" CssClass="form-control input-sm" placeholder="Full Name" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        Username:
                        <asp:TextBox ID="txtSignInUserName" CssClass="form-control input-sm" placeholder="User Name" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        Password
                        <asp:TextBox ID="txtSignInPassword" TextMode="Password" CssClass="form-control input-sm" placeholder="Password" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        Role
                        <asp:DropDownList ID="ddlUserRole" runat="server" CssClass="form-control input-sm">
                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Super Admin"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Gym Admin"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Gym User"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Admin"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        Latitude
                        <asp:TextBox ID="txtLatitude" CssClass="form-control input-sm" placeholder="Latitude" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        Longitude
                        <asp:TextBox ID="txtLongitude" CssClass="form-control input-sm" placeholder="Longitude" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-sm-12">
                        <asp:Button ID="btnNewBranch" runat="server" Text="Create New User" Style="margin-top: 15px" OnClick="btnNewBranch_Click" CssClass="btn btn-success" />
                        <asp:Label id="lblInfo" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-12">
                        <div class="table-responsive">
                            <asp:GridView ID="GridUserMgmt" runat="server" Font-Size="12px" AutoGenerateColumns="false" OnRowCommand="GridUserMgmt_RowCommand" CssClass="table table-bordered">
                                <HeaderStyle BackColor="#a3b8c4" />
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sn.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Full Name">
                                        <ItemTemplate>
                                            <asp:TextBox  ID="txtFullName" runat="server" CssClass="form-control input-sm" Text='<%# Eval("firstname") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="User Name" DataField="username" />
                                    <asp:TemplateField HeaderText="Password" >
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control input-sm" Text='<%# Eval("password") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Role" DataField="role1" />
                                    <asp:TemplateField HeaderText="Latitude">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLatitude" runat="server" CssClass="form-control input-sm" Text='<%# Eval("latitude") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Longitude">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLongitude" runat="server" CssClass="form-control input-sm" Text='<%# Eval("longitude") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>Action</HeaderTemplate>
                                        <ItemTemplate>
                                            &nbsp;&nbsp;
                                            <asp:LinkButton ID="linkEdit" runat="server" Text="edit" CommandName="editroww" OnClientClick="if(! UserEditConfirmation()) return false;" CommandArgument='<%# Eval("loginId")%>'><img src="Assets/Icon/edit.png" class="iconEdit" /></asp:LinkButton>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="LinkDelete" runat="server" Text="delete" OnClientClick="if(! UserDeleteConfirmation()) return false;" CommandName="deleteRow" CommandArgument='<%# Eval("loginId")%>'><img src="Assets/Icon/delete.png" class="iconEdit" /></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>No Record Found!</EmptyDataTemplate>
                                <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</asp:Content>
