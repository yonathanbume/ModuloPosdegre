var registered  = function () {
    var datatable = null;
    var eid = $("#Id").val();
    var options = {
        data: {
            type: "remote",
            source: {
                read: {
                    method: "GET",
                    url: `/admin/eventos/registrados/detalle/${eid}`.proto().parseURL()
                }
            }
        },
        sortable: false,
        pagination: false,
        columns: [
            {
                field: 'name',
                title: 'Nombres Completos',
                width: 250
            },
            {
                field: 'email',
                title: 'Email',
                width: 150
            },
            {
                field: 'dni',
                title: 'DNI',
                width: 100
            },
            {
                field: 'phoneNumber',
                title: 'Teléfono',
                width: 100
            }
        ]
    };

   
    var loadDatatable = function () {
        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        datatable = $(".m-datatable").mDatatable(options);
    };

    return {
        load: function () {
            loadDatatable();
        }
    };
}();

$(function () { 
    registered.load();
})