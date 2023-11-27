var TeachersTable = function () {
    var select = {
        academicDepartment: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: `/departamentos-academicos/get`.proto().parseURL()
                }).done(function (data) {
                    $(".academicDepartment-select").select2({
                        placeholder: "Sin asignar",
                        data: data,
                        allowClear: true
                    });
                });
            },
            events: function () {
                $(".academicDepartment-select").on("change", function () {
                    datatable.teachers.reload();
                });
            }
        },
        init: function () {
            this.academicDepartment.init();
        }
    };

    var datatable = {
        teachers: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/docentes/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.search = $("#search").val();
                        data.academicDepartmentId = $("#academicDepartmentId").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Foto",
                        data: null,
                        orderable: false,
                        render: function (data, type, row) {
                            var tmp = "";
                            if (row.picture != null) {
                                tmp += `<img src="/imagenes/${row.picture}" width="50px" />`;
                            } else {
                                tmp += `<img src="/images/demo/user.png" width="50px" />`;
                            }
                            return tmp;
                        }
                    },
                    {
                        title: "Nombre y Apellidos",
                        data: "fullName"
                    },
                    {
                        title: "DNI",
                        data: "dni"
                    },
                    {
                        title: "Usuario",
                        data: "userName"
                    },
                    {
                        title: "Teléfono",
                        data: "phoneNumber"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data, type, row) {
                            return `<button data-id="${
                                row.id
                                }" class="btn btn-primary btn-sm m-btn m-btn--icon btn-edit" title="Editar"><span><i class="la la-edit"></i><span>Editar</span></span></button> <button data-id="${
                                row.id
                                }" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete" title="Eliminar"><i class="la la-trash"></i></button>`;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#data-table").on('click', '.btn-edit', function () {
                    var aux = $(this);
                    var id = aux.data('id');
                    location.href = '/admin/docentes/editar/' + id;
                });

                $("#data-table").on('click', '.btn-delete', function () {
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
                                    url: "/admin/docentes/eliminar/post".proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        id: id
                                    },
                                    success: function (result) {
                                        datatable.ajax.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El usuario ha sido eliminado con éxito",
                                            confirmButtonText: "Entendido"
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
                            })
                        }
                    });
                });
            }
        },
        init: function () {
            this.teachers.init();
        }
    };

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.teachers.reload();
            });
        }
    }

    return {
        init: function () {
            select.init();
            datatable.init();
            search.init();
        }
    }
}();

$(function () {
    TeachersTable.init();
});