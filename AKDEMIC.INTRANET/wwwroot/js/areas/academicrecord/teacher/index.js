var teacher = function () {
    var datatable = {
        object: null,
        options: getSimpleDataTableConfiguration({
            url: "/registrosacademicos/docentes/get".proto().parseURL(),
            data: function (data) {
                data.search = $("#search").val();
                data.facultyId = $("#select_faculties").val();
            },
            pageLength: 20,
            columns: [
                {
                    title: "Usuario",
                    data: "user.userName"
                },
                {
                    title: "Nombre Completo",
                    data: "user.fullName"
                },
                {
                    title: "Facultad",
                    data: "career.faculty.name"
                },
                {
                    title: "Carrera",
                    data: "career.name"
                }
            ]
        }),
        reload: function () {
            datatable.object.ajax.reload();
        },
        init: function () {
            datatable.object = $("#teacher-datatable").DataTable(datatable.options);
        }
    };

    var events = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.reload();
            });
        }
    };

    var select2 = {
        faculty: {
            load: function () {
                $.ajax({
                    url: "/facultades/registroacademico/get"
                })
                    .done(function (e) {
                        $("#select_faculties").select2({
                            placeholder: "Seleccionar facultad..",
                            data: e
                        });
                    });
            },
            events: {
                onChange: function () {
                    $("#select_faculties").on("change", function () {
                        datatable.reload();
                    });
                },
                init: function () {
                    this.onChange();
                }
            },
            init: function () {
                this.load();
                this.events.init();
            }
        },
        init: function () {
            this.faculty.init();
        }
    };

    return {
        init: function () {
            datatable.init();
            events.init();
            select2.init();
        }
    };
}();

$(function () {
    teacher.init();
})