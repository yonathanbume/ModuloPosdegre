var safeinfo = function () {

    var form = {
        repeater: {
            currentDetail : 1,
            init: function () {
                $('#m_repeater_1').repeater({
                    initEmpty: false,

                    defaultValues: {
                        'text-input': 'foo'
                    },

                    show: function () {
                        $(this).slideDown();
                        var inputs = $(this).find(":input");
                    },

                    hide: function (deleteElement) {
                        $(this).slideUp(deleteElement);
                    }
                });
            }
        },
        object: $("#main_form").validate({
            submitHandler: function (formElement, e) {
                e.preventDefault();
                $("#save_data").addLoader();
                var formData = new FormData($(formElement)[0]);

                var divs = $(".div_container_detail");

                var index = 0;
                $.each(divs, function (i, v) {
                    var term = $(v).find(":input[data-name='Term']").val();
                    var meritOrder = $(v).find(":input[data-name='MeritOrder']").val();
                    var weightedAverage = $(v).find(":input[data-name='WeightedAverage']").val();
                    var approvedCredits = $(v).find(":input[data-name='ApprovedCredits']").val();
                    var totalStudents = $(v).find(":input[data-name='TotalStudents']").val();
                    var observation = $(v).find(":input[data-name='Observation']").val();
                    var upperFifthTotalStudents = $(v).find(":input[data-name='UpperFifthTotalStudents']").val();
                    var upperThirdTotalStudents = $(v).find(":input[data-name='UpperThirdTotalStudents']").val();

                    formData.append(`Detail[${index}].Term`, term);
                    formData.append(`Detail[${index}].MeritOrder`, meritOrder);
                    formData.append(`Detail[${index}].WeightedAverage`, weightedAverage);
                    formData.append(`Detail[${index}].ApprovedCredits`, approvedCredits);
                    formData.append(`Detail[${index}].TotalStudents`, totalStudents);
                    formData.append(`Detail[${index}].Observation`, observation);
                    formData.append(`Detail[${index}].UpperFifthTotalStudents`, upperFifthTotalStudents);
                    formData.append(`Detail[${index}].UpperThirdTotalStudents`, upperThirdTotalStudents);
                    index++;
                });

                $.ajax({
                    url: `/registrosacademicos/solicitudes/guardar-archivo`,
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false
                })
                    .done(function (e) {
                        window.open(`/documentos/${e}`, "_blank");

                        swal({
                            type: "success",
                            title: "Completado",
                            text: "Archivo generado.",
                            allowOutsideClick: false,
                            confirmButtonText: "Aceptar"
                        }).then((result) => {
                            if (result.value) {
                                window.location.href = "/registrosacademicos/solicitudes";
                            }
                        });
                    })
                    .fail(function () {
                        swal({
                            type: "error",
                            title: "Error",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            confirmButtonText: "Aceptar",
                            text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                        });
                    })
                    .always(function () {
                        $("#save_data").removeLoader();
                    });

            }
        }),
        init: function () {
            this.repeater.init();
        }
    };


    return {
        init: function () {
            form.init();
        }
    };
}();

$(() => {
    safeinfo.init();
});