var edit = function () {

    var form = {
        object: $("#main_form").validate({
            submitHandler: function (formElement, e) {
                e.preventDefault();
                var formData = new FormData(formElement);
                $("#main_form").find(":input").attr("disabled", true);

                $.ajax({
                    url: "/admin/constancias/editar",
                    method: "POST",
                    data: formData,
                    contentType: false,
                    processData: false,
                })
                    .done(function (e) {
                        swal({
                            type: "success",
                            title: "Completado",
                            text: "Documento actualizado con éxito",
                            confirmButtonText: "Excelente"
                        });
                    })
                    .fail(function (e) {
                        swal({
                            type: "error",
                            title: "Error",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            confirmButtonText: "Entendido",
                            text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                        });
                    })
                    .always(function () {
                        $("#main_form").find(":input").attr("disabled", false);
                    });
            }
        })
    }

    var summernote = {
        content: {
            object: null,
            init: function () {
                this.object = $("#Content").summernote({
                    height: 450,
                    toolbar: [
                        ['style', ['bold', 'italic', 'underline']],
                        ['fontsize', ['fontsize']],
                        ['color', ['color']],
                        ['para', ['ul', 'ol', 'paragraph']],
                        ['height', ['height']]
                    ]
                });
            }
        },
        init: function () {
            summernote.content.init();
        }
    }

    return {
        init: function () {
            summernote.init();
        }
    }
}();

$(() => {
    edit.init();
});