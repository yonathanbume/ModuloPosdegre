var InitApp = function () {
    var surveyId = $("#SurveyId").val();
    var selection = [];
    var datatable = {
        users: {
            object: null,
            options: getSimpleDataTableConfiguration({
                url: (`/admin/gestion-encuestas/getusers`).proto().parseURL(),
                data : function(data){
                    data.facultyId = $("#faculties").val();
                    data.careerId = $("#careers").val();
                    data.specialtyId = $("#specialties").val();
                    data.academicDepartmentId = $("#academicDepartments").val();
                    data.rol = $("#roles").val();
                    data.onlyEnrolled = $('#onlyEnrolled').is(':checked');
                    data.academicYears = JSON.stringify($("#academicYears").val());
                },
                pageLength: 20,
                orderable: [],
                columns: [
                    {
                        title: "#",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            if (selection.includes(data.id) == true) {
                                template += `<label class="m-checkbox m-checkbox--solid m-checkbox--single m-checkbox--brand"><input type="checkbox" data-id="${data.id}" onchange="InitApp.select(this);" checked><span></span></label>`;
                            } else {
                                template += `<label class="m-checkbox m-checkbox--solid m-checkbox--single m-checkbox--brand"><input type="checkbox" data-id="${data.id}" onchange="InitApp.select(this);"><span></span></label>`;
                            }
                            return template;
                        }
                    },
                    {
                        title: "Nombre Completo",
                        data: "fullName",
                        orderable: false
                    },
                    {
                        title: "Correo",
                        data: "email",
                        orderable: false
                    }
                ]
            }),
            init: function () {
                $(".btn-custom-send").prop('disabled', true);
                datatable.users.object = $("#users_table").DataTable(datatable.users.options);
            },
            reload: function () {
                datatable.users.object.ajax.reload();
            }
        },
        init: function () {
            this.users.init();
        }
    };
    var select2 = {
        init: function () {
            this.faculties.init();
            this.careers.init();
            this.speciality.init();
            this.roles.init();
            this.academicDepartments.init();
            this.academicYears.init();
        },
        academicYears: {
            init: function () {
                $('#academicYears').selectpicker({
                    actionsBox: true,
                    selectAllText: 'Marcar todos',
                    deselectAllText: 'Desmarcar todos',
                    noneSelectedText: 'Seleccionar',
                    size: 10,
                });
                $('#academicYears').selectpicker("refresh");
            }
        },
        academicDepartments: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: `/departamentos-academicos/get`.proto().parseURL()
                }).done(function (result) {
                    $('#academicDepartments').html(`<option value="0" selected>Todas</option>`);
                    $(".select2-academicDepartments").select2({
                        data: result,
                    });
                });
            }
        },
        faculties: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: `/facultades/get`.proto().parseURL()
                }).done(function (result) {
                    $('#faculties').html(`<option value="0" selected>Todas</option>`);
                    $(".select2-faculties").select2({
                        data: result.items,
                    });
                });
            },
            events: function () {
                $("#faculties").on("change", function () {
                    $("#specialties").val("0").trigger("change",[false]);
                    $("#specialties").prop("disabled", true);
                    select2.careers.load();
                })
            }
        },
        careers: {
            init: function () {
                $("#careers").prop("disabled", true);
                this.events();
            },
            load: function () {
                $("#careers").prop("disabled", false);
                $.ajax({
                    url: `/carrerasporfacultad/${$("#faculties").val()}/get`.proto().parseURL()
                }).done(function (result) {
                    $('#careers').html(`<option value="0" selected>Todas</option>`);
                    $(".select2-careers").select2({
                        data: result.items,
                    });
                });
            },
            events: function () {
                $("#careers").on("change", function (e, state) {
                    //we check state if exists and is true then event was triggered
                    if (typeof state != 'undefined' && state) {
                        return false;
                    }
                    select2.speciality.load();
                })
            }
        },
        speciality: {
            init: function () {
                $("#specialties").prop("disabled", true);
            },
            load: function () {
                $("#specialties").prop("disabled", false);
                $.ajax({
                    url: `/especialidadporcarrera/${$("#careers").val()}/get`.proto().parseURL()
                }).done(function (result) {
                    $('#specialties').html(`<option value="0" selected>Todas</option>`);
                    $(".select2-specialties").select2({
                        data: result.items,
                    });
                });
            }
        },
        roles: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: `/roles_selected/get`.proto().parseURL()
                }).done(function (result) {
                    $(".select2-roles").select2({
                        data: result.items,
                    });
                });
            },
            events: function () {
                $("#roles").on("change", function (e, state) {
                    var rolText = $("#roles option:selected").text();
                    if (rolText === "Alumnos") {
                        $("#onlyEnrolledContainer").css("display", "block");
                        $("#facultiesContainer").css("display", "block");
                        $("#careersContainer").css("display", "block");
                        $("#specialtiesContainer").css("display", "block");
                        $("#academicDepartmentsContainer").css("display", "none");
                    } else if (rolText === "Docentes") {
                        $("#onlyEnrolledContainer").css("display", "none");
                        $("#facultiesContainer").css("display", "none");
                        $("#careersContainer").css("display", "none");
                        $("#specialtiesContainer").css("display", "none");
                        $("#academicDepartmentsContainer").css("display", "block");
                        $("#academicYearsContainer").css("display", "none");
                        $('#onlyEnrolled').prop('checked', false).trigger("change");
                        $('#academicYears').selectpicker('deselectAll');

                    } else {
                        $("#onlyEnrolledContainer").css("display", "none");
                        $("#facultiesContainer").css("display", "none");
                        $("#careersContainer").css("display", "none");
                        $("#specialtiesContainer").css("display", "none");
                        $("#academicDepartmentsContainer").css("display", "none");
                        $("#academicYearsContainer").css("display", "none");
                        $('#onlyEnrolled').prop('checked', false).trigger("change");
                        $('#academicYears').selectpicker('deselectAll');

                    }
                })
            }
        }
    };
    var selectValues = function (element) {
        const mode = $(element).prop('checked');
        const value = $(element).data("id");
        if (mode === true) {
            selection.push(value);
            $(".btn-custom-send").prop('disabled', false);
        }
        else {
            selection = selection.filter(function (item) {
                return item !== value
            });
            if (selection.length === 0)
                $(".btn-custom-send").prop('disabled', true);
        }
        $("#tableinfo").html(selection.length);
    };
    var events = {
        init: function () {
            $("#onlyEnrolled").on('change', function () {
                if ($('#onlyEnrolled').is(':checked')) {
                    $("#academicYearsContainer").css("display", "block");
                } else {
                    $("#academicYearsContainer").css("display", "none");
                    $('#academicYears').selectpicker('deselectAll');
                }
            });


            $("#btn-apply").on('click', function () {
                datatable.users.reload();
            });
            $(".btn-send").on('click', function () {
                $(".btn-send").addLoader();
                $.ajax({
                    url: `/admin/gestion-encuestas/enviar/validar`.proto().parseURL(),
                    type: 'POST',
                    data: {
                        facultyId: $("#faculties").val(),
                        careerId: $("#careers").val(),
                        specialtyId: $("#specialties").val(),
                        academicDepartmentId: $("#academicDepartments").val(),
                        rol: $("#roles").val(),
                        onlyEnrolled: $('#onlyEnrolled').is(':checked'),
                        academicYears: JSON.stringify($("#academicYears").val())
                    }
                }).done(function (result) {                  
                    if (result > 1000) {
                        //toastr.success("La encuesta estará disponible en una hora", _app.constants.toastr.title.success);
                        swal({
                            title: "¿Está seguro?, Se enviarán más de 1000 encuestas...",
                            text: "Las encuestas pueden tardar en ser enviadas debido a la cantidad de usuarios seleccionados."
                                 + "En este caso la encuesta estará disponible para los usuarios en unas horas.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, enviar",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $(".btn-send").addLoader();
                                    $.ajax({
                                        url: `/admin/gestion-encuestas/job/enviar/usuarios/${surveyId}`.proto().parseURL(),
                                        type: "POST",
                                        async: true,
                                        data: {
                                            facultyId: $("#faculties").val(),
                                            careerId: $("#careers").val(),
                                            specialtyId: $("#specialties").val(),
                                            academicDepartmentId: $("#academicDepartments").val(),
                                            rol: $("#roles").val(),
                                            onlyEnrolled: $('#onlyEnrolled').is(':checked'),
                                            academicYears: JSON.stringify($("#academicYears").val())
                                        }
                                    });
                                    setTimeout(function () { location.href = `/admin/gestion-encuestas`.proto().parseURL(); }, 5000);
                                });
                            },
                            allowOutsideClick: () => !swal.isLoading()
                        });
                    } else {
                        $.ajax({
                            url: `/admin/gestion-encuestas/nojob/enviar/usuarios/${surveyId}`.proto().parseURL(),
                            type: `POST`,
                            data: {
                                facultyId: $("#faculties").val(),
                                careerId: $("#careers").val(),
                                specialtyId: $("#specialties").val(),
                                academicDepartmentId: $("#academicDepartments").val(),
                                rol: $("#roles").val(),
                                onlyEnrolled: $('#onlyEnrolled').is(':checked'),
                                academicYears: JSON.stringify($("#academicYears").val())
                            }
                        }).done(function (result) {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            setTimeout(function () { location.href = `/admin/gestion-encuestas`.proto().parseURL(); }, 2000);
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            setTimeout(function () { location.href = `/admin/gestion-encuestas`.proto().parseURL(); }, 2000);
                        });
                    }

                }).fail(function (error) {
                    toastr.error(error.responseText, _app.constants.toastr.title.error);
                    setTimeout(function () { location.href = `/admin/gestion-encuestas`.proto().parseURL(); }, 3000);
                });
            });
            $(".btn-custom-send").on('click', function () {
                if (selection.length == 0) {
                    toastr.error("Seleccione los usuarios a enviar la encuesta.", _app.constants.toastr.title.error);
                    return;
                }
                $(".btn-custom-send").addLoader();
                $.ajax({
                    url: `/admin/gestion-encuestas/sendSurveys`.proto().parseURL(),
                    type: 'POST',
                    data: {
                        users: selection,
                        surveyId: surveyId
                    },
                }).done(function (result) {
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    setTimeout(function () { location.href = `/admin/gestion-encuestas`.proto().parseURL(); }, 3000);
                }).fail(function (error) {
                    toastr.error(error.responseText, _app.constants.toastr.title.error);
                    setTimeout(function () { location.href = `/admin/gestion-encuestas`.proto().parseURL(); }, 3000);
                });
            })
        }
    };

    return {
        init: function () {
            datatable.init();
            select2.init();
            events.init();
            ///*SIGNALR - NOTIFICATIONS */
            //"use strict";
            //var connection = new signalR.HubConnectionBuilder().withUrl("/Hubs/Akdemic").build();
            //connection.start().then(function () {
            //    //document.getElementById("sendButton").disabled = false;
            //})
            //    .catch(function (err) {
            //        return console.error(err.toString());
            //    });

        },
        select: function (element) {
            selectValues(element);
        }
    };
}();
$(function () {
    InitApp.init();
});