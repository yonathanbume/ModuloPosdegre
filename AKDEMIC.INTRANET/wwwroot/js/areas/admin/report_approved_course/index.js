var reportApprovedCourses = function () {
    var datatable = null;

    var options = {
        ajax: {
            url: `/admin/reporte_aprobados_desaprobados_cursos/get`.proto().parseURL(),
            data: function (data) {
                data.cid = $("#select_courses").val();
                data.carId = $("#select_careers").val();
                data.curId = $("#select_curriculums").val();
                data.pid = $("#select_terms").val();
                data.name = $("#search").val();
            },
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
                title: 'Programa Académico',
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
        ],
        dom: 'Bfrtip',
        buttons: [
            {
                text: 'Excel',
                action: function (e, dt, node, config) {
                    var url = `/admin/reporte_aprobados_desaprobados_cursos/get-excel?careerId=${$("#select_careers").val()}&curriculumId=${$("#select_curriculums").val()}&termId=${$("#select_terms").val()}&courseId=${$("#select_courses").val()}`;
                    window.open(url, "_blank");
                }
            }
        ]
    };

    var search = function () {
        $("#search").doneTyping(function () {
            loadDatatable();
        });
    };

    //var initCoursesSelect = function () {
    //    $("#select_courses").select2({
    //        placeholder: "Cursos"
    //    });
    //
    //    $("#select_courses").on("change", function (e) {
    //        loadChartApprobedDisapprobedCourse();
    //        loadDatatable();
    //    });
    //};
    var loadCareersSelect = function () {
        $.ajax({
            url: `/carreras/v2/get`.proto().parseURL()
        }).done(function (data) {
            $("#select_careers").empty();
            $("#select_careers").select2({
                data: data.items,                
                placeholder: "Escuelas"
            });

            $("#select_careers").on('change', function () {
                loadCurriculumsSelect();
            });
            $("#select_careers").trigger("change")
        });
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
    //var loadCoursesSelect = function () {
    //    var cid = $("#select_terms").val();
    //    $.ajax({
    //        url: `/admin/reporte_aprobados_desaprobados_cursos/getCourses/${cid}`.proto().parseURL()
    //    }).done(function (data) {
    //        $("#select_courses").empty();
    //        $("#select_courses").select2({
    //            data: data,
    //            placeholder: "Cursos"
    //        }).trigger('change');
    //    });
    //};

    var loadTermsSelect = function () {
        $("#select_terms").select2({
            placeholder: "Periodo Académico"
        });

        $("#select_terms").on("change", function (e) {
             loadChartApprobedDisapprobedCourse();
                loadDatatable();  
        });

        $.ajax({
            url: `/periodos/get`.proto().parseURL()
        }).done(function (data) {
            $("#select_terms").select2({
                data: data.items
            }).val(data.selected).trigger('change');
        });


    };

    var loadDatatable = function () {
        if (datatable === null) {
            datatable = $("#datatable_index").DataTable(options);
        } else {
            datatable.ajax.reload();
        }
    };

    var initializer = function () {
        search();
        //initCoursesSelect();
        loadTermsSelect();
        loadCareersSelect();      
    };

    function LoadData() {
        var pid = $("#select_terms").val();
        var cid = $("#select_courses").val();
        var carId = $("#select_careers").val();
        var curId = $("#select_curriculums").val();
        $.ajax({
            type: "GET",
            url: `/admin/reporte_aprobados_desaprobados_cursos/chart/${carId}/${curId}/${cid}/${pid}`.proto().parseURL(),
            error: function (xhr, status, error) {
                toastr.error(xhr.responseText, _app.constants.toastr.title.error);
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
            colors: ['#43A047', '#D32F2F'],
            titleFontSize: 20,
            is3D: true
        };
        var chart = new google.visualization.PieChart(document.getElementById('chart_div_report_approbed_disapprobed_courses'));
        chart.draw(data, options);
    }

    var loadChartApprobedDisapprobedCourse = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);
    };


    return {
        load: function () {
            initializer();
        }
    };
}();

$(function () {
    reportApprovedCourses.load();
});