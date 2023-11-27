var InitApp = function () {
    var datatable = {
        payments: {
            object: null,
            options: {
                ajax: {
                    url: "/alumno/tramites_pendientes/get".proto().parseURL(),
                    type: "GET",
                    data: function (data) {
                        data.search = $("#search").val();
                    }
                },
                order: [[2, "desc"]],
                columns: [
                    {
                        data: "concept",
                        title: "Concepto"
                    },
                    {
                        data: "issueDate",
                        title: "Fecha emisión",
                        width: 100
                    },
                    {
                        data: "totalamount",
                        title: "Monto Total",
                        width: 90,
                        className: "text-right",
                        render: function (data) {
                            return "S/. " + data.toFixed(2);
                        }
                    },
                    {
                        data: "status",
                        title: "Estado",
                        orderable: false,
                        width: 100,
                        className: "text-center",
                        render: function (data) {
                            switch (data) {
                                case 1: return "<span class='m-badge m-badge--warning m-badge--wide'>Pendiente</span>";
                                case 2: return "<span class='m-badge m-badge--success m-badge--wide'>Aprobado</span>";
                                default:
                                    return "<span class='m-badge m-badge--secundary m-badge--wide'>--</span>";
                            }
                        }
                    },
                    {
                        data: null,
                        title: "Opciones",
                        orderable: false,
                        render: function (row) {
                            if (row.status == 2) {
                                if (row.requestType == 1)
                                    return '<button data-paymentid="' + row.id + '" type="button" class="btn btn-sm m-btn m-btn--icon btn-warning coursewithdrawal-request"><span><i class="la la-times"></i><span> Retirar Curso </span></span></button>';
                                if (row.requestType == 2)
                                    return '<button data-paymentid="' + row.id + '" type="button" class="btn btn-sm m-btn m-btn--icon btn-danger academicyearwithdrawal-request"><span><i class="la la-times"></i><span> Retirar Ciclo </span></span></button>';
                                if (row.requestType == 3)
                                    return '<button data-paymentid="' + row.id + '" type="button" class="btn btn-sm m-btn m-btn--icon btn-success substitute-exam"><span><i class="la la-check"></i><span> Seleccionar Curso </span></span></button>';
                                return '---';
                            }
                            return '---';
                        }
                    }
                ]
            },
            events: {
                buttons: function () {
                    $("#data-table")
                        .on("click", ".coursewithdrawal-request", function () {
                            var paymentid = $(this).data("paymentid");
                            $("#paymentId").val(paymentid);
                            //iniciar select de cursos
                            select2.enrolledcourses.init();
                            //abrir modal
                            forms.coursewithdrawal.init(paymentid);
                            forms.coursewithdrawal.modalObject.modal("show");
                        })
                        .on("click", ".academicyearwithdrawal-request", function () {
                            var id = $(this).data("userid");
                            var paymentid = $(this).data("paymentid");
                            swal({
                                title: "¿Desea retirarse del ciclo?",
                                text: "Se retirará de todas sus asignaturas. Este proceso es irreversible.",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonText: "Si, retirar",
                                confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                                cancelButtonText: "Cancelar"
                            }).then(function (result) {
                                if (result.value) {
                                    $.ajax({
                                        url: `/alumno/tramites_pendientes/retiro-ciclo/${paymentid}`.proto().parseURL(),
                                        type: "POST",
                                    }).done(function () {
                                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                        datatable.payments.reload();
                                    }).fail(function (e) {
                                        if (e.responseText !== null && e.responseText !== "") {
                                            toastr.error(e.responseText, _app.constants.toastr.title.error);
                                        }
                                        else {
                                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                        }
                                    });
                                }
                            })
                        })
                        .on("click", ".substitute-exam", function () {
                            var id = $(this).data("userid");
                            var paymentid = $(this).data("paymentid");
                            $("#UserId").val(id);
                            $("#substitute-paymentId").val(paymentid);
                            //iniciar select de cursos
                            select2.enrolledcourses.init();
                            //abrir modal
                            forms.substituteexams.init(id, paymentid);
                            forms.substituteexam.modalObject.modal("show");
                        })
                },
                init: function () {
                    this.buttons();
                }
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
                this.events.init();
            },
            reload: function () {
                this.object.ajax.reload();
            }
        },
        init: function () {
            datatable.payments.init();
        }
    };
    var forms = {
        coursewithdrawal: {
            modalObject: $("#request-coursewithdrawal-modal"),
            object: null,
            clear: function () {
                this.object.resetForm();
                $("#enrolledCoursesSelect").val(null).trigger("change");
            },
            init: function (paymentid) {
                this.object = $("#coursewithdrawal-request-form").validate({
                    submitHandler: function (form, e) {
                        e.preventDefault();

                        if ($("#enrolledCoursesSelect").val() == null) {
                            $("#coursewithdrawal_msg_txt").html("Debe seleccionar un curso");
                            $("#coursewithdrawal-modal-form-alert").removeClass("m--hide").show();
                            return false;
                        }

                        swal({
                            title: "Retirar de asignatura",
                            text: "Se retirará de la asignatura seleccionada",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Si, retirar",
                            confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                            cancelButtonText: "Cancelar"
                        }).then(function (result) {
                            if (result.value) {
                                mApp.block("#coursewithdrawal-request-form");

                                $.ajax({
                                    url: `/alumno/tramites_pendientes/retiro-curso/${paymentid}`,//$(form).attr("action"),
                                    type: "POST",
                                    processData: false,
                                    contentType: false,
                                    data: $(form).serialize()
                                })
                                    .done(function () {
                                        $(".modal").modal("hide");
                                        $(".m-alert").addClass("m--hide");

                                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                        forms.coursewithdrawal.clear();
                                        datatable.payments.reload();
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#coursewithdrawal_msg_txt").html(error.responseText);
                                        else $("#coursewithdrawal_msg_txt").html(_app.constants.ajax.message.error);

                                        $("#coursewithdrawal-modal-form-alert").removeClass("m--hide").show();
                                    })
                                    .always(function () {
                                        mApp.unblock("#coursewithdrawal-request-form");
                                    });
                            }
                        });
                    }
                });
            }
        },
        substituteexam: {
            modalObject: $("#substitute-exam-modal"),
            object: null,
            clear: function () {
                this.object.resetForm();
                $("#coursesSelect").val(null).trigger("change");
            },
            init: function (id, paymentid) {
                this.object = $("#substitute-exam-form").validate({
                    submitHandler: function (form, e) {
                        e.preventDefault();

                        if ($("#coursesSelect").val() == null) {
                            $("#substitute_exam_msg_txt").html("Debe seleccionar un curso");
                            $("#substitute-exam-form-alert").removeClass("m--hide").show();
                            return false;
                        }

                        swal({
                            title: "Aprobar Exámen",
                            text: "Se solicitará el exámen sustitutorio de la asignatura seleccionada",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Si, solicitar",
                            confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                            cancelButtonText: "Cancelar"
                        }).then(function (result) {
                            if (result.value) {
                                mApp.block("#substitute-exam-form");

                                $.ajax({
                                    url: `/alumno/tramites_pendientes/examen-sustitutorio/${paymentid}`,//$(form).attr("action"),
                                    type: "POST",
                                    processData: false,
                                    contentType: false,
                                    data: $(form).serialize()
                                })
                                    .done(function () {
                                        $(".modal").modal("hide");
                                        $(".m-alert").addClass("m--hide");

                                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                        forms.substituteexam.clear();
                                        datatable.payments.reload();
                                    })
                                    .fail(function (error) {
                                        if (error.responseText !== null && error.responseText !== "") $("#substitute_exam_msg_txt").html(error.responseText);
                                        else $("#substitute_exam_msg_txt").html(_app.constants.ajax.message.error);

                                        $("#substitute-exam-form-alert").removeClass("m--hide").show();
                                    })
                                    .always(function () {
                                        mApp.unblock("#substitute-exam-form");
                                    });
                            }
                        });
                    }
                });
            }
        },
    };
    var select2 = {
        enrolledcourses: {
            init: function () {
                $.ajax({
                    url: `/alumno/tramites_pendientes/cursos-matriculados`.proto().parseURL()
                }).done(function (data) {
                    $("#enrolledCoursesSelect, #coursesSelect").select2({
                        data: data,
                        minimumResultsForSearch: -1
                    });
                });
            }
        },
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.payments.reload();
            });
        }
    };

    return {
        init: function () {
            datatable.init();
            search.init();
            forms.init();
        }
    };
}();

$(function () {
    InitApp.init();
});
