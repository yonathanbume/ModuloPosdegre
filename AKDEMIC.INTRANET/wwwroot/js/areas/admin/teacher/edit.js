jQuery.extend(jQuery.validator.messages, {
    pattern: "La contraseña debe tener 6 caracteres mínimo"
});

var Form = function () {
    var formValidate = null;
    var disabledInputs={
        init:function(){
            //$("#Name").attr("disabled",true);
            //$("#PaternalSurname").attr("disabled",true);
            //$("#MaternalSurname").attr("disabled",true);
        }
    };
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

    var datepicker = {
        init: function () {
            this.birthDate.init();
        },
        birthDate: {
            init: function () {
                $("#BirthDate").datepicker("setEndDate", moment().format(_app.constants.formats.datepickerJsMoment));
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
            $("#edit-form input").attr("disabled", true);
            $("#edit-form select").attr("disabled", true);
            $("#btnSave").addLoader();
            $.ajax({
                data: formData,
                type: "POST",
                contentType: false,
                processData: false,
                url: $(form).attr("action")
            })
                .always(function () {
                    $("#edit-form input").attr("disabled", false);
                    $("#edit-form select").attr("disabled", false);
                    $("#btnSave").removeLoader();
                    disabledInputs.init();
                })
                .done(function (result) {
                    $("#m-form_alert").addClass("m--hide").show();
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                    location.href = "/admin/docentes".proto().parseURL();

                })
                .fail(function (e) {
                    if (e.responseText !== null) $("#alert-text").html(e.responseText); 
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
            formValidate = $("#edit-form").validate({
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
                    form.submit(formElement);
                    disabledInputs.init();
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

                    if ($("#currentacademicDepartmentId").val() !== _app.constants.guid.empty) {
                        $(".academicDepartment-select").val($("#currentacademicDepartmentId").val()).trigger("change");
                    }
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
            disabledInputs.init();
            select.init();
        }
    }
}();

$(function () {
    Form.init();
})