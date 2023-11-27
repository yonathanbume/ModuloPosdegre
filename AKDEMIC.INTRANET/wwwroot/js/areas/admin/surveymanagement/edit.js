var InitApp = function () {
    var sent = $("#Sended").val();
    var SurveyId = $("#SurveyId").val();
    var formSection = {
        create: {
            object: $("#add-section-form").validate({
                submitHandler: function (form, e) {
                    e.preventDefault();
                    mApp.block("#add_section_modal");
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: $(form).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");

                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        formSection.create.clear();
                        manageSection.loadData();
                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "") $("#add_sectionform_msg_txt").html(error.responseText);
                        else $("#add_sectionform_msg_txt").html(_app.constants.ajax.message.error);
                        $("#add_sectionform_msg").removeClass("m--hide").show();
                    }).always(function () {
                        mApp.unblock("#add_section_modal");
                    });
                }
            }),
            show: function () {
                $("#add_section_modal").modal("toggle");
                $("#add_section_modal").one("hidden.bs.modal",
                    function (e) {
                        formSection.create.clear();
                    });
            },
            clear: function () {
                formSection.create.object.resetForm();
            }
        },
        delete: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "La Sección y sus preguntas serán eliminadas.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarla",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/admin/gestion-encuestas/item/eliminar").proto().parseURL(),
                            type: "POST",
                            data: {
                                id: id
                            },
                            success: function (result) {
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "La Sección y sus preguntas han sido eliminadas con exito",
                                    confirmButtonText: "Excelente"
                                }).then(manageSection.loadData());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "La Sección y sus preguntas presentan información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },
        edit: {
            object: $("#edit-section-form").validate({
                submitHandler: function (e) {
                    if (sent == 1) {
                        toastr.error("Esta encuesta ya fue enviada, no puede ser editada", _app.constants.toastr.title.error);
                    } else if (sent == 0) {
                        console.log($("#edit_Id").val());
                        mApp.block("#edit_section_modal");
                        $.ajax({
                            url: `/admin/gestion-encuestas/encuesta-seccion/editar`,
                            type: "POST",
                            data: {
                                id: $("#edit_Id").val(),
                                title: $("#edit_Title").val(),
                                isLikert: $("#edit-section-form  input[name=IsLikert]").is(":checked"),
                            }
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            formSection.edit.clear();
                            manageSection.loadData();
                        }).fail(function (error) {
                            if (error.responseText !== null && error.responseText !== "") $("#edit_sectionform_msg_txt").html(error.responseText);
                            else $("#edit_sectionform_msg_txt").html(_app.constants.ajax.message.error);
                            $("#edit_sectionform_msg").removeClass("m--hide").show();
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        }).always(function () {
                            mApp.unblock("#edit_section_modal");
                        });
                    }
                }
            }),
            show: function (id) {
                if (sent == 1) {
                    toastr.error("Esta encuesta ya fue enviada, no puede ser editada", _app.constants.toastr.title.error);
                } else if (sent == 0) {
                    this.load(id)
                    $("#edit_section_modal").modal("toggle");
                }
            },
            load: function (id) {
                $.ajax({
                    url: `/admin/gestion-encuestas/encuesta-seccion/${id}/get`,
                    type: "GET"
                }).done(function (result) {
                    console.log(result);
                    $("#edit_Id").val(result.id);
                    $("#edit_Title").val(result.title);
                    $('#edit-section-form input[name=IsLikert]').prop('checked', result.isLikert);
                });
            },
            clear: function () {
                this.object.resetForm();
            },
            events: function () {
                $("#edit_section_modal").one("hidden.bs.modal",
                    function (e) {
                        formSection.edit.clear();
                    });
            }
        },
        init: function () {
            this.edit.events();
        }
    };
    var formLikertQuestion = {
        create: {
            object: $("#add-likert-question-form").validate({
                submitHandler: function (form, e) {
                    e.preventDefault();
                    mApp.block("#add_likertquestion_modal");
                    $("#btnAddLikert").addLoader();
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: $(form).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        formLikertQuestion.create.clear();
                        manageSection.loadData();
                        $("#btnAddLikert").removeLoader();
                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "") $("#add_likertquestionform_msg_txt").html(error.responseText);
                        else $("#add_likertquestionform_msg_txt").html(_app.constants.ajax.message.error);
                        $("#add_likertquestionform_msg").removeClass("m--hide").show();
                        $("#btnAddLikert").removeLoader();
                    }).always(function () {
                        mApp.unblock("#add_likertquestion_modal");
                    });
                }
            }),
            clear: function () {
                formLikertQuestion.create.object.resetForm();
            },
            show: function (id) {
                $('#add-likert-question-form input[name=SurveyItemId]').val(id);
                $("#add_likert_question_modal").modal("toggle");
            },
            events: function () {
                $("#add_likert_question_modal").on("hidden.bs.modal",
                    function (e) {
                        formLikertQuestion.create.clear();
                    });
            }
        },
        edit: {
            object: $("#edit-likert-question-form").validate({
                submitHandler: function (form, e) {
                    e.preventDefault();
                    mApp.block("#edit_likertquestion_modal");
                    $("#btnEditLikert").addLoader();
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: $(form).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        manageSection.loadData();
                        $("#btnEditLikert").removeLoader();
                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "") $("#edit_likertquestionform_msg_txt").html(error.responseText);
                        else $("#edit_likertquestionform_msg_txt").html(_app.constants.ajax.message.error);
                        $("#edit_likertquestionform_msg").removeClass("m--hide").show();
                        $("#btnEditLikert").removeLoader();
                    }).always(function () {
                        mApp.unblock("#edit_likertquestion_modal");
                    });
                }
            }),
            show: function (id) {
                this.load(id);
                $("#edit_likert_question_modal").modal("toggle");
            },
            load: function (id) {
                $.ajax({
                    url: (`/admin/gestion-encuestas/pregunta/get/${id}`).proto().parseURL()
                }).done(function (result) {
                    $('#edit-likert-question-form input[name=QuestionId]').val(result.id);
                    $('#edit-likert-question-form input[name=SurveyItemId]').val(result.surveyItemId);
                    $('#edit-likert-question-form input[name=Description]').val(result.description);
                });
            }
        },
        init: function () {
            this.create.events();
        }
    };
    var formQuestion = {
        create: {
            object: $("#add-question-form").validate({
                submitHandler: function (form, e) {
                    e.preventDefault();
                    mApp.block("#add_question_modal");
                    var Type = parseInt($('#Type').val());

                    var Answers = [];
                    var elements = document.getElementsByClassName("answer");
                    var ids = document.getElementsByClassName("answerId");
                    if (elements.length < 2 && parseInt($('#Type').val()) !== _app.constants.survey.text_question) {
                        mApp.unblock("#add_question_modal");
                        toastr.error("Agregue por lo menos dos respuestas", _app.constants.toastr.title.error);
                        return;
                    }
                    if (parseInt($('#Type').val()) !== _app.constants.survey.text_question) {
                        for (var i = 0; i < elements.length; i++) {
                            if (elements[i].value === "") {
                                mApp.unblock("#add_question_modal");
                                toastr.error("Campos vacíos", _app.constants.toastr.title.error);
                                return;
                            }
                            var answer = { description: elements[i].value, id: ids[i].value };
                            Answers.push(answer);
                        }
                    }

                    $.ajax({
                        url: "/admin/gestion-encuestas/registrar/pregunta/post".proto().parseURL(),
                        type: "POST",
                        data: {
                            surveyId: SurveyId,
                            surveyItemId: $('#SurveyItemId').val(),
                            type: Type,
                            description: $("#add-question-form input[name='Description']").val(),
                            answers: Answers
                        }
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        formQuestion.create.clear();
                        manageSection.loadData();
                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "") $("#add_questionform_msg_txt").html(error.responseText);
                        else $("#add_questionform_msg_txt").html(_app.constants.ajax.message.error);
                        $("#add_questionform_msg").removeClass("m--hide").show();
                    }).always(function () {
                        mApp.unblock("#add_question_modal");
                    });
                }
            }),
            show: function (id) {
                $(".select2-questionType").val("").trigger("change");
                $('#add-question-form input[name=SurveyItemId]').val(id);
                $("#add_question_modal").modal("toggle");
            },
            clear: function () {
                formQuestion.create.object.resetForm();
                $('.formanswers').empty();
            },
            events: function () {
                $('#Type').on('change', function () {
                    switch (this.value) {
                        case "1":
                            $("#question-text").css("display", "block");
                            $("#question-multiple").css("display", "none");
                            $("#question-answers").css("display", "none");
                            $('.formanswers').empty();
                            break;
                        case "2": $("#question-text").css("display", "none");
                            $("#question-multiple").css("display", "block");
                            $("#question-answers").css("display", "block"); break;
                        case "3": $("#question-text").css("display", "none");
                            $("#question-multiple").css("display", "block");
                            $("#question-answers").css("display", "block"); break;
                    }
                });
                $("#add-answer").on('click', function () {
                    var htmldata = '<div class="form-group col-lg-12" style="display:flex;"><input class="answerId" value="" hidden/><input class="form-control m-input answer" style="margin-right: 10px;" required><button class="btn btn-danger btn-sm m-btn--icon delete-answer" type="button" onclick="this.parentNode.outerHTML = \'\';"><span><i class="la la-trash"></i></span></button></div>';
                    var e = document.createElement('div');
                    e.innerHTML = htmldata;
                    document.getElementById("question-answers").appendChild(e.firstChild);
                });

                $("#add_question_modal").one("hidden.bs.modal",
                    function (e) {
                        formQuestion.create.clear();
                    });
            }
        },
        delete: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "La Pregunta se eliminará.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarla",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/admin/gestion-encuestas/preguntas/eliminar").proto().parseURL(),
                            type: "POST",
                            data: {
                                id: id
                            },
                            success: function (result) {
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "La pregunta ha sido eliminada con exito",
                                    confirmButtonText: "Excelente"
                                }).then(manageSection.loadData());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "La Pregunta presenta información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },
        edit: {
            object: $("#edit-question-form").validate({
                submitHandler: function (e) {
                    mApp.block("#edit_question_modal");
                    var Type = parseInt($('#Edit_Type').val());
                    var Answers = [];
                    var elements = document.getElementsByClassName("Edit_answer");
                    var ids = document.getElementsByClassName("Edit_answerId");
                    if (elements.length < 2 && Type !== _app.constants.survey.text_question) {
                        mApp.unblock("#edit_question_modal");
                        toastr.error("Agregue por lo menos dos respuestas", _app.constants.toastr.title.error);
                        return;
                    }
                    if (Type !== _app.constants.survey.text_question) {
                        for (var i = 0; i < elements.length; i++) {
                            if (elements[i].value === "") {
                                mApp.unblock("#edit_question_modal");
                                toastr.error("Campos vacíos", _app.constants.toastr.title.error);
                                return;
                            }
                            var answer = { description: elements[i].value, id: ids[i].value };
                            Answers.push(answer);
                        }
                    }
                    $.ajax({
                        url: "/admin/gestion-encuestas/editar/pregunta/post".proto().parseURL(),
                        type: "POST",
                        data: {
                            id: $('#questionId').val(),
                            type: Type,
                            description: $("#Edit_Description").val(),
                            answers: Answers
                        }
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        formQuestion.edit.clear();
                        manageSection.loadData();
                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "") $("#edit_questionform_msg_txt").html(error.responseText);
                        else $("#edit_questionform_msg_txt").html(_app.constants.ajax.message.error);
                        $("#edit_questionform_msg").removeClass("m--hide").show();
                    }).always(function () {
                        mApp.unblock("#edit_question_modal");
                    });
                }
            }),
            show: function (id) {
                $('#questionId').val(id);
                mApp.unblock("#edit_question_modal");
                $("#edit_question_modal").modal("toggle");
                $("#edit_question_modal").one("shown.bs.modal",
                    function (e) {
                        mApp.block("#edit_question_modal .modal-content", { type: "loader", message: "Cargando..." });
                        formQuestion.edit.load(id);
                        mApp.unblock("#edit_question_modal .modal-content");
                    });
                $("#edit_question_modal").one("hidden.bs.modal",
                    function (e) {
                        formQuestion.edit.clear();
                    });
            },
            clear: function () {
                formQuestion.edit.object.resetForm();
                $('.formanswers').empty();
            },
            events: function () {
                $('#Edit_Type').on('change', function () {
                    switch (this.value) {
                        case "1":
                            $("#edit-question-form .question-text").css("display", "block");
                            $("#edit-question-form .question-multiple").css("display", "none");
                            $("#edit-question-form .question-answers").css("display", "none");
                            $("#edit-question-form .formanswers").css("display", "none");
                            $('.formanswers').empty();
                            break;
                        case "2":
                            $("#edit-question-form .question-text").css("display", "none");
                            $("#edit-question-form .question-multiple").css("display", "block");
                            $("#edit-question-form .question-answers").css("display", "block");
                            $("#edit-question-form .formanswers").css("display", "block");
                            break;
                        case "3":
                            $("#edit-question-form .question-text").css("display", "none");
                            $("#edit-question-form .question-multiple").css("display", "block");
                            $("#edit-question-form .question-answers").css("display", "block");
                            $("#edit-question-form .formanswers").css("display", "block");
                            break;
                    }
                });
                $("#edit-question-form .add-answer").on('click', function () {
                    var htmldata = '<div class="form-group col-lg-12" style="display:flex;"><input class="Edit_answerId" value="" hidden/><input class="form-control m-input Edit_answer" style="margin-right: 10px;" required><button class="btn btn-danger btn-sm m-btn--icon delete-answer" type="button" onclick="this.parentNode.outerHTML = \'\';"><span><i class="la la-trash"></i></span></button></div>';
                    var e = document.createElement('div');
                    e.innerHTML = htmldata;
                    $("#edit-question-form .formanswers").append(e.firstChild);
                });
            },
            load: function (id) {
                $.ajax({
                    url: (`/admin/gestion-encuestas/pregunta/get/${id}`).proto().parseURL()
                }).done(function (result) {
                    $("#Edit_questionId").val(result.id);
                    $('#Edit_Type').val(result.type).trigger("change");
                    $('#Edit_Description').val(result.description);


                    if (result.type !== _app.constants.survey.text_question) {
                        $("#edit-question-form .question-text").css("display", "none");
                        $("#edit-question-form .question-multiple").css("display", "block");
                        $("#edit-question-form .question-answers").css("display", "block");
                        for (var i = 0; i < result.answers.length; i++) {
                            var htmldata = '<div class="form-group col-lg-12" style="display:flex;"><input class="Edit_answerId" value="' + result.answers[i].id + '" hidden/>' +
                                '<input class="form-control m-input Edit_answer" style="margin-right: 10px;" required value="' + result.answers[i].description + '" /> <button class="btn btn-danger btn-sm m-btn--icon delete-answer" type="button" onclick="this.parentNode.outerHTML = \'\';"><span><i class="la la-trash"></i></span></button></div> ';
                            var e = document.createElement('div');
                            e.innerHTML = htmldata;
                            $("#edit-question-form .formanswers").append(e.firstChild);
                        }
                        $("#edit-question-form .formanswers").css("display", "block");
                    }
                    else {
                        $("#edit-question-form .question-text").css("display", "block");
                        $("#edit-question-form .question-multiple").css("display", "none");
                        $("#edit-question-form .question-answers").css("display", "none");
                        $("#edit-question-form .formanswers").css("display", "none");
                    }
                });
            }
        },
        events: function () {
            formQuestion.create.events();
            formQuestion.edit.events();
        }
    };
    var formSurvey = {
        editDate: {
            object: $("#edit-survey-form").validate({
                submitHandler: function (form, e) {
                    e.preventDefault();
                    mApp.block("#edit-survey-form");
                    $("#btn-edit-date-survey").addLoader();
                    $.ajax({
                        url: "/admin/gestion-encuestas/editar/cambiarfechas".proto().parseURL(),
                        type: "POST",
                        data: {
                            id: SurveyId,
                            publicationDate: $("#PublicationDate").val(),
                            finishDate: $("#FinishDate").val(),
                        }
                    }).done(function () {
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#edit-survey-form");
                        $("#btn-edit-date-survey").removeLoader();
                    });
                }
            }),
        },
        edit: {
            object: $("#edit-survey-form").validate({
                submitHandler: function (form, e) {
                    e.preventDefault();
                    mApp.block("#edit-survey-form");
                    $("#btn-edit-Survey").addLoader();
                    $.ajax({
                        url: "/admin/gestion-encuestas/editar/post".proto().parseURL(),
                        type: "POST",
                        data: {
                            id: SurveyId,
                            name: $("#Name").val(),
                            publicationDate: $("#PublicationDate").val(),
                            finishDate: $("#FinishDate").val(),
                            code: $("#Code").val(),
                            description: $("#Description").val(),
                            isRequired: $("#IsRequired").is(":checked"),
                            isAnonymous: $("#IsAnonymous").is(":checked")
                        }
                    }).done(function () {
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                    }).always(function () {
                        mApp.unblock("#edit-survey-form");
                        $("#btn-edit-Survey").removeLoader();
                    });
                }
            }),
            load: function (id) {
                $.ajax({
                    url: ("/admin/gestion-encuestas/get/" + id).proto().parseURL(),
                    type: "GET",
                    data: {
                        id: id
                    },
                    success: function (result) {
                        $("#Name").val(result.name);
                        $("#Name").prop('disabled', true);

                        $("#Code").val(result.code);

                        $("#Description").val(result.description);
                        $("#Description").prop('disabled', true);

                        $("#PublicationDate").datepicker('update', result.publicationDate);
                        $("#FinishDate").datepicker('update', result.finishDate);

                        $("#PublicationDate").datepicker("setEndDate", result.finishDate);
                        $("#FinishDate").datepicker("setStartDate", result.publicationDate);

                        $("#IsRequired").prop('checked', result.isRequired);
                        $("#IsRequired").prop('disabled', true);

                        $("#IsAnonymous").prop('checked', result.isAnonymous);
                        $("#IsAnonymous").prop('disabled', true);
                    },
                    error: function () {
                        toastr.error("Error al cargar la encuesta", _app.constants.toastr.title.error);
                    }
                });
            }

        },
        send: {
            validate: function (id) {
                $("#send-survey").addLoader();
                $.ajax({
                    url: `/admin/gestion-encuestas/general/enviar/validate`.proto().parseURL(),
                    type: "POST",
                    data: {
                        id: id
                    }
                }).done(function (data) {
                    location.href = `/admin/gestion-encuestas/enviar/${id}`.proto().parseURL();
                }).fail(function (error) {
                    toastr.error("Debes agregar al menos una pregunta para poder enviar la encuesta", _app.constants.toastr.title.error);
                    $("#send-survey").removeLoader();
                });
            },
            events: function () {
                $(".btn-send").on('click', function () {
                    var id = SurveyId;
                    formSurvey.send.validate(id);
                });
            },
            init: function () {
                formSurvey.send.events();
            }
        },
        init: function () {
            formSurvey.send.init();
        }
    }
    var manageSection = {
        loadData: function () {
            $.ajax({
                type: "GET",
                url: `/admin/gestion-encuestas/items/get/${SurveyId}`.proto().parseURL(),
            }).done(function (data) {
                $("#section_container").html(data);
            });
        },
        events: function () {
            $("#manage_section").on('click', '.btn-add',
                function () {
                    formSection.create.show();
                });
            $("#manage_section").on('click', '.btn-add-question',
                function () {
                    var id = $(this).data('id');
                    $.ajax({
                        url: `/admin/gestion-encuestas/seccion/${id}/validar-likert`.proto().parseURL(),
                        type: "GET",
                    }).done(function (result) {
                        if (result.isLikert) {
                            formLikertQuestion.create.show(id);
                        } else {
                            formQuestion.create.show(id);
                        }
                    }).fail(function (error) {
                        window.location.href = window.location.href
                    });
                });
            $("#manage_section").on('click', '.delete',
                function () {
                    var id = $(this).data("id");
                    formQuestion.delete(id);
                });
            $("#manage_section").on('click', '.btn-edit-section',
                function () {
                    var id = $(this).data('id');
                    formSection.edit.show(id);
                });
            $("#manage_section").on('click', '.delete-item',
                function () {
                    var id = $(this).data("id");
                    formSection.delete(id);
                });
            $("#manage_section").on('click', '.edit',
                function () {
                    var id = $(this).data("id");

                    $.ajax({
                        url: `/admin/gestion-encuestas/pregunta/${id}/validar-likert`.proto().parseURL(),
                        type: "GET",
                    }).done(function (result) {
                        if (result.isLikert) {
                            formLikertQuestion.edit.show(id);
                        } else {
                            formQuestion.edit.show(id);
                        }
                    }).fail(function (error) {
                        window.location.href = window.location.href
                    });

                });
        },
        init: function () {
            manageSection.loadData();
            formQuestion.events();
            manageSection.events();

        }
    };
    var datepicker = {
        init: function () {
            $("#PublicationDate").datepicker()
                .on("changeDate", function (e) {
                    $("#FinishDate").datepicker("setStartDate", e.date);
                });
            $("#FinishDate").datepicker()
                .on("changeDate", function (e) {
                    $("#PublicationDate").datepicker("setEndDate", e.date);
                });
        }
    };
    var select2 = {
        init: function () {
            this.questionType.init();
        },
        questionType: {
            init: function () {
                $.ajax({
                    url: ("/tipospregunta/get").proto().parseURL()
                }).done(function (result) {
                    $(".select2-questionType").select2({
                        data: result.items,
                        minimumResultsForSearch: -1
                    });
                });
            }
        }
    };
    return {
        init: function () {
            manageSection.init();
            select2.init();
            formLikertQuestion.init();
            formSection.init();
            formSurvey.init();
            formSurvey.edit.load(SurveyId);
            datepicker.init();
        }
    };
}();

$(function () {
    InitApp.init();
});
