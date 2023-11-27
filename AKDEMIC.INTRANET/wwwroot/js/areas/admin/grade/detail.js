var Grades = function () {

    var init = function () {
        $('#select-term').on('change', function (e) {
            $("#lblAcademicYear").text('-');
            $("#lblSectionCount").text('-');
            $("#lblCreditSum").text('-');

            load();
        });
        if ($('#select-term').val()) {
            load();
        }
    }

    var load = function () {
        mApp.block('.m-portlet', { type: 'loader', message: 'Cargando...' });
        $.ajax({
            type: "GET",
            url: `/admin/notas/detalle/${$('#student').val()}/periodo/${$('#select-term').val()}/get`.proto().parseURL()
        }).done(function (data) {
            $('#enrolled-courses-container').html(data);
            $("[data-toggle='m-tooltip']").tooltip();
            $("[rel='m-tooltip']").tooltip();
            $("#lblAcademicYear").text($('#summaryAcademicYear').val());
            $("#lblSectionCount").text($('#summarySectionCount').val());
            $("#lblCreditSum").text($('#summaryCreditSum').val());
            mApp.unblock('.m-portlet');
        });
    }

    return {
        init: function () {
            init();
        }
    }
}();

$(function () {
    Grades.init();
});
