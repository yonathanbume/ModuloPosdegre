var StudentTable = function () {


    var datatable = {
        object: null,
        options: {
            responsive: true,
            processing: true,
            serverSide: true,
            ajax: {
                url: `/admin/alumnos/get`.proto().parseURL(),
                data: function (data) {
                    delete data.columns;

                    data.search = $("#search").val();
                    data.facultyId = $(".select2-faculties").val();
                    data.careerId = $(".select2-careers").val();
                    data.programId = $(".select2-programs").val();

                    data.curriculumId = $(".select2-curriculums").val();
                    data.campusId = $(".select2-campus").val();

                }
            },
            columns: [
                {
                    title: "Foto",
                    data: null,
                    orderable: false,
                    render: function (data, type, row) {
                        var tmp = "";
                        if (row.picture == null) {
                            tmp += `<img src="/images/demo/user.png" width="50px" />`;
                        } else {
                            tmp += `<img src="/imagenes/${row.picture}" width="50px" />`;
                        }
                        return tmp;
                    }
                },
                { title: "Nombre y Apellidos", data: "user.fullName" },
                { title: "DNI", data: "user.dni" },
                { title: "Carrera", data: "career.name" },
                { title: "Usuario", data: "user.userName" },
                { title: "Teléfono", data: "user.phoneNumber" },
                {
                    title: "Opciones",
                    data: null,
                    orderable: false,
                    render: function (data) {
                        var url = `/admin/alumnos/editar/${data.id}`;
                        return `<a href="${url}" class="btn btn-primary btn-sm m-btn m-btn--icon" title="Editar"><span><i class="la la-edit"></i><span>Editar</span></span></a> `;
                        //+ `<button data-id="${row.id}" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete" title="Eliminar"><i class="la la-trash"></i></button>`;
                    }
                }
            ]
        },
        init: function () {
            this.object = $(".students-datatable").DataTable(this.options);

            //$(".students-datatable").on("click", ".btn-delete", function () {
            //    var id = $(this).data("id");
            //    location.href = `/admin/alumnos/editar/${id}`;
            //});

        },
        reload: function () {
            this.object.ajax.reload();
        }
    };

    var select2 = {
        init: function () {
            this.faculties.init();
            this.careers.init();
            this.programs.init();
            this.curriculums.init();
            this.campus.init();
        },
        faculties: {
            init: function () {
                $.ajax({
                    url: "/facultades/get".proto().parseURL()
                }).done(function (result) {
                    $(".select2-faculties").select2({
                        data: result.items
                    });

                    $(".select2-faculties").on("change", function () {
                        select2.careers.reload($(this).val());
                        datatable.reload();
                    });
                });
            }
        },
        careers: {
            init: function () {
                $(".select2-careers").select2({
                    placeholder: "Seleccione una carrera"
                });

                $(".select2-careers").on("change", function () {
                    select2.programs.reload($(this).val());
                    select2.curriculums.reload();
                    datatable.reload();
                });
            },
            reload: function (facultyId) {
                $(".select2-careers").prop("disabled", true);
                $(".select2-careers").empty();

                $.ajax({
                    url: `/carreras/get?fid=${facultyId}`.proto().parseURL()
                }).done(function (result) {

                    result.items.unshift({ id: "Todas", text: "Todas" });

                    $(".select2-careers").select2({
                        data: result.items,
                        placeholder: "Seleccione una carrera"
                    });

                    if (result.items.length > 1) {
                        $(".select2-careers").prop("disabled", false);
                    }

                    //$(".select2-careers").trigger("change");
                });
            }
        },
        programs: {
            init: function () {
                $(".select2-programs").select2();

                $(".select2-programs").on("change", function () {
                    select2.curriculums.reload();
                    datatable.reload();
                });
            },
            reload: function (careerId) {
                $(".select2-programs").prop("disabled", true);
                $(".select2-programs").empty();

                $.ajax({
                    url: `/carreras/${careerId}/programas/get`.proto().parseURL()
                }).done(function (result) {


                    result.items.unshift({ id: "Todos", text: "Todos" });

                    $(".select2-programs").select2({
                        data: result.items,
                        placeholder: "Programas"
                    });

                    if (result.items.length > 1) {
                        $(".select2-programs").prop("disabled", false);
                    }
                    //$(".select2-programs").trigger("change");
                });
            }
        },
        curriculums: {
            init: function () {
                $(".select2-curriculums").select2();

                $(".select2-curriculums").on("change", function () {
                    datatable.reload();
                });
            },
            reload: function () {
                var careerId = $(".select2-careers").val();
                var academicProgramId = $(".select2-programs").val();
                $(".select2-curriculums").prop("disabled", true);
                $(".select2-curriculums").empty();

                if (careerId != "" && careerId != null && careerId != "Todas") {

                    $.ajax({
                        url: `/planes-estudio/${careerId}/get?academicProgramId=${academicProgramId}`
                    })
                        .done(function (result) {

                            result.items.unshift({ id: "Todos", text: "Todos" });

                            $(".select2-curriculums").select2({
                                data: result.items,
                                placeholder: "Seleccionar plan"
                            });
                            $(".select2-curriculums").prop("disabled", false);
                        })
                }
            }
        },
        campus: {
            init: function () {

                $.ajax({
                    url: `/sedes/get`
                })
                    .done(function (result) {

                        result.items.unshift({ id: "Todos", text: "Todos" });

                        $(".select2-campus").select2({
                            data: result.items,
                            placeholder: "Seleccionar plan"
                        });

                        $(".select2-campus").on("change", function () {
                            datatable.reload();
                        })
                    })
            }
        }
    };

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.reload();
            });
        }
    }

    return {
        init: function () {
            select2.init();
            search.init();
            datatable.init();
        }
    };
}();

$(function () {
    StudentTable.init();
});