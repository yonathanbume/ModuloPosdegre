var InvoiceTable = function () {
    var datatable;
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ("/caja/recibos/get/" + $("#pettycash").val()).proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "number",
                title: "Nro. de recibo"
            },
            {
                field: "client",
                title: "Cliente"
            },
            {
                field: "amount",
                title: "Monto Total"
            },
            {
                field: "state",
                title: "Estado",
                template: function (row) {
                    if (row.state) return '<span class="m-badge m-badge--danger m-badge--wide">Anulado</span>';
                    else return '<span class="m-badge m-badge--success m-badge--wide">Pagado</span>';
                }
            }
        ]
    }
    return {
        init: function () {
            datatable = $(".m-datatable").mDatatable(options);
        }
    }
}();

jQuery(document).ready(function () {
    InvoiceTable.init();
});