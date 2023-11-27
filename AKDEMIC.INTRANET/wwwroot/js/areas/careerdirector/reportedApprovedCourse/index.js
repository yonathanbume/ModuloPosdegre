var reportApprovedCourses = function () {
    var datatable = null;

    var options = getSimpleDataTableConfiguration({
        url: `/director-carrera/reporte-aprobados-desaprobados-cursos/get`.proto().parseURL(),
        data: function (data) {
            data.cid = $("#select_courses").val();
            data.pid = $("#select_terms").val();
            data.carId = $("#select_careers").val();
            data.curId = $("#select_curriculums").val();
            data.name = $("#search").val();
        },
        pageLength: 10,
        orderable: [],
        columns: [
            {
                data: 'name',
                title: 'Nombres Completos',
            },
            {
                data: 'code',
                title: 'Código',
            },
            {
                data: 'career',
                title: 'Carrera',
            },
            {
                data: 'academicProgram',
                title: 'Especialidad',
            },
            {
                data: 'intents',
                title: 'Veces',
            },
            {
                data: 'grade',
                title: 'Nota',
            },
            {
                data: 'approbed',
                title: 'Estado',
                render: function (row) {
                    if (row === true) {
                        return `<span class="m-badge  m-badge--success m-badge--wide">Aprobado</span>`;
                    } else {
                        return `<span class="m-badge  m-badge--danger m-badge--wide">Desaprobado</span>`;
                    }

                }
            }
        ]
    });

    var search = function () {
        var timing = 0;
        $("#search").keyup(function () {
            clearTimeout(timing);
            timing = setTimeout(function () {
                loadDatatable();
            }, 500);
        });
    };

    //var loadCoursesSelect = function () {
    //    var cid = $("#select_terms").val();
    //    $.ajax({
    //        url: `/director-carrera/reporte-aprobados-desaprobados-cursos/getCourses/${cid}`.proto().parseURL(),
    //    }).done(function (data) {
    //        $("#select_courses").empty();
    //        $("#select_courses").select2({
    //            data: data.items,
    //            placeholder: "Cursos"
    //        }).trigger('change');
    //    });
    //    $("#select_courses").on("change", function (e) {
    //        loadChartApprobedDisapprobedCourse();
    //        loadDatatable();
    //    });
    //}
    var loadCareersSelect = function () {
        $.ajax({
            url: `/carreras/v2/get`.proto().parseURL()
        }).done(function (data) {
            $("#select_careers").empty();
            $("#select_careers").select2({
                data: data.items,                
                placeholder: "Escuelas"
            });
        });
        $("#select_careers").on('change', function () {
            loadCurriculumsSelect();            
        });
        $("#select_careers").trigger("change")
    };
    var loadCurriculumsSelect = function(){
        var cid = $("#select_careers").val();
        $.ajax({
            url: `/carreras/${cid}/planestudio/get`.proto().parseURL()
        }).done(function (data) {
            $("#select_curriculums").empty();
            $("#select_curriculums").select2({
                data: data.items,                
                placeholder: "Planes de estudio"
            }).trigger("change");
        });
        $("#select_curriculums").on('change', function () {
            //datatable.reloadTable(); 
            if(isFIrst) {
                isFIrst=false;
                loadCoursesSelect.init();          
            }else{
                loadCoursesSelect.load();          
            }
        });
    };
    var isFIrst=true;
    var loadCoursesSelect = {
        init: function(){
            var cid = $("#select_curriculums").val();
            $.ajax({
                url: `/curriculum/${cid}/cursos/get`.proto().parseURL()
                //url: `/admin/reporte_asistencia_curso/cursos/periodo/${pid}`.proto().parseURL()
            }).done(function (data) {
                $("#select_courses").empty();
                $("#select_courses").select2({
                    data: data.items,                
                    placeholder: "Cursos"
                });
                loadCoursesSelect.event();  
                $("#select_courses").trigger("change")
            });      
        },
        event: function(){
            $("#select_courses").on('change', function () {
                 loadChartApprobedDisapprobedCourse();
                 loadDatatable();     
            });
        },
        load: function(){
            var cid = $("#select_curriculums").val();
            $.ajax({
                url: `/curriculum/${cid}/cursos/get`.proto().parseURL()
                //url: `/admin/reporte_asistencia_curso/cursos/periodo/${pid}`.proto().parseURL()
            }).done(function (data) {
                $("#select_courses").empty();
                $("#select_courses").select2({
                    data: data.items,                
                    placeholder: "Cursos"
                }).trigger("change");
            });
        }
    };
    var loadTermsSelect = function () {
        $("#select_terms").select2({
            ajax: {
                url: `/director-carrera/reporte-aprobados-desaprobados-cursos/periodos/get`.proto().parseURL(),
                delay: 300,
                processResults: function (data) {
                    return {
                        results: data.results
                    };
                }
            },
            minimumInputLength: 0,
            placeholder: 'Periodo Académico',
            allowClear: true
        }).on('change', function () {
             loadChartApprobedDisapprobedCourse();
             loadDatatable();
        });
        // $.ajax({
        //     url: `/periodos/get`.proto().parseURL()
        // }).done(function (data) {
        //     $("#select_terms").select2({
        //         data: data.items
        //     }).val(data.selected).trigger('change');
        // });
        // $("#select_terms").on("change", function (e) {
        //     loadCoursesSelect();
        // });

    }
    var loadDatatable = function () {
        if (datatable === null) {
            datatable = $("#datatable_index").DataTable(options);
        } else {
            datatable.ajax.reload();
        }

    }

    var initializer = function () {
        $.when(loadCareersSelect(), loadTermsSelect()).done(function () {
            loadChartApprobedDisapprobedCourse();
            loadDatatable();
            search();
        });
    }

    var loadChartApprobedDisapprobedCourse = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {
            var pid = $("#select_terms").val();
            var cid = $("#select_courses").val();
            var carId = $("#select_careers").val();
            var curId = $("#select_curriculums").val();

            $.ajax({
                type: "GET",
                url: `/director-carrera/reporte-aprobados-desaprobados-cursos/chart/${carId}/${curId}/${cid}/${pid}`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartApprobedDisapprobedCourse(data);
                    }
                    else {
                        document.getElementById('chart_div_report_approbed_disapprobed_courses').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartApprobedDisapprobedCourse(data) {
            var dataArray = [
                ['', '']
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.approbedname, item.approbeds]);
                dataArray.push([item.disapprobedname, item.disapprobeds]);
            });
            data = google.visualization.arrayToDataTable(dataArray);
            var options = {
                title: "Aprobados vs desaprobados",
                height: 400,
                width: 700,
                colors: ['#D32F2F', '#43A047'],
                titleFontSize: 20,
                is3D: true
            };
            var chart = new google.visualization.PieChart(document.getElementById('chart_div_report_approbed_disapprobed_courses'));
            chart.draw(data, options);
        }
    }


    return {
        load: function () {
            initializer();
        }
    }
}();

$(function () {
    reportApprovedCourses.load();
})