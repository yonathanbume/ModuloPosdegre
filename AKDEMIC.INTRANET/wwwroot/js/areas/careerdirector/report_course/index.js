var reportCourse = function () {
    var datatable = {
        object: null,
        options: getSimpleDataTableConfiguration({
            url: `/director-carrera/reporte_asistencia_curso/curso/periodo`.proto().parseURL(),
            data: function (data) {
                data.pid = $("#select_terms").val();
                data.cid = $("#select_courses").val();
                data.search = $("#search").val();
            },
            pageLength: 10,
            orderable: [],
            columns: [
                {
                    data: 'code',
                    title: 'Código'
                },
                {
                    data: 'name',
                    title: 'Nombre'
                },
                {
                    data: null,
                    title: "Opciones",
                    render: function (row) {
                        return `<button data-id="${row.idcourseterm}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span>Ver Detalle </span></span></button>`;
                    }
                }
            ]
        }),
        reloadTable: function () {
            if (datatable.object === null) {
                datatable.init();
            } else {
                datatable.object.ajax.reload();
            }
        },
        events: {
            onDetails: function () {
                $("#report-datatable").on('click', '.btn-detail', function () {
                    var ctid = $(this).data("id");
                    location.href = `/director-carrera/reporte_asistencia_curso/reporte_curso_vista/${ctid}`.proto().parseURL();
                });
            },
            init: function () {
                this.onDetails();
            }
        },
        init: function () {
            datatable.events.init();
            datatable.object = $("#report-datatable").DataTable(datatable.options);
        }
    };
    var loadCoursesSelect = function () {
        var pid = $("#select_terms").val();
        $("#select_courses").select2({
            ajax: {
                url: `/director-carrera/reporte_asistencia_curso/cursos/periodo/${pid}`.proto().parseURL(),
                delay: 300,
                processResults: function (data) {
                    return {
                        results: data.results
                    };
                }
            },
            minimumInputLength: 0,
            placeholder: 'Cursos',
            allowClear: true
        }).on('change', function () {
            datatable.reloadTable();
        });
    }
    var loadTermsSelect = function () {
        $("#select_terms").select2({
            ajax: {
                url: `/director-carrera/reporte_asistencia_curso/periodos/get`.proto().parseURL(),
                delay: 300,
                processResults: function (data) {
                    return {
                        results: data.results
                    };
                }
            },
            minimumInputLength: 0,
            placeholder: 'Periodo Académico',
            allowClear: true
        }).on('change', function () {
            loadCoursesSelect();
        });
        $("#search").doneTyping(function () {
            datatable.reloadTable();
        });
    }

    return {
        load: function () {
            loadTermsSelect();
        }
    }
}();

$(function () {
    reportCourse.load();
})