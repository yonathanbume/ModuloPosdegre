let Datatable = function () {
    let id = '#datatable';
    let datatable;

    let options = {
        ajax: {
            data: function (data, settings) {
                data.searchValue = $('#search').val();
            },
            url: '/admin/feriados/listar'.proto().parseURL()
        },
        columns: [
            {
                data: "name",
                title: "Nombre"
            },
            {
                data: "date",
                title: "Fecha"
            },
            {
                data: null,
                title: "Tipo",
                render: function (data, type, row) {
                    return row.type === 0 ? "Nacional" : "Universitario";
                }
            },
            {
                data: "options",
                title: "Opciones",
                className: "text-center",
                orderable: false,
                render: function (data, type, row) {
                    let template = "";
                    template += "<button data-id='" + row.id + "' class='btn btn-info btn-sm m-btn m-btn--icon btn-edit' data-toggle='modal' data-target='#edit-modal'><span><i class='la la-edit'></i>Editar</span></button> ";
                    template += "<button data-id='" + row.id + "' class='btn btn-danger btn-sm m-btn--icon btn-delete'><i class='la la-trash'></i></button>";
                    return template;
                }
            }
        ]
    };

    let events = {
        init: function () {
            $(id).on("click", ".btn-edit", function () {
                let dataId = $(this).data("id");
                Form.Edit.show(dataId);
            });
            $(id).on("click", ".btn-delete", function () {
                let dataId = $(this).data("id");
                swal({
                    title: "¿Está seguro?",
                    text: "El feriado será eliminado",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise(() => {
                            $.ajax({
                                url: "/admin/feriados/eliminar".proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: dataId
                                },
                                success: function (e) {
                                    datatable.ajax.reload();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: e,
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function (e) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                    });
                                }
                            });
                        });
                    }
                });
            });
        }
    };

    let select2 = {
        init: function () {
            $.when(
                $.ajax({
                    url: `/tipos-feriado`.proto().parseURL()
                })
            ).then(function (data) {
                $("#Add_Type").select2({
                    placeholder: "Seleccione tipo feriado",
                    minimumResultsForSearch: -1,
                    allowClear: true,
                    data: data.items,
                    dropdownParent: $("#add-form")
                });
                $('#Add_Type').val('').trigger('change');
                $("#Edit_Type").select2({
                    placeholder: "Seleccione tipo feriado",
                    minimumResultsForSearch: -1,
                    allowClear: true,
                    data: data.items,
                    dropdownParent: $("#edit-form")
                });
                $('#Edit_Type').val('').trigger('change');
            });
        }
    };

    var datepicker = {
        init: function () {
            $(".datepicker-input").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            });

        }
    };

    let loadDatatable = function () {
        $("#search").doneTyping(function () {
            datatable.ajax.reload();
        });
        datatable = $(id).DataTable(options);
        events.init();
        select2.init();
        datepicker.init();
    };

    return {
        init: function () {
            loadDatatable();
        },
        reload: function () {
            datatable.ajax.reload();
        }
    };
}();

let Form = function () {

    let modalCreateId = "#add-modal";
    let formCreateId = "#add-form";
    let formCreateValidate;

    let modalEditId = "#edit-modal";
    let formEditId = "#edit-form";
    let formEditValidate;

    let events = {
        init: function () {
            $(modalCreateId).on("hidden.bs.modal", function () {
                form.reset.create();
            });
        }
    };

    let form = {
        submit: {
            create: function (formElements) {
                let formData = new FormData($(formElements)[0]);
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
                    $(modalCreateId).modal('hide');
                    swal({
                        type: "success",
                        title: "Completado",
                        text: "El feriado se creó correctamente",
                        confirmButtonText: "Excelente"
                    });
                    Datatable.reload();
                }).fail(function (e) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (e.responseText !== null) $("#add_form_msg_txt").html(e.responseText);
                    else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#add_form_msg").removeClass("m--hide").show();
                });
            },
            edit: function (formElements) {
                let formData = new FormData($(formElements)[0]);
                $("#btnEdit").addLoader();
                $.ajax({
                    url: $(formElements).attr("action"),
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false
                }).always(function () {
                    $("#btnEdit").removeLoader();
                }).done(function () {
                    $(modalEditId).modal('hide');
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    Datatable.reload();
                }).fail(function (e) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (e.responseText !== null) $("#edit_form_msg_txt").html(e.responseText);
                    else $("#edit_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#edit_form_msg").removeClass("m--hide").show();
                });
            }
        },
        reset: {
            create: function () {
                $("#add_form_msg").addClass("m--hide").hide();
                $('#Add_Type').val('').trigger('change');
                $(".datepicker-input").datepicker('update', '');
                formCreateValidate.resetForm();
            },
            edit: function () {
                $("#edit_form_msg").addClass("m--hide").hide();
                $('#Edit_Type').val('').trigger('change');
                $(".datepicker-input").datepicker('update', '');
                formEditValidate.reset();
            }
        },
        show: {
            edit: function (id) {
                mApp.blockPage();
                $.ajax({
                    url: `/admin/feriados/${id}`.proto().parseURL()
                }).done(function (result) {
                    let formElements = $("#edit-form").get(0).elements;
                    console.log(result);
                    formElements["Edit_Id"].value = result.id;
                    formElements["Edit_Name"].value = result.name;
                    formElements["Edit_Date"].value = result.date;
                    $(formElements["Edit_Date"]).attr("disabled", true);
                    $(formElements["Edit_Type"]).attr("disabled", true);
                    $(formElements["Edit_NeedReschedule"]).prop('checked', result.needReschedule);
                    $(formElements["Edit_NeedReschedule"]).attr('disabled', true);
                    $('#Edit_Type').val(result.type).trigger('change');
                    mApp.unblockPage();
                    $(modalEditId).on("hidden.bs.modal", function () {
                        form.reset.edit();
                    });
                }).fail(function () {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            }
        }
    };

    let validateRules = {
        Add_Name: {
            required: true,
            maxlength: 100,
        },
        Edit_Name: {
            required: true,
            maxlength: 100
        },
        Add_Type: {
            required: true
        },
        Edit_Type: {
            required: true
        },
        Add_Date: {
            required: true
        },
        Edit_Date: {
            required: true
        }
    };

    let validate = {
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
    };
    return {
        init: function () {
            events.init();
            validate.init();
        },
        Edit: {
            show: function (id) {
                form.show.edit(id);
            }
        }
    };
}();

$(function () {
    Datatable.init();
    Form.init();
});