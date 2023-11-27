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
                template: function (row) {
                    var tmp = "";
                    if (row.dpi) {
                        tmp += "<p style='color:red; font-weight: bold;'>";
                        tmp += `${row.studentName} (DPI)`;
                        tmp += "</p>";
                    } else
                        tmp += row.studentName;
                    return tmp;
                }
            },
            {
                field: "maxAbsences",
                title: "% Faltas",
                textAlign: "center",
                width: 120,
                template: function (row) {
                    var tmp = "";
                    if (row.dpi) {
                        tmp += "<p style='color:red; font-weight: bold;'>";
                        tmp += `${row.absencePercentage}%`;
                        tmp += "</p>";
                    } else
                        tmp += `${row.absencePercentage}%`;
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
                    if (row.dpi) {
                        tmp += "<p style='color:red; font-weight: bold;'>";
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
                    tmp += `<input hidden name='assists.List[${index}].ClassStudentId' value='${row.id}' />`;
                    tmp +=
                        `<label class='m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand' ${row.dpi? "style = 'cursor:not-allowed'":""}>`;
                    tmp += `<input id='input_${row.id}' type='checkbox' name='assists.List[${index}].IsAbsent' value='true' ${row.dpi ? "disabled" : ""} ${row.isAbsent
                        ? "checked"
                        : ""}/>`;
                    tmp += `<input name='assists.List[${index}].DPI' type='hidden' value='${row.dpi}' />`;
                    tmp += `<input name='assists[${index}].IsAbsent' type='hidden' value='false' />`;
                    tmp += `<input hidden name='assists.List[${index}].StudentId' value='${row.studentId}' />`;
                    tmp += "<span></span></label>";

                    return tmp;
                }
            }
        ]
    };

    var form = {
        objet: $("#frm-assistance").validate({
            submitHandler: function (formElement, e) {
                e.preventDefault();
                var formData = new FormData(formElement);
                $("#form_msg").addClass("m--hide").hide();
                $("#form_msg_txt").html("");
                $("#frm-assistance").find(":input").attr("disabled", true);
                $("#btnAssists").addLoader();

                $.ajax({
                    url: $(formElement).attr("action"),
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false
                })
                    .done(function (data) {
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        window.location.href = `/profesor/reporte_asistencia/asistencias/${$("#SectionId").val()}`.proto().parseURL();
                    })
                    .fail(function (e) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (e.responseText != null) $("#form_msg_txt").html(e.responseText);
                        else $("#form_msg_txt").html(_app.constants.toastr.message.error.task);
                        $("#form_msg").removeClass("m--hide").show();
                    })
                    .always(function () {
                        $("#btnAssists").removeLoader();
                        $("#frm-assistance").find(":input").attr("disabled", false);
                    });
            }
        })
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
                        allowClear: true,
                        placeholder: "Selecciona un tema de clase",
                        dropdownParent: $("#frm-assistance")
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
                        allowClear : true,
                        placeholder: "Selecciona un tema de videoconferencias"
                    });

                    $("#VirtualClassId").val(null).trigger("change");
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

    return {
        init: function () {
            datatable = $("#assistance-datatable").mDatatable(options);
            select2.init();
            events.init();
        }
    }
}();

$(function () {
    Assistances.init();
});