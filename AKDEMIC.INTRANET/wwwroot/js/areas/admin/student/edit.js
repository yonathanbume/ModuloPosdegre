var Form = function () {
    var disabledInputs={
        init:function(){
            $("#Name").attr("disabled",true);
            $("#PaternalSurname").attr("disabled",true);
            $("#MaternalSurname").attr("disabled", true);
            $("#UserName").attr("disabled", true);
        }
    };
    var select2 = {
        init: function () {
            this.faculties.init();
        },
        faculties: {
            init: function () {
                $.ajax({
                    url: "/facultades/get".proto().parseURL()
                }).done(function (result) {
                    $(".select2-faculties").select2({
                        placeholder: "Facultades",
                        minimumInputLength: 0,
                        data: result.items
                    })
                        .val($("#FacultyIdValue").val())
                        .on("change", function () {
                            select2.careers.init($(this).val());
                        })
                        .trigger("change");

                    select2.careers.init($(this).val());
                });
            }
        },
        careers: {
            init: function (facultyId) {
                $(".select2-careers").prop("disabled", true);
                $(".select2-careers").empty();
                $(".select2-careers").select2({
                    placeholder: "Carrera"
                });
                $.ajax({
                    url: `/carreras/get?fid=${facultyId}`.proto().parseURL()
                }).done(function (data) {
                    $(".select2-careers").select2({
                        placeholder: "Carrera",
                        minimumInputLength: 0,
                        data: data.items
                    });
                    if (data.items.length) {
                        //$(".select2-careers").prop("disabled", false);
                        var careerId = $("#CareerId").val();
                        if (careerId !== null && careerId !== "") {
                            $(".select2-careers").val(careerId).trigger("change");
                            $("#CareerId").val(null).trigger("change");
                        }
                    }
                });
                $(".select2-faculties").prop("disabled", true);
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

    var datepicker = {
        init: function () {
            this.birthDate.init();
        },
        birthDate: {
            init: function () {
                $("#BirthDate").datepicker("setEndDate", moment().format(_app.constants.formats.datepickerJsMoment));
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
        }
    }

    var form = {
        submit: function (form) {
            var formData = new FormData($(form)[0]);
            $("#edit-form input").attr("disabled", true);
            $("#edit-form select").attr("disabled", true);
            $("#btnSave").addLoader();
            $.ajax({
                url: $(form).attr("action"),
                type: "POST",
                data: formData,
                contentType: false,
                processData: false
            })
                .always(function () {
                    $("#edit-form input").attr("disabled", false);
                    disabledInputs.init();
                    $("#edit-form select").attr("disabled", false);
                    $("#btnSave").removeLoader();
                })
                .done(function () {
                    $("#m-form_alert").addClass("m--hide").show();
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                })
                .fail(function (e) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (e.responseText != null) $("#alert-text").html(e.responseText);
                    else $("#alert-text").html(_app.constants.toastr.message.error.task);
                    $("#m-form_alert").removeClass("m--hide").show();
                    mApp.scrollTop();
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
                //Name: {
                //    pattern: "Por favor el apellido solo debe contener letras."
                //},
                //PaternalSurname: {
                //    pattern: "Por favor el apellido solo debe contener letras."
                //},
                //MaternalSurname: {
                //    pattern: "Por favor el apellido solo debe contener letras."
                //},
                PhoneNumber: {
                    pattern: "El campo no tiene el formato correcto (Ejemplo: 989419189 o 3255564 )"
                },
                Dni: {
                    pattern: "Por favor el Dni solo debe contener números.",
                    maxlength: "Por favor el Dni solo debe 8 contener dígitos.",
                    minlength: "Por favor el Dni debe 8 contener dígitos.",
                }
            },
            submitHandler: function (formElement, e) {
                e.preventDefault();
                form.submit(formElement);
            }
        }),

        reset: function () {
            validate.form.resetForm();
        }
    };

    return {
        init: function () {
            select2.init();
            datepicker.init();
            fileInput.init();
            events.init();
            disabledInputs.init();
        }
    }
}();

$(function () {
    Form.init();
});