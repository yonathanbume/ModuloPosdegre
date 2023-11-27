var classReschedule = (function () {
    var private = {
        ajax: {
            objects: {}
        },
        datatable: {
            load: {
                get: function () {
                    private.datatable.objects["class-reschedule-datatable-get"] = $("#class-reschedule-datatable-get").mDatatable({
                        data: {
                            source: {
                                read: {
                                    method: "GET",
                                    url: "/profesor/clases/reprogramaciones/get".proto().parseURL()
                                }
                            }
                        },
                        columns: [
                            {
                                field: "class.classSchedule.section.courseTerm.course.fullName",
                                title: "Curso"
                            },
                            {
                                field: "class.classSchedule.section.code",
                                title: "Sección"
                            },
                            {
                                field: "class.startTime",
                                title: "Clase"
                            },
                            {
                                field: "status",
                                title: "Estado",
                                template: function (row) {
                                    var template = "";
                                    template += classRescheduleStatusValues[row.status];

                                    return template;
                                }
                            },
                            {
                                field: "createdAt",
                                title: "Fecha de Solicitud"
                            },
                            {
                                field: "options",
                                title: "Opciones",
                                sortable: false,
                                filterable: false,
                                template: function (row) {
                                    var template = "";
                                    template += "<button class=\"btn btn-primary btn-sm m-btn m-btn--icon\" onclick=\"classReschedule.modal.load.detail(this, event, '";
                                    template += row.proto().encode();
                                    template += "')\"><span><i class=\"la la-eye\"></i><span> Detalle </span></span></button> ";

                                    if (row.status == classRescheduleStatusIndices["IN_PROCESS"]) {
                                        template += "<button class=\"btn btn-danger m-btn btn-sm m-btn--icon m-btn--icon-only\" onclick=\"classReschedule.swal.load.delete(this, event, '";
                                        template += "/profesor/clases/reprogramaciones/eliminar/post".proto().parseURL();
                                        template += "', '";
                                        template += row.proto().encode();
                                        template += "')\"><i class=\"la la-trash\"></i></button>";
                                    }

                                    return template;
                                }
                            }
                        ]
                    });
                }
            },
            objects: {}
        },
        datepicker: {
            load: {
                startDate: function () {
                    private.datepicker.objects["class-reschedule-datepicker-start-date"] = $("#class-reschedule-modal-request-form .start-date-datepicker").datepicker({
                        startDate: "0d"
                    });
                }
            },
            objects: {}
        },
        modal: {
            objects: {}
        },
        other: {
            classes: null,
            sections: null
        },
        select2: {
            load: {
                class: function () {
                    $("#class-reschedule-modal-request-form .class-select2").on("select2:select", function (event) {
                        var data = $(this).select2('data')[0];
                        var startDateTimeMoment = moment(data.text, _app.constants.formats.datepickerJsMoment);

                        if (startDateTimeMoment.isValid()) {
                            classReschedule.datepicker.getObject("class-reschedule-datepicker-start-date").datepicker("setStartDate", startDateTimeMoment.format(_app.constants.formats.datepickerJsMoment));
                        }
                    });
                },
                course: function () {
                    $("#class-reschedule-modal-request-form .course-select2").on("select2:select", function (event) {
                        var data = $(this).select2('data')[0];
                        var tmpSections = [];

                        for (var key in private.other.sections) {
                            var section = private.other.sections[key];

                            if (section.courseTerm.courseId == data.id) {
                                tmpSections.push(section);
                            }
                        }

                        $("#class-reschedule-modal-request-form .section-select2").proto().htmlElement(function (element) {
                            var jQueryElement = $(element);

                            if (jQueryElement.hasClass("select2-hidden-accessible")) {
                                jQueryElement.select2("destroy");
                                jQueryElement.html("");
                            }

                            _app.modules.select.fill({
                                data: tmpSections,
                                element: element,
                                name: "code"
                            });

                            jQueryElement.select2({
                                dropdownParent: jQueryElement.closest(".modal")
                            });
                            jQueryElement.trigger("select2:select");
                        });
                    });
                },
                section: function () {
                    $("#class-reschedule-modal-request-form .section-select2").on("select2:select", function (event) {
                        var data = $(this).select2('data')[0];
                        var tmpClasses = [];

                        for (var key in private.other.classes) {
                            var $class = private.other.classes[key];

                            if ($class.sectionId == data.id) {
                                tmpClasses.push($class);
                            }
                        }

                        $("#class-reschedule-modal-request-form .class-select2").proto().htmlElement(function (element) {
                            var jQueryElement = $(element);

                            if (jQueryElement.hasClass("select2-hidden-accessible")) {
                                jQueryElement.select2("destroy");
                                jQueryElement.html("");
                            }

                            _app.modules.select.fill({
                                data: tmpClasses,
                                element: element,
                                name: "startTime"
                            });

                            jQueryElement.select2({
                                dropdownParent: jQueryElement.closest(".modal")
                            });
                            jQueryElement.trigger("select2:select");
                        });
                    });
                },
                status: function () {
                    $("#class-reschedule-header-form .status-select2").proto().htmlElement(function (element) {
                        var jQueryElement = $(element);

                        _app.modules.select.fill({
                            data: classRescheduleStatusValues,
                            element: element,
                            nullable: true
                        });

                        var jQueryElementModalParent = jQueryElement.parents(".modal");
                        var select2Options = {};

                        if (jQueryElementModalParent.length > 0) {
                            select2Options.dropdownParent = jQueryElementModalParent;
                        }

                        jQueryElement.select2(select2Options);
                    });

                    $("#class-reschedule-header-form .status-select2").on("select2:select", function (event) {
                        var data = $(this).select2('data')[0];
                        var classRescheduleDatatableGet = classReschedule.datatable.getObject("class-reschedule-datatable-get");

                        classRescheduleDatatableGet.setDataSourceParam("status", data.id);
                        classRescheduleDatatableGet.load();
                    });
                }
            },
            objects: {}
        },
        swal: {
            load: {
                delete: function (element, event) {
                    private.swal.objects["class-reschedule-swal-delete"] = swal({
                        preConfirm: function () {
                            return new Promise(function (resolve, reject) {
                                classReschedule.ajax.load.delete(element, event);
                            });
                        },
                        title: "¿Desea eliminar esta solicitud de reprogramación de clase?",
                        type: "warning",
                        showCancelButton: true,
                        showLoaderOnConfirm: true
                    });
                }
            },
            objects: {}
        },
        timepicker: {
            load: {
                init: function () {
                    $(".start-time-timepicker").timepicker({ minuteStep: 5 }).on("changeTime.timepicker", function (e) {
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

                        private.validate.objects["class-reschedule-modal-request-form"].element(".start-time-timepicker");
                        private.validate.objects["class-reschedule-modal-request-form"].element(".end-time-timepicker");
                    });

                    $(".end-time-timepicker").timepicker({ minuteStep: 5 }).on("changeTime.timepicker", function (e) {
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

                        private.validate.objects["class-reschedule-modal-request-form"].element(".start-time-timepicker");
                        private.validate.objects["class-reschedule-modal-request-form"].element(".end-time-timepicker");
                    });
                },
                endTime: function () {                    
                    private.timepicker.objects["class-reschedule-datepicker-end-time"] = $("#class-reschedule-modal-request-form .end-time-timepicker").timepicker({ minuteStep: 5 });
                   
                },
                startTime: function () {
                    
                    private.timepicker.objects["class-reschedule-datepicker-start-time"] = $("#class-reschedule-modal-request-form .start-time-timepicker").timepicker({ minuteStep: 5 });
                }
            },
            objects: {}
        },
        validate: {
            load: {
                request: function () {
                    private.validate.objects["class-reschedule-modal-request-form"] = $("#class-reschedule-modal-request-form").validate({
                        rules: {
                            EndTime: {
                                required: true,
                                timeRange: [
                                    $(".start-time-timepicker"),
                                    ">"
                                ]
                            },
                            StartTime: {
                                required: true,
                                timeRange: [
                                    $(".end-time-timepicker"),
                                    "<"
                                ]
                            },
                        },
                        submitHandler: function (form, event) {
                            event.preventDefault();
                            classReschedule.ajax.load.request(form, event);
                        }
                    });
                }
            },
            objects: {}
        }
    };

    return {
        ajax: {
            getObject: function (key) {
                return private.ajax.objects[key];
            },
            load: {
                class: function () {
                    private.ajax.objects["class-reschedule-ajax-class"] = $.ajax({
                        type: "GET",
                        url: "/profesor/clases/get-no-dictadas".proto().parseURL()
                    })
                        .done(function (data, textStatus, jqXHR) {
                            $(".class-select2").proto().htmlElement(function (element) {
                                private.other.classes = data;

                                $("#class-reschedule-modal-request-form .section-select2").proto().htmlElement(function (element) {
                                    var jQueryElement = $(element);

                                    jQueryElement.trigger("select2:select");
                                });
                            });
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.get, _app.constants.toastr.title.error);
                        });
                },
                course: function () {
                    private.ajax.objects["class-reschedule-ajax-course"] = $.ajax({
                        type: "GET",
                        url: "/profesor/cursos/get".proto().parseURL()
                    })
                        .always(function (data, textStatus, jqXHR) {
                            classReschedule.ajax.load.section();
                        })
                        .done(function (data, textStatus, jqXHR) {
                            $(".course-select2").proto().htmlElement(function (element) {
                                var jQueryElement = $(element);
                                var jQueryElementVal = jQueryElement.val();

                                if (jQueryElement.hasClass("select2-hidden-accessible")) {
                                    jQueryElement.select2("destroy");
                                    jQueryElement.html("");
                                }

                                _app.modules.select.fill({
                                    data: data,
                                    element: element,
                                    name: "courseTerm.course.fullName",
                                    selectedValue: jQueryElementVal,
                                    value: "courseTerm.courseId"
                                });

                                jQueryElement.select2({
                                    dropdownParent: jQueryElement.closest(".modal")
                                });
                            });
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.get, _app.constants.toastr.title.error);
                        });
                },
                delete: function (data, url) {
                    private.ajax.objects["class-reschedule-ajax-delete"] = $.ajax({
                        data: {
                            Id: data.id
                        },
                        type: "POST",
                        url: url
                    })
                        .always(function (data, textStatus, jqXHR) {
                            swal.close();
                        })
                        .done(function (data, textStatus, jqXHR) {
                            classReschedule.datatable.getObject("class-reschedule-datatable-get").reload();
                            toastr.success(_app.constants.toastr.message.success.delete, _app.constants.toastr.title.success);
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.delete, _app.constants.toastr.title.error);
                        });
                },
                request: function (element, event) {
                    var formElements = element.elements;

                    mApp.block(".modal-content");

                    private.ajax.objects["class-reschedule-ajax-request"] = $.ajax({
                        data: {
                            ClassId: formElements["ClassId"].value,
                            Date: formElements["StartDate"].value,
                            StartTime: formElements["StartTime"].value,
                            EndTime: formElements["EndTime"].value,
                            //StartDateTime: moment(`${formElements["StartDate"].value} ${formElements["StartTime"].value}`, _app.constants.formats.datetimepickerJsMoment).toISOString(),
                            //EndDateTime: moment(`${formElements["StartDate"].value} ${formElements["EndTime"].value}`, _app.constants.formats.datetimepickerJsMoment).toISOString(),
                            Justification: formElements["Justification"].value,
                            IsPermanent: $(formElements["IsPermanent"]).is(":checked")
                        },
                        type: element.method,
                        url: element.action
                    })
                        .always(function (data, textStatus, jqXHR) {
                            mApp.unblock(".modal-content");
                        })
                        .done(function (data, textStatus, jqXHR) {
                            _app.modules.form.reset({
                                element: element
                            });

                            classReschedule.datatable.getObject("class-reschedule-datatable-get").reload();
                            classReschedule.modal.getObject("class-reschedule-modal-request").modal("hide");
                            toastr.success(_app.constants.toastr.message.success.create, _app.constants.toastr.title.success);
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            var responseText = jqXHR.responseText;

                            if (responseText != "" && jqXHR.status == 400) {
                                toastr.error(responseText, _app.constants.toastr.title.error);
                            } else {
                                toastr.error(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                            }
                        });
                },
                section: function () {
                    private.ajax.objects["class-reschedule-ajax-section"] = $.ajax({
                        type: "GET",
                        url: "/profesor/secciones/get".proto().parseURL()
                    })
                        .always(function (data, textStatus, jqXHR) {
                            classReschedule.ajax.load.class();
                        })
                        .done(function (data, textStatus, jqXHR) {
                            private.other.sections = data;

                            $("#class-reschedule-modal-request-form .course-select2").proto().htmlElement(function (element) {
                                var jQueryElement = $(element);

                                jQueryElement.trigger("select2:select");
                            });
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.get, _app.constants.toastr.title.error);
                        });
                }
            }
        },
        datatable: {
            getObject: function (key) {
                return private.datatable.objects[key];
            }
        },
        datepicker: {
            getObject: function (key) {
                return private.datepicker.objects[key];
            }
        },
        init: function () {
            private.datatable.load.get();
            private.datepicker.load.startDate();
            private.select2.load.class();
            private.select2.load.course();
            private.select2.load.section();
            private.select2.load.status();
            private.timepicker.load.endTime();
            private.timepicker.load.startTime();
            private.timepicker.load.init();
            private.validate.load.request();
            classReschedule.ajax.load.course();
        },
        modal: {
            getObject: function (key) {
                return private.modal.objects[key];
            },
            load: {
                detail: function (element, event, data) {
                    var classRescheduleModalDetailForm = document.getElementById("class-reschedule-modal-detail-form");
                    data = data.proto().decode();
                    _app.modules.form.fill({
                        element: classRescheduleModalDetailForm,
                        data: {
                            CourseId: data.class.classSchedule.section.courseTerm.course.fullName,
                            SectionId: data.class.classSchedule.section.code,
                            ClassId: data.class.startTime,
                            StartDate: moment(data.startDateTime, _app.constants.formats.datetimepickerJsMoment).format(_app.constants.formats.datepickerJsMoment),
                            StartTime: moment(data.startDateTime, _app.constants.formats.datetimepickerJsMoment).format(_app.constants.formats.timepickerJsMoment),
                            EndTime: moment(data.endDateTime, _app.constants.formats.datetimepickerJsMoment).format(_app.constants.formats.timepickerJsMoment),
                            Justification: data.justification,
                            IsPermanent: data.isPermanent ? "Sí" : "No"
                        }
                    });

                    private.modal.objects["class-reschedule-modal-detail"] = $("#class-reschedule-modal-detail").modal("show");
                },
                request: function (element, event) {
                    var classRescheduleModalRequestForm = document.getElementById("class-reschedule-modal-request-form");

                    _app.modules.form.reset({
                        element: classRescheduleModalRequestForm
                    });

                    private.modal.objects["class-reschedule-modal-request"] = $("#class-reschedule-modal-request").modal("show");
                    var currentTime = moment();
                    var roundMinutes = Math.ceil(currentTime.minute() / 15) * 15;
                    var formated = currentTime.minute(roundMinutes).format(_app.constants.formats.timepickerJsMoment);
                    $("#class-reschedule-modal-request-form input[name='StartTime']").val(formated);
                    $("#class-reschedule-modal-request-form input[name='EndTime']").val(formated);
                }
            }
        },
        other: {
            getClasses: function () {
                return private.other.classes;
            },
            getSections: function () {
                return private.other.sections;
            }
        },
        select2: {
            getObject: function (key) {
                return private.select2.objects[key];
            }
        },
        swal: {
            getObject: function (key) {
                return private.swal.objects[key];
            },
            load: {
                delete: function (element, event, url, data) {
                    data = data.proto().decode();

                    private.swal.objects["class-reschedule-swal-delete"] = swal({
                        preConfirm: function () {
                            return new Promise(function (resolve, reject) {
                                classReschedule.ajax.load.delete(data, url);
                            });
                        },
                        title: _app.constants.swal.title.delete,
                        type: "warning",
                        showCancelButton: true,
                        showLoaderOnConfirm: true
                    });
                }
            }
        },
        timepicker: {
            getObject: function (key) {
                return private.timepicker.objects[key];
            }
        },
        validate: {
            getObject: function (key) {
                return private.validate.objects[key];
            }
        }
    };
})();

$(function () {
    classReschedule.init();
})
