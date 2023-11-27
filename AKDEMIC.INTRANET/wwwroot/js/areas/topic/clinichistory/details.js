var clinicHistoryDetails = function () {
    var datatable = null;
    var id = $("#Id").val();
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/topico/historiales-clinicos/receta/${id}`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'dateatention',
                title: 'Fecha de atención',
                width: 150
            },
            {
                field: 'reason',
                title: 'Servicio',
                width: 150
            },
            {
                field: 'doctor_name',
                title: 'Doctor responsable',
                width: 150
            }
        ]
    }
    var loadDatatable = function () {

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }

        datatable = $(".m-datatable").mDatatable(options);

        //datatable.on('click', '.btn-detail', function () {
        //    var id = $(this).data("id");
        //    mApp.block(".m-portlet", { type: "loader", message: "Cargando..." });
        //    location.href = `/topico/historiales-clinicos/detalle/${id}`.proto().parseURL();
        //});

    }
    return {
        load: function () {
            loadDatatable();
        }
    }
}();

$(function () {
    clinicHistoryDetails.load();
})