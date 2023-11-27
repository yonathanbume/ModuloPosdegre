var report_title_modality = function () {

    var datepicker = function () {
        $("#dateStartFilter").datepicker();
        $("#dateEndFilter").datepicker();

        $("#dateStartFilter").datepicker({
            clearBtn: true,
            orientation: "bottom",
            format: _app.constants.formats.datepicker
        }).on("changeDate", function (e) {
            $("#dateEndFilter").datepicker("setStartDate", moment(e.date).toDate());

        });

        $("#dateEndFilter").datepicker({
            clearBtn: true,
            orientation: "bottom",
            format: _app.constants.formats.datepicker
        }).on("changeDate", function (e) {
            $("#dateStartFilter").datepicker("setEndDate", e.date);


        });
    };

    var searchByDate = function () {
        $("#btn-search-dates").on('click', function (e) {
            e.preventDefault();
            var dateStartVal = $("#dateStartFilter").val();
            var dateEndVal = $("#dateEndFilter").val();
            if (dateStartVal === null || dateEndVal === null) {
                return false;
            }
            if (dateStartVal === "" || dateEndVal === "") {
                return false;
            }
            else {
                loadChartReportTitleModality();
            }

        });
    };
    var loadChartReportTitleModality = function () {
       $.ajax({
            type: 'GET',
            url: `/admin/reporte-alumnos-titulados-segun-modalidad/reporte-titulados-segun-modalidad`.proto().parseURL(),
            processData: false,
            contentType: false,
            data: {
                startDate: $("#dateStartFilter").val(),
                endDate: $("#dateEndFilter").val()
            },
            success: function (data) {
                var sum = 0;
                var categoriesCareer = [];
                var dataCareer = [];
                if (data != undefined && data != null) {
                    for (var i = 0; i < data.length; i++) {
                        categoriesCareer.push(data[i].year);
                        dataCareer.push(data[i].titleCount);
                        sum += data[i].titleCount;
                    }
                }
                Highcharts.chart('chart_div_report_title_modality', {
                    title: {
                        text: 'Número de estudiantes titulados por modalidad de trámite por título profesional'
                    },

                    subtitle: {
                        text: `Total :${sum}`
                    },

                    xAxis: {
                        categories: categoriesCareer
                    },
                    yAxis: {
                        title: {
                            text: ''
                        }
                    },
                    credits: {
                        text: '',
                        href: ''
                    },
                    series: [{
                        type: 'column',
                        colorByPoint: true,
                        data: dataCareer,
                        showInLegend: false
                    }],
                    exporting: {
                        enabled: true
                    },
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

            },
            error: function () {
                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
            },
            complete: function () {
                $("#btnSend").removeLoader();
                mApp.unblock("#chart_div_report_bachelor", { type: "loader", message: "Cargando..." });
            }
        });

    };
    var initializer = function () {
        loadChartReportTitleModality();
    };

    return {
        load: function () {
            datepicker();
            initializer();
            searchByDate();
        }
    };
}();
$(function () {
    report_title_modality.load();
});