var Form = function () {
    var formValidate = null;

    var select2 = {
        dependencies: {
            init: function () {
                $.ajax({
                    url: ("/dependencias/todas").proto().parseURL()
                }).done(function (data) {
                    $(".select2-dependencies").select2({
                        minimumInputLength: 0,
                        placeholder: "Dependencias",
                        data: data.items
                    });
                });
            }
        },
        roles: {
            init: function () {
                $.ajax({
                    url: ("/roles/get").proto().parseURL()
                }).done(function (data) {
                    $(".select2-roles").select2({
                        placeholder: "Roles",
                        minimumInputLength: 0,
                        data: data.items
                    });
                });
            }
        }
    }

    var fileInputs = {
        init: function () {
            this.picture.init();
        },
        picture: {
            init: function () {
                $("#Picture").on("change",
                    function (e) {
                        var tgt = e.target || window.event.srcElement,
                            files = tgt.files;
                        // FileReader support
                        if (FileReader && files && files.length) {
                            var fr = new FileReader();
                            fr.onload = function () {
                                $("#current-picture").attr("src", fr.result);
                            }
                            fr.readAsDataURL(files[0]);
                        }
                        // Not supported
                        else {
                            console.log("File Reader not supported.");
                        }
                    });
            }
        }
    };

    var datepicker = {
        init: function () {
            this.birthDate.init();
        },
        birthDate: {
            init: function () {
                $("#BirthDate").datepicker({
                    format: _app.constants.formats.datepicker
                }).one("show", function () {
                    $(this).val("01/01/1990").datepicker("update").trigger("change");
                });
            }
        }
    };
    var events = {
        init: function () {
            $("#Dni").keypress(function (event) {
                if (event.which === 46 || (event.which < 48 || event.which > 57)) {
                    event.preventDefault();
                }
            }).on('paste', function (event) {
                event.preventDefault();
            });


            $("#PhoneNumber").keypress(function (event) {
                if (event.which === 46 || (event.which < 48 || event.which > 57)) {
                    event.preventDefault();
                }
            }).on('paste', function (event) {
                event.preventDefault();
            });
        }
    };
    var form = {
        submit: function (form) {
            var formData = new FormData($(form).get(0));
            $("#add-form input").attr("disabled", true);
            $("#add-form select").attr("disabled", true);
            $("#btnSave").addLoader();
            $.ajax({
                data: formData,
                type: "POST",
                contentType: false,
                processData: false,
                url: $(form).attr("action")
            })
                .always(function () {
                    $("#add-form input").attr("disabled", false);
                    $("#add-form select").attr("disabled", false);
                    $("#btnSave").removeLoader();
                })
                .done(function (result) {
                    location.href = "/admin/usuarios".proto().parseURL();
                })
                .fail(function (e) {
                    if (e.responseText != null) $("#alert-text").html(e.responseText);
                    else $("#alert-text").html(_app.constants.toastr.message.error.task);

                    $("#m-form_alert").removeClass("m--hide").show();
                    $("body,html").animate(
                        {
                            scrollTop: $("#m-form_alert").offset().top - 70
                        },
                        800 //speed
                    );                });
        }
    };

    var validate = {
        init: function () {
            formValidate = $("#add-form").validate({
                rules: {
                    ConfirmedPassword: {
                        equalTo: "#Password"
                    }
                },
                messages: {
                    PaternalSurname: {
                        pattern: "Por favor, el apellido solo debe contener letras."
                    },
                    MaternalSurname: {
                        pattern: "Por favor, el apellido solo debe contener letras."
                    }, PhoneNumber: {
                        pattern: "El campo no tiene el formato correcto (Ejemplo: 989419189 o 3255564 )"
                    },
                    Dni: {
                        pattern: "Por favor, el Dni solo debe contener números.",
                        maxlength: "Por favor, el Dni solo debe 8 contener dígitos.",
                        minlength: "Por favor, el Dni debe 8 contener dígitos.",
                    }
                },
                submitHandler: function (formElement, e) {
                    e.preventDefault();

                    $.ajax({
                        url: "/admin/usuarios/validar-correo".proto().parseURL(),
                        data: {
                            email: $("#Email").val()
                        }
                    }).done(function (data) {
                        if (data == true || data == "true") {
                            swal({
                                type: "warning",
                                title: "Correo ya registrado",
                                text: "Ya existe un usuario con el correo especificado. ¿Desea continuar y usar el mismo correo?",
                                confirmButtonText: "Aceptar",
                                showCancelButton: true,
                                showLoaderOnConfirm: true,
                                allowOutsideClick: () => !swal.isLoading(),
                                preConfirm: () => {
                                    return new Promise(() => {
                                        form.submit(formElement);
                                        swal.close();
                                    });
                                }
                            });

                        }
                        else {
                            form.submit(formElement);
                        }
                    });
                }
            });
        }
    };

    return {
        init: function () {
            select2.dependencies.init();
            select2.roles.init();
            validate.init();
            fileInputs.init();
            datepicker.init();
            events.init();
        }
    }
}();

$(function () {
    Form.init();
})