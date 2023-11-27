var indexcounter = function () {

    var data = {
        getNumber: function () {
            mApp.block("#container_number_request", {
                message : "Cargando datos.."
            });

            $.ajax({
                url: "/solicitudes/por-usuario",
                type: "get"
            })
                .done(function (e) {
                    $("#number_request").text(e);
                })
                .always(function () {
                    mApp.unblock("#container_number_request");
                });
        },
        getChart: function () {
            mApp.block("#container_chart", { type: "loader", message: "Cargando gráfico..." });
            $.ajax({
                url: "/solicitudes/por-usuario/chart",
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
                            text: 'Detalles de solicitudes'
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
                            showTable: false,
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
            data.getNumber();
            data.getChart();
        }
    };

    return {
        init: function () {
            data.init();
        }
    };
}();

$(function () {
    indexcounter.init();
});