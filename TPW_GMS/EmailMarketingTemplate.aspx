<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmailMarketingTemplate.aspx.cs" Inherits="TPW_GMS.EmailMarketingTemplate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
    
    <asp:HiddenField ID="hidHeader" runat="server" Value="Email Marketing Template" />
     <asp:UpdatePanel runat="server">
        <ContentTemplate>
         <asp:Panel ID="pnlEmMarketing" runat="server">
             <section class="content">
                 <div class="row">
                     <div class="box box-default">
                         <div class="box-header">
                             <h4>Email Marketing 1</h4>
                             <div class="col-sm-3">
                                <asp:TextBox ID="txtSubject1" runat="server" placeholder="Subject" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                         </div>
                         <div class="box-body">
                             
                             <CKEditor:CKEditorControl ID="CKEditor1" BasePath="/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                         </div>
                         <div class="box-footer">
                             <asp:Button ID="btnCKEditor1" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnCKEditor_Click" />
                         </div>
                     </div>
                 </div>
                 <div class="row">
                     <div class="box box-default">
                         <div class="box-header">
                             <h4>Email Marketing 2</h4>
                             <div class="col-sm-3">
                                <asp:TextBox ID="txtSubject2" runat="server" placeholder="Subject" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                         </div>
                         <div class="box-body">
                             <CKEditor:CKEditorControl ID="CKEditor2" BasePath="/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                         </div>
                         <div class="box-footer">
                             <asp:Button ID="btnCKEditor2" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnCKEditor_Click" />
                         </div>
                     </div>
                 </div>
                 <div class="row">
                     <div class="box box-default">
                         <div class="box-header">
                             <h4>Email Marketing 3</h4>
                             <div class="col-sm-3">
                                <asp:TextBox ID="txtSubject3" runat="server" placeholder="Subject" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                         </div>
                         <div class="box-body">
                             <CKEditor:CKEditorControl ID="CKEditor3" BasePath="/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                         </div>
                         <div class="box-footer">
                             <asp:Button ID="btnCKEditor3" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnCKEditor_Click" />
                         </div>
                     </div>
                 </div>
                 <div class="row">
                     <div class="box box-default">
                         <div class="box-header">
                             <h4>Email Marketing 4</h4>
                             <div class="col-sm-3">
                                <asp:TextBox ID="txtSubject4" runat="server" placeholder="Subject" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                         </div>
                         <div class="box-body">
                             <CKEditor:CKEditorControl ID="CKEditor4" BasePath="/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                         </div>
                         <div class="box-footer">
                             <asp:Button ID="btnCKEditor4" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnCKEditor_Click" />
                         </div>
                     </div>
                 </div>
             </section>
         </asp:Panel>
            </ContentTemplate>
         </asp:UpdatePanel>
</asp:Content>
