var InitApp = function () {
    var requiredEnabled = $("#surveyRequiredFeature").val();
    var datatable = {
        surveys: {
            object: null,
            options: getSimpleDataTableConfiguration({
                url: `/admin/gestion-encuestas/get`.proto().parseURL(),
                data: function (data) {
                    delete data.columns;
                    data.searchValue = $("#search").val()
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Código",
                        data: "code"
                    },
                    {
                        title: "Estado",
                        data: "status"
                    },
                    {
                        title: "Nombre",
                        data: "title"
                    },
                    {
                        title: "Fecha Publicación",
                        data: "publishDate"
                    },
                    {
                        title: "Fecha Finalización",
                        data: "finishDate"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        render: function (data) {
                            var template = "";
                            var detailUrl = `/admin/gestion-encuestas/editar/${data.id}`.proto().parseURL();
                            var answersUrl = `/admin/gestion-encuestas/respuestas/${data.id}`.proto().parseURL();
                            //Detail
                            template += `<a href="${detailUrl}"  `;
                            template += "class='btn btn-secondary ";
                            template += "m-btn btn-sm m-btn--icon' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-edit'></i><span>Detalle</span></span></a> ";
                            //Answers
                            template += `<a href="${answersUrl}"  `;
                            template += "class='btn btn-secondary ";
                            template += "m-btn btn-sm m-btn--icon' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-eye'></i><span>Respuestas</span></span></a> ";
                            if (data.statusId == _app.constants.survey_states.notsent) {
                                //Delete
                                template += "<button ";
                                template += "class='btn btn-danger btn-delete ";
                                template += "m-btn btn-sm  m-btn--icon-only' ";
                                template += " data-id='" + data.id + "'>";
                                template += "<i class='la la-trash'></i></button>";
                            }
                            return template;
                        }
                    }
                ]
            }),
            init: function () {
                datatable.surveys.object = $("#data-table").DataTable(datatable.surveys.options);
                datatable.surveys.events();
            },
            reload: function () {
                datatable.surveys.object.ajax.reload();
            },
            events: function () {
                $("#data-table").on('click', '.btn-delete', function (e) {
                    var id = $(this).data('id');
                    form.delete(id);
                })
            }
        },
        init: function () {
            datatable.surveys.init();
        }
    };
    var form = {
        create: {
            object: $("#create-form").validate({
                submitHandler: function (e) {
                    $("#btn-Add").addLoader();
                    $.ajax({
                        url: $(e).attr("action"),
                        type: 'POST',
                        data: $(e).serialize()
                    }).done(function () {
                        $("#create_modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        datatable.surveys.reload();
                    }).fail(function (e) {
                        if (e.responseText !== null && e.responseText !== "") $("#create_msg_txt").html(e.responseText);
                        else $("#create_msg_txt").html(_app.constant.ajax.message.error);
                        $("#create_msg").removeClass("m--hide").show();
                        $("#btn-Add").removeLoader();
                    });
                }
            }),
            show: function () {
                $("#create_modal").modal("toggle");
                $("#btn-Add").removeLoader();
                form.create.clear();
            },
            events: function(){
                $("#addSurvey").on("click", function () {
                    form.create.show();
                });
            },
            clear: function () {
                form.create.object.resetForm();
                $("#create_msg").addClass("m--hide");
                $("#PublicationDate").datepicker('update', '');
                $("#FinishDate").datepicker('update', '');
                $("#PublicationDate").datepicker("setStartDate", moment().toDate());
                $("#PublicationDate").datepicker("setEndDate", false);
                $("#FinishDate").datepicker("setStartDate", moment().add(1, 'd').toDate());
                $("#FinishDate").datepicker("setEndDate", false);
            }
        },
        delete: function(id){
            swal({
                title: "¿Está seguro?",
                text: "La encuesta será eliminada permanentemente",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarla",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/admin/gestion-encuestas/eliminar").proto().parseURL(),
                            type: "POST",
                            data: {
                                id: id
                            },
                            success: function (result) {
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "La Encuesta ha sido eliminada con exito",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.surveys.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "La Encuesta presenta información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },
        init: function () {
            form.create.events();
        }
    };
    var datePickers = {
        init: function () {
            $("#PublicationDate").datepicker("setStartDate", moment().toDate());
            $("#FinishDate").datepicker("setStartDate", moment().add(1, 'd').toDate());

            $("#PublicationDate").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            }).on("changeDate", function (e) {
                $("#FinishDate").datepicker("setStartDate", moment(e.date).add(1, 'd').toDate());
            });

            $("#FinishDate").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            }).on("changeDate", function (e) {
                $("#PublicationDate").datepicker("setEndDate", e.date);
            });
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.surveys.reload();
            });
        }
    };
    return {
        init: function () {
            if (requiredEnabled) {
                $("#requiredContainer").css("display", "block");
            }
            datatable.init();
            form.init();
            search.init();
            datePickers.init();
        }
    }
}();

$(function () {
    InitApp.init();
});