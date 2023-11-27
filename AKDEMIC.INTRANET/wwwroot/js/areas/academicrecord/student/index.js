var StudentTable = function () {
    var studentDatatable = null;

    var options = {
        responsive: true,
        processing: true,
        serverSide: true,
        ajax: {
            url: "/registrosacademicos/alumnos/get".proto().parseURL(),
            data: function (d) {
                d.search = $("#search").val();
            }
        },
        columns: [
            {
                title: "Foto",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<img src="${row.picture || "/images/demo/user.png"}" width="50px" />`;
                    return tmp;
                }
            },
            { title: "Nombre y Apellidos", data: "user.fullName" },
            { title: "DNI", data: "user.dni" },
            { title: "Carrera", data: "career.name" },
            { title: "Usuario", data: "user.userName" },
            { title: "Teléfono", data: "user.phoneNumber" },
        ]
    };

    var events = {
        init: function () {
            $("#search").doneTyping(function () {
                studentDatatable.ajax.reload();
            });
        }
    };

    var select2 = {
        init: function () {
            this.faculties.init();
            this.careers.initEvents();
        },
        faculties: {
            init: function () {
                $.ajax({
                    url: "/facultades/registroacademico/get".proto().parseURL()
                }).done(function (result) {
                    $(".select2-faculties").select2({
                        data: result
                    }).on("change", function () {
                        select2.careers.init($(this).val());
                    }).trigger("change");
                });
            }
        },
        careers: {
            initEvents: function () {
                $(".select2-careers").on("change",
                    function () {
                        datatable.init($(".select2-faculties").val(), $(this).val());
                    });
            },
            init: function (facultyId) {
                $(".select2-careers").prop("disabled", true);
                $.ajax({
                    url: `/carreras/registroacademico/get?facultyId=${facultyId}`.proto().parseURL()
                }).done(function (result) {
                    $(".select2-careers").empty();
                    result.unshift({ id: "Todas", text: "Todas" });
                    $(".select2-careers").select2({
                        data: result,
                        placeholder: "Carrera"
                    });
                    if (result.length > 1) {
                        $(".select2-careers").prop("disabled", false);
                    }
                    $(".select2-careers").trigger("change");
                });
            }
        }
    };

    var datatable = {
        init: function (facultyId, careerId) {
            if (studentDatatable !== null) {
                studentDatatable.destroy();
                studentDatatable = null;
            }
            options.ajax.url = `/registrosacademicos/alumnos/get?fid=${facultyId}&cid=${careerId}`.proto().parseURL();
            studentDatatable = $(".students-datatable").DataTable(options);
            events.init();
        }
    };

    return {
        init: function () {
            select2.init();
        }
    };
}();

$(function () {
    StudentTable.init();
});