var detail = function () {

    var datatable = {
        students: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/evaluacion-extraordinaria/get-estudiantes-asignados",
                    type: "GET",
                    data: function (data) {
                        data.extraordinaryEvaluationId = $("#Id").val();
                    }
                },
                columns: [
                    {
                        title: "Código",
                        data : "userName"
                    },
                    {
                        title: "Estudiante",
                        data: "fullName"
                    },
                    {
                        title: "Carrera",
                        data : "career"
                    },
                    {
                        title: "Nota Obtenida",
                        data: null,
                        render: function (row) {
                            var tpm = "";
                            if (row.isPending) {
                                tpm = "Sin asignar";
                            } else {
                                tpm = row.grade;
                            }
                            return tpm;
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        render: function (row) {
                            var tpm = "";
                            tpm += `<button data-id=${row.id} class="btn btn-danger m-btn m-btn--icon btn-sm m-btn--icon-only btn-delete"><i class="la la-trash"></i></button>`;
                            return tpm;
                        }
                    }
                ]
            },
            events: {
                onDelete: function () {
                    $("#data-table").on("click", ".btn-delete", function () {
                        var id = $(this).data("id");
                        swal({
                            type: "warning",
                            title: "Removerá al estudiante de la evaluación.",
                            text: "¿Seguro que desea removerlo?.",
                            confirmButtonText: "Aceptar",
                            showCancelButton: true,
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise(() => {
                                    $.ajax({
                                        type: "POST",
                                        url: `/admin/evaluacion-extraordinaria/eliminar-estudiante?id=${id}`
                                    })
                                        .done(function (data) {
                                            datatable.students.reload();
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: "Estudiante eliminado con éxito.",
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
                    this.onDelete();
                }
            },
            reload: function () {
                datatable.students.object.ajax.reload();
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

    var form = {
        object: $("#add_student_form").validate({
            submitHandler: function (formElement, e) {
                e.preventDefault();
                var fomData = new FormData(formElement);
                $("#add_student_form").find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                $.ajax({
                    url: "/admin/evaluacion-extraordinaria/agregar-estudiante",
                    type: "POST",
                    data: fomData,
                    contentType: false,
                    processData: false
                })
                    .done(function (e) {
                        select.students.clear();
                        datatable.students.reload();
                        swal({
                            type: "success",
                            title: "Hecho!",
                            text: "Estudiante agregado satisfactoriamente.",
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
                        $("#add_student_form").find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                    });
            }
        })
    };

    var select = {
        students: {
            load: function () {
                $("#StudentId").select2({
                    ajax: {
                        delay: 300,
                        url: `/admin/evaluacion-extraordinaria/get-estudiantes-json`,
                        data: function (params) {
                            var query = {
                                searchValue: params.term,
                                page: params.page || 1,
                                courseId: $("#CourseId").val()
                            };

                            return query;
                        }
                    },
                    allowClear: true,
                    minimumInputLength: 2,
                    placeholder: "Seleccione estudiante",
                });
            },
            clear: function () {
                $("#StudentId").val(null).trigger("change");
            },
            init: function () {
                this.load();
            }
        },
        init: function () {
            this.students.init();
        }
    };

    return {
        init: function () {
            datatable.init();
            select.init();
        }
    };
}();

$(() => {
    detail.init();
});