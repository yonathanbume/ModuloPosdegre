var TeachingLoad = function () {
    $('#btn-student').click(function () {
        var dni = $("#startDateDni").val();
        alert(dni);
        $.ajax({
            url: `/admin/TeachingLoad/getallstudent/${dni}`,
            type: "Post",

        }).done(function (data) {
            $('#AddMatricula').modal('show');
        }).fail(function (jqXHR, textStatus, errorThrown) {
            // Manejar el error aquí
            console.error("Error en la solicitud AJAX:", errorThrown);
            // Por ejemplo, mostrar un mensaje de error al usuario
            alert("Error al cargar estudiantes. Por favor, inténtalo de nuevo más tarde.");
        });
    });
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


$(() => {
    TeachingLoad.load();

});



