<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditForm.aspx.cs" Inherits="TPW_GMS.EditForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .text-label{
            font-weight:500;
        }
        .lockerNo{
            color:white;
            background-color: dodgerblue;
            padding: 5px;
        }
        .lockerExpired{
            color:white;
            background-color: red;
            padding: 5px;
        }
        .lockerNotExpired{
            color:white;
            background-color: green;
            padding: 5px;
        }
    </style>
<div id="newForm">
    <asp:HiddenField ID="hidImage" runat="server" />
    <asp:HiddenField ID="hidHeader" runat="server" Value="Membership Registration Form" />
    <asp:UpdatePanel ID="upnlNewForm" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
            <asp:PostBackTrigger ControlID="btnStartStopModal" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="pnlEditForm" runat="server">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box box-info">
                                <div class="box-body">
                                    <div class="col-sm-2">
                                        <asp:Image ID="image1" runat="server" Width="130px" Height="150px" />
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                                        <asp:Label ID="lblFileUploadErrorMessage" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                       <%-- <% if (qrshow)
                                            { %>--%>
                                        <qrcode :value="qrId" :options="{ width: 200 }"></qrcode>
                                        <%--<%} %>--%>
                                    </div>
                                    <div class="col-sm-2">
                                        Status
<%--                                        <asp:DropDownList ID="ddlActiveInactive" onchange="javascript:activeInactiveBGChange();" runat="server" CssClass="form-control input-sm">
                                            <asp:ListItem Value="0" Text="Active"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="InActive"></asp:ListItem>
                                        </asp:DropDownList>--%>
                                        <asp:DropDownList ID="ddlActiveInactive" onchange="javascript:activeInactiveBGChange();" runat="server"  CssClass="form-control input-sm">
                                            <asp:ListItem Value="0" Text="Active"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="InActive"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-2">
                                        Locker<br />
                                        <asp:Label ID="lblLocker" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">
                                        Universal Membership Limit
                                        <asp:TextBox ID="txtUnivershipMembershipLimit" runat="server" CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Button ID="btnStart" runat="server" Text="Start" CssClass="btn btn-success" OnClick="btnStart_Click" />
                                        <asp:Button ID="btnStop" runat="server" Text="Stop" CssClass="btn btn-danger" OnClick="btnStop_Click" /><br />
                                        <table class="table table-bordered table-striped">
                                            <asp:Panel ID="pnlStopStart" runat="server" Visible="true">
                                                <tr>
                                                    <td>Stop Date:</td>
                                                    <td>
                                                        <asp:Label ID="lblStop" runat="server" /><br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Start Date:</td>
                                                    <td>
                                                        <asp:Label ID="lblStart" runat="server" /><br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Stop Days:</td>
                                                    <td>
                                                        <asp:Label ID="lblStopDays" runat="server" /></td>
                                                </tr>
                                                <tr>
                                                    <td>Stop Limit:</td>
                                                    <td>
                                                        <asp:Label ID="lblStopLimit" runat="server"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblStrtStopInfo" runat="server"></asp:Label></td>
                                                </tr>
                                            </asp:Panel>
                                        </table>
                                        <asp:Button ID="btnReset" runat="server" Visible="false" CssClass="btn btn-danger" Text="Reset" OnClick="btnReset_Click" />
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
                                       <asp:TextBox ID="txtuname" runat="server" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Password</span> <span class="asterik">*</span><span><input type="checkbox" onclick="myFunction()">Show Password</span>
                                      <asp:TextBox ID="txtPwd" TextMode="Password" Enabled="false" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                     <span class="text-label"> Membership Option.......<span class="asterik">*</span>
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
                                      </asp:DropDownList>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Membership Catagory</span> <span class="asterik">*</span><div></div>
                                      <asp:DropDownList ID="ddlCatagoryType" runat="server" Width="49%" OnSelectedIndexChanged="ddlCatagoryType_SelectedIndexChanged" Style="display: inline-block;" CssClass="form-control input-sm" AutoPostBack="true">
                                      </asp:DropDownList>
                                      <asp:DropDownList ID="ddlSubCatagoryType" runat="server" Width="49%" Style="display: inline-block;" CssClass="form-control input-sm"></asp:DropDownList>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Membership Date </span><span class="asterik">*</span>
                                      <asp:TextBox ID="txtMembershipDate" placeholder="yyyy/mm/dd" runat="server" ClientIDMode="Static" CssClass="form-control input-sm nepCalendar"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Membership payment type</span> <span class="asterik">*</span><asp:Label ID="lblPrePaymentType" runat="server"></asp:Label>
                                      <asp:DropDownList ID="ddlMembershipPaymentType" Enabled="false" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlMembershipPaymentType_SelectedIndexChanged" data-toggle="tooltip" data-delay="{ show: 1000, hide: 10000}" data-placement="top" title="Note: Membership payment Type is enabled only if the Reason for Update is Renew" AutoPostBack="true">
                                          <asp:ListItem Text="Select"></asp:ListItem>
                                          <asp:ListItem Value="1" Text="1 Month"></asp:ListItem>
                                          <asp:ListItem Value="3" Text="3 Month"></asp:ListItem>
                                          <asp:ListItem Value="6" Text="6 Month"></asp:ListItem>
                                          <asp:ListItem Value="12" Text="12 Month"></asp:ListItem>
                                          <asp:ListItem Value="60" Text="N/A"></asp:ListItem>
                                      </asp:DropDownList>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Membership Renew Date </span><span class="asterik">*</span>
                                      <asp:TextBox ID="txtMembershipBeginDate" placeholder="mm/dd/yyyy" Enabled="false" runat="server" AutoPostBack="true" ClientIDMode="Static" CssClass="form-control input-sm nepCalendar" OnTextChanged="txtMembershipBeginDate_TextChanged"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                     <span class="text-label"> Membership Expire Date </span><span class="asterik">*</span><asp:Label ID="lblPreExpireDate" runat="server"></asp:Label>
                                      <asp:TextBox ID="txtMembershipExpireDate" placeholder="mm/dd/yyyy" Enabled="false" runat="server" CssClass="form-control input-sm nepCalendar"></asp:TextBox>
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
                                     <span class="text-label"> Email</span><span class="asterik">*</span>
                                      <asp:TextBox ID="txtEmail" TextMode="Email" runat="server" CssClass="form-control form-control-uppercase"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                     <span class="text-label"> First Name </span><span class="asterik">*</span>
                                      <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                     <span class="text-label"> Last Name </span><span class="asterik">*</span>
                                      <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                     <span class="text-label"> Contact No </span><span class="asterik">*</span>
                                      <asp:TextBox ID="txtContactNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                     <span class="text-label"> Date Of Birth </span><span class="asterik">*</span>
                                      <asp:TextBox ID="txtDateOfBirth" runat="server" placeholder="yyyy/mm/dd" ClientIDMode="Static" CssClass="form-control input-sm nepCalendar"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                     <span class="text-label"> Address</span> <span class="asterik">*</span>
                                      <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                     <span class="text-label"> Emergency  Contact Person</span> <span class="asterik">*</span>
                                      <asp:TextBox ID="txtEmergencyContactPerson" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                     <span class="text-label"> Gender</span> <span class="asterik">*</span>
                                       <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control input-sm">
                                  <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                  <asp:ListItem Value="1" Text="Male"></asp:ListItem>
                                  <asp:ListItem Value="2" Text="Female"></asp:ListItem>
                                  <asp:ListItem Value="3" Text="Others"></asp:ListItem>
                              </asp:DropDownList>
                                  </div>
                                  <div class="col-sm-3">
                                      <span class="text-label">Emergency Contact No</span> <span class="asterik">*</span>
                                      <asp:TextBox ID="txtEmergencyContactPhone" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                       <span class="text-label">Been to Gym before, How Long</span>
                                       <asp:DropDownList ID="ddlGymAnytimeBefore" OnSelectedIndexChanged="ddlGymAnytimeBefore_SelectedIndexChanged" Style="display: inline-block;" Width="49%" runat="server" AutoPostBack="true" CssClass="form-control input-sm">
                                           <asp:ListItem Value="1" Text="Select"></asp:ListItem>
                                           <asp:ListItem Value="2" Text="Yes"></asp:ListItem>
                                           <asp:ListItem Value="3" Text="No"></asp:ListItem>
                                       </asp:DropDownList>
                                      <asp:TextBox ID="txtHowLong" Width="49%" Visible="false" runat="server" Style="display: inline-block;" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                  <div class="col-sm-3">
                                     <span class="text-label">Any Health Issues</span>
                                       <asp:TextBox ID="txtAnyHealthIssue" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                 <div class="col-sm-3">
                                        <asp:TextBox ID="txtGenerateDate" runat="server" placeholder="yyyy/mm/dd" CssClass="form-control input-sm nepCalendar" Width="250px"></asp:TextBox>
                                        <asp:Button ID="btnGenerateDate" OnClick="btnGenerateDate_Click" runat="server" CssClass="btn btn-danger" style="display:inline" Text="Change Renew Date" />
                                 </div>
                                <div class="col-sm-12 header">
                                    <label data-toggle="collapse" style="cursor:pointer;" data-target="#demo1">Body Measurements</label>
                                </div>
                                  <div class="col-sm-12">
                                      <div class="table table-responsive">
                                          <div id="demo1" class="<%=state %>">
                                              <asp:GridView ID="gridBodyMesurement" Width="100%" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" GridLines="None" OnRowDataBound="gridBodyMesurement_RowDataBound" OnRowDeleting="gridBodyMesurement_RowDeleting">
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
                                                              <asp:TextBox ID="txtDate" CssClass="form-control input-sm  nepCalendar" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Weight">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtWeight" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Height">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtHeight" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Upper Arm">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtUpperArm" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Fore Arm">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtForeArm" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Chest">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtChest" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Waist">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtWaist" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Thigh">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtThighs" CssClass="form-control input-sm" runat="server"></asp:TextBox>
                                                          </ItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Calf">
                                                          <ItemTemplate>
                                                              <asp:TextBox ID="txtCalf" CssClass="form-control input-sm" runat="server"></asp:TextBox>
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
                                        <label data-toggle="collapse" style="cursor:pointer;" data-target="#demo2">Payment History</label>
                                  </div>
                                    <div class="col-sm-12">
                                        <div class="table table-responsive">
                                            <div id="demo2" class="<%=state %>">
                                                <asp:GridView ID="gridReport" runat="server" AutoGenerateColumns="false" OnRowDataBound="gridReport_RowDataBound" CssClass="table table-bordered">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sn." HeaderStyle-Width="10px">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Receipt No" DataField="receiptNo" />
                                                        <asp:BoundField HeaderText="Payment Method" DataField="paymentMethod" />
                                                        <asp:TemplateField HeaderText="Renew Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRenewDate" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Expired Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblExpiredDate" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Catagory" DataField="memberCatagory" />
                                                        <asp:BoundField HeaderText="Payment Type" DataField="memberpaymentType" />
                                                        <asp:BoundField HeaderText="Member Option" DataField="memberOption" />
                                                         <asp:BoundField HeaderText="Discount" DataField="discount" />
                                                         <asp:BoundField HeaderText="Discount Reason" DataField="discountReason" />
                                                         <asp:BoundField HeaderText="Discount Code" DataField="discountCode" />
                                                        <asp:BoundField HeaderText="Fee" DataField="finalAmount" />
                                                        <asp:BoundField HeaderText="paid Amount" DataField="paidAmount" />
                                                        <asp:BoundField HeaderText="Due Amount" DataField="dueAmount" />
                                                        <asp:BoundField HeaderText="Due Clear Amount" DataField="dueClearAmount" />
                                                         <asp:TemplateField HeaderText="Created">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCreated" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:BoundField HeaderText="Action Taker" DataField="actionTaker" />
                                                    </Columns>
                                                    <%--<PagerStyle HorizontalAlign = "Right" CssClass = "GridPager" />--%>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-12 header">
                                        <label data-toggle="collapse" style="cursor:pointer;" data-target="#demo3">Version History</label>
                                  </div>
                                    <div class="col-sm-12">
                                        <div class="table table-responsive">
                                            <div id="demo3" class="<%=state %>">
                                                <asp:GridView ID="gridVersionHistory" runat="server" AutoGenerateColumns="false" OnRowDataBound="gridVersionHistory_RowDataBound" CssClass="table table-bordered">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sn." HeaderStyle-Width="10px">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Renew Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRenewDate" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Expired Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblExpiredDate" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Catagory" DataField="memberCatagory" />
                                                        <asp:BoundField HeaderText="Payment Type" DataField="memberpaymentType" />
                                                        <asp:BoundField HeaderText="Member Option" DataField="memberOption" />
                                                         <asp:TemplateField HeaderText="Created">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCreated" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Remark" DataField="remark" />
                                                        <asp:BoundField HeaderText="Branch" DataField="createdBy" />
                                                         <asp:BoundField HeaderText="Action Taker" DataField="actionTaker" />
                                                    </Columns>
                                                </asp:GridView>
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
                                   <div class="col-sm-2">
                                        Paid Amount
                                        <asp:TextBox ID="txtpaidAmount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                   <div class="col-sm-2">
                                        Due Amount
                                        <asp:TextBox ID="txtDueAmount" runat="server" Enabled="false" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                    <div class="col-sm-2">
                                        Due Clear Amount
                                        <asp:TextBox ID="txtDueClearAmount" runat="server" CssClass="form-control input-sm"></asp:TextBox>
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
                                      Receipt No<span class="asterik" style="font-weight:100">*(latest receipt no, please see in payment history)</span><br />
                                      <%--<asp:TextBox ID="txtReceiptNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>--%>
                                      <div class="col-md-4" style="padding-left: 0px; padding-right: 0px;">
                                          <asp:TextBox ID="txtStatic" CssClass="form-control" ClientIDMode="Static" runat="server" disabled ></asp:TextBox>
                                      </div>
                                      <div class="col-md-8" style="padding-left: 0px; padding-right: 0px;">
                                          <asp:TextBox ID="txtReceiptNo" CssClass=" form-control" ClientIDMode="Static" runat="server" disabled ></asp:TextBox>
                                      </div>
                                  </div>
                                    <div class="col-sm-2">
                                        <%--<asp:CheckBox ID="chkIsRenewExtended" CssClass="checkbox-inline" Checked="true" Text="Is Renewed" runat="server" />--%>
                                        <span class="text-label">Reason for Update</span><span class="asterik">*</span>
                                        <asp:DropDownList ID="ddlRenewExtendNormal" runat="server" CssClass="form-control input-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlRenewExtendNormal_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select--" Selected="True"></asp:ListItem>
                                           <asp:ListItem Value="1" Text="Normal Changes"></asp:ListItem>
                                           <asp:ListItem Value="2" Text="Renew"></asp:ListItem>
                                       </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-2">
                                      Remark:
                                      <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                  </div>
                                    <div class="col-sm-2">
                                      Action Taker<span class="asterik">*</span>
                                      <asp:DropDownList ID="ddlActionTaker" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                  </div>
                                  <div class="col-sm-12">
                                       <asp:Button ID="btnPriceCalculate" runat="server" Text="Calculate" CssClass="btn btn-danger" OnClick="btnPriceCalculate_Click" />
                                      <asp:Button ID="btnEdit" runat="server" Text="Edit" Enabled="false" CssClass="btn btn-success" Width="85px" OnClick="btnSubmit_Click" />
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
                <div class="modal fade" id="conformStopStart" role="dialog">
                    <div class="modal-dialog">
                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Stop/Start(15 days minimum)</h4>
                            </div>
                            <div class="modal-body">
                                <h6>Are you sure?</h6><br />
                                    <i style="color:red">Note:"Stop/Start Date will be by default of today's Date"</i><br />
                                <i style="color:green">If you want to override the default Stop Date then set into below date box</i>
                                <asp:TextBox ID="txtChangeInStartStopDate" Width="237px" runat="server" CssClass="form-control input-sm nepCalendar"></asp:TextBox>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnStartStopModal" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnStartStopModal_Click" />
                                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>                
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
    <script type="text/javascript">
        $(document).ready(function () {
            activeInactiveBGChange();
            //gymAdmin();
            $('#ContentPlaceHolder1_ddlActiveInactive').change(function () {
                activeInactiveBGChange();
            });
        });
        function pageLoad(sender, args) {
            $("#txtMembershipBeginDate").nepaliDatePicker({
                ndpYear: true,
                ndpMonth: true,
                ndpYearCount: 10,
                onChange: renewChange,
                dateFormat: "YYYY/MM/DD"
            });
            $(".nepCalendar").nepaliDatePicker({
                ndpYear: true,
                ndpMonth: true,
                ndpYearCount: 10,
                dateFormat: "YYYY/MM/DD"
            });
        }
        function renewChange() {
            let addMonth = $('#ContentPlaceHolder1_ddlMembershipPaymentType').val();
            let renewDateNep = $("#txtMembershipBeginDate").val();
            let renewDateObjNep = NepaliFunctions.ConvertToDateObject(renewDateNep, "YYYY/MM/DD");
            let renewDateObjEng = NepaliFunctions.BS2AD(renewDateObjNep);
            let renewDateEng = NepaliFunctions.ConvertDateFormat(renewDateObjEng, "YYYY/MM/DD");

            let dt = new Date(renewDateEng);
            dt.setMonth(dt.getMonth() + parseInt(addMonth));
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
        function myFunction() {
            var x = document.getElementById("ContentPlaceHolder1_txtPwd");
            if (x.type === "password") {
                x.type = "text";
            } else {
                x.type = "password";
            }
        }
    </script>
    <script>
        new Vue({
            el: '#newForm',
            data: {
                qrId: '<%=mId %>',
            }
        })
    </script>
</asp:Content>
