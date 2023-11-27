var index = function () {

    var form = {
        request: {
            object: $("#request_form").validate({
                submitHandler: function (formElement, event) {
                    event.preventDefault();
                    var formData = new FormData(formElement);

                    if ($("#RecordHistory_JsonAcademicYears")[0]) {
                        formData.delete('RecordHistory.JsonAcademicYears');
                        formData.append('RecordHistory.JsonAcademicYears', JSON.stringify($("#RecordHistory_JsonAcademicYears").val()));
                    }

                    if ($("#Activity_JsonStudentSectionIds")[0]) {
                        formData.delete('Activity.JsonStudentSectionIds');
                        formData.append('Activity.JsonStudentSectionIds', JSON.stringify($("#Activity_JsonStudentSectionIds").val()));
                    }

                    mApp.block("#request_form", {
                        message: "Enviando datos..."
                    });

                    $.ajax({
                        url: `/tramites/iniciar-tramite`,
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    })
                        .done(function (e) {
                            $("#request_result_modal").find(".btn_request_result_accept").attr("href", `/tramites/detalles-solicitud/${e.id}`);

                            if (e.hasPreviousPayment) {
                                $("#request_result_modal").find(".btn_request_result_pdf").attr("href", `/tramites/esquela-pago-pdf/${e.id}`);
                                $("#request_result_modal").find(".btn_request_result_pdf").removeClass("d-none");
                            }
                            else {
                                $("#request_result_modal").find(".btn_request_result_pdf").addClass("d-none");
                            }

                            $("#request_result_modal").modal("show");
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
                            mApp.unblock("#request_form");
                        })
                }
            })
        },
    }

    var select = {
        //RecordHistory
        term: {
            load: function () {
                if ($("#RecordHistory_TermId")[0]) {
                    $.ajax({
                        url: `/periodos-por-estudiante/get/v2`,
                        type: "GET"
                    })
                        .done(function (e) {
                            $("#RecordHistory_TermId").select2({
                                data: e.items,
                                placeholder: "Seleccionar periodo"
                            });
                        })
                }
               
            },
            load_v2: function () {
                if ($("#RecordHistory_TermId_Report_Card")[0]) {
                    $.ajax({
                        url: `/periodos-por-estudiante/get/v3`,
                        type: "GET"
                    })
                        .done(function (e) {
                            $("#RecordHistory_TermId_Report_Card").select2({
                                data: e.items,
                                placeholder: "Seleccionar periodo"
                            });
                        })
                }
            },
            init: function () {
                select.term.load();
                select.term.load_v2();
            }
        },

        cycles: {
            load: function () {
                $.ajax({
                    url: `/get-ciclos-alumno`,
                    type: "GET"
                })
                    .done(function (data) {
                        var html = '';

                        $.each(data, function (i, v) {
                            html += `<option value="${v.id}">${v.text}</option>`;
                        });

                        $("#RecordHistory_JsonAcademicYears").html(html);

                        $("#RecordHistory_JsonAcademicYears").selectpicker({
                            actionsBox: true,
                            selectAllText: 'Marcar todos',
                            deselectAllText: 'Desmarcar todos',
                            noneSelectedText: 'Seleccionar',
                            //size: 10,
                        });
                        $('#RecordHistory_JsonAcademicYears').selectpicker("refresh");
                        $('#RecordHistory_JsonAcademicYears').trigger("change");

                        $('#RecordHistory_JsonAcademicYears').on("change", function () {

                            if ($(".certificate_studies_partial_total_amount").length) {
                                var count = $(this).val().length;
                                var conceptAmountText = $("#ConceptAmount").val();
                                var conceptAmount = parseFloat(conceptAmountText);

                                $(".certificate_studies_partial_total_amount").removeClass("d-none");
                                $(".certificate_studies_partial_total_amount").text(`MONTO TOTAL : S/ ${(conceptAmount * count).toFixed(2)}`)
                            }
                        })
                    })
            },
            init: function () {
                select.cycles.load();
            }
        },

        //Activity
        career : {
            load: function () {
                $.ajax({
                    url: `/carreras/get`,
                    type : "GET"
                })
                    .done(function (data) {
                        $("#Activity_CareerId").select2({
                            data: data.items,
                            placeholder: "Seleccionar escuela profesional..."
                        });

                        $("#Activity_CareerId").val(null).trigger("change");

                        $("#Activity_CareerId").on("change", function (e) {
                            var value = $(this).val();
                            select.curriculum.load(value);
                        })
                    })
            },
            init: function () {
                select.career.load();
            }
        },
        curriculum: {
            load: function (careerId) {
                $("#Activity_CurriculumId").empty();

                if (careerId == null) {
                    $("#Activity_CurriculumId").select2({
                        placeholder: "Seleccionar plan de estudio..."
                    });
                } else {
                    $.ajax({
                        url: `/planes-estudio/${careerId}/get`,
                        type : "GET"
                    })
                        .done(function (data) {
                            $("#Activity_CurriculumId").select2({
                                data: data.items,
                                placeholder: "Seleccionar plan de estudio..."
                            });
                        })
                }
            },
            init: function () {
                select.curriculum.load();
            }
        },
        studentSection: {
            load: function () {
                $.ajax({
                    url: `/cursos-matriculados-usuario?`.proto().parseURL()
                }).done(function (data) {
                    $(".courses_enrolled").select2({
                        data: data,
                        placeholder : "Seleccionar curso..."
                    });

                    $(".courses_enrolled").val(null).trigger("change");

                    //

                    var html = '';

                    $.each(data, function (i, v) {
                        html += `<option value="${v.id}">${v.text}</option>`;
                    });

                    $("#Activity_JsonStudentSectionIds").html(html);

                    $("#Activity_JsonStudentSectionIds").selectpicker({
                        actionsBox: true,
                        selectAllText: 'Marcar todos',
                        deselectAllText: 'Desmarcar todos',
                        noneSelectedText: 'Seleccionar',
                        //size: 10,
                    });
                    $('#Activity_JsonStudentSectionIds').selectpicker("refresh");
                    $('#Activity_JsonStudentSectionIds').trigger("change");

                    $('#Activity_JsonStudentSectionIds').on("change", function () {

                        if ($(".course_withdrawal_massive_total_amount").length) {
                            var count = $(this).val().length;
                            var conceptAmountText = $("#ConceptAmount").val();
                            var conceptAmount = parseFloat(conceptAmountText);

                            $(".course_withdrawal_massive_total_amount").removeClass("d-none");
                            $(".course_withdrawal_massive_total_amount").text(`MONTO TOTAL : S/ ${(conceptAmount * count).toFixed(2)}`)
                        }
                    })

                });
            },
            init: function () {
                select.studentSection.load();
            }
        },
        grade_recovery: {
            load: function () {
                if ($(".courses_grade_recovery")[0]) {
                    $.ajax({
                        url: `/get-cursos-disponibles-recuperacion-nota`
                    })
                        .done(function (data) {
                            $(".courses_grade_recovery").select2({
                                data: data,
                                placeholder: "Seleccionar curso..."
                            });

                            $(".courses_grade_recovery").val(null).trigger("change");

                        });
                }
            },
            init: function () {
                this.load();
            }
        },
        substitute_exam: {
            load: function () {
                if ($(".courses_subtitute_exam")[0]) {
                    $.ajax({
                        url: `/get-cursos-disponibles-examen-sustitutorio`
                    })
                        .done(function (data) {
                            $(".courses_subtitute_exam").select2({
                                data: data,
                                placeholder: "Seleccionar curso..."
                            });

                            $(".courses_subtitute_exam").val(null).trigger("change");

                        });
                }
            },
            init: function () {
                this.load();
            }
        },
        academicProgram: {
            load: function () {
                $.ajax({
                    url: `/programas-academico-por-usuario/get`,
                })
                    .done(function (e) {
                        $("#Activity_AcademicProgramId").select2({
                            data: e.items,
                            placeholder: "Seleccionar programa académico"
                        });

                        $("#Activity_AcademicProgramId").val(null).trigger("change");
                    })
            },
            init: function () {
                select.academicProgram.load();
            }
        },
        exoneratedCourse: {
            load: function () {
                if ($(".exonerated_course_select")[0]) {

                    $("#Activity_CourseId").attr("disabled", true);

                    $.ajax({
                        url: `/get-cursos-disponibles-tramite-exoneracion`
                    })
                        .done(function (e) {
                            $("#Activity_CourseId").select2({
                                data: e,
                                placeholder: "Seleccionar curso...",
                                disabled: false
                            }).val(null).trigger("change");
                        })

                }
            },
            init: function () {
                this.load();
            }
        },
        extraordinaryEvaluation: {
            load: function () {
                if ($(".extraordinary_evaluation_course_select")[0]) {
                    $("#Activity_CourseId").attr("disabled", true);

                    $.ajax({
                        url: `/get-cursos-disponibles-tramite-evaluacion-extraordinaria`
                    })
                        .done(function (e) {
                            $("#Activity_CourseId").select2({
                                data: e,
                                placeholder: "Seleccionar curso...",
                                disabled: false
                            }).val(null).trigger("change");
                        })
                }
            },
            init: function () {
                this.load();
            }
        },
        init: function () {
            select.cycles.init();
            select.grade_recovery.init();
            select.substitute_exam.init();
            select.career.init();
            select.curriculum.init();
            select.studentSection.init();
            select.academicProgram.init();
            select.exoneratedCourse.init();
            select.extraordinaryEvaluation.init();
            select.term.init();
        }
    }

    var modal = {
        userImage: {
            object: $("#modal_user_image_modal"),
            cropper : null,
            events: {
                onAdd: function () {
                    $("#input_user_image").on("change", function () {

                        var files = this.files;

                        if (files.length > 0) {
                            modal.userImage.object.modal("show");
                            setTimeout(() => {
                                var reader = new FileReader();
                                reader.readAsDataURL(this.files[0]);

                                reader.onload = function (event) {
                                    var image = $('#current-img-offi')[0];
                                    var container = $('.cropper-container cropper-bg').prevObject;
                                    $("#upload-offi div").remove();

                                    modal.userImage.cropper = new Cropper(image);

                                    modal.userImage.cropper.destroy();
                                    modal.userImage.cropper.reset();
                                    modal.userImage.cropper.clear();
                                    container.remove();

                                    $('.preview').css({
                                        width: '100%',
                                        overflow: 'hidden',
                                        height: 300,
                                        maxWidth: 300,
                                        maxHeight: 300
                                    });

                                    $("#current-img-offi").attr("src", event.target.result);
                                    $("#cropper-hide").attr("src", event.target.result);

                                    modal.userImage.cropper = new Cropper(image, {
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
                                    });

                                    modal.userImage.cropper.reset();
                                    modal.userImage.cropper.clear();

                                    $("#btn_edit_user_image_cropper").removeClass("d-none");
                                    mApp.unblock("#container_modal_user");
                                }
                            }, 800);
                        }
                    });
                },
                onSaveImgCropped: function () {
                    $("#btn_save_cropper").on("click", function () {
                        var $btn = $(this);
                        $btn.addLoader();

                        var croppedimage = modal.userImage.cropper.getCroppedCanvas().toDataURL("image/png");

                        $("#user_cropped_image_preview").attr("src", croppedimage);
                        $("#UserCroppedImage").val(croppedimage);
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        modal.userImage.object.modal("hide");
                        $btn.removeLoader();
                    })
                },
                onEditImageCropped: function () {
                    $("#btn_edit_user_image_cropper").on("click", function () {
                        modal.userImage.object.modal("show");
                        setTimeout(() => {
                            mApp.unblock("#container_modal_user");
                        }, 800);
                    })
                },
                onShown: function () {
                    modal.userImage.object.on('shown.bs.modal', function (e) {
                        mApp.block("#container_modal_user", {
                            message: "Cargando imagen..."
                        });
                    })
                },
                init: function () {
                    this.onAdd();
                    this.onEditImageCropped();
                    this.onSaveImgCropped();
                    this.onShown();
                }
            },
            init: function () {
                modal.userImage.events.init();
            }
        },
        paymentReceipt: {
            object: $("#modal_payment_receipt_detail"),
            form: {
                object: $("#form_payment_receipt_detail").validate({
                    submitHandler: function (formElement, event) {
                        event.preventDefault();
                        var formData = new FormData(formElement);

                        $("#form_payment_receipt_detail").find(":input").attr("disabled", true);

                        mApp.block("#form_payment_receipt_detail", {
                            message: "Validando datos..."
                        });

                        $.ajax({
                            url: `/pagos/validar-pago`,
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        })
                            .done(function (e) {
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "Recibo validado con éxito.",
                                    confirmButtonText: "Aceptar",
                                    allowOutsideClick: false
                                })
                                events.setPayment(e);
                                modal.paymentReceipt.object.modal("hide");
                            })
                            .fail(function (e) {
                                events.setPayment();
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Aceptar",
                                    text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                });
                            })
                            .always(function () {
                                $("#form_payment_receipt_detail").find(":input").attr("disabled", false);
                                mApp.unblock("#form_payment_receipt_detail");
                            })
                    }
                })
            },
            init: function () {

            }
        },
        cashier_payment: {
            object: $("#modal_cashier_payments"),
            events: {
                onShow: function () {
                    modal.cashier_payment.object.on('show.bs.modal', function (e) {
                        datatable.cashier_payments.reload();
                    })
                },
                init: function () {
                    modal.cashier_payment.events.onShow();
                }
            },
            init: function () {
                modal.cashier_payment.events.init();
            }
        },
        init: function () {
            modal.userImage.init();
            modal.paymentReceipt.init();
            modal.cashier_payment.init();
        }
    }

    var upload = {
        init: function () {
            var cropper = null;
            $('#Image').on('change', function () {

                var reader = new FileReader();
                reader.readAsDataURL(this.files[0]);



                reader.onload = function (event) {

                    var image = $('#img-offi')[0];
                    var container = $('.cropper-container cropper-bg').prevObject;
                    $("#upload-offi div").remove();

                    cropper = new Cropper(image);


                    cropper.destroy();
                    cropper.reset();
                    cropper.clear();
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
                    cropper.reset();
                    cropper.clear();

                    $('#btnCropSave').on('click', function () {

                        $("#btnCropSave").addLoader();

                        var croppedimage = cropper.getCroppedCanvas().toDataURL("image/png");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        $("#image-modal").modal("hide");

                        $("#imageFile").prop('required', false);
                        $("#img-offi-preview").attr("src", croppedimage)
                        $("#urlCropImg").val(croppedimage)

                        $("#btnCropSave").removeLoader();


                        // window.location.href = `/tramites/usuarios?tab=2`.proto().parseURL();

                    });

                }
                $("#Image").next().html("Reemplazar imagen");
            });

        }
    }

    var datepicker = {
        init: function () {
            $(".input-datepicker").datepicker();
        }
    }

    var datatable = {
        cashier_payments: {
            object: null,
            options: {
                ajax: {
                    url: `/pagos/get-pagos-no-usados/datatable`,
                    type: "GET",
                    data: function (data) {
                        data.conceptId = $("#ConceptId").val()
                    }
                },
                columns: [
                    {
                        data: "paymentDate",
                        title: "Fec. Pago",
                        orderable: false
                    },
                    {
                        data: "description",
                        title: "Descripción",
                        orderable: false
                    },
                    {
                        data: "total",
                        title: "Monto Total",
                        orderable: false
                    },
                    {
                        data: null,
                        title: "Opciones",
                        orderable: false,
                        render: function (data) {
                            var tpm = "";
                            tpm += `<button type='button' data-object='${data.proto().encode()}' class="btn btn-select-cashier-payment btn-primary btn-sm m-btn  m-btn m-btn--icon"><span><i class="la la-check"></i><span>Seleccionar</span></span></button>`;
                            return tpm;
                        }
                    }
                ]
            },
            events: {
                onSelect: function () {
                    $("#table_cashier_payments").on("click", ".btn-select-cashier-payment", function () {
                        var data = $(this).data("object").proto().decode();
                        events.setPayment(data);
                        modal.cashier_payment.object.modal("hide");
                        swal({
                            type: "success",
                            title: "Completado",
                            text: "Pago seleccionado con éxito.",
                            confirmButtonText: "Aceptar",
                            allowOutsideClick: false
                        });
                    });
                },
                init: function () {
                    datatable.cashier_payments.events.onSelect();
                }
            },
            reload: function () {
                datatable.cashier_payments.object.ajax.reload();
            },
            init: function () {
                datatable.cashier_payments.object = $("#table_cashier_payments").DataTable(datatable.cashier_payments.options);
                datatable.cashier_payments.events.init();
            }
        },
        init: function () {
            datatable.cashier_payments.init();
        }
    }

    var events = {
        setPayment: function (data) {
            var $input = $("#Payment_OperationCode");

            if (data != null) {
                $input.css("border", "2px solid #34bfa3");
                $input.val(data.operationCodeB);
                $("#Payment_Id").val(data.id);
            } else {
                $input.css("border", "");
                $input.val("");
                $("#Payment_Id").val("");
            }
        }
    }

    return {
        init: function () {
            select.init();
            modal.init();
            upload.init();
            datepicker.init();
            datatable.init();
        }
    }
}();

$(() => {
    index.init();
});