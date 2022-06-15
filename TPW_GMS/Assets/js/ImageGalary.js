$('#mix-wrapper').mixItUp({
    load: {
        sort: "order:asc"
    },
    animation: {
        effects: "fade rotateZ(0deg)",
        duration: 700
    },
    selectors: {
        target: ".mix-target",
        filter: ".filter-btn",
        sort: ".sort-btn"
    },
    callbacks: {
        onMixEnd: function (state) {
            console.log(state);
        }
    }
});

/* counter start */
$(document).ready(function () {
    var max_text = 100;
    $("#character_remaining").html(
      "<b>" + max_text + " characters remaining.</b>"
    );
    $("textarea").keyup(function () {
        var text_length = $("textarea").val().length;
        var total_character_remaining = max_text - text_length;
        $("#character_remaining").html(
          "<b>" + total_character_remaining + " characters remaining.</b>"
        );
    });
});

/*background color  */
$(document).ready(function () {
    var color = $("#bgcolor").val();
    $("body").css("background", color);

    $("#bgcolor").change(function () {
        color = $("#bgcolor").val();
        $("body").css("background", color);
    });
});
/*date picker  */
$(function () {
    $("#datepicker").datepicker();
});

/*tabbed  */
$(document).ready(function () {
    $("ul.tabs li").click(function () {
        var tab_id = $(this).attr("data-tab");

        $("ul.tabs li").removeClass("current");
        $(".tab-content").removeClass("current");

        $(this).addClass("current");
        $("#" + tab_id).addClass("current");
    });
});
/*paralux  */
/* 
See https://codepen.io/MarcelSchulz/full/lCvwq

The effect doens't appear as nice when viewing in split view :-)

Fully working version can also be found at (http://schulzmarcel.de/x/drafts/parallax).

*/

jQuery(document).ready(function () {
    $(window).scroll(function (e) {
        parallaxScroll();
    });

    function parallaxScroll() {
        var scrolled = $(window).scrollTop();
        $("#parallax-bg-1").css("top", 0 - scrolled * 0.25 + "px");
        $("#parallax-bg-2").css("top", 0 - scrolled * 0.4 + "px");
        $("#parallax-bg-3").css("top", 0 - scrolled * 0.75 + "px");
    }
});
