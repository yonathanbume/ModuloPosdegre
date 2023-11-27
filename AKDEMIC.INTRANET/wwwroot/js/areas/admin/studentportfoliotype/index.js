var InitApp = function () {

    var datatable = {
        categories: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/tipos-portafolio/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        title: "Nombre",
                        data: "name",
                    },
                    {
                        title: "Tipo",
                        data: "typeName",
                    },
                    {
                        title: "Dependencia",
                        data: "dependency",
                    },
                    {
                        title: "¿Puede subirlo el estudiante?",
                        data : null,
                        render: function (row) {
                            var tpm = "";
                            tpm += row.canUploadStudent ? "Sí" : "No";

                            return tpm;
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        //width: "100px",
                        render: function (data) {
                            return `<button type="button" data-object="${data.proto().encode()}" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit" title="Editar"><i class="la la-edit"></i> Editar</button>`
                                + ` <button class="btn btn-danger m-btn btn-sm m-btn--icon m-btn--icon-only btn-delete" data-id="${data.id}"><i class="la la-trash"></i></button>`;
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
                        datatable.categories.reload();
                        form.create.clear();
                        $("#Type").val(null).trigger("change");
                        $("#eCanUploadStudent").prop("ckecked",false);

                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "") $("#create-alert-text").html(error.responseText);
                        else $("#create-alert-text").html(_app.constants.ajax.message.error);
                        $("#create-alert").removeClass("m--hide").show();
                    }).always(function () {
                        mApp.unblock("#create-modal .modal-content");
                    });
                }
            }),
            clear: function () {
                this.object.resetForm();
                $(".create-dependency-select").val(null).trigger('change');
                $("#edit-form").find("[name='Type']").val(1).trigger("change");
            }
        },
        delete: {
            load: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "Se eliminará el tipo de portafolio seleccionado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Si, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar"
                }).then(function (result) {
                    if (result.value) {
                        $.ajax({
                            url: "/admin/tipos-portafolio/eliminar",
                            type: "POST",
                            data: {
                                id: id
                            },
                            success: function () {
                                datatable.categories.reload();
                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            },
                            error: function () {
                                toastr.error("Hay información relacionada al tipo", _app.constants.toastr.title.error);
                            }
                        });
                    }
                });
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
                        datatable.categories.reload();
                        form.edit.clear();
                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "") $("#edit-alert-text").html(error.responseText);
                        else $("#edit-alert-text").html(_app.constants.ajax.message.error);
                        $("#edit-alert").removeClass("m--hide").show();
                    }).always(function () {
                        mApp.unblock("#edit-modal .modal-content");
                    });
                }
            }),
            clear: function () {
                form.edit.object.resetForm();
                $(".edit-dependency-select").val(null).trigger('change');
            },
            load: function (object) {
                $("#edit-id").val(object.id);
                $("#eName").val(object.name);
                $("#eDependencyId").val(object.dependencyId).trigger("change");

                if (object.canUploadStudent) {
                    $("#eCanUploadStudent").prop("checked", true);
                } else {
                    $("#eCanUploadStudent").prop("checked", false);
                }

                $("#edit-modal").modal("show");
            }
        },
        init: function () {

        }
    };

    var select = {
        dependency: {
            init: function () {
                $.ajax({
                    url: "/dependencias/todas".proto().parseURL()
                }).done(function (data) {
                    $(".create-dependency-select").select2({
                        data: data.items,
                        placeholder: 'Seleccione una dependencia',
                        dropdownParent: $('#create-modal'),
                        allowClear: true,
                    });
                    $(".edit-dependency-select").select2({
                        data: data.items,
                        placeholder: 'Seleccione una dependencia',
                        dropdownParent: $('#edit-modal'),
                        allowClear: true,
                    });
                });
            }
        },
        type: {
            init: function () {
                $("#create-modal").find("[name='Type']").select2({
                    dropdownParent: $('#create-modal'),
                    placeholder: 'Seleccione un tipo',
                });
            }
        },
        init: function () {
            this.type.init();
            this.dependency.init();
        }
    };

    var events = {
        init: function () {
            $('#create-modal').on('hidden.bs.modal', function () {
                form.create.clear();
                $(".m-alert").addClass("m--hide");
            });

            $('#edit-modal').on('hidden.bs.modal', function () {
                form.edit.clear();
                $(".m-alert").addClass("m--hide");
            });
        }
    };

    return {
        init: function () {
            datatable.categories.init();
            select.init();

            form.init();
            events.init();
        }
    };
}();

$(function () {
    InitApp.init();
});