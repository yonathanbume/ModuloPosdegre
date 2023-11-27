InitApp = function () {

    var private = {
        objects: {}
    };

    var modal = function () {
        $("#btnModalAdd").on('click', function () {
            $("#addModal").modal('show');
        });
    };

    var inputs = {
        init: function () {
            $("#search").doneTyping(function () {
                private.objects["tbl-data"].draw();
            });
        }
    };
    var options = {
        columnDefs: [
            { "orderable": false, "targets": [1] }
        ],
        ajax: {
            type: "GET",
            dataType: "JSON",
            url: `/welfare/rubricas-de-calificacion/obtener-criterios-de-rubrica`.proto().parseURL(),
            data: function (values) {
                values.searchValue = $("#search").val();
                values.studentRubricId = $("#StudentRubricId").val();
            }
        },
        columns: [
            { data: "name", title: "Descripción" },
            { data: "min", title: "Min" },        
            { data: "max", title: "Max" },                
            {
                data: null,
                title: "Opciones",
                render: function (data, type, row, meta) {
                    var tmp = `<button type="button" data-id="${data.id}" class="btn btn-sm btn-info edit"><i class="fa fa-edit"></i> </button>
                               <button type="button" data-id="${data.id}" class="btn btn-sm btn-danger delete"> <i class="fa fa-trash"></i> </button>`;
                    return tmp;
                }
            }
        ]
    };

    var events = {
        datatable_init: function () {
            private.objects["tbl-data"].on("click", ".criterias",
                function () {
                    var id = $(this).data('id');
                    window.location.href = `/welfare/rubricas-de-calificacion/criterios-de-evaluacion/${id}`.proto().parseURL();
                });
            private.objects["tbl-data"].on("click", ".edit",
                function () {
                    var id = $(this).data('id');
                    $.ajax({
                        type: 'GET',
                        url: `/welfare/rubricas-de-calificacion/obtener-criterio-evaluacion/${id}`.proto().parseURL(),
                        success: function (data) {
                            $("#edit-form input[id='Name_e']").val(data.name);                      
                            $("#edit-form input[id='Min_e']").val(data.min);                      
                            $("#edit-form input[id='Max_e']").val(data.max);              
                            $("#edit-form input[id='Id']").val(data.id);              
                            $("#editModal").modal('show');
                        }
                    })
                });
            private.objects["tbl-data"].on("click", ".delete",
                function () {
                    var btn = $(this);
                    btn.addLoader();
                    var id = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "El registro será eliminado permanentemente",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar"
                    }).then(function (result) {
                        if (result.value) {
                            $.ajax({
                                url: `/welfare/rubricas-de-calificacion/eliminar-criterio-evaluacion`.proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: id
                                },
                                success: function () {
                                    private.objects["tbl-data"].draw();
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                },
                                error: function () {
                                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                },
                                complete: function () {
                                    btn.removeLoader();
                                }
                            });
                        } else {
                            btn.removeLoader();
                        }
                    });
                });
        }

    };
    var dataTable = {
        init: function () {
            private.objects["tbl-data"] = $("#rubric-items-datatable").DataTable(options);
            events.datatable_init();
        }
    };

    var validate = {
        add: function () {
            $("#add-form").validate({
                submitHandler: function (form) {
                    var btn = $(form).find("button[ type='submit']");
                    btn.addLoader();
                    $.ajax({
                        type: "POST",
                        url: `/welfare/rubricas-de-calificacion/agregar-criterio-evaluacion`.proto().parseURL(),
                        data: $(form).serialize(),
                        success: function () {
                            $("#addModal").modal('hide');
                            $("#add-form").validate().resetForm();
                            private.objects["tbl-data"].draw();
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
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
        edit: function () {
            $("#edit-form").validate({
                submitHandler: function (form) {
                    var btn = $(form).find("button[ type='submit']");
                    btn.addLoader();
                    $.ajax({
                        type: "POST",
                        url: `/welfare/rubricas-de-calificacion/editar-criterio-evaluacion`.proto().parseURL(),
                        data: $(form).serialize(),
                        success: function () {
                            $("#editModal").modal('hide');
                            $("#edit-form").validate().resetForm();
                            private.objects["tbl-data"].draw();
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
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
        }
    };

    return {
        init: function () {
            inputs.init();
            dataTable.init();
            validate.add();
            validate.edit();
            modal();
        }
    }
}();

$(function () {
    InitApp.init();
});