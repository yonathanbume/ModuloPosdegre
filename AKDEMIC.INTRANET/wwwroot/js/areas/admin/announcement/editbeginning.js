var UniversityAuthorityManagement = function () {

    var inputs = {
        init: function () {
            $("#StartDate, #EndDate").datepicker({
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
                        url: `/admin/anuncios/por-sistema/editar`.proto().parseURL(),
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


    var select = {
        appearsin: {
            init: function () {
                $("#AppearsIn").on("change", function () {
                    var value = $(this).val();

                    if (value == 1) {
                        $("#div_roles").removeClass("d-none");
                    } else {
                        $("#div_roles").addClass("d-none");
                        $("#Roles").val(null).trigger("change");
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
                        e.items.unshift({ id: '0', text: "Todos" });

                        $("#Roles").select2({
                            data: e.items,
                            placeholder: "Seleccionar Roles",
                            allowClear: true
                        });

                        $("#Roles").on("change", function () {
                            var values = $(this).val();
                            if (values.length > 1 && values.some((e) => e == "0")) {
                                $("#Roles").val(["0"]).trigger("change");
                            }
                        });

                        var toSelect = $("#Roles_input").val().split(",");

                        if (toSelect.length > 0 && $("#Roles_input").val() != "") {
                            $("#Roles").val(toSelect).trigger("change");
                        } else {
                            $("#Roles").val(["0"]).trigger("change");
                        }

                    });
            }
        },
        init: function () {
            select.roles.init();
            select.appearsin.init();
        }
    }

    var summernote = {
        init: function () {
            $("#Description").summernote(this.defaultOptions);
        },
        defaultOptions: {
            lang: "es-ES",
            airMode: false,
            height: 250,
        },
    };


    return {
        init: function () {
            validate.add();
            summernote.init();
            inputs.init();
            select.init();

            if ($("#ImgOrVid").val() == 1) {
                $(".image-div").show();
                $(".youtube-div").hide();
            } else {
                $(".image-div").hide();
                $(".youtube-div").show();
            }
        }
    };
}();

$(function () {
    UniversityAuthorityManagement.init();
});