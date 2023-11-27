var InitApp = function () {

    const status = {
        1: { "title": "Solicitado", "class": " m-badge--metal" },
        2: { "title": "Visto docente", "class": " m-badge--info" },
        3: { "title": "Observada", "class": " m-badge--warning" },
        4: { "title": "Aprobada", "class": " m-badge--success" },
        5: { "title": "Rechazada", "class": " m-badge--danger" }
    };

    var datatable = {
        justifications: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/justificacion-inasistencias/alumnos/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.search = $("#search").val();
                        data.status = $("#state-select").val();
                        data.termId = $("#select_term").val();
                    }
                },
                columns: [
                    {
                        title: "F. Solicitud",
                        data: "date"
                    },
                    {
                        title: "Código",
                        data: "code"
                    },
                    {
                        title: "Estudiante",
                        data: "name"
                    },
                    {
                        title: "Curso",
                        data: "course"
                    },
                    {
                        title: "Estado",
                        data: "status",
                        className: "text-center",
                        render: function (data) {                           
                            return '<span class="m-badge ' + status[data].class + ' m-badge--wide">' + status[data].title + "</span>";
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var tmp = "";
                            tmp = `<button class="btn btn-default m-btn btn-sm m-btn--icon m-btn--icon-only btn-detail" data-object="${data.proto().encode()}" title="Ver detalle"><i class="la la-eye"></i></button>`;

                            if (data.status == 2 || data.status == 3) {
                                tmp += ' <button class="btn btn-success m-btn btn-sm m-btn--icon m-btn--icon-only approve" data-id="' + data.id + '" title="Aprobar"><i class="la la-check"></i></button>';
                                tmp += ' <button class="btn btn-danger m-btn btn-sm m-btn--icon m-btn--icon-only disapprove" data-id="' + data.id + '" title="Rechazar"><i class="la la-remove"></i></button>';
                            }

                            return tmp;
                        }
                    }
                ]
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);

                $('#data-table').on('click', '.btn-detail', function (e) {
                    var object = $(this).data("object");
                    object = object.proto().decode();

                    $("#detail-student").val(object.name);
                    $("#detail-teacher").val(object.teacher);
                    $("#detail-course").val(object.course);
                    $("#detail-class").val(`Clase dictada el ${object.classDate}`);
                    $("#detail-date").val(object.date);
                    $("#detail-status").val(status[object.status].title);
                    $("#detail-description").val(object.description);

                    if (object.observation != null && object.observation != "") {
                        $("#teacher-observation").show();
                        $("#detail-observation").val(object.observation);
                    }
                    else {
                        $("#teacher-observation").hide();
                    }

                    if (object.file) {
                        $("#fileDiv").show();
                        $("#file-url").attr("href", `/admin/justificacion-inasistencias/alumnos/${object.id}/archivo/descargar`.proto().parseURL());
                    } else {
                        $("#fileDiv").hide();
                    }

                    $("#detail_modal").modal("toggle");
                });

                $('#data-table').on("click", ".approve", function () {
                    var id = $(this).data("id");
                    form.approve.load(id);
                });

                $('#data-table').on("click", ".disapprove", function () {
                    var id = $(this).data("id");
                    form.disapprove.load(id);
                });
            },
            reload: function () {
                this.object.ajax.reload();
            }
        }
    };

    var form = {
        approve: {
            load: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "La solicitud será aprobada",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, continuar",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: "/admin/justificacion-inasistencias/alumnos/post",
                                type: "POST",
                                data: {
                                    id: id,
                                    approved: true
                                },
                                success: function () {
                                    datatable.justifications.reload();
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                },
                                error: function () {
                                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                },
                                complete: function () {
                                    swal.close();
                                }
                            });
                        });
                    }
                });
            }
        },
        disapprove: {
            load: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "La solicitud será desaprobada",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, continuar",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: "/admin/justificacion-inasistencias/alumnos/post",
                                type: "POST",
                                data: {
                                    id: id,
                                    approved: false
                                },
                                success: function () {
                                    datatable.justifications.reload();
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                },
                                error: function () {
                                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                },
                                complete: function () {
                                    swal.close();
                                }
                            });
                        });
                    }
                });
            }
        }
    };

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.justifications.reload();
            });
        }
    };

    var select = {
        term: {
            init: function () {
                $.ajax({
                    url: "/periodos/get".proto().parseURL()
                }).done(function (data) {
                    $("#select_term").select2({
                        data: data.items
                    });


                    $("#select_term").on("change", function () {
                        if ($("#select_term").val() !== null) {
                            datatable.justifications.reload();
                        }
                    });
                }).fail(function () {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            }
        },
        states: {
            init: function () {
                $("#state-select").select2();

                $("#state-select").on("change", function () {
                    datatable.justifications.reload();
                });
            }
        },
        init: function () {
            select.term.init();
            select.states.init();
        }
    };


    return {
        init: function () {
            datatable.justifications.init();
            search.init();
            select.init();
        }
    };
}();


$(function () {
    InitApp.init();
})