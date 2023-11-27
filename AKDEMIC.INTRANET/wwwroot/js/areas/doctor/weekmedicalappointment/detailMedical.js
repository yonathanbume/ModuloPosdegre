var MedicalCareDetail = function () {
    var pid = $("#Id").val();
    var datatable = null;
    var options = {
        data: {
            source: {
                read: {
                    type: 'GET',
                    url: `/doctor/horario-citas/detalle-historico-todos/${pid}`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'username',
                title: 'Código',
                width: 70
            },
            {
                field: 'fullname',
                title: 'Nombres Completos',
                width: 70
            },
            {
                field: 'doctor',
                title: 'Doctor Asignado',
                width: 150
            },
            {
                field: 'datemedicalcare',
                title: 'Fecha',
                width: 150
            },
            {
                field: "options",
                title: "Opciones",
                width: 200,
                sortable: false,
                filterable: false,
                template: function (row) {
                    return `<button data-id=${row.psychologicalid}  class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span> Ver Ficha </span></span></button>`;
                }
            }
        ]
    }

    var LoadDatatable = function () {
        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
       
        datatable = $(".m-datatable").mDatatable(options);

        datatable.on('click', '.btn-detail', function () {
            var pid = $(this).data("id");
            location.href = `/doctor/horario-citas/ficha/${pid}`.proto().parseURL();     
            
        });

    }
    //var SubmitAjax = function () {

    //    $("#medical-form").validate({
    //        submitHandler: function (form, event) {
    //            $.ajax({
    //                type: 'POST',
    //                url: ("/doctor/horario-citas/detalle-historico/post").proto().parseURL(),
    //                data: $("#medical-form").serialize(),
    //                success: function (data) {
    //                    toastr.success(_app.constants.toastr.message.success.update, _app.constants.toastr.title.update);
    //                },
    //                error: function () {
    //                    toastr.error(_app.constants.toastr.message.error.update, _app.constants.toastr.title.error);
    //                }
    //            });

    //        }
    //    });

    //}
    return {
        init: function () {            
            LoadDatatable();
            //SubmitAjax();
            
        }
    }
}();

$(function () {
    MedicalCareDetail.init();
});