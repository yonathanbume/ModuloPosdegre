var InitApp = function () {
    var report = {
        init: function () {
            $("#printReport").on("click", function () {
                var sectionId = $(this).data("id");
                window.location.href = `/admin/planes-de-estudios/pdf/${sectionId}`.proto().parseURL();
            });
        }
    };
    return {
        init: function () {
            report.init();
        }
    }
}();

$(function () {
    InitApp.init();
});