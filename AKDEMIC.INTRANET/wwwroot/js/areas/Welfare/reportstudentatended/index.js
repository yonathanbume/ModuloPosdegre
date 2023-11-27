
var reportStudentAtended = function () {
    var colorArray = ['#d50000', '#f50057', '#aa00ff', '#6200ea', '#304ffe', '#2962ff', '#0091ea', '#00e5ff', '#00bfa5', '#00c853', '#76ff03', '#ffab00', '#dd2c00'];
    var totalstudents = 0;
    var datatable = null;
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ""
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
                field: "monthDescription",
                title: "Mes",
                width: 200
            }
        ]
    };

    var loadDatatable = function (currentyear) {

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        options.data.source.read.url = `/welfare/reporte_alumnos_atendidos/estudiantes-por-mes/${currentyear}`.proto().parseURL();
        datatable = $(".m-datatable").mDatatable(options);
    };

    var loadChartStudentAtended = function (currentyear) {

        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {
            const MonthsData = [
                { Month: 1, Total: 0, Description: "Enero" },
                { Month: 2, Total: 0, Description: "Febrero" },
                { Month: 3, Total: 0, Description: "Marzo" },
                { Month: 4, Total: 0, Description: "Abril" },
                { Month: 5, Total: 0, Description: "Mayo" },
                { Month: 6, Total: 0, Description: "Junio" },
                { Month: 7, Total: 0, Description: "Julio" },
                { Month: 8, Total: 0, Description: "Agosto" },
                { Month: 9, Total: 0, Description: "Septiembre" },
                { Month: 10, Total: 0, Description: "Octubre" },
                { Month: 11, Total: 0, Description: "Noviembre" },
                { Month: 12, Total: 0, Description: "Diciembre" }
            ];
            var Months = [];
            Months = MonthsData;
            mApp.block(".m-portlet", { type: "loader", message: "Cargando..." });
            $.ajax({
                type: "GET",
                url: `/welfare/reporte_alumnos_atendidos/alumnos_nuevos_atendidos/${currentyear}/get`.proto().parseURL(),
                success: function (data) {
                    if (data.length > 0) {
                        $.each(Months, function (index, item) {

                            $.each(data, function (index1, item2) {
                                if (item.Month == item2.month) {
                                    item.Total = item2.total
                                }
                            })
                            totalstudents += item.Total;
                        });
                        totalstudents = totalstudents == 0 ? 1 : totalstudents;
                        PopulationChartStudentAtended(Months);
                    }
                    else {
                        PopulationChartStudentAtended(Months);

                    }


                },
                error: function (request, status, error) {
                    document.getElementById('chart_div_report_student_new_atented').innerHTML = "";
                    toastr.error(request.responseText);
                },
                complete: function () {
                    mApp.unblock(".m-portlet");
                }
            });
            loadDatatable(currentyear);
        }


        function PopulationChartStudentAtended(Months) {
            totalstudents = totalstudents == 0 ? 1 : totalstudents;
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(Months, function (i, item) {
                dataArray.push([item.Description, item.Total, item.Total, colorArray[i]]);

            });
            var dataTotal = new google.visualization.arrayToDataTable(dataArray);
            var options = {
                vAxis: {
                    minValue: 0,
                    viewWindow: {
                        min: 0,
                        max: totalstudents

                    }
                },
                is3D: true,
                legend: { position: "none" }
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_student_new_atented'));
            chart.draw(dataTotal, options);
            totalstudents = 0;
            dataArray = null;
            Months = new Array();

        }
    };

    var datePickerInit = function () {
        $("#currentYear").datepicker({
            format: "yyyy",
            viewMode: "years",
            minViewMode: "years"
        });
    };
    

    var returnAll = function () {
        $("#report-form").validate({
            submitHandler: function (form, event) {
                var year = $("#currentYear").val();
                if (!isNaN(year)) {
                    loadChartStudentAtended(year);
                }                
            }
        });        
    };


    
    return {
        load: function () {
            datePickerInit();
            returnAll();
            
        }
    }
}();

$(function () {
    reportStudentAtended.load();
})