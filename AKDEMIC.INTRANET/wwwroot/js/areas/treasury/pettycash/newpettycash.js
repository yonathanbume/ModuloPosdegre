var SerialSelect = function () {

    var select = $("#serial_select");
    var correlative = $("#correlative");

    var loadCorrelative = function (id) {
        $.ajax({
            url: ("/caja/series/correlativo/get/" + id).proto().parseURL()
        }).done(function (data) {
            correlative.val(data);
        }).fail(function () {
            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
        });
    }

    var select2Initializer = function() {
        return $.ajax({
            url: "/caja/series/get".proto().parseURL()
        }).done(function (data) {
            var $select2 = select.select2({
                placeholder: "Nro. de serie",
                data: data.items,
                minimumResultsForSearch: -1
            });
            
            $select2.data("select2").$container.addClass("input-group-select2");

            select.on("change", function () {
                $(this).valid();
                loadCorrelative(select.val());
            });
        }).fail(function () {
            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
        });
    }

    return {
        init: function () {
            select2Initializer();
        },
        reload: function () {
            select.empty();
            select2Initializer().done(function() {
                loadCorrelative(select.val());
            });
        }
    };
}();

var NewSerialForm = function() {
    var form;

    var formInitializer = function() {
        form = $("#serial-form").validate({
            submitHandler: function (e) {
                $(".btn-submit").addLoader();

                $.ajax({
                    url: $(e).attr("action"),
                    type: "POST",
                    data: $(e).serialize()
                }).done(function() {
                    $("#create_serial").modal("hide");
                    SerialSelect.reload();

                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                }).fail(function(e) {
                    if (e.responseText !== null && e.responseText !== "") $("#create_msg_txt").html(e.responseText);
                    else $("#create_msg_txt").html(_app.constants.toastr.message.error.task);
                    $("#create_msg").removeClass("m--hide").show();
                }).always(function() {
                    $(".btn-submit").removeLoader();
                });
            }
        });

        $("#create_serial").on("hidden.bs.modal",
            function() {
                $("#create_msg").addClass("m--hide");
                form.resetForm();
            });
    }

    return {
        init: function() {
            formInitializer();
        },
        reset: function() {
            form.resetForm();
        }
    }
}();

var PettyCashForm = function() {
    var form;

    var formInitializer = function() {
        form = $("#pettycash-form").validate();
    }
    return {
        init: function () {
            formInitializer();
        },
        reset: function() {
            form.resetForm();
        }
    }
}();

jQuery(document).ready(function () {
    SerialSelect.init();
    PettyCashForm.init();
    NewSerialForm.init();
});

