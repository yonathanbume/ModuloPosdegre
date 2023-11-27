var InitApp = function () {

    var datatable = {
        students: {
            object: null,
            options: getSimpleDataTableConfiguration({
                url: "/academico/estudiantes-invictos/get".proto().parseURL(),
                data: function (data) {
                    delete data.columns;

                    data.faculty = $("#faculty_select").val();
                    data.career = $("#career_select").val();
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
                        title: "Nombre completo",
                        data: "name"
                    },
                    {
                        title: "Carrera",
                        data: "career"
                    },
                    {
                        title: "Facultad",
                        data: "faculty"
                    },
                    {
                        title: "Ciclo",
                        data: "academicYear"
                    }
                ]
            }),
            init: function () {
                datatable.students.object = $("#students_table").DataTable(datatable.students.options);


                $("#search").doneTyping(function () {
                    datatable.students.reload();
                });
            },
            reload: function () {
                this.object.ajax.reload();
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
                        data: data.items,
                        minimumResultsForSearch: -1
                    });

                    datatable.students.init();

                    select.faculty.events();
                });
            },
            events: function () {
                $("#faculty_select").on("change", function () {
                    var facultyId = $("#faculty_select").val();

                    if (facultyId === _app.constants.guid.empty) {
                        $("#career_select").empty();
                        $("#career_select").select2({
                            placeholder: "Seleccione una escuela",
                            disabled: true
                        });
                    } else {
                        select.career.load($("#faculty_select").val());
                    }

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
                $.ajax({
                    url: `/facultades/${faculty}/carreras/v2/get`.proto().parseURL()
                }).done(function (data) {
                    $("#career_select").empty();
                    $("#career_select").select2({
                        placeholder: "Seleccione una carrera",
                        data: data.items,
                        minimumResultsForSearch: -1,
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
        init: function () {
            this.faculty.init();
            this.career.init();
        }
    };
    
    return {
        init: function () {
            select.init();        }
    };
}();

$(function () {
    InitApp.init();
});