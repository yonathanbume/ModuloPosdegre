var index = function () {

    var events = {
        validateGrades: function () {
            $("#btn-view-grades").on("click", function () {
                var sectionId = $("#sectionId").val();
                var username = $("#username").val();

                if (username === "" || username === null || username === undefined) {
                    toastr.info("Es necesario ingresar el usuario del alumno", "Información");
                    return;
                }

                if (sectionId === "" || sectionId === null || sectionId === undefined) {
                    toastr.info("Es necesario ingresar el identificador de la sección", "Información");
                    return;
                }
         
                modal.grades.show(username, sectionId);
                $("#btn-view-grades").addLoader();
            })
        },
        init: function () {
            events.validateGrades();
        }
    }

    var form = {
        object: $("#main_form").validate({
            submitHandler: function (formElement, e) {
                e.preventDefault();
                var formData = new FormData(formElement);
                swal({
                    type: "warning",
                    text: `¿Seguro que desea mover las notas oficiales a las notas temporales?`,
                    title: "Mover notas oficiales",
                    confirmButtonText: "Aceptar",
                    showCancelButton: true,
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise(() => {
                            $.ajax({
                                type: "POST",
                                url: `/admin/notas/borrar-notas`,
                                data: formData,
                                contentType: false,
                                processData: false
                            })
                                .done(function (e) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "Proceso realizado con éxito",
                                        confirmButtonText: "Aceptar"
                                    });

                                    modal.grades.object.modal("hide");
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
            }
        })
    }


    var modal = {
        grades: {
            object: $("#modal_grades"),
            show: function (username, sectionId) {
                $.ajax({
                    url: `/admin/notas/get-notas/estudiante/${username}/seccion/${sectionId}`,
                    type: "GEt",
                })
                    .done(function (e) {
                        modal.grades.object.modal("show");
                        $("#container_table").html(e);
                        $("#btn-view-grades").removeLoader();
                    })
                    .fail(function (e) {
                        $("#btn-view-grades").removeLoader();

                        swal({
                            type: "error",
                            title: "Error",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            confirmButtonText: "Aceptar",
                            text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                        });
                    })
            }
        }
    }

    return {
        init: function () {
            events.init();
        }
    }
}();

$(() => {
    index.init();
});