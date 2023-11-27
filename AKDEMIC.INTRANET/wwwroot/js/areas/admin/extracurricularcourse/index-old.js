var extracurricularcourseTable = function () {
    var id = "#extracurricularcourse-datatable";
    var datatable;
    var options = {
        search: {
            input: $("#search")
        },
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ("/admin/cursosextracurriculares/get").proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "code",
                title: "Código",
            }, {
                field: "name",
                title: "Nombre",
            }, {
                field: "price",
                title: "Precio",
            }, {
                field: "credits",
                title: "Créditos",
            }, {
                field: "options",
                title: "Opciones",
                textAlign: "center",
                sortable: false, // disable sort for this column
                filterable: false, // disable or enable filtering
                template: function (row) {
                    var template = "";
                    template += "<button data-toggle='modal' data-target='#edit_extracurricularcourse_modal' data-id='" +
                        row.id +
                        "' class='btn btn-info btn-sm m-btn m-btn--icon btn-edit'><span><i class='la la-edit'></i><span>Editar</span></span></button> ";
                    template += "<button data-id='" + row.id + "' class='btn btn-danger btn-sm m-btn--icon btn-delete'><i class='la la-trash'></i></button>";
                    return template;
                }
            }
        ]
    }
    var events = {
        init: function () {
            $(".btn-edit").on("click",
            function () {
                var dataId = $(this).data("id");
                Form.Edit.show(dataId);
            });
            $(".btn-delete").on("click",
                function () {
                    var dataId = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "El curso extracurricular será eliminada",
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
                                    url: ("/admin/cursosextracurriculares/eliminar/post").proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        id: dataId
                                    },
                                    success: function () {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El curso extracurricular ha sido eliminado con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                    },
                                    error: function () {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Al parecer el curso extracurricular tiene información relacionada"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });
            $('.letters').keypress(function (event) {
                var inputValue = event.which;
                // allow letters and whitespaces only.
                if (!(inputValue >= 65 && inputValue <= 120) && (inputValue != 32 && inputValue != 0)) {
                    event.preventDefault();
                }
            });
        }
    }
    var loadDatatable = function () {
        if (datatable !== undefined)
            datatable.destroy();
        options.data.source.read.url = ("/admin/cursosextracurriculares/get").proto().parseURL();
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
    var modalCreateId = "#add_extracurricularcourse_modal";
    var formCreateId = "#add-extracurricularcourse-form";
    var formCreateValidate;

    var modalEditId = "#edit_extracurricularcourse_modal";
    var formEditId = "#edit-extracurricularcourse-form";
    var formEditValidate;
    var events = {
        init: function () {
            $(".btn-add").on("click",
                function () {

                    $(modalCreateId).one("hidden.bs.modal", function () {
                        form.reset.create();
                    });
                });

        }
    }
    var form = {
        submit: {
            create: function (formElements) {
                var data = $(formElements).serialize();
                $(`${modalCreateId} input, ${modalCreateId} select, ${modalCreateId} textarea`).attr("disabled", true);
                $("#btnCreate").addLoader();
                $.ajax({
                    url: $(formElements).attr("action"),
                    type: "POST",
                    data: data
                }).always(function () {
                    $(`${modalCreateId} input, ${modalCreateId} select, ${modalCreateId} textarea`).attr("disabled", false);
                    $("#btnCreate").removeLoader();
                }).done(function () {
                    $(modalCreateId).modal("toggle");
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    extracurricularcourseTable.reload();
                }).fail(function (e) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (e.responseText != null) $("#add_form_msg_txt").html(e.responseText);
                    else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#add_form_msg").removeClass("m--hide").show();
                });
            },
            edit: function (formElements) {
                var data = $(formElements).serialize();
                $(`${modalEditId} input, ${modalEditId} select, ${modalEditId} textarea`).attr("disabled", true);
                $("#btnEdit").addLoader();
                $.ajax({
                    url: $(formElements).attr("action"),
                    type: "POST",
                    data: data
                }).always(function () {
                    $(`${modalEditId} input, ${modalEditId} select, ${modalEditId} textarea`).attr("disabled", false);
                    $("#btnEdit").removeLoader();
                }).done(function () {
                    $(modalEditId).modal("toggle");
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    extracurricularcourseTable.reload();
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
                formEditValidate.resetForm();
            }
        },
        show: {
            edit: function (id) {
                mApp.blockPage();
                $.ajax({
                    url: `/admin/cursosextracurriculares/${id}/get`.proto().parseURL()
                }).done(function (result) {
                    var formElements = $(formEditId).get(0).elements;
                    formElements["Edit_Id"].value = result.id;
                    formElements["Edit_Name"].value = result.name;
                    formElements["Edit_Code"].value = result.code;
                    formElements["Edit_Price"].value = result.price;
                    formElements["Edit_Credits"].value = result.credits;
                    formElements["Edit_Description"].value = result.description;

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


    var validate = {
        init: function () {
            this.create.init();
            this.edit.init();
        },
        create: {
            init: function () {
                formCreateValidate = $(formCreateId).validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        form.submit.create(formElement);
                    }
                });
            }
        },
        edit: {
            init: function () {
                formEditValidate = $(formEditId).validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        form.submit.edit(formElement);
                    }
                });
            }
        }
    }
    return {
        init: function () {
            validate.init();
            events.init();
        },
        Edit: {
            show: function (id) {
                form.show.edit(id);
            }
        }
    }
}();
$(function () {
    extracurricularcourseTable.init();
    Form.init();
});