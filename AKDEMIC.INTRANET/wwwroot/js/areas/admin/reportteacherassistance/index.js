var InitApp = function () { 

    var CurrentTeacherId = null;

    var datatable = {
        teachers: {
            object: null,
            options: {
                ajax: {
                    url: `/admin/reporte_asistencia_docentes/get`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.academicDepartmentId = $("#select_academicDepartments").val();
                        data.search = $("#search").val();
                        data.termId = $("#select_Term").val();
                    },
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        data: 'academicDepartment',
                        title: 'Departamento Académico'
                    },
                    {
                        data: 'userName',
                        title: 'Usuario'
                    },
                    {
                        data: 'fullname',
                        title: 'Nombre'
                    },
                    {
                        data: "hours",
                        orderable: false,
                        title : "Horas Asignadas"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        orderable : false,
                        render: function (row) {
                            var url = `/admin/reporte_asistencia_docentes/${row.id}`;
                            return `<a href="${url}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail" <span><i class="la la-eye"> </i> </span>Ver Reporte </span></span></a>`;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
            },
        },
        missing: {
            object: null,
            options: {
                ajax: {
                    url: `/admin/reporte_asistencia_docentes/reporte-faltas-get`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.academicDepartmentId = $("#select_academicDepartments_2").val();
                        data.search = $("#search_2").val();
                        data.startDate = $("#startDate_2").val();
                        data.termId = $("#select_Term_2").val();
                        data.endDate = $("#endDate_2").val();
                    },
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        data: 'academicDepartment',
                        title: 'Departamento Académico'
                    },
                    {
                        data: 'username',
                        title: 'Usuario'
                    },
                    {
                        data: 'name',
                        title: 'Nombre'
                    },
                    {
                        data: null,
                        title: "Opciones",
                        orderable: false,
                        render: function (row) {
                            return `<button data-id='${row.id}' class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail" <span><i class="la la-eye"> </i> </span>Detalle</span></span></a>`;
                        }
                    }
                ],
            },
            events: {
                onDetail: function () {
                    $("#missing-table").on("click", ".btn-detail", function () {
                        var id = $(this).data("id");
                        modal.detailedMissig.show(id);
                    });
                },
                init: function () {
                    this.onDetail();
                }
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#missing-table").DataTable(this.options);
                this.events.init();
            }
        },
        detailedMissing: {
            object: null,
            options: {
                ajax: {
                    url: `/admin/reporte_asistencia_docentes/get-detalle-falta`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.endDate = $("#endDate_2").val();
                        data.teacherId = CurrentTeacherId;
                    }
                },
                pageLength: 10,
                orderable: [],
                columns: [
                    {
                        data: 'course',
                        title: 'Curso'
                    },
                    {
                        data: 'section',
                        title: 'Sección'
                    },
                    {
                        data: 'clases',
                        title :"Clases"
                    }
                ]
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#detailed_missing_table").DataTable(this.options);
            }
        },
        init: function () {
            this.teachers.init();
            this.missing.init();
            this.detailedMissing.init();
        }
    };

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.teachers.reload();
            });

            $("#search_2").doneTyping(function () {
                datatable.missing.reload();
            });
        }
    };

    var select = {
        init: function () {
            this.academicDepartment.init();
            this.term.init();
        },
        academicDepartment: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: `/departamentos-academicos/get`.proto().parseURL()
                }).done(function (data) {
                    $("#select_academicDepartments").empty();
                    $("#select_academicDepartments").html(`<option value="0" disabled selected>Seleccione una Opcion</option>`);
                    $("#select_academicDepartments").select2({
                        data: data
                    });

                    $("#select_academicDepartments_2").empty();
                    $("#select_academicDepartments_2").html(`<option value="0" disabled selected>Seleccione una Opcion</option>`);
                    $("#select_academicDepartments_2").select2({
                        data: data
                    });
                });
            },
            events: function () {
                $("#select_academicDepartments").on("change", function () {
                    datatable.teachers.reload();
                });

                $("#select_academicDepartments_2").on("change", function () {
                    datatable.missing.reload();
                });
            }
        },
        term: {
            load: function () {
                $.ajax({
                    url: `/periodos/get`,
                    type : "GET"
                })
                    .done(function (e) {
                        console.log(e);
                        $("#select_Term").select2({
                            data: e.items,
                            placeholder : "Seleccionar periodo..."
                        });

                        $("#select_Term_2").select2({
                            data: e.items,
                            placeholder: "Seleccionar periodo..."
                        });

                        $("#select_Term").val(e.selected).trigger("change");
                        $("#select_Term_2").val(e.selected).trigger("change");

                        $("#select_Term").on("change", function () {
                            datatable.teachers.reload();
                        })

                        $("#select_Term_2").on("change", function () {
                            datatable.missing.reload();
                        })
                    })
            },
            init: function () {
                select.term.load();
            }
        }
    };

    var datepicker = {
        endDate: function () {
            $("#startDate").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            });

            $("#endDate").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            });

            $("#endDate_2").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            }).on("change", function () {
                datatable.missing.reload();
            });
        },
        init: function () {
            this.endDate();
        }
    };

    var events = {
        onExportDetailed: function () {
            $("#btn_export").on("click", function () {
                var academicDepartmentId = $("#select_academicDepartments").val();
                var startdate = $("#startDate").val();
                var endDate = $("#endDate").val();
                var termId = $("#select_Term").val();

                if (termId == "" || termId == null || termId == undefined) {
                    toastr.info("Es necesario ingresar el periodo.", "Información");
                    return;
                }

                if (startdate === null || startdate === undefined || startdate === "") {
                    toastr.info("La fecha inicio es obligatoria", "Información");
                    return;
                }

                if (endDate === null || endDate === undefined || endDate === "") {
                    toastr.info("La fecha fin es obligatoria", "Información");
                    return;
                }

                var url = `/admin/reporte_asistencia_docentes/detallado-excel?academicDepartmentId=${academicDepartmentId}&termId=${termId}&startDate=${startdate}&endDate=${endDate}`;
                window.open(url, "_blank");
            });
        },
        onExportConsolidated: function () {
            $("#btn_export_consolidated").on("click", function () {
                var academicDepartmentId = $("#select_academicDepartments").val();
                var startdate = $("#startDate").val();
                var endDate = $("#endDate").val();
                var termId = $("#select_Term").val();

                if (termId == "" || termId == null || termId == undefined) {
                    toastr.info("Es necesario ingresar el periodo.", "Información");
                    return;
                }

                if (startdate === null || startdate === undefined || startdate === "") {
                    toastr.info("La fecha inicio es obligatoria", "Información");
                    return;
                }

                if (endDate === null || endDate === undefined || endDate === "") {
                    toastr.info("La fecha fin es obligatoria", "Información");
                    return;
                }

                var url = `/admin/reporte_asistencia_docentes/consolidado-excel?academicDepartmentId=${academicDepartmentId}&termId=${termId}&startDate=${startdate}&endDate=${endDate}`;
                window.open(url, "_blank");
            });
        },
        onMissing: function () {
            $("#btn_export_missing").on("click", function () {
                var academicDepartmentId = $("#select_academicDepartments_2").val();
                var endDate = $("#endDate_2").val();
                var termId = $("#select_Term_2").val();

                if (endDate === null || endDate === undefined || endDate === "") {
                    toastr.info("La fecha fin es obligatoria", "Información");
                    return;
                }

                if (termId == "" || termId == null || termId == undefined) {
                    toastr.info("Es necesario ingresar el periodo.", "Información");
                    return;
                }

                var url = `/admin/reporte_asistencia_docentes/reporte-faltas-excel?academicDepartmentId=${academicDepartmentId}&endDate=${endDate}&termId=${termId}`;
                window.open(url, "_blank");

            });
        },
        init: function () {
            this.onExportConsolidated();
            this.onExportDetailed();
            this.onMissing();
        }
    };

    var modal = {
        detailedMissig: {
            object: $("#detailed_missing_modal"),
            show: function (id) {
                CurrentTeacherId = id;
                this.object.modal("show");
                datatable.detailedMissing.reload();
            }
        }
    };
    

    return {
        init: function () {
            select.init();
            search.init();
            datatable.init();
            events.init();
            datepicker.init();
        }
    };
}();

$(function () {
    InitApp.init();
});