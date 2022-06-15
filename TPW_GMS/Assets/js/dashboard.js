$(document).ready(function () {
    $('#btnLoadData').on('click', function () {
        loadChart();
    });
    /*$('#myModal').modal('show');*/
    $('#activeHistoryModal').modal('show');
    loadChart();
    var loginUser = $("#lblUserLogin").text();
    if (!loginUser.includes("superadmin")) {
        $(".showAdmins").css("display", "none");
    }
});
function loadChart() {
    var colorArray = ['blue', 'aqua', 'green', 'red', 'yellow', 'orange', 'gray', 'black', 'purple', 'lime'];
    var todayDate = new Date();
    var startdt = document.getElementById('ContentPlaceHolder1_txtStartDate').value;
    var enddt = document.getElementById('ContentPlaceHolder1_txtEndDate').value;
    var branch = $('#ContentPlaceHolder1_ddlBranch').val();
    var tm = 0;
    var am = 0;
    var avm = 0;
    var im = 0;
    var gt = 0;
    if (startdt === '' && enddt === '') {
        startdt = '2019/1/1';
        enddt = '2019/12/30';
        $('#ContentPlaceHolder1_txtStartDate').val(startdt);
        $('#ContentPlaceHolder1_txtEndDate').val(enddt);
    }
    //total Membership
    $.ajax({
        url: 'api/GetTotalMembershipCount',
        headers: {
            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
        },
        type: "get",
        async: false,
        data: {
            startdate: startdt,
            enddate: enddt,
            branch: branch
        },
        success: function (response) {
            $("#totalMemberShipCount").empty();
            for (var i = 0; i < response.length; i++) {
                if (branch === 'All') {
                    var info = `<li><a id="tm_${response[i].branchName}" href="#">${response[i].branchName}<span class="pull-right badge bg-${colorArray[i]}">${response[i].Count}</span></a></li>`
                    $("#totalMemberShipCount").append(info);
                    tm += response[i].Count;
                }
                else if (branch === response[i].branchName.charAt(0).toLowerCase() + response[i].branchName.slice(1)) {
                    var info1 = `<li><a id="tm_${response[i].branchName}" href="#">${response[i].branchName}<span class="pull-right badge bg-${colorArray[i]}">${response[i].Count}</span></a></li>`
                    $("#totalMemberShipCount").append(info1);
                    tm += response[i].Count;
                }
                //var info = `<li><a id="tm_${response[i].branchName}" href="#">${response[i].branchName}<span class="pull-right badge bg-${colorArray[i]}">${response[i].Count}</span></a></li>`
                //$("#totalMemberShipCount").append(info);
                //tm += response[i].Count;
            }
            $("#totalMembership").text(tm);
        },
        error: function () {

        }
    });
    //Active Members
    $.ajax({
        url: 'api/GetActiveMembership',
        headers: {
            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
        },
        type: "get",
        async: false,
        success: function (response) {
            $("#activeMembers").empty();
            for (var i = 0; i < response.length; i++) {
                if (branch === 'All') {
                    let info = `<li><a id="am_${response[i].branch}" href="#">${response[i].branch}
                                            <span style="margin-left:10px" class="pull-right badge">${response[i].Avg}</span>
                                            <span class="pull-right badge bg-${colorArray[i]}">${response[i].No_of_member}</span>
                                        </a>
                                    </li>`
                    $("#activeMembers").append(info);
                    am += response[i].No_of_member;
                    avm += response[i].Avg;
                }
                else if (branch === response[i].branch.charAt(0).toLowerCase() + response[i].branch.slice(1)) {
                    var info = `<li><a id="am_${response[i].branch}" href="#">${response[i].branch}
                                            <span style="margin-left:10px" class="pull-right badge bg-${colorArray[i]}">${response[i].Avg}</span>
                                            <span class="pull-right badge bg-${colorArray[i]}">${response[i].No_of_member}</span>
                                        </a>
                                    </li>`
                    $("#activeMembers").append(info);
                    am += response[i].No_of_member;
                    avm += response[i].Avg;
                }
                //let info = `<li>
                //                <a id="am_${response[i].branch}" href="#">${response[i].branch}
                //                     <span style="margin-left:10px" class="pull-right badge bg-${colorArray[i]}">${response[i].Avg}</span>
                //                    <span class="pull-right badge bg-${colorArray[i]}">${response[i].No_of_member}</span>
                //                </a>
                //            </li>`
                //$("#activeMembers").append(info);
                //am += response[i].No_of_member;
                //avm += response[i].Avg;
            }
            $("#activeMemberhip").text(am);
            $("#activeAvgMemberhip").text(avm);


        },
        error: function () {

        }
    });
    //INactive Members
    $.ajax({
        url: 'api/GetInactiveMembershipCount',
        headers: {
            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
        },
        type: "get",
        async: false,
        data: {
            startdate: startdt,
            enddate: enddt
        },
        success: function (response) {
            $("#inactiveMembers").empty();
            for (var i = 0; i < response.length; i++) {
                if (branch === 'All') {
                    let info = `<li><a id="im_${response[i].branchName}" href="#">${response[i].branchName}<span class="pull-right badge bg-${colorArray[i]}">${response[i].Count}</span></a></li>`
                    $("#inactiveMembers").append(info);
                    im += response[i].Count;
                }
                if (branch === response[i].branchName.charAt(0).toLowerCase() + response[i].branchName.slice(1)) {
                    var info = `<li><a id="im_${response[i].branchName}" href="#">${response[i].branchName}<span class="pull-right badge bg-${colorArray[i]}">${response[i].Count}</span></a></li>`
                    $("#inactiveMembers").append(info);
                    im += response[i].Count;
                }
                //let info = `<li><a id="im_${response[i].branchName}" href="#">${response[i].branchName}<span class="pull-right badge bg-${colorArray[i]}">${response[i].Count}</span></a></li>`
                //$("#inactiveMembers").append(info);
                //im += response[i].Count;
            }
            $("#inactiveMembership").text(im);
        },
        error: function () {

        }
    });
    //GYM Traffic
    $.ajax({
        url: 'api/GetGymTraffic',
        headers: {
            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
        },
        type: "get",
        async: false,
        data: {
            startdate: startdt,
            enddate: enddt
        },
        success: function (response) {
            for (var i = 0; i < response.length; i++) {
                var info = `<li><a id="gt_${response[i].branchName}" href="#">${response[i].branchName}<span class="pull-right badge bg-${colorArray[i]}">${response[i].Count}</span></a></li>`
                $("#gymTraffic").append(info);
                gt += response[i].Count;
            }
            $("#gymTraffics").text(gt);
        },
        error: function () {

        }
    });
    //Admitted and Renew Pie
    $.ajax({
        url: 'api/GetAdmittedRenewData',
        headers: {
            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
        },
        type: "get",
        async: false,
        data: {
            startDate: startdt,
            endDate: enddt,
            branch: branch,
            type: 'pieData'
        },
        success: function (response) {
            var dat = [];
            $.each(response, (key, value) => {
                dat.push({ y: value.Numbers, name: value.Description });
            });
            //for (i = 0; i < response.length; i++) {
            //    dat.push('y:' + response[i].Count + ',' + 'label:' + response[i].branchName);
            //}
            var chart = new CanvasJS.Chart("pieAdmitRenew", {
                theme: "light2",
                animationEnabled: true,
                legend: {
                    cursor: "pointer"
                },
                data: [{
                    type: "pie",
                    showInLegend: true,
                    indexLabel: "{name}: {y}",
                    indexLabelFontWeight: "bold",
                    indexLabelFontColor: "white",
                    indexLabelPlacement: "inside",
                    dataPoints: dat
                }]
            });
            chart.render();
        },
        error: function () {

        }
    });
    //Admitted and Renew BarGraph
    $.ajax({
        url: 'api/GetAdmittedRenewData',
        headers: {
            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
        },
        type: "get",
        async: false,
        data: {
            startDate: startdt,
            endDate: enddt,
            branch: branch,
            type: 'barData'
        },
        success: function (response) {
            var datAdmitted = [];
            var datRenew = [];
            $.each(response, (key, value) => {
                if (value.type === "newAdmitted") {
                    datAdmitted.push({ label: value.MonthName, y: value.count });
                }
                else if (value.type === "renew") {
                    datRenew.push({ label: value.MonthName, y: value.count });
                }
            });
            //for (i = 0; i < response.length; i++) {
            //    dat.push('y:' + response[i].Count + ',' + 'label:' + response[i].branchName);
            //}
            var chart = new CanvasJS.Chart("barAdmitRenew", {
                animationEnabled: true,
                theme: "light2",
                axisX: {
                    title: "month",
                    valueFormatString: "DD MMM"

                },
                axisY: {
                    title: "Number of Customer"
                },
                data: [{
                    type: "column",
                    showInLegend: true,
                    name: "Renew",
                    indexLabel: "{y}",
                    color: "#51cda0",
                    dataPoints: datRenew
                },
                {
                    type: "column",
                    showInLegend: true,
                    name: "New Admitted",
                    indexLabel: "{y}",
                    xValueFormatString: "DD MMM, YYYY",
                    color: "#6d78ad",
                    dataPoints: datAdmitted
                }]
            });
            chart.render();
        },
        error: function () {

        }
    });
    //Membership Option Pie
    $.ajax({
        url: 'api/GetMembershipOption',
        headers: {
            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
        },
        type: "get",
        async: false,
        data: {
            startDate: startdt,
            endDate: enddt,
            branch: branch
        },
        success: function (response) {
            var chart = new CanvasJS.Chart("pieMembershipOption", {
                animationEnabled: true,
                legend: {
                    cursor: "pointer"
                },
                data: [{
                    type: "pie",
                    showInLegend: true,
                    legendText: "{label}",
                    indexLabel: "{label}: {y}",
                    indexLabelFontWeight: "bold",
                    indexLabelFontColor: "white",
                    indexLabelPlacement: "inside",
                    dataPoints: response
                }]
            });
            chart.render();
        },
        error: function () {

        }
    });
    //Earning From GYM Graph
    $.ajax({
        url: 'api/GetTotalEarningGymGraph',
        headers: {
            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
        },
        type: "get",
        async: false,
        data: {
            startdate: startdt,
            enddate: enddt,
            branch: branch
        },
        success: function (response) {
            //var dat = [];
            //for (i = 0; i < response.length; i++) {
            //    dat.push('y:' + response[i].Count + ',' + 'label:' + response[i].branchName);
            //}
            var chart = new CanvasJS.Chart("chartEarningGym", {
                animationEnabled: true,
                theme: "light2", // "light1", "light2", "dark1", "dark2"
                axisY: {
                    title: "Total Earnings(Rs)"
                },
                data: [{
                    type: "column",
                    showInLegend: true,
                    legendMarkerColor: "grey",
                    legendText: "Months",
                    indexLabel: "{y}",
                    dataPoints: response
                }]
            });
            chart.render();
        },
        error: function () {

        }
    });
    //AverageActiveMemberBranchWise
    $.ajax({
        url: 'api/GetTotalBranchAverageActiveCountMonthly',
        headers: {
            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
        },
        type: "get",
        success: function (response) {
            var dat = [];
            var ban = [{
                name: "Baneshwor",
                type: "spline",
                showInLegend: true,
                dataPoints: []
            }];
            var kum = [{
                name: "Kumaripati",
                type: "spline",
                showInLegend: true,
                dataPoints: []
            }];
            var kam = [{
                name: "Kamaladi",
                type: "spline",
                showInLegend: true,
                dataPoints: []
            }];
            var mai = [{
                name: "Maitidevi",
                type: "spline",
                showInLegend: true,
                dataPoints: []
            }];
            var mah = [{
                name: "Maharajgunj",
                type: "spline",
                showInLegend: true,
                dataPoints: []
            }];
            //var zAll = [{
            //    name: "All",
            //    type: "spline",
            //    showInLegend: true,
            //    dataPoints: []
            //}];
            $.each(response, (key, value) => {
                if (value.branch === 'Baneshwor')
                    ban[0].dataPoints.push({
                        label: ADToBS(value.date.split('T')[0]),
                        y: value.average
                    });
                else if (value.branch === 'Kamaladi')
                    kam[0].dataPoints.push({
                        label: ADToBS(value.date.split('T')[0]),
                        y: value.average
                    });
                else if (value.branch === 'Kumaripati')
                    kum[0].dataPoints.push({
                        label: ADToBS(value.date.split('T')[0]),
                        y: value.average
                    });
                else if (value.branch === 'Maharajgunj')
                    mah[0].dataPoints.push({
                        label: ADToBS(value.date.split('T')[0]),
                        y: value.average
                    });
                else if (value.branch === 'Maitidevi')
                    mai[0].dataPoints.push({
                        label: ADToBS(value.date.split('T')[0]),
                        y: value.average
                    });
                //else if (value.branch === 'zAll')
                //    zAll[0].dataPoints.push({
                //        label: ADToBS(value.date.split('T')[0]),
                //        y: value.count
                //    });
            })
            dat = [...ban, ...kum, ...kam, ...mai, ...mah]
            var chart = new CanvasJS.Chart("chartAverageActiveBranchWiseMonthly", {
                animationEnabled: true,
                axisY: {
                    title: "Number of Active Members"
                },
                toolTip: {
                    shared: true
                },
                legend: {
                    cursor: "pointer",
                    fontSize: 16,
                },
                data: dat
            });
            chart.render();
        },
        error: function () {

        }
    });
    //Average Active Members
    $.ajax({
        url: 'api/GetAverageActiveMembers',
        headers: {
            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
        },
        type: "get",
        async: false,
        data: {
            branch: branch
        },
        success: function (response) {
            var dat = [];
            for (i = 0; i < response.length; i++) {
                dat.push({
                    x: new Date(response[i].x),
                    y: response[i].Y
                });
            }
            var chart = new CanvasJS.Chart("chartAverageActiveMembers", {
                animationEnabled: true,
                theme: "light2", // "light1", "light2", "dark1", "dark2"
                axisY: {
                    title: "Average"
                },
                data: [{
                    type: "line",
                    dataPoints: dat
                }]
            });
            chart.render();
        },
        error: function () {

        }
    }); 


}