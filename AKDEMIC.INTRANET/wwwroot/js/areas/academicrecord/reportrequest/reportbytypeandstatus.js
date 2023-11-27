var report = function () {
    var chart = {
        bytype: {
            load: function () {
                mApp.block("#container_chart", { type: "loader", message: "Cargando..." });
                $.ajax({
                    url: "/registrosacademicos/reporte-solicitudes/por-tipo-chart?year="+$("#year").val(),
                    type: "GET"
                })
                    .done(function (e) {
                        Highcharts.chart('chart', {
                            chart: {
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
                            title: {
                                text: 'Cantidad de Trámites por Tipo'
                            },
                            subtitle: {
                                text: ''
                            },
                            credits: {
                                text: 'Fuente: AKDEMIC',
                                href: ''
                            },
                            xAxis: {
                                title: {
                                    text: "Tipo de Trámite"
                                },
                                type: 'category'
                            },
                            yAxis: {
                                title: {
                                    text: 'Cantidad Total'
                                }
                            },
                            series: [{
                                type: 'column',
                                name: 'Cantidad de Trámites',
                                colorByPoint: true,
                                data: e,
                                showInLegend: false
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
                chart.bytype.load();
            }
        },
        bystatus: {
            load: function () {
                mApp.block("#container_chart_status", { type: "loader", message: "Cargando..." });
                $.ajax({
                    url: "/registrosacademicos/reporte-solicitudes/por-estado-chart?year=" + $("#year").val(),
                    type: "GET"
                })
                    .done(function (e) {
                        Highcharts.chart('chart_status', {
                            chart: {
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
                            title: {
                                text: 'Cantidad de Trámites por Estado'
                            },
                            subtitle: {
                                text: ''
                            },
                            credits: {
                                text: 'Fuente: AKDEMIC',
                                href: ''
                            },
                            xAxis: {
                                title: {
                                    text: "Estado del Trámite"
                                },
                                type: 'category'
                            },
                            yAxis: {
                                title: {
                                    text: 'Cantidad Total'
                                }
                            },
                            series: [{
                                type: 'column',
                                name: 'Cantidad de Trámites',
                                colorByPoint: true,
                                data: e,
                                showInLegend: false
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
                        mApp.unblock("#container_chart_status");
                    });
            },
            init: function () {
                chart.bystatus.load();
            }
        },
        init: function () {
            chart.bytype.init();
            chart.bystatus.init();
        }
    };

    var select = {
        year: {
            events: {
                init: function () {
                    $("#year").on("change", function () {
                        $(".highcharts-data-table").empty();
                        chart.bytype.load();
                        chart.bystatus.load();
                    });
                }
            },
            init: function () {
                select.year.events.init();
                $("#year").select2({
                    minimumResultsForSearch: -1
                });
            }
        },
        init: function () {
            select.year.init();
        }
    };

    return {
        init: function () {
            select.init();
            chart.init();
        }
    }
}();

$(function () {
    report.init();
})