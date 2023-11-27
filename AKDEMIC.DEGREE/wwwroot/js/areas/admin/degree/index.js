var degrees = function () {
    var datatable = null;

    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/admin/gestion-grados-titulos/todos`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'name',
                title: 'Descripción',
                width: 500
            },
            {
                field: 'abbreviation',
                title: 'Abreviatura',
                width: 100
            },            
            {
                field: 'options',
                title: 'Opciones',
                sortable: false,
                filterable: false,
                template: function (row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit" ><span><i class="flaticon-edit"></i><span>Editar</span></span></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete"><span><i class="flaticon-delete-1"></i><span>Eliminar</span></span></button>`;
                    return tmp;
                }
            }
        ]
    }
    var modals = function () {
        $(".btn-add").on('click', function () {
            $('#add_area_modal').modal('show');
        });
    }

    var events = function () {

        $("#add-form").validate({
            submitHandler: function (form, event) {                
                $.ajax({
                    type: "POST",
                    url: `/admin/gestion-grados-titulos/agregar`.proto().parseURL(),
                    data: $(form).serialize(),
                    success: function () {
                        $('#add_area_modal').modal('hide');
                        $("#add-form").resetForm();
                        datatable.reload();
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    },
                    error: function () {
                        toastr.success(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                    }
                });
            }
        });

        $("#edit-form").validate({
            submitHandler: function (form, event) {
                $.ajax({
                    type: "POST",
                    url: `/admin/gestion-grados-titulos/editar`.proto().parseURL(),
                    data: $(form).serialize(),
                    success: function () {
                        $('#edit_area_modal').modal('hide');                        
                        datatable.reload();
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.update);
                    },
                    error: function () {
                        toastr.success(_app.constants.toastr.message.error.update, _app.constants.toastr.title.error);
                    }
                });
            }
        });
    }
    var eventsDatatable = function () {
        datatable.on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var did = $(this).data('id');
            $.ajax({
                type: "GET",
                url: `/admin/gestion-grados-titulos/obtener/${did}`.proto().parseURL(),                
                success: function (data) {
                    $('#edit_area_modal').modal('show');
                    $('#edit_area_modal input[name="Id"]').val(data.id);
                    $('#edit_area_modal input[name="Name"]').val(data.name);
                    $('#edit_area_modal input[name="Abbreviation"]').val(data.abbreviation);                               
                }
                
            });
            
        });
        datatable.on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var did = $(this).data('id');
            swal({
                type: "warning",
                title: "¿Desea eliminar el registro?",
                showCancelButton: true,
                showLoaderOnConfirm: true,
                allowOutsideClick: () => !swal.isLoading(),
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            type: "POST",
                            url: `/admin/gestion-grados-titulos/eliminar/${did}`.proto().parseURL()
                        }).always(function () {
                            swal.close();
                        })
                            .done(function (result) {
                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                datatable.reload();                                
                            })
                            .fail(function (jqXHR, textStatus, errorThrown) {
                                var responseText = jqXHR.responseText;

                                if (responseText != "" && jqXHR.status == 400) {
                                    toastr.error(responseText, _app.constants.toastr.title.error);
                                } else {
                                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                }
                            });
                    })
                }
            });
        });
    }

    var loadDatatable = function () {
        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }       
        datatable = $(".m-datatable").mDatatable(options);
        eventsDatatable();

    }
   
    return {
        load: function () {
            loadDatatable();
            modals();
            events();
        }
    }
}();

$(function () {
    degrees.load();
});


