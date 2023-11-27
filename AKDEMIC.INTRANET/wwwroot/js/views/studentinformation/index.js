var student_information = (function () {
    var IdStudentInformation = $("#Id").val();
    var UserNameParameter = $("#UserNameParameter").val();

    var datatable = {
        studentFamily: {
            object: null,
            options: {
                serverSide: false,
                filter: false,
                lengthChange: false,
                responsive: true,
                ajax: {
                    url: "/ficha-socioeconomica/composicion-familiar/estudiante/datatable".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {

                    }
                },
                pageLength: 15,
                orderable: [],
                columns: [
                    {
                        title: 'Nombre',
                        data: 'name'
                    },
                    {
                        title: 'Apellido Paterno',
                        data: 'paternalname'
                    },
                    {
                        title: 'Apellido Materno',
                        data: 'maternalname'
                    },
                    {
                        title: 'F.Nacimiento',
                        data: 'birthday'
                    },
                    {
                        title: 'Parentesco',
                        data: 'relationship'
                    },
                    {
                        title: 'Estado Civil',
                        data: 'civilstatus'
                    },
                    {
                        title: 'Grado de instrucción',
                        data: 'degreeinstruction'
                    },
                    {
                        title: 'Titulado/Maestria',
                        data: 'certificated'
                    },
                    {
                        title: 'Ocupación',
                        data: 'occupation'
                    },
                    {
                        title: 'Centro Laboral Y/o estudios',
                        data: 'workcenter'
                    },
                    {
                        title: 'Localidad',
                        data: 'location'
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Edit
                            template += "<button ";
                            template += "class='btn btn-info ";
                            template += "m-btn btn-sm m-btn--icon btn-edit' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-edit'></i><span>Editar</span></span></button> ";
                            //Delete
                            template += "<button ";
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
                    $("#data-table-studentFamily").DataTable().columns.adjust();
                }
            },
            init: function () {
                this.object = $("#data-table-studentFamily").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#data-table-studentFamily").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    $.ajax({
                        type: 'GET',
                        url: `/ficha-socioeconomica/studentfamily/get/${id}`.proto().parseURL(),
                        success: function (data) {
                            $("#edit_student_family input[name='Name']").val(data.name);
                            $("#edit_student_family input[name='PaternalName']").val(data.paternalname);
                            $("#edit_student_family input[name='MaternalName']").val(data.maternalname);
                            $("#edit_student_family input[name='Birthday']").val(data.birthday);
                            $("#edit_student_family select[name='RelationshipInt']").val(data.relationship).trigger('change');
                            $("#edit_student_family select[name='CivilStatusInt']").val(data.civilstatus).trigger('change');
                            $("#edit_student_family select[name='DegreeInstructionInt']").val(data.degreeinstruction).trigger('change');
                            $("#edit_student_family input[name='Certificated']").val(data.certificated);
                            $("#edit_student_family input[name='Occupation']").val(data.occupation);
                            $("#edit_student_family input[name='WorkCenter']").val(data.workcenter);
                            $("#edit_student_family input[name='Location']").val(data.location);
                        }
                    });
                    $("#model_edit_studentfamily").modal('show');
                });

                $("#data-table-studentFamily").on('click', '.btn-delete', function () {
                    let id = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "El familiar del estudiante será eliminado permanentemente",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Si, eliminarlo",
                        cancelButtonText: "Cancelar"
                    }).then(function (result) {
                        if (result.value) {
                            $.ajax({
                                url: `/ficha-socioeconomica/eliminar/studentFamily/${id}`.proto().parseURL(),
                                type: "POST",
                                success: function () {
                                    toastr.success(_app.constants.toastr.message.success.delete, _app.constants.toastr.title.success);
                                    datatable.studentFamily.reload();
                                    datatable.familyHealth.reload();
                                },
                                error: function () {
                                    toastr.error(_app.constants.toastr.message.error.delete, _app.constants.toastr.title.error);
                                }
                            });
                        }
                    });
                });
            }
        },
        familyHealth: {
            object: null,
            options: {
                serverSide: false,
                filter: false,
                lengthChange: false,
                responsive: true,
                ajax: {
                    url: "/ficha-socioeconomica/composicion-familiar/estudiante/datatable".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {

                    }
                },
                pageLength: 15,
                orderable: [],
                columns: [                  
                    {
                        title: 'Nombre',
                        data: 'name'
                    },
                    {
                        title: 'Apellido Paterno',
                        data: 'paternalname'
                    },
                    {
                        title: 'Apellido Materno',
                        data: 'maternalname'
                    },
                    {
                        title: 'Parentesco',
                        data: 'relationship'
                    },
                    {
                        title: "¿Está enfermo?",
                        data: null,
                        orderable: false,
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
                        title: 'Tipo de Enfermedad',
                        data: 'diseaseType'
                    },
                    {
                        title: "¿Tiene intervenciones quirúrgicas?",
                        data: null,
                        orderable: false,
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
                            template += `<button data-id="${data.id}" type="button" class="btn btn-primary btn-sm m-btn m-btn--icon btn-edit-health"<span><i class="la la-edit"> </i> </span> Editar </span></span></button>`;
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
                    $("#data-table-familyHealth").DataTable().columns.adjust();
                }
            },
            init: function () {
                this.object = $("#data-table-familyHealth").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#data-table-familyHealth").on('click', '.btn-edit-health', function () {
                    var id = $(this).data("id");
                    $.ajax({
                        type: 'GET',
                        url: `/ficha-socioeconomica/studentfamily/get/${id}`.proto().parseURL(),
                        success: function (data) {
                            $("#edit_student_family_health input[name='Id']").val(data.id);
                            $("#edit_student_family_health input[name='Name']").val(data.name);
                            $("#edit_student_family_health input[name='PaternalName']").val(data.paternalname);
                            $("#edit_student_family_health input[name='MaternalName']").val(data.maternalname);

                            $("#edit_student_family_health input[name='IsSick2']").prop('checked', data.isSick);
                            if (data.isSick) {
                                $("#edit_student_family_health input[name='DiseaseType']").val(data.diseaseType);
                                $("#edit_student_family_health input[name='DiseaseType']").prop("disabled", false);
                            } else {
                                $("#edit_student_family_health input[name='DiseaseType']").val(data.diseaseType);
                                $("#edit_student_family_health input[name='DiseaseType']").prop("disabled", true);
                            }

                            $("#edit_student_family_health input[name='SurgicalIntervention2']").prop('checked', data.surgicalIntervention);

                        }
                    });
                    $("#model_edit_studentfamily_health").modal('show');
                });                
            }
        },
        init: function () {
            this.studentFamily.init();
            this.familyHealth.init();
        }
    };


    var record = function () {
        mApp.block("#questions", {
            message: "Cargando Evaluación.."
        });

        $.ajax({
            url: `/ficha-socioeconomica/obtener-ficha-evaluacion`,
            type: "GET",
            data: {
                username: UserNameParameter
            }
        })
            .done(function (data) {
                $("#questions").html(data);
            })
            .fail(function () {
                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
            })
            .always(function () {
                $(".selection-option").select2({
                    placeholder: "Selecciona una opción",
                    minimumResultsForSearch: -1,
                    allowClear: true

                });
                $(".selection-option").val(null).trigger("change");
                mApp.unblock("#questions");
                $("#send_requeriments").show();
            });
    };
       
    var modal = function myfunction() {
        $("#btn-view-studentfamily-modal").on('click', function () {
            $("#model_add_studentfamily").modal('show');
            document.getElementById("create_student_family").reset();

        });

    };
    var addStudentFamily = function () {
        $("#create_student_family").validate({
            submitHandler: function (form, event) {
                event.preventDefault();
                var btn = $(form).find(':submit');
                btn.addLoader();
                $.ajax({
                    type: 'POST',
                    url: ("/ficha-socioeconomica/agregar/studentFamily").proto().parseURL(),
                    data: $(form).serialize(),
                    success: function () {
                        btn.removeLoader();
                        $("#model_add_studentfamily").modal("hide");
                        toastr.success(_app.constants.toastr.message.success.create, _app.constants.toastr.title.success);
                        datatable.studentFamily.reload();
                        datatable.familyHealth.reload();
                    },
                    error: function () {
                        btn.removeLoader();
                        toastr.error(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                    }
                });
            }

        });

    };

    var editStudentFamily = function () {
        $("#edit_student_family").validate({
            submitHandler: function (form, event) {
                event.preventDefault();
                var btn = $(form).find(':submit');
                btn.addLoader();
                $.ajax({
                    type: 'POST',
                    url: ("/ficha-socioeconomica/editar/studentFamily").proto().parseURL(),
                    data: $(form).serialize(),
                    success: function () {
                        $("#model_edit_studentfamily").modal("hide");
                        toastr.success(_app.constants.toastr.message.success.update, _app.constants.toastr.title.success);
                        datatable.studentFamily.reload();
                        datatable.familyHealth.reload();
                    },
                    error: function () {
                        toastr.error(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                    }
                });
            }

        });
    };

    var editStudentFamilyHealth = function () {
        $("#edit_student_family_health").validate({
            submitHandler: function (form, event) {
                event.preventDefault();
                var btn = $(form).find(':submit');
                btn.addLoader();
                $.ajax({
                    type: 'POST',
                    url: ("/ficha-socioeconomica/editar/studentFamilyHealth").proto().parseURL(),
                    data: $(form).serialize(),
                    success: function (data) {
                        btn.removeLoader();
                        $("#model_edit_studentfamily_health").modal("hide");
                        toastr.success(_app.constants.toastr.message.success.update, _app.constants.toastr.title.success);
                        datatable.familyHealth.reload();
                    },
                    error: function () {
                        btn.removeLoader();
                        toastr.error(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                    }
                });
            }

        });
    };

    var getDeparmentsOrigin = function () {
        $.ajax({
            type: 'GET',
            url: ('/departamentos/get').proto().parseURL(),
            dataType: 'JSON'
        }).done(function (data) {
            $("#originDepartmentId").select2({
                data: data.items
            });
            if (originDepartmentId !== null) {
                $("#originDepartmentId").val(originDepartmentId).trigger('change');
                originDepartmentId = null;
            }
        });

    };
    var getProvincesOrigin = function () {
        $("#originDepartmentId").on('change', function () {
            $(this).valid();
            var Id = $(this).val();
            $.ajax({
                type: 'GET',
                url: ('/departamentos/' + Id + '/provincias/get').proto().parseURL()
            }).done(function (data) {
                $("#originProvinceId").empty();
                $("#originProvinceId").select2({
                    data: data.items
                });
                if (originProvinceId !== null) {
                    $("#originProvinceId").val(originProvinceId).trigger('change');
                    originProvinceId = null;
                } else {
                    $("#originProvinceId").val($("#originProvinceId").val()).trigger('change');
                }
             
            });

        });
    };
    var getDistrictsOrigin = function () {
        $("#originProvinceId").on('change', function () {
            $(this).valid();
            var Id = $(this).val();
            var IdDepartamentOrigin = $("#originDepartmentId").val();
            $.ajax({
                type: 'GET',
                url: ('/departamentos/' + IdDepartamentOrigin + '/provincias/' + Id + '/distritos/get').proto().parseURL()
            }).done(function (data) {
                $("#originDistrictId").empty();
                $("#originDistrictId").select2({
                    data: data.items
                });
                if (originDistrictId !== null) {
                    $("#originDistrictId").val(originDistrictId).trigger('change');
                    originDistrictId = null;
                }
                $("#originDistrictId").valid(); 
            });
        });
    };

    var getDeparmentsCurrent = function () {
        $.ajax({
            type: 'GET',
            url: ('/departamentos/get').proto().parseURL()
        }).done(function (data) {
            $("#currentDepartmentId").select2({
                data: data.items
            });
            if (currentDepartmentId !== null) {
                $("#currentDepartmentId").val(currentDepartmentId).trigger('change');
                currentDepartmentId = null;
            }
        });

    };
    var getProvincesCurrent = function () {
        $("#currentDepartmentId").on('change', function () {
            $(this).valid();
            var Id = $(this).val();
            $.ajax({
                type: 'GET',
                url: ('/departamentos/' + Id + '/provincias/get').proto().parseURL()
            }).done(function (data) {
                $("#currentProvinceId").empty();
                $("#currentProvinceId").select2({
                    data: data.items
                });
                if (currentProvinceId !== null) {
                    $("#currentProvinceId").val(currentProvinceId).trigger('change');
                    currentProvinceId = null;
                }
                else {
                    $("#currentProvinceId").val($("#currentProvinceId").val()).trigger('change');
                }
                //$('#m_form').valid();
            });
        });
    };
    var getDistrictsCurrent = function () {
        $("#currentProvinceId").on('change', function () {
            $(this).valid();
            var Id = $(this).val();
            var IdDepartamentoCurrent = $("#currentDepartmentId").val();
            $.ajax({
                type: 'GET',
                url: ('/departamentos/' + IdDepartamentoCurrent + '/provincias/' + Id + '/distritos/get').proto().parseURL()
            }).done(function (data) {
                $("#currentDistrictId").empty();
                $("#currentDistrictId").select2({
                    data: data.items
                });
                if (currentDistrictId !== null) {
                    $("#currentDistrictId").val(currentDistrictId).trigger('change');
                    currentDistrictId = null;
                }
                $("#currentDistrictId").valid(); 
            });

        });
    };


    var getPlaceOriginDeparments = function () {
        $.ajax({
            type: 'GET',
            url: ('/departamentos/get').proto().parseURL()
        }).done(function (data) {
            $("#placeOriginDepartmentId").select2({
                data: data.items
            });
            if (placeOriginDepartmentId !== null) {
                $("#placeOriginDepartmentId").val(placeOriginDepartmentId).trigger('change');
                placeOriginDepartmentId = null;
            }
        });

    };
    var getPlaceOriginProvinces = function () {
        $("#placeOriginDepartmentId").on('change', function () {
            $(this).valid();
            var Id = $(this).val();
            $.ajax({
                type: 'GET',
                url: ('/departamentos/' + Id + '/provincias/get').proto().parseURL()
            }).done(function (data) {
                $("#placeOriginProvinceId").empty();
                $("#placeOriginProvinceId").select2({
                    data: data.items
                });
                if (placeOriginProvinceId !== null) {
                    $("#placeOriginProvinceId").val(placeOriginProvinceId).trigger('change');
                    placeOriginProvinceId = null;
                }
                else {
                    $("#placeOriginProvinceId").val($("#placeOriginProvinceId").val()).trigger('change');
                }
                //$('#m_form').valid();
            });
        });
    };
    var getPlaceOriginDistricts = function () {
        $("#placeOriginProvinceId").on('change', function () {
            $(this).valid();
            var Id = $(this).val();
            var placeOriginDepartmentId = $("#placeOriginDepartmentId").val();
            $.ajax({
                type: 'GET',
                url: ('/departamentos/' + placeOriginDepartmentId + '/provincias/' + Id + '/distritos/get').proto().parseURL()
            }).done(function (data) {
                $("#placeOriginDistrictId").empty();
                $("#placeOriginDistrictId").select2({
                    data: data.items
                });
                if (placeOriginDistrictId !== null) {
                    $("#placeOriginDistrictId").val(placeOriginDistrictId).trigger('change');
                    placeOriginDistrictId = null;
                }
                $("#placeOriginDistrictId").valid();
                //$('#m_form').valid();
            });

        });
    };



    var InitSelect2Origin = function () {
        $.when(getDeparmentsOrigin()).done(function () {
            getProvincesOrigin();
            getDistrictsOrigin();
        });
    };

    var InitSelect2Current = function () {
        $.when(getDeparmentsCurrent()).done(function () {
            getProvincesCurrent();
            getDistrictsCurrent();
        });
    };

    var InitSelect2PlaceOrigin = function () {
        $.when(getPlaceOriginDeparments()).done(function () {
            getPlaceOriginProvinces();
            getPlaceOriginDistricts();
        });
    };

    var Disables = function () {


        $("#IsSick").on('change', function () {
            if (parseInt($(this).val(), 10) === 0) {
                $("#TypeParentIllness").prop("disabled", true);
            }
            else {
                $("#TypeParentIllness").prop("disabled", false);
            }

        });

        $("#HaveInsurance").on('change', function () {
            if (parseInt($(this).val(), 10) === 0) {
                $("#InsuranceDescription ").prop("disabled", true);
            }
            else {
                $("#InsuranceDescription ").prop("disabled", false);
            }

        });

        $("#TypeInsurance").on('change', function () {
            if ($(this).val() === 4) {
                $("#OtherInsurance").prop("disabled", false);

            }
            else {
                $("#OtherInsurance").prop("disabled", true);
            }
        });

        $(document).on('change', '.custom-file-input', function (event) {
            //            $("#DocumentFiles").valid();
            $(this).next('.custom-file-label').html(event.target.files[0].name);
        });  


    }
    var datepicker = function () {
        $("#Birthday-datepicker").datepicker();
    };

    var extensions = function () {
        $("#form_datos_personales input[name='PhoneNumber']").keypress(function (event) {
            if (event.which === 46 || (event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
        }).on('paste', function (event) {
            event.preventDefault();
        });
        $("#form_datos_personales input[name='PhoneNumber']").attr("maxlength", "9");

        $("#BirthDate").datepicker().on("change", function () {
            var currentDate = new Date().getUTCFullYear();
            var birthday = $("#BirthDate").val();
            var birthdayYear = parseInt(birthday.substr(birthday.length - 4), 10);
            if (!isNaN(currentDate - birthdayYear)) {
                $("#Age").val((currentDate - birthdayYear).toString());
            } else {
                $("#Age").val("");
            }
        });

        $("#BirthDate").doneTyping(function () {
            var currentDate = new Date().getUTCFullYear();
            var birthday = $("#BirthDate").val();
            var birthdayYear = parseInt(birthday.substr(birthday.length - 4), 10);
            if (!isNaN(currentDate - birthdayYear)) {
                $("#Age").val((currentDate - birthdayYear).toString());
            } else {
                $("#Age").val("");
            }

        });
        //$("#currentDepartmentId").on('change', function () {
        //    var current = $(this).val();
        //    if (current !== null || current !== undefined) {
        //        $(this).attr('aria-invalid', 'false');
        //        $(this).parentElement.removeClass("has-danger");
        //    }

        //});
        //$('#currentDistrictId').on('change', function () {
        //    $("#m_form").valid();
        //});

        //$('#originDistrictId').on('change', function () {
        //    $("#m_form").valid();
        //});        


        $("#PrincipalPerson").on('change', function () {
            var currentValue = $(this).val();
            currentValue = parseInt(currentValue, 10);
            if (currentValue === 0) {
                //$("#FatherMother").hide();
                $("#IsMother").show();
                $("#IsFather").show();
            } else if (currentValue === 1) {
                //$("#FatherMother").show();
                $("#IsMother").hide();
                $("#IsFather").show();
            } else if (currentValue === 2) {
                //$("#FatherMother").show();
                $("#IsFather").hide();
                $("#IsMother").show();
            } else {
                //$("#FatherMother").show();
                $("#IsMother").hide();
                $("#IsFather").show();
            }
        });

        $('.amt').keyup(function () {
            var importe_total = 0;
            $(".amt").each(
                function (index, value) {
                    if ($.isNumeric($(this).val())) {
                        importe_total = importe_total + eval($(this).val());
                    }
                }
            );
            $("#TotalRemuneration").val(importe_total);

        });

        $('input[id=AuthorizeCheck]').on('change', function () {
            var currentValue = $(this)[0].checked;
            if (currentValue === true) {
                $(".authorize").prop("disabled", false);
            } else {
                $(".authorize").prop("disabled", true);
            }
        });

        $("#DSectorFatherTutor").on('change', function () {
            if (parseInt($(this).val(), 10) !== 0 || parseInt($("#DWorkConditionFatherTutor").val(), 10) !== 0 || parseInt($("#DBusyFatherTutor").val(), 10) !== 0 || $("#DEspecificActivityFatherTutor").val() !== "") {
                $("#ISectorFatherTutor").prop("disabled", true);
                $("#IWorkConditionFatherTutor").prop("disabled", true);
                $("#IEspecificActivityFatherTutor").prop("disabled", true);
            } else {
                $("#ISectorFatherTutor").prop("disabled", false);
                $("#IWorkConditionFatherTutor").prop("disabled", false);
                $("#IEspecificActivityFatherTutor").prop("disabled", false);
            }

        });

        $("#DWorkConditionFatherTutor").on('change', function () {
            if (parseInt($(this).val(), 10) !== 0 || parseInt($("#DSectorFatherTutor").val(), 10) !== 0 || parseInt($("#DBusyFatherTutor").val(), 10) || 0 || $("#DEspecificActivityFatherTutor").val() !== "") {
                $("#ISectorFatherTutor").prop("disabled", true);
                $("#IWorkConditionFatherTutor").prop("disabled", true);
                $("#IEspecificActivityFatherTutor").prop("disabled", true);
            } else {
                $("#ISectorFatherTutor").prop("disabled", false);
                $("#IWorkConditionFatherTutor").prop("disabled", false);
                $("#IEspecificActivityFatherTutor").prop("disabled", false);
            }
        });

        $("#DBusyFatherTutor").on('change', function () {
            if (parseInt($(this).val(), 10) !== 0 || parseInt($("#DSectorFatherTutor").val(), 10) !== 0 && parseInt($("#DWorkConditionFatherTutor").val(), 10) !== 0 || $("#DEspecificActivityFatherTutor").val() !== "") {
                $("#ISectorFatherTutor").prop("disabled", true);
                $("#IWorkConditionFatherTutor").prop("disabled", true);
                $("#IEspecificActivityFatherTutor").prop("disabled", true);
            } else {
                $("#ISectorFatherTutor").prop("disabled", false);
                $("#IWorkConditionFatherTutor").prop("disabled", false);
                $("#IEspecificActivityFatherTutor").prop("disabled", false);
            }
        });

        $("#DEspecificActivityFatherTutor").on('keyup', function () {
            if ($(this).val() !== "" || parseInt($("#DSectorFatherTutor").val(), 10) !== 0 || parseInt($("#DWorkConditionFatherTutor").val(), 10) !== 0 || parseInt($("#DBusyFatherTutor").val(), 10) !== 0) {
                $("#ISectorFatherTutor").prop("disabled", true);
                $("#IWorkConditionFatherTutor").prop("disabled", true);
                $("#IEspecificActivityFatherTutor").prop("disabled", true);
            } else {
                $("#ISectorFatherTutor").prop("disabled", false);
                $("#IWorkConditionFatherTutor").prop("disabled", false);
                $("#IEspecificActivityFatherTutor").prop("disabled", false);
            }
        });

        $("#ISectorFatherTutor").on('change', function () {
            if (parseInt($(this).val(), 10) !== 0 || parseInt($("#IWorkConditionFatherTutor").val(), 10) !== 0 || $("#IEspecificActivityFatherTutor").val() !== "") {
                $("#DSectorFatherTutor").prop("disabled", true);
                $("#DWorkConditionFatherTutor").prop("disabled", true);
                $("#DBusyFatherTutor").prop("disabled", true);
                $("#DEspecificActivityFatherTutor").prop("disabled", true);

            } else {
                $("#DSectorFatherTutor").prop("disabled", false);
                $("#DWorkConditionFatherTutor").prop("disabled", false);
                $("#DBusyFatherTutor").prop("disabled", false);
                $("#DEspecificActivityFatherTutor").prop("disabled", false);
            }

        });


        $("#IWorkConditionFatherTutor").on('change', function () {
            if (parseInt($(this).val(), 10) !== 0 || parseInt($("#ISectorFatherTutor").val(), 10) !== 0 || $("#IEspecificActivityFatherTutor").val() !== "") {
                $("#DSectorFatherTutor").prop("disabled", true);
                $("#DWorkConditionFatherTutor").prop("disabled", true);
                $("#DBusyFatherTutor").prop("disabled", true);
                $("#DEspecificActivityFatherTutor").prop("disabled", true);

            } else {
                $("#DSectorFatherTutor").prop("disabled", false);
                $("#DWorkConditionFatherTutor").prop("disabled", false);
                $("#DBusyFatherTutor").prop("disabled", false);
                $("#DEspecificActivityFatherTutor").prop("disabled", false);
            }

        });

        $("#IEspecificActivityFatherTutor").on('keyup', function () {
            if ($(this).val() !== "" || parseInt($("#ISectorFatherTutor").val(), 10) !== 0 || parseInt($("#IWorkConditionFatherTutor").val(), 10) !== 0) {
                $("#DSectorFatherTutor").prop("disabled", true);
                $("#DWorkConditionFatherTutor").prop("disabled", true);
                $("#DBusyFatherTutor").prop("disabled", true);
                $("#DEspecificActivityFatherTutor").prop("disabled", true);
            } else {
                $("#DSectorFatherTutor").prop("disabled", true);
                $("#DWorkConditionFatherTutor").prop("disabled", true);
                $("#DBusyFatherTutor").prop("disabled", true);
                $("#DEspecificActivityFatherTutor").prop("disabled", true);
            }
        });






        $("#DSectorMother").on('change', function () {
            if (parseInt($(this).val(), 10) !== 0 || parseInt($("#DWorkConditionMother").val(), 10) !== 0 || parseInt($("#DBusyMother").val(), 10) !== 0 || $("#DEspecificActivityMother").val() !== "") {
                $("#ISectorMother").prop("disabled", true);
                $("#IWorkConditionMother").prop("disabled", true);
                $("#IEspecificActivityMother").prop("disabled", true);
            } else {
                $("#ISectorMother").prop("disabled", false);
                $("#IWorkConditionMother").prop("disabled", false);
                $("#IEspecificActivityMother").prop("disabled", false);
            }

        });

        $("#DWorkConditionMother").on('change', function () {
            if (parseInt($(this).val(), 10) !== 0 || parseInt($("#DSectorMother").val(), 10) !== 0 || parseInt($("#DBusyMother").val(), 10) || 0 || $("#DEspecificActivityMother").val() !== "") {
                $("#ISectorMother").prop("disabled", true);
                $("#IWorkConditionMother").prop("disabled", true);
                $("#IEspecificActivityMother").prop("disabled", true);
            } else {
                $("#ISectorMother").prop("disabled", false);
                $("#IWorkConditionMother").prop("disabled", false);
                $("#IEspecificActivityMother").prop("disabled", false);
            }
        });

        $("#DBusyMother").on('change', function () {
            if (parseInt($(this).val(), 10) !== 0 || parseInt($("#DSectorMother").val(), 10) !== 0 && parseInt($("#DWorkConditionMother").val(), 10) !== 0 || $("#DEspecificActivityMother").val() !== "") {
                $("#ISectorMother").prop("disabled", true);
                $("#IWorkConditionMother").prop("disabled", true);
                $("#IEspecificActivityMother").prop("disabled", true);
            } else {
                $("#ISectorMother").prop("disabled", false);
                $("#IWorkConditionMother").prop("disabled", false);
                $("#IEspecificActivityMother").prop("disabled", false);
            }
        });

        $("#DEspecificActivityMother").on('keyup', function () {
            if ($(this).val() !== "" || parseInt($("#DSectorMother").val(), 10) !== 0 || parseInt($("#DWorkConditionMother").val(), 10) !== 0 || parseInt($("#DBusyMother").val(), 10) !== 0) {
                $("#ISectorMother").prop("disabled", true);
                $("#IWorkConditionMother").prop("disabled", true);
                $("#IEspecificActivityMother").prop("disabled", true);
            } else {
                $("#ISectorMother").prop("disabled", false);
                $("#IWorkConditionMother").prop("disabled", false);
                $("#IEspecificActivityMother").prop("disabled", false);
            }
        });


        $("#ISectorMother").on('change', function () {
            if (parseInt($(this).val(), 10) !== 0 || parseInt($("#IWorkConditionMother").val(), 10) !== 0 || $("#IEspecificActivityMother").val() !== "") {
                $("#DSectorMother").prop("disabled", true);
                $("#DWorkConditionMother").prop("disabled", true);
                $("#DBusyMother").prop("disabled", true);
                $("#DEspecificActivityMother").prop("disabled", true);

            } else {
                $("#DSectorMother").prop("disabled", false);
                $("#DWorkConditionMother").prop("disabled", false);
                $("#DBusyMother").prop("disabled", false);
                $("#DEspecificActivityMother").prop("disabled", false);
            }

        });


        $("#IWorkConditionMother").on('change', function () {
            if (parseInt($(this).val(), 10) !== 0 || parseInt($("#ISectorMother").val(), 10) !== 0 || $("#IEspecificActivityMother").val() !== "") {
                $("#DSectorMother").prop("disabled", true);
                $("#DWorkConditionMother").prop("disabled", true);
                $("#DBusyMother").prop("disabled", true);
                $("#DEspecificActivityMother").prop("disabled", true);

            } else {
                $("#DSectorMother").prop("disabled", false);
                $("#DWorkConditionMother").prop("disabled", false);
                $("#DBusyMother").prop("disabled", false);
                $("#DEspecificActivityMother").prop("disabled", false);
            }

        });

        $("#IEspecificActivityMother").on('keyup', function () {
            if ($(this).val() !== "" || parseInt($("#ISectorMother").val(), 10) !== 0 || parseInt($("#IWorkConditionMother").val(), 10) !== 0) {
                $("#DSectorMother").prop("disabled", true);
                $("#DWorkConditionMother").prop("disabled", true);
                $("#DBusyMother").prop("disabled", true);
                $("#DEspecificActivityMother").prop("disabled", true);
            } else {
                $("#DSectorMother").prop("disabled", true);
                $("#DWorkConditionMother").prop("disabled", true);
                $("#DBusyMother").prop("disabled", true);
                $("#DEspecificActivityMother").prop("disabled", true);
            }
        });

        $("#btn-view-studentfamily-modal").on('click', function (e) {
            $("#model_add_studentfamily").modal('show');
        });

        $("#edit_student_family_health input[id='isSick2']").on('click', function () {
            var currentValue = $(this)[0].checked;
            if (currentValue === true) {
                $("#edit_student_family_health input[name='DiseaseType']").prop("disabled", false);
            } else {
                $("#edit_student_family_health input[name='DiseaseType']").prop("disabled", true);
            }
        });

    };


    var WizardDemo = function () {
        $("#m_wizard");
        var e, r, i = $("#m_form");
        return {
            init: function () {
                var n;
                $("#m_wizard"), i = $("#m_form"), (r = new mWizard("m_wizard",
                    {
                        startStep: 1
                    })).on("beforeNext", function (r) {
                        !0 !== e.form() && r.stop()
                    }), r.on("change", function (e) {
                        //$("#m_form").valid();

                        datatable.studentFamily.adjustTable();
                        datatable.familyHealth.adjustTable();

                        mUtil.scrollTop()
                    }), r.on("change", function (e) {
                        //$("#m_form").valid();
                        1 === e.getStep()
                    }), e = i.validate({
                        ignore: ":hidden",
                        messages: {
                            accept: {
                                required: "¡Debe aceptar el acuerdo de Términos y condiciones!"
                            }
                        },
                        submitHandler: function (formElement, e) {
                            e.preventDefault();
                        }
                    }),
                    (n = i.find('[data-wizard-action="submit"]')).on("click", function (r) {
                        $("#m_form").valid();
                        if (i.validate().errorList.length == 0) {
                            $("#wizard_button_safe").addLoader();
                            $.ajax({
                                type: 'POST',
                                url: `/ficha-socioeconomica/guardar/post`.proto().parseURL(),
                                data: new FormData(i[0]),
                                contentType: false,
                                processData: false,
                                success: function (result) {
                                    mApp.unprogress(n);

                                    $("#wizard_button_safe").removeLoader();


                                    $(".success-message").show();
                                    $(".btn-view").prop("disabled", true);
                                    $(".wizard-buttons").hide();
                                    $(".btn-constancy-download").attr('data-url', result);
                                    $(".btn-constancy-download").on('click', function () {
                                        $(".btn-view").prop("disabled", false);
                                        var url = $(this).data('url');
                                        window.open(url, '_blank');
                                    });
                                    $(".btn-view").on('click', function () {                                       
                                        window.location.href = "/";                                
                                    });             

                                },
                                error: function (e) {

                                    toastr.error(e.responseText, _app.constants.toastr.title.error);
                                }
                            });
                        }

                    })


            }
        }

    }();
    return {
        init: function () {
            datatable.init();
            record();
            InitSelect2Origin();
            InitSelect2Current();
            InitSelect2PlaceOrigin();
            Disables();
            datepicker();
            extensions();
            modal();
            addStudentFamily();
            editStudentFamily();
            editStudentFamilyHealth();
            WizardDemo.init();
        }
    };

})();


$(function () {
    student_information.init();
/*    StudentFamilyTable.init();*/
    $("#IsSick").trigger('change');
    $("#HaveInsurance").trigger('change');
    $("#PrincipalPerson").trigger('change');
});