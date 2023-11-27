var ReportTeacher = function () {
    var datatable = null;
    var options = {
        data: {
            type: 'remote',
            source: {
                read: {
                    method: 'GET',
                    url: ''
                }
                
            },
            pageSize: 10,
            saveState: {
                cookie: true,
                webstorage: true
            }
        },

        columns: [
            {
                field: 'name',
                title: 'Nombre',
                width: 150
            },
            {
                field: 'assistance',
                title: 'Asistencias',
                width: 80
            },
            {
                field: 'nonAssistance',
                title: 'Inasistencias',
                width: 100
            },
            {
                field: 'firstLate',
                title: 'Tardanzas Mañana',
                width: 100
            },
            {
                field: 'secondLate',
                title: 'Tardanzas Tarde',
                width: 100
            },
        ]
    };

    var initFormDatepickers = function () {    

        $("#select_initdate").datepicker();
        $("#select_finishdate").datepicker();

        $("#select_initdate").datepicker()
            .on("changeDate", function (e) {

                $("#select_finishdate").datepicker('setStartDate', e.date);
            });


        $("#select_finishdate").datepicker()
            .on("changeDate", function (e) {

                $("#select_initdate").datepicker("setEndDate", e.date);
            }); 
    };

    var loadDatatable = function (startDate, endDate) {

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }

        options.data.source.read.url = `/admin/reporte_asistencia_docentes/getReport?initDate=${startDate}&finishDate=${endDate}`.proto().parseURL();
        datatable = $(".m-datatable").mDatatable(options);

    };
    var searchReport = function () {
        $("#btn-search").on("click", function () {

            var startDate = $("#select_initdate").datepicker({ dateFormat: 'dd/mm/yyyy' }).val();
            var endDate = $("#select_finishdate").datepicker({ dateFormat: 'dd/mm/yyyy' }).val();
            loadDatatable(startDate, endDate);
        });
    };


    return {
        init: function () {
            initFormDatepickers();
            searchReport();

        }
    };

}();


$(function () {
    ReportTeacher.init();
});