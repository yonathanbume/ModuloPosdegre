var TeachingLoad = function () {
    $('#btn-student').click(function () {
        var dni = $("#startDateDni").val();
        $.ajax({
            url: `/admin/TeachingLoad/getallstudent/${dni}`,
            type: "Post",

        }).done(function (data) {
         $('#startNumeroOperacion').val(data.dni); 
          //  modal.projectDirector.AddStudent.show(data);
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
    var modal = {
        projectDirector: {
            object: $("#AddMatricula"),
            AddStudent: {
                show: function (data) { 
                 /*modal.projectDirector.object.find(".modal-title").text("Regitrar un estudiante posgrado");
                    $("#add-matricula").attr("action", "/admin/Student/registrar");
                    $("#add-matricula").attr("data-message", "Registro actualizado con éxito");*/
                    modal.projectDirector.object.find("[name='Id']").val(data.id);
                    modal.projectDirector.object.find("[name='Codigo']").val(data.codigo);
                    modal.projectDirector.object.find("[name='Dni']").val(data.dni);
                    modal.projectDirector.object.find("[name='Nombre']").val(data.name);
                    modal.projectDirector.object.find("[name='ApellidoP']").val(data.paternalSurname);
                    modal.projectDirector.object.find("[name='ApellidoM']").val(data.maternalSurname);
                    //modal.projectDirector.object.find("[name='SemestreId']").val(select.Semestre());
                    modal.projectDirector.object.modal("show");
                }
            }
        }
    }
    return {
        load: function () {
            Promise.all([select.Semestre(), select.Master()]).then(() => {
               
                $("#semestreIdP").on('change', function () {
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



