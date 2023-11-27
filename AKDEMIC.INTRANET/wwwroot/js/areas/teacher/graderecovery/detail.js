var detail = function () {

    var gradeRecoveryExamId = $("#GradeRecoveryExamId").val();

    var datatable = {
        studentsAssigned: {
            object: null,
            options: {
                ajax: {
                    url: `/profesor/examenes-recuperacion-nota/get-estudiantes-asignados`,
                    data: function (data) {
                        data.gradeRecoveryExamId = gradeRecoveryExamId;
                    },
                    method: 'GET',
                    dataSrc: ""
                },
                bPaginate: false,
                bLengthChange: false,
                bFilter: true,
                bInfo: false,
                columns: [
                    {
                        data: "userName",
                        title: "Usuario"
                    },
                    {
                        data: "fullName",
                        title: "Estudiante"
                    },
                    {
                        data: "minGrade",
                        title: "Nota a Reemplazar"
                    },
                    {
                        data: "evaluation",
                        title: "Evaluación"
                    },
                    {
                        data: null,
                        title: "¿Ausente?",
                        render: function (row) {
                            var tpm = "";
                            tpm += `<span class="m-switch m-switch--outline m-switch--icon m-switch--primary m-switch--sm">
                                <label>
                                    <input type="checkbox" class="students_check check_${row.studentId}" data-id="${row.studentId}">
                                    <span></span>
                                </label>
                            </span>`;
                            return tpm;
                        }
                    },
                    {
                        data: null,
                        title: "Nota del Exámen",
                        render: function (row) {
                            var tpm = "";
                            tpm += `<input type='number' min='0' max='20' step='1' required data-id="${row.studentId}" class="form-control input-sm m-input input_student input_${row.studentId}">`;
                            return tpm;
                        }
                    }
                ]
            },
            init: function () {
                this.object = $("#assigned_students").DataTable(this.options);
            }
        },
        studentsExecuted: {
            object : null,
            options: {
                ajax: {
                    url: `/profesor/examenes-recuperacion-nota/get-estudiantes-asignados-examen-concluido`,
                    data: function (data) {
                        data.gradeRecoveryExamId = gradeRecoveryExamId;
                    },
                    method: 'GET',
                    dataSrc: ""
                },
                bPaginate: false,
                bLengthChange: false,
                bFilter: true,
                bInfo: false,
                columns: [
                    {
                        data: "userName",
                        title: "Usuario"
                    },
                    {
                        data: "fullName",
                        title: "Estudiante"
                    },
                    {
                        data: "prevFinalScore",
                        title: "Nota Reemplazada"
                    },
                    {
                        data: "evaluation",
                        title: "Evaluación"
                    },
                    {
                        data: "examScore",
                        title: "Nota Obtenida"
                    },

                ]
            },
            init: function () {
                this.object = $("#assigned_students_executed").DataTable(this.options);
            }
        },
        init: function () {
            this.studentsAssigned.init();
            this.studentsExecuted.init();
        }
    };

    var events = {
        onChangeStatus: function () {
            $("#assigned_students").on("change", ".students_check", function () {
                var id = $(this).data("id");
                if ($(this).is(":checked")) {
                    $("#assigned_students").find(`.input_${id}`).val(0);
                    $("#assigned_students").find(`.input_${id}`).attr("disabled", true);
                } else {
                    $("#assigned_students").find(`.input_${id}`).attr("disabled", false);
                    $("#assigned_students").find(`.input_${id}`).val("");
                }
            });
        },
        onSave: function () {
            $("#save_grades").on("click", function () {
                var $btn = $(this);
                $btn.addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);

                var formData = new FormData();
                formData.append("GradeRecoveryExam", gradeRecoveryExamId);

                var students = $("#assigned_students").find(".input_student");

                for (var i = 0; i < students.length; i++) {
                    var studentId = $(students[i]).data("id");

                    var isAbsent = $("#assigned_students").find(`.check_${studentId}`).is(":checked");
                    var grade = $("#assigned_students").find(`.input_${studentId}`).val();

                    formData.append(`Students[${i}].StudentId`, studentId);
                    formData.append(`Students[${i}].Value`, grade);
                    formData.append(`Students[${i}].IsAbsent`, isAbsent);
                }

                $.ajax({
                    url: "/profesor/examenes-recuperacion-nota/guardar-notas",
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false
                })
                    .done(function () {
                        window.location.reload();
                    })
                    .fail(function (e) {
                        $btn.removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                        swal({
                            type: "error",
                            title: "Error",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            confirmButtonText: "Aceptar",
                            text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                        });
                    })
                    .always(function () {
                        $btn.removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                    });

            });
        },
        init: function () {
            this.onChangeStatus();
            this.onSave();
        }
    };

    return {
        init: function () {
            datatable.init();
            events.init();
        }
    };
}();

$(() => {
    detail.init();
});