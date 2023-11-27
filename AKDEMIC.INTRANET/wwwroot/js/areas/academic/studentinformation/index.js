var InitApp = function () {

    var datatable = {
        students: {
            object: null,
            options: getSimpleDataTableConfiguration({
                dom: '<"top"i>rt<"bottom"flp><"clear">',
                url: "/academico/alumnos/get".proto().parseURL(),
                data: function (data) {
                    delete data.columns;

                    data.faculty = $("#faculty_select").val();
                    data.career = $("#career_select").val();
                    data.academicProgram = $("#academicProgram_select").val();
                    data.search = $("#search").val();
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Código",
                        data: "code"
                    },
                    {
                        title: "Estudiante",
                        data: "name"
                    },
                    {
                        title: "Facultad",
                        data: "faculty"
                    },
                    {
                        title: "Escuela",
                        data: "career"
                    },
                    {
                        title: "Especialidad",
                        data: "academicProgram"
                    },
                    {
                        title: "Detalle",
                        data: null,
                        render: function (data) {

                            var url = `/academico/alumnos/informacion/${data.id}`.proto().parseURL();

                            return `<div class="table-options-section">
                                <a href="${url}" class="btn btn-info btn-sm m-btn m-btn--icon m-btn--icon-only"><i class="la la-eye"></i></a>                            
                            </div>`;
                        }
                    }
                ]
            }),
            init: function () {
                datatable.students.object = $("#students_table").DataTable(datatable.students.options);
                $("#search").doneTyping(function () {
                    datatable.students.object.draw();
                });
            },
            reload: function () {
                datatable.students.object.draw();
            }
        }
    };

    var select = {
        faculty: {
            init: function () {
                $.ajax({
                    url: ("/facultades/v2/get").proto().parseURL()
                }).done(function (data) {

                    $("#faculty_select").select2({
                        data: data.items
                    });

                    select.faculty.events();
                });
            },
            events: function () {
                $("#faculty_select").on("change", function () {
                    var facultyId = $("#faculty_select").val();

                    if (facultyId === _app.constants.guid.empty) {
                        $("#career_select").select2({
                            placeholder: "Seleccione una Carrera",
                            disabled: true
                        });
                    } else {
                        select.career.load($("#faculty_select").val());
                    }
                    $("#career_select").val(_app.constants.guid.empty).trigger('change');
                    $("#academicProgram_select").empty();
                    $("#academicProgram_select").select2({
                        disabled: true,
                        placeholder: "Seleccione una Especialidad"
                    });

                    datatable.students.reload();
                });
            }
        },
        career: {
            init: function () {
                $("#career_select").select2({
                    placeholder: "Seleccione una escuela"
                });
                select.career.events();
            },
            load: function (faculty) {
                $("#career_select").empty();
                $.ajax({
                    url: `/academico/alumnos/facultades/${faculty}/carreras/v2/get`.proto().parseURL()
                }).done(function (data) {
                    $("#career_select").select2({
                        data: data.items,
                        disabled: false,
                        placeholder: "Seleccione una carrera",
                    });
                });
            },
            events: function () {
                $("#career_select").on("change", function () {
                    var careeId = $("#career_select").val();
                    if (careeId === _app.constants.guid.empty) {
                        $("#academicProgram_select").empty();
                        $("#academicProgram_select").select2({
                            disabled: true,
                            placeholder: "Seleccione una Especialidad"
                        });
                    } else {
                        select.academicProgram.load($("#career_select").val());
                    }
                    datatable.students.reload();
                });
            }
        },
        academicProgram: {
            init: function () {
                $("#academicProgram_select").select2({
                    placeholder: "Seleccione una Especialidad"
                });
                select.career.events();
            },
            load: function (careerId) {
                $.ajax({
                    url: `/academico/alumnos/especialidades/get/${careerId}`.proto().parseURL()
                }).done(function (data) {
                    $("#academicProgram_select").empty();
                    $("#academicProgram_select").select2({
                        placeholder: "Seleccione una Especialidad",
                        data: data.items,
                        disabled: false
                    });
                });
            },
            events: function () {
                $("#academicProgram_select").on("change", function () {
                    datatable.students.reload();
                });
            }
        },
        init: function () {
            select.faculty.init();
            select.academicProgram.init();
            select.career.init();
        }
    };

    return {
        init: function () {
            datatable.students.init();
            select.init();
        }
    };
}();

var Search = function () {

    var select = {
        search: {
            init: function () {
                $("#search").select2({
                    width: "100%",
                    placeholder: "Buscar...",
                    ajax: {
                        url: "/academico/alumnos/buscar".proto().parseURL(),
                        dataType: "json",
                        data: function (params) {
                            return {
                                term: params.term,
                                page: params.page
                            };
                        },
                        processResults: function (data, params) {
                            return {
                                results: data.items
                            };
                        },
                        cache: true
                    },
                    escapeMarkup: function (markup) {
                        return markup;
                    },
                    minimumInputLength: 3
                });

                $("#btn-search").on("click", function () {
                    select.search.load();
                });

                console.log("cargo");

                //$("#search").on("keypress", function (e) {

                //    console.log("entro");

                //    if (e.keyCode === 13) select.search.load();
                //});

                $('.select2-results__option').on('keydown', function (e) {
                    if (e.keyCode === 13) {
                        alert('Enter key');
                    }
                });
            },
            load: function () {
                var id = $("#search").val();
                if (id == "") {
                    swal("Seleccione un estudiante", "Debe buscar y seleccionar un estudiante primero", "warning");
                    return false;
                }
                window.location.href = `/academico/alumnos/informacion/${id}`.proto().parseURL();
            }
        }
    };

    return {
        init: function () {
            select.search.init();
        }
    };
}();


$(function () {
    //InitApp.init();

    Search.init();
});