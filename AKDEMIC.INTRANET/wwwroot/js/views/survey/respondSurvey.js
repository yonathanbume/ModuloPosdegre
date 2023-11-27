var InitApp = function () {
    var SurveyUserId = $("#SurveyUserId").val();
    var FormSurvey = {
        send: {
            object: $("#survey-form").validate({
                submitHandler: function (form, e) {
                    e.preventDefault();
                    mApp.block("#questions");
                    var data = $(form).serialize();
                    $(".btn-send").addLoader();
                    $(`#survey-form input`).attr("disabled", true);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: data
                    }).always(function () {
                        mApp.unblock("#questions");
                        $(".btn-send").removeLoader();
                    }).done(function (e) {
                        $("#survey-form button").hide();
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        location.href = e;
                    }).fail(function (e) {
                        $(`#survey-form input`).attr("disabled", false);
                        toastr.error(e.responseText, _app.constants.toastr.title.error);
                    });
                }
            })
        },
        load: function () {
            mApp.block("#questions");
            $.ajax({
                url: `/encuestas/${SurveyUserId}/preguntas`.proto().parseURL()
            }).done(function (data) {
                $("#questions").html(data);
                $("#btnSend").prop('disabled', false);
                $("#btnCancel").prop('disabled', false);
            }).fail(function () {
                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
            }).always(function () {
                mApp.unblock("#questions");
            });
        },
        events: function () {
            $(".btn-cancel").click(function () {
                location.href = `/encuestas`.proto().parseURL();
            });
        },
        init: function () {
            FormSurvey.load();
            FormSurvey.events();
        }
    };
    return {
        init: function () {
            $("#btnSend").prop('disabled', true);
            $("#btnCancel").prop('disabled', true);
            FormSurvey.init();
        }
    }
}();
$(function () {
    InitApp.init();
});