var MedicalAppointment = function () {
    var fullCalendar;
    

    var doctorsSelect2 = function () {
        $.ajax({
            type: 'GET',
            url: `/doctores/get`.proto().parseURL(),
            success: function (data) {
                $("#doctorSelect2").select2({
                    data: data.items
                }).trigger('change');
            }
        });
        $("#doctorSelect2").on('change', function () {
            var did = $(this).val();
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
                eventRender: function(event, element) {
                    if (element.hasClass("fc-day-grid-event")) {
                        //element.data("content");
                        element.data("placement", "top");
                        mApp.initPopover(element);
                    } else if (element.hasClass("fc-time-grid-event")) {
                        element.find(".fc-title").append("<div class=\"fc-description\"></div>");


                    } else if (element.find(".fc-list-item-title").length !== 0) {
                        element.find(".fc-list-item-title").append("<div class=\"fc-description\"></div>");

                    }

                    var fc_title = element.find(".fc-title").clone().children().remove().end().text();

                    element.css("border-color", "#" + fc_title.proto().toRGB()).css("border-width", "2px");
                    element.find(".fc-content").css('cursor', 'pointer');
                },
                events: {
                    className: "m-fc-event--primary",
                    error: function() {
                        toastr.error("Ocurrió un problema con el servidor", "Error");
                    },
                    url: `/alumno/citas_medicas/doctor/${did}/citas/get`.proto().parseURL()
                },
                eventAfterAllRender: function() {
                    $(".fc-prev-button").prop("disabled", false);
                    $(".fc-next-button").prop("disabled", false);
                    mApp.unblock(".m-calendar__container");
                },
                eventClick: function (calEvent, jsEvent, view) {

                    if (calEvent.busy == true) {
                        toastr.error("No puede reservar una cita ocupada", _app.constants.toastr.title.error);
                    }
                    else {                        
                                swal({
                                    title: "¿Está seguro de reservar la cita médica?",
                                    text: "No puedes reservar otra cita médica hasta no cerrar tus citas pendientes",
                                    type: "warning",
                                    showCancelButton: true,
                                    confirmButtonText: "Si, reservarlo",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    cancelButtonText: "Cancelar"
                                }).then(function (result) {
                                    if (result.value) {
                                        $.ajax({
                                            url: `/alumno/citas_medicas/reservar_cita_medica/${calEvent.id}/post`.proto().parseURL(),
                                            type: "POST",
                                            success: function () {                                                
                                                if (fullCalendar !== null) {
                                                    $(fullCalendar).fullCalendar("destroy");
                                                    fullCalendar = null;
                                                }
                                                fullCalendar = $(".m-calendar__container").fullCalendar(defaultSettings);
                                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                            },
                                            error: function () {
                                                toastr.error("Tienes citas pendientes sin cerrar", _app.constants.toastr.title.error);
                                            }
                                        });
                                    }
                                });
                            
                            
                    }
                   

                    
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
                slotDuration: "00:10:00",
                timeFormat: "h(:mm)A"

            };
            
            fullCalendar = $(".m-calendar__container").fullCalendar(defaultSettings);
        });
        
    }
    
    

   
    //var modal = function () {
    //    $("#btnAdd").on('click', function () {
    //        $("#add-medicalappointment-modal").modal('show');
    //    });
    //}
    
    return {
        init: function () {
            //init();
            //modal();
            doctorsSelect2();
        }
    }
}();

$(function () {
    MedicalAppointment.init();
});