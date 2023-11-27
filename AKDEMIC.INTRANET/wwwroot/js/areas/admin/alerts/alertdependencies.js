var reportAlertDependencies = function () {
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
                field: 'type',
                title: 'Tipo de alerta',
                width: 150,
                template: function (row) {
                    console.log(row);
                    if (row.type == 1) {
                        return `<span class="m-badge  m-badge--success m-badge--wide">Alerta Verde</span>`;
                    } else if (row.type == 2) {
                        return `<span class="m-badge  m-badge--warning m-badge--wide">Alerta Amarila</span>`;
                    } else if (row.type == 3) {
                        return `<span class="m-badge  m-badge--danger m-badge--wide">Alerta Roja</span>`;
                    }
                    else {
                        return ``;
                    }
                }
            },
            {
                field: 'description',
                title: 'Descripción',
                width: 100
            },
            {
                field: 'registerdate',
                title: 'Fecha de emisión',
                width: 180
            },                            
            {
                field: 'status',
                title: 'Estado',
                template: function (row) {
                    if (row.status == true) {
                        return `<span class="m-badge  m-badge--success m-badge--wide">Activo</span>`;
                    } else {
                        return `<span class="m-badge  m-badge--metal m-badge--wide">Inactivo</span>`;
                    }

                }
            }
        ]
    }

    var loadDependenciesSelect = function () {
        $.ajax({
            url: `/dependencias/get`.proto().parseURL()
        }).done(function (data) {
            $("#select_dependencies").select2({
                data: data.items
            }).trigger('change');
        });
        $("#select_dependencies").on("change", function (e) {                      
            loadDatatable();              
        });

    }
    var loadDatatable = function () {
        var pid = $("#select_dependencies").val();        

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        options.data.source.read.url = `/admin/reporte_alerta_dependencias/${pid}/get`.proto().parseURL();
        datatable = $(".m-datatable").mDatatable(options);

    }


    var loadChartAlertDependencies = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {            

            $.ajax({
                type: "GET",
                url: `/admin/reporte_alerta_dependencias/chart`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartApprobedDisapprobedCourse(data);
                    }
                    else {
                        document.getElementById('chart_div_report_alert_dependencies').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartApprobedDisapprobedCourse(data) {
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
                //colors: ['#D32F2F', '#43A047'],
                titleFontSize: 20,
                is3D: true
            };
            var chart = new google.visualization.PieChart(document.getElementById('chart_div_report_alert_dependencies'));
            chart.draw(data, options);
        }
    }


    return {
        load: function () {
            loadChartAlertDependencies();
            loadDependenciesSelect();
        }
    }
}();

$(function () {
    reportAlertDependencies.load();
})