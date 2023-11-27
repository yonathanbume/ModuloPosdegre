 

var diploma = function () {

    var datepickers = function () {
        $("#OriginDiplomatDate").datepicker();
        $("#ResolutionDateByUniversityCouncil").datepicker();
        $("#DuplicateDiplomatDate").datepicker();
    };

    var select2init = function () {
        $("#OriginGradeDenomination").select2();
    }

    var events = function () {

        $("#DocumentNumber").keypress(function (event) {
            if (event.which === 46 || (event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
        }).on('paste', function (event) {
            event.preventDefault();
        });



        $("#update-form").validate({
            submitHandler: function (form, event) {
                $.ajax({
                    type: "POST",
                    url: `/admin/generacion-diplomas/editar`.proto().parseURL(),
                    data: $(form).serialize(),
                    success: function () {
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.update);
                    },
                    error: function () {
                        toastr.success(_app.constants.toastr.message.error.update, _app.constants.toastr.title.error);
                    }
                });
            } 
        });
         
        $("#OriginGradeDenomination").on('change', function () {
            var gradeDenomination = $(this).val();
            $("#GradeAbbreviation").val(gradeDenomination);
        });
    };
    return {
        load: function () {
            datepickers();
            events();
           // select2init();
        }
    }
}();

$(function () {
    diploma.load();
});


