var InitApp = function () {

    var options = {
        ajax: {
            type: "GET",
            dataType: "JSON",
            url: "/coordinador-academico/visualizador-actas/get".proto().parseURL(),
            data: function (values) {
                values.courseName = $("#courseName").val();
                values.careerId = $("#careerId").val();
                values.academicProgramId = $("#academicProgramId").val();
                values.curriculumId = $("#curriculumId").val();
                values.evaluationCode = $("#evaluation_report_code").val();
                values.termId = $("#term_select").val();
            }
        },
        columns: [
            {
                data: "career",
                title: "Escuela",
                orderable: false
            },
            {
                data: "course",
                title: "Curso",
                orderable: false
            },
            {
                data: "section",
                title: "Sección",
                width: 100,
                orderable: false
            },
            {
                data: "teachers",
                title: "Profesor(es)",
                orderable: false
            },
            //{
            //    data: "academicYear",
            //    title: "Semestre",
            //    width: 100
            //},
            //{
            //    data: "status",
            //    title: "Estado",
            //    width: 100,
            //    render: function (data) {
            //        var status = {
            //            "Recibido": { "title": "Recibido", "class": " m-badge--success" },
            //            "Generado": { "title": "Generado", "class": " m-badge--warning" },
            //            "Pendiente": { "title": "Pendiente", "class": " m-badge--metal" }
            //        };
            //        return '<span class="m-badge ' + status[data].class + ' m-badge--wide">' + status[data].title + "</span>";
            //    }
            //},
            //,
            {
                data: "evaluationReportCode",
                title: "Acta",
                width: 100,
                orderable: false

            },
            {
                data: null,
                title: "Detalle",
                orderable: false,
                render: function (data) {
                    var buttons = `<button data-id=${data.id} class="btn btn-primary btn-sm m-btn m-btn--icon btn-preview-details"><i class="la la-eye"></i></button> `;
                    return buttons;

                }
            },

        ],
        rowGroup: {
            dataSrc: "academicYearName"
        },
    };

    var datatable = {
        reports: {
            object: null,
            init: function () {
                $("#data-table")
                    .on('click', ".btn-preview-details", function () {
                        var id = $(this).data('id');
                        $("#preview-datatable").html('');
                        $("#previewView").modal('show');

                        mApp.block("#preview-datatable");

                        $.ajax({
                            url: `/coordinador-academico/visualizador-actas/obtener-vista-previa/${id}`.proto().parseURL(),
                            type: "GET",
                            dataType: "html",
                            contextType: "application/json"
                        })
                            .done(function (data) {
                                console.log(data);
                                $("#preview-datatable").html(data);
                            })
                            .fail(function (data) {
                                console.log(data);
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            }).always(function () {
                                mApp.unblock("#preview-datatable");
                            });

                    });
                $("#courseName").doneTyping(function () {
                    datatable.reports.reload();
                });

            },
            reload: function () {
                if (datatable.reports.object == null) {
                    datatable.reports.object = $("#data-table").DataTable(options);
                } else {
                    datatable.reports.object.ajax.reload();
                }
            }
        }
    };

    var select2 = {
        career: {
            events: {
                load: function () {
                    $.ajax({
                        url: ("/carreras/v3/get").proto().parseURL()
                    }).done(function (data) {
                        $("#careerId").select2({
                            data: data.items,
                            allowClear: true,
                            placeholder: "Seleccione Escuela Profesional"
                        });
                        $('#careerId').prepend($('<option>', {
                            value: _app.constants.guid.empty,
                            text: 'Todas'
                        }));
                        $('#careerId').val(_app.constants.guid.empty).trigger('change');
                        //datatable.reports.reload();
                    });

                },
                onChange: function () {
                    //$("#careerId").on("change", function () {
                    //    var careerId = $(this).val();
                    //    select2.curriculum.events.load(careerId);
                    //});
                },
                init: function () {
                    this.load();
                    this.onChange();
                }
            }
        },
        curriculum: {
            events: {
                load: function (careerId) {
                    $("#curriculumId").empty();
                    if (careerId == _app.constants.guid.empty) {
                        $("#curriculumId").select2({
                            allowClear: true,
                            placeholder: "Seleccione Escuela Profesional"
                        });
                        $('#curriculumId').prepend($('<option>', {
                            value: _app.constants.guid.empty,
                            text: 'Todas'
                        }));
                        $('#curriculumId').val(_app.constants.guid.empty).trigger('change');

                    } else {
                        $.ajax({
                            url: (`/planes-estudio/${careerId}/get`).proto().parseURL()
                        }).done(function (data) {
                            $("#curriculumId").select2({
                                data: data.items,
                            });
                            $('#curriculumId').prepend($('<option>', {
                                value: _app.constants.guid.empty,
                                text: 'Todas'
                            }));
                            $('#curriculumId').val($('#curriculumId').val()).trigger('change');
                        });

                    }
                    //$("#curriculumId").on("change", function () {
                    //    var curriculumId = $(this).val();
                    //    select2.academicProgram.events.load(curriculumId);
                    //    datatable.reports.reload();
                    //});
                },
                onChange: function () {
                    //$("#curriculumId").on("change", function () {
                    //    var curriculumId = $(this).val();
                    //    select2.academicProgram.events.load(curriculumId);
                    //});
                },
                init: function () {
                    this.load();
                    this.onChange();

                }
            }
        },
        academicProgram: {
            events: {
                load: function (curriculumId) {
                    $("#academicProgramId").empty();
                    if (curriculumId == _app.constants.guid.empty) {

                        $("#academicProgramId").select2({
                            allowClear: true,
                            placeholder: "Seleccione Programa/Especialidad"
                        });
                        $('#academicProgramId').prepend($('<option>', {
                            value: _app.constants.guid.empty,
                            text: 'Todas'
                        }));
                        $('#academicProgramId').val($('#academicProgramId').val()).trigger('change');
                    } else {

                        $.ajax({
                            url: (`/programas-por-plan/${curriculumId}`).proto().parseURL()
                        }).done(function (data) {
                            $("#academicProgramId").select2({
                                data: data,
                            });
                            $('#academicProgramId').val($('#academicProgramId').val()).trigger('change');
                        });
                    }

                    //$("#academicProgramId").on("change", function () {
                    //    datatable.reports.reload();
                    //});
                },
                onChange: function () {
                    //$("#academicProgramId").on("change", function () {
                    //    datatable.reports.reload();
                    //});
                },
                init: function () {
                    this.load();
                    this.onChange();
                }
            }
        },
        term: {
            events: {
                load: function () {
                    $.ajax({
                        url: ("/coordinador-academico/visualizador-actas/get-periodos").proto().parseURL()
                    }).done(function (data) {
                        $("#term_select").select2({
                            data: data
                        });
                        //$("#term_select").val(data.selected).trigger("change");
                    });

                },
                init: function () {
                    this.load();
                }
            },
            init: function () {
                this.events.init();
            }
        },
        init: function () {
            select2.term.init();
            select2.career.events.init();
            $("#careerId").on("change", function () {
                var careerId = $(this).val();
                select2.curriculum.events.load(careerId);
            });
            $("#curriculumId").on("change", function () {
                var curriculumId = $(this).val();
                select2.academicProgram.events.load(curriculumId);
                //datatable.reports.reload();
            });
            $("#academicProgramId").on("change", function () {
                //datatable.reports.reload();
            });

        }
    };

    var findEvaluationReportCode = function () {
        $("#search_by_code").on('click', function () {
            datatable.reports.reload();
        });
        $("#search_all").on('click', function () {
            datatable.reports.reload();
        });
    };

    return {
        init: function () {
            findEvaluationReportCode();
            select2.init();
            datatable.reports.init();

        }
    };
}();

$(function () {
    InitApp.init();
});