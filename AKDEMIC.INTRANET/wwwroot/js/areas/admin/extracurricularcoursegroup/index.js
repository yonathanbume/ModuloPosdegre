var InitApp = function () {

    const datatableElement = $("#data-table");
    const studentsDatatableElement = $("#students-table");

    const modalCreateId = "#create-modal";
    const formCreateId = "#create-form";

    const modalEditId = "#edit-modal";
    const formEditId = "#edit-form";

    var datatable = {
        groups: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/grupos-extracurriculares/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        title: "Periodo",
                        data: "term"
                    },
                    {
                        title: "Curso",
                        data: "course"
                    },
                    {
                        title: "Código",
                        data: "code"
                    },
                    {
                        title: "Docente",
                        data: "teacher"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            return `<button type="button" data-object="${data.proto().encode()}" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit" title=""><i class="la la-edit"></i> Editar</button> ` +
                                `<button class="btn btn-danger m-btn btn-sm m-btn--icon m-btn--icon-only btn-delete" data-id="${data.id}"><i class="la la-trash"></i></button> ` +
                                `<button class="btn btn-primary btn-sm m-btn--icon m-btn--icon-only btn-students" data-id="${data.id}"><i class="la la-user"></i></button>`;
                        }
                    }
                ]
            },
            init: function () {
                this.object = datatableElement.DataTable(this.options);

                datatableElement.on('click', '.btn-edit', function (e) {
                    var object = $(this).data("object");
                    object = object.proto().decode();
                    form.edit.load(object);
                });

                datatableElement.on('click', '.btn-delete', function (e) {
                    var id = $(this).data("id");
                    form.delete.load(id);
                });

                datatableElement.on('click', '.btn-students', function (e) {
                    var id = $(this).data("id");
                    datatable.students.load(id);
                });
            },
            reload: function () {
                this.object.ajax.reload();
            }
        },
        students: {
            object: null,
            options: {
                serverSide: false,
                ajax: {
                    url: "/admin/grupos-extracurriculares/alumnos/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    dataSrc: "",
                    data: function (data) {
                        data.id = $("#groupId").val();
                    }
                },
                columns: [
                    {
                        title: "Código",
                        data: "username"
                    },
                    {
                        title: "Nombre",
                        data: "name"
                    },
                ]
            },
            load: function (id) {
                $("#groupId").val(id);

                if (this.object == null) {
                    this.object = studentsDatatableElement.DataTable(this.options);
                }
                else {
                    this.object.clear().draw();
                    this.object.ajax.reload();
                }
                $("#students-modal").modal('show');
            }
        }
    };

    var form = {
        create: {
            object: $(formCreateId).validate({
                submitHandler: function (e) {
                    mApp.block(`${modalCreateId} .modal-content`);

                    $.ajax({
                        url: $(e).attr("action"),
                        type: "POST",
                        data: $(e).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");

                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        datatable.groups.reload();
                        form.create.clear();
                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "")
                            $("#create-form-alert-txt").html(error.responseText);
                        else
                            $("#create-form-alert-txt").html(_app.constants.ajax.message.error);

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
        edit: {
            object: $(formEditId).validate({
                submitHandler: function (e) {
                    mApp.block(`${modalEditId} .modal-content`);

                    $.ajax({
                        url: $(e).attr("action"),
                        type: "POST",
                        data: $(e).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");

                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        datatable.groups.reload();
                        form.edit.clear();
                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "") $("#edit-form-alert-txt").html(error.responseText);
                        else $("#edit-form-alert-txt").html(_app.constants.ajax.message.error);

                        $("#edit-form-alert").removeClass("m--hide").show();
                    }).always(function () {
                        mApp.unblock("#edit-modal .modal-content");
                    });
                }
            }),
            load: function (object) {

                $("#Edit_Id").val(object.id);
                $("#Edit_Code").val(object.code);
                $("#Edit_TermId").val(object.termId).trigger("change");
                $("#Edit_ExtracurricularCourseId").val(object.courseId).trigger("change");

                $("#Edit_TeacherId").empty().append(`<option value="${object.teacherId}">${object.teacher}</option>`).val(`${object.teacherId}`).trigger('change');

                $(modalEditId).modal("toggle");
            },
            clear: function () {
                form.edit.object.resetForm();
            }
        },
        delete: {
            load: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El grupo será elimininado permanentemente. En caso de tener información relacionada no podrá eliminarse.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Si, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar"
                }).then(function (result) {
                    if (result.value) {
                        $.ajax({
                            url: "/admin/actividades-extracurriculares/eliminar",
                            type: "POST",
                            data: {
                                id: id
                            },
                            success: function () {
                                datatable.groups.reload();
                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            },
                            error: function () {
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            }
                        });
                    }
                });
            }
        },
    };

    var select = {
        terms: {
            init: function () {
                $.ajax({
                    url: "/periodos/pendientes/get".proto().parseURL()
                })
                    .done(function (data) {
                        $("#Add_TermId").select2({
                            placeholder: "Seleccione un periodo",
                            dropdownParent: $(modalCreateId),
                            data: data.items
                        });

                        $("#Edit_TermId").select2({
                            placeholder: "Seleccione un periodo",
                            dropdownParent: $(modalEditId),
                            data: data.items
                        });

                        if (data.selected !== null) {
                            $("#Add_TermId").val(data.selected);
                            $("#Add_TermId").trigger("change.select2");

                            $("#Edit_TermId").val(data.selected);
                            $("#Edit_TermId").trigger("change.select2");
                        }
                    });
            }
        },
        courses: {
            init: function () {
                $.ajax({
                    url: "/cursosextracurriculares/get".proto().parseURL()
                })
                    .done(function (data) {
                        $("#Add_ExtracurricularCourseId").select2({
                            placeholder: "Seleccione un curso",
                            dropdownParent: $(modalCreateId),
                            data: data.items
                        });

                        $("#Edit_ExtracurricularCourseId").select2({
                            placeholder: "Seleccione un curso",
                            dropdownParent: $(modalEditId),
                            data: data.items
                        });
                    });
            }
        },
        teachers: {
            init: function () {

                $("#Add_TeacherId").select2({
                    ajax: {
                        url: "/profesores/get",
                        type: 'GET',
                        datatype: 'json',
                        delay: 250,
                        data: function (params) {
                            return {
                                q: params.term,
                                page: params.page
                            };
                        },
                        processResults: function (data, params) {
                            params.page = params.page || 1;

                            return {
                                results: data.items.results,
                                pagination: {
                                    more: (params.page * 30) < data.total_count
                                }
                            };
                        },
                        cache: true
                    },
                    placeholder: 'Buscar docentes',
                    minimumInputLength: 3,
                });


                $("#Edit_TeacherId").select2({
                    ajax: {
                        url: "/profesores/get",
                        type: 'GET',
                        datatype: 'json',
                        delay: 250,
                        data: function (params) {
                            return {
                                q: params.term,
                                page: params.page
                            };
                        },
                        processResults: function (data, params) {
                            params.page = params.page || 1;

                            return {
                                results: data.items.results,
                                pagination: {
                                    more: (params.page * 30) < data.total_count
                                }
                            };
                        },
                        cache: true
                    },
                    placeholder: 'Buscar docentes',
                    minimumInputLength: 3,
                });
            }
        },
        init: function () {
            this.terms.init();
            this.courses.init();
            this.teachers.init();
        }
    };

    var events = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.groups.reload();
            });
        }
    };

    return {
        init: function () {
            datatable.groups.init();
            select.init();
            events.init();
        }
    };
}();

$(function () {
    InitApp.init();
});