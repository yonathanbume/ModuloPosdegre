var reportEvent = function () {
    var datatable = null;
    var options = getSimpleDataTableConfiguration({
        url: `/admin/reporte_evento/getEvents`.proto().parseURL(),
        pageLength: 10,
        orderable: [],
        columns: [
            {
                data: 'name',
                title: 'Nombre'
            },
            {
                data: 'description',
                title: 'Descripción'
            },
            {
                data: 'eventtype',
                title: 'Tipo de Evento'
            },
            {
                data: 'organizer',
                title: 'Organizador'
            },
            {
                data: 'place',
                title: 'Lugar'
            },
            {
                data: 'eventdate',
                title: 'Fecha del Evento'
            },
            {
                data: 'registrationStartDate',
                title: 'Inicio de Inscripciones'
            },
            {
                data: 'registrationEndDate',
                title: 'Fin de Inscripciones'
            },
            {
                data: null,
                title: "Opciones",
                render: function (row) {
                    return `<button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span>Ver reporte </span></span></button>`;
                }
            }
        ]
    });
    var loadDatatable = function () {

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }

        datatable = $("#ajax_data").DataTable(options);
        datatable.on('click', '.btn-detail', function () {
            var eid = $(this).data("id");
            location.href = `/admin/reporte_evento/detalle/${eid}`.proto().parseURL();
        });
    }

    return {
        load: function () {
            loadDatatable();
        }
    }
}();

$(function () {
    reportEvent.load();
})