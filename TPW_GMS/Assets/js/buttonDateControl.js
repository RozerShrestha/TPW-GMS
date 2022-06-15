$("#btnStop").click(function () {
    $("#btnStop").attr("class", "btnStart");
    return false;
});
$("#btnStart").click(function () {
    $("#btnStart").attr("class", "btnStop");
    return false;
});



//similar to document reload, this function runs on page reload
//$(function () {
//    $(".dateControl").datepicker({
//        dateFormat:'yy/mm/dd',
//        changeMonth: true,
//        changeYear: true
//    });
//    $('.timepicker').timepicker({
//            timeFormat: 'h:mm p',
//            interval: 60,
//            minTime: '06',
//            maxTime: '10:00pm',
//            defaultTime: '',
//            startTime: '06:00',
//            dynamic: false,
//            dropdown: true,
//            scrollbar: true
//        });
//});