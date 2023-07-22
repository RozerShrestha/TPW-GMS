
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberDashboard.aspx.cs" Inherits="TPW_GMS.MemberDashboard" %>
<link href="Assets/css/bootstrap.min.css" rel="stylesheet" />
<link href="Assets/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
<link href="Assets/css/jquery-ui.css" rel="stylesheet" />
<link href="dist/css/AdminLTE.css" rel="stylesheet">
<style type="text/css">
    .widget-user-2 .widget-user-username, .widget-user-2 .widget-user-desc {
        margin-left: 0px !important;
    }
</style>
<div id="app">
    <div class="container-fluid">
        <div class="box box-widget widget-user-2" >
            <div class="box box-header">
                <div style="text-align:center; margin-top: 40px;">
                    <qrcode :value="encId" :options="{ width: 300 }"></qrcode>
                </div>

                <div style="text-align:center; font-size:larger; font-weight:700">
                    Current Time:<span style="color:green">{{todaydate}}</span> 
                </div>
            </div>

            <div :class="{'widget-user-header bg-red':condition.isExpired==true, 'widget-user-header bg-green':condition.isExpired==false}">
                    <h3 class="widget-user-username">{{result.fullname}}</h3>
                    <h5 class="widget-user-desc">Branch: {{result.branch}}</h5>
                    <h5 class="widget-user-desc">Shift: {{result.shift}}</h5>
            </div>
            <div class="box-body table-responsive no-padding">
                <table class="table table-hover">
                    <tr><td><span class="text-bold">Phone No:</span> </td><td>{{result.contactNo}}</td></tr>
                            <tr><td><span class="text-bold">Email:</span> </td><td>{{result.email}}</td></tr>
                            <tr><td><span class="text-bold">Date Of Birth:</span> </td><td>{{result.dateOfBirth}}</td></tr>
                            <tr><td><span class="text-bold">Address:</span> </td><td>{{result.address}}</td></tr>
                            <tr><td><span class="text-bold">Gender:</span> </td><td>{{result.gender}}</td></tr>
                            <tr><td><span class="text-bold">Membership Type:</span> </td><td>{{result.memberOption}}</td></tr>
                            <tr><td><span class="text-bold">Membership Catagory:</span> </td><td>{{result.memberCatagory}}-{{result.memberSubCatagory}}</td></tr>
                            <tr><td><span class="text-bold">Membership Date:</span> </td><td>{{result.memberDate}}</td></tr>
                            <tr><td><span class="text-bold">Membership Renew Date:</span> </td><td>{{result.memberBeginDate}}</td></tr>
                            <tr><td><span class="text-bold">Membership Expire Date:</span> </td><td>{{result.memberExpireDate}}</td></tr> 
                </table>
            </div>
        </div>
        <br />
        <div v-show="isMember" id="divPaymentAttendanceHistory" class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">Payment History</h3>

                <div class="box-tools pull-right">
                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                </div>
            </div>
            <div class="box-body">
                <div class="table-responsive">
                <table class="table no-margin">
                    <thead>
                    <tr>
                        <th>Member Id</th>
                        <th>Receipt No</th>
                        <th>Renew Date</th>
                        <th>Expire Date</th>
                        <th>MemberShip Type</th>
                        <th>Category</th>
                        <th>Payment TYpe</th>
                        <th>Fee</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="(item, index) in paymentHsitory" :key="index">
                        <td>{{item.memberId}}</td>
                        <td>{{item.receiptNo}}</td>
                        <td>{{item.memberBeginDate}}</td>
                        <td>{{item.memberExpireDate}}</td>
                        <td>{{item.memberOption}}</td>
                        <td>{{item.memberCatagory}}</td>
                        <td>{{item.memberPaymentType}}</td>
                        <td>{{item.finalAmount}}</td>
                    </tr>
                    </tbody>
                </table>
                </div>
            </div>
    </div>
        <br />
        <div v-show="isMember" id="divMemberAttendanceHistory" class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">Attendance History for 30 days</h3>
                <div class="box-tools pull-right">
                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                </div>
            </div>
            <div class="box-body">
                <div class="table-responsive">
                <table class="table no-margin">
                    <thead>
                    <tr>
                        <th>Member Id</th>
                        <th>Full Name</th>
                        <th>Check In</th>
                        <th>Check Out</th>
                        <th>Branch</th>
                        <th>Checkin Branch</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="(item, index) in memberAttendanceHistory" :key="index">
                        <td>{{item.memberId}}</td>
                        <td>{{item.fullName}}</td>
                        <td>{{item.checkin}}</td>
                        <td>{{item.checkout}}</td>
                        <td>{{item.branch}}</td>
                        <td>{{item.checkinBranch}}</td>
                    </tr>
                    </tbody>
                </table>
                </div>
            </div>
    </div>
        <br />
        <div v-show="isStaff" id="divStaffAttendanceHistory" class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">Staff Attendance History for 30 days</h3>
                <div class="box-tools pull-right">
                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                </div>
            </div>
            <div class="box-body">
                <div class="table-responsive">
                <table class="table no-margin">
                    <thead>
                    <tr>
                        <th>Member Id</th>
                        <th>Full Name</th>
                        <th>Check In</th>
                        <th>Check Out</th>
                        <th>Branch</th>
                        <th>Checkin Branch</th>
                        <th>Remark</th>
                        <th>Late Flag</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="(item, index) in staffAttendanceHistory" :key="index">
                        <td>{{item.memberId}}</td>
                        <td>{{item.fullName}}</td>
                        <td>{{item.checkin}}</td>
                        <td>{{item.checkout}}</td>
                        <td>{{item.branch}}</td>
                        <td>{{item.checkinBranch}}</td>
                        <td>{{item.remark}}</td>
                        <td>{{item.lateFlag}}</td>

                    </tr>
                    </tbody>
                </table>
                </div>
            </div>
        </div>
        <br />
        <div v-show="isGuest" id="divGuestAttendanceHistory" class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">Guest Attendance History for 30 days</h3>
                <div class="box-tools pull-right">
                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                </div>
            </div>
            <div class="box-body">
                <div class="table-responsive">
                <table class="table no-margin">
                    <thead>
                    <tr>
                        <th>Id</th>
                        <th>Full Name</th>
                        <th>Email</th>
                        <th>Check In</th>
                        <th>Check Out</th>
                        <th>Checkin Branch</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="(item, index) in guestAttendanceHistory" :key="index">
                        <th>{{index+1}}</th>
                        <td>{{item.fullName}}</td>
                        <td>{{item.email}}</td>
                        <td>{{item.checkin}}</td>
                        <td>{{item.checkout}}</td>
                        <td>{{item.checkinBranch}}</td>
                    </tr>
                    </tbody>
                </table>
                </div>
            </div>
        </div>
    </div>
