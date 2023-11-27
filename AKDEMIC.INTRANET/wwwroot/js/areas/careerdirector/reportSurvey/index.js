var reportSurvey = function () {
    var dataTable = {
        object: null,
        options: getSimpleDataTableConfiguration({
            url: `/director-carrera/reporte-encuesta/getAllSurveys`.proto().parseURL(),
            data: function (data) {
                data.careerId = $("#select_career").val();
                data.search = $("#search").val();
            },
            pageLength: 10,
            orderable: [],
            columns: [
                {
                    data: 'code',
                    title: 'Código'
                },
                {
                    data: "status",
                    title: "Estado"
                },
                {
                    data: 'title',
                    title: 'Nombre'
                },
                {
                    data: 'answers',
                    title: 'Respuestas'
                },
                {
                    data: 'publishDate',
                    title: 'Fecha de publicación'
                },
                {
                    data: 'finishDate',
                    title: 'Fecha de finalización'
                },
                {
                    data: null,
                    title: "Opciones",
                    render: function (row) {
                        return `<button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span>Ver reporte </span></span></button>`;
                    }
                }
            ]
        }),
        reloadTable: function () {
            dataTable.object.ajax.reload();
        },
        events: {
            onView: function () {
                $("#datatable_survey").on("click", '.btn-detail', function () {
                    var eid = $(this).data("id");
                    location.href = `/director-carrera/reporte-encuesta/detalle/${eid}`.proto().parseURL();
                });
            },
            init: function () {
                this.onView();
            }
        },
        init: function () {
            this.events.init();
        }
    };
    var input = {
        init: function () {
            $("#search").doneTyping(function () {
                dataTable.reloadTable();
            });
        }
    };
    var loadCareerSelect = function () {
        $.ajax({
            url: `/director-carrera/reporte-encuesta/carreras/get`.proto().parseURL()
        }).done(function (data) {
            $("#select_career").select2({
                data: data.items,
                placeholder: "Carreras"
            }).trigger('change');
        });
        $("#select_career").on("change", function (e) {
            if (dataTable.object === null) {
                dataTable.object = $("#datatable_survey").DataTable(dataTable.options);
            } else {
                dataTable.reloadTable();
            }
        });
    }
    return {
        init: function () {
            loadCareerSelect();
            input.init();
            dataTable.init();
        }
    };

}();

$(function () {
    reportSurvey.init();
});