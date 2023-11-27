var InitApp = function () {
    var select = {
        term: {
            init: function () {
                this.events();
                this.load();
            },
            load: function () {
                $.ajax({
                    url: "/periodos/get".proto().parseURL()
                }).done(function (data) {
                    $("#term-select").select2({
                        data: data.items
                    });

                    if (data.selected !== null) {
                        $("#term-select").val(data.selected).trigger("change");
                    }
                });
            },
            events: function () {
                $("#term-select").on("change", function (e) {
                    //datatable.sections.reload();
                });
            }
        },
        career: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: `/carreras/get`.proto().parseURL()
                })
                    .done(function (data) {
                        $("#career-select").select2({
                            placeholder: "Seleccione una escuela",
                            data: data.items,
                            minimumResultsForSearch: 10,
                        });
                        select.curriculum.load($("#career-select").val());
                    });
            },
            events: function () {
                $("#career-select").on("change", function () {
                    var careerId = $("#career-select").val();
                    select.curriculum.load(careerId);
                });
            }
        },
        curriculum: {
            init: function () {
                $("#curriculum-select").select2({
                    placeholder: "Seleccione un Plan",
                    disabled: true
                });

                this.events();
            },
            load: function (careerId) {
                $.ajax({
                    url: `/planes-estudio/${careerId}/get`.proto().parseURL()
                }).done(function (data) {
                    $("#curriculum-select").empty();
                    $("#curriculum-select").select2({
                        placeholder: "Seleccione un Plan",
                        data: data.items,
                        minimumResultsForSearch: 10,
                        disabled: false
                    });
                    select.academicyear.load($("#curriculum-select").val());
                });
            },
            events: function () {
                $("#curriculum-select").on("change", function () {
                    var curriculumId = $("#curriculum-select").val();
                    select.academicyear.load(curriculumId);
                });
            }
        },
        academicyear: {
            init: function () {
                $("#academicyear-select").select2({
                    placeholder: "Seleccione un Ciclo"
                });
            },
            load: function (curriculumId) {
                $.ajax({
                    url: `/planes-estudio/${curriculumId}/niveles/get`.proto().parseURL()
                }).done(function (data) {
                    $("#academicyear-select").empty();
                    $("#academicyear-select").select2({
                        placeholder: "Seleccione un Ciclo",
                        data: data.items,
                        minimumResultsForSearch: 10,
                        disabled: false
                    });
                });
            }
        },
        init: function () {
            this.academicyear.init();
            this.curriculum.init();
            this.term.init();
            this.career.init();
        }
    };

    var datatable = {
        sections: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/reporte/curso-periodo/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.termId = $("#term-select").val();
                        data.curriculum = $("#curriculum-select").val();
                        data.academicyear = $("#academicyear-select").val();
                    }
                },
                pageLength: 50,
                columns: [
                    {
                        data: "curriculum",
                        title: "Plan"
                    },
                    {
                        data: "course",
                        title: "Curso"
                    },
                    {
                        data: "sections",
                        title: "Secciones",
                    },
                    {
                        data: null,
                        title: "Opciones",
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var url = `/reporte/curso-periodo/detalle/${data.id}/${$("#term-select").val()}`.proto().parseURL();
                            template += `<a class="btn btn-brand m-btn btn-sm m-btn--icon btn-manage" href="${url}"><span><i class="la la-gear"></i><span>Detalle</span></span></a> `;
                            return template;
                        }
                    }
                ],
            },
            reload: function () {
                if (this.object == null) {
                    this.object = $("#data-table").DataTable(this.options);
                } else {
                    this.object.ajax.reload();
                }
            }
        }
    };

    var search = {
        init: function () {
            $("#btn-show").click(function () {
                datatable.sections.reload();
            });

            $("#search").doneTyping(function () {
                datatable.sections.reload();
            });
        }
    }

    return {
        init: function () {
            select.init();
            search.init();
        }
    }
}();

$(function () {
    InitApp.init();
});

