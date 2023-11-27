var creation_grade_report = function () {
    var datepicker = function () {

        $("#btn-redirect").on('click', function () {
            var url = $("#ResearchWorkURL").val();
            window.open(url, '_blank');
        });

        $("#Date").datepicker();
        $("#GraduationDate").datepicker();
        $("#Year").datepicker({
            format: "yyyy",
            viewMode: "years",
            minViewMode: "years"
        });
        $("#AdmissionYear").datepicker({
            format: "yyyy",
            viewMode: "years",
            minViewMode: "years"
        });
        $("#GraduationYear").datepicker({
            format: "yyyy",
            viewMode: "years",
            minViewMode: "years"
        });              

    };

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
            $("#GraduationTermId").on("select2:close", function (e) {
                $(this).valid();
            });
        });        
        $("#GraduationTermId").on('change', function () {

            var termId = $(this).val();
            if (termId !== undefined && termId !== null) {
                $.ajax({
                    type: 'GET',
                    url: `/registrosacademicos/informes-de-grados/obtener-fecha-finalizacion/${termId}`.proto().parseURL(),
                    success: function (data) {
                        $("#GraduationDate").val(data);
                    }
                });
            }
            
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
        });
        
    };

    var clear = function () {
        $("#btn-reset").on('click', function (e) {
            e.preventDefault();          
            var validator = $("#grade-report-information").validate();
            $("#username-search").val('');
            $("#username-search").prop('disabled', false);
            $("#grade-report-information input[name='StudentId']").val('');
            $("#Code").val('');
            $("#PaternalSurName").val('');
            $("#MaternalSurName").val('');
            $("#Name").val('');
            $("#Faculty").val('');
            $("#Career").val('');
            $("#CurricularSystem").val('');
            $("#Curriculum").val('');
            $("#AcademicProgram").val('');
            $("#student-fullname").html('');
            $("#GraduationDate").val('');
            validator.resetForm();

        });
    };

    var validate = function () {
        $("#student-information").validate({
            submitHandler: function (formElement, e) {
                e.preventDefault();                   
                var button = $(formElement).find("button[type='submit']");
                button.addLoader();                                
                mApp.block("#div-rendering", { type: "loader", message: "Cargando..." });
               
                var username = $(formElement).find("input[id='username-search']").val();
                $.ajax({
                    url: `/registrosacademicos/informes-de-grados/busqueda-por-alumno/${username}`.proto().parseURL(),
                    type: "POST",
                    success: function (data) {
                        if (data !== null && data !== undefined) {
                            $("#student-fullname").html(`(Alumno : ${data.fullName} )`);
                            $("#username-search").prop('disabled', true);
                            $("#grade-report-information input[name='StudentId']").val(data.id);
                            $("#Code").val(data.code);
                            $("#PaternalSurName").val(data.paternalSurname);
                            $("#MaternalSurName").val(data.maternalSurname);
                            $("#Name").val(data.name);
                            $("#Faculty").val(data.facultyName);
                            $("#Career").val(data.careerName);
                            $("#CurricularSystem").val(data.curricularSystem);
                            $("#Curriculum").val(data.curriculum);
                            $("#AcademicProgram").val(data.academicProgram);
                            $("#Year").val(data.year);
                            $("#Date").val(data.date);
                            $("#AdmissionTermId").val(data.admissionTermId).trigger('change');
                            $("#GraduationTermId").val(data.graduationTermId).trigger('change');
                            if (data.hasGraduationTerm) {
                                if (!$("#GraduationTermId").is(":disabled")) {
                                    $("#GraduationTermId").attr("disabled", true);
                                }
                            } else {
                                $("#GraduationTermId").attr("disabled", false);
                            }

                            $("#PromotionGrade").val(data.promotionGrade);
                            $("#Credits").val(data.credits);
                            $("#Number").val(data.number);
                        } else {
                            mApp.unblock("#div-rendering");
                            button.removeLoader();
                        }

                    },
                    complete: function () {
                        mApp.unblock("#div-rendering");
                        button.removeLoader();
                    }
                });
            }
        });

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
                if ($("#StudentId").val() === "" || $("#StudentId").val() === null || $("#StudentId").val() === undefined ) {
                    toastr.error("No se especificó el estudiante para la creación del informe de grados", _app.constants.toastr.title.error);
                    return false;
                }
                mApp.block("#grade-report-information", { type: "loader", message: "Cargando..." });
                var button = $(formElement).find("input[type='submit']");
                button.addLoader();
                $.ajax({
                    url: `/registrosacademicos/informes-de-grados/registrar-informe-grado`.proto().parseURL(),
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false,
                    success: function () {                        
                        toastr.success("Informe generado correctamente", _app.constants.toastr.title.success);
                    }
                }).fail(function (error) {
                    toastr.error(error.responseText, _app.constants.toastr.title.error);                  
                }).done(function (entityId) {
                    $.fileDownload(`/registrosacademicos/informes-de-grados/generar-constancia-grado/${entityId}`.proto().parseURL());
                }).always(function () {
                    mApp.unblock("#grade-report-information");
                    button.removeLoader();
                });
            }
        });
    };

    return {
        init: function () {
            select();
            datepicker();
            validate();
            clear();
            $("#student-information button[type='submit']").trigger('submit');
        }
    };
}();

$(function () {
    creation_grade_report.init();
    
    $("#GraduationTermId").trigger('change');
});