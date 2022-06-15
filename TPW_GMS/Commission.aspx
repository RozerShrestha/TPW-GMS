<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Commission.aspx.cs" Inherits="TPW_GMS.Commission" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        //function UserDeleteConfirmation() {
        //    return confirm("Are you sure you want to delete this user?");
        //}
        function EditCommission() {
            return confirm("Are you sure you want to change the Payment Status ?");
        }
        function EditCommissionAll() {
            return "Are you sure, you want to change the payment Status of All Selected";
        }
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original 
                        if (row.rowIndex % 2 == 0) {
                            //Alternating Row Color
                        }
                        else {
                        }
                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script>
    <asp:HiddenField ID="hidHeader" runat="server" Value="Commission Search" />
    <asp:UpdatePanel ID="upnlCommission" runat="server">
        <%-- <Triggers>
              <asp:PostBackTrigger ControlID="chkHeader" />              
          </Triggers>--%>
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-info">
                        <div class="box-title">

                        </div>
                        <div class="box-body">
                            <div class="col-sm-2">
                                Branch
                                <asp:DropDownList ID="ddlBranch" ClientIDMode="Static" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                            </div>
                            <div class="col-sm-2">
                                Type
                                <asp:DropDownList ID="ddlType" runat="server" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control">
                                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Trainer"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Influencer"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Gym Admin"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Operation Manager"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-2">
                                Name
                                <asp:DropDownList ID="ddlName" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="col-sm-1">
                                From
                                <asp:TextBox ID="txtDateCommissionFrom" runat="server" CssClass="form-control nepCalendar" />
                            </div>
                            <div class="col-sm-1">
                                To
                                <asp:TextBox ID="txtDateCommissionTo" runat="server" CssClass="form-control nepCalendar" />
                            </div>
                            <div class="col-sm-2">
                                Payment Status
                                <asp:DropDownList ID="ddlPaidUnpaid" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="2" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="true" Text="Paid"></asp:ListItem>
                                    <asp:ListItem Value="false" Text="Not Paid"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" Style="margin-top: 18px" OnClick="btnSearch_Click" CssClass="btn btn-success" />
                            </div>
                            <br />
                            <div class="col-sm-12">
                                <asp:Label ID="lblInfo" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-solid">
                        <div class="box box-body">
                            <div class="table-responsive">
                                <asp:GridView ID="gridCommission" runat="server" Font-Size="12px" ShowFooter="true" AutoGenerateColumns="false" OnPageIndexChanging="gridCommission_PageIndexChanging" OnRowCommand="gridCommission_RowCommand" OnRowDataBound="gridCommission_RowDataBound" AllowPaging="true" PageSize="50" CssClass="table table-bordered table table-hover">
                                    <HeaderStyle BackColor="#a3b8c4" />
                                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                                    <FooterStyle Font-Bold="true" BackColor="#f1f1f1" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sn" HeaderStyle-Font-Bold="true">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" HeaderStyle-Font-Bold="true">
                                            <ItemTemplate>
                                                <asp:Label ID="txtId" Text='<%# Eval("Id") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Branch" DataField="branch" />
                                        <asp:BoundField HeaderText="Name" DataField="Name" />
                                        <asp:BoundField HeaderText="Discount Code" DataField="DiscountCode" />
                                        <asp:BoundField HeaderText="Member Id" DataField="MemberId" />
                                        <asp:BoundField HeaderText="Member Name" DataField="MemberName" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkHeader" onclick="checkAll(this)" runat="server" />Paid Status
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%--&nbsp;--%>
                                                <asp:CheckBox ID="chkPaidStatus" Enabled="false" Checked='<%# Eval("status") %>' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="label1" runat="server" Text="Total"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Commission</HeaderTemplate>
                                            <ItemTemplate>
                                                <%--&nbsp;--%>
                                                <asp:Label ID="lblCommission" Text='<%# Eval("Comission") %>' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalCommission" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Action</HeaderTemplate>
                                            <ItemTemplate>
                                                &nbsp;&nbsp;
                                                    <asp:LinkButton ID="linkEdit" runat="server" Text="edit" CommandName="editroww" CommandArgument='<%# Eval("Id")%>'>pay</asp:LinkButton>
                                                <%--&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                                <%--<asp:LinkButton ID="LinkDelete" runat="server" Text="delete" OnClientClick="if(! UserDeleteConfirmation()) return false;" CommandName="deleteRow" CommandArgument='<%# Eval("loginId")%>'><img src="Assets/Icon/delete.png" class="iconEdit" /></asp:LinkButton>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>No Record Found!</EmptyDataTemplate>
                                    <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                </asp:GridView>
                                <asp:LinkButton ID="lnkPayAll" style="float:right" runat="server" OnClick="lnkPayAll_Click" Text="PayAll"></asp:LinkButton>
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>
