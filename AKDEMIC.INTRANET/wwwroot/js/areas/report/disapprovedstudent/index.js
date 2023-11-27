var initApp = function () {

    var datatable = {
        students: {
            object: null,
            options: {
                pageLength: 50,
                ajax: {
                    url: "/reporte/estudiantes-desaprobados/get",
                    type: "GET",
                    data: function (data) {
                        delete data.columns;

                        data.termId = $("#term_select").val();
                        data.facultyId = $("#faculty_select").val();
                        data.careerId = $("#career_select").val();
                        data.studentTry = $("#try_select").val();
                    }
                },
                columns: [
                    {
                        title: "Escuela Profesional",
                        data: "career"
                    },
                    {
                        title: "Plan",
                        data: "curriculum"
                    },
                    {
                        title: "Código",
                        data: "username"
                    },
                    {
                        title: "Apellidos y Nombres",
                        data: "fullname"
                    },
                    {
                        title: "Ciclo",
                        data: "year"
                    },
                    {
                        title: "Cod Curso",
                        data: "code"
                    },
                    {
                        title: "Asignatura",
                        data: "course"
                    },
                    {
                        title: "Nro veces",
                        data: "time"
                    },
                    {
                        title: "Semestre",
                        data: "term"
                    },
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Excel',
                        action: function () {
                            var button = $(this)[0].node;
                            $(button).addClass("m-loader m-loader--right m-loader--primary").attr("disabled", true);
                            $.fileDownload(`/reporte/estudiantes-desaprobados/reporte-excel`.proto().parseURL(),
                                {
                                    httpMethod: 'GET',
                                    data: {
                                        termId: $("#term_select").val(),
                                        facultyId: $("#faculty_select").val(),
                                        careerId: $("#career_select").val(),
                                        studentTry : $("#try_select").val()
                                    },
                                    successCallback: function () {
                                        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                    }
                                }
                            ).done(function () {
                            })
                                .fail(function () {
                                    toastr.error("Error al descargar el archivo", "Error");
                                })
                                .always(function () {
                                    $(button).removeClass("m-loader m-loader--right m-loader--primary").attr("disabled", false);
                                });
                        }
                    },
                ]
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
            }
        }
    };

    var select = {
        term: {
            init: function () {
                $.ajax({
                    url: "/periodos/get".proto().parseURL()
                }).done(function (data) {
                    $("#term_select").select2({
                        data: data.items
                    });

                    if (data.selected !== null) {
                        $("#term_select").val(data.selected);
                        $("#term_select").trigger("change.select2");
                    }

                    datatable.students.init();

                    $("#term_select").on("change", function (e) {
                        datatable.students.reload();
                    });
                });
            }
        },
        faculty: {
            init: function () {
                $.ajax({
                    url: "/facultades/get".proto().parseURL()
                }).done(function (data) {

                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $("#faculty_select").select2({
                        data: data.items,
                        minimumResultsForSearch: 10
                    });

                    select.career.events();
                    select.faculty.events();
                });
            },
            events: function () {
                $("#faculty_select").on("change", function () {
                    var facultyId = $("#faculty_select").val();

                    if (facultyId === _app.constants.guid.empty) {
                        datatable.students.reload();

                        $("#career_select").empty();
                        $("#career_select").select2({
                            placeholder: "Seleccione una Escuela",
                            disabled: true
                        });
                    } else {
                        datatable.students.reload();
                        select.career.load($("#faculty_select").val());
                    }
                });
            }
        },
        career: {
            init: function () {
                $("#career_select").select2({
                    placeholder: "Seleccione una Escuela"
                });
            },
            load: function (faculty) {
                $.ajax({
                    url: `/facultades/${faculty}/carreras/v2/get?hasAll=false`.proto().parseURL()
                }).done(function (data) {
                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $("#career_select").empty();
                    $("#career_select").select2({
                        placeholder: "Seleccione una Escuela",
                        data: data.items,
                        minimumResultsForSearch: 10,
                        disabled: false
                    });
                });
            },
            events: function () {
                $("#career_select").on("change", function () {
                    datatable.students.reload();
                });
            }
        },
        try: {
            init: function () {
                $("#try_select").select2({
                    minimumResultsForSearch: -1
                }).on("change", function () {
                    datatable.students.reload();
                })
            }
        },
        init: function () {
            select.term.init();
            select.faculty.init();
            select.career.init();
            select.try.init();
        }
    };

    return {
        init: function () {
            select.init();
        }
    };
}();

$(function () {
    initApp.init();
});