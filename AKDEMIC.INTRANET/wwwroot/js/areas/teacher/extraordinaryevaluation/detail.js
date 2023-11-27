var detail = function () {
    var extraordinaryEvaluationId = $("#Id").val();

    var pdfReport = {
        init: function () {
            $('#btn-download-pdf').on('click', function () {
                var $btn = $(this);
                $btn.addLoader();
                $.fileDownload(`/profesor/evaluacion-extraordinaria/${extraordinaryEvaluationId}/reporte-pdf`.proto().parseURL(), {
                    httpMethod: 'GET', successCallback: function (url) {
                        $btn.removeLoader();
                        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                    },
                    failCallback: function (responseHtml, url) {
                        toastr.error("No se pudo descargar el archivo", "Error");
                    }
                });
            });
        }
    };
    var hasPendingCalification = {
        init: function () {
            $.ajax({
                url: `/profesor/evaluacion-extraordinaria/${extraordinaryEvaluationId}/calificacion-pendiente/get`,
                type: "GET",
            })
            .done(function (data) {
                if (data) {
                    $("#btn-download-pdf").css("display", "none");
                } else {
                    $("#btn-download-pdf").css("display", "inline-block");
                }
            })
            .fail(function (e) {
            });
        }
    };
    var datatable = {
        students: {
            object: null,
            options: {
                ajax: {
                    url: "/profesor/evaluacion-extraordinaria/get-estudiantes",
                    data: function (data) {
                        delete data.columns;
                        data.extraordinaryEvalutionId = $("#Id").val()
                    },
                    method: 'GET',
                    dataSrc: ""
                },
                bPaginate: false,
                bLengthChange: false,
                bFilter: true,
                bInfo: false,
                columns: [
                    {
                        data: "userName",
                        title: "Cod. Estudiante"
                    },
                    {
                        data: "fullName",
                        title: "Estudiante"
                    },
                    {
                        data: null,
                        title: "Nota",
                        render: function (row) {
                            if (row.isPending) {
                                return "-";
                            }
                            else {
                                return row.grade;
                            }
                        }
                    },
                    {
                        data: null,
                        title: "Opciones",
                        render: function (row) {
                            var tpm = "-";
                            if (row.isPending) {
                                tpm = `<button type='button' data-id="${row.id}" class="btn btn-info btn-sm m-btn m-btn--icon btn-grade"><span><i class="la la-edit"></i><span> Calificar </span></span></button>`;
                            }
                            //else {
                            //    tpm = `<button type='button' data-id="${row.id}" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit-grade"><span><i class="la la-edit"></i><span> Editar </span></span></button>`;
                            //}
                            return tpm;
                        }
                    }

                ]
            },
            reload: function () {
                this.object.ajax.reload();
                hasPendingCalification.init();
            },
            events: {
                qualify: function () {
                    $("#data-table").on("click", ".btn-grade", function () {
                        var id = $(this).data("id");
                        modal.grade.add.show(id);
                    });
                },
                //edit: function () {
                //    $("#data-table").on("click", ".btn-edit-grade", function () {
                //        var id = $(this).data("id");
                //        modal.grade.edit.show(id);
                //    });
                //},
                init: function () {
                    this.qualify();
                    /*this.edit();*/
                }
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
                this.events.init();
            }
        },
        init: function () {
            datatable.students.init();
        }
    };

    var modal = {
        grade: {
            object: $("#grade-modal"),
            form: {
                object: $("#grade-form").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        var fomData = new FormData(formElement);
                        $("#grade-form").find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: fomData,
                            contentType: false,
                            processData: false
                        })
                            .done(function (e) {
                                datatable.students.reload();
                                modal.grade.object.modal("hide");
                                swal({
                                    type: "success",
                                    title: "Hecho!",
                                    text: "Nota asiganda satisfactoriamente.",
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
                                $("#grade-form").find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                            });
                    }
                })
            },
            add: {
                show: function (id) {
                    var url = "/profesor/evaluacion-extraordinaria/calificar";
                    $("#grade-form").attr("action", url);
                    modal.grade.object.find("[name='Id']").val(id);
                    modal.grade.object.modal("show");
                }
            },
            //edit: {
            //    show: function (id) {
            //        var url = "/profesor/evaluacion-extraordinaria/editar-calificacion";
            //        $("#grade-form").attr("action", url);
            //        modal.grade.object.find("[name='Id']").val(id);
            //        modal.grade.object.find(":input").attr("disabled",true);
            //        modal.grade.object.modal("show");

            //        $.ajax({
            //            url: `/profesor/evaluacion-extraordinaria/get-evaluacion-estudiante?extraordinaryEvaluationStudentId=${id}`,
            //            type: "GET"
            //        })
            //            .done(function (e) {
            //                console.log(e);
            //                modal.grade.object.find("[name='Observations']").val(e.observations);
            //                modal.grade.object.find("[name='Grade']").val(e.grade);
            //                modal.grade.object.find(":input").attr("disabled",false);

            //            });
            //    }
            //},
            events: {
                onHidden: function () {
                    modal.grade.object.on('hidden.bs.modal', function (e) {
                        modal.grade.form.object.resetForm();
                    });
                },
                init: function () {
                    this.onHidden();
                }
            },
            init: function () {
                this.events.init();
            }
        },
        init: function () {
            this.grade.init();
        }
    };

    return {
        init: function () {
            datatable.init();
            modal.init();
            hasPendingCalification.init();
            pdfReport.init();
        }
    };
}();

$(() => {
    detail.init();
});