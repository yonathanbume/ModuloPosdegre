var reportDetails = function () {
    var datatable = {
        object: null,
        sectionId: null,
        options: getSimpleDataTableConfiguration({
            url: `/director-carrera/reporte_asistencia_docentes/secciones/get`.proto().parseURL(),
            data: function (data) {
                delete data.columns;
                data.termId = $("#select2-terms").val();
                data.teacherId = $("#TeacherId").val();
            },
            pageLength: 50,
            orderable: [],
            columns: [
                {
                    data: "course",
                    title: "Curso"
                },
                {
                    data: "section",
                    title: "Sección"
                },
                {
                    data: "dictatedClasses",
                    title: "Clases Dictadas"
                },
                {
                    data: "rescheduledClasses",
                    title: "Clases Reprogramadas"
                },
                {
                    data: "totalClasses",
                    title: "Clases Totales"
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
        init: function () {
            datatable.object = $("#data-datatable").DataTable(datatable.options);
        }
    };

    var loadTermSelect = function () {
        $.ajax({
            url: "/periodos/get".proto().parseURL()
        }).done(function (data) {
            $("#select2-terms").select2({
                data: data.items
            });

            $("#select2-terms").on("change", function (e) {
                datatable.reloadTable();
            });

            if (data.selected !== null) {
                $("#select2-terms").val(data.selected).trigger("change");
            }
        });
    };

    return {
        load: function () {
            loadTermSelect();
        }
    };
}();

$(function () {
    reportDetails.load();
});