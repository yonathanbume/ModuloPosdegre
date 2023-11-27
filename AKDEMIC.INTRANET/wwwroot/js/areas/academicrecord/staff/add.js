var addUser = function () {
    var form = {
        object: $("#new-user"),
        validate: function () {
            form.object.validate({
                rules: {
                    Name: {
                        required: true,
                        maxlength: 30
                    },
                    PaternalSurname: {
                        required: true,
                        maxlength: 30
                    },
                    MaternalSurname: {
                        required: true,
                        maxlength: 30
                    },
                    UserName: {
                        required: true,
                        maxlength: 15
                    },
                    Password: {
                        required: true,
                        maxlength: 15
                    },
                    PasswordVerifier: {
                        required: true,
                        maxlength: 15
                    },
                    Email: {
                        required: true,
                        email: true
                    },
                    PhoneNumber: {
                        required: true,
                        minlength : 7,
                        maxlength: 9,
                        digits: true
                    }
                },
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit(formElement);
                }
            });
        },
        submit: function (formElement) {

            var password = $("#Password").val();
            var passwordVerifier = $("#PasswordVerifier").val();
            if (password !== passwordVerifier) {
                toastr.error("Verificar contraseñas", "Error!");
                return;
            }

            mApp.block("#new__portlet");
            var formData = new FormData($(formElement)[0]);

            $.ajax({
                url: "/registrosacademicos/personal/nuevo",
                type: "POST",
                data: formData,
                contentType: false,
                processData: false
            })
                .done(function (e) {
                    swal({
                        type: "success",
                        title: "Completado",
                        text: "Usuario Creado Satisfactoriamente.",
                        confirmButtonText: "Aceptar"
                    }).then(function (isConfirm) {
                        if (isConfirm) {
                            location.href = '/registrosacademicos/personal';
                        }
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
                    mApp.unblock("#new__portlet");

                });
        },
        events: {
            init: function () {
                $("#btn-back").click(function () {
                    swal({
                        type: "error",
                        title: "Saldrá de la página.",
                        text: "¿Seguro que desea salir?.",
                        confirmButtonText: "Aceptar",
                        showCancelButton: true,
                    }).then(function (isConfirm) {
                        if (isConfirm.value) {
                            location.href = '/registrosacademicos/personal';
                        }
                    });
                });
            }
        },
        init: function () {
            form.validate();
            form.events.init();
        }
    };

    return {
        init: function () {
            form.init();
        }
    };
}();

$(function () {
    addUser.init();
})