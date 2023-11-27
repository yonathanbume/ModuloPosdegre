var ActivitiesTable = function () {
    var id = "#activities-datatable";
    var datatable;
    var datatableDetailEvaluation;
    var datatableDetailSection;

    var options = {
        search: {
            input: $("#search")
        },
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ("/admin/actividades-extracurriculares/get").proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "code",
                title: "Código"
            },
            {
                field: "name",
                title: "Nombre"
            },
            {
                field: "credits",
                title: "Créditos"
            },
            {
                field: "options",
                title: "Opciones",
                textAlign: "center",
                width: 125,
                sortable: false,
                filterable: false,
                template: function (row) {
                    var template = "";
                    template += "<button data-id='" + row.id + "' class='btn btn-info btn-sm m-btn m-btn--icon btn-edit' data-toggle='modal' data-target='#edit_activity_modal'><span><i class='la la-edit'></i><span>Editar</span></span></button> ";
                    template += "<button data-id='" + row.id + "' class='btn btn-danger btn-sm m-btn--icon btn-delete'><i class='la la-trash'></i></button>";
                    return template;
                }
            }
        ]
    };

    var events = {
        init: function () {
            $(".btn-delete").on("click", function () {
                var dataId = $(this).data("id");
                swal({
                    title: "¿Está seguro?",
                    text: "La actividad extracurricular será eliminada",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarla",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise(() => {
                            $.ajax({
                                url: ("/admin/actividades-extracurriculares/eliminar").proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: dataId
                                },
                                success: function () {
                                    datatable.reload();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "La actividad extracurricular ha sido eliminada con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function () {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "La actividad extracurricular tiene información relacionada"
                                    });
                                }
                            });
                        });
                    }
                });
            });
            $(".btn-edit").on("click", function () {
                var dataId = $(this).data("id");
                Form.Edit.show(dataId);
            });
        }
    };

    var loadDatatable = function () {
        if (datatable !== undefined)
            datatable.destroy();
        options.data.source.read.url = ("/admin/actividades-extracurriculares/get").proto().parseURL();
        datatable = $(id).mDatatable(options);
        $(datatable).on("m-datatable--on-layout-updated", function () {
            events.init();
        });
    };
    return {
        init: function () {
            loadDatatable();
        },
        reload: function () {
            datatable.reload();
        }
    }
}();

var Form = function () {
    var modalCreateId = "#add_activity_modal";
    var formCreateId = "#add-activity-form";
    var formCreateValidate;

    var modalEditId = "#edit_activity_modal";
    var formEditId = "#edit-activity-form";
    var formEditValidate;

    var events = {
        init: function () {
            $(modalCreateId).one("hidden.bs.modal", function () {
                form.reset.create();
            });
            $("#Edit_Credits").on("blur", function () {
                var value = parseInt(this.value);
                if (isNaN(value))
                    value = 0;
                this.value = value;
                if (this.value < 0)
                    this.value = 0;
            });
            $("#Add_Credits").on("blur", function () {
                var value = parseInt(this.value);
                if (isNaN(value))
                    value = 0;
                this.value = value;
                if (this.value < 0)
                    this.value = 0;
            });
        }
    }

    var form = {
        submit: {
            create: function (formElements) {
                var data = $(formElements).serialize();
                var formData = new FormData($(formElements)[0]);
                $(`${modalCreateId} input, ${modalCreateId} select, ${modalCreateId} textarea`).attr("disabled", true);
                $("#btnCreate").addLoader();
                $.ajax({
                    url: $(formElements).attr("action"),
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false
                }).always(function () {
                    $(`${modalCreateId} input, ${modalCreateId} select, ${modalCreateId} textarea`).attr("disabled", false);
                    $("#btnCreate").removeLoader();
                }).done(function () {
                    $(modalCreateId).modal("toggle");
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    ActivitiesTable.reload();
                }).fail(function (e) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (e.responseText != null) $("#add_form_msg_txt").html(e.responseText);
                    else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#add_form_msg").removeClass("m--hide").show();
                });
            },
            edit: function (formElements) {
                var data = $(formElements).serialize();
                var formData = new FormData($(formElements)[0]);
                $(`${modalEditId} input, ${modalEditId} select, ${modalEditId} textarea`).attr("disabled", true);
                $("#btnEdit").addLoader();
                $.ajax({
                    url: $(formElements).attr("action"),
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false
                }).always(function () {
                    $(`${modalEditId} input, ${modalEditId} select, ${modalEditId} textarea`).attr("disabled", false);
                    $("#btnEdit").removeLoader();
                }).done(function () {
                    $(modalEditId).modal("toggle");
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    ActivitiesTable.reload();
                }).fail(function (e) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (e.responseText != null) $("#edit_form_msg_txt").html(e.responseText);
                    else $("#edit_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#edit_form_msg").removeClass("m--hide").show();
                });
            }
        },
        reset: {
            create: function () {
                $("#add_form_msg").addClass("m--hide").hide();
                formCreateValidate.resetForm();
            },
            edit: function () {
                $("#edit_form_msg").addClass("m--hide").hide();
                formEditValidate.reset();
            }
        },
        show: {
            edit: function (id) {
                mApp.blockPage();
                $.ajax({
                    url: `/admin/actividades-extracurriculares/${id}/get`.proto().parseURL()
                }).done(function (result) {
                    var formElements = $("#edit-activity-form").get(0).elements;
                    formElements["Edit_Id"].value = result.id;
                    formElements["Edit_Code"].value = result.code;
                    formElements["Edit_Name"].value = result.name;
                    formElements["Edit_Credits"].value = result.credits;
                    formElements["Edit_Description"].value = result.description;
                    formElements["Edit_TermId"].value = result.termId;
                    formElements["Edit_AreaId"].value = result.areaId;
                    mApp.unblockPage();

                    $(modalEditId).one("hidden.bs.modal", function () {
                        form.reset.edit();
                    });
                }).fail(function (error) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            }
        }
    }

    var validateRules = {
        Add_Name: {
            required: true,
            maxlength: 100
        },
        Edit_Name: {
            required: true,
            maxlength: 100
        }
    };
    var validate = {
        init: function () {
            formCreateValidate = $(formCreateId).validate({
                rules: validateRules,
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.create(formElement);
                }
            });

            formEditValidate = $(formEditId).validate({
                rules: validateRules,
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.edit(formElement);
                }
            });
        }
    }

    var select = {
        terms: {
            init: function () {
                $.ajax({
                    url: "/periodos/get".proto().parseURL()
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

    return {
        init: function () {
            events.init();
            validate.init();
            select.init();
        },
        Edit: {
            show: function (id) {
                form.show.edit(id);
            }
        }
    }
}();

$(function () {
    ActivitiesTable.init();
    Form.init();
});