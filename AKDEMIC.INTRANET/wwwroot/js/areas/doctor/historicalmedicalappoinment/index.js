var historicalmedicalappointment = function () {
    var datatable = null;
    var loadDatatable = function () {
        var sid = $("#select_specialties").val();        

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        options.data.source.read.url = `/doctor/historial-citas/especialidad/${sid}`.proto().parseURL();
        datatable = $(".m-datatable").mDatatable(options);        

        datatable.on('click', '.btn-detail', function () {
            var dic = $(this).data("id");
            location.href = `/doctor/historial-citas/historial/doctor/${dic}`.proto().parseURL();
        });

    }    
    
  
    var loadSpecialtiesSelect = function () {
        $.ajax({
            url: `/especialidades/get`.proto().parseURL()
        }).done(function (data) {
            $("#select_specialties").select2({
                 data: data.items
            }).trigger('change');            
            });
        $("#select_specialties").on('change', function () {
            loadDatatable();      
        });
    }  
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ""
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
                    return `<button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span> Ver Historial </span></span></button>`;
                }
            }
        ]
    }
    
    return {
        load: function () {
            loadSpecialtiesSelect();                        
        }
    }
}();

$(function () {
    historicalmedicalappointment.load();    
})