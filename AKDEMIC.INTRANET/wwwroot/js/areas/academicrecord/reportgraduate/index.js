var report = function () {

    var datatable = {
        object: null,
        options: {
            ajax: {
                url: `/registrosacademicos/reporte-egresados/get`.proto().parseURL(),
                type: "GET",
                data: function (result) {
                    delete result.columns;
                    result.gradeType = $("#type").val();
                    result.careerId = $("#career").val();
                    result.year = $("#year").val();
                }
            },
            columns: [
                { title: "Código", data: "code" },
                { title: "Apellidos Nombres", data: "name" },
                { title: "Sexo", data: "sex" },
                { title: "Prácticas", data: "practices" },
                { title: "Teléfono", data: "number" },
                { title: "Escuela Profesional", data: "career" },
                { title: "Correo", data: "email" },
                { title: "DNI", data: "dni" },
                { title: "Dirección", data: "address" }
            ],
            buttons: [
                {
                    extend: 'excel',
                    title: 'Reporte de egresados',
                    messageTop: "\n"
                },
                {
                    extend: 'pdf',
                    orientation: 'landscape',
                    pageSize: 'A4',
                    title: 'Reporte de egresados'
                }
            ]
        },
        reload: function () {
            datatable.object.ajax.reload();
        },
        init: function () {
            datatable.object =  $("#table").DataTable(datatable.options); 
        }
    };

    var select2 = {
        type: {
            init: function () {
                $("#type").select2({
                    placeholder: "Seleccionar tipo",
                    minimumResultsForSearch: -1
                }).on("change", function () {
                    datatable.reload();
                });
            }
        },
        year: {
            load: function () {
                $.ajax({
                    url: "/registrosacademicos/reporte-egresados/get-años-egreso"
                })
                    .done(function (e) {
                        $("#year").select2({
                            placeholder: "Seleccionar año",
                            data: e,
                            minimumResultsForSearch: -1
                        }).on("change", function () {
                            datatable.reload();
                        });;
                    });
            },
            init: function () {
                this.load();
            }
        },
        career: {
            load: function () {
                $.ajax({
                    url: "/carreras/registroacademico/get"
                })
                    .done(function (e) {
                        $("#career").select2({
                            placeholder: "Seleccionar carrera",
                            data: e
                        }).on("change", function () {
                            datatable.reload();
                        });;
                    });
            },
            init: function () {
                this.load();
            }
        },
        init: function () {
            select2.type.init();
            select2.career.init();
            select2.year.init();
        }
    };

    return {
        init: function () {
            datatable.init();
            select2.init();
        }
    };
}();

$(function () {
    report.init();
});