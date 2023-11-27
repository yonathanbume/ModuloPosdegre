var InitApp = function () {

    const lang = function () {
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
        });
    };

    const buttons = {
        contextButton: {
            menuItems: ["viewFullscreen", "printChart", "separator", "downloadPNG", "downloadJPEG", "downloadPDF", "downloadSVG", "separator", "downloadCSV", "downloadXLS"]
        }
    };

    var datatable = {
        students: {
            object: null,
            options: {
                data: {},
                serverSide: false,
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        title: 'Reporte de ingresantes por escuela',
                        exportOptions: {
                            columns: [0, 1]
                        }
                    },
                    {
                        extend: 'pdfHtml5',
                        title: 'Reporte de ingresantes por escuela',
                        exportOptions: {
                            columns: [0, 1]
                        }
                    }


                ],
                columns: [
                    {
                        data: "name",
                        title: "Escuela Profesional"
                    },
                    {
                        data: "count",
                        title: "Ingresantes"
                    }
                ]
            },
            init: function () {
                datatable.students.object = $("#student-table").DataTable(datatable.students.options);
            },
            reload: function (data) {
                datatable.students.object.clear().rows.add(data).draw()
            }
        },
        init: function () {
            datatable.students.init();
        }
    };

    var chart = {
        main: {
            load: function (data) {
                Highcharts.chart('career-chart', {
                    title: {
                        text: 'Ingresantes por Escuela Profesional'
                    },
                    chart: {
                        type: 'column'
                    },
                    xAxis: {
                        type: 'category'
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Cantidad'
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    credits: {
                        text: '',
                        href: ''
                    },
                    tooltip: {
                        pointFormat: 'Cantidad Ingresantes: <b>{point.y}</b>'
                    },
                    series: [data],
                    exporting: {
                        //showTable: true,
                        buttons: buttons
                    },
                });
            },
        }
    }

    var select = {
        init: function () {
            this.terms.init();
        },
        terms: {
            init: function () {
                $.ajax({
                    url: `/periodos-finalizados/get`.proto().parseURL()
                }).done(function (result) {

                    $("#term-select").select2({
                        data: result.items
                    });

                    $("#term-select").on('change', function () {
                        functions.initializeChart();
                    });

                    functions.initializeChart();
                });
            }
        }
    };

    var functions = {
        initializeChart: function () {
            mApp.block(".m-portlet__body", {
                message: "Cargando datos..."
            });

            $.ajax({
                url: `/registrosacademicos/reporte-ingresantes/consolidado/get`,
                type: "GET",
                data: {
                    termId: $("#term-select").val()
                }
            })
                .done(function (data) {
                    console.log(data);

                    chart.main.load(data.chart);
                    datatable.students.reload(data.table);

                    $("#data-container").removeClass("m--hide");
                })
                .always(function () {
                    mApp.unblock(".m-portlet__body");
                })
        }
    }

    return {
        init: function () {
            lang();
            datatable.init();
            select.init();
        }
    };
}();

$(function () {
    InitApp.init();
});