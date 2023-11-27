var EventsTypeTable = function () {
    var datatable;
    var options = {
        search: {
            input: $('#search')
        },
        data: {
            type: 'remote',
            source: {
                read: {
                    method: 'GET',
                    url: ('/admin/tipo-eventos/get').proto().parseURL()
                }
            },
            pageSize: 10,
            saveState: {
                cookie: true,
                webstorage: true
            }
        },
        columns: [
            {
                field: 'name',
                title: 'Nombre',
                width: 70
            },
            {
                field: 'color',
                title: 'Color',
                width: 70,
                template: function (row) {
                    return '<div class="circle" style="background-color:' + row.color + '";></div>';
                }
            },
            {
                field: 'options',
                title: 'Opciones',
                width: 250,
                sortable: false,
                filterable: false,
                template: function (row) {
                    return '<button data-id="' + row.id + '"class="btn btn-primary btn-sm m-btn m-btn--icon btn-edit" data-toggle="modal" data-target="#edit_modal" title="Editar"><span><span><i class="la la-edit"></i></span><span>Editar</span></span></button>&nbsp' +
                        '<button data-id="'+row.id+'" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete" title="Eliminar"><span><i class="la la-trash"></i><span>Eliminar</span></span></button>'
                }
            }

        ]
    };

    var initFormValidation = function () {
        formCreate = $("#create-form").validate();
        formEdit = $("#edit-form").validate();
        
    };
    

    var events = {
        init: function () {
            datatable.on('click', '.btn-edit', function () {
                var dataId = $(this).data("id"); 
                $.ajax({
                    url: ("/admin/tipo-eventos/get/"+dataId).proto().parseURL(),
                    type: "POST", 
                    success: function (result) {  

                        console.log(result);
                        var formElements = $("#edit-form").get(0).elements;
                        formElements["Name"].value = result.name;
                        formElements["Color"].value = result.color;
                        formElements["Id"].value = result.id;  
                    }, 
                    error: function () {
                        swal({
                            type: "error",
                            title: "Error",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            confirmButtonText: "Entendido",
                            text: "Al parecer el tipo de evento tiene información relacionada"
                        });
                    }  
                });
            });

            datatable.on('click', '.btn-delete', function () { 
                var dataId = $(this).data("id"); 
                swal({
                    title: "¿Está seguro que desea eliminar el tipo de evento?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminalo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    showLoaderOnConfirm: true,
                    cancelButtonText: "Cancelar",
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve, reject) => {
                                $.ajax({
                                url: ("/admin/tipo-eventos/eliminar/post").proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: dataId
                                },
                                success: function () {
                                    datatable.reload();

                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El tipo de evento ha sido registrado con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function () {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "Al parecer el tipo de evento tiene información relacionada"
                                    });
                                } 
                            });
                        });
                    }
                }); 
            });


        }
    }

    return {
        init: function () {
            datatable = $(".m-datatable").mDatatable(options);
            events.init();
            initFormValidation();
        },
        reloadTable: function () {
            datatable.reload();
        }
    }

}();


var DefaultAjaxFunctions = function () {
    var beginAjaxCall = function () {
        $(".btn-submit").each(function (index, element) {
            $(this).addLoader();
        });
    };
    var endAjaxCall = function () {
        $(".btn-submit").each(function (index, element) {
            $(this).removeLoader();
        });
    };
    var ajaxSuccess = function () {
        $("#create_modal").modal("hide");
        $("#edit_modal").modal("hide");

        formCreate.resetForm();
        formEdit.resetForm(); 
        EventsTypeTable.reloadTable();
        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
    };
    var createFailure = function (e) {
        toastr.success(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
    };

    return {
        beginAjaxCall: function () {
            beginAjaxCall();
        },
        endAjaxCall: function () {
            endAjaxCall();
        },
        ajaxSuccess: function () {
            ajaxSuccess();
        },
        createFailure: function (e) {
            createFailure(e);
        }
    };
}();
$(function () {
    EventsTypeTable.init();
});
 