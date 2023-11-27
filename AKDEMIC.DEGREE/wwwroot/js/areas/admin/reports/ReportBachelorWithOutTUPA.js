var report_bachelor_without_request = function () {

    var selects = {
        academic_programs: function () {

            $("#careers").on('change', function () {

                var careerListSelect = $(this).val();
                var formData = new FormData();
                for (var i = 0; i < careerListSelect.length; i++) {
                    formData.append(`Select2Data[${i}]`, careerListSelect[i]);
                }
                $.ajax({
                    type: 'POST',
                    url: `/admin/reporte-alumnos-egresados-sin-solicitud-bachiller/programas-academicos`.proto().parseURL(),
                    data: formData,
                    contentType: false,
                    processData: false
                }).done(function (data) {
                    $("#programs").empty();
                    $("#programs").select2({
                        placeholder: "Todas",
                        multiple: true,
                        data: data
                    });
                });

            });



        },
        faculties: function () {
            $.ajax({
                type: 'GET',
                url: `/admin/reporte-alumnos-egresados-sin-solicitud-bachiller/facultades/get`.proto().parseURL()
            }).done(function (data) {
                $("#faculties").select2({
                    placeholder: "Todas",
                    data: data.items,
                    multiple: true
                });
                $('#faculties').val(facultyList).trigger('change');

                //if ($("#FacultyIdSe").length) {
                //    var id = $("#FacultyIdSe").val();
                //    $("#faculties").val(id).trigger('change');                    
                //} else {
                //    $("#faculties").removeAttr("disabled");
                //}
                //$("#faculties").trigger('change');
            });
        },
        careers: function () {
            $("#faculties").on('change', function () {

                var facultyListSelect = $(this).val();
                var formData = new FormData();
                for (var i = 0; i < facultyListSelect.length; i++) {
                    formData.append(`Select2Data[${i}]`, facultyListSelect[i]);
                }

                $.ajax({
                    type: 'POST',
                    url: `/admin/reporte-alumnos-egresados-sin-solicitud-bachiller/carreras`.proto().parseURL(),
                    data: formData,
                    contentType: false,
                    processData: false,
                }).done(function (data) {
                    //$('.disabledCheckboxes').prop("disabled", true);
                    $("#careers").empty();
                    $("#careers").select2({
                        placeholder: "Todas",
                        data: data,
                        multiple: true
                    });
                    $('#careers').val(careerList).trigger('change');
                });

            });
        }
    };

    var validate = function () {
        $("#frmReport").validate({
            submitHandler: function (form) {
                $("#btnSend").addLoader();
                loadChartReportBachelorWithoutRequest();
            }
        });

    };

    var loadChartReportBachelorWithoutRequest = function () {
        //google.charts.load('current', {
        //    packages: ['corechart', 'bar']
        //});
        //google.charts.setOnLoadCallback(LoadData);

        //function PopulationChartReportBachelorWihtout(data1) {
        //    var dataArray = [
        //        ['Filtro', 'Cantidad']
        //    ];

        //    dataArray.push(['Con trámite solicitado', data1.has]);  //           
        //    dataArray.push(['Sin trámite solicitado', data1.notHave]);

        //    var data = google.visualization.arrayToDataTable(dataArray);
        //    var options = {
        //        title: `Trámites realizados vs Trámites pendientes`,
        //        height: 400,
        //        width: 1400,
        //        colors: ['#43A047', '#D32F2F'],
        //        titleFontSize: 20,
        //        is3D: true
        //    };
        //    var chart = new google.visualization.PieChart(document.getElementById('chart_div_report_bachelor_without_request'));
        //    chart.draw(data, options);
        //}
        var formData = new FormData();
        var arrayPrograms = $("#programs").val();
        var arrayCareers = $("#careers").val();
        var arrayFaculties = $("#faculties").val();

        for (var i = 0; i < arrayPrograms.length; i++) {
            formData.append(`LstPrograms[${i}]`, arrayPrograms[i]);

        }

        for (var j = 0; j < arrayCareers.length; j++) {
            formData.append(`LstCareers[${j}]`, arrayCareers[j]);

        }

        for (var z = 0; z < arrayFaculties.length; z++) {
            formData.append(`LstFaculties[${z}]`, arrayFaculties[z]);

        }

        $.ajax({
            type: 'POST',
            url: `/admin/reporte-alumnos-egresados-sin-solicitud-bachiller/reporte-cantidad-bachilleres_sin_tramite_solicitado`.proto().parseURL(),
            processData: false,
            contentType: false,
            data: formData,
            success: function (data) {

                var listData = [];

                if (data != undefined && data != null) {
                    listData.push({ name: 'Con trámite solicitado', y: data.has });
                    listData.push({ name: 'Sin trámite solicitado', y: data.notHave });
                }
                Highcharts.chart('chart_div_report_bachelor_without_request', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'Trámites realizados vs Trámites pendientes'
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                    },
                    accessibility: {
                        point: {
                            valueSuffix: '%'
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
                        name: 'Brands',
                        colorByPoint: true,
                        data: listData
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
            error: function (error) {
            toastr.error(error.responseText, _app.constants.toastr.title.error);
            },
            complete: function () {
                $("#btnSend").removeLoader();
            }
        });
        

    };




    var initializer = function () {
        $("#careers").select2();
        $("#programs").select2();

        selects.faculties();
        selects.careers();
        selects.academic_programs();


    };


    return {
        load: function () {
            initializer();
            validate();
        }
    };
}();

$(function () {
    report_bachelor_without_request.load();
});