var report_bachelor = function () {
    var selects = {
        academic_programs: function () {
            $("#careers").on('change', function () {
                $(this).valid();
                var careerListSelect = $(this).val();
                var formData = new FormData();
                for (var i = 0; i < careerListSelect.length; i++) {
                    formData.append(`Select2Data[${i}]`, careerListSelect[i]);
                }
                $.ajax({
                    type: 'POST',
                    url: `/admin/reporte-alumnos-bachilleres-y-titulados/programas-academicos`.proto().parseURL(),
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
                url: `/admin/reporte-alumnos-bachilleres-y-titulados/facultades/get`.proto().parseURL()
            }).done(function (data) {
                $("#faculties").select2({
                    placeholder: "Todas",
                    data: data.items,
                    multiple: true
                });
                if (facultyList != null) {
                    $('#faculties').val(facultyList).trigger('change');
                }


            });
        },
        careers: function () {
            $("#faculties").on('change', function () {
                $(this).valid();
                var facultyListSelect = $(this).val();
                var formData = new FormData();
                for (var i = 0; i < facultyListSelect.length; i++) {
                    formData.append(`Select2Data[${i}]`, facultyListSelect[i]);
                }

                $.ajax({
                    type: 'POST',
                    url: `/admin/reporte-alumnos-bachilleres-y-titulados/carreras`.proto().parseURL(),
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
                    if (careerList != null) {
                        $('#careers').val(careerList).trigger('change');
                    }

                });

            });
        }
    };
    var initializer = function () {
        $("#form-valid").validate({
            submitHandler: function (form, event) {
                $("#btnSend").addLoader();
                $("#chart_div_report_bachelor").css('display', 'block');
                mApp.block("#chart_div_report_bachelor", { type: "loader", message: "Cargando..." });
                var formData = new FormData();
                var arrayFaculties = $("#faculties").val();
                var arrayPrograms = $("#programs").val();
                var arrayCareers = $("#careers").val();
                var gradeType = $("#gradeType").val();
                if (arrayFaculties != null) {
                    for (var i = 0; i < arrayFaculties.length; i++) {
                        formData.append(`LstFaculties[${i}]`, arrayFaculties[i]);
                    }
                }
                if (arrayPrograms != null) {
                    for (var i = 0; i < arrayPrograms.length; i++) {
                        formData.append(`LstPrograms[${i}]`, arrayPrograms[i]);
                    }
                }

                if (arrayCareers != null) {
                    for (var i = 0; i < arrayCareers.length; i++) {
                        formData.append(`LstCareers[${i}]`, arrayCareers[i]);
                    }
                }
                formData.append('GradeType', gradeType);
                var titleDef = "";
                switch (gradeType) {
                    case "0":
                        titleDef += "Número de bachilleres y titulados";
                        break;
                    case "1":
                        titleDef += "Número de bachilleres";
                        break;
                    case "2":
                        titleDef += "Número de titulados";
                        break;
                }
                $.ajax({
                    type: 'POST',
                    url: `/admin/reporte-alumnos-bachilleres-y-titulados/reporte-cantidad`.proto().parseURL(),
                    processData: false,
                    contentType: false,
                    data: formData,
                    success: function (data) {
                        var sum = 0;
                        var categoriesCareer = [];
                        var dataCareer = [];
                        if (data != undefined && data != null) {
                            for (var i = 0; i < data.length; i++) {
                                categoriesCareer.push(data[i].program);
                                dataCareer.push(data[i].bachelorCount);
                                sum += data[i].bachelorCount;
                            }
                        }
                        Highcharts.chart('chart_div_report_bachelor', {
                            title: {
                                text: titleDef
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
            }
        });
        $("#careers").select2({ placeholder: "Todas" });
        $("#programs").select2({ placeholder: "Todas" });
        $("#gradeType").select2({ placeholder: "Todas" });
        $("#programs").on('change', function () {
            $(this).valid();
        });

        selects.faculties();
        selects.careers();
        selects.academic_programs();



    };


    return {
        load: function () {
            initializer();
        }
    };
}();
$(function () {

    report_bachelor.load();

});









//var Reports = function () {

//    const lang = function () {
//        Highcharts.setOptions({
//            lang: {
//                contextButtonTitle: "Opciones",
//                viewFullscreen: "Ver en pantalla completa",
//                printChart: "Imprimir",
//                downloadPNG: "Descargar PNG",
//                downloadJPEG: "Descargar JPEG",
//                downloadPDF: "Descargar PDF",
//                downloadSVG: "Descargar SVG",
//                downloadCSV: "Descargar CSV",
//                downloadXLS: "Descargar XLS",
//                openInCloud: "Abrir editor online"
//            }
//        });
//    };

//    const buttons = {
//        contextButton: {
//            menuItems: ["viewFullscreen", "printChart", "separator", "downloadPNG", "downloadJPEG", "downloadPDF", "downloadSVG", "separator", "downloadCSV", "downloadXLS"]
//        }
//    };

//    const report1 = function () {
//        $.ajax({
//            url: ("/admin/reportes/empresas/reporte/1/" + $("#offerType").val()).proto().parseURL(),
//            type: "GET",
//            success: function (result) {
//                Highcharts.chart('report-1', {
//                    chart: {
//                        events: {
//                            exportData: function () {
//                                var total = $("body").find(".highcharts-data-table");
//                                var current = $(".m-portlet").find(".highcharts-data-table");
//                                var currentLength = current.length;
//                                for (currentLength; currentLength < total.length; currentLength++) {
//                                    $(total[currentLength]).remove();
//                                }
//                            }
//                        }
//                    },
//                    title: {
//                        text: 'Número de ofertas por empresa'
//                    },
//                    subtitle: {
//                        text: $("option:selected").text()
//                    },
//                    xAxis: {
//                        categories: result.categories,
//                        title: {
//                            text: 'Empresas'
//                        }
//                    },
//                    yAxis: {
//                        min: 0,
//                        title: {
//                            text: 'Número de ofertas'
//                        }
//                    },
//                    series: [{
//                        type: 'column',
//                        name: 'Número de ofertas',
//                        colorByPoint: true,
//                        data: result.data,
//                        showInLegend: true
//                    }],
//                    exporting: {
//                        showTable: true,
//                        buttons: buttons
//                    },
//                });
//                $("#btn1").prop('disabled', 'true');
//            },
//            error: function (error) {
//                toastr.error(error.responseText, _app.constants.toastr.title.error);
//            },
//            complete: function () {
//                mApp.unblock("#report-1-header", { type: "loader", message: "Cargando..." });
//            }
//        });
//    };


//    //var eventButtons = function () {
//    //    $("#btn1").on('click', function () {
//    //        $("#report-1-header").css('display', 'block');
//    //        mApp.block("#report-1-header", { type: "loader", message: "Cargando..." });
//    //        report1();
//    //    });
//    //    $("#btn2").on('click', function () {
//    //        $("#report-2-header").css('display', 'block');
//    //        mApp.block("#report-2-header", { type: "loader", message: "Cargando..." });
//    //        report2();
//    //    });
//    //    $("#btn3").on('click', function () {
//    //        $("#report-3-header").css('display', 'block');
//    //        mApp.block("#report-3-header", { type: "loader", message: "Cargando..." });
//    //        report3();
//    //    });
//    //};

//    return {
//        init: function () {
//            lang();
//            eventButtons();
//            events();
//        }
//    };
//}();

//$(function () {
//    Reports.init();
//});