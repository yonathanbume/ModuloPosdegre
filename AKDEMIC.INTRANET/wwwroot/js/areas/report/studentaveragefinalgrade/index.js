var InitApp = function () {
    var datatable = {
        students: {
            object: null,
            options: {
                serverSide: false,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/reporte/estudiantes-promedios-del-periodo/datatable/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.search = $("#search").val();
                        data.termId = $("#termId").val();
                        data.academicYear = $("#academicYear").val();
                        data.careerId = $("#careerId").val();
                        data.facultyId = $("#facultyId").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Usuario",
                        data: "userName"
                    },
                    {
                        title: "Apellido Paterno",
                        data: "paternalSurname"
                    },
                    {
                        title: "Apellido Materno",
                        data: "maternalSurname"
                    },
                    {
                        title: "Nombres",
                        data: "name"
                    },
                    {
                        title: "Escuela Profesional",
                        data: "career"
                    },
                    {
                        title: "Promedio Ponderado",
                        data: "weightedAverageGrade"
                    },
                    {
                        title: "Promedio Aritmetico",
                        data: "arithmeticAverageGrade"
                    }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excel',
                        title: 'Reporte de promedios de estudiante por periodo académico'
                    },
                    {
                        extend: 'pdf',
                        title: 'Reporte de promedios de estudiante por periodo académico'
                    }
                ]
            },
            reload: function () {
                if (this.object == null) {
                    this.object = $("#data-table").DataTable(this.options);
                } else {
                    this.object.ajax.reload();
                }
            }
        }
    };
    var search = {
        init: function () {
            $("#btn-search").on('click', function () {
                if ($("#termId").val() == null || $("#termId").val() == "" || $("#termId").val() == "undefined" || $("#termId").val() == "0" ||
                    $("#facultyId").val() == null || $("#facultyId").val() == "" || $("#facultyId").val() == "undefined" || $("#facultyId").val() == "0" ) {
                    showToastrFail("Debe escoger un Periodo y una Facultad al menos");
                } else {
                    datatable.students.reload();
                }
            });
        }
    };
    var select2 = {
        init: function () {
            this.terms.init();
            this.academicYears.init();
            this.faculties.init();
            this.careers.init();
        },
        terms: {
            init: function () {
                $("#termId").select2();
                this.load();
            },
            load: function () {
                $.ajax({
                    url: ("/periodos/get").proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#termId").select2({
                        data: result.items
                    });

                    if (result.selected != null) {
                        $("#termId").val(result.selected).trigger("change");
                    }
                });
            }
        },
        academicYears: {
            init: function () {
                $("#academicYear").select2();
                this.load();
            },
            load: function () {
                $.ajax({
                    url: ("/ciclos/numeros-romanos/get").proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#academicYear").select2({
                        data: result
                    });
                });
            }
        },
        careers: {
            init: function () {
                $("#careerId").select2();
            },
            load: function (id) {
                $("#careerId").prop("disabled", false);
                $("#careerId").empty();
                $("#careerId").html('<option value="0" selected>Todas</option>');
                $.ajax({
                    url: (`/facultades/${id}/carreras/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#careerId").select2({
                        data: result.items
                    });
                });
            }
        },
        faculties: {
            init: function () {
                $("#facultyId").select2();
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: ("/facultades/get").proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#facultyId").select2({
                        data: result.items
                    });
                });
            },
            events: function () {
                $("#facultyId").on("change", function () {
                    let id = $(this).val();
                    select2.careers.load(id);
                });
            }
        }
    };

    return {
        init: function () {
            search.init();
            select2.init();
        }
    }
}();

$(function () {
    InitApp.init();
});
