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
                    url: "/reporte/estudiantes-condicionados/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.termId = $("#term_select").val();
                        data.careerId = $("#career_select").val();
                        data.conditionType = $("#type_select").val();
                        data.search = $("#search").val();
                    }
                },
                order: [[1, "asc"]],
                columns: [
                    {
                        title: "Código",
                        data: "userName",
                        width: "50px"
                    },
                    {
                        title: "Nombre completo",
                        data: "fullName"
                    },
                    {
                        title: "Escuela Profesional",
                        data: "career"
                    },
                    {
                        title: "Curso",
                        data: "course"
                    },
                    {
                        title: "Nro de Intento",
                        data: "try",
                        width: "150px",
                        render: function (data) {
                            return data;
                        }
                    },
                    {
                        title: "Periodo",
                        orderable: false,
                        data: "term"
                    }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Excel',
                        action: function () {
                            var button = $(this)[0].node;
                            $(button).addClass("m-loader m-loader--right m-loader--primary").attr("disabled", true);
                            $.fileDownload(`/reporte/estudiantes-condicionados/reporte-excel`.proto().parseURL(),
                                {
                                    httpMethod: 'GET',
                                    data: {
                                        termId: $("#term_select").val(),
                                        careerId: $("#career_select").val(),
                                        conditionType: $("#type_select").val(),
                                        search: $("#search").val()
                                    },
                                    successCallback: function () {
                                        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                    }
                                }
                            ).done(function () {
                            })
                                .fail(function () {
                                    toastr.error("Error al descargar el archivo", "Error");
                                })
                                .always(function () {
                                    $(button).removeClass("m-loader m-loader--right m-loader--primary").attr("disabled", false);
                                });
                        }
                    },
                    {
                        extend: 'pdfHtml5',
                        title: 'Estudiantes desaprobados por Nro de intento',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
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
        type: {
            init: function () {
                $("#type_select").select2();
                $("#type_select").on("change", function () {
                    datatable.students.load();
                });
            }
        },
        init: function () {
            select.term.init();
            select.career.init();
            select.type.init();
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