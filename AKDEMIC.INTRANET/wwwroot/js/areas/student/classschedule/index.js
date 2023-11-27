var ClassSchedule = function () {
    var fullCalendar;

    var init = function () {
        var defaultSettings = {
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

                var fc_title = element.find(".fc-title").clone().children().remove().end().text();

                element.css("border-color", "#" + fc_title.proto().toRGB()).css("border-width", "2px");
            },
            events: {
                className: "m-fc-event--primary",
                error: function () {
                    toastr.error("Ocurrió un problema con el servidor", "Error");
                },
                url: ("/alumno/horario-ciclo/get").proto().parseURL()
            },
            eventClick: function (calEvent, jsEvent, view) {
                form.detail.load(calEvent.id);
            },
            firstDay: 1,
            header: {
            },
            height: 600,
            locale: "es",
            maxTime: "24:00:00",
            minTime: "06:00:00",
            nowIndicator: true,
            viewSubSlotLabel: true,
            slotLabelFormat: "h:mmA",
            monthNames: monthNames,
            dayNames: dayNames,
            monthNamesShort: monthNamesShort,
            dayNamesShort: dayNamesShort,
            slotDuration: "00:30:00",
            timeFormat: "h(:mm)A"
        };
        var defaultSettings2 = {
            allDaySlot: false,
            aspectRatio: 2,
            businessHours: {
                dow: [1, 2, 3, 4, 5, 6],
                end: "24:00",
                start: "07:00"
            },
            columnFormat: "dddd",
            contentHeight: 600,
            defaultView: "agendaDay",
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
                url: ("/alumno/horario-ciclo/get").proto().parseURL()
            },
            eventClick: function (calEvent, jsEvent, view) {
                form.detail.load(calEvent.id);
            },
            firstDay: 1,
            header: {
                center: "agendaWeek,agendaDay"
            },
            height: 600,
            locale: "es",
            maxTime: "24:00:00",
            minTime: "06:00:00",
            nowIndicator: true,
            viewSubSlotLabel: true,
            slotLabelFormat: "h:mmA",
            monthNames: monthNames,
            dayNames: dayNames,
            monthNamesShort: monthNamesShort,
            dayNamesShort: dayNamesShort,
            slotDuration: "00:30:00",
            timeFormat: "h(:mm)A"
        };
        var defaultSettings3 = {
            allDaySlot: false,
            aspectRatio: 2,
            businessHours: {
                dow: [1, 2, 3, 4, 5, 6],
                end: "24:00",
                start: "07:00"
            },
            columnFormat: "dddd",
            contentHeight: 600,
            defaultView: "agendaDay",
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
                url: ("/alumno/horario-ciclo/get").proto().parseURL()
            },
            eventClick: function (calEvent, jsEvent, view) {
                form.detail.load(calEvent.id);
            },
            firstDay: 1,
            header: {
            },
            height: 600,
            locale: "es",
            maxTime: "24:00:00",
            minTime: "06:00:00",
            nowIndicator: true,
            viewSubSlotLabel: true,
            slotLabelFormat: "h:mmA",
            monthNames: monthNames,
            dayNames: dayNames,
            monthNamesShort: monthNamesShort,
            dayNamesShort: dayNamesShort,
            slotDuration: "00:30:00",
            timeFormat: "h(:mm)A"
        };
        if ($(window).width() < 700) {
            fullCalendar = $(".m-calendar__container").fullCalendar(defaultSettings3);
        }
        if ($(window).width() > 700 && $(window).width() < 960) {
            fullCalendar = $(".m-calendar__container").fullCalendar(defaultSettings2);
        }
        if ($(window).width() > 960) {
            fullCalendar = $(".m-calendar__container").fullCalendar(defaultSettings);
        }
    };

    var form = {
        detail: {
            load: function (id) {
                mApp.blockPage();
                $.ajax({
                        url: `/alumno/horario-ciclo/${id}/get`.proto().parseURL()
                    })
                    .always(function () {
                        mApp.unblockPage();
                    })
                    .done(function (result) {
                        $("#DetailCourse").val(result.course);
                        $("#DetailSection").val(result.section);
                        $("#DetailDay").val(result.day);
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

    return {
        init: function () {
            init();
        }
    }
}();

$(function () {
    ClassSchedule.init();
});