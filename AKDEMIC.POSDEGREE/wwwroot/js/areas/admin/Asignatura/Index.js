var index = function () {

    var datatable = {
        projectDirector: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/Asignatura/getallasignatura",
                    type: "GET",
                    data: function (data) {
                        data.search = $("#search").val();
                      
                    }
                },
                columns: [
                    { data: "code", title: "Codigo" },
                    { data: "nameAsignatura", title: "Nombre Asignatura" },
                    { data: "credits", title: "Creditos" },
                    { data: "practicalHours", title: " Horas Practicas" },
                    { data: "teoricasHours", title: "Horas Teoricas" },
                    { data: "totalHours", title: "Total Horas" },
                    { data: "requisito", title: "Requisito" },
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
                                        url: `/admin/Asignatura/eliminar/${id}`,
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
            object: $("#AddAsignatura"),
            form: {
                object: $("#add-asignatura").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        var formData = new FormData(formElement);
                        modal.projectDirector.object.find(":input").attr("disabled", true);

                        var message = $("#add-asignatura").data("message");

                        $.ajax({
                            url: $("#add-asignatura").attr("action"),
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
                        modal.projectDirector.object.find(".modal-title").text("Agregar Asignatura");
                        $("#add-asignatura").attr("action", "Asignatura/Agregar");
                        $("#add-asignatura").attr("data-message", "Registro agregado con éxito");
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
                    $("#add-asignatura").attr("action", "/admin/Asignatura/editar");
                    $("#add-asignatura").attr("data-message", "Registro actualizado con éxito");
                    modal.projectDirector.object.find("[name='Id']").val(data.id);
                    modal.projectDirector.object.find("[name='codigo']").val(data.code);
                    modal.projectDirector.object.find("[name='nameAsignatura']").val(data.nameAsignatura);
                    modal.projectDirector.object.find("[name='credito']").val(data.credits);
                    modal.projectDirector.object.find("[name='hteoricas']").val(data.teoricasHours);
                    modal.projectDirector.object.find("[name='hpracticas']").val(data.practicalHours);
                    modal.projectDirector.object.find("[name='totalhoras']").val(data.totalHours);
                    modal.projectDirector.object.find("[name='requisito']").val(data.requisito);

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