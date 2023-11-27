var statistics = function () {

    var StudentId = $("#StudentId").val();

    var chart = {
        avergePerTerm: {
            load: function () {
                mApp.block("#average_term_portlet", {
                    message: "Cargando gráficos...",
                    size: 'xl'
                });

                $.ajax({
                    url: `/academico/alumnos/informacion/${StudentId}/estadisticas/promedio-periodo-grafico`,
                    type : "GET"
                })
                    .done(function (e) {
                        Highcharts.chart('average_term', {
                            title: {
                                text: 'Promedio por Periodo'
                            },
                            subtitle: {
                                text: ''
                            },
                            yAxis: {
                                title: {
                                    text: 'Rango de notas'
                                }
                            },
                            xAxis: {
                                categories: e.terms
                            },
                            legend: {
                                enabled: false
                            },
                            series: [
                                {
                                    name: 'Promedio',
                                    data: e.averages
                                }
                            ],
                        });

                        mApp.unblock("#average_term_portlet");
                    })
              
            },
            init: function () {
                this.load();
            }
        },
        init: function () {
            chart.avergePerTerm.init();
        }
    };

    var events = {
        competencies: {
            load: function () {

                mApp.block("#student_statistics_competencies_detailed", {
                    message: "Cargando datos...",
                    size: 'xl'
                });

                $.ajax({
                    url: `/academico/alumnos/informacion/${StudentId}/estadisticas/competencias-detallado`,
                    type : "GET"
                })
                    .done(function (e) {
                        $("#student_statistics_competencies_detailed").html(e);
                    })
            },
            init: function () {
                events.competencies.load();
            }
        },
        competencies_chart: {
            load: function () {
                mApp.block("#student_competencies_chart", {
                    message: "Cargando gráficos...",
                    size: 'xl'
                });

                $.ajax({
                    url: `/academico/alumnos/informacion/${StudentId}/estadisticas/competencias-graficos`,
                    type: "GET"
                })
                    .done(function (e) {
                        $("#student_competencies_chart").html(e);
                    })
            },
            init: function () {
                events.competencies_chart.load();
            }
        },
        init: function () {
            events.competencies.init();
            events.competencies_chart.init();
        }
    }

    return {
        init: function () {
            chart.init();
            events.init();
        }
    }
}();

$(() => {
    statistics.init();
});

