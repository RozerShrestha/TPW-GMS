<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewForm.aspx.cs" Inherits="TPW_GMS.NewForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <meta http-equiv="Refresh" />   
    <style>
        .col-sm-3{
            margin-bottom:10px;
        }
        .text-label{
            font-weight:500;
        }
    </style>
  <div id="newForm">
    <asp:HiddenField ID="hidImage" runat="server" /> 
      <asp:HiddenField ID="hidCommission" runat="server" />
      <asp:HiddenField ID="hidHeader" runat="server" Value="Membership Registration Form" />
      <asp:UpdatePanel ID="upnlNewForm" runat="server">
          <Triggers>
              <asp:PostBackTrigger ControlID="btnUpload" />              
          </Triggers>
          <ContentTemplate>         
              <asp:Panel ID="pnlNewform" runat="server">
                  <div class="row">
                      <div class="col-xs-12">
                          <div class="box box-info">
                              <div class="box-body">
                                  <div class="col-sm-3">
                                      <asp:Image ID="image1" runat="server" Width="130px" Height="150px" />
                                      <asp:FileUpload ID="FileUpload1" runat="server" />
                                      <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                                      <asp:Label ID="lblFileUploadErrorMessage" runat="server" Text=""></asp:Label>
                                  </div>
                                  <div class="col-sm-3">
                                      <asp:DropDownList ID="ddlActiveInactive" onchange="javascript:activeInactiveBGChange();" runat="server" CssClass="form-control input-sm">
                                          <asp:ListItem Value="0" Text="Active"></asp:ListItem>
                                          <asp:ListItem Value="1" Text="InActive"></asp:ListItem>
                                      </asp:DropDownList>
                                  </div>
                                  <div class="col-sm-6">
                                  </div>
                                  <div class="col-sm-12 header">
                                      Personal Information
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Member Id</span><span class="asterik">*</span><asp:Label ID="lblmemberId" runat="server" Style="color: red; float: right;"></asp:Label>
                                      <asp:TextBox ID="txtMemberId" runat="server" Enabled="false" AutoPostBack="true" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Branch</span>
                                      <asp:TextBox ID="txtBranch" runat="server" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Username</span> <span class="asterik">*</span><asp:Label ID="lblUserValid" runat="server" Style="float: right;"></asp:Label>
                                      <asp:TextBox ID="txtuname" runat="server" OnTextChanged="txtuname_TextChanged" AutoPostBack="true" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Password</span> <span class="asterik">*</span>
                                      <asp:TextBox ID="txtPwd" TextMode="Password" Enabled="false" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Membership Option.......<span class="asterik">*</span>
                                              <asp:DropDownList ID="ddlMemberOption" runat="server" OnSelectedIndexChanged="ddlMemberOption_SelectedIndexChanged" CssClass="form-control input-sm" AutoPostBack="true">
                                                  <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                  <asp:ListItem Value="1" Text="Regular"></asp:ListItem>
                                                  <asp:ListItem Value="2" Text="OffHour"></asp:ListItem>
                                                  <asp:ListItem Value="3" Text="Universal"></asp:ListItem>
                                                  <asp:ListItem Value="5" Text="Trainer"></asp:ListItem>
                                                  <asp:ListItem Value="6" Text="Gym Admin"></asp:ListItem>
                                                  <asp:ListItem Value="7" Text="Operation Manager"></asp:ListItem>
                                                  <%--<asp:ListItem Value="8" Text="Free User"></asp:ListItem>--%>
                                                  <asp:ListItem Value="9" Text="Intern"></asp:ListItem>
                                                  <asp:ListItem Value="10" Text="Temporary Member"></asp:ListItem>
                                              </asp:DropDownList>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Membership Category</span> <span class="asterik">*</span><br />
                                      <asp:DropDownList ID="ddlCatagoryType" runat="server" Width="49%" OnSelectedIndexChanged="ddlCatagoryType_SelectedIndexChanged" Style="display: inline-block;" CssClass="form-control input-sm" AutoPostBack="true">
                                      </asp:DropDownList>
                                      <asp:DropDownList ID="ddlSubCatagoryType" runat="server" Width="49%" Style="display: inline-block;" CssClass="form-control input-sm"></asp:DropDownList>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Membership Date </span><span class="asterik">*</span>
                                      <asp:TextBox ID="txtMembershipDate" placeholder="yyyy/mm/dd" runat="server" CssClass="form-control input-sm nepCalendar"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Membership payment type</span> <span class="asterik">*</span>
                                      <asp:DropDownList ID="ddlMembershipPaymentType" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlMembershipPaymentType_SelectedIndexChanged" AutoPostBack="true">
                                          <asp:ListItem Text="Select"></asp:ListItem>
                                          <asp:ListItem Value="1" Text="1 Month"></asp:ListItem>
                                          <asp:ListItem Value="3" Text="3 Month"></asp:ListItem>
                                          <asp:ListItem Value="6" Text="6 Month"></asp:ListItem>
                                          <asp:ListItem Value="12" Text="12 Month"></asp:ListItem>
                                          <asp:ListItem Value="60" Text="N/A"></asp:ListItem>
                                          <asp:ListItem Value="PerDay" Text="1 Day"></asp:ListItem>
                                          <asp:ListItem Value="TenDays" Text="10 Days"></asp:ListItem>
                                      </asp:DropDownList>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Membership Renew Date </span><span class="asterik">*</span>
                                      <%--<asp:TextBox ID="txtMembershipBeginDate" ClientIDMode="Static" AutoPostBack="true" OnTextChanged="txtMembershipBeginDate_TextChanged" runat="server" CssClass="form-control input-sm nepali-calendar"></asp:TextBox>--%>
                                      <asp:TextBox ID="txtMembershipBeginDate" ClientIDMode="Static" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Membership Expire Date </span><span class="asterik">*</span>
                                      <asp:TextBox ID="txtMembershipExpireDate" ClientIDMode="Static" Enabled="false" runat="server" CssClass="form-control input-sm nepCalendar"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Shift </span><span class="asterik">*</span>
                                      <asp:DropDownList ID="ddlShift" runat="server" CssClass="form-control input-sm">
                                          <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                          <asp:ListItem Value="1" Text="Morning"></asp:ListItem>
                                          <asp:ListItem Value="2" Text="Day"></asp:ListItem>
                                          <asp:ListItem Value="3" Text="Evening"></asp:ListItem>
                                      </asp:DropDownList>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Email</span><span class="asterik">*</span>
                                      <asp:TextBox ID="txtEmail" TextMode="Email" runat="server" CssClass="form-control form-control-uppercase"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">First Name </span><span class="asterik">*</span>
                                      <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Last Name </span><span class="asterik">*</span>
                                      <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Contact No </span><span class="asterik">*</span>
                                      <asp:TextBox ID="txtContactNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Date Of Birth </span><span class="asterik">*</span>
                                      <asp:TextBox ID="txtDateOfBirth" runat="server" placeholder="yyyy/mm/dd" ClientIDMode="Static" CssClass="form-control input-sm nepCalendar"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Address</span> <span class="asterik">*</span>
                                      <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Emergency  Contact Person</span> <span class="asterik">*</span>
                                      <asp:TextBox ID="txtEmergencyContactPerson" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Gender</span> <span class="asterik">*</span>
                                      <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control input-sm">
                                          <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                          <asp:ListItem Value="1" Selected="True" Text="Male"></asp:ListItem>
                                          <asp:ListItem Value="2" Text="Female"></asp:ListItem>
                                          <asp:ListItem Value="3" Text="Others"></asp:ListItem>
                                      </asp:DropDownList>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Emergency Contact No</span> <span class="asterik">*</span>
                                      <asp:TextBox ID="txtEmergencyContactPhone" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Been to Gym before, How Long</span><br />
                                      <asp:DropDownList ID="ddlGymAnytimeBefore" OnSelectedIndexChanged="ddlGymAnytimeBefore_SelectedIndexChanged" Style="display: inline-block;" Width="51%" runat="server" AutoPostBack="true" CssClass="form-control input-sm">
                                          <asp:ListItem Value="1" Text="Select"></asp:ListItem>
                                          <asp:ListItem Value="2" Text="Yes"></asp:ListItem>
                                          <asp:ListItem Value="3" Selected="True" Text="No"></asp:ListItem>
                                      </asp:DropDownList>
                                      <asp:TextBox ID="txtHowLong" Width="47%" Visible="false" runat="server" Style="display: inline-block;" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Any Health Issues</span>
                                      <asp:TextBox ID="txtAnyHealthIssue" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-12">
                                  </div>
                                  <div class="col-sm-12 header">
                                      <label data-toggle="collapse" style="cursor: pointer;" data-target="#demo1">Body Measurements</label>
                                  </div>
                                  <div class="col-sm-12">
                                      <div class="table table-responsive">
                                          <div id="demo1">
                                          <%--<div id="demo1" class="<%=state %>">--%>
                                              <asp:GridView ID="gridBodyMesurement" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" GridLines="None" OnRowDeleting="gridBodyMesurement_RowDeleting">
                                                  <AlternatingRowStyle BackColor="white" />
                                                  <HeaderStyle Font-Bold="true" />
                                                  <Columns>
                                                      <asp:TemplateField HeaderText="Sn.">
                                                          <ItemTemplate>
                                                              <%#Container.DataItemIndex+1 %>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Date">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtDate" Width="100%" CssClass="form-control input-sm nepCalendar" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Weight">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtWeight" Width="100%" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Height">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtHeight" Width="100%" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Upper Arm">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtUpperArm" Width="100%" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Fore Arm">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtForeArm" Width="100%" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Chest">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtChest" Width="100%" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Waist">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtWaist" Width="100%" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Thigh">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtThighs" Width="100%" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Calf">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtCalf" Width="100%" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:CommandField HeaderText="Action" ShowDeleteButton="true" />
                                                  </Columns>

                                              </asp:GridView>
                                              <div style="text-align: right;">
                                                  <asp:Button ID="btnAddMore" CssClass="btn btn-sm" runat="server" Text="Add New Row" OnClick="btnAddMore_Click" />
                                              </div>
                                          </div>
                                      </div>
                                  </div>

                                  <div class="col-sm-12 header">
                                      Payment Detail
                                  </div>
                                  <div class="col-sm-3">
                                      Payment Amount
                                        <asp:TextBox ID="txtPaymentAmount" runat="server" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      Discount Code
                                      <asp:TextBox ID="txtDiscountCode" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      Discount
                                        <asp:TextBox ID="txtDiscount" runat="server" Style="display: inline" Text="0" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      Discount Reason
                                        <asp:TextBox ID="txtDiscountReason" placeholder="Discount Reason" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      Admission Fee
                                        <asp:TextBox ID="txtAdmissionFee" runat="server" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      Final Amount
                                        <asp:TextBox ID="txtFinalAmount" runat="server" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      Paid Amount
                                        <asp:TextBox ID="txtpaidAmount" runat="server" Text="0" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      Due Amount
                                        <asp:TextBox ID="txtDueAmount" runat="server" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      Payment Method
                                      <asp:DropDownList ID="ddlPaymentMethod" runat="server" OnSelectedIndexChanged="ddlPaymentMethod_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control input-sm">
                                          <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                          <asp:ListItem Value="1" Text="Cash"></asp:ListItem>
                                          <asp:ListItem Value="2" Text="Card"></asp:ListItem>
                                           <asp:ListItem Value="3" Text="Cheque"></asp:ListItem>
                                           <asp:ListItem Value="4" Text="E-Banking"></asp:ListItem>
                                      </asp:DropDownList>
                                      <asp:Panel ID="pnlCheque" runat="server" Visible="false">
                                          Bank
                                          <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                          Cheque Number
                                          <asp:TextBox ID="txtChequeNumber" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                      </asp:Panel>
                                     <asp:Panel ID="pnlEBanking" runat="server" Visible="false">
                                         Reference ID
                                         <asp:TextBox ID="txtReferenceId" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                     </asp:Panel>
                                  </div>
                                  <div class=" form-row col-sm-3">
                                      Receipt No<span class="asterik">*</span><br />
                                      <%--<asp:TextBox ID="txtReceiptNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>--%>
                                      <div class="col-md-4" style="padding-left: 0px; padding-right: 0px;">
                                          <asp:TextBox ID="txtStatic" CssClass="form-control" ClientIDMode="Static" runat="server" disabled ></asp:TextBox>
                                      </div>
                                      <div class="col-md-8" style="padding-left: 0px; padding-right: 0px;">
                                          <asp:TextBox ID="txtReceiptNo" CssClass=" form-control" ClientIDMode="Static" runat="server" ></asp:TextBox>
                                      </div>
                                  </div>
                                  <div class="col-sm-4">
                                      Remark:
                                      <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-2">
                                      Action Taker<span class="asterik">*</span><br />
                                      <asp:DropDownList ID="ddlActionTaker" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                  </div>
                                  <div class="col-sm-12">
                                      <asp:Button ID="btnPriceCalculate" runat="server" Text="Calculate" CssClass="btn btn-danger" OnClick="btnPriceCalculate_Click" />
                                      <asp:Button ID="btnSubmit" runat="server" Text="Submit" Enabled="false" CssClass="btn btn-success" Width="85px" OnClick="btnSubmit_Click" />
                                      <asp:Label ID="lblInformation" runat="server"></asp:Label>
                                  </div>
                              </div>
                          </div>
                      </div>
                  </div>
                  <div class="modal fade" id="errorModal" role="dialog">
                      <div class="modal-dialog">
                          <!-- Modal content-->
                          <div class="modal-content">
                              <div class="modal-header">
                                  <button type="button" class="close" data-dismiss="modal">&times;</button>
                                  <h4 class="modal-title">Error</h4>
                              </div>
                              <div class="modal-body">
                                  Error has occured:
                            <asp:Label ID="lblPopupError" runat="server" ForeColor="Red" />
                              </div>
                              <div class="modal-footer">
                                  <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Close</button>
                              </div>
                          </div>
                      </div>
                  </div>
              </asp:Panel>
          </ContentTemplate>
      </asp:UpdatePanel>        
  </div>
    <script>
        $(document).ready(function () {
            activeInactiveBGChange();
            
        });
        $("#txtMembershipBeginDate").nepaliDatePicker({
            ndpYear: true,
            ndpMonth: true,
            ndpYearCount: 10,
            onChange: renewChange,
            dateFormat: "YYYY/MM/DD"
            });
      
        function renewChange() {
            //check if the intern
            let addMonth = $('#ContentPlaceHolder1_ddlMemberOption').val() == "9" ? "3" : $('#ContentPlaceHolder1_ddlMembershipPaymentType').val();
            let addDays =  $('#ContentPlaceHolder1_ddlMembershipPaymentType').val() == "PerDay" ? "1" :
                           $('#ContentPlaceHolder1_ddlMembershipPaymentType').val() == "TenDays" ? "10" : 0;

            let renewDateNep = $("#txtMembershipBeginDate").val();
            let renewDateObjNep = NepaliFunctions.ConvertToDateObject(renewDateNep, "YYYY/MM/DD");
            let renewDateObjEng = NepaliFunctions.BS2AD(renewDateObjNep);
            let renewDateEng = NepaliFunctions.ConvertDateFormat(renewDateObjEng, "YYYY/MM/DD");

            let dt = new Date(renewDateEng);
            if ($('#ContentPlaceHolder1_ddlMemberOption').val() == "10") {
                dt.setDate(dt.getDate() + parseInt(addDays));
            }
            else {
                dt.setMonth(dt.getMonth() + parseInt(addMonth));
            }
            
            let expiryDateEng = $.datepicker.formatDate('yy/mm/dd', dt);

            let expiryDateObjEng = NepaliFunctions.ConvertToDateObject(expiryDateEng, "YYYY/MM/DD");
            let expiryDateObjNep = NepaliFunctions.AD2BS(expiryDateObjEng);
            let expiryDate = NepaliFunctions.ConvertDateFormat(expiryDateObjNep, "YYYY/MM/DD");
            //let expireDateObj = NepaliFunctions.BsAddDays(renewDateObj, parseInt(addDays));
            //let expireDate = NepaliFunctions.ConvertDateFormat(expireDateObj, "YYYY/MM/DD");
            $("#txtMembershipExpireDate").val(expiryDate);
        }

       
        function activeInactiveBGChange() {
            var ddlValue = $('#ContentPlaceHolder1_ddlActiveInactive option:selected').text();
            if (ddlValue == "Active")
                $('#ContentPlaceHolder1_ddlActiveInactive').css({ "background": "#057d05", "font-weight": "bold", "color": "white" });
            else
                $('#ContentPlaceHolder1_ddlActiveInactive').css({ "background": "#c0262e", "font-weight": "bold", "color": "white" });
        }
        function unameToLower() {
            var usernameNoSpace = document.getElementById('ContentPlaceHolder1_txtuname');
            usernameNoSpace.value = $('#ContentPlaceHolder1_txtuname').val().toLowerCase();
        }
        function RemoveSpaceUsername() {
            var username = document.getElementById('ContentPlaceHolder1_txtuname').value;
            $('#ContentPlaceHolder1_txtuname').val(username.replace(/\s/g, ''))
        }

    </script>
</asp:Content>
