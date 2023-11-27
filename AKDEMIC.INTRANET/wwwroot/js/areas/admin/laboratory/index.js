var RequestsTable = function () {
    var datatable;

    var formRequestValidation = $("#request-form").validate();

    $("#cSectionSearch").select2({
        width: "100%",
        dropdownParent: $("#request_modal"),
        placeholder: "Buscar una sección",
        ajax: {
            url: "/admin/laboratory/sections/select/filter/get",
            dataType: "json",
            data: function (params) {
                return {
                    term: params.term,
                    page: params.page
                };
            },
            processResults: function (data, params) {
                return {
                    results: data.items
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) {
            return markup;
        },
        minimumInputLength: 3
    });

    $("#Date").datepicker({
        minDate: moment()
    });

    $("#Date").datepicker("setStartDate", new Date())

    $("#TimeEnd, #TimeStart").timepicker();

    var options = {
        search: {
            input: $('#search')
        },
        data: {
            type: 'remote',
            source: {
                read: {
                    method: 'GET',
                    url: (('/admin/laboratory/get/')).proto().parseURL(),
                },
            },
            pageSize: 10,
            saveState: {
                cookie: true,
                webstorage: true
            }
        },
        columns: [
            {
                field: 'section',
                title: 'Sección',
                width: 190
            },
            {
                field: 'date',
                title: 'Fecha',
                width: 120
            },
            {
                field: 'dateRequest',
                title: 'Fecha a reservar',
                width: 120
            },
            {
                field: 'string_state',
                title: 'Estado',
                width: 120,
                template: function (row) {
                    if (row.state === 0) {
                        return '<span style="width: 100px;"><span class="m-badge m-badge--brand m-badge--wide">' + row.string_state + '</span></span>'
                    }
                    else if (row.state === 1) {
                        return '<span style="width: 100px;"><span class="m-badge  m-badge--success m-badge--wide">' + row.string_state + '</span></span>'
                    }
                    else if (row.state === 2) {
                        return '<span style="width: 100px;"><span class="m-badge  m-badge--danger m-badge--wide">' + row.string_state + '</span></span>'
                    }
                }
            },
            {
                field: 'options',
                title: 'Opciones',
                width: 300,
                sortable: false,
                filterable: false,
                template: function (row) {
                    if (!row.denied) {
                        return '<button data-id="' + row.id + '" class="btn btn-success btn-sm m-btn m-btn--icon btn-format" title="Formato"><span><i class="la la-check"></i><span>Formato ATS</span></span></button>'
                    }
                    else {
                        return ''
                    }

                }
            }
        ]
    };

    var events = {
        init: function () {
            $(".btn-add").on("click",
               function () {
                   modal.add();
               });

            datatable.on('click', '.btn-format', function () {
                var aux = $(this);
                var id = aux.data('id');
                window.open(`/admin/format/ats/${id}`.proto().parseURL(), "_blank");
            });
        }
    };

    var modal = {
        add: function () {
            $("#request_modal").modal("toggle");

            $("#request-form").find("input").prop("disabled", false);
            $("#request-form").find("select").prop("disabled", false);
            $("#request-form").find("textarea").prop("disabled", false);

            $("#request_modal").one("hidden.bs.modal",
                function (e) {
                    modal.reset();
                });
        },
        reset: function () {
            $("#request_form_msg").addClass("m--hide").hide();
            $("#cSectionSearch").val("").trigger('change');
            formRequestValidation.resetForm();
        },
        begin: function () {
            $("#request_modal input").attr("disabled", true);
            $("#request_modal select").attr("disabled", true);
            $("#btnSave").addLoader();
        },
        complete: function () {
            $("#request_modal input").attr("disabled", false);
            $("#request_modal select").attr("disabled", false);
            $("#btnSave").removeLoader();
        },
        success: function (e) {
            $('#request_modal').modal("toggle");
            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
            datatable.reload();
        },
        failure: function (e) {
            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
            if (e.responseText != null) $("#request_form_msg_txt").html(e.responseText);
            else $("#request_form_msg_txt").html(_app.constants.ajax.message.error);
            $("#request_form_msg").removeClass("m--hide").show();
        }
    };

    return {
        init: function () {
            datatable = $(".m-datatable").mDatatable(options);
            events.init();
        },
        Request: {
            begin: function () {
                modal.begin();
            },
            complete: function () {
                modal.complete();
            },
            success: function (e) {
                modal.success(e);
            },
            failure: function (e) {
                modal.failure(e);
            }
        }
    };
}();

$(function () {
    RequestsTable.init();
});