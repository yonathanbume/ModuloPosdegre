var InitApp = function () {
    var datatable = {
        report: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/reporte_aprobados_desaprobados_cursos/seccion/datatable/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.termId = $("#select_terms").val();
                        data.careerId = $("#select_careers").val();
                        data.academicYear = $("#academicYear").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Cod. Alumno",
                        data: "userName"
                    },
                    {
                        title: "Nombre Completo",
                        data: "fullName"
                    },
                    {
                        title: "Periodo Académico",
                        data: "term"
                    },
                    {
                        title: "Sección",
                        data: "section"
                    },
                    {
                        title: "Curso",
                        data: "course"
                    },
                    {
                        title: "Nota Final",
                        data: "finalGrade",
                        orderable: false
                    },
                    {
                        title: "Promedio Semestral",
                        data: "weightedAverageGrade",
                        orderable: false
                    },
                    {
                        title: "% de Asistencia",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";

                            if (data.totalClassSection == 0) {
                                template += `No hay clases creadas`
                            } else {
                                var percentageValue = (data.attendanceClass * 100.00) / data.totalClassSection;
                                template += `${percentageValue} %`
                            }
                            return template;
                        }
                    }, 
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        title: 'Reporte de estudiantes',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7]
                        }
                    },
                    {
                        extend: 'pdfHtml5',
                        title: 'Reporte de estudiantes',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7]
                        }
                    },
                ]
            },
            reload: function () {
                if (this.object == null) {
                    this.object = $("#data-table").DataTable(this.options);
                } else {
                    this.object.ajax.reload();
                }

            },
        },
    };

    var search = {
        init: function () {
            $("#btn-search").on('click', function () {
                if ($("#select_terms").val() == null || $("#select_terms").val() == "" || $("#select_terms").val() == "undefined" ) {
                    showToastrFail("Debe completar todos los campos");
                } else {
                    datatable.report.reload();
                }
            });
        }
    };

    var select2 = {
        init: function () {
            this.terms.init();
            this.careers.init();
            this.academicYears.init();
        },
        terms: {
            init: function () {
                $("#select_terms").select2();
                this.load();
            },
            load: function () {
                $.ajax({
                    url: ("/periodos/get").proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#select_terms").select2({
                        data: result.items
                    });

                    if (result.selected != null) {
                        $("#select_terms").val(result.selected).trigger("change");
                    }
                });
            }
        },
        careers: {
            init: function () {
                $.ajax({
                    url: `/carreras/get`.proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#select_careers").select2({
                        data: result.items
                    });
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


