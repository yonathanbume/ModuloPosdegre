var UserTable = function () {
    var userDatatable = null;

    var options = {
        responsive: true,
        processing: true,
        serverSide: true,
        ajax: {
            url: "/admin/anuncios/por-usuarios/get".proto().parseURL(),
            data: function (d) {
                d.search = $("#search").val();
            }
        },
        columns: [
            { data: "fullName", title: "Usuario" },
            { data: "title", title: "Título" },
            { data: "type", title: "Tipo" },
            { data: "startDate", title: "Fecha Inicio" },
            { data: "endDate", title: "Fecha Fin" },
            {
                data: null, title: "Opciones",
                render: function (data, type, row, meta) {
                    return `<button data-toggle="modal" data-target="#EditModal" data-id="${data.id}" class="btn btn-sm btn-info edit"> <i class= "fa fa-edit"></i> </button>
                            <button data-id="${data.id}" class="btn btn-sm btn-danger delete"> <i class="fa fa-trash"></i> </button>`;
                }
            }
        ]
    };

    var validate = {
        add: function () {
            $("#add-form").validate({
                submitHandler: function (form) {
                    var btn = $(form).find('button[type="submit"]');
                    btn.addLoader();
                    var fd = new FormData($("#add-form")[0]);
                    $.ajax({
                        type: "POST",
                        url: `/admin/anuncios/por-usuarios/guardar`.proto().parseURL(),
                        data: fd,
                        processData: false,
                        contentType: false,
                        success: function () {
                            $("#AddModal").modal('hide');
                            swal({
                                type: "success",
                                title: _app.constants.toastr.title.success,
                                text: _app.constants.toastr.message.success.task,
                                confirmButtonText: "Excelente"
                            }).then(userDatatable.ajax.reload());
                            $("#add-form")[0].reset();
                        },
                        error: function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                        },
                        complete: function () {
                            btn.removeLoader();
                        }
                    });
                }
            });

        },
        edit: function () {
            $("#edit-form").validate({
                submitHandler: function (form) {
                    var btn = $(form).find('button[type="submit"]');
                    btn.addLoader();

                    var fd = new FormData($("#edit-form")[0]);
                    $.ajax({
                        type: "POST",
                        url: `/admin/anuncios/por-usuarios/editar`.proto().parseURL(),
                        data: fd,
                        processData: false,
                        contentType: false,
                        success: function () {
                            $("#EditModal").modal('hide');
                            
                            swal({
                                type: "success",
                                title: _app.constants.toastr.title.success,
                                text: _app.constants.toastr.message.success.task,
                                confirmButtonText: "Excelente"
                            }).then(userDatatable.ajax.reload());
                            $("#edit-form")[0].reset();
                        },
                        error: function (error) {
                            swal({
                                type: "error",
                                title: _app.constants.toastr.title.error,
                                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                confirmButtonText: "Entendido",
                                text: error.responseText
                            });
                        },
                        complete: function () {
                            btn.removeLoader();
                        }
                    });
                }
            });
        }
    };

    var events = {
        init: function () {
            userDatatable.on("click", ".edit",
                function () {
                    var aid = $(this).data("id");
                    $.ajax({
                        url: `/admin/anuncios/por-usuarios/get/${aid}`.proto().parseURL(),
                        type: "GET",
                        contentType: "application/json",
                        success: function (data) {
                            $("#edit-form select[name='Type']").val(data.type).trigger('change');
                            var newOption = new Option(data.fullName, data.userId, true, true);
                            $("#edit-user-select2").append(newOption).trigger("change");
                            //$("#edit-form select[name='System']").val(data.system).trigger('change');
                            $("#edit-form input[name='Id']").val(data.id);
                            $("#edit-form input[name='Description']").val(data.description);
                            $("#edit-form input[name='Title']").val(data.title);
                            $("#edit-form input[name='StartDate']").val(data.startDate);
                            $("#edit-form input[name='EndDate']").val(data.endDate);
                        }
                    });
                });
            userDatatable.on("click", ".delete",
                function () {
                    var aid = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "El registro será eliminado permanentemente",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, eliminarlo",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: (`/admin/anuncios/por-usuarios/eliminar/${aid}`).proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        id: aid
                                    },
                                    success: function (result) {
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El registro ha sido eliminado con éxito",
                                            confirmButtonText: "Excelente"
                                        }).then(userDatatable.ajax.reload());
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "El registro presenta información relacionada"
                                        });
                                    },

                                });
                            });
                        },
                        allowOutsideClick: () => !swal.isLoading()
                    });
                });
            $("#search").doneTyping(function () {
                datatable.init();
            });

            $("#StartDateAdd, #StartDateEdit, #EndDateAdd, #EndDateEdit").datepicker({
                format: _app.constants.formats.datepicker,
            });

            $("#add-user-select2").select2({
                placeholder: "Buscar...",
                dropdownParent: $("#AddModal"),
                ajax: {
                    url: "/admin/anuncios/por-usuarios/usuarios/get".proto().parseURL(),
                    dataType: "json",
                    data: function (params) {
                        return {
                            term: params.term
                        };
                    },
                    processResults: function (data, params) {
                        return {
                            results: data.items
                        };
                    },
                    cache: true
                },
                minimumInputLength: 3
            });
            $("#edit-user-select2").select2({
                placeholder: "Buscar...",
                dropdownParent: $("#EditModal"),
                ajax: {
                    url: "/admin/anuncios/por-usuarios/usuarios/get".proto().parseURL(),
                    dataType: "json",
                    data: function (params) {
                        return {
                            term: params.term
                        };
                    },
                    processResults: function (data, params) {
                        return {
                            results: data.items
                        };
                    },
                    cache: true
                },
                minimumInputLength: 3
            });
        }
    }

    var datatable = {
        init: function () {
            if (userDatatable)
            {
                userDatatable.ajax.reload();
            }
            else {
                userDatatable = $(".users-datatable").DataTable(options);
                events.init();
            }
        }
    }

    return {
        init: function () {
            datatable.init();
            validate.add();
            validate.edit();
        }
    }
}();

$(function () {
    UserTable.init();
});