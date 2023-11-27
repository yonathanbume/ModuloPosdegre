var InitApp = function () {

    var studentid = $("#Id").val();

    var studentModal = {
        object: $("#modal-student-documents"),
        events: {
            click: function () {
                $(".btn-student-download").on("click", function () {
                    $(this).addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);

                    var btnurl = "";
                    if ($(this).hasClass("studentCertificate"))
                        btnurl = (`/academico/alumnos/informacion/${studentid}/v2/certificado-estudios`).proto().parseURL();

                    if ($(this).hasClass("studentCurriculum"))
                        btnurl = (`/academico/alumnos/informacion/${studentid}/plan-estudios`).proto().parseURL();

                    window.open(btnurl, '_blank');
                    studentModal.object.modal("toggle");
                    $(this).removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                });
            }
        },
        init: function () {
            this.events.click();
        }
    };

    var modal = {
        object: $("#modal-documents"),
        events: {
            click: function () {
                $(".btn-download").on("click", function () {

                    console.log("descarga");

                    $(this).addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);

                    var btnurl = "";
                    if ($(this).hasClass("proofOfStudies"))
                        btnurl = (`/academico/alumnos/informacion/${studentid}/certificado-estudios`).proto().parseURL();

                    if ($(this).hasClass("proofOfIncome"))
                        btnurl = (`/academico/alumnos/informacion/${studentid}/constancia-ingreso`).proto().parseURL();

                    if ($(this).hasClass("enrollmentRecord"))
                        btnurl = (`/academico/alumnos/informacion/${studentid}/constancia-matricula`).proto().parseURL();

                    if ($(this).hasClass("recordOfRegularStudies"))
                        btnurl = (`/academico/alumnos/informacion/${studentid}/constancia-estudios-regulares`).proto().parseURL();

                    if ($(this).hasClass("recordOfEgress"))
                        btnurl = (`/academico/alumnos/informacion/${studentid}/constancia-egreso`).proto().parseURL();

                    if ($(this).hasClass("meritChart"))
                        btnurl = (`/academico/alumnos/informacion/${studentid}/cuadro-meritos`).proto().parseURL();

                    if ($(this).hasClass("upperfifth"))
                        btnurl = (`/academico/alumnos/informacion/${studentid}/quinto-superior`).proto().parseURL();

                    if ($(this).hasClass("bachelorsDegree"))
                        btnurl = (`/academico/alumnos/informacion/${studentid}/grado-bachiller`).proto().parseURL();

                    if ($(this).hasClass("jobTitle"))
                        btnurl = (`/academico/alumnos/informacion/${studentid}/titulo-profesional`).proto().parseURL();

                    if ($(this).hasClass("academicRecord"))
                        btnurl = (`/academico/alumnos/informacion/${studentid}/record-academico-titulacion`).proto().parseURL();

                    //if ($(this).hasClass("upperthirth"))
                    //    btnurl = (`/admin/constancias/constancia-tercio-superior/${studentid}`).proto().parseURL();
                    if ($(this).hasClass("upperthirth"))
                        btnurl = (`/academico/alumnos/informacion/${studentid}/tercio-superior`).proto().parseURL();

                    if ($(this).hasClass("academicperformancesummary"))
                        btnurl = (`/academico/alumnos/informacion/${studentid}/resumen-rendimeinto`).proto().parseURL();

                    window.open(btnurl, '_blank');
                    modal.object.modal("toggle");
                    $(this).removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                });
            }
        },
        init: function () {
            this.events.click();
        }
    };

    var events = {
        return: {
            load: function () {
                $(".m-content").on("click", ".index-return", function () {
                    $("#option-block").removeClass("m--hide");
                    $("#information-block").addClass("m--hide");
                    $("#information-block").html("");
                    pages.clear();
                    //mApp.block("#information-block");

                    //$.ajax({
                    //    url: (`/academico/alumnos/informacion/opciones`).proto().parseURL()
                    //}).done(function (data) {
                    //    pages.clear();

                    //    $("#information-block").html(data);

                    //    events.options.load();
                    //}).fail(function () {
                    //    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    //}).always(function () {
                    //    mApp.unblock("#information-block");
                    //});
                })
            }
        },
        options: {
            load: function () {
                //events.return.load();

                $(".student-record").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/academico/alumnos/informacion/${studentid}/estudiante/historial`).proto().parseURL()
                    }).done(function (data) {
                        $("#information-block").html(data);
                        events.return.load();
                        pages.studentRecord.init();
                    }).fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#information-block");
                    });
                });

                $(".student-situation").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/academico/alumnos/informacion/${studentid}/estudiante/situacion`).proto().parseURL()
                    }).done(function (data) {
                        $("#information-block").html(data);
                        events.return.load();
                        pages.studentSituation.init();
                    }).fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#information-block");
                    });
                });

                $(".general-data").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/academico/alumnos/informacion/${studentid}/datos-generales`).proto().parseURL()
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
                        url: (`/academico/alumnos/informacion/${studentid}/cambiar-clave`).proto().parseURL()
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
                        url: (`/academico/alumnos/informacion/${studentid}/tramites`).proto().parseURL()
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
                        url: (`/academico/alumnos/informacion/${studentid}/matricula-actual`).proto().parseURL()
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
                        url: (`/academico/alumnos/informacion/${studentid}/horario`).proto().parseURL()
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
                    //Current Version 2
                    const ver = 2;
                    if (ver == 1) {
                        $("#option-block").addClass("m--hide");
                        $("#information-block").removeClass("m--hide");

                        mApp.block("#information-block");

                        $.ajax({
                            url: (`/academico/alumnos/informacion/${studentid}/historial`).proto().parseURL()
                        }).done(function (data) {
                            $("#information-block").html(data);
                            events.return.load();
                            pages.academichistoryv1.init();
                        }).fail(function () {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        }).always(function () {
                            mApp.unblock("#information-block");
                        });
                    } else if (ver == 2) {
                        $("#option-block").addClass("m--hide");
                        $("#information-block").removeClass("m--hide");

                        mApp.block("#information-block");

                        $.ajax({
                            url: (`/academico/alumnos/informacion/${studentid}/historial`).proto().parseURL()
                        }).done(function (data) {
                            $("#information-block").html(data);
                            events.return.load();
                            pages.academichistoryv2.init();
                        }).fail(function () {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        }).always(function () {
                            mApp.unblock("#information-block");
                        });
                    }

                });

                $(".assistance-history").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/academico/alumnos/informacion/${studentid}/asistencia`).proto().parseURL()
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
                        url: (`/academico/alumnos/informacion/${studentid}/boleta-notas`).proto().parseURL()
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
                        url: (`/academico/alumnos/informacion/${studentid}/situacion-academica`).proto().parseURL()
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
                        url: (`/academico/alumnos/informacion/${studentid}/solicitud-tramites`).proto().parseURL()
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
                        url: (`/academico/alumnos/informacion/${studentid}/observaciones`).proto().parseURL()
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

                if ($('#IsBachelor').val() == 0) {
                    $("#modal-documents .notBachelor").hide();
                }
                studentModal.init();
                modal.init();

                pages.procedurerequest.datatablePay.paymentmades.init();

                $(".student-portfolio").on("click", function () {
                    $("#option-block").addClass("m--hide");
                    $("#information-block").removeClass("m--hide");

                    mApp.block("#information-block");

                    $.ajax({
                        url: (`/academico/alumnos/informacion/${studentid}/portafolio`).proto().parseURL()
                    }).done(function (data) {
                        $("#information-block").html(data);
                        events.return.load();
                        pages.portfolio.init();
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
                        rules: {
                            PersonalEmail: {
                                email: true,
                                //required: false
                            }
                        },
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

                    this.districts.init();
                    this.provinces.init();
                    this.departments.init();
                    //this.scales.init();
                    this.enrollmentFees.init();
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
                                $(".select2-careers").prop("disabled", true);

                                var careerId = $("#CareerId").val();

                                if (careerId !== null && careerId !== "") {
                                    $(".select2-careers").val(careerId).trigger("change");
                                    $("#CareerId").val(null).trigger("change");
                                }
                            }
                        });
                    }
                },
                departments: {
                    init: function () {
                        $.ajax({
                            url: "/departamentos/get".proto().parseURL()
                        }).done(function (result) {

                            $(".select2-departments").select2({
                                placeholder: "Seleccione un departamento",
                                minimumResultsForSearch: -1,
                                data: result.items
                            });

                            if ($("#departmentId").val() != "" && $("#departmentId").val() != null) {
                                $(".select2-departments").val($("#departmentId").val()).trigger("change");

                                $.ajax({
                                    url: `/departamentos/${$("#departmentId").val()}/provincias/get`.proto().parseURL()
                                }).done(function (result) {
                                    $(".select2-provinces").empty();

                                    $(".select2-provinces").select2({
                                        placeholder: "Provincias",
                                        minimumResultsForSearch: -1,
                                        data: result.items,
                                        disabled: false
                                    });

                                    if ($("#provinceId").val() != "" && $("#provinceId").val() != null) {
                                        $(".select2-provinces").val($("#provinceId").val()).trigger("change");

                                        $.ajax({
                                            url: `/departamentos/${$("#departmentId").val()}/provincias/${$("#provinceId").val()}/distritos/get`.proto().parseURL()
                                        }).done(function (result) {
                                            $(".select2-districts").empty();

                                            $(".select2-districts").select2({
                                                placeholder: "Distritos",
                                                minimumResultsForSearch: -1,
                                                data: result.items,
                                                disabled: false
                                            });

                                            if ($("#districtId").val() != "" && $("#districtId").val() != null) {
                                                $(".select2-districts").val($("#districtId").val()).trigger("change");
                                            }
                                        });
                                    }
                                });


                            }

                            $(".select2-departments").on("change", function () {
                                pages.general.select.provinces.load($(this).val());
                            });

                            $(".select2-provinces").on("change", function () {
                                pages.general.select.districts.load($(this).val());
                            });
                        });
                    }
                },
                provinces: {
                    init: function () {
                        $(".select2-provinces").select2({
                            placeholder: "Seleccione un departamento",
                            disabled: true
                        });
                    },
                    load: function (departmentId) {

                        $.ajax({
                            url: `/departamentos/${departmentId}/provincias/get`.proto().parseURL()
                        }).done(function (result) {
                            $(".select2-provinces").empty();

                            $(".select2-provinces").select2({
                                placeholder: "Provincias",
                                minimumResultsForSearch: -1,
                                data: result.items,
                                disabled: false
                            }).trigger("change");
                        });
                    }
                },
                districts: {
                    init: function () {
                        $(".select2-districts").select2({
                            placeholder: "Seleccione una provincia",
                            disabled: true
                        });
                    },
                    load: function (provinceId) {
                        var departmentId = $("#department-select").val();

                        $.ajax({
                            url: `/departamentos/${departmentId}/provincias/${provinceId}/distritos/get`.proto().parseURL()
                        }).done(function (result) {
                            $(".select2-districts").empty();

                            $(".select2-districts").select2({
                                placeholder: "Distritos",
                                minimumResultsForSearch: -1,
                                data: result.items,
                                disabled: false
                            });
                        });
                    }
                },
                //scales: {
                //    init: function () {
                //        $.ajax({
                //            url: "/get-escalas-estudiantes".proto().parseURL()
                //        }).done(function (result) {
                //            $(".select2-scales").select2({
                //                placeholder: "Escala de Pago",
                //                minimumInputLength: -1,
                //                data: result.items
                //            })
                //                .val($("#studentScaleId").val())
                //                .trigger("change");
                //        });
                //    }
                //},
                enrollmentFees: {
                    init: function () {
                        $.ajax({
                            url: "/get-pensiones-matricula".proto().parseURL()
                        }).done(function (result) {
                            $(".select2-fees").select2({
                                placeholder: "Pensión de Matrícula",
                                minimumInputLength: -1,
                                data: result.items
                            })
                                .val($("#enrollmentFeeId").val())
                                .trigger("change");
                        });
                    }
                },
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
                                    toastr.error(e.responseText, _app.constants.toastr.title.error);
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
                        url: `/academico/alumnos/informacion/${studentid}/tramites/get`.proto().parseURL(),
                        data: function (data) {
                            data.search = $("#search").val();
                        },
                        pageLength: 50,
                        orderable: [],
                        columns: [
                            {
                                data: "code"
                            },
                            {
                                data: "description"
                            },
                            {
                                data: "date"
                            },
                            {
                                data: "term"
                            },
                            {
                                data: "status"
                            },
                            {
                                data: "dependency"
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
                        url: `/academico/alumnos/informacion/${studentid}/matriculas/get`.proto().parseURL(),
                        data: function (data) {
                            //data.search = $("#search").val();
                            data.termId = $("#terms_select2_enrollment").val();
                        },
                        pageLength: 50,
                        orderable: [],
                        columns: [
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
                                data: "try"
                            },
                            {
                                data: null,
                                render: function (data) {
                                    var result = "-";
                                    if (data.isActiveTerm && data.try != "RET") {
                                        result = `<button data-object="${data.proto().encode()}" class="btn btn-info btn-sm m-btn m-btn--icon m-btn--icon-only btn-change"><i class="la la-eye"></i></button>`;
                                    }
                                    return result;
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

                            $("#currentId").val(object.id);

                            $("#currentSection").val(object.section);

                            console.log(object.id);
                            pages.enrollment.select.section.load(object.id);
                        });
                    },
                    reload: function () {
                        pages.enrollment.datatable.courses.object.ajax.reload();
                        $("#enrollment_table_processing").css("z-index", 1);
                    }
                },
                otherCourses: {
                    object: null,
                    options: getSimpleDataTableConfiguration({
                        url: `/academico/alumnos/informacion/${studentid}/matriculas/otros-cursos/get`.proto().parseURL(),
                        data: function (data) {
                            data.termId = $("#terms_select2_enrollment").val();
                        },
                        pageLength: 50,
                        orderable: [],
                        columns: [
                            {
                                data: "type"
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
                                data: "note"
                            }
                        ]
                    }),
                    reload: function () {
                        pages.enrollment.datatable.otherCourses.object.ajax.reload();
                        $("#other_enrollment_table_processing").css("z-index", 1);
                    },
                    init: function () {
                        pages.enrollment.datatable.otherCourses.object = $("#other_enrollment_table").DataTable(pages.enrollment.datatable.otherCourses.options);
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
                            url: `/academico/alumnos/informacion/matricula-actual/${id}/secciones-disponibles`.proto().parseURL()
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
                terms: {
                    load: function () {
                        $.ajax({
                            url: "/periodos-por-estudiante/get?studentId=" + studentid,
                            type: "GET"
                        })
                            .done(function (data) {
                                $("#terms_select2_enrollment").select2({
                                    data: data.items
                                });
                                $("#enrollment_terms_information").html("Periodos matriculados: " + data.items.length);
                                pages.enrollment.datatable.courses.init();
                                pages.enrollment.datatable.otherCourses.init();
                            });
                    },
                    events: {
                        onChange: function () {
                            $("#terms_select2_enrollment").on("change", function () {
                                pages.enrollment.datatable.courses.reload();
                                pages.enrollment.datatable.otherCourses.reload();

                            });
                        },
                        init: function () {
                            this.onChange();
                        }
                    },
                    init: function () {
                        this.load();
                        this.events.init();
                    }
                },
                init: function () {
                    select.section.init();
                    select.terms.init();
                }
            },
            form: {
                section: {
                    object: null,
                    clear: function () {
                        this.object.resetForm();
                        $("#enrollment-section-form").find(".custom-file-label").text("Buscar archivo..");
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
                pages.enrollment.select.section.init();
                pages.enrollment.select.terms.init();
                pages.enrollment.form.section.init();

                $(".btn-enrollment-report").on("click", function () {
                    var url = `/academico/alumnos/informacion/${studentid}/matriculas/reporte/${$("#terms_select2_enrollment").val()}`.proto().parseURL();
                    window.open(url, "_blank");
                });

                $(".btn-enrollment-report-pronabec").on("click", function () {
                    var url = `/academico/alumnos/informacion/${studentid}/matriculas/reporte/${$("#terms_select2_enrollment").val()}?pronabec=true`.proto().parseURL();
                    window.open(url, "_blank");
                });
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
                        url: `/academico/alumnos/informacion/${studentid}/horario/get`.proto().parseURL()
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
                settings2: {
                    allDaySlot: false,
                    aspectRatio: 2,
                    businessHours: {
                        dow: [1, 2, 3, 4, 5, 6],
                        end: "24:00",
                        start: "07:00"
                    },
                    columnFormat: "dddd",
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
                        url: `/academico/alumnos/informacion/${studentid}/horario/get`.proto().parseURL()
                    },
                    //eventClick: function (calEvent, jsEvent, view) {
                    //    form.detail.load(calEvent.id);
                    //},
                    firstDay: 1,
                    header: {
                    },
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
                    if ($(window).width() < 960) {
                        pages.schedule.fullcalendar.object = $("#schedule-calendar").fullCalendar(pages.schedule.fullcalendar.settings2);
                    }
                    if ($(window).width() > 960) {
                        pages.schedule.fullcalendar.object = $("#schedule-calendar").fullCalendar(pages.schedule.fullcalendar.settings);
                    }
                }
            },
            init: function () {
                pages.schedule.fullcalendar.init();
                $("#btnDownloadStudentSchedulePdf").on("click", function () {
                    var $btn = $(this);
                    console.log("click");
                    $btn.addLoader();
                    $.fileDownload(`/academico/alumnos/informacion/${studentid}/horario/reporte-pdf`.proto().parseURL(), {
                        httpMethod: "GET",
                    })
                        .done(function () {
                            $btn.removeLoader();
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        })
                        .fail(function (e) {
                            $btn.removeLoader();
                            if (e.responseText !== null && e.responseText !== "") {
                                toastr.error(e.responseText, _app.constants.toastr.title.error);
                            }
                            else {
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            }
                        });

                });
            }
        },
        academichistoryv1: {
            ajax: function () {
                mApp.block("#courses-block");
                var term = $('#select-term').val();

                $.ajax({
                    type: "GET",
                    url: `/academico/alumnos/informacion/${studentid}/historial/${term}/notas`.proto().parseURL()
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

                    pages.academichistoryv1.ajax();
                });

                if ($('#select-term').val()) {
                    pages.academichistoryv1.ajax();
                }
            }
        },
        academichistoryv2: {
            datatable: {
                courses: {
                    object: null,
                    options: {
                        responsive: true,
                        processing: true,
                        serverSide: true,
                        filter: false,
                        lengthChange: false,
                        paging: true,
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
                        pageLength: 50,
                        ajax: {
                            url: `/academico/alumnos/informacion/${studentid}/historial/cursos`.proto().parseURL(),
                            type: "GET",
                            dataType: "JSON",
                            data: function (data) {
                                delete data.columns;
                            }
                        },
                        orderable: [],
                        columns: [
                            {
                                data: "year"
                            },
                            {
                                data: null,
                                render: function (row) {
                                    return `${row.curriculum} - ${row.code}`;
                                }
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
                                    if (data.validated) {
                                        if (data.grade <= 0)
                                            return `<a href="#" data-id="${data.id}" class="show-equiv"><span class="m--font-info"> CE </span></a>`;
                                        else
                                            return `<a href="#" data-id="${data.id}" class="show-equiv"><span class="m--font-info"> ${data.grade} </span></a>`;
                                    }

                                    if (data.withdrawn == true) {
                                        return `<span style="color: red;">RET</span>`;
                                    }

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
                        pages.academichistoryv2.datatable.courses.object = $("#academichistoryv2_table").DataTable(pages.academichistoryv2.datatable.courses.options);
                    }
                }
            },
            init: function () {
                pages.academichistoryv2.datatable.courses.init();
            },
            clear: function () {
                pages.academichistoryv2.datatable.courses.object = null;
            }
        },
        assistancehistory: {
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
                            url: null,
                            type: "GET",
                            dataType: "JSON",
                            data: function (data) {
                                delete data.columns;
                            }
                        },
                        orderable: [],
                        columns: [
                            {
                                data: "course"
                            },
                            {
                                data: "total"
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
                                    <button data-id="${data.id}" class="btn btn-info btn-sm m-btn m-btn--icon m-btn--icon-only btn-course-detail" title=""><i class="la la-eye"></i></button>
                                    </div>`;
                                }
                            }
                        ],
                        dom: 'Bfrtip',
                        buttons: [
                            'csv', 'excel', 'pdf'
                        ]
                    },
                    load: function () {
                        var term = $("#select-term").val();
                        var url = `/academico/alumnos/informacion/${studentid}/asistencia/${term}/cursos`.proto().parseURL();

                        if (pages.assistancehistory.datatable.courses.object !== undefined && pages.assistancehistory.datatable.courses.object !== null) {
                            pages.assistancehistory.datatable.courses.object.ajax.url(url).load();
                        } else {
                            pages.assistancehistory.datatable.courses.options.ajax.url = url;
                            pages.assistancehistory.datatable.courses.object = $("#course_assistance_table").DataTable(pages.assistancehistory.datatable.courses.options);
                        }
                    },
                    init: function () {
                        var term = $("#select-term").val();
                        var url = `/academico/alumnos/informacion/${studentid}/asistencia/${term}/cursos`.proto().parseURL();

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
                                data: "isAbsent",
                                render: function (data) {
                                    if (data) {
                                        return `<span class="m--font-danger">Falta</span>`
                                    } else {
                                        return `<span class="m--font-primary">Asistió</span>`
                                    }
                                }
                            }
                        ]
                    }),
                    load: function (sectionid) {
                        $("#assistance_table_block").removeClass("m--hide");
                        var url = `/academico/alumnos/informacion/${studentid}/asistencia/${sectionid}/detalle`.proto().parseURL();

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
                            url: null,
                            type: "GET",
                            dataType: "JSON"
                        },
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
                                data: null,
                                render: function (row) {
                                    if (row.status == 3) return 'RET';

                                    if (row.status == 0) {
                                        var tpm = "";
                                        tpm += `<button type="button" data-id="${row.id}" data-course="${row.code} - ${row.course}" class="btn-view-grades btn btn-primary m-btn m-btn--icon btn-sm m-btn--icon-only"><i class="la la-eye" title="Ver notas parciales"></i></button>`;
                                        return tpm;
                                    }

                                    return row.grade;
                                }
                            },
                            {
                                data: "teacher"
                            }
                        ]
                    },
                    load: function () {
                        var term = $("#select-term").val();
                        var url = `/academico/alumnos/informacion/${studentid}/boleta-notas/${term}/cursos`.proto().parseURL();

                        if (pages.reportcard.datatable.courses.object !== undefined && pages.reportcard.datatable.courses.object !== null) {
                            pages.reportcard.datatable.courses.object.ajax.url(url).load();
                        } else {
                            pages.reportcard.datatable.courses.options.ajax.url = url;
                            pages.reportcard.datatable.courses.object = $("#card_table").DataTable(pages.reportcard.datatable.courses.options);
                        }
                    },
                    init: function () {
                        var term = $("#select-term").val();
                        var url = `/academico/alumnos/informacion/${studentid}/boleta-notas/${term}/cursos`.proto().parseURL();

                        pages.reportcard.datatable.courses.options.ajax.url = url;
                        pages.reportcard.datatable.courses.object = $("#card_table").DataTable(pages.reportcard.datatable.courses.options);
                    }
                },
                grades: {
                    object: null,
                    options: {
                        bPaginate: false,
                        bLengthChange: false,
                        bFilter: false,
                        bInfo: false,
                        ajax: {
                            url: null,
                            type: "GET",
                            dataSrc : "",
                            dataType: "JSON"
                        },
                        columns: [
                            {
                                data: "evaluation",
                                title : "Evaluación"
                            },
                            {
                                data: "percentage",
                                title: "Porcentaje",
                                render: function (row) {
                                    return `${row}%`;
                                }
                            },
                            {
                                data: "value",
                                title: "Nota",
                                render: function (row) {
                                    return row == null ? "-" : `${row}`;
                                }
                            }
                        ]
                    },
                    load: function (studentSectionId) {
                        var url = `/academico/alumnos/informacion/${studentid}/boleta-notas/detalle-notas/${studentSectionId}`;

                        if (pages.reportcard.datatable.grades.object !== undefined && pages.reportcard.datatable.grades.object !== null) {
                            pages.reportcard.datatable.grades.object.ajax.url(url).load();
                        } else {
                            pages.reportcard.datatable.grades.options.ajax.url = url;
                            pages.reportcard.datatable.grades.object = $("#report_card_student_grades_datatable").DataTable(pages.reportcard.datatable.grades.options);
                        }

                    }
                }
            },
            init: function () {
                if ($('#select-term').val()) {
                    pages.reportcard.datatable.courses.load();

                    $("#reportcard-link").prop("href", `/academico/constancias/notas/${studentid}/${$('#select-term').val()}`.proto().parseURL());
                    $.ajax({
                        url: (`/academico/alumnos/informacion/${studentid}/boleta-notas/${$('#select-term').val()}/cabecera`).proto().parseURL()
                    }).done(function (data) {
                        $("#header-student-name").text(data.name);
                        $("#header-cycle").text(data.cycle);
                        $("#header-credits").text(data.credits);
                        $("#header-courses").text(data.courses);
                    });
                }

                $("#card_table").on("click", ".btn-view-grades", function () {
                    var course = $(this).data("course");
                    var id = $(this).data("id");

                    $("#report_card_student_grades_modal").find(".modal-title").text(course);
                    pages.reportcard.datatable.grades.load(id);
                    $("#report_card_student_grades_modal").modal("show");
                })

                $("#select-term").on("change", function () {
                    pages.reportcard.datatable.courses.load();
                    $("#reportcard-link").prop("href", `/academico/constancias/notas/${studentid}/${$('#select-term').val()}`.proto().parseURL());

                    $.ajax({
                        url: (`/academico/alumnos/informacion/${studentid}/boleta-notas/${$('#select-term').val()}/cabecera`).proto().parseURL()
                    }).done(function (data) {
                        $("#header-student-name").text(data.name);
                        $("#header-cycle").text(data.cycle);
                        $("#header-credits").text(data.credits);
                        $("#header-courses").text(data.courses);
                    });
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
                    optionsWithoutER: {
                        responsive: true,
                        processing: true,
                        serverSide: false,
                        filter: false,
                        lengthChange: false,
                        paging: false,
                        ordering: false,
                        orderMulti: false,
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
                            url: `/academico/alumnos/informacion/${studentid}/situacion-academica/cursos`.proto().parseURL(),
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
                                    if (data.validated) {
                                        if (data.grade <= 0)
                                            return `<a href="#" data-id="${data.id}" class="show-equiv"><span class="m--font-info"> CE </span></a>`;
                                        else
                                            return `<a href="#" data-id="${data.id}" class="show-equiv"><span class="m--font-info"> ${data.grade} </span></a>`;
                                    }

                                    return data.tries === 0 ? " - " : (data.grade < 11 ? `<span style="color: red;">${data.grade}</span>` : data.grade);
                                }
                            },
                            {
                                data: null,
                                render: function (data) {
                                    return data.tries === 0 ? " - " : data.term;
                                }
                            }
                        ],
                        dom: 'Bfrtip',
                        buttons: [
                            {
                                text: 'Excel',
                                action: function (e, dt, node, config) {
                                    var url = `/academico/alumnos/informacion/${studentid}/situacion-academica/reporte-excel`;
                                    window.open(url, "_blank");
                                }
                            }
                        ]
                    },
                    options: {
                        responsive: true,
                        processing: true,
                        serverSide: false,
                        filter: false,
                        lengthChange: false,
                        paging: false,
                        ordering: false,
                        orderMulti: false,
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
                            url: `/academico/alumnos/informacion/${studentid}/situacion-academica/cursos`.proto().parseURL(),
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
                                    if (data.validated) {
                                        if (data.grade <= 0)
                                            return `<a href="#" data-id="${data.id}" class="show-equiv"><span class="m--font-info"> CE </span></a>`;
                                        else
                                            return `<a href="#" data-id="${data.id}" class="show-equiv"><span class="m--font-info"> ${data.grade} </span></a>`;
                                    }

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
                                data: null,
                                render: function (row) {
                                    var tpm = "";
                                    if (row.evaluationReport == "" || row.evaluationReport == null) {
                                        tpm = "-";
                                    } else {
                                        tpm = row.evaluationReport;
                                    }

                                    return tpm;
                                }
                            },
                            {
                                data: null,
                                render: function (row) {
                                    var tpm = "";
                                    if (row.evaluationReportDate == "" || row.evaluationReportDate == null) {
                                        tpm = "-";
                                    } else {
                                        tpm = row.evaluationReportDate;
                                    }

                                    return tpm;
                                }
                            },
                            {
                                data: null,
                                render: function (row) {
                                    var tpm = "";
                                    if ($("#isCareerDirector").val() != "1") {
                                        if (row.academicHistoryId != _app.constants.guid.empty) {
                                            tpm += `<button data-course='${row.code}-${row.course}' data-id='${row.academicHistoryId}' class="btn btn-edit-evaluation-report btn-primary m-btn m-btn--icon btn-sm m-btn--icon-only"><i class="la la-edit"></i></button>`;
                                        } else {
                                            tpm += "-";
                                        }
                                    }
                                    return tpm;
                                }
                            }
                        ],
                        rowGroup: {
                            dataSrc: "yearName"
                        },
                        columnDefs: [
                            {
                                visible: false,
                                targets: 0
                            }
                        ],
                        dom: 'Bfrtip',
                        buttons: [
                            {
                                text: 'Excel',
                                action: function (e, dt, node, config) {
                                    var url = `/academico/alumnos/informacion/${studentid}/situacion-academica/reporte-excel`;
                                    window.open(url, "_blank");
                                }
                            }
                        ]
                    },
                    init: function () {
                        var ahWithEvaluationReport = $("#ahWthEvaluationReport").val();
                        if (ahWithEvaluationReport == "true") {
                            pages.curriculumprogress.datatable.courses.object = $("#curriculum_table").DataTable(pages.curriculumprogress.datatable.courses.options);
                        } else {
                            pages.curriculumprogress.datatable.courses.object = $("#curriculum_table").DataTable(pages.curriculumprogress.datatable.courses.optionsWithoutER);
                        }


                        $("#curriculum_table").on("click", ".show-equiv", function () {
                            var id = $(this).data("id");

                            $.ajax({
                                url: `/academico/alumnos/informacion/${studentid}/situacion-academica/equivalencia`.proto().parseURL(),
                                type: "GET",
                                data: {
                                    course: id
                                },
                                success: function (data) {
                                    $("#equivalence-data").html(data);

                                    $("#equivalence-modal").modal("show");
                                },
                                error: function () {
                                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                }
                            });
                        });

                        $("#curriculum_table").on("click", ".btn-edit-evaluation-report", function () {
                            var id = $(this).data("id");
                            var course = $(this).data("course");
                            pages.curriculumprogress.modal.evaluationReport.events.show(id, course);
                        });
                    }
                },
                elective: {
                    object: null,
                    optionsWithoutER: {
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
                            url: `/academico/alumnos/informacion/${studentid}/situacion-academica/electivos`.proto().parseURL(),
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
                            }
                        ]
                    },
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
                            url: `/academico/alumnos/informacion/${studentid}/situacion-academica/electivos`.proto().parseURL(),
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
                                data: null,
                                render: function (row) {
                                    var tpm = "";
                                    if (row.evaluationReport == "" || row.evaluationReport == null) {
                                        tpm = "-";
                                    } else {
                                        tpm = row.evaluationReport;
                                    }

                                    return tpm;
                                }
                            },
                            {
                                data: null,
                                render: function (row) {
                                    var tpm = "";
                                    if (row.evaluationReportDate == "" || row.evaluationReportDate == null) {
                                        tpm = "-";
                                    } else {
                                        tpm = row.evaluationReportDate;
                                    }

                                    return tpm;
                                }
                            },
                            {
                                data: null,
                                render: function (row) {
                                    var tpm = "";
                                    if ($("#isCareerDirector").val() != "1") {
                                        if (row.academicHistoryId != _app.constants.guid.empty) {
                                            tpm += `<button data-course='${row.code}-${row.course}' data-id='${row.academicHistoryId}' class="btn btn-edit-evaluation-report btn-primary m-btn m-btn--icon btn-sm m-btn--icon-only"><i class="la la-edit"></i></button>`;
                                        } else {
                                            tpm += "-";
                                        }
                                    }

                                    return tpm;
                                }
                            }
                        ]
                    },
                    init: function () {

                        var ahWithEvaluationReport = $("#ahWthEvaluationReport").val();
                        if (ahWithEvaluationReport == "true") {
                            pages.curriculumprogress.datatable.elective.object = $("#elective_table").DataTable(pages.curriculumprogress.datatable.elective.options);
                        } else {
                            pages.curriculumprogress.datatable.elective.object = $("#elective_table").DataTable(pages.curriculumprogress.datatable.elective.optionsWithoutER);
                        }

                        $("#elective_table").on("click", ".btn-edit-evaluation-report", function () {
                            var id = $(this).data("id");
                            var course = $(this).data("course");
                            pages.curriculumprogress.modal.evaluationReport.events.show(id, course);
                        });
                    }
                }
            },
            select: {
                term: {
                    load: function () {
                        $.ajax({
                            url: "/periodos/get",
                            type: "GET"
                        })
                            .done(function (e) {
                                $("[name='TermId']").select2({
                                    data: e.items,
                                    dropdownParent: $("#evaluation_report_modal")
                                });
                            });
                    },
                    init: function () {
                        this.load();
                    }
                },
                init: function () {
                    this.term.init();
                }
            },
            init: function () {
                $("#uploaddigital").on("click", function () {
                    var $div = $("#uploaddigital_div_dowload_document");
                    $(".cp-document-file-label").html("Seleccione un archivo");
                    $("#uploaddigital-modal").modal("show");
                    $div.addClass("d-none");

                    $.ajax({
                        url: `/academico/alumnos/informacion/${studentid}/situacion-academica/archivo-alumno`,
                        type: "GET"
                    })
                        .done(function (e) {
                            if (e !== null && e !== undefined) {
                                var $link = $div.find("a");
                                var url = `/academico/alumnos/informacion/${studentid}/situacion-academica/archivo-alumno/descargar`;
                                $div.removeClass("d-none");
                                $link.attr("href", url);
                            }
                        })
                        .always(function () {

                        });

                });



                $("#btn_curriculum_progress_pdf").on("click", function () {
                    var $btn = $(this);
                    var id = $btn.data("id");

                    $btn.addLoader();

                    $.ajax({
                        url: `/academico/alumnos/informacion/${id}/situacion-academica/validar-reporte-pdf`,
                        type: "GET"
                    })
                        .done(function () {
                            var url = `/academico/alumnos/informacion/${id}/situacion-academica/reporte-pdf`;
                            window.open(url, "_blank");
                        })
                        .fail(function (e) {
                            swal({
                                type: "info",
                                title: "Información",
                                allowOutsideClick: false,
                                text: e.responseText,
                                confirmButtonText: "Aceptar"
                            }).then(function (result) {
                                if (result.value) {
                                    $("#academic_history_data_admission_modal").modal("show");
                                }
                            });
                        })
                        .always(function () {
                            $btn.removeLoader();
                        });
                });

                $('#academic_history_data_admission_modal').on('show.bs.modal', function (e) {
                    $.ajax({
                        url: `/academico/alumnos/informacion/${studentid}/situacion-academica/data-admision/get`,
                        type: "GET"
                    })
                        .done(function (e) {
                            var $form = $("#academic_history_data_admission_form");
                            $form.find("[name='Score']").attr("disabled", false);
                            $form.find("[name='PossibleScore']").attr("disabled", false);
                            $form.find("[name='Order']").attr("disabled", false);
                            $form.find("[name='TotalStudents']").attr("disabled", false);
                            $form.find("[name='RegistrationNumber']").attr("disabled", false);

                            if (e !== null && e !== undefined) {
                                $form.find("[name='Score']").val(e.score);
                                $form.find("[name='PossibleScore']").val(e.possibleScore);
                                $form.find("[name='Order']").val(e.order);
                                $form.find("[name='TotalStudents']").val(e.totalStudents);
                                $form.find("[name='RegistrationNumber']").val(e.registrationNumber);
                            }
                        });

                });

                $('#academic_history_data_admission_modal').on('hidden.bs.modal', function (e) {
                    pages.curriculumprogress.form.academicHistoryAdmissionData.object.resetForm();
                })

                $(".cp-document-file").on("change", function () {
                    $(this).valid();
                    var fileName = $(this).val();
                    $(this).next().html(fileName);
                });
                pages.curriculumprogress.datatable.courses.init();
                pages.curriculumprogress.datatable.elective.init();
                pages.curriculumprogress.form.academicHistoryDocument.init();
                pages.curriculumprogress.form.academicHistoryAdmissionData.init();
                pages.curriculumprogress.modal.init();
                pages.curriculumprogress.select.init();
            },
            form: {
                academicHistoryDocument: {
                    object: null,
                    clear: function () {
                        $(".cp-document-file-label").html("Seleccione un archivo");
                        this.object.resetForm();
                    },
                    init: function () {
                        this.object = $("#uploaddigital-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();
                                var formData = new FormData($(form).get(0));
                                mApp.block("#uploaddigital-form");

                                $.ajax({
                                    url: `/academico/alumnos/informacion/${studentid}/situacion-academica/archivo-alumno`,
                                    type: "GET"
                                })
                                    .done(function (e) {
                                        if (e === null || e === undefined) {
                                            $.ajax({
                                                url: `/academico/alumnos/informacion/${studentid}/situacion-academica/adjuntar-archivo`,
                                                type: "POST",
                                                data: formData,
                                                contentType: false,
                                                processData: false
                                            }).done(function () {
                                                $(".modal").modal("hide");
                                                $(".m-alert").addClass("m--hide");
                                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                                pages.curriculumprogress.form.academicHistoryDocument.clear();

                                                $("#hasFile").val(true);
                                            }).fail(function (error) {
                                                if (error.responseText !== null && error.responseText !== "") $("#uploaddigital_form_msg_txt").html(error.responseText);
                                                else $("#uploaddigital_form_msg_txt").html(_app.constants.ajax.message.error);
                                                $("#uploaddigital_form_msg").removeClass("m--hide").show();
                                            }).always(function () {
                                                mApp.unblock("#uploaddigital-form");
                                            });
                                        }
                                        else {
                                            swal({
                                                type: "warning",
                                                title: "Actualmente existe un archivo.",
                                                text: "¿Desea actualizarlo?.",
                                                confirmButtonText: "Aceptar",
                                                showCancelButton: true,
                                                showLoaderOnConfirm: true,
                                                allowOutsideClick: () => !swal.isLoading(),

                                                preConfirm: () => {
                                                    return new Promise(() => {
                                                        $.ajax({
                                                            url: `/academico/alumnos/informacion/${studentid}/situacion-academica/adjuntar-archivo`,
                                                            type: "POST",
                                                            data: formData,
                                                            contentType: false,
                                                            processData: false
                                                        }).done(function () {
                                                            $(".modal").modal("hide");
                                                            $(".m-alert").addClass("m--hide");
                                                            pages.curriculumprogress.form.academicHistoryDocument.clear();
                                                            swal({
                                                                type: "success",
                                                                title: "Completado",
                                                                text: "Archivo actualizado satisfactoriamente.",
                                                                confirmButtonText: "Aceptar"
                                                            });
                                                        }).fail(function (error) {
                                                            if (error.responseText !== null && error.responseText !== "") $("#uploaddigital_form_msg_txt").html(error.responseText);
                                                            else $("#uploaddigital_form_msg_txt").html(_app.constants.ajax.message.error);
                                                            $("#uploaddigital_form_msg").removeClass("m--hide").show();
                                                        }).always(function () {
                                                            mApp.unblock("#uploaddigital-form");
                                                        });
                                                    });
                                                }
                                            }).then(function (e) {
                                                if (e.dismiss) {
                                                    mApp.unblock("#uploaddigital-form");
                                                }
                                            });
                                        }
                                    });
                                //}


                            }
                        });
                    }
                },
                academicHistoryAdmissionData: {
                    object: null,
                    clear: function () {
                        this.object.resetForm();
                        var $form = $("#academic_history_data_admission_form");
                        $form.find("[name='Score']").attr("disabled", true);
                        $form.find("[name='PossibleScore']").attr("disabled", true);
                        $form.find("[name='Order']").attr("disabled", true);
                        $form.find("[name='TotalStudents']").attr("disabled", true);
                        $form.find("[name='RegistrationNumber']").attr("disabled", true);
                    },
                    init: function () {
                        this.object = $("#academic_history_data_admission_form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();
                                $("#academic_history_data_admission_form").find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                                var formData = new FormData($(form).get(0));

                                $.ajax({
                                    url: `/academico/alumnos/informacion/${studentid}/situacion-academica/data-admision`,
                                    type: "post",
                                    data: formData,
                                    contentType: false,
                                    processData: false
                                })
                                    .done(function (e) {
                                        $("#academic_history_data_admission_modal").modal("hide");
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "Datos actualizados satisfactoriamente.",
                                            confirmButtonText: "Aceptar"
                                        });
                                        pages.curriculumprogress.form.academicHistoryAdmissionData.clear();
                                    })
                                    .fail(function (e) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            text: e.responseText,
                                            confirmButtonText: "Aceptar"
                                        });
                                    })
                                    .always(function () {
                                        $("#academic_history_data_admission_form").find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                                    });
                            }
                        });
                    }
                }
            },
            modal: {
                evaluationReport: {
                    form: {
                        object: null,
                        validate: function () {
                            this.object = $("#evaluation_report_form").validate({
                                submitHandler: function (formElement, e) {
                                    var formData = new FormData(formElement);
                                    $("#evaluation_report_modal").find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);

                                    $.ajax({
                                        url: `/academico/alumnos/informacion/${studentid}/situacion-academica/actualizar-acta`,
                                        type: "POST",
                                        data: formData,
                                        contentType: false,
                                        processData: false
                                    })
                                        .done(function (e) {
                                            pages.curriculumprogress.datatable.courses.object.ajax.reload();
                                            $("#evaluation_report_modal").modal("hide");
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: "El registro ha sido actualizar satisfactoriamente.",
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
                                        .always(function () {
                                            $("#evaluation_report_modal").find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                                        });
                                }
                            });
                        },
                        init: function () {
                            this.validate();
                        }
                    },
                    events: {
                        show: function (academicHistoryId, course) {
                            var obj = $("#evaluation_report_modal");

                            obj.find(".modal-title").text(course);
                            obj.modal("show");

                            $.ajax({
                                url: `/academico/alumnos/informacion/${studentid}/situacion-academica/get-detalles-acta?academicHistoryId=${academicHistoryId}`,
                                type: "GET"
                            })
                                .done(function (e) {

                                    obj.find("[name='AcademicHistoryId']").val(academicHistoryId);
                                    obj.find("[name='EvaluationReportId']").val(e.evaluationReportId);
                                    obj.find("[name='EvaluationReportCode']").val(e.evaluationReportCode);

                                    $("#evaluation_report_modal").find("[name='EvaluationReportReceptionDate']").datepicker("destroy");
                                    obj.find("[name='EvaluationReportReceptionDate']").val(e.receptionDate);
                                    $("#evaluation_report_modal").find("[name='EvaluationReportReceptionDate']").datepicker({
                                        orientation: "bottom"
                                    });
                                    if (e.evaluationReportCode !== null && e.evaluationReportCode !== "") {
                                        obj.find("[name='EvaluationReportReceptionDate']").attr("disabled", false);
                                    } else {
                                        obj.find("[name='EvaluationReportReceptionDate']").attr("disabled", true);
                                    }


                                    $("#evaluation_report_modal").find("[name='EvaluationReportCreatedAt']").datepicker("destroy");
                                    obj.find("[name='EvaluationReportCreatedAt']").val(e.receptionDate);
                                    $("#evaluation_report_modal").find("[name='EvaluationReportCreatedAt']").datepicker({
                                        orientation: "bottom"
                                    });
                                    if (e.evaluationReportCode !== null && e.evaluationReportCode !== "") {
                                        obj.find("[name='EvaluationReportCreatedAt']").attr("disabled", false);
                                    } else {
                                        obj.find("[name='EvaluationReportCreatedAt']").attr("disabled", true);
                                    }

                                 
                                    //$("#evaluation_report_modal").find("[name='EvaluationReportLastGradePublished']").datetimepicker("destroy");
                                    obj.find("[name='EvaluationReportLastGradePublished']").val(e.lastGradePublished);
                                    $("#evaluation_report_modal").find("[name='EvaluationReportLastGradePublished']").datetimepicker({
                                        format: "dd/mm/yyyy HH:ii P",
                                        showMeridian: true,
                                        autoclose : true
                                    });
                                 
                                    if (e.evaluationReportCode !== null && e.evaluationReportCode !== "") {
                                        obj.find("[name='EvaluationReportLastGradePublished']").attr("disabled", false);
                                    } else {
                                        obj.find("[name='EvaluationReportLastGradePublished']").attr("disabled", true);
                                    }

                                    obj.find("[name='TermId']").val(e.termId).trigger("change");
                                    obj.find("[name='Observation']").val(e.observation);

                                })
                        },
                        onValidateCode: function () {
                            $("#evaluation_report_modal").on("click", ".validate_evaluationrepot", function () {

                                if (code === "" || code === null) {
                                    toastr.error("Es necesario ingresar el código.", "Error");
                                    return;
                                }

                                var $btn = $(this);
                                $btn.addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);

                                var termId = $("#evaluation_report_modal").find("[name='TermId']").val();
                                var code = $("#evaluation_report_modal").find("[name='EvaluationReportCode']").val();


                                $.ajax({
                                    url: `/academico/alumnos/informacion/${studentid}/situacion-academica/validar-acta?termId=${termId}&code=${code}`,
                                    type: "GET"
                                })
                                    .done(function (e) {
                                        $("#evaluation_report_modal").find("[name='EvaluationReportReceptionDate']").datepicker("destroy");
                                        $("#evaluation_report_modal").find("[name='EvaluationReportReceptionDate']").val(e.receptionDate);
                                        $("#evaluation_report_modal").find("[name='EvaluationReportReceptionDate']").attr("disabled", false);
                                        $("#evaluation_report_modal").find("[name='EvaluationReportReceptionDate']").datepicker({
                                            orientation: "bottom"
                                        });


                                        $("#evaluation_report_modal").find("[name='EvaluationReportCreatedAt']").datepicker("destroy");
                                        $("#evaluation_report_modal").find("[name='EvaluationReportCreatedAt']").val(e.cretedAt);
                                        $("#evaluation_report_modal").find("[name='EvaluationReportCreatedAt']").attr("disabled", false);
                                        $("#evaluation_report_modal").find("[name='EvaluationReportCreatedAt']").datepicker({
                                            orientation: "bottom"
                                        });

                                        //$("#evaluation_report_modal").find("[name='EvaluationReportLastGradePublished']").datetimepicker("destroy");
                                        $("#evaluation_report_modal").find("[name='EvaluationReportLastGradePublished']").val(e.lastGradePublishedDate);
                                        $("#evaluation_report_modal").find("[name='EvaluationReportLastGradePublished']").attr("disabled", false);
                                        //$("#evaluation_report_modal").find("[name='EvaluationReportLastGradePublished']").datetimepicker({
                                        //    format: "dd MM yyyy - HH:ii P",
                                        //    showMeridian: !0,
                                        //    todayHighlight: !0,
                                        //    autoclose: !0,
                                        //    pickerPosition: "bottom-left"
                                        //});
                                    })
                                    .fail(function (e) {
                                        $("#evaluation_report_modal").find("[name='EvaluationReportReceptionDate']").val("");
                                        $("#evaluation_report_modal").find("[name='EvaluationReportReceptionDate']").attr("disabled", true);

                                        $("#evaluation_report_modal").find("[name='EvaluationReportCreatedAt']").val("");
                                        $("#evaluation_report_modal").find("[name='EvaluationReportCreatedAt']").attr("disabled", true);

                                        $("#evaluation_report_modal").find("[name='EvaluationReportLastGradePublished']").val("");
                                        $("#evaluation_report_modal").find("[name='EvaluationReportLastGradePublished']").attr("disabled", true);
                                        toastr.error(e.responseText, "Error");
                                    })
                                    .always(function () {
                                        $btn.removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                                    })
                            })
                        },
                        onHidden: function () {
                            $("#evaluation_report_modal").on('hidden.bs.modal', function (e) {
                                var obj = $("#evaluation_report_modal");

                                obj.find("[name='AcademicHistoryId']").val("");
                                obj.find("[name='EvaluationReportId']").val("");
                                obj.find("[name='EvaluationReportCode']").val("");
                                obj.find("[name='TermId']").val(null).trigger("change");
                                obj.find("[name='Observation']").val("");
                                obj.find("[name='File']").val("");
                                obj.find(".custom-file-label").text("Seleccione un archivo");
                            });
                        },
                        init: function () {
                            this.onHidden();
                            this.onValidateCode();
                        }
                    },
                    datepicker: {
                        init: function () {
                            $("#evaluation_report_modal").find("[name='EvaluationReportReceptionDate']").datepicker({
                                orientation: "bottom"
                            });
                        }
                    }
                },
                init: function () {
                    this.evaluationReport.events.init();
                    this.evaluationReport.form.init();
                    this.evaluationReport.datepicker.init();
                }
            },
            clear: function () {
                pages.curriculumprogress.datatable.courses.object = null;
                pages.curriculumprogress.datatable.elective.object = null;
            }
        },
        procedurerequest: {
            events: {
                init: function () {
                    //$(".reservation-request").on("click", function () {
                    //    swal({
                    //        title: "Registrar reserva de matrícula",
                    //        text: "Se registrará una nueva reserva de matrícula a partir de la fecha actual.",
                    //        type: "warning",
                    //        showCancelButton: true,
                    //        confirmButtonText: "Si, reservar",
                    //        confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                    //        cancelButtonText: "Cancelar"
                    //    }).then(function (result) {
                    //        if (result.value) {
                    //            $.ajax({
                    //                url: `/academico/alumnos/informacion/${$("#Id").val()}/solicitud-tramites/reserva`.proto().parseURL(),
                    //                type: "POST",
                    //            }).done(function () {
                    //                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    //                datatable.students.load();
                    //            }).fail(function (e) {
                    //                if (e.responseText !== null && e.responseText !== "") {
                    //                    toastr.error(e.responseText, _app.constants.toastr.title.error);
                    //                }
                    //                else {
                    //                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    //                }
                    //            });
                    //        }
                    //    });
                    //});

                    $(".academicyearwithdrawal-request").on("click", function () {
                        setTimeout(function () {
                            $("#withdrawal-file").on('change', function (event) {
                                $('.custom-file-label').html(event.target.files[0].name);
                            });
                        }, 1000);
                        swal({
                            title: "¿Desea retirar al estudiante del ciclo?",
                            //text: "Se retirará al estudiante de todas sus asignaturas. Este proceso es irreversible.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Si, retirar",
                            confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                            cancelButtonText: "Cancelar",
                            html: `Se retirará al estudiante de todas sus asignaturas. Este proceso es irreversible.</br>
                                        <div class="form-group m-form__group col-xl-12">                     
                                            <div class="custom-file">
                                                <input id="withdrawal-file" type="file" class="custom-file-input" lang="es"/>
                                                <label class="custom-file-label" for="withdrawal-file">
                                                    Seleccione un archivo
                                                </label>
                                            </div>                                              
                                        </div>`,
                        }).then(function (result) {
                            if (result.value) {
                                var fd = new FormData();
                                fd.append("file", $('#withdrawal-file')[0].files[0]);

                                $.ajax({
                                    url: `/academico/alumnos/informacion/${$("#Id").val()}/solicitud-tramites/retiro-ciclo`.proto().parseURL(),
                                    type: "POST",
                                    processData: false,
                                    contentType: false,
                                    data: fd
                                }).done(function () {
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
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

                    $(".expelled-request").on("click", function () {
                        setTimeout(function () {
                            $("#File").on('change', function (event) {
                                $('.custom-file-label').html(event.target.files[0].name);
                            });
                        }, 1000);
                        swal({
                            title: "¿Expulsar al estudiante?",
                            //text: "El alumno será expulsado, removiendolo de todas sus secciones matriculadas. Este proceso es irreversible.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Si, expulsarlo",
                            confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                            cancelButtonText: "Cancelar",
                            html: `El estudiante será expulsado, removiendolo de todas sus secciones matriculadas. Este proceso es irreversible.</br>
                                       <div class="form-group m-form__group col-xl-12">
                                            <div class="m-input  m-input--solid">
                                                <label for="Reason"></label>
                                                <input type="text" placeholder="Motivo" class="form-control m-input" name="Reason" id="Reason" required>
                                            </div>
                                        </div>
                                        <div class="form-group m-form__group col-xl-12">
                                            <label asp-for="File"></label>
                                            <div class="custom-file">
                                                <input name="File" id="File" type="file" class="custom-file-input" lang="es"/>
                                                <label class="custom-file-label" for="File">
                                                    Seleccione un archivo
                                                </label>
                                            </div>                                              
                                        </div>`,
                            preConfirm: () => {
                                var errors = {
                                    Reason: 'Introduzca un motivo de expulsión',
                                    File: 'Introduzca un archivo'
                                };
                                var validform = true;
                                $.each(errors, function (key, value) {
                                    if ($('input[name="Reason"]').val() == "" || $('#File')[0].files.length === 0) {
                                        if (!$('input[name="' + key + '"]').hasClass("invalid")) {
                                            $('input[name="' + key + '"]').addClass('invalid').after('<div class="invalid-feedback">' + value + '</div>');
                                            if (key == "File") {
                                                $(".custom-file").addClass('invalid');
                                            }
                                        }
                                        validform = false;
                                    } else {
                                        $('input.invalid').removeClass('invalid');
                                        $('.invalid-feedback').remove();
                                        if (key == "File") {
                                            $(".custom-file").removeClass('invalid');
                                        }
                                    }
                                });
                                if (!validform) return false;
                            },
                        }).then((result) => {
                            if (result.value) {
                                var fd = new FormData();
                                fd.append("File.File", $('#File')[0].files[0]);
                                fd.append("Reason", $('input[name="Reason"]').val())
                                $.ajax({
                                    url: `/academico/alumnos/informacion/${$("#Id").val()}/solicitud-tramites/expulsar-alumno`.proto().parseURL(),
                                    type: "POST",
                                    processData: false,
                                    contentType: false,
                                    data: fd
                                    //    Reason: $('input[name="Reason"]').val(),
                                    //    File: $('#File')[0].files
                                    //}
                                }).done(function () {
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                }).fail(function (e) {
                                    if (e.responseText !== null && e.responseText !== "") {
                                        toastr.error(e.responseText, _app.constants.toastr.title.error);
                                    }
                                    else {
                                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                    }
                                });
                            }
                        })
                    });

                    $(".resign-request").on("click", function () {
                        setTimeout(function () {
                            $("#resign-file").on('change', function (event) {
                                console.log("cambio");
                                $('.custom-file-label').html(event.target.files[0].name);
                            });
                        }, 1000);

                        swal({
                            title: "Renunciar como estudiante",
                            //text: "El alumno será expulsado, removiendolo de todas sus secciones matriculadas. Este proceso es irreversible.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Si, retirarlo",
                            confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                            cancelButtonText: "Cancelar",
                            html: `El estudiante renunciará a la institución, removiendolo de todas sus secciones matriculadas. Este proceso es irreversible.</br>
                                       <div class="form-group m-form__group col-xl-12">
                                            <div class="m-input  m-input--solid">
                                                <label for="Reason"></label>
                                                <input type="text" placeholder="Motivo" class="form-control m-input" name="ResignReason" id="resign-reason" required>
                                            </div>
                                        </div>
                                        <div class="form-group m-form__group col-xl-12">
                                            <label asp-for="File"></label>
                                            <div class="custom-file">
                                                <input name="ResignFile" id="resign-file" type="file" class="custom-file-input" lang="es"/>
                                                <label class="custom-file-label" for="File">
                                                    Seleccione un archivo
                                                </label>
                                            </div>                                              
                                        </div>`,
                            preConfirm: () => {
                                var errors = {
                                    ResignReason: 'Introduzca un motivo de expulsión',
                                    ResignFile: 'Introduzca un archivo'
                                };
                                var validform = true;
                                $.each(errors, function (key, value) {
                                    if (key == "ResignFile") {
                                        if ($('input[name="' + key + '"]')[0].files.length === 0) {
                                            if (!$('input[name="' + key + '"]').hasClass("invalid")) {
                                                $('input[name="' + key + '"]').addClass('invalid').after('<div class="invalid-feedback">' + value + '</div>');
                                                if (key == "ResignFile") {
                                                    $(".custom-file").addClass('invalid');
                                                }
                                            }
                                            validform = false;
                                        }
                                        else {
                                            $('input[name="' + key + '"]').removeClass('invalid');
                                            $('input[name="' + key + '"]').parent().children('.invalid-feedback').remove();
                                            $(".custom-file").removeClass('invalid');
                                        }
                                    }
                                    else {

                                        if ($('#resign-reason').val() == "" || $('#resign-file')[0].files.length === 0) {
                                            if (!$('input[name="' + key + '"]').hasClass("invalid")) {
                                                $('input[name="' + key + '"]').addClass('invalid').after('<div class="invalid-feedback">' + value + '</div>');
                                                if (key == "ResignFile") {
                                                    $(".custom-file").addClass('invalid');
                                                }
                                            }
                                            validform = false;
                                        } else {
                                            $('input.invalid').removeClass('invalid');
                                            $('.invalid-feedback').remove();
                                            if (key == "ResignFile") {
                                                $(".custom-file").removeClass('invalid');
                                            }
                                        }

                                    }
                                });
                                if (!validform) return false;
                            },
                        }).then((result) => {
                            if (result.value) {
                                var fd = new FormData();
                                fd.append("File.File", $('#resign-file')[0].files[0]);
                                fd.append("Reason", $('#resign-reason').val())

                                $.ajax({
                                    url: `/academico/alumnos/informacion/${$("#Id").val()}/solicitud-tramites/renuncia`.proto().parseURL(),
                                    type: "POST",
                                    processData: false,
                                    contentType: false,
                                    data: fd
                                }).done(function () {
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                }).fail(function (e) {
                                    if (e.responseText !== null && e.responseText !== "") {
                                        toastr.error(e.responseText, _app.constants.toastr.title.error);
                                    }
                                    else {
                                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                    }
                                });
                            }
                        })
                    });


                    $(".cancellation-request").on("click", function () {
                        setTimeout(function () {
                            $("#cancellation-file").on('change', function (event) {
                                console.log("cambio");
                                $('.custom-file-label').html(event.target.files[0].name);
                            });
                        }, 1000);

                        swal({
                            title: "Anular ingreso del estudiante",
                            //text: "El alumno será expulsado, removiendolo de todas sus secciones matriculadas. Este proceso es irreversible.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Si, anularlo",
                            confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                            cancelButtonText: "Cancelar",
                            html: `Se realizará la anulación de ingreso del estudiante a la institución. Este proceso es irreversible.</br>
                                       <div class="form-group m-form__group col-xl-12">
                                            <div class="m-input  m-input--solid">
                                                <label for="Reason"></label>
                                                <input type="text" placeholder="Motivo" class="form-control m-input" name="ResignReason" id="resign-reason" required>
                                            </div>
                                        </div>
                                        <div class="form-group m-form__group col-xl-12">
                                            <label asp-for="File"></label>
                                            <div class="custom-file">
                                                <input name="ResignFile" id="cancellation-file" type="file" class="custom-file-input" lang="es"/>
                                                <label class="custom-file-label" for="File">
                                                    Seleccione un archivo
                                                </label>
                                            </div>                                              
                                        </div>`,
                            preConfirm: () => {
                                var errors = {
                                    ResignReason: 'Introduzca un motivo de anulación',
                                    ResignFile: 'Introduzca un archivo'
                                };
                                var validform = true;
                                $.each(errors, function (key, value) {
                                    if (key == "ResignFile") {
                                        if ($('input[name="' + key + '"]')[0].files.length === 0) {
                                            if (!$('input[name="' + key + '"]').hasClass("invalid")) {
                                                $('input[name="' + key + '"]').addClass('invalid').after('<div class="invalid-feedback">' + value + '</div>');
                                                if (key == "ResignFile") {
                                                    $(".custom-file").addClass('invalid');
                                                }
                                            }
                                            validform = false;
                                        }
                                        else {
                                            $('input[name="' + key + '"]').removeClass('invalid');
                                            $('input[name="' + key + '"]').parent().children('.invalid-feedback').remove();
                                            $(".custom-file").removeClass('invalid');
                                        }
                                    }
                                    else {

                                        if ($('#resign-reason').val() == "" || $('#cancellation-file')[0].files.length === 0) {
                                            if (!$('input[name="' + key + '"]').hasClass("invalid")) {
                                                $('input[name="' + key + '"]').addClass('invalid').after('<div class="invalid-feedback">' + value + '</div>');
                                                if (key == "ResignFile") {
                                                    $(".custom-file").addClass('invalid');
                                                }
                                            }
                                            validform = false;
                                        } else {
                                            $('input.invalid').removeClass('invalid');
                                            $('.invalid-feedback').remove();
                                            if (key == "ResignFile") {
                                                $(".custom-file").removeClass('invalid');
                                            }
                                        }

                                    }
                                });
                                if (!validform) return false;
                            },
                        }).then((result) => {
                            if (result.value) {
                                var fd = new FormData();
                                fd.append("File.File", $('#cancellation-file')[0].files[0]);
                                fd.append("Reason", $('#resign-reason').val())

                                $.ajax({
                                    url: `/academico/alumnos/informacion/${$("#Id").val()}/solicitud-tramites/anular-ingreso`.proto().parseURL(),
                                    type: "POST",
                                    processData: false,
                                    contentType: false,
                                    data: fd
                                }).done(function () {
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                }).fail(function (e) {
                                    if (e.responseText !== null && e.responseText !== "") {
                                        toastr.error(e.responseText, _app.constants.toastr.title.error);
                                    }
                                    else {
                                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                    }
                                });
                            }
                        })
                    });

                    $(".reentry-request").on("click", function () {
                        setTimeout(function () {
                            $("#reentry-file").on('change', function (event) {
                                $('.custom-file-label').html(event.target.files[0].name);
                            });
                        }, 1000);

                        swal({
                            title: "Reingreso del estudiante",
                            //text: "El alumno será expulsado, removiendolo de todas sus secciones matriculadas. Este proceso es irreversible.",
                            type: "warning",
                            showCancelButton: true,
                            showLoaderOnConfirm: true,
                            confirmButtonText: "Si, reingresarlo",
                            confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                            cancelButtonText: "Cancelar",
                            html: `El estudiante reingresará a la institución con el estado previo a su retiro. Por favor, adjuntar el documento correspondiente:</br>
                                        <div class="form-group m-form__group">
                                            <label asp-for="File"></label>
                                            <div class="custom-file">
                                                <input name="ReentryFile" id="reentry-file" type="file" class="custom-file-input" lang="es"/>
                                                <label class="custom-file-label" for="File">
                                                    Seleccione un archivo
                                                </label>
                                            </div>                                              
                                        </div>`,
                            preConfirm: () => {
                                return new Promise(() => {
                                    //if ($('#reentry-file')[0].files[0] == null) {
                                    //    toastr.error("Debe ingresar un documento", _app.constants.toastr.title.error);
                                    //    swal.close();
                                    //    return false;
                                    //}

                                    var fd = new FormData();
                                    fd.append("file", $('#reentry-file')[0].files[0]);

                                    $.ajax({
                                        url: `/academico/alumnos/informacion/${$("#Id").val()}/solicitud-tramites/reingreso`.proto().parseURL(),
                                        type: "POST",
                                        processData: false,
                                        contentType: false,
                                        data: fd
                                    }).done(function () {
                                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                    }).fail(function (e) {
                                        if (e.responseText !== null && e.responseText !== "") {
                                            toastr.error(e.responseText, _app.constants.toastr.title.error);
                                        }
                                        else {
                                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                        }
                                    }).always(function () {
                                        swal.close();
                                    });
                                });
                            }
                        })
                    });
                }
            },
            select: {
                campus: {
                    init: function () {
                        $.ajax({
                            url: `/sedes/get`,
                            type: "GET"
                        })
                            .done(function (e) {
                                $("#request-change-campus-modal").find("[name='CampusId']").select2({
                                    data: e.items,
                                    dropdownParent: $("#request-change-campus-modal"),
                                    placeholder: "Seleccionar sede"
                                });
                                $("#request-change-campus-modal").find("[name='CampusId']").val(null).trigger("change");
                            })
                    }
                },
                courses: {
                    init: function () {
                        var id = $("#Id").val();

                        $.ajax({
                            url: `/academico/alumnos/informacion/${id}/cursos-pendientes`.proto().parseURL()
                        }).done(function (data) {
                            $("#evaluationCourseSelect").select2({
                                data: data.items,
                                minimumResultsForSearch: -1
                            });

                            $("#directedCourseSelect").select2({
                                data: data.items,
                                minimumResultsForSearch: -1
                            });
                        });
                    }
                },
                teachers: {
                    init: function () {

                        $("#directedCourseTeacherSelect").select2({
                            ajax: {
                                delay: 300,
                                url: (`/profesores/get/v2`).proto().parseURL(),
                            },
                            allowClear: true,
                            minimumInputLength: 2,
                            placeholder: "Seleccione profesor",
                            dropdownParent: $('#request-directedcourse-modal')
                        });

                        $("#evaluationTeacherSelect").select2({
                            ajax: {
                                delay: 300,
                                url: (`/profesores/get/v2`).proto().parseURL(),
                            },
                            allowClear: true,
                            minimumInputLength: 2,
                            placeholder: "Seleccione profesor",
                            dropdownParent: $('#request-evaluation-modal')
                        });

                        $("#evaluationTeachersCommiteeSelect").select2({
                            ajax: {
                                delay: 300,
                                url: (`/profesores/get/v2`).proto().parseURL(),
                            },
                            allowClear: true,
                            minimumInputLength: 1,
                            placeholder: "Seleccione el comité",
                            dropdownParent: $('#request-evaluation-modal')
                        });
                    }
                },
                enrolledcourses: {
                    init: function () {
                        var id = $("#Id").val();

                        $.ajax({
                            url: `/academico/alumnos/informacion/${id}/cursos-matriculados`.proto().parseURL()
                        }).done(function (data) {
                            $("#enrolledCoursesSelect").select2({
                                data: data,
                                minimumResultsForSearch: -1
                            });
                        });
                    },
                    reload: function () {
                        var id = $("#Id").val();

                        $.ajax({
                            url: `/academico/alumnos/informacion/${id}/cursos-matriculados`.proto().parseURL()
                        }).done(function (data) {
                            $("#enrolledCoursesSelect").empty();

                            $("#enrolledCoursesSelect").select2({
                                data: data,
                                minimumResultsForSearch: -1
                            });
                        });
                    }
                },
                careers: {
                    init: function () {
                        $.ajax({
                            url: `/carreras/get`.proto().parseURL()
                        }).done(function (data) {
                            $("#careerSelect").select2({
                                data: data.items,
                                dropdownParent: $("#request-transfer-modal")
                                //minimumResultsForSearch: -1
                            });
                        });
                        this.events();
                    },
                    events: function () {
                        $("#careerSelect").on("change", function () {
                            let id = $(this).val();
                            pages.procedurerequest.select.curriculums.load(id);
                        });
                    }
                },
                curriculums: {
                    init: function () {
                        $("#curriculumSelect").select2();

                        $.ajax({
                            url: `/planes-estudio/${$("#CareerId").val()}/get`.proto().parseURL()
                        }).done(function (data) {
                            $("#change-curriculum-modal").find("[name='CurriculumId']").select2({
                                data: data.items,
                                dropdownParent: $("#change-curriculum-modal")
                            }).on("change", function () {

                                mApp.block("#change-curriculum-form", {
                                    message: "Cargando programas académicos..."
                                })

                                var curriculumId = $(this).val();

                                $("#change-curriculum-modal").find("[name='AcademicProgramId']").empty();

                                $.ajax({
                                    url: `/programas-academico/get?curriculumId=${curriculumId}`,
                                    type: "GET"
                                })
                                    .done(function (e) {
                                        $("#change-curriculum-modal").find("[name='AcademicProgramId']").empty();

                                        if (e.items.length > 1) {
                                            $(".div-academic-program-container").removeClass("d-none");
                                        } else {
                                            $(".div-academic-program-container").addClass("d-none");
                                        }

                                        $("#change-curriculum-modal").find("[name='AcademicProgramId']").select2({
                                            data: e.items,
                                            placeholder: "Seleccionar programa académico"
                                        })

                                        mApp.unblock("#change-curriculum-form");
                                    })
                            })
                        });

                    },
                    load: function (id) {
                        this.clear();
                        $.ajax({
                            url: `/planes-activos/ultimo-ano/carrera/${id}/get`.proto().parseURL()
                        }).done(function (data) {
                            $("#curriculumSelect").select2({
                                data: data.items,
                                dropdownParent: $("#request-transfer-modal")
                                //minimumResultsForSearch: -1
                            });
                        });
                    },
                    clear: function () {
                        $("#curriculumSelect").empty();
                        $("#curriculumSelect").html(`<option value="0" selected disabled>Seleccione un plan de estudio</option>`);
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
                },
                courseCertificates: {
                    init: function () {
                        var id = $("#Id").val();

                        $.ajax({
                            url: `/certificados-cursos`.proto().parseURL()
                        }).done(function (data) {
                            $("#courseCertificateSelect").select2({
                                data: data.items,
                                minimumResultsForSearch: 10
                            });
                        });
                    }
                },
                benefits: {
                    init: function () {
                        $.ajax({
                            url: `/beneficios-estudiantes/get`.proto().parseURL(),
                            data: {
                                studentId: $("#Id").val()
                            }
                        }).done(function (data) {
                            $("#benefitSelect").select2({
                                data: data.items,
                                minimumResultsForSearch: 10
                            });

                            if (data.selected != null) {
                                $("#benefitSelect").val(data.selected).trigger("change");
                            }
                        });
                    }
                },
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

                                var formData = new FormData($(form).get(0));
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
                reservation: {
                    object: null,
                    clear: function () {
                        this.object.resetForm();
                    },
                    init: function () {
                        this.object = $("#reservation-request-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();

                                mApp.block("#reservation-request-form");

                                var formData = new FormData($(form).get(0));

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
                                        pages.procedurerequest.form.reservation.clear();
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#reservation_msg_txt").html(error.responseText);
                                        else $("#reservation_msg_txt").html(_app.constants.ajax.message.error);

                                        $("#reservation-modal-form-alert").removeClass("m--hide").show();
                                    })
                                    .always(function () {
                                        mApp.unblock("#reservation-request-form");
                                    });
                            }
                        });

                        $(".reservation-file").on("change", function () {
                            $(this).valid();

                            var fileName = $(this).val();
                            $(this).next().html(fileName);
                        });
                    }
                },
                extemporaneous: {
                    object: null,
                    clear: function () {
                        this.object.resetForm();
                    },
                    init: function () {
                        this.object = $("#extemporaneous-enrollment-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();

                                mApp.block("#extemporaneous-enrollment-form");

                                var formData = new FormData($(form).get(0));

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
                                        pages.procedurerequest.form.extemporaneous.clear();
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#extemporaneous_msg_txt").html(error.responseText);
                                        else $("#extemporaneous_msg_txt").html(_app.constants.ajax.message.error);

                                        $("#extemporaneous-enrollment-modal-form-alert").removeClass("m--hide").show();
                                    })
                                    .always(function () {
                                        mApp.unblock("#extemporaneous-enrollment-form");
                                    });
                            }
                        });

                        $(".extemporaneous-file").on("change", function () {
                            $(this).valid();

                            var fileName = $(this).val();
                            $(this).next().html(fileName);
                        });
                    }
                },
                changeCampus: {
                    object: null,
                    clear: function () {
                        this.object.resetForm();
                        $("#request-change-campus-modal").find("[name='CampusId']").val(null).trigger("change");
                    },
                    init: function () {
                        this.object = $("#request-change-campus-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();

                                mApp.block("#request-change-campus-form");

                                $.ajax({
                                    url: $(form).attr("action"),
                                    type: "POST",
                                    data: $(form).serialize()
                                })
                                    .done(function () {
                                        $(".modal").modal("hide");
                                        $(".m-alert").addClass("m--hide");

                                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                        pages.procedurerequest.form.transfer.clear();
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#change_campus_msg_txt").html(error.responseText);
                                        else $("#change_campus_msg_txt").html(_app.constants.ajax.message.error);

                                        $("#change-campus-form-alert").removeClass("m--hide").show();
                                    })
                                    .always(function () {
                                        mApp.unblock("#request-change-campus-form");
                                    });
                            }
                        });

                        $("#request-change-campus-modal").on("hidden.bs.modal", function () {
                            pages.procedurerequest.form.changeCampus.clear();
                        })

                    }
                },
                transfer: {
                    object: null,
                    clear: function () {
                        this.object.resetForm();
                        $("#careerSelect").val(0).trigger("change");
                        $("#curriculumSelect").val(0).trigger("change");
                    },
                    init: function () {
                        this.object = $("#transfer-request-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();

                                mApp.block("#transfer-request-form");

                                $.ajax({
                                    url: $(form).attr("action"),
                                    type: "POST",
                                    data: $(form).serialize()
                                })
                                    .done(function () {
                                        $(".modal").modal("hide");
                                        $(".m-alert").addClass("m--hide");

                                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                        pages.procedurerequest.form.transfer.clear();
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#transfer_msg_txt").html(error.responseText);
                                        else $("#transfer_msg_txt").html(_app.constants.ajax.message.error);

                                        $("#transfer-modal-form-alert").removeClass("m--hide").show();
                                    })
                                    .always(function () {
                                        mApp.unblock("#transfer-request-form");
                                    });
                            }
                        });
                        $("#request-transfer-modal").on("hidden.bs.modal", function () {
                            pages.procedurerequest.form.transfer.clear();
                        })

                    }
                },
                directedcourse: {
                    object: null,
                    clear: function () {
                        this.object.resetForm();
                        $("#directedCourseSelect").val(null).trigger("change");
                    },
                    init: function () {
                        this.object = $("#directedcourse-request-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();

                                mApp.block("#directedcourse-request-form");

                                var formData = new FormData($(form).get(0));

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
                                        pages.procedurerequest.form.directedcourse.clear();
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#directedcourse_msg_txt").html(error.responseText);
                                        else $("#directedcourse_msg_txt").html(_app.constants.ajax.message.error);

                                        $("#directedcourse-modal-form-alert").removeClass("m--hide").show();
                                    })
                                    .always(function () {
                                        mApp.unblock("#directedcourse-request-form");
                                    });
                            }
                        });

                        $(".directedcourse-file").on("change", function () {
                            $(this).valid();

                            var fileName = $(this).val();
                            $(this).next().html(fileName);
                        });
                    }
                },
                coursewithdrawal: {
                    object: null,
                    clear: function () {
                        this.object.resetForm();
                        $("#enrolledCoursesSelect").val(null).trigger("change");
                    },
                    init: function () {
                        this.object = $("#coursewithdrawal-request-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();

                                if ($("#enrolledCoursesSelect").val() == null) {
                                    $("#coursewithdrawal_msg_txt").html("Debe seleccionar un curso");
                                    $("#coursewithdrawal-modal-form-alert").removeClass("m--hide").show();
                                    return false;
                                }

                                swal({
                                    title: "Retirar de asignatura",
                                    text: "Se retirará al estudiante de la asignatura seleccionada",
                                    type: "warning",
                                    showCancelButton: true,
                                    confirmButtonText: "Si, retirar",
                                    confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                                    cancelButtonText: "Cancelar"
                                }).then(function (result) {
                                    if (result.value) {
                                        mApp.block("#coursewithdrawal-request-form");

                                        $.ajax({
                                            url: $(form).attr("action"),
                                            type: "POST",
                                            data: $(form).serialize()
                                        })
                                            .done(function () {
                                                $(".modal").modal("hide");
                                                $(".m-alert").addClass("m--hide");

                                                pages.procedurerequest.select.enrolledcourses.reload();

                                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                                pages.procedurerequest.form.coursewithdrawal.clear();
                                            })
                                            .fail(function (error) {
                                                if (error.responseText !== null && error.responseText !== "") $("#coursewithdrawal_msg_txt").html(error.responseText);
                                                else $("#coursewithdrawal_msg_txt").html(_app.constants.ajax.message.error);

                                                $("#coursewithdrawal-modal-form-alert").removeClass("m--hide").show();
                                            })
                                            .always(function () {
                                                mApp.unblock("#coursewithdrawal-request-form");
                                            });
                                    }
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
                amnesty: {
                    object: null,
                    clear: function () {
                        this.object.resetForm();
                    },
                    init: function () {
                        this.object = $("#amnesty-request-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();

                                mApp.block("#amnesty-request-form");

                                $.ajax({
                                    url: $(form).attr("action"),
                                    type: "POST",
                                    data: $(form).serialize()
                                })
                                    .done(function (data) {
                                        $(".modal").modal("hide");
                                        //$(".m-alert").addClass("m--hide");
                                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                                        $("#CurriculumId").val(data.id);
                                        $("#curriculum-label").text(data.code);
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#amnesty-request-alert-txt").html(error.responseText);
                                        else $("#amnesty-request-alert-txt").html(_app.constants.ajax.message.error);

                                        $("#amnesty-request-alert").removeClass("m--hide").show();

                                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);

                                    })
                                    .always(function () {
                                        mApp.unblock("#amnesty-request-form");
                                    });
                            }
                        });
                    }
                },

                courseCertificate: {
                    object: null,
                    clear: function () {
                        this.object.resetForm();
                        $("#courseCertificateSelect").val(null).trigger("change");
                    },
                    init: function () {
                        this.object = $("#coursecertificate-request-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();

                                mApp.block("#coursecertificate-request-form");

                                var formData = new FormData($(form).get(0));

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
                                        pages.procedurerequest.form.courseCertificate.clear();
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#coursecertificate_msg_txt").html(error.responseText);
                                        else $("#coursecertificate_msg_txt").html(_app.constants.ajax.message.error);

                                        $("#coursecertificate-modal-form-alert").removeClass("m--hide").show();
                                    })
                                    .always(function () {
                                        mApp.unblock("#coursecertificate-request-form");
                                    });
                            }
                        });

                        $(".directedcourse-file").on("change", function () {
                            $(this).valid();

                            var fileName = $(this).val();
                            $(this).next().html(fileName);
                        });
                    }
                },
                changeCurriculum: {
                    object: null,
                    init: function () {
                        this.object = $("#change-curriculum-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();
                                mApp.block("#change-curriculum-form");

                                var formData = new FormData($(form).get(0));
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
                                        pages.procedurerequest.form.evaluation.clear();
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#change_curriculum_msg_txt").html(error.responseText);
                                        else $("#change_curriculum_msg_txt").html(_app.constants.ajax.message.error);

                                        $("#change-curriculum-form-alert").removeClass("m--hide").show();
                                    })
                                    .always(function () {
                                        mApp.unblock("#change-curriculum-form");
                                    });
                            }
                        })
                    }
                },

                changeBenefit: {
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
            datatablePay: {
                paymentmades: {
                    object: null,
                    options: {
                        ajax: {
                            url: `/academico/alumnos/informacion/${$("#Id").val()}/solicitud-tramites/getpaymenthistory`.proto().parseURL(),
                            type: "GET",
                            data: function (data) {

                            }
                        },
                        columns: [
                            {
                                data: "type",
                                title: "Tipo"
                            },
                            {
                                data: 'description',
                                title: 'Descripción'
                            },
                            {
                                data: 'paymentDate',
                                title: 'Fec. Pago'
                            },
                            {
                                data: 'term',
                                title: 'Periodo'
                            },
                            {
                                data: 'total',
                                title: 'Total'
                            },
                            {
                                data: null,
                                title: 'Estado',
                                render: function (data) {
                                    switch (data.status) {
                                        case 1:
                                            return `<span class="m-badge m-badge--metal m-badge--wide">Pendiente</span>`;
                                        case 2:
                                            return `<span class="m-badge m-badge--primary m-badge--wide">Pagado</span>`;
                                        case 3:
                                            return `<span class="m-badge m-badge--danger m-badge--wide">Anulado</span>`;
                                        default:
                                            return "-";
                                    }
                                }
                            }
                        ]
                    },

                    init: function () {
                        this.object = $("#payment-table").DataTable(this.options);
                    }
                }
            },
            init: function () {
                this.select.courses.init();
                this.select.careers.init();
                this.select.curriculums.init();
                this.select.academicPrograms.init();
                this.select.teachers.init();
                this.select.enrolledcourses.init();
                this.select.courseCertificates.init();
                this.select.campus.init();
                this.select.benefits.init();

                //this.datatablePay.paymentmades.init();
                $("[data-switch=true]").bootstrapSwitch();

                this.form.evaluation.init();
                this.form.reservation.init();

                this.form.extemporaneous.init();
                this.form.transfer.init();
                this.form.academicProgram.init();
                this.form.directedcourse.init();
                this.form.coursewithdrawal.init();
                this.form.amnesty.init();
                this.form.courseCertificate.init();
                this.form.changeCurriculum.init();
                this.form.changeCampus.init();
                this.events.init();
            }
        },
        observations: {
            datatable: {
                observations: {
                    object: null,
                    options: getSimpleDataTableConfiguration({
                        url: `/academico/alumnos/informacion/${studentid}/observaciones/get`.proto().parseURL(),
                        data: function (data) {
                            //data.search = $("#search").val();
                        },
                        pageLength: 10,
                        ordering: false,
                        columns: [
                            {
                                data: "type",
                                title: "Tipo"
                            },
                            {
                                data: "observation",
                                title: "Observaciones"
                            },
                            {
                                data: "user",
                                title: "Usuario"
                            },
                            {
                                data: "date",
                                title: "Fecha"
                            },
                            {
                                data: null,
                                title: "Opciones",
                                orderable: false,
                                render: function (data) {
                                    var template = "";
                                    template += "<button ";
                                    template += "class='btn btn-info btn-studentObservationView ";
                                    template += "m-btn btn-sm  m-btn--icon-only' ";
                                    template += " data-id='" + data.id + "'>";
                                    template += "<i class='la la-eye'></i></button>";
                                    template += " ";
                                    return template;
                                }
                            }
                        ]
                    }),
                    init: function () {
                        this.object = $("#observations_table").DataTable(this.options);
                        this.events();
                    },
                    reload: function () {
                        this.object.ajax.reload();
                    },
                    events: function () {
                        $("#observations_table").on('click', '.btn-studentObservationView', function () {
                            let id = $(this).data("id");
                            //ModalTODO
                            pages.observations.modal.detailObservation.load(id);
                        });
                    }
                }
            },
            form: {
                observation: {
                    object: null,
                    clear: function () {
                        $("#create-observation-modal").find("[name='observationType']").val(1).trigger("change");
                        $("#create-observation-modal").find("[name='termId']").val(null).trigger("change");
                        this.object.resetForm();
                    },
                    init: function () {
                        this.object = $("#create-observation-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();

                                mApp.block("#create-observation-form");
                                var formData = new FormData(form);
                                $.ajax({
                                    url: `/academico/alumnos/informacion/${studentid}/observaciones/crear`.proto().parseURL(),
                                    type: "POST",
                                    data: formData,
                                    processData: false,
                                    contentType: false
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
            modal: {
                detailObservation: {
                    hide: function () {
                        $("#detail-observation-modal").modal("hide");
                    },
                    show: function () {
                        $("#detail-observation-modal").modal("show");
                    },
                    load: function (id) {
                        $.ajax({
                            url: `/academico/alumnos/informacion/observacion/${id}/get`.proto().parseURL(),
                            type: "GET"
                        })
                            .done(function (data) {
                                $('#detail-observation-modal').find("[name='termName']").val(data.term)
                                $('#detail-observation-modal').find("[name='observationType']").val(data.observationTypeName)
                                $('#detail-observation-modal textarea[name="observation"]').text(data.observation);
                                if (!(data.observationFile == null || data.observationFile == "")) {
                                    $('#fileContainer').css('display', 'block');
                                    $('#detail-observation-modal .btn-downloadObservationFile').attr('href', `/documentos/${data.observationFile}`);
                                }
                                pages.observations.modal.detailObservation.show();
                            })
                            .fail(function (error) {
                            })
                            .always(function () {
                            });
                    },
                    clear: function () {
                        $('#detail-observation-modal textarea[name="observation"]').text('');
                        $('#fileContainer').css('display', 'none');
                        $('#detail-observation-modal .btn-downloadObservationFile').attr('href', '');
                    },
                    events: function () {
                        $("#detail-observation-modal").on('hidden.bs.modal', function () {
                            pages.observations.modal.detailObservation.clear();
                        });
                    },
                    init: function () {
                        this.events();
                    }
                },
                init: function () {
                    this.detailObservation.init();
                }
            },
            select: {
                term: {
                    load: function () {
                        $.ajax({
                            url: `/periodos/get`,
                            type: "GET"
                        })
                            .done(function (e) {
                                $("#create-observation-modal").find("[name='termId']").select2({
                                    data: e.items,
                                    placeholder: "Seleccionar periodo académico"
                                });

                                if (e.selected != null) {
                                    $("#create-observation-modal").find("[name='termId']").val(e.selected).trigger("change");
                                }
                            })
                    },
                    init: function () {
                        this.load();
                    }
                },
                type: {
                    load: function () {
                        $("#create-observation-modal").find("[name='observationType']").select2({
                            dropdownParent: $("#create-observation-modal")
                        });
                    },
                    events: {
                        onChange: function () {
                            $("#create-observation-modal").find("[name='observationType']").on("change", function () {
                                var value = $(this).val();
                                console.log(value);
                                switch (value) {
                                    case "1":
                                        $("#create-observation-modal").find(".student_observation_term").addClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", true);
                                        break;

                                    case "2":
                                        $("#create-observation-modal").find(".student_observation_term").removeClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", false);
                                        break;

                                    case "6":
                                        $("#create-observation-modal").find(".student_observation_term").removeClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", false);
                                        break;

                                    case "10":
                                        $("#create-observation-modal").find(".student_observation_term").removeClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", false);
                                        break;

                                    case "11":
                                        $("#create-observation-modal").find(".student_observation_term").addClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", true);
                                        break;

                                    case "12":
                                        $("#create-observation-modal").find(".student_observation_term").removeClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", false);
                                        break;

                                    case "16":
                                        $("#create-observation-modal").find(".student_observation_term").removeClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", false);
                                        break;

                                    default:
                                        $("#create-observation-modal").find(".student_observation_term").addClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", true);
                                        break;
                                }
                            })
                        },
                        init: function () {
                            this.onChange();
                        }
                    },
                    init: function () {
                        this.load();
                        this.events.init();
                    }
                },
                init: function () {
                    this.type.init();
                    this.term.init();
                }
            },
            init: function () {
                this.modal.init();
                this.datatable.observations.init();
                this.form.observation.init();
                this.select.init();
            }
        },
        studentSituation: {
            init: function () {
                pages.studentSituation.academicCicles.init();
                pages.studentSituation.electiveCourses.init();
            },
            academicCicles: {
                options: {
                    data: {
                        type: "remote",
                        source: {
                            read: {
                                method: "GET",
                                url: ""
                            }
                        },
                        pageSize: 10,
                        saveState: {
                            cookie: true,
                            webstorage: true,
                        }
                    },
                    pagination: false,
                    columns: [
                        {
                            field: "course",
                            title: "Curso",
                            width: 200,
                            sortable: false
                        },
                        {
                            field: "credits",
                            title: "Total",
                            textAlign: "center",
                            width: 80,
                            sortable: false
                        },
                        {
                            field: "try",
                            title: "N° de Veces",
                            textAlign: "center",
                            width: 120,
                            sortable: false
                        },
                        {
                            field: "grade",
                            title: "Promedio Final",
                            textAlign: "center",
                            width: 120,
                            sortable: false,
                            template: function (data) {
                                if (data.validated) {
                                    return `<span class="m--font-info"> ${data.grade} </span>`;
                                }
                                return data.grade;
                            }
                        },
                        {
                            field: "term",
                            title: "Ciclo",
                            textAlign: "center",
                            width: 80,
                            sortable: false
                        },
                        {
                            field: "status",
                            title: "Estado",
                            textAlign: "center",
                            width: 100,
                            sortable: false,
                            template: function (row) {
                                return row.status ? "Cumplido" : "Pendiente";
                            }
                        }
                    ]
                },
                datatable: {
                    init: {
                        all: function () {
                            $(".academic-year-datatable").each(function (index, datatable) {
                                var ayid = $(datatable).data("ayid");
                                var studentId = location.pathname.match(/(.{0,8}-.{0,4}-.{0,4}-.{0,4}-.{0,12})/).pop();
                                pages.studentSituation.academicCicles.options.data.source.read.url = `/alumnos/${studentId}/situacion/nivel/${ayid}/get`.proto().parseURL();
                                datatable.id = `academic-year-${index}`;
                                datatable.dataset.number = index;
                                $("#" + datatable.id).mDatatable(pages.studentSituation.academicCicles.options);
                            });
                        }
                    },
                    events: {
                        load: function () {
                            $(".academic-year-datatable").on("m-datatable--on-init", function () {
                                var totalCredits = 0;
                                var allApproved = true;

                                $(`#${this.id} td[data-field="credits"]`).each(function () {
                                    var value = $(this).text();
                                    if (!isNaN(value) && value.length > 0 && !isNaN(parseInt(value))) {
                                        totalCredits += parseInt(value);
                                    }
                                });

                                $(`#${this.id} td[data-field="status"`).each(function () {
                                    var value = $(this).text();
                                    if (value.length > 0) {
                                        allApproved = allApproved && (value === "Cumplido");
                                    }
                                });

                                var num = $(this).data("number");
                                var oldTitle = $(`#m-accordion-title-${num}`).text();
                                $(`#m-accordion-title-${num}`).html(`${oldTitle}&emsp;<i class="fa fa-angle-double-right"></i>&emsp;${totalCredits} créditos`);
                                var iconClass = (allApproved ? "fa fa-check" : "fa fa-exclamation-triangle");
                                $(`#m-accordion-icon-${num}`).html(`<i class="${iconClass}"></i>`);
                                $(`#m-accordion-icon-${num}`).addClass(allApproved ? "text-success" : "text-warning");
                            });
                        }
                    }
                },
                init: function () {
                    pages.studentSituation.academicCicles.datatable.init.all();
                    pages.studentSituation.academicCicles.datatable.events.load();
                },
            },
            electiveCourses: {
                options: {
                    data: {
                        type: "remote",
                        source: {
                            read: {
                                method: "GET",
                                url: "/alumno/progreso/electivos/get".proto().parseURL()
                            }
                        },
                        pageSize: 10,
                        saveState: {
                            cookie: true,
                            webstorage: true,
                        }
                    },
                    pagination: false,
                    columns: [
                        {
                            field: "course",
                            title: "Curso",
                            width: 200,
                            sortable: false
                        },
                        {
                            field: "academicYear",
                            title: "Nivel",
                            width: 70,
                            textAlign: "center",
                            sortable: false
                        },
                        {
                            field: "credits",
                            title: "Créditos",
                            textAlign: "center",
                            width: 80,
                            sortable: false
                        },
                        {
                            field: "try",
                            title: "N° de Veces",
                            textAlign: "center",
                            width: 100,
                            sortable: false
                        },
                        {
                            field: "grade",
                            title: "Promedio Final",
                            textAlign: "center",
                            width: 120,
                            sortable: false
                        },
                        {
                            field: "term",
                            title: "Ciclo",
                            textAlign: "center",
                            width: 80,
                            sortable: false
                        },
                        {
                            field: "status",
                            title: "Estado",
                            textAlign: "center",
                            width: 100,
                            sortable: false,
                            template: function (row) {
                                return row.status ? "Cumplido" : "-";
                            }
                        }
                    ]
                },
                datatable: {
                    init: {
                        all: function () {
                            var studentId = location.pathname.match(/(.{0,8}-.{0,4}-.{0,4}-.{0,4}-.{0,12})/).pop();
                            pages.studentSituation.electiveCourses.options.data.source.read.url = `/alumnos/${studentId}/situacion/electivos/get`.proto().parseURL();
                            $(".elective-courses-datatable").mDatatable(pages.studentSituation.electiveCourses.options);
                        }
                    }
                },
                init: function () {
                    pages.studentSituation.electiveCourses.datatable.init.all();
                }
            }

        },
        studentRecord: {
            options: {
                data: {
                    type: "remote",
                    source: {
                        read: {
                            method: "GET",
                            url: ""
                        }
                    },
                    pageSize: 10,
                    saveState: {
                        cookie: true,
                        webstorage: true
                    }
                },
                pagination: false,
                columns: [
                    {
                        field: "course",
                        title: "Curso",
                        width: 200,
                        sortable: false
                    },
                    {
                        field: "academicYear",
                        title: "Ciclo",
                        textAlign: "center",
                        width: 80,
                        sortable: false
                    },
                    {
                        field: "credits",
                        title: "Créditos",
                        textAlign: "center",
                        width: 80,
                        sortable: false
                    },
                    {
                        field: "finalGrade",
                        title: "Promedio Final",
                        textAlign: "center",
                        width: 120,
                        sortable: false
                    },
                    {
                        field: "try",
                        title: "N° de Veces",
                        textAlign: "center",
                        width: 120,
                        sortable: false
                    }
                ]
            },
            load: {
                all: function () {
                    $(".summary-detail-datatable").each(function (index, datatable) {
                        var pid = $(datatable).data("pid");
                        var studentId = location.pathname.match(/(.{0,8}-.{0,4}-.{0,4}-.{0,4}-.{0,12})/).pop();
                        pages.studentRecord.options.data.source.read.url = `/alumnos/${studentId}/historial/periodo/${pid}/get`.proto().parseURL();
                        datatable.id = `summary-detail-${index}`;
                        $(`#${datatable.id}`).mDatatable(pages.studentRecord.options);
                    });

                    $(".btn-export").on("click",
                        function () {
                            var $btn = $(this);
                            var studentId = location.pathname.match(/(.{0,8}-.{0,4}-.{0,4}-.{0,4}-.{0,12})/).pop();
                            $btn.addLoader();
                            $.fileDownload(`/admin/situacion-academica/${studentId}/historial/reporte`.proto().parseURL())
                                .always(function () {
                                    $btn.removeLoader();
                                }).done(function () {
                                    toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                }).fail(function () {
                                    toastr.error("No se pudo descargar el archivo", "Error");
                                });
                            return false;
                        });
                }
            },
            init: function () {
                pages.studentRecord.load.all();
            }
        },

        portfolio: {
            datatable: {
                portfolio: {
                    object: null,
                    options: getSimpleDataTableConfiguration({
                        serverSide: false,
                        url: `/academico/alumnos/informacion/${studentid}/portafolio/get`.proto().parseURL(),
                        data: function (data) {
                            //data.search = $("#search").val();
                        },
                        pageLength: 50,
                        ordering: false,
                        columns: [
                            //{
                            //    data: "id",
                            //    title: "N°"
                            //},
                            {
                                data: "name",
                                title: "Requisito"
                            },
                            {
                                data: null,
                                title: "Entregado",
                                orderable: false,
                                render: function (data) {
                                    if (data.received) {
                                        return `<span class="m-badge m-badge--success m-badge--wide"> Si </span>`;
                                    }
                                    return `<span class="m-badge m-badge--danger m-badge--wide"> No </span>`;
                                }
                            },
                            {
                                data: null,
                                title: "Documento",
                                orderable: false,
                                render: function (data) {
                                    if (data.file == null || data.file == "") {
                                        return `<button class='btn btn-primary btn-upload m-btn btn-sm' data-id='${data.id}' data-name='${data.name}'><i class='la la-cloud-upload'></i> Subir</button>`;
                                    }

                                    return `<button class='btn btn-info btn-show btn-sm m-btn m-btn--icon' data-obj='${data.proto().encode()}'>
                                                    <span><i class='la la-eye'></i><span>Ver documento</span></span>
                                            </button>`;
                                }
                            },
                            {
                                data: null,
                                title: "Validado",
                                orderable: false,
                                render: function (data) {
                                    if (data.validated) {
                                        return `<span class="m-badge m-badge--success m-badge--wide"> Si </span>`;
                                    }
                                    return `<span class="m-badge m-badge--danger m-badge--wide"> No </span>`;
                                }
                            },
                        ]
                    }),
                    init: function () {
                        this.object = $("#portfolio_table").DataTable(this.options);
                        this.events();
                    },
                    reload: function () {
                        this.object.ajax.reload();
                    },
                    events: function () {
                        $("#portfolio_table").on('click', '.btn-upload', function () {
                            var id = $(this).data("id");
                            var name = $(this).data("name");

                            $('#create-portfolio-modal .modal-title').html("Portafolio - " + name);
                            $('#create-portfolio-modal').find("[name='Type']").val(id);
                            $("#create-portfolio-modal").modal("toggle");
                        });

                        $("#portfolio_table").on('click', '.btn-show', function () {
                            var obj = $(this).data("obj");
                            obj = obj.proto().decode();

                            $('#detail-portfolio-modal .modal-title').html("Portafolio - " + obj.name);

                            var html = "";
                            if (obj.file.includes('.pdf') || obj.file.includes('.PDF')) {
                                $("#detail-portfolio-modal-container").addClass('modal-xl');

                                html = `<object width="100%" height="500" hspace="0" vspace="0" src="/documentos/${obj.file}">
                                            <embed width="100%" height="500" hspace="0" vspace="0" src="/documentos/${obj.file}"></embed>
                                            </object>`;
                            }
                            else {
                                $("#detail-portfolio-modal-container").removeClass('modal-xl');

                                html = `<a href="/documentos/${obj.file}" class ="btn btn-info m-btn m-btn--icon" title="Descargar Archivo" download>
                                                    <span><i class='la la-file-pdf-o'></i><span>Descargar archivo</span></span>
                                                </a>`;
                            }

                            if (obj.validated) {
                                $('#btn-verify').html('Invalidar');
                                $('#btn-verify').removeClass('btn-success');
                                $('#btn-verify').addClass('btn-danger');
                            }
                            else {
                                $('#btn-verify').html('Verificar');
                                $('#btn-verify').addClass('btn-success');
                                $('#btn-verify').removeClass('btn-danger');
                            }

                            $('#detail-portfolio-type').val(obj.id);
                            $('#detail-portfolio-content').html(html);

                            $("#detail-portfolio-modal").modal("toggle");
                        });
                    }
                }
            },
            form: {
                portfolio: {
                    object: null,
                    clear: function () {
                        this.object.resetForm();
                    },
                    init: function () {
                        this.object = $("#create-portfolio-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();

                                mApp.block("#create-portfolio-form");
                                var formData = new FormData(form);
                                $.ajax({
                                    url: `/academico/alumnos/informacion/${studentid}/portafolio/crear`.proto().parseURL(),
                                    type: "POST",
                                    data: formData,
                                    processData: false,
                                    contentType: false
                                })
                                    .done(function () {
                                        $(".modal").modal("hide");
                                        $(".m-alert").addClass("m--hide");

                                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                        pages.portfolio.form.portfolio.clear();
                                        pages.portfolio.datatable.portfolio.reload();
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#portfolio_msg_txt").html(error.responseText);
                                        else $("#portfolio_msg_txt").html(_app.constants.ajax.message.error);

                                        $("#portfolio-modal-form-alert").removeClass("m--hide").show();
                                    })
                                    .always(function () {
                                        mApp.unblock("#create-portfolio-form");
                                    });
                            }
                        });
                    }
                },
                verify: {
                    object: null,
                    init: function () {
                        this.object = $("#portfolio-verify-form").validate({
                            submitHandler: function (form, e) {
                                e.preventDefault();

                                mApp.block("#create-portfolio-form");
                                var formData = new FormData(form);
                                $.ajax({
                                    url: `/academico/alumnos/informacion/${studentid}/portafolio/crear`.proto().parseURL(),
                                    type: "POST",
                                    data: formData,
                                    processData: false,
                                    contentType: false
                                })
                                    .done(function () {
                                        $(".modal").modal("hide");
                                        $(".m-alert").addClass("m--hide");

                                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                        pages.portfolio.form.portfolio.clear();
                                        pages.portfolio.datatable.portfolio.reload();
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#portfolio_msg_txt").html(error.responseText);
                                        else $("#portfolio_msg_txt").html(_app.constants.ajax.message.error);

                                        $("#portfolio-modal-form-alert").removeClass("m--hide").show();
                                    })
                                    .always(function () {
                                        mApp.unblock("#create-portfolio-form");
                                    });
                            }
                        });
                    }
                }
            },
            modal: {
                detailObservation: {
                    hide: function () {
                        $("#detail-observation-modal").modal("hide");
                    },
                    show: function () {
                        $("#detail-observation-modal").modal("show");
                    },
                    load: function (id) {
                        $.ajax({
                            url: `/academico/alumnos/informacion/observacion/${id}/get`.proto().parseURL(),
                            type: "GET"
                        })
                            .done(function (data) {
                                $('#detail-observation-modal').find("[name='termName']").val(data.term)
                                $('#detail-observation-modal').find("[name='observationType']").val(data.observationTypeName)
                                $('#detail-observation-modal textarea[name="observation"]').text(data.observation);
                                if (!(data.observationFile == null || data.observationFile == "")) {
                                    $('#fileContainer').css('display', 'block');
                                    $('#detail-observation-modal .btn-downloadObservationFile').attr('href', `/documentos/${data.observationFile}`);
                                }
                                pages.observations.modal.detailObservation.show();
                            })
                            .fail(function (error) {
                            })
                            .always(function () {
                            });
                    },
                    clear: function () {
                        $('#detail-observation-modal textarea[name="observation"]').text('');
                        $('#fileContainer').css('display', 'none');
                        $('#detail-observation-modal .btn-downloadObservationFile').attr('href', '');
                    },
                    events: function () {
                        $("#detail-observation-modal").on('hidden.bs.modal', function () {
                            pages.observations.modal.detailObservation.clear();
                        });
                    },
                    init: function () {
                        this.events();
                    }
                },
                init: function () {
                    this.detailObservation.init();
                }
            },
            select: {
                term: {
                    load: function () {
                        $.ajax({
                            url: `/periodos/get`,
                            type: "GET"
                        })
                            .done(function (e) {
                                $("#create-observation-modal").find("[name='termId']").select2({
                                    data: e.items,
                                    placeholder: "Seleccionar periodo académico"
                                });

                                if (e.selected != null) {
                                    $("#create-observation-modal").find("[name='termId']").val(e.selected).trigger("change");
                                }
                            })
                    },
                    init: function () {
                        this.load();
                    }
                },
                type: {
                    load: function () {
                        $("#create-observation-modal").find("[name='observationType']").select2({
                            dropdownParent: $("#create-observation-modal")
                        });
                    },
                    events: {
                        onChange: function () {
                            $("#create-observation-modal").find("[name='observationType']").on("change", function () {
                                var value = $(this).val();
                                console.log(value);
                                switch (value) {
                                    case "1":
                                        $("#create-observation-modal").find(".student_observation_term").addClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", true);
                                        break;

                                    case "2":
                                        $("#create-observation-modal").find(".student_observation_term").removeClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", false);
                                        break;

                                    case "6":
                                        $("#create-observation-modal").find(".student_observation_term").removeClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", false);
                                        break;

                                    case "10":
                                        $("#create-observation-modal").find(".student_observation_term").removeClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", false);
                                        break;

                                    case "11":
                                        $("#create-observation-modal").find(".student_observation_term").addClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", true);
                                        break;

                                    case "12":
                                        $("#create-observation-modal").find(".student_observation_term").removeClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", false);
                                        break;

                                    case "16":
                                        $("#create-observation-modal").find(".student_observation_term").removeClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", false);
                                        break;

                                    default:
                                        $("#create-observation-modal").find(".student_observation_term").addClass("d-none");
                                        $("#create-observation-modal").find("[name='termId']").attr("disabled", true);
                                        break;
                                }
                            })
                        },
                        init: function () {
                            this.onChange();
                        }
                    },
                    init: function () {
                        this.load();
                        this.events.init();
                    }
                },
                init: function () {
                    this.type.init();
                    this.term.init();
                }
            },
            events: function () {
                $("#detail-portfolio-modal").on("click", ".btn-verify", function () {
                    var btn = $(this);
                    btn.addLoader();

                    $.ajax({
                        url: `/academico/alumnos/informacion/${studentid}/portafolio/validar`.proto().parseURL(),
                        type: "POST",
                        data: {
                            type: $("#detail-portfolio-type").val()
                        }
                    })
                        .done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#detail-portfolio-modal").modal("toggle");

                            pages.portfolio.datatable.portfolio.reload();
                        })
                        .fail(function () {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        })
                        .always(function () {
                            btn.removeLoader();
                        });
                });
            },
            init: function () {
                //this.modal.init();
                this.datatable.portfolio.init();
                this.form.portfolio.init();
                //this.select.init();
                this.events();
            }
        },
        clear: function () {
            pages.assistancehistory.clear();
            pages.reportcard.clear();
            pages.curriculumprogress.clear();
        }
    };

    var profileViewer = {
        init: function () {
            this.modal.init();
            this.cropper.init();
        },
        modal: {
            init: function () {
                $("#btnProfileImageViewer").on("click", function () {
                    $("#modal-profile-viewer").modal("show");
                });
            }
        },
        cropper: {
            init: function () {
                $('#ProfilePicture').on('change', function () {
                    var reader = new FileReader();
                    reader.onload = function (event) {

                        var image = $('#img-offi')[0];
                        var container = $('.cropper-container cropper-bg').prevObject;
                        //$("#upload-offi div").html("");
                        $("#upload-offi div").remove();
                        var cropper = new Cropper(image);
                        cropper.destroy();
                        cropper.reset();
                        cropper.clear();
                        cropper = null;


                        container.remove();

                        $('.preview').css({
                            width: '100%', //width,  sets the starting size to the same as orig image
                            overflow: 'hidden',
                            height: 300,
                            maxWidth: 300,
                            maxHeight: 300
                        });


                        $("#img-offi").attr("src", event.target.result);
                        $("#cropper-hide").attr("src", event.target.result);


                        cropper = new Cropper(image, {
                            dragMode: 'move',
                            preview: '.preview',
                            aspectRatio: 12 / 12,
                            minContainerWidth: 300,
                            maxContainerWidth: 300,
                            minContainerHeight: 300,
                            maxContainerHeight: 300,
                            background: false,
                            viewMode: 1, aspectRatio: 1,
                            responsive: true,
                            autoCrop: true,
                            ready: function () {
                                $('#crop_button').trigger('click');
                            },
                        });

                        $('#btnCropSave').on('click', function () {
                            $("#btnCropSave").addLoader();

                            var formData = new FormData();
                            var croppedimage = cropper.getCroppedCanvas().toDataURL("image/png");
                            console.log($("#ProfilePicture").prop('files')[0]);
                            formData.append("UrlPhotoCropImg", croppedimage);
                            formData.append("Picture", $("#ProfilePicture").prop('files')[0]);

                            $.ajax({
                                url: `/academico/alumnos/informacion/${studentid}/actualizar-foto`,
                                type: "POST",
                                data: formData,
                                contentType: false,
                                processData: false
                            })
                                .done(function (e) {
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                })
                                .fail(function (e) {
                                    toastr.error("Error al actualizar la foto de perfil.", "Error.");
                                })
                                .always(function () {
                                    $("#modal-profile-viewer").modal("hide");
                                    $("#imageFile").prop('required', false);
                                    $("#student-picture").attr("src", croppedimage)
                                    $("#urlCropImg").val(croppedimage)
                                    $("#btnCropSave").removeLoader();
                                });
                        });
                    }
                    $("#ProfilePicture").next().html("Reemplazar imagen");
                    reader.readAsDataURL(this.files[0]);
                });
            }
        }
    }
    //var responsive = {
    //    init: function () {

    //        $(window).resize(function () {

    //            var width = $(window).width();
    //            if (width < 960) {
    //                pages.schedule.init();
    //            }
    //            if (width > 960) {
    //                pages.schedule.init();
    //            }
    //        });
    //    }
    //}

    return {
        init: function () {
            // responsive.init();
            profileViewer.init();
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