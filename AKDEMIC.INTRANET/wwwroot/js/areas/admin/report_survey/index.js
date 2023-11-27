var InitApp = function () {
    var datatable = {
        surveys: {
            object: null,
            options: getSimpleDataTableConfiguration({
                url: `/admin/reporte_encuesta/listar`.proto().parseURL(),
                data: function (data) {
                    delete data.columns;
                    data.search = $("#search").val();
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Codigo",
                        data: "code"
                    },
                    {
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "Fecha de Publicación",
                        data: "publishDate"
                    },
                    {
                        title: "Encuestas Enviadas",
                        data: "surveyuserCount"
                    },
                    {
                        title: "Encuestas Respondidas",
                        data: "surveyAnswered"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        render: function (data) {
                            var detailUrl = `/admin/reporte_encuesta/${data.id}/detalle`.proto().parseURL();
                            var excelUrl = `/admin/reporte_encuesta/${data.id}/reporte-excel`.proto().parseURL();
                            var template = "";
                            //Detail
                            template += `<a href="${detailUrl}"  `;
                            template += "class='btn btn-brand ";
                            template += "m-btn btn-sm m-btn--icon btn-detail' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-eye'></i><span>Ver Resultados</span></span></a> ";
                            //Excel
                            if (!data.isAnonymous) {
                                template += `<a href="${excelUrl}"  `;
                                template += "class='btn btn-success ";
                                template += "m-btn btn-sm m-btn--icon' ";
                                template += " data-id='" + data.id + "'>";
                                template += "<span><i class='flaticon-file'></i></a> ";
                            }
                            return template;
                        }
                    }
                ]
            }),
            init: function () {
                datatable.surveys.object = $("#data-table").DataTable(datatable.surveys.options);
            },
            reload: function () {
                datatable.surveys.object.ajax.reload();
            }
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.surveys.object.ajax.reload();
            });
        }
    };

    var other = {
        init: function () {
            $(".download-report").click(function () {
                var that = $(this);
                that.attr("disabled", "disabled");
                var url = "/admin/reporte_encuesta/reporte";

                $.fileDownload(url,
                    {
                        httpMethod: 'GET',
                        successCallback: function () {
                            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                            that.removeAttr("disabled");
                        }
                    }
                ).done(function () {
                    that.removeAttr("disabled");
                })
                    .fail(function () { alert('File download failed!'); })
                    .always(function () {
                        console.log("always");
                    });
            });
        }
    };


    return {
        init: function () {
            datatable.surveys.init();
            search.init();
            other.init();
        }
    };
}();
$(function () {
    InitApp.init();
});

