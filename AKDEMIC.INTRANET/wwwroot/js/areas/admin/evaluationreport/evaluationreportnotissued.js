var report = function () {

    var datatable = {
        evaluationreports: {
            object: null,
            options: {
                ajax: {
                    url: `/admin/generacion-actas/buscador/datatable/no-emitidas`,
                    data: function (data) {
                        delete data.columns;
                        data.termId = $("#term_select").val();
                        data.careerId = $("#career_select").val();
                        data.curriculumId = $("#curriculum_select").val();
                    }
                },
                columns: [
                    {
                        title: "Código",
                        data: "courseCode",
                        orderable : false
                    },
                    {
                        title: "Curso",
                        data: "courseName",
                        orderable: false
                    },
                    {
                        title: "Sección",
                        data: "section",
                        orderable: false
                    },
                    {
                        title: "Profesor(es)",
                        data: "teachers",
                        orderable: false
                    },
                    {
                        title: "Periodo académico",
                        data: "academicYear",
                        orderable: false
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (row) {
                            var tpm = "";
                            tpm += `<button data-id=${row.id} class="btn btn-primary btn-sm m-btn m-btn--icon btn-preview-details"><i class="la la-eye"></i></button> `;
                            tpm += `<button data-id='${row.id}' class="btn btn-primary btn-sm m-btn m-btn--icon btn-assign"><span><i class="la la-edit"></i><span>Asignar</span></span></button>`;
                            return tpm;
                        }
                    }

                ],
                rowGroup: {
                    dataSrc: "academicYear"
                },
                columnDefs: [
                    {
                        visible: false,
                        targets: 0
                    }
                ]
            },
            reload: function () {
                if (datatable.evaluationreports.object === null) {
                    datatable.evaluationreports.object = $("#data-table").DataTable(datatable.evaluationreports.options);
                } else {
                    datatable.evaluationreports.object.ajax.reload();
                }
            },
            events: function () {
                $("#data-table").on('click', '.btn-assign', function () {
                    var id = $(this).data("id");
                    modal.assignCode.show(id);

                });

                $("#data-table")
                    .on('click', ".btn-preview-details", function () {
                        var id = $(this).data('id');
                        $("#preview-datatable").html('');
                        $("#previewView").modal('show');

                        mApp.block("#preview-datatable");

                        $.ajax({
                            url: `/admin/generacion-actas/obtener-vista-previa/${id}`.proto().parseURL(),
                            type: "GET",
                            dataType: "html",
                            contextType: "application/json"
                        })
                            .done(function (data) {
                                $("#preview-datatable").html(data);
                            })
                            .fail(function (data) {
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            }).always(function () {
                                mApp.unblock("#preview-datatable");
                            });

                    });
            },
            init: function () {
                datatable.evaluationreports.events();
            }
        },
        init: function () {
            datatable.evaluationreports.init();
        }
    };

    var select2 = {
        terms: {
            init: function () {
                select2.terms.load();
            },
            load: function () {
                $.ajax({
                    url: ("/periodos/get").proto().parseURL()
                }).done(function (result) {
                    $("#term_select").select2({
                        data: result.items
                    });

                    datatable.evaluationreports.object = $("#data-table").DataTable(datatable.evaluationreports.options);

                    $("#term_select").on("change", function () {
                        datatable.evaluationreports.reload();
                    })
                });
            }
        },
        careers: {
            init: function () {
                select2.careers.load();
                select2.careers.events();
            },
            load: function () {
                $.ajax({
                    url: ("/carreras/v2/get").proto().parseURL()
                }).done(function (result) {
                    result.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });
                    $("#career_select").select2({
                        placeholder: "Seleccionra escuela",
                        data: result.items
                    });
                });
            },
            events: function () {
                $("#career_select").on('change', function (e, state) {
                    select2.curriculums.load();
                    datatable.evaluationreports.reload();
                });
            }
        },
        curriculums: {
            init: function () {
                $("#curriculum_select").select2();
                select2.curriculums.events();
            },
            load: function () {
                $("#curriculum_select").prop('disabled', false);
                $('#curriculum_select').html("<option value='0' disabled selected>Seleccione un Plan</option>");
                $.ajax({
                    url: (`/carreras/${$("#career_select").val()}/planestudio/get`).proto().parseURL()
                }).done(function (result) {
                    $("#curriculum_select").select2({
                        data: result.items
                    });
                });
            },
            events: function () {
                $("#curriculum_select").on('change', function (e, state) {
                    datatable.evaluationreports.reload();
                });
            }
        },
        init: function () {
            select2.terms.init();
            select2.careers.init();
            select2.curriculums.init();
        }
    };

    var events = {
        onValidateCode: function () {
            $(".validate_evaluationrepot").on("click", function () {

                var $btn = $(this);

                var code = $("[name='EvaluationReportCode']").val();
                var sectionId = $("[name='SectionId']").val();

                if (code == null || code == "") {
                    toastr.info("Es necesario ingresar el código de acta", "Información");
                    return;
                }

                $btn.addLoader();

                $.ajax({
                    url: `/admin/generacion-actas/validar-codigo-acta-seccion?code=${code}&sectionId=${sectionId}`,
                    type : "GET"
                })
                    .done(function (e) {
                        $("[name='EvaluationReportId']").val(e.id);
                        modal.assignCode.object.find("[name='ReceptionDate']").attr("disabled", false);
                        modal.assignCode.object.find("[name='ReceptionDate']").datepicker("destroy");
                        $("[name='ReceptionDate']").val(e.receptionDate).trigger("change");
                        modal.assignCode.object.find("[name='ReceptionDate']").datepicker();
                    })
                    .fail(function (e) {
                        toastr.error(e.responseText, "Error");
                        modal.assignCode.object.find("[name='ReceptionDate']").attr("disabled", true);
                        modal.assignCode.object.find("[name='ReceptionDate']").val("");
                        modal.assignCode.object.find("[name='EvaluationReportId']").val("");
                    })
                    .always(function (e) {
                        $btn.removeLoader();
                    })
            })
        },
        init: function () {
            this.onValidateCode();
        }
    }

    var modal = {
        assignCode: {
            object: $("#modal_assign_code"),
            form: {
                object: $("#generate_evaluation_report").validate({
                    submitHandler: function () {
                        var $btn = $("#submit_evaluation_assign");
                        var sectionId = modal.assignCode.object.find("[name='SectionId']").val();
                        var evaluationReportId = modal.assignCode.object.find("[name='EvaluationReportId']").val();
                        var receptionDate = modal.assignCode.object.find("[name='ReceptionDate']").val();

                        if (evaluationReportId == "" || evaluationReportId == null || evaluationReportId == undefined) {
                            toastr.error("Es necesario validar el acta.", "Error");
                            return;
                        }

                        var url = `/admin/generacion-actas/asignar-acta-codigo-seccion?evaluationReportId=${evaluationReportId}&sectionId=${sectionId}&receptionDate=${receptionDate}`;
                        $btn.addLoader();

                        $.ajax({
                            url: url,
                            type: "POST"
                        })
                            .done(function (e) {
                                toastr.success("Acta asignada con éxito", "Error");
                                datatable.evaluationreports.reload();
                                modal.assignCode.object.modal("hide");
                            })
                            .fail(function (e) {
                                toastr.error(e.responseText, "Error al asignar el acta");
                            })
                            .always(function () {
                                $btn.removeLoader();
                            })
                    }
                })
            },
            show: function (sectionId) {
                modal.assignCode.object.find("[name='SectionId']").val(sectionId);
                this.object.modal("show");
            },
            onHidden: function () {
                modal.assignCode.object.on('hidden.bs.modal', function (e) {
                    modal.assignCode.object.find("[name='SectionId']").val("");
                    modal.assignCode.object.find("[name='EvaluationReportId']").val("");
                    modal.assignCode.object.find("[name='EvaluationReportCode']").val("");
                    modal.assignCode.object.find("[name='ReceptionDate']").val("");
                    modal.assignCode.object.find("[name='ReceptionDate']").attr("disabled", true);
                });
            },
            init: function () {
                this.onHidden();
            }
        },
        init: function () {
            this.assignCode.init();
        }
    };

    var datepicker = {
        init: function () {
            modal.assignCode.object.find("[name='IssueDate']").datepicker();
            modal.assignCode.object.find("[name='ReceptionDate']").datepicker();
        }
    }

    return {
        init: function () {
            select2.init();
            datatable.init();
            datepicker.init();
            modal.init();
            events.init();
        }
    };
}();

$(() => {
    report.init();
});