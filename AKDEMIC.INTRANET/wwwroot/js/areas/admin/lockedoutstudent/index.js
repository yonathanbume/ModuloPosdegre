var StudentTable = function () {


    var datatable = {
        object: null,
        options: {
            responsive: true,
            processing: true,
            serverSide: true,
            ajax: {
                url: `/admin/alumnos-bloqueados/get`.proto().parseURL(),
                data: function (data) {
                    delete data.columns;
                    data.search = $("#search").val();
                    data.facultyId = $(".select2-faculties").val();
                    data.careerId = $(".select2-careers").val();
                    data.programId = $(".select2-programs").val();
                }
            },
            columns: [
                { title: "Usuario", data: "user.userName" },
                { title: "Nombre y Apellidos", data: "user.fullName" },
                { title: "DNI", data: "user.dni" },
                { title: "Escuela", data: "career.name" },
                { title: "Teléfono", data: "user.phoneNumber" },
                { title: "Fec. Bloqueo", data: "user.address" },
                {
                    title: "Opciones",
                    data: null,
                    orderable: false,
                    render: function (data) {
                        return `<a data-id="${data.userId}" data-toggle="modal" data-target="#EditLockdOutModal" class="btn btn-primary btn-sm m-btn m-btn--icon btn-edit text-white" title="Editar Motivo"><i class="la la-edit"></i>Editar</a> <button data-id="${data.userId}" class="btn btn-danger btn-sm m-btn m-btn--icon btn-unlock" title="Desbloquear"><i class="la la-unlock-alt"></i>Desbloquear</button>`;
                    }
                }
            ]
        },
        init: function () {
            this.object = $(".students-datatable").DataTable(this.options);
        },
        reload: function () {
            this.object.ajax.reload();
        }
    };
    var events = {
        buttons: {
            unlock: function () {
                $(".students-datatable").on("click", ".btn-edit", function () {
                    var aid = $(this).data("id");
                    $("#EditReason").prop("disabled", true)
                    $.ajax({
                        url: ("/admin/alumnos-bloqueados/get-motivo?userId=" + aid).proto().parseURL(),
                    })
                        .done(function (data) {
                            $("#EditReason").val(data);
                            $("#EditId").val(aid);
                        })
                        .always(function () {
                            $("#EditReason").prop("disabled",false)
                        });
                });
                $(".students-datatable").on("click", ".btn-unlock",
                    function () {
                        var aid = $(this).data("id");
                        //////////////////////////
                        swal({
                            title: "¿Está seguro?",
                            //text: "El alumno será expulsado, removiendolo de todas sus secciones matriculadas. Este proceso es irreversible.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, desbloquear",
                            confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                            cancelButtonText: "Cancelar",
                            html: `El usuario será desbloqueado.</br>
                                       <div class="form-group m-form__group col-xl-12">
                                            <div class="m-input  m-input--solid">
                                                <label for="Reason2"></label>
                                                <input type="text" placeholder="Motivo" class="form-control m-input" name="Reason2" id="Reason2" required>
                                            </div>
                                        </div>`,
                            preConfirm: () => {
                                var errors = {
                                    Reason2: 'Introduzca un motivo',
                                    //File: 'Introduzca un archivo'
                                };
                                var validform = true;
                                $.each(errors, function (key, value) {
                                    if ($('input[name="Reason2"]').val() == "") {
                                        if (!$('input[name="' + key + '"]').hasClass("invalid")) {
                                            $('input[name="' + key + '"]').addClass('invalid').after('<div class="invalid-feedback">' + value + '</div>');
                                        }
                                        validform = false;
                                    } else {
                                        $('input.invalid').removeClass('invalid');
                                        $('.invalid-feedback').remove();
                                    }
                                });
                                if (!validform) return false;
                            },
                            allowOutsideClick: () => !swal.isLoading()
                        }).then((result) => {
                            if (result.value) {
                                var fd = new FormData();
                                fd.append("reason", $('input[name="Reason2"]').val())
                                $.ajax({
                                    url: (`/admin/alumnos-bloqueados/desbloquear/${aid}`).proto().parseURL(),
                                    type: "POST",
                                    processData: false,
                                    contentType: false,
                                    data: fd,
                                    success: function (result) {
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El registro ha sido desbloqueado con éxito",
                                            confirmButtonText: "Excelente"
                                        }).then(datatable.reload());
                                    },
                                    error: function (e) {
                                        var text = "";
                                        if (e.responseText !== null && e.responseText !== "") {
                                            text = e.responseText;
                                        }
                                        else {
                                            text = _app.constants.toastr.message.error.task;
                                        }
                                        swal({
                                            type: "error",
                                            title: _app.constants.toastr.title.error,
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: text
                                        });
                                    },
                                });
                            }
                        })
                    });
            }
        },
        init: function () {
            this.buttons.unlock();
        }
    };
    var validate = {
        add: function () {
            $("#add-form").validate({
                submitHandler: function (form) {
                    var btn = $(form).find('button[type="submit"]');
                    btn.addLoader();
                    var fd = new FormData($("#add-form")[0]);
                    $.ajax({
                        type: "POST",
                        url: `/admin/alumnos-bloqueados/guardar`.proto().parseURL(),
                        data: fd,
                        processData: false,
                        contentType: false,
                        success: function () {
                            $("#AddLockdOutModal").modal('hide');
                            datatable.reload();
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#add-form")[0].reset();
                        },
                        error: function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                        },
                        complete: function () {
                            btn.removeLoader();
                        }
                    });
                }
            });
            $("#edit-form").validate({
                submitHandler: function (form) {
                    var btn = $(form).find('button[type="submit"]');
                    btn.addLoader();
                    var fd = new FormData($("#edit-form")[0]);
                    $.ajax({
                        type: "POST",
                        url: `/admin/alumnos-bloqueados/guardar`.proto().parseURL(),
                        data: fd,
                        processData: false,
                        contentType: false,
                        success: function () {
                            $("#EditLockdOutModal").modal('hide');
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#edit-form")[0].reset();
                        },
                        error: function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                        },
                        complete: function () {
                            btn.removeLoader();
                        }
                    });
                }
            });
        },
    };
    var select2 = {
        init: function () {
            this.faculties.init();
            this.careers.init();
            this.programs.init();
            $("#add-user-select2").select2({
                placeholder: "Buscar...",
                dropdownParent: $("#AddLockdOutModal"),
                ajax: {
                    url: "/admin/alumnos-bloqueados/usuarios/get".proto().parseURL(),
                    dataType: "json",
                    data: function (params) {
                        return {
                            term: params.term
                        };
                    },
                    processResults: function (data, params) {
                        return {
                            results: data.items
                        };
                    },
                    cache: true
                },
                minimumInputLength: 3
            });
        },
        faculties: {
            init: function () {
                $.ajax({
                    url: "/facultades/get".proto().parseURL()
                }).done(function (result) {
                    $(".select2-faculties").select2({
                        data: result.items
                    });

                    $(".select2-faculties").on("change", function () {
                        select2.careers.reload($(this).val());
                        datatable.reload();
                    });
                });
            }
        },
        careers: {
            init: function () {
                $(".select2-careers").select2({
                    placeholder: "Seleccione una carrera"
                });

                $(".select2-careers").on("change", function () {
                    select2.programs.reload($(this).val());
                    datatable.reload();
                });
            },
            reload: function (facultyId) {
                $(".select2-careers").prop("disabled", true);
                $(".select2-careers").empty();

                $.ajax({
                    url: `/carreras/get?fid=${facultyId}`.proto().parseURL()
                }).done(function (result) {

                    result.items.unshift({ id: "Todas", text: "Todas" });

                    $(".select2-careers").select2({
                        data: result.items,
                        placeholder: "Seleccione una carrera"
                    });

                    if (result.items.length > 1) {
                        $(".select2-careers").prop("disabled", false);
                    }

                    //$(".select2-careers").trigger("change");
                });
            }
        },
        programs: {
            init: function () {
                $(".select2-programs").select2();

                $(".select2-programs").on("change", function () {
                    datatable.reload();
                });
            },
            reload: function (careerId) {
                $(".select2-programs").prop("disabled", true);
                $(".select2-programs").empty();

                $.ajax({
                    url: `/carreras/${careerId}/programas/get`.proto().parseURL()
                }).done(function (result) {


                    result.items.unshift({ id: "Todos", text: "Todos" });

                    $(".select2-programs").select2({
                        data: result.items,
                        placeholder: "Programas"
                    });

                    if (result.items.length > 1) {
                        $(".select2-programs").prop("disabled", false);
                    }
                    //$(".select2-programs").trigger("change");
                });
            }
        }
    };

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.reload();
            });
        }
    };

    return {
        init: function () {
            select2.init();
            search.init();
            datatable.init();
            validate.add();
            events.init();
        }
    };
}();

$(function () {
    StudentTable.init();
});