var UniversityAuthorityManagement = function () {
    var inputs = {
        init: function () {
            $("#StartDateAdd, #EndDateAdd").datepicker({
                format: _app.constants.formats.datepicker,
            });
            $("#ImgOrVid").on("change", function () {
                var typee = $(this).val();
                if (typee == 1) {
                    $(".image-div").show();
                    $(".youtube-div").hide();
                    $("#YoutubeUrlAdd").val("");
                } else {
                    $(".image-div").hide();
                    $("#Image").val(null);
                    $(".youtube-div").show();
                }
            });
        }
    };
    var validate = {
        add: function () {
            $("#add-form").validate({
                submitHandler: function (form) {
                    var btn = $(form).find('button[type="submit"]');
                    btn.addLoader();
                    var fd = new FormData($("#add-form")[0]);
                    $.ajax({
                        type: "POST",
                        url: `/admin/anuncios/por-sistema/guardar`.proto().parseURL(),
                        data: fd,
                        processData: false,
                        contentType: false,
                        success: function () {
                            window.location.href = "/admin/anuncios/por-sistema".proto().parseURL()
                        },
                        error: function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                        },
                        complete: function () {
                            btn.removeLoader();
                        }
                    });
                }
            });

        },
    };

    var summernote = {
        init: function () {
            $("#DescriptionAdd").summernote(this.defaultOptions);
        },
        defaultOptions: {
            lang: "es-ES",
            airMode: false,
            height: 250,
        },
    };

    var select = {
        appearsin: {
            init: function () {
                $("#AppearsInAdd").on("change", function () {
                    var value = $(this).val();

                    if (value == 1) {
                        $("#div_roles").removeClass("d-none");
                    } else {
                        $("#div_roles").addClass("d-none");
                        $("#RolesAdd").val(null).trigger("change");
                    }
                });
            }
        },
        roles: {
            init: function () {
                $.ajax({
                    url: "/roles-anuncios",
                    type: "GET"
                })
                    .done(function (e) {
                        e.items.unshift({ id: '0', text: "Todos"});

                        $("#RolesAdd").select2({
                            data: e.items,
                            placeholder: "Seleccionar Roles",
                            allowClear : true
                        });

                        $("#RolesAdd").on("change", function () {
                            var values = $(this).val();
                            if (values.length > 1 && values.some((e) => e == "0")) {
                                $("#RolesAdd").val(["0"]).trigger("change");
                            }
                        });
                    });
            }
        },
        init: function () {
            select.roles.init();
            select.appearsin.init();
        }
    }

    return {
        init: function () {
            validate.add();
            summernote.init();
            inputs.init();
            $(".youtube-div").hide();
            select.init();
        }
    };
}();

$(function () {
    UniversityAuthorityManagement.init();
});