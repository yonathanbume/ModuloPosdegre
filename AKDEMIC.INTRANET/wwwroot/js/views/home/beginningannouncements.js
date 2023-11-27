var InitAppAnnouncements = function () {
    var modal = {
        init: function () {
            $("#BeginningAnnouncement_modal").modal("show");
            $(".modal-backdrop").removeClass("modal-backdrop fade show");

        }
    }

    return {
        init: function () {
            modal.init();
        }
    }
}();
$(function () {
    if ($("#BeginningAnnouncement").val() == "true") {
        InitAppAnnouncements.init();
    }
    $('.carousel_announcement').carousel({
        interval: 2000
    })
});