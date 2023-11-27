var index = function () {

    var SectionId = $("#SectionId").val();
    var CourseUnitId = $("#CourseUnitId").val();
    var reload = false;

    var form = {
        object: null,
        rules: function () {
            $.validator.addMethod("grade", function (value, element, params, message) {
                if (value.toLowerCase() == "nr")
                    return true;

                if (/^\d{0,2}(\.\d{0,3})?$/.test(value)) {
                    var grade = parseFloat(value);
                    if (isNaN(grade))
                        return false;

                    if (grade > 20 || grade < 0)
                        return false;

                    return true;
                }
                return false;

            }, 'Por favor ingrese una nota válida.');



        },
        validate: function () {
            form.object = $("#form-validation").validate({
                submitHandler: function (form) {
                    mApp.block(".m-portlet", {
                        message: "Guardando notas..."
                    });

                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: $(form).serialize(),
                        success: function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                            if (reload) {
                                swal({
                                    type: "success",
                                    title: "Notas publicadas",
                                    text: "Las notas han sido publicadas y seran visibles para los estudiantes",
                                    confirmButtonText: "Aceptar"
                                }).then(function (isConfirm) {
                                    if (isConfirm) {
                                        window.location.href = `/profesor/notas/detalle/${SectionId}`;
                                    }
                                });
                            }
                        },
                        error: function (e) {
                            if (e.responseText != null && e.responseText != "") {
                                toastr.error(e.responseText, _app.constants.toastr.title.error);
                                return;
                            }
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        },
                        complete: function () {
                            mApp.unblock(".m-portlet");
                        }
                    });
                }
            })


        },
        init: function () {
            form.rules();
            form.validate();
        }


    }

    var events = {
        onGetStudents: function () {

            mApp.block(".m-portlet", {
                message: "Cargando alumnos..."
            })

            $.ajax({
                url: `/profesor/notas/registrar/getalumnos-unidad/${SectionId}/${CourseUnitId}`,
                type: "GET"
            })
                .done(function (e) {
                    $("#container_datatable").html(e);
                    mApp.unblock(".m-portlet");

                    $('.input-grade').each(function () {
                        $(this).rules('add', {
                            grade: true,
                        });
                    });
                })
        },
        onChangeSubmit: function () {
            $("#publish-btn").on("click", function () {
                $("#form-validation").attr("action", "/profesor/notas/registrar-notas-unidades/publicar");
                reload = true;
                $("#form-validation").submit();
            });

            $("#save-btn").on("click", function () {
                $("#form-validation").attr("action", "/profesor/notas/registrar-notas-unidades/post");
                reload = false;
                $("#form-validation").submit();
            });
        },
        onValidate: function () {
            $("#container_datatable").on("keydown", ".input-grade", function (e) {

                var key = e.charCode || e.keyCode || 0;
                console.log(key);
                // allow backspace, tab, delete, enter, arrows, numbers and keypad numbers ONLY
                // home, end, period, and numpad decimal
                return (
                    //NR
                    key == 78 || key == 82 ||
                    //Digits
                    key == 8 ||
                    key == 9 ||
                    key == 13 ||
                    key == 46 ||
                    key == 110 ||
                    key == 190 ||
                    (key >= 35 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105));
            })
        },
        init: function () {
            events.onGetStudents();
            events.onChangeSubmit();
            events.onValidate();
        }
    }

    return {
        init: function () {
            events.init();
            form.init();
        }
    }
}();

$(() => {
    index.init();
});