</div>

<script src="Assets/jquery/jquery.min.js"></script>
<script src="Assets/js/bootstrap.min.js"></script>
<script src="Assets/js/jquery-ui.min.js"></script>
<script src="Assets/Vue/vue.js"></script>
<script src="Assets/axios/dist/axios.js"></script>
<script src="Assets/js/vue-qrcode.js"></script>
<script>Vue.component(VueQrcode.name, VueQrcode);</script>
<script>
    new Vue({
        el: '#app',
        data: {
            encId: '<%=encryptedMemberId %>',
            isStaff: false,
            isMember: false,
            isGuest:false,
            result: '',
            todaydate:'',
            paymentHsitory: [],
            memberAttendanceHistory: [],
            staffAttendanceHistory: [],
            guestAttendanceHistory:[],
            condition: {
                isExpired:false
            },
            totalLate: '',
            totalDayDeduction:''
                
        },
        methods: {
            getMemberInfo(memId) {
                const params = new URLSearchParams();
                params.append('memberId', memId);
                axios({
                    url: 'api/getMemberLoginInfo',
                    method: 'post',
                    data:params,
                    dataType: "JSON",
                    headers: {
                        'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token_staff')
                    },
                }).then(response => {
                    if (response.data.memberOption === "Gym Admin" || response.data.memberOption === "Trainer" || response.data.memberOption === "Operation Manager" || response.data.memberOption === "Intern") {
                        this.isStaff = true;
                    }
                    else if (response.data.memberId == null) {
                        this.isGuest = true;
                    }
                    else {
                        this.isMember = true;
                    }
                    this.result = response.data;
                    this.paymentHsitory = response.data.memberPaymentHistorys;
                    this.memberAttendanceHistory = response.data.memberAttendances;
                    this.staffAttendanceHistory = response.data.staffAttendance;
                    this.guestAttendanceHistory = response.data.guestAttendance;
                    this.totalLate = response.data.staffAttendance.filter(item => item.lateFlag == true).length;
                    this.totalDayDeduction = Math.floor(this.totalLate / 3);
                    this.checkExpire();
                }).catch(function (error) {

                })
            },
            checkExpire() {
                var expireDate = new Date(this.result.memberExpireDate);
                var today = new Date();
                this.condition.isExpired = Math.sign(today - expireDate) === 1 ? true : false;
            }
        },
        beforeMount() {
            this.todaydate = new Date().toLocaleString();
            this.getMemberInfo(this.encId);
        }
    })
</script>
<%--</asp:Content>--%>
