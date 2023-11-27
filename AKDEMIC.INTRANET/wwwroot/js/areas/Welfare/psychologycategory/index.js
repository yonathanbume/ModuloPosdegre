var SimulationSubjectTable = function () {
    var id = "#simulationsubject-datatable";
    var datatable;
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ("/welfare/psicologia_categorias/listar").proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "name",
                title: "Nombre",
                width: 200
            },
            {
                field: "options",
                title: "Opciones",
                sortable: false,
                filterable: false,
                template: function (row) {
                    var template = "";
                    template +=
                        "<button class='btn btn-primary m-btn btn-sm m-btn--icon btn-edit' data-id='" + row.id + "'><i class='la la-edit'></i> Editar</button>";
                    template +=
                        " <button class='btn btn-danger m-btn btn-sm m-btn--icon btn-delete' data-id='" + row.id + "'><i class='la la-trash'></i> Eliminar</button>";
                
                    return template;
                }
            }
        ]
    };
    var events = {
        init: function () {
            datatable.on("click",
                ".btn-delete",
                function () {
                    var dataId = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "La asignatura será eliminada",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, eliminarla",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        allowOutsideClick: () => !swal.isLoading(),
                        preConfirm: () => {
                            return new Promise(() => {
                                $.ajax({
                                    url: ("/welfare/psicologia_categorias/eliminar/post").proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        id: dataId
                                    },
                                    success: function () {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "La categoría ha sido eliminada con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                    },
                                    error: function () {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Al parecer la categoría tiene información relacionada"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });
        },
        addform: function (e) {
            $("#add-form").validate({
                submitHandler: function () {
                    $.ajax({
                        type: 'POST',
                        url: ("/welfare/psicologia_categorias/crear/post").proto().parseURL(),
                        data: $("#add-form").serialize(),
                        success: function () {
                            datatable.reload();
                            $("#add_area_modal").modal("hide");
                            toastr.success(_app.constants.toastr.message.success.create, _app.constants.toastr.title.success);
                        },
                        error: function (e) {
                            toastr.error(e.responseJSON.Name, _app.constants.toastr.title.error);
                        }

                    });
                }
            });

        },
        editform: function (e) {
            $("#edit-form").validate({
                submitHandler: function () {
                    $.ajax({
                        type: 'POST',
                        url: ("/welfare/psicologia_categorias/editar/post").proto().parseURL(),
                        data: $("#edit-form").serialize(),
                        success: function () {
                            datatable.reload();
                            $("#edit_area_modal").modal("hide");
                            toastr.success(_app.constants.toastr.message.success.update, _app.constants.toastr.message.success.title);
                        }

                    });
                }
            });

        },
        subjectarea: function () {
            datatable.on("click", ".btn-details", function () {
                var dataId = $(this).data("id");
                window.location.href = `/welfare/psicologia_categorias/${dataId}`.proto().parseURL();
            });
        }

    };
    var modal = {
        initEdit: function () {
            datatable.on("click", ".btn-edit", function () {
                var id = $(this).data("id");
                $("#edit_area_modal").modal("show");
                $("#edit-form input[name='Id']").val(id);
                $.ajax({
                    type: 'GET',
                    url: `/welfare/psicologia_categorias/${id}/get`.proto().parseURL(),
                    success: function (data) {
                        $("#edit-form input[name='Name']").val(data.name);
                    }
                });

            });
        },
        initAdd: function () {
            $(".btn-addsubject").on('click', function () {
                $("#add_area_modal").modal("show");
                $("#add-form input[name='Name']").val("");
            });
        }
    };

    return {
        init: function () {
            datatable = $(id).mDatatable(options);
            events.init();
            events.addform();
            events.editform();
            events.subjectarea();
            modal.initAdd();
            modal.initEdit();
        },
        reload: function () {
            datatable.reload();
        }
    }
}();

$(function () {
    SimulationSubjectTable.init();
});