var GenerateFormat = function () {
    var private = {
        objects: {}
    };

    var searchByDate = function () {
        $("#btn-search-dates").on('click', function (e) {
            e.preventDefault();
            var dateStartVal = $("#dateStartFilter").val();
            var dateEndVal = $("#dateEndFilter").val();
            if (dateStartVal === null || dateEndVal === null) {
                return false;
            } else {
                private.objects["tbl-data"].draw();
            }


        });

        $("#btn-search-filters").on('click', function (e) {
            e.preventDefault();
            private.objects["tbl-data"].draw();
        });
    };

    var inputs = {
        init: function () {
            $("#search").doneTyping(function () {
                private.objects["tbl-data"].draw();
            });

            $("#searchBookNumber").doneTyping(function () {
                private.objects["tbl-data"].draw();
            });
        }
    };
    var options = {
        columnDefs: [
            { "orderable": false, "targets": [1] }
        ],
        ajax: {
            type: "GET",
            dataType: "JSON",
            url: `/admin/generacion-de-formato-registro/listado`.proto().parseURL(),
            data: function (values) {
                values.searchValue = $("#search").val();
                values.searchBookNumber = $("#searchBookNumber").val();
                values.dateStartFilter = $("#dateStartFilter").val();
                values.dateEndFilter = $("#dateEndFilter").val();
            }
        },
        columns: [

            { data: "user" },
            { data: "dni" },
            { data: "careerName" },
            {
                data: null,
                render: function (data, type, row, meta) {                             
                    return `<p> ${row.type} </p>`;
                }
            }
        ]
    };

    var events = function () {
        $(".btn-format").on('click', function (e) {
            e.preventDefault();
            var $btn = $(this);
            $btn.addLoader();
            
            $.fileDownload(`/admin/generacion-de-formato-registro/generar-formatos`.proto().parseURL(), {
                httpMethod: "GET",
                data: {
                    searchValue : $("#search").val(),
                    searchBookNumber: $("#searchBookNumber").val(),
                    dateStartFilter: $("#dateStartFilter").val(),
                    dateEndFilter: $("#dateEndFilter").val()
                }
            })
                .always(function () {
                    $btn.removeLoader();
                }).done(function () {
                    toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                }).fail(function () {
                    toastr.error("No se pudo descargar el archivo", "Error");
                });



        });
    };

    var datepicker = function () {
        $("#dateStartFilter").datepicker();
        $("#dateEndFilter").datepicker();



        $("#dateStartFilter").datepicker({
            clearBtn: true,
            orientation: "bottom",
            format: _app.constants.formats.datepicker
        }).on("changeDate", function (e) {
            $("#dateEndFilter").datepicker("setStartDate", moment(e.date).toDate());

        });

        $("#dateEndFilter").datepicker({
            clearBtn: true,
            orientation: "bottom",
            format: _app.constants.formats.datepicker
        }).on("changeDate", function (e) {
            $("#dateStartFilter").datepicker("setEndDate", e.date);


        });
    };
    var dataTable = {
        init: function () {
            private.objects["tbl-data"] = $("#tbl-data").DataTable(options);            
        }
    };

    return {
        init: function () {
            datepicker();
            events();
            dataTable.init();
            inputs.init();      
            searchByDate();
        }
    };
}();

$(function () {
    GenerateFormat.init();
});