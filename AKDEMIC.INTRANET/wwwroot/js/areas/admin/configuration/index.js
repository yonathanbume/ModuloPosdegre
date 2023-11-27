var InitApp = function () {

    var selects = {
        extraordinaryEvaluationTypes: function () {
            $("#ExtraordinaryEvaluationTypesEnabled").select2();
            var json = $("#ExtraordinaryEvaluationTypesEnabled_Values").val();
            var values = JSON.parse($("#ExtraordinaryEvaluationTypesEnabled_Values").val()).map((x) => parseInt(x));
            $("#ExtraordinaryEvaluationTypesEnabled").val(values).trigger("change");
        },
        substitoryEvaluation: function () {
            $(".select2-substitutoryExamEvaluationType").select2();
        },
        concepSelect: function () {
            $.ajax({
                url: ("/conceptos/get").proto().parseURL()
            }).done(function (result) {
                $(".select2-concepts").select2({
                    data: result
                });
                if ($("#CourseWithdrawalConceptId").val() === _app.constants.guid.empty) {
                    $("#CourseWithdrawalConcept").val(null).trigger("change");
                } else {
                    $("#CourseWithdrawalConcept").val($("#CourseWithdrawalConceptId").val()).trigger("change");
                }
                if ($("#CycleWithdrawalConceptId").val() === _app.constants.guid.empty) {
                    $("#CycleWithdrawalConcept").val(null).trigger("change");
                } else {
                    $("#CycleWithdrawalConcept").val($("#CycleWithdrawalConceptId").val()).trigger("change");
                }
                if ($("#SubstitutoryExamConceptId").val() === _app.constants.guid.empty) {
                    $("#SubstitutoryExamConcept").val(null).trigger("change");
                } else {
                    $("#SubstitutoryExamConcept").val($("#SubstitutoryExamConceptId").val()).trigger("change");
                }

                //Records
                if ($("#RecordStudyConceptId").val() === _app.constants.guid.empty) {
                    $("#RecordStudyConcept").val(null).trigger("change");
                } else {
                    $("#RecordStudyConcept").val($("#RecordStudyConceptId").val()).trigger("change");
                }
                if ($("#RecordProofOnIncomeConceptId").val() === _app.constants.guid.empty) {
                    $("#RecordProofOnIncomeConcept").val(null).trigger("change");
                } else {
                    $("#RecordProofOnIncomeConcept").val($("#RecordProofOnIncomeConceptId").val()).trigger("change");
                }
                if ($("#RecordEnrollmentConceptId").val() === _app.constants.guid.empty) {
                    $("#RecordEnrollmentConcept").val(null).trigger("change");
                } else {
                    $("#RecordEnrollmentConcept").val($("#RecordEnrollmentConceptId").val()).trigger("change");
                }
                if ($("#RecordRegularStudiesConceptId").val() === _app.constants.guid.empty) {
                    $("#RecordRegularStudiesConcept").val(null).trigger("change");
                } else {
                    $("#RecordRegularStudiesConcept").val($("#RecordRegularStudiesConceptId").val()).trigger("change");
                }
                if ($("#RecordEgressConceptId").val() === _app.constants.guid.empty) {
                    $("#RecordEgressConcept").val(null).trigger("change");
                } else {
                    $("#RecordEgressConcept").val($("#RecordEgressConceptId").val()).trigger("change");
                }
                if ($("#RecordMeritChartConceptId").val() === _app.constants.guid.empty) {
                    $("#RecordMeritChartConcept").val(null).trigger("change");
                } else {
                    $("#RecordMeritChartConcept").val($("#RecordMeritChartConceptId").val()).trigger("change");
                }
                if ($("#RecordUpperFifthConceptId").val() === _app.constants.guid.empty) {
                    $("#RecordUpperFifthConcept").val(null).trigger("change");
                } else {
                    $("#RecordUpperFifthConcept").val($("#RecordUpperFifthConceptId").val()).trigger("change");
                }
                if ($("#RecordUpperThirdConceptId").val() === _app.constants.guid.empty) {
                    $("#RecordUpperThirdConcept").val(null).trigger("change");
                } else {
                    $("#RecordUpperThirdConcept").val($("#RecordUpperThirdConceptId").val()).trigger("change");
                }
                if ($("#RecordAcademicRecordConceptId").val() === _app.constants.guid.empty) {
                    $("#RecordAcademicRecordConcept").val(null).trigger("change");
                } else {
                    $("#RecordAcademicRecordConcept").val($("#RecordAcademicRecordConceptId").val()).trigger("change");
                }
                if ($("#RecordAcademicPerformanceSummaryConceptId").val() === _app.constants.guid.empty) {
                    $("#RecordAcademicPerformanceSummaryConcept").val(null).trigger("change");
                } else {
                    $("#RecordAcademicPerformanceSummaryConcept").val($("#RecordAcademicPerformanceSummaryConceptId").val()).trigger("change");
                }
                if ($("#RecordBachelorConceptId").val() === _app.constants.guid.empty) {
                    $("#RecordBachelorConcept").val(null).trigger("change");
                } else {
                    $("#RecordBachelorConcept").val($("#RecordBachelorConceptId").val()).trigger("change");
                }
                if ($("#RecordJobTitleConceptId").val() === _app.constants.guid.empty) {
                    $("#RecordJobTitleConcept").val(null).trigger("change");
                } else {
                    $("#RecordJobTitleConcept").val($("#RecordJobTitleConceptId").val()).trigger("change");
                }
                if ($("#RecordCertificateStudiesConceptId").val() === _app.constants.guid.empty) {
                    $("#RecordCertificateStudiesConcept").val(null).trigger("change");
                } else {
                    $("#RecordCertificateStudiesConcept").val($("#RecordCertificateStudiesConceptId").val()).trigger("change");
                }


                if ($("#RecordRectificationChargeId").val() === _app.constants.guid.empty) {
                    $("#RecordRectificationCharge").val(null).trigger("change");
                } else {
                    $("#RecordRectificationCharge").val($("#RecordRectificationChargeId").val()).trigger("change");
                }

            });
        },
        evaluationReport: {
            actFormat: {
                init: function () {
                    $("#EvaluationReportActFormat").select2();
                    //$("#EvaluationReportActFormat").val($("#EvaluationReportActFormat_Value").val()).trigger("change");
                }
            },
            registerFormat: {
                init: function () {
                    $("#EvaluationReportRegisterFormat").select2();
                    //$("#EvaluationReportRegisterFormat").val($("#EvaluationReportRegisterFormat_Value").val()).trigger("change");
                }
            },
            date: {
                init: function () {
                    $("#EvaluationReportFormatDate").select2();
                }
            },
            init: function () {
                selects.evaluationReport.actFormat.init();
                selects.evaluationReport.registerFormat.init();
                selects.evaluationReport.date.init();
            }
        },
        evaluationTypes: {
            init: function () {
                $.ajax({
                    type: "GET",
                    url: "/get-tipos-evaluaciones"
                })
                    .done(function (e) {
                        $(".select2-evaluation-types").select2({
                            data: e,
                            placeholder: "Seleccionar tipo de evaluación"
                        });

                        $("#EvaluationTypeGradeRecovery").val($("#EvaluationTypeGradeRecovery_Value").val()).trigger("change");
                    })
            }
        },
        gradeRecoveryModality: {
            init: function () {
                $("#GradeRecoveryModality").select2({
                    placeholder: "Seleccionar modalidad"
                });

                $("#GradeRecoveryModality").val($("#GradeRecoveryModality_Value").val()).trigger("change");
            }
        },
        records: {
            init: function () {
                $("#FormatCertificateOfStudies").select2();
                $("#FormatFirstEnrollment").select2();

            }
        },
        init: function () {
            this.records.init();
            this.concepSelect();
            this.evaluationTypes.init();
            this.substitoryEvaluation();
            this.extraordinaryEvaluationTypes();
            this.evaluationReport.init();
            this.gradeRecoveryModality.init();
        }
    };

    var form = {
        init: function () {
            $("#configForm").validate({
                submitHandler: function (e) {
                    swal({
                        title: "Confirmacíon de cambios",
                        text: "Se actualizaran las variables del sistema. Esto afecttará de manera inmediata a las funcionalidades relacionadas.",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Confirmar",
                        confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                        cancelButtonText: "Cancelar"
                    }).then(function (result) {
                        if (result.value) {
                            //$(".btn-submit").addLoader();
                            mApp.block("#configForm");
                            var formData = new FormData($(e)[0]);

                            formData.delete("ExtraordinaryEvaluationTypesEnabled");
                            formData.append("ExtraordinaryEvaluationTypesEnabled", JSON.stringify($("#ExtraordinaryEvaluationTypesEnabled").val()));

                            $.ajax({
                                url: $(e).attr("action"),
                                type: "POST",
                                //data: $(e).serialize()
                                data: formData,
                                contentType: false,
                                processData: false
                            }).done(function () {
                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            }).fail(function (e) {
                                toastr.error(e.responseText, _app.constants.toastr.title.error);
                            }).always(function () {
                                //$(".btn-submit").removeLoader();
                                mApp.unblock("#configForm");
                            });
                        }
                    });
                }
            });
        }
    };

    var events = {
        onSubmit: function () {
            $(".btn-submit").on("click", function () {
                if ($("#configForm").valid()) {
                    $("#configForm").submit();
                } else {
                    var $div = $(".has-danger");
                    if ($div) {
                        var parent = $div.parent().parent();
                        var id = parent.attr("id");
                        $(`a[href="#${id}"]`).tab('show');
                    }
                }
            })
        },
        onDownloadActPreview: function () {
            $("#download_act_preview").on("click", function () {
                var format = $("#EvaluationReportActFormat").val();
                var url = `/admin/configuracion/obtener-previsualizacion-acta/formato/${format}`;

                var $btn = $(this);
                $btn.addLoader();
                $.fileDownload(url, {
                    httpMethod: 'GET', successCallback: function () {
                        $btn.removeLoader();
                        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                        datatable.reports.reload();
                    },
                    failCallback: function (responseHtml, url) {
                        toastr.error("No se pudo descargar el archivo", "Error");
                    }
                });
            })
        },
        onChangeStudentGradeCorrectionRequest: function () {
            $("#EnableStudentGradeCorrectionRequest").on("change", function () {
                if ($(this).is(":checked")) {
                    $(".student_grade_correction_request_container").removeClass("d-none");
                } else {
                    $(".student_grade_correction_request_container").addClass("d-none");
                }
            })
        },
        onChangeEnabledSpecialAbsencePercentage: function () {
            $("#EnabledSpecialAbsencePercentage").on("change", function () {
                if ($(this).is(":checked")) {
                    $(".enabled_special_absence_percentage_container").removeClass("d-none");
                } else {
                    $(".enabled_special_absence_percentage_container").addClass("d-none");
                }
            })
        },
        onChangeEnabledImageWatermarkRecord: function () {
            $("#EnabledImageWatermarkRecord").on("change", function () {

                $("#ImageWatermarkRecord").val(null).trigger("change");
                $(".image_watermark_record_label").text("Seleccione un archivo");

                if ($(this).is(":checked")) {
                    $("#ImageWatermarkRecord").attr("disabled", false);
                } else {
                    $("#ImageWatermarkRecord").attr("disabled", true);
                }
            })
        },
        init: function () {
            this.onChangeEnabledImageWatermarkRecord();
            this.onSubmit();
            this.onDownloadActPreview();
            this.onChangeStudentGradeCorrectionRequest();
            this.onChangeEnabledSpecialAbsencePercentage();
        }
    }

    var summernote = {
        options: {
            toolbar: [
                // [groupName, [list of button]]
                ['style', ['bold', 'italic', 'underline', 'clear']],
                ['fontsize', ['fontsize']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
            ],
            height: 240,
            maxHeight: 240,
            minHeight: 240,
            disableResizeEditor: true
        },
        enrollmentProformaFooterText: {
            init: function () {
                $("#EnrollmentProformaFooterText")
                    .summernote(summernote.options)
                    .on("summernote.enter", function (we, e) {
                        $(this).summernote('pasteHTML', '<br>&VeryThinSpace;');
                        e.preventDefault();
                    });
            }
        },
        init: function () {
            summernote.enrollmentProformaFooterText.init();
        }
    }

    return {
        init: function () {
            form.init();
            selects.init();
            events.init();
            summernote.init();
            //showCheck.init();
        }
    };
}();

jQuery(document).ready(function () {
    InitApp.init();
});