var CourseTable = function () {
    var courseDatatable = null;

    var options = {//getSimpleDataTableConfiguration({
        ajax: {
            type: "GET",
            dataType: "JSON",
            url: "/admin/examen-sustitutorio/get".proto().parseURL(),
            data: function (data) {
                delete data.columns;
                var careerId = $(".select2-areacareers").val();
                var curriculumId = $(".select2-plans").val();
                var cycleId = $(".select2-cyccles").val();
                var courseId = $(".select2-course").val();
                var termId = $(".select2-terms").val();

                careerId = careerId === _app.constants.guid.empty ? null : careerId;
                curriculumId = careerId === _app.constants.guid.empty ? null : curriculumId;
                courseId = courseId === _app.constants.guid.empty ? null : courseId;

                data.careerId = careerId;
                data.curriculumId = curriculumId;
                data.cycleId = cycleId;
                data.courseId = courseId;
                data.termId = termId;

                data.search = $("#search").val();
            },
        },
        pageLength: 10,
        orderable: [],
        columns: [
            {
                title: "Escuela",
                data: "career",
                orderable: false
            },
            {
                title: "Código",
                data: "code",
                orderable: false
            },
            {
                title: "Curso",
                data: "name",
                orderable: false
            },
            {
                title: "Ciclo",
                data: "academicYear",
                orderable: false
            },
            {
                title: "Modalidad",
                data: "modality",
                orderable: false
            },
            {
                title: "Grupos",
                data: "group",
                orderable: false
            },
            {
                data: null,
                orderable: false,
                title: "Opciones",
                render: function (row) {
                    var tmp = "";
                    tmp += '<button data-id="' +
                        row.id +
                        '" class="btn btn-brand btn-sm m-btn m-btn--icon btn-watch-detail" title="Ver Alumnos"><span><i class="la la-cog"></i><span>Ver Alumnos</span></span></button>';


                    return tmp;
                }
            }
        ]
    };

    var events = {
        init: function () {
            $("body").on("click", ".btn-watch-detail", function () {
                var id = $(this).data("id");

                location.href = `/admin/examen-sustitutorio/estudiantes/${id}`.proto().parseURL();
            });
        }
    };

    var select = {
        init: function () {
            this.careers.init();
            this.plans.init();
            this.course.init();
            this.term.init();
            this.cicles.init();
        },
        term: {
            load: function () {
                $.ajax({
                    url: "/ultimos-periodos/get?yearDifference=4",
                    type: "GET"
                })
                    .done(function (e) {
                        $(".select2-terms").select2({
                            placeholder: "Selecciona periodo",
                            data: e.items
                        });

                        $("[name='TermId']").select2({
                            placeholder: "Selecciona periodo",
                            data: e.items
                        });


                        $("[name='TermId']").on("change", function () {
                            var careerId = $("#cCareer").val();
                            var id = $(this).val();

                            if (id === _app.constants.guid.empty) {
                                //selectsModal.cicles.empty();
                            } else {
                                selectsModal.course.load(careerId);
                                //selectsModal.cicles.load($(this).val());
                            }
                        });

                        $(".select2-terms").on("change", function () {
                            var careerId = $("#cCareer").val();
                            courseDatatable.ajax.reload();
                        });

                        $("#cCareer").trigger("change");
                    });
            },
            init: function () {
                this.load();
            }
        },
        careers: {
            init: function () {
                $(".select2-areacareers").empty();
                $.ajax({
                    url: `/carreras/get`
                }).done(function (result) {
                    result.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $(".select2-areacareers").select2({
                        data: result.items
                    });

                    $(".select2-areacareers").on("change", function () {

                        var id = $(this).val();

                        if (id === _app.constants.guid.empty) {
                            select.plans.empty();
                            select.course.empty();
                            select.cicles.empty();
                            courseDatatable.ajax.reload();
                        } else {
                            select.cicles.empty();
                            select.plans.load($(this).val());
                            //select.programs.load($(this).val());
                        }
                    });
                });
            }
        },
        plans: {
            init: function () {
                $(".select2-plans").select2({
                    placeholder: "Seleccione un plan",
                    disabled: true
                });

                $(".select2-plans").on("change", function () {
                    var id = $(this).val();

                    if (id === _app.constants.guid.empty) {
                        select.course.empty();
                        select.cicles.empty();
                        courseDatatable.ajax.reload();
                    } else {
                        select.course.load($(this).val());
                        select.cicles.load($(this).val());
                    }
                });
            },
            load: function (careerId) {
                $(".select2-plans").empty();
                $.ajax({
                    url: `/admin/examen-sustitutorio/carreras/${careerId}/planestudio/get`.proto().parseURL()
                }).done(function (data) {

                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });
                    $(".select2-plans").select2({
                        data: data.items,
                        disabled: false
                    }).trigger("change");

                });
            },
            empty: function () {
                $(".select2-plans").empty();
                $(".select2-plans").select2({
                    placeholder: "Seleccione un plan",
                    disabled: true
                });
            }
        },
        course: {
            init: function () {
                $(".select2-course").select2({
                    placeholder: "Seleccione un Curso",
                    disabled: true
                });

                $(".select2-course").on("change", function () {
                    var id = $(this).val();
                    courseDatatable.ajax.reload();
                });
            },
            load: function (curriculumId) {
                $(".select2-course").empty();
                $.ajax({
                    url: `/curriculum/${curriculumId}/cursos/get`.proto().parseURL()
                }).done(function (data) {

                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });

                    $(".select2-course").select2({
                        data: data.items,
                        disabled: false
                    }).trigger("change");

                });
            },
            empty: function () {
                $(".select2-course").empty();
                $(".select2-course").select2({
                    placeholder: "Seleccione un Curso",
                    disabled: true
                });
            }
        },
        cicles: {
            init: function () {
                $(".select2-cyccles").select2({
                    placeholder: "Seleccione un ciclo",
                    disabled: true
                });
                $(".select2-cyccles").on("change", function () {
                    courseDatatable.ajax.reload();
                });
            },
            load: function (planId) {
                $(".select2-cyccles").empty();
                $.ajax({
                    url: `/planes-estudio/${planId}/niveles/get`.proto().parseURL()
                }).done(function (data) {
                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });

                    $(".select2-cyccles").select2({
                        data: data.items,
                        disabled: false
                    });
                });
            },
            empty: function () {
                $(".select2-cyccles").empty();
                $(".select2-cyccles").select2({
                    placeholder: "Seleccione un ciclo",
                    disabled: true
                });
            }
        }
    };

    var datatable = {
        init: function () {
            if (courseDatatable !== null && courseDatatable !== undefined) {
                courseDatatable.destroy();
                courseDatatable = null;
            }
            courseDatatable = $(".students-datatable").DataTable(options);
        }
    };

    var selectsModal = {
        init: function () {
            this.careers.init();
            this.course.init();
            this.classroom.init();
            this.section.init();
            //this.cicles.init();
        },
        careers: {
            init: function () {
                $("#cCareer").empty();
                $.ajax({
                    url: `/carreras/get`
                }).done(function (result) {

                    $("#cCareer").select2({
                        data: result.items
                    });

                    $("#cCareer").on("change", function () {

                        var id = $(this).val();

                        if (id === _app.constants.guid.empty) {
                            //selectsModal.cicles.empty();
                        } else {
                            selectsModal.course.load($(this).val());
                            //selectsModal.cicles.load($(this).val());
                        }
                    });

                    $("#cCareer").trigger("change");
                });
            }
        },
        course: {
            init: function () {
                $("#cCourse").select2({
                    placeholder: "Seleccione un Curso",
                    disabled: true
                });

                $("#cCourse").on("change", function () {
                    var id = $(this).val();
                    selectsModal.section.load(id);
                });
            },
            load: function (careerId) {
                var termId = $("[name='TermId']").val();
                $("#cCourse").empty();
                $.ajax({
                    url: `/carreras/${careerId}/cursos/activos/get?termId=${termId}`.proto().parseURL()
                }).done(function (data) {
                    $("#cCourse").select2({
                        data: data.items,
                        disabled: false
                    }).trigger("change");

                });
            },
        },
        section: {
            init: function () {
                $("#cSection").select2({
                    placeholder: "Seleccione un Grupo",
                    disabled: true
                });
            },
            load: function (courseId) {
                var termId = $("[name='TermId']").val();
                $("#cSection").empty();
                $.ajax({
                    url: `/cursos/${courseId}/secciones/get?termId=${termId}`.proto().parseURL()
                }).done(function (data) {
                    $("#cSection").select2({
                        data: data.items,
                        disabled: false
                    }).trigger("change");
                });
            },
            empty: function () {
                $("#cSection").empty();
                $("#cSection").select2({
                    placeholder: "Seleccione un grupo",
                    disabled: true
                });
            }
        },
        classroom: {
            load: function () {
                $("#create_modal").find("[name='ClassroomId']").select2({
                    ajax: {
                        delay: 1000,
                        url: (`/aular/get/v2`).proto().parseURL()
                    },
                    allowClear: true,
                    minimumInputLength: 0,
                    placeholder: "Seleccione aula",
                    dropdownParent: $("#create_modal")
                });
            },
            init: function () {
                this.load();
            }
        },
    }


    var datepicker = {
        startDate: function () {
            $("#create_modal").find("[name='StartDate']").datepicker();
        },
        startTime: function () {
            $("#create_modal").find("[name='StartTime']").timepicker({ minuteStep: 5 }).timepicker("setTime", "7:00 AM");
            $("#create_modal").find("[name='StartTime']").val("");
        },
        init: function () {
            this.startDate();
            this.startTime();
        }
    };

    var forms = {
        create: {
            init: function () {
                var validation = $("#create-form").validate({
                    rules: {
                        CareerId: {
                            required: true
                        },
                        CourseId: {
                            required: true
                        }
                    },
                    submitHandler: function (form, e) {
                        e.preventDefault();
                        $("#create_modal .btn-submit").addLoader();
                        $(form).ajaxSubmit({
                            success: function (data) {
                                toastr.success(data);
                                $("#create_modal").modal("hide");
                                datatable.init();
                            },
                            error: function (jqXhr) {
                                toastr.error(jqXhr.responseText);
                            },
                            complete: function () {
                                $("#create_modal .btn-submit").removeLoader();
                            }
                        });
                    }
                });
            }
        },
        init: function () {
            forms.create.init();
        }
    };

    var itext = {
        init: function () {
            datatable.init();
            //$(".btn-will-search").click(function () {
            //    datatable.init();
            //});
            $("#btn-pdf").click(function () {
                var url = "/admin/examen-sustitutorio/get/pdf?";

                var careerId = $(".select2-areacareers").val();
                var curriculumId = $(".select2-plans").val();
                var cycleId = $(".select2-cyccles").val();
                var courseId = $(".select2-course").val();

                careerId = careerId === _app.constants.guid.empty ? null : careerId;
                curriculumId = careerId === _app.constants.guid.empty ? null : curriculumId;
                courseId = courseId === _app.constants.guid.empty ? null : courseId;

                url += "careerId=" + careerId;
                url += "&curriculumId=" + curriculumId;
                url += "&cycleId=" + cycleId;
                url += "&courseId=" + courseId;
                url += "search=" + $("#search").val();

                window.open(url.proto().parseURL(), "_blank");
            });
            $('#create_modal').on('shown.bs.modal', function (e) {
                selectsModal.init();
            });

            $("#search").doneTyping(function () {
                courseDatatable.ajax.reload();
            });

            events.init();
        }
    }
    return {
        init: function () {
            select.init();
            itext.init();
            forms.create.init();
            datepicker.init();
        }
    }
}();

$(function () {
    CourseTable.init();
});