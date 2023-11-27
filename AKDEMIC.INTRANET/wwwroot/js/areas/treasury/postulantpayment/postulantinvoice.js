var PostulantInvoiceDetailTable = function () {
    var datatable;
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ("/pagos/recibo/detalle/get/" + $("#invoice").val()).proto().parseURL()
                }
            }
        },
        sortable: false,
        pagination: false,
        columns: [
            {
                field: "concept",
                sortable: false,
                filterable: false,
                title: "Concepto"
            },
            {
                field: "cost",
                title: "Costo",
                sortable: false,
                filterable: false,
                template: function (row, index) {

                    if ($("#canceled").val()) return "<span class='m--font-danger'> S/. " + row.cost.toFixed(2) + "</span>";

                    return '<input hidden name="detail[' + index + '].Id" value="' + row.id + '"><div class="input-group btn--icon-only" style="max-width: 200px" ><input type="text" name="detail[' + index + '].Cost" class="form-control m-detail-value m--align-right input-cost" placeholder= "0.00" value="' + row.cost.toFixed(2) + '" disabled></div >';
                }
            }
        ]
    }

    return {
        init: function () {
            datatable = $("#detail_table").mDatatable(options);
        },
        reload: function () {
            datatable.reload();
        }
        
    }
}();

var VoucherTable = function () {
    var datatable;
    var source = [];
    var voucherForm;

    var options = {
        sortable: false,
        pagination: false,
        columns: [
            {
                field: "code",
                title: "Código"
            },
            {
                field: "cost",
                title: "Monto",
                textAlign: "right",
                template: function (row) {
                    var cost = Number(row.cost);
                    return "S/. " + cost.toFixed(2);
                }
            },
            {
                field: "options",
                title: " ",
                textAlign: "right",
                template: function (row, index) {
                    return '<input hidden name="Vouchers[' + index + '].Code" value="' + row.code + '"><input hidden name="Vouchers[' + index + '].amount" value="' + row.cost + '"><button onclick="VoucherTable.deleteRow(' + index + ')" type="button" class="btn btn-danger m-btn btn-sm m-btn m-btn--icon btn-delete-voucher"' + ($("#canceled").val() ? " disabled" : "") + '><span><i class="la la-trash"></i><span>Eliminar</span></span> </button>';
                }
            }
        ]
    }

    if ($("#canceled").val()) {
        options.data = {
            source: {
                read: {
                    method: "GET",
                    url: ("/pagos/recibo/vouchers/get/" + $("#invoice").val()).proto().parseURL()
                }
            }
        }
    }
    else {
        options.data = {
            type: "local",
            source: source,
            pageSize: 10
        }
    }

    var formInitializer = function () {
        voucherForm = $("#voucher_form").validate({
            submitHandler: function () {
                var code = $("input[name=code]").val();
                var amount = $("input[name=amount]").val();

                VoucherTable.addRow(code, amount);

                $("#voucher_modal").modal("hide");
                voucherForm.resetForm();
            }
        });
    }

    var deleteRow = function (id) {
        VoucherTable.removeRow(id);
    }

    return {
        init: function () {
            datatable = $("#voucher_table").mDatatable(options);

            $("input[type=radio][name=PaymentType]").change(function () {
                if (this.value == 2) {
                    $("#m_datatable_vouchers").collapse("show");
                }
                else {
                    $("#m_datatable_vouchers").collapse("hide");
                }
            });

            formInitializer();
        },
        addRow: function (code, value) {
            source.push({ code: code, cost: value });
            datatable.originalDataSet = source;
            datatable.reload();
        },
        removeRow: function (id) {
            source.splice(id, 1);
            datatable.originalDataSet = source;
            datatable.reload();
        },
        getCount: function () {
            return datatable.getTotalRows();
        },
        reload: function () {
            datatable.reload();
        },
        deleteRow: function (id) {
            deleteRow(id);
        }
    }
}();

var InvoiceForm = function () {
    var form;

    var formInitializer = function () {
        form = $("#invoice-form").validate({
            submitHandler: function (form) {
                if ($("input[name=PaymentType]:checked").val() === 2) {
                    var count = VoucherTable.getCount();
                    if (count === 0) {
                        swal("No existen vouchers de pago", "Debe ingresar por lo menos un voucher de pago para generar el recibo", "error");
                        return;
                    }
                }
                form.submit();
                $(":input").prop("disabled", true);
                return;
            }
        });
    }

    return {
        init: function () {
            formInitializer();
        }
    }
}();


jQuery(document).ready(function () {
    PostulantInvoiceDetailTable.init();
    VoucherTable.init();
    InvoiceForm.init();
});


