var InitApp = function () {
    var excel = function () {

        $(".btn-excel").on('click', function (e) {
            e.preventDefault();
            var $btn = $(this);
            $btn.addLoader();
            $.fileDownload(`/reporte/egresados/reporte-excel`.proto().parseURL(), {
                httpMethod: 'GET',
                data: {
                    gradeType: $("#col0_filter").val(),
                    careerParameterId: $("#col1_filter").val(),
                    year: $("#col2_filter").val(),
                    admissionYear: $("#col3_filter").val()

                }

            }).always(function () {
                $btn.removeLoader();
            }).done(function () {
                toastr.success("Archivo descargado satisfactoriamente", "Éxito");
            }).fail(function () {
                toastr.error("No se pudo descargar el archivo", "Error");
            });

        });

    };

    var options = {
        filter: false,
        lengthChange: false,
        ajax: {
            url: `/reporte/egresados/listado`.proto().parseURL(),
            type: "GET",
            data: function (result) {
                delete result.columns;
                result.gradeType = $("#col0_filter").val();
                result.careerParameterId = $("#col1_filter").val();
                result.year = $("#col2_filter").val();
                result.admissionYear = $("#col3_filter").val();
            }
        },
        columns: [
            { title: "Código", data: "code" },
            { title: "Apellido Paterno", data: "paternalSurname" },
            { title: "Apellido Materno", data: "maternalSurname" },
            { title: "Nombres", data: "name" },
            { title: "Facultad", data: "faculty" },
            { title: "Escuela Profesional", data: "career" },
            { title: "Programa Académico/Especialidad", data: "academicProgram" },
            { title: "Sexo", data: "sex" },
            { title: "Prácticas", data: "practices" },
            { title: "Teléfono", data: "number" },
            { title: "Correo", data: "email" },
            { title: "DNI", data: "dni" },
            { title: "Dirección", data: "address" }
        ],
    };

    var datatable = $("#table").DataTable(options);

    var datepicker = {
        init: function () {
            this.admissionYear.init();
            this.graduatedYear.init();
        },
        admissionYear: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $("#col2_filter").datepicker({
                    format: "yyyy",
                    viewMode: "years",
                    minViewMode: "years"
                });
            },
            events: function () {
                $("#col2_filter").on('change', function () {
                    datatable.draw();
                });
            }
        },
        graduatedYear: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $("#col3_filter").datepicker({
                    format: "yyyy",
                    viewMode: "years",
                    minViewMode: "years"
                });
            },
            events: function () {
                $("#col3_filter").on('change', function () {
                    datatable.draw();
                });
            }
        }
    }

    $("#col0_filter").select2();
    $("#col1_filter").select2();

    $("#col0_filter").on('change', function () {
        if ($(this).val() == 6) {
            $("#col2_filter").prop("disabled", false);
        } else {
            $("#col2_filter").prop("disabled", false);
        }
        datatable.draw();
    });

    $("#col1_filter").on('change', function () {
        datatable.draw();
    });



    return {
        init: function () {
            excel();
            datepicker.init();
        }
    };
}();

$(function () {
    InitApp.init();
});