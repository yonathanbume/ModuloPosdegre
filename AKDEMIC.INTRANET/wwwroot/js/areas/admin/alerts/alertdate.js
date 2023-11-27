
var reportAlertDates = function () {
    var datatable = null;


    var datepickers = function () {
        $("#StartDate").datepicker({
            format: _app.constants.formats.datepicker
        }).one("changeDate", function (e) {
            $("#EndDate").datepicker("setStartDate", e.date);
        });

        $("#EndDate").datepicker({
            format: _app.constants.formats.datepicker
        }).one("changeDate", function (e) {
            $("#StartDate").datepicker("setEndDate", e.date);
        });
    }

    var method = function () {
        $("#btn-search").on('click', function () {
            var startDate = $("#StartDate").val();
            var endDate = $("#EndDate").val();

            if (startDate === null || startDate === "") {
                toastr.error("Ingresar fecha de inicio", "Error");
                return;
            }

            if (endDate === null || endDate === "") {
                toastr.error("Ingresar fecha fin", "Error");
                return;
            }

            $("#btn-search").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
            loadChartAlertDate();
        });
    };

    var loadChartAlertDate = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            var StarDate = $("#StartDate").val();
            var EndDate = $("#EndDate").val();
            $.ajax({
                type: "GET",
                url: `/admin/reporte_alertas_fechas/chart?StartDate=${StarDate}&EndDate=${EndDate}`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(_app.constants.toastr.message.error.get, _app.constants.toastr.title.error);
                    $("#btn-search").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartAlertDate(data);
                    }
                    else {
                        document.getElementById('chart_div_report_alert_date').innerHTML = "";
                    }
                    $("#btn-search").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                }
            });

        }

        function PopulationChartAlertDate(data) {
            var dataArray = [
                ['', '']
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.dependency.name, item.alertCount]);

            });
            data = google.visualization.arrayToDataTable(dataArray);
            var options = {

                height: 400,
                width: 700,
                colors: ['#D32F2F', '#43A047'],
                titleFontSize: 20,
                is3D: true
            };
            var chart = new google.visualization.PieChart(document.getElementById('chart_div_report_alert_date'));
            chart.draw(data, options);
        }
    }


    return {
        load: function () {
            datepickers();
            method();
        }
    }
}();

$(function () {
    reportAlertDates.load();
})