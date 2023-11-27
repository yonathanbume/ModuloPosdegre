var wizard_student_information = (function () {
   
    var getDeparmentsOrigin = function () {
        $.ajax({
            type: 'GET',
            url: ('/departamentos/get').proto().parseURL()
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
                }
            });
        });
    };
    var getDistrictsOrigin = function () {
        $("#originProvinceId").on('change', function () {
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
            });
        });
    };
    var getDistrictsCurrent = function () {
        $("#currentProvinceId").on('change', function () {
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
            });

        });
    };
    var getCareers = function () {
        $.ajax({
            type: 'GET',
            url: ('/carreras/v2/get').proto().parseURL()
        }).done(function (data) {
            $("#Career").select2({
                data: data.items
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
    var OtherFunctions = function () {
        $('.amt').keyup(function () {
            var importe_total = 0
            $(".amt").each(
                function (index, value) {
                    if ($.isNumeric($(this).val())) {
                        importe_total = importe_total + eval($(this).val());
                    }
                }
            );
            $("#TotalRemuneration").val(importe_total);

        });

        $("#Birthdate").datepicker({
            format: _app.constants.formats.datepicker
        });
        $("#Birthdate").datepicker().on("change", function () {
            var currentDate = new Date().getUTCFullYear();
            var birthday = $("#Birthdate").val();
            var birthdayYear = parseInt(birthday.substr(birthday.length - 4), 10);
            if (!isNaN(currentDate - birthdayYear)) {
                $("#Age").val((currentDate - birthdayYear).toString());
            } else {
                $("#Age").val("");
            }
        });

        $("#Birthdate").doneTyping(function () {
            var currentDate = new Date().getUTCFullYear();
            var birthday = $("#Birthdate").val();
            var birthdayYear = parseInt(birthday.substr(birthday.length - 4), 10);
            if (!isNaN(currentDate - birthdayYear)) {
                $("#Age").val((currentDate - birthdayYear).toString());
            } else {
                $("#Age").val("");
            }

        });
        $('input[id=AuthorizeCheck]').on('change', function () {
            var currentValue = $(this)[0].checked;
            if (currentValue === true) {
                $(".authorize").prop("disabled", false);
            } else {
                $(".authorize").prop("disabled", true);
            }
        });

    };
   var Disables = function () {

       $("#ISSickStudent").on('change', function () {
           if ($(this).val() == 0) {
               $("#TypeIllness").prop('disabled', true);
           } else {
               $("#TypeIllness").prop('disabled', false);
           }

       });

        $("#IsSick").on('change', function () {
            if ($(this).val() == 0) {
                if ($('#ParentSick option').length == 4) {
                    $('#ParentSick').append($('<option>', { value: 0, text: '---' }));
                }
                $("#ParentSick").val(0).trigger('change');
                $("#ParentSick").prop("disabled", true);
                $("#TypeParentIllness").prop("disabled", true);
            }
            else {
                $("#ParentSick").prop("disabled", false);
                $("#ParentSick").find('option[value="0"]').remove();
                $("#ParentSick").val(1).trigger('change');
                $("#TypeParentIllness").prop("disabled", false);
            }

       });

       $("#Insurance").on('change', function () {
           if ($(this).val() == "False") {
               $("#TypeInsurance").prepend("<option value='0'>---</option>");
               $("#TypeInsurance").val('0').trigger('change');
               $("#TypeInsurance").prop("disabled", true);
               $("#OtherInsurance").prop("disabled", true);
               $("#OtherInsurance").val("");
           }
           else {

               $("#TypeInsurance option[value='0']").remove();
               $("#TypeInsurance").trigger('change');
               $("#TypeInsurance").prop("disabled", false);

           }

       });

       $("#TypeInsurance").on('change', function () {
           if ($(this).val() == 4) {
               $("#OtherInsurance").prop("disabled", false);

           }
           else {
               $("#OtherInsurance").prop("disabled", true);
           }

       });
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


    }

   return {
       init: function () {
           InitSelect2Origin();
           InitSelect2Current();           
           getCareers();
           Disables();
           OtherFunctions();           
       }       
   }
})();
var StudentFamilyTable = (function () {
    var source = [];    

    var lstRelationShip = [
        { value: 0, name: "--" },
        { value: 1, name: "Padre" },
        { value: 2, name: "Madre" },
        { value: 3, name: "Hijo" },
        { value: 4, name: "Hija" },
        { value: 5, name: "Otros" }
    ];
    var lstCivilStatus = [
        { value: 0, name: "--" },
        { value: 1, name: "Soltero(a)" },
        { value: 2, name: "Divorciado(a)" },
        { value: 3, name: "Casado(a)" },
        { value: 4, name: "Viudo(a)" }        
    ];
    var lstDegreeInstruction = [
        { value: 0, name: "--" },
        { value: 1, name: "Primaria completa" },
        { value: 2, name: "Primaria incompleta" },
        { value: 3, name: "Secundaria completa" },
        { value: 4, name: "Secundaria incompleta" },
        { value: 5, name: "Superior técnica completa" },
        { value: 6, name: "Superior técnica incompleta" },
        { value: 7, name: "Universitario completo" },
        { value: 8, name: "Universitario incompleto" },
        { value: 9, name: "Postgrado" },
        { value: 10, name: "Sin nivel" }
    ];

    var studentfamilyForm;
    var datatable = null;
    var options = {
        data: {
            type: "local",
            source: source
        },
        columns: [
            {
                field: 'completename',
                title: 'Nombre',
                width: 150
            },
            {
                field: 'paternalname',
                title: 'Apellido Paterno',
                width: 150
            },
            {
                field: 'maternalname',
                title: 'Apellido Materno',
                width: 150
            },
            {
                field: 'birthday',
                title: 'F.Nacimiento',
                width: 100

            },
            {
                field: 'relationshipint',
                title: 'Parentesco',
                width: 100,
                template: function (row) {
                    tmp = "";
                    var index = parseInt(row.relationshipint);                    
                    tmp = `<span>${lstRelationShip[index].name}</span>`;
                    return tmp;
                }
            },
            {
                field: 'civilstatusint',
                title: 'Estado Civil',
                width: 100,
                template: function (row) {
                    tmp = "";
                    var index = parseInt(row.civilstatusint);
                    tmp = `<span>${lstCivilStatus[index].name}</span>`;
                    return tmp;
                }
            },
            {
                field: 'degreeinstructionint',
                title: 'Grado de instrucción',
                width: 150,
                template: function (row) {
                    tmp = "";
                    var index = parseInt(row.degreeinstructionint);
                    tmp = `<span>${lstDegreeInstruction[index].name}</span>`;
                    return tmp;
                }
            },
            {
                field: 'certificated',
                title: 'Titulado/Maestria',
                width: 150
            },
            {
                field: 'occupation',
                title: 'Ocupación',
                width: 150
            },
            {
                field: 'workcenter',
                title: 'Centro Laboral Y/o estudios',
                width: 150
            },
            {
                field: 'location',
                title: 'Localidad',
                width: 150
            }
            ,
            {
                field: "options",
                title: "",
                className: "left",
                width: 100,
                sortable: false,
                filterable: false,
                template: function (row ,index) {
                    var tmp = '';                    
                    tmp += `<button onclick="StudentFamilyTable.deleteRow(${index})" type="button" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete"<span><i class="la la-trash"> </i> </span> Eliminar </span></span></button>`;
                    return tmp;
                }
            }
        ]
    }
    var formInitializer = function () {
        studentfamilyForm = $("#create_student_family").validate({
            submitHandler: function () {
                var completename = $("input[name=CompleteName]").val();
                var paternalname = $("input[name=PaternalName]").val();
                var maternalname = $("input[name=MaternalName]").val();
                var birthday = $("input[name=Birthday]").val();
                var relationship = $("select[name=RelationshipInt]").val();
                var civilstatus = $("select[name=CivilStatusInt]").val();
                var certificated = $("input[name=Certificated]").val();
                var degreeinstruction = $("select[name=DegreeInstructionInt]").val();
                var occupation = $("input[name=Occupation]").val();
                var workcenter = $("input[name=WorkCenter]").val();
                var location = $("input[name=Location]").val();

                StudentFamilyTable.addRow(completename, paternalname, maternalname, birthday, relationship, civilstatus, certificated, degreeinstruction, occupation, workcenter, location);

                $("#model_add_studentfamily").modal("hide");
                studentfamilyForm.resetForm();
            }
        });
    }
    var deleteRow = function (id) {
        StudentFamilyTable.removeRow(id);
       
    }
    return {
        init: function () {
            datatable = $("#studentFamily-datatable").mDatatable(options);
            formInitializer();
        },
        addRow: function (completename, paternalname, maternalname, birthday, relationship, civilstatus, certificated, degreeinstruction, occupation, workcenter, location) {
            source.push({ completename: completename, paternalname: paternalname, maternalname: maternalname, birthday: birthday, relationshipint: relationship, civilstatusint: civilstatus, certificated: certificated, degreeinstructionint: degreeinstruction, occupation: occupation, workcenter: workcenter, location: location })
            datatable.originalDataSet = source;
            datatable.reload();
        },        
        removeRow: function (id) {
            source.splice(id, 1);
            datatable.originalDataSet = source;
            datatable.reload();
        },
        getCount: function () {
            return datatable.getTotalRows();
        },
        reload: function () {
            datatable.reload();
        },
        deleteRow: function (id) {
            deleteRow(id);
        },
        getSource : function () {
            return source;
        }
    }
})();   
$(function () {    
    wizard_student_information.init();
    StudentFamilyTable.init();
    $("#IsSick").trigger('change');
    $("#ISSickStudent").trigger('change');
    $("#Insurance").trigger('change');
})