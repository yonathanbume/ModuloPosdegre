
var workerPositionForm = function () {
    var validate = {
        init: function() {
            $("#add-form").validate({});
            if ($("#add-form").isValid()) {
                $("#add-form input").attr("disabled", true);
                $("#btnSave").attr("disabled", true);
                showPageLoaderForSaving();
            }
        }
    };

    return {
        init: function () {
            validate.init();
        }
    }

}();

$(function() {
    workerPositionForm.init();
});
