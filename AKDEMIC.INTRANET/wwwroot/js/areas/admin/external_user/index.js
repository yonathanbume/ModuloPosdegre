var index = function () {

    var datatable = {
        external_user: {
            object: null,
            options: {
                ajax: {
                    url: `/admin/usuarios-externos/get-datatable`,
                    type: "GET",
                    data: function (data) {
                        data.search = $("#search").val()
                    }
                },
                columns: [
                    {
                        data: "userName",
                        title: "Usuario"
                    },
                    {
                        data: "fullName",
                        title: "Nombre Completo"
                    },
                    {
                        data: "document",
                        title: "Documento"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        render: function (row) {
                            var tpm = "";
                            tpm += `<a href="/admin/usuarios-externos/editar/${row.id}" class="btn btn-primary btn-sm m-btn  m-btn m-btn--icon"><span><i class="la la-edit"></i><span>Editar</span></span></a>`;
                            return tpm;
                        }
                    }
                ]
            },
            reload: function () {
                datatable.external_user.object.reload();
            },
            init: function () {
                datatable.external_user.object = $("#users-datatable").DataTable(datatable.external_user.options);
            }
        },
        init: function () {
            datatable.external_user.init();
        }
    }

    var events = {
        onSearch: function () {
            $("#search").doneTyping(function () {
                datatable.external_user.reload();
            })
        },
        init: function () {
            this.onSearch();
        }
    }

    return {
        init: function () {
            datatable.init();
            events.init();
        }
    }
}();

$(() => {
    index.init();
});