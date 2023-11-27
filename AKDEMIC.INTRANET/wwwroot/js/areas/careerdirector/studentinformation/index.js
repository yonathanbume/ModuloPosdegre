var InitApp = function () {

    var datatable = {
        students: {
            object: null,
            options: getSimpleDataTableConfiguration({
                dom: '<"top"i>rt<"bottom"flp><"clear">',
                url: "/director-carrera/alumnos/get".proto().parseURL(),
                data: function (data) {
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
                        data: "user.userName"
                    },
                    {
                        title: "Estudiante",
                        data: "user.fullName"
                    },
                    {
                        title: "Facultad",
                        data: "career.faculty.name"
                    },
                    {
                        title: "Carrera",
                        data: "career.name"
                    },
                    {
                        title: "Especialidad",
                        data: "academicProgram.name"
                    },
                    {
                        title: "Detalle",
                        data: null,
                        render: function (data) {
                            var url = `/director-carrera/alumnos/informacion/${data.id}`.proto().parseURL();

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
                    datatable.students.object.ajax.reload();
                });
            },
            reload: function () {
                datatable.students.object.ajax.reload();
            }
        }
    };

    var select = {
        faculty: {
            init: function () {
                $.ajax({
                    url: ("/director-carrera/alumnos/facultades/get").proto().parseURL()
                }).done(function (data) {
                    $("#faculty_select").select2({
                        data: data.results
                    });

                    select.faculty.events();
                });
            },
            events: function () {
                $("#faculty_select").on("change", function () {
                    var facultyId = $("#faculty_select").val();
                    select.career.load(facultyId);
                    datatable.students.reload();
                });
            }
        },
        career: {
            init: function () {
                $("#career_select").select2({
                    placeholder: "Seleccione una Carrera"
                });
                select.career.events();
            },
            load: function (faculty) {
                $("#career_select").empty();
                $.ajax({
                    url: `/director-carrera/alumnos/carreras/get?facultyId=${faculty}`.proto().parseURL()
                }).done(function (data) {
                    $("#career_select").select2({
                        data: data.results,
                        disabled: false,
                        placeholder: "Seleccione una Carrera"
                    }).trigger("change");
                });
            },
            events: function () {
                $("#career_select").on("change", function () {
                    select.academicProgram.load($("#career_select").val());
                    datatable.students.reload();
                });
            }
        },
        academicProgram: {
            init: function () {
                $("#academicProgram_select").select2({
                    placeholder: "Seleccione una Especialidad"
                });
                select.academicProgram.events();
            },
            load: function (careerId) {
                $("#academicProgram_select").empty();
                $.ajax({
                    url: `/director-carrera/alumnos/especialidades/get?careerId=${careerId}`.proto().parseURL()
                }).done(function (data) {
                    data.results.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $("#academicProgram_select").select2({
                        placeholder: "Seleccione una Especialidad",
                        data: data.results,
                        disabled: false,
                        allowClear: true
                    });
                });
            },
            events: function () {
                $("#academicProgram_select").on("change", function () {
                    datatable.students.reload();
                });
            },
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
                        url: "/director-carrera/alumnos/buscar".proto().parseURL(),
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
                    var id = $("#search").val();
                    if (id == "") {
                        swal("Seleccione un estudiante", "Debe buscar y seleccionar un estudiante primero", "warning");
                        return false;
                    }
                    window.location.href = `/director-carrera/alumnos/informacion/${id}`.proto().parseURL();
                });
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