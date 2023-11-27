var report = function () {

    var chart = {
        byacademicrecord: {
            load: function () {
                mApp.block("#container_chart", { type: "loader", message: "Cargando..." });
                $.ajax({
                    url: "/registrosacademicos/reporte-solicitudes/por-usuario-encargado-chart",
                    type: "GET"
                })
                    .done(function (e) {
                        Highcharts.chart('chart', {
                            chart: {
                                type: 'column',
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
                                text: 'Reporte por Usuario Registro Académico'
                            },
                            credits: {
                                text: 'Fuente: AKDEMIC',
                                href: ''
                            },
                            subtitle: {
                                text: 'Click en las columnas para ver el detalle'
                            },
                            xAxis: {
                                type: 'category'
                            },
                            yAxis: {
                                title: {
                                    text: 'Cantidad de Solicitudes Asignadas'
                                }

                            },
                            legend: {
                                enabled: false
                            },
                            plotOptions: {
                                series: {
                                    borderWidth: 0,
                                    dataLabels: {
                                        enabled: true
                                    }
                                }
                            },

                            tooltip: {
                                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.0f}</b><br/>'
                            },
                            series: [
                                {
                                    name: "Usuario Asignado",
                                    colorByPoint: true,
                                    data : e.users
                                }

                            ],
                            drilldown: {
                                series : e.details
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
                chart.byacademicrecord.load();
            }
        },
        init: function () {
            chart.byacademicrecord.init();
        }
    };

    return {
        init: function () {
            chart.init();
        }
    };
}();

$(function () {
    report.init();
})