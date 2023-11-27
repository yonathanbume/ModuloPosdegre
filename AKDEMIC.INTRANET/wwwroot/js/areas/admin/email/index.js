var index = function () {

    var datatable = {
        systemEmail: {
            object : null,
            options: {
                ajax: {
                    url: "/admin/gestion-correos/get-datatable",
                    type: "GET",
                    data: function (data) {
                        data.system = $("#select_system").val();
                    }
                },
                columns: [
                    {
                        data: "email",
                        title :"Correo"
                    },
                    {
                        data: "system",
                        title :"Sistema"
                    }
                ]
            },
            reload: function () {
                datatable.systemEmail.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
            }
        },
        init: function () {
            this.systemEmail.init();
        }
    }

    var form = {
        configuration: {
            object: $("#configuration_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.configuration.submit(formElement);
                }
            }),
            submit: function (formElement) {
                var formData = new FormData(formElement);
                $("#configuration_form").find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                $.ajax({
                    url: "/admin/gestion-correos/actualizar-general",
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false
                })
                    .done(function (e) {
                        swal({
                            type: "success",
                            title: "Completado",
                            text: "Datos actualizados satisfactoriametne.",
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
                        $("#configuration_form").find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                    })
            }
        },
        init: function () {

        }
    }

    var select = {
        system: {
            load: function () {
                $(".m-select2").select2({
                    allowClear: true,
                    placeholder : "Seleccionar sistema"
                });
            },
            onChange: function () {
                $("#select_system").on("change", function () {
                    datatable.systemEmail.reload();
                })
            },
            init: function () {
                this.load();
                this.onChange();
            }
        },
        init: function () {
            this.system.init();
        }
    }

    var events = {
        onPasswordEvent: function () {
            $("#CurrentPasswordEye").on("click", function () {
                var currentType = $("#Email_Password").attr("type");
                if (currentType === "password") {
                    $("#Email_Password").attr("type","text");
                } else {
                    $("#Email_Password").attr("type","password");
                }
            })
        },
        init: function () {
            this.onPasswordEvent();
            $("#Email_Password").val($("#current_password").val());
        }
    }

    var modal = {
        resource: {
            object: $("#document-modal"),
            form: {
                object: $("#document-form").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        modal.resource.object.find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                        var formData = new FormData(formElement);
                        $.ajax({
                            type: "POST",
                            url: $(formElement).attr("action"),
                            data: formData,
                            contentType: false,
                            processData: false
                        })
                            .done(function (e) {
                                datatable.reload();
                                modal.resource.object.modal("hide");
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "Proceso Completado.",
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
                                modal.resource.object.find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                            })
                    }
                }),
                reset: function () {
                    modal.resource.form.object.resetForm();
                }
            },
            add: {
                show: function () {
                    modal.resource.object.find(".modal-title").text("Agregar Recurso");
                    $("#document-form").attr("action", "/admin/recursos-digitales/agregar");
                    modal.resource.object.modal("show");
                }
            },
            edit: {
                show: function (id) {
                    modal.resource.object.find(".modal-title").text("Editar Recurso");
                    $("#document-form").attr("action", "/admin/recursos-digitales/editar");
                    modal.resource.object.modal("show");
                    this.getData(id);
                },
                getData: function (id) {
                    modal.resource.object.find(":input").attr("disabled", true);
                    $.ajax({
                        url: `/admin/recursos-digitales/get/${id}`,
                        type: "GET"
                    })
                        .done(function (e) {
                            modal.resource.object.find("[name='Id']").val(e.id);
                            modal.resource.object.find("[name='Sorter']").val(e.sorter);
                            modal.resource.object.find("[name='Title']").val(e.title);
                            select2.career.setValue(e.careerId, e.careerName);

                            $("#div_last_file").removeClass("d-none");
                            $("#last_file").attr("href", `/documentos/${e.fileUrl}`);
                            modal.resource.object.find(":input").attr("disabled", false);
                        })
                }
            },
            events: {
                onHidden: function () {
                    modal.resource.object.on('hidden.bs.modal', function (e) {
                        modal.resource.form.reset();
                        $("#div_last_file").addClass("d-none");
                        $("#last_file").attr("href", "");
                        modal.resource.object.find(".custom-file-label").text("Seleccionar archivo")
                        select2.career.clear();
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
            this.resource.init();
        }
    }


    return {
        init: function () {
            events.init();
            form.init();
            select.init();
            datatable.init();
        }
    }
}();

$(() => {
    index.init();
})

