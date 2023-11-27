var Tutorial = function () {
    var tutorialCalendar = null;

    var calendar = {
        load: function() {
            mApp.block(".m-calendar__container", { type: "loader", message: "Cargando..." });
            tutorialCalendar = $(".m-calendar__container").fullCalendar(defaultSettings);
        },
        reload: function() {
            if (tutorialCalendar !== null) {
                $(tutorialCalendar).fullCalendar("destroy");
                tutorialCalendar = null;
            }
            defaultSettings.events.url =
                `/alumno/tutorias/get?onlyRegistered=${$("#onlyRegistered").is(":checked")}`.proto().parseURL();
            this.load();
        }
    };

    var defaultSettings = {
        allDaySlot: false,
        aspectRatio: 2,
        businessHours: {
            dow: [1, 2, 3, 4, 5, 6],
            end: "24:00",
            start: "07:00"
        },
        columnFormat: "dddd",
        contentHeight: 700,
        defaultView: "agendaWeek",
        editable: false,
        eventRender: function (event, element) {
            if (element.hasClass("fc-day-grid-event")) {
                element.data("content", event.description);
                element.data("placement", "right");
                mApp.initPopover(element);
            } else if (element.hasClass("fc-time-grid-event")) {
                element.find(".fc-title").append("<div class=\"fc-description\">" + event.description + "</div>");
                if (event.registered) {
                    var startHours = event.start.hours();
                    var startMins = event.start.minutes();
                    var endHours = event.end.hours();
                    var endMins = event.end.minutes();
                    if (endHours === 0) endHours += 24;
                    startMins += startHours * 60;
                    endMins += endHours * 60;
                    var tmp = "<span class=\"fc-registered-info pull-right align-bottom\" style=\"font-weight:bold;color:green\"><i class=\"la la-check\"></i>Inscrito</span>";
                    
                    if (endMins - startMins <= 120) {
                        element.find(".fc-description").append(tmp);
                    } else {
                        element.find(".fc-content").append(tmp);
                    }
                }
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
            url: "/alumno/tutorias/get".proto().parseURL()
        },
        eventAfterAllRender: function () {
            $(".fc-prev-button").prop("disabled", false);
            $(".fc-next-button").prop("disabled", false);
            mApp.unblock(".m-calendar__container");
        },
        eventClick: function (calEvent, jsEvent, view) {
            form.show.detail(calEvent.id);
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
    };

    var form = {
        show: {
            detail: function(id) {
                mApp.blockPage();
                $.ajax({
                    url: `/alumno/tutorias/${id}/get`.proto().parseURL()
                }).done(function(result) {
                    var formElements = $("#detail-form").get(0).elements;
                    formElements["id"].value = result.id;
                    formElements["section"].value = result.section;
                    formElements["date"].value = result.date;
                    formElements["classroom"].value = result.classroom;
                    formElements["start"].value = result.start;
                    formElements["end"].value = result.end;
                    formElements["teacher"].value = result.teacher;
                    if (result.canRegister) {
                        if (result.registered)
                            form.switch.inscriptionToCancel();
                        else
                            form.switch.cancelToInscription();
                    } else {
                        $("#btnInscription").hide();
                        $("#btnCancelInscription").hide();
                    }
                    mApp.unblockPage();
                    $("#detail_modal").modal("show");
                }).fail(function() {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            }
        },
        switch: {
            inscriptionToCancel: function() {
                $("#btnInscription").hide();
                $("#btnCancelInscription").show();
            },
            cancelToInscription: function () {
                $("#btnInscription").show();
                $("#btnCancelInscription").hide();
            }
        }
    }
    
    var events = {
        init: function() {
            $("#btnInscription").on("click",
                function () {
                    var $btn = $(this);
                    $btn.addLoader();
                    $.ajax({
                            url: "/alumno/tutorias/inscripcion".proto().parseURL(),
                            type: "POST",
                            data: {
                                id: $("#id").val()
                            }
                        }).always(function() {
                            form.switch.inscriptionToCancel();
                            $btn.removeLoader();
                        })
                        .done(function() {
                            calendar.reload();
                            form.switch.inscriptionToCancel();
                            toastr.success(_app.constants.toastr.message.success.task,
                                _app.constants.toastr.title.success);
                        }).fail(function() {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        });
                });
            $("#btnCancelInscription").on("click",
                function() {
                    var $btn = $(this);
                    $btn.addLoader();
                    $.ajax({
                        url: "/alumno/tutorias/anular-inscripcion".proto().parseURL(),
                        type: "POST",
                        data: {
                            id: $("#id").val()
                        }
                    }).always(function () {
                        form.switch.cancelToInscription();
                        $btn.removeLoader();
                    }).done(function() {
                        calendar.reload();
                        toastr.success(_app.constants.toastr.message.success.task,
                            _app.constants.toastr.title.success);
                    }).fail(function() {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    });
                });
            $("#onlyRegistered").on("change",
                function() {
                    calendar.reload();
                });
        }
    }

    return {
        init: function() {
            calendar.load();
            events.init();
        }
    }
}();

$(function () {
    Tutorial.init();
});