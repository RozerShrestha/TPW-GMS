<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DailyReport.aspx.cs" Inherits="TPW_GMS.DailyReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .ovrRvnew {
            font-size: 20px;
            font-weight: bold;
            color: green;
        }
    </style>
    <asp:HiddenField ID="hidHeader" runat="server" Value="Daily Report" />
    <div id="app">
        <section class="content">
            <div class="row">
                <div class="box box-info">
                        <div class="box-body">
                            <div class="col-xs-12">
                                <div class="col-sm-2">
                                    Start Date
                                    <input id="txtStartDate1" type="text" v-model="startDate" class="form-control input-sm" />
                                </div>
                                 <div class="col-sm-2">
                                    End Date
                                     <input id="txtEndDate1" type="text" v-model="endDate"  class="form-control input-sm" />
                                </div>
                                <div class="col-sm-2" style="margin-top:18px">
                                    <button type="button" v-on:click="callAllMethod()" class="btn btn-success">Submit</button>
                                    <asp:Label ID="lblInfo" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
            </div>
            <span class="ovrRvnew">OVerAll Revenew:</span><span class="ovrRvnew" style="color:red !important">{{overAllRevenew}}</span>
        <div class="row">
            <div class="col-lg-4">
                    <div class="box box-default">
                        <div class="box-header with-border">
                            <h3 class="box-title">Overall Membership Revenue(New Admitted + Renewed)</h3>
                        </div>
                        <div class="box-body">
                            <div class="table-responsive">
                                <table class="table table-bordered table-hover" style="font-weight: 600">
                                <thead class="bg-danger">
                                    <th>Payment Method</th>
                                    <th>Count</th>
                                    <th>Revenue</th>
                                </thead>
                                <tbody>
                                    <tr class="" v-for="(i,index) in overAllMembership" :key="index" v-bind:class="{'bg-gray-active':(i.PaymentMethod=='SUM')}" >
                                        <td>{{i.PaymentMethod}}</td><td>{{i.Count}}</td><td>{{Math.abs(i.Revenue).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}}</td>
                                    </tr>
                                </tbody>
                            </table>
                            </div>
                        </div>
                    </div>
                </div>
            <div class="col-lg-4">
                    <div class="box box-default">
                        <div class="box-header with-border">
                            <h3 class="box-title">Overall New Admitted Revenue</h3>
                        </div>
                        <div class="box-body">
                            <div class="table-responsive">
                                <table class="table table-bordered table-hover" style="font-weight: 600">
                                <thead class="bg-danger">
                                    <th>Payment Method</th>
                                    <th>Count</th>
                                    <th>Revenue</th>
                                </thead>
                                <tbody>
                                    <tr class="" v-for="(i,index) in overAllNewAdmitted" :key="index" v-bind:class="{'bg-gray-active':(i.PaymentMethod=='SUM')}" >
                                        <td>{{i.PaymentMethod}}</td><td>{{i.Count}}</td><td>{{Math.abs(i.Revenue).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}}</td>
                                    </tr>
                                </tbody>
                            </table>
                            </div>
                        </div>
                    </div>
                </div>
            <div class="col-lg-4">
                    <div class="box box-default">
                        <div class="box-header with-border">
                            <h3 class="box-title">Overall Renewed Revenue</h3>
                        </div>
                        <div class="box-body">
                            <div class="table-responsive">
                                <table class="table table-bordered table-hover" style="font-weight: 600">
                                <thead class="bg-danger">
                                    <th>Payment Method</th>
                                    <th>Count</th>
                                    <th>Revenue</th>
                                </thead>
                                <tbody>
                                    <tr class="" v-for="(i,index) in overAllRenew" :key="index" v-bind:class="{'bg-gray-active':(i.PaymentMethod=='SUM')}" >
                                        <td>{{i.PaymentMethod}}</td><td>{{i.Count}}</td><td>{{Math.abs(i.Revenue).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}}</td>
                                    </tr>
                                </tbody>
                            </table>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
        <div class="row">
            <div class="col-lg-4">
                    <div class="box box-default">
                        <div class="box-header with-border">
                            <h3 class="box-title">Overall Merchandise Revenue</h3>
                        </div>
                        <div class="box-body">
                            <div class="table-responsive">
                                <table class="table table-bordered table-hover" style="font-weight: 600">
                                <thead class="bg-danger">
                                    <th>Payment Method</th>
                                    <th>Count</th>
                                    <th>Revenue</th>
                                </thead>
                                <tbody>
                                    <tr class="" v-for="(i,index) in overAllMerchandise" :key="index" v-bind:class="{'bg-gray-active':(i.PaymentMethod=='SUM')}" >
                                        <td>{{i.PaymentMethod}}</td><td>{{i.Count}}</td><td>{{Math.abs(i.Revenue).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}}</td>
                                    </tr>
                                </tbody>
                            </table>
                            </div>
                        </div>
                    </div>
                </div>
            <div class="col-lg-4">
                    <div class="box box-default">
                        <div class="box-header with-border">
                            <h3 class="box-title">Overall Supplements Revenue</h3>
                        </div>
                        <div class="box-body">
                            <div class="table-responsive">
                                <table class="table table-bordered table-hover" style="font-weight: 600">
                                <thead class="bg-danger">
                                    <th>Payment Method</th>
                                    <th>Count</th>
                                    <th>Revenue</th>
                                </thead>
                                <tbody>
                                    <tr class="" v-for="(i,index) in overAllSuplement" :key="index" v-bind:class="{'bg-gray-active':(i.PaymentMethod=='SUM')}" >
                                        <td>{{i.PaymentMethod}}</td><td>{{i.Count}}</td><td>{{Math.abs(i.Revenue).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}}</td>
                                    </tr>
                                </tbody>
                            </table>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
        <div class="row">
            <div class="col-lg-6">
                <div class="box box-default">
                    <div class="box-header with-border">
                        <h3 class="box-title">Branch Wise Membership Revenue</h3>
                    </div>
                    <div class="box-body">
                        <div class="col-md-6" v-for="(item,index) in dataMembership" :key="index">
                        <div class="box">
                            <div class="box-header with-border" v-bind:class="{'bg-aqua-active':index==0,'bg-green-active':index==1,'bg-red-active':index==2,'bg-yellow-active':index==3,'bg-gray-active':index==4}" style="height:65px !important">
                                <h4 class="box-title">{{item[0].Branches}}</h4>
                                <center><span class="label label-danger" style="font-size:initial">{{Math.abs(sumDataMembership[index]).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}}</span></center>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="table-responsive">
                                        <table class="table table-bordered table-hover" style="font-weight:600">
                                            <thead class="bg-danger">
                                                <th>Payment Method</th>
                                                <th>Count</th>
                                                <th>Revenue</th>
                                            </thead>
                                            <tbody>
                                                <tr class="" v-for="(i,index) in item" :key="index" v-bind:class="{'bg-gray-active':(i.PaymentMethod=='SUM')}" >
                                                    <td>{{i.PaymentMethod}}</td><td>{{i.Count}}</td><td>{{Math.abs(i.Revenue).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}}</td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="box box-default">
                    <div class="box-header with-border">
                        <h3 class="box-title">Branch Wise New Admitted Revenue</h3>
                    </div>
                    <div class="box-body">
                        <div class="col-md-6" v-for="(item,index) in dataNewAdmitted" :key="index">
                        <div class="box">
                            <div class="box-header with-border" v-bind:class="{'bg-aqua-active':index==0,'bg-green-active':index==1,'bg-red-active':index==2,'bg-yellow-active':index==3,'bg-gray-active':index==4}" style="height:65px !important">
                                <h4 class="box-title">{{item[0].Branches}}</h4>
                                <center><span class="label label-danger" style="font-size:initial">{{Math.abs(sumDataNewAdmitted[index]).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}}</span></center>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="table-responsive">
                                        <table class="table table-bordered table-hover" style="font-weight:600">
                                            <thead class="bg-danger">
                                                <th>Payment Method</th>
                                                <th>Count</th>
                                                <th>Revenue</th>
                                            </thead>
                                            <tbody>
                                                <tr class="" v-for="(i,index) in item" :key="index" v-bind:class="{'bg-gray-active':(i.PaymentMethod=='SUM')}" >
                                                    <td>{{i.PaymentMethod}}</td><td>{{i.Count}}</td><td>{{Math.abs(i.Revenue).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}}</td>
                                                </tr>
                                            </tbody>
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
        <div class="row">
            <div class="col-lg-6">
                <div class="box box-default">
                    <div class="box-header with-border">
                        <h3 class="box-title">Branch Wise Renewed Revenue</h3>
                    </div>
                    <div class="box-body">
                        <div class="col-md-6" v-for="(item,index) in dataRenew" :key="index">
                        <div class="box">
                            <div class="box-header with-border" v-bind:class="{'bg-aqua-active':index==0,'bg-green-active':index==1,'bg-red-active':index==2,'bg-yellow-active':index==3,'bg-gray-active':index==4}" style="height:65px !important">
                                <h4 class="box-title">{{item[0].Branches}}</h4>
                                <center><span class="label label-danger" style="font-size:initial">{{Math.abs(sumRenew[index]).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}}</span></center>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="table-responsive">
                                        <table class="table table-bordered table-hover" style="font-weight:600">
                                            <thead class="bg-danger">
                                                <th>Payment Method</th>
                                                <th>Count</th>
                                                <th>Revenue</th>
                                            </thead>
                                            <tbody>
                                                <tr class="" v-for="(i,index) in item" :key="index" v-bind:class="{'bg-gray-active':(i.PaymentMethod=='SUM')}" >
                                                    <td>{{i.PaymentMethod}}</td><td>{{i.Count}}</td><td>{{Math.abs(i.Revenue).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}}</td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="box box-default">
                    <div class="box-header with-border">
                        <h3 class="box-title">Branch Wise Merchandise Revenue</h3>
                    </div>
                    <div class="box-body">
                        <div class="col-md-6" v-for="(item,index) in dataMerchandise" :key="index">
                        <div class="box">
                            <div class="box-header with-border" v-bind:class="{'bg-aqua-active':index==0,'bg-green-active':index==1,'bg-red-active':index==2,'bg-yellow-active':index==3,'bg-gray-active':index==4}" style="height:65px !important">
                                <h4 class="box-title">{{item[0].Branches}}</h4>
                                <center><span class="label label-danger" style="font-size:initial">{{Math.abs(sumDataMerchandise[index]).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}}</span></center>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="table-responsive">
                                        <table class="table table-bordered table-hover" style="font-weight:600">
                                            <thead class="bg-danger">
                                                <th>Payment Method</th>
                                                <th>Count</th>
                                                <th>Revenue</th>
                                            </thead>
                                            <tbody>
                                                <tr class="" v-for="(i,index) in item" :key="index" v-bind:class="{'bg-gray-active':(i.PaymentMethod=='SUM')}" >
                                                    <td>{{i.PaymentMethod}}</td><td>{{i.Count}}</td><td>{{Math.abs(i.Revenue).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}}</td>
                                                </tr>
                                            </tbody>
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
        <div class="row">
            <div class="col-lg-6">
                    <div class="box box-default">
                        <div class="box-header with-border">
                            <h3 class="box-title">Branch Wise Supplement Revenue</h3>
                        </div>
                        <div class="box-body">
                            <div class="col-md-6" v-for="(item,index) in dataSuplement" :key="index">
                            <div class="box">
                                <div class="box-header with-border" v-bind:class="{'bg-aqua-active':index==0,'bg-green-active':index==1,'bg-red-active':index==2,'bg-yellow-active':index==3,'bg-gray-active':index==4}" style="height:65px !important">
                                    <h4 class="box-title">{{item[0].Branches}}</h4>
                                    <center><span class="label label-danger" style="font-size:initial">{{Math.abs(sumDataSuplement[index]).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}}</span></center>
                                </div>
                                <div class="box-body">
                                   <div class="row">
                                       <div class="table-responsive">
                                           <table class="table table-bordered table-hover" style="font-weight:600">
                                                <thead class="bg-danger">
                                                    <th>Payment Method</th>
                                                    <th>Count</th>
                                                    <th>Revenue</th>
                                                </thead>
                                                <tbody>
                                                    <tr class="" v-for="(i,index) in item" :key="index" v-bind:class="{'bg-gray-active':(i.PaymentMethod=='SUM')}" >
                                                        <td>{{i.PaymentMethod}}</td><td>{{i.Count}}</td><td>{{Math.abs(i.Revenue).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}}</td>
                                                    </tr>
                                                </tbody>
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
    </div>
    <script>
        new Vue({
            el: "#app",
            data: {
                startDate: NepaliFunctions.ConvertDateFormat(NepaliFunctions.GetCurrentBsDate(), "YYYY/MM/DD"),   /*'2019-01-01'new Date().toISOString().slice(0, 10),*/
                endDate: NepaliFunctions.ConvertDateFormat(NepaliFunctions.GetCurrentBsDate(), "YYYY/MM/DD"),/*'2019-12-30'new Date().toISOString().slice(0, 10),*/
                reportTypeBrn: ['bwMem', 'bwMer', 'bwSup', 'bwNewAdmitted','bwRenewed'],
                reportTypeOv: ['overAllMem', 'overAllMer', 'overAllSup', 'overAllNewAdmitted', 'overAllRenewed'],
                overAllRevenew:0,
                dataMembership: [],
                sumDataMembership: [],
                overAllMembership: [],

                dataNewAdmitted: [],
                sumDataNewAdmitted: [],
                overAllNewAdmitted: [],

                dataRenew: [],
                sumRenew: [],
                overAllRenew:[],

                dataMerchandise: [],
                sumDataMerchandise: [],
                overAllMerchandise: [],

                dataSuplement: [],
                sumDataSuplement: [],
                overAllSuplement: [],

                sumData2: [],
                testData2: [],
                branchList: [],
                overAllData1: [],
                overAllData2:[],
                revenueSum: 0,
            },
            methods: {
                callAllMethod() {
                    this.getBranchWiseRevenewReport();
                    this.getOverAllRevenewReport();
                    //this.getRevenewReportMercghandise();
                    //this.getOverAllRevenewReportMerchandise();
                },
                getBranchWiseRevenewReport() {
                    this.dataMembership = [];
                    this.sumDataMembership = [];

                    this.dataMerchandise = [];
                    this.sumDataMerchandise = [];

                    this.dataSuplement = [];
                    this.sumDataSuplement = [];

                    this.dataNewAdmitted = [];
                    this.sumDataNewAdmitted = [];

                    this.dataRenew = [];
                    this.sumRenew = [];

                    const params = new URLSearchParams();
                    params.append('startDate', this.startDate);
                    params.append('endDate', this.endDate);
                    params.append('reportType', 'bw');
                    axios({
                        url: 'api/GetDailyReport',
                        method: 'post',
                        data: params,
                        dataType: "JSON",
                        headers: {
                            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                        },
                    }).then(response => {
                        if (response.status == 200) {
                            //response.data.filter
                            this.reportTypeBrn.map((value1, key) => {
                                let tempData1 = response.data.filter(p => p.Category == value1);
                                this.branchList.map((value2, key) => {
                                    let tempData2 = tempData1.filter(p => p.Branches == value2);
                                    if (!tempData2.length == 0) {
                                        if (value1 == 'bwMem') {
                                            this.sumDataMembership.push(_.sumBy(tempData2, 'Revenue'));
                                            this.dataMembership.push(tempData2);
                                        }
                                        else if (value1 == 'bwMer') {
                                            this.sumDataMerchandise.push(_.sumBy(tempData2, 'Revenue'));
                                            this.dataMerchandise.push(tempData2);
                                        }
                                        else if (value1 == 'bwSup') {
                                            this.sumDataSuplement.push(_.sumBy(tempData2, 'Revenue'));
                                            this.dataSuplement.push(tempData2);
                                        }
                                        else if (value1 == 'bwNewAdmitted') {
                                            this.sumDataNewAdmitted.push(_.sumBy(tempData2, 'Revenue'));
                                            this.dataNewAdmitted.push(tempData2);
                                        }
                                        else if (value1 == 'bwRenewed') {
                                            this.sumRenew.push(_.sumBy(tempData2, 'Revenue'));
                                            this.dataRenew.push(tempData2);
                                        }
                                    }
                                })
                            })
                        }
                        else {
                            console.log("Error");
                        }

                    }).catch(function (error) {
                        alert(error)
                    })
                },
                getOverAllRevenewReport() {
                    const params = new URLSearchParams();
                    let sum = 0;
                    params.append('startDate', this.startDate);
                    params.append('endDate', this.endDate);
                    params.append('reportType', 'ov');
                    axios({
                        url: 'api/GetDailyReport',
                        method: 'post',
                        data: params,
                        dataType: "JSON",
                        headers: {
                            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                        },
                    }).then(response => {
                        if (response.status == 200) {
                            response.data.filter((value, key) => {
                                if (value.Category != "overAllNewAdmitted" && value.Category != "overAllRenewed") {
                                    if (value.PaymentMethod == "SUM") {
                                        sum += value.Revenue;
                                    }
                                }
                            })
                            this.reportTypeOv.map((value1, key) => {
                                let tempData1 = response.data.filter(p => p.Category == value1);
                                if (value1 == 'overAllMem')
                                    this.overAllMembership = tempData1;
                                else if (value1 == 'overAllMer')
                                    this.overAllMerchandise = tempData1;
                                else if (value1 == 'overAllSup')
                                    this.overAllSuplement = tempData1;
                                else if (value1 == 'overAllNewAdmitted')
                                    this.overAllNewAdmitted = tempData1;
                                else if (value1 == 'overAllRenewed')
                                    this.overAllRenew = tempData1;
                            })
                            this.overAllRevenew = sum;
                        }
                        else {
                            console.log("Error");
                        }

                    }).catch(function (error) {
                        alert(error)
                    })
                },
                getAllBranch() {
                    axios({
                        url: 'api/GetAllBranch',
                        method: 'get',
                        dataType: "JSON",
                        headers: {
                            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                        },
                    }).then(response => {
                        if (response.status == 200) {
                            response.data.map((value, key) => {
                                this.branchList.push(value.charAt(0).toUpperCase()+value.slice(1))
                            });
                        }
                        else {
                            console.log("Error");
                        }

                    }).catch(function (error) {
                        alert(error)
                    })
                }
            },
            mounted() {
                this.getAllBranch();
                var vm = this
                $("#txtStartDate1").nepaliDatePicker({
                    ndpYear: true,
                    ndpMonth: true,
                    ndpYearCount: 10,
                    dateFormat: "YYYY/MM/DD",
                    onChange: function (dateText) {
                        vm.startDate = dateText.bs
                    },
                });
                $("#txtEndDate1").nepaliDatePicker({
                    ndpYear: true,
                    ndpMonth: true,
                    ndpYearCount: 10,
                    dateFormat: "YYYY/MM/DD",
                    onChange: function (dateText) {
                        vm.endDate = dateText.bs
                    },
                });
                this.callAllMethod();
            }
        })
    </script>
</asp:Content>