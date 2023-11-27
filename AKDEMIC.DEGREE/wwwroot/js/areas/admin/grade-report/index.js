var GradeReportManagement = function () {
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
  

    var options = {
        columnDefs: [
            { "orderable": false, "targets": [1] }
        ],
        ajax: {
            type: "GET",
            dataType: "JSON",
            url: `/coordinador-academico/informes-de-grados/obtener-informes-grados`.proto().parseURL(),
            data: function (values) {
                values.searchValue = $("#search").val();
        
            }
        },
        columns: [

            { data: "userName", title: "Código" },
            { data: "name", title: "Nombres" },
            { data: "paternalSurname", title: "Apellidos paternos" },
            { data: "maternalSurname", title: "Apellidos maternos" },
            { data: "careerName", title: "Escuela Profesional" },
            { data: "type", title: "Tipo de grado" },
            {
                data: null,
                title: "Opciones",
                render: function (data, type, row, meta) {
                    return `<button data-id="${row.id}" class="btn btn-info btn-sm m-btn m-btn--icon detail" title="Ver detalles"><i class="la la-eye"></i></button>`;

                }
            }
        ]
    };


    var events = {
        datatable_init: function () {
            private.objects["tbl-data"].on("click", ".detail",
                function () {
                    var gradereportId = $(this).data("id");
                    window.location.href = `/coordinador-academico/informes-de-grados/detalle/${gradereportId}`.proto().parseURL();

                });
        }

    };
    var dataTable = {
        init: function () {
            private.objects["tbl-data"] = $("#grade_report_datatable").DataTable(options);
            events.datatable_init();
        }
    };

    return {
        init: function () {
            dataTable.init();
            inputs.init();
      
        }
    };
}();

$(function () {
    GradeReportManagement.init();
});