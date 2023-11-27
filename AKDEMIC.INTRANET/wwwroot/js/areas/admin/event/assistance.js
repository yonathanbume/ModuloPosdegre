var assistance = function () {
    var datatable = null;  
    var eid = $("#Id").val();    
    var options = {
        data: {
            type: "remote",
            source: {
                read: {
                    method: "GET",
                    url: `/admin/eventos/asistencia/detalle/${eid}`.proto().parseURL()
                }
            }
        },     
        sortable: false,
        pagination: false,
        columns: [
            {
                field: "assistance",
                title: "Inasistencia",
                width: 90,
                textAlign: "center",
                template: function (row,index) {
                    var tmp = ``;
                    tmp += `<label class='m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand'>`;
                    tmp += `<input data-id='${row.id}' type='checkbox' name='${index}' value='true' ${row.assistance
                        ? "checked"
                        : ""}/>`;
                    tmp += `<input name='${index}' type='hidden' value='false' />`;
                    tmp += "<span></span></label>";
                    return tmp;
                }              
            },
            {
                field: 'fullname',
                title: 'Nombres Completos',
                width: 250  
            },
            {
                field: 'email',
                title: 'Email',
                width: 250
            }
        ]
    }

    var events = function () {

        $("#btn-save").on('click', function () {
            var asistanceArray = [];
            $("input:checkbox").each(function () {
                var json_object = {
                    eventId: eid,
                    id: $(this).data("id"),
                    absent: $(this).is(":checked")
                };
                asistanceArray.push(json_object);
            });                          
            $("#btn-save").addLoader();
            $.ajax({
                type: "POST",
                url: `/admin/eventos/asistencia/detalle/${eid}/post`.proto().parseURL(),
                data: {
                    data: {
                        List: asistanceArray
                    }
                },                
                success: function () {
                    toastr.success(_app.constants.toastr.message.success.update, _app.constants.toastr.title.success);
                    datatable.reload();
                    $("#btn-save").removeLoader();
                },
                error: function () {
                    toastr.error(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                    $("#btn-save").removeLoader();
                }
            });
            
        });
    }
  
    var loadDatatable = function () {

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }

        datatable = $(".m-datatable").mDatatable(options);
        events();    

    }  
    return {
        load: function () {
            
            loadDatatable();
        }
    }
}();

$(function () {

    assistance.load();
})