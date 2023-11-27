var diplomas = function () {

    var validate = {
        search: function () {
            $("#form").validate({           
                submitHandler: function () {
                    var userName = $("#form input[name='userName']").val();
                    $.ajax({
                        type: "GET",
                        url: `/seguimiento-diploma-2/${userName}`.proto().parseURL(),                 
                        success: function (data) {
                            $("#rendering").css("display", "block");
                            $("#message").html(data);
                        },
                        error: function () {
                   
                        }
                    });
                }
            });

        }
    }; 

    return {
        load: function () {  
            validate.search();            
        }
    };
}();

$(function () {
    diplomas.load();
});


