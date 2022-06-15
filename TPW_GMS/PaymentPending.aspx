<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PaymentPending.aspx.cs" Inherits="TPW_GMS.PaymentPending" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upnlPaymentPending" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-info">
                        <div class="box-title">
                            <asp:HiddenField ID="hidHeader" runat="server" Value="Payment Pending" />
                        </div>
                        <div class="box-body">
                            <div class="col-sm-2">
                                Member Id
                                <asp:TextBox ID="txtCustomerIDPayment" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-2">
                                Full Name
                                <asp:TextBox ID="txtCustomerNamePayment" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-2">
                                Contact Number
                                <asp:TextBox ID="txtCustomerNumberPayment" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-2">
                                Branch
                                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                            </div>
                            <div class="col-sm-1">
                                Status
                                <asp:DropDownList ID="ddlActiveInactive" runat="server" CssClass="form-control input-sm">
                                    <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                                    <asp:ListItem Text="InActive" Value="InActive"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-1">
                                Flag
                                <asp:DropDownList ID="ddlFlag" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlFlag_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Email Sent" Value="true"></asp:ListItem>
                                    <asp:ListItem Text="Email Not Sent" Value="false"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-2">
                                Post Pending
                                <asp:DropDownList ID="ddlPrePendingCheck" runat="server" CssClass="form-control input-sm">
                                    <asp:ListItem Value="0" Text="Expired"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="1 day after Expiry"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="2 days after Expiry"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="3 days after Expiry"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="4 days after Expiry"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="5 days after Expiry"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="6 days after Expiry"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="7 days after Expiry"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="btnSearchPayment" runat="server" Style="margin-top: 20px" CssClass="btn btnTheme" Text="Search" OnClick="btnSearchPayment_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <asp:Button ID="btnSentEmailAll" style="float:right; margin-right:10px" Text="Send Bulk Email" Enabled="false" runat="server" OnClick="btnSentEmailAll_Click" CssClass="btn btn-sm btn-default" />
                </div>
                <div class="col-xs-12">
                    <div class="box box-solid">
                        <div class="box box-body">
                            <div class="table-responsive">
                                <asp:GridView ID="GridViewaPayment" runat="server" AutoGenerateColumns="false" Font-Size="11px" OnPageIndexChanging="GridViewaPayment_PageIndexChanging" AllowPaging="true" PageSize="30" OnRowCommand="GridViewaPayment_RowCommand" CssClass="table table-bordered table table-hover">
                                    <HeaderStyle BackColor="#a3b8c4" />
                                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                                    <Columns>  
                                        <asp:TemplateField HeaderText="Sn" HeaderStyle-Font-Bold="true" HeaderStyle-Width="10px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Id" DataField="memberId" ItemStyle-ForeColor="red" />
                                        <asp:BoundField HeaderText="Name" DataField="fullname" />
                                        <asp:BoundField HeaderText="Email" DataField="email" />
                                        <asp:BoundField HeaderText="Status" DataField="ActiveInactive" />
                                        <asp:BoundField HeaderText="Branch" DataField="branch" />
                                        <asp:BoundField HeaderText="Option" DataField="memberOption" />
                                        <asp:BoundField HeaderText="Catagory" DataField="memberCatagory" HeaderStyle-Width="20px" />
                                        <asp:BoundField HeaderText="Duration" DataField="memberPaymentType" HeaderStyle-Width="78px" />
                                        <asp:BoundField HeaderText="Renew Date" DataField="memberBeginDate" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="108px" />
                                        <asp:BoundField HeaderText="Expire Date" DataField="memberExpireDate" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="108px" />
                                        <asp:BoundField HeaderText="Contact No" DataField="contactNo" />
                                        <asp:BoundField HeaderText="Fee" DataField="finalAmount" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>Flg</HeaderTemplate>
                                            <ItemTemplate>
                                                &nbsp;                                 
                                                <asp:CheckBox ID="chkEmailStatus" CssClass="check" Enabled="false" Checked='<%# Eval("emailStatus") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="80px">
                                            <HeaderTemplate>Action</HeaderTemplate>
                                            <ItemTemplate>
                                                &nbsp;&nbsp;
                                                <a href="EditForm.aspx?ID=<%# Eval("memberId") %>&key=editP" target="_blank"><img src="Assets/Icon/edit.png" class="iconEdit" /></a>
                                                <%--<asp:LinkButton ID="linkEdit" runat="server" Text="edit" CommandName="edit" CommandArgument='<%# Eval("memberId")%>'><img src="Assets/Icon/edit.png" class="iconEdit" /></asp:LinkButton>--%>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="linkEmail" runat="server" Text="Email" CommandName="email" CommandArgument='<%# Eval("memberId")%>' OnClientClick="return confirm('Are you sure want to send Email?');"><img src="Assets/Icon/email.png" width="15px" height="13px"/></asp:LinkButton>
                                            
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
