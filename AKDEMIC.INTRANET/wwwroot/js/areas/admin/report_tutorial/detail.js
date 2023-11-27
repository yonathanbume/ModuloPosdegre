var reportTutorialAssistance = function () {
    var datatable = null;
    var eid = $("#Id").val();
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/admin/reporte_tutoria/tutorias_efectuadas/${eid}/alumnos`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'fullname',
                title: 'Nombres Completos',
                width: 200
            },
            {
                field: 'code',
                title: 'Código',
                width: 100
            },
            {
                field: 'email',
                title: 'Correo electrónico',
                width: 180
            },
            {
                field: 'career',
                title: 'Carrera',
                width: 180
            },
            {
                field: 'status',
                title: 'Estado',
                template: function (row) {
                    if (row.absent != true) {
                        return `<span class="m-badge  m-badge--success m-badge--wide">Asistió</span>`
                    } else {
                        return `<span class="m-badge  m-badge--danger m-badge--wide">No Asistió</span>`
                    }

                }
            }
        ]
    }

  
    var loadDatatable = function () { 
        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }       
        datatable = $(".m-datatable").mDatatable(options);        
    }

  
    var loadChartTutorialAssitance = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {      
            $.ajax({
                type: "GET",
                url: `/admin/reporte_tutoria/chart/${eid}`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTutorialStudentAssistance(data);
                    }
                    else {
                        document.getElementById('chart_div_report_assistance_tutorial').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTutorialStudentAssistance(data) {
            var dataArray = [
                ['', '']
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.assist, item.assistants]);
                dataArray.push([item.absent, item.missing]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {
                title: "Asistentes vs ausentes",
                height: 400,
                width: 700,
                colors: ['#D32F2F', '#43A047'],
                titleFontSize: 20,
                is3D: true
            };
            var chart = new google.visualization.PieChart(document.getElementById('chart_div_report_assistance_tutorial'));
            chart.draw(data, options);
        }
    }


    return {
        load: function () {
            loadChartTutorialAssitance();
            loadDatatable();
            
        }
    }
}();

$(function () {
    reportTutorialAssistance.load();
})