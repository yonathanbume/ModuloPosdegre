InitApp = function () {

    var modal = function () {
        $("#add_modal_button").on('click', function () {
            $('#addModal').modal('show');
        });
    };

    var select = function () {
        $("#type_a").select2();
        $.ajax({
            type: 'GET',
            url: `/welfare/sugerencias-y-tips/categorias-bienestar-select2`.proto().parseURL(),
            success: function (dataResponse) {
                $("#welfareCategorySelect2").select2({
                    data: dataResponse
                });

                dataResponse.splice(0, 1);
                if ($("#welfareCategorySelect2_a").length) {
                    $("#welfareCategorySelect2_a").select2({
                        data: dataResponse,
                        dropdownParent: $('#addModal')
                    });
                }
                
            }
        });
        $("#welfareCategorySelect2").on('change', function () {
            suggestion_and_tips.init();
        });
    };

    var suggestion_and_tips = {
        select2: function () {
            select();
        },
        init: function () {
            mApp.block("#body_container_data");

            $.ajax({
                url: "/welfare/sugerencias-y-tips/vista".proto().parseURL(),
                data: {
                    welfareCategoryId: $("#welfareCategorySelect2").val()
                }
            }).done(function (data) {
                $("#body_container_data").html(data);
                $("#list-tabs li a.active").removeClass('active');
                $("#first_tab").addClass('active');     
                events();

            }).fail(function () {
                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
            }).always(function () {
                mApp.unblock("#body_container_data");
                
            });
        }
    };

    var validate = function () {
        $("#add-form").validate({
            submitHandler: function (form) {
                var btn = $(form).find("button[ type='submit']");
                btn.addLoader();
                $.ajax({
                    type: "POST",
                    url: `/welfare/sugerencias-y-tips/agregar-sugerencia-tip`.proto().parseURL(),
                    data: $(form).serialize(),
                    success: function () {
                        $("#addModal").modal('hide');
                        suggestion_and_tips.init();
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    },
                    error: function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    },
                    complete: function () {
                        btn.removeLoader();
                        $("#add-form").validate().resetForm();
                    }
                });
            }

        });
    };

    var events = function () {
        $(".touchable").on('click', function () {
            var currentId = $(this).data('id');
            window.open(`/welfare/sugerencias-y-tips/detalle/${currentId}`, '_blank');
        });
    };

    return {
        init: function () {
            suggestion_and_tips.init();
            suggestion_and_tips.select2();
            validate();
            modal();
            
        }
    };
}();

$(function () {
    InitApp.init();
});