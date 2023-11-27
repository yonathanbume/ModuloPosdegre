var dailyRecords = function () {
    var datatable = null;

    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/welfare/registro-diario-de-atenciones/get`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'date',
                title: 'Fecha',
                width: 100
            },
            {
                field: 'hour',
                title: 'Horario',
                width: 150
            },
            {
                field: 'fullname',
                title: 'Nombres completos',
                width: 120
            },
            {
                field: 'age',
                title: 'Edad',
                width: 50
            },
            {
                field: 'faculty',
                title: 'Escuela Profesional',
                width: 150
            },
            {
                field: 'doctor',
                title: 'Responsable',
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

    }



    return {
        load: function () {
            loadDatatable();
        }
    }
}();

$(function () {
    dailyRecords.load();
})