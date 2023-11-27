var UserTable = function () {
    var userDatatable = null;

    var options = {
        responsive: true,
        processing: true,
        serverSide: true,
        ajax: {
            url: "/admin/usuarios/get".proto().parseURL(),
            data: function (d) {
                d.search = $("#search").val();
                d.rolesJson = JSON.stringify($("#select_roles").val());
            }
        },
        columns: [
            {
                title: "Foto",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<img src="${row.picture || "/images/demo/user.png"}" width="50px" />`;
                    return tmp;
                }
            },
            { title: "Nombre y Apellidos", data: "fullName" },
            { title: "DNI", data: "dni" },
            { title: "Usuario", data: "userName" },
            { title: "Teléfono", data: "phoneNumber" },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    return `<button data-id="${row.id}" class="btn btn-info btn-sm m-btn m-btn--icon btn-detail" title="Detalle"><span><i class="la la-eye"></i><span>Ver Detalle</span></span></button> <button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-edit" title="Editar"><span><i class="la la-edit"></i><span>Editar</span></span></button> <button data-id="${row.id}" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete" title="Eliminar"><i class="la la-trash"></i></button>`;
                }
            }
        ]
    };


    var events = {
        init: function () {
            userDatatable.on("click",
                ".btn-detail",
                function () {
                    var aux = $(this);
                    var id = aux.data("id");
                    location.href = `/admin/usuarios/${id}/detalle`.proto().parseURL();
                });

            userDatatable.on("click", ".btn-edit", function () {
                var aux = $(this);
                var id = aux.data("id");
                location.href = `/admin/usuarios/${id}/editar`.proto().parseURL();
            });

            userDatatable.on("click", ".btn-delete", function () {
                var id = $(this).data("id");
                swal({
                    title: "¿Está seguro?",
                    text: "El usuario será eliminado",
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
                                url: "/admin/usuarios/eliminar/post".proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: id
                                },
                                success: function (result) {
                                    userDatatable.ajax.reload();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El usuario ha sido eliminado con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "Al parecer el usuario tiene información relacionada"
                                    });
                                }
                            });
                        });
                    }
                });
            });

            $("#search").doneTyping(function () {
                datatable.init();
            });
        }
    }

    var datatable = {
        init: function () {
            if (userDatatable) {
                userDatatable.ajax.reload();
            }
            else {
                userDatatable = $(".users-datatable").DataTable(options);
                events.init();
            }
        }
    }

    var select = {
        init: function () {
            $.ajax({
                url: ("/roles/get").proto().parseURL()
            }).done(function (data) {
                $("#select_roles").select2({
                    placeholder: "Seleccionar roles...",
                    minimumInputLength: 0,
                    data: data.items
                }).
                    on("change", function () {
                        datatable.init();
                    })
            });
        }
    }

    return {
        init: function () {
            datatable.init();
            select.init();
        }
    }
}();

$(function () {
    UserTable.init();
});