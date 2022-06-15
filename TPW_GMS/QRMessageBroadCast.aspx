<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QRMessageBroadCast.aspx.cs" Inherits="TPW_GMS.QRMessageBroadCase" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://unpkg.com/multiple-select@1.3.1/dist/multiple-select.css" rel="stylesheet">
    <style>
        select {
            width: 500px;
        }
    </style>
    <asp:HiddenField ID="hidHeader" runat="server" Value="Send Email" />
    <asp:HiddenField ID="hidEmails" runat="server" />
    <div class="row">
        <div class="col-xs-12">
            <div class="box box-solid">
                <div class="box-body">
                    <br />
                    <div class="col-md-12">
                        <div class="col-sm-4">
                            All Members:
                            <asp:DropDownList ID="ddlGymMember" multiple="multiple" runat="server"></asp:DropDownList>
                        </div>
                        <div class="col-sm-4">
                            Subject:
                            <asp:TextBox ID="txtSubject" runat="server" Text="QR Code" placeholder="Subject" CssClass="form-control input-sm"></asp:TextBox>
                        </div>
                        <br />
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
    </div>
    <script src="https://unpkg.com/multiple-select@1.3.1/dist/multiple-select.js"></script>
    <script>
        $(function () {
            let editorMessage;
            $('#ContentPlaceHolder1_ddlGymMember').multipleSelect();
            DecoupledEditor
                .create(document.querySelector('#editor'))
                .then(editor => {
                    editorMessage = editor;
                    const toolbarContainer = document.querySelector('#toolbar-container');

                    toolbarContainer.appendChild(editor.ui.view.toolbar.element);
                })
                .catch(error => {
                    console.error(error);
                });
            $('#btnSendEmail').click(function () {
                $("#imgLoading").css("display", "block");
                var fullName_email = [];
                //var message = CKEDITOR.instances.editor1.getData();
                var message = editorMessage.getData();
                var subject = document.getElementById("ContentPlaceHolder1_txtSubject").value;
                if (subject != "") {
                    $('SELECT#ContentPlaceHolder1_ddlGymMember option:selected').each(function () {
                        fullName_email.push($(this).text() + "##" + $(this).val());
                    });

                    $.ajax({
                        url: 'api/SendBulkEmail',
                        headers: {
                            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                        },
                        type: "post",
                        async: true,
                        data: {
                            message: message,
                            fullName_email: fullName_email,
                            subject: subject,
                            type: "QR",
                        },
                        success: function (response) {
                            alert(response);
                            $("#imgLoading").css("display", "none");
                        },
                        error: function () {
                        }
                    })
                }
                else {
                    alert("Subject Required");
                }
            });
        });
       
        

</script>
</asp:Content>
