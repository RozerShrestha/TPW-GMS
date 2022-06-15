<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test1.aspx.cs" Inherits="TPW_GMS.test1" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<form id="form1" runat="server">
    <div>
        <CKEditor:CKEditorControl ID="CKEditor1" BasePath="/ckeditor/" runat="server"></CKEditor:CKEditorControl>
    </div>
    <div>
        <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
    </div>
    <div>
        <asp:label runat="server" ID="lblText"></asp:label>
    </div>
    <div>
        <asp:label runat="server" ID="lbl2"></asp:label>
    </div>
    <div>
       
    </div>
</form>

