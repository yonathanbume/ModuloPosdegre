var postulantPayment = function () {
    var datatable = null;
 
    var loadDatatable = function () {

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        //options.data.source.read.url = `/pago_postulante/get`.proto().parseURL();
        datatable = $(".m-datatable").mDatatable(options);
        datatable.on('click', '.btn-success', function () {
            var id = $(this).data("id");            
            $.ajax({
                type: "POST",
                url: `/pago_postulante/recibo/generar/${id}`.proto().parseURL(),
                success: function (data) {
                    window.location.href = `/pago_postulante/recibo/${data.id}/${data.pid}`.proto().parseURL();
                }
            });
        });

    }       
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/pago_postulante/get`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'dni',
                title: 'DNI',
                width: 90
            },
            {
                field: 'fullname',
                title: 'Nombres Completos',
                width: 180
            },            
            {
                field: 'registerdate',
                title: 'Fecha de registro',
                width: 150
            },
            {
                field: 'postulantto',
                title: 'Carrera a postular',
                width: 180
            },
            {
                field: 'modality',
                title: 'Modalidad de postulante',
                textAlign: "center",
                width: 180
            },
            {
                field: 'status',
                title: 'Estado',
                width: 100         
            },
            {
                field: 'Pagar',
                title: 'Pagar',
                textAlign : 'center',
                width: 110,
                template: function (row) {
                    if (row.status == "Pagó") {
                        return `<span class="m-badge  m-badge--primary m-badge--wide">Cancelado</span>`
                    } else {
                        return `<button type="button" data-id="${row.id}" data-uname="${row.username}" class="m-btn btn btn-success"><i class="la la-check"></i></button>`
                    }
                    }
                    
            }
        ]
    }

    

    return {
        load: function () {
            loadDatatable();
            
        }
    }
}();

$(function () {
    postulantPayment.load();
})