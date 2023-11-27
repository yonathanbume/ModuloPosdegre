$('img').on('dragstart', function (event) { event.preventDefault(); });
$(document).on("mousemove", function (i) {
    function e(e, t) {
        return {
            "margin-top": -((i.pageY - $(window).scrollTop() - $(window).innerHeight() / 2) / t * 0.25).toFixed(1) + "px",
            "margin-left": -((i.pageX - $(window).innerWidth() / 2) / e * 0.25).toFixed(1) + "px"
        };
    }
    1024 <= $(window).width() && ($(".container-shape").css(e(30, 30)));
});
