var details = function () {
    var datatable = null;
    var dic = $("#Id").val();



    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/doctor/historial-citas/historicalmedicalappointments/doctor/${dic}`.proto().parseURL()
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
                field: 'date',
                title: 'Fecha',
                width: 150
            },

            {
                field: "options",
                title: "Opciones",
                width: 200,
                sortable: false,
                filterable: false,
                template: function (row) {
                    if (row.specialty == "Nutrición")
                    {
                        return `<button data-id="${row.id}" data-doctor="${row.doctorid}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span> Ver Ficha </span></span></button>`;
                    }
                    else if (row.specialty == "Psicología") {

                        return `<button data-id="${row.id}" data-doctor="${row.doctorid}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span> Ver Ficha </span></span></button>`;
                    }
                    else {
                        return `<button data-id="${row.id}" data-doctor="${row.doctorid}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span> Ver Consulta </span></span></button>`;
                    }
                    
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
            var did = $(this).data("doctor");
            var sid = $(this).data("id");
            location.href = `/doctor/historial-citas/detalle/${did}/${sid}/get`.proto().parseURL();
        });

    }    
    
  
    
    
    return {
        load: function () {
            loadDatatable();                        
        }
    }
}();

$(function () {
    details.load();    
})