var surveyReport = function () {
    var datatable = {
        object: null,
        options: getSimpleDataTableConfiguration({
            url: `/director-carrera/reporte-encuesta/get-usuarios`.proto().parseURL(),
            data: function (data) {
                data.search = $("#search").val();
                data.surveyId = $("#Id").val()
            },
            pageLength: 50,
            orderable: [],
            columns: [
                {
                    title: "Estudiante",
                    data: "student"
                },
                {
                    title: "Fecha",
                    data: "date"
                }
            ]
        }),
        reload: function () {
            datatable.object.ajax.reload();
        },
        init: function () {
            datatable.object = $("#data-table").DataTable(datatable.options);
        }
    };

    var reportSection = {
        load: function () {
            $.ajax({
                url: (`/director-carrera/reporte-encuesta/get-preguntas?surveyId=`+$("#Id").val()).proto().parseURL(),
                type: "GET",
                dataType: 'html'
            }).done(function (data) {
                console.log(data);
                $("#questions").html(data);
            }).fail(function (error) {
                console.log(error);
                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
            });
        },
        init: function () {
            this.load();
        }
    };

    return {
        init: function () {
            datatable.init();
            reportSection.init();
        }
    };
}();

$(function () {
    surveyReport.init();
});