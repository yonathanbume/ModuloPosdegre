var UniversityAuthorityManagement = function () {
    var private = {
        objects: {}
    };
    var inputs = {
        init: function () {
            $("#search").doneTyping(function () {
                private.objects["tbl-data"].draw();
            });
            $("#StartDateAdd, #StartDateEdit, #EndDateAdd, #EndDateEdit").datepicker({
                format: _app.constants.formats.datepicker,
            });
        }
    };
    var options = {
        ajax: {
            type: "GET",
            dataType: "JSON",
            url: `/admin/anuncios/por-sistema/get`.proto().parseURL(),
            data: function (values) {
                values.searchValue = $("#search").val();
            }
        },
        columns: [
            { data: "title", title: "Título" },
            { data: "type", title: "Tipo" },
            { data: "system", title: "Sistema" },
            { data: "startDate", title: "Fecha Inicio" },
            { data: "endDate", title: "Fecha Fin" },
            {
                data: null, title: "Opciones",
                render: function (data, type, row, meta) {
                    return `<a href="/admin/anuncios/por-sistema/editar/${data.id}" data-id="${data.id}" class="btn btn-sm btn-info edit"> <i class= "fa fa-edit"></i> </a>
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
                        url: `/admin/anuncios/por-sistema/guardar`.proto().parseURL(),
                        data: fd,
                        processData: false,
                        contentType: false,
                        success: function () {
                            $("#AddModal").modal('hide');
                            private.objects["tbl-data"].draw();
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
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
                        url: `/admin/anuncios/por-sistema/editar`.proto().parseURL(),
                        data: fd,
                        processData: false,
                        contentType: false,
                        success: function () {
                            $("#EditModal").modal('hide');
                            private.objects["tbl-data"].draw();
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#edit-form")[0].reset();
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
        }
    };

    var events = {
        datatable_init: function () {
            private.objects["tbl-data"].on("click", ".edit",
                function () {
                    var aid = $(this).data("id");
                    $.ajax({
                        url: `/admin/anuncios/por-sistema/get/${aid}`.proto().parseURL(),
                        type: "GET",
                        contentType: "application/json",
                        success: function (data) {
                            $("#edit-form select[name='Type']").val(data.type).trigger('change');
                            $("#edit-form select[name='System']").val(data.system).trigger('change');
                            $("#edit-form input[name='Id']").val(data.id);
                            $("#edit-form input[name='Description']").val(data.description);
                            $("#edit-form input[name='Title']").val(data.title);
                            $("#edit-form input[name='StartDate']").val(data.startDate);
                            $("#edit-form input[name='EndDate']").val(data.endDate);
                        }
                    });
                });
            private.objects["tbl-data"].on("click", ".delete",
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
                                    url: (`/admin/anuncios/por-sistema/eliminar/${aid}`).proto().parseURL(),
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
                                        }).then(private.objects["tbl-data"].draw());
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
        }
    };
    var dataTable = {
        init: function () {
            private.objects["tbl-data"] = $("#tbl-data").DataTable(options);
            events.datatable_init();
        }
    };

    return {
        init: function () {
            dataTable.init();
            inputs.init();
            validate.add();
            validate.edit();
        }
    };
}();

$(function () {
    UniversityAuthorityManagement.init();
});