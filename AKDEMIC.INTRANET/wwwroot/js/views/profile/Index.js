var personalInformation = (function () {

    var events = {
        init: function () {
            $("#CurrentPasswordEye").on('click', function () {                
                if ($("#CurrentPassword").attr('type') === 'password') {
                    console.log("ISÄSS");
                    $("#CurrentPassword").attr('type', "text");
                } else {
                     
                        $("#CurrentPassword").attr('type', "password"); 
                }
            });
        }       
    };

    var select = {
        init: function () {
            this.districts.init();
            this.provinces.init();
            this.departments.init();
        },
        departments: {
            init: function () {
                $.ajax({
                    url: "/departamentos/get".proto().parseURL()
                }).done(function (result) {

                    $(".select2-departments").select2({
                        placeholder: "Departamentos",
                        minimumResultsForSearch: -1,
                        data: result.items
                    });

                    if ($("#departmentId").val() != "" && $("#departmentId").val() != null) {
                        $(".select2-departments").val($("#departmentId").val()).trigger("change");

                        $.ajax({
                            url: `/departamentos/${$("#departmentId").val()}/provincias/get`.proto().parseURL()
                        }).done(function (result) {
                            $(".select2-provinces").empty();

                            $(".select2-provinces").select2({
                                placeholder: "Provincias",
                                minimumResultsForSearch: -1,
                                data: result.items,
                                disabled: false
                            });

                            if ($("#provinceId").val() != "" && $("#provinceId").val() != null) {
                                $(".select2-provinces").val($("#provinceId").val()).trigger("change");

                                $.ajax({
                                    url: `/departamentos/${$("#departmentId").val()}/provincias/${$("#provinceId").val()}/distritos/get`.proto().parseURL()
                                }).done(function (result) {
                                    $(".select2-districts").empty();

                                    $(".select2-districts").select2({
                                        placeholder: "Distritos",
                                        minimumResultsForSearch: -1,
                                        data: result.items,
                                        disabled: false
                                    });

                                    if ($("#districtId").val() != "" && $("#districtId").val() != null) {
                                        $(".select2-districts").val($("#districtId").val()).trigger("change");
                                    }
                                });
                            }
                        });


                    }

                    $(".select2-departments").on("change", function () {
                        select.provinces.load($(this).val());
                    });

                    $(".select2-provinces").on("change", function () {
                        select.districts.load($(this).val());
                    });
                });
            }
        },
        provinces: {
            init: function () {
                $(".select2-provinces").select2({
                    placeholder: "Seleccione un departamento",
                    disabled: true
                });
            },
            load: function (departmentId) {

                $.ajax({
                    url: `/departamentos/${departmentId}/provincias/get`.proto().parseURL()
                }).done(function (result) {
                    $(".select2-provinces").empty();

                    $(".select2-provinces").select2({
                        placeholder: "Provincias",
                        minimumResultsForSearch: -1,
                        data: result.items,
                        disabled: false
                    }).trigger("change");
                });
            }
        },
        districts: {
            init: function () {
                $(".select2-districts").select2({
                    placeholder: "Seleccione una provincia",
                    disabled: true
                });
            },
            load: function (provinceId) {
                var departmentId = $("#department-select").val();

                $.ajax({
                    url: `/departamentos/${departmentId}/provincias/${provinceId}/distritos/get`.proto().parseURL()
                }).done(function (result) {
                    $(".select2-districts").empty();

                    $(".select2-districts").select2({
                        placeholder: "Distritos",
                        minimumResultsForSearch: -1,
                        data: result.items,
                        disabled: false
                    });
                });
            }
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
                            Email: formElements["Email"].value,
                            DepartmentId: formElements["DepartmentId"].value,
                            ProvinceId: formElements["ProvinceId"].value,
                            DistrictId: formElements["DistrictId"].value,
                            Address: formElements["Address"].value,
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
                    select.init();
                    events.init();
                }
            },
            extra: {
                numbers_validations: function () {
                    $("#formPersonal-information-update input[name='PhoneNumber']").keypress(function (event) {
                        if (event.which === 46 || (event.which < 48 || event.which > 57)) {
                            event.preventDefault();
                        }
                    }).on('paste', function (event) {
                        event.preventDefault();
                    });
                    $("#formPersonal-information-update input[name='PhoneNumber']").attr("maxlength", "9");
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
                    if (formElements["NewPassword"].value !== formElements["RepeatPassword"].value) {
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

                            window.location.href = "/";
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
    personalInformation.validate.extra.numbers_validations();
    changePassword.validate.load.create();
});

