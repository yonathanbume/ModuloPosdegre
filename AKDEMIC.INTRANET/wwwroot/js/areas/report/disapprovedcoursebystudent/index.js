var initApp = function () {

    var datatable = {
        students: {
            object: null,
            options: {
                pageLength: 50,
                serverSide: false,
                ajax: {
                    url: "/reporte/cursos-desaparobados-estudiante/get",
                    type: "GET",
                    data: function (data) {
                        delete data.columns;

                        data.termId = $("#term_select").val();
                        data.facultyId = $("#faculty_select").val();
                        data.careerId = $("#career_select").val();
                        data.courses = $("#disapproved_select").val();
                    },
                    dataSrc: ""
                },
                columns: [
                    {
                        title: "Código",
                        data: "username"
                    },
                    {
                        title: "Nombre completo",
                        data: "fullname"
                    },
                    {
                        title: "Facultad",
                        data: "faculty"
                    },
                    {
                        title: "Escuela Profesional",
                        data: "career"
                    },
                    {
                        title: "Cantidad",
                        data: "count"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        orderable: false,
                        render: function (data) {
                            return `<button data-id="${data.studentId}" class="btn btn-info btn-sm m-btn m-btn--icon btn-courses" title="Cursos desaprobados"><span><i class="la la-list"></i><span>Ver cursos</span></span></button>`;
                        }
                    },
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Excel',
                        action: function () {
                            var button = $(this)[0].node;
                            $(button).addClass("m-loader m-loader--right m-loader--primary").attr("disabled", true);
                            $.fileDownload(`/reporte/cursos-desaparobados-estudiante/reporte-excel`.proto().parseURL(),
                                {
                                    httpMethod: 'GET',
                                    data: {
                                        termId: $("#term_select").val(),
                                        facultyId: $("#faculty_select").val(),
                                        careerId: $("#career_select").val(),
                                        courses: $("#disapproved_select").val()
                                    }
                                }
                            )
                                .done(function () {
                                    toastr.success("Archivo descargado satisfactoriamente", "Éxito");

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

                $("#data-table").on("click", ".btn-courses", function () {
                    console.log('entro');
                    var id = $(this).data("id");
                    datatable.courses.load(id);
                });
            }
        },
        courses: {
            object: null,
            options: {
                pageLength: 50,
                serverSide: false,
                ajax: {
                    url: "/reporte/cursos-desaparobados-estudiante/get-cursos",
                    type: "GET",
                    data: function (data) {
                        delete data.columns;

                        data.termId = $("#term_select").val();
                        data.studentId = $("#studentId").val();
                    },
                    dataSrc: ""
                },
                columns: [
                    {
                        title: "Código",
                        data: "code"
                    },
                    {
                        title: "Curso",
                        data: "name"
                    },
                    {
                        title: "Nota",
                        data: "grade"
                    },
                ]
            },
            load: function (studentId) {
                $('#studentId').val(studentId);

                if (this.object == null)
                    this.object = $("#courses-table").DataTable(this.options);
                else
                    this.object.ajax.reload();

                $("#courses_modal").modal("toggle");
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

                    $("#term_select").on("change", function (e) {
                        datatable.students.reload();
                    });

                    datatable.students.init();
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
                $("#disapproved_select").select2({
                    minimumResultsForSearch: -1
                })
                    .on("change", function () {
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