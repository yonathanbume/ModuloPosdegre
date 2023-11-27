var followuserdegree = function () {
    var datatable = null;

    var loadDatatable = function (did) {
        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }        
        datatable = $(".m-datatable").mDatatable(options);
        eventsDatatable();

    }
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/admin/seguimiento-de-requerimientos/todos`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'fullname',
                title: 'Estudiante',
                width: 250
            },
            {
                field: 'email',
                title: 'Email',
                width: 200
            },
            {
                field: 'faculty',
                title: 'Facultad',
                width: 200
            },                        
            {
                field: 'options',
                title: 'Opciones',
                width: 200,
                sortable: false,
                filterable: false,
                template: function (row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm m-btn m-btn--icon btn-asign"><span><i class="flaticon-user-add"></i><span>Ver Expediente</span></span></button>`;
                    return tmp;
                }
            }
        ]
    }

  
    var eventsDatatable = function () {

        datatable.on('click', '.btn-asign', function (e) {
            var uid = $(this).data("id");
            e.preventDefault();
            mApp.block(".m-portlet", { type: "loader", message: "Cargando..." });
            location.href = `/admin/seguimiento-de-requerimientos/${uid}`.proto().parseURL();
        });      
    }



    return {
        load: function () {
           
            loadDatatable();
        }
    }
}();

$(function () {
    followuserdegree.load();
});


