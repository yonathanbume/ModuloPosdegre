var InitApp = function () {
    var datatable = null;
    var datatable2 = null;
    var select = {
        term: {
            init: function () {
                $.ajax({
                    url: `/periodos/get`.proto().parseURL()
                })
                    .done(function (data) {
                        $("#termId").select2({
                            placeholder: "Seleccione una periodo académico",
                            minimumResultsForSearch: 10,
                            data: data.items,
                            selected: data.selected
                        });
                    });
            }
        },
        faculty: {
            init: function () {
                $.ajax({
                    url: `/facultades/get`.proto().parseURL()
                }).done(function (data) {
                    $("#facultyId").select2({
                        data: data.items,
                        placeholder: "Seleccione una facultad",
                        minimumResultsForSearch: 10
                    });

                    $("#facultyId").val(null).trigger("change");

                    $("#facultyId").on("change", function () {
                        select.career.load($("#facultyId").val());
                    });
                });
            }
        },
        career: {
            init: function () {
                $("#careerId").select2({
                    placeholder: "Seleccione una escuela",
                    disabled: true
                });

                $("#careerId").on("change", function () {
                    select.curriculum.load($("#careerId").val());
                });
            },
            load: function (faculty) {
                $.ajax({
                    url: `/carreras/get?fid=${faculty}`.proto().parseURL(),
                }).done(function (data) {
                    $("#careerId").empty();

                    $("#careerId").select2({
                        placeholder: "Seleccione una escuela",
                        data: data.items,
                        disabled: false,
                        minimumResultsForSearch: 10
                    });

                    if (data.items.length > 0) {
                        $("#careerId").trigger("change");
                    } else {
                        select.curriculum.empty();
                        select.academicyear.empty();
                        select.course.empty();
                    }
                });
            }
        },
        curriculum: {
            init: function () {
                $("#curriculumId").select2({
                    placeholder: "Seleccione un plan",
                    disabled: true
                });
            },
            load: function (career) {
                $.ajax({
                    url: `/carreras/${career}/planestudio/get`.proto().parseURL(),
                }).done(function (data) {
                    $("#curriculumId").empty();

                    $("#curriculumId").select2({
                        placeholder: "Seleccione un plan de estudios",
                        data: data.items,
                        disabled: false
                    });

                    if (data.items.length > 0) {
                        $("#curriculumId").trigger("change");
                    }
                });
            },
            empty: function () {
                $("#curriculumId").empty();
                $("#curriculumId").select2({
                    placeholder: "Seleccione un plan",
                    disabled: true
                });
            }
        },

        init: function () {
            select.term.init();
            select.faculty.init();
            select.career.init();
            select.curriculum.init();

            $(".btn-find").on("click", function () {
                if ($("#facultyId").val() == null || $("#facultyId").val() == "") {
                    toastr.error("Debe seleccionar una facultad", _app.constants.toastr.title.error);
                    return false;
                }
                loadChart2();
                loadChart();
            });

        }
    };

    var loadChart = function () {
        mApp.block("#container", "Cargando...");
        //$("#preview-datatable").html('');
        //$("#previewView").modal('show');

        //mApp.block("#preview-datatable");
        var termId = $("#termId").val();
        var facultyId = $("#facultyId").val();
        var careerId = $("#careerId").val();
        var curriculumId = $("#curriculumId").val();

        $.ajax({
            url: `/director-carrera/notas-por-competencias/nivel-de-logro-datos/${termId}/${facultyId}/${careerId}/${curriculumId}`.proto().parseURL(),
            type: "GET",
            dataType: "html",
            contextType: "application/json",
            beforeSend: function () {
                $(".portlet-details").addClass("m--hide");
                $("#txt-detail").html('Listado detallado');
            }
        }).done(function (data) {
            $("#container").html(data);
            $(".competence-details").on('click', function () {
                var avg = $(this).data('typeavg');
                var competenceId = $(this).data('competenceid');

                if (datatable != null && datatable != undefined) {
                    datatable.clear();
                    datatable.destroy();
                }

                datatable = $("#tbl-data").DataTable({
                    serverSide: false,
                    "columnDefs": [
                        { "orderable": true, "targets": [0, 1, 2] }
                    ],
                    dom: 'Bfrtip',
                    buttons: [
                        {
                            extend: 'excel',
                            footer: false,
                            exportOptions: {
                                columns: [0, 1, 2]
                            }
                        },
                        {
                            extend: 'pdf',
                            footer: false,
                            exportOptions: {
                                columns: [0, 1, 2]
                            }

                        },

                    ],
                    searching: false
                });

                $.ajax({
                    type: "GET",
                    url: `/director-carrera/notas-por-competencias/obtener-datos-por-cantidad/${termId}/${facultyId}/${careerId}/${curriculumId}/${competenceId}/${avg}`.proto().parseURL(),
                    beforeSend: function () {
                        mApp.block("#rendering", "Cargando...");
                    }
                }).done(function (data) {

                    if (data != null) {
                        if (data.rowChilds.length > 0) {
                            $("#txt-detail").html(`${data.competenceName}`);
                            for (var i = 0; i < data.rowChilds.length; i++) {
                                var iTable = datatable
                                    .row
                                    .add([`${data.rowChilds[i].userName}`, `${data.rowChilds[i].studentFullName}`, `${data.rowChilds[i].average}`, `<button data-studentid =${data.rowChilds[i].studentId} data-compentenceid =${data.rowChilds[i].competenceId} data-typeavg =${data.rowChilds[i].type}  class='btn btn-sm btn-primary view-courses'> <i class='fa fa-eye'></i> </button>`])
                                    .draw()
                                    .node();

                                $(iTable)
                                    .css('text-align', 'center');
                            }

                            datatable.on('click', '.view-courses', function (e) {
                                var studentId = $(this).data('studentid');
                                var competenceId = $(this).data('compentenceid');
                                var type = $(this).data('typeavg');
                                var termId = $("#termId").val();
                                var facultyId = $("#facultyId").val();
                                var careerId = $("#careerId").val();
                                var curriculumId = $("#curriculumId").val();

                                $("#courses-modal").modal('show');
                                $.ajax({
                                    type: "GET",
                                    url: `/director-carrera/notas-por-competencias/obtener-datos-por-cantidad-por-estudiante/${termId}/${facultyId}/${careerId}/${curriculumId}/${competenceId}/${type}/${studentId}`.proto().parseURL(),
                                    beforeSend: function () {
                                        mApp.block("#rendering-modal", "Cargando...");
                                    }
                                }).done(function (data) {

                                    if (datatable2 != null && datatable2 != undefined) {
                                        datatable2.clear();
                                        datatable2.destroy();
                                    }

                                    datatable2 = $("#tbl-data-2").DataTable({
                                        serverSide: false,
                                        "columnDefs": [
                                            { "orderable": false, "targets": [0, 1, 2] }
                                        ],
                                        dom: 'Bfrtip',
                                        buttons: [
                                            {
                                                extend: 'excel',
                                                footer: false,
                                                exportOptions: {
                                                    columns: [0, 1, 2]
                                                }
                                            },
                                            {
                                                extend: 'pdf',
                                                footer: false,
                                                exportOptions: {
                                                    columns: [0, 1, 2]
                                                }

                                            },

                                        ]
                                    });
                                    if (data != null) {
                                        $("#title-studentname").html(`Estudiante: ${data.studentFullName}`);
                                        if (data.rowChilds.length > 0) {
                                            for (var i = 0; i < data.rowChilds.length; i++) {
                                                var iTable = datatable2
                                                    .row
                                                    .add([`${data.rowChilds[i].courseName}`, `${data.rowChilds[i].credits}`, `${data.rowChilds[i].average}`])
                                                    .draw()
                                                    .node();

                                                $(iTable)
                                                    .css('text-align', 'center');
                                            }
                                            mApp.unblock("#rendering-modal");
                                        }
                                    }
                                });
                            });

                        }

                    }

                    $(".portlet-details").removeClass("m--hide");
                    mApp.unblock("#rendering");
                });

            });
            $("#btn-pdf").on('click', function () {
                var button = $(this);
                button.addLoader();
                $.fileDownload(`/director-carrera/notas-por-competencias/descargar/pdf/${termId}/${facultyId}/${careerId}/${curriculumId}`.proto().parseURL())
                    .done(function () {
                        button.removeLoader();
                        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                    })
                    .fail(function (e) {
                        button.removeLoader();
                        toastr.error("Ocurrió un error al generar el archivo", "Error");
                    });
            });
            $("#btn-excel").on('click', function () {
                var button = $(this);
                button.addLoader();
                $.fileDownload(`/director-carrera/notas-por-competencias/reporte-excel/${termId}/${facultyId}/${careerId}/${curriculumId}`.proto().parseURL())
                    .done(function () {
                        button.removeLoader();
                        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                    })
                    .fail(function (e) {
                        button.removeLoader();
                        toastr.error("Ocurrió un error al generar el archivo", "Error");
                    });

            });        
        }).fail(function (data) {
            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
        }).always(function () {
            mApp.unblock("#container");
        });


    };


    var loadChart2 = function () {
        $.ajax({
            url: ("/director-carrera/notas-por-competencias/reporte").proto().parseURL(),
            data: {
                termId: $("#termId").val(),
                facultyId: $("#facultyId").val(),
                careerId: $("#careerId").val(),
                curriculumId: $("#curriculumId").val()        

            },
            beforeSend: function () {
                mApp.block("#container-2", "Cargando...");
            }
        })
            .done(function (data) {
                console.log(data);
                var categoriesName = [];
                var listData = [];


                if (data.rangeLevels != undefined && data.rangeLevels != null) {
                    for (var i = 0; i < data.rangeLevels.length; i++) {
                        //categoriesName.push(data[i].competenceName);
                        //finalData.push(data[i].finalResult);
                        listData.push({ name: data.rangeLevels[i].name, data: data.rangeLevels[i].array });
                    }
                }


                Highcharts.chart('container-2', {
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: 'Niveles por competencias'
                    },
                    xAxis: {
                        categories: data.competencesNames
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: ''
                        },
                        stackLabels: {
                            enabled: true,
                            style: {
                                fontWeight: 'bold',
                                color: ( // theme
                                    Highcharts.defaultOptions.title.style &&
                                    Highcharts.defaultOptions.title.style.color
                                ) || 'gray'
                            }
                        }
                    },
                    legend: {
                        align: 'right',
                        x: -30,
                        verticalAlign: 'top',
                        y: 25,
                        floating: true,
                        backgroundColor:
                            Highcharts.defaultOptions.legend.backgroundColor || 'white',
                        borderColor: '#CCC',
                        borderWidth: 1,
                        shadow: false
                    },
                    //tooltip: {
                    //    headerFormat: '<b>{point.x}</b><br/>',
                    //    pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
                    //},
                    plotOptions: {
                        column: {
                            stacking: 'normal',
                            dataLabels: {
                                enabled: true
                            }
                        }
                    },
                    series: listData,
                    credits: {
                        text: '',
                        href: ''
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


            }).fail(function (e) {
                toastr.info(e.responseText, "Información!");
            })
            .always(function () {
                mApp.unblock("#container-2", "Cargando...");
            });
    };


    return {
        init: function () {
            select.init();
            //exportFiles();
        }
    };
}();

$(function () {
    InitApp.init();
});