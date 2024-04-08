var index = function () {

    var datatable = {
        projectDirector: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/typeenrollment/getallTypeEnrollment",
                    type: "GET",
                    data: function (data) {
                        data.search = $("#search").val();

                    }
                },
                columns: [
                    { data: "name", title: "tipo de matricula" },
                    { data: "description", title: "Descripcion" },
                    { data: "costo", title: "Costo" },
                    {
                        data: null,
                        title: "Opciones",
                        orderable: false,
                        render: function (data) {
                            var tpm = "";
                            tpm += `<button title="Editar" data-object="${data.proto().encode()}" type="button" class="btn-edit btn btn-primary m-btn m-btn--icon btn-sm m-btn--icon-only"><i class="la la-edit"></i></button> `;
                            tpm += `<button title="Eliminar" data-id="${data.id}" type="button" class="btn-delete btn btn-danger m-btn m-btn--icon btn-sm m-btn--icon-only"><i class="la la-trash"></i></button>`;
                            return tpm;
                        }
                    }
                ]
            },
            events: {
                onEdit: function () {
                    $("#datatable_data").on("click", ".btn-edit", function () {
                        var data = $(this).data("object").proto().decode();
                        modal.projectDirector.edit.show(data);
                    })
                },
                onDelete: function () {
                    $("#datatable_data").on("click", ".btn-delete", function () {
                        var id = $(this).data("id");

                        swal({
                            title: "¿Está seguro?",
                            text: "El registro será eliminado",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarlo",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise(() => {
                                    $.ajax({
                                        url: `/admin/typeenrollment/eliminar/${id}`,
                                        type: "POST",
                                        data: {
                                            id: id
                                        },
                                        success: function (e) {
                                            datatable.projectDirector.reload();
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: "Registro eliminado con éxito.",
                                                confirmButtonText: "Excelente"
                                            });
                                        },
                                        error: function (e) {
                                            swal({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                                confirmButtonText: "Entendido",
                                                text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                            });
                                        }
                                    });
                                });
                            }
                        });
                    })
                },
                init: function () {
                    this.onEdit();
                    this.onDelete();
                }
            },
            reload: function () {
                datatable.projectDirector.object.ajax.reload();
            },
            init: function () {
                datatable.projectDirector.object = $("#datatable_data").DataTable(datatable.projectDirector.options);
                datatable.projectDirector.events.init();
            }
        },
        init: function () {
            datatable.projectDirector.init();
        }
    }

    var events = {
        onSearch: function () {
            $("#search").doneTyping(function () {
                datatable.projectDirector.reload();
            })
        },
        init: function () {
            this.onSearch();
        }
    }

    var modal = {
        projectDirector: {
            object: $("#AddTypeEnrollment"),
            form: {
                object: $("#add-typeEnrollment").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        var formData = new FormData(formElement);
                        modal.projectDirector.object.find(":input").attr("disabled", true);

                        var message = $("#add-typeEnrollment").data("message");

                        $.ajax({
                            url: $("#add-typeEnrollment").attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false,
                        })
                            .done(function (e) {
                                modal.projectDirector.object.modal("hide");
                                datatable.projectDirector.reload();
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: message,
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
                                modal.projectDirector.object.find(":input").attr("disabled", false);
                            });
                    }
                })
            },
            add: {
                show: function () {
                    $("#add_projectDirector").on("click", function () {
                        modal.projectDirector.object.find(".modal-title").text("Agregar TypeMatricula");
                        $("#add-typeEnrollment").attr("action", "typeenrollment/Agregar");
                        $("#add-typeEnrollment").attr("data-message", "Registro agregado con éxito");
                        modal.projectDirector.object.modal("show");
                    })
                },
                init: function () {
                    this.show();
                }
            },
            edit: {
                show: function (data) {
                    modal.projectDirector.object.find(".modal-title").text("Editar una Asignatura");
                    $("#add-typeEnrollment").attr("action", "/admin/TypeEnrollment/editar");
                    $("#add-typeEnroolment").attr("data-message", "Registro actualizado con éxito");
                    modal.projectDirector.object.find("[name='Id']").val(data.id);
                    modal.projectDirector.object.find("[name='name']").val(data.name);
                    modal.projectDirector.object.find("[name='Description']").val(data.description);
                    modal.projectDirector.object.find("[name='costo']").val(data.costo);
                 
                    modal.projectDirector.object.modal("show");
                }
            },
            init: function () {
                modal.projectDirector.add.init();

            }
        },
        init: function () {
            modal.projectDirector.init();
        }
    }
    return {
        init: function () {
            datatable.init();
            modal.init();
            // select.init();
            events.init();
        }
    }
}();

$(() => {
    index.init();
});