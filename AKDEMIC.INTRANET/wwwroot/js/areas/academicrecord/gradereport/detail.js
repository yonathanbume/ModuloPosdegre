var detail_grade_report = function () {

    var select = function () {

        $.ajax({
            type: 'GET',
            url: `/registrosacademicos/informes-de-grados/periodos/get`.proto().parseURL(),
            success: function (result) {
                $("#AdmissionTermId").select2({
                    data: result
                });

                $("#GraduationTermId").select2({
                    data: result
                });
            }
        }).done(function () {
            $("#AdmissionTermId").val(admissionTermId).trigger('change');
            $("#GraduationTermId").val(graduationTermId).trigger('change');
        });       


        $.ajax({
            type: 'GET',
            url: `/registrosacademicos/informes-de-grados/obtener-modalidades-grados`.proto().parseURL(),
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

    var downloadDocument = function () {
        $(".download").on('click', function () {
            var id = $(this).data('id');
            window.open(`/registrosacademicos/informes-de-grados/descargar-documento/${id}`.proto().parseURL(), '_blank');
        });
    };

    return {
        init: function () {
            select();
            downloadDocument();
        }
    };
}();

$(function () {
    detail_grade_report.init();
});