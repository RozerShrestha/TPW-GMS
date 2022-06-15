<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Locker.aspx.cs" Inherits="TPW_GMS.Locker" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css">
	<asp:HiddenField ID="hidHeader" runat="server" Value="Locker Management" />
    <div id="app">
        <section class="content">
            <div id="locIns" disabled class="row" v-bind:class="{'disabledbutton':login=='admin' || login=='superadmin'}">
                    <div class="box box-info">
                        <div class="box-title">
                        </div>
                        <div class="box-body">
                            <div class="col-xs-12">
                                <div class="col-sm-2">
                                    Locker Number
                                    <asp:DropDownList ID="ddlLockerNumber" v-model="lockerNumSelected" runat="server" CssClass="form-control input-sm">
                                    </asp:DropDownList>
                                </div>
                                 <div class="col-sm-2">
                                    Status
                                     <select name="category_id" v-model="statusSelected" class="form-control input-sm">
                                         <option value="1">Active</option>
                                         <option value="0">Deactive</option>  
                                     </select>
                                </div>
                                <div class="col-sm-2" style="margin-top:18px">
                                    <button type="button" v-on:click="insertIntoLocker()" class="btn btn-success">Submit</button>
                                    <asp:Label ID="lblInfo" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            <%--Locker Box--%>
            <div class="row">
                <div class="box box-default" v-for="(item, index) in branchWiseList" :key="index" >
                    <div class="box-title">
                        <div class="col-sm-12">
                            <h3>{{item.branchName}}</h3>
                        </div> 
                    </div>
                    <div class="box-body">
                            <div class="col-sm-3" style="padding-top:10px" v-for="(item, index) in item.lockers" :key="index" >
                                <div class="locker" v-bind:class="{expired:item.isExpired==true ,notExpired:item.isExpired==false, notAssigned:item.isAssigned==false, unavailable:item.flag==false}" >
                                    <h1 class="badge" v-bind:class="{expiredBg:item.isExpired==true ,notExpiredBg:item.isExpired==false, notAssignedBg:item.isAssigned==false, unavailableBg:item.flag==false}" style="font-size:33px; margin-top:-10px">{{item.lockerNumber}}</h1>
                                    <ul id="totalMemberShipCount" class="nav nav-stacked compress">
                                        <li><a href="#"><b>Member Id</b><span class="pull-right">{{item.memberId}}</span></a></li>
                                        <li><a href="#"><b>Locker Assigned</b><span class="pull-right">{{item.fullName}}</span></a></li>
                                        <li><a href="#"><b>Renew Date</b><span class="pull-right">{{item.renewDate}}</span></a></li>
                                        <li><a href="#"><b>Duration</b><span class="pull-right">{{item.duration}}</span></a></li>
                                        <li><a href="#"><b>Expire Date</b><span class="pull-right">{{item.expireDate}}</span></a></li>
                                   <%--     <li><a id="tm_Baneshwor" href="#">FullName<span class="pull-right">Rozer Shrestha</span></a></li>
                                        <li><a id="tm_Kamaladi" href="#">Renew Date<span class="pull-right">2020 Jan 1</span></a></li>
                                        <li><a id="tm_Kumaripati" href="#">Expire Date<span class="pull-right">2020 Feb 1</span></a></li>--%>
                                    </ul>
                                    <button class="material-button material-button-toggle" data-toggle="modal" v-on:click="getLockerInfo(item.lockerNumber)" v-bind:disabled=login=="admin"?true:false data-target="#myModal" type="button">
                                        <span  v-bind:class="{'fa fa-lock':item.isAssigned==true,'fa fa-unlock':item.isAssigned==false}"></span>
                                    </button>
                                </div>
                            </div>
                        </div>
                </div>
            </div>
            <%--Locker Box--%>
            <div class="modal fade" id="myModal" data-keyboard="false" data-backdrop="static">
                    <div class="modal-dialog">
                        <div class="modal-content" style="width:700px">
                            <!-- Modal Header -->
                            <div class="modal-header">
                                <h4 class="modal-title">Locker</h4>
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                            </div>
                            <!-- Modal body -->
                            <div class="modal-body">
                                <div class="row">
                                     <div class="col-sm-2">
                                        <label for="lockerNumber">Locker No</label>
                                        <input type="text" id="txtLockerNumber" v-model="lockerId" readonly class="form-control input-sm" />
                                    </div>
                                    <div class="col-sm-6">
                                       <label for="MemberName">Member Name</label>
                                        <select id="ddlMemberName" @change="getMemberId($event)" v-model="selectedMemberName" name="memberName" type="text" class="form-control input-sm">
                                            <option v-for="(mem,index) in memberList" :key="index" :value="mem">{{mem}}</option>
                                        </select>
                                    </div>
                                    <div class="col-sm-4">
                                        <label for="MemberId">Member Id</label>
                                        <select id="ddlMemberId" v-model="memberId" name="memberId" type="text" class="form-control input-sm">
                                            <option v-for="(memId,index) in memberIds" :key="index" :value="memId">{{memId}}</option>
                                        </select>
                                    </div>
                                    </div>
                                <div class="row">
                                     <div class="col-sm-4">
                                        <label for="renewDate">Renew Date</label>
                                        <input type="text" v-model="locRenewDate" name="locRenewDate" id="txtRenewDate1" class="form-control input-sm" />
                                    </div>
                                    <div class="col-sm-4">
                                        <label for="duration">Duration</label>
                                        <select id="ddlDuration" v-model="locDuration" @change="DurationChange(locDuration)" class="form-control input-sm">
                                            <option value="0">Select</option>
                                            <option value="1">1 Month</option>
                                            <option value="3">3 Month</option>
                                            <option value="6">6 Month</option>
                                            <option value="12">12 Month</option>
                                        </select>
                                    </div>
                                    <div class="col-sm-4">
                                        <label for="MemberId">Expire Date</label>
                                        <input type="text" id="txtExpireDate" v-model="locExpireDate" readonly class="form-control input-sm" />
                                    </div>
                                </div>   
                                <div class="row">
                                    <div class="col-sm-4">
                                        <label for="Charge">Charge</label>
                                        <input type="text" id="txtCharge" v-model="locCharge" readonly class="form-control input-sm" />
                                    </div>
                                    <div class="col-sm-4">
                                        <label for="paymentMethod">Payment Method</label>
                                        <select id="ddlpaymentMethod" v-model="locPaymentMethod" class="form-control input-sm">
                                            <option value="Select">Select</option>
                                            <option value="Cash">Cash</option>
                                            <option value="Card">Card</option>
                                            <option value="E Banking">E Banking</option>
                                        </select>
                                    </div>
                                    <div class="form-row col-sm-4">
                                          Receipt No<span class="asterik">*</span><br />
                                          <%--<asp:TextBox ID="txtReceiptNo" runat="server" CssClass="form-control input-sm"></asp:TextBox>--%>
                                          <div class="col-md-8" style="padding-left: 0px; padding-right: 0px;">
                                              <asp:TextBox ID="txtStatic" v-model="locReceiptNoStatic" CssClass="form-control" ClientIDMode="Static" runat="server" disabled ></asp:TextBox>
                                          </div>
                                          <div class="col-md-4" style="padding-left: 0px; padding-right: 0px;">
                                              <asp:TextBox ID="txtReceiptNo" v-model="locReceiptNo" CssClass=" form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                                          </div>
                                      </div>
                                </div>
                            </div>
                            <!-- Modal footer -->
                            <div class="modal-footer">
                                <button type="button" v-bind:disabled=!isExpired v-on:click="assignMemberToLocker" class="btn btn-success">Submit</button>
                                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                                <button type="button" v-on:click="reset" class="btn btn-info">Reset</button>
                            </div>
                        </div>
                    </div>
                </div>
        </section>
    </div>

    <script>
        new Vue({
            el: '#app',
            data: {
                login: '',
                lockerNumSelected: '',
                statusSelected: '',
                lockerId: '',
                locRenewDate: '',
                locDuration:'',
                locExpireDate: '',
                locCharge: '',
                locPaymentMethod:'',
                locReceiptNoStatic:'',
                locReceiptNo:'',
                selectedMemberName: '',
                memberId: '',
                isExpired:false,
                lockerData: [],
                memberList: [],
                memberIds: [],
                branchWiseList:[],
                date1:''
            },
            methods: {
                insertIntoLocker() {
                    const params = new URLSearchParams();
                    params.append('branch', this.login);
                    params.append('lockerNumber', this.lockerNumSelected);
                    params.append('flag', this.statusSelected == '0' ? false : true);
                    axios({
                        url: 'api/InsertIntoLocker',
                        method: 'post',
                        data: params,
                        dataType: "JSON",
                        headers: {
                            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                        },
                    }).then(response => {
                        if (response.status == 200) {
                            this.getBranchWise(this.login);
                        }
                        else {
                            console.log("Error");
                        }

                    }).catch(function (error) {
                        alert(error)
                    })
                },
                getMemberList(brnch, isNull) {
                    axios({
                        url: 'api/GetMemberList?branch=' + brnch +'&memberId='+isNull,
                        method: 'get',
                        dataType: "JSON",
                        headers: {
                            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                        },
                    }).then(response => {
                        if (response.status == 200) {
                            this.memberList = response.data;
                        }
                        else {
                            console.log("Error");
                        }

                    }).catch(function (error) {
                        alert(error)
                    })
                },
                getLockerData(login) {
                    const params = new URLSearchParams();
                    params.append('loginUser', login);
                    axios({
                        url: 'api/GetLockerData',
                        method: 'post',
                        data: params,
                        dataType: "JSON",
                        headers: {
                            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                        },
                    }).then(response => {
                        if (response.status == 200) {
                            this.lockerData = _.sortBy(response.data, "lockerNumber");
                            //var testLocData=response.data.filter(p=p.branch)
                        }
                        else {
                            console.log("Error");
                        }
                        
                    }).catch(function (error) {

                    })
                },
                getMemberId(event) {
                    axios({
                        url: 'api/GetMemberId?fullname=' + event.target.value+'&branch='+this.login,
                        method: 'get',
                        dataType: "JSON",
                        headers: {
                            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                        },
                    }).then(response => {
                        if (response.status == 200) {
                            this.memberIds = response.data;
                        }
                        else {
                            console.log("Error");
                        }

                    }).catch(function (error) {
                        alert(error)
                    })
                },
                getLockerCharge() {
                    axios({
                        url: 'api/GetLockerCharge?duration=' + this.locDuration,
                        method: 'get',
                        dataType: "JSON",
                        headers: {
                            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                        },
                    }).then(response => {
                        if (response.status == 200) {
                            this.locCharge = response.data;
                        }
                        else {
                            console.log("Error");
                        }

                    }).catch(function (error) {
                        alert(error)
                    })
                },
                getLockerInfo(locNum) {
                    var locData = this.branchWiseList[0].lockers.find(f => f.lockerNumber === locNum);
                    
                    this.lockerId = locData.lockerNumber;
                    try {
                        var dur = parseInt(locData.duration);
                        this.locReceiptNoStatic = locData.receiptNoStatic;
                        this.locReceiptNo = locData.receiptNo.split('-')[1];
                    } catch (e) {

                    }
                    this.locPaymentMethod = locData.paymentMethod;
                    this.locRenewDate = locData.renewDate;
                    this.locExpireDate = locData.expireDate;
                    this.locDuration = dur;
                    this.selectedMemberName = locData.fullName;          
                    this.locCharge = locData.charge;
                    this.memberIds.push(locData.memberId);
                    this.memberId = locData.memberId;
                    this.isExpired = locData.isExpired == null ? true : locData.isExpired == true ? true : false;

                    if (locData.memberId == null) {
                        this.getMemberList(this.login, locData.memberId);
                    }
                    else {
                        this.getMemberList(this.login, locData.memberId);
                    }
                },
                addDays(date, month) {
                    const copy = new Date(Number(date))
                    copy.setMonth(copy.getMonth() + month)
                    return copy
                },
                formatDate(date) {
                    var d = new Date(date),
                    month = '' + (d.getMonth() + 1),
                    day = '' + d.getDate(),
                    year = d.getFullYear();

                    if(month.length < 2)
                        month = '0' + month;
                    if (day.length < 2)
                        day = '0' + day;

                    return [year, month, day].join('/');
                },
                BSToAD(bs) {
                    let bsObj = NepaliFunctions.ConvertToDateObject(bs, "YYYY/MM/DD");
                    let adObj = NepaliFunctions.BS2AD(bsObj);
                    let ad = NepaliFunctions.ConvertDateFormat(adObj, "YYYY/MM/DD");
                    return ad;
                },
                ADToBS(ad) {
                    let adObj = NepaliFunctions.ConvertToDateObject(ad, "YYYY/MM/DD");
                    let bsObj = NepaliFunctions.AD2BS(adObj);
                    let bs = NepaliFunctions.ConvertDateFormat(bsObj, "YYYY/MM/DD");
                    return bs;
                },
                assignMemberToLocker() {
                    const params = new URLSearchParams();
                    params.append('branch', this.login);
                    params.append('lockerNumber', this.lockerId);
                    params.append('flag', true);
                    params.append('memberId', this.memberId);
                    params.append('paymentMethod', this.locPaymentMethod);
                    params.append('renewDate', this.BSToAD(this.locRenewDate));
                    params.append('duration', this.locDuration);
                    params.append('expireDate', this.BSToAD(this.locExpireDate));
                    params.append('amount', this.locCharge);
                    params.append('receiptNo', this.locReceiptNoStatic +"-"+ this.locReceiptNo);
                    axios({
                        url: 'api/AssignMemberIntoLocker',
                        method: 'post',
                        data: params,
                        dataType: "JSON",
                        headers: {
                            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                        },
                    }).then(response => {
                        if (response.status == 200) {
                            if (response.data == "Success") {
                                this.getBranchWise(this.login);
                                $('#myModal').modal('hide');
                            }
                        }
                        else {
                            console.log("Error");
                        }

                    }).catch(function (error) {
                        alert(error)
                    })
                },
                reset() {
                    let r = confirm("Are you sure wwant to Reset");
                    if (r) {
                        const params = new URLSearchParams();
                        params.append('branch', this.login);
                        params.append('lockerNumber', this.lockerId);
                        axios({
                            url: 'api/ResetLocker',
                            method: 'post',
                            data: params,
                            dataType: "JSON",
                            headers: {
                                'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                            },
                        }).then(response => {
                            if (response.status == 200) {
                                if (response.data == "Success") {
                                    this.getBranchWise(this.login);
                                    $('#myModal').modal('hide');
                                }
                            }
                            else {
                                console.log("Error");
                            }

                        }).catch(function (error) {
                            alert(error)
                        })
                    }
                    debugger;
                },
                resetFormData() {
                    this.memberIds = [];
                    this.locReceiptNo = '';
                },
                setLocRenewDate(dt) {
                    this.locRenewDate = dt
                },
                getBranchWise(branch) {
                    axios({
                        url: 'api/GetBranchWiseLocker',
                        method: 'get',
                        dataType: "JSON",
                        headers: {
                            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                        },
                    }).then(response => {
                        if (response.status == 200) {

                            this.branchWiseList = branch == 'admin' || branch=='superadmin' ? response.data : response.data.filter(p => p.branchName == branch);
                        }
                        else {
                            console.log("Error");
                        }

                    }).catch(function (error) {
                        alert(error)
                    })
                    
                },
                DurationChange(ev) {
                    let dt = new Date(this.BSToAD(this.locRenewDate));
                    dt.setMonth(dt.getMonth() + parseInt(ev));
                    let expiryDateEng = this.formatDate(dt);
                    this.locExpireDate = this.ADToBS(expiryDateEng);
                    this.getLockerCharge();
                },
            },
            mounted() {
                var vm = this
                this.login = $('#lblUserLogin').text().split('-')[0];
                this.getBranchWise(this.login);
                //this.getMemberList(this.login);

                var modalInput = document.getElementById("txtRenewDate1");
                modalInput.nepaliDatePicker({
                    ndpYear: true,
                    ndpMonth: true,
                    ndpYearCount: 30,
                    container: '#myModal',
                    dateFormat: "YYYY/MM/DD",
                    onChange: function (dateText) {
                        vm.locRenewDate = dateText.bs
                        vm.DurationChange(vm.locDuration);
                    },
                });
                
                //$(".nepCalendar1").nepaliDatePicker({
                //    ndpYear: true,
                //    ndpMonth: true,
                //    ndpYearCount: 30,
                //    dateFormat: "YYYY/MM/DD",
                //    onChange: function (dateText) {
                //        vm.locRenewDate = dateText.bs
                //        vm.DurationChange(vm.locDuration);
                //    },
                //});
                $('#myModal').on("hidden.bs.modal", this.resetFormData)
            }, 
            beforeMount() {
                
            }
        })
    </script>
		

</asp:Content>
