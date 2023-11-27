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
                    url: `/admin/usuarios-externos/editar`,
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
                        text: "Datos actualizados correctamente.",
                        confirmButtonText: "Aceptar"
                    })
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