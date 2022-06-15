<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TPWReport.aspx.cs" Inherits="TPW_GMS.TPWReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hidHeader" runat="server" Value="Reports" />
    <div class="row">
        <div class="box box-solid">
            <div class="box-body">
                <div class="col-xs-12">
                    <div class="col-sm-12">
                        <strong>Branch</strong>
                            <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control input-sm">
                            </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <strong>No of Member Joined</strong>
                            <asp:TextBox ID="txtStartDateNewMember" runat="server" ClientIDMode="Static" Style="border-bottom-color: black; border-bottom-width: 1.5px" placeholder="start date" CssClass="form-control input-sm dateControl"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        .
                            <asp:TextBox ID="txtEndDateNewMember" runat="server" ClientIDMode="Static" Style="border-bottom-color: black; border-bottom-width: 1.5px" placeholder="End date" CssClass="form-control input-sm dateControl"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        Membership Option
                            <asp:DropDownList ID="ddlOption" runat="server" Style="border-bottom-color: black; border-bottom-width: 1.5px" CssClass="form-control input-sm">
                                <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Regular"></asp:ListItem>
                                <asp:ListItem Value="2" Text="OffHour"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Universal"></asp:ListItem>
                            </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        Catagory
                            <asp:DropDownList ID="ddlCatagorySearch" runat="server" Style="border-bottom-color: black; border-bottom-width: 1.5px" CssClass="form-control input-sm">
                                <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Any1"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Any2"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Any3"></asp:ListItem>
                            </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        Duration
                            <asp:DropDownList ID="ddlMembershipPaymentType" runat="server" Style="border-bottom-color: black; border-bottom-width: 1.5px" CssClass="form-control input-sm">
                                <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1 Month"></asp:ListItem>
                                <asp:ListItem Value="2" Text="3 Month"></asp:ListItem>
                                <asp:ListItem Value="3" Text="6 Month"></asp:ListItem>
                                <asp:ListItem Value="4" Text="12 Month"></asp:ListItem>
                            </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        Shift
                                <asp:DropDownList ID="ddlShift" runat="server" Style="border-bottom-color: black; border-bottom-width: 1.5px" CssClass="form-control input-sm">
                                    <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Morning"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Day"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Evening"></asp:ListItem>
                                </asp:DropDownList>
                    </div>



                     <div class="col-sm-2">
                        <strong>No of Member Renewed</strong>
                            <asp:TextBox ID="txtStartDateRenewed" runat="server" ClientIDMode="Static" Style="border-bottom-color:#b93b3b; border-bottom-width: 1.5px" placeholder="start date" CssClass="form-control input-sm dateControl"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        .
                            <asp:TextBox ID="txtEndDateRenewed" runat="server" ClientIDMode="Static" Style="border-bottom-color: #b93b3b; border-bottom-width: 1.5px" placeholder="End date" CssClass="form-control input-sm dateControl"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        Membership Option
                            <asp:DropDownList ID="ddlOptionRenewed" runat="server" Style="border-bottom-color: #b93b3b; border-bottom-width: 1.5px" CssClass="form-control input-sm">
                                <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Regular"></asp:ListItem>
                                <asp:ListItem Value="2" Text="OffHour"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Universal"></asp:ListItem>
                            </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        Catagory
                            <asp:DropDownList ID="ddlCatagorySearchRenewed" runat="server" Style="border-bottom-color: #b93b3b; border-bottom-width: 1.5px" CssClass="form-control input-sm">
                                <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Any1"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Any2"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Any3"></asp:ListItem>
                            </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        Duration
                            <asp:DropDownList ID="ddlMembershipPaymentTypeRenewed" runat="server" Style="border-bottom-color: #b93b3b; border-bottom-width: 1.5px" CssClass="form-control input-sm">
                                <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1 Month"></asp:ListItem>
                                <asp:ListItem Value="2" Text="3 Month"></asp:ListItem>
                                <asp:ListItem Value="3" Text="6 Month"></asp:ListItem>
                                <asp:ListItem Value="4" Text="12 Month"></asp:ListItem>
                            </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        Shift
                                <asp:DropDownList ID="ddlShiftRenewed" runat="server" Style="border-bottom-color: #b93b3b; border-bottom-width: 1.5px" CssClass="form-control input-sm">
                                    <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Morning"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Day"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Evening"></asp:ListItem>
                                </asp:DropDownList>
                    </div>



                    <div class="col-sm-2" style="display: none;">
                        Total Earnings from Gym
                                <asp:TextBox ID="txtStartDateTotalEarning" runat="server" ClientIDMode="Static" Style="border-bottom-color: red; border-bottom-width: 1.5px" placeholder="start date" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-2" style="display: none;">
                        .
                                <asp:TextBox ID="txtEndDateTotalEarning" runat="server" ClientIDMode="Static" Style="border-bottom-color: red; border-bottom-width: 1.5px" placeholder="End date" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <strong>Suplements</strong>
                            <asp:TextBox ID="txtStartDateSuplements" runat="server" ClientIDMode="Static" Style="border-bottom-color: lawngreen; border-bottom-width: 1.5px" placeholder="start date" CssClass="form-control input-sm dateControl"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        .
                            <asp:TextBox ID="txtEndDateSuplements" runat="server" ClientIDMode="Static" Style="border-bottom-color: lawngreen; border-bottom-width: 1.5px" placeholder="End date" CssClass="form-control input-sm dateControl"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <strong>Expenditure</strong>
                            <asp:TextBox ID="txtStartDateExpenditure" runat="server" ClientIDMode="Static" Style="border-bottom-color: blue; border-bottom-width: 1.5px" placeholder="start date" CssClass="form-control input-sm dateControl"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        .
                            <asp:TextBox ID="txtEndDateExpenditure" runat="server" ClientIDMode="Static" Style="border-bottom-color: blue; border-bottom-width: 1.5px" placeholder="End date" CssClass="form-control input-sm dateControl"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <strong>OverAll Report</strong>
                            <asp:TextBox ID="txtStartDateOverAllReport" runat="server" ClientIDMode="Static" Style="border-bottom-color: saddlebrown; border-bottom-width: 1.5px" placeholder="start date" CssClass="form-control input-sm dateControl"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        .
                            <asp:TextBox ID="txtEndDateOverAllReport" runat="server" ClientIDMode="Static" Style="border-bottom-color: saddlebrown; border-bottom-width: 1.5px" placeholder="End date" CssClass="form-control input-sm dateControl"></asp:TextBox>
                    </div>
                    <div class="col-sm-12" style="margin-top: 5px; margin-left: 5px;">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-success" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnClearAll" runat="server" Text="Clear All" CssClass="btn btn-danger" OnClick="btnClearAll_Click" />
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-12">
                        <hr />
                    </div>
                    <div class="col-sm-12">
                        <div class="table-responsive">
                            
                            <asp:GridView ID="GridViewReportSearch" runat="server" ShowFooter="true" Style="font-size: 11px" OnPageIndexChanging="GridViewReportSearch_PageIndexChanging" EmptyDataText="No Record Found!!" AllowPaging="true" PageSize="20" AutoGenerateColumns="false" CssClass="table table-bordered">
                                <HeaderStyle BackColor="#a3b8c4" />
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sn" HeaderStyle-Font-Bold="true" HeaderStyle-Width="10px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Member Id" DataField="memberId" ItemStyle-ForeColor="Red" HeaderStyle-Width="150px" />
                                    <asp:BoundField HeaderText="First Name" DataField="fullname" HeaderStyle-Width="150px" />
                                    <asp:BoundField HeaderText="Shift" DataField="shift" HeaderStyle-Width="100px" />
                                    <asp:BoundField HeaderText="Membership Date" DataField="memberDate" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="150px" />
                                     <asp:BoundField HeaderText="Renew Date" DataField="memberBeginDate" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="150px" />
                                    <asp:BoundField HeaderText="Membership Option" DataField="memberOption" HeaderStyle-Width="150px" />
                                    <asp:BoundField HeaderText="Catagory" DataField="memberCatagory" HeaderStyle-Width="100px" />
                                    <asp:BoundField HeaderText="Payment Type" DataField="memberpaymentType" HeaderStyle-Width="170px" />
                                    <asp:BoundField HeaderText="Branch" DataField="branch" HeaderStyle-Width="170px" />
                                    <asp:TemplateField HeaderText="Fee" HeaderStyle-Font-Bold="true" HeaderStyle-Width="10px">
                                        <ItemTemplate>
                                            <%# Eval("finalAmount") %>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <%=sumNoOfMember %>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <%--<FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />--%>
                                <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="col-sm-12">
                        <div class="table-responsive">
                            <asp:Panel ID="pnlSuplementsReport" runat="server" Visible="false">
                                <table class="table table-bordered">
                                    <tr class="subheader">
                                        <th>Suplements Buying Record
                                        </th>
                                        <th>Suplements Selling Record
                                        </th>
                                    </tr>
                                    <tr>
                                        <td width="50%">
                                            <asp:GridView ID="gridSuplementBuyingReport" Style="font-size: 12px" ShowFooter="true" EmptyDataText="No Record Found!!" OnPageIndexChanging="gridSuplementBuyingReport_PageIndexChanging" AllowPaging="true" PageSize="30" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" OnRowDataBound="gridSuplementBuyingReport_RowDataBound">
                                                <HeaderStyle BackColor="Lavender" />
                                                <FooterStyle Font-Bold="true" BackColor="#f1f1f1" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sn" HeaderStyle-Font-Bold="true" HeaderStyle-Width="10px">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Date" DataField="date" DataFormatString="{0:MM/dd/yyyy}" />
                                                    <asp:TemplateField HeaderText="Suplement">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBuyer" runat="server" Text='<%# Eval("nameOfSuplement") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTot" runat="server" Text="Total"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qty">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("quantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblGrandQuantity" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Price">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPerPrice" runat="server" Text='<%# Eval("perPrice") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblGrandPerPrice" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalPrice" runat="server" Text='<%# Eval("totalPrice") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblGrandTotalPrice" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Discount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("discount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblGrandDiscount" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Final">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFinal" runat="server" Text='<%# Eval("finalPrice") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblGrandFinalPrice" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <%--<FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />--%>
                                                <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#b6c7d0" />
                                            </asp:GridView>
                                        </td>
                                        <td width="50%">
                                            <asp:GridView ID="gridSuplementSellingReport" runat="server" Style="font-size: 12px" AutoGenerateColumns="false" EmptyDataText="No Record Found!!" OnPageIndexChanging="gridSuplementSellingReport_PageIndexChanging" AllowPaging="true" PageSize="30" ShowFooter="true" CssClass="table table-bordered" OnRowDataBound="gridSuplementSellingReport_RowDataBound">
                                                <HeaderStyle BackColor="Lavender" />
                                                <FooterStyle Font-Bold="true" BackColor="#f1f1f1" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sn" HeaderStyle-Font-Bold="true" HeaderStyle-Width="10px">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Date" DataField="date_Sell" DataFormatString="{0:MM/dd/yyyy}" />
                                                    <asp:BoundField HeaderText="Suplement" DataField="nameOfSuplement_Sell" />
                                                    <asp:TemplateField HeaderText="Buyer">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBuyer" runat="server" Text='<%# Eval("customer_Sell") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTot" runat="server" Text="Total"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qty">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("quantity_Sell") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblGrandQuantity" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Price">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPerPrice" runat="server" Text='<%# Eval("perPrice_Sell") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblGrandPerPrice" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalPrice" runat="server" Text='<%# Eval("totalPrice_Sell") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblGrandTotalPrice" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Discount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("discount_Sell") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblGrandDiscount" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Final">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFinalPrice" runat="server" Text='<%# Eval("finalPrice_Sell") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblGrandFinalPrice" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <%--<FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />--%>
                                                <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#b6c7d0" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="col-sm-12">
                        <div class="table-responsive">
                            <asp:GridView ID="GridExpenditure" runat="server" ShowFooter="true" Style="font-size: 12px" OnPageIndexChanging="GridExpenditure_PageIndexChanging" OnRowDataBound="GridExpenditure_RowDataBound" EmptyDataText="No Record Found!!" AllowPaging="true" PageSize="30" AutoGenerateColumns="false" CssClass="table table-bordered">
                                <HeaderStyle BackColor="#a3b8c4" />
                                <FooterStyle Font-Bold="true" BackColor="#f1f1f1" />
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sn" HeaderStyle-Font-Bold="true" HeaderStyle-Width="10px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Date" DataField="expenditureDate" DataFormatString="{0:MM/dd/yyyy}" />
                                    <asp:BoundField HeaderText="Branch" DataField="branch" DataFormatString="{0:MM/dd/yyyy}" />
                                    <asp:TemplateField HeaderText="Expenditure Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExpenditureType" runat="server" Text='<%# Eval("expenditureType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTot" runat="server" Text="Total"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Expenditure Rate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExpenditureRate" runat="server" Text='<%# Eval("expenditureRate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalRate" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#b6c7d0" />
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="col-sm-12">
                        <div class="table-responsive">
                            <asp:Panel ID="pnlOverAllReport" runat="server" Visible="false">
                                <div class="container">
                                    <div class="Head" style="text-align: center">
                                        <h3>Over All Report Summary</h3>
                                    </div>
                                    <div>
                                        <b>Start Date: </b><%=txtStartDateOverAllReport.Text %><br />
                                        <b>End Date: </b><%=txtEndDateOverAllReport.Text %>
                                    </div>
                                    <br />
                                    <div>
                                        <table class="table table-bordered" style="width: 98%">
                                            <tr>
                                                <td width="20%">No of Member Joined:</td>
                                                <td>
                                                    <asp:Label ID="lblTotalNoOfMemberJoined" runat="server" /></td>
                                            </tr>
                                              <tr>
                                                <td width="20%">No of Member Renewed:</td>
                                                <td>
                                                    <asp:Label ID="lblTotalNoOfMemberRenewed" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td width="20%">Earning From Gym:</td>
                                                <td>
                                                    <asp:Label ID="lblEarningFromGym" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td width="20%">Total Suplements Bought:</td>
                                                <td>
                                                    <asp:Label ID="lblTotalSuplementsBought" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td width="20%">Total Suplements Sold:</td>
                                                <td>
                                                    <asp:Label ID="lblTotalSuplementsSold" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td width="20%">Total Expenses:</td>
                                                <td>
                                                    <asp:Label ID="lblTotalExpenses" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </div>

                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
