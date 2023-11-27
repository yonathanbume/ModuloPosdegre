var InitApp = function () {
    var datatable = {
        students: {
            object: null,
            options: {
                columnDefs: [
                    { "orderable": false, "targets": [] }
                ],
                ajax: {
                    url: "/reporte/rendimiento-tutorados/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.termId = $("#term-select").val();
                        data.careerId = $("#career-select").val();
                        data.search = $("#search").val();
                        data.onlyDisapproved = $("#onlydisapproved-switch").is(":checked");

                    }
                },
                columns: [
                    {
                        //title: "Tutor",
                        data: "tutor"
                    },
                    {
                        //title: "Código",
                        data: "code",
                        width: "100px"
                    },
                    {
                        //title: "Estudiante",
                        data: "name",
                    },
                    {
                        //title: "Escuela Profesional",
                        data: "career"
                    },
                    {
                        //title: "Cred. Mat.",
                        data: "prevTotalCredits",
                        orderable: false,
                        className: "text-right"
                    },
                    {
                        //title: "Cred. Des.",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            $("#prev-term-title").html(`Periodo ${data.prevTermName}`);
                            return `<a href="javascript:void(0);" class="show-grades" data-student="${data.id}" data-term="${data.prevTerm}" style="color: cornflowerblue;text-decoration: underline;">${data.prevDisapprovedCredits}</a>`;
                        },
                        className: "text-right"
                    },
                    {
                        //title: "PPS Prev.",
                        data: "prevGrade",
                        orderable: false,
                        className: "text-right",
                        render: function (data) {
                            return data.toFixed(2);
                        }
                    },
                    {
                        //title: "Cred. Mat.",
                        data: "totalCredits",
                        orderable: false,
                        className: "text-right"
                    },
                    {
                        //title: "Cred. Des.",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            $("#term-title").html(`Periodo ${data.termName}`);
                            return `<a href="javascript:void(0);" class="show-grades" data-student="${data.id}" data-term="${data.term}" style="color: cornflowerblue;text-decoration: underline;">${data.disapprovedCredits}</a>`;
                        },
                        className: "text-right"
                    },
                    {
                        //title: "PPS",
                        data: null,
                        render: function (data) {
                            if (data.grade > data.prevGrade)
                                return `<i class="la la-angle-double-up m--font-success"></i> ${data.grade.toFixed(2)}`;
                            else if (data.grade == data.prevGrade)
                                return `<i class="la la-bars"></i> ${data.grade.toFixed(2)}`;
                            else
                                return `<i class="la la-angle-double-down m--font-danger"></i> ${data.grade.toFixed(2)}`;
                        },
                        orderable: false,
                        className: "text-right"
                    }               
                ]
            },
            init: function () {
                datatable.students.object = $("#data-table").DataTable(datatable.students.options);

                datatable.students.object.on("click", ".show-grades", function () {
                    console.log("entro");
                    var student = $(this).data("student");
                    var term = $(this).data("term");
                    datatable.courses.load(student, term);
                });
            },
            reload: function () {
                $("#prev-term-title").html(`Periodo`);
                $("#term-title").html(`Periodo`);

                datatable.students.object.ajax.reload();
            }
        },
        courses: {
            object: null,
            options: {
                serverSide: false,
                ajax: {
                    url: "/reporte/rendimiento-tutorados/cursos-desaprobados/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    dataSrc: ""
                },
                columns: [
                    {
                        title: "Curso",
                        data: "course"
                    },
                    {
                        title: "Créditos",
                        data: "credits",
                        width: "100px"
                    },
                    {
                        title: "Intento",
                        data: "try",
                    },
                    {
                        title: "Nota",
                        data: "finalGrade"
                    }                  
                ]
            },
            load: function (student, term) {
                $("#grades-modal").modal("toggle");
                var url = `/reporte/rendimiento-tutorados/cursos-desaprobados/get?studentId=${student}&termId=${term}`.proto().parseURL();

                var newoptions = this.options;
                newoptions.ajax.url = url;

                if (this.object != null) {
                    this.object.destroy();
                }

                this.object = $("#grades-table").DataTable(newoptions);
            }
        }
    };

    var select = {
        term: {
            init: function () {
                $.ajax({
                    url: "/periodos/get".proto().parseURL()
                }).done(function (data) {
                    $("#term-select").select2({
                        data: data.items
                    });

                    if (data.selected !== null) {
                        $("#term-select").val(data.selected);
                        $("#term-select").trigger("change.select2");
                    }

                    datatable.students.init();

                    $("#term-select").on("change", function (e) {
                        datatable.students.reload();
                    });
                });
            }
        },
        career: {
            init: function () {
                $.ajax({
                    url: `/carreras/get`.proto().parseURL()
                }).done(function (data) {
                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $("#career-select").select2({
                        placeholder: "Seleccione una Escuela",
                        data: data.items,
                        minimumResultsForSearch: 10
                    });

                    $("#career-select").on("change", function () {
                        datatable.students.reload();
                    });
                });
            }
        },
        init: function () {
            select.term.init();
            select.career.init();
        }
    };

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.students.reload();
            });

            $('#onlydisapproved-switch').change(function () {
                datatable.students.reload();
            });
        }
    };

    return {
        init: function () {
            select.init();
            search.init();
        }
    };
}();

jQuery(document).ready(function () {
    InitApp.init();
});