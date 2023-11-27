var requirementes = function () {
    var datatable = null;
    var Id = $("#Id").val();
    var loadDatatable = function () {
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
                    url: `/admin/seguimiento-de-requerimientos/obtener-requerimientos/${Id}`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'requeriment_description',
                title: 'Descripción del requisito',
                width: 500
            },
            {
                field: 'current_status',
                title: 'Estado actual',
                width: 150,
                template: function (row) {
                    if (row.current_status == 0) {
                        return `<span class="m-badge  m-badge--metal m-badge--wide">Pendiente</span>`;
                    } else if (row.current_status == 1) {
                        return `<span class="m-badge  m-badge--info m-badge--wide">Presentado</span>`;
                    } else if (row.current_status == 2) {
                        return `<span class="m-badge  m-badge--success m-badge--wide">Aprobado</span>`;
                    } else {
                        return `<span class="m-badge  m-badge--primary m-badge--wide">Con Observaciones</span>`;
                    }
                }
            },
            {
                field: 'options',
                title: 'Opciones',
                width: 200,
                sortable: false,
                filterable: false,
                template: function (row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-update"><span><i class="flaticon-cogwheel-2"></i><span>Actualizar Estado</span></span></button>`;
                    return tmp;
                }
            }
        ]
    }


    var eventsDatatable = function () {
        datatable.on('click', '.btn-update', function (e) {
            e.preventDefault();
            var rid = $(this).data("id");
            $("#update_area_modal").modal("show");
            $("#update-form input[id='rid']").val(rid);
            $("#update-form select[id='status']").select2();        
        });

        $("#update-form").validate({
            submitHandler: function (form, event) {
                $.ajax({
                    type: "POST",
                    url: `/admin/seguimiento-de-requerimientos/actualizar-estado`.proto().parseURL(),
                    data: {
                        rid: $("#rid").val(),
                        status: $("#status").val()
                    },
                    success: function () {
                        $("#update_area_modal").modal("hide");
                        datatable.reload();
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    },
                    error: function () {
                        toastr.success(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                    }
                });
                
            }

        });

    }
    
    return {
        load: function () {
            
            loadDatatable();
        }
    }
}();

$(function () {
    requirementes.load();
});


