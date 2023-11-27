var InitApp = function () {
    var surveyId = $("#SurveyId").val();
    var datatable = {
        surveyUsers: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/admin/gestion-encuestas/getsurveyusers/${surveyId}`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON"
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Nombre Completo",
                        data: "fullName"
                    },
                    {
                        title: "Correo",
                        data: "email"
                    },
                    {
                        title: "Opciones",
                        orderable: false,
                        data: null,
                        render: function (data) {
                            var template = "";
                            var detailUrl = `/admin/gestion-encuestas/respuestas/detalle/${data.surveyUserId}`.proto().parseURL();
                            //Detail
                            template += `<a href="${detailUrl}"  `;
                            template += "class='btn btn-brand ";
                            template += "m-btn btn-sm m-btn--icon btn-detail' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-eye'></i><span>Detalle</span></span></a> ";
                            return template;
                        }
                    }
                ]
            },
            load: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
            }
        },
        init: function () {
            this.surveyUsers.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.surveyUsers.object.ajax.reload();
            });
        }
    };
    return {
        init: function () {
            datatable.init();
            search.init();
        }
    }
}();

$(function () {
    InitApp.init();
});
