var NutritionalPage = function () {
    var datatable_done = null;

    var OtherFunctions = function () {      
           $('.amt').keyup(function () {
               var IMC = 0;
               var talla =  parseInt($("#Size").val());
               var peso =   parseInt($("#CurrentWeight").val());
               IMC =    (peso/((talla*0.01) * (talla*0.01)));
               
               $("#IMC").val(parseFloat(IMC).toFixed(2));               
           });

       }

   
    var SubmitForm = function () {
        $("#nutritional-form").submit(function (e) {
            e.preventDefault();

            $.ajax({
                type: 'POST',
                url: `/admin/bienestar_institucional/SaveNutritionalResponses/post`.proto().parseURL(),
                data: $("#nutritional-form").serialize(),                
                success: function () {                    
                    location.href = `/nutricion/horario-citas`.proto().parseURL();                            
                },
                error: function () {
                    toastr.error(_app.constants.toastr.message.error, _app.constants.toastr.title.error);
                }

            });
        });
    }
    return {
        load: function () {            
            OtherFunctions();
            SubmitForm();
        }
    }
}();

$(function () {

    NutritionalPage.load();
    
});