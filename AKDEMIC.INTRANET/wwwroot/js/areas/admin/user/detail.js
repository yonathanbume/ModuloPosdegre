

var Form = function () {
    var formValidate = null;

    var select2 = {
        dependencies: {
            init: function () {
                $.ajax({
                    url: ("/dependencias/get").proto().parseURL()
                }).done(function (data) {
                    $(".select2-dependencies").select2({
                        placeholder: "Dependencias",
                        minimumInputLength: 0,
                        data: data.items
                    });

                    select2.dependencies.load();
                });
            },
            load: function () {
                var id = $("#Id").val();
                mApp.block(".m-content", { type: "loader", message: "Cargando..." });
                $.ajax({
                    url: ("/admin/usuarios/" + id + "/dependencias/get").proto().parseURL()
                }).done(function (data) {
                    var selectedValues = [];
                    data.items.forEach(function (item) {
                        selectedValues.push(item.id);
                    });
                    $(".select2-dependencies").val(selectedValues).trigger("change");
                }).always(function (result) {
                    mApp.unblock(".m-content");
                });
            }
        },
        roles: {
            init: function () {
                $.ajax({
                    url: ("/roles/get").proto().parseURL()
                }).done(function (data) {
                    $(".select2-roles").select2({
                        placeholder: "Roles",
                        minimumInputLength: 0,
                        data: data.items
                    });

                    select2.roles.load();
                });
            },
            load: function () {
                var id = $("#Id").val();
                mApp.block(".m-content", { type: "loader", message: "Cargando..." });
                $.ajax({
                    url: ("/admin/usuarios/" + id + "/roles/get").proto().parseURL()
                }).done(function (data) {
                    data.items.forEach(function (item) {
                        var opt = new Option(item.text, item.id, true, true);
                        $(".select2-roles").append(opt).trigger("change");
                    });
                }).always(function (result) {
                    mApp.unblock(".m-content");
                });
            }
        }
    }

    return {
        init: function () {
            select2.dependencies.init();
            select2.roles.init();
        }
    }
}();

$(function () {
    Form.init();
});