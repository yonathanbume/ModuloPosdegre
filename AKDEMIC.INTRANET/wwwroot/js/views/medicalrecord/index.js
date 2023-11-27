var medicalRecord = function () {

    var initEvents = function () {
        $("#HasDrugAllergy").on('change');
        $("#BloodTypeKnowledge").on('change');
    }

    var events = function () {
        $("#HasDrugAllergy").on('change', function () {
            var value = $(this).val();
            var isTrueSet = (value == 'True') || (value == 'true');
            if (isTrueSet) {
                $("#DrugAllergyDescription").prop("disabled", false);
            } else {
                $("#DrugAllergyDescription").prop("disabled", true);
            }
            
        });

        $("#BloodTypeKnowledge").on('change', function () {
            var value = $(this).val();
            var isTrueSet = (value == 'True') || (value == 'true');
            if (isTrueSet) {
                $("#BloodType").prop("disabled", false);
                $("#RhFactor").prop("disabled", false);

            } else {
                $("#BloodType").prop("disabled", true);
                $("#RhFactor").prop("disabled", true);
            }

        });
    };

    var validate = {
        add: function () {
            $("#medical-record-form").validate({
                submitHandler: function (form) {
                    var btn = $(form).find("button[type='submit']");
                    btn.addLoader();
                    $.ajax({
                        type: "POST",
                        url: `/ficha-medica/guardar`.proto().parseURL(),
                        data: $(form).serialize(),
                        success: function (data) {  
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btn-go-to").show();
                            window.open(data, '_blank');
                            
                        },
                        error: function () {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        },
                        complete: function () {
                            btn.removeLoader();
                        }
                    });
                }
            });

        }
    };

    return {
        init: function () {     
            events();
            initEvents();            
            validate.add();      
        }
    };
}();

$(function () {
    medicalRecord.init();
});