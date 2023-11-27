var InitApp = function () {

    var TeacherId = $("#TeacherId").val();
    var TermId = $("#TermId").val();

    var datatable = {
        assistance: {
            object: null,
            options: {
                serverSide: false,
                ajax: {
                    url: `/admin/reporte-docentes/detalle/asistencia/get`.proto().parseURL(),
                    type: "GET",
                    data: function (data) {
                        data.sectionId = $("#section_select2").val();
                    }
                },
                columns: [
                    {
                        data: "student",
                        title: "Estudiante"
                    },
                    {
                        data: "absences",
                        title: "Faltas"
                    },
                    {
                        data: "assisted",
                        title: "Asistidas"
                    },
                    {
                        data: "dictated",
                        title: "Dictadas"
                    }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Excel',
                        action: function (e, dt, node, config) {
                            window.location.href = `/admin/reporte-docentes/seccion/${$("#section_select2").val()}/control-asistencia-excel`.proto().parseURL();
                            //window.location.href = `/admin/reporte-docentes/detalle/${$("#section_select2").val()}/asistencia/excel`.proto().parseURL();
                        }
                    },
                    {
                        text: 'Pdf',
                        action: function (e, dt, node, config) {
                            window.location.href = `/admin/reporte-docentes/detalle/${$("#section_select2").val()}/asistencia/pdf?teacherId=${$("#TeacherId").val()}`.proto().parseURL();
                        }
                    }
                ]
            },
            reload: function () {
                if ($("#section_select2").val() !== null) {
                    if (this.object === null) {
                        datatable.assistance.object = $("#assistance-table").DataTable(this.options);
                    } else {
                        datatable.assistance.object.ajax.reload();
                    }
                }
            }
        },
        grade: {
            reload: function () {
                $("#grades_partial").html("");
                mApp.block("#grades_partial", {
                    message: "Cargando datos..."
                });

                $.ajax({
                    url: `/admin/reporte-docentes/detalle/notas/get?sectionId=${$('#section_select2').val()}`,
                    type: "GET",
                    dataType: "HTML"
                })
                    .done(function (e) {
                        $("#grades_partial").html(e);
                    });
            }
        }
    };

    var select2 = {
        course: {
            load: function () {
                $.ajax({
                    url: `/cursos/docente/${TeacherId}?termId=${TermId}`,
                    type: "GET"
                })
                    .done(function (e) {
                        $("#course_select2").select2({
                            placeholder: "Seleccionar cursos",
                            data: e
                        }).trigger("change");
                    });

            },
            events: {
                onChange: function () {
                    $("#course_select2").on("change", function () {
                        var id = $(this).val();
                        select2.sections.load(id);
                    });
                },
                init: function () {
                    this.onChange();
                }
            },
            init: function () {
                this.events.init();
                this.load();
            }
        },
        sections: {
            load: function (courseId) {
                $("#section_select2").empty();

                $.ajax({
                    url: `/secciones-por-curso/${courseId}/docente/${TeacherId}?termId=${TermId}`,
                    type: "GET"
                })
                    .done(function (e) {
                        $("#section_select2").select2({
                            placeholder: "Seleccionar secciones",
                            data: e
                        }).trigger("change");
                    });
            },
            events: {
                onChange: function () {
                    $("#section_select2").on("change", function () {
                        var value = $(this).val();
                        if (value !== null && value !== "") {
                            datatable.assistance.reload();
                            datatable.grade.reload();
                            select2.sectionGroups.load();
                        }
                    });
                },
                init: function () {
                    this.onChange();
                }
            },
            init: function () {
                this.events.init();
                this.load();
            }
        },
        dictatedClasses: {
            load: function () {
                var sectionId = $("#section_select2").val();
                var startDate = modal.cleanClasses.object.find("[name='StartDate']").val();
                var endDate = modal.cleanClasses.object.find("[name='EndDate']").val();
                var sectionGroupId = modal.cleanClasses.object.find("[name='SectionGroupId']").val();

                modal.cleanClasses.object.find("[name='ClassesId']").empty();

                $.ajax({
                    url: `/admin/reporte-docentes/get-clases-dictadas`,
                    data: {
                        sectionId: sectionId,
                        startDate: startDate,
                        endDate: endDate,
                        sectionGroupId: sectionGroupId
                    }
                })
                    .done(function (data) {

                        var html = '';

                        $.each(data, function (i, v) {
                            html += `<option value="${v.id}">${v.text}</option>`;
                        });

                        modal.cleanClasses.object.find("[name='ClassesId']").html(html);

                        modal.cleanClasses.object.find("[name='ClassesId']").selectpicker({
                            actionsBox: true,
                            selectAllText: 'Marcar todos',
                            deselectAllText: 'Desmarcar todos',
                            noneSelectedText: 'Seleccionar',
                            //width : '100%'
                            //size: 10,
                        });
                        modal.cleanClasses.object.find("[name='ClassesId']").selectpicker("refresh");
                        modal.cleanClasses.object.find("[name='ClassesId']").trigger("change");





                        //modal.cleanClasses.object.find("[name='ClassesId']").select2({
                        //    data: e,
                        //    dropdownParent: modal.cleanClasses.object,
                        //    placeholder: "Seleccionar clases a eliminar"
                        //}).val(null).trigger("change");
                    })
            },
            init: function () {
                this.load();
            }
        },
        sectionGroups: {
            load: function () {
                var sectionId = $("#section_select2").val();

                modal.cleanClasses.object.find("[name='SectionGroupId']").empty();

                $.ajax(({
                    url: `/get-sub-grupos-seccion`,
                    data: {
                        sectionId: sectionId
                        }
                }))
                    .done(function (e) {
                        modal.cleanClasses.object.find("[name='SectionGroupId']").select2({
                            data: e,
                            dropdownParent: modal.cleanClasses.object,
                            placeholder: "Seleccionar subgrupo",
                            allowClear: true
                        }).val(null).trigger("change");
                    })
            },
            events: {
                init: function () {
                    modal.cleanClasses.object.find("[name='SectionGroupId']").on("change", function () {
                        select2.dictatedClasses.load();
                    })
                }
            },
            init: function () {
                this.events.init();
            }
        },
        init: function () {
            select2.course.init();
            select2.sections.init();
            select2.sectionGroups.init();
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
                url: `/admin/carga-lectiva/get-horario/${$("#TeacherId").val()}`
            },
            eventAfterAllRender: function () {
                //$(".fc-prev-button").prop("disabled", false);
                //$(".fc-next-button").prop("disabled", false);
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

            calendar.object = $("#schedule").fullCalendar(calendar.defaultSettings);
        }
    };

    var events = {
        firstTime: true,
        onShowCalendarModal: function () {
            $("#show_schedule").click(function () {
                $("#modal_calendar").modal("show");
            });

            $('#modal_calendar').on('shown.bs.modal', function (e) {
                if (events.firstTime) {
                    events.firstTime = false;
                    calendar.load();
                }
            });
        },
        cleanGrades: function () {
            $("#clean_grades").on("click", function () {
                var sectionId = $("#section_select2").val();
                var course = $("#course_select2").select2('data')[0].text;
                var section = $("#section_select2").text();

                swal({
                    type: "warning",
                    text: `¿Seguro que desea eliminar las asistencias del curso ${course} - ${section}? Esta acción no es recuperable.`,
                    title: `Eliminar asistencias`,
                    confirmButtonText: "Aceptar",
                    showCancelButton: true,
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise(() => {
                            $.ajax({
                                type: "POST",
                                url: `/admin/reporte-docentes/detalle/${sectionId}/limpiar-asistencias`
                            })
                                .done(function (e) {
                                    datatable.assistance.reload();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: e,
                                        confirmButtonText: "Aceptar"
                                    });
                                })
                                .fail(function (e) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Aceptar",
                                        text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                    });
                                })
                        });
                    }
                });
            })
        },
        init: function () {
            this.onShowCalendarModal();
            this.cleanGrades();
        }
    };

    var modal = {
        cleanClasses: {
            object: $("#modal_clean_classes"),
            form: {
                object: $("#form_clean_classes").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();

                        var sectionId = $("#section_select2").val();
                        var course = $("#course_select2").select2('data')[0].text;
                        var section = $("#section_select2").text();

                        var formData = new FormData(formElement);

                        formData.append("SectionId", sectionId);

                        swal({
                            type: "warning",
                            text: `¿Seguro que desea eliminar las asistencias del curso ${course} - ${section}? Esta acción no es recuperable.`,
                            title: `Eliminar asistencias`,
                            confirmButtonText: "Aceptar",
                            showCancelButton: true,
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise(() => {
                                    $.ajax({
                                        type: "POST",
                                        url: `/admin/reporte-docentes/detalle/${sectionId}/borrar-asistencias-seleccionadas`,
                                        data: formData,
                                        contentType: false,
                                        processData: false
                                    })
                                        .done(function (e) {
                                            datatable.assistance.reload();
                                            select2.dictatedClasses.load();
                                            modal.cleanClasses.object.modal("hide");
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: e,
                                                confirmButtonText: "Aceptar"
                                            });
                                        })
                                        .fail(function (e) {
                                            swal({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                                confirmButtonText: "Aceptar",
                                                text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                            });
                                        })
                                });
                            }
                        });
                    }
                })
            },
            show: function () {
                $("#clean_classes").on("click", function () {
                    modal.cleanClasses.object.modal("show");
                })
            },
            onHidden: function () {
                modal.cleanClasses.object.on('hidden.bs.modal', function (e) {
                    modal.cleanClasses.form.object.resetForm();
                    modal.cleanClasses.object.find("[name='SectionGroupId']").val(null).trigger("change");
                    select2.dictatedClasses.load();
                })
            },
            init: function () {
                this.onHidden();
                this.show();
            }
        },
        init: function () {
            modal.cleanClasses.init();
        }
    }

    var datetime = {
        init: function () {
            modal.cleanClasses.object.find("[name='StartDate']").datepicker();
            modal.cleanClasses.object.find("[name='EndDate']").datepicker();

            modal.cleanClasses.object.find("[name='StartDate']").on("change", function () {
                select2.dictatedClasses.load();
            })

            modal.cleanClasses.object.find("[name='EndDate']").on("change", function () {
                select2.dictatedClasses.load();
            })
        }
    }

    return {
        init: function () {
            select2.init();
            events.init();
            modal.init();
            datetime.init();
        }
    };
}();

$(function () {
    InitApp.init();
});