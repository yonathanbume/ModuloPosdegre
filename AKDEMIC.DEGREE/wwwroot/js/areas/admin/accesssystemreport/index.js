var AccessSystemReport = function () {

    var private = {
        objects: {}
    };
    var inputs = {
        init: function () {
            $("#search").doneTyping(function () {
                private.objects["tbl-data"].draw();
            });
        },
        buttonsearch: function () {
            $("#btnsearch").on('click', function () {
                private.objects["tbl-data"].draw();
            });
        }
    };


    var options = {
        columnDefs: [
            { "orderable": false }
        ],
        ajax: {
            type: "GET",
            dataType: "JSON",
            url: `/admin/accesos-sistema/reporte-accesos-sistema-listado`.proto().parseURL(),
            data: function (values) {
                values.search = $("#search").val();
                values.roleType = $("#roleTypeSelect").val();
                values.startDate = $("#startDate").val();
                values.endDate = $("#finalDate").val();
            }
        },
        columns: [

            { data: "code", title: "Código" },
            { data: "name", title: "Nombres completos" },
            { data: "career", title: "Escuela Profesional" },
            { data: "firstLoginStr", title: "Primer logeo" },
            { data: "lastLoginStr", title: "Último logeo" }
        ]
    };


    var dataTable = {
        init: function () {
            private.objects["tbl-data"] = $("#tbl-data").DataTable(options);
        }
    };

    var select = {
        init: function () {
            $("#roleTypeSelect").select2();
        }
    };

    var datepickers = {
        init: function () {
            $("#startDate").datepicker();
            $("#finalDate").datepicker();
        },

    };

    return {
        init: function () {
            dataTable.init();
            inputs.init();
            select.init();
            datepickers.init();
            inputs.buttonsearch();
        }
    };
}();

$(function () {
    AccessSystemReport.init();
});