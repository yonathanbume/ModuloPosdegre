var InitApp = function () {
    var datatable = {
        students: {
            object: null,
            options: {
                bInfo: true,
                columnDefs: [
                    { "orderable": false, "targets": [] }
                ],
                ajax: {
                    url: "/reporte/estudiantes-egresados/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.termId = $("#term_select").val();
                        data.careerId = $("#career_select").val();
                        data.search = $("#search").val();
                    }
                },
                order: [[1, "asc"]],
                columns: [
                    {
                        title: "Código",
                        data: "code",
                        width: "50px"
                    },
                    {
                        title: "Nombre completo",
                        data: "name"
                    },
                    {
                        title: "Escuela Profesional",
                        data: "career",
                        width: "250px"
                    },
                    {
                        title: "Per. Ingreso",
                        data: "admissionTerm",
                        width: "150px"
                    },
                    {
                        title: "Per. Egreso",
                        data: "graduationTerm",
                        width: "150px"
                    },
                    {
                        title: "Nota Prom.",
                        orderable: false,
                        data: "weightedAverageGrade"
                    }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'PDF',
                        action: function (e, dt, node, config) {
                            var termId = $("#term_select").val();
                            var careerId = $("#career_select").val();

                            var url = `/reporte/estudiantes-egresados/pdf?termId=${termId}&careerId=${careerId}`;
                            window.open(url, "_blank");
                        }
                    },
                    {
                        text: 'EXCEL',
                        action: function (e, dt, node, config) {
                            var termId = $("#term_select").val();
                            var careerId = $("#career_select").val();

                            var url = `/reporte/estudiantes-egresados/excel?termId=${termId}&careerId=${careerId}`
                            window.open(url, "_blank");
                        }
                    }
                ]
            },
            init: function () {
                this.object = $("#students_table").DataTable(this.options);
            },
            load: function () {
                this.object.ajax.reload();
            }
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
                        datatable.students.load();
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

                    $("#career_select").select2({
                        placeholder: "Seleccione una Escuela",
                        data: data.items,
                        minimumResultsForSearch: 10,
                        disabled: false
                    });

                    $("#career_select").on("change", function () {
                        datatable.students.load();
                    });
                });
            }
        },
        init: function () {
            select.term.init();
            select.career.init();
        }
    };

    var events = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.students.load();
            });
        }
    }

    return {
        init: function () {
            select.init();
            events.init();
        }
    };
}();

jQuery(document).ready(function () {
    InitApp.init();
});