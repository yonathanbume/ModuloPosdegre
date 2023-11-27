var Event = (function () {

    var initFormValidation = function () {
        formCreate = $("#create-form").validate({
            submitHandler: function (formElements, e) {
                e.preventDefault();
                let formData = new FormData($(formElements)[0]);
                $(`#create-form input, #create-form select,#create-form textarea`).attr("disabled", true);
                $(".add").addLoader();
                $.ajax({
                    url: $(formElements).attr("action"),
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false
                }).always(function () {
                    $(`#create-form input, #create-form select,#create-form textarea`).attr("disabled", false);
                    $(".add").removeLoader();
                }).done(function () {
                    swal({
                        type: "success",
                        title: 'Éxito',
                        text: 'Solicitud registrada correctamente',
                        confirmButtonText: "Ok"
                    }).then(function () {
                        window.location = "/estudiante/apoyo-economico".proto().parseURL()
                    });
                }).fail(function (e) {
                    var text = "";
                    if (e.responseText !== null)
                        text = e.responseText;
                    else
                        text = _app.constants.toastr.message.error.task;

                    swal({
                        type: "error",
                        title: _app.constants.toastr.title.error,
                        text: text,
                        confirmButtonText: "Ok"
                    });
                });
            }
        });
    };

    var initCall = function () {
        $(document).on('change', '.custom-file-input', function (event) {

            $(this).next('.custom-file-label').html(event.target.files[0].name);

        });

    };

    return {
        init: function () {
            initCall();
            initFormValidation();
        }
    }


})();

$(function () {
    Event.init();
});
