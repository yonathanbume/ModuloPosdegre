var reportApproved = function () {
    var datatable = null;

    var options = getSimpleDataTableConfiguration({
        url: `/director-carrera/reporte-aprobados-desaprobados/get`.proto().parseURL(),
        data: function (data) {
            data.cid = $("#select_career").val();
            data.pid = $("#select_terms").val();
            data.name = $("#search").val();
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
                title: 'Especialidad',
            },
            {
                data: 'observations',
                title: 'Mérito',
                render: function (row) {
                    if (row === 0) {
                        return `<span >${_app.constants.academicOrder.upperThird.text}</span>`;
                    } else if (row === 1) {
                        return `<span >${_app.constants.academicOrder.upperFifth.text}</span>`;
                    } else {
                        return `<span >${_app.constants.academicOrder.upperTenth.text}</span>`;
                    }

                }
            },
            {
                data: 'order',
                title: 'Orden de Mérito',
            },
            {
                data: 'finalgrade',
                title: 'Nota final',
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
        ]
    });

    var search = function () {
        var timing = 0;
        $("#search").keyup(function () {
            clearTimeout(timing);
            timing = setTimeout(function () {
                loadDatatable();
            }, 500);
        });
    };
    var loadCareerSelect = function () {
        $("#select_career").select2({
            ajax: {
                delay: 1000,
                url: `/director-carrera/reporte-aprobados-desaprobados/getCarreras`.proto().parseURL(),
                data: function (params) {
                    var query = {
                        search: params.term,
                    };
                    return query;
                },
            },
            allowClear: true,
            minimumInputLength: 0,
            placeholder: "Seleccione carrera"
        }).on("change", function (e) {
            loadChartApprobedDisapprobed();
            loadDatatable();
        });
        // $.ajax({
        //     url: `/director-carrera/reporte-aprobados-desaprobados/getCarreras`.proto().parseURL()
        // }).done(function (data) {
        //     $("#select_career").empty();
        //     $("#select_career").select2({
        //         data: data.items,
        //         placeholder: "Carreras"
        //     }).trigger('change');
        // });
        // $("#select_career").on("change", function (e) {
        //     loadChartApprobedDisapprobed();
        //     loadDatatable();
        // });

    }
    var loadTermsSelect = function () {
        $.ajax({
            url: `/periodos/get`.proto().parseURL()
        }).done(function (data) {
            $("#select_terms").select2({
                data: data.items
            }).val(data.selected).trigger('change');
        });
        $("#select_terms").on("change", function (e) {
            loadChartApprobedDisapprobed();
            loadDatatable();
        });

    }
    var loadDatatable = function () {

        if (datatable === null) {
            datatable = $("#datatable_index").DataTable(options);
        } else {
            datatable.ajax.reload();
        }
    }

    var initializer = function () {
        $.when(loadCareerSelect(), loadTermsSelect()).done(function () {
            loadChartApprobedDisapprobed();
            loadDatatable();
            search();
        });
    }
    var loadChartApprobedDisapprobed = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {
            var pid = $("#select_terms").val();
            var cid = $("#select_career").val();

            $.ajax({
                type: "GET",
                url: `/director-carrera/reporte-aprobados-desaprobados/chart/${cid}/${pid}`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartApprobedDisapprobed(data);
                    }
                    else {
                        document.getElementById('chart_div_report_approbed_disapprobed').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartApprobedDisapprobed(data) {
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
                colors: ['#D32F2F', '#43A047'],
                titleFontSize: 20,
                is3D: true
            };
            var chart = new google.visualization.PieChart(document.getElementById('chart_div_report_approbed_disapprobed'));
            chart.draw(data, options);
        }
    }


    return {
        load: function () {
            initializer();
        }
    }
}();

$(function () {
    reportApproved.load();
})