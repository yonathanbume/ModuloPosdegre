var InitApp = function () {

    var datatable = {
        students: {
            object: null,
            options: {
                serverSide: false,
                ajax: {
                    url: `/reporte/desaprobados-unidad/get`,
                    type: "GET",
                    data: function (data) {
                        data.careerId = $("#career_select").val();
                        data.curriculumId = $("#curriculum_select").val();
                        data.academicYear = $("#academicyear_select").val();
                        data.unit = $("#unit_select").val();
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        data: "code",
                        title: "Usuario"
                    },
                    {
                        data: "name",
                        title: "Estudiante"
                    },
                    {
                        data: "unit",
                        title: "Unidad"
                    },
                    {
                        data: "count",
                        title: "Cursos desaprobados"
                    }
                ],
                dom: 'Bfrtip',
                buttons: ['excel']
            },
            reload: function () {
                if (this.object == undefined || this.object == null) {
                    this.object = $("#data-table").DataTable(this.options);
                } else {
                    this.object.ajax.reload();
                }
            },
            init: function () {
                //this.object = $("#data-table").DataTable(this.options);
            }
        },
        init: function () {
            datatable.students.init();
        }
    }

    var select = {
        term: {
            init: function () {
                $.ajax({
                    url: "/periodos/get".proto().parseURL()
                }).done(function (data) {
                    $("#term_select").select2({
                        data: data.items,
                        disabled: true
                    });

                    if (data.selected !== null) {
                        $("#term_select").val(data.selected).trigger("change");
                    }
                    else $("#term_select").val(null).trigger("change");
                });
            },
        },
        careers: {
            init: function () {
                $.ajax({
                    url: `/carreras/get`
                })
                    .done(function (e) {
                        $("#career_select").select2({
                            data: e.items,
                            disabled: false
                        });

                        $("#career_select").on("change", function () {
                            var id = $(this).val();
                            select.curriculum.load(id);
                            //datatable.graduated.reload();
                        });
                    });
            }
        },
        curriculum: {
            load: function (careerId) {
                $("#curriculum_select").empty();
                if (careerId === null || careerId === _app.constants.guid.empty || careerId === "Todos") {
                    $("#curriculum_select").select2({
                        disabled: true,
                        placeholder: "Seleccionar plan de estudio"
                    });
                } else {
                    $.ajax({
                        url: `/carreras/${careerId}/planestudio/get`
                    })
                        .done(function (e) {
                            $("#curriculum_select").select2({
                                data: e.items,
                                disabled: false
                            }).trigger("change");
                        });
                }
            },
            init: function () {
                $("#curriculum_select").select2({
                    disabled: true,
                    placeholder: "Seleccionar plan de estudio"
                });

                $("#curriculum_select").on("change", function () {
                    //datatable.graduated.reload(); 
                    var id = $(this).val();
                    select.academicYear.load(id);
                });
            }
        },
        academicYear: {
            load: function (curriculumId) {
                $("#academicyear_select").empty();

                if (curriculumId === null || curriculumId === _app.constants.guid.empty) {
                    $("#academicyear_select").select2({
                        disabled: true,
                        placeholder: "Ciclo"
                    });
                } else {
                    $.ajax({
                        url: `/planes-estudio/${curriculumId}/niveles/get`
                    })
                        .done(function (e) {
                            $("#academicyear_select").select2({
                                data: e.items,
                                disabled: false
                            }).trigger("change");
                        });
                }
            },
            onChange: function () {
                $("#academicyear_select").on("change", function () {
                    //datatable.graduated.reload();
                    var academicYear = $(this).val();
                    var curriculum = $('#curriculum_select').val();
                    select.unit.load(curriculum, academicYear);
                });
            },
            init: function () {
                $("#academicyear_select").select2({
                    disabled: true,
                    placeholder: "Ciclo"
                });
                this.onChange();
            }
        },
        unit: {
            load: function (curriculumId, academicYear) {
                $("#unit_select").empty();

                $.ajax({
                    url: `/reporte/desaprobados-unidad/get-unidades`,
                    data: {
                        curriculumId: curriculumId,
                        academicYear: academicYear
                    }
                })
                    .done(function (e) {
                        $("#unit_select").select2({
                            data: e.items,
                            disabled: false
                        }).trigger("change");
                    });
            },
            init: function () {
                $("#unit_select").select2({
                    disabled: true,
                    placeholder: "Unidad"
                });
            }
        },
        init: function () {
            select.term.init();
            select.careers.init();
            select.curriculum.init();
            select.academicYear.init();
            select.unit.init();
        }
    }


    var events = {
        init: function () {
            $(".btn-search").on("click", function () {

                var career = $('#career_select').val();
                var curriculum = $('#curriculum_select').val();
                var academicyear = $('#academicyear_select').val();
                var unit = $('#unit_select').val();


                if (career == null || career == ''
                    || curriculum == null || curriculum == ''
                    || academicyear == null || academicyear == ''
                    || unit == null || unit == '') {

                    toastr.error("Debe completar todos los campos", "Error");

                    return false;
                }
                datatable.students.reload();
            })
        }
    };

    return {
        init: function () {
            //datatable.init();
            select.init();
            events.init();
        }
    }
}();

$(function () {
    InitApp.init();
});