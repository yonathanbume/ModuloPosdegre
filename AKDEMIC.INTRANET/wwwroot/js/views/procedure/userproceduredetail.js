var index = function () {

    var datatable = {
        userRequirementFiles: {
            object: null,
            options: {
                bPaginate: false,
                bLengthChange: false,
                bFilter: false,
                bInfo: false,
                ajax: {
                    url: "/tramites/get-archivos-usuario-tramite",
                    dataSrc: "",
                    data: function (row) {
                        row.userProcedureId = $("#Id").val();
                    }
                },
                columns: [
                    {
                        title: "Requisito",
                        data: "name",
                        orderable : false
                    },
                    {
                        title: "Estado",
                        data: "status",
                        orderable: false,
                        render: function (row) {
                            if (row == 3) {
                                return '<span class="m-badge m-badge--metal m-badge--wide">Observado</span>';
                            }
                            else {
                                return '<span class="m-badge m-badge--primary m-badge--wide">Enviado</span>';
                            }
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (row) {
                            var tpm = "";
                            if (row.status == 3) {
                                tpm += `<button data-id="${row.id}" class="btn btn-primary m-btn m-btn--icon btn-sm m-btn--icon-only btn-user-requirement-file-upload"><i class="la la-upload"></i></button> `;
                            }

                            tpm += `<a href="/pdf/${row.url}" target="_blank" class="btn btn-primary m-btn m-btn--icon btn-sm m-btn--icon-only"><i class="la la-download"></i></a>`;

                            return tpm;
                        }
                    }
                ]
            },
            events: {
                onUploadFile: function () {
                    $("#user_requirement_files_datatable").on("click", ".btn-user-requirement-file-upload", function () {
                        var id = $(this).data("id");
                        modal.userRequirementFileUpload.show(id);
                    })
                },
                init: function () {
                    this.onUploadFile();
                }
            },
            reload: function () {
                datatable.userRequirementFiles.object.ajax.reload();
            },
            init: function () {
                datatable.userRequirementFiles.object = $("#user_requirement_files_datatable").DataTable(datatable.userRequirementFiles.options);
                this.events.init();
            }
        },
        init: function () {
            datatable.userRequirementFiles.init();
        }
    }

    var modal = {
        userRequirementFileUpload: {
            object: $("#user_requirement_files_upload_modal"),
            form: {
                object: $("#user_requirement_files_upload_form").validate({
                    submitHandler: function (formElement, event) {
                        event.preventDefault();
                        var formData = new FormData(formElement);

                        $("#user_requirement_files_upload_modal").find(":input").attr("disabled", true);

                        $.ajax({
                            url: `/tramites/archivo-usuario-tramite/actualizar`,
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        })
                            .done(function (e) {
                                datatable.userRequirementFiles.reload();
                                modal.userRequirementFileUpload.object.modal("hide");
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "Archivo actualizado con éxito.",
                                    confirmButtonText: "Aceptar",
                                    allowOutsideClick: false
                                })
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
                                $("#user_requirement_files_upload_modal").find(":input").attr("disabled", false);
                            })
                    }
                })
            },
            show: function (id) {
                modal.userRequirementFileUpload.object.find("[name='Id']").val(id);
                modal.userRequirementFileUpload.object.modal("show");
            },
            events: {
                onHidden: function () {
                    modal.userRequirementFileUpload.object.on('hidden.bs.modal', function (e) {
                        modal.userRequirementFileUpload.object.find("[name='File']").val(null).trigger("change");
                        modal.userRequirementFileUpload.object.find(".custom-file-label").text("Seleccione un archivo");
                        modal.userRequirementFileUpload.form.object.resetForm();

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
            modal.userRequirementFileUpload.init();
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