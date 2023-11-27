var Absences = function () {
    var datatable;
    var formValidate = null;

    var options = {
        data: {
            type: "remote",
            source: {
                read: {
                    method: "GET",
                    url: ("/justificacion-inasistencias/get").proto().parseURL(),
                }
            }
        },
        columns: [
            {
                field: "date",
                title: "Fecha de la Inasistencia",
                textAlign: "center",
                width: 180
            },
            {
                field: "registerDate",
                title: "Fecha de la Solicitud",
                textAlign: "center",
                width: 220
            },
            {
                field: "approved",
                title: "Estado",
                textAlign: "center",
                width: 150,
                template: function (row) {
                    var status = {
                        3: { text: _app.constants.request.inProcess.text, value: _app.constants.request.inProcess.value, class: "m-badge--metal" },
                        4: { text: _app.constants.request.approved.text, value: _app.constants.request.approved.value, class: "m-badge--success" },
                        5: { text: _app.constants.request.disapproved.text, value: _app.constants.request.disapproved.value, class: "m-badge--danger" }
                    };
                    return "<span class='m-badge " + status[row.status].class + " m-badge--wide'>" + status[row.status].text + "</span>";
                }
            },
            {
                field: "options",
                title: "Opciones",
                width: 180,
                template: function (row) {
                    var tmp = "";
                    tmp += "<button class='btn btn-default btn-sm m-btn--icon btn-detail' data-id='" + row.id + "'><span><i class='la la-eye'></i><span>Ver Detalle</span></span></button> ";
                    if (row.canDelete) {
                        tmp += "<button class='btn btn-danger btn-sm m-btn--icon btn-delete' data-id='" + row.id + "'><i class='la la-trash'></i></button>";
                    }
                    return tmp;
                }
            }
        ]
    }

    var events = {
        init: function () {
            $("#btn-add").on("click", function () {
                form.load.create();
            });

            datatable.on("click", ".btn-detail", function () {
                var id = $(this).data("id");
                detail.show(id);
            });

            datatable.on("click", ".btn-delete", function () {
                var id = $(this).data("id");
                swal({
                    title: "¿Está seguro?",
                    text: "La solicitud será eliminada permanentemente",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarla",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: "/justificacion-inasistencias/eliminar/post",
                                type: "POST",
                                data: {
                                    id: id
                                },
                                success: function (result) {
                                    datatable.reload();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "La solicitud ha sido eliminada con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "Ocurrió un error al intentar eliminar la solicitud"
                                    });
                                }
                            });
                        });
                    }
                });
            });
        }
    }

    var select2 = {
        init: function () {
            this.absences.init();
        },
        absences: {
            init: function () {
                $.ajax({
                    url: "/personal/inasistencias/get"
                }).done(function (result) {
                    $(".select2-absences").select2({
                        data: result.items,
                        dropdownParent: $("#add_modal")
                    });
                });
            }
        }
    }

    var form = {
        submit: {
            create: function (formElement) {
                var data = new FormData($(formElement).get(0));
                var emptyFile = $("#File").get(0).files.length === 0;
                $(formElement).find("input").prop("disabled", true);
                $(formElement).find("textarea").prop("disabled", true);
                $(formElement).find("select").prop("disabled", true);
                $(formElement).find("#btnCreate").addLoader();
                if (!emptyFile) {
                    $(formElement).find(".custom-file").append(
                        "<div id='space-bar' class='m--space-10'></div><div class='progress m-progress--sm'><div class='progress-bar progress-bar-striped progress-bar-animated m--bg-primary' role='progressbar'></div></div>");
                    $(".progress-bar").width("0%");
                }
                $.ajax({
                    url: $(formElement).attr("action"),
                    type: "POST",
                    contentType: false,
                    processData: false,
                    data: data,
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        if (!emptyFile) {
                            xhr.upload.onprogress = function(evt) {
                                if (evt.lengthComputable) {
                                    var percentComplete = evt.loaded / evt.total;
                                    percentComplete = parseInt(percentComplete * 100);
                                    $(".progress-bar").width(percentComplete + "%");
                                }
                            }
                        }
                        return xhr;
                    }
                })
                    .always(function () {
                        $(formElement).find("input").prop("disabled", false);
                        $(formElement).find("textarea").prop("disabled", false);descargar
                        $(formElement).find("select").prop("disabled", false);
                        $(formElement).find("#btnCreate").removeLoader();
                        $(".progress").empty().remove();
                        if (!emptyFile) {
                            $("#space-bar").remove();
                        }
                    })
                    .done(function () {
                        $("#add_modal").modal("toggle");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        datatable.reload();
                    }).fail(function (e) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (e.responseText != null) $("#add_form_msg_txt").html(e.responseText);
                        else $("#add_form_msg_txt").html(_app.constant.ajax.message.error);
                        $("#add_form_msg").removeClass("m--hide").show();
                    });
            }
        },
        reset: {
            create: function () {
                formValidate.resetForm();
                $("#add_form_msg").addClass("m--hide").hide();
            }
        },
        load: {
            create: function() {
                $("#add_modal").modal("show");
                $("#add_modal").one("hidden.bs.modal",
                    function () {
                        form.reset.create();
                    });
            }
        }
    }

    var validate = {
        init: function () {
            formValidate = $("#add-form").validate({
                rules: {
                    File: {
                        fileSizeMBs: 20
                    }
                },
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.create(formElement);
                }
            });
        }
    }

    var detail = {
        show: function (id) {
            mApp.blockPage();
            $.ajax({
                url: ("/justificacion-inasistencias/" + id + "/get").proto().parseURL()
            }).done(function (result) {
                var formElements = $("#detail-form").get(0).elements;
                formElements["DetailDate"].value = result.date;
                formElements["DetailRegisterDate"].value = result.registerDate;
                var status = {
                    3: { text: _app.constants.request.inProcess.text },
                    4: { text: _app.constants.request.approved.text },
                    5: { text: _app.constants.request.disapproved.text }
                };
                formElements["DetailStatus"].value = status[result.status].text;
                formElements["DetailJustification"].value = result.justification;
                if (result.file) {
                    $("#fileDiv").show();
                    $("#fileUrl").attr("href", `/justificacion-inasistencias/${id}/archivo/descargar`.proto().parseURL());
                } else {
                    $("#fileDiv").hide();
                }
                mApp.unblockPage();
                $("#detail_modal").modal("toggle");
                $("#detail_modal").on("hidden.bs.modal", function () {
                    detail.reset();
                });
            });
        },
        reset: function () {
            var formElements = $("#detail-form").get(0).elements;
            formElements["DetailDate"].value = null;
            formElements["DetailRegisterDate"].value = null;
            formElements["DetailStatus"].value = null;
            formElements["DetailJustification"].value = null;
        }
    }

    return {
        init: function () {
            datatable = $(".m-datatable").mDatatable(options);
            validate.init();
            events.init();
            select2.init();
        }
    }
}();

$(function () {
    Absences.init();
})