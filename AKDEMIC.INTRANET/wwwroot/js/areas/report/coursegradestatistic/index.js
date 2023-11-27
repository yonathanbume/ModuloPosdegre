var InitApp = function () {
    var datatable = {
        courses: {
            object: null,
            options: {
                bInfo: true,
                columnDefs: [
                    { "orderable": false, "targets": [] }
                ],
                ajax: {
                    url: "/reporte/notas-curso/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.termId = $("#term_select").val();
                        data.careerId = $("#career_select").val();
                        data.curriculumId = $("#curriculum_select").val();
                        data.search = $("#search").val();
                    }
                },
                order: [[1, "asc"]],
                columns: [
                    {
                        title: "Código",
                        data: "code",
                        width: "50px"
                    },
                    {
                        title: "Curso",
                        data: "course"
                    },
                    {
                        title: "Escuela Profesional",
                        data: "career"
                    },
                    {
                        title: "Nro. Notas",
                        orderable: false,
                        data: "gradeCount"
                    },
                    {
                        title: "Media",
                        orderable: false,
                        data: "mean"
                    },
                    {
                        title: "Mediana",
                        orderable: false,
                        data: "median"
                    },
                    {
                        title: "Des. Estandar",
                        orderable: false,
                        data: "standardDeviation"
                    },
                    {
                        title: "Perc. 25",
                        orderable: false,
                        data: "percentile25"
                    },
                    {
                        title: "Perc. 50",
                        orderable: false,
                        data: "percentile50"
                    },
                    {
                        title: "Perc. 75",
                        orderable: false,
                        data: "percentile75"
                    }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Excel',
                        action: function (e, dt, node, config) {
                            var url = `/reporte/notas-curso/get-excel?termId=${$("#term_select").val()}&careerId=${$("#career_select").val()}&curriculumId=${$("#curriculum_select").val()}`;
                            window.open(url, "_blank");
                        }
                    }
                ]
            },
            init: function () {
                this.object = $("#course_table").DataTable(this.options);
            },
            load: function () {
                this.object.ajax.reload();
            }
        }
    };

    var select = {
        term: {
            init: function () {
                $.ajax({
                    url: "/periodos/get".proto().parseURL()
                }).done(function (data) {
                    $("#term_select").select2({
                        data: data.items
                    });

                    if (data.selected !== null) {
                        $("#term_select").val(data.selected);
                        $("#term_select").trigger("change.select2");
                    }

                    datatable.courses.init();

                    $("#term_select").on("change", function (e) {
                        datatable.courses.load();
                    });
                });
            }
        },
        career: {
            init: function () {
                $.ajax({
                    url: `/carreras/get`.proto().parseURL()
                }).done(function (data) {
                    //data.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $("#career_select").select2({
                        placeholder: "Seleccione una Escuela",
                        data: data.items,
                        minimumResultsForSearch: 10,
                        disabled: false
                    });

                    select.career.events();
                });
            },
            events: function () {
                $("#career_select").on("change", function () {
                    var careerId = $("#career_select").val();

                    if (careerId === _app.constants.guid.empty) {
                        datatable.courses.load();

                        $("#curriculum_select").empty();
                        $("#curriculum_select").select2({
                            placeholder: "Seleccione un plan",
                            disabled: true
                        });
                    } else {
                        datatable.courses.load();
                        select.curriculum.load(careerId);
                    }
                });
            }
        },
        curriculum: {
            init: function () {
                $("#curriculum_select").select2({
                    placeholder: "Seleccione un plan",
                    disabled: true
                });
            },
            load: function (careerId) {
                $.ajax({
                    url: `/carreras/${careerId}/planestudio/get`.proto().parseURL()
                }).done(function (data) {
                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $("#curriculum_select").select2({
                        placeholder: "Seleccione un plan",
                        data: data.items,
                        minimumResultsForSearch: 10,
                        disabled: false
                    });

                    $("#curriculum_select").on("change", function () {
                        datatable.courses.load();
                    });
                });
            }
        },
        init: function () {
            select.term.init();
            select.career.init();
            select.curriculum.init();
        }
    };

    var events = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.courses.load();
            });
        }
    }

    return {
        init: function () {
            select.init();
            events.init();
        }
    };
}();

$(function () {
    InitApp.init();
});