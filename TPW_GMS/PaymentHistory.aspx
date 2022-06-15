<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PaymentHistory.aspx.cs" Inherits="TPW_GMS.PaymentHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="box box-info">
                    <div class="box-title">
                        <asp:HiddenField ID="hidHeader" runat="server" Value="Payment History" />
                    </div>
                    <div class="box-body">
                        <div class="col-xs-12">
                            <div class="col-sm-3">
                                TPW-Member Name
                            <asp:DropDownList ID="ddlMemberName" AutoPostBack="true" OnSelectedIndexChanged="ddlMemberName_SelectedIndexChanged" runat="server" CssClass="select2Example form-control input-sm"></asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                Member Id:
                            <asp:DropDownList ID="ddlMemberId" AutoPostBack="true" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                Receipt No
                        <asp:TextBox ID="txtCustomerReceiptNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                            </div>
                        </div>
                        <br />
                        <br />
                        <br />
                        <div class="col-xs-12">
                            <div class="col-sm-1">
                                <asp:Button ID="btnSearchReport" runat="server" CssClass="btn btn-successs" Text="Search" OnClick="btnSearchReport_Click" />&nbsp;&nbsp;&nbsp;&nbsp; 
                            </div>
                            <div class="col-sm-1">
                                <asp:ImageButton ID="printB" ImageUrl="Assets/Images/print_icon1.gif" OnClientClick="PrintDivPaymentHistory()" runat="server" Style="width: 30px;" />
                            </div>
                            <div class="col-sm-10">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-solid">
                        <div class="box-body">
                            <div id="pnlPrintPaymentHistory" class="table-responsive">
                                <asp:GridView ID="GridViewReportSearch" runat="server" AutoGenerateColumns="false" OnRowDataBound="GridViewReportSearch_RowDataBound" Style="font-size: 11px;" CssClass="table table-bordered">
                                    <HeaderStyle BackColor="#a3b8c4" />

                                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                                    <Columns>
                                        <asp:TemplateField HeaderText=".Sn" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Member Id" DataField="memberId" ItemStyle-ForeColor="Red" HeaderStyle-Width="150px" />
                                        <asp:BoundField HeaderText="Receipt No" DataField="receiptNo" HeaderStyle-Width="150px" />
                                        <asp:TemplateField HeaderText="Renew Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRenewDate" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                        <%--<asp:BoundField HeaderText="Renew Date" DataField="memberBeginDate" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="150px" />--%>
                                        <asp:TemplateField HeaderText="Expire Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblExpiredDate" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
<%--                                        <asp:BoundField HeaderText="Expire Date" DataField="memberExpireDate" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-Width="150px" />--%>
                                        <asp:BoundField HeaderText="Option" DataField="memberOption" HeaderStyle-Width="100px" />
                                        <asp:BoundField HeaderText="Catagory" DataField="memberCatagory" HeaderStyle-Width="100px" />
                                        <asp:BoundField HeaderText="Payment Type" DataField="memberpaymentType" HeaderStyle-Width="170px" />
                                        <%--<asp:BoundField HeaderText="Fee" DataField="finalAmount"  />--%>
                                        <asp:TemplateField HeaderText="Fee" HeaderStyle-Font-Bold="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalFee" runat="server" Text='<%# Eval("finalAmount")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                                <asp:Label ID="lblTotalAmountPaymentHistory" runat="server" Text="Total Amount-:" Visible="false"></asp:Label>
                                <asp:Label ID="lblTotalmoneyPaid" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
