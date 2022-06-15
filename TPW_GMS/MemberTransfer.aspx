<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MemberTransfer.aspx.cs" Inherits="TPW_GMS.MemberTransfer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box box-info">
                                <div class="box-title">
                                    <asp:HiddenField ID="hidHeader" runat="server" Value="Membership Transfer" />
                                </div>
                                <div class="box-body">
                                    <div class="col-sm-3">
                                        TPW-Member Name
                                        <asp:DropDownList ID="ddlMemberName" AutoPostBack="true" CssClass="select2Example form-control input-sm" OnSelectedIndexChanged="ddlMemberName_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3">
                                        Member Id:
                                        <asp:DropDownList ID="ddlMemberId" AutoPostBack="true" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3">
                                        Current Branch:
                                        <asp:DropDownList ID="ddlCurrentBranch" AutoPostBack="true" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3">
                                        Destination Branch:
                                        <asp:DropDownList ID="ddlDestinationBranch" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Button ID="btnSubmit" runat="server" Style="margin-top: 19px" OnClick="btnSubmit_Click" Text="Submit" CssClass="btn btn-sm btn-success" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <asp:Label ID="lblMessage" Style="margin-left: 10px" runat="server"></asp:Label>
                    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
