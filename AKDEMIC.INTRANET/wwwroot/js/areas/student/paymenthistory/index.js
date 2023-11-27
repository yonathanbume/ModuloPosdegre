var paymentHistory = function () {
    var datatable = null;
    var datatable_details = null;
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: "/alumno/historial_de_pagos/get".proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "type",
                title: "Tipo",
                width: 150,
                template: function (row) {
                    return `<span class="m-badge m-badge--brand m-badge--wide">${row.type}</span>`;
                }
            },
            {
                field: "concept",
                title: "Concepto"
            },
            {
                field: 'code',
                title: 'Código',
                width: 150
            },
            {
                field: 'import',
                title: 'Importe',
                width: 150
            },
            {
                field: 'paymentDate',
                title: 'Fecha de pago',
                width: 150
            }
        ]
    };

    var options_details = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ""
                }
            }
        },
        columns: [
            //{
            //    field: 'numDocument',
            //    title: 'Num. Documento',
            //    width: 150
            //},
            {
                field: 'concept',
                title: 'Concepto',
                width: 200
            },
            {
                field: 'amount',
                title: 'Monto',
                width: 150
            }
        ]
    }

    var loadDatatable = function () {
        //var pid = $("#select_terms").val();        

        //if (datatable !== null) {
        //    datatable.destroy();
        //    datatable = null;
        //}
        //options.data.source.read.url = `/alumno/historial_de_pagos/get/periodo/${pid}`.proto().parseURL();    

        datatable = $(".m-datatable").mDatatable(options);

        datatable.on('click', '.btn-detail', function () {
            var id = $(this).data("id");
            $("#payment_historial_details").modal('show');
            loadDatatable_details(id);
        });
    };

    var loadDatatable_details = function (id) {
        if (datatable_details !== null) {
            datatable_details.destroy();
            datatable_details = null;
        }
        options_details.data.source.read.url = `/alumno/historial_de_pagos/detalles/${id}`.proto().parseURL();
        datatable_details = $(".m-datatable-detail").mDatatable(options_details);
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
    paymentHistory.load();
});