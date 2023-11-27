var SimulationSubjectTable = function () {
    var id = "#psychologytestquestion-datatable";
    var datatable;
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ("/welfare/preguntas-categoria/list").proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "category",
                title: "Categoría",
                width: 200
            },
            {
                field: "question",
                title: "Pregunta",
                width: 300
            },
            {
                field: "state",
                title: "Estado",
                width : 200,
                template: function (row) {
                    if (row.state === true) {
                        return `<span class="m-badge  m-badge--success m-badge--wide">Activo</span>`;
                    } else {
                        return `<span class="m-badge  m-badge--danger m-badge--wide">Desactivado</span>`;
                    }
                    
                }
            },
            {
                field: "options",
                title: "Opciones",
                width: 300,
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
                        text: "La pregunta será eliminada",
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
                                    url: ("/welfare/preguntas-categoria/eliminar/post").proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        id: dataId
                                    },
                                    success: function () {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "La pregunta ha sido eliminada con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                    },
                                    error: function () {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Al parecer la pregunta tiene información relacionada"
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
                        url: ("/welfare/preguntas-categoria/crear/post").proto().parseURL(),
                        data: $("#add-form").serialize(),
                        success: function () {
                            datatable.reload();
                            $("#add_area_modal").modal("hide");
                            toastr.success(_app.constants.toastr.message.success.create, _app.constants.toastr.title.success);
                        },
                        error: function (e) {
                            toastr.error(e.responseJSON.Question, _app.constants.toastr.title.error);
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
                        url: ("/welfare/preguntas-categoria/editar/post").proto().parseURL(),
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
        selectform: function () {
            $.ajax({
                url: '/welfare/preguntas-categoria/list/category'.proto().parseURL()
                })
                .done(function (data) {
                    $(".select2-categorias").select2({
                        data: data.items,
                        placeholder: "Categorias"
                    });
                });
        },
        updateState: function() {

            datatable.on("click", "span.bootstrap-switch-off", function () {
                var t = $(this).find(".custom_checkbox");
                console.log(t.data("id"));
                var id = $(this).data("id");
                console.log(id);
                $.ajax({
                    type: "GET",
                    url: `/welfare/${id}/estado`.proto().parseURL(),
                    success: function(data) {
                        console.log("GO");
                    }
                });
            });
        },
        switcher: function() {
            datatable.on('m-datatable--on-init m-datatable--on-layout-updated', function() {
                $(".custom_checkbox").bootstrapSwitch({
                    onText: "Sí",
                    offText: "No",
                    onColor: "success",
                    offColor: "danger",
                });
                
                events.updateState();
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
                    url: `/welfare/preguntas-categoria/${id}/get`.proto().parseURL(),
                    success: function (data) {
                        $("#edit-form select[name='CategoryId']").val(data.categoryId).trigger('change');
                        $("#edit-form textarea[name='Question']").val(data.questions);
                        if (data.state === true)
                        {
                            $("#edit-form input[name='State']").prop("checked", true);
                        }
                        else {
                            $("#edit-form input[name='State']").prop("checked", false);
                        }
                        
                    }
                });

            });
        },
        initAdd: function () {
            $(".btn-addpsychologytestquestion").on('click', function () {
                $("#add_area_modal").modal("show");
                $("#add-form input[name='Name']").val("");
            });
        }
    };
    return {
        init: function () {
            datatable = $(id).mDatatable(options);
            events.init();
            events.switcher();
            events.addform();
            events.editform();
            //events.subjectarea();
            events.selectform();
            events.updateState();
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