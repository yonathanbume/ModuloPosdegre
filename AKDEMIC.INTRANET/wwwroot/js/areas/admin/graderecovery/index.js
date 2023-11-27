var graderecover = function () {

    var datatable = {
        sections: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/recuperacion-notas/get",
                    type: "GET",
                    data: function(data){
                        data.careerId = $("#careerId").val();
                        data.curriculumId = $("#curriculumId").val();
                        data.courseId = $("#courses").val();
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        data: "career",
                        title : "Carrera"
                    },
                    {
                        data : "courseCode",
                        title : "Cod. Curso"
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
                        data: "academicYear",
                        title :"Ciclo"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        render: function (row) {
                            var tmp = "";
                            tmp += `<a class="btn btn-primary m-btn btn-sm m-btn--icon m-btn--icon-only" href="/admin/recuperacion-notas/examen/detalles/${row.id}" title="Aprobar"><i class="la la-eye"></i></a>`;
                            return tmp;
                        }
                    }
                ]
            },
            reload: function () {
                if (this.object === null) {
                    this.object = $("#ajax_data").DataTable(this.options);
                } else {
                    this.object.ajax.reload();
                }
            },
            init: function () {
                datatable.sections.reload();
            }
        },
        init: function () {
            datatable.sections.init();
        }
    };

    var modal = {
        addExam: {
            object: $("#create_modal"),
            validate: function () {
                $("#create_form").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        var $btn = modal.addExam.object.find("button[type='submit']");
                        $btn.addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                        var formData = new FormData(formElement);

                        $.ajax({
                            url: "/admin/recuperacion-notas/agregar-seccion",
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        })
                            .done(function (data) {
                                window.location.href = `/admin/recuperacion-notas/examen/detalles/${data}`;
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
                    }
                });
            },
            init: function () {
                this.validate();
            }
        },
        init: function () {
            this.addExam.init();
        }
    };

    var select = {
        career: {
            load: function () {
                $.ajax({
                    url: (`/carreras/v3/get`).proto().parseURL(),
                    type: "get"
                })
                    .done(function (e) {
                        modal.addExam.object.find("[name='CareerId']").select2({
                            data: e.items,
                            allowClear: true,
                            minimumInputLength: 0,
                            placeholder: "Seleccione escuela profesional",
                            dropdownParent: modal.addExam.object
                        }).trigger("change");
                        $("#careerId").select2({
                            data: e.items,
                            minimumInputLength: 0,
                            placeholder: "Seleccione escuela profesional",
                        });
                    });
                //modal.addExam.object.find("[name='CareerId']").select2({
                //    ajax: {
                //        delay: 1000,
                //        url: (`/carreras/v3/get`).proto().parseURL()
                //    },
                //    allowClear: true,
                //    minimumInputLength: 0,
                //    placeholder: "Seleccione escuela profesional",
                //    dropdownParent: modal.addExam.object
                //});

                //$("#careerId").select2({
                //    ajax: {
                //        delay: 1000,
                //        url: (`/carreras/v3/get`).proto().parseURL()
                //    },
                //    allowClear: true,
                //    minimumInputLength: 0,
                //    placeholder: "Seleccione escuela profesional",
                //});
            },
            onChange: function () {
                modal.addExam.object.find("[name='CareerId']").on("change", function () {
                    var careerId = $(this).val();
                    select.curriculum.load(careerId);
                    modal.addExam.object.find("[name='CourseId']").attr("disabled", true);
                });

                $("#careerId").on("change", function () {
                    var careerId = $(this).val();
                    select.curriculum.load_v2(careerId);
                });
            },
            init: function () {
                this.load();
                this.onChange();
            }
        },
        curriculum: {
            load: function (careerId) {
                modal.addExam.object.find("[name='CurriculumId']").empty();
                modal.addExam.object.find("[name='CurriculumId']").attr("disabled", true);

                if (careerId !== null) {
                    $.ajax({
                        url: `/carreras/${careerId}/planestudio/get`
                    })
                        .done(function (data) {
                            modal.addExam.object.find("[name='CurriculumId']").select2({
                                placeholder: "Seleccionar Plan de Estudio",
                                data: data.items,
                                dropdownParent: modal.addExam.object
                            }).trigger("change");
                            modal.addExam.object.find("[name='CurriculumId']").attr("disabled", false);
                        });
                } else {
                    modal.addExam.object.find("[name='CurriculumId']").select2({
                        placeholder: "Seleccionar Plan de Estudio",
                        disabled : true,
                        dropdownParent: modal.addExam.object
                    }).trigger("change");
                }
              
            },
            load_v2: function (careerId) {
                $("#curriculumId").empty();

                if (careerId !== null) {
                    $.ajax({
                        url: `/carreras/${careerId}/planestudio/get`
                    })
                        .done(function (data) {
                            data.items.unshift({ id: '0', text: "Todos"});
                            $("#curriculumId").select2({
                                placeholder: "Seleccionar Plan de Estudio",
                                data: data.items
                            }).trigger("change");
                            $("#curriculumId").attr("disabled", false);
                        });
                } else {
                    $("#curriculumId").select2({
                        placeholder: "Seleccionar Plan de Estudio",
                        disabled: true
                    }).trigger("change");
                }
              
            },
            onChange: function () {
                modal.addExam.object.find("[name='CurriculumId']").on("change", function () {
                    var curriculumId = $(this).val();
                    select.courses.load(curriculumId);
                });

                $("#curriculumId").on("change", function () {
                    var curriculumId = $(this).val();
                    datatable.sections.reload();
                    //select.courses.load_v2(curriculumId);
                });
            },
            init: function () {
                modal.addExam.object.find("[name='CurriculumId']").select2({
                    placeholder: "Seleccionar Plan de Estudio",
                    disabled :true
                });

                $("#curriculumId").select2({
                    placeholder: "Seleccionar Plan de Estudio",
                    disabled: true
                });

                this.onChange();
            }
        },
        courses: {
            load: function (curriculumId) {
                modal.addExam.object.find("[name='CourseId']").empty();
                modal.addExam.object.find("[name='CourseId']").attr("disabled", true);

                $.ajax({
                    url: `/curriculum/${curriculumId}/cursos/get`
                })
                    .done(function (data) {
                        modal.addExam.object.find("[name='CourseId']").select2({
                            placeholder: "Seleccionar Curso",
                            data: data.items,
                            dropdownParent: modal.addExam.object,
                        }).trigger("change");
                        modal.addExam.object.find("[name='CourseId']").attr("disabled", false);
                    });

            },
            load_v2: function (curriculumId) {
                $("#courses").empty();

                if (curriculumId !== null) {
                    $.ajax({
                        url: `/curriculum/${curriculumId}/cursos/get`
                    })
                        .done(function (data) {
                            $("#courses").select2({
                                placeholder: "Seleccionar Curso",
                                data: data.items,
                            }).trigger("change");
                            $("#courses").attr("disabled", false);
                        });
                } else {
                    $("#courses").select2({
                        placeholder: "Seleccionar Curso",
                        disabled: true
                    }).trigger("change");
                }
            },
            onChange: function () {
                modal.addExam.object.find("[name='CourseId']").on("change", function () {
                    var id = $(this).val();
                    select.sections.load(id);
                });
            },
            init: function () {
                modal.addExam.object.find("[name='CourseId']").select2({
                    placeholder: "Seleccionar Curso",
                    disabled: true
                });
                $("#courses").select2({
                    placeholder: "Seleccionar Curso",
                    disabled: true
                });
                this.onChange();
            }
        },
        sections: {
            load: function (courseId) {
                modal.addExam.object.find("[name='SectionId']").empty();
                modal.addExam.object.find("[name='SectionId']").attr("disabled", true);

                $.ajax({
                    url: `/cursos/${courseId}/secciones/get`
                })
                    .done(function (data) {
                        modal.addExam.object.find("[name='SectionId']").attr("disabled", false);
                        modal.addExam.object.find("[name='SectionId']").select2({
                            data: data.items,
                            dropdownParent: modal.addExam.object,
                            placeholder: "Seleccionar sección"
                        });

                    });
            },
            init: function () {
                modal.addExam.object.find("[name='CourseId']").select2({
                    placeholder: "Seleccionar sección",
                    disabled: true
                });
            }
        },
        classroom: {
            load: function () {
                modal.addExam.object.find("[name='ClassroomId']").select2({
                    ajax: {
                        delay: 1000,
                        url: (`/aular/get/v2`).proto().parseURL()
                    },
                    allowClear: true,
                    minimumInputLength: 0,
                    placeholder: "Seleccione aula",
                    dropdownParent: modal.addExam.object
                });
            },
            init: function () {
                this.load();
            }
        },
        init: function () {
            this.career.init();
            this.curriculum.init();
            this.courses.init();
            this.sections.init();
            this.classroom.init();
        }
    };

    var datepicker = {
        startDate: function () {
            modal.addExam.object.find("[name='StartDate']").datepicker();
        },
        startTime: function () {
            modal.addExam.object.find("[name='StartTime']").timepicker({ minuteStep: 5 }).timepicker("setTime", "7:00 AM");
        },
        init: function () {
            this.startDate();
            this.startTime();
        }
    };

    var events = {
        onSearch: function () {
            $("#search").doneTyping(function () {
                datatable.sections.reload();
            });
        },
        init: function () {
            this.onSearch();
        }
    };


    return {
        init: function () {
            datatable.init();
            select.init();
            datepicker.init();
            modal.init();
            events.init();
        }
    };
}();

$(() => {
    graderecover.init();
});