<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="TPW_GMS.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Assets/js/dashboard.js"></script>
    <style>
    .bargraph {
        height: 250px;
        width: 400px;
        min-height:10px;
        margin-bottom:10px;
        background-color: #f3f3f3;
    }
    .iconplacement{
        margin-top: 0px;
        font-size:0px
    }
    .dashHeader{
        margin-left: 9px;
        font-size: 16px;
        margin-top: 0px;
        font-family: -webkit-pictograph;
        font-weight: 600;
    }
    .subheading_style{
        font-weight: 400;
        font-size: 12px;
        padding-right:5px;
    }
    .num_style{
        font-weight: 500;
        font-size: 12px;
        margin-left:10px;
        margin-right:10px;
        color:red;
    }
    .info-box-icon{
        height:118px;
        width:104px;
    }
    .info-box-content{
        margin-left:104px;
    }
    .info-box-text{
        font-weight:bold;
        font-size:12px;
        /*margin-left:-30px;*/
    }
    .widget-user-2 .widget-user-header {
        padding: 7px;
        border-top-right-radius: 3px;
        border-top-left-radius: 3px;
    }
    .widget-user-2 .widget-user-username {
        margin-top: 5px;
        margin-bottom: 5px;
        font-size: 18px;
        font-weight: 300;
        margin-left:40px;
    }
    .nav>li>a {
        position: relative;
        display: block;
        padding: 5px 15px;
    }
    .headerTotal{
        margin-right:19px;
    }
    .card-padding{
        padding-right: 5px;
        padding-left: 5px;
    }
    /*.branch{
        visibility:hidden;
    }*/
	@media screen and (max-width: 767px) {
	    #totalMemberShipCount.nav > li a{
            display: flex!important;
			width:100%!important;
            justify-content: space-between!important;
			}
		.dashHeader{
				display: flex;
		justify-content: space-between;
		}
		.box-widget{
			overflow:hidden;
		}
		.contentq{
			overflow:hidden;
		}
	}
    
