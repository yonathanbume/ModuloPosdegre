var InitApp = function () {

    const datatableElement = $("#data-table");

    const modalCreateId = "#add_activity_modal";
    const formCreateId = "#add-activity-form";

    const modalEditId = "#edit_activity_modal";
    const formEditId = "#edit-activity-form";

    var datatable = {
        activities: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/actividades-extracurriculares/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {

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
                        data: "area"
                    },
                    {
                        title: "Créditos",
                        data: "credits"
                    },
                    {
                        title: "Periodo",
                        data: "term"
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

                        datatable.activities.reload();
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

                        datatable.activities.reload();
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

                $("#Edit_AreaId").val(object.areaId).trigger("change");
                $("#Edit_TermId").val(object.termId).trigger("change");

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
                    text: "La actividad será elimininada permanentemente",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Si, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar"
                }).then(function (result) {
                    if (result.value) {
                        $.ajax({
                            url: "/admin/actividades-extracurriculares/eliminar",
                            type: "POST",
                            data: {
                                id: id
                            },
                            success: function () {
                                datatable.activities.reload();
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
        terms: {
            init: function () {
                $.ajax({
                    url: "/periodos/pendientes/get".proto().parseURL()
                })
                    .done(function (data) {
                        $("#Add_TermId").select2({
                            placeholder: "Seleccione un periodo",
                            dropdownParent: $(modalCreateId),
                            data: data.items
                        });

                        $("#Edit_TermId").select2({
                            placeholder: "Seleccione un periodo",
                            dropdownParent: $(modalEditId),
                            data: data.items
                        });

                        if (data.selected !== null) {
                            $("#Add_TermId").val(data.selected);
                            $("#Add_TermId").trigger("change.select2");

                            $("#Edit_TermId").val(data.selected);
                            $("#Edit_TermId").trigger("change.select2");
                        }
                    });
            }
        },
        areas: {
            init: function () {
                $.ajax({
                    url: "/areas-extracurriculares/actividades/get".proto().parseURL()
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
            this.terms.init();
            this.areas.init();
        }
    };

    var events = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.activities.reload();
            });
        }
    };

    return {
        init: function () {
            datatable.activities.init();
            select.init();
            events.init();
        }
    };
}();

$(function () {
    InitApp.init();
});