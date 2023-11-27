const Datatable = function () {

    const id = $("#id").val();

    const teacher = {
        init: function () {
            $.when(
                $.ajax({
                    url: `/alumno/evaluacion-docente/encuesta/info/${id}`.proto().parseURL()
                })
            ).then(function (data) {
                $(".teacher-sex").text(data.sex);
                $(".teacher-name").text(data.name);
                $(".teacher-date").text(data.date);
                $(".teacher-career").text(data.academicDepartment);
            });
        }
    };

    return {
        init: function () {
            teacher.init();
        }
    };
}();

var Form = function () {
    const formCreateId = "#survey";

    var form = {
        submit: function (formElements) {
            var formData = new FormData($(formElements)[0]);
            $(`${formCreateId} input, ${formCreateId} button`).attr("disabled", true);
            $.ajax({
                url: $(formElements).attr("action"),
                type: "POST",
                data: formData,
                contentType: false,
                processData: false
            }).done(function (e) {
                swal({
                    type: "success",
                    title: "Completado",
                    text: "Evaluación enviada correctamente",
                    confirmButtonText: "Excelente"
                }).then(function (isConfirm) {
                    if (isConfirm) {
                        location.href = "/";
                        //location.href = e;
                    }
                });
            }).fail(function (e) {
                $(`${formCreateId} input, ${formCreateId} button`).attr("disabled", false);
                swal({
                    type: "error",
                    title: "Error",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    confirmButtonText: "Entendido",
                    text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                });
            });
        }
    };

    var events = {
        init: function () {
            $(".btn-back").click(function () {
                location.href = "/";
                //location.href = `/alumno/evaluacion-docente/${FromAccount}`;
            });
        }
    };

    var validate = {
        init: function () {
            formCreateValidate = $(formCreateId).validate({
                //rules: validateRules,
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit(formElement);
                }
            });
        }
    };
    return {
        init: function () {
            validate.init();
            events.init();
        }
    };
}();

$(function () {
    Datatable.init();
    Form.init();
});