var StudentGradeReport = function () {
    var careers = null;
    var courses = null;
    var chartGeneral = null;
    var chartGeneral2 = null;

    var init = function () {
        google.charts.load("current", { "packages": ["bar"] });
        google.charts.load("current", { packages: ["corechart", "bar"] });

        $(".save-pdf").on('click', function () {
            var id = $(this).data("id");
            var doc = new jsPDF("p", "mm", "a4");

            var width = doc.internal.pageSize.width;
            var height = doc.internal.pageSize.height;

            if (id == 1) {
                var input = document.getElementById('columnchart_material_1');

                var inputH = height / input.clientHeight;
                var inputW = width / input.clientWidth;

                var ratio = inputW > inputH ? inputH : inputW;

                doc.addImage(chartGeneral.getImageURI(), 0, 0, input.clientWidth * ratio, input.clientHeight * ratio);
                doc.save('chart1.pdf');
            } else {
                var input = document.getElementById('columnchart_material_2');

                var inputH = height / input.clientHeight;
                var inputW = width / input.clientWidth;

                var ratio = inputW > inputH ? inputH : inputW;
                doc.addImage(chartGeneral2.getImageURI(), 0, 0, input.clientWidth * ratio, input.clientHeight * ratio);
                doc.save('chart2.pdf');
            }
        });
    }

    var chart = {
        init1: function () {
            var cid = $("#career-filter-1").val();
            var pid = $("#program-filter-1").val();
            var termId = $("#term-filter-1").val();

            $("#btnLoad").show();
            $("#btnLoad").addLoader();
            $.ajax({
                url: "/admin/reporte-notas/carrera".proto().parseURL(),
                data: {
                    cid: cid,
                    pid: pid,
                    tid: termId
                }
            }).done(function (result) {
                $("#btnLoad").removeLoader();
                $("#load").hide();
                $("#chart-filter-1").show();
                var dataGroup = [
                    ["Periodo", "[0-5]", "[5-10]", "[10-15]", "[15-20]"]
                ];

                result.forEach(function (e, index) {
                    dataGroup.push(e);
                });
                var data = google.visualization.arrayToDataTable(dataGroup);

                var options = {
                    //chart: {
                        title: "Distribución de Estudiantes por Promedio Ponderado",
                        subtitle: "Clasificados por Rango de Nota y Periodo Académico",
                    //},
                    vAxes: {
                        0: {
                            title: '',
                            viewWindow: {
                                min: 0
                            },
                            gridlines: {
                                count: 10
                            }
                        }
                    }
                };
               
                var chart = new google.visualization.ColumnChart(document.getElementById('columnchart_material_1'));
                chart.draw(data, options);
                chartGeneral = chart;
            });
        },
        init2: function () {
            var cid = $("#career-filter-2").val();
            var ccid = $("#course-filter-2").val();
            var termId = $("#term-filter-2").val();
            var curriculumId = $("#curriculums-filter-2").val();

            $("#btnLoad-2").show();
            $("#btnLoad-2").addLoader();
            $.ajax({
                url: "/admin/reporte-notas/curso".proto().parseURL(),
                type: "GET",
                dataType: "JSON",
                data: {
                    tid: termId,
                    cid: cid,
                    ccid: ccid,
                    planId: curriculumId
                }
            }).done(function (result) {
                $("#btnLoad-2").removeLoader();
                $("#load-2").hide();
                $("#columnchart_material_2").show();
                mApp.unblockPage();
                var dataGroup = [
                    ["Periodo", "[0-5]", "[5-10]", "[10-15]", "[15-20]"]
                ];

                result.forEach(function (e, index) {
                    dataGroup.push(e);
                });

                var data = google.visualization.arrayToDataTable(dataGroup);

                var options = {
                    title: "Distribución de Estudiantes por Promedio Final de Curso",
                    subtitle: "Clasificados por Rango de Nota y Periodo Académico",
                    vAxes: {
                        0: {
                            title: '',
                            viewWindow: {
                                min: 0,
                            },
                            gridlines: {
                                count: 10,
                            }
                        }
                    },
                };
                var chart = new google.visualization.ColumnChart(document.getElementById('columnchart_material_2'));
                chart.draw(data, options);
                chartGeneral2 = chart;
            });
        }
    }

    var select2 = {
        init: function () {
            $("#btnLoad").hide();
            $("#chart-filter-1").hide();

            $("#btnLoad-2").hide();
            $("#columnchart_material_2").hide();

            $("#term-filter-1").on("change", function () {
                if ($("#program-filter-1").val() != null && $("#program-filter-1").val() != "") {
                    chart.init1();
                }
            });

            $("#faculty-filter-1").on("change", function () {
                select2.careers.filter1($(this).val());
            });

            $("#career-filter-1").on("change", function () {
                //chart.init1();
                select2.academicPrograms.filter1($(this).val());
            });

            $("#program-filter-1").on("change", function () {
                chart.init1();
            });


            $("#term-filter-2").on("change", function () {
                if ($("#course-filter-2").val() != null && $("#course-filter-2").val() != "") {
                    chart.init2();
                }
            });

            $("#faculty-filter-2").on("change", function () {
                select2.careers.filter2($(this).val());
            });

            $("#career-filter-2").on("change", function () {
                select2.curriculums.filter2($(this).val());
            });
            $("#curriculums-filter-2").on("change", function () {
                select2.courses.filter2($(this).val());
            });
            $("#course-filter-2").on("change", function () {
                chart.init2();
            });

            $.ajax({
                url: "/periodos/get".proto().parseURL()
            }).done(function (data) {
                $(".select2-terms").select2({
                    data: data.items
                });

                if (data.selected !== null) {
                    $(".select2-terms").val(data.selected);
                    $(".select2-terms").trigger("change.select2");
                }
            });

            $.ajax({
                url: ("/facultades/get").proto().parseURL()
            }).done(function (result) {
                $(".select2-faculties").select2({
                    data: result.items
                }).trigger("change");
            });
        },
        careers: {
            filter1: function (facultyId) {
                $.ajax({
                    url: ("/carreras/get?fid=" + facultyId).proto().parseURL()
                }).done(function (result) {
                    $("#career-filter-1").empty();
                    $("#career-filter-1").select2({
                        data: result.items,
                        placeholder: "Escuela"
                    }).trigger("change");
                });
            },
            filter2: function (facultyId) {
                $.ajax({
                    url: ("/carreras/get?fid=" + facultyId).proto().parseURL()
                }).done(function (result) {
                    $("#career-filter-2").empty();
                    $("#career-filter-2").select2({
                        data: result.items,
                        placeholder: "Escuela"
                    }).trigger("change");
                });
            }
        },
        curriculums: {
            filter2: function (careerId) {
                if (careerId != null && careerId != _app.constants.guid.empty) {
                    $.ajax({
                        url: ("/carreras/" + careerId + "/planestudio/get").proto().parseURL()
                        //url: ("/carreras/" + careerId + "/cursos/get").proto().parseURL()
                    }).done(function (result) {
                        $("#curriculums-filter-2").empty();
                        $("#curriculums-filter-2").select2({
                            data: result.items,
                            placeholder: "Planes"
                        }).trigger("change");
                    });
                } else {
                    $("#curriculums-filter-2").empty();
                    $("#curriculums-filter-2").select2({
                        placeholder: "Planes"
                    }).trigger("change");
                }
            }
        },
        courses: {
            filter2: function (planId) {
                if (planId != null && planId != _app.constants.guid.empty) {
                    $.ajax({
                        url: ("/curriculum/" + planId + "/cursos/get").proto().parseURL()
                        //url: ("/carreras/" + careerId + "/cursos/get").proto().parseURL()
                    }).done(function (result) {
                        $("#course-filter-2").empty();
                        $("#course-filter-2").select2({
                            data: result.items,
                            placeholder: "Curso"
                        });
                        if (result.items.length > 0) {
                            chart.init2();
                        }
                    });
                } else {
                    $("#course-filter-2").empty();
                    $("#course-filter-2").select2({
                        placeholder: "Curso"
                    });
                }
            }
        },
        academicPrograms: {
            filter1: function (careerId) {
                $.ajax({
                    url: ("/carreras/" + careerId + "/programas/get").proto().parseURL()
                }).done(function (result) {
                    $("#program-filter-1").empty();
                    $("#program-filter-1").select2({
                        data: result.items,
                        placeholder: "Programa"
                    });
                    if (result.items.length > 0) {
                        chart.init1();
                    }

                });
            }
        },
    }

    return {
        init: function () {
            init();
            select2.init();
        }
    };
}();

$(function () {
    StudentGradeReport.init();
});