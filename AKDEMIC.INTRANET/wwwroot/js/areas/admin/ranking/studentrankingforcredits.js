var InitApp = function () {
    var datatable = {
        students: {
            object: null,
            options: {
                serverSide: false,
                ajax: {
                    url: "/admin/ranking/estudiantes-por-creditos/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        var termId = $(".select2-terms").val();
                        var campusId = $(".select2-campus").val();
                        var careerId = $(".select2-careers").val();
                        var academicProgramId = $(".select2-programs").val();

                        careerId = careerId === _app.constants.guid.empty ? null : careerId;
                        campusId = campusId === _app.constants.guid.empty ? null : campusId;
                        academicProgramId = academicProgramId === _app.constants.guid.empty ? null : academicProgramId;

                        data.termId = termId;
                        data.careerId = careerId;
                        data.campusId = campusId;
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
                        title: "Créditos en Ciclo",
                        data: "maxCredits",
                        orderable: false
                    }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'pdfHtml5',
                        title: 'Ranking de estudiantes',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                ]
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
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
            this.campus.init();
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
        campus: {
            init: function () {
                $.ajax({
                    url: "/sedes/get".proto().parseURL()
                }).done(function (data) {
                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });
                    $(".select2-campus").select2({
                        data: data.items
                    })

                    $(".select2-campus").on("change", function () {
                        datatable.students.reload();
                    });
                });
            }
        }
    };

    var events = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.students.reload();
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
    InitApp.init();
});