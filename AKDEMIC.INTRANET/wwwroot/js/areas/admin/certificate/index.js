var certificate = function () {
    var datatable = null;

    var options = getSimpleDataTableConfiguration({
        pageLength: 10,
        url: "/admin/alumnos/get".proto().parseURL(),
        data: function (d) {
            d.search = $("#search").val();
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
            { title: "Nombre y Apellidos", data: "user.fullName" },
            { title: "DNI", data: "user.dni" },
            { title: "Carrera", data: "career.name" },
            { title: "Usuario", data: "user.userName" },
            { title: "Teléfono", data: "user.phoneNumber" },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    return `<button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-pdf"<span><i class="la la-download"> </i> </span>    Descargar certificado</span></span></button> `;
                }
            }
        ]
    });

    var loadDatatable = function () {

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }

        datatable = $(".students-datatable").DataTable(options);
        events.init();
        datatable.on('click', '.btn-pdf',
            function () {
                var sid = $(this).data("id");
                var $btn = $(this);
                $btn.addLoader();
                $.fileDownload(`/admin/certificado-de-estudios/pdf/${sid}`.proto().parseURL(), {
                    httpMethod: 'GET', successCallback: function (url) {
                        $btn.removeLoader();
                        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                    }
                });
            });

    };

    var events = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.ajax.reload();
            });
        }
    };

    return {
        load: function () {
            loadDatatable();
        }
    };
}();

$(function () {
    certificate.load();
})