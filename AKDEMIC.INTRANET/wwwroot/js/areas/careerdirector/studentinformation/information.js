var InitApp = function () {

    var studentid = $("#Id").val();

    var events = {
        return: {
            load: function () {
                $(".index-return").on("click", function () {

                    $("#option-block").removeClass("m--hide");
                    $("#information-block").addClass("m--hide");
                    $("#information-block").html("");

                    pages.clear();
                    //mApp.block("#information-block");

                    //$.ajax({
                    //    url: (`/director-carrera/alumnos/informacion/opciones`).proto().parseURL()
                    //}).done(function (data) {
                    //    pages.clear();

                    //    $("#information-block").html(data);

                    //    events.options.load();
                    //}).fail(function () {
                    //    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    //}).always(function () {
                    //    mApp.unblock("#information-block");
                    //});
                });
            }
        },
        options: {
            load: function () {
                //events.return.load();

                $(".general-data").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/director-carrera/alumnos/informacion/${studentid}/datos-generales`).proto().parseURL()
                    }).done(function (data) {
                        $("#information-block").html(data);

                        events.information.load();
                        pages.general.init();
                    }).fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#information-block");
                    });
                });

                $(".password").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/director-carrera/alumnos/informacion/${studentid}/cambiar-clave`).proto().parseURL()
                    }).done(function (data) {
                        $("#information-block").html(data);

                        events.password.load();
                        pages.password.init();
                    }).fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#information-block");
                    });
                });

                $(".user-procedures").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/director-carrera/alumnos/informacion/${studentid}/tramites`).proto().parseURL()
                    }).done(function (data) {
                        $("#information-block").html(data);

                        events.information.load();

                        pages.procedures.init();
                    }).fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#information-block");
                    });
                });

                $(".enrollment").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/director-carrera/alumnos/informacion/${studentid}/matricula-actual`).proto().parseURL()
                    }).done(function (data) {
                        $("#information-block").html(data);

                        events.return.load();
                        pages.enrollment.init();
                    }).fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#information-block");
                    });
                });

                $(".schedule").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/director-carrera/alumnos/informacion/${studentid}/horario`).proto().parseURL()
                    }).done(function (data) {
                        $("#information-block").html(data);
                        events.return.load();
                        pages.schedule.init();
                    }).fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#information-block");
                    });
                });

                $(".academic-history").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/director-carrera/alumnos/informacion/${studentid}/historial`).proto().parseURL()
                    }).done(function (data) {
                        $("#information-block").html(data);
                        events.return.load();
                        pages.academichistory.init();
                    }).fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#information-block");
                    });
                });

                $(".assistance-history").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/director-carrera/alumnos/informacion/${studentid}/asistencia`).proto().parseURL()
                    }).done(function (data) {
                        $("#information-block").html(data);
                        events.return.load();
                        pages.assistancehistory.init();
                    }).fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#information-block");
                    });
                });

                $(".report-card").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/director-carrera/alumnos/informacion/${studentid}/boleta-notas`).proto().parseURL()
                    }).done(function (data) {
                        $("#information-block").html(data);
                        events.return.load();
                        pages.reportcard.init();
                    }).fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#information-block");
                    });
                });

                $(".curriculum-progress").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/director-carrera/alumnos/informacion/${studentid}/situacion-academica`).proto().parseURL()
                    }).done(function (data) {
                        $("#information-block").html(data);
                        events.return.load();
                        pages.curriculumprogress.init();
                    }).fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#information-block");
                    });
                });

                $(".procedure-request").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/director-carrera/alumnos/informacion/${studentid}/solicitud-tramites`).proto().parseURL()
                    }).done(function (data) {
                        $("#information-block").html(data);
                        events.return.load();
                        pages.procedurerequest.init();
                    }).fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#information-block");
                    });
                });

                $(".observations").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/director-carrera/alumnos/informacion/${studentid}/observaciones`).proto().parseURL()
                    }).done(function (data) {
                        $("#information-block").html(data);
                        events.return.load();
                        pages.observations.init();
                    }).fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#information-block");
                    });
                });
            }
        },
        information: {
            load: function () {
                events.return.load();
            }
        },
        password: {
            load: function () {
                events.return.load();
            }
        },
        procedures: {
            load: function () {
                events.return.load();


            }
        },
        init: function () {
            events.options.load();
        }
    };

    var pages = {
        general: {
            form: {
                object: null,
                init: function () {
                    this.object = $("#edit-form").validate({
                        messages: {
                            Name: {
                                pattern: "Por favor el apellido solo debe contener letras."
                            },
                            PaternalSurname: {
                                pattern: "Por favor el apellido solo debe contener letras."
                            },
                            MaternalSurname: {
                                pattern: "Por favor el apellido solo debe contener letras."
                            },
                            PhoneNumber: {
                                pattern: "El campo no tiene el formato correcto (Ejemplo: 989419189 o 3255564 )"
                            },
                            Dni: {
                                pattern: "Por favor el Dni solo debe contener números.",
                                maxlength: "Por favor el Dni solo debe 8 contener dígitos.",
                                minlength: "Por favor el Dni debe 8 contener dígitos.",
                            }
                        },
                        submitHandler: function (form) {

                            var formData = new FormData($(form)[0]);

                            mApp.block("#information-block");

                            $.ajax({
                                url: $(form).attr("action"),
                                type: "POST",
                                data: formData,
                                contentType: false,
                                processData: false
                            })
                                .done(function () {
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                                    $("#student-picture").attr("src", $("#current-picture").attr("src"));

                                    var fullname = $("#MaternalSurname").val() !== "" ?
                                        $("#PaternalSurname").val() + " " + $("#MaternalSurname").val() + ", " + $("#Name")
                                        : $("#PaternalSurname").val() + ", " + $("#Name");

                                    $("#student-picture").text(fullname);

                                    $("#student-code").val($("#UserName").val());

                                    $("#student-career").val($("#SelectedCareer").text());

                                    $("#student-dni").val($("#Dni").val());
                                })
                                .fail(function (e) {
                                    //toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                    if (e.responseText !== null)
                                        $("#alert-text").html(e.responseText);
                                    else
                                        $("#alert-text").html(_app.constants.toastr.message.error.task);

                                    $("#m-form_alert").removeClass("m--hide").show();
                                })
                                .always(function () {
                                    mApp.unblock("#information-block");
                                });
                        }
                    });
                }
            },
            select: {
                init: function () {
                    this.faculties.init();
                },
                faculties: {
                    init: function () {
                        $.ajax({
                            url: "/facultades/get".proto().parseURL()
                        }).done(function (result) {

                            $(".select2-faculties").select2({
                                placeholder: "Facultades",
                                minimumInputLength: -1,
                                data: result.items
                            })
                                .val($("#FacultyIdValue").val())
                                .trigger("change")
                                .on("change", function () {
                                    pages.general.select.careers.init($(this).val());
                                });

                            pages.general.select.careers.init($("#FacultyIdValue").val());
                        });
                    }
                },
                careers: {
                    init: function (facultyId) {
                        $(".select2-careers").prop("disabled", true);
                        $(".select2-careers").empty();

                        $.ajax({
                            url: `/carreras/get?fid=${facultyId}`.proto().parseURL()
                        }).done(function (data) {

                            $(".select2-careers").select2({
                                placeholder: "Seleccione una carrera",
                                minimumInputLength: -1,
                                data: data.items
                            });

                            if (data.items.length) {
                                $(".select2-careers").prop("disabled", false);

                                var careerId = $("#CareerId").val();

                                if (careerId !== null && careerId !== "") {
                                    $(".select2-careers").val(careerId).trigger("change");
                                    $("#CareerId").val(null).trigger("change");
                                }
                            }
                        });
                    }
                }
            },
            fileInput: {
                init: function () {
                    this.picture.init();
                },
                picture: {
                    init: function () {
                        $("#Picture").on("change",
                            function (e) {
                                var tgt = e.target || window.event.srcElement,
                                    files = tgt.files;
                                // FileReader support
                                if (FileReader && files && files.length) {
                                    var fr = new FileReader();
                                    fr.onload = function () {
                                        $("#current-picture").attr("src", fr.result);
                                    };
                                    fr.readAsDataURL(files[0]);
                                }
                                // Not supported
                                else {
                                    console.log("File Reader not supported.");
                                }
                            });
                    }
                }
            },
            datepicker: {
                init: function () {
                    $("#BirthDate").datepicker("setEndDate", moment().format(_app.constants.formats.datepickerJsMoment));
                }
            },
            init: function () {
                this.select.init();
                this.form.init();
                this.fileInput.init();
                this.datepicker.init();
            }
        },
        password: {
            form: {
                object: null,
                init: function () {
                    pages.password.form.object = $("#password-form").validate({
                        rules: {
                            ConfirmPassword: {
                                equalTo: "#Password"
                            }
                        },
                        submitHandler: function (form, e) {
                            e.preventDefault();

                            mApp.block("#information-block");

                            $.ajax({
                                url: $(form).attr("action"),
                                type: "POST",
                                data: $(form).serialize()
                            })
                                .done(function () {
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                })
                                .fail(function (e) {
                                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                })
                                .always(function () {
                                    mApp.unblock("#information-block");
                                });
                        }
                    });
                }
            },
            init: function () {
                pages.password.form.init();
            }
        },
        procedures: {
            datatable: {
                procedures: {
                    object: null,
                    options: getSimpleDataTableConfiguration({
                        url: `/director-carrera/alumnos/informacion/${studentid}/tramites/get`.proto().parseURL(),
                        data: function (data) {
                            data.search = $("#search").val();
                        },
                        pageLength: 50,
                        orderable: [],
                        columns: [
                            {
                                data: "procedure.code"
                            },
                            {
                                data: "procedure.name"
                            },
                            {
                                data: "createdFormattedDate"
                            },
                            {
                                data: "term.name"
                            },
                            {
                                data: "statusString"
                            },
                            {
                                data: "dependency.name"
                            }
                        ]
                    }),
                    init: function () {
                        pages.procedures.datatable.procedures.object = $("#procedures_table").DataTable(pages.procedures.datatable.procedures.options);
                        $("#search").doneTyping(function () {
                            pages.procedures.datatable.procedures.object.draw();
                        });
                    },
                    load: function () {
                        pages.procedures.datatable.procedures.object.draw();
                    }
                }
            },
            init: function () {
                pages.procedures.datatable.procedures.init();
            }
        },
        enrollment: {
            datatable: {
                courses: {
                    object: null,
                    options: getSimpleDataTableConfiguration({
                        url: `/director-carrera/alumnos/informacion/${studentid}/matricula-actual/get`.proto().parseURL(),
                        data: function (data) {
                            data.search = $("#search").val();
                        },
                        pageLength: 50,
                        orderable: [],
                        columns: [
                            {
                                data: "courseTerm.course.code"
                            },
                            {
                                data: "courseTerm.course.name"
                            },
                            {
                                data: "code"
                            },
                            {
                                data: "studentsCount"
                            },
                            {
                                data: "courseTerm.course.credits"
                            },
                            {
                                data: null,
                                render: function (data) {
                                    return `<button data-object="${data.proto().encode()}" class="btn btn-info btn-sm m-btn m-btn--icon m-btn--icon-only btn-change"><i class="la la-eye"></i></button>` ;
                                }
                            }
                        ]
                    }),
                    init: function () {
                        pages.enrollment.datatable.courses.object = $("#enrollment_table").DataTable(pages.enrollment.datatable.courses.options);

                        $("#enrollment_table").on('click', '.btn-change', function (e) {
                            var object = $(this).data("object");

                            object = object.proto().decode();

                            $("#enrollment-section-modal").modal("toggle");

                            $("#currentId").val(object.studentSectionId);

                            $("#currentSection").val(object.code);

                            pages.enrollment.select.section.load(object.studentSectionId);
                        });
                    },
                    reload: function () {
                        pages.enrollment.datatable.courses.object.ajax.reload();
                    }
                }
            },
            select: {
                section: {
                    init: function () {
                        $("#newSectionId").select2({
                            placeholder: "Seleccione una sección"
                        });
                    },
                    load: function (id) {

                        $.ajax({
                            url: `/director-carrera/alumnos/informacion/matricula-actual/${id}/secciones-disponibles`.proto().parseURL()
                        }).done(function (data) {
                            $("#newSectionId").empty();
                            $("#newSectionId").select2({
                                placeholder: "Seleccione una sección",
                                data: data.items,
                                minimumResultsForSearch: -1
                            });
                        });
                    }
                },              
                init: function () {
                    select.section.init();
                }
            },
            form: {
                section: {
                    object: null,
                    clear: function () {
                        this.object.resetForm();
                    },
                    init: function () {
                        this.object = $("#enrollment-section-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();

                                mApp.block("#enrollment-section-form");

                                var formData = new FormData($(form)[0]);

                                $.ajax({
                                    url: $(form).attr("action"),
                                    type: "POST",
                                    data: formData,
                                    contentType: false,
                                    processData: false
                                })
                                    .done(function () {
                                        $(".modal").modal("hide");
                                        $(".m-alert").addClass("m--hide");

                                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                                        pages.enrollment.form.section.clear();

                                        pages.enrollment.datatable.courses.reload();
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#enrollment-section-modal-text").html(error.responseText);
                                        else $("#enrollment-section-modal-text").html(_app.constants.ajax.message.error);

                                        $("#enrollment-section-modal-alert").removeClass("m--hide").show();
                                    })
                                    .always(function () {
                                        mApp.unblock("#enrollment-section-form");
                                    });
                            }
                        });

                        $("#sectionFile").on('change', function (event) {
                            $(this).valid();
                            $(this).next('.custom-file-label').html(event.target.files[0].name);
                        });
                    }
                }
            },
            init: function () {        
                pages.enrollment.datatable.courses.init();
                pages.enrollment.select.section.init();
                pages.enrollment.form.section.init();
            }
        },
        schedule: {
            fullcalendar: {
                object: null,
                settings: {
                    allDaySlot: false,
                    aspectRatio: 2,
                    businessHours: {
                        dow: [1, 2, 3, 4, 5, 6],
                        end: "24:00",
                        start: "07:00"
                    },
                    columnFormat: "dddd",
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
                        url: `/director-carrera/alumnos/informacion/${studentid}/horario/get`.proto().parseURL()
                    },
                    //eventClick: function (calEvent, jsEvent, view) {
                    //    form.detail.load(calEvent.id);
                    //},
                    firstDay: 1,
                    header: false,
                    //contentHeight: 725,
                    height: 730,
                    locale: "es",
                    maxTime: "24:00:00",
                    minTime: "06:00:00",
                    monthNames: monthNames,
                    dayNames: dayNames,
                    monthNamesShort: monthNamesShort,
                    dayNamesShort: dayNamesShort,
                    slotDuration: "00:30:00",
                    timeFormat: "h(:mm)A"
                },
                init: function () {
                    pages.schedule.fullcalendar.object = $("#schedule-calendar").fullCalendar(pages.schedule.fullcalendar.settings);
                }
            },
            init: function () {
                pages.schedule.fullcalendar.init();
            }
        },
        academichistory: {
            ajax: function () {
                mApp.block("#courses-block");
                var term = $('#select-term').val();

                $.ajax({
                    type: "GET",
                    url: `/director-carrera/alumnos/informacion/${studentid}/historial/${term}/notas`.proto().parseURL()
                }).done(function (data) {

                    $('#enrolled-courses-container').html(data);

                    $("[data-toggle='m-tooltip']").tooltip();

                    $("[rel='m-tooltip']").tooltip();

                    $("#lblAcademicYear").text($('#summaryAcademicYear').val());
                    $("#lblSectionCount").text($('#summarySectionCount').val());
                    $("#lblCreditSum").text($('#summaryCreditSum').val());

                }).fail(function () {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                }).always(function () {
                    mApp.unblock("#courses-block");
                });
            },
            init: function () {
                $('#select-term').on('change', function (e) {
                    $("#lblAcademicYear").text('-');
                    $("#lblSectionCount").text('-');
                    $("#lblCreditSum").text('-');

                    pages.academichistory.ajax();
                });

                if ($('#select-term').val()) {
                    pages.academichistory.ajax();
                }
            }
        },
        assistancehistory: {
            datatable: {
                courses: {
                    object: null,

                    options: getSimpleDataTableConfiguration({
                        pageLength: 10,
                        orderable: [],
                        columns: [
                            {
                                data: "courseName"
                            },
                            {
                                data: "classCount"
                            },
                            {
                                data: "dictated"
                            },
                            {
                                data: "assisted"
                            },
                            {
                                data: "absences"
                            },
                            {
                                data: null,
                                render: function (data) {
                                    return `<div class="table-options-section">
                                    <button data-id="${data.sectionId}" class="btn btn-info btn-sm m-btn m-btn--icon m-btn--icon-only btn-course-detail" title=""><i class="la la-eye"></i></button> 
                                    </div>`;
                                }
                            }
                        ]
                    }),
                    load: function () {
                        var term = $("#select-term").val();
                        var url = `/director-carrera/alumnos/informacion/${studentid}/asistencia/${term}/cursos`.proto().parseURL();

                        if (pages.assistancehistory.datatable.courses.object !== undefined && pages.assistancehistory.datatable.courses.object !== null) {
                            pages.assistancehistory.datatable.courses.object.ajax.url(url).load();
                        } else {
                            pages.assistancehistory.datatable.courses.options.ajax.url = url;
                            pages.assistancehistory.datatable.courses.object = $("#course_assistance_table").DataTable(pages.assistancehistory.datatable.courses.options);
                        }
                    },
                    init: function () {
                        var term = $("#select-term").val();
                        var url = `/director-carrera/alumnos/informacion/${studentid}/asistencia/${term}/cursos`.proto().parseURL();

                        pages.assistancehistory.datatable.courses.options.ajax.url = url;
                        pages.assistancehistory.datatable.courses.object = $("#course_assistance_table").DataTable(pages.assistancehistory.datatable.courses.options);
                    }
                },
                assistance: {
                    object: null,
                    options: getSimpleDataTableConfiguration({
                        url: null,
                        //data: function (data) {
                        //},
                        pageLength: 50,
                        orderable: [],
                        columns: [
                            {
                                data: "week"
                            },
                            {
                                data: "sessionNumber"
                            },
                            {
                                data: "date"
                            },
                            {
                                data: "weekDay"
                            },
                            {
                                data: "isAbsent"
                            }
                        ]
                    }),
                    load: function (sectionid) {
                        $("#assistance_table_block").removeClass("m--hide");
                        var url = `/director-carrera/alumnos/informacion/${studentid}/asistencia/${sectionid}/detalle`.proto().parseURL();

                        if (pages.assistancehistory.datatable.assistance.object !== undefined && pages.assistancehistory.datatable.assistance.object !== null) {
                            pages.assistancehistory.datatable.assistance.object.ajax.url(url).load();
                        } else {
                            pages.assistancehistory.datatable.assistance.options.ajax.url = url;
                            pages.assistancehistory.datatable.assistance.object = $("#assistance_table").DataTable(pages.assistancehistory.datatable.assistance.options);
                        }
                    }
                }
            },
            init: function () {
                if ($('#select-term').val()) {
                    pages.assistancehistory.datatable.courses.load();

                    $("#select-term").on("change", function () {
                        pages.assistancehistory.datatable.courses.load();
                        $("#assistance_table_block").addClass("m--hide");
                    });

                    $("#course_assistance_table")
                        .on("click", ".btn-course-detail", function () {
                            var id = $(this).data("id");
                            pages.assistancehistory.datatable.assistance.load(id);
                        });
                }
            },
            clear: function () {
                pages.assistancehistory.datatable.assistance.object = null;
                pages.assistancehistory.datatable.courses.object = null;
            }
        },
        reportcard: {
            datatable: {
                courses: {
                    object: null,
                    options: getSimpleDataTableConfiguration({
                        pageLength: 10,
                        orderable: [],
                        columns: [
                            {
                                data: "curriculum"
                            },
                            {
                                data: "year"
                            },
                            {
                                data: "code"
                            },
                            {
                                data: "course"
                            },
                            {
                                data: "section"
                            },
                            {
                                data: "credits"
                            },
                            {
                                data: "theoreticalHours"
                            },
                            {
                                data: "practicalHours"
                            },
                            {
                                data: "grade"
                            },
                            {
                                data: "status"
                            },
                            {
                                data: "teacher"
                            }
                        ]
                    }),
                    load: function () {
                        var term = $("#select-term").val();
                        var url = `/director-carrera/alumnos/informacion/${studentid}/boleta-notas/${term}/cursos`.proto().parseURL();

                        if (pages.reportcard.datatable.courses.object !== undefined && pages.reportcard.datatable.courses.object !== null) {
                            pages.reportcard.datatable.courses.object.ajax.url(url).load();
                        } else {
                            pages.reportcard.datatable.courses.options.ajax.url = url;
                            pages.reportcard.datatable.courses.object = $("#card_table").DataTable(pages.reportcard.datatable.courses.options);
                        }
                    },
                    init: function () {
                        var term = $("#select-term").val();
                        var url = `/director-carrera/alumnos/informacion/${studentid}/boleta-notas/${term}/cursos`.proto().parseURL();

                        pages.reportcard.datatable.courses.options.ajax.url = url;
                        pages.reportcard.datatable.courses.object = $("#card_table").DataTable(pages.reportcard.datatable.courses.options);
                    }
                }
            },
            init: function () {
                if ($('#select-term').val()) {
                    pages.reportcard.datatable.courses.load();

                    $("#reportcard-link").prop("href", `/academico/constancias/notas/${studentid}/${$('#select-term').val()}`.proto().parseURL());

                    $("#select-term").on("change", function () {
                        pages.reportcard.datatable.courses.load();

                        $("#reportcard-link").prop("href", `/academico/constancias/notas/${studentid}/${$('#select-term').val()}`.proto().parseURL());
                    });
                }

                $("#select-term").on("change", function () {
                    pages.reportcard.datatable.courses.load();
                });
            },
            clear: function () {
                pages.reportcard.datatable.courses.object = null;
            }
        },
        curriculumprogress: {
            datatable: {
                courses: {
                    object: null,
                    options: {
                        responsive: true,
                        processing: true,
                        serverSide: false,
                        filter: false,
                        lengthChange: false,
                        paging: false,
                        ordering: false,
                        orderMulti: false,
                        columnDefs: [
                            { "orderable": false, "targets": [] }
                        ],
                        language: {
                            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>",
                            "lengthMenu": "",
                            "infoEmpty": "",
                            "zeroRecords": "No existen registros",
                            "info": "",
                            "infoFiltered": "_MAX_ / _TOTAL_",
                            "paginate": {
                                "next": ">>",
                                "previous": "<<"
                            }
                        },
                        ajax: {
                            url: `/director-carrera/alumnos/informacion/${studentid}/situacion-academica/cursos`.proto().parseURL(),
                            type: "GET",
                            dataType: "JSON"
                        },
                        orderable: [],
                        columns: [
                            {
                                data: "year"
                            },
                            {
                                data: "code"
                            },
                            {
                                data: "course"
                            },
                            {
                                data: "credits"
                            },
                            {
                                data: null,
                                render: function (data) {
                                    return data.tries === 0 ? " - " : data.tries;
                                }
                            },
                            {
                                data: null,
                                render: function (data) {
                                    return data.tries === 0 ? " - " : (data.grade < 11 ? `<span style="color: red;">${data.grade}</span>` : data.grade);
                                }
                            },
                            {
                                data: null,
                                render: function (data) {
                                    return data.tries === 0 ? " - " : data.term;
                                }
                            },
                            {
                                data: "status"
                            }
                        ]
                    },
                    init: function () {
                        pages.curriculumprogress.datatable.courses.object = $("#curriculum_table").DataTable(pages.curriculumprogress.datatable.courses.options);
                    }
                },
                elective: {
                    object: null,
                    options: {
                        responsive: true,
                        processing: true,
                        serverSide: false,
                        filter: false,
                        lengthChange: false,
                        paging: false,
                        ordering: false,
                        orderMulti: false,
                        columnDefs: [
                            { "orderable": false, "targets": [] }
                        ],
                        language: {
                            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>",
                            "lengthMenu": "",
                            "infoEmpty": "",
                            "zeroRecords": "No existen registros",
                            "info": "",
                            "infoFiltered": "_MAX_ / _TOTAL_",
                            "paginate": {
                                "next": ">>",
                                "previous": "<<"
                            }
                        },
                        ajax: {
                            url: `/director-carrera/alumnos/informacion/${studentid}/situacion-academica/electivos`.proto().parseURL(),
                            type: "GET",
                            dataType: "JSON"
                        },
                        orderable: [],
                        columns: [
                            {
                                data: "year"
                            },
                            {
                                data: "code"
                            },
                            {
                                data: "course"
                            },
                            {
                                data: "credits"
                            },
                            {
                                data: null,
                                render: function (data) {
                                    return data.tries === 0 ? " - " : data.tries;
                                }
                            },
                            {
                                data: null,
                                render: function (data) {
                                    return data.tries === 0 ? " - " : (data.grade < 11 ? `<span style="color: red;">${data.grade}</span>` : data.grade);
                                }
                            },
                            {
                                data: null,
                                render: function (data) {
                                    return data.tries === 0 ? " - " : data.term;
                                }
                            },
                            {
                                data: "status"
                            }
                        ]
                    },
                    init: function () {
                        pages.curriculumprogress.datatable.elective.object = $("#elective_table").DataTable(pages.curriculumprogress.datatable.elective.options);
                    }
                }
            },
            init: function () {
                pages.curriculumprogress.datatable.courses.init();
                pages.curriculumprogress.datatable.elective.init();
            },
            clear: function () {
                pages.curriculumprogress.datatable.courses.object = null;
                pages.curriculumprogress.datatable.elective.object = null;
            }
        },
        procedurerequest: {
            events: {
                init: function () {
                    $(".reservation-request").on("click", function () {
                        swal({
                            title: "Registrar reserva de matrícula",
                            text: "Se registrará una nueva reserva de matrícula a partir de la fecha actual.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Si, reservar",
                            confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                            cancelButtonText: "Cancelar"
                        }).then(function (result) {
                            if (result.value) {
                                $.ajax({
                                    url: `/director-carrera/alumnos/informacion/${$("#Id").val()}/solicitud-tramites/reserva`.proto().parseURL(),
                                    type: "POST",
                                }).done(function () {
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                    datatable.students.load();
                                }).fail(function (e) {
                                    if (e.responseText !== null && e.responseText !== "") {
                                        toastr.error(e.responseText, _app.constants.toastr.title.error);
                                    }
                                    else {
                                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                    }
                                });
                            }
                        });
                    });
                }
            },
            select: {
                courses: {
                    init: function () {
                        var id = $("#Id").val();

                        $.ajax({
                            url: `/director-carrera/alumnos/informacion/${id}/cursos-pendientes`.proto().parseURL()
                        }).done(function (data) {
                            $("#evaluationCourseSelect").select2({
                                data: data.items,
                                minimumResultsForSearch: -1
                            });
                        });
                    }
                },
                academicPrograms: {
                    init: function () {
                        $.ajax({
                            url: `/programas-academico/get`.proto().parseURL(),
                            data: {
                                curriculumId: $("#CurriculumId").val()
                            }
                        }).then(function (data) {
                            $("#academicProgramSelect").select2({
                                data: data.items,
                                dropdownParent: $("#request-program-modal")
                            });

                            $("#academicProgramSelect").val($("#AcademicProgramId").val()).trigger("change");
                        });
                    }
                }
            },
            form: {
                evaluation: {
                    object: null,
                    clear: function () {
                        pages.procedurerequest.form.evaluation.object.resetForm();
                        //$("#Special").prop("checked", false);
                    },
                    init: function () {
                        pages.procedurerequest.form.evaluation.object = $("#evaluation-request-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();

                                mApp.block("#evaluation-request-form");

                                $.ajax({
                                    url: $(form).attr("action"),
                                    type: "POST",
                                    data: $(form).serialize()
                                })
                                    .done(function () {
                                        $(".modal").modal("hide");
                                        $(".m-alert").addClass("m--hide");

                                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                        pages.procedurerequest.form.evaluation.clear();
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#evaluation_msg_txt").html(error.responseText);
                                        else $("#evaluation_msg_txt").html(_app.constants.ajax.message.error);

                                        $("#evaluation-modal-form-alert").removeClass("m--hide").show();
                                    })
                                    .always(function () {
                                        mApp.unblock("#evaluation-request-form");
                                    });
                            }
                        });
                    }
                },
                academicProgram: {
                    object: null,
                    clear: function () {
                        //this.object.resetForm();
                        //$("#academicProgramSelect").val(null).trigger("change");
                    },
                    init: function () {
                        this.object = $("#program-request-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();

                                mApp.block("#program-request-form");

                                $.ajax({
                                    url: $(form).attr("action"),
                                    type: "POST",
                                    data: $(form).serialize()
                                })
                                    .done(function () {
                                        $(".modal").modal("hide");
                                        $(".m-alert").addClass("m--hide");

                                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                        //pages.procedurerequest.form.transfer.clear();
                                        $("#AcademicProgramId").val($("#academicProgramSelect").val());
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#program_msg_txt").html(error.responseText);
                                        else $("#program_msg_txt").html(_app.constants.ajax.message.error);

                                        $("#program-modal-form-alert").removeClass("m--hide").show();
                                    })
                                    .always(function () {
                                        mApp.unblock("#program-request-form");
                                    });
                            }
                        });
                    }
                },
            },
            init: function () {
                this.select.courses.init();
                this.select.academicPrograms.init();
                $("[data-switch=true]").bootstrapSwitch();

                this.form.evaluation.init();
                this.form.academicProgram.init();
                this.events.init();
            }
        },
        observations: {
            datatable: {
                observations: {
                    object: null,
                    options: getSimpleDataTableConfiguration({
                        url: `/director-carrera/alumnos/informacion/${studentid}/observaciones/get`.proto().parseURL(),
                        data: function (data) {
                            //data.search = $("#search").val();
                        },
                        pageLength: 50,
                        ordering: false,
                        columns: [
                            {
                                data: "observation",
                                title:"Observaciones"
                            },
                            {
                                data: "user.userName",
                                title: "Usuario"
                            },
                            {
                                data: "createdFormattedDate",
                                title: "Fecha"
                            }
                        ]
                    }),
                    init: function () {
                        this.object = $("#observations_table").DataTable(this.options);
                    },
                    reload: function () {
                        this.object.ajax.reload();
                    }
                }
            },
            form: {
                observation: {
                    object: null,
                    clear: function () {
                        this.object.resetForm();
                    },
                    init: function () {
                        this.object = $("#create-observation-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();

                                mApp.block("#create-observation-form");

                                $.ajax({
                                    url: $(form).attr("action"),
                                    type: "POST",
                                    data: $(form).serialize()
                                })
                                    .done(function () {
                                        $(".modal").modal("hide");
                                        $(".m-alert").addClass("m--hide");

                                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                        pages.observations.form.observation.clear();

                                        pages.observations.datatable.observations.reload();
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#observation_msg_txt").html(error.responseText);
                                        else $("#observation_msg_txt").html(_app.constants.ajax.message.error);

                                        $("#observation-modal-form-alert").removeClass("m--hide").show();
                                    })
                                    .always(function () {
                                        mApp.unblock("#create-observation-form");
                                    });
                            }
                        });
                    }
                }
            },
            init: function () {
                this.datatable.observations.init();
                this.form.observation.init();
            }
        },
        clear: function () {
            pages.assistancehistory.clear();
            pages.reportcard.clear();
            pages.curriculumprogress.clear();
        }
    };

    return {
        init: function () {
            events.init();
        },
        load: function () {
            pages.reportcard.datatable.courses.load();
        }
    };
}();

$(function () {
    InitApp.init();
});