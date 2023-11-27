var detail = function () {

    var gradeRecoveryExamId = $("#GradeRecoveryExamId").val();
    var studentsAssigned = [];
    var datatable = {
        students: {
            object: null,
            options: {
                pageLength: 5,
                serverSide: false,
                ajax: {
                    url: `/admin/recuperacion-notas/get-estudiantes/${gradeRecoveryExamId}`,
                    method: 'GET',
                    dataSrc : ""
                },
                columns: [
                    {
                        data: "userName",
                        title: "Código"
                    },
                    {
                        data: "fullName",
                        title: "Estudiante"
                    },
                    {
                        data: "finalGrade",
                        title: "Nota Final"
                    },
                    {
                        data: null,
                        title: "Asignar",
                        render: function (row) {
                            var tmp = "";
                            var checked = row.isAssigned ? "checked" : "";

                            if (checked) {
                                if (!studentsAssigned.includes(row.id)) {
                                    studentsAssigned.push(row.id);
                                }
                            }

                            tmp += `<span class="m-switch m-switch--outline m-switch--icon m-switch--primary">
                                <label>
                                    <input type="checkbox" ${checked} class="students_check" data-id="${row.id}">
                                    <span></span>
                                </label>
                            </span>`;
                            return tmp;
                        }
                    }
                ]
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#students_table").DataTable(this.options);
            }
        },
        assignedStudents: {
            object: null,
            options: {
                ajax: {
                    url: `/admin/recuperacion-notas/get-estudiantes-asignados/${gradeRecoveryExamId}`,
                    method: 'GET',
                    data: function (data) {
                        data.searchValue = $("#search").val();
                    }
                },
                columns: [
                    {
                        data: "userName",
                        title : "Usuario"
                    },
                    {
                        data: "fullName",
                        title : "Estudiante"
                    },
                    {
                        data: "minGrade",
                        title : "Nota a Reemplazar"
                    },
                    {
                        data: "evaluation",
                        title : "Evaluación"
                    }
                ]
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#assigned_students").DataTable(this.options);
            }
        },
        studentsExecuted: {
            object: null,
            options: {
                ajax: {
                    url: `/admin/recuperacion-notas/get-estudiantes-asignados-examen-concluido`,
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
            this.students.init();
            this.studentsExecuted.init();
            this.assignedStudents.init();
        }
    };

    var events = {
        onChangeStudentStatus: function () {
            $("#students_table").on("click", ".students_check", function () {
                $(this).attr("disabled", true);

                var id = $(this).data("id");
                var isChecked = $(this).is(":checked");

                if (isChecked) {
                    if (!studentsAssigned.includes(id)) {
                        studentsAssigned.push(id);
                    }
                } else {
                    var index = studentsAssigned.indexOf(id);
                    if (index > -1) {
                        studentsAssigned.splice(index, 1);
                    }
                }
                $(this).attr("disabled", false);
                
            });
        },
        onAssignStudents: function () {
            $("#assign_students_button").on("click", function () {
                var $btn = $(this);
                $btn.addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                var formData = new FormData();

                formData.append("GradeRecoveryExamId", gradeRecoveryExamId);

                for (var i = 0; i < studentsAssigned.length; i++) {
                    formData.append(`Students[${i}]`, studentsAssigned[i]);
                }

                $.ajax({
                    url: "/admin/recuperacion-notas/asignar-estudiantes",
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false
                })
                    .done(function (data) {
                        datatable.students.reload();
                        datatable.assignedStudents.reload();
                        swal({
                            type: "success",
                            title: "Hecho!",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            confirmButtonText: "Aceptar",
                            text: "Estudiantes asignados satisfactoriamente."
                        });
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
                        $("#assigned_students_modal").modal("hide");
                    });

            });
        },
        onConfirmExam: function () {
            $("#confirm_exam").on("click", function () {
                var $btn = $(this);

                swal({
                    title: "¿Seguro que desea confirmar el exámen?",
                    text: "Ya no se podrá asignar o remover estudiantes",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, confirmar",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: `/admin/recuperacion-notas/confirmar-examen?gradeRecoveryExamId=${gradeRecoveryExamId}`,
                                type: "POST"
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
                                });
                        });
                    }
                });
            });
        },
        onSearch: function () {
            $("#search").doneTyping(function () {
                datatable.assignedStudents.reload();
            });
        },
        init: function () {
            this.onSearch();
            this.onAssignStudents();
            this.onChangeStudentStatus();
            this.onConfirmExam();
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
})