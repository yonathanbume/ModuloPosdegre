var NutritionalCare = function () {
    var nid = $("#MedicalAppointmentId").val();
    var datatable = null;
    var options = {
        data: {
            source: {
                read: {
                    type: 'GET',
                    url: `/nutricion/horario-citas/detalle-historico-todos/${nid}`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'username',
                title: 'Código',
                width: 100
            },
            {
                field: 'fullname',
                title: 'Nombres Completos',
                width: 120
            },
            {
                field: 'doctor',
                title: 'Doctor Asignado',
                width: 120
            },
            {
                field: 'datemedicalcare',
                title: 'Fecha',
                width: 120
            },
            {
                field: "options",
                title: "Opciones",
                width: 200,
                sortable: false,
                filterable: false,
                template: function (row) {
                    return `<button data-id='${row.id}' data-backid ='${row.back_id}' class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span> Ver Ficha </span></span></button>`;
                }
            }
        ]
    };

    var LoadDatatable = function () {
        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }

        datatable = $(".m-datatable").mDatatable(options);
        datatable.on('click', '.btn-detail', function () {
            var pid = $(this).data("id");
            var bid = $(this).data("backid");
            
            location.href = `/nutricion/horario-citas/ficha/${pid}/${bid}`.proto().parseURL();
        });
    };
   
    return {
        init: function () {
            LoadDatatable();

        }
    };
}();

$(function () {
    NutritionalCare.init();
});