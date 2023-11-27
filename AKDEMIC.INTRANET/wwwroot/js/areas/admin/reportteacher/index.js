var reportSurvey = function () {
    var datatable = null;
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/admin/reporte_encuesta/getAllSurveys`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'codigo',
                title: 'Código',
                width: 70
            },
            {
                field: 'name',
                title: 'Nombre',
                width: 150
            },
            {
                field: 'description',
                title: 'Descripción',
                width: 150
            },
            {
                field: 'publicationdate',
                title: 'Fecha de publicación',
                width: 150
            },
            {
                field: 'finishdate',
                title: 'Fecha de finalización',
                width: 150
            },
            {
                field: "options",
                title: "Opciones",
                width: 200,
                sortable: false,
                filterable: false,
                template: function (row) {
                    return `<button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span>Ver reporte </span></span></button>`;
                }
            }
        ]
    }

    var loadDatatable = function () {

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }

        datatable = $(".m-datatable").mDatatable(options);

        datatable.on('click', '.btn-detail', function () {
            var eid = $(this).data("id");
            location.href = `/admin/reporte_encuesta/getReportSurveyDetail/${eid}`.proto().parseURL();
        });

    }

    return {
        load: function () {
            loadDatatable();
        }
    }
}();

$(function () {
    reportSurvey.load();
});