var InitApp = function () {
    var datatable = {
        teachers: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/director-carrera/reporte-asistencia-docentes/get`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.academicDepartmentId = $("#select_academicDepartments").val();
                        data.facultyId = $("#select_faculties").val();
                        data.search = $("#search").val();
                    },
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        data: 'username',
                        title: 'Usuario'
                    },
                    {
                        data: 'name',
                        title: 'Nombre'
                    },
                    {
                        data: 'academicDepartment',
                        title: 'Departamento Académico'
                    },
                    {
                        data: 'faculty',
                        title: 'Facultad'
                    },
                    {
                        data: 'email',
                        title: 'Correo electrónico'
                    },
                    {
                        data: 'phoneNumber',
                        title: 'Teléfono'
                    },
                    {
                        data: null,
                        title: "Opciones",
                        render: function (row) {
                            var url = `/director-carrera/reporte-asistencia-docentes/${row.id}`;
                            return `<a href="${url}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail" <span><i class="la la-eye"> </i> </span>Ver Reporte </span></span></a>`;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
            },
        },
        init: function () {
            this.teachers.init();
        }
    };

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.teachers.reload();
            });
        }
    }

    var select = {
        init: function () {
            this.faculty.init();
            this.academicDepartment.init();
        },
        faculty: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: `/facultades/get`.proto().parseURL()
                }).done(function (data) {
                    $("#select_faculties").select2({
                        data: data.items
                    });
                });
            },
            events: function () {
                $("#select_faculties").on("change", function () {
                    select.academicDepartment.load();
                    datatable.teachers.reload();
                });
            }

        },
        academicDepartment: {
            init: function () {
                $("#select_academicDepartments").select2();
                this.events();
            },
            load: function () {
                var facultyId = $("#select_faculties").val();
                $.ajax({
                    url: `/departamentos-academicos/get?facultyId=${facultyId}`.proto().parseURL()
                }).done(function (data) {
                    $("#select_academicDepartments").empty();
                    $("#select_academicDepartments").html(`<option value="0" disabled selected>Seleccione una Opcion</option>`);
                    $("#select_academicDepartments").select2({
                        data: data
                    });
                });
            },
            events: function () {
                $("#select_academicDepartments").on("change", function () {
                    datatable.teachers.reload();
                });
            }
        }
    };


    return {
        init: function () {
            select.init();
            search.init();
            datatable.init();
        }
    };
}();

$(function () {
    InitApp.init();
});