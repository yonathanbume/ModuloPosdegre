var index = function () {

    var datatable = {
        students: {
            object: null,
            options: {
                pageLength: 10,
                ajax: {
                    url: "/admin/estudiantes-egresados/get",
                    type: "GET",
                    data: function (data) {
                        data.facultyId = $("#faculty_select").val();
                        data.careerId = $("#career_select").val();
                        data.curriculumId = $("#curriculum_select").val();
                        data.searchValue = $("#search").val();
                        data.termId = $("#term_select").val();
                    }
                },
                columns: [
                    {
                        title: "Código",
                        data : "userName"
                    },
                    {
                        title: "Estudiante",
                        data : "fullName"
                    },
                    {
                        title: "Escuela",
                        data : "career"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        render: function (row) {
                            var tpm = "";
                            tpm += `<button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-change-status"><span><i class="la la-edit"></i><span>Cambiar Estado</span></span></button>`;
                            tpm += `<button data-id="${row.id}" class="ml-1 btn btn-info btn-sm m-btn m-btn--icon btn-history"><span><i class="la la-list-ul"></i><span>Historial</span></span></button>`;
                            return tpm;
                        }
                    }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [0, 1, 2 ]
                        }
                    },
                    {
                        extend: 'pdfHtml5',
                        exportOptions: {
                            columns: [0, 1, 2]
                        }
                    },
                ]
            },
            events: {
                onHistory: function () {
                    $("#data-table").on("click", ".btn-history", function () {
                        var id = $(this).data("id");
                        modal.history.show(id);
                    });
                },
                onChangeStatus: function () {
                    $("#data-table").on("click", ".btn-change-status", function () {
                        var id = $(this).data("id");

                        swal({
                            type: "warning",
                            title: "El estado del estudiante pasará a EGRESADO.",
                            text: "¿Seguro que desea hacer el cambio?.",
                            confirmButtonText: "Aceptar",
                            showCancelButton: true,
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise(() => {
                                    $.ajax({
                                        type: "POST",
                                        url: `/admin/estudiantes-egresados/cambiar-estado?studentId=${id}`
                                    })
                                        .done(function (data) {
                                            datatable.students.reload();
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: "El estado del estudiante ha sido actualizado con éxito.",
                                                confirmButtonText: "Aceptar"
                                            });
                                        })
                                        .fail(function (e) {
                                            swal({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                                confirmButtonText: "Aceptar",
                                                text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                            });
                                        });
                                });
                            }
                        });
                    });
                },
                init: function () {
                    this.onHistory();
                    this.onChangeStatus();
                }
            },
            reload: function () {
                if (datatable.students.object === null) {
                    datatable.students.object = $("#data-table").DataTable(datatable.students.options);
                } else {
                    datatable.students.object.ajax.reload();
                }
            },
            init: function () {
                datatable.students.events.init();
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
                    url: "/admin/estudiantes-egresados/historial-academico/alumno",
                    type : "GET",
                    data: function (data) {
                        data.studentId = $("#StudentId").val();
                    }
                },
                columns: [
                    {
                        data: "year",
                        title : "Ciclo"
                    },
                    {
                        data: "code",
                        title : "Código"
                    },
                    {
                        data: "course",
                        title : "Curso"
                    },
                    {
                        data: "credits",
                        searchable: false,
                        title : "Créditos"
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
        init: function () {
            datatable.students.init();
            datatable.history.init();
        }
    };

    var modal = {
        history: {
            object: $("#modal_student_history"),
            show: function (studentId) {
                $("#StudentId").val(studentId);
                datatable.history.reload();
                events.getApprovedCredits(studentId);
                modal.history.object.modal("show");
            }
        }
    };

    var events = {
        getApprovedCredits: function (studentId) {
            $.ajax({
                url: "/admin/estudiantes-egresados/creditos-aprobados/alumno?studentId=" + studentId,
                type: "GET"
            })
                .done(function (e) {
                    $("#credit_approved").text(e);
                });
        },
        onSearch: function () {
            $("#btn_search_student").click(function () {
                var facultyId = $("#faculty_select").val();
                var careerId = $("#career_select").val();
                var curriculumId = $("#curriculum_select").val();
                var termId = $("#term_select").val();

                if (termId === null) {
                    toastr.error("Es necesario seleccionar un periodo académico.", "Error!");
                    return;
                }

                if (facultyId === null) {
                    toastr.error("Es necesario seleccionar una facultad.", "Error!");
                    return;
                }

                if (careerId === null) {
                    toastr.error("Es necesario seleccionar una escuela.", "Error!");
                    return;
                }

                if (curriculumId === null) {
                    toastr.error("Es necesario seleccionar un plan de estudio.", "Error!");
                    return;
                }

                datatable.students.reload();
            });
        },
        init: function () {
            this.onSearch();
        }
    };

    var select = {
        academicYear: {
            load: function () {
                $("#academicyear-select").select2();
            },
            events: {
                onChange: function () {
                    $("#academicyear-select").on("change", function () {
                        var value = this.value;
                        console.log(value);
                        if (value == "-1") {
                            value = "";
                        }
                        datatable.history.object.column(0).search(value).draw();
                    });
                },
                init: function () {
                    this.onChange();
                }
            },
            init: function () {
                this.load();
                this.events.init();
            }
        },
        faculty: {
            load: function () {
                $.ajax({
                    url: "/facultades/get"
                })
                    .done(function (e) {
                        $("#faculty_select").select2({
                            data: e.items,
                            placeholder : "Seleccionar facultad..."
                        });
                        $("#faculty_select").val(null).trigger("change");

                    });
            },
            onChange: function () {
                $("#faculty_select").on("change", function () {
                    var id = $(this).val();
                    select.careers.load(id);
                });
            },
            init: function () {
                this.load();
                this.onChange();
            }
        },
        careers: {
            load: function (facultyId) {
                $("#career_select").empty();
                if (facultyId === null || facultyId === _app.constants.guid.empty) {
                    $("#career_select").select2({
                        disabled: true,
                        placeholder: "Seleccionar escuela"
                    });
                } else {
                    $.ajax({
                        url: `/facultades/${facultyId}/carreras/v2/get?hasAll=false`
                    })
                        .done(function (e) {
                            $("#career_select").select2({
                                data: e.items,
                                disabled: false
                            }).trigger("change");
                        });
                }
               
            },
            onChange: function() {
                $("#career_select").on("change", function () {
                    var id = $(this).val();
                    select.curriculum.load(id);
                });
            },
            init: function () {
                $("#career_select").select2({
                    disabled: true,
                    placeholder : "Seleccionar escuela"
                });
                this.onChange();
            }
        },
        curriculum: {
            load: function (careerId) {
                $("#curriculum_select").empty();
                if (careerId === null || careerId === _app.constants.guid.empty) {
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
            init: function () {
                $("#curriculum_select").select2({
                    disabled: true,
                    placeholder: "Seleccionar plan de estudio"
                });
            }
        },
        term: {
            load: function() {
                $.ajax({
                    url: `/periodos/get`
                })
                    .done(function (e) {
                        $("#term_select").select2({
                            data: e.items,
                            disabled: false
                        }).trigger("change");
                    });
            },
            init: function () {
                this.load();
            }
        },
        init: function () {
            this.academicYear.init();
            this.faculty.init();
            this.careers.init();
            this.curriculum.init();
            this.term.init();
        }
    };

    return {
        init: function () {
            datatable.init();
            select.init();
            events.init();
        }
    };
}();

$(() => {
    index.init();
});