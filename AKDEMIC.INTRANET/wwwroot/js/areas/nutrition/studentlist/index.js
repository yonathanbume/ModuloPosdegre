var studentlist = function () {
    var datatable = null;

    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ("/nutricion/alumnos/get").proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'username',
                title: 'Usuario',
                width: 100
            },
            {
                field: 'fullname',
                title: 'Nombres Completos',
                width: 200
            },
            {
                field: 'email',
                title: 'Correo electrónico',
                width: 180
            },
            {
                field: 'faculty',
                title: 'Escuela Profesional',
                width: 180
            },
            {
                field: 'options',
                title: 'Opciones',
                width: 350,
                sortable: false,
                filterable: false,
                template: function (row) {
                    var template = "";  
                    if (row.iscreatednutritionalrecord === false) {

                        template += ` <span style="width: 163px;" class="m-badge  m-badge--info m-badge--wide"> Sin fichas nutricionales</span>`;
                    }
                    else {
                        template += `<button data-id='${row.id}' class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail" > <span><i class="la la-eye"></i><span> Ver Ficha Nutricional</span></span></button>`;
                        template += `  <button data-id="${row.id}" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete"<span><i class="la la-trash"> </i> </span> Eliminar </span></span></button>`;
                    }                 
                    return template;

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
            var pid = $(this).data("id");
            mApp.block(".m-portlet", { type: "loader", message: "Cargando..." });    
            location.href = `/nutricion/alumnos/ficha/${pid}`.proto().parseURL();
        });

        datatable.on('click', '.btn-delete', function () {
            var sid = $(this).data("id");
            swal({
                title: "¿Está seguro?",
                text: "Todos los datos relacionados de la ficha nutricional del estudiante serán eliminado permanentemente",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Si, eliminarlo",
                cancelButtonText: "Cancelar"
            }).then(function (result) {
                if (result.value) {
                    $.ajax({
                        url: `/nutricion/alumnos/ficha-nutricional/eliminar/${sid}`.proto().parseURL(),
                        type: "POST",
                        success: function () {
                            toastr.success(_app.constants.toastr.message.success.delete, _app.constants.toastr.title.success);
                            datatable.reload();
                        },
                        error: function () {
                            toastr.error(_app.constants.toastr.message.error.delete, _app.constants.toastr.title.error);
                        }
                    });
                }
            });
        });

    }        
    return {
        load: function () {
            loadDatatable();                        
        }
    }
}();

$(function () {
    studentlist.load();    
})