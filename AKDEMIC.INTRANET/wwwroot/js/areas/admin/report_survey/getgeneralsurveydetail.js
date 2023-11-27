var InitApp = function () {
    var SurveyId = $("#SurveyId").val();
    var datatable = {
        users: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/admin/reporte_encuesta/usuarios/listar`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.surveyId = SurveyId;
                        data.searchValue = $("#search").val();
                        data.answered = $("#answered").val();
                    }
                },
                pageLength: 20,
                orderable: [],
                columns: [
                    {
                        title: "Nombre Completo",
                        data: "user"
                    },
                    {
                        title: "Estado de Respuesta",
                        data: "answered",
                        orderable: false
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
            },
        },
        init: function () {
            this.users.init();
        }
    };
    var select2 = {
        init: function () {
            this.answered.init();
        },
        answered: {
            init: function () {
                $("#answered").select2();
                this.events();
            },
            events: function () {
                $("#answered").on("change", function () {
                    datatable.users.reload();
                });
            }
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.users.reload();
            });
        }
    };
    var reportSection = {
        init: function () {
            this.load();
        },
        load: function () {
            $.ajax({
                url: (`/admin/reporte_encuesta/${SurveyId}/preguntas`).proto().parseURL(),
                type: "GET",
                dataType: 'html'
            }).done(function (data) {
                $("#questions").html(data);
                $("#questionsLoader").css("display", "none")
                $(".table-class-question").each(function () {
                    var questionId = $(this).attr("id");
                    var urlQuestion = `/admin/gestion-encuestas/pregunta/${questionId}/respuestas`;
                    $(`#${questionId}`).DataTable({
                        serverSide: true,
                        filter: false,
                        lengthChange: false,
                        ajax: {
                            url: urlQuestion,
                            type: "GET",
                            dataType: "JSON"
                        },
                        pageLength: 10,
                        orderable: [],
                        columns: [
                            {
                                title: "Respuesta",
                                orderable: false,
                                data: "answer"
                            },
                        ],
                        dom: 'Bfrtip',
                        buttons: [
                            'excel', 'pdf'
                        ]
                    });
                });
            }).fail(function (error) {
                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
            }).always(function () {
            })
        },
    };
    return {
        init: function () {
            datatable.init();
            reportSection.init();
            select2.init();
            search.init();
        }
    };
}();

$(function () {
    InitApp.init();
});

