var index = function () {

    var CourseTermId = $("#CourseTermId").val();

    var select = {
        sections : {
            load: function () {
                $.ajax({
                    url: `/admin/notas/limpiarnotas/curso-periodo/${CourseTermId}/get-secciones`,
                    type: "GET"
                })
                    .done(function (e) {
                        $("#section_select").select2({
                            placeholder: "Seleccionar sección",
                            data: e
                        }).trigger("change");
                    })
            },
            events: {
                onChange: function () {
                    $("#section_select").on("change", function () {
                        var id = $(this).val();
                        partialView.evaluations.load(id);
                    })
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
        init: function () {
            select.sections.init();
        }
    };

    var events = {
        cleanGrades: function () {
            $("#container_evaluations").on("click", ".btn-clean-grades", function () {
                var sectionId = $(this).data("sectionid");
                var evaluationId = $(this).data("evaluationid");
                var evaluation = $(this).data("evaluation");

                swal({
                    type: "warning",
                    text: `¿Seguro que desea mover las notas oficiales de la evaluación ${evaluation} a las notas temporales?`,
                    title: "Mover notas oficiales",
                    confirmButtonText: "Aceptar",
                    showCancelButton: true,
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise(() => {
                            $.ajax({
                                type: "POST",
                                url: `/admin/notas/limpiarnotas/seccion/${sectionId}/evaluacion/${evaluationId}`
                            })
                                .done(function (e) {
                                    partialView.evaluations.load(sectionId);
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: e,
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
                        });
                    }
                });
            })
        },
        init: function () {
            this.cleanGrades();
        }
    }

    var partialView = {
        evaluations: {
            load: function (sectionId) {
                $("#container_evaluations").html("");
                mApp.block("#container_evaluations", {
                    message: "Cargando datos..."
                });

                $.ajax({
                    url: `/admin/notas/get-evaluaciones-notas/${sectionId}`,
                    type: "GET",
                    dataType: "HTML"
                })
                    .done(function (e) {
                        mApp.unblock("#container_evaluations");
                        $("#container_evaluations").html(e);
                    });

            }
        }
    }

    return {
        init: function () {
            select.init();
            events.init();
        }
    }
}();

$(() => {
    index.init();
})