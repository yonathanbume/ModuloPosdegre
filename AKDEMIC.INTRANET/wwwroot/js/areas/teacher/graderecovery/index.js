var index = function () {

    var datatable = {
        object: null,
        options: {
            ajax: {
                url: "/profesor/examenes-recuperacion-nota/get",
                data: function (data) {
                    data.status = $("#status").val();
                }
            },
            columns: [
                {
                    data: "career",
                    title: "Carrera"
                },
                {
                    data: "courseCode",
                    title: "Cod. Curso"
                },
                {
                    data: "courseName",
                    title: "Curso"
                },
                {
                    data: "section",
                    title: "Sección"
                },
                {
                    data: "academicYear",
                    title: "Ciclo"
                },
                {
                    data: null,
                    title: "Estado",
                    render: function (row) {
                        var tpm = "";
                        if (row.status === 1) {
                            tpm = '<span class="m-badge  m-badge--metal m-badge--wide">Pendiente</span>"';
                        } else if (row.status === 2) {
                            tpm = '<span class="m-badge  m-badge--success m-badge--wide">Confirmado</span>"';
                        } else if (row.status === 3) {
                            tpm = '<span class="m-badge  m-badge--primary m-badge--wide">Realizado</span>"';
                        }
                        return tpm;
                    }
                },
                {
                    data: null,
                    title: "Opciones",
                    render: function (row) {
                        var tpm = "";
                        tpm += `<a class="btn btn-primary m-btn btn-sm m-btn--icon m-btn--icon-only" href="/profesor/examenes-recuperacion-nota/detalles/${row.id}" title="Detalles"><i class="la la-eye"></i></a>`;
                        return tpm;
                    }
                }
            ]
        },
        reload: function () {
            this.object.ajax.reload();
        },
        init: function () {
            datatable.object = $("#exams-datatable").DataTable(this.options);
        }
    };

    var select = {
        status: function () {
            $("#status").select2({
                minimumResultsForSearch: -1
            });

            $("#status").on("change", function () {
                datatable.reload();
            });
        },
        init: function () {
            this.status();
        }
    };

    return {
        init: function () {
            select.init();
            datatable.init();
        }
    };
}();

$(() => {
    index.init();
});
