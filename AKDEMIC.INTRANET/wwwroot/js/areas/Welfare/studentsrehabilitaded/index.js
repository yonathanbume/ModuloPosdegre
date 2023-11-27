var reportstudentrehabilitaded = function () {
    var datatable = null;

    var options = {
        ajax: {
            url: `/welfare/reporte-estudiantes-rehabilitados/detalle`.proto().parseURL(),
            type : "GET"
        },
        columns: [

            {
                data: 'code',
                title: 'Código'
            },
            {
                data: 'name',
                title: 'Nombres Completos'
            },
            {
                data: 'email',
                title: 'Email'
            },
            {
                data: 'faculty',
                title: 'Facultad'
            },
            {
                data: 'status',
                title: 'Estado',
                render: function (row) {
                    if (row.isrehabilitaded == true) {
                        return `<span class="m-badge  m-badge--success m-badge--wide">Rehabilitado</span>`
                    } else {
                        return `<span class="m-badge  m-badge--danger m-badge--wide">Por rehabilitar</span>`
                    }

                }
            }
        ]
    };

    var loadDatatable = function () {
        if (datatable === null) {
            datatable = $("#ajax_datatable").DataTable(options);
        } else {
            datatable.ajax.reload();
        }
    };

    var loadChartApprobedDisapprobedCourse = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {
            mApp.block("#container_chart", {
                message : "Cargando gráfico..."
            });

            $.ajax({
                type: "GET",
                url: `/welfare/reporte-estudiantes-rehabilitados/chart`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartStudentRehabilitaded(data);
                    }
                    else {
                        document.getElementById('chart_div_report_student_rehabilitaded').innerHTML = "No hay datos para mostrar.";
                    }
                },
                complete: function () {
                    mApp.unblock("#container_chart", {
                        message: "Cargando gráfico..."
                    });
                }
            });

        }

        function PopulationChartStudentRehabilitaded(data) {
            var dataArray = [
                ['', '']
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.rehabilitadedname, item.rehabilitadeds]);
                dataArray.push([item.norehabilitadedname, item.norehabilitadeds]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {
                title: "Rehabilitados vs Sin Rehabilitar",
                height: 400,
                width: 700,
                colors: ['#43A047', '#D32F2F'],
                titleFontSize: 20,
                is3D: true
            };
            var chart = new google.visualization.PieChart(document.getElementById('chart_div_report_student_rehabilitaded'));
            chart.draw(data, options);
        }
    };


    return {
        load: function () {
            loadChartApprobedDisapprobedCourse();
            loadDatatable();
        }
    }
}();

$(function () {
    reportstudentrehabilitaded.load();
})