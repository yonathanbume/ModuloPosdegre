var _app = (typeof _app !== "undefined") ? _app : {};
_app.modules = (typeof _app.modules !== "undefined") ? _app.modules : {};

_app.modules.calendar = {
    load: function (jQueryElement, settings) {
        var calendarSettings = this.settings.merge(settings);

        jQueryElement.fullCalendar(calendarSettings);
    },
    settings: {
        allDaySlot: false,
        aspectRatio: 2,
        businessHours: {
            dow: [1, 2, 3, 4, 5, 6],
            end: "24:00",
            start: "07:00"
        },
        columnFormat: "dddd",
        contentHeight: 600,
        dayNames: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
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
            url: urlResource
        },
        firstDay: 1,
        header: false,
        height: 600,
        locale: "es",
        maxTime: "24:00:00",
        minTime: "07:00:00",
        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"],
        slotDuration: "00:30:00",
        timeFormat: "h(:mm)A"
    }
};
