var ReportApp = function () {

    var datatable = {
        tableId: "#exams-datatable",
        currentObject: null,
        options: {
            destroy: true,
            ajax: {
                data: function (data, settings) {
                    data["termId"] = $(".select2-terms").val();
                    data["statusId"] = $(".select2-status").val();
                },
                url: `${location.href}/get`,
                type: "GET"
            },
            columns: [
                {
                    data: "courseName",
                    name: "courseName",
                    title: "Curso",
                    orderable: false
                },
                {
                    data: "sectionCode",
                    name: "sectionCode",
                    title: "Seccion",
                    orderable: false
                },
                {
                    data: "studentName",
                    name: "studentName",
                    title: "Estudiante",
                    orderable: false
                },
                {
                    data: "status",
                    name: "status",
                    title: "Estado",
                    orderable: false,
                    render: function (data, type, row, meta) {
                        var status = row.status;
                        if (status === 0) {
                            return `<span class="m-badge m-badge--info m-badge--wide">Registrado</span>`;
                        } else if (status === 1) {
                            return `<span class="m-badge m-badge--success m-badge--wide">Evaluado</span>`;
                        } else {
                            return `<span class="m-badge m-badge--danger m-badge--wide">No aplica</span>`;
                        }
                    }
                },
                {
                    data: "Actions",
                    orderable: false,
                    width: "250px"
                }
            ],
            columnDefs: [
                {
                    targets: -1,
                    className: 'text-center',
                    title: "Acciones",
                    orderable: false,
                    render: function (a, b, row) {
                        if (row.status === 0) {
                            return '<button data-id="' + row.id + '" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit"><span><i class="la la-edit"></i><span>Evaluar</span></span></button>';
                        }
                        else if (row.status === 1) {
                            return '<button data-id="' + row.id + '" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"><span><i class="la la-eye"></i><span>Detalle</span></span></button>';
                        }
                        else {
                            return '--'
                        }
                    }
                }
            ],
            drawCallback: function () {
                datatable.updating.fired = false;
            }
        },
        updating: {
            fired: false,
            run: function () {
                if (datatable.currentObject === null) {
                    datatable.initialize();
                    datatable.updating.fired = true;
                    return;
                }

                if (datatable.updating.fired) {
                    return;
                }

                datatable.updating.fired = true;
                datatable.currentObject.draw();
            }
        },
        initialize: function () {
            datatable.currentObject = $(datatable.tableId).DataTable(datatable.options);
        },
        events: function () {
            $(datatable.tableId).on("click", ".btn-edit", function () {
                var id = $(this).data("id");

                $.ajax({
                    url: `${location.href}/${id}/get`,
                    type: "get"
                })
                    .done(function (data) {
                        $("#cId").val(data.id);
                        $("#StudentName").text(data.studentName);
                        $("#CourseName").text(data.coursName);
                        $("#request-modal-create").modal("show");
                    });
            });

            $(datatable.tableId).on("click", ".btn-detail", function () {
                var id = $(this).data("id");
                $("#request_detail").modal("show");

                $.ajax({
                    url: `${location.href}/${id}/detalles`,
                    type: "get"
                })
                    .done(function (e) {
                        $("#request_detail").find(":input").attr("disabled", true);
                        $("#request_detail").find("[name='Student']").val(e.fullName);
                        $("#request_detail").find("[name='LastGraded']").val(e.prevFinalScore);
                        $("#request_detail").find("[name='FinalGrade']").val(e.teacherExamScore == null ? e.examScore : e.teacherExamScore);
                        $("#request_detail").find(":input").attr("disabled", false);
                    });
            });
        }
    };

    var select2 = {
        sections: {
            currentValue: _app.constants.guid.empty,
            events: function () {
                $(".select2-status").change(function () {
                    select2.sections.currentValue = $(this).val();
                    datatable.updating.run();
                });
            },
            initialize: function () {
                $(".select2-sections").select2();
                $(".select2-status").select2();
            },
            update: function () {
                $.ajax({
                    url: `${location.href}/cursos/${$(".select2-courses").val()}/secciones`,
                    type: "get"
                })
                    .done(function (data) {
                        $(".select2-sections").select2({
                            data: data.items
                        })
                            .prop("disabled", false);
                    });
            },
            run: function () {
                this.initialize();
                this.events();
            }
        },
        courses: {
            object: null,
            run: function () {
                if (select2.courses.object !== null) {
                    select2.courses.object.select2("destroy");
                }

                $.ajax({
                    url: `${location.href}/cursos`,
                    type: "get"
                }).done(function (data) {
                    select2.courses.object = $(".select2-courses").select2({
                        data: data
                    })
                        .prop("disabled", false)
                        .change(function () {
                            select2.sections.update();
                            datatable.updating.run();
                        });
                    datatable.updating.run();
                });
            }
        },
        terms: {
            load: function () {
                $.ajax({
                    url: "/ultimos-periodos/get?yearDifference=1",
                    type: "GET"
                })
                    .done(function (e) {
                        $(".select2-terms").select2({
                            data: e.items,
                            placeholder : "Seleccionar periodo"
                        })

                        $(".select2-terms").on("change", function () {
                            datatable.updating.run();
                        })
                    })
            },
            init: function () {
                this.load();
            }
        },
        run: function () {
            this.sections.run();
            this.courses.run();
            this.terms.init();
        }
    };

    var modalManagement = {
        createModal: {
            run: function () {
                var currentForm = $("#request-modal-create-form").validate({
                    rules: {
                        Score: {
                            required: true,
                            range: [0, 20]
                        }
                    },
                    submitHandler: function (form, e) {
                        e.preventDefault();
                swal({
                    title: '¿Está seguro?',
                    text: `La nota(${$(form).find("#cScore").val()}) será asignada.`,
                    type: 'info',
                    showCancelButton: true,
                    confirmButtonText: 'Sí, asignar',
                    confirmButtonClass: 'btn btn-danger m-btn m-btn--custom',
                    cancelButtonText: 'Cancelar',
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: $(form).attr("action"),
                                data: $(form).serialize(),
                                type: "POST",
                                success: function (result) {
                                    currentForm.resetForm();
                                    datatable.currentObject.draw();
                                    $("#request-modal-create").modal("hide");
                                    swal({
                                        type: 'success',
                                        title: 'Completado',
                                        text: 'La nota ha sido asignada',
                                        confirmButtonText: 'Excelente'
                                    });
                                },
                                error: function (errormessage) {
                                    console.log(errormessage);
                                    currentForm.resetForm();
                                    swal({
                                        type: 'error',
                                        title: 'Error',
                                        confirmButtonClass: 'btn btn-danger m-btn m-btn--custom',
                                        confirmButtonText: 'Entendido',
                                        text: errormessage.responseText
                                    });
                                }
                            });
                        })
                    }
                });


                    }
                });
            }
        },
        run: function () {
            this.createModal.run();
        }
    };

    return {
        run: function () {
            datatable.events();
            select2.run();
            modalManagement.run();
            //datatable.initialize();
            //reportManagement.run();
        }
    };
}();

$(function () {
    ReportApp.run();
});