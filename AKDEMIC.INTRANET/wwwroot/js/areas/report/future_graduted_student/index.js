var index = function () {

    var datatable = {
        graduated: {
            object: null,
            options: {
                ajax: {
                    url: `/reporte/estudiantes-por-egresar/get`,
                    type: "GET",
                    data: function (data) {
                        data.careerId = $("#career_select").val();
                        data.curriculumId = $("#curriculum_select").val();
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        data: "userName",
                        title: "Usuario"
                    },
                    {
                        data: "fullName",
                        title: "Estudiante"
                    },
                    {
                        data: "career",
                        title: "Escuela profesional"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        render: function (row) {
                            var tpm = "";
                            tpm += `<button data-id="${row.id}" class="ml-1 btn btn-info btn-sm m-btn m-btn--icon btn-history"><span><i class="la la-list-ul"></i><span>Historial</span></span></button>`;
                            return tpm;
                        }
                    }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Excel',
                        action: function (e, dt, node, config) {
                            var url = `/reporte/estudiantes-por-egresar/get-excel?careerId=${$("#career_select").val()}&curriculumId=${$("#curriculum_select").val()}`
                            window.open(url, "_blank");
                        }
                    }
                ]
            },
            events: {
                onHistory: function () {
                    $("#data-table").on("click", ".btn-history", function () {
                        var id = $(this).data("id");
                        modal.history.show(id);
                    });
                },
                init: function () {
                    this.onHistory();
                }
            },
            reload: function () {
                datatable.graduated.object.ajax.reload();
            },
            init: function () {
                datatable.graduated.object = $("#data-table").DataTable(datatable.graduated.options);
                datatable.graduated.events.init();
            }
        },
        history: {
            object: null,
            options: {
                pageLength: 20,
                responsive: true,
                processing: true,
                serverSide: false,
                filter: true,
                lengthChange: false,
                paging: false,
                ordering: false,
                orderMulti: false,
                columnDefs: [
                    { "orderable": false, "targets": [] }
                ],
                ajax: {
                    url: "/reporte/estudiantes-por-egresar/historial-academico/alumno",
                    type: "GET",
                    data: function (data) {
                        data.studentId = $("#StudentId").val();
                    }
                },
                columns: [
                    {
                        data: "year",
                        title: "Ciclo"
                    },
                    {
                        data: "code",
                        title: "Código"
                    },
                    {
                        data: "course",
                        title: "Curso"
                    },
                    {
                        data: "credits",
                        searchable: false,
                        title: "Créditos"
                    },
                    {
                        title: "N° veces",
                        searchable: false,
                        data: null,
                        render: function (data) {
                            return data.tries === 0 ? " - " : data.tries;
                        }
                    },
                    {
                        title: "Nota",
                        searchable: false,
                        data: null,
                        render: function (data) {
                            if (data.validated) {
                                if (data.grade <= 0)
                                    return `<a href="#" data-id="${data.id}" class="show-equiv"><span class="m--font-info"> CE </span></a>`;
                                else
                                    return `<a href="#" data-id="${data.id}" class="show-equiv"><span class="m--font-info"> ${data.grade} </span></a>`;
                            }

                            return data.tries === 0 ? " - " : (data.grade < 11 ? `<span style="color: red;">${data.grade}</span>` : data.grade);
                        }
                    },
                    {
                        title: "Semestre",
                        searchable: false,
                        data: null,
                        render: function (data) {
                            return data.tries === 0 ? " - " : data.term;
                        }
                    },
                    {
                        title: "Estado",
                        searchable: false,
                        data: "status"
                    }
                ]
            },
            reload: function () {
                if (datatable.history.object === null) {
                    datatable.history.object = $("#student_history").DataTable(datatable.history.options);
                } else {
                    datatable.history.object.ajax.reload();
                }
            },
            init: function () {
                $('#history-search').on('keyup', function () {
                    datatable.history.object.search(this.value).draw();
                });
            }
        },
        student_sections: {
            object: null,
            options: {
                ajax: {
                    url: `/reporte/estudiantes-por-egresar/cursos-matriculados/alumno`,
                    type: "GET",
                    data: function (data) {
                        data.studentId = $("#StudentId").val();
                    }
                },
                columns: [
                    {
                        data: "course",
                        title: "CUrso"
                    },
                    {
                        data: "section",
                        title: "Sección"
                    },
                    {
                        data: "credits",
                        title: "Credits"
                    },
                    {
                        data: "status",
                        title: "Estado",
                        render: function (row) {
                            if (row === 0) {
                                return 'En Curso'
                            }
                            else if (row === 1) {
                                return 'Aprobado'
                            }
                            else if (row === 2) {
                                return 'Desaprobado'
                            }
                            else if (row === 3) {
                                return 'Retirado'
                            }

                            return "-";
                        }
                    },
                ]
            },
            reload: function () {
                if (datatable.student_sections.object === null) {
                    datatable.student_sections.object = $("#student_sections_datatable").DataTable(datatable.student_sections.options);
                } else {
                    datatable.student_sections.object.ajax.reload();
                }
            }
        },
        init: function () {
            datatable.graduated.init();
            datatable.history.init();
            //datatable.student_sections.init();
        }
    }

    var modal = {
        history: {
            object: $("#modal_student_history"),
            show: function (studentId) {
                $("#StudentId").val(studentId);
                datatable.history.reload();
                datatable.student_sections.reload();
                events.getApprovedCredits(studentId);
                events.getEnrolledCredits(studentId);
                modal.history.object.modal("show");
            }
        }
    };

    var select = {
        careers: {
            load: function (facultyId) {
                $.ajax({
                    url: `/carreras/get`
                })
                    .done(function (e) {
                        $("#career_select").select2({
                            data: e.items,
                            disabled: false
                        });
                    });
            },
            onChange: function () {
                $("#career_select").on("change", function () {
                    var id = $(this).val();
                    select.curriculum.load(id);
                    datatable.graduated.reload();
                });
            },
            init: function () {
                this.load();
                this.onChange();
            }
        },
        curriculum: {
            load: function (careerId) {
                $("#curriculum_select").empty();
                if (careerId === null || careerId === _app.constants.guid.empty || careerId === "Todos") {
                    $("#curriculum_select").select2({
                        disabled: true,
                        placeholder: "Seleccionar plan de estudio"
                    });
                } else {
                    $.ajax({
                        url: `/carreras/${careerId}/planestudio/get`
                    })
                        .done(function (e) {
                            $("#curriculum_select").select2({
                                data: e.items,
                                disabled: false
                            }).trigger("change");
                        });
                }
            },
            onChange: function () {
                $("curriculum_select").on("change", function () {
                    datatable.graduated.reload();
                });
            },
            init: function () {
                $("#curriculum_select").select2({
                    disabled: true,
                    placeholder: "Seleccionar plan de estudio"
                });
                this.onChange();
            }
        },
        init: function () {
            select.careers.init();
            select.curriculum.init();
        }
    }


    var events = {
        getApprovedCredits: function (studentId) {
            $.ajax({
                url: "/reporte/estudiantes-por-egresar/creditos-aprobados/alumno?studentId=" + studentId,
                type: "GET"
            })
                .done(function (e) {
                    $("#credit_approved").text(e);
                });
        },
        getEnrolledCredits: function (studentId) {
            $.ajax({
                url: "/reporte/estudiantes-por-egresar/creditos-matriculados/alumno?studentId=" + studentId,
                type: "GET"
            })
                .done(function (e) {
                    $("#enrollment_credits").text(e);
                });
        },
        onSearch: function () {
            $("#search").doneTyping(function () {
                datatable.graduated.reload();
            });
        },
        init: function () {
            this.onSearch();
        }
    };

    return {
        init: function () {
            datatable.init();
            select.init();
        }
    }
}();

$(() => {
    index.init();
});