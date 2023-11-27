var indexrequ = function () {
    var datatable = {
        object: null,
        options: {
            ajax: {
                url: `/registrosacademicos/solicitudes/get`.proto().parseURL(),
                type: "GET",
                data: function (result) {
                    result.search = $("#search").val();
                    result.status = $("#status_select").val();
                }
            },
            columns: [
                {
                    data: "date",
                    title: "Fec. Solicitud",
                },
                {
                    data: "username",
                    title: "Cod. Estudiante",
                },
                {
                    data: "student",
                    title: "Nombre Completo"
                },
                {
                    data: "code",
                    title: "Código"
                },
                {
                    data: "type",
                    title: "Tipo"
                },
                {
                    data: "receiptCode",
                    title: "Recibo",
                    render: function (row) {
                        return row === null ? "Sin registrar" : row;
                    }
                },
                {
                    data: "status",
                    title: "Estado",
                    orderable: false
                },
                {
                    title: "Observaciones",
                    data: null,
                    render: function (data) {
                        return `<button data-id='${data.id}' class="btn btn-primary btn-sm m-btn m-btn--icon btn-observations" <span=""><i class="la la-eye"></i>Observaciones</button>`;
                    }
                },
                {
                    data: null,
                    title: "Opciones",
                    render: function (row) {
                        var tmp = "";
                        if (row.isTypeBachelor) {
                            tmp += `<a href='/registrosacademicos/informes-de-grados/creacion-informe-grados/${row.username}/${row.id}' class='btn btn-secondary btn-sm m-btn--icon btn-grade'>Generar informe</a>`;
                        }
                        else if (row.isTypeJobTitle) {
                            tmp += `<a href='/registrosacademicos/informes-de-titulos/creacion-informe-titulo/${row.username}/${row.id}' class='btn btn-secondary btn-sm m-btn--icon btn-grade'>Generar informe</a>`;
                        }
                        else {
                            if (!row.withProcedure) {
                                tmp += `<button data-id='${row.id}' type='button' class="btn btn-change-status btn-secondary m-btn m-btn--icon btn-sm m-btn--icon-only" title='Editar'><i class="la la-edit"></i></button>`;
                            }
                            if (row.needData && row.needToSaveFile) {
                                tmp += `<button data-id='${row.id}' type='button' class="btn btn-need-data btn-secondary m-btn m-btn--icon btn-sm m-btn--icon-only" title='Imprimir'><i class="la la-eye"></i></button>`;
                            }
                            else if (row.needData && !row.needToSaveFile) {
                                tmp += `<a target="_blank" href='/documentos/${row.urlFile}' class="btn btn-secondary m-btn m-btn--icon btn-sm m-btn--icon-only" title='Imprimir Automatico'><i class="la la-print"></i></button>`;
                            }
                            else {
                                tmp += "<button data-id='" + row.id + "' class='ml-1 btn btn-secondary btn-sm m-btn--icon btn-print'><i class='la la-print'></i></button>";
                            }
                        }
                        return tmp;
                    }
                }
            ]
        },
        events: {
            onChangeStatus: function () {
                $("#datatable").on("click", ".btn-change-status", function () {
                    var id = $(this).data("id");
                    modal.changeStatus.events.show(id);
                });

                $("#datatable").on("click", ".btn-grade", function () {
                    var username = $(this).data('identify');
                    var id = $(this).data("id");
                    window.location.href = `/registrosacademicos/informes-de-grados/creacion-informe-grados/${username}/${id}`.proto().parseURL();
                });
            },
            onObservations: function () {
                $("#datatable").on("click", ".btn-observations", function () {
                    var id = $(this).data("id");
                    $("#recordHistoryId").val(id);
                    datatableObservation.reload();
                    $("#observation_modal").modal("show");
                });
            },
            onPrint: function () {
                $("#datatable").on("click", ".btn-print", function () {
                    var recordHistoryId = $(this).data("id");
                    var url = `/registrosacademicos/solicitudes/imprimir/${recordHistoryId}`;
                    datatable.object.ajax.reload();
                    window.open(url, "_blank");
                });

                $("#datatable").on("click", ".btn-need-data", function () {
                    var id = $(this).data("id");
                    modal.needData.events.show(id);
                });
            },
            onReceipt: function () {
                $("#datatable").on("click", ".btn-receipt", function () {
                    var id = $(this).data("id");
                    $("#receipt_recordHistoryId").val(id);
                    $("#receipt_modal").modal("show");
                });
            },
            init: function () {
                this.onChangeStatus();
                this.onPrint();
                this.onObservations();
                this.onReceipt();
            }
        },
        reload: function () {
            datatable.object.ajax.reload();
        },
        init: function () {
            //datatable.options.order = [[0, "DESC"]];
            datatable.object = $("#datatable").DataTable(datatable.options);
            datatable.events.init();
        }
    };

    var datatableObservation = {
        object: null,
        options: getSimpleDataTableConfiguration({
            url: "/registrosacademicos/solicitudes/get-observaciones".proto().parseURL(),
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
            datatableObservation.object.ajax.reload();
        },
        init: function () {
            datatableObservation.object = $("#table_observations").DataTable(datatableObservation.options);
        }
    };

    var modal = {
        update: {
            object: $("#change-status"),
            show: function (status, userInternalProcedureId, recordHistoryId) {
                modal.update.object.find("[name='UserInternalProcedureId']").val(userInternalProcedureId);
                modal.update.object.find("[name='RecordHistoryId']").val(recordHistoryId);
                modal.update.object.find("[name='Status']").val(status).trigger("change");
                modal.update.object.find("[name='Observation']").val("");
            },
            form: {
                object: $("#update-form"),
                validate: function () {
                    modal.update.form.object.validate({
                        rules: {
                            Status: {
                                required: true
                            },
                            Observation: {
                                required: true,
                                maxlength: 200
                            }
                        },
                        submitHandler: function (formElement, e) {
                            e.preventDefault();
                            modal.update.form.submit(formElement);
                        }
                    });
                },
                submit: function (formElement) {
                    var formData = new FormData($(formElement)[0]);
                    modal.update.form.object.find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);

                    $.ajax({
                        url: "/registrosacademicos/solicitudes/actualizar-estado",
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    })
                        .done(function (data) {
                            modal.update.object.modal('hide');
                            swal({
                                type: "success",
                                title: "Completado",
                                text: "Estado Actualizado Satisfactoriamente.",
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
                            datatable.reload();
                            modal.update.form.object.find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);

                        });
                },
                init: function () {
                    this.validate();
                }
            },
            init: function () {
                modal.update.form.init();
            }
        },
        receipt: {
            object: $("#receipt_modal"),
            form: {
                object: $("#receipt_form"),
                validate: function () {
                    modal.receipt.form.object.validate({
                        rules: {
                            Receipt: {
                                required: true,
                                maxlength: 100
                            }
                        },
                        submitHandler: function (formElement, e) {
                            e.preventDefault();
                            modal.receipt.form.submit(formElement);
                        }
                    });
                },
                submit: function (formElement) {
                    var formData = new FormData($(formElement)[0]);
                    modal.receipt.form.object.find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);

                    $.ajax({
                        url: "/registrosacademicos/solicitudes/actualizar-recibo",
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    })
                        .done(function (data) {
                            modal.receipt.object.modal('hide');
                            swal({
                                type: "success",
                                title: "Completado",
                                text: "Recibo Actualizado Satisfactoriamente.",
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
                            datatable.reload();
                            modal.receipt.form.object.find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                        });
                },
                events: function () {
                    $("#receipt_modal").on('hidden.bs.modal', function () {
                        $("#receipt_form input[name=Receipt]").val("");

                    });
                },
                init: function () {
                    this.events();
                    this.validate();
                }
            },
            init: function () {
                this.form.init();
            }
        },
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
                                datatable.reload();
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
                        datatable.object.ajax.reload();
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
            modal.update.init();
            modal.receipt.init();
            modal.needData.init();
        }
    };

    var events = {
        onSearch: function () {
            $("#search").doneTyping(function () {
                datatable.reload();
            });
        },
        inputMaskReceipt: function () {
            $("[name='Receipt']").inputmask({
                "mask": "****-99999999"
            });
        },
        init: function () {
            events.onSearch();
            events.inputMaskReceipt();
        }
    };

    var select = {
        status: {
            init: function () {
                $(".m-select2").select2({
                    minimumResultsForSearch: -1
                })

                $("#status_select").on("change", function () {
                    datatable.reload();
                });
            }
        },
        init: function () {
            select.status.init();
        }
    };

    return {
        init: function () {
            events.init();
            datatable.init();
            datatableObservation.init();
            select.init();
            modal.init();
        }
    };
}();

$(function () {
    indexrequ.init();
});