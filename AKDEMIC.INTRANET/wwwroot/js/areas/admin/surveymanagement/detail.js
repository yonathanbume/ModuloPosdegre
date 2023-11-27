var InitApp = function () {
    var SurveyUserId = $("#SurveyUserId").val();
    var FormSurvey = {        
        load: function () {
            mApp.block("#questions");
            $.ajax({
                url: `/admin/gestion-encuestas/encuestas/${SurveyUserId}/preguntas`.proto().parseURL()
            }).done(function (data) {
                $("#questions").html(data);
            }).fail(function () {
                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
            }).always(function () {
                mApp.unblock("#questions");
            });
        },
        init: function () {
            FormSurvey.load();
        }
    };
    return {
        init: function () {
            FormSurvey.init();
        }
    }
}();
$(function () {
    InitApp.init();
});



