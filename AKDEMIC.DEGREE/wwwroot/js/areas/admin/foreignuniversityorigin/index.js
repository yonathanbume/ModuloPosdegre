var InitApp = function () {
    var datatable = {
        origins: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/universidad-extranjera/datatable/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        render: function (data) {
                            var template = "";
                            //Edit
                            template += "<button ";
                            template += "class='btn btn-info ";
                            template += "m-btn btn-sm m-btn--icon btn-edit' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-edit'></i><span>Editar</span></span></button> ";
                            //Delete
                            template += "<button ";
                            template += "class='btn btn-danger btn-delete ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-trash'></i></button>";
                            return template;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#data-table").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.edit.show(id);
                });

                $("#data-table").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.delete(id);
                });
            }
        },
        init: function () {
            this.origins.init();
        }
    };
    var modal = {
        create: {
            object: $("#add-form").validate({
                submitHandler: function (form, e) {
                    $("#btnSave").addLoader();
                    e.preventDefault();
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: $(form).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        datatable.origins.reload();
                        modal.create.clear();
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        $("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            show: function () {
                $("#btnSave").removeLoader();
                $("#addModal").modal("toggle");
            },
            clear: function () {
                modal.create.object.resetForm();
            },
            events: function () {
                $("#addModal").on("hidden.bs.modal", function () {
                    modal.create.clear();
                });
            }
        },
        edit: {
            object: $("#edit-form").validate({
                submitHandler: function (form, e) {
                    $("#btnEdit").addLoader();
                    e.preventDefault();
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: $(form).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        datatable.origins.reload();
                        modal.edit.clear();
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        $("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            load: function (id) {
                $.ajax({
                    url: `/admin/universidad-extranjera/${id}/detalle`.proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#edit-form input[name=Name]").val(result.name);
                    $("#edit-form input[name=Id]").val(result.id);
                }).fail(function (error) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                    else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#add_form_msg").removeClass("m--hide").show();
                }).always(function () {

                });
            },
            show: function (id) {
                $("#btnEdit").removeLoader();
                modal.edit.load(id);
                $("#editModal").modal("toggle");
            },
            clear: function () {
                modal.edit.object.resetForm();
            },
        },
        delete: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "El origen de la Universidad Extranjera será eliminado.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/admin/universidad-extranjera/eliminar").proto().parseURL(),
                            type: "POST",
                            data: {
                                id: id
                            },
                            success: function (result) {
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "El origen de la Universidad Extranjera ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.origins.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El origen de la Universidad Extranjera presenta información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },
        init: function () {
            this.create.events();
        }

    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.origins.reload();
            });
        }
    };
    var events = {
        init: function () {
            $("#addBtn").on('click', function () {
                modal.create.show();
            });
        }
    }
    return {
        init: function () {
            datatable.init();
            search.init();
            modal.init();
            events.init();
        }
    }
}();

$(function () {
    InitApp.init();
})