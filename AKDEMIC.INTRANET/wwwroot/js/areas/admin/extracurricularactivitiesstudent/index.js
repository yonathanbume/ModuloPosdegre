var ActivitiesTable = function () {
    var id = "#activities-datatable";
    var datatable;

    var options = {
        ajax: {
            url: "/admin/actividades-extracurriculares-alumnos/get-datatable",
            type: "GET",
            data: function (data) {
                data.search = $("#search").val();
            }
        },
        columns: [
            {
                data: "activityCode",
                title: "Código Actividad"
            },
            {
                data: "activity",
                title: "Actividad"
            },
            {
                data: "name",
                title: "Estudiante"
            },
            {
                data: "credits",
                title: "Créditos"
            },
            {
                data: null,
                title: "Opciones",
                render: function (row) {
                    var template = "";
                    template += "<button data-id='" + row.id + "' class='btn btn-info btn-sm m-btn m-btn--icon btn-edit' data-toggle='modal' data-target='#edit_activity_modal'><span><i class='la la-edit'></i><span>Editar</span></span></button> ";
                    template += "<button data-id='" + row.id + "' class='btn btn-danger btn-sm m-btn--icon btn-delete'><i class='la la-trash'></i></button>";
                    return template;
                }
            }
        ],
    };

    var events = {
        init: function () {
            $("#data-table").on("click", ".btn-delete", function () {
                var dataId = $(this).data("id");
                swal({
                    title: "¿Está seguro?",
                    text: "La actividad extracurricular del estudiante será eliminada",
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
                                url: ("/admin/actividades-extracurriculares-alumnos/eliminar").proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: dataId
                                },
                                success: function () {
                                    datatable.reload();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "La actividad extracurricular del estudiante ha sido eliminada con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function () {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "Error al eliminar la actividad extracurricular del estudiante"
                                    });
                                }
                            });
                        });
                    }
                });
            });

            $("#data-table").on("click", ".btn-edit", function () {
                var dataId = $(this).data("id");
                Form.Edit.show(dataId);
            });

            $("#search").doneTyping(function () {
                loadDatatable();
            })
        }
    };

    var loadDatatable = function () {

        if (datatable == null) {
            datatable = $("#data-table").DataTable(options);
        } else {
            datatable.ajax.reload();
        }
    };

    return {
        init: function () {
            events.init();
            loadDatatable();
        },
        reload: function () {
            datatable.ajax.reload();
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

            $(".input-datepicker").datepicker();
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
                }).fail(function () {
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
                $("#Add_StudentId").val(null).trigger("change.select2");
                $("#Add_ExtracurricularActivityId").val(null).trigger("change.select2");
                formCreateValidate.resetForm();
            },
            edit: function () {
                $("#edit_form_msg").addClass("m--hide").hide();
                $("#Edit_StudentId").val(null).trigger("change.select2");
                $("#Edit_ExtracurricularActivityId").val(null).trigger("change.select2");
                formEditValidate.reset();
            }
        },
        show: {
            edit: function (id) {
                mApp.blockPage();
                $.ajax({
                    url: `/admin/actividades-extracurriculares-alumnos/${id}/get`.proto().parseURL()
                }).done(function (result) {
                    var formElements = $("#edit-activity-form").get(0).elements;
                    formElements["Edit_Id"].value = result.id;

                    $('#Edit_ExtracurricularActivityId').val(result.extracurricularActivityId).trigger('change');

                    if ($('#Edit_StudentId').find("option[value='" + result.studentId + "']").length) {
                        $('#Edit_StudentId').val(result.studentId).trigger('change');
                    } else {
                        var newOption = new Option(result.studentName, result.studentId, true, true);
                        $('#Edit_StudentId').append(newOption).trigger('change');
                    } 

                    $('#Edit_Grade').val(result.grade);
                    $('#Edit_Resolution').val(result.resolution);
                    $('#Edit_EvaluationReportDate').val(result.evaluationReportDate);

                    if (result.urlFile != null) {
                        $("#link_file").removeClass("d-none");
                        $("#link_file").attr("href", `/documentos/${result.urlFile}`)

                    } else {
                        $('#link_file').addClass("d-none");
                    }

                    mApp.unblockPage();
                    $(modalEditId).one("hidden.bs.modal", function () {
                        form.reset.edit();
                    });
                }).fail(function (error) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            }
        }
    };

    var select = {
        modal: {
            init: function () {

                $.ajax({
                    url: (`/extracurricular-activities/get`).proto().parseURL()
                }).done(function (data) {

                    $("#Add_ExtracurricularActivityId").select2({
                        placeholder: "Seleccione una actividad",
                        dropdownParent: $("#add_activity_modal"),
                        minimumResultsForSearch: 1,
                        data: data.items
                    });

                    $("#Edit_ExtracurricularActivityId").select2({
                        placeholder: "Seleccione una actividad",
                        dropdownParent: $("#edit_activity_modal"),
                        minimumResultsForSearch: 1,
                        data: data.items
                    });
                });

                $("#Add_StudentId").select2({
                    width: "100%",
                    placeholder: "Seleccione un estudiante",
                    dropdownParent: $("#add_activity_modal"),
                    ajax: {
                        url: "/academico/alumnos/buscar".proto().parseURL(),
                        dataType: "json",
                        data: function (params) {
                            return {
                                term: params.term,
                                page: params.page
                            };
                        },
                        processResults: function (data, params) {
                            return {
                                results: data.items
                            };
                        },
                        cache: true
                    },
                    escapeMarkup: function (markup) {
                        return markup;
                    },
                    minimumInputLength: 3
                });
                $("#Edit_StudentId").select2({
                    width: "100%",
                    placeholder: "Seleccione un estudiante",
                    dropdownParent: $("#edit_activity_modal"),
                    ajax: {
                        url: "/academico/alumnos/buscar".proto().parseURL(),
                        dataType: "json",
                        data: function (params) {
                            return {
                                term: params.term,
                                page: params.page
                            };
                        },
                        processResults: function (data, params) {
                            return {
                                results: data.items
                            };
                        },
                        cache: true
                    },
                    escapeMarkup: function (markup) {
                        return markup;
                    },
                    minimumInputLength: 3
                });
            }
        }
    };

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

    return {
        init: function () {
            events.init();
            validate.init();
            select.modal.init();
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