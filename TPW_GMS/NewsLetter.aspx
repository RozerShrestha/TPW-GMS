<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewsLetter.aspx.cs" Inherits="TPW_GMS.NewsLetter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="Assets/MultiSelect/multi-select.css" rel="stylesheet" />
    <asp:HiddenField ID="hidHeader" runat="server" Value="NewsLetter/Email" />
    <asp:HiddenField ID="hidEmails" runat="server" />
    <div class="row">
        <div class="col-xs-12">
            <div class="box box-solid">
                <div class="box-body">
                    <div class="col-sm-3">
                        Date
                        <asp:TextBox ID="txtDateNewsletter" placeholder="Date" runat="server" CssClass="form-control input-sm dateControl" />
                    </div>
                    <div class="col-sm-3">
                        Member Option
                        <asp:DropDownList ID="ddlMemberOption" runat="server" CssClass="form-control input-sm">
                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Regular"></asp:ListItem>
                            <asp:ListItem Value="2" Text="OffHour"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Universal"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Trainer"></asp:ListItem>
                            <asp:ListItem Value="6" Text="Gym Admin"></asp:ListItem>
                            <asp:ListItem Value="7" Text="Super Admin"></asp:ListItem>
                            <asp:ListItem Value="8" Text="Free User"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-3">
                        <asp:Button ID="btnLoadMember" Style="margin-top: 18px" runat="server" OnClick="btnLoadMember_Click" CssClass="btn btn-success btn-sm" Text="Load" />
                    </div>
                    <div class="col-sm-12">
                        List of Members 
                    </div>
                    <div class="col-sm-12">
                        <asp:DropDownList ID="ddlGymMember" multiple="multiple" Width="500px"  CssClass="select" placeholder="List of Members" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-sm-12">
                        Subject
                        <asp:TextBox ID="txtSubject" runat="server" placeholder="Subject" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-12">
                        <!-- The toolbar will be rendered in this container. -->
                        <div id="toolbar-container"></div>
                        <!-- This container will become the editable. -->
                        <div id="editor" style="min-height: 100px; border-color:#c7c7c7; border-width:1px">Dear $,</div>
                    </div>
                    <br />
                    <div class="col-sm-12">
                         <button type="button" id="btnSendEmail" class="btn btn-success btn-sm">Send Email</button>
                         <img id="imgLoading" src="Assets/Images/ajax-loader.gif" style="margin-top: 14px; width: 226px; display:none" />
                    </div> 
                </div>
            </div>
        </div>
    </div>
</asp:Content>
