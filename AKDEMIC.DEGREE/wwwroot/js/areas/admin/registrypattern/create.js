var registrypatterns_create = function () {

    var moreInformation = function () {
        $("#btn-more-info").on('click', function (e) {
            e.preventDefault();
            $("#modal-info").modal('show');
        });
    };

    var select = function () {        
        $.ajax({
            type: 'GET',
            url: `/admin/padron-de-registro/periodos/get`.proto().parseURL(),
            success: function (data) {
                $("#graduationTermId").select2({
                    data: data
                });
            }
        }).done(function () {
            if (GraduationTermId != null && GraduationTermId !== undefined) {
                $("#graduationTermId").val(GraduationTermId).trigger('change');
            }
        });


        $.ajax({
            type: 'GET',
            url: `/admin/padron-de-registro/programaacademicos2/${CareerId}/get`.proto().parseURL()
        }).done(function (data) {
            $("#AcademicProgramId").select2({
                data: data.items
            });
            if (AcademicProgramId != null && AcademicProgramId != '') {
                $("#AcademicProgramId").val(AcademicProgramId).trigger('change');
            } 
    
        });


        $.ajax({
            type: 'GET',
            url: `/admin/padron-de-registro/universidades-extranjeras/get`.proto().parseURL(),
            success: function (data) {
                $("#OriginPreRequisiteDegreeUniversityId").select2({
                    data: data
                });
            }
        }).done(function () {
            if (ForeignUniversityOriginId != null && ForeignUniversityOriginId !== undefined) {
                $("#OriginPreRequisiteDegreeUniversityId").val(ForeignUniversityOriginId).trigger('change');
            }
        });
    };

    var datepickers = function () {
        $("#ResolutionDateByUniversityCouncil").datepicker();
        $("#GraduationDate").datepicker();
        $("#DuplicateDiplomatDate").datepicker();
        $("#DateEnrollmentProgram").datepicker();
        $("#StartDateEnrollmentProgram").datepicker();
        $("#EndDateEnrollmentProgram").datepicker();
        $("#OriginDiplomatDate").datepicker();
        $("#UniversityCouncilDate").datepicker();
        $("#FacultyCouncilDate").datepicker();
        $("#RegistrationEnd").datepicker();


        $("#SineaceAcreditationStartDate").datepicker();
        $("#SineaceAcreditationEndDate").datepicker();
        $("#SineaceAcreditationDegreeModalityStartDate").datepicker();
        $("#SineaceAcreditationDegreeModalityEndDate").datepicker();
        $("#ProcessDegreeDate").datepicker();
        $("#DegreeSustentationDate").datepicker();


        $("#StartDateEnrollmentProgram").datepicker()
            .on("changeDate", function (e) {
                $(this).valid();

                $("#EndDateEnrollmentProgram").datepicker("setStartDate", e.date);              
            });

        $("#EndDateEnrollmentProgram").datepicker()
            .on("changeDate", function (e) {
                $(this).valid();
                $("#StartDateEnrollmentProgram").datepicker("setEndDate", e.date);
                
            });

    };

    var events = function () {
        $("#update-form").validate({
            rules: {
                FolioCode: "number",
                Credits: "number",
                ResearchWorkURL : "url"                
            },
            submitHandler: function (form, event) {
                $("#update-form input[type='submit']").addLoader();
                $.ajax({
                    type: "POST",
                    url: `/admin/padron-de-registro/editar`.proto().parseURL(),
                    data: $(form).serialize(),
                    success: function () {
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.update);
                        window.location.href = `/admin/padron-de-registro`.proto().parseURL();
                    },
                    error: function () {
                        toastr.error(_app.constants.toastr.message.error.update, _app.constants.toastr.title.error);
                    },
                    complete: function(){
                       $("#update-form input[type='submit']").removeLoader(); 
                    }
                });
            }

        });

    };   
    return {
        load: function () {
            datepickers();       
            events();
            select();
            moreInformation();
        }
    };
}();

$(function () {
    registrypatterns_create.load();
});


