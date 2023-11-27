var reportEventDetail = function () {
    var eid = $("#Id").val();
    var loadChartAssistences = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte_evento/getChartAbsents/${eid}`.proto().parseURL(),
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    toastr.error(err.message);
                },
                success: function (data) {
                    PopulationChartAbsent(data);
                }
            });

        }

        function PopulationChartAbsent(data) {
            var dataArray = [
                ['', '']
            ];
            if (data != null && data.length > 0) {
                $.each(data, function (i, item) {
                    dataArray.push([item.presentsName, item.presents]);
                    dataArray.push([item.absentsName, item.absents]);
                });
                var data = google.visualization.arrayToDataTable(dataArray);
                var options = {
                    title: "Presentes vs ausentes",
                    height: 300,
                    width: 450
                };
                var chart = new google.visualization.PieChart(document.getElementById('chart_div_report_assitence'));
                chart.draw(data, options);
                $("#chart_div_report_assitence_no_data").hide();
                $("#chart_div_report_assitence").show();
            } else {
                $("#chart_div_report_assitence").hide();
                $("#chart_div_report_assitence_no_data").show();  
            }
            return false;
        }
    }
    var loadChartCosts = function () {
        google.charts.load('current', { 'packages': ['bar'] });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte_evento/getChartCost/${eid}`.proto().parseURL(),
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    toastr.error(err.message);
                },
                success: function (data) {
                    PopulationChartCost(data);
                }
            });

        }

        function PopulationChartCost(data) {
            var dataArray = [
                ['Costo Totalizado', 'Costo Esperado', 'Costo Real']
            ];
            if (data != null && data.length > 0) {
                $.each(data, function (i, item) {
                    dataArray.push([item.cost, item.expectedCost, item.realCost]);
                });
                var data = google.visualization.arrayToDataTable(dataArray);
                var options = {
                    height: 300,
                    width: 450,
                    colors: ['#d95f02', '#1b9e77']
                };
                var chart = new google.charts.Bar(document.getElementById('chart_div_cost'));
                chart.draw(data, google.charts.Bar.convertOptions(options));
                $("#chart_div_cost_no_data").hide();
                $("#chart_div_cost").show();  
            } else {
                $("#chart_div_cost").hide();  
                $("#chart_div_cost_no_data").show();  
            }
            return false;
        }
    }

    return {
        load: function () {
            loadChartAssistences();
            loadChartCosts();
        }
    }
}();

$(function () {
    reportEventDetail.load();
})