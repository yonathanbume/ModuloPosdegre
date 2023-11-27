var pendingdebt = function () {
    var datatable = null;
    //var datatable_details = null;
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/alumno/deudas_pendientes/get`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "type",
                title: "Tipo",
                width: 100,
                template: function (row) {
                    return `<span class="m-badge m-badge--brand m-badge--wide">${row.type}</span>`;
                }
            },
            {
                field: 'code',
                title: 'Código',
                width: 70
            },
            {
                field: 'concept',
                title: 'Concepto'
            },
            {
                field: "issueDate",
                title: "Fecha emisión",
                width: 100
            },
            {
                field: 'import',
                title: 'Importe',
                width: 150,
                textAlign: "right",
                template: function (row) {
                    return "S/. " + row.import.toFixed(2);
                }
            },
            {
                field: 'studentPaymentFeature',
                title: 'Opciones',
                width: 150,
                textAlign: "right",
                template: function (row) {
                    var tmp = "";
                    if (row.studentPaymentFeature == true) {
                        tmp += `<a href="${row.paymentUrl}"  `;
                        tmp += "class='btn btn-primary ";
                        tmp += "m-btn btn-sm m-btn--icon' >";
                        tmp += "<span><i class='la la-credit-card'></i><span>Pagar</span></span></a> ";

                    }

                    if (row.userProcedureId != null) {
                        tmp += `<a target="_blank" href="/tramites/esquela-pago-pdf/${row.userProcedureId}" class="btn btn-primary m-btn m-btn--icon btn-sm m-btn--icon-only"><i class="la la-file-pdf-o"></i></a>`;
                    }

                    return tmp;
                }
            }
        ]
    };

    var loadDatatable = function () {
        datatable = $(".m-datatable").mDatatable(options);
    };

    //var loadTermsSelect = function () {
    //    $.ajax({
    //        url: `/periodos/get`.proto().parseURL()
    //    }).done(function (data) {
    //        $("#select_terms").select2({
    //            data: data.items
    //        }).val(data.selected).trigger('change');
    //        //$('#select_terms').prepend($('<option>', { value: 0, text: 'TODOS' }))
    //    });
    //    $("#select_terms").on('change', function () {
    //        loadDatatable();
    //    });
    //}

    return {
        load: function () {
            loadDatatable();
        }
    };
}();

$(function () {
    pendingdebt.load();
});