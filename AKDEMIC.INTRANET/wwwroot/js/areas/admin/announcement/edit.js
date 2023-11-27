$(function () {
    $.ajax({
        url: ("/roles_todos/get").proto().parseURL()
    }).done(function (data) {
        $(".select2-roles").select2({
            placeholder: "Roles",
            minimumInputLength: 0,
            data: data.items
        });
        $(".select2-roles").val(RolesEdit).trigger('change');

    });

});