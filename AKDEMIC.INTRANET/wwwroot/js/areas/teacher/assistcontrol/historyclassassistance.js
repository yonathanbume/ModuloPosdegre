var Assistances = function () {
    var datatable;
    var formValidate;

    var options = {
        search: {
            input: $("#search"),
        },
        data: {
            type: "remote",
            source: {
                read: {
                    method: "GET",
                    url: `/profesor/asistencia/get?classId=${$("#classId").val()}`.proto().parseURL()
                }
            },
            pageSize: 10,
            saveState: {
                cookie: true,
                webstorage: true,
            },
        },
        layout: {
            footer: true
        },
        pagination: false,
        columns: [
            {
                field: "studentName",
                title: "Nombre",
                width: 260,
                template: function (row) {
                    var tmp = "";
                    if (row.absences > row.maxAbsences) {
                        tmp += "<p style='color:red;'>";
                        tmp += row.studentName;
                        tmp += "</p>";
                    } else
                        tmp += row.studentName;
                    return tmp;
                }
            },
            {
                field: "maxAbsences",
                title: "Máx. Faltas",
                textAlign: "center",
                width: 120,
                template: function (row) {
                    var tmp = "";
                    if (row.absences > row.maxAbsences) {
                        tmp += "<p style='color:red;'>";
                        tmp += row.maxAbsences;
                        tmp += "</p>";
                    } else
                        tmp += row.maxAbsences;
                    return tmp;
                }
            },
            {
                field: "absences",
                title: "Faltas",
                textAlign: "center",
                width: 120,
                template: function (row) {
                    var tmp = "";
                    if (row.absences > row.maxAbsences) {
                        tmp += "<p style='color:red;'>";
                        tmp += row.absences;
                        tmp += "</p>";
                    } else
                        tmp += row.absences;
                    return tmp;
                }
            },
            {
                field: "status",
                title: "Inasistencia",
                width: 250,
                sortable: false,
                overflow: "inherit",
                textAlign: "center",
                template: function (row, index) {
                    var tmp = "";
                    tmp += `<input  hidden name='assists.List[${index}].ClassStudentId' value='${row.id}' />`;
                    tmp +=
                        "<label class='m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand'>";
                    tmp += `<input disabled id='input_${row.id}' type='checkbox' name='assists.List[${index}].IsAbsent' value='true' ${row.isAbsent
                        ? "checked"
                        : ""}/>`;
                    tmp += `<input name='assists[${index}].IsAbsent' type='hidden' value='false' />`;
                    tmp += `<input hidden name='assists.List[${index}].StudentId' value='${row.studentId}' />`;
                    tmp += "<span></span></label>";

                    return tmp;
                }
            }
        ]
    };

    var form = {
        begin: function () {
            $("#form_msg").addClass("m--hide").hide();
            $("#form_msg_txt").html("");

            $("select").attr("disabled", true);
            $("#btnAssists").addLoader();
        },
        complete: function () {
            $("select").attr("disabled", false);
            $("#btnAssists").removeLoader();
        },
        success: function (e) {
            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
        },
        failure: function (e) {
            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
            if (e.responseText != null) $("#form_msg_txt").html(e.responseText);
            else $("#form_msg_txt").html(_app.constants.toastr.message.error.task);
            $("#form_msg").removeClass("m--hide").show();
        }
    };

    var select2 = {
        init: function () {
            this.activities.init();
            this.virtualClass.init();
        },
        activities: {
            init: function () {
                $.ajax({
                    url: `/cursos/${$("#CourseId").val()}/temario/get`
                }).done(function (data) {
                    $(".select2-activities").select2({
                        data: data.items,
                        placeholder: "Selecciona un tema de clase",
                        dropdownParent: $("#frm-assistance"),
                        disabled: !EnableUpdateClass
                    }).val($("#defaultActivityId").val()).trigger("change");
                });
            }
        },
        virtualClass: {
            init: function () {

                $.ajax({
                    url: `/get-videoconferencias-select?sectionId=${$("#SectionId").val()}`
                }).done(function (e) {
                    $(".select2-videoconferences").select2({
                        data: e,
                        placeholder: "Selecciona un tema de videoconferencias"
                    });

                    $("#VirtualClassId").val($("#defaultVirtualClassId").val()).trigger("change");
                });
            }
        }
    };

    var events = {
        onChange: function () {
            $("#Class_HasVirtualClass").on("change", function () {
                if ($(this).is(":checked")) {
                    $("#div_virtualclass").removeClass("d-none");
                    $("#VirtualClassId").val($("#defaultVirtualClassId").val()).trigger("change");
                    $("#VirtualClassId").attr("required", true);
                } else {
                    $("#div_virtualclass").addClass("d-none");
                    $("#VirtualClassId").val(null).trigger("change");
                    $("#VirtualClassId").attr("required", false);
                }
            })
        },
        init: function () {
            this.onChange();
        }
    };

    var validate = {
        init: function () {
            formValidate = $("#frm-assistance").validate();
        }
    };

    return {
        init: function () {
            datatable = $("#assistance-datatable").mDatatable(options);
            select2.init();
            validate.init();
            events.init();
        },
        Form: {
            begin: function () {
                form.begin();
            },
            complete: function () {
                form.complete();
            },
            success: function (e) {
                form.success(e);
            },
            failure: function (e) {
                form.failure(e);
            }
        }
    }
}();

$(function () {
    Assistances.init();
});