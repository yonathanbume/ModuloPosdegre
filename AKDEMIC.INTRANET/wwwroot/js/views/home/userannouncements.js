var InitAppUserAnnouncements = function () {
    var modal = {
        init: function () {
            $("#UserAnnouncement_modal").modal("show");
        }
    }

    return {
        init: function () {
            modal.init();
        }
    }
}();
$(function () {
    if ($("#UserAnnouncement").val() == "true") {
        InitAppUserAnnouncements.init();
    }
});