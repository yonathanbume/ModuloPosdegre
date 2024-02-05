var TeachingLoad = function () {

    var select = {
     
        Semestre: function () {
            return $.ajax({
                type: 'GET',
                url: `/admin/TeachingLoad/Semestre/get`.proto().parseURL()
            }).then(function (data) {
                $("#semestreId").select2({
                    data: data.items
                });
            });
        },
        Master: function () {
            return $.ajax({
                type: 'GET',
                url: `/admin/TeachingLoad/Master/get`.proto().parseURL()
            }).then(function (data) {
                $("#maestriaId").select2({
                    data: data.items
                });
            });
        },
    };
    return {
        load: function () {
            Promise.all([select.Semestre(), select.Master()]).then(() => {
               
                $("#semestreId").on('change', function () {
                    var facultyId = $(this).val();
                    select.career(facultyId);
                });
                $("#maestriaId").on('change', function () {
                    var facultyId = $(this).val();
                    select.career(facultyId);
                });
            });
        }
    };
}();

$(function () {
    TeachingLoad.load();
});





