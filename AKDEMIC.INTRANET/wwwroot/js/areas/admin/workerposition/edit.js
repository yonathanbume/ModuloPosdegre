
var userForm = function () {
    
    var form = {
        submit: function (formElement) {
            var formData = new FormData($(formElement).get(0));
            $("#btnSave").attr("disabled", true);
            showPageLoaderForSaving();

            $.ajax({
                type: "POST",
                data: formData,
                contentType: false,
                processData: false,
                url: $(formElement).attr("action").proto().parseURL()
            })
            .always(function() {
                $("#btnSave").attr("disabled", false);
                hidePageLoader();
            })
            .done(function (result) {
                showToastrSuccess();
            })
            .fail(function(e) {
                showToastrFail(e.responseText);
            });
        }
    };

    var validate = {
        init: function () {
            $("#edit-form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit(formElement);
                }
            });
        },
        reset: function() {
            validate.form.resetForm();
        }
    };

    return {
        init: function () {
            validate.init();
        }
    }
}();

$(function () {
    userForm.init();
});
