var degreeConfiguration = function () {

    var select = function () {
        $("#grade_select").select2({
            placeholder: "Seleccione el grado"
        });

        $("#student_select").select2({
            width: "100%",
            placeholder: "Buscar...",
            ajax: {
                url: "/admin/solicitud-grado-por-requisitos/buscar".proto().parseURL(),
                dataType: "json",
                data: function (params) {
                    return {
                        term: params.term,
                        page: params.page,
                        degreeType : $("#grade_select").val()
                    };
                },
                processResults: function (data, params) {
                    return {
                        results: data.items
                    };
                },
                cache: true
            },
            escapeMarkup: function (markup) {
                return markup;
            },
            minimumInputLength: 3
        });
    };

    var buttom_actions = function () {
        $("#btn-create").on('click', function () {
            var studentId = $("#student_select").val();
            var gradeType = $("#grade_select").val();
            if (studentId !== undefined && studentId !== null) {
                window.location.href = `/admin/solicitud-grado-por-requisitos/informe/${gradeType}/${studentId}`.proto().parseURL();
            }
        });
    };


    var validate = function () {      

        $("#grade-report-information").validate({
            submitHandler: function (formElement, e) {
                e.preventDefault();
                var formData = new FormData($('#grade-report-information')[0]);
                $('.d_requirement').each(function (index, element) {
                    var file = $($(element).find("div").find("input"))["0"].files[0];
                    var observation = $(element).find("textarea").val();
                    formData.append(`DegreeRequirements[${index}].DocumentFile`, file);
                    formData.append(`DegreeRequirements[${index}].Observation`, observation);
                    formData.append(`DegreeRequirements[${index}].DegreeRequirementId`, $(element).attr('id'));


                });
                mApp.block("#grade-report-information", { type: "loader", message: "Cargando..." });
                var button = $(formElement).find("input[type='submit']");
                button.addLoader();
                $.ajax({
                    url: `/admin/solicitud-grado-por-requisitos/registrar-informe-grado`.proto().parseURL(),
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false,
                    success: function () {
                        toastr.success("Registrado correctamente", _app.constants.toastr.title.success);
                        window.location.href = "/admin/solicitud-grado-por-requisitos";
                    }
                }).fail(function (error) {
                    toastr.error(error.responseText, _app.constants.toastr.title.error);
                }).always(function () {
                    mApp.unblock("#grade-report-information");
                    button.removeLoader();

                });
            }
        });
    };

    return {
        load: function () {
            select();
            buttom_actions();
            validate();
        }
    };

}();

$(function () {
    degreeConfiguration.load();
});

