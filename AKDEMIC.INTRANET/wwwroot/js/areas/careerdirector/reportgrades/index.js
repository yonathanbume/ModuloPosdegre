var InitApp = function () {
    var datatable;
    var exportFiles = function () {
        $("#download_excel").on("click", function () {
            var url = `/director-carrera/notas-por-curso/reporte-excel?termId=${$("#termId").val()}&courseId=${$('#courseId').val()}&curriculumId=${$("#curriculumId").val()}&academicYear=${$("#academicyear-select").val()}`;
            var $btn = $(this);
            $btn.addLoader();
            window.open(url,"_blank");
            $btn.removeLoader();
            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
        });
    };
    var select = {
        term: {
            init: function () {
                $.ajax({
                    url: `/periodos/get`.proto().parseURL()
                })
                    .done(function (data) {
                        $("#termId").select2({
                            placeholder: "Seleccione una periodo académico",
                            minimumResultsForSearch: 10,
                            data: data.items,
                            selected: data.selected
                        });
                    });
            }
        },
        faculty: {
            init: function () {
                $.ajax({
                    url: `/facultades/get`.proto().parseURL()
                }).done(function (data) {
                    $("#facultyId").select2({
                        data: data.items,
                        placeholder: "Seleccione una facultad",
                        minimumResultsForSearch: 10
                    });

                    $("#facultyId").val(null).trigger("change");

                    $("#facultyId").on("change", function () {
                        select.career.load($("#facultyId").val());
                    });
                });
            }
        },
        career: {
            init: function () {
                $("#careerId").select2({
                    placeholder: "Seleccione una escuela",
                    disabled: true
                });

                $("#careerId").on("change", function () {
                    select.curriculum.load($("#careerId").val());
                });            },
            load: function (faculty) {
                $.ajax({
                    url: `/carreras/get?fid=${faculty}`.proto().parseURL(),
                }).done(function (data) {
                    $("#careerId").empty();

                    $("#careerId").select2({
                        placeholder: "Seleccione una escuela",
                        data: data.items,
                        disabled: false,
                        minimumResultsForSearch: 10
                    });

                    if (data.items.length > 0) {
                        $("#careerId").trigger("change");
                    } else {
                        select.curriculum.empty();
                        select.academicyear.empty();
                        select.course.empty();
                    }
                });
            }
        },
        curriculum: {
            init: function () {
                $("#curriculumId").select2({
                    placeholder: "Seleccione un plan",
                    disabled: true
                });

                $("#curriculumId").on("change", function () {
                    select.academicyear.load($("#curriculumId").val());
                });
            },
            load: function (career) {
                $.ajax({
                    url: `/carreras/${career}/planestudio/get`.proto().parseURL(),
                }).done(function (data) {
                    $("#curriculumId").empty();

                    $("#curriculumId").select2({
                        placeholder: "Seleccione un plan de estudios",
                        data: data.items,
                        disabled: false
                    });

                    if (data.items.length > 0) {
                        $("#curriculumId").trigger("change");
                    } else {
                        select.course.empty();
                        select.academicyear.empty();
                    }
                });
            },
            empty: function () {
                $("#curriculumId").empty();
                $("#curriculumId").select2({
                    placeholder: "Seleccione un plan",
                    disabled: true
                });
            }
        },
        academicyear: {
            init: function () {
                $("#academicyear-select").select2({
                    placeholder: "Seleccione un ciclo",
                    disabled: true
                });

                $("#academicyear-select").on("change", function () {
                    select.course.load($("#curriculumId").val(), $("#academicyear-select").val());
                });
            },
            load: function (curriculumid) {
                $.ajax({
                    url: `/planes-estudio/${curriculumid}/niveles/get`.proto().parseURL(),
                }).done(function (data) {
                    $("#academicyear-select").empty();
                    $("#academicyear-select").select2({
                        placeholder: "Seleccione un ciclo",
                        data: data.items,
                        disabled: false,
                        minimumResultsForSearch: -1
                    });

                    if (data.items.length > 0) {
                        $("#academicyear-select").trigger("change");
                    } else {
                        select.course.empty();
                    }
                });
            },
            empty: function () {
                $("#academicyear-select").empty();
                $("#academicyear-select").select2({
                    placeholder: "Seleccione un curso",
                    disabled: true
                });
            }
        },
        course: {
            init: function () {
                $("#courseId").select2({
                    placeholder: "Seleccione un curso",
                    disabled: true
                });
            },
            load: function (curriculumid, academicyear) {
                $.ajax({
                    url: `/planes-estudio/${curriculumid}/niveles/${academicyear}/get`.proto().parseURL(),
                }).done(function (data) {

                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });

                    $("#courseId").empty();
                    $("#courseId").select2({
                        placeholder: "Seleccione un curso",
                        data: data.items,
                        disabled: false
                    });
                });
            },
            empty: function () {
                $("#courseId").empty();
                $("#courseId").select2({
                    placeholder: "Seleccione un curso",
                    disabled: true
                });
            }
        },
        init: function () {
            select.term.init();
            select.faculty.init();
            select.career.init();
            select.curriculum.init();
            select.academicyear.init();
            select.course.init();


            $(".btn-find").on("click", function () {
                if ($("#courseId").val() == null || $("#courseId").val() == "") {
                    toastr.error("Debe seleccionar un curso", _app.constants.toastr.title.error);
                    return false;
                }

                loadChartByCourses();
            });

        }
    };

    var loadChartByCourses = function () {
        mApp.block(".m-portlet__body", "Cargando...");
        $.ajax({
            url: ("/director-carrera/notas-por-curso/get").proto().parseURL(),
            data: {
                termId: $("#termId").val(),
                courseId: $('#courseId').val(),
                curriculumId: $("#curriculumId").val(),
                academicYear: $("#academicyear-select").val()
            }
        })
            .done(function (data) {
                var arraycumulative = [];
                //for 
                $.each(data.data, function (index, value) {
                    if (index == 0) {
                        arraycumulative[index] = (value / data.totalstudents * 100);
                    } else {
                        arraycumulative[index] = arraycumulative[index - 1] + (value / data.totalstudents * 100);
                    }
                });

                Highcharts.chart('container', {
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: 'Reportes de notas por curso'
                    },
                    tooltip: {
                        formatter: function () {
                            return '<b>' + this.x + ' ptos</b>: ' + this.y + ' estudiantes.<br/><sapn>' + (this.y / data.totalstudents * 100).toFixed(2) + '% del total</span><br/><sapn> Percentil: ' + arraycumulative[this.point.index].toFixed(2) + '%</span>';
                        }
                    },
                    yAxis: {
                        title: {
                            enabled: true,
                            text: 'Cant. Estudiantes',
                            style: {
                                fontWeight: 'normal'
                            }
                        }
                    },
                    xAxis: {
                        categories: data.categories,
                        labels: {
                            formatter: function () {
                                return this.value + " ptos.";
                            }
                        }
                    },
                    series: [{
                        name: 'Notas ',
                        data: data.data
                    }]
                });
                $(".highcharts-Average-1").text(" " + data.average + " ptos.");
                $(".highcharts-Median-1").text(" " + data.median + " ptos.");
                $(".highcharts-Deviation-1").text(" " + data.standarDeviation + " ptos.");
                $(".highcharts-Under-1").text(" " + data.undermedian + " %");
                $(".highcharts-Over-1").text(" " + data.overmedian + " %");

                if (datatable != null && datatable != undefined) {
                    datatable.clear();
                    datatable.destroy();
                }
                var count = 0;
                datatable = $("#tbl-data").DataTable({ serverSide: false });
                $.each(data.datatable, function (index, value) {
                    count++;

                    datatable
                        .row
                        .add([count, value.name, value.course, value.sectionCode, value.grade])
                        .draw()
                        .node();
                });

                $(".portlet-details").removeClass("m--hide");
                $(".highcharts-figure").removeClass("m--hide");

            }).fail(function (e) {
                toastr.info(e.responseText, "Información!");
            })
            .always(function () {
                mApp.unblock(".m-portlet__body");
            });
    };

    return {
        init: function () {
            select.init();
            exportFiles();
        }
    };
}();

$(function () {
    InitApp.init();
});