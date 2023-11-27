var degreeConfiguration = function () {

    var TupaBachelorAutomatic = $("#TupaBachelorAutomaticInput").val();
    var TupaBachelorRequested = $("#TupaBachelorRequestedInput").val();
    var TupaTitleDegreeSupportTesis = $("#TupaTitleDegreeSupportTesisInput").val();
    var TupaTitleDegreeProfessionalExperience = $("#TupaTitleDegreeProfessionalExperienceInput").val();
    var TupaTitleDegreeSufficiencyExam = $("#TupaTitleDegreeSufficiencyExamInput").val();

    var ConceptBachelorAutomatic = $("#ConceptBachelorAutomaticInput").val();
    var ConceptBachelorRequested = $("#ConceptBachelorRequestedInput").val();
    var ConceptTitleDegreeSupportTesis = $("#ConceptTitleDegreeSupportTesisInput").val();
    var ConceptTitleDegreeProfessionalExperience = $("#ConceptTitleDegreeProfessionalExperienceInput").val();
    var ConceptTitleDegreeSufficiencyExam = $("#ConceptTitleDegreeSufficiencyExamInput").val();
    
    var selects = {
        tupa: {
            bachelor_automatic: function () {
                $.ajax({
                    type: "GET",
                    url: `/admin/configuracion/obtener-lista-tramites-grados`.proto().parseURL()
                }).done(function (dataFromServer) {
                    $("#TupaBachelorAutomatic").select2({
                        data: dataFromServer
                    });
                    if (TupaBachelorAutomatic !== undefined && TupaBachelorAutomatic !== null) {
                        $("#TupaBachelorAutomatic").val(`${TupaBachelorAutomatic}`).trigger('change');
                    }

                });

            },
            bachelor_requested: function () {
                $.ajax({
                    type: "GET",
                    url: `/admin/configuracion/obtener-lista-tramites-grados`.proto().parseURL()
                }).done(function (dataFromServer) {
                    $("#TupaBachelorRequested").select2({
                        data: dataFromServer
                    });
                    if (TupaBachelorRequested !== undefined && TupaBachelorRequested !== null) {
                        $("#TupaBachelorRequested").val(`${TupaBachelorRequested}`).trigger('change');
                    }

                });

            },
            title_profesional_by_sustentation_tesis: function () {
                $.ajax({
                    type: "GET",
                    url: `/admin/configuracion/obtener-lista-tramites-grados`.proto().parseURL()
                }).done(function (dataFromServer) {
                    $("#TupaTitleDegreeSupportTesis").select2({
                        data: dataFromServer
                    });
                    if (TupaTitleDegreeSupportTesis !== undefined && TupaTitleDegreeSupportTesis !== null) {
                        $("#TupaTitleDegreeSupportTesis").val(`${TupaTitleDegreeSupportTesis}`).trigger('change');
                    }

                });

            },
            title_profesional_by_proffesional_experience: function () {
                $.ajax({
                    type: "GET",
                    url: `/admin/configuracion/obtener-lista-tramites-grados`.proto().parseURL()
                }).done(function (dataFromServer) {
                    $("#TupaTitleDegreeProfessionalExperience").select2({
                        data: dataFromServer
                    });
                    if (TupaTitleDegreeProfessionalExperience !== undefined && TupaTitleDegreeProfessionalExperience !== null) {
                        $("#TupaTitleDegreeProfessionalExperience").val(`${TupaTitleDegreeProfessionalExperience}`).trigger('change');
                    }

                });
            },
            title_profesional_by_sufficiency_exam: function () {
                $.ajax({
                    type: "GET",
                    url: `/admin/configuracion/obtener-lista-tramites-grados`.proto().parseURL()
                }).done(function (dataFromServer) {
                    $("#TupaTitleDegreeSufficiencyExam").select2({
                        data: dataFromServer
                    });
                    if (TupaTitleDegreeSufficiencyExam !== undefined && TupaTitleDegreeSufficiencyExam !== null) {
                        $("#TupaTitleDegreeSufficiencyExam").val(`${TupaTitleDegreeSufficiencyExam}`).trigger('change');
                    }

                });
            }
        },
        concept: {

            bachelor_automatic: function () {
                $.ajax({
                    type: "GET",
                    url: `/admin/configuracion/obtener-lista-conceptos`.proto().parseURL()
                }).done(function (dataFromServer) {
                    $("#ConceptBachelorAutomatic").select2({
                        data: dataFromServer
                    });
                    if (ConceptBachelorAutomatic !== undefined && ConceptBachelorAutomatic !== null) {
                        $("#ConceptBachelorAutomatic").val(`${ConceptBachelorAutomatic}`).trigger('change');
                    }

                });

            },
            bachelor_requested: function () {
                $.ajax({
                    type: "GET",
                    url: `/admin/configuracion/obtener-lista-conceptos`.proto().parseURL()
                }).done(function (dataFromServer) {
                    $("#ConceptBachelorRequested").select2({
                        data: dataFromServer
                    });
                    if (ConceptBachelorRequested !== undefined && ConceptBachelorRequested !== null) {
                        $("#ConceptBachelorRequested").val(`${ConceptBachelorRequested}`).trigger('change');
                    }

                });

            },
            title_profesional_by_sustentation_tesis: function () {
                $.ajax({
                    type: "GET",
                    url: `/admin/configuracion/obtener-lista-conceptos`.proto().parseURL()
                }).done(function (dataFromServer) {
                    $("#ConceptTitleDegreeSupportTesis").select2({
                        data: dataFromServer
                    });
                    if (ConceptTitleDegreeSupportTesis !== undefined && ConceptTitleDegreeSupportTesis !== null) {
                        $("#ConceptTitleDegreeSupportTesis").val(`${ConceptTitleDegreeSupportTesis}`).trigger('change');
                    }

                });

            },
            title_profesional_by_proffesional_experience: function () {
                $.ajax({
                    type: "GET",
                    url: `/admin/configuracion/obtener-lista-conceptos`.proto().parseURL()
                }).done(function (dataFromServer) {
                    $("#ConceptTitleDegreeProfessionalExperience").select2({
                        data: dataFromServer
                    });
                    if (ConceptTitleDegreeProfessionalExperience !== undefined && ConceptTitleDegreeProfessionalExperience !== null) {
                        $("#ConceptTitleDegreeProfessionalExperience").val(`${ConceptTitleDegreeProfessionalExperience}`).trigger('change');
                    }

                });

            },
            title_profesional_by_sufficiency_exam: function () {
                $.ajax({
                    type: "GET",
                    url: `/admin/configuracion/obtener-lista-conceptos`.proto().parseURL()
                }).done(function (dataFromServer) {
                    $("#ConceptTitleDegreeSufficiencyExam").select2({
                        data: dataFromServer
                    });
                    if (ConceptTitleDegreeSufficiencyExam !== undefined && ConceptTitleDegreeSufficiencyExam !== null) {
                        $("#ConceptTitleDegreeSufficiencyExam").val(`${ConceptTitleDegreeSufficiencyExam}`).trigger('change');
                    }

                });

            }
        }        
        
      };

    var form = {
        configuration: {
            init: function () {
                $("#configForm").validate({
                    submitHandler: function (e) {
                        var formData = new FormData(e);

                        swal({
                            title: "Confirmación de cambios",
                            text: "Se actualizarán las variables del sistema. Esto afectará de manera inmediata a las funcionalidades relacionadas.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Confirmar",
                            confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                            cancelButtonText: "Cancelar"
                        }).then(function (result) {
                            if (result.value) {
                                mApp.block("#configForm");
                                $.ajax({
                                    url: $(e).attr("action"),
                                    type: "POST",
                                    data: formData,
                                    contentType: false,
                                    processData: false
                                }).done(function () {
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                }).fail(function (error) {
                                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                }).always(function () {
                                    mApp.unblock("#configForm");
                                });
                            }
                        });
                    }
                });
            }
        }

    };

    return {
        load: function () {
            form.configuration.init();
            if (IsIntegrated) {
                selects.tupa.bachelor_automatic();
                selects.tupa.bachelor_requested();
                selects.tupa.title_profesional_by_sustentation_tesis();
                selects.tupa.title_profesional_by_proffesional_experience();
                selects.tupa.title_profesional_by_sufficiency_exam();
            } else {
                selects.concept.bachelor_automatic();
                selects.concept.bachelor_requested();
                selects.concept.title_profesional_by_sustentation_tesis();
                selects.concept.title_profesional_by_proffesional_experience();
                selects.concept.title_profesional_by_sufficiency_exam();
            }                      
            
        }
    };

}();  

$(function() {
    degreeConfiguration.load();
});

