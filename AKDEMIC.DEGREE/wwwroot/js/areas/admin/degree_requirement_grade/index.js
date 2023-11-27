var DegreeRequirement = function () {
    var private = {
        objects: {}
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
            url: `/admin/gestion-de-requerimientos-de-grado/obtener-requerimientos`.proto().parseURL(),
            data: function (values) {
                values.searchValue = $("#search").val();
            }
        },
        columns: [

            {
                data: "name",
                title : "Descripción"
            },            
            {
                data: "type",
                title: "Tipo"                
            },
            {
                data: null,
                title: "Opciones",
                render: function (data, type, row, meta) {
                    return `<button data-id="${data.id}" class="btn btn-sm btn-info edit"> <i class="fa fa-edit"></i> </button>
                            <button type="button" data-id="${data.id}" class="btn btn-sm btn-danger delete"> <i class="fa fa-trash"></i> </button>`;

                }
            }
        ]
    };

    var validate = {
        add: function () {
            $("#add-form").validate({
                //rules: {
                //    Name: {
                //        NotAcceptSpace: true
                //    }
                //},
                submitHandler: function (form, e) {
                    e.preventDefault();
                    $.ajax({
                        type: "POST",
                        url: `/admin/gestion-de-requerimientos-de-grado/agregar`.proto().parseURL(),
                        data: $(form).serialize(),
                        success: function () {
                            $("#AddModal").modal('hide');
                            private.objects["tbl-data"].draw();
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        },
                        error: function () {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        },
                        complete: function () {
                            $("#add-form").validate().resetForm();
                        }
                    });
                    
                }
            });

        },
        edit: function () {
            $("#edit-form").validate({
                //rules: {
                //    Name: {
                //        NotAcceptSpace: true
                //    }
                //},
                submitHandler: function (form, e) {
                    e.preventDefault();
                    $.ajax({
                        type: "POST",
                        url: `/admin/gestion-de-requerimientos-de-grado/editar`.proto().parseURL(),
                        data: $(form).serialize(),
                        success: function () {
                            $("#EditModal").modal('hide');
                            private.objects["tbl-data"].draw();
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        },
                        error: function () {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        },
                        complete: function () {
                            $("#edit-form").validate().resetForm();
                        }
                    });
                }
            });
        }

    };


    var events = {
        datatable_init: function () {
            private.objects["tbl-data"].on("click", ".delete",
                function () {
                    var aid = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "El requerimiento será eliminado permanentemente",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, eliminarlo",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar"
                    }).then(function (result) {
                        if (result.value) {
                            $.ajax({
                                url: `/admin/gestion-de-requerimientos-de-grado/eliminar/${aid}`.proto().parseURL(),
                                type: "POST",
                                success: function () {
                                    private.objects["tbl-data"].draw();
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                },
                                error: function (error) {
                                    toastr.error("Existe data relacionada", _app.constants.toastr.title.error);
                                }
                            });
                        }
                    });

                });
            private.objects["tbl-data"].on("click", ".edit",
                function () {
                    var aid = $(this).data("id");
                    $.ajax({
                        url: `/admin/gestion-de-requerimientos-de-grado/obtener-requerimiento/${aid}`.proto().parseURL(),
                        type: "GET",
                        success: function (data) {
                            $("#edit-form input[name='Name']").val(data.name);
                            $("#edit-form select[name='Type']").val(data.type).trigger('change');
                            $("#edit-form input[name='Id']").val(data.id);
                            $("#EditModal").modal('show');
                        }
                    });

                });
        }

    };
    var dataTable = {
        init: function () {
            private.objects["tbl-data"] = $("#tbl-data").DataTable(options);
            events.datatable_init();
        }
    };

    var modal = function () {
        $("#AddModal").on('hidden.bs.modal', function () {
            $("#add-form").validate().resetForm();
        });

        $("#EditModal").on('hidden.bs.modal', function () {
            $("#edit-form").validate().resetForm();
        });
    };

    return {
        init: function () {
            dataTable.init();
            inputs.init();
            validate.add();
            validate.edit();
            modal();
        }
    };
}();

$(function () {
    DegreeRequirement.init();
});