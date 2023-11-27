var InitApp = function () {

    var datatable = {
        students: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/matricula/reservas/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns,
                            data.search = "";
                        data.term = $("#term_select").val();
                        data.faculty = $("#faculty_select").val();
                        data.career = $("#career_select").val();
                    },
                },
                dom: 'Bfrtip',
                buttons: [
                    //'excel',
                    {
                        extend: 'excel',
                        title: 'Reservas de Matrícula',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6]
                        }
                    },
                    //'pdf'
                    {
                        extend: 'pdf',
                        title: 'Reservas de Matrícula',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6]
                        },
                        orientation: 'landscape'
                    }
                ],
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        data: "term",
                        title: "Periodo",
                        orderable: false
                    },
                    {
                        data: "code",
                        title: "Código",
                        orderable: false
                    },
                    {
                        data: "name",
                        title: "Estudiante",
                        orderable: false
                    },
                    {
                        data: "career",
                        title: "Escuela Profesional",
                        orderable: false
                    },
                    {
                        data: "faculty",
                        title: "Facultad",
                        orderable: false
                    },
                    {
                        data: "startDate",
                        title: "Inicio Reserva",
                        orderable: false
                    },
                    {
                        data: "endDate",
                        title: "Fin Reserva",
                        orderable: false
                    },
                    {
                        title: "Opciones",
                        orderable: false,
                        render: function (data, type, row, meta) {
                            var tmp = "";
                            tmp += `<button data-id= ${row.id} type="button" class="btn btn-sm m-btn m-btn--icon btn-download btn-success"><i class='la la-download'></i></button>`;
                            return tmp;
                        },

                    }
                ]
            },
            init: function () {
                this.object = $("#students_table").DataTable(this.options);
                datatable.events();
            },
            reload: function () {
                this.object.ajax.reload();
            }
        },
        events: function () {
            datatable.students.object.on("click", ".btn-download",
                function () {
                    var button = $(this);
                    var id = button.data("id");
                    button.addLoader();
                    $.fileDownload(`/admin/matricula/reservas/descargar-pdf/${id}`.proto().parseURL())
                        .done(function () {
                            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                        })
                        .fail(function (e) {
                            toastr.error("Ocurrió un error", "Error");
                        })
                        .always(function () {
                            button.removeLoader();
                        });

                });
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
                    url: ("/facultades/v2/get").proto().parseURL()
                }).done(function (data) {
                    $("#faculty_select").select2({
                        data: data.items,
                        minimumResultsForSearch: -1
                    });

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
                    placeholder: "Seleccione una facultad"
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
            select.term.init();
            select.faculty.init();
            select.career.init();
        }
    };




    return {
        init: function () {
            select.init();
        }
    };
}();

$(function () {
    InitApp.init();
});