var reportAlertDependencies = function () {
    var datatable = null;
    

    var loadDependenciesSelect = function () {
        $.ajax({
            url: `/dependencias/get`.proto().parseURL()
        }).done(function (data) {
            $("#select_dependencies").select2({
                data: data.items
            }).trigger('change');
        });
        $("#select_dependencies").on("change", function (e) {                      
            loadChartAlertAtention();
        });

    }

    var loadChartAlertAtention = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {            

            var did = $("#select_dependencies").val();
            $.ajax({
                type: "GET",
                url: `/admin/reporte_alertas_atencion/chart/${did}`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartAlertAtention(data);
                    }
                    else {
                        document.getElementById('chart_div_report_alert_atention').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartAlertAtention(data) {
            var dataArray = [
                ['', '']
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.alertStatus, item.alertCount]);
                
            });
            data = google.visualization.arrayToDataTable(dataArray);
            var options = {
                
                height: 400,
                width: 700,
                colors: ['#D32F2F', '#43A047'],
                titleFontSize: 20,
                is3D: true
            };
            var chart = new google.visualization.PieChart(document.getElementById('chart_div_report_alert_atention'));
            chart.draw(data, options);
        }
    }


    return {
        load: function () {            
            loadDependenciesSelect();
        }
    }
}();

$(function () {
    reportAlertDependencies.load();
})