</style>
    <section class="contentq">
        <div class="box box-info">
            <div class="box-title">
                <asp:HiddenField ID="hidHeader" runat="server" Value="Dashboard" />
            </div>
            <div class="box-body">
                <div class="row ">
                    <div class="col-lg-2">
                        Start Date:
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control input-sm dateControl"></asp:TextBox>
                    </div>
                    <div class="col-lg-2">
                        End Date:
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control input-sm dateControl"></asp:TextBox>
                    </div>
                    <div class="col-lg-2">
                        Branch:
                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control input-sm">
                        </asp:DropDownList>
                    </div>
                    <div class="col-lg-2">
                        <asp:Button ID="btnSubmit" style="margin-top:20px" runat="server" CssClass="btn btn-sm btn-success showAdmins" Text="Save" OnClick="btnSubmit_Click" />
                        <input type="button" id="btnLoadData" style="margin-top:20px" class="btn btn-sm btn-primary" name="Load Data" value="Load Data" />
                    </div>
                    <div class="col-lg-2">
                        
                    </div>
                    <div class="col-lg-6">
                        <span class="subheading_style">Note:</span><span class="num_style">For accurate result, start date to be always january 1st and range to be always 1 year</span>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-4 card-padding">
                        <div class="box box-widget widget-user-2">
                            <div class="widget-user-header bg-green">
                               <%-- <div class="widget-user-image">
                                    <i class="fa fa-users iconplacement"></i>
                                </div>--%>
                                <div class="dashHeader">
                                    Total Membership<span id="totalMembership" class="pull-right headerTotal"></span>
                                </div>
                            </div>
                            <div class="box-footer no-padding">
                                <ul id="totalMemberShipCount" class="nav nav-stacked">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 card-padding">
                        <div class="box box-widget widget-user-2">
                            <div class="widget-user-header bg-yellow">
                                <%--<div class="widget-user-image">
                                    <i class="fa fa-users iconplacement"></i>
                                </div>--%>
                                <div class="dashHeader">
                                    Active<span id="activeMemberhip" class="pull-right headerTotal"></span>
                                </div>
                            </div>
                            <div class="box-footer no-padding">
                                <ul id="activeMembers" class="nav nav-stacked">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="box box-widget widget-user-2">
                            <div class="widget-user-header bg-red">
                                <%--<div class="widget-user-image">
                                    <i class="fa fa-users iconplacement"></i>
                                </div>--%>
                                <div class="dashHeader" data-toggle="tooltip" data-delay="{ show: 1000, hide: 10000}" data-placement="top" title="Note: Average Data is calculated 11 PM onwards and will reflect after day end">
                                    Average !<span id="activeAvgMemberhip" class="pull-right headerTotal"></span>
                                </div>
                            </div>
                            <div class="box-footer no-padding">
                                <ul id="activeAvgMembers" class="nav nav-stacked">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 card-padding">
                        <div class="box box-widget widget-user-2">
                            <div class="widget-user-header bg-blue">
                                <%--<div class="widget-user-image">
                                    <i class="fa fa-users iconplacement"></i>
                                </div>--%>
                                <div class="dashHeader">
                                    InActive<span id="inactiveMembership" class="pull-right headerTotal"></span>
                                </div>
                            </div>
                            <div class="box-footer no-padding">
                                <ul id="inactiveMembers" class="nav nav-stacked">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 card-padding">
                        <div class="box box-widget widget-user-2">
                            <div class="widget-user-header bg-aqua">
                                <%--<div class="widget-user-image">
                                    <i class="fa fa-users iconplacement"></i>
                                </div>--%>
                                <div class="dashHeader">
                                    Gym Traffic<span id="gymTraffics" class="pull-right headerTotal"></span>
                                </div>
                            </div>
                            <div class="box-footer no-padding">
                                <ul id="gymTraffic" class="nav nav-stacked">
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-3">
                        <div class="box box-info">
                            <div class="box-header with-border">
                                <h3 class="box-title">Renew vs New Admitted</h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body">
                                <div id="pieAdmitRenew" style="height: 300px; width: 100%;"></div>
                            </div>
                        </div>
                    </div>
                     <div class="col-lg-9">
                        <div class="box box-info">
                            <div class="box-header with-border">
                                <h3 class="box-title">Renew vs New Admitted Detail</h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body">
                                <div id="barAdmitRenew" style="height: 300px; width: 100%;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-3">
                        <div class="box box-info">
                            <div class="box-header with-border">
                                <h3 class="box-title">Membership Option</h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                            <!-- /.box-header -->
                            <div class="box-body">
                                <div id="pieMembershipOption" style="height: 300px; width: 100%;"></div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-9">
                        <div class="box box-info showAdmins">
                            <div class="box-header with-border">
                                <h3 class="box-title">Monthly Earning From GYM</h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                            <!-- /.box-header -->
                            <div class="box-body">
                                <div id="chartEarningGym" style="height: 300px; width: 100%;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <div class="box box-info">
                            <div class="box-header with-border">
                                <h3 class="box-title">Average Active Members</h3>
                            </div>
                             <div class="box-body">
                                <div id="chartAverageActiveMembers" style="height: 370px; width: 100%;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                 <div class="row">
                    <div class="col-lg-12 showAdmins">
                        <div class="box box-info">
                            <div class="box-header with-border">
                                <h3 class="box-title">Monthly Average Active Member(Branch Wise)</h3>
                            </div>
                             <div class="box-body">
                                <div id="chartAverageActiveBranchWiseMonthly" style="height: 570px; width: 100%;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                
        </div>
        </div>
        <%--for information sharing purpose which popup at page load--%>
        <div class="modal fade" id="myModal">
            <div class="modal-dialog">
                <div class="modal-content" style="width:800px">

                    <!-- Modal Header -->
                    <div class="modal-header">
                        <h4 class="modal-title">INFORMATION</h4>
                    </div>

                    <!-- Modal body -->
                    <div class="modal-body">
                         <%--<h3>Dear All,</h3>
                            <p>In the Membership Form, Date field is set with Nepali Calender.</p>
                            <p>Please don't set English Date</p>--%>
                           <%-- <p>Scan the QR to login into an App</p>
                            <img src="Assets/Images/qr.png" />--%>
                            <p>Thank you.</p>
                        <p>Video Tutorial For Membership Renew, Extend and Normal Changes</p>
                        <video width="720" height="500" controls>
                            <source src="Assets/Newfeature.mp4" type="video/mp4">
                            Your browser does not support the video tag.
                        </video>
                    </div>

                    <!-- Modal footer -->
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                    </div>

                </div>
            </div>
        </div>

        <%--for active member list on click--%>
        <div class="modal fade" id="activeMemberListModal" style="display: none;" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Active Member List</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="box box-info">
                                    <div class="box-body">
                                        <div class="table-responsive">
                                            <table id="activeMemberList" style="font-size: 12px; width: 100%" class="table table-striped table-bordered table-sm">
                                                <thead>
                                                    <tr class="border-bottom-0 tr-header header">
                                                        <th style="min-width: 100px">MemberId</th>
                                                        <th style="min-width: 100px">Name</th>
                                                        <th>Branch</th>
                                                        <th>Duration</th>
                                                        <th>Renew</th>
                                                        <th>Expire Date</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--for active history monthly list on click--%>
        <div class="modal fade" id="activeHistoryMonthModal" style="display: none;" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Average Active Member Per Day</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="box box-info">
                                    <div class="box-body">
                                        <div class="table-responsive">
                                            <table id="activeHistoryMonth" style="font-size: 12px; width: 100%" class="table table-striped table-bordered table-sm">
                                                <thead>
                                                    <tr class="border-bottom-0 tr-header header">
                                                        <th style="min-width: 100px">Id</th>
                                                        <th style="min-width: 100px">Date</th>
                                                        <th>Branch</th>
                                                        <th>Count</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


    </section>

</asp:Content>
