var RoleTable = function () {
    var iterator = 0;
    var arr = [];
    var roleDatatable;
    var options = {
        search: {
            input: $("#search")
        },
        data: {
            type: "remote",
            source: {
                read: {
                    method: "GET",
                    url: "/admin/permisos/roles/get".proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "name",
                title: "Nombre",
                width: 300
            },
            {
                field: "options",
                title: "Opciones",
                width: 250,
                sortable: false, // disable sort for this column
                filterable: false, // disable or enable filtering
                template: function (row) {
                    return `<button data-id="${
                        row.id
                        }" class="btn btn-primary btn-sm m-btn m-btn--icon btn-edit" title="Editar"><span><i class="la la-edit"></i><span>Editar</span></span></button> <button data-id="${
                        row.id
                        }" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete" title="Eliminar"><i class="la la-trash"></i></button>`;
                }
            }
        ]
    };

    var loadPermissions = function (id) {
        $.ajax({
            url: ("/admin/permisos/roles/" + id + "/get").proto().parseURL(),
            type: "GET",
            data: {
                id: id
            },
            success: function (result) {
                $("#roleid").val(id);
                $("#role_modal").modal("show");
                $('#rDescription').val(result.role.name);

                if (result.role.isStatic) {
                    $("#rDescription").attr('disabled', 'disabled');
                }

                document.getElementById("role-permissions").innerHTML = "";
                for (var i = 0; i < result.permissions.length; i++) {
                    var permission = result.permissions[i];

                    var htmldata = '<div id="p-' + (++iterator) + '" class="form-group col-lg-12" style="display:flex;"><input class="roleId" value="' + permission.id + '" hidden/><input class="form-control m-input answer" style="margin-right: 10px;" required value="' + permission.name + '" disabled><button class="btn btn-danger btn-sm m-btn--icon remove-permission" data-key="' + iterator + '" type="button"><span><i class="la la-trash"></i></span></button></div>';

                    arr.push({
                        value: permission.value,
                        domkey: iterator
                    });

                    var e = document.createElement('div');
                    e.innerHTML = htmldata;
                    document.getElementById("role-permissions").appendChild(e.firstChild);

                    //console.log(arr);
                }
            },
            error: function () {
                toastr.error("Error al cargar los permisos", _app.constants.toastr.title.error);
            }
        });
    };

    var addEditRole = function () {
        var roleid = $("#roleid").val();
        $.ajax({
            url: "/admin/permisos/registrar/post",
            type: "POST",
            data: {
                id: roleid,
                name: $("#rDescription").val(),
                listPermissions: arr
            },
            beforeSend: function () {
                DefaultAjaxFunctions.beginAjaxCall();
            },
            success: function () {
                $("#role_modal").modal("hide");
                $('#rDescription').val("");
                document.getElementById("role-permissions").innerHTML = "";
                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
            },
            error: function () {
                toastr.error("No se puedo completar su solicitud", _app.constants.toastr.title.error);
            },
            complete: function () {
                roleDatatable.reload();
                DefaultAjaxFunctions.endAjaxCall();
            }
        });
    };

    var events = {
        init: function () {

            roleDatatable.on("click", ".btn-edit", function () {
                loadPermissions($(this).data("id"));
            });

            roleDatatable.on("click", ".btn-delete", function () {
                var id = $(this).data("id");
                swal({
                    title: "¿Está seguro?",
                    text: "El rol será eliminado",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: "/admin/permisos/roles/eliminar/post".proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: id
                                },
                                success: function (result) {
                                    roleDatatable.reload();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El rol ha sido eliminado con éxito",
                                        confirmButtonText: "Continuar"
                                    });
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "Al parecer el rol tiene usuarios relacionados"
                                    });
                                }
                            });
                        });
                    }
                });
            });

            $("#add-permission").click(function () {
                var text = $('#Permissions').find(":selected").text();
                var val = $('#Permissions').find(":selected").val();

                var htmldata = '<div id="p-' + (++iterator) + '" class="form-group col-lg-12" style="display:flex;"><input class="roleId" value="" hidden/><input value="' + text + '" class="form-control m-input answer" style="margin-right: 10px;" disabled><button class="btn btn-danger btn-sm m-btn--icon remove-permission" data-key="' + iterator + '" type="button"><span><i class="la la-trash"></i></span></button></div>';

                arr.push({
                    value: val,
                    domkey: iterator
                });

                var e = document.createElement('div');
                e.innerHTML = htmldata;
                document.getElementById("role-permissions").appendChild(e.firstChild);
            });

            $("body").on("click", ".remove-permission", function () {
                event.preventDefault();
                var id = $(this).data("key");

                var _id = "#p-" + id;
                $(_id).remove();
                arr.splice(arr.findIndex(x => x.domkey === id), 1);
            });

            $("#role_modal").on("hidden.bs.modal",
                function () {
                    $("#roleid").val("");
                    $("#rDescription").val("");
                    $("#rDescription").removeAttr('disabled');
                    iterator = 0;
                    arr = [];
                    document.getElementById("role-permissions").innerHTML = "";
                });

        }
    };

    var datatable = {
        init: function () {
            roleDatatable = $(".m-datatable").mDatatable(options);
            events.init();
            $(".add").click(function () {
                addEditRole();
            });
        }
    };
    return {
        init: function () {
            datatable.init();
            //modal.init();
        }
    };
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
        $("#role-permissions").modal("hide");
        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
    };
    var createFailure = function (e) {
        if (e.responseText !== null && e.responseText !== "") $("#create_msg_txt").html(e.responseText);
        else $("#create_msg_txt").html(_app.constant.ajax.message.error);

        $("#create_msg").removeClass("m--hide").show();
    };
    var editFailure = function (e) {
        if (e.responseText !== null && e.responseText !== "") $("#edit_msg_txt").html(e.responseText);
        else $("#edit_msg_txt").html("asdasd");

        $("#edit_msg").removeClass("m--hide").show();
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
        },
        editFailure: function (e) {
            editFailure(e);
        }
    };
}();

$(function () {
    RoleTable.init();
});