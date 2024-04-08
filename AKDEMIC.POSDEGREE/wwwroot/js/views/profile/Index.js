var personalInformation = (function () {

    var events = {
        init: function () {
            $("#CurrentPassword").on('click', function () {
                var form = $("#form-password-change");
                console.log(form);
                var formElements = form.elements;

                console.log(formElements);
                var element = formElements["CurrentPassword"].value;
                
                

            });
        }
    };

    var result = {
        ajax: {
            list: {},
            load: {
                update: function (element, events) {
                    var formElements = element.elements;
                    personalInformation.ajax.list["personal-information-ajax-update"] = $.ajax({
                        data: {
                            PhoneNumber: formElements["PhoneNumber"].value,
                            Email: formElements["Email"].value
                        },
                        type: element.method,
                        url: element.action,
                        beforeSend: function (jqXHR, settings) {
                            $(element).addLoader();
                        },
                        complete: function (jqXHR, textStatus) {
                            $(element).removeLoader();
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                        },
                        success: function (data, textStatus, jqXHR) { 
                            toastr.success("Se actualizaron los datos correctamente", _app.constants.toastr.title.success);

                        }
                    });
                } 
               
            }
        },
        validate: {
            list: {},
            load: {
                create: function () {
                    personalInformation.validate.list["persona-information-update"] = $("#formPersonal-information-update").validate({
                        submitHandler: function (form, event) { 
                            personalInformation.ajax.load.update(form, event);
                        }
                    });
                    events.init();
                }
            }
        }
    }
    return result;
}());

var changePassword = (function () {
    var result = {
        ajax: {
            list: {},
            load: {
                create: function (element, events) {
                    var formElements = element.elements;
                    if (formElements["NewPassword"].value != formElements["RepeatPassword"].value) {
                        toastr.error("Las contraseñas deben ser iguales", "Error");
                        return;
                    }

                    changePassword.ajax.list["form-password-ajax-change"] = $.ajax({
                        data: {
                            CurrentPassword: formElements["CurrentPassword"].value,
                            NewPassword: formElements["NewPassword"].value
                        },
                        type: element.method,
                        url: element.action,
                        beforeSend: function (jqXHR, settings) {
                            $(element).addLoader();
                        },
                        complete: function (jqXHR, textStatus) {
                            $(element).removeLoader();
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            toastr.error("No se pudo actualizar la contraseña", "Error");
                        },
                        success: function (data, textStatus, jqXHR) { 
                            toastr.success(_app.constants.toastr.message.success.create, _app.constants.toastr.title.success);

                        }
                    });
                }

            }
        },
        validate: {
            list: {},
            load: {
                create: function () {
                    changePassword.validate.list["form-password-change"] = $("#form-password-change").validate({
                        submitHandler: function (form, event) {
                            changePassword.ajax.load.create(form, event);
                        }


                    });
                }
            }
        }
    }
    return result;
}());

$(function () { 
    personalInformation.validate.load.create();
    changePassword.validate.load.create();
});

