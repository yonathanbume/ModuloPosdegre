var reportGraphicsTotalized = function () {
    var colorArray = ['#34bfa3', '#ffb822', '#f4516c', '#36a3f7', '#5867dd', '#9816f4'];

    var loadChartTotalizedManWoman = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-hombre-mujer`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        var count = 0;
                        for (var i = 0; i < data.length; i++) {
                            count += data[i].total;
                        }
                        if (count === 0) {
                            document.getElementById('chart_div_report_man_woman').innerHTML = "No hay datos disponibles";
                        } else {
                            PopulationChartTotalizedManWoman(data);
                        }
                    }
                    else {
                        document.getElementById('chart_div_report_man_woman').innerHTML = "No hay datos disponibles";
                    }
                }
            });

        }

        function PopulationChartTotalizedManWoman(data) {
            var dataArray = [
                ['', '']
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.sexdescription, item.total]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {
                colors: colorArray,
                is3D: true
            };
            var chart = new google.visualization.PieChart(document.getElementById('chart_div_report_man_woman'));
            chart.draw(data, options);
        }
    }

    var loadChartTotalizedUniversityPreparation = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-preparacion-universitaria`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedUniversityPreparation(data);
                    }
                    else {
                        document.getElementById('chart_div_report_university_preparation').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedUniversityPreparation(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];

            $.each(data, function (i, item) {
                dataArray.push([item.universitypreparationname, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);

            var options = {
                is3D: true,
                legend: { position: "none" }
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_university_preparation'));
            chart.draw(data, options);
        }
    }

    var loadChartTotalizedStudentCareer = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-estudiante-por-carrera`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedStudentCareer(data);
                    }
                    else {
                        document.getElementById('chart_div_report_student_career').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedStudentCareer(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.careername, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {

                legend: { position: "none" },
                is3D: true
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_student_career'));
            chart.draw(data, options);
        }
    }

    var loadChartTotalizedModalityType = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-tipo-modalidad`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedModalityType(data);
                    }
                    else {
                        document.getElementById('chart_div_report_modality_type').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedModalityType(data) {
            var dataArray = [
                ['', '']
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.admissiontypename, item.total]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);

            var options = {
                colors: colorArray,
                sliceVisibilityThreshold: 0,
                is3D: true
            };
            var chart = new google.visualization.PieChart(document.getElementById('chart_div_report_modality_type'));
            chart.draw(data, options);
        }
    }

    var loadChartTotalizedAgeRanges = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-rango-edades`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedAgeRanges(data);
                    }
                    else {
                        document.getElementById('chart_div_report_age_ranges').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedAgeRanges(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.rangeagedescription, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {

                legend: { position: "none" },
                is3D: true
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_age_ranges'));
            chart.draw(data, options);
        }
    }

    var loadChartDependencyEconomic = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-dependencia-economica`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedDependencyEconomic(data);
                    }
                    else {
                        document.getElementById('chart_div_report_dependency_economic').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedDependencyEconomic(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.dependencyname, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {

                legend: { position: "none" },
                is3D: true
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_dependency_economic'));
            chart.draw(data, options);
        }
    }

    var loadChartSchoolType = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-tipo-escuela`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedSchoolType(data);
                    }
                    else {
                        document.getElementById('chart_div_report_schooltype').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedSchoolType(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.schooltypename, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {
                legend: { position: "none" },
                is3D: true
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_schooltype'));
            chart.draw(data, options);
        }
    }

    var loadChartLevelEducation = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-nivel-de-educacion`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedLevelEducation(data);
                    }
                    else {
                        document.getElementById('chart_div_report_level_education').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedLevelEducation(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.leveleducationname, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {

                legend: { position: "none" },
                is3D: true
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_level_education'));
            chart.draw(data, options);
        }
    }

    var loadChartCivilStatus = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-estado-civil`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedCivilStatus(data);
                    }
                    else {
                        document.getElementById('chart_div_report_civil_status').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedCivilStatus(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.civilstatusname, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {

                legend: { position: "none" },
                is3D: true
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_civil_status'));
            chart.draw(data, options);
        }
    }

    var loadChartPrincipalPerson = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-persona-a-cargo`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedPrincipalPerson(data);
                    }
                    else {
                        document.getElementById('chart_div_report_principal_person').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedPrincipalPerson(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.principalpersondescription, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {

                legend: { position: "none" },
                is3D: true
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_principal_person'));
            chart.draw(data, options);
        }
    }

    var loadChartTotalRemuneration = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-remuneracion-total`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedTotalRemuneration(data);
                    }
                    else {
                        document.getElementById('chart_div_report_total_remuneration').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedTotalRemuneration(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.totalremunerationdescription, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {
                colors: ['#375ea1'],
                legend: { position: "none" },
                is3D: true
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_total_remuneration'));
            chart.draw(data, options);
        }
    }

    var loadChartStudentCoexistence = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-convivencia-de-alumnos`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedStudentCoexistence(data);
                    }
                    else {
                        document.getElementById('chart_div_report_student_coexistence').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedStudentCoexistence(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.studentcoexistencedescription, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {

                legend: { position: "none" },
                is3D: true
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_student_coexistence'));
            chart.draw(data, options);
        }
    }

    var loadChartFamilyRisk = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-riesgo-familiar`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedFamilyRisk(data);
                    }
                    else {
                        document.getElementById('chart_div_report_family_risk').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedFamilyRisk(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.familyriskdescription, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {

                legend: { position: "none" },
                is3D: true
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_family_risk'));
            chart.draw(data, options);
        }
    }

    var loadChartStudentTenure = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-tenencia`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedStudentTenure(data);
                    }
                    else {
                        document.getElementById('chart_div_report_Student_Tenure').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedStudentTenure(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.tenuredescription, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {

                legend: { position: "none" },
                is3D: true
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_Student_Tenure'));
            chart.draw(data, options);
        }
    }

    var loadChartStudentZoneType = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-tipo-zona`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedStudentZoneType(data);
                    }
                    else {
                        document.getElementById('chart_div_report_Student_ZoneType').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedStudentZoneType(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.zonetypedescription, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {

                legend: { position: "none" },
                is3D: true
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_Student_ZoneType'));
            chart.draw(data, options);
        }
    }

    var loadChartStudentConstructionType = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-tipo-construccion`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedConstructionType(data);
                    }
                    else {
                        document.getElementById('chart_div_report_Student_Construction_Type').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedConstructionType(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.constructiontypedescription, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {

                legend: { position: "none" },
                is3D: true
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_Student_Construction_Type'));
            chart.draw(data, options);
        }
    }

    var loadChartStudentBuildType = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-tipo-acabado`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedStudentBuildType(data);
                    }
                    else {
                        document.getElementById('chart_div_report_Student_BuildType').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedStudentBuildType(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.buildtypedescription, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {

                legend: { position: "none" },
                is3D: true
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_Student_BuildType'));
            chart.draw(data, options);
        }
    }

    var loadChartStudentConstructionCondition = function () {
        google.charts.load('current', {
            packages: ['corechart']
        });
        google.charts.setOnLoadCallback(LoadData);

        function LoadData() {

            $.ajax({
                type: "GET",
                url: `/admin/reporte-graficas-totales/reporte-condicion-de-contruccion`.proto().parseURL(),
                error: function (xhr, status, error) {
                    toastr.error(err.message);
                },
                success: function (data) {
                    if (data.length > 0) {
                        PopulationChartTotalizedStudentConstructionCondition(data);
                    }
                    else {
                        document.getElementById('chart_div_report_Student_Construction_Condition').innerHTML = "";
                    }
                }
            });

        }

        function PopulationChartTotalizedStudentConstructionCondition(data) {
            var dataArray = [
                ['', '', { role: 'annotation' }, { role: 'style' }]
            ];
            $.each(data, function (i, item) {
                dataArray.push([item.constructionconditiondescription, item.total, item.total, colorArray[i]]);
            });
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {

                legend: { position: "none" },
                is3D: true
            };
            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div_report_Student_Construction_Condition'));
            chart.draw(data, options);
        }
    }


    return {
        load: function () {
            mApp.block(".m-portlet", { type: "loader", message: "Cargando..." });
            loadChartTotalizedManWoman();
            loadChartTotalizedUniversityPreparation();
            loadChartTotalizedModalityType();
            loadChartTotalizedStudentCareer();
            loadChartTotalizedAgeRanges();
            loadChartDependencyEconomic();
            loadChartSchoolType();
            loadChartLevelEducation();
            loadChartCivilStatus();
            loadChartPrincipalPerson();
            loadChartTotalRemuneration();
            loadChartStudentCoexistence();
            loadChartFamilyRisk();
            loadChartStudentTenure();
            loadChartStudentZoneType();
            loadChartStudentConstructionType();
            loadChartStudentBuildType();
            loadChartStudentConstructionCondition();
            mApp.unblock(".m-portlet");
        }
    }
}();

$(function () {

    reportGraphicsTotalized.load();

})