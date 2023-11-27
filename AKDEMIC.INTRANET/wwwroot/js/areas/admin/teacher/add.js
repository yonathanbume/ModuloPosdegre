jQuery.extend(jQuery.validator.messages, {
    pattern: "La contraseña debe tener 6 caracteres mínimo"
});

var Form = function () {
    var formValidate = null;
       
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
                        if (FileReader && files && files.length) {
                            var fr = new FileReader();
                            fr.onload = function () {
                                $("#current-picture").attr("src", fr.result);
                            };
                            fr.readAsDataURL(files[0]);
                        } 
                        else {
                            console.log("File Reader not supported.");
                        }
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
    }

    var datepicker = {
        init: function () {
            this.birthDate.init();
        },
        birthDate: {
            init: function () {
                $("#BirthDate").datepicker("setEndDate",
                    moment().format(_app.constants.formats.datepickerJsMoment)).one("show", function () {
                    $(this).val("01/01/1990").datepicker("update").trigger("change");
                });
            }
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
                    location.href = "/admin/docentes".proto().parseURL();
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
                    );
                });
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
                    Name: {
                        pattern: "El nombre solo puede contener letras."
                    },
                    PaternalSurname: {
                        pattern: "El apellido paterno solo puede contener letras."
                    },
                    MaternalSurname: {
                        pattern: "El apellido materno solo puede contener letras."
                    },
                    UserName: {
                        pattern: "El usuario no puede contener espacios."
                    }
                },
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit(formElement);
                }
            });
        }
    };

    var select = {
        academicDepartment: {
            init: function () {
                $.ajax({
                    url: `/departamentos-academicos/get`.proto().parseURL()
                }).done(function (data) {
                    $(".academicDepartment-select").select2({
                        placeholder: "Sin asignar",
                        data: data,
                        allowClear: true
                    }).trigger("change");
                });
            }
        },
        init: function () {
            this.academicDepartment.init();
        }
    };

    return {
        init: function () {
            validate.init();
            fileInputs.init();
            datepicker.init();
            events.init();
            select.init();
        }
    }
}();

$(function () {
    Form.init();
})