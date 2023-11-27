var users = function () {
    var datatable = {
        object: null,
        options: getSimpleDataTableConfiguration({
            url: "/registrosacademicos/personal/get-users".proto().parseURL(),
            data: function (data) {
                data.search = $("#search").val();
            },
            pageLength: 20,
            orderable: [],
            columns: [
                {
                    title: "Nombre",
                    data: "name"
                },
                {
                    title: "Usuario",
                    data : "username"
                },
                {
                    title: "Correo",
                    data : "email"
                },
                {
                    title: "Registro Academico",
                    data: null,
                    render: function (row) {
                        var tmp = "";
                        if (row.isRecordAcademic) {
                            tmp += "<span class='m-badge m-badge--brand m-badge--wide'>Asignado</span>";
                        } else {
                            tmp += "<span class='m-badge m-badge--metal m-badge--wide'>No Asignado</span>";
                        }
                        return tmp;
                    }
                },
                {
                    title: "Opciones",
                    data: null,
                    render: function (row) {
                        var tmp = "";
                        if (row.isRecordAcademic) {
                            tmp += "<button data-id='" + row.id + "' class='btn btn-primary btn-sm m-btn--icon btn-change-rol'><i class='la la-close'></i>Remover Rol</button>";
                        } else {
                            tmp += "<button data-id='" + row.id + "' class='btn btn-primary btn-sm m-btn--icon btn-change-rol'><i class='la la-check'></i>Asignar Rol</button>";
                        }
                        return tmp;
                    }
                }
            ]
        }),
        reload: function () {
            datatable.object.ajax.reload();
        },
        events: {
            OnAssignRemoveRol: function () {
                $("#users_datatable").on("click", ".btn-change-rol", function () {
                    var $btn = $(this);
                    $btn.addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                    var userId = $(this).data("id");
                    $.ajax({
                        url: `/registrosacademicos/personal/asignar-remover-rol?userId=${userId}`,
                        type: "POST"
                    })
                        .done(function (e) {
                            swal({
                                type: "success",
                                title: "Completado",
                                text: e,
                                confirmButtonText: "Aceptar"
                            });
                            datatable.reload();
                        })
                        .fail(function (e) {
                            swal({
                                type: "error",
                                title: "Error",
                                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                confirmButtonText: "Aceptar",
                                text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                            });
                        })
                        .always(function () {
                            $btn.removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                        });
                });
            },
            onSearch: function () {
                $("#search").doneTyping(function () {
                    datatable.reload();
                });
            },
            init: function () {
                this.onSearch();
                this.OnAssignRemoveRol();
            }
        },
        init: function () {
            datatable.object = $("#users_datatable").DataTable(datatable.options);
            datatable.events.init();
        }
    };

    return {
        init: function () {
            datatable.init();
        }
    };
}();

$(() => {
    users.init();
});