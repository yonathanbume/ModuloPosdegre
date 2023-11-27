var NewStudents = function () {
    var studentsDatatable = null;
    var termsInit = false;
    var careersInit = false;
    var academicProgramsInit = false;

    var input = {
        init: function () {
           
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
                    url: "/admin/ranking/periodos/get".proto().parseURL()
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
                        url: "/admin/ranking/carreras/get".proto().parseURL(),
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
                        url: "/admin/ranking/especialidades/get".proto().parseURL(),
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

var InitApp = function () {
    var datatable = {
        students: {
            object: null,
            options: {
                serverSide: false,
                ajax: {
                    url: "/admin/ranking/ingresantes/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        var admissionTermId = $(".select2-terms").val();
                        var careerId = $(".select2-careers").val();
                        var status = $(".select2-status").val();
                        var academicProgramId = $(".select2-programs").val();

                        admissionTermId = admissionTermId === _app.constants.guid.empty ? null : admissionTermId;
                        careerId = careerId === _app.constants.guid.empty ? null : careerId;
                        academicProgramId = academicProgramId === _app.constants.guid.empty ? null : academicProgramId;
                        status = status == 0 ? null : status;

                        data.admissionTermId = admissionTermId;
                        data.careerId = careerId;
                        data.status = status;
                        data.academicProgramId = academicProgramId;
                        data.search = $("#search").val();
                    }
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
            },
            init: function () {
                this.object = $(".students-datatable").DataTable(this.options);
            },
            reload: function () {
                this.object.ajax.reload();
            }
        }
    }

    var select = {
        init: function () {
            this.terms.init();
            this.careers.init();
            this.academicPrograms.init();
            this.status.init();
        },
        terms: {
            init: function () {
                $.ajax({
                    url: "/periodos/get".proto().parseURL()
                }).done(function (data) {
                    $(".select2-terms").select2({
                        data: data.items
                    })

                    if (data.selected !== null) {
                        $(".select2-terms").val(data.selected);
                        $(".select2-terms").trigger("change.select2");
                    }

                    datatable.students.init();

                    $(".select2-terms").on("change", function () {
                        datatable.students.reload();
                    });
                });
            }
        },
        careers: {
            init: function () {
                $.ajax({
                    url: "/carreras/get".proto().parseURL()
                }).done(function (data) {
                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });

                    $(".select2-careers").select2({
                        data: data.items
                    })

                    $(".select2-careers").on("change", function () {
                        var careerId = $(".select2-careers").val();

                        if (careerId === _app.constants.guid.empty) {
                            $(".select2-programs").empty();
                            $(".select2-programs").select2({
                                placeholder: "Seleccione una escuela",
                                disabled: true
                            });
                        } else {
                            select.academicPrograms.load(careerId);
                        }

                        datatable.students.reload();
                    });
                });
            }
        },
        academicPrograms: {
            init: function () {
                $(".select2-programs").select2({
                    placeholder: "Seleccione una escuela"
                });

                $(".select2-programs").on("change", function () {
                    datatable.students.reload();
                });
            },
            load: function (career) {
                $.ajax({
                    url: `/carreras/${career}/programas/get/`.proto().parseURL()
                }).done(function (data) {
                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });

                    $(".select2-programs").empty();
                    $(".select2-programs").select2({
                        placeholder: "Seleccione una Escuela",
                        data: data.items,
                        minimumResultsForSearch: 10,
                        disabled: false
                    });
                });
            },
        },
        status: {
            init: function () {
                $(".select2-status").select2()
                    .on("change", function () {
                        datatable.students.reload();
                    });
            }
        }
    };

    var events = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.students.reload();
            });

            $(".btn-report").on("click",
                function () {
                    var $btn = $(this);
                    var admissionTermId = $(".select2-terms").val();
                    var careerId = $(".select2-careers").val();
                    var status = $(".select2-status").val();

                    admissionTermId = admissionTermId === _app.constants.guid.empty ? null : admissionTermId;
                    careerId = careerId === _app.constants.guid.empty ? null : careerId;
                    status = status == 0 ? null : status;

                    $btn.addLoader();
                    $.fileDownload(`${location.pathname}/pdf/get?admissionTermId=${admissionTermId}&careerId=${careerId}&status=${status}`.proto().parseURL())
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
            select.init();
            events.init();
        }
    }
}();

$(function () {
    //NewStudents.init();
    InitApp.init();
});