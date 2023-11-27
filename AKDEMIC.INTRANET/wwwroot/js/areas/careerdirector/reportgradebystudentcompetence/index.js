var InitApp = function () {
    var datatable = null;
    var datatableComplete = null;

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

                $("#curriculumId").on("change", function () {
                    select.academicyear.load($("#curriculumId").val());
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
                    } else {

                        select.academicyear.empty();
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
        academicyear: {
            init: function () {
                $("#academicyear-select").select2({
                    placeholder: "Seleccione un ciclo",
                    disabled: true
                });
            },
            load: function (curriculumid) {
                $.ajax({
                    url: `/planes-estudio/${curriculumid}/niveles/get`.proto().parseURL(),
                }).done(function (data) {
                    $("#academicyear-select").empty();
                    $("#academicyear-select").select2({
                        placeholder: "Seleccione un ciclo",
                        data: data.items,
                        disabled: false,
                        minimumResultsForSearch: -1
                    });

                    if (data.items.length > 0) {
                        $("#academicyear-select").trigger("change");
                    }
                });
            },
            empty: function () {
                $("#academicyear-select").empty();
                $("#academicyear-select").select2({
                    placeholder: "Seleccione un ciclo",
                    disabled: true
                });
            }
        },

        init: function () {
            select.term.init();
            select.faculty.init();
            select.career.init();
            select.curriculum.init();

            select.academicyear.init();



            $(".btn-find").on("click", function () {
                if ($("#facultyId").val() == null || $("#facultyId").val() == "") {
                    toastr.error("Debe seleccionar una facultad", _app.constants.toastr.title.error);
                    return false;
                } else {
                    var button = $(this);
                  
                    var termId = $("#termId").val();
                    var facultyId = $("#facultyId").val();
                    var careerId = $("#careerId").val();
                    var curriculumId = $("#curriculumId").val();
                    var academicYear = $('#academicyear-select').val();
                  
                    $.ajax({
                        url: (`/director-carrera/reporte-alumnos-competencias/get/${termId}/${facultyId}/${careerId}/${curriculumId}/${academicYear}`).proto().parseURL(),
                        type: "GET",
                        dataType: "html",
                        contextType: "application/json",
                        beforeSend: function () {
                            button.addLoader();
                            $(".highcharts-figure").removeClass("m--hide");
                            mApp.block("#container", "Cargando...");
                        }
                    }).done(function (data) {

                        $("#container").html(data);
                        button.removeLoader();

                        if (datatableComplete != null && datatableComplete != undefined) {
                            datatableComplete.clear();
                            datatableComplete.destroy();
                        }
                        datatableComplete = $("#datatable-a").DataTable({
                            serverSide: false,                          
                            dom: 'Bfrtip',
                            searching: true,
                            buttons: [
                                {
                                    extend: 'excel',
                                    footer: false,
                                    //exportOptions: {
                                    //    columns: [0, 1, 2]
                                    //}
                                },
                                {
                                    extend: 'pdf',
                                    footer: false,
                                    //exportOptions: {
                                    //    columns: [0, 1, 2]
                                    //}

                                },

                            ]
                        });

                        $(".details").on('click', function () {

                            var studentId = $(this).data('studentid');
                            var competenceId = $(this).data('competenceid');                          
                            var termId = $("#termId").val();
                            var facultyId = $("#facultyId").val();
                            var careerId = $("#careerId").val();
                            var curriculumId = $("#curriculumId").val();
                            var academicYear = $("#academicyear-select").val();

                            $("#courses-modal").modal('show');
                            mApp.block(".rendering-modal");   
                            $.ajax({
                                type: "GET",
                                url: `/director-carrera/reporte-alumnos-competencias/obtener-datos-por-cantidad-por-estudiante/${termId}/${facultyId}/${careerId}/${curriculumId}/${competenceId}/${studentId}/${academicYear}`.proto().parseURL(),
                                //beforeSend: function () {
                                      
                                //    //mApp.blockUI("#rendering-modal");
                                //}
                            }).done(function (data) {

                                if (datatable != null && datatable != undefined) {
                                    datatable.clear();
                                    datatable.destroy();
                                }

                                datatable = $("#tbl-data-2").DataTable({
                                    serverSide: false,
                                    "columnDefs": [
                                        { "orderable": false, "targets": [0, 1, 2] }
                                    ],
                                    dom: 'Bfrtip',
                                    processing:true,
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
                          
                                    if (data.length > 0) {
                                        for (var i = 0; i < data.length; i++) {
                                            var iTable = datatable
                                                .row
                                                .add([`${data[i].courseName}`, `${data[i].credits}`, `${data[i].grade}`])
                                                .draw()
                                                .node();

                                            $(iTable)
                                                .css('text-align', 'center');
                                        }
                                        mApp.unblock(".rendering-modal");
                                        //mApp.unblockUI("#rendering-modal");
                                    }
                                }
                            });                        

                        });
                    }).fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {
                        button.removeLoader();
                        mApp.unblock("#container");
                    });
                }

               
                //loadChart();
            });

        }
    };

    //var loadChart = function () {
    //    mApp.block(".m-portlet__body", "Cargando...");
    //    $.ajax({
    //        url: ("/director-carrera/notas-por-competencias/get").proto().parseURL(),
    //        data: {                
    //            termId: $("#termId").val(),
    //            facultyId: $("#facultyId").val(),
    //            careerId: $("#careerId").val(),
    //            curriculumId: $("#curriculumId").val(),
    //            competenceId: $('#competenceId').val()     

    //        }
    //    })
    //        .done(function (data) {

    //            var categoriesName = [];
    //            var finalData = [];

    //            if (data != undefined && data != null) {
    //                for (var i = 0; i < data.length; i++) {
    //                    categoriesName.push(data[i].competenceName);
    //                    finalData.push(data[i].finalResult);            
    //                }
    //            }

    //            Highcharts.chart('container', {
    //                chart: {
    //                    type: 'column'
    //                },
    //                title: {
    //                    text: 'Reportes de notas por competencias'
    //                },
    //                yAxis: {
    //                    title: {
    //                        enabled: false
    //                    }
    //                },
    //                xAxis: {

    //                    title: {
    //                        enabled: true,
    //                        text: 'Competencias',
    //                        style: {
    //                            fontWeight: 'normal'
    //                        }
    //                    },
    //                    categories: categoriesName

    //                },
    //                series: [{
    //                    name: 'Notas ',
    //                    data: finalData,                    
    //                    showInLegend: true
    //                }],
    //                credits: {
    //                    text: '',
    //                    href: ''
    //                }
    //            });


    //            if (datatable != null && datatable != undefined) {
    //                datatable.clear();
    //                datatable.destroy();
    //            }
    //            var count = 0;
    //            //var datatableTmp = [{ competenceName, courseName, average, }]
    //            datatable = $("#tbl-data").DataTable({
    //                serverSide: false,
    //                "columnDefs": [
    //                    { "orderable": false, "targets": [0,1,2,3] }    
    //                ],                             
    //                dom: 'Bfrtip',
    //                buttons: [
    //                    'excel', 'pdf'
    //                ]
    //            });

    //            for (var i = 0; i < data.length; i++) {
    //                var i_table = datatable
    //                    .row
    //                    .add([`${data[i].competenceName} ( ${data[i].finalResult} )`,null, null,null])
    //                    .draw()
    //                    .node();
    //                $(i_table)
    //                    .css('color', 'white')
    //                    .css('background', 'gray')
    //                    .animate({ color: 'black' });

    //                $.each(data[i].rowChilds, function (index, value) {
    //                    count++;
    //                    datatable
    //                        .row
    //                        .add([null,value.courseName, value.average, value.credits ])
    //                        .draw()
    //                        .node();
    //                });                   
    //            }


    //            $(".portlet-details").removeClass("m--hide");
    //            $(".highcharts-figure").removeClass("m--hide");

    //        }).fail(function (e) {
    //            toastr.info(e.responseText, "Información!");
    //        })
    //        .always(function () {
    //            mApp.unblock(".m-portlet__body");
    //        });
    //};

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