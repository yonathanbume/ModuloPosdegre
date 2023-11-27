var RequestsTable = function () {

    var dataTable;
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: null
                }
            }
        },
        columns: [
            {
                field: "course",
                title: "Curso"
            },
            {
                field: "section",
                title: "Sección"
            },
            {
                field: "code",
                title: "Código"
            },
            {
                field: "student",
                title: "Estudiante"
            },
            {
                field: "evaluation",
                title: "Evaluación"
            },
            {
                field: "grade",
                title: "Nueva Nota"
            },
            {
                field: "state",
                title: "Estado",
                sortable: false,
                filterable: false,
                template: function (row) {
                    switch (row.state) {
                    case 1:
                        return "<span class='m-badge m-badge--metal m-badge--wide'> Pendiente </span>";
                    case 2:
                        return "<span class='m-badge m-badge--primary m-badge--wide'> Aprobado </span>";
                    case 3:
                        return "<span class='m-badge m-badge--danger m-badge--wide'> Rechazado </span>";
                    case 4:
                        return "<span class='m-badge m-badge--warning m-badge--wide'> Solicitado</span>";
                    }
                }
            },
            {
                field: "options",
                title: "Opciones",
                sortable: false,
                filterable: false,
                template: function (row) {
                    var tpm = "-";
                    if (row.state == 4 && row.requestedByStudent) {
                        tpm = `<button data-object="${row.proto().encode()}" type="button" title="Asignar nota" class="btn btn-primary m-btn m-btn--icon btn-sm m-btn--icon-only btn-assign-student-request"><i class="la la-edit"></i></button>`;
                    }
                    return tpm;
                }
            }
        ]
    }

    return {
        loadTable: function (id) {
            options.data.source.read.url = ("/profesor/correccion-notas/get/" + id).proto().parseURL();
            if (dataTable !== undefined) dataTable.destroy();
            dataTable = $(".m-datatable").mDatatable(options);
        },
        reloadTable: function() {
            dataTable.reload();
        }
    }

}();
var TermSelect = function () {
    return {
        init: function () {
            $.ajax({
                url: "/periodos/get".proto().parseURL(),
            }).done(function (data) {
                $("#select_term").select2({
                    data: data.items
                });

                if (data.selected !== null) {
                    $("#select_term").val(data.selected);
                    $("#select_term").trigger("change.select2");
                }

                RequestsTable.loadTable($("#select_term").val());

                $("#select_term").on("change", function (e) {
                    if ($("#select_term").val() != null) {
                        RequestsTable.loadTable($("#select_term").val());
                    }
                });
            });
        }
    }
}();
var SelectGroup = function () {
    var courseSelect;
    var sectionSelect;
    var studentSelect;
    var gradeSelect;
    return {
        courseInit: function () {
            $.ajax({
                url: "/profesor/correccion-notas/cursos/get".proto().parseURL()
            }).done(function (data) {
                courseSelect = $("#courses").select2({
                    minimumResultsForSearch: -1,
                    placeholder: "Seleccione un curso",
                    data: data.items,
                    dropdownParent: $("#request_modal")
                });

                sectionSelect = $("#section").select2({
                    minimumResultsForSearch: -1,
                    placeholder: "Seleccione una sección",
                    dropdownParent: $("#request_modal")
                });

                studentSelect = $("#student").select2({
                    minimumResultsForSearch: -1,
                    placeholder: "Seleccione un estudiante",
                    dropdownParent: $("#request_modal")
                });

                gradeSelect = $("#GradeId").select2({
                    minimumResultsForSearch: -1,
                    placeholder: "Seleccione una evaluación",
                    dropdownParent: $("#request_modal")
                });

                $("#courses").on("change", function (e) {
                    if ($("#courses").val() != null) {
                        SelectGroup.sectionList($("#courses").val());
                    }
                });

                $("#section").on("change", function (e) {
                    if ($("#section").val() != null) {
                        SelectGroup.studentList($("#section").val());
                    }
                });

                $("#student").on("change", function (e) {
                    if ($("#student").val() != null) {
                        SelectGroup.gradeList($("#student").val());
                    }
                });
            });
        },
        sectionList: function (id) {
            $.ajax({
                url: ("/profesor/correccion-notas/secciones/get/" + id).proto().parseURL()
            }).done(function (data) {
                sectionSelect.empty();
                sectionSelect.select2({
                    minimumResultsForSearch: -1,
                    placeholder: "Seleccione una sección",
                    data: data.items
                });
                sectionSelect.val(null).trigger("change");

                studentSelect.empty();
                gradeSelect.empty();
            });
        },
        studentList: function (id) {
            $.ajax({
                url: ("/profesor/correccion-notas/alumnos/get/" + id).proto().parseURL()
            }).done(function (data) {
                studentSelect.empty();
                studentSelect.select2({
                    dropdownParent : $("#request_modal"),
                    //minimumResultsForSearch: -1,
                    placeholder: "Seleccione un estudiante",
                    data: data.items
                });
                studentSelect.val(null).trigger("change");

                gradeSelect.empty();
            });
        },
        gradeList: function (id) {
            gradeSelect.empty();

            $.ajax({
                url: ("/profesor/correccion-notas/notas/get/" + id).proto().parseURL()
            }).done(function (data) {
                gradeSelect.select2({
                    minimumResultsForSearch: -1,
                    placeholder: "Seleccione una evaluación",
                    data: data.items
                });
                gradeSelect.val(null).trigger("change");
            });
        },
        resetSelects: function () {
            courseSelect.val(null).trigger("change.select2");
            sectionSelect.empty().trigger("change.select2");
            studentSelect.empty().trigger("change.select2");
            gradeSelect.empty().trigger("change.select2");
        }
    }
}();

