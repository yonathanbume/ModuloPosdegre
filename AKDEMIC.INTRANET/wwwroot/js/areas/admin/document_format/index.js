var index = function () {

    var datatable = {
        constancies: {
            object: null,
            options: {
                ajax: {
                    url: `/admin/constancias/get-datatable`,
                    type: "GET",
                    data: function (data) {

                    }
                },
                columns: [
                    {
                        data: "createdAt",
                        title :" Fec. Creación"
                    },
                    {
                        data: "name",
                        title: "Nombre"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        render: function (data) {
                            var tpm = "";
                            tpm += `<a href="/admin/constancias/editar/${data.id}" class="btn btn-primary m-btn m-btn--icon btn-sm m-btn--icon-only"><i class="la la-edit"></i></a> `;
                            tpm += `<button data-id="${data.id}" type="button" class="btn-delete btn btn-danger m-btn m-btn--icon btn-sm m-btn--icon-only"><i class="la la-trash"></i></button>`;

                            return tpm;
                        }
                    }
                ]
            },
            events: {
                onDelete: function () {

                    $("#document_format_datatable").on("click", ".btn-delete", function () {
                        var id = $(this).data('id');
                        swal({
                            title: '¿Está seguro?',
                            text: "La constancia será eliminado",
                            type: 'warning',
                            showCancelButton: true,
                            confirmButtonText: 'Sí, eliminarla',
                            confirmButtonClass: 'btn btn-danger m-btn m-btn--custom',
                            cancelButtonText: 'Cancelar',
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: `/admin/constancias/eliminar`.proto().parseURL(),
                                        type: "POST",
                                        data: {
                                            id: id
                                        },
                                    })
                                        .done(function (e) {
                                            datatable.constancies.reload();
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: "Constancia eliminada con éxito",
                                                confirmButtonText: "Excelente"
                                            });
                                        })
                                        .fail(function (e) {
                                            swal({
                                                type: "error",
                                                title: "Error",
                                                text: e.responseText,
                                                confirmButtonText: "Excelente"
                                            });
                                        })
                                })
                            }
                        });
                    })

                },
                init: function () {
                    this.onDelete();
                }
            },
            reload: function () {
                datatable.constancies.object.ajax.reload();
            },
            init: function () {
                datatable.constancies.object = $("#document_format_datatable").DataTable(datatable.constancies.options);
                datatable.constancies.events.init();
            }
        },
        init: function () {
            datatable.constancies.init();
        }
    }

    var modal = {
        constancies: {
            object: $("#document_format_modal"),
            form: {
                object: $("#document_format_form").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        var formData = new FormData(formElement);
                        modal.constancies.object.find(":input").attr("disabled", true);

                        var response = modal.constancies.object.data("rpta");

                        $.ajax({
                            url: $(modal.constancies.form.object.currentForm).attr("action"),
                            method: "POST",
                            data: formData,
                            contentType: false,
                            processData: false,
                        })
                            .done(function (e) {
                                modal.constancies.object.modal("hide");
                                datatable.constancies.reload();
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: response,
                                    confirmButtonText: "Excelente"
                                });

                            })
                            .fail(function (e) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                });
                            })
                            .always(function () {
                                modal.constancies.object.find(":input").attr("disabled", false);
                            });
                    }
                })
            },
            add: {
                show: function () {
                    modal.constancies.object.modal("show");
                    modal.constancies.object.find(".modal-title").text("Agregar Constancia");
                    modal.constancies.object.data("rpta", "Registro agregado satisfactoriamente");
                    $(modal.constancies.form.object.currentForm).attr("action", `/admin/constancias/crear`);
                }
            },
            events: {
                onHidden: function () {
                    modal.constancies.object.on('hidden.bs.modal', function (e) {
                        $(modal.constancies.form.object.currentForm)[0].reset();
                        modal.constancies.form.object.resetForm();
                        modal.constancies.object.find("[name='Id']").val(null).trigger("change");
                        modal.constancies.object.find("[name='Type']").val(null).trigger("change");
                    })
                },
                init: function () {
                    this.onHidden();
                }
            },
            init: function () {
                modal.constancies.events.init();
            }
        },
        init: function () {
            modal.constancies.init();
        }
    }

    var select = {
        recordType: {
            init: function () {
                modal.constancies.object.find("[name='Id']").select2({
                    dropdownParent: modal.constancies.object
                });
            }
        },
        init: function () {
            select.recordType.init();
        }
    }

    var events = {
        onAddScale: function () {
            $("#btn_add_document_format").on("click", function () {
                modal.constancies.add.show();
            })
        },
        init: function () {
            this.onAddScale();
        }
    }

    return {
        init: function () {
            datatable.init();
            modal.init();
            events.init();
            select.init();
        }
    }
}();

$(() => {
    index.init();
});