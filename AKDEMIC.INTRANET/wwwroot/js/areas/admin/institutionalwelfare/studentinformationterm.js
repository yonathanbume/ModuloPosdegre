var InitApp = function () {
    var studentId = $("#StudentId").val();
    var termId = $("#TermId").val();
    var datatable = {
        studentFamily: {
            object: null,
            options: {
                serverSide: false,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/admin/bienestar_institucional/familiar/${studentId}/datatable`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "A.Paterno",
                        data: "paternalName"
                    },
                    {
                        title: "A.Materno",
                        data: "maternalName"
                    },
                    {
                        title: "F.Nacimiento",
                        data: "birthday"
                    },
                    {
                        title: "Parentesco",
                        data: "relationship"
                    },
                    {
                        title: "Estado Civil",
                        data: "civilStatus"
                    },
                    {
                        title: "Grado de instrucción",
                        data: "degreeInstruction"
                    },
                    {
                        title: "Titulado/Maestria",
                        data: "certificated"
                    },
                    {
                        title: "Ocupación",
                        data: "occupation"
                    },
                    {
                        title: "Centro Laboral Y/o estudios",
                        data: "workcenter"
                    },
                    {
                        title: "Localidad",
                        data: "location"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Edit
                            template += `<button type="button" `;
                            template += "class='btn btn-info ";
                            template += "m-btn btn-sm m-btn--icon btn-edit' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-edit'></i><span>Editar</span></span></button> ";
                            //Delete
                            template += `<button type="button" `;
                            template += "class='btn btn-danger btn-delete ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-trash'></i></button>";
                            return template;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            adjustTable: function () {
                if (this.object != null) {
                    $("#studentFamily-datatable").DataTable().columns.adjust();
                }
            },
            init: function () {
                this.object = $("#studentFamily-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#studentFamily-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.studentFamily.edit.show(id);
                });

                $("#studentFamily-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.studentFamily.delete(id);
                });
            }
        },
        studentFamilyHealth: {
            object: null,
            options: {
                serverSide: false,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/admin/bienestar_institucional/familiar-salud/${studentId}/datatable`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "A.Paterno",
                        data: "paternalName"
                    },
                    {
                        title: "A.Materno",
                        data: "maternalName"
                    },
                    {
                        title: "Parentesco",
                        data: "relationship"
                    },
                    {
                        title: "¿Está enfermo?",
                        data: null,
                        render: function (data) {
                            var template = '';
                            if (data.isSick === true) {
                                template += `<span class="m-badge  m-badge--danger m-badge--wide">Si</span>`;
                            }
                            else {
                                template += `<span class="m-badge  m-badge--success m-badge--wide">No</span>`;
                            }
                            return template;
                        }
                    },
                    {
                        title: "Tipo de enfermedad",
                        data: "diseaseType"
                    },
                    {
                        title: "¿Tiene intervenciones quirúrgicas?",
                        data: null,
                        render: function (data) {
                            var template = '';
                            if (data.surgicalIntervention === true) {
                                template += `<span class="m-badge  m-badge--danger m-badge--wide">Si</span>`;
                            }
                            else {
                                template += `<span class="m-badge  m-badge--success m-badge--wide">No</span>`;
                            }
                            return template;
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Edit
                            template += `<button type="button" `;
                            template += "class='btn btn-info ";
                            template += "m-btn btn-sm m-btn--icon btn-edit' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-edit'></i><span>Editar</span></span></button> ";
                            return template;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            adjustTable: function () {
                if (this.object != null) {
                    $("#studentFamily-health-datatable").DataTable().columns.adjust();
                }
            },
            init: function () {
                this.object = $("#studentFamily-health-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#studentFamily-health-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.studentFamilyHealth.edit.show(id);
                });
            }
        },
        init: function () {
            this.studentFamily.init();
            this.studentFamilyHealth.init();
        }
    };

    var modal = {
        studentFamily: {
            create: {
                object: $("#create_student_family").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnAddStudentFamily").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action").proto().parseURL(),
                            type: 'POST',
                            data: $(formElement).serialize(),
                        })
                            .done(function (result) {
                                $("#model_add_studentfamily").modal("hide");
                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                datatable.studentFamily.reload();
                                datatable.studentFamilyHealth.reload();
                                $("#btnAddStudentFamily").removeLoader();
                            })
                            .fail(function (e) {
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                $("#btnAddStudentFamily").removeLoader();
                            });
                    }

                }),
                clear: function () {
                    $("#btnAddStudentFamily").removeLoader();
                    modal.studentFamily.create.object.resetForm();
                    $("#create_student_family select[name='CivilStatusInt']").val(-1).trigger("change");
                    $("#create_student_family select[name='RelationshipInt']").val(-1).trigger("change");
                    $("#create_student_family select[name='DegreeInstructionInt']").val(-1).trigger("change");

                },
                events: function () {
                    $("#model_add_studentfamily").on("hidden.bs.modal", function () {
                        modal.studentFamily.create.clear();
                    });
                },
                init: function () {
                    this.events();
                }
            },
            edit: {
                object: $("#edit_student_family").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEditStudentFamily").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action").proto().parseURL(),
                            type: 'POST',
                            data: $(formElement).serialize(),
                        }).done(function (result) {
                            $("#model_edit_studentfamily").modal("hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.studentFamily.reload();
                            $("#btnEditStudentFamily").removeLoader();
                        }).fail(function (e) {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            $("#btnEditStudentFamily").removeLoader();
                        });
                    }

                }),
                load: function (id) {
                    $.ajax({
                        url: `/admin/bienestar_institucional/familiar/get/${id}`.proto().parseURL(),
                        type: 'GET',
                    }).done(function (data) {
                        $("#edit_student_family input[name='StudentFamilyId']").val(data.id);
                        $("#edit_student_family input[name='Name']").val(data.name);
                        $("#edit_student_family input[name='PaternalName']").val(data.paternalname);
                        $("#edit_student_family input[name='MaternalName']").val(data.maternalname);
                        $("#edit_student_family input[name='Birthday']").val(data.birthday);
                        $("#edit_student_family select[name='RelationshipInt']").val(data.relationship).trigger('change');
                        $("#edit_student_family select[name='CivilStatusInt']").val(data.civilstatus).trigger('change');
                        $("#edit_student_family select[name='DegreeInstructionInt']").val(data.degreeinstruction).trigger('change');;
                        $("#edit_student_family input[name='Certificated']").val(data.certificated);
                        $("#edit_student_family input[name='Occupation']").val(data.occupation);
                        $("#edit_student_family input[name='WorkCenter']").val(data.workcenter);
                        $("#edit_student_family input[name='Location']").val(data.location);
                        $("#model_edit_studentfamily").modal('show');
                    }).fail(function (e) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    });
                },
                show: function (id) {
                    this.load(id);
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El familiar del alumno será eliminado permanentemente",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Si, eliminarlo",
                    cancelButtonText: "Cancelar"
                }).then(function (result) {
                    if (result.value) {
                        $.ajax({
                            url: `/admin/bienestar_institucional/eliminar/familiar/${id}`.proto().parseURL(),
                            type: "POST",
                            success: function () {
                                toastr.success(_app.constants.toastr.message.success.delete, _app.constants.toastr.title.success);
                                datatable.studentFamily.reload();
                                datatable.studentFamilyHealth.reload();
                            },
                            error: function () {
                                toastr.error(_app.constants.toastr.message.error.delete, _app.constants.toastr.title.error);
                            }
                        });
                    }
                });
            },
            init: function () {
                this.create.init();
            }
        },
        studentFamilyHealth: {
            edit: {
                object: $("#edit_student_family_health").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEditStudentFamilyHealth").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action").proto().parseURL(),
                            type: 'POST',
                            data: $(formElement).serialize(),
                        }).done(function (result) {
                            $("#model_edit_studentfamily_health").modal("hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.studentFamilyHealth.reload();
                            $("#btnEditStudentFamilyHealth").removeLoader();
                        }).fail(function (e) {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            $("#btnEditStudentFamilyHealth").removeLoader();
                        });
                    }

                }),
                load: function (id) {
                    $.ajax({
                        url: `/admin/bienestar_institucional/familiar-salud/get/${id}`.proto().parseURL(),
                        type: 'GET',
                    }).done(function (data) {
                        $("#edit_student_family_health input[name='StudentFamilyId']").val(data.id);
                        $("#edit_student_family_health input[name='Name']").val(data.name);
                        $("#edit_student_family_health input[name='PaternalName']").val(data.paternalname);
                        $("#edit_student_family_health input[name='MaternalName']").val(data.maternalname);

                        $("#edit_student_family_health input[name='DiseaseType']").val(data.diseaseType);
                        $("#edit_student_family_health input[name='IsSick']").prop('checked', data.isSick);
                        $("#edit_student_family_health input[name='SurgicalIntervention']").prop('checked', data.surgicalIntervention);

                        $("#model_edit_studentfamily_health").modal('show');
                    }).fail(function (e) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    });
                },
                show: function (id) {
                    this.load(id);
                }
            },
            init: function () {
                //Nothing to init Yet
            }
        },
        init: function () {
            this.studentFamily.init();
            this.studentFamilyHealth.init();
        }
    };

    var sections = {
        formPersonalInformation: {
            validate: function () {
                $("#form_personal_information").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        $("#btnSavePersonalInformation").addLoader();
                        $.ajax({
                            url: $(formElement).attr("action").proto().parseURL(),
                            type: "POST",
                            data: $(formElement).serialize(),
                        })
                            .done(function (result) {
                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                $("#btnSavePersonalInformation").removeLoader();
                            })
                            .fail(function (e) {
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                $("#btnSavePersonalInformation").removeLoader();
                            });
                    }
                });
            },
            load: function () {
                $.ajax({
                    url: `/admin/bienestar_institucional/ficha-estudiante/datos-personales/${studentId}/periodo/${termId}`,
                    type: "GET",
                }).done(function (data) {
                    $("#form_personal_information input[name='UserName']").val(data.userName);
                    $("#form_personal_information input[name='Term']").val(data.termName);
                    $("#form_personal_information input[name='Dni']").val(data.dni);
                    $("#form_personal_information input[name='Name']").val(data.name);
                    $("#form_personal_information input[name='PaternalSurname']").val(data.paternalSurname);
                    $("#form_personal_information input[name='MaternalSurname']").val(data.maternalSurname);
                    $("#form_personal_information input[name='Faculty']").val(data.facultyName);
                    $("#form_personal_information input[name='Career']").val(data.careerName);
                    $("#form_personal_information input[name='CurrentAcademicYear']").val(data.currentAcademicYear);
                    $("#form_personal_information input[name='Birthdate']").val(data.birthDate);
                    $("#form_personal_information input[name='Age']").val(data.age);
                    $("#form_personal_information input[name='Email']").val(data.email);

                    $("#form_personal_information select[name='Sex']").val(data.sex).trigger("change");
                    $("#form_personal_information select[name='CivilStatus']").val(data.civilStatus).trigger("change");

                    $("#form_personal_information input[name='OriginAddress']").val(data.originAddress);
                    $("#form_personal_information input[name='OriginPhoneNumber']").val(data.originPhoneNumber);

                    $("#form_personal_information input[name='CurrentAddress']").val(data.currentAddress);
                    $("#form_personal_information input[name='CurrentPhoneNumber']").val(data.currentPhoneNumber);


                    $("#form_personal_information input[name='FullNameExternalPerson']").val(data.fullNameExternalPerson);
                    $("#form_personal_information input[name='PhoneExternalPerson']").val(data.phoneExternalPerson);
                    $("#form_personal_information input[name='AddressExternalPerson']").val(data.addressExternalPerson);
                    $("#form_personal_information input[name='EmailExternalPerson']").val(data.emailExternalPerson);

                }).fail(function (e) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            },
            init: function () {
                this.validate();
                this.load();
            }
        },
        formAcademicBackground: {
            validate: function () {
                $("#form_academic_background").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        $("#btnAcademicBackground").addLoader();
                        $.ajax({
                            url: $(formElement).attr("action").proto().parseURL(),
                            type: "POST",
                            data: $(formElement).serialize(),
                        })
                            .done(function (result) {
                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                $("#btnAcademicBackground").removeLoader();
                            })
                            .fail(function (e) {
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                $("#btnAcademicBackground").removeLoader();
                            });
                    }
                });
            },
            load: function () {
                $.ajax({
                    url: `/admin/bienestar_institucional/ficha-estudiante/antecedentes-academicos/${studentId}/periodo/${termId}`,
                    type: "GET",
                }).done(function (data) {
                    $("#form_academic_background input[name='OriginSchool']").val(data.originSchool);
                    $("#form_academic_background input[name='OriginSchoolPlace']").val(data.originSchoolPlace);
                    $("#form_academic_background select[name='SchoolType']").val(data.schoolType).trigger("change");
                    $("#form_academic_background select[name='UniversityPreparationId']").val(data.universityPreparationId).trigger("change");

                }).fail(function (e) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            },
            init: function () {
                this.validate();
                this.load();
            }
        },
        formEconomy: {
            validate: function () {
                $("#form_economy").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        $("#btnSaveEconomy").addLoader();
                        $.ajax({
                            url: $(formElement).attr("action").proto().parseURL(),
                            type: "POST",
                            data: $(formElement).serialize(),
                        }).done(function (result) {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnSaveEconomy").removeLoader();
                        }).fail(function (e) {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            $("#btnSaveEconomy").removeLoader();
                        });
                    }
                });
            },
            load: function () {
                $.ajax({
                    url: `/admin/bienestar_institucional/ficha-estudiante/economia/${studentId}/periodo/${termId}`,
                    type: "GET",
                }).done(function (data) {

                    $("#form_economy select[name='PrincipalPerson']").val(data.principalPerson).trigger("change");

                    $("#form_economy select[name='EconomicMethodFatherTutor']").val(data.economicMethodFatherTutor).trigger("change");
                    $("#form_economy select[name='DSectorFatherTutor']").val(data.dSectorFatherTutor).trigger("change");
                    $("#form_economy select[name='DWorkConditionFatherTutor']").val(data.dWorkConditionFatherTutor).trigger("change");
                    $("#form_economy input[name='DEspecificActivityFatherTutor']").val(data.dEspecificActivityFatherTutor);
                    $("#form_economy select[name='DBusyFatherTutor']").val(data.dBusyFatherTutor).trigger("change");

                    $("#form_economy select[name='ISectorFatherTutor']").val(data.iSectorFatherTutor).trigger("change");
                    $("#form_economy select[name='IWorkConditionFatherTutor']").val(data.iWorkConditionFatherTutor).trigger("change");
                    $("#form_economy input[name='IEspecificActivityFatherTutor']").val(data.iEspecificActivityFatherTutor);
                    $("#form_economy select[name='IBusyFatherTutor']").val(data.iBusyFatherTutor).trigger("change");

                    $("#form_economy select[name='EconomicMethodMother']").val(data.economicMethodMother).trigger("change");
                    $("#form_economy select[name='DSectorMother']").val(data.dSectorMother).trigger("change");
                    $("#form_economy select[name='DWorkConditionMother']").val(data.dWorkConditionMother).trigger("change");
                    $("#form_economy input[name='DEspecificActivityMother']").val(data.dEspecificActivityMother);
                    $("#form_economy select[name='DBusyMother']").val(data.dBusyMother).trigger("change");

                    $("#form_economy select[name='ISectorMother']").val(data.iSectorMother).trigger("change");
                    $("#form_economy select[name='IWorkConditionMother']").val(data.iWorkConditionMother).trigger("change");
                    $("#form_economy input[name='IEspecificActivityMother']").val(data.iEspecificActivityMother);
                    $("#form_economy select[name='IBusyMother']").val(data.iBusyMother).trigger("change");
                    

                    $("#form_economy input[name='EconomicExpensesFeeding']").val(data.economicExpensesFeeding);
                    $("#form_economy input[name='EconomicExpensesBasicServices']").val(data.economicExpensesBasicServices);
                    $("#form_economy input[name='EconomicExpensesEducation']").val(data.economicExpensesEducation);
                    $("#form_economy input[name='EconomicExpensesOthers']").val(data.economicExpensesOthers);

                    $("#form_economy input[name='FatherRemuneration']").val(data.fatherRemuneration);
                    $("#form_economy input[name='MotherRemuneration']").val(data.motherRemuneration);
                    $("#form_economy input[name='StudentRemuneration']").val(data.studentRemuneration);
                    $("#form_economy input[name='OtherRemuneration']").val(data.otherRemuneration);
                    $("#form_economy input[name='TotalRemuneration']").val(data.totalRemuneration);

                    $("#form_economy select[name='StudentDependency']").val(data.studentDependency).trigger("change");
                    $("#form_economy select[name='StudentCoexistence']").val(data.studentCoexistence).trigger("change");
                    $("#form_economy select[name='FamilyRisk']").val(data.familyRisk).trigger("change");
                    $("#form_economy select[name='StudentWorkDedication']").val(data.studentWorkDedication).trigger("change");
                    $("#form_economy input[name='StudentWorkDescription']").val(data.studentWorkDescription);
                    $("#form_economy select[name='StudentWorkCondition']").val(data.studentWorkCondition).trigger("change");

                    if (data.authorizeCheck === true) {
                        $("#form_economy input[name='AuthorizedPersonFullName']").val(data.authorizedPersonFullName);
                        $("#form_economy input[name='AuthorizedPersonAddress']").val(data.authorizedPersonAddress);
                        $("#form_economy input[name='AuthorizedPersonPhone']").val(data.authorizedPersonPhone);

                        $("#form_economy input[name='AuthorizeCheck']").prop("checked", true).trigger("change");
                    }


                }).fail(function (e) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            },
            init: function () {
                this.validate();
                this.load();
                $("a[href='#tab3']").on('shown.bs.tab', function () {
                    datatable.studentFamily.adjustTable();
                });
            }
        },
        formHealth: {
            validate: function () {
                $("#form_health").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        $("#btnSaveHealthInformation").addLoader();
                        $.ajax({
                            url: $(formElement).attr("action").proto().parseURL(),
                            type: "POST",
                            data: $(formElement).serialize(),
                        }).done(function (result) {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnSaveHealthInformation").removeLoader();
                        }).fail(function (e) {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            $("#btnSaveHealthInformation").removeLoader();
                        });
                    }
                });
            },
            load: function () {
                $.ajax({
                    url: `/admin/bienestar_institucional/ficha-estudiante/salud/${studentId}/periodo/${termId}`,
                    type: "GET",
                }).done(function (data) {
                    $("#form_health select[name='IsSick']").val(data.isSick).trigger("change");
                    $("#form_health select[name='HaveInsurance']").val(data.haveInsurance).trigger("change");
                    $("#form_health select[name='InsuranceDescription']").val(data.insuranceDescription).trigger("change");
                    $("#form_health input[name='TypeParentIllness']").val(data.typeParentIllness);

                }).fail(function (e) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            },
            init: function () {
                this.validate();
                this.load();
                $("a[href='#tab4']").on('shown.bs.tab', function () {
                    datatable.studentFamilyHealth.adjustTable();
                });
            }
        },
        formFeeding: {
            validate: function () {
                $("#form_feeding").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        $("#btnSaveFeedingInformation").addLoader();
                        $.ajax({
                            url: $(formElement).attr("action").proto().parseURL(),
                            type: "POST",
                            data: $(formElement).serialize(),
                        }).done(function (result) {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnSaveFeedingInformation").removeLoader();
                        }).fail(function (e) {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            $("#btnSaveFeedingInformation").removeLoader();
                        });
                    }
                });
            },
            load: function () {
                $.ajax({
                    url: `/admin/bienestar_institucional/ficha-estudiante/alimentacion/${studentId}/periodo/${termId}`,
                    type: "GET",
                }).done(function (data) {
                    $("#form_feeding input[name='BreakfastHome']").prop("checked", data.breakfastHome).trigger("change");
                    $("#form_feeding input[name='BreakfastPension']").prop("checked", data.breakfastPension).trigger("change");
                    $("#form_feeding input[name='BreakfastRelativeHome']").prop("checked", data.breakfastRelativeHome).trigger("change");
                    $("#form_feeding input[name='BreakfastOther']").prop("checked", data.breakfastOther).trigger("change");

                    $("#form_feeding input[name='LunchHome']").prop("checked", data.lunchHome).trigger("change");
                    $("#form_feeding input[name='LunchPension']").prop("checked", data.lunchPension).trigger("change");
                    $("#form_feeding input[name='LunchRelativeHome']").prop("checked", data.lunchRelativeHome).trigger("change");
                    $("#form_feeding input[name='LunchOther']").prop("checked", data.lunchOther).trigger("change");

                    $("#form_feeding input[name='DinnerHome']").prop("checked", data.dinnerHome).trigger("change");
                    $("#form_feeding input[name='DinnerPension']").prop("checked", data.dinnerPension).trigger("change");
                    $("#form_feeding input[name='DinnerRelativeHome']").prop("checked", data.dinnerRelativeHome).trigger("change");
                    $("#form_feeding input[name='DinnerOther']").prop("checked", data.dinnerOther).trigger("change");

                }).fail(function (e) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            },
            init: function () {
                this.validate();
                this.load();
            }
        },
        formLivingPlace: {
            validate: function () {
                $("#form_living_place").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        $("#btnSaveLivingPlaceInformation").addLoader();
                        $.ajax({
                            url: $(formElement).attr("action").proto().parseURL(),
                            type: "POST",
                            data: $(formElement).serialize(),
                        }).done(function (result) {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnSaveLivingPlaceInformation").removeLoader();
                        }).fail(function (e) {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            $("#btnSaveLivingPlaceInformation").removeLoader();
                        });
                    }
                });
            },
            load: function () {
                $.ajax({
                    url: `/admin/bienestar_institucional/ficha-estudiante/vivienda/${studentId}/periodo/${termId}`,
                    type: "GET",
                }).done(function (data) {
                    $("#form_living_place select[name='Tenure']").val(data.tenure).trigger("change");
                    $("#form_living_place select[name='ContructionType']").val(data.contructionType).trigger("change");
                    $("#form_living_place select[name='ZoneType']").val(data.zoneType).trigger("change");
                    $("#form_living_place select[name='BuildType']").val(data.buildType).trigger("change");
                    $("#form_living_place input[name='OtherTypeLivingPlace']").val(data.otherTypeLivingPlace);

                    $("#form_living_place input[name='NumberFloors']").val(data.numberFloors);
                    $("#form_living_place input[name='NumberRooms']").val(data.numberRooms);
                    $("#form_living_place input[name='NumberKitchen']").val(data.numberKitchen);
                    $("#form_living_place input[name='NumberBathroom']").val(data.numberBathroom);
                    $("#form_living_place input[name='NumberLivingRoom']").val(data.numberLivingRoom);
                    $("#form_living_place input[name='NumberDinningRoom']").val(data.numberDinningRoom);

                    $("#form_living_place input[name='Water']").prop("checked", data.water).trigger("change");
                    $("#form_living_place input[name='Drain']").prop("checked", data.drain).trigger("change");
                    $("#form_living_place input[name='LivingPlacePhone']").prop("checked", data.livingPlacePhone).trigger("change");
                    $("#form_living_place input[name='Light']").prop("checked", data.light).trigger("change");
                    $("#form_living_place input[name='Internet']").prop("checked", data.internet).trigger("change");

                    $("#form_living_place input[name='TV']").prop("checked", data.tV).trigger("change");
                    $("#form_living_place input[name='HasPhone']").prop("checked", data.hasPhone).trigger("change");
                    $("#form_living_place input[name='Radio']").prop("checked", data.radio).trigger("change");
                    $("#form_living_place input[name='Stereo']").prop("checked", data.stereo).trigger("change");
                    $("#form_living_place input[name='Iron']").prop("checked", data.iron).trigger("change");
                    $("#form_living_place input[name='EquipPhone']").prop("checked", data.equipPhone).trigger("change");
                    $("#form_living_place input[name='Laptop']").prop("checked", data.laptop).trigger("change");
                    $("#form_living_place input[name='Closet']").prop("checked", data.closet).trigger("change");
                    $("#form_living_place input[name='Fridge']").prop("checked", data.fridge).trigger("change");
                    $("#form_living_place input[name='PersonalLibrary']").prop("checked", data.personalLibrary).trigger("change");
                    $("#form_living_place input[name='EquipComputer']").prop("checked", data.equipComputer).trigger("change");
                    
                }).fail(function (e) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            },
            init: function () {
                this.validate();
                this.load();
            }
        },
        init: function () {
            this.formPersonalInformation.init();
            this.formAcademicBackground.init();
            this.formEconomy.init();
            this.formHealth.init();
            this.formFeeding.init();
            this.formLivingPlace.init();
        }
    };

    var switchInput = {
        init: function () {
            this.economicPersonAuthorizeCheck.init();
        },
        economicPersonAuthorizeCheck: {
            init: function () {
                $('#form_economy input[name=AuthorizeCheck]').on('change', function () {
                    var isChecked = $(this)[0].checked;
                    if (isChecked === true) {
                        $(".authorize").prop("disabled", false);
                    } else {
                        $(".authorize").prop("disabled", true);
                    }
                });
            }
        }
    };

    var select = {
        init: function () {
            this.sex.init();
            this.civilStatus.init();
            this.relationship.init();
            this.degreeInstruction.init();
            this.departments.init();
            this.provinces.init();
            this.districts.init();
            this.schoolTypes.init();
            this.universityPreparations.init();
            this.economyPrincipalPerson.init();
            this.economyEconomicMethod.init();
            this.economyDSector.init();
            this.economyDWorkCondition.init();
            this.economyBusy.init();
            this.economyISector.init();
            this.economyIWorkCondition.init();
            this.economyStudentDependency.init();
            this.economyStudentCoexistence.init();
            this.economyStudentRisk.init();
            this.economyWorkDedication.init();
            this.economyWorkCondition.init();
            this.healthHasInsurance.init();
            this.healthParentSick.init();
            this.healthTypeInsurance.init();

            this.livingPlaceBuildType.init();
            this.livingPlaceConstructionType.init();
            this.livingPlaceTenure.init();
            this.livingPlaceType.init();
        },
        sex: {
            init: function () {
                $("#form_personal_information select[name='Sex']").select2();
            }
        },
        civilStatus: {
            init: function () {
                $("#form_personal_information select[name='CivilStatus']").select2();
                $("#create_student_family select[name='CivilStatusInt']").select2();
                $("#edit_student_family select[name='CivilStatusInt']").select2();
            }
        },
        relationship: {
            init: function () {
                $("#create_student_family select[name='RelationshipInt']").select2();
                $("#edit_student_family select[name='RelationshipInt']").select2();

            }
        },
        degreeInstruction: {
            init: function () {
                $("#create_student_family select[name='DegreeInstructionInt']").select2();
                $("#edit_student_family select[name='DegreeInstructionInt']").select2();
            }

        },
        departments: {
            origin: {
                init: function () {
                    this.load();
                    this.events();
                },
                load: function () {
                    $.ajax({
                        type: 'GET',
                        url: ('/departamentos/get').proto().parseURL()
                    }).done(function (data) {
                        $("#form_personal_information select[name='OriginDepartmentId']").select2({
                            data: data.items
                        });
                        let originDepartmentIdDefault = $("#OriginDepartmentIdDefault").val();
                        if (!(originDepartmentIdDefault == null || originDepartmentIdDefault == "")) {
                            $("#form_personal_information select[name='OriginDepartmentId']").val(originDepartmentIdDefault).trigger("change", [true]);
                        } else {
                            $("#form_personal_information select[name='OriginDepartmentId']").val(0).trigger("change", [true]);
                        }
                    });
                },
                events: function () {
                    $("#form_personal_information select[name='OriginDepartmentId']").on("change", function (e, state) {
                        let departmentId = $(this).val();
                        select.provinces.origin.load(departmentId, state);
                        select.districts.origin.clear();
                    });
                }
            },
            current: {
                init: function () {
                    this.load();
                    this.events();
                },
                load: function () {
                    $.ajax({
                        type: 'GET',
                        url: ('/departamentos/get').proto().parseURL()
                    }).done(function (data) {
                        $("#form_personal_information select[name='CurrentDepartmentId']").select2({
                            data: data.items
                        });

                        let currentDepartmentIdDefault = $("#CurrentDepartmentIdDefault").val();
                        if (!(currentDepartmentIdDefault == null || currentDepartmentIdDefault == "")) {
                            $("#form_personal_information select[name='CurrentDepartmentId']").val(currentDepartmentIdDefault).trigger("change", [true]);
                        } else {
                            $("#form_personal_information select[name='CurrentDepartmentId']").val(0).trigger("change", [true]);
                        }
                    });
                },
                events: function () {
                    $("#form_personal_information select[name='CurrentDepartmentId']").on("change", function (e, state) {
                        let departmentId = $(this).val();
                        select.provinces.current.load(departmentId, state);
                        select.districts.current.clear();
                    });
                }
            },
            init: function () {
                this.origin.init();
                this.current.init();
            },
        },
        provinces: {
            origin: {
                init: function () {
                    $("#form_personal_information select[name='OriginProvinceId']").select2();
                    this.events();
                },
                clear: function () {
                    $("#form_personal_information select[name='OriginProvinceId']").empty();
                    $("#form_personal_information select[name='OriginProvinceId']").html(`<option value="0" selected disabled>Selecciona una Provincia</option>`);
                },
                load: function (departmentId, state) {
                    this.clear();
                    $.ajax({
                        type: 'GET',
                        url: (`/departamentos/${departmentId}/provincias/get `).proto().parseURL()
                    }).done(function (data) {
                        $("#form_personal_information select[name='OriginProvinceId']").select2({
                            data: data.items
                        });

                        if (typeof state != 'undefined' && state) {
                            let originProvinceIdDefault = $("#OriginProvinceIdDefault").val();
                            if (!(originProvinceIdDefault == null || originProvinceIdDefault == "")) {
                                $("#form_personal_information select[name='OriginProvinceId']").val(originProvinceIdDefault).trigger("change", [true]);
                            } else {
                                $("#form_personal_information select[name='OriginProvinceId']").val(0).trigger("change", [true]);
                            }
                        }
                    });
                },
                events: function () {
                    $("#form_personal_information select[name='OriginProvinceId']").on("change", function (e, state) {
                        let provinceId = $(this).val();
                        select.districts.origin.load(provinceId, state);
                    });
                }
            },
            current: {
                init: function () {
                    $("#form_personal_information select[name='CurrentProvinceId']").select2();
                    this.events();
                },
                clear: function () {
                    $("#form_personal_information select[name='CurrentProvinceId']").empty();
                    $("#form_personal_information select[name='CurrentProvinceId']").html(`<option value="0" selected disabled>Selecciona una Provincia</option>`);
                },
                load: function (departmentId, state) {
                    this.clear();
                    $.ajax({
                        type: 'GET',
                        url: (`/departamentos/${departmentId}/provincias/get `).proto().parseURL()
                    }).done(function (data) {
                        $("#form_personal_information select[name='CurrentProvinceId']").select2({
                            data: data.items
                        });

                        if (typeof state != 'undefined' && state) {
                            let currentProvinceIdDefault = $("#CurrentProvinceIdDefault").val();
                            if (!(currentProvinceIdDefault == null || currentProvinceIdDefault == "")) {
                                $("#form_personal_information select[name='CurrentProvinceId']").val(currentProvinceIdDefault).trigger("change", [true]);
                            } else {
                                $("#form_personal_information select[name='CurrentProvinceId']").val(0).trigger("change", [true]);
                            }
                        }
                    });
                },
                events: function () {
                    $("#form_personal_information select[name='CurrentProvinceId']").on("change", function (e, state) {
                        let provinceId = $(this).val();
                        select.districts.current.load(provinceId, state);
                    });
                }
            },
            init: function () {
                this.origin.init();
                this.current.init();
            },
        },
        districts: {
            origin: {
                init: function () {
                    $("#form_personal_information select[name='OriginDistrictId']").select2();
                },
                clear: function () {
                    $("#form_personal_information select[name='OriginDistrictId']").empty();
                    $("#form_personal_information select[name='OriginDistrictId']").html(`<option value="0" selected disabled>Selecciona un Distrito</option>`);
                },
                load: function (provinceId, state) {
                    this.clear();
                    $.ajax({
                        type: 'GET',
                        url: (`/provincias/${provinceId}/distritos/get`).proto().parseURL()
                    }).done(function (data) {
                        $("#form_personal_information select[name='OriginDistrictId']").select2({
                            data: data.items
                        });

                        if (typeof state != 'undefined' && state) {
                            let originDistrictIdDefault = $("#OriginDistrictIdDefault").val();
                            if (!(originDistrictIdDefault == null || originDistrictIdDefault == "")) {
                                $("#form_personal_information select[name='OriginDistrictId']").val(originDistrictIdDefault).trigger("change");
                            } else {
                                $("#form_personal_information select[name='OriginDistrictId']").val(0).trigger("change");
                            }
                        }
                    });
                },
            },
            current: {
                init: function () {
                    $("#form_personal_information select[name='CurrentDistrictId']").select2();
                },
                clear: function () {
                    $("#form_personal_information select[name='CurrentDistrictId']").empty();
                    $("#form_personal_information select[name='CurrentDistrictId']").html(`<option value="0" selected disabled>Selecciona un Distrito</option>`);
                },
                load: function (provinceId, state) {
                    this.clear();
                    $.ajax({
                        type: 'GET',
                        url: (`/provincias/${provinceId}/distritos/get`).proto().parseURL()
                    }).done(function (data) {
                        $("#form_personal_information select[name='CurrentDistrictId']").select2({
                            data: data.items
                        });

                        if (typeof state != 'undefined' && state) {
                            let currentDistrictIdDefault = $("#CurrentDistrictIdDefault").val();
                            if (!(currentDistrictIdDefault == null || currentDistrictIdDefault == "")) {
                                $("#form_personal_information select[name='CurrentDistrictId']").val(currentDistrictIdDefault).trigger("change");
                            } else {
                                $("#form_personal_information select[name='CurrentDistrictId']").val(0).trigger("change");
                            }
                        }
                    });
                },
            },
            init: function () {
                this.origin.init();
                this.current.init();
            },
        },
        schoolTypes: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/tipodeescuela/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_academic_background select[name='SchoolType']").select2({
                        data: data
                    });
                });
            }
        },
        universityPreparations: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/preparacion-universitaria/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_academic_background select[name='UniversityPreparationId']").select2({
                        data: data
                    });
                });
            }
        },
        economyPrincipalPerson: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/sostiene-hogar/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_economy select[name='PrincipalPerson']").select2({
                        data: data
                    });
                });
            },
            events: function () {
                $("#form_economy select[name='PrincipalPerson']").on('change', function () {
                    var currentValue = $(this).val();
                    currentValue = parseInt(currentValue, 10);
                    if (currentValue === 0) {
                        $("#IsMother").show();
                        $("#IsFather").show();
                    } else if (currentValue === 1) {
                        $("#IsMother").hide();
                        $("#IsFather").show();
                    } else if (currentValue === 2) {
                        $("#IsFather").hide();
                        $("#IsMother").show();
                    } else {
                        $("#IsMother").hide();
                        $("#IsFather").show();
                    }
                });
            }
        },
        economyEconomicMethod: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/metodo-economico/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_economy select[name='EconomicMethodFatherTutor']").select2({
                        data: data
                    });

                    $("#form_economy select[name='EconomicMethodMother']").select2({
                        data: data
                    });
                });
            }
        },
        economyDSector: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/d-sector-economico/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_economy select[name='DSectorFatherTutor']").select2({
                        data: data
                    });

                    $("#form_economy select[name='DSectorMother']").select2({
                        data: data
                    });
                });
            }
        },
        economyDWorkCondition: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/d-condicion-laboral/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_economy select[name='DWorkConditionFatherTutor']").select2({
                        data: data
                    });

                    $("#form_economy select[name='DWorkConditionMother']").select2({
                        data: data
                    });
                });
            }
        },
        economyBusy: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/desocupado/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_economy select[name='DBusyFatherTutor']").select2({
                        data: data
                    });

                    $("#form_economy select[name='DBusyMother']").select2({
                        data: data
                    });
                });
            }
        },
        economyISector: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/i-sector-economico/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_economy select[name='ISectorFatherTutor']").select2({
                        data: data
                    });

                    $("#form_economy select[name='ISectorMother']").select2({
                        data: data
                    });
                });
            }
        },
        economyIWorkCondition: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/i-condicion-laboral/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_economy select[name='IWorkConditionFatherTutor']").select2({
                        data: data
                    });

                    $("#form_economy select[name='IWorkConditionMother']").select2({
                        data: data
                    });
                });
            }
        },
        economyStudentDependency: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/dependencia-economica/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_economy select[name='StudentDependency']").select2({
                        data: data
                    });
                });
            }
        },
        economyStudentCoexistence: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/vivienda/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_economy select[name='StudentCoexistence']").select2({
                        data: data
                    });
                });
            }
        },
        economyStudentRisk: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/riesgo-familiar/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_economy select[name='FamilyRisk']").select2({
                        data: data
                    });
                });
            }
        },
        economyWorkDedication: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/estudiante-dedicacion-laboral/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_economy select[name='StudentWorkDedication']").select2({
                        data: data
                    });
                });
            }
        },
        economyWorkCondition: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/estudiante-condicion-laboral/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_economy select[name='StudentWorkCondition']").select2({
                        data: data
                    });
                });
            }
        },
        healthParentSick: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/salud-familiar-enfermo/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_health select[name='IsSick']").select2({
                        data: data
                    });
                });
            }
        },
        healthHasInsurance: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/salud-tiene-seguro/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_health select[name='HaveInsurance']").select2({
                        data: data
                    });
                });
            }
        },
        healthTypeInsurance: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/salud-tipo-de-seguro/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_health select[name='InsuranceDescription']").select2({
                        data: data
                    });
                });
            }
        },
        healthTypeInsurance: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    type: 'GET',
                    url: ('/salud-tipo-de-seguro/get').proto().parseURL()
                }).done(function (data) {
                    $("#form_health select[name='InsuranceDescription']").select2({
                        data: data
                    });
                });
            }
        },
        livingPlaceTenure: {
            init: function () {
                $("#form_living_place select[name='Tenure']").select2();
            }
        },
        livingPlaceConstructionType: {
            init: function () {
                $("#form_living_place select[name='ContructionType']").select2();
            }
        },
        livingPlaceType: {
            init: function () {
                $("#form_living_place select[name='ZoneType']").select2();
            }
        },
        livingPlaceBuildType: {
            init: function () {
                $("#form_living_place select[name='BuildType']").select2();
            }
        }

    };

    var datePickers = {
        init: function () {
            $(".Birthday").datepicker({
                format: _app.constants.formats.datepicker
            });
        }
    };

    return {
        init: function () {
            sections.init();
            select.init();
            switchInput.init();
            datatable.init();
            modal.init();
            datePickers.init();
        }
    };
}();

$(function () {
    InitApp.init();
});