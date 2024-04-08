var ResetPassword = function () {
    
    var validate = function () { 
        $("#form").validate({
            rules: {
                ConfirmPassword: {
                    equalTo: "#Password"
                }
            }
        });
    }; 

    return {
        init: function () {
            if ($('#Message').val() !== '') {
                toastr.error($("#Message").val(), "Error");
                $("#Message").val('');
            }
             validate();
        }
    }
}();


$(function () {
   ResetPassword.init();
});
