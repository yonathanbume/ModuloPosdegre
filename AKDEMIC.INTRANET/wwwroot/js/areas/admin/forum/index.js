var forums = (function () {
    var result = {
        ajax: {
            list: {},
            load: {
                create: function (element, event) {
                    var formElements = element.elements;
                    forums.ajax.list["forums-ajax-create"] = $.ajax({
                        data: $(element).serialize(),
                        //data: {
                        //    Name: formElements["Name"].value,
                        //    Description: formElements["Description"].value,
                        //    Careers: formElements["Careers"].values
                        //},
                        type: element.method,
                        url: element.action,
                        beforeSend: function (jqXHR, settings) {
                            mApp.block($(element).closest(".modal-content"));
                        },
                        complete: function (jqXHR, textStatus) {
                            mApp.unblock($(element).closest(".modal-content"));
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                        },
                        success: function (data, textStatus, jqXHR) {
                            forums.datatable.list["ajax_data"].reload();
                            forums.modal.list["forums-modal-create"].modal("hide");
                            toastr.success(_app.constants.toastr.message.success.create, _app.constants.toastr.title.success);
                        }
                    });
                },
                editById: function (element, event) {

                    var id = $(element).data("id");
                    forums.ajax.list["forums-ajax-edit"] = $.ajax({
                        data: {
                            Id: id
                        },
                        type: 'GET',
                        url: ('/admin/foros/editar/' + id + '').proto().parseURL(),
                        error: function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                        },
                        success: function (data, textStatus, jqXHR) {
                            //forums.datatable.list["ajax_data"].reload();
                            //forums.modal.list["forums-modal-"].modal("hide");
                            //toastr.success(_app.constants.toastr.message.success.edit, _app.constants.toastr.title.success);
                            var formElements = $('#forums-modal-edit-form');
                            let active = data.active;
                            //$('#activeForum').value = data.active;

                            $("#activeForum option[value='" + active + "']").prop('selected', true);
                            $("#activeForum").trigger("change");
                            $("input[name='Name']").val(data.name);
                            $("textarea[name='Description']").val(data.description);
                            $("input[name='Id']").val(data.id);
                            $("select[name='Careers']").val(data.careers);
                            $("select[name='Careers']").trigger("change");
                        }
                    });
                },
                edit: function (element, event) {
                    var formElements = element.elements;
                    forums.ajax.list["forums-ajax-edit"] = $.ajax({
                        data: $(element).serialize(),
                        //data: {
                        //    Id : formElements["Id"].value,
                        //    Name: formElements["Name"].value,
                        //    Active: $("#activeForum option:selected").val(), // formElements["Active"].value,
                        //    Description: formElements["Description"].value
                        //},

                        type: element.method,
                        url: element.action,
                        beforeSend: function (jqXHR, settings) {
                            mApp.block($(element).closest(".modal-content"));
                        },
                        complete: function (jqXHR, textStatus) {
                            mApp.unblock($(element).closest(".modal-content"));
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.update, _app.constants.toastr.title.error);
                        },
                        success: function (data, textStatus, jqXHR) {

                            forums.datatable.list["ajax_data"].reload();
                            forums.modal.list["forums-modal-edit"].modal("hide");
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
                    forums.datatable.list["ajax_data"] = $("#ajax_data").mDatatable({
                        data: {
                            source: {
                                read: {
                                    method: "GET",
                                    url: ("/admin/foros/get").proto().parseURL(),
                                },
                            },
                        },
                        columns: [
                            {
                                field: "name",
                                title: "Nombre"
                            },
                            {
                                field: "description",
                                title: "Descripción"
                            },
                            {
                                field: "state",
                                title: "Estado",
                                template: function (row) {
                                    if (row.state) {
                                        return '<span class="m-badge m-badge--success m-badge--wide">Activo</span>';
                                    }
                                    else {
                                        return '<span class="m-badge  m-badge--metal m-badge--wide">Inactivo</span>';
                                    }
                                }
                            },
                            {
                                field: "options",
                                title: "Opciones",
                                sortable: false,
                                filterable: false,
                                template: function (row) {
                                    var template = "";
                                    template += '<button type="button" data-id="' + row.id + '" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit" onClick="forums.modal.load.edit(this, event)"><span><i class="la la-edit"></i><span>Editar</span></span></button>';
                                    template += " <button class=\"btn btn-danger m-btn btn-sm m-btn--icon\" onclick =\"forums.swal.load.delete(this, event, '";
                                    template += ("/admin/foros/eliminar/post").proto().parseURL();
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
        select2: {
            init: function () {
                this.careers.init();
            },
            events: {
                init: function () {

                    $("#careers-select1").on("select2:select", function () {
                        var values = $('#careers-select1').val();

                        if (values !== undefined && values !== null && values.length > 0) {
                            var firstCount = values.length;
                            var hasEmptyGuid = values.filter(x => x === _app.constants.guid.empty).length > 0;
                            if (hasEmptyGuid) {

                                if (firstCount > 1) {
                                    toastr.success("Se ha seleccionado 'TODOS' como valor único.", "ADVERTENCIA");
                                }

                                 var newValues = values.filter(x => x === _app.constants.guid.empty);
                                $('#careers-select1').val(newValues).trigger("change");
                            }
                        } else {
                            $('#careers-select1').val("").trigger("change");
                            $('#careers-select1').prop("disabled", false);
                        }
                    });

                    $("#careers-select2").on("select2:select", function () {
                        var values = $('#careers-select2').val();

                        if (values !== undefined && values !== null && values.length > 0) {
                            var firstCount = values.length;
                            var hasEmptyGuid = values.filter(x => x === _app.constants.guid.empty).length > 0;
                            if (hasEmptyGuid) {

                                if (firstCount > 1) {
                                    toastr.success("Se ha seleccionado 'TODOS' como valor único.", "ADVERTENCIA");
                                }

                                var newValues = values.filter(x => x === _app.constants.guid.empty);
                                $('#careers-select2').val(newValues).trigger("change");
                            }
                        } else {
                            $('#careers-select2').val("").trigger("change");
                            $('#careers-select2').prop("disabled", false);
                        }
                    });
                }
            },
            careers: {
                init: function () {
                    $(".select2-careers").prop("disabled", true);
                    $.ajax({
                        url: `/admin/foros/carreras`.proto().parseURL()
                    }).done(function (result) {
                        $(".select2-careers").empty();
                        result.unshift({ id: _app.constants.guid.empty, text: "Todas" });
                        $(".select2-careers").select2({
                            data: result,
                            placeholder: "Carrera"
                        });
                        if (result.length > 1) {
                            $(".select2-careers").prop("disabled", false);
                        }
                        $(".select2-careers").trigger("change");

                        forums.select2.events.init();
                    });
                }
            }
        },
        modal: {
            list: {},
            load: {
                create: function (element, event) {
                    var forumsModalCreateForm = document.getElementById("forums-modal-create-form");
                    $(".select2-forumactive").select2();  
                    _app.modules.form.reset({
                        element: forumsModalCreateForm
                    });

                    forums.modal.list["forums-modal-create"] = $("#forums-modal-create").modal("show");
                },
                edit: function (element, event) {
                    var forumsModalEditForm = document.getElementById("forums-modal-edit-form");

                    _app.modules.form.reset({
                        element: forumsModalEditForm
                    });

                    forums.ajax.load.editById(element, event);
                    forums.modal.list["forums-modal-edit"] = $("#forums-modal-edit").modal("show");
                }
            }
        },
        swal: {
            list: {},
            load:{
                    delete: function (element, event, url, data) {
                    data = data.proto().decode();

                    forums.swal.list["forums-swal-delete"] = swal({
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
                                        forums.datatable.list["ajax_data"].reload();
                                        toastr.success(_app.constants.toastr.message.success.delete, _app.constants.toastr.title.success);
                                    },
                                    error: function (jqXHR, textStatus, errorThrown) {
                                        toastr.error("La categoría tiene información relacionada", _app.constants.toastr.title.error);
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
                    forums.validate.list["forums-modal-create-form"] = $("#forums-modal-create-form").validate({
                        submitHandler: function (form, event) {
                            event.preventDefault();
                            forums.ajax.load.create(form, event);                            
                        }
                    });
                },
                edit: function () {

                    forums.validate.list["forums-modal-edit-form"] = $("#forums-modal-edit-form").validate({
                        submitHandler: function (form, event) {
                            forums.ajax.load.edit(form, event);   
                        }
                    });              
                } 
            }
        }
    }

    return result;

}());

window.onload = function () {
    forums.datatable.load.get();
    forums.validate.load.create();
    forums.validate.load.edit();
    forums.select2.init();
    $("#activeForum").select2();
}