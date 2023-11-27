var certificate = function () {
    var datatable = null;
    var sid = $("#Id").val();

    var loadDatatable = function () {
        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        $.ajax({
            url: `/academic-assistant/academic-assistant/scores/${sid}`.proto().parseURL()
        }).done(function (data) {
            var source = [];
            var columns = [];
            columns.push({ "field": 'estudiante', "title": 'Estudiante' });
            for (var k = 0; k < data.evaluations.length; k++)
                columns.push({ "field": "score" + (k + 1), "title": data.evaluations[k] });
            for (var i = 0; i < data.scores.length; i++) {
                source.push({ "alumno": data.scores[i].alumno });
                for (var j = 0; j < data.evaluations.length; j++)
                    source[i]["score" + (j + 1)] = data.scores[i].scores[j];
            }
            var options = {
                data: {
                    type: "local",
                    source: source
                },
                columns: columns
            };
            datatable = $("#datatable").mDatatable(options);
            });


        $("#notification").on('click', function () {
            $.ajax({
                url: `/academic-assistant/academic-assistant/notification/${sid}`.proto().parseURL()
            }).done(function () {
                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
            });
        });
    };
    return {
        load: function () {
            loadDatatable();
        }
    };
}();

$(function () {
    certificate.load();
});