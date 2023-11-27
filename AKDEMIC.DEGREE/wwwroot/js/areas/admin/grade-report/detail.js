var detail_grade_report = function () {

    var select = function () {

        $.ajax({
            type: 'GET',
            url: `/coordinador-academico/informes-de-grados/obtener-modalidades-grados`.proto().parseURL(),
            success: function (result) {
                if (isIntegrated) {
                    $("#ProcedureId").select2({
                        data: result
                    });
                } else {
                    $("#ConceptId").select2({
                        data: result
                    });
                }
            }
        }).done(function () {
            if (isIntegrated) {
                $("#ProcedureId").val(procedureId).trigger('change');
            } else {
                $("#ConceptId").val(conceptId).trigger('change');
            }
        });



    };

    return {
        init: function () {
            select();
        }
    };
}();

$(function () {
    detail_grade_report.init();
});