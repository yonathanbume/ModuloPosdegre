var index = function () {

    var datatable = {
        student: {
            object: null,
            options: {
                ajax: {
                    url: `/admin/actualizar-nota-estudiante/get-courses-datatable`,
                    type: "GET",
                    data: function (data) {
                        data.termId = $("#term_select").val();
                        data.studentId = $("#student_select").val();
                        data.type = $("#academic_history_type").val();
                    }
                },
                columns: [
                    {
                        data: "courseCode",
                        title :" Código"
                    },
                    {
                        data: "courseName",
                        title : "Curso"
                    },
                    {
                        data: "section",
                        title : "Sección"
                    },
                    {
                        data: "grade",
                        title : "Nota"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        render: function (row) {
                            var tpm = "";
                            if (row.isRegular) {
                                tpm += `<button data-id="${row.id}" class="btn btn-view-grades btn-sm btn-primary m-btn m-btn--custom m-btn--icon"><span><i class="la la-eye"></i><span>Ver Notas</span></span></button>`;
                            }
                            else {
                                tpm += `<button data-id="${row.id}" data-grade="${row.grade}" data-course="${row.courseCode}-${row.courseName}" class="btn btn-history btn-sm btn-primary m-btn m-btn--custom m-btn--icon"><span><i class="la la-edit"></i><span>Editar nota</span></span></button>`;
                            }
                            return tpm;
                        }
                    }
                ]
            },
            reload: function () {
                if (datatable.student.object == null) {
                    datatable.student.object = $("#datatable_courses").DataTable(datatable.student.options);
                } else {
                    datatable.student.object.ajax.reload();
                }
            },
            events: {
                onViewGrades: function () {
                    $("#datatable_courses").on("click", ".btn-view-grades", function () {
                        var id = $(this).data("id");
                        modal.viewGrades.events.show(id);
                    })
                },
                onViewHistory: function () {
                    $("#datatable_courses").on("click", ".btn-history", function () {
                        var id = $(this).data("id");
                        var grade = $(this).data("grade");
                        var course = $(this).data("course");
                        modal.viewHistory.events.show(id, grade, course);
                    })
                },
                init: function () {
                    this.onViewGrades();
                    this.onViewHistory();
                }
            },
            init: function () {
                this.events.init();
            }
        },

        grades: {
            object: null,
            options: {
                ajax: {
                    url: `/admin/actualizar-nota-estudiante/get-notas-datatable`,
                    type: "GET",
                    data: function (data) {
                        data.academicHistoryId = $("#Current_Grades_AcademicHistoryId").val();
                    }
                },
                columns: [
                    {
                        data: "name",
                        title: " Evaluación"
                    },
                    {
                        data: "value",
                        title: "Nota"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        render: function (row) {
                            var tpm = "";
                            tpm += `<button type="button" data-id="${row.id}" data-fullname="${row.fullName}" data-evaluation="${row.name}-${row.value}" class="btn-show-form btn btn-sm btn-primary m-btn m-btn--custom m-btn--icon"><span><i class="la la-edit"></i><span>Modificar</span></span></button>`;
                            return tpm;
                        }
                    }
                ]
            },
            events: {
                onShowForm: function () {
                    $("#datatable_grades").on("click", ".btn-show-form", function () {
                        var fullName = $(this).data("fullname");
                        var evaluation = $(this).data("evaluation");
                        var id = $(this).data("id");

                        modal.viewGrades.events.showForm(id, fullName, evaluation);
                    })
                },
                init: function () {
                    this.onShowForm();
                }
            },
            reload: function () {
                if (datatable.grades.object == null) {
                    datatable.grades.object = $("#datatable_grades").DataTable(datatable.grades.options);
                } else {
                    datatable.grades.object.ajax.reload();
                }
            },
            init: function () {
                this.events.init();
            }
        },
        init: function () {
            datatable.student.init();
            datatable.grades.init();
        }
    }

    var select = {
        term: {
            init: function () {
                $.ajax({
                    url: `/periodos/get`
                })
                    .done(function (e) {
                        $("#term_select").select2({
                            data: e.items,
                            placeholder: "Seleccionar periodo académico"
                        });

                        if (e.selected != null) {
                            $("#term_select").val(e.selected).trigger("change");
                        }
                    })
            }
        },
        student: {
            init: function () {
                $("#student_select").select2({
                    width: "100%",
                    placeholder: "Buscar...",
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
        },
        academic_history_type: {
            init: function () {
                $("#academic_history_type").select2({
                    minimumResultsForSearch: -1
                });
            }
        },
        init: function () {
            select.term.init();
            select.student.init();
            select.academic_history_type.init();
        }
    }

    var events = {
        onSearch: function () {
            $("#btn_search").click(function () {
                datatable.student.reload();
            })
        },
        init: function () {
            this.onSearch();
        }
    }

    var modal = {
        viewGrades: {
            form: {
                object: $("#form_grade").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        var formData = new FormData(formElement);
                        $(".btn-submit-grades").addLoader();

                        $.ajax({
                            url: "/admin/actualizar-nota-estudiante/actualizar-nota-estudiante",
                            method: "POST",
                            data: formData,
                            contentType: false,
                            processData: false,
                        })
                            .done(function (e) {
                                modal.viewGrades.object.modal("hide");
                                datatable.grades.reload();
                                datatable.student.reload();
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "Nota actualizada con éxito",
                                    confirmButtonText: "Excelente"
                                });
                            })
                            .fail(function (e) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                });
                            })
                            .always(function () {
                                $(".btn-submit-grades").removeLoader();
                            });

                    }
                })
            },
            object: $("#modal_grades"),
            events: {
                show: function (academicHistoryId) {
                    $("#Current_Grades_AcademicHistoryId").val(academicHistoryId);
                    datatable.grades.reload();
                    modal.viewGrades.object.modal("show");
                },
                showForm: function (gradeId, fullName, evaluation) {
                    $("#container_form_grades_academicHistory").removeClass("d-none");
                    $("#container_datatable_grades").addClass("d-none");

                    $("#form_grade").find("[name='StudentFUllName']").val(fullName);
                    $("#form_grade").find("[name='Evaluation']").val(evaluation);
                    $("#form_grade").find("[name='Id']").val(gradeId);

                },
                onHidden: function () {
                    modal.viewGrades.object.on('hidden.bs.modal', function (e) {
                        $("#container_form_grades_academicHistory").addClass("d-none");
                        $("#container_datatable_grades").removeClass("d-none");
                        modal.viewGrades.form.object.resetForm();
                        $("#form_grade").find("[name='Resolution']").val(null).trigger("change");
                        $(".custom-file-label").text("Seleccione un archivo");
                    })
                },
                init: function () {
                    this.onHidden();
                }
            },
            init: function () {
                modal.viewGrades.events.init();
            }
        },
        viewHistory: {
            object: $("#modal_history"),
            form: {
                object: $("#form_history").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        var formData = new FormData(formElement);
                        $(".btn-submit-history").addLoader();

                        $.ajax({
                            url: "/admin/actualizar-nota-estudiante/actualizar-historial-academico",
                            method: "POST",
                            data: formData,
                            contentType: false,
                            processData: false,
                        })
                            .done(function (e) {
                                modal.viewHistory.object.modal("hide");
                                datatable.student.reload();
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "Nota actualizada con éxito",
                                    confirmButtonText: "Excelente"
                                });
                            })
                            .fail(function (e) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                });
                            })
                            .always(function () {
                                $(".btn-submit-history").removeLoader();
                            });

                    }
                })
            },
            events: {
                show: function (id, grade, course) {
                    modal.viewHistory.object.modal("show");
                    modal.viewHistory.object.find("[name='AcademicHistoryId']").val(id);
                    modal.viewHistory.object.find("[name='Course']").val(course);
                    modal.viewHistory.object.find("[name='OldGrade']").val(grade);
                },
                onHidden: function () {
                    modal.viewHistory.object.on('hidden.bs.modal', function (e) {
                        modal.viewHistory.form.object.resetForm();
                        $("#form_history").find("[name='Resolution']").val(null).trigger("change");
                        $(".custom-file-label").text("Seleccione un archivo");
                    })
                },
                init: function () {
                    this.onHidden();
                }
            },
            init: function () {
                modal.viewHistory.events.init();
            }
        },
        init: function () {
            modal.viewGrades.init();
            modal.viewHistory.init();
        }
    }

    return {
        init: function () {
            select.init();
            events.init();
            modal.init();
            datatable.init();
        }
    }
}();

$(() => {
    index.init();
})