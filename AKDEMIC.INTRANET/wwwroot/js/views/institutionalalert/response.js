var InstitutionalAlert = function () {

    var loadSelect2 = function(){
        $("#alert_type").select2();
    }

    var send = function(){ 
        $("#form-institutional-alert").validate({
                submitHandler: function(e){
                    
                     $(".btn-save").addLoader();
                         $.ajax({
                        type: 'POST',
                        url: `/alertaInstitucional/responderalertainstitucional`.proto().parseURL(),
                        data: $("#form-institutional-alert").serialize(),
                        success: function () {
                            toastr.success(_app.constants.toastr.message.success.update, _app.constants.toastr.title.edit);
                            $(".btn-save").removeLoader();
                            location.reload(); 
                        },
                        error: function () {
                            toastr.error(_app.constants.toastr.message.error, _app.constants.toastr.title.error);
                        
                        } 
                    });
            }
            }); 
    };
   
 
    return {
        init: function () {
           send();
        }   
    }    
}();


jQuery(document).ready(function () {
    InstitutionalAlert.init(); 
});