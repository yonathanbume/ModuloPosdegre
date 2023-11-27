var reportCourseDetail = function () {
    var datatable = {
        object: null,
        options: getSimpleDataTableConfiguration({
            url: `/director-carrera/reporte_asistencia_curso/reporte_curso_detalles`.proto().parseURL(),
            data: function (data) {
                data.ctid = $("#IdCourseTerm").val();
            },
            pageLength: 10,
            orderable: [],
            columns: [
                {
                    data: 'fullname',
                    title: 'Nombre Completo',
                },
                {
                    data: 'absences',
                    title: 'Faltas',
                },
                {
                    data: 'dictated',
                    title: 'Clases dictadas',
                },
                {
                    data: 'maxAbsences',
                    title: 'Máx.Faltas',
                }],
        }),
        reloadTable: function () {
            if (datatable.object === null) {
                datatable.init();
            } else {
                datatable.object.ajax.reload();
            }
        },
        init: function () {
            datatable.object = $("#report-datatable").DataTable(datatable.options);
        }
    };

    return {
        load: function () {
            datatable.init();
        }
    }
}();

$(function () {
    reportCourseDetail.load();
})