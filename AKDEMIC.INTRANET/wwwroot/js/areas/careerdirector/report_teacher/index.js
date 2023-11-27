var Teachers = function () {
    var datatable = {
        object: null,
        options: getSimpleDataTableConfiguration({
            url: ("/director-carrera/reporte_docentes/getTeachers").proto().parseURL(),
            data: function (data) {
                data.termId = $(".select2-terms").val();
                data.name = $("#search").val();
            },
            pageLength: 20,
            orderable: [],
            columns: [
                {
                    data: "fullName",
                    title: "Docente",
                },
                {
                    data: "courseName",
                    title: "Curso",
                },
                {
                    data: "section",
                    title: "Sección",
                }
            ]
        }),
        reloadTable: function () {
            datatable.object.ajax.reload();
        },
        search: {
            teacher: function () {
                var timing = 0;
                $("#search").keyup(function () {
                    clearTimeout(timing);
                    timing = setTimeout(function () {
                        datatable.reloadTable();
                    }, 500);
                });
            },
            init: function () {
                this.teacher();
            }
        },
        init: function () {
            this.search.init();
            datatable.object = $("#datatable_report").DataTable(datatable.options);
        }
    };

    var select = {
        terms: function () {
            $.ajax({
                url: ("/director-carrera/reporte_docentes/getTerms").proto().parseURL()
            }).done(function (result) {
                $(".select2-terms").empty();
                $(".select2-terms").select2({
                    data: result
                }).on("change", function () {
                    if (datatable.object === null) {
                        datatable.init();
                    } else {
                        datatable.reloadTable()
                    }
                }).trigger("change");
                var DefaultTermId = $("#DefaultTermId").val();
                $(".select2-terms").val(DefaultTermId);
                $(".select2-terms").trigger("change");
            });
        },
        init: function () {
            this.terms();
        }
    }

    return {
        init: function () {
            select.init();
        }
    }
}();

$(function () {
    Teachers.init();
});