var index = function () {

    var alert = {
        object: $(".validation-summary-valid li"),
        show: function (message) {
            alert.object.text(message);
            alert.object.css("display", "block");
            $("#NewPassword").val("");
            $("#ConfirmPassword").val("");
        },
        hide: function () {
            alert.object.text("");
            alert.object.css("display", "none");
        }
    }

    var form = {
        object: $("#change_password_form"),
        validate: function () {
            form.object.validate({
                rules: {
                    ConfirmPassword: {
                        equalTo: "#NewPassword"
                    }
                },
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    var $btn = $("#btn_submit_change_password");
                    $btn.addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                    var formData = new FormData(formElement);

                    $.ajax({
                        url: $(form.object).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    })
                        .done(function (response) {
                            alert.hide();
                            window.location.href = "/";
                        })
                        .fail(function (e) {
                            console.log(e);
                            var message = "";
                            switch (e.status) {
                                case 0:
                                    if (e.statusText == "timeout") {
                                        window.location.reload();
                                    }
                                    break;
                                case 500:
                                    message = "Ocurrió un error en el servidor.";
                                    break;
                                case 400:
                                    message = e.responseText;
                                    break;

                                default:
                                    message = "Error al intentar ingresar.";
                                    break;
                            }
                            alert.show(message);
                            $btn.removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                        });
                }
            })
        },
        init: function () {
            this.validate();
        }
    }

    return {
        init: function () {
            form.init();
        }
    }
}();

$(() => {

    index.init();
});