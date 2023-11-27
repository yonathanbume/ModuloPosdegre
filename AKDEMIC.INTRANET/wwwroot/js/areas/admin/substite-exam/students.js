var CourseTable = function () {
    var courseDatatable = null;
    var sectionId = $("#Id").val();
    var selectedStudents = [];
    //
    var lstAddes = [];
    var lstDeleteds = [];
    var lstAddCheckeds = [];

    var options = getSimpleDataTableConfiguration({
        url: `/admin/examen-sustitutorio/estudiantes/${sectionId}/getstudents`.proto().parseURL(),
        data: function (data) {
            delete data.columns;
            data.search = $("#search").val();
        },
        pageLength: 20,
        orderable: [],
        columns: [
            {
                data: "code",
                orderable: false
            },
            {
                data : null,
                orderable: false,
                render: function (row) {
                    var tpm = `<span>${row.name}</span>`;

                    if (row.hasGradeCorrection) {
                        tpm += `<span class="m--font-danger m--font-boldest">*Corrección de nota pendiente</span>`;
                    }

                    if (row.hasGradeRecoveryExam) {
                        tpm += `<span class="m--font-danger m--font-boldest">*Examen de recuperación de nota pendiente</span>`;
                    }

                    return tpm;
                }
            },
            {
                data: null,
                orderable: false,
                render: function (row) {
                    var tpm = row.score;
                    //if (!row.termIsActive) {
                    //    if (row.isChecked) {
                    //        tpm = `<button data-id="${row.id}" class="btn-qualify btn btn-brand btn-sm m-btn m-btn--icon btn-watch-detail" title="Ver Alumnos"><span><i class="la la-cog"></i><span>Calificar</span></span></button>`;
                    //    }
                    //} 

                    return tpm;
                }
            },
            {
                data: null,
                orderable: false,
                title: "Opciones",
                render: function (row) {
                    var tmp = "";
                    var checked = "";

                    if (row.isChecked) {
                        checked = 'checked="checked"';
                    } else {
                        if (lstAddCheckeds.length > 0) {
                            if (lstAddCheckeds.findIndex(x => x.studentId === row.id) === -1) {
                                checked = '';
                            } else {
                                checked = 'checked="checked"';
                            }
                        } else {
                            checked = '';
                        }
                    }

                    if (!row.evaluated) {
                        tmp += `<span class="m-switch">
                               <label>
                                   <input type="checkbox" ${checked} class="checkbox-status" data-studentId="${row.studentId}" name="cbkStatus">
                                   <span></span>
                               </label>
                           </span>`;
                    } else {
                        tmp += `-`;
                    }
                    return tmp;
                }
            }
        ]
    });

    var events = {
        init: function () {

            $(".students-datatable").on("change", "[name='cbkStatus']", function () {
                var obj = $(this);
                var value = $(this).is(":checked");
                var studentId = $(this).data("studentid");
                var formData = new FormData();
                formData.append("SectionId", sectionId);
                formData.append("StudentId", studentId);
                formData.append("Checked", value);

                $.ajax({
                    url: "/admin/examen-sustitutorio/actualizar-alumno-examen",
                    type: "POST",
                    data: formData,
                    processData: false,
                    contentType: false
                })
                    .done(function () {
                        courseDatatable.ajax.reload();
                        swal({
                            type: "success",
                            title: "Completado",
                            text: "Estado actualizado con éxito",
                            confirmButtonText: "Excelente"
                        });
                    })
                    .fail(function (e) {
                        var currentValue = $(".students-datatable").find("[name='cbkStatus']").is(":checked");
                        obj.prop("checked", !currentValue);
                        swal({
                            type: "error",
                            title: "Error",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            confirmButtonText: "Entendido",
                            text: e.responseText
                        });
                    })
            });

            $(".btn-update-students").click(function () {
                var $btn = $(this);
                $btn.addLoader();

                $.ajax({
                    url: `/admin/examen-sustitutorio/actualizar-alumnos?sectionId=${sectionId}`,
                    type: "POST"
                })
                    .done(function (e) {
                        swal({
                            type: "success",
                            title: "Completado",
                            text: "Estudiantes actualizados con éxito",
                            confirmButtonText: "Excelente"
                        });
                        datatable.reload();
                    })
                    .fail(function (e) {
                        swal({
                            type: "error",
                            title: "Error",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            confirmButtonText: "Entendido",
                            text: e.responseText
                        });
                    })
                    .always(function (e) {
                        $btn.removeLoader();
                    });
            });
        },
    };

    var datatable = {
        init: function () {
            //if (courseDatatable !== null && courseDatatable !== undefined) {
            //    courseDatatable.destroy();
            //    courseDatatable = null;
            //}
            courseDatatable = $(".students-datatable").DataTable(options);
        },
        reload: function () {
            courseDatatable.ajax.reload();
        }
    }

    var itext = {
        init: function () {
            datatable.init();

            $("#search").doneTyping(function () {
                if (datatable != null && datatable != undefined)
                    datatable.reload();
            });

            $("#btn-pdf").on("click", function () {

            });

            events.init();
        }
    }


    var modal = {
        saveGrade: {
            object: $("#modal_save_grade"),
            form: {
                object: $("#form_save_grade").validate({
                    submitHandler: function (form) {
                        $("#form_save_grade").find(":input").attr("disabled", true);
                        var formData = new FormData();
                        formData.append("Score", $("#modal_save_grade").find("[name='Score']").val());
                        formData.append("Id", $("#modal_save_grade").find("[name='Id']").val());

                        $.ajax({
                            url: "/admin/examen-sustitutorio/asignar-nota",
                            data: formData,
                            type: "POST",
                            contentType: false,
                            processData: false,
                            success: function () {
                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                courseDatatable.ajax.reload();
                                modal.saveGrade.object.modal("hide");
                            },
                            error: function (e) {
                                if (e.responseText != null && e.responseText != "") {
                                    toastr.error(e.responseText, _app.constants.toastr.title.error);
                                    return;
                                }
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            },
                            complete: function () {
                                $("#form_save_grade").find(":input").attr("disabled", false);
                            }
                        });
                    }
                })
            },
            show: function () {
                $(".students-datatable").on("click", ".btn-qualify", function () {
                    var id = $(this).data("id");
                    modal.saveGrade.object.find("[name='Id']").val(id);
                    modal.saveGrade.object.find("[name='Score']").val("");
                    modal.saveGrade.object.modal("show");
                });
            },
            init: function () {
                this.show();
            }
        },
        init: function () {
            this.saveGrade.init();
        }
    } 

    return {
        init: function () {
            itext.init();
            modal.init();
        }
    }
}();

$(function () {
    CourseTable.init();
});