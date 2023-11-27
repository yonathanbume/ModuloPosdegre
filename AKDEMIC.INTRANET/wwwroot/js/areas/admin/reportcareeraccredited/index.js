var InitApp = function () {
    var datatable = {
        reports: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/admin/reporte-escuelas-acreditadas/datatable/get`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.careerId = $("#careers").val();
                    },
                },
                dom: 'Bfrtip',
                buttons: [
                    'csv', 'excel', 'pdf'
                ],
                pageLength: 20,
                orderable: [],
                columns: [
                    {
                        title: "Descripción",
                        data: "description"
                    },
                    {
                        title: "Escuela Profesional",
                        data: "career"
                    },
                    {
                        title: "Fecha de Inicio",
                        data: "startDate"
                    },
                    {
                        title: "Fecha de Fin",
                        data: "endDate"
                    }
                ],
            },
            init: function () {
                datatable.reports.object = $("#data-table").DataTable(datatable.reports.options);
            },
            reload: function () {
                datatable.reports.object.ajax.reload();
            }
        },
        init: function () {
            datatable.reports.init();
        }
    };
    var highchart = {
        lang: function () {
            Highcharts.setOptions({
                lang: {
                    contextButtonTitle: "Opciones",
                    viewFullscreen: "Ver en pantalla completa",
                    printChart: "Imprimir",
                    downloadPNG: "Descargar PNG",
                    downloadJPEG: "Descargar JPEG",
                    downloadPDF: "Descargar PDF",
                    downloadSVG: "Descargar SVG",
                    downloadCSV: "Descargar CSV",
                    downloadXLS: "Descargar XLS",
                    openInCloud: "Abrir editor online"
                }
            })
        },
        credits: {
            text: 'Fuente: AKDEMIC',
            href: ''
        },
        buttons: {
            contextButton: {
                menuItems: ["viewFullscreen", "printChart", "separator", "downloadPNG", "downloadJPEG", "downloadPDF", "downloadSVG", "separator", "downloadCSV", "downloadXLS"]
            }
        },
        loadReport: function () {
            $("#chart-report-container").html("");
            $("#chart-report-container").append(`<div id="chart-report" class="chart"></div>`);
            $.ajax({
                url: `/admin/reporte-escuelas-acreditadas/chart/get`,
                type: "GET",
                data: {
                    careerId: $("#careers").val(),
                }
            }).done(function (result) {
                var categoriesCount = result.categories.length;
                var heightChart = 350;
                if (categoriesCount > 15) {
                    heightChart = categoriesCount * 20;
                }
                Highcharts.chart('chart-report', {
                    chart: {
                        height: heightChart,
                        type: 'bar',
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
                        text: 'Número de Acreditaciones por Escuela Profesional'
                    },
                    subtitle: {
                        text: ""
                    },
                    credits: highchart.credits,
                    xAxis: {
                        categories: result.categories,
                        title: {
                            text: 'Escuela Profesional'
                        }
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Número de acreditaciones'
                        }
                    },
                    series: [{
                        type: 'column',
                        name: 'Cantidad',
                        data: result.data,
                        colorByPoint: true,
                        showInLegend: true
                    }],
                    exporting: {
                        showTable: true,
                        buttons: highchart.buttons
                    }
                })
            }).fail(function (error) {
                toastr.error(error.responseText, _app.constants.toastr.title.error);
            })
        },
        init: function () {
            highchart.lang();
            highchart.loadReport();
        }
    };
    var select2 = {
        init: function () {
            this.careers.init();
        },
        careers: {
            init: function () {
                select2.careers.load();
                select2.careers.events();
            },
            load: function () {
                $.ajax({
                    url: `/carreras/v2/get`.proto().parseURL()
                }).done(function (result) {
                    $("#careers").select2({
                        data: result.items
                    });
                });
            },
            events: function () {
                $("#careers").on('change', function () {
                    datatable.reports.reload();
                    highchart.loadReport();
                })
            }
        }      
    };
    return {
        init: function () {
            select2.init();
            highchart.init();
            datatable.init();
        }
    }
}();

$(function () {
    InitApp.init();
})