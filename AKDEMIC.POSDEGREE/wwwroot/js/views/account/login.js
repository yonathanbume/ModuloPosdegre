var login = function () {
    var alert = {
        object: $(".validation-summary-valid li"),
        show: function (message) {
            alert.object.text(message);
            alert.object.css("display", "block");
            $("#Password").val("");
        },
        hide: function () {
            alert.object.text("");
            alert.object.css("display", "none");
        }
    }

    var form = {
        object: $("#main_form_login"),
        validate: function () {
            form.object.validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    var $btn = $("#m_login_signin_submit");
                    $btn.addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                    var formData = new FormData(formElement);

                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        //timeout: 35000,
                        contentType: false,
                        processData: false
                    })
                        .done(function (response) {
                            alert.hide();
                            window.location.href = response;
                        })
                        .fail(function (e) {
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
    login.init();
});