var admins = function () {
    var datatable;

    var options = {
        data: {
            type: 'remote',
            source: {
                read: {
                    method: 'GET',
                    url: `/admin/permisos/usuarios/admin/get`.proto().parseURL(),
                },
            }
        },
        columns: [
            {
                field: 'name',
                title: 'Nombre',
            },
            {
                field: 'username',
                title: 'Usuario',
            },
            {
                field: 'options',
                title: 'Opciones',
                sortable: false,
                filterable: false,
                template: function (row) {
                    return '<button data-id="' + row.id + '" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete" title="Eliminar"><span><i class="la la-trash"></i><span>Remover</span></span></button> ' +
                           '<button data-id="' + row.id + '" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit" title="Editar"><span><i class="la la-edit"></i><span>Editar</span></span></button>';
                }
            }
        ]
    };

    var selects = {
        teacherSelect: {
            init: function () {
                $.ajax({
                    url: "/admin/permisos/usuarios/get",
                    type: "get"
                }).done((data) => {
                    //var newHtml = "";

                    //$.each(data, function (i, v) {
                    //    newHtml += `<option value="${v.id}">${v.name}</option>`;
                    //});

                    //$("#cTeacherId").html(newHtml);
                    $("#cTeacherId").select2({
                        dropdownParent: $("#create_modal"),
                        data: data
                    });

                });
            }
        },
        init: function () {
            selects.teacherSelect.init();
        }
    };


    var events = {
        init: function () {

            $('#create_modal').on('shown.bs.modal', function (e) {
                $("#cTeacherId").val(null).trigger("change");

                $('input[type="checkbox"]').prop('checked', false);
            });

            datatable.on('click', '.btn-delete', function () {
                var id = $(this).data('id');
                swal({
                    title: '¿Está seguro?',
                    text: "Los permisos de administrador serán removidos.",
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
                                url: "/admin/permisos/usuarios/admin/delete",
                                type: "POST",
                                data: {
                                    id: id
                                },
                                success: function (result) {
                                    datatable.reload();
                                    swal({
                                        type: 'success',
                                        title: 'Completado',
                                        text: 'La solicitud ha sido eliminado con éxito',
                                        confirmButtonText: 'Excelente'
                                    });
                                }
                            });
                        })
                    }
                });
            });

            datatable.on('click', '.btn-edit', function () {
                var id = $(this).data('id');                
                $("#edit_modal").modal('show');

                $("#edit_modal #cTeacherId").val(id);

                $.ajax({
                    url: `/admin/permisos/usuarios/admin/detail/get/${id}`.proto().parseURL(),
                    type: "get"
                }).done((data) => {
                    $.each(data, function (i, v) {
                        $("#edit_modal #chbx-" + v).prop('checked', true);
                    });
                });
            });
        }
    };
    var modal = {
        add: {
            object: $("#create_modal"),
            form: $("#create-form"),
            events: {
                onHidden: function () {
                    modal.add.object.on('hidden.bs.modal', function (e) {
                        $.each($("#create_modal input[type='checkbox']"), function (i, v) {
                            $(this).prop('checked', false);
                        });
                    });
                },
                validate: function () {
                    modal.add.form.validate({
                        rules: {
                            TeacherId: {
                                required: true,
                            },
                        },
                        submitHandler: function (formElement, e) {
                            modal.add.events.submit(formElement);
                        }
                    });
                },
                submit: function (formElement) {
                    var formData = new FormData(formElement);
                    if (!$("input[type='checkbox']:checked").length) {
                        swal({
                            type: "error",
                            title: "Error",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            confirmButtonText: "Aceptar",
                            text: "Seleccione al menos un sistema"
                        });
                        return;
                    }

                    modal.add.form.find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                    $.ajax({
                        url: "/admin/permisos/usuarios/admin/add",
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    })
                        .done(function (data) {
                            modal.add.object.modal("hide");
                            swal({
                                type: "success",
                                title: "Completado",
                                text: "Permisos otorgados satisfactoriamente.",
                                confirmButtonText: "Aceptar"
                            }).then(function (isConfirm) {
                                if (isConfirm.value) {
                                    datatable.reload();
                                }
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
                            modal.add.form.find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                        });
                },
                init: function () {
                    this.validate();
                    this.onHidden();
                }
            },
            init: function () {
                this.events.init();
            }
        },
        edit: {
            object: $("#edit_modal"),
            form: $("#edit-form"),
            events: {
                validate: function () {
                    modal.edit.form.validate({
                        rules: {
                            TeacherId: {
                                required: true,
                            },
                        },
                        submitHandler: function (formElement, e) {
                            e.preventDefault();
                            modal.edit.events.submit(formElement);
                        }
                    });
                },
                onHidden: function () {
                    modal.edit.object.on('hidden.bs.modal', function (e) {
                        $.each($("#edit_modal input[type='checkbox']"), function (i, v) {
                            $(this).prop('checked', false);
                        });
                    });
                },
                submit: function (formElement) {
                    var formData = new FormData(formElement);
                    var formData = new FormData(formElement);
                    if (!$("#edit_modal input[type='checkbox']:checked").length) {
                        swal({
                            type: "error",
                            title: "Error",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            confirmButtonText: "Aceptar",
                            text: "Seleccione al menos un sistema"
                        });
                        return;
                    }

                    modal.edit.form.find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                    $.ajax({
                        url: "/admin/permisos/usuarios/admin/add",
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    })
                        .done(function () {
                            modal.edit.object.modal("hide");
                            datatable.reload();
                            swal({
                                type: "success",
                                title: "Completado",
                                text: "Permisos editados satisfactoriamente.",
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
                            modal.edit.form.find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                        });
                },
                init: function () {
                    this.validate();
                    this.onHidden();
                }
            },
            init: function () {
                this.events.init();
            }
        },
        init: function () {
            this.add.init(); 
            this.edit.init();
        }
    };

    return {
        init: function () {
            datatable = $(".m-datatable").mDatatable(options);
            events.init();
            selects.teacherSelect.init();
            modal.init();
        }
    }
}();

$(function () {
    admins.init();
});