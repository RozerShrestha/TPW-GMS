<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PriceList.aspx.cs" Inherits="TPW_GMS.Views.PriceList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>   
        input {
            /*background-color: transparent;*/
            background-color: aliceblue;
            border: 0px solid;
            color: #383838;
            text-align: center;
        }
    </style>
    <asp:HiddenField ID="hidHeader" runat="server" Value="Price List" />
    <asp:Panel ID="pnlWorkoutChart" runat="server">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-info">
                        <div class="box-header">
                            <h4>MEMBERSHIP OPTIONS : GYM | CARDIO | ZUMBA</h4>
                        </div>
                        <div class="box-body">
                            <div class="table-responsive">
                                <table class="table table-bordered" style=" ">
                                    <tr class="headerHome well">
                                        <th colspan="5">
                                            <center><span class="subheaderColor">Regular Membership</span></center>
                                        </th>
                                        <th colspan="5">
                                            <center><span class="subheaderColor">Off Peak Hour membership</span></center>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td width="10%">Type</td>
                                        <td width="10%">1 Month</td>
                                        <td width="10%">3 Month</td>
                                        <td width="10%">6 Month</td>
                                        <td width="10%">12 Month</td>
                                        <td width="10%">Type</td>
                                        <td width="10%">1 Month</td>
                                        <td width="10%">3 Month</td>
                                        <td width="10%">6 Month</td>
                                        <td width="10%">12 Month</td>
                                    </tr>
                                    <tr>
                                        <td>Any 1</td>
                                        <td>
                                            <asp:TextBox ID="txtRegularAny1_1month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtRegularAny1_3month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtRegularAny1_6month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtRegularAny1_12month" runat="server" Width="100px" /></td>
                                        <td>Any 1</td>
                                        <td>
                                            <asp:TextBox ID="txtOffHourAny1_1month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtOffHourAny1_3month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtOffHourAny1_6month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtOffHourAny1_12month" runat="server" Width="100px" /></td>
                                    </tr>
                                    <tr>
                                        <td>Any 2</td>
                                        <td>
                                            <asp:TextBox ID="txtRegularAny2_1month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtRegularAny2_3month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtRegularAny2_6month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtRegularAny2_12month" runat="server" Width="100px" /></td>
                                        <td>Any 2</td>
                                        <td>
                                            <asp:TextBox ID="txtOffHourAny2_1month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtOffHourAny2_3month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtOffHourAny2_6month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtOffHourAny2_12month" runat="server" Width="100px" /></td>
                                    </tr>
                                    <tr>
                                        <td>Any 3</td>
                                        <td>
                                            <asp:TextBox ID="txtRegularAny3_1month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtRegularAny3_3month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtRegularAny3_6month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtRegularAny3_12month" runat="server" Width="100px" /></td>
                                        <td>Any 3</td>
                                        <td>
                                            <asp:TextBox ID="txtOffHourAny3_1month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtOffHourAny3_3month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtOffHourAny3_6month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtOffHourAny3_12month" runat="server" Width="100px" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="text-align: left;">
                                            <div class="membershipInfo">
                                                <div>Admission Fee: 500</div>
                                                <div>• 6 sessions of universal membership per month</div>
                                                <div>• If universal membership session needed after quota expires, pay extra 100 per session</div>
                                                <div>• 3% discount on all supplement purchases</></div>
                                            </div>
                                        </td>
                                        <td colspan="5" style="text-align: left;">
                                            <div class="membershipInfo">
                                                <div>Admission Fee: 500</div>
                                                <div>• 6 sessions of universal membership per month only during off peak hour</div>
                                                <div>• If universal membership session needed after quota expires, pay extra 100 per session</div>
                                                <div>• 3% discount on all supplement purchases</></div>
                                                <div>• To come in other time, users have to pay Rs 75 per session.</div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="headerHome">
                                        <th colspan="5">
                                            <center><span class="subheaderColor">The Universal Member</span></center>
                                        </th>
                                        <th colspan="5">
                                            <center><span class="subheaderColor">Personal Training Charges</span></center>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td>Type</td>
                                        <td>1 Month</td>
                                        <td>3 Month</td>
                                        <td>6 Month</td>
                                        <td>12 Month</td>
                                        <td>Type</td>
                                        <td>1 Month</td>
                                        <td>3 Month</td>
                                        <td>6 Month</td>
                                        <td>12 Month</td>

                                    </tr>
                                    <tr>
                                        <td>Any 1</td>
                                        <td>
                                            <asp:TextBox ID="txtUniversalAny1_1month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtUniversalAny1_3month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtUniversalAny1_6month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtUniversalAny1_12month" runat="server" Width="100px" /></td>
                                        <td>1 Person</td>
                                        <td>
                                            <asp:TextBox ID="txtPersonal1_1month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtPersonal1_3month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtPersonal1_6month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtPersonal1_12month" runat="server" Width="100px" /></td>
                                    </tr>
                                    <tr>
                                        <td>Any 2</td>
                                        <td>
                                            <asp:TextBox ID="txtUniversalAny2_1month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtUniversalAny2_3month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtUniversalAny2_6month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtUniversalAny2_12month" runat="server" Width="100px" /></td>
                                        <td>2 or More</td>
                                        <td>
                                            <asp:TextBox ID="txtPersonal2_1month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtPersonal2_3month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtPersonal2_6month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtPersonal2_12month" runat="server" Width="100px" /></td>
                                    </tr>
                                    <tr>
                                        <td>Any 3</td>
                                        <td>
                                            <asp:TextBox ID="txtUniversalAny3_1month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtUniversalAny3_3month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtUniversalAny3_6month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtUniversalAny3_12month" runat="server" Width="100px" /></td>
                                        <td>-</td>
                                        <td>
                                            <asp:TextBox ID="txtPersonal3_1month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtPersonal3_3month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtPersonal3_6month" runat="server" Width="100px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtPersonal3_12month" runat="server" Width="100px" /></td>

                                    </tr>

                                    <tr>
                                        <td colspan="5" style="text-align: left;">
                                            <div class="membershipInfo">
                                                <div>Admission Fee: 500</div>
                                                <div>• 6 sessions of universal membership per month</div>
                                                <div>• If universal membership session needed after quota expires, pay extra 100 per session</div>
                                                <div>• 3% discount on all supplement purchases</></div>
                                            </div>
                                        </td>
                                        <td colspan="5" style="text-align: left;">
                                            <div class="membershipInfo">
                                                <div>Personal trainers get paid according to their ratings given</div>
                                                <div>
                                                    <span class="spanWeight">Class A :</span><asp:TextBox ID="txtTrainerClass1" runat="server" Width="20px" />
                                                    % of PT charges
                                                </div>
                                                <div>
                                                    <span class="spanWeight">Class B :</span><asp:TextBox ID="txtTrainerClass2" runat="server" Width="20px" />
                                                    % of PT charges
                                                </div>
                                                <div>
                                                    <span class="spanWeight">Class C :</span><asp:TextBox ID="txtTrainerClass3" runat="server" Width="20px" />
                                                    % of PT charges
                                                </div>
                                                <div><span class="spanWeight">Per Day Pass:</span>
                                                    Any 1:<asp:TextBox ID="txtPerDayAny1" runat="server" Width="50px" />    
                                                    Any 2:<asp:TextBox ID="txtPerDayAny2" runat="server" Width="50px" />
                                                    Any 3:<asp:TextBox ID="txtPerDayAny3" runat="server" Width="50px" /></div>
                                                <div><span class="spanWeight">Per 10 Day Pass:</span>
                                                    Any 1: <asp:TextBox ID="txtTenDaysAny1" runat="server" Width="50px" /> 
                                                    Any 2: <asp:TextBox ID="txtTenDaysAny2" runat="server" Width="50px" />
                                                    Any 3: <asp:TextBox ID="txtTenDaysAny3" runat="server" Width="50px" />

                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="headerHome">
                                        <th colspan="5">
                                            <center><span class="subheaderColor">Consultation charges(After first free consultation)</span></center>
                                        </th>
                                        <th colspan="5">
                                            <center><span class="subheaderColor">Lockers Fee</span></center>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="text-align: left;">
                                            <div class="membershipInfo">
                                                <div>
                                                    <span class="spanWeight">• Rs. </span>
                                                    <asp:TextBox ID="txtConsultationCharge" Text="300" runat="server" Width="20px" />
                                                    per consultation (usually reminded every 2 months)
                                                </div>
                                                <div>
                                                    <span class="spanWeight">• Trainers get </span>
                                                    <asp:TextBox ID="txtConsultationFeeToTrainer" Text="250" runat="server" Width="20px" />
                                                    per counseling
                                                </div>
                                            </div>
                                        </td>
                                        <td colspan="5">
                                            <table class="table">
                                                <tr>
                                                    <td>1 Month</td>
                                                    <td>3 Month</td>
                                                    <td>6 Month</td>
                                                    <td>12 Month</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtLocker1Month" runat="server" Width="100px" /></td>
                                                    <td>
                                                        <asp:TextBox ID="txtLocker3Month" runat="server" Width="100px" /></td>
                                                    <td>
                                                        <asp:TextBox ID="txtLocker6Month" runat="server" Width="100px" /></td>
                                                    <td>
                                                        <asp:TextBox ID="txtLocker12Month" runat="server" Width="100px" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="10">
                                            <asp:Button ID="btnSubmit" runat="server" Style="margin-right: 10px; float: right;" Text="Submit" CssClass="btn btn-sm btn-success" OnClick="btnSubmit_Click" />
                                        </td>
                                    </tr>

                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    </asp:Panel>
</asp:Content>
