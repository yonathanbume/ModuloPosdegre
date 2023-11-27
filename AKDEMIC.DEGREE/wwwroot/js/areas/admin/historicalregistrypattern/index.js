var InitApp = function () {

    var datatable = {
        patterns: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/historial-padrones/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.search= $("#search").val();
                        //data.faculty = $("#faculty_select").val();
                        //data.career = $("#career_select").val();
                        //data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        title: "Nro. Oficio",
                        data: "number"
                    },
                    {
                        title: "Descripción",
                        data: "description"
                    },
                    {
                        title: "Descargar",
                        data: "file",
                        width: "80px",
                        render: function (data, type, row, meta) {
                            if (data != null && data != "") {
                             
                                return `<button data-id="${row.id}" class="btn btn-success m-btn btn-sm m-btn--icon m-btn--icon-only btn-download"><i class="la la-download"></i></button>`;
                            }
                            return "---";
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        width: "100px",
                        render: function (data) {
                            return `<button type="button" data-object="${data.proto().encode()}" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit" title=""><i class="la la-edit"></i> Editar</button> ` +
                                `<button class="btn btn-danger m-btn btn-sm m-btn--icon m-btn--icon-only btn-delete" data-id="${data.id}"><i class="la la-trash"></i></button>`;
                        }
                    }
                ]
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);

                $('#data-table').on('click', '.btn-edit', function (e) {
                    var object = $(this).data("object");

                    object = object.proto().decode();

                    form.edit.load(object);
                });

                $('#data-table').on('click', '.btn-delete', function (e) {
                    var id = $(this).data("id");
                    form.delete.load(id);
                });

                $('#data-table').on('click', '.btn-download', function (e) {
                    var id = $(this).data("id");
                    //var btn = $(this);
                    //btn.addLoader();
                    //$.fileDownload(`/admin/admin/historial-padrones/descargar/${id}`.proto().parseURL())
                    //    .done(function () {
                    //        btn.removeLoader();
                    //        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                    //    })
                    //    .fail(function (e) {
                    //        btn.removeLoader();
                    //        var response = decodeHTMLEntities(e);
                    //        toastr.error(response, "Error");
                    //    });
                    //var id = $btn.data('id');
                    window.open(`/admin/historial-padrones/descargar/${id}`.proto().parseURL(), '_blank');
                    
                });

            },
            reload: function () {
                this.object.ajax.reload();
            }
        }
    };

    var events = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.patterns.reload();
            });
        }
    };

    var form = {
        create: {
            object: $("#create-form").validate({
                submitHandler: function (e) {
                    mApp.block("#enrollment_modal .modal-content");
                    var formData = new FormData($(e).get(0));
                    $("#create-form .btn-save").addLoader();
                    $.ajax({
                        url: $(e).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    })
                        .done(function (e) {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");

                            swal({
                                type: "success",
                                title: "Tarea Completada",
                                html: e
                            });

                            datatable.patterns.reload();
                            form.create.clear();
                        })
                        .fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                        })
                        .always(function () {
                            mApp.unblock("#enrollment_modal .modal-content");
                            $("#create-form .btn-save").removeLoader();
                        });
                }
            }),
            clear: function () {
                form.edit.object.resetForm();
                $("input").val("").text("");
                $("#Special").prop("checked", false);
                $(".custom-file-label").text("Seleccionar Archivo...");
            }
        },
        edit: {
            object: $("#edit-form").validate({
                submitHandler: function (e) {
                    mApp.block("#enrollment_modal .modal-content");                                       
                    var formData = new FormData($(e).get(0));

                    $.ajax({
                        url: $(e).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    })
                        .done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");

                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                            datatable.patterns.reload();
                            form.edit.clear();
                        })
                        .fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                        })
                        .always(function () {
                            mApp.unblock("#enrollment_modal .modal-content");
                        });
                }
            }),
            load: function (object) {
                $("#edit-modal").modal("toggle");

                $("#eId").val(object.id);

                $("#eOfficeNumber").val(object.number);
                $("#eDescription").val(object.description);
            },
            clear: function () {
                form.edit.object.resetForm();
            }
        },
        delete: {
            load: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El padrón será eliminado permanentemente",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Si, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar"
                }).then(function (result) {
                    if (result.value) {
                        $.ajax({
                            url: "/admin/historial-padrones/eliminar",
                            type: "POST",
                            data: {
                                id: id
                            },
                            success: function () {
                                datatable.patterns.reload();
                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            },
                            error: function () {
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            }
                        });
                    }
                });
            }
        }
    };

    return {
        init: function () {
            datatable.patterns.init();
            events.init();
        }
    };
}();

$(function () {
    InitApp.init();
});