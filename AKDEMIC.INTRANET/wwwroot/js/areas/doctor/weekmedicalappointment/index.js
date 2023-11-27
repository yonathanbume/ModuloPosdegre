var MedicalCare= function () {
    var fullCalendar;
    var hide = $("#PersonalizedStudent");
    var hide2 = $("#PersonalizedStudent2");
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
                //element.data("content");
                element.data("placement", "top");
                mApp.initPopover(element);
            } else if (element.hasClass("fc-time-grid-event")) {
                element.find(".fc-title").append("<div class=\"fc-description\"></div>");


            } else if (element.find(".fc-list-item-title").length !== 0) {
                element.find(".fc-list-item-title").append("<div class=\"fc-description\"></div>");

            }

            var fc_title = element.find(".fc-title").clone().children().remove().end().text();
            element.find(".fc-description").append(`<button data-id='${event.id}' style='height: 13px;width: 22px;float: right;' class='btn btn-danger delete'><i style='font-size: 7px;' class='fa fa-trash'></i></button> <button data-id='${event.id}' style='height: 13px;width: 22px;float: right;margin-right: 3px;' class='btn btn-info edit'><i style='font-size: 7px;' class='fa fa-edit'></i></button> `);
            element.css("border-color", "#" + fc_title.proto().toRGB()).css("border-width", "2px");
            element.find(".fc-content").css('cursor', 'pointer');
            element.find(".delete").click(function (e) {
                e.stopPropagation();
                var id = $(this).data("id");
                swal({
                    title: "¿Está seguro?",
                    text: "La cita será eliminada permanentemente",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: `/doctor/horario-citas/eliminar/${id}`.proto().parseURL(),
                                type: "POST",
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "La cita se eliminó correctamente",
                                        confirmButtonText: "Excelente"
                                    });
                                    if (fullCalendar !== null) {
                                        $(fullCalendar).fullCalendar("destroy");
                                        fullCalendar = null;
                                    }
                                    fullCalendar = $(".m-calendar__container").fullCalendar(defaultSettings);
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "No se pudo eliminar la cita"
                                    });
                                }
                            });
                        })
                    }
                });
            });
            element.find(".edit").click(function (e) {
                e.stopPropagation();
                var id = $(this).data("id");
                $("#edit-psicologyappointment-modal").modal('show');

                $.ajax({
                    type: 'GET',
                    url: `/doctor/horario-citas/obtener/${id}`.proto().parseURL(),
                    success: function (data) {
                        if (data.studentid !== null) {
                            $("#swichPersonalized2").trigger('click');
                        }                                          
                        $("#modal-edit-psicologyappointment select[id='StudentId2']").val(data.studentid).trigger('change');
                        $("#modal-edit-psicologyappointment input[id='Id']").val(data.id);
                        $("#modal-edit-psicologyappointment input[id='DateMedicalCare2']").val(data.datemedicalcare);
                        $("#modal-edit-psicologyappointment input[id='TimeMedicalCare2']").val(data.timemedicalcare);
                    }

                });

            });
        },
        events: {
            className: "m-fc-event--primary",
            error: function () {
                toastr.error("Ocurrió un problema con el servidor", "Error");
            },
            url: ("/doctor/horario-citas/get").proto().parseURL()
        },
        eventAfterAllRender: function () {
            $(".fc-prev-button").prop("disabled", false);
            $(".fc-next-button").prop("disabled", false);
            mApp.unblock(".m-calendar__container");
        },
        eventClick: function (calEvent, jsEvent, view) {
            //mApp.blockPage();
            if (calEvent.studentId === _app.constants.guid || calEvent.studentId === null)
                toastr.warning("No existe un estudiante para la cita seleccionada", _app.constants.toastr.title.warning);
            else if (calEvent.attended === true)
                toastr.warning("La cita ha sido atendida", _app.constants.toastr.title.warning);            
            else {
                mApp.block(".m-portlet", { type: "loader", message: "Cargando..." });    
                location.href = `/doctor/horario-citas/detalle-historico/${calEvent.id}/${calEvent.studentId}`.proto().parseURL();
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
    

    var init = function () {
        
        fullCalendar = $(".m-calendar__container").fullCalendar(defaultSettings);        
    }
    var modal = function () {
        $("#btnAdd").on('click', function () {
            $("#add-medicalappointment-modal").modal('show');
        });
        $("#edit-psicologyappointment-modal").on("hidden.bs.modal", function () {
            $("#swichPersonalized2").trigger('click');
            $("#edit-psicologyappointment-modal").modal('hide');
        });
    };
    var select2Students = function () {
        $('#StudentId').select2({
            allowClear: true,
            placeholder: "Buscar..",
            ajax: {
                type: 'GET',
                url: ("/alumnosv2/get").proto().parseURL(),
                delay: 1000,
                data: function (params) {
                    return {
                        searchValue: params.term
                    };
                },
                processResults: function (data) {
                    return {
                        results: data.results
                    };
                    //return {
                    //    results: $.map(result, function (item) {
                    //        return {
                    //            text: item.text,
                    //            id: item.id
                    //        }
                    //    })
                    //};
                },
                escapeMarkup: function (markup) {
                    return markup;
                },
                minimumInputLength: 1
            }
        });

        $('#StudentId2').select2({
            allowClear: true,
            placeholder: "Buscar..",
            ajax: {
                type: 'GET',
                url: ("/alumnosv2/get").proto().parseURL(),
                delay: 1000,
                data: function (params) {
                    return {
                        searchValue: params.term
                    };
                },
                processResults: function (data) {
                    return {
                        results: data.results
                    };
                    //return {
                    //    results: $.map(result, function (item) {
                    //        return {
                    //            text: item.text,
                    //            id: item.id
                    //        }
                    //    })
                    //};
                },
                escapeMarkup: function (markup) {
                    return markup;
                },
                minimumInputLength: 1
            }
        }); 
       
        
        $("#TimeMedicalCare").timepicker();         
        $("#TimeMedicalCare2").timepicker();    
        $("#modal-create-medicalappointment").validate({
            submitHandler: function (form, event) {
                $( ":submit" ).addLoader();
                $.ajax({
                    type: 'POST',
                    url: ("/doctor/horario-citas/agregar-cita/post").proto().parseURL(),
                    data: $("#modal-create-medicalappointment").serialize(),
                    success: function (data) {                        
                        toastr.success(_app.constants.toastr.message.success.create, _app.constants.toastr.title.update);
                        $("#add-medicalappointment-modal").modal("hide");
                        if (fullCalendar !== null) {
                            $(fullCalendar).fullCalendar("destroy");
                            fullCalendar = null;
                        }                                               
                        fullCalendar = $(".m-calendar__container").fullCalendar(defaultSettings);
                         
                    },
                    error: function (data) {
                        toastr.error(data.responseText, _app.constants.toastr.title.error);
                    },
                    complete : function (){
                        $( ":submit" ).removeLoader();       
                    }
                });

            }

        });

        $("#modal-edit-psicologyappointment").validate({
            submitHandler: function (form, event) {                
                $.ajax({
                    type: 'POST',
                    url: ("/doctor/horario-citas/editar-cita/post").proto().parseURL(),
                    data: $("#modal-edit-psicologyappointment").serialize(),
                    success: function (data) {
                        toastr.success(_app.constants.toastr.message.success.update, _app.constants.toastr.title.update);
                        $("#edit-psicologyappointment-modal").modal("hide");
                        if (fullCalendar !== null) {
                            $(fullCalendar).fullCalendar("destroy");
                            fullCalendar = null;
                        }
                        fullCalendar = $(".m-calendar__container").fullCalendar(defaultSettings);

                    },
                    error: function (data) {
                        toastr.error(data.responseText, _app.constants.toastr.title.error);
                    }
                });

            }

        });
    }
    var Personalized = function () {                

        $("#swichPersonalized").on('click', function () {
            if ($(this).is(":checked")) {
                hide.find("#StudentId").attr("name", "StudentId");
                hide.css("display", "block");
                
            } else {    
                hide.find("#StudentId").attr("name", "");
                hide.css("display", "none");
                
            }
        });

        $("#swichPersonalized2").on('click', function () {
            if ($(this).is(":checked")) {
                hide2.find("#StudentId2").attr("name", "StudentId");
                hide2.css("display", "block");

            } else {
                hide2.find("#StudentId2").attr("name", "");
                hide2.css("display", "none");

            }
        });

        $("#DateMedicalCare").datepicker();
        $("#DateMedicalCare2").datepicker();
        
    }
    return {
        init: function () {
            init();
            modal();
            select2Students();
            Personalized();
        }
    }
}();

$(function () {
    MedicalCare.init();
});