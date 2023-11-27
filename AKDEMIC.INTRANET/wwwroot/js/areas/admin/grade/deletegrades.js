var CourseTable = function () {

    var select = {
        init: function () {
            this.terms.init();
            this.careers.init();
            this.plans.init();
            this.programs.init();
            this.cicles.init();
        },
        terms: {
            init: function () {
                $(".select2-terms").select2();
                $.ajax({
                    url: "/periodos/get".proto().parseURL()
                }).done(function (result) {
                    $(".select2-terms").select2({
                        data: result.items
                    });

                    if (result.selected) {
                        $(".select2-terms").val(result.selected).trigger("change");
                    }
                });
            }
        },
        careers: {
            init: function () {

                $(".select2-areacareers").select2({
                    ajax: {
                        delay: 1000,
                        url: (`/carreras/get/v4`).proto().parseURL()
                    },
                    allowClear: true,
                    minimumInputLength: 0,
                    placeholder: "Seleccione escuela profesional"
                });


                $(".select2-areacareers").on("change", function () {

                    var id = $(this).val();

                    if (id === _app.constants.guid.empty) {
                        select.plans.empty();
                        select.programs.empty();
                        select.cicles.empty();
                    } else {
                        select.cicles.empty();
                        //select.plans.empty();
                        select.plans.load2($(this).val());
                        select.programs.load($(this).val());
                    }
                });


            }
        },
        plans: {
            init: function () {
                $(".select2-plans").select2({
                    minimumResultsForSearch: -1,
                    placeholder: "Seleccione un plan",
                    disabled: true
                });

                $(".select2-plans").on("change", function () {
                    var id = $(this).val();

                    if (id === _app.constants.guid.empty) {
                        //select.programs.empty();
                        select.cicles.empty();
                    } else {
                        //select.programs.load($(this).val());
                        select.cicles.load($(this).val());
                    }
                });
            },
            load: function (programId) {
                $(".select2-plans").empty();
                $.ajax({
                    url: `/planes-estudio/${programId}/programas-academicos/get`.proto().parseURL()
                    //url: `/planes2?careerId=${careerId}`.proto().parseURL()
                }).done(function (data) {

                    //data.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $(".select2-plans").select2({
                        minimumResultsForSearch: -1,
                        data: data,
                        disabled: false
                    }).trigger("change");

                });
            },
            load2: function (careerId) {
                $(".select2-plans").empty();

                $.ajax({
                    url: `/planes2?careerId=${careerId}`.proto().parseURL()
                    //url: `/planes2?careerId=${careerId}`.proto().parseURL()
                }).done(function (data) {

                    data.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $(".select2-plans").select2({
                        minimumResultsForSearch: -1,
                        data: data,
                        disabled: false
                    }).trigger("change");

                });
            },
            empty: function () {
                $(".select2-plans").empty();
                $(".select2-plans").select2({
                    placeholder: "Seleccione un plan",
                    disabled: true
                });
            }
        },
        programs: {
            init: function () {
                $(".select2-tprograms").select2({
                    minimumResultsForSearch: -1,
                    placeholder: "Seleccione un programa",
                    disabled: true
                });
                $(".select2-tprograms").on("change", function () {
                    var id = $(this).val();

                    if (id === _app.constants.guid.empty) {
                        select.plans.load2($(".select2-areacareers").val());
                        select.cicles.empty();
                    } else {
                        select.plans.load($(this).val());
                        select.cicles.load($(this).val());
                    }
                });
            },
            load: function (careerId) {
                //plans
                $(".select2-tprograms").empty();
                $(".select2-tprograms").prop("disabled", true);
                $.ajax({
                    url: `/carreras/${careerId}/programas/get`.proto().parseURL()
                    //url: `/programas-por-plan?planId=${planId}`.proto().parseURL()
                }).done(function (data) {

                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $(".select2-tprograms").select2({
                        minimumResultsForSearch: -1,
                        data: data.items,
                        disabled: false
                    }).trigger("change");
                });
            },
            empty: function () {
                $(".select2-tprograms").empty();
                $(".select2-tprograms").select2({
                    placeholder: "Seleccione un programa",
                    disabled: true
                });
            }
        },
        cicles: {
            init: function () {
                $(".select2-cyccles").select2({
                    minimumResultsForSearch: -1,
                    placeholder: "Seleccione un ciclo",
                    disabled: true
                });
            },
            load: function (planId) {
                $(".select2-cyccles").empty();
                $.ajax({
                    url: `/planes-estudio/${planId}/niveles/get`.proto().parseURL()
                }).done(function (data) {
                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $(".select2-cyccles").select2({
                        minimumResultsForSearch: -1,
                        data: data.items,
                        disabled: false
                    });
                });
            },
            empty: function () {
                $(".select2-cyccles").empty();
                $(".select2-cyccles").select2({
                    placeholder: "Seleccione un ciclo",
                    disabled: true
                });
            }
        }
    };

    var datatable = {
        courses: {
            object: null,
            options: {
                ajax: {
                    url: `/admin/notas/limpiarnotas/get-cursos`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.termId = $(".select2-terms").val();
                        data.careerId = $(".select2-areacareers").val();
                        data.curriculumId = $(".select2-plans").val();
                        data.academicProgramId = $(".select2-tprograms").val();
                        data.academicYear = $(".select2-cyccles").val();
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        data: "code",
                        title: "Código",
                        width: 80
                    },
                    {
                        data: "name",
                        title: "Curso"
                    },
                    {
                        data: "area",
                        title: "Área Curricular"
                    },
                    {
                        data: "program",
                        title: "Programa de Estudios"
                    },
                    {
                        data: "cycle",
                        title: "Ciclo",
                        width: 50
                    },
                    {
                        data: "type",
                        title: "Condición"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        width: 200,
                        orderable: false,
                        render: function (row) {
                            var tmp = "";
                            tmp += `<a href='/admin/notas/limpiarnotas/curso/${row.id}/periodo/${$(".select2-terms").val()}' class='btn btn-brand btn-sm m-btn m-btn--icon' title="Sílabo"><span><i class="la la-cog"></i><span>Detalles</span></span></a>`;
                            return tmp;
                        }
                    }
                ]
            },
            reload: function () {
                datatable.courses.object.ajax.reload();
            },
            init: function () {
                datatable.courses.object = $("#ajax_data").DataTable(datatable.courses.options);
            }
        }
    };

    var events = {
        onSearch: function () {
            $(".btn-search").on("click", function () {
                if (datatable.courses.object === null) {
                    datatable.courses.init();
                } else {
                    datatable.courses.reload();
                }
            });
        },
        init: function () {
            this.onSearch();
        }
    }

    return {
        init: function () {
            select.init();
            events.init();
        }
    }
}();

$(function () {
    CourseTable.init();
});