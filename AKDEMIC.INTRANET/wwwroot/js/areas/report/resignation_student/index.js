var index = function () {

    var datatable = {
        resignation: {
            object: null,
            options: {
                ajax: {
                    url: `/reporte/estudiantes-renuncia/get`,
                    type: "GET",
                    data: function (data) {
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        data: "userName",
                        title : "Usuario"
                    },
                    {
                        data: "fullName",
                        title : "Nombre Completo"
                    },
                    {
                        data: "dni",
                        title :"Documento"
                    },
                    {
                        data: "career",
                        title : "Escuela profesional"
                    },
                    {
                        data: "resignationDateTime",
                        title: "Fec. Renuncia"
                    }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Excel',
                        action: function (e, dt, node, config) {
                            var url = `/reporte/estudiantes-renuncia/get-excel`
                            window.open(url, "_blank");
                        }
                    }
                ]
            },
            init: function () {
                datatable.resignation.object = $("#students_table").DataTable(datatable.resignation.options);
            }
        },
        init: function () {
            datatable.resignation.init();
        }
    }

    var events = {
        search: function () {
            $("#search").doneTyping(function (e) {
                datatable.resignation.object.ajax.reload();
            })
        },
        init: function () {
            events.search();
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