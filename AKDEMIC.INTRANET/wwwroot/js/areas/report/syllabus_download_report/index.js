var index = function () {

    var CurrentSectionId = null;

    var datatable = {
        sections: {
            object: null,
            options: {
                ajax: {
                    url: `/reporte/descarga-silabo/get-seciones-datatable`,
                    type: "GET",
                    data: function (data) {
                        data.termId = $("#term_select").val();
                        data.careerId = $("#career_select").val();
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        data: "courseCode",
                        title: "Código"
                    },
                    {
                        data: "courseName",
                        title: "Curso"
                    },
                    {
                        data: "code",
                        title: "Sección"
                    },
                    {
                        data: "enrolled",
                        title: "Cantidad matriculados"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        render: function (row) {
                            var tpm = "";
                            tpm += `<button type="button" data-id="${row.id}" class="btn btn-students btn-primary btn-sm m-btn  m-btn m-btn--icon"><span><i class="la la-eye"></i><span>Estudiantes</span></span></button>`;
                            return tpm;
                        }
                    }
                ]
            },
            events: {
                onView: function () {
                    $("#sections_datatable").on("click", ".btn-students", function () {
                        CurrentSectionId = $(this).data("id");
                        datatable.students.reload();
                        $("#modal_students").modal("show");
                    })
                },
                init: function () {
                    this.onView();
                }
            },
            reload: function () {
                if (this.object != null) {
                    datatable.sections.object.ajax.reload();
                } else {
                    datatable.sections.object = $("#sections_datatable").DataTable(datatable.sections.options);
                }
            },
            init: function () {
                this.events.init();
            }
        },
        students: {
            object: null,
            options: {
                ajax: {
                    url: `/reporte/descarga-silabo/get-datatable`,
                    type: "GET",
                    data: function (data) {
                        data.sectionId = CurrentSectionId;
                        data.search = $("#search_student").val();
                    }
                },
                pageLength: 10,
                columns: [
                    {
                        data: "userName",
                        title: "Usuario"
                    },
                    {
                        data: "fullName",
                        title: "Nombre Completo"
                    },
                    {
                        data: null,
                        title: "Estado",
                        render: function (row) {
                            var tpm = "";

                            if (row.syllabusDownloadDate != null) {
                                tpm += `<span class="m-badge m-badge--primary m-badge--wide">Descargado</span>`;
                            } else {
                                tpm += `<span class="m-badge m-badge--metal m-badge--wide">Pendiente</span>`;
                            }

                            return tpm;
                        }
                    },
                    {
                        data: null,
                        title: "Fecha Descarga",
                        render: function (row) {
                            var tpm = "-";

                            if (row.syllabusDownloadDate != null) {
                                tpm = row.syllabusDownloadDate;
                            } 

                            return tpm;
                        }
                    },
                ]
            },
            reload: function () {
                if (this.object != null) {
                    datatable.students.object.ajax.reload();
                } else {
                    datatable.students.object = $("#students_datatable").DataTable(datatable.students.options);
                }
            }
        },
        init: function () {
            datatable.sections.init();
        }
    }

    var select = {
        term: {
            init: function () {
                $.ajax({
                    url: `/periodos/get`
                })
                    .done(function (e) {
                        $("#term_select").select2({
                            data: e.items,
                        });


                        $("#term_select").on("change", function () {
                            datatable.sections.reload();
                        })

                        $("#term_select").val(e.selected).trigger("change");
                    })
            }
        },
        career: {
            init: function () {
                $.ajax({
                    url: `/carreras/get`
                })
                    .done(function (e) {
                        $("#career_select").select2({
                            data: e.items,
                            allowClear: true,
                            placeholder: "Seleccionar escuela profesinal"
                        });

                        $("#career_select").val(null).trigger("change");

                        $("#career_select").on("change", function () {
                            datatable.sections.reload();
                        })
                    })
            }
        },
        init: function () {
            select.career.init();
            select.term.init();
        }
    }

    var events = {
        onSearch: function () {
            $("#search").doneTyping(function () {
                datatable.sections.reload();
            })

            $("#search_student").doneTyping(function () {
                datatable.students.reload();
            })
        },
        init: function () {
            this.onSearch();
        }
    }

    return {
        init: function () {
            select.init();
            events.init();
            datatable.init();
        }
    }
}();

$(() => {
    index.init();
});