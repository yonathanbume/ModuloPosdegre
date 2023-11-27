var index = function () {

    var datatable = {
        students: {
            object: null,
            options: {
                ajax: {
                    url: "/reporte/promedio-unidad-estudiantes/get-datatable",
                    data: function (data) {
                        data.termId = $("#term_select").val();
                        data.careerId = $("#career_select").val();
                        data.unitNumber = $("#unit_select").val();
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        data: "userName",
                        title: "Usuario"
                    },
                    {
                        data: "fullName",
                        title: "Nombre Completo"
                    },
                    {
                        data: "course",
                        title: "Curso"
                    },
                    {
                        data: "career",
                        title: "Escuela profesional"
                    },
                    {
                        data: "totalEvaluations",
                        title: "Cant. Evaluaciones"
                    },
                    {
                        data: "average",
                        title: "Promedio"
                    }
                ]
            },
            reload: function () {
                if (datatable.students.object == null) {
                    datatable.students.object = $("#students_table").DataTable(datatable.students.options);
                } else {
                    datatable.students.object.ajax.reload();
                }
            }
        }
    }

    var events = {
        init: function () {
            $("#btn-apply").on("click", function () {
                datatable.students.reload();
            })
        }
    }

    var select = {
        term: {
            load: function () {
                $.ajax({
                    url: "/periodos/get",
                    type: "GET"
                })
                    .done(function (e) {

                        $("#term_select").on("change", function () {
                            select.unit.load($(this).val());
                        })

                        $("#term_select").select2({
                            data: e.items,
                            placeholder: "Seleccionar periodos"
                        });

                        if (e.selected != null) {
                            $("#term_select").val(e.selected).trigger("change");
                        }
                    })

            },
            init: function () {
                this.load();
            }
        },
        faculty: {
            load: function () {
                $("#faculty_select").on("change", function () {
                    select.career.load($(this).val());;
                })

                $.ajax({
                    url: "/facultades/get"
                })
                    .done(function (e) {
                        $("#faculty_select").select2({
                            data: e.items,
                            placeholder: "Seleccionar facultad"
                        }).trigger("change");
                    })
            },
            init: function () {
                this.load();
            }
        },
        career: {
            load: function (faculyId) {
                $.ajax({
                    url: `/carreras/get?fid=${faculyId}`,
                    type: "GEt"
                })
                    .done(function (e) {
                        $("#career_select").empty();

                        $("#career_select").select2({
                            data: e.items,
                            placeholder: "Seleccionar escuela profesional"
                        });
                    })
            }
        },
        unit: {
            load: function (termId) {
                $.ajax({
                    url: `/get-maximas-unidades/${termId}`,
                    type: "GEt"
                })
                    .done(function (e) {
                        $("#unit_select").empty();

                        var data = [];

                        for (var i = 1; i <= e; i++) {
                            data.push({
                                id: i,
                                text: `Unidad ${i}`
                            });
                        }

                        $("#unit_select").select2({
                            data: data,
                            placeholder: "Seleccione unidad"
                        }).trigger("change");
                    })
            }
        },
        init: function () {
            select.term.init();
            select.faculty.init();
        }
    }

    return {
        init: function () {
            select.init();
            events.init();
        }
    }
}();

$(() => {
    index.init();
});