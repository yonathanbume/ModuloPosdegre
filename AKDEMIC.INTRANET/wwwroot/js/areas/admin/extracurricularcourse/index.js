var InitApp = function () {

    const datatableElement = $("#data-table");

    const modalCreateId = "#create-modal";
    const formCreateId = "#create-form";

    const modalEditId = "#edit-modal";
    const formEditId = "#edit-form";

    var datatable = {
        courses: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/cursos-extracurriculares/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        title: "Código",
                        data: "code"
                    },
                    {
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "Área",
                        data: "area",
                    },
                    {
                        title: "Créditos",
                        data: "credits"
                    },
                    {
                        title: "Precio",
                        data: "price",
                        render: function (data) {
                            if (data == 0) {
                                return "Gratuito"
                            }
                            return `S/. ${data.toFixed(2)}`
                        }
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
                this.object = datatableElement.DataTable(this.options);

                datatableElement.on('click', '.btn-edit', function (e) {
                    var object = $(this).data("object");
                    object = object.proto().decode();
                    form.edit.load(object);
                });

                datatableElement.on('click', '.btn-delete', function (e) {
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
            object: $(formCreateId).validate({
                submitHandler: function (e) {
                    mApp.block(`${modalCreateId} .modal-content`);

                    $.ajax({
                        url: $(e).attr("action"),
                        type: "POST",
                        data: $(e).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");

                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        datatable.courses.reload();
                        form.create.clear();
                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "")
                            $("#create-form-alert-txt").html(error.responseText);
                        else
                            $("#create-form-alert-txt").html(_app.constants.ajax.message.error);

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
            object: $(formEditId).validate({
                submitHandler: function (e) {
                    mApp.block(`${modalEditId} .modal-content`);

                    $.ajax({
                        url: $(e).attr("action"),
                        type: "POST",
                        data: $(e).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");

                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        datatable.courses.reload();
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

                $("#Edit_Id").val(object.id);
                $("#Edit_Code").val(object.code);
                $("#Edit_Name").val(object.name);
                $("#Edit_Credits").val(object.credits);
                $("#Edit_Description").val(object.description);
                $("#Edit_Price").val(object.price);

                $("#Edit_AreaId").val(object.areaId).trigger("change");

                $(modalEditId).modal("toggle");
            },
            clear: function () {
                form.edit.object.resetForm();
            }
        },
        delete: {
            load: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El curso será elimininado permanentemente",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Si, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar"
                }).then(function (result) {
                    if (result.value) {
                        $.ajax({
                            url: "/admin/cursos-extracurriculares/eliminar",
                            type: "POST",
                            data: {
                                id: id
                            },
                            success: function () {
                                datatable.courses.reload();
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
    };

    var select = {
        areas: {
            init: function () {
                $.ajax({
                    url: "/areas-extracurriculares/cursos/get".proto().parseURL()
                })
                    .done(function (data) {
                        $("#Add_AreaId").select2({
                            placeholder: "Seleccione un área",
                            dropdownParent: $(modalCreateId),
                            data: data.items
                        });

                        $("#Edit_AreaId").select2({
                            placeholder: "Seleccione un área",
                            dropdownParent: $(modalEditId),
                            data: data.items
                        });
                    });
            }
        },
        init: function () {
            this.areas.init();
        }
    };

    var events = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.courses.reload();
            });
        }
    };

    return {
        init: function () {
            datatable.courses.init();
            select.init();
            events.init();
        }
    };
}();

$(function () {
    InitApp.init();
});