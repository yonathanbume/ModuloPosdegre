var MeritTable = function () {

    var studentsDatatable = null;
    var termsInit = false;
    var careersInit = false;
    var campusInit = false;
    var academicProgramsInit = false;

    var options = getSimpleDataTableConfiguration({
        url: "/director-carrera/ranking/cuadro-meritos/get".proto().parseURL(),
        data: function (data) {
            delete data.columns;
            var termId = $(".select2-terms").val();
            var campusId = $(".select2-campuses").val();
            var careerId = $(".select2-careers").val();
            var academicProgramId = $(".select2-academicPrograms").val();

            careerId = careerId === _app.constants.guid.empty ? null : careerId;
            campusId = campusId === _app.constants.guid.empty ? null : campusId;
            academicProgramId = academicProgramId === _app.constants.guid.empty ? null : academicProgramId;

            data.termId = termId;
            data.careerId = careerId;
            data.campusId = campusId;
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
                title: "Ciclo",
                data: "academicYear",
                orderable: false
            },
            {
                title: "Código",
                data: "code",
                orderable: false
            },
            {
                title: "Apellidos, Nombres",
                data: "name",
                orderable: false
            },
            {
                title: "Carrera",
                data: "career",
                orderable: false
            },
            {
                title: "Sede",
                data: "campus",
                orderable: false
            },
            {
                title: "Promedio",
                data: "weightedAverageGrade",
                orderable: false
            },
            {
                title: "Créditos",
                data: "credits",
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
            if (termsInit && careersInit && campusInit) {
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
            this.campuses.init();
        },
        terms: {
            init: function () {
                $.ajax({
                    url: "/director-carrera/ranking/periodos/get".proto().parseURL()
                }).done(function (result) {
                    $(".select2-terms").select2({
                        data: result
                    }).on("change", function () {
                        termsInit = true;
                        $(".select2-careers").empty();
                        $(".select2-academicPrograms").empty();
                        $(".select2-campuses").empty();
                        careersInit = false;
                        academicProgramsInit = false;
                        campusInit = false;
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
                        campusInit = false;
                        $(".select2-academicPrograms").empty();
                        $(".select2-campuses").empty();
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
                        campusInit = false;
                        $(".select2-campuses").empty();
                        select2.campuses.init();
                    }
                });
            }
        },
        campuses: {
            init: function () {
                $(".select2-campuses").select2({
                    ajax: {
                        url: "/director-carrera/ranking/sedes/get".proto().parseURL(),
                        delay: 300,
                    },
                    minimumInputLength: 0,
                    placeholder: 'Campus',
                    allowClear: true
                }).on("change", function () {
                    if ($(this).val() !== "") {
                        campusInit = true;
                        datatable.init();
                    }
                });
            }
        }
    };

    var fileDownload = {
        init: function () {
            $(".btn-report").on("click",
                function () {
                    var $btn = $(this);
                    var termId = $(".select2-terms").val();
                    var campusId = $(".select2-campuses").val();
                    var careerId = $(".select2-careers").val();

                    careerId = careerId === _app.constants.guid.empty ? null : careerId;
                    campusId = campusId === _app.constants.guid.empty ? null : campusId;

                    $btn.addLoader();
                    $.fileDownload(`${location.pathname}/pdf/get?termId=${termId}&careerId=${careerId}&campusId=${campusId}`.proto().parseURL())
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
    MeritTable.init();
});