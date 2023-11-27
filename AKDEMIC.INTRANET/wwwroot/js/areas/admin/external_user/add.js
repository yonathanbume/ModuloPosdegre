var index = function () {

    var form = {
        main: $("#main_form").validate({
            rules: {
                PasswordVerifier: {
                    equalTo: $("#Password")
                }
            },
            submitHandler: function (formElements, e) {
                e.preventDefault();
                var formData = new FormData($(formElements)[0]);

                mApp.block("#main_form", {
                    message: "Enviando formulario..."
                });

                $.ajax({
                    url: `/admin/usuarios-externos/agregar`,
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false
                }).always(function () {
                    mApp.unblock("#main_form");
                }).done(function () {
                    swal({
                        type: "success",
                        title: "Hecho!",
                        text: "Datos agregados correctamente.",
                        confirmButtonText: "Aceptar",
                        closeOnClickOutside: false
                    }).then(function (isConfirm) {
                        if (isConfirm.value) {
                            window.location.href = `/admin/usuarios-externos`;
                        }
                    });
                }).fail(function (e) {
                    swal({
                        type: "error",
                        title: "Error al guardar la informaciñon",
                        text: e.responseText,
                        confirmButtonText: "Aceptar",
                        closeOnClickOutside: false
                    });
                });
            }
        })
    }

    var datepicker = {
        init: function () {
            $("#BirthDate").datepicker({
                format: _app.constants.formats.datepicker
            });
        }
    }

    return {
        init: function () {
            datepicker.init();
        }
    }
}();

$(() => {
    index.init();
});