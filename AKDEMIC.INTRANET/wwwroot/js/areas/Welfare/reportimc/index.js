var reportIMC = function () {
    var colorArray = ['#d50000', '#f50057', '#aa00ff', '#6200ea', '#304ffe', '#2962ff', '#0091ea', '#00e5ff', '#00bfa5', '#00c853', '#76ff03', '#ffab00', '#dd2c00'];
    var datatable = null;
   
    var loadChartIMC = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);
  
        function LoadData() {  
            mApp.block(".m-portlet", { type: "loader", message: "Cargando..." });
            $.ajax({
                type: "GET",
                url: `/welfare/reporte_IMC/Chart`.proto().parseURL(),                 
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {                   
                    if (data.length > 0)                   
                        PopulationChartIMC(data , data.length);                  
                },
                complete: function () {
                    mApp.unblock(".m-portlet");
                }
            });  
        }
  
        function PopulationChartIMC(data, datalength) {
            var dataArray = [
                ['', '' , { role: 'annotation' } , { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.clasification, item.total , item.total , colorArray[i] ]);                
                 
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {     
                vAxis: {
                    minValue: 0,
                    viewWindow: {
                        min: 0,
                        max: datalength
                    }
                },
                is3D: true,
                vAxis: {
                    gridlines: {
                        color: 'transparent'
                    }
                },
                legend: { position: "none" }
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_imc'));
            chart.draw(data, options);
          
            
        }




    } 

    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/welfare/reporte_IMC/estudiantes-IMC`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "userName",
                title: "Código",
                width: 200
            },
            {
                field: "fullname",
                title: "Nombres Completos",
                width: 200
            },
            {
                field: "email",
                title: "Email",
                width: 200
            },
            {
                field: "faculty",
                title: "Escuela Profesional",
                width: 200
            },
            {
                field: "categoryIMC",
                title: "Clasificación IMC",
                width: 200
            }
        ]
    };

    var loadDatatable = function () {

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        datatable = $(".m-datatable").mDatatable(options);
    };
   

    return {
        load: function () {
            loadChartIMC();
            loadDatatable();
        }
    };
}();

$(function () {
    reportIMC.load();
})