var TeachingLoad = function () {
    var datatable = {
        projectDirector: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/Student/getallstudent",
                    type: "GET",
                },
                columns: [{ data: "", title: "Ciclo" },
                    { data: "", title: "Código" },
                    { data: "", title: "Curso" },
                    { data: "", title: "Créditos" },
                    { data: "", title: "Vez" },
                    { data: "", title: "Tipo" },
                    {
                        data: null,
                        title: "Opciones",
                        orderable: false,
                        render: function (data) {
                            var tpm = "";
                            tpm += `<input type="checkbox" title="Completar" class="btn-check btn m-btn m-btn--icon btn-sm m-btn--icon-only"><i class="la la-check"></i></input>`;
                            return tpm;
                        }
                    }
                ]
            },
            init: function () {
                datatable.projectDirector.object = $("#datatable_data_Matricula").DataTable(datatable.projectDirector.options);
            }
        },
        init: function () {
          
            datatable.projectDirector.init();
        }
    }

    $('#btn-student').click(function () {
        var dni = $("#startDateDni").val();
        $.ajax({
            url: `/admin/TeachingLoad/getallstudent/${dni}`,
            type: "Post",
        }).done(function (data) {
            $('#Codigop').val(data.codigo);
            $('#namepersonal').val(data.name);
            $('#apellidop').val(data.paternalSurname);
            $('#apellidom').val(data.maternalSurname);
            $('#AddMatricula').show();
          //  window.location.href = "https://localhost:7273/admin/master/matricula";
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
        Operacion: function () {
            return $.ajax({
                type: 'GET',
                url: `/admin/TeachingLoad/Semestre/get`.proto().parseURL()
            }).then(function (data) {
                $("#startNumeroOperacion").select2({
                    data: data.items
                });
            });
        }
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
         
            $('#mostrarTable').on('click', function (event) {
                event.preventDefault();
                    datatable.init();            
            });
            Promise.all([select.Semestre(), select.Master(), select.Operacion()]).then(() => {
               
                $("#semestreId").on('change', function () {
                    var semestre = $(this).val();
                    select.career(semestre);
                });
                $("#maestriaId").on('change', function () {
                    var maestria= $(this).val();
                    select.career(maestria);
                });
                $("#startNumeroOperacion").on('change', function () {
                    var operacion = $(this).val();
                    select.career(operacion);
                });
               
            });
        }
    };
}();

$(function () {
    TeachingLoad.load();
});




