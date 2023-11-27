var psychologicalDiagnostics = (function () {
    var result = {
        ajax: {
            list: {},
            load: {
                create: function (element, event) {
                    var formElements = element.elements;
                    psychologicalDiagnostics.ajax.list["psychologicalDiagnostics-ajax-create"] = $.ajax({
                        data: {
                            Description: formElements["Description"].value,
                            Code: formElements["Code"].value
                        },
                        type: element.method,
                        url: element.action,
                        beforeSend: function (jqXHR, settings) {
                            $(element).addLoader();
                        },
                        complete: function (jqXHR, textStatus) {
                            $(element).removeLoader();
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                        },
                        success: function (data, textStatus, jqXHR) {
                            psychologicalDiagnostics.datatable.list["ajax_data"].reload();
                            psychologicalDiagnostics.modal.list["psychologicalDiagnostics-modal-create"].modal("hide");
                            toastr.success(_app.constants.toastr.message.success.create, _app.constants.toastr.title.success);
                        }
                    });
                },
                editById: function (element, event) {

                    var id = $(element).data("id");
                    psychologicalDiagnostics.ajax.list["psychologicalDiagnostics-ajax-edit"] = $.ajax({
                        data: {
                            Id: id
                        },
                        type: 'GET',
                        url: ('/welfare/diagnosticos-psicologicos/editar/' + id + '').proto().parseURL(),
                        error: function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                        },
                        success: function (data, textStatus, jqXHR) {
                            //psychologicalDiagnostics.datatable.list["ajax_data"].reload();
                            //psychologicalDiagnostics.modal.list["psychologicalDiagnostics-modal-"].modal("hide");
                            //toastr.success(_app.constants.toastr.message.success.edit, _app.constants.toastr.title.success);
                            var formElements = $('#psychologicalDiagnostics-modal-edit-form');               
                            
                            $("input[name='Code']").val(data.code);
                            $("textarea[name='Description']").val(data.description);
                            $("input[name='Id']").val(data.id);

                        }
                    });
                },
                edit: function (element, event) {
                    var formElements = element.elements;
                    psychologicalDiagnostics.ajax.list["psychologicalDiagnostics-ajax-edit"] = $.ajax({
                        data: {
                            Id: formElements["Id"].value,
                            Code: formElements["Code"].value,                            
                            Description: formElements["Description"].value
                        },

                        type: element.method,
                        url: element.action,
                        beforeSend: function (jqXHR, settings) {
                            $(element).addLoader();
                        },
                        complete: function (jqXHR, textStatus) {
                            $(element).removeLoader();
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.update, _app.constants.toastr.title.error);
                        },
                        success: function (data, textStatus, jqXHR) {

                            psychologicalDiagnostics.datatable.list["ajax_data"].reload();
                            psychologicalDiagnostics.modal.list["psychologicalDiagnostics-modal-edit"].modal("hide");
                            toastr.success(_app.constants.toastr.message.success.update, _app.constants.toastr.title.success);
                        }
                    });
                }
            }
        },
        datatable: {
            list: {},
            load: {
                get: function () {
                    psychologicalDiagnostics.datatable.list["ajax_data"] = $("#ajax_data").mDatatable({
                        data: {
                            source: {
                                read: {
                                    method: "GET",
                                    url: `/welfare/diagnosticos-psicologicos/get`.proto().parseURL(),
                                },
                            },
                        },
                        columns: [
                            {
                                field: "description",
                                title: "Descripción"
                            },
                            {
                                field: "code",
                                title: "Código"
                            },                            
                            {
                                field: "options",
                                title: "Opciones",
                                sortable: false,
                                filterable: false,
                                template: function (row) {
                                    var template = "";
                                    template += '<button type="button" data-id="' + row.id + '" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit" onClick="psychologicalDiagnostics.modal.load.edit(this, event)"><span><i class="la la-edit"></i><span>Editar</span></span></button>'
                                    template += " <button class=\"btn btn-danger m-btn btn-sm m-btn--icon\" onclick =\"psychologicalDiagnostics.swal.load.delete(this, event, '";
                                    template += ("/welfare/diagnosticos-psicologicos/eliminar/post").proto().parseURL();
                                    template += "', '";
                                    template += row.proto().encode();
                                    template += "')\"><i class=\"la la-trash\"></i></button>";
                                    return template;
                                }
                            }
                        ]
                    });
                }
            }
        },
        modal: {
            list: {},
            load: {
                create: function (element, event) {
                    var psychologicalDiagnosticsModalCreateForm = document.getElementById("psychologicalDiagnostics-modal-create-form");                    
                    _app.modules.form.reset({
                        element: psychologicalDiagnosticsModalCreateForm
                    });

                    psychologicalDiagnostics.modal.list["psychologicalDiagnostics-modal-create"] = $("#psychologicalDiagnostics-modal-create").modal("show");
                },
                edit: function (element, event) {
                    var psychologicalDiagnosticsModalEditForm = document.getElementById("psychologicalDiagnostics-modal-edit-form");

                    _app.modules.form.reset({
                        element: psychologicalDiagnosticsModalEditForm
                    });

                    psychologicalDiagnostics.ajax.load.editById(element, event);
                    psychologicalDiagnostics.modal.list["psychologicalDiagnostics-modal-edit"] = $("#psychologicalDiagnostics-modal-edit").modal("show");
                }
            }
        },
        swal: {
            list: {},
            load: {
                delete: function (element, event, url, data) {
                    data = data.proto().decode();

                    psychologicalDiagnostics.swal.list["psychologicalDiagnostics-swal-delete"] = swal({
                        title: _app.constants.swal.title.delete,
                        type: "warning",
                        showCancelButton: true,
                        showLoaderOnConfirm: true,
                        preConfirm: () => {
                            return new Promise((resolve, reject) => {
                                $.ajax({
                                    data: {
                                        id: data.id
                                    },
                                    type: "POST",
                                    url: url,
                                    beforeSend: function (jqXHR, settings) {

                                    },
                                    complete: function (jqXHR, textStatus) {
                                        resolve();
                                    },
                                    success: function (data, textStatus, jqXHR) {
                                        psychologicalDiagnostics.datatable.list["ajax_data"].reload();
                                        toastr.success(_app.constants.toastr.message.success.delete, _app.constants.toastr.title.success);
                                    },
                                    error: function (jqXHR, textStatus, errorThrown) {
                                        toastr.error("Error al tratar de eliminar el registro", _app.constants.toastr.title.error);
                                    }
                                });
                            });
                        }
                    });
                }
            }
        },
        validate: {
            list: {},
            load: {
                create: function () {
                    psychologicalDiagnostics.validate.list["psychologicalDiagnostics-modal-create-form"] = $("#psychologicalDiagnostics-modal-create-form").validate({
                        submitHandler: function (form, event) {
                            event.preventDefault();
                            psychologicalDiagnostics.ajax.load.create(form, event);
                        }
                    });
                },
                edit: function () {

                    psychologicalDiagnostics.validate.list["psychologicalDiagnostics-modal-edit-form"] = $("#psychologicalDiagnostics-modal-edit-form").validate({
                        submitHandler: function (form, event) {
                            psychologicalDiagnostics.ajax.load.edit(form, event);
                        }
                    });
                }
            }
        }
    }
    return result;

}());

window.onload = function () {
    psychologicalDiagnostics.datatable.load.get();
    psychologicalDiagnostics.validate.load.create();
    psychologicalDiagnostics.validate.load.edit();    
}