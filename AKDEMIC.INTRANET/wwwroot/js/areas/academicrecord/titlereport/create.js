var creation_title_report = function () {
    var datepicker = function () {
        $("#Date").datepicker();
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


        $("#AdmissionYear").datepicker()
            .on("changeDate", function (e) {
                $(this).valid();

                $("#GraduationYear").datepicker("setStartDate", e.date);

            });

        $("#GraduationYear").datepicker()
            .on("changeDate", function (e) {
                $(this).valid();

                $("#AdmissionYear").datepicker("setEndDate", e.date);

            });

    };

    var select = function () {
        $("#btn-redirect").on('click', function () {
            var url = $("#ResearchWorkURL").val();
            window.open(url, '_blank');
        });

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
        });      

        $.ajax({
            type: 'GET',
            url: `/registrosacademicos/informes-de-titulos/obtener-modalidades-titulo`.proto().parseURL(),
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
            var validator = $("#title-report-information").validate();
            $("#username-search").val('');
            $("#username-search").prop('disabled', false);
            $("#title-report-information input[name='StudentId']").val('');
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
                    url: `/registrosacademicos/informes-de-titulos/busqueda-por-alumno/${username}`.proto().parseURL(),
                    type: "POST",
                    success: function (data) {                        
                        if (data !== null && data !== undefined) {
                            $("#student-fullname").html(`(Estudiante : ${data.fullName} )`);
                            $("#username-search").prop('disabled', true);
                            $("#title-report-information input[name='StudentId']").val(data.id);
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
                            $("#PromotionGrade").val(data.promotionGrade);
                            $("#Credits").val(data.credits);
                            $("#YearsStudied").val(data.yearsStudied);
                            $("#SemesterStudied").val(data.semesterStudied);
                            $("#ResearchWork").val(data.researchWork);
                            $("#ResearchWorkURL").val(data.researchWorkURL);
                            $("#BachelorOrigin").val(data.bachelorOrigin);
                            $("#PedagogicalTitleOrigin").val(data.pedagogicalTitleOrigin);
                            $("#StudyModality").val(data.studyModality).trigger('change');
                            $("#OriginDegreeCountry").val(data.originDegreeCountry);
                            $("#Observation").val(data.observation);
                            $("#Number").val(data.number);
                            $("#GraduationDate").val(data.graduationDate);
                            
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

        $("#title-report-information").validate({
            submitHandler: function (formElement, e) {
                e.preventDefault();      
                var formData = new FormData($('#title-report-information')[0]);
                $('.d_requirement').each(function (index, element) {
                    var file = $($(element).find("div").find("input"))["0"].files[0];
                    var observation = $(element).find("textarea").val();
                    formData.append(`DegreeRequirements[${index}].DocumentFile`, file);
                    formData.append(`DegreeRequirements[${index}].Observation`, observation);
                    formData.append(`DegreeRequirements[${index}].DegreeRequirementId`, $(element).attr('id'));


                });
                if ($("#StudentId").val() === "" || $("#StudentId").val() === null || $("#StudentId").val() === undefined ) {
                    toastr.error("No se especificó el estudiante para la creación del informe de título", _app.constants.toastr.title.error);
                    return false;
                }
                mApp.block("#title-report-information", { type: "loader", message: "Cargando..." });
                var button = $(formElement).find("input[type='submit']");
                button.addLoader();
                $.ajax({
                    url: `/registrosacademicos/informes-de-titulos/registrar-informe-titulo`.proto().parseURL(),
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
                    $.fileDownload(`/registrosacademicos/informes-de-titulos/generar-constancia-titulo/${entityId}`.proto().parseURL());
                }).always(function () {
                    mApp.unblock("#title-report-information");
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
    creation_title_report.init();
});