var InitApp = function () {

    var datatable = {
        areas: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/areas-extracurriculares/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {

                    }
                },
                columns: [
                    {
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "Tipo",
                        data: "typeText"
                    },
                    {
                        title: "F. Creación",
                        data: "createdAt"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            return `<button type="button" data-object="${data.proto().encode()}" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit" title=""><i class="la la-edit"></i> Editar</button> ` +
                                `<button class="btn btn-danger m-btn btn-sm m-btn--icon m-btn--icon-only btn-delete" data-id="${data.id}"><i class="la la-trash"></i></button>`;
                        }
                    }
                ]
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);

                $('#data-table').on('click', '.btn-edit', function (e) {
                    var object = $(this).data("object");
                    object = object.proto().decode();
                    form.edit.load(object);
                });

                $('#data-table').on('click', '.btn-delete', function (e) {
                    var id = $(this).data("id");
                    form.delete.load(id);
                });
            },
            reload: function () {
                this.object.ajax.reload();
            }
        }
    };

    var form = {
        create: {
            object: $("#create-form").validate({
                submitHandler: function (e) {
                    mApp.block("#create-modal .modal-content");

                    $.ajax({
                        url: $(e).attr("action"),
                        type: "POST",
                        data: $(e).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");

                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        datatable.areas.reload();
                        form.create.clear();
                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "") $("#create-form-alert-txt").html(error.responseText);
                        else $("#create-form-alert-txt").html(_app.constants.ajax.message.error);

                        $("#create-form-alert").removeClass("m--hide").show();
                    }).always(function () {
                        mApp.unblock("#create-modal .modal-content");
                    });
                }
            }),
            clear: function () {
                form.create.object.resetForm();
            }
        },
        edit: {
            object: $("#edit-form").validate({
                submitHandler: function (e) {
                    mApp.block("#edit-modal .modal-content");

                    $.ajax({
                        url: $(e).attr("action"),
                        type: "POST",
                        data: $(e).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");

                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        datatable.areas.reload();
                        form.edit.clear();
                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "") $("#edit-form-alert-txt").html(error.responseText);
                        else $("#edit-form-alert-txt").html(_app.constants.ajax.message.error);

                        $("#edit-form-alert").removeClass("m--hide").show();
                    }).always(function () {
                        mApp.unblock("#edit-modal .modal-content");
                    });
                }
            }),
            load: function (object) {
                console.log(object);
                $("#Edit_Id").val(object.id);
                $("#Edit_Name").val(object.name);
                $("#Edit_Type").val(object.type).trigger("change");
                $("#edit-modal").modal("toggle");
            },
            clear: function () {
                form.edit.object.resetForm();
            }
        },
        delete: {
            load: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El área será elimininado permanentemente",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Si, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar"
                }).then(function (result) {
                    if (result.value) {
                        $.ajax({
                            url: "/admin/areas-extracurriculares/eliminar",
                            type: "POST",
                            data: {
                                id: id
                            },
                            success: function () {
                                datatable.areas.reload();
                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            },
                            error: function () {
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            }
                        });
                    }
                });
            }
        },
        career: {
            object: $("#career-form").validate({
                submitHandler: function (e) {
                    mApp.block("#career-modal .modal-content");

                    $.ajax({
                        url: $(e).attr("action"),
                        type: "POST",
                        data: $(e).serialize()
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        form.career.clear();

                        datatable.careers.reload();
                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "") $("#career-form-alert-txt").html(error.responseText);
                        else $("#career-form-alert-txt").html(_app.constants.ajax.message.error);

                        $("#career-form-alert").removeClass("m--hide").show();
                    }).always(function () {
                        mApp.unblock("#career-modal .modal-content");
                    });
                }
            }),
            load: function (object) {
                $("#career-modal").modal("toggle");
                $("#caId").val(object.id);
                $("#caName").val(object.name);
                datatable.careers.load();
            },
            clear: function () {
                //form.career.object.resetForm();
                $("#career-select").val(null).trigger("change");
            }
        },
    };

    var select = {
        types: {
            init: function () {
                $(".type-select").select2({
                    placeholder: "Seleccione un tipo",
                    minimumResultsForSearch: -1
                });
            }
        },
        init: function () {
            this.types.init();
        }
    };

    var events = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.areas.reload();
            });
        }
    };

    return {
        init: function () {
            datatable.areas.init();
            select.init();
            events.init();
        }
    };
}();

$(function () {
    InitApp.init();
});