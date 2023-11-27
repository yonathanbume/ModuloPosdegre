var index = function () {

    var datatable = {
        portfolio: {
            object: null,
            options: {
                serverSide: false,
                pageLength: 50,
                ajax: {
                    url: "/alumno/portafolio/get-datatable"
                },
                columns: [
                    {
                        data: "name",
                        title: "Requisito"
                    },
                    {
                        data: null,
                        title: "Entregado",
                        orderable: false,
                        render: function (data) {
                            if (data.received) {
                                return `<span class="m-badge m-badge--success m-badge--wide"> Si </span>`;
                            }
                            return `<span class="m-badge m-badge--danger m-badge--wide"> No </span>`;
                        }
                    },
                    {
                        data: null,
                        title: "Validado",
                        orderable: false,
                        render: function (data) {
                            if (data.validated) {
                                return `<span class="m-badge m-badge--success m-badge--wide"> Si </span>`;
                            }
                            return `<span class="m-badge m-badge--danger m-badge--wide"> No </span>`;
                        }
                    },
                    {
                        data: null,
                        title: "Documento",
                        orderable: false,
                        render: function (data) {
                            var tpm = "";

                            if (data.file == null || data.file == "") {
                                tpm += `<button class='btn btn-primary btn-upload m-btn btn-sm' data-id='${data.id}' data-name='${data.name}'><i class='la la-cloud-upload'></i> Subir</button> `;
                            }
                            else {
                                tpm += `<button class='btn btn-primary btn-upload m-btn btn-sm' data-id='${data.id}' data-name='${data.name}'><i class='la la-cloud-upload'></i> Reemplazar</button> `;
                                tpm += `<a href="/documentos/${data.file}" target="_blank" class='btn btn-info btn-sm m-btn m-btn--icon'><span><i class='la la-download'></i><span>Descargar</span></span></a>`;
                            }

                            return tpm;
                        }
                    },
                ]
            },
            reload: function () {
                datatable.portfolio.object.ajax.reload();
            },
            events: {
                onLoadFile: function () {
                    $("#datatable").on("click", ".btn-upload", function () {
                        var id = $(this).data("id");
                        var name = $(this).data("name");

                        modal.portfolio.object.find("[name='typeId']").val(id);
                        modal.portfolio.object.find(".modal-title").text(name);
                        modal.portfolio.object.modal("show");
                    })
                },
                init: function () {
                    this.onLoadFile();
                }
            },
            init: function () {
                datatable.portfolio.object = $("#datatable").DataTable(datatable.portfolio.options);
                datatable.portfolio.events.init();
            }
        },
        init: function () {
            datatable.portfolio.init();
        }
    }

    var modal = {
        portfolio: {
            object: $("#portfolio-modal"),
            form: {
                object: $("#portfolio-form").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        var formData = new FormData(formElement);

                        modal.portfolio.object.find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);

                        $.ajax({
                            url: "/alumno/portafolio/subir",
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        })
                            .done(function (e) {
                                datatable.portfolio.reload();
                                modal.portfolio.object.modal("hide");
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    confirmButtonClass: "btn btn-primary m-btn m-btn--custom",
                                    text: "Archivo adjunto con éxito.",
                                    confirmButtonText: "Aceptar"
                                });
                            })
                            .fail(function (e) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Aceptar",
                                    text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                });
                            })
                            .always(function () {
                                modal.portfolio.object.find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                            });
                    }
                })
            },
            events: {
                onHidden: function () {
                    modal.portfolio.object.on('hidden.bs.modal', function (e) {
                        modal.portfolio.form.object.resetForm();
                        $("#portfolio-file").val(null).trigger("change");
                        $("#portfolio-form").find(".custom-file-label").text("Buscar archivo...");
                    })
                },
                init: function () {
                    this.onHidden();
                }
            },
            init: function () {
                this.events.init();
            }
        },
        init: function () {
            modal.portfolio.init();
        }
    }

    return {
        init: function () {
            datatable.init();
            modal.init();
        }
    }
}();

$(() => {
    index.init();
})