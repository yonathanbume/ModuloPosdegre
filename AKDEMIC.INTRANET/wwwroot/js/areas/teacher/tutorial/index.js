var Tutorial = function() {
    var fullCalendar;
    var createValidateForm;
    var editValidateForm;
    var studentsDatatable;

    var calendar = {
        load: function () {
            mApp.block(".m-calendar__container", { type: "loader", message: "Cargando..." });
            fullCalendar = $(".m-calendar__container").fullCalendar(defaultSettings);
            this.events();
        },
        reload: function () {
            if (fullCalendar !== null) {
                $(fullCalendar).fullCalendar("destroy");
                fullCalendar = null;
            }
            this.load();
        },
        events: function () {
            fullCalendar.on("click",
                ".fc-prev-button",
                function () {
                    $(this).prop("disabled", true);
                    mApp.block(".m-calendar__container", { type: "loader", message: "Cargando..." });
                });
            fullCalendar.on("click",
                ".fc-next-button",
                function () {
                    $(this).prop("disabled", true);
                    mApp.block(".m-calendar__container", { type: "loader", message: "Cargando..." });
                });
        },
        updateEvents: function (event, delta, revertFunc) {
            if (event.start.date() !== event.end.date()) {
                if (event.end.hours() !== 0 && event.end.minutes() !== 0) {
                    toastr.error("Rango de horas inválido", _app.constants.toastr.title.error);
                    revertFunc();
                    return;
                }
            }
            var startHour = event.start.hours();
            var startMin = event.start.minutes();
            var endHour = event.end.hours();
            var endMin = event.end.minutes();
            startMin += startHour * 60;
            endMin += endHour * 60;
            if (startMin > 0 && startMin < 420 || endMin > 0 && endMin < 420) {
                toastr.error("Rango de horas inválido", _app.constants.toastr.title.error);
                revertFunc();
                return;
            }
            $.ajax({
                url: "/profesor/tutorias/editartiempo/post".proto().parseURL(),
                type: "POST",
                data: {
                    date: event.start.format(_app.constants.formats.datepickerJsMoment),
                    start: event.start.format(_app.constants.formats.timepickerJsMoment),
                    end: event.end.format(_app.constants.formats.timepickerJsMoment),
                    id: event.id
                }
            }).done(function () {
                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
            }).fail(function(error) {
                if (error.responseText)
                    toastr.error(error.responseText, _app.constants.toastr.title.error);
                else
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                revertFunc();
            });
        }
    }

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
        editable: true,
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
            url: "/profesor/tutorias/get".proto().parseURL()
        },
        eventAfterAllRender: function () {
            $(".fc-prev-button").prop("disabled", false);
            $(".fc-next-button").prop("disabled", false);
            mApp.unblock(".m-calendar__container");
        },
        eventClick: function (calEvent, jsEvent, view) {
            mApp.blockPage();
            form.show.detail(calEvent.id, function () {
                $("#tabContent a").first().addClass("active").addClass("show");
                $("#general").addClass("active").addClass("show");
                mApp.unblockPage();
                datatable.load(calEvent.id);
                $("#edit_modal").modal("show");
                $("#edit_modal").one("hidden.bs.modal",
                    function() {
                        $("#tabContent a").removeClass("active").removeClass("show");
                        $(".tab-pane").removeClass("active").removeClass("show");
                        datatable.destroy();
                    });
            });
        },
        eventResize: calendar.updateEvents,
        eventDrop: calendar.updateEvents,
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

    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: "/profesor/tutorias/alumnos/get".proto().parseURL()
                }
            }
        },
        pagination: false,
        columns: [
            {
                field: "code",
                title: "Código",
                width: 120
            },
            {
                field: "name",
                title: "Nombre Completo",
                width: 250
            }
        ]
    };

    var form = {
        show: {
            create: function() {
                $("#add_modal").modal("show");
                $("#add_modal").one("hidden.bs.modal",
                    function() {
                        form.reset.create();
                    });
            },
            detail: function (id, postFill) {
                $.ajax({
                    url: `/profesor/tutorias/${id}/get`.proto().parseURL()
                }).done(function(result) {
                    var formElements = $("#edit-form").get(0);
                    formElements["Edit_ClassroomId"].value = result.classroomId;
                    formElements["Edit_SectionId"].value = result.sectionId;
                    formElements["Edit_Id"].value = result.id;
                    $("#Edit_Date").datepicker("setDate", result.date);
                    $("#Edit_Start").timepicker("setTime", result.start);
                    $("#Edit_End").timepicker("setTime", result.end);
                    $("#edit-form input").prop("disabled", true);
                    $("#edit-form select").prop("disabled", true);
                    if (!result.editable) {
                        $("#btnEdit").hide();
                        $("#btnDelete").hide();
                        $("#btnSave").hide();
                        $("#btnCancelEdit").hide();
                        $("#separator").hide();
                    } else {
                        $("#btnEdit").show();
                        $("#btnDelete").show();
                        $("#btnSave").hide();
                        $("#btnCancelEdit").hide();
                        $("#separator").show();
                    }
                    postFill();
                }).fail(function() {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            }
        },
        submit: {
            create: function (formElement) {
                var data = $(formElement).serialize();
                $(formElement).find("#btnCreate").addLoader();
                $(formElement).find("input").prop("disabled", true);
                $(formElement).find("select").prop("disabled", true);

                $.ajax({
                        url: $(formElement).attr("action"),
                        type: "POST",
                        data: data
                    })
                    .always(function() {
                        $(formElement).find("#btnCreate").removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                        $(formElement).find("select").prop("disabled", false);
                    })
                    .done(function () {
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#add_modal").modal("hide");
                        calendar.reload();
                    }).fail(function(e) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (e.responseText != null) $("#add_form_msg_txt").html(e.responseText);
                        else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        $("#add_form_msg").removeClass("m--hide").show();
                    });
            },
            edit: function (formElement) {
                var data = $(formElement).serialize();
                $(formElement).find("#btnSave").addLoader();
                $(formElement).find("#btnCancelEdit").addLoader();
                $(formElement).find("input").prop("disabled", true);
                $(formElement).find("select").prop("disabled", true);

                $.ajax({
                        url: $(formElement).attr("action"),
                        type: "POST",
                        data: data
                    })
                    .always(function () {
                        $(formElement).find("#btnSave").removeLoader();
                        $(formElement).find("#btnCancelEdit").removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                        $(formElement).find("select").prop("disabled", false);
                    })
                    .done(function () {
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        form.switch.editToDetail();
                        calendar.reload();
                    }).fail(function (e) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (e.responseText != null) $("#edit_form_msg_txt").html(e.responseText);
                        else $("#edit_form_msg_txt").html(_app.constants.ajax.message.error);
                        $("#edit_form_msg").removeClass("m--hide").show();
                    });
            }
        },
        reset: {
            create: function() {
                createValidateForm.resetForm();
                var currentTime = moment();
                var roundMinutes = Math.ceil(currentTime.minute() / 15) * 15;
                var formated = currentTime.minute(roundMinutes).format(_app.constants.formats.timepickerJsMoment);
                $("#Add_Start").timepicker("setTime", formated);
                $("#Add_End").timepicker("setTime", formated);
                $("#Add_SectionId").prop("selectedIndex", 0).trigger("change");
                $("#Add_ClassroomId").prop("selectedIndex", 0).trigger("change");
            }
        },
        switch: {
            detailToEdit: function() {
                $("#edit-form input").prop("disabled", false);
                $("#edit-form select").prop("disabled", false);
                $("#btnEdit").hide();
                $("#btnDelete").hide();
                $("#btnSave").show();
                $("#btnCancelEdit").show();
            },
            editToDetail: function () {
                $("#btnCancelEdit").addLoader();
                $("#btnSave").prop("disabled", true);
                form.show.detail($("#Edit_Id").val(),
                    function () {
                        $("#btnCancelEdit").removeLoader();
                        $("#btnSave").prop("disabled", false);
                    });
            }
        },
        delete: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "La tutoría será eliminada permanentemente",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarla",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                allowOutsideClick: () => !swal.isLoading(),
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: "/profesor/tutorias/eliminar/post".proto().parseURL(),
                            type: "POST",
                            data: {
                                id: id
                            },
                            success: function (result) {
                                $("#edit_modal").modal("hide");
                                calendar.reload();
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "La tutoría ha sido eliminada con éxito",
                                    confirmButtonText: "Excelente"
                                });
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "Ocurrió un error al intentar eliminar la tutoría"
                                });
                            }
                        });
                    });
                }
            });
        }
    };

    var datatable = {
        load: function (id) {
            options.data.source.read.url = `/profesor/tutorias/${id}/alumnos/get`.proto().parseURL();
            studentsDatatable = $(".students-datatable").mDatatable(options);
        },
        destroy: function() {
            studentsDatatable.destroy();
            studentsDatatable = null;
        }
    }

    var events = {
        init: function() {
            $("#btnAdd").on("click",
                function () {
                    form.show.create();
                });
            $("#btnEdit").on("click",
                function() {
                    form.switch.detailToEdit();
                });
            $("#btnCancelEdit").on("click",
                function() {
                    form.switch.editToDetail();
                });
            $("#btnDelete").on("click",
                function() {
                    form.delete($("#Edit_Id").val());
                });
            $(".nav-link").on("shown.bs.tab", function (event) {
                studentsDatatable.adjustCellsWidth();
            });
        }
    }

    var select2 = {
        init: function() {
            this.sections.init();
            this.classrooms.init();
        },
        sections: {
            init: function() {
                $.ajax({
                    url: "/profesor/seccionescurso/get".proto().parseURL()
                }).done(function(result) {
                    $(".select2-sections").select2({
                        data: result.items,
                        placeholder: "Seccíón",
                        minimumResultsForSearch: -1
                    });
                });
            }
        },
        classrooms: {
            init: function() {
                $.ajax({
                    url: "/aulas/get".proto().parseURL()
                }).done(function (result) {
                    $("#Add_ClassroomId").select2({
                        data: result.items,
                        placeholder: "Aula",
                        dropdownParent: $("#add_modal .modal-body")
                    });
                    $("#Edit_ClassroomId").select2({
                        data: result.items,
                        placeholder: "Aula",
                        dropdownParent: $("#edit_modal .modal-body")
                    });
                });
            }
        }
    }

    var datepicker = {
        init: function () {
            var date = new Date();
            date.setDate(date.getDate());
            $(".date-picker").datepicker({
                startDate: date
            });
        }
    }

    var timepicker = {
        init: function () {
            $(".timepicker").timepicker().on("changeTime.timepicker", function(e) {
                var h = e.time.hours;
                var m = e.time.minutes;
                var mer = e.time.meridian;
                m += h * 60;
                if (mer === "AM") {
                    if (m === 360 || m === 405)
                        $(this).timepicker("setTime", "12:00 AM");
                    else if (m === 735 || m === 60)
                        $(this).timepicker("setTime", "7:00 AM");
                    else if (m > 180 && m < 420)
                        $(this).timepicker("setTime", "7:00 AM");
                    else if (m > 720 || m <= 180)
                        $(this).timepicker("setTime", "12:00 AM");
                }
            });
        }
    }

    var validate = {
        init: function() {
            createValidateForm = $("#add-form").validate({
                submitHandler: function(formElement, e) {
                    e.preventDefault();
                    form.submit.create(formElement);
                }
            });

            editValidateForm = $("#edit-form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.edit(formElement);
                }
            });
        }
    }

    return {
        init: function () {
            events.init();
            calendar.load();
            select2.init();
            datepicker.init();
            timepicker.init();
            validate.init();
        }
    }
}();

$(function () {
    Tutorial.init();
});