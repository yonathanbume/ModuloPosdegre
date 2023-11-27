var index = function () {

    var datatable = {
        request: {
            object: null,
            options: {
                ajax: {
                    url: `/alumno/correccion-notas/get`,
                    type: "GET",
                    data: function (data) {
                        data.termId = $("#select_term").val();
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
                        data: "section",
                        title: "Sección"
                    },
                    {
                        data: "evaluation",
                        title: "Evaluación"
                    },
                    {
                        data: "newGrade",
                        title: "Nueva nota",
                        render: function (row) {
                            var tpm = "Sin asignar";
                            if (row != null) {
                                tpm = row;
                            }

                            return tpm;
                        }
                    },
                    {
                        data: "state",
                        title: "Estado",
                        render: function (row) {
                            var tpm = "";
                            switch (row) {
                                case 1:
                                    tpm = `<span class="m-badge m-badge--metal m-badge--wide">Pendiente</span>`;
                                    break;
                                case 2:
                                    tpm = `<span class="m-badge m-badge--primary m-badge--wide">Aprobado</span>`;
                                    break;
                                case 3:
                                    tpm = `<span class="m-badge m-badge--danger m-badge--wide">Rechazado</span>`;
                                    break;
                                case 4:
                                    tpm = `<span class="m-badge m-badge--warning m-badge--wide">Solicitado</span>`;
                                    break;
                            }

                            return tpm;
                        }
                    },
                    {
                        data: null,
                        title: "Opciones",
                        render: function (row) {
                            var tpm = `<button data-object='${row.proto().encode()}' title='Detalles' class="btn-details btn btn-primary m-btn m-btn--icon btn-sm m-btn--icon-only"><i class="la la-eye"></i></a>`;
                            return tpm;
                        }
                    }
                ]
            },
            events: {
                onDetails: function () {
                    $("#data-table").on("click", ".btn-details",function () {
                        var data = $(this).data("object").proto().decode();

                        $("#request_grade_details_modal").find("[name='CourseFullName']").val(`${data.courseCode}-${data.courseName}`);
                        $("#request_grade_details_modal").find("[name='Section']").val(data.section);
                        $("#request_grade_details_modal").find("[name='Evaluation']").val(data.evaluation);
                        $("#request_grade_details_modal").find("[name='OldGrade']").val(data.oldGrade ?? "Sin asignar");
                        $("#request_grade_details_modal").find("[name='NewGrade']").val(data.newGrade ?? "Sin asignar");
                        $("#request_grade_details_modal").find("[name='Observations']").val(data.observations);
                        $("#request_grade_details_modal").find("[name='Status']").val(data.statusStr);

                        if (data.filePath != null && data.filePath != "") {
                            $(".grade_correction_file_container").removeClass("d-none");
                            $(".grade_correction_file_path").attr("href", `/documentos/${data.filePath}`);
                        }
                        else {
                            $(".grade_correction_file_container").addClass("d-none");
                            $(".grade_correction_file_path").attr("href", "");
                        }

                        $("#request_grade_details_modal").modal("show");
                    })
                },
                init: function () {
                    this.onDetails();
                }
            },
            reload: function () {
                if (datatable.request.object == null) {
                    datatable.request.object = $("#data-table").DataTable(datatable.request.options);
                } else {
                    datatable.request.object.ajax.reload();
                }
            },
            init: function () {
                datatable.request.events.init();
            }
        },
        init: function () {
            datatable.request.init();
        }
    }

    var modal = {
        request: {
            object: $("#request_grade_correction_modal"),
            form: {
                object: $("#request_grade_correction_form").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        var formData = new FormData(formElement);

                        $("#request_grade_correction_form").find(":input").attr("disabled", true);

                        $.ajax({
                            url: `/alumno/correccion-notas/solicitar`,
                            type: "POST",
                            processData: false,
                            contentType: false,
                            data: formData,
                        })
                            .done(function (e) {
                                datatable.request.reload();
                                modal.request.object.modal("hide");
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "Solicitud registrada satisfactoriamente.",
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
                            })
                            .always(function () {
                                $("#request_grade_correction_form").find(":input").attr("disabled", false);
                            })
                    }
                })
            },
            events: {
                onHidden: function () {
                    modal.request.object.on('hidden.bs.modal', function (e) {
                        modal.request.form.object.resetForm();
                        modal.request.object.find(".custom-file-label").text("Seleccione un archivo");
                    })
                },
                init: function () {
                    modal.request.events.onHidden();
                }
            },
            init: function () {
                modal.request.events.init();
            }
        },
        init: function () {
            modal.request.init();
        }
    }

    var select = {
        term: {
            load: function () {
                $.ajax({
                    url: `/periodos/get`,
                    type : "GET"
                })
                    .done(function (e) {
                        $("#select_term").select2({
                            data : e.items
                        })

                        $("#select_term").on("change", function () {
                            datatable.request.reload();
                        })

                        $("#select_term").val(e.selected).trigger("change");
                    })
            },
            init: function () {
                select.term.load();
            }
        },
        courses: {
            load: function () {
                $.ajax({
                    url: `/cursos-matriculados-usuario`,
                    type : "GET"
                })
                    .done(function (e) {
                        modal.request.object.find("[name='StudentSectionId']").select2({
                            placeholder: "Seleccionar curso...",
                            data: e,
                            dropdownParent: modal.request.object
                        });

                        modal.request.object.find("[name='StudentSectionId']").val(null).trigger("change");

                        modal.request.object.find("[name='StudentSectionId']").on("change", function () {
                            select.grades.load($(this).val());
                        })
                    })
            },
            init: function () {
                select.courses.load();
            }
        },
        grades: {
            load: function (studentSectionId) {
                modal.request.object.find("[name='GradeId']").empty();
                modal.request.object.find("[name='GradeId']").attr("disabled", true);


                if (studentSectionId == null) {
                    modal.request.object.find("[name='GradeId']").select2({
                        placeholder: "Seleccionar nota...",
                        disabled : true
                    });
                } else {

                    mApp.block(modal.request.object, {
                        message: "Cargando evaluaciones..."
                    })

                    $.ajax({
                        url: `/alumno/correccion-notas/get-notas?studentSectionId=${studentSectionId}`,
                        type: "GET"
                    })
                        .done(function (e) {
                            modal.request.object.find("[name='GradeId']").select2({
                                placeholder: "Seleccionar nota...",
                                data: e,
                                disabled: false,
                                dropdownParent: modal.request.object
                            });

                            modal.request.object.find("[name='GradeId']").val(null).trigger("change");


                            mApp.unblock(modal.request.object);
                        })
                }
            },
            init: function () {
                select.grades.load();
            }
        },
        init: function () {
            select.term.init();
            select.grades.init();
            select.courses.init();
        }
    }

    return {
        init: function () {
            modal.init();
            datatable.init();
            select.init();
        }
    }
}();

$(() => {
    index.init();
});