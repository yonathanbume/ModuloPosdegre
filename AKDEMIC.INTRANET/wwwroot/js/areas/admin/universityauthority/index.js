var UniversityAuthorityManagement = function () {
    var private = {
        objects: {}
    };
    var inputs = {
        init: function () {
            $("#search").doneTyping(function () {
                private.objects["tbl-data"].draw();
            });
            $("#add-user-select2").select2({
                placeholder: "Buscar...",
                dropdownParent: $("#AddAuthorityModal"),
                ajax: {
                    url: "/admin/gestion-de-autoridades/usuarios/get".proto().parseURL(),
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
                dropdownParent: $("#EditAuthorityModal"),
                ajax: {
                    url: "/admin/gestion-de-autoridades/usuarios/get".proto().parseURL(),
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
            $("#ResolutionDateAdd, #ResolutionDateEdit").datepicker({
                format: _app.constants.formats.datepicker,
            });
            $("#ResolutionFile").on('change', function (event) {
                $(this).valid();
                $(this).next('.custom-file-label').html(event.target.files[0].name);
            });
        }
    };
    var options = {
        ajax: {
            type: "GET",
            dataType: "JSON",
            url: `/admin/gestion-de-autoridades/obtener-autoridades`.proto().parseURL(),
            data: function (values) {
                values.searchValue = $("#search").val();
            }
        },
        columns: [
            { data: "name", title: "Nombres Completos" },
            { data: "type", title: "Tipo" },
            {
                data: null, title: "Opciones",
                render: function (data, type, row, meta) {
                    return `<button data-id="${data.id}" class="btn btn-success btn-sm m-btn m-btn--icon btn-history" title="Historial de Autoridad"><span><i class="la la-history"></i><span>Historial</span></span></button>
                            <button data-toggle="modal" data-target="#EditAuthorityModal" data-id="${data.id}" class="btn btn-sm btn-info edit"> <i class= "fa fa-edit"></i> </button>
                            <button data-id="${data.id}" class="btn btn-sm btn-danger delete"> <i class="fa fa-trash"></i> </button>`;

                }
            }
        ]
    };

    var validate = {
        add: function () {
            $("#add-university-authority-form").validate({
                submitHandler: function (form) {
                    var btn = $(form).find('button[type="submit"]');
                    btn.addLoader();
                    var fd = new FormData($("#add-university-authority-form")[0]);
                    fd.append("File.File", $('#FileAdd')[0].files[0]);
                    $.ajax({
                        type: "POST",
                        url: `/admin/gestion-de-autoridades/guardar-autoridad`.proto().parseURL(),
                        data: fd,
                        processData: false,
                        contentType: false,
                        success: function () {
                            $("#AddAuthorityModal").modal('hide');
                            private.objects["tbl-data"].draw();
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#add-university-authority-form")[0].reset();
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
            $("#edit-university-authority-form").validate({
                submitHandler: function (form) {
                    var btn = $(form).find('button[type="submit"]');
                    btn.addLoader();

                    var fd = new FormData($("#edit-university-authority-form")[0]);
                    fd.append("File.File", $('#FileEdit')[0].files[0]);
                    $.ajax({
                        type: "POST",
                        url: `/admin/gestion-de-autoridades/editar-autoridad`.proto().parseURL(),
                        data: fd,
                        processData: false,
                        contentType: false,
                        success: function () {
                            $("#EditAuthorityModal").modal('hide');
                            private.objects["tbl-data"].draw();
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#edit-university-authority-form")[0].reset();
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

    var history = {
        load: function (id) {
            $("#history_modal").modal("toggle");
            var url = `/admin/gestion-de-autoridades/historial?id=${id}`.proto().parseURL();
            var newoptions = history.options;
            newoptions.ajax.url = url;
            if (history.object != null) {
                history.object.destroy();
            }
            history.object = $("#history_table").DataTable(newoptions);
        },
        object: null,
        options: {
            serverSide: false,
            filter: false,
            lengthChange: false,
            //pageLength: 50,
            paging: false,
            ajax: {
                url: `/admin/gestion-de-autoridades?id={id}`.proto().parseURL(),
                type: "GET"
            },
            columns: [
                {
                    data: 'date',
                    title: 'Fecha',
                },
                {
                    data: "name",
                    title: "Nombres Completos"
                },
                {
                    data: "type",
                    title: "Tipo"
                },
                {
                    data: null, title: "Opciones",
                    render: function (data, type, row, meta) {
                        return `<a href="/file/${data.fileUrl}" class="btn btn-sm btn-primary"> <i class="la la-download"></i> </a>`;
                    }
                }
            ]
        },
    };
    var events = {
        datatable_init: function () {
            private.objects["tbl-data"].on("click",
                ".btn-history",
                function () {
                    var id = $(this).data("id");
                    history.load(id);
                });
            private.objects["tbl-data"].on("click", ".edit",
                function () {
                    var aid = $(this).data("id");
                    $.ajax({
                        url: `/admin/gestion-de-autoridades/obtener-autoridad/${aid}`.proto().parseURL(),
                        type: "GET",
                        contentType: "application/json",
                        success: function (data) {
                            var newOption = new Option(data.fullName, data.userId, true, true);
                            $("#edit-user-select2").append(newOption).trigger("change");
                            $("#edit-university-authority-form select[name='Type']").val(data.type).trigger('change');
                            $("#edit-university-authority-form input[name='Id']").val(data.id);
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
                                    url: (`/admin/gestion-de-autoridades/eliminar/${aid}`).proto().parseURL(),
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