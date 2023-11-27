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
                    url: "/profesor/justificacion-inasistencias-alumnos/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.search = $("#search").val();
                        data.status = $("#state-select").val();
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

                            if (data.status == 1) {
                                tmp += ` <button class="btn btn-success m-btn btn-sm m-btn--icon m-btn--icon-only btn-validate" data-object="${data.proto().encode()}" title="Validar"><i class="la la-check"></i></button>`;
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
                    $("#detail-teacher").val(object.teacher);
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

                $('#data-table').on('click', '.btn-validate', function (e) {
                    var object = $(this).data("object");
                    object = object.proto().decode();
                    form.validate.load(object);
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
                        console.log(error);
                        if (error.responseText !== null && error.responseText !== "") {
                            console.log(error.responseText);
                            $("#create-form-alert-txt").html(error.responseText);
                        }
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
        validate: {
            object: $("#validate-form").validate({
                submitHandler: function (e) {
                    mApp.block("#validate-modal .modal-content");

                    $.ajax({
                        url: $(e).attr("action"),
                        type: "POST",
                        data: $(e).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");

                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        datatable.justifications.reload();
                        form.validate.clear();
                    }).fail(function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#validate-modal .modal-content");
                    });
                }
            }),
            load: function (object) {
                $("#validate-student").val(object.name);
                $("#validate-course").val(object.course);
                $("#validate-class").val(`Clase dictada el ${object.classDate}`);

                $("#justificationId").val(object.id);
                $("#validate-observation").val("");
                $("#validate-modal").modal("toggle");
            },
            clear: function () {
                form.validate.object.resetForm();
            }
        }
    };


    var select = {
        init: function () {
            this.classes.init();
            this.students.init();
            this.course.init();
            this.states.init();
        },
        course: {
            init: function () {
                $.ajax({
                    url: "/profesor/seccionescurso/get".proto().parseURL()
                }).done(function (result) {

                    $(".select2-courses").select2({
                        data: result.items,
                        placeholder: "Seleccione un curso",
                        dropdwonParent: $("#create-modal"),
                        minimumResultsForSearch: -1
                    });

                    $(".select2-courses").on("change", function () {
                        select.students.load($(this).val());
                    });

                    $(".select2-courses").trigger("change");
                });               
            }
        },
        students: {
            init: function () {
                $(".select2-students").select2({
                    placeholder: "Seleccione un estudiante",
                    dropdwonParent: $("#create-modal"),
                    minimumResultsForSearch: 10
                });

                $(".select2-students").on("change", function () {
                    select.classes.load();
                });
            },
            load: function (id) {
                $.ajax({
                    url: `/profesor/justificacion-inasistencias-alumnos/alumnos/${id}/get`.proto().parseURL()
                }).done(function (result) {
                    $(".select2-students").empty();

                    $(".select2-students").select2({
                        data: result.items,
                        placeholder: "Seleccione un estudiante",
                        dropdwonParent: $("#create-modal"),
                        minimumResultsForSearch: 10
                    }).trigger("change");;
                });
            }
        },
        classes: {
            init: function () {
                $(".select2-classes").select2({
                    placeholder: "Inasistencias",
                    dropdwonParent: $("#create-modal"),
                    minimumResultsForSearch: 10
                });
            },
            load: function () {
                var sectionId = $(".select2-courses").val();
                var userId = $(".select2-students").val();

                $.ajax({
                    url: `/profesor/justificacion-inasistencias-alumnos/alumno/${userId}/inasistencias/${sectionId}/get`.proto().parseURL()
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
        },
        states: {
            init: function () {
                $("#state-select").select2();

                $("#state-select").on("change", function () {
                    datatable.justifications.reload();
                });
            }
        }
    }

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.justifications.reload();
            });
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
    //Absences.Table.init();

    InitApp.init();
})