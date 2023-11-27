var reportDetails = function () {
    var datatable = {
        object: null,
        sectionId: null,
        options: {
            ajax: {
                url: `/admin/reporte_asistencia_docentes/secciones/get`.proto().parseURL(),
                data: function (data) {
                    delete data.columns;
                    data.termId = $("#select2-terms").val();
                    data.teacherId = $("#TeacherId").val();
                },
            },
            pageLength: 50,
            orderable: [],
            columns: [
                {
                    data: "course",
                    title: "Curso"
                },
                {
                    data: "section",
                    title: "Sección"
                },
                {
                    data: "dictatedClasses",
                    title: "Clases Dictadas"
                },
                {
                    data: "rescheduledClasses",
                    title: "Clases Reprogramadas"
                },
                {
                    data: "totalClasses",
                    title: "Clases Totales"
                }
            ],
            dom: 'Bfrtip',
            buttons: [
                {
                    text: 'Excel',
                    action: function () {
                        $.fileDownload(`/admin/reporte_asistencia_docentes/secciones/get/reporte-excel`.proto().parseURL(),
                            {
                                httpMethod: 'GET',
                                data: {
                                    teacherId: $("#TeacherId").val(),
                                    termId: $("#select2-terms").val()
                                },
                                successCallback: function () {
                                    toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                }
                            }
                        ).done(function () {
                        })
                            .fail(function () {
                                toastr.error("Error al descargar el archivo", "Error");
                            })
                            .always(function () {
                            });
                    }
                },
                {
                    text: 'PDF',
                    action: function () {
                        var teacherId = $("#TeacherId").val();
                        var termId = $("#select2-terms").val();
                        window.open(`/admin/reporte_asistencia_docentes/secciones/get/reporte-pdf?teacherId=${teacherId}&termId=${termId}`, "_blank");
                    }
                }
            ]
        },
        reloadTable: function () {
            if (datatable.object === null) {
                datatable.init();
            } else {
                datatable.object.ajax.reload();
            }
        },
        init: function () {
            datatable.object = $("#data-datatable").DataTable(datatable.options);
        }
    };

    var loadTermSelect = function () {
        $.ajax({
            url: "/periodos/get".proto().parseURL()
        }).done(function (data) {
            $("#select2-terms").select2({
                data: data.items
            });

            $("#select2-terms").on("change", function (e) {
                events.needReload = true;
                datatable.reloadTable();
            });

            if (data.selected !== null) {
                $("#select2-terms").val(data.selected).trigger("change");
            }
        });
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
        load: function () {
            mApp.block("#schedule", {
                message: "Cargando horario..."
            });

            if (calendar.object !== null) {
                calendar.object.fullCalendar("destroy");
            }
            
            calendar.object = $("#schedule").fullCalendar(calendar.defaultSettings);
        }
    };

    var events = {
        needReload: true,
        onShowCalendarModal: function () {
            $("#show_schedule").click(function () {
                $("#modal_calendar").modal("show");
            });

            $('#modal_calendar').on('shown.bs.modal', function (e) {
                if (events.needReload) {
                    var termId = $("#select2-terms").val();
                    console.log(termId);
                    var teacherId = $("#TeacherId").val();
                    calendar.defaultSettings.events.url = `/admin/reporte_asistencia_docentes/get-horario/${teacherId}/${termId}`;
                    calendar.load();
                    events.needReload = false;
                }
            });
        },
        init: function () {
            this.onShowCalendarModal();
        }
    };

    return {
        load: function () {
            events.init();
            loadTermSelect();
        }
    };
}();

$(function () {
    reportDetails.load();
});