var InitApp = function () {
    var form = {
        object: $("#create-form").validate({
            submitHandler: function (e) {
                mApp.block("#request_modal .modal-content");

                var formData = new FormData($(e)[0]);

                $.ajax({
                    url: $(e).attr("action"),
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false
                }).done(function () {
                    $(".modal").modal("hide");
                    $(".m-alert").addClass("m--hide");

                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                    RequestsTable.reloadTable();
                    form.reset();
                    SelectGroup.resetSelects();
                }).fail(function (error) {
                    if (error.responseText !== null && error.responseText !== "") $("#create-form-alert-txt").html(error.responseText);
                    else $("#create-form-alert-txt").html(_app.constants.ajax.message.error);

                    $("#create-form-alert").removeClass("m--hide").show();
                }).always(function () {
                    mApp.unblock("#request_modal .modal-content");
                });
            }
        }),
        reset: function () {
            form.object.resetForm();
        }
    };

    var formStudentRequest = {
        object: $("#request_student_form").validate({
            submitHandler: function (e) {
                mApp.block("#request_student_modal .modal-content");

                var formData = new FormData($(e)[0]);

                $.ajax({
                    url: $(e).attr("action"),
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false
                }).done(function () {
                    $("#request_student_modal").modal("hide");
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    RequestsTable.reloadTable();
                }).fail(function (error) {
                    swal({
                        type: "error",
                        title: "Error",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        confirmButtonText: "Aceptar",
                        text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                    });
                }).always(function () {
                    mApp.unblock("#request_student_modal .modal-content");
                });
            }
        }),
        events: {
            init: function () {
                $(".btn-student-request-reject").on("click", function () {
                    $("#request_student_form").attr("action", "/profesor/correccion-notas/denegar-solicitud-estudiante");
                    $("#request_student_form").find("[name='NewGrade']").attr("required",false);
                    $("#request_student_form").submit();
                });

                $(".btn-student-request-accept").on("click", function () {
                    $("#request_student_form").attr("action", "/profesor/correccion-notas/aceptar-solicitud-estudiante");
                    $("#request_student_form").find("[name='NewGrade']").attr("required", true);
                    $("#request_student_form").submit();
                });
            }
        },
        init: function () {
            this.events.init();
        }
    };

    return {
        init: function () {
            formStudentRequest.init();
        },
        reset: function() {
            form.reset();
        }
    }
}();

$(function () {
    TermSelect.init();
    SelectGroup.courseInit();
    InitApp.init();

    $("#NotTaken").on("change", function () {
        console.log("asdasd");
        if ($(this).is(":checked")) {
            $("#create-form").find("[name='NewGrade']").val(null).trigger("change").attr("disabled", true);
        } else {
            $("#create-form").find("[name='NewGrade']").val(null).trigger("change").attr("disabled", false);
        }
    })

    $("select").on("select2:close", function (e) {
        $(this).valid();
    });

    $("#request_modal").on("hidden.bs.modal", function () {
        InitApp.reset();
        SelectGroup.resetSelects();
        $("#create_msg").addClass("m--hide");
    })

    $("#ajax_data").on("click", ".btn-assign-student-request", function () {
        var data = $(this).data("object").proto().decode();

        $("#request_student_modal").find("[name='CourseFullName']").val(`${data.code}-${data.course}`);
        $("#request_student_modal").find("[name='Section']").val(data.section);
        $("#request_student_modal").find("[name='StudentFullName']").val(data.student);
        $("#request_student_modal").find("[name='Evaluation']").val(`${data.evaluation}- NOTA : ${data.oldGrade}`);
        $("#request_student_modal").find("[name='NewGrade']").val(null);
        $("#request_student_modal").find("[name='Observations']").val(data.observations);
        $("#request_student_modal").find("[name='Id']").val(data.id);

        if (data.filePath != "" && data.filePath != null) {
            $("#request_student_modal").find(".grade_correction_file_container").removeClass("d-none");
            $("#request_student_modal").find(".grade_correction_file_path").attr("href", `/documentos/${data.filePath}`);

        } else {
            $("#request_student_modal").find(".grade_correction_file_container").addClass("d-none");
            $("#request_student_modal").find(".grade_correction_file_path").attr("href", "");
        }

        $("#request_student_modal").modal("show");
    })
});