
var Workload = function () {
    var coursesDatatable = null;
    var studentsDatatable = null;
    var evaluationsDatatable = null;

    var subTableFunction = function (e) {
        var subDatatable = $("<div/>").attr("id", "child_data_local_" + e.data.id).appendTo(e.detailCell).mDatatable({
            data: {
                type: "remote",
                source: {
                    read: {
                        url: `/admin/carga-lectiva/${$(".select2-teachers").val()}/cursos/${e.data.id}/secciones/get`.proto().parseURL()
                    }
                }
            },
            sortable: false,
            pagination: false,
            columns: [
                {
                    field: "code",
                    title: "Sección",
                    textAlign: "center"
                },
                {
                    field: "studentsCount",
                    title: "N° de Estudiantes",
                    textAlign: "center"
                },
                {
                    field: "Id",
                    title: "Estudiantes",
                    template: function (row) {
                        return `<button data-sectionid="${row.id}" data-courseid="${e.data.id}" class="btn btn-info btn-sm m-btn m-btn--icon btn-students"><span><i class="la la-edit students"></i><span> Alumnos </span></span></button>`;
                    }
                },
                {
                    field: "Id2",
                    title: "Evaluaciones",
                    template: function (row) {
                        return `<button data-sectionid="${row.id}" data-courseid="${e.data.id}" class="btn btn-success btn-sm m-btn m-btn--icon btn-evaluations"><span><i class="la la-edit evaluations"></i><span> Evaluaciones </span></span></button>`;
                    }
                }
            ]
        });

        subDatatable.on("click", ".btn-students",
            function () {
                var courseId = $(this).data("courseid");
                var sectionId = $(this).data("sectionid");
                $("#students_modal").one("shown.bs.modal",
                    function () {
                        datatable.students.init(sectionId, courseId);
                    });
                $("#students_modal").one("hidden.bs.modal",
                    function () {
                        datatable.students.destroy();
                    });
                $("#students_modal").modal("show");
            });

        subDatatable.on("click", ".btn-evaluations",
            function () {
                var courseId = $(this).data("courseid");
                var sectionId = $(this).data("sectionid");
                $("#evaluations_modal").one("shown.bs.modal",
                    function () {
                        datatable.evaluations.init(sectionId, courseId);
                    });
                $("#evaluations_modal").one("hidden.bs.modal",
                    function () {
                        datatable.evaluations.destroy();
                    });
                $("#evaluations_modal").modal("show");
            });
    };

    var courseSettings = {
        data: {
            type: "remote",
            source: {
                read: {
                    url: ""
                }
            }
        },
        detail: {
            title: "Cargar subtablas",
            content: subTableFunction
        },
        sortable: false,
        pagination: false,
        columns: [
            {
                field: "id",
                title: "",
                sortable: false,
                width: 20,
                textAlign: "center"
            },
            {
                field: "code",
                title: "Código",
                textAlign: "center"
            },
            {
                field: "name",
                title: "Curso"
            },
            {
                field: "courseType.name",
                title: "Tipo",
                textAlign: "center"
            },
            {
                field: "areaCareer",
                title: "Área o Carrera"
            }
        ]
    };

    var studentsSettings = {
        data: {
            type: "remote",
            source: {
                read: {
                    url: ""
                }
            }
        },
        sortable: false,
        pagination: false,
        columns: [
            {
                field: "user.userName",
                title: "Código",
                width: 80
            },
            {
                field: "user.fullName",
                title: "Estudiante"
            },
            {
                field: "career.name",
                title: "Carrera"
            }
        ]
    };

    var evaluationsSettings = {
        data: {
            type: "remote",
            source: {
                read: {
                    url: ""
                }
            }
        },
        sortable: false,
        pagination: false,
        columns: [
            {
                field: "name",
                title: "Evaluación"
            },
            {
                field: "taken",
                title: "Tomada",
                width: 80,
                textAlign: "center",
                template: function (row) {
                    if (row.taken) return "Si";
                    else return "No";
                }
            }
        ]
    };

    var datatable = {
        courses: {
            init: function (teacherId) {
                if (coursesDatatable !== null) {
                    coursesDatatable.destroy();
                    coursesDatatable = null;
                }
                courseSettings.data.source.read.url = `/admin/carga-lectiva/${teacherId}/cursos/get`.proto().parseURL();
                coursesDatatable = $("#workload_table").mDatatable(courseSettings);
            }
        },
        students: {
            init: function (sectionId, courseId) {
                this.destroy();
                studentsSettings.data.source.read.url = `/admin/carga-lectiva/${$(".select2-teachers").val()}/cursos/${courseId}/secciones/${sectionId}/alumnos/get`.proto().parseURL();
                studentsDatatable = $("#students_table").mDatatable(studentsSettings);
            },
            destroy: function () {
                if (studentsDatatable !== null) {
                    studentsDatatable.destroy();
                    studentsDatatable = null;
                }
            }
        },
        evaluations: {
            init: function (sectionId, courseId) {
                this.destroy();
                evaluationsSettings.data.source.read.url = `/admin/carga-lectiva/${$(".select2-teachers").val()}/cursos/${courseId}/secciones/${sectionId}/evaluaciones/get`.proto().parseURL();
                evaluationsDatatable = $("#evaluations_table").mDatatable(evaluationsSettings);
            },
            destroy: function () {
                if (evaluationsDatatable !== null) {
                    evaluationsDatatable.destroy();
                    evaluationsDatatable = null;
                }
            }
        }
    };

    var events = {
        onShowCalendarModal: function () {
            $("#show_schedule").click(function () {
                modal.calendar.show();
            });
        },
        init: function () {
            this.onShowCalendarModal();

            $('#modal_calendar').on('shown.bs.modal', function (e) {
                var id = $("#teachers").val();
                calendar.load(id);
            })
        }
    }

    var select2 = {
        init: function () {
            this.faculties.init();
            this.academicDepartments.init();
            this.teachers.init();
        },
        faculties: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: ("/facultades/get").proto().parseURL()
                }).done(function (result) {
                    $("#faculties").select2({
                        data: result.items,
                        placeholder: "Facultad"
                    });
                });
            },
            events: function () {
                $("#faculties").on("change", function () {
                    select2.academicDepartments.load();
                    select2.teachers.load();
                });
            }
        },
        academicDepartments: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                var facultyId = $("#faculties").val();
                $.ajax({
                    url: (`/departamentos-academicos/get?facultyId=${facultyId}`).proto().parseURL()
                }).done(function (result) {
                    $("#academicDepartments").empty();
                    $("#academicDepartments").html(`<option value="0" disabled selected>Seleccione una Opcion</option>`);
                    $("#academicDepartments").select2({
                        data: result,
                        placeholder: "Departamento Académico"
                    });
                });
            },
            events: function () {
                $("#academicDepartments").on("change", function () {
                    select2.teachers.load();
                });
            }
        },
        teachers: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                var facultyId = $("#faculties").val();
                var academicDepartmentId = $("#academicDepartments").val();
                $.ajax({
                    url: (`/profesores/departamento-academico/get/${facultyId}/${academicDepartmentId}`.proto().parseURL())
                }).done(function (result) {
                    $("#teachers").empty();
                    $("#teachers").select2({
                        data: result.items,
                        placeholder: "Docente"
                    }).trigger("change");
                });
            },
            events: function () {
                $("#teachers").on("change", function () {
                    datatable.courses.init($(this).val());
                });
            }
        }
    };

    var calendar = {
        object: null,
        defaultSettings: {
            allDaySlot: false,
            aspectRatio: 2,
            businessHours: {
                dow: [1, 2, 3, 4, 5, 6],
                end: "24:00",
                start: "07:00"
            },
            columnFormat: "dddd",
            contentHeight: 600,
            defaultView: "agendaWeek",
            editable: false,
            eventRender: function (event, element) {
                if (element.hasClass("fc-day-grid-event")) {
                    element.data("content", event.description);
                    element.data("placement", "top");
                    mApp.initPopover(element);
                } else if (element.hasClass("fc-time-grid-event")) {
                    element.find(".fc-title").append("<div class=\"fc-description\">" + event.description + "</div>");
                } else if (element.find(".fc-list-item-title").length !== 0) {
                    element.find(".fc-list-item-title").append("<div class=\"fc-description\">" + event.description + "</div>");
                }

                var fcTitle = element.find(".fc-title").clone().children().remove().end().text();

                element.css("border-color", "#" + fcTitle.proto().toRGB()).css("border-width", "2px");
            },
            events: {
                className: "m-fc-event--primary",
                error: function () {
                    toastr.error("Ocurrió un problema con el servidor", "Error");
                },
                url: ``
            },
            eventAfterAllRender: function () {
                $(".fc-prev-button").prop("disabled", false);
                $(".fc-next-button").prop("disabled", false);
                mApp.unblock(".m-calendar__container");
            },
            firstDay: 1,
            header: {
                left: "prev,next today",
                center: "title"
            },
            height: 600,
            locale: "es",
            maxTime: "24:00:00",
            minTime: "06:00:00",
            monthNames: monthNames,
            dayNames: dayNames,
            monthNamesShort: monthNamesShort,
            dayNamesShort: dayNamesShort,
            buttonText: {
                today: "hoy",
                month: "mes",
                week: "semana",
                day: "día"
            },
            slotDuration: "00:30:00",
            timeFormat: "h(:mm)A"
        },
        load: function (id) {
            calendar.destroy();

            calendar.defaultSettings.events.url = `/admin/carga-lectiva/get-horario/${id}`.proto().parseURL();
            mApp.block(".m-calendar__container", { type: "loader", message: "Cargando..." });
            calendar.object = $(".m-calendar__container").fullCalendar(calendar.defaultSettings);
        },
        destroy: function () {
            if (calendar.object !== null) {
                $(calendar.object).fullCalendar("destroy");
                calendar.object = null;
            }
        }

    };

    var modal = {
        calendar: {
            show: function (id) {
                $("#modal_calendar").modal("show");
            }
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
        }
    }
}();

jQuery(document).ready(function () {
    Workload.init();
});