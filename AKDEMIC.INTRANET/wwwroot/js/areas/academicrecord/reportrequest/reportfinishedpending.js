var report = function () {

    var chart = {
        finishedpending: {
            load: function () {
                mApp.block("#container_chart", { type: "loader", message: "Cargando..." });
                $.ajax({
                    url: "/registrosacademicos/reporte-solicitudes/finalizados-pendientes-chart?month=" + $("#month").val(),
                    type: "GET"
                })
                    .done(function (e) {
                        var data = [];

                        data.push({
                            name: "Finalizados / Aceptados",
                            y : e.finished
                        });

                        data.push({
                            name: "Pendientes",
                            y: e.pending
                        });

                        Highcharts.chart('chart', {
                            title: {
                                text: 'Solicitudes Pendientes y Finalizadas'
                            },
                            chart: {
                                plotBackgroundColor: null,
                                plotBorderWidth: null,
                                plotShadow: false,
                                type: 'pie',
                                events: {
                                    exportData: function () {
                                        var total = $("body").find(".highcharts-data-table");
                                        var current = $(".m-portlet").find(".highcharts-data-table");
                                        var currentLength = current.length;
                                        for (currentLength; currentLength < total.length; currentLength++) {
                                            $(total[currentLength]).remove();
                                        }
                                    }
                                }
                            },
                            tooltip: {
                                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                            },
                            plotOptions: {
                                pie: {
                                    allowPointSelect: true,
                                    cursor: 'pointer',
                                    dataLabels: {
                                        enabled: true,
                                        format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                                    }
                                }
                            },
                            series: [{
                                name: 'Cantidad',
                                colorByPoint: true,
                                data: data
                            }],
                            exporting: {
                                showTable: true,
                                buttons: {
                                    contextButton: {
                                        menuItems: ["viewFullscreen", "printChart", "separator", "downloadPNG", "downloadJPEG", "downloadPDF", "downloadSVG", "separator", "downloadCSV", "downloadXLS"]
                                    }
                                }
                            }
                        });
                    })
                    .fail(function () {
                        toastr.error("Error al cargar el grafico.", "Error!");
                    })
                    .always(function () {
                        mApp.unblock("#container_chart");
                    });
            },
            init: function () {
                chart.finishedpending.load();
            }
        },
        init: function () {
            chart.finishedpending.init();
        }
    };

    var select = {
        month: {
            events: {
                onChange: function () {
                    $("#month").on("change", function () {
                        $(".highcharts-data-table").empty();
                        chart.finishedpending.load();
                    });
                },
                init: function () {
                    select.month.events.onChange();
                }
            },
            init: function () {
                select.month.events.init();
                $("#month").select2();
            }
        },
        init: function () {
            select.month.init();
        }
    };

    return {
        init: function () {
            select.init();
            chart.init();
        }
    };
}();

$(function () {
    report.init();
})