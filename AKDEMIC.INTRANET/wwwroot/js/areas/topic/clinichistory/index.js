﻿var clinicHistory = function () {
    var datatable = null;

    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/topico/historiales-clinicos/get`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'code',
                title: 'Código',
                width: 70
            },
            {
                field: 'name',
                title: 'Nombre',
                width: 150
            },
            {
                field: 'email',
                title: 'Correo Electrónico',
                width: 150
            },
            {
                field: "options",
                title: "Opciones",
                width: 200,
                sortable: false,
                filterable: false,
                template: function (row) {
                    return `<button data-id=${row.id}  class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span> Ver Detalle </span></span></button>`;
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
            var id = $(this).data("id");
            mApp.block(".m-portlet", { type: "loader", message: "Cargando..." });
            location.href = `/topico/historiales-clinicos/detalle/${id}`.proto().parseURL();
        });

    }
    return {
        load: function () {
            loadDatatable();
        }
    }
}();

$(function () {
    clinicHistory.load();
})