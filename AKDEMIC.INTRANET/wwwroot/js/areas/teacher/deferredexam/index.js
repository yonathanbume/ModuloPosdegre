var index = function () {

    var datatable = {
        sections: {
            object: null,
            options: {
                ajax: {
                    url: `/profesor/examenes-aplazados/get-datatable`,
                    type: "GET",
                    data: function (data) {
                        data.search = $("#search").val();
                        data.termId = $(".select2-terms").val();
                    }
                },
                columns: [
                    {
                        data: "courseCode",
                        title : "Código"
                    },
                    {
                        data: "courseName",
                        title : "Curso"
                    },
                    {
                        data: "section",
                        title : "Sección"
                    },
                    {
                        data: "career",
                        title: "Escuela Profesional"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        data: function (row) {
                            var tpm = `<a href="/profesor/examenes-aplazados/${row.id}/estudiantes" class="btn btn-primary btn-sm m-btn  m-btn m-btn--icon"><span><i class="la la-edit"></i><span>Calificar</span></span></a>`;
                            return tpm;
                        }
                    }
                ]
            },
            reload: function () {
                if (datatable.sections.object == null) {
                    datatable.sections.object = $("#exams-datatable").DataTable(datatable.sections.options);
                } else {
                    datatable.sections.object.ajax.reload();
                }
            },
        }
    }

    var select = {
        term: {
            load: function () {
                $.ajax({
                    url: "/ultimos-periodos/get?yearDifference=5",
                    type: "GET"
                })
                    .done(function (e) {

                        $(".select2-terms").on("change", function () {
                            datatable.sections.reload();
                        })

                        $(".select2-terms").select2({
                            data: e.items,
                            placeholder: "Seleccionar periodo"
                        }).trigger("change");
                    })
            },
            init: function () {
                this.load();
            }
        },
        init: function () {
            select.term.init();
        }
    }

    return {
        init: function () {
            select.init();
        }
    }
}();

$(() => {
    index.init();
});