var index = function () {

    var datatable = {
        projectDirector: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/master/getallmaster",
                    type: "GET",
                    data: function (data) {
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    { data: "nombre", title: "Nombre" },
                    { data: "duracion", title: "Duracion" },
                    { data: "creditos", title: "Creditos" },
                    { data: "descripcion", title: "Descripcion" },
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
                                        url:  `/admin/master/eliminar/${id}`,
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
            object: $("#AddMaster"),
            form: {
                object: $("#add-master").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        var formData = new FormData(formElement);
                        modal.projectDirector.object.find(":input").attr("disabled", true);

                        var message = $("#add-master").data("message");

                        $.ajax({
                            url: $("#add-master").attr("action"),
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
                        modal.projectDirector.object.find(".modal-title").text("Agregar Maestria");
                        $("#add-master").attr("action", "Master/Agregar");
                        $("#add-master").attr("data-message", "Registro agregado con éxito");
                        modal.projectDirector.object.modal("show");
                    })
                },
                init: function () {
                    this.show();
                }
            },
            edit: {
                show: function (data) {
                    modal.projectDirector.object.find(".modal-title").text("Editar una maestria");
                    $("#add-master").attr("action", "/admin/master/editar");
                    $("#add-master").attr("data-message", "Registro actualizado con éxito");
                    modal.projectDirector.object.find("[name='Id']").val(data.id);
                    modal.projectDirector.object.find("[name='Nombre']").val(data.nombre);
                    modal.projectDirector.object.find("[name='Duracion']").val(data.duracion);
                    modal.projectDirector.object.find("[name='Creditos']").val(data.creditos);
                    modal.projectDirector.object.find("[name='Descripcion']").val(data.descripcion);
                    modal.projectDirector.object.modal("show");
                }
            },
            events: {
                onHidden: function () {
                    modal.projectDirector.object.on('hidden.bs.modal', function (e) {
                        modal.projectDirector.form.object.resetForm();
                        modal.projectDirector.object.find("[name='Sex']").val(null).trigger("change");
                        modal.projectDirector.object.find("[name='DepartmentId']").val(null).trigger("change");
                        modal.projectDirector.object.find("[name='CivilStatus']").val(null).trigger("change");
                    })
                },
                init: function () {
                    this.onHidden();
                }
            },
            init: function () {
                modal.projectDirector.add.init();
                modal.projectDirector.events.init();
            }
        },
        init: function () {
            modal.projectDirector.init();
        }
    }

   /* var select = {
        init: function () {
            modal.projectDirector.object.find("[name='Sex']").select2({
                placeholder: "Seleccionar"
            });

            modal.projectDirector.object.find("[name='CivilStatus']").select2({
                placeholder: "Seleccionar"
            });

            $.ajax({
                url: "/departamentos/get"
            })
                .done(function (e) {
                    modal.projectDirector.object.find("[name='DepartmentId']").select2({
                        data: e,
                        placeholder: "Seleccionar departamento"
                    })
                })
        }
    }*/

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