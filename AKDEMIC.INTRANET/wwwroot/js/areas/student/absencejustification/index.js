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
                    url: "/alumno/justificacion-inasistencias/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        title: "F. Solicitud",
                        data: "date"
                    },                
                    {
                        title: "Curso",
                        data: "course"
                    },
                    {
                        title: "F. Clase",
                        data: "classDate",
                        orderable: false
                    },
                    {
                        title: "Estado",
                        data: "status",
                        orderable: false,
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
                            tmp = `<button class="btn btn-info m-btn btn-sm m-btn--icon m-btn--icon-only btn-detail" data-object="${data.proto().encode()}" title="Ver detalle"><i class="la la-eye"></i></button>`;

                            if (data.status == 1) {
                                tmp += ` <button class="btn btn-danger m-btn btn-sm m-btn--icon m-btn--icon-only btn-delete" data-id="${data.id}" title="Eliminar"><i class="la la-trash"></i></button>`;
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
                        $("#file-url").attr("href", `/alumno/justificacion-inasistencias/${object.id}/archivo/descargar`.proto().parseURL());
                    } else {
                        $("#fileDiv").hide();
                    }

                    $("#detail_modal").modal("toggle");
                });

                $('#data-table').on('click', '.btn-delete', function (e) {
                    var id = $(this).data("id");
                    form.delete.load(id);
                });
            },
            reload: function () {
                this.object.ajax.reload();
            }
        }
    };

    var form = {
        create: {
            object: $("#create-form").validate({
                submitHandler: function (e) {
                    mApp.block("#create-modal .modal-content");

                    var formData = new FormData($(e)[0]);

                    $.ajax({
                        url: $(e).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        datatable.justifications.reload();
                        form.create.clear();
                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "") $("#create-form-alert-txt").html(error.responseText);
                        else $("#create-form-alert-txt").html(_app.constants.ajax.message.error);

                        $("#create-form-alert").removeClass("m--hide").show();
                    }).always(function () {
                        mApp.unblock("#create-modal .modal-content");
                    });
                }
            }),
            clear: function () {
                form.create.object.resetForm();
            }
        },
        delete: {
            load: function (id) {

                swal({
                    title: "¿Está seguro?",
                    text: "La solicitud será eliminada permanentemente",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarla",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: "/alumno/justificacion-inasistencias/eliminar",
                                type: "POST",
                                data: {
                                    id: id
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
        init: function () {
            this.course.init();
        },
        course: {
            init: function () {
                $.ajax({
                    url: "/alumno/seccionescurso/get".proto().parseURL()
                }).done(function (result) {

                    $(".select2-courses").select2({
                        data: result.items,
                        placeholder: "Seleccione un curso",
                        dropdwonParent: $("#create-modal"),
                        minimumResultsForSearch: -1
                    }).trigger("change");

                    select.classes.init($(".select2-courses").val());
                });

                $(".select2-courses").on("change", function () {
                    select.classes.init($(this).val());
                });
            }
        },
        classes: {
            init: function (sectionId) {
                $.ajax({
                    url: `/alumno/secciones/${sectionId}/inasistencias/get`.proto().parseURL()
                }).done(function (result) {
                    $(".select2-classes").empty();

                    $(".select2-classes").select2({
                        data: result.items,
                        placeholder: "Inasistencias",
                        dropdwonParent: $("#create-modal"),
                        minimumResultsForSearch: 10
                    });
                });
            }
        }
    }

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