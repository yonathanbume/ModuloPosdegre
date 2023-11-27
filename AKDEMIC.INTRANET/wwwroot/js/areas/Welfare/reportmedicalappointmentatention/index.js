var reportMedicalAppointmentAtention = function () {
    
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
                field: 'code',
                title: 'Código',
                width: 100
            },
            {
                field: 'fullname',
                title: 'Nombres Completos',
                width: 200
            },            
            {
                field: 'email',
                title: 'Email',
                width: 150
            },                        
            {
                field: 'faculty',
                title: 'Escuela Profesional',
                width: 150
            },
            {
                field: 'date',
                title: 'Fecha de la cita',
                width: 150
            },
            {
                field: 'status',
                title: 'Estado',
                template: function (row) {
                    if (row.status == true) {
                        return `<span class="m-badge  m-badge--success m-badge--wide">Atendido</span>`
                    } else {
                        return `<span class="m-badge  m-badge--danger m-badge--wide">Pendiente</span>`
                    }

                }
            }
        ]
    }

    var loadDatatable = function (doctorId, month, year) {

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        options.data.source.read.url = `/welfare/reporte_citas_medicas/${doctorId}/${month}/${year}/atendidos`.proto().parseURL();
        datatable = $(".m-datatable").mDatatable(options);

    };
    var select2Doctors = function () {
        $.ajax({
            url: `/doctores/get`.proto().parseURL()
        }).done(function (data) {
            $("#doctorSelect2").select2({
                data: data.items
            }).trigger('change');
        });
    };

    var loadChartMedicalAppointmentAtention = function (did, month, year) {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {   
            
            $.ajax({
                type: "GET",
                url: `/welfare/reporte_citas_medicas/${did}/${month}/${year}/todas`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartMedicalAppointmentAtention(data);
                    }
                    else {
                        document.getElementById('chart_div_report_atention_alerts').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartMedicalAppointmentAtention(data) {
            var dataArray = [
                ['', '']
            ];
            $.each(data, function (i, item) {
                dataArray.push(["atendidos", item.atendidos]);                
                dataArray.push(["pendientes", item.pendientes]);  
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {
                title: "Atendidos vs pendientes",
                height: 400,
                width: 700,
                colors: ['#43A047','#D32F2F'],
                titleFontSize: 20,
                is3D: true
            };
            var chart = new google.visualization.PieChart(document.getElementById('chart_div_report_atention_alerts'));
            chart.draw(data, options);
        }
    }

    var datePickerInit = function () {
        $("#currentYearMonth").datepicker({
            format: "mm-yyyy",
            viewMode: "months",
            minViewMode: "months"
        });
    };

    var submitAction = function () {
        $(".btn-search-report").on('click', function () {
            var did = $("#doctorSelect2").val();
            var month = 0;
            var year = 0;
            var currentYearMonth = $("#currentYearMonth").val();
            var array = currentYearMonth.split("-");
            var tempMonth = parseInt(array[0]);
            var tempYear = parseInt(array[1]);
            if (!isNaN(tempMonth)) {
                month = tempMonth;
            }
            if (!isNaN(tempYear)) {
                year = tempYear;
            }
            loadChartMedicalAppointmentAtention(did, month, year);
            loadDatatable(did, month, year);
        });        
    };
    return {
        load: function () {
            select2Doctors();
            datePickerInit();
            submitAction();
        }
    };
}();

$(function () {
    reportMedicalAppointmentAtention.load();
})