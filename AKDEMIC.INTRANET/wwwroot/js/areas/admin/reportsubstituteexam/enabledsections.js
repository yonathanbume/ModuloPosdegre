var index = function () {

    var datatable = {
        object: null,
        options: {
            ajax: {
                url: "/admin/reporte-examen-sustitutorio/get-secciones-habilitadas",
                type: "GET",
                data: function (data) {
                    data.termId = $("#term_select").val();
                    data.careerId = $("#career_select").val();
                    data.curriculumId = $("#curriculum_select").val();
                    data.searchValue = $("#search").val();
                }
            },
            columns: [
                {
                    data: "section",
                    title : "Sección"
                },
                {
                    data: "courseCode",
                    title : "Cod"
                },
                {
                    data: "course",
                    title : "Curso"
                },
                {
                    data: "failedStudents",
                    title : "Cant. Estudiantes Desaprobados"
                }
            ]
        },
        reload: function () {
            if (datatable.object == null) {
                this.object = $("#datatable").DataTable(this.options);
            } else {
                this.object.ajax.reload();
            }
        }
    };

    var select = {
        curriculum: {
            load: function (careerId) {

                $("#curriculum_select").empty();

                if (careerId == null || careerId == "Todos") {
                    $("#curriculum_select").select2({
                        disabled: true,
                        placeholder: "Seleccionar plan de estudio"
                    }).trigger("change");
                } else {

                    $.ajax({
                        url: `/planes2?careerId=${careerId}`,
                        type : "GET"
                    })
                        .done(function (e) {
                            e.unshift("Todos");
                            $("#curriculum_select").select2({
                                disabled: false,
                                data: e
                            }).trigger("change");
                        })
                }


            },
            events: {
                init: function () {
                    $("#curriculum_select").on("change", function () {
                        datatable.reload();
                    })
                }
            },
            init: function () {
                select.curriculum.events.init();
                select.curriculum.load();
            }
        },
        career: {
            init: function () {
                $.ajax({
                    url: `/carreras/get`,
                    type : "GET"
                })
                    .done(function (e) {

                        e.items.unshift("Todos")

                        $("#career_select").select2({
                            data: e.items
                        })

                        $("#career_select").on("change", function () {
                            var value = $(this).val();
                            select.curriculum.load(value);
                        })
                    })
            }
        },
        term: {
            init: function () {
                $.ajax({
                    url: `/periodos/get`,
                    type : "GET"
                })
                    .done(function (e) {
                        $("#term_select").select2({
                            data : e.items
                        });

                        $("#term_select").val(e.selected).trigger("change");

                        $("#term_select").on("change", function () {
                            datatable.reload();
                        })
                    })
            }
        },
        init: function () {
            select.career.init();
            select.term.init();
            select.curriculum.init();
        }
    }

    var events = {
        onSearch: function () {
            $("#search").doneTyping(function () {
                datatable.reload();
            });
        },
        init: function () {
            this.onSearch();
        }
    };

    return {
        init: function () {
            select.init();
            events.init();
        }
    };
}();

$(() => {
    index.init();
});