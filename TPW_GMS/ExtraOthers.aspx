<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExtraOthers.aspx.cs" Inherits="TPW_GMS.ExtraOthers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hidHeader" runat="server" Value="Extras" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <section class="content">
                <div class="row">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
                <div class="row">
                    <div class="box box-default">
                        <div class="box-header with-border">
                            <h3 class="box-title header">Details</h3>
                        </div>
                        <div class="box-body">
                            <div class="col-sm-12">
                                Email Address
                            <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                Password
                            <asp:TextBox ID="txtEmailPassword" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                Admission
                            <asp:TextBox ID="txtAdmission" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                SMTP Client
                            <asp:TextBox ID="txtSmtpClient" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                Port
                            <asp:TextBox ID="txtPort" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                Staff Intime Extension
                            <asp:TextBox ID="txtStaffIntimeExtenstion" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                Current Nepali Year
                                <asp:TextBox ID="txtNepaliDate" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="btnEmailSave" runat="server" CssClass=" btn btn-success" Text="Save" Style="margin-top: 17px" OnClick="btnEmailSave_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="box box-default">
                        <div class="box-header with-border">
                            <h3 class="box-title header">Stop Start</h3>
                        </div>
                        <div class="box-body">
                             <div class="col-sm-2">
                                1 month
                            <asp:TextBox ID="txtOneMonth" TextMode="Number" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-2">
                                3 month
                            <asp:TextBox ID="txtThreeMonth" TextMode="Number" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-2">
                                6 month
                            <asp:TextBox ID="txtSixMonth" TextMode="Number" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-2">
                                12 month
                            <asp:TextBox ID="txtTwelveMonth" TextMode="Number" runat="server" CssClass="form-control input-sm" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="btnSaveStopStart" runat="server" CssClass="btn btn-success" Style="margin-top: 17px" Text="save" OnClick="btnSaveStopStart_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="box box-default">
                        <div class="box-header with-border">
                            <h3 class="box-title header">Influencer</h3>
                        </div>
                        <div class="box-body">
                            <div class="col-sm-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gridInfluencer" runat="server" head Width="100%" AutoGenerateColumns="false" OnRowDeleting="gridInfluencer_RowDeleting" OnRowDataBound="gridInfluencer_RowDataBound" CellPadding="4" GridLines="None" CssClass="table table-bordered table-hover">
                                        <AlternatingRowStyle BackColor="White" />
                                        <HeaderStyle Font-Bold="true" />
                                        <HeaderStyle BackColor="#dedede" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sn." HeaderStyle-Font-Bold="true" HeaderStyle-Width="10px" ItemStyle-Width="10px">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Influencer Name">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtInfluencerName" CssClass="form-control input-sm" Width="240px" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="From">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFrom" CssClass="form-control input-sm nepCalendar" Width="240px" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="To">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTo" CssClass="form-control input-sm nepCalendar" Width="240px" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount Code">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtInfluencerCode" CssClass="form-control input-sm" Width="240px" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Commission">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtInfluencerCommision" CssClass="form-control input-sm" Width="240px" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ContactNo">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtContactNO" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkStatus" runat="server"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField HeaderText="Action" ShowDeleteButton="true" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Label ID="lblInfo" Style="margin-left: 10px" runat="server"></asp:Label>
                                    <asp:Button ID="btnAddMore" CssClass="btn btn-sm" runat="server" Style="float: right" Text="Add New Row" OnClick="btnAddMore_Click" />
                                    <asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" Style="float: left" Text="Submit" OnClick="btnSubmit_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
               <%-- <div class="row">
                    <div class="box box-default">
                        <div class="box-header with-border">
                            <h3 class="box-title header">Email Marketing Content</h3>
                        </div>
                        <div class="box-body">
                             <asp:TextBox ID="txtSubject" runat="server" placeholder="Subject" CssClass="form-control input-sm"></asp:TextBox>
                            <!-- The toolbar will be rendered in this container. -->
                            <div id="toolbar-container"></div>
                            <!-- This container will become the editable. -->
                            <div id="editor" style="min-height: 100px; border-color:#c7c7c7; border-width:1px"></div>
                            <button type="button" id="btnSave" class="btn btn-success btn-sm">Save</button>
                        </div>
                    </div>
                </div>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
          <script>
        //function pageLoad() {
        //    let editorMessage;
        //    DecoupledEditor
        //        .create(document.querySelector('#editor'))
        //        .then(editor => {
        //            editorMessage = editor;
        //            const toolbarContainer  = document.querySelector('#toolbar-container');

        //            toolbarContainer .appendChild(editor.ui.view.toolbar.element);
        //        })
        //        .catch(error => {
        //            console.error(error);
        //        });
        //    loadEmailMarketingContent();
        //    $('#btnSave').click(function () {
        //        var message = editorMessage.getData();
        //        var subject = document.getElementById("ContentPlaceHolder1_txtSubject").value;
        //        if (message == "" || subject == "") {
        //            alert("Subject or Message Block should not be empty");
        //            return;
        //        }
        //        $.ajax({
        //            url: 'api/SaveMarketingEmailFormat',
        //            headers: {
        //                'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
        //            },
        //            type: "patch",
        //            async: true,
        //            data: {
        //                message: message,
        //                subject: subject,
        //                type:'write'
        //            },
        //            success: function (response) {
        //                alert(response.status);
        //            },
        //            error: function () {

        //            }
        //        })

        //    });
        //    function loadEmailMarketingContent() {
        //        $.ajax({
        //            url: 'api/SaveMarketingEmailFormat',
        //            headers: {
        //                'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
        //            },
        //            type: "patch",
        //            async: true,
        //            data: {
        //                type: 'read'
        //            },
        //            success: function (response) {
        //                //editor.setData(response.message)
        //                editorMessage.setData(response.message.emailFormat);
        //                $('#ContentPlaceHolder1_txtSubject').val(response.message.subject);
        //                //ClassicEditor.instances['editor'].setData(response.message);
        //                //alert(response);
        //            },
        //            error: function () {

        //            }
        //        })
        //    } 
        //}
</script>
         
</asp:Content>
