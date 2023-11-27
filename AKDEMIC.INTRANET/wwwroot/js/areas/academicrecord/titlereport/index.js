var TitleReportManagement = function () {
    var private = {
        objects: {}
    };
    var inputs = {
        init: function () {
            $("#search").doneTyping(function () {
                private.objects["tbl-data"].draw();
            });
        }
    };

    var select = function () {
        $('#careerId').select2({
            allowClear: true,
            placeholder: "Buscar..",
            ajax: {
                type: 'GET',
                url: ("/registrosacademicos/informes-de-titulos/obtener-carreras").proto().parseURL(),
                delay: 1000,
                data: function (params) {
                    return {
                        searchValue: params.term
                    };
                },
                processResults: function (data) {
                    return {
                        results: data.results
                    };                    
                },
                escapeMarkup: function (markup) {
                    return markup;
                },
                minimumInputLength: 1
            }
        }); 
        $('#careerId').on('change', function () {
            private.objects["tbl-data"].draw();
        })
    };

    var options = {
        columnDefs: [
            { "orderable": false, "targets": [1] }
        ],
        ajax: {
            type: "GET",
            dataType: "JSON",
            url: `/registrosacademicos/informes-de-titulos/obtener-informes-titulos`.proto().parseURL(),
            data: function (values) {
                values.searchValue = $("#search").val();
                values.careerId = $("#careerId").val();
            }
        },
        columns: [

            { data: "userName", title: "Código" },
            { data: "name", title: "Nombres" },
            { data: "paternalSurname", title: "Apellidos paternos" },
            { data: "maternalSurname", title: "Apellidos maternos" },
            { data: "careerName", title: "Escuela Profesional" },
            {
                data: null,
                title: "Opciones",
                render: function (data, type, row, meta) {
                    return `<button data-id="${row.id}" class="btn btn-info btn-sm m-btn m-btn--icon detail" title="Ver detalles"><i class="la la-eye"></i></button> 
                            <button data-id="${row.id}" class="btn btn-success btn-sm m-btn m-btn--icon download" title="Descargar constancia de grado"><i class="la la-download"></i></button>`;

                }
            }
        ]
    };   


    var events = {
        datatable_init: function () {
            private.objects["tbl-data"].on("click", ".detail",
                function () {
                    var titlereportId = $(this).data("id");
                    window.location.href = `/registrosacademicos/informes-de-titulos/detalle/${titlereportId}`.proto().parseURL();

                });

            private.objects["tbl-data"].on("click", ".download",
                function () {
                    var $btn = $(this);
                    $btn.addLoader();
                    var gradereportId = $(this).data("id");
                    $.fileDownload(`/registrosacademicos/informes-de-titulos/generar-constancia-titulo/${gradereportId}`.proto().parseURL())
                        .always(function () {
                            $btn.removeLoader();
                        }).done(function () {
                            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                        }).fail(function () {
                            toastr.error("No se pudo descargar el archivo", "Error");
                        });
                });
        }

    };
    var dataTable = {
        init: function () {
            private.objects["tbl-data"] = $("#title_report_datatable").DataTable(options);
            events.datatable_init();
        }
    };

    return {
        init: function () {
            dataTable.init();
            inputs.init();   
            select();
        }
    };
}();

$(function () {
    TitleReportManagement.init();
});