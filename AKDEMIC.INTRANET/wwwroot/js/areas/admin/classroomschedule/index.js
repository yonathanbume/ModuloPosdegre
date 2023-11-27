var ClassroomSchedule = function () {
    var fullCalendar;

    var init = function() {
        select2.init();
    }

    var select2 = {
        init: function () {
            this.classrooms.initEvents();
            this.buildings.initEvents();
            this.campuses.init();
        },
        campuses: {
            init: function () {
                $(".select2-campuses").prop("disabled", true).select2({
                    placeholder: "Campus"
                });
                $.ajax({
                    url: "/sedes/get".proto().parseURL()
                }).done(function(result) {
                    $(".select2-campuses").select2({
                        placeholder: "Campus",
                        data: result.items
                    }).on("change",
                        function() {
                            select2.buildings.init($(this).val());
                        }).trigger("change");
                    if (result.items.length) {
                        $(".select2-campuses").prop("disabled", false);
                    }
                });
            }
        },
        buildings: {
            initEvents: function() {
                $(".select2-buildings").on("change",
                    function() {
                        select2.classrooms.init($(this).val());
                    });
            },
            init: function(campusId) {
                $(".select2-buildings").prop("disabled", true).empty();
                $.ajax({
                    url: `/sedes/${campusId}/pabellones/get`.proto().parseURL()
                }).done(function(result) {
                    $(".select2-buildings").select2({
                        placeholder: "Pabellón",
                        data: result.items
                    });
                    if (result.items.length) {
                        $(".select2-buildings").prop("disabled", false);
                    }
                    $(".select2-buildings").trigger("change");
                });
            }
        },
        classrooms: {
            initEvents: function() {
                $(".select2-classrooms").on("change",
                    function () {
                        var classroomId = $(this).val();
                        calendar.destroy();
                        if (classroomId !== null) {
                            calendar.load(classroomId);
                        }
                    });
            },
            init: function(buildingId) {
                $(".select2-classrooms").prop("disabled", true).empty();
                $.ajax({
                    url: `/sedes/${$(".select2-campuses").val()}/pabellones/${buildingId}/aulas/get`.proto()
                        .parseURL()
                }).done(function(result) {
                    $(".select2-classrooms").select2({
                        placeholder: "Aula",
                        data: result.items
                    });
                    if (result.items.length) {
                        $(".select2-classrooms").prop("disabled", false);
                    }
                    $(".select2-classrooms").trigger("change");
                });
            }
        }
    }

    var defaultSettings = {
        allDaySlot: false,
        aspectRatio: 2,
        businessHours: {
            dow: [1, 2, 3, 4, 5, 6],
            end: "23:40",
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
            var fc_title = element.find(".fc-title").clone().children().remove().end().text();
            element.css("border-color", "#" + fc_title.proto().toRGB()).css("border-width", "2px");
        },
        events: {
            className: "m-fc-event--primary",
            error: function () {
                toastr.error("Ocurrió un problema con el servidor", "Error");
            },
            url: ""
        },
        eventClick: function (calEvent, jsEvent, view) {
            console.log(calEvent.id);
            form.detail.load(calEvent.id);
        },
        eventAfterAllRender: function () {
            mApp.unblock(".m-calendar__container");
        },
        firstDay: 1,
        header: false,
        height: 600,
        contentHeight: 900,
        locale: "es",
        maxTime: "24:00:00",
        minTime: "06:00:00",
        nowIndicator: true,
        slotLabelInterval: "01:00:00",
        viewSubSlotLabel: true,
        slotLabelFormat: "h:mmA",
        monthNames: monthNames,
        dayNames: dayNames,
        monthNamesShort: monthNamesShort,
        dayNamesShort: dayNamesShort,
        slotDuration: "01:00:00",
        timeFormat: "h(:mm)A"
    };

    var calendar = {
        load: function (id) {
            defaultSettings.events.url = `/admin/horarioaulas/${id}/get`.proto().parseURL();
            mApp.block(".m-calendar__container", { type: "loader", message: "Cargando..." });
            fullCalendar = $(".m-calendar__container").fullCalendar(defaultSettings);
        },
        destroy: function() {
            if (fullCalendar !== null) {
                $(fullCalendar).fullCalendar("destroy");
                fullCalendar = null;
            }
        }
    }

    var form = {
        detail: {
            load: function (id) {
                mApp.blockPage();
                $.ajax({
                    url: `/admin/horarioaulas/${id}/get-detalle`.proto().parseURL()
                })
                    .always(function () {
                        mApp.unblockPage();
                    })
                    .done(function (result) {
                        $("#DetailCourse").val(result.course);
                        $("#DetailSection").val(result.section);
                        $("#DetailDate").val(result.date);
                        $("#DetailStart").val(result.start);
                        $("#DetailEnd").val(result.end);
                        $("#DetailClassroom").val(result.classroom);
                        $("#DetailType").val(result.type);
                        $("#DetailTeachers").val(result.teachers.join("\r\n"));

                        $("#modal-detail").modal("show");
                    })
                    .fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    });
            }
        }
    };

    var events = {
        init: function () {
            $("#export_excel").on("click", function () {
                var classromId = $(".select2-classrooms").val();
                window.open(`/admin/horarioaulas/get-excel?cid=${classromId}`, "_blank");
            })
        }
    }

    return {
        init: function() {
            init();
            events.init();
        }
    }
}();

$(function () {
    ClassroomSchedule.init();
});
