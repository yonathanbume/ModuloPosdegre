var InitApp = function () {
    var select = {
        init: function () {
            select.types.init();
            select.students.init();
            select.academic_record.init();
            $("#term_select").select2();
            select.certificatePartial.init();
        },
        academic_record: {
            init: function () {
                $.ajax({
                    url: ("/registros_academicos/get").proto().parseURL()
                })
                    .done(function (data) {
                        $("#academic_record_select").select2({
                            placeholder: "Seleccionar usuario",
                            data: data
                        });
                    });
            }
        },
        types: {
            init: function () {
                $.ajax({
                    url: ("/ventanilla/solicitudes/tipos/get").proto().parseURL()
                }).done(function (data) {

                    $("#type_select").select2({
                        data: data.items
                    }).on("change", function () {
                        if ($(this).val() == 18 && ($("#student_select").val() == "" || $("#student_select").val() == null)) {
                            $("#row_certificatestudies_partial").addClass("d-none");
                            $("#term_select").select2({
                                disabled: true
                            });
                            $(".term-select").show();
                        } else if ($(this).val() == 18 && $("#student_select").val() != "" && $("#student_select").val() != null) {
                            $("#row_certificatestudies_partial").addClass("d-none");
                            $("#term_select").select2({
                                disabled: false
                            });
                            $(".term-select").show();
                        }
                        else if ($(this).val() == 25) {
                            $("#row_certificatestudies_partial").removeClass("d-none");
                            select.certificatePartial.range.load();
                        }
                        else {
                            $("#row_certificatestudies_partial").addClass("d-none");
                            $("#term_select").select2('data', null);
                            $(".term-select").hide();
                        }
                    });
                })
            }
        },
        students: {
            init: function () {
                $("#student_select").select2({
                    width: "100%",
                    placeholder: "Buscar...",
                    ajax: {
                        url: "/ventanilla/solicitudes/buscar".proto().parseURL(),
                        dataType: "json",
                        data: function (params) {
                            return {
                                term: params.term,
                                page: params.page
                            };
                        },
                        processResults: function (data, params) {
                            return {
                                results: data.items
                            };
                        },
                        cache: true
                    },
                    escapeMarkup: function (markup) {
                        return markup;
                    },
                    minimumInputLength: 3
                }).on("change", function () {
                    if ($("#type_select").val() == 18) {
                        $("#term_select").empty();
                        studentId = $(this).val();
                        $.ajax({
                            url: (`/periodos-por-estudiante/get?studentId=${studentId}`).proto().parseURL()
                        }).done(function (data) {
                            $("#term_select").select2({
                                data: data.items, disabled: false
                            });
                        });
                    }

                    if ($("#type_select").val() == 25) {
                        select.certificatePartial.range.load();
                    }

                });
            }
        },
        certificatePartial: {
            rangeType: {
                load: function () {
                    $("#range_type_select").select2({
                        placeholder: "Seleccionar tipo",
                        minimumResultsForSearch: -1
                    })
                        .on("change", function () {
                            select.certificatePartial.range.load();
                        });
                },
                init: function () {
                    this.load();
                }
            },
            range: {
                load: function () {
                    var type = $("#range_type_select").val();
                    var student = $("#student_select").val();

                    $("#range_start_select").empty();
                    $("#range_end_select").empty();

                    if (type == 1) {//por ciclo
                        $.ajax({
                            url: `/ventanilla/solicitudes/get-ciclos-alumno?studentId=${student}`
                        })
                            .done(function (e) {
                                $("#range_start_select").select2({
                                    placeholder: "Seleccionar",
                                    data: e
                                });

                                $("#range_end_select").select2({
                                    placeholder: "Seleccionar",
                                    data: e
                                });
                            });
                    } else {
                        $.ajax({
                            url: `/ventanilla/solicitudes/get-periodos-alumno?studentId=${student}`
                        })
                            .done(function (e) {
                                $("#range_start_select").select2({
                                    placeholder: "Seleccionar",
                                    data: e
                                });

                                $("#range_end_select").select2({
                                    placeholder: "Seleccionar",
                                    data: e
                                });
                            });
                    }
                },
                init: function () {
                    $("#range_start_select").select2({
                        placeholder: "Seleccionar",
                    });

                    $("#range_end_select").select2({
                        placeholder: "Seleccionar",
                    });
                }
            },
            init: function () {
                this.rangeType.init();
                this.range.init();
            }
        }
    };

    var datatable = {
        history: {
            object: null,
            options: getSimpleDataTableConfiguration({
                serverSide: false,
                url: "/ventanilla/solicitudes/historial/get".proto().parseURL(),
                data: function (data) {
                    delete data.columns;
                    data.studentId = $("#student_select").val();
                    data.type = $("#type_select").val();
                    data.academicrecordId = $("#academic_record_select").val();
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Fecha emisión",
                        data: "date"
                    },
                    {
                        title: "Nro. constancia",
                        data: "number"
                    },
                    {
                        title: "Personal Asignado",
                        data: "academicrecord"
                    },
                    {
                        title: "Estado",
                        data: "status"
                    },
                    {
                        title: "Observaciones",
                        data: null,
                        render: function (data) {
                            return `<button data-id='${data.id}' class="btn btn-primary btn-sm m-btn m-btn--icon btn-observations" <span=""><i class="la la-eye"></i>Observaciones</button>`;
                        }
                    },
                    //{
                    //    title: "Opciones",
                    //    data: null,
                    //    render: function (data) {
                    //        var tmp = "";
                    //        if (!data.isFinished) {
                    //            tmp += `<button data-id='${data.userInternalId}' class="btn btn-primary btn-sm m-btn m-btn--icon btn-finish" <span=""><i class="la la-check"></i>Finalizar</button>`;
                    //        } else {
                    //            tmp += `-`;
                    //        }
                    //        return tmp;
                    //    }
                    //}
                ]
            }),
            events: {
                onObservations: function () {
                    $("#data-table").on("click", ".btn-observations", function () {
                        var id = $(this).data("id");
                        $("#recordHistoryId").val(id);
                        datatable.observation.reload();
                        $("#observation_modal").modal("show");
                    });
                },
                onFinish: function () {
                    $("#data-table").on("click", ".btn-finish", function () {
                        var id = $(this).data("id");
                        swal({
                            title: "Proceso de Solicitud",
                            text: "¿Seguro que desea finalizar la solicitud?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise(() => {
                                    $.ajax({
                                        url: "/ventanilla/solicitudes/finalizar?id=" + id,
                                        type: "GET"
                                    })
                                        .done(function () {
                                            datatable.history.load();
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: "La solicitud ha sido finalizada con éxito",
                                                confirmButtonText: "Excelente"
                                            });
                                        })
                                        .fail(function () {
                                            swal({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                                confirmButtonText: "Entendido",
                                                text: "Error al enviar la solicitud"
                                            });
                                        });
                                });
                            }
                        });
                    });
                },
                init: function () {
                    this.onObservations();
                    this.onFinish();
                }
            },
            load: function () {
                if (this.object !== undefined && this.object !== null)
                    this.object.ajax.reload();
                else
                    this.object = $("#data-table").DataTable(this.options);
            },
            init: function () {
                datatable.history.events.init();
            }
        },
        observation: {
            object: null,
            options: getSimpleDataTableConfiguration({
                url: "/ventanilla/solicitudes/get-observaciones".proto().parseURL(),
                data: function (data) {
                    data.recordHistoryId = $("#recordHistoryId").val();
                },
                columns: [
                    {
                        title: "Fecha",
                        data: "date"
                    },
                    {
                        title: "Observación",
                        data: "observation"
                    },
                    {
                        title: "Generado con Estado",
                        data: "status"
                    }
                ]
            }),
            reload: function () {
                datatable.observation.object.ajax.reload();
            },
            init: function () {
                datatable.observation.object = $("#table_observations").DataTable(datatable.observation.options);
            }
        },
        init: function () {
            datatable.history.init();
            datatable.observation.init();
        }
    };

    var events = {
        onHistoric: function () {
            $("#btn-history").click(function () {
                if ($("#student_select").val() == "" || $("#student_select").val() == null) {
                    toastr.error("Es necesario seleccionar al estudiante", _app.constants.toastr.title.error);
                    return false;
                }
                datatable.history.load();
            });
        },
        onRegisterRequest: function () {
            $("#btn-create").click(function () {
                var type = $("#type_select").val();
                var student = $("#student_select").val();
                var academic_record = $("#academic_record_select").val();
                var term_select = $("#term_select").val();

                var rangeType = $("#range_type_select").val();

                var start = $("#range_start_select").val();
                var end = $("#range_end_select").val();

                if (student == "" || student == null) {
                    toastr.error("Debe completar todos los campos", _app.constants.toastr.title.error);
                    return false;
                }
                if (type == 18 && (type == "" || type == null)) {
                    toastr.error("Debe completar todos los campos", _app.constants.toastr.title.error);
                    return false;
                }

                if (academic_record === "" || academic_record === null) {
                    toastr.error("Debe completar todos los campos", _app.constants.toastr.title.error);
                    return false;
                }


                var formData = new FormData();
                formData.append("StudentId", student);
                formData.append("RecordType", type);
                formData.append("AcademicRecordStaffId", academic_record);
                formData.append("TermId", term_select);
                formData.append("RangeType", rangeType);

                formData.append("StartAcademicYear", start);
                formData.append("EndAcademicYear", end);
                formData.append("StartTerm", start);
                formData.append("EndTerm", end);


                swal({
                    title: "Proceso de Solicitud",
                    text: "¿Seguro que desea enviar la solicitud?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise(() => {
                            $.ajax({
                                url: "/ventanilla/solicitudes/generar",
                                type: "POST",
                                data: formData,
                                processData: false,
                                contentType: false
                            })
                                .done(function () {
                                    datatable.history.load();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "La solicitud ha sido enviado con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                })
                                .fail(function (e) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: e.responseText
                                    });
                                });
                        });
                    }
                });
            });
        },
        init: function () {
            events.onHistoric();
            events.onRegisterRequest();
        }
    };

    return {
        init: function () {
            select.init();
            events.init();
            datatable.init();
        }
    };
}();

$(function () {
    InitApp.init();
}); 