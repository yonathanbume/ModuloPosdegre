var Form = function () {
    var disabledInputs={
        init:function(){
            $("#Name").attr("disabled",true);
            $("#PaternalSurname").attr("disabled",true);
            $("#MaternalSurname").attr("disabled",true);
        }
    };
    var select2 = {
        dependencies: {
            init: function () {
                $.ajax({
                    url: ("/dependencias/todas").proto().parseURL()
                }).done(function (data) {
                    $(".select2-dependencies").select2({
                        placeholder: "Dependencias",
                        //minimumInputLength: 0,
                        data: data.items
                    });

                    select2.dependencies.load();
                });
            },
            load: function () {
                var id = $("#Id").val();
                mApp.block(".m-content", { type: "loader", message: "Cargando..." });
                $.ajax({
                    url: ("/admin/usuarios/" + id + "/dependencias/get").proto().parseURL()
                }).done(function (data) {
                    var selectedValues = [];
                    data.items.forEach(function (item) {
                        selectedValues.push(item.id);
                    });
                    $(".select2-dependencies").val(selectedValues).trigger("change");
                }).always(function (result) {
                    mApp.unblock(".m-content");
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

                    select2.roles.load();
                });
            },
            load: function () {
                var id = $("#Id").val();
                mApp.block(".m-content", { type: "loader", message: "Cargando..." });
                $.ajax({
                    url: `/admin/usuarios/${id}/roles/get`.proto().parseURL()
                }).done(function (data) {
                    console.log(data);
                    data.items.forEach(function (item) {
                        var opt = new Option(item.text, item.id, true, true);
                        $(".select2-roles").append(opt).trigger("change");
                    });
                }).always(function (result) {
                    mApp.unblock(".m-content");
                });
            }
        }
    }

    var fileInput = {
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
    }

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

    var formSwitch = {
        init: function() {
            this.status.init();
        },
        status: {
            init: function() {
                $("#IsActive").on("change",
                    function () {
                        var isActive = $(this).prop("checked");
                        var prevValue = !isActive;
                        var msg = isActive ? "habilitando" : "deshabilitando";
                        swal({
                            title: `Estás ${msg} al usuario <strong>${$("#current-username").val()}</strong> ¿Desea continuar?`,
                            text: "Los cambios se guardarán permanentemente",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            confirmButtonText: "Sí, continuar",
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: `/admin/usuarios/${$("#Id").val()}/editar/estado/post`.proto().parseURL(),
                                        data: {
                                            isActive: isActive
                                        },
                                        type: "POST",
                                        success: function(result) {
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: `Se actualizó el estado del usuario ${$("#current-username").val()
                                                    }`,
                                                confirmButtonText: "Excelente"
                                            });
                                        },
                                        error: function(errormessage) {
                                            swal({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                                confirmButtonText: "Entendido",
                                                text: "Ocurrió un error al intentar actualizar el estado del usuario"
                                            });
                                            $("#IsActive").prop("checked", prevValue);
                                        }
                                    });
                                });
                            }
                        }).then((result) => {
                            if (!result.value) {
                                $("#IsActive").prop("checked", prevValue);
                            }
                        });
                    });
            }
        }
    }

    var datepicker = {
        init: function() {
            this.birthDate.init();
        },
        birthDate: {
            init: function() {
                $("#BirthDate").datepicker("setEndDate", moment().format(_app.constants.formats.datepickerJsMoment));
            }
        }
    }

    var form = {
        submit: function(form) {
            var formData = new FormData($(form)[0]);

            mApp.block(".m-content");

            console.log("test123");
            $.ajax({
                url: $(form).attr("action"),
                type: "POST",
                data: formData,
                contentType: false,
                processData: false
            })
                .done(function () {
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                })
                .fail(function(e) {
                    if (e.responseText != null) $("#alert-text").html(e.responseText);
                    else $("#alert-text").html(_app.constants.toastr.message.error.task);

                    $("#m-form_alert").removeClass("m--hide").show();
                    $("body,html").animate(
                        {
                            scrollTop: $("#m-form_alert").offset().top - 70
                        },
                        800 //speed
                    );                })
                .always(function () {
                    console.log("test");
                    mApp.unblock(".m-content");
                });
        }
    };

    var validate = {
        form: $("#edit-form").validate({
            rules: {
                ConfirmedPassword: {
                    equalTo: "#Password"
                }
            },
            messages: {
                //PaternalSurname: {
                //    pattern: "Por favor, el apellido solo debe contener letras."
                //},
                //MaternalSurname: {
                //    pattern: "Por favor, el apellido solo debe contener letras."
                //}, 
                PhoneNumber: {
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
                        email: $("#Email").val(),
                        userId: $("#Id").val()
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
                                    disabledInputs.init();
                                    swal.close();
                                });
                            }
                        });

                    }
                    else {
                        form.submit(formElement);
                        disabledInputs.init();
                    }
                });
            }
        }),
        reset: function() {
            validate.form.resetForm();
        }
    };


    return {
        init: function () {
            select2.dependencies.init();
            select2.roles.init();
            fileInput.init();
            formSwitch.init();
            datepicker.init();
            events.init();
            disabledInputs.init();
        }
    }
}();

$(function () {
    Form.init();
});