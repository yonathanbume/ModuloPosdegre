var NewStudents = function () {
    var studentsDatatable = null;
    var termsInit = false;
    var careersInit = false;
    var academicProgramsInit = false;

    var options = getSimpleDataTableConfiguration({
        url: "/director-carrera/ranking/ingresantes/get".proto().parseURL(),
        data: function (data) {
            delete data.columns;
            var admissionTermId = $(".select2-terms").val();
            var careerId = $(".select2-careers").val();
            var status = $(".select2-status").val();
            var academicProgramId = $(".select2-academicPrograms").val();

            admissionTermId = admissionTermId === _app.constants.guid.empty ? null : admissionTermId;
            careerId = careerId === _app.constants.guid.empty ? null : careerId;
            academicProgramId = academicProgramId === _app.constants.guid.empty ? null : academicProgramId;
            status = status == 0 ? null : status;

            data.admissionTermId = admissionTermId;
            data.careerId = careerId;
            data.status = status;
            data.academicProgramId = academicProgramId;
            data.search = $("#search").val();
        },
        pageLength: 10,
        orderable: [],
        columns: [
            {
                title: "Posición",
                data: "position",
                orderable: false
            },
            {
                title: "Código",
                data: "code",
                orderable: false
            },
            {
                title: "DNI",
                data: "dni",
                orderable: false
            },
            {
                title: "Apellidos, Nombres",
                data: "name",
                orderable: false
            },
            {
                title: "Carr.",
                data: "careerCode",
                orderable: false
            },
            {
                title: "Curr.",
                data: "curriculumCode",
                orderable: false
            },
            {
                title: "Sede Ingr.",
                data: "firstCampus",
                orderable: false
            },
            {
                title: "Sede Actu.",
                data: "currentCampus",
                orderable: false
            },
            {
                title: "Sem. de Ingreso",
                data: "admissionTerm",
                orderable: false
            },
            {
                title: "Sem. Ultimo",
                data: "lastTerm",
                orderable: false
            },
            {
                title: "Promedio Ultimo",
                data: "lastWeightedAverageGrade",
                orderable: false
            },
            {
                title: "Sem. de Egreso",
                data: "graduationTerm",
                orderable: false
            },
            {
                title: "Prom. de Egreso",
                data: "meritType",
                orderable: false
            },
            {
                title: "Estado",
                data: "status",
                orderable: false
            },
            {
                title: "Cicl. Actual",
                data: "currentAcademicYear",
                orderable: false
            },
            {
                title: "Orden de Mer.",
                data: "meritType",
                orderable: false
            }
        ]
    });

    var datatable = {
        init: function () {
            if (termsInit && careersInit) {
                if (studentsDatatable === null) {
                    studentsDatatable = $(".students-datatable").DataTable(options);
                }
                else {
                    studentsDatatable.ajax.reload();
                }
            }
        }
    };

    var input = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.init();
            });
        }
    };

    var select2 = {
        init: function () {
            this.terms.init();
            this.status.init();
        },
        terms: {
            init: function () {
                $.ajax({
                    url: "/director-carrera/ranking/periodos/get".proto().parseURL()
                }).done(function (result) {
                    $(".select2-terms").select2({
                        data: result
                    }).on("change", function () {
                        $(".select2-careers").empty();
                        $(".select2-academicPrograms").empty();
                        careersInit = false;
                        academicProgramsInit = false;
                    });
                    termsInit = true;
                    select2.careers.init();
                });
                $(".select2-careers").select2();
                $(".select2-academicPrograms").select2();
            }
        },
        careers: {
            init: function () {
                $(".select2-careers").select2({
                    ajax: {
                        url: "/director-carrera/ranking/carreras/get".proto().parseURL(),
                        delay: 300,
                    },
                    minimumInputLength: 0,
                    placeholder: 'Carreras',
                    allowClear: true
                }).on("change", function () {
                    if ($(this).val() !== "") {
                        careersInit = true;
                        academicProgramsInit = false;
                        $(".select2-academicPrograms").empty();
                        select2.academicPrograms.init();
                    }
                });
            }
        },
        academicPrograms: {
            init: function () {
                $(".select2-academicPrograms").select2({
                    ajax: {
                        url: "/director-carrera/ranking/especialidades/get".proto().parseURL(),
                        data: function (params) {
                            var query = {
                                q: params.term,
                                cid: $(".select2-careers").val()
                            };
                            return query;
                        },
                        delay: 300,
                    },
                    minimumInputLength: 0,
                    placeholder: 'Programa Académico',
                    allowClear: true
                }).on("change", function () {
                    if ($(this).val() !== "") {
                        academicProgramsInit = true;
                        datatable.init();
                    }
                });
            }
        },
        status: {
            init: function () {
                $(".select2-status").select2()
                    .on("change", function () {
                        datatable.init();
                    });
            }
        }
    };

    var fileDownload = {
        init: function () {
            $(".btn-report").on("click",
                function () {
                    var $btn = $(this);
                    var admissionTermId = $(".select2-terms").val();
                    var careerId = $(".select2-careers").val();
                    var academicProgramId = $(".select2-academicPrograms").val();
                    var status = $(".select2-status").val();

                    admissionTermId = admissionTermId === _app.constants.guid.empty ? null : admissionTermId;
                    careerId = careerId === _app.constants.guid.empty ? null : careerId;
                    status = status == 0 ? null : status;

                    $btn.addLoader();
                    $.fileDownload(`${location.pathname}/pdf/get?admissionTermId=${admissionTermId}&careerId=${careerId}&academicProgramId=${academicProgramId}&status=${status}`.proto().parseURL())
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
    };

    return {
        init: function () {
            select2.init();
            input.init();
            fileDownload.init();
        }
    }
}();

$(function () {
    NewStudents.init();
});