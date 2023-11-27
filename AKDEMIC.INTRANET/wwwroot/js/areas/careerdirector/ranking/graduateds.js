var Graduateds = function () {

    var studentsDatatable = null;
    var termsInit = false;
    var careersInit = false;
    var academicProgramsInit = false;

    var options = getSimpleDataTableConfiguration({
        url: "/director-carrera/ranking/egresados/get".proto().parseURL(),
        data: function (data) {
            delete data.columns;
            var admissionTermId = $("#admission-term").val();
            var graduationTermId = $("#graduation-term").val();
            var careerId = $(".select2-careers").val();
            var academicProgramId = $(".select2-academicPrograms").val();

            admissionTermId = admissionTermId === _app.constants.guid.empty ? null : admissionTermId;
            graduationTermId = graduationTermId === _app.constants.guid.empty ? null : graduationTermId;
            careerId = careerId === _app.constants.guid.empty ? null : careerId;
            academicProgramId = academicProgramId === _app.constants.guid.empty ? null : academicProgramId;

            data.academicProgramId = academicProgramId;
            data.admissionTermId = admissionTermId;
            data.graduationTermId = graduationTermId;
            data.careerId = careerId;
            data.search = $("#search").val();
        },
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
                title: "Semestre de Ingreso",
                data: "admissionTerm",
                orderable: false
            },
            {
                title: "Semestre de Egreso",
                data: "graduationTerm",
                orderable: false
            },
            {
                title: "Promedio Ponderado",
                data: "weightedAverageGrade",
                orderable: false
            },
            {
                title: "Orden de Mérito",
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
                    select2.careers.init();
                    termsInit = true;
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
                        delay: 300,
                        data: function (params) {
                            var query = {
                                q: params.term,
                                cid: $(".select2-careers").val()
                            };
                            return query;
                        },
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
    };

    var fileDownload = {
        init: function () {
            $(".btn-report").on("click",
                function () {
                    var $btn = $(this);
                    var admissionTermId = $("#admission-term").val();
                    var graduationTermId = $("#graduation-term").val();
                    var careerId = $(".select2-careers").val();

                    admissionTermId = admissionTermId === _app.constants.guid.empty ? null : admissionTermId;
                    graduationTermId = graduationTermId === _app.constants.guid.empty ? null : graduationTermId;
                    careerId = careerId === _app.constants.guid.empty ? null : careerId;

                    $btn.addLoader();
                    $.fileDownload(`${location.pathname}/pdf/get?admissionTermId=${admissionTermId}&graduationTermId=${graduationTermId}&careerId=${careerId}`.proto().parseURL())
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
    Graduateds.init();
});