var InitApp = function () {

    var CurrentRecordHistoryId = null;

    var datatable = {
        history: {
            object: null,
            optionsIntegrated: {
                serverSide: false,
                ajax: {
                    url: "/registrosacademicos/solicitudes/get-historial",
                    type: "GET",
                    data: function (data) {
                        delete data.columns;
                        data.studentId = $("#student_select").val();
                        data.type = $("#type_select").val();
                        data.academicrecordId = $("#academic_record_select").val();
                    }
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
                        title: "Tipo de Constancia",
                        data: "type"
                    },
                    {
                        title: "Trámite Asociado",
                        data: "userProcedure"
                    },
                    {
                        title: "Estado",
                        data: "status"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        render: function (row) {
                            var tpm = "";
                            if (row.isTypeBachelor) {
                                tpm += `<a href='/registrosacademicos/informes-de-grados/creacion-informe-grados/${row.username}/${row.id}' class='btn btn-secondary btn-sm m-btn--icon btn-grade'>Generar informe</a>`;
                            }
                            else if (row.isTypeJobTitle) {
                                tpm += `<a href='/registrosacademicos/informes-de-titulos/creacion-informe-titulo/${row.username}/${row.id}' class='btn btn-secondary btn-sm m-btn--icon btn-grade'>Generar informe</a>`;
                            }
                            else {
                                if (!row.withProcedure) {
                                    tpm += `<button data-id='${row.id}' type='button' class="btn btn-change-status btn-secondary m-btn m-btn--icon btn-sm m-btn--icon-only" title='Editar'><i class="la la-edit"></i></button>`;
                                }

                                tpm += "<button data-id='" + row.id + "' class='ml-1 btn btn-secondary btn-sm m-btn--icon btn-print'><i class='la la-print'></i></button>";
                            }
                            return tpm;
                        }
                    }
                ]
            },
            options: {
                serverSide: false,
                ajax: {
                    url: "/registrosacademicos/solicitudes/get-historial",
                    type: "GET",
                    data: function (data) {
                        delete data.columns;
                        data.studentId = $("#student_select").val();
                        data.type = $("#type_select").val();
                        data.academicrecordId = $("#academic_record_select").val();
                    }
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
                        title: "Tipo de Constancia",
                        data: "type"
                    },
                    {
                        title: "Estado",
                        data: "status"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        render: function (row) {
                            var tpm = "";
                            if (row.isTypeBachelor) {
                                tpm += `<a href='/registrosacademicos/informes-de-grados/creacion-informe-grados/${row.username}/${row.id}' class='btn btn-secondary btn-sm m-btn--icon btn-grade'>Generar informe</a>`;
                            }
                            else if (row.isTypeJobTitle) {
                                tpm += `<a href='/registrosacademicos/informes-de-titulos/creacion-informe-titulo/${row.username}/${row.id}' class='btn btn-secondary btn-sm m-btn--icon btn-grade'>Generar informe</a>`;
                            }
                            else {
                                if (!row.withProcedure) {
                                    tpm += `<button data-id='${row.id}' type='button' class="btn btn-change-status btn-secondary m-btn m-btn--icon btn-sm m-btn--icon-only" title='Editar'><i class="la la-edit"></i></button>`;
                                }

                                tpm += "<button data-id='" + row.id + "' class='ml-1 btn btn-secondary btn-sm m-btn--icon btn-print'><i class='la la-print'></i></button>";

                            }
                            return tpm;
                        }
                    }
                ]
            },
            reload: function () {
                if (this.object !== null) {
                    this.object.ajax.reload();
                }
                else {
                    if (IsIntegrated) {
                        this.object = $("#data-table").DataTable(this.optionsIntegrated);
                    } else {
                        this.object = $("#data-table").DataTable(this.options);
                    }
                }
            },
            events: {
                onChangeStatus: function () {
                    $("#data-table").on("click", ".btn-change-status", function () {
                        var id = $(this).data("id");
                        modal.changeStatus.events.show(id);
                    });

                    $("#data-table").on("click", ".btn-need-data", function () {
                        var id = $(this).data("id");
                        modal.needData.events.show(id);
                    });
                },
                onViewObservations: function () {
                    $("#data-table").on("click", ".btn-observations", function () {
                        var id = $(this).data("id");
                        modal.viewObservation.events.show(id);
                    });
                },
                onPrint: function () {
                    $("#data-table").on("click", ".btn-print", function () {
                        var $btn = $(this);
                        var recordHistoryId = $(this).data("id");
                        var url = `/registrosacademicos/solicitudes/imprimir/${recordHistoryId}`;

                        $btn.addLoader();
                        $.fileDownload(url, {
                            httpMethod: 'GET',
                            successCallback: function () {
                                $btn.removeLoader();
                                toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                datatable.reports.reload();
                            },
                            failCallback: function (responseHtml, url) {
                                $btn.removeLoader();
                                responseHtml = responseHtml.replace(`<pre style="word-wrap: break-word; white-space: pre-wrap;">`, "");
                                responseHtml = responseHtml.replace(`</pre>`, "");
                                toastr.error(responseHtml, "Error");
                            }
                        });
                    });

                    $("#data-table").on("click", ".btn-need-data", function () {
                        var id = $(this).data("id");
                        modal.needData.events.show(id);
                    });
                },
                onReceipt: function () {
                    $("#data-table").on("click", ".btn-receipt", function () {
                        var id = $(this).data("id");
                        $("#receipt_recordHistoryId").val(id);
                        $("#receipt_modal").modal("show");
                    });
                },
                init: function () {
                    this.onChangeStatus();
                    this.onViewObservations();
                    this.onPrint();
                }
            },
            init: function () {
                this.events.init();
            }
        },
        observations: {
            object: null,
            options: {
                ajax: {
                    url: `/registrosacademicos/solicitudes/get-observaciones`,
                    type: "GET",
                    data: function (data) {
                        data.recordHistoryId = CurrentRecordHistoryId;
                    }
                },
                columns: [
                    {
                        data: "date",
                        title: "Fecha",
                        orderable: false
                    },
                    {
                        data: "status",
                        title: "Estado",
                        orderable: false
                    },
                    {
                        data: "observation",
                        title: "Observaciones",
                        orderable: false
                    }
                ]
            },
            reload: function () {
                if (datatable.observations.object === null) {
                    datatable.observations.object = $("#table_observations").DataTable(datatable.observations.options);
                } else {
                    datatable.observations.object.ajax.reload();
                }
            }
        },
        init: function () {
            this.history.init();
        }
    };

    var select = {
        student_select: {
            load: function () {
                $("#student_select").select2({
                    placeholder: "Buscar...",
                    ajax: {
                        delay: 500,
                        url: "/alumnosv2/get".proto().parseURL(),
                        data: function (params) {
                            return {
                                searchValue: params.term,
                                page: params.page || 1
                            };
                        },
                    },
                    minimumInputLength: 3
                });
            },
            onChange: function () {
                $("#student_select").on("change", function () {
                    var id = $(this).val();
                    select.userProcedure.load(id);
                    select.term.load(id);
                });
            },
            init: function () {
                this.load();
                this.onChange();
            }
        },
        userProcedure: {
            load: function (studentId) {
                $.ajax({
                    url: `/registrosacademicos/solicitudes/tramites-asociados?studentId=${studentId}`,
                    type: "GET"
                }).done(function (e) {
                    $("#user_procedures").empty();

                    $("#user_procedures").select2({
                        placeholder: "Seleccionar trámite",
                        data: e,
                        disabled: false,
                        allowClear: true
                    });

                    $("#user_procedures").val(null).trigger("change");
                });
            },
            init: function () {
                $("#user_procedures").select2({
                    placeholder: "Seleccionar trámite",
                    disabled: true,
                });
            }
        },
        type: {
            load: function () {
                $.ajax({
                    url: "/tipos-constancias/get",
                    type: "GET"
                })
                    .done(function (e) {
                        $("#type_select").select2({
                            placeholder: "Seleccionar tipo de constancia",
                            data: e.items
                        });
                    });
            },
            onChange: function () {
                $("#type_select").on("change", function () {
                    if ($(this).val() == 18 && ($("#student_select").val() == "" || $("#student_select").val() == null)) {
                        $("#row_certificatestudies_partial").addClass("d-none");
                        $("#div_select_term").removeClass("d-none");

                    } else if ($(this).val() == 18 && $("#student_select").val() != "" && $("#student_select").val() != null) {
                        $("#row_certificatestudies_partial").addClass("d-none");
                        $("#div_select_term").removeClass("d-none");
                    }
                    else if ($(this).val() == 11 && $("#student_select").val() != "" && $("#student_select").val() != null) {
                        $("#row_certificatestudies_partial").addClass("d-none");
                        $("#div_select_term").removeClass("d-none");
                    }
                    else if ($(this).val() == 13 && $("#student_select").val() != "" && $("#student_select").val() != null) {
                        $("#row_certificatestudies_partial").addClass("d-none");
                        $("#div_select_term").removeClass("d-none");
                    }
                    else if ($(this).val() == 30 && $("#student_select").val() != "" && $("#student_select").val() != null) {
                        $("#row_certificatestudies_partial").addClass("d-none");
                        $("#div_select_term").removeClass("d-none");
                    }
                    else if ($(this).val() == 31 && $("#student_select").val() != "" && $("#student_select").val() != null) {
                        $("#row_certificatestudies_partial").addClass("d-none");
                        $("#div_select_term").removeClass("d-none");
                    }
                    else if ($(this).val() == 5 && $("#student_select").val() != "" && $("#student_select").val() != null) {
                        $("#row_certificatestudies_partial").addClass("d-none");
                        $("#div_select_term").removeClass("d-none");
                    }
                    else if ($(this).val() == 33 && $("#student_select").val() != "" && $("#student_select").val() != null) {
                        $("#row_certificatestudies_partial").addClass("d-none");
                        $("#div_select_term").removeClass("d-none");
                    }
                    else if ($(this).val() == 25) {
                        $("#row_certificatestudies_partial").removeClass("d-none");
                        $("#div_select_term").addClass("d-none");
                        select.certificatePartial.range.load();
                    }
                    else {
                        $("#row_certificatestudies_partial").addClass("d-none");
                        $("#div_select_term").addClass("d-none");
                    }
                });
            },
            init: function () {
                this.load();
                this.onChange();
            }
        },
        term: {
            load: function (studentId) {
                $.ajax({
                    url: `/periodos-por-estudiante/get/v3?studentId=${studentId}`,
                    type: "GET"
                })
                    .done(function (e) {
                        $("#term_select").empty();
                        $("#term_select").select2({
                            placeholder: "Seleccionar tipo de constancia",
                            data: e.items,
                            disabled: false
                        });
                    });
            },
            init: function () {
                $("#term_select").select2({
                    placeholder: "Seleccionar periodo",
                    disabled: true
                });
            }
        },
        status: {
            init: function () {
                $("[name='Status']").select2({
                    minimumResultsForSearch: -1
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

                    $("#academicyear_partial_select").empty();

                    $.ajax({
                        url: `/ventanilla/solicitudes/get-ciclos-alumno?studentId=${student}`
                    })
                        .done(function (data) {

                            var html = '';

                            $.each(data, function (i, v) {
                                html += `<option value="${v.id}">${v.text}</option>`;
                            });

                            $("#academicyear_partial_select").html(html);

                            $("#academicyear_partial_select").selectpicker({
                                actionsBox: true,
                                selectAllText: 'Marcar todos',
                                deselectAllText: 'Desmarcar todos',
                                noneSelectedText: 'Seleccionar',
                                //size: 10,
                            });
                            $('#academicyear_partial_select').selectpicker("refresh");
                            $('#academicyear_partial_select').trigger("change");
                        });
                },
                init: function () {
                    //$("#academicyear_partial_select").select2({
                    //    placeholder: "Seleccionar",
                    //});
                }
            },
            init: function () {
                this.rangeType.init();
                this.range.init();
            }
        },
        init: function () {
            select.student_select.init();
            select.type.init();
            select.userProcedure.init();
            select.term.init();
            select.status.init();
            select.certificatePartial.init();
        }
    };

    var events = {
        onHistory: function () {
            $("#btn-history").click(function () {
                if ($("#student_select").val() === "" || $("#student_select").val() === null) {
                    toastr.error("Es necesario seleccionar al estudiante", _app.constants.toastr.title.error);
                    return false;
                }

                datatable.history.reload();
            });
        },
        onRegister: function () {
            $("#btn-create").click(function () {
                var type = $("#type_select").val();
                var student = $("#student_select").val();
                var term_select = $("#term_select").val();
                var user_Procedure = $("#user_procedures").val();


                var rangeType = $("#range_type_select").val();
                var academicYearPartials = JSON.stringify($("#academicyear_partial_select").val());

                if (student === "" || student === null) {
                    toastr.error("Debe completar todos los campos", _app.constants.toastr.title.error);
                    return false;
                }

                if (type === "" || type === null) {
                    toastr.error("Debe completar todos los campos", _app.constants.toastr.title.error);
                    return false;
                }

                if (type == 18 && (term_select === "" || term_select === null)) {
                    toastr.error("Debe completar todos los campos", _app.constants.toastr.title.error);
                    return false;
                }

                if (type == 5 && (term_select === "" || term_select === null)) {
                    toastr.error("Debe completar todos los campos", _app.constants.toastr.title.error);
                    return false;
                }

                var formData = new FormData();
                formData.append("StudentId", student);
                formData.append("RecordType", type);
                formData.append("UserProcedureId", user_Procedure);
                formData.append("TermId", term_select);
                formData.append("RangeType", rangeType);

                formData.append("JsonAcademicYearPartials", academicYearPartials);

                swal({
                    title: "Generar Documento",
                    text: "¿Seguro que desea generar el documento?",
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
                                url: "/registrosacademicos/solicitudes/generar",
                                type: "POST",
                                data: formData,
                                processData: false,
                                contentType: false
                            })
                                .done(function () {
                                    datatable.history.reload();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El documento ha sido generado con éxito.",
                                        confirmButtonText: "Excelente"
                                    });
                                })
                                .fail(function () {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "Error al generar el documento"
                                    });
                                });
                        });
                    }
                });
            });
        },
        init: function () {
            this.onHistory();
            this.onRegister();
        }
    };

    var modal = {
        changeStatus: {
            object: $("#change_status_modal"),
            form: {
                object: $("#change_status_form").validate({
                    submitHandler: function (formElement, e) {
                        var $btn = $("#change_status_form").find(".btn-primary");
                        $btn.addLoader();
                        var formData = new FormData($(formElement)[0]);
                        $.ajax({
                            url: `/registrosacademicos/solicitudes/actualizar-estado`,
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        })
                            .done(function (e) {
                                datatable.history.reload();
                                modal.changeStatus.object.modal("hide");
                                swal({
                                    type: "success",
                                    title: "Hecho!",
                                    text: "Registro actualizado con éxito.",
                                    allowOutsideClick: false,
                                    confirmButtonText: "Aceptar"
                                });
                            })
                            .fail(function () {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Aceptar",
                                    text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                });
                            })
                            .always(function () {
                                $btn.removeLoader();
                            });
                    }
                })
            },
            events: {
                show: function (recordHistoryId) {
                    modal.changeStatus.object.find("[name='RecordHistoryId']").val(recordHistoryId);
                    modal.changeStatus.object.find(":input").attr("disabled");
                    modal.changeStatus.object.modal("show");

                    $.ajax({
                        url: `/registrosacademicos/solicitudes/get-record?recordHistoryId=${recordHistoryId}`
                    })
                        .done(function (e) {
                            modal.changeStatus.object.find("[name='Status']").val(e.status).trigger("change");
                        })
                        .fail(function (e) {
                            toastr.error("No se pudo obtener los datos del registro seleccionado", "Error");
                        });
                },
                onHidden: function () {
                    modal.changeStatus.object.on('hidden.bs.modal', function (e) {
                        modal.changeStatus.form.object.resetForm();
                    });
                },
                init: function () {
                    this.onHidden();
                }
            },
            init: function () {
                this.events.init();
            }
        },
        viewObservation: {
            object: $("#observation_modal"),
            events: {
                show: function (id) {
                    CurrentRecordHistoryId = id;
                    modal.viewObservation.object.modal("show");
                    datatable.observations.reload();
                }
            }
        },
        needData: {
            object: $("#edit_datainfo_modal"),
            events: {
                show: function (academicRecordId) {
                    modal.needData.object.modal("show");
                    modal.needData.object.find("[name='RecordHistoryId']").val(academicRecordId);
                    mApp.block("#pdf_preview", {
                        message: "Cargando previsualización..."
                    });

                    $.ajax({
                        url: `/registrosacademicos/solicitudes/imprimir/${academicRecordId}?preview=true`,
                        type: "GET"
                    })
                        .done(function (e) {
                            $("#preview_pdf_frame").attr("src", `data:application/pdf;base64,${e}`);
                            mApp.unblock("#pdf_preview");
                        });
                },
                onSelect: function () {
                    modal.needData.object.find(".btn-automatic").on("click", function () {
                        var recordHistoryId = modal.needData.object.find("[name='RecordHistoryId']").val();
                        var withBackground = $("#with_background").is(":checked");
                        var url = `/registrosacademicos/solicitudes/imprimir/${recordHistoryId}?withBackground=${withBackground}`;
                        datatable.history.object.ajax.reload();
                        modal.needData.object.modal("hide");
                        window.open(url, "_blank");
                    });

                    modal.needData.object.find(".btn-manual").on("click", function () {
                        var recordHistoryId = modal.needData.object.find("[name='RecordHistoryId']").val();
                        var withBackground = $("#with_background").is(":checked");
                        var url = `/registrosacademicos/solicitudes/guardar-informacion/${recordHistoryId}`;
                        window.location.href = url;
                    });
                },
                init: function () {
                    this.onSelect();
                }
            },
            init: function () {
                this.events.init();
            }
        },
        init: function () {
            modal.changeStatus.init();
            modal.needData.init();
        }
    };

    return {
        init: function () {
            select.init();
            events.init();
            datatable.init();
            modal.init();
        }
    };
}();

$(function () {
    InitApp.init();
}); 