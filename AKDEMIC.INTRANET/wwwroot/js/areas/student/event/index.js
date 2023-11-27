var Events = function () {
    
    var table = $('#table-events');

    var divInitRow = '<tr class=""><td><div class="row row-event">';
    var divCloseRow = '</tr></td></div>';

    var loadSelect = function () {
        return $.ajax({
            url: ("/alumno/eventos/getEventTypes").proto().parseURL()
        }).done(function (data) {
            $("#select_eventtype").select2({
                data: data.eventTypes
            }).on("change", function (e) { 
                $(".pager").remove();
                $("#table-events tbody").empty();
                tablejs.init();
            });
        });
    } 

    var tablejs = { 
        paginate: function () {
            var currentPage = 0;
            var numPerPage = 3; 
            table.bind('repaginate', function () {
                table.find('tbody tr').hide().slice(currentPage * numPerPage, (currentPage + 1) * numPerPage).show();
            });

            table.trigger('repaginate');
            var numRows = table.find('tbody tr').length;
            console.log(numRows);
            var numPages = Math.ceil(numRows / numPerPage);
            var pager = $('<div class="pager"></div>');
            for (var page = 0; page < numPages; page++) {
                $('<span class="page-number"></span>').text(page + 1).bind('click', {
                    newPage: page
                }, function (event) {
                    currentPage = event.data['newPage'];
                    table.trigger('repaginate');
                    $(this).addClass('active').siblings().removeClass('active');
                }).appendTo(pager).addClass('clickable');
            }
            pager.insertAfter(table).find('span.page-number:first').addClass('active');

            $(".btn-inscription").on('click', function () {
                var id = $(this).data('id');
                swal({
                    title: "¿Está seguro?",
                    text: "El estudiante será inscrito",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, inscribir",
                    confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: "/alumno/eventos/agregar",
                                type: "POST",
                                data: {
                                    id: id
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El estudiante ha inscrito con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "No se pudo registrar la inscripción, es posible que ya esté registrado."
                                    });
                                }
                            });
                        })
                    }
                }); 
            });

            $(".btn-read-more").on('click', function () {
                var id = $(this).data('id'); 
                $.ajax({
                    url: "/alumno/eventos/getEvent",
                    type: "POST",
                    data: {
                        id: id
                    },
                    success: function (result) {
                        console.log(result);
                        var div = '';
                        div += '<div class="m-widget19__content">';
                        div += '    <div class="m-widget19__header">';
                        div += '        <div class="m-widget19__stats">';
                        div += '            <span class="m-widget19__number m--font-brand">';
                        div += '                <i class="flaticon-calendar-2"></i>';
                        div += '            </span>';
                        div += '            <span class="m-widget19__comment">';
                        div += result.eventDate;
                        div += '            </span >'; 
                        div += '        </div>';
                        div += '    </div>';
                        div += '    <div class="m-widget19__body">';
                        div += '        <label> Inscripciones</label>';
                        div += '        <span class="m-widget19__comment">';
                        div += result.registrationStartDate + ' - ' + result.registrationEndDate;
                        div += '        </span ><br />';
                        div += '        <label>Creado por:</label>';
                        div += '        <span> ' + result.creator + '</span>'; 
                        div += '        <br />';
                        div += '        <label>';
                        div += result.description;
                        div += '</label>';
                        div += '    </div>';
                        div += ' </div>';
                        $("#detail_modal .modal-body").empty();
                        $("#detail_modal .modal-body").append(div);
                    },
                    error: function (errormessage) { 
                    }
                }); 
            });
        },         
        init: function () {

            $.ajax({
                url: ('/alumno/eventos/getEvents/' + $("#select_eventtype").val()).proto().parseURL(),
                type: 'POST',
                success: function (events) { 
                    var div = '';
                    for (var key = 0; key < events.length; key++) { 
                        if (key % 3 == 0) { 
                            div += divInitRow;
                        } 
                        div += '<div class="col-xl-4">';
                        div += '<div class="m-portlet m-portlet--bordered-semi m-portlet--full-height ">';
                        div += '    <div class="m-portlet__head m-portlet__head--fit">';
                        div += '        <div class="m-portlet__head-caption">';
                        div += '            <div class="m-portlet__head-action">';
                        div += '                <button type="button" class="btn btn-sm m-btn--pill btn-brand btn-event-type" style="background-color:' + events[key].eventTypeColor + '">';
                        div +=                  events[key].eventTypeName;
                        div += '                </button>';
                        div += '            </div>';
                        div += '       </div>';
                        div += '    </div>';
                        div += '    <div class="m-portlet__body">';
                        div += '    <div class="m-widget19">';
                        div += '        <div class="m-widget19__pic m-portlet-fit--top m-portlet-fit--sides" style="min-height-: 286px">';
                        div += '            <img src="' + events[key].pathPicture + '" alt="">';
                        div += '            <h3 class="m-widget19__title m--font-light" style="display:block">' + events[key].eventName + '</h3>';
                        div += '            <div class="m-widget19__shadow"></div>';
                        div += '        </div>';
                        div += '    <div class="m-widget19__content">';
                        div += '        <div class="m-widget19__header">';
                        div += '            <div class="m-widget19__stats">';
                        div += '                <span class="m-widget19__number m--font-brand"><i class="flaticon-calendar-2"></i></span>';
                        div += '                <span class="m-widget19__comment">'+events[key].eventDate+'</span>';
                        div += '            </div>';
                        div += '        </div>';
                        div += '    <div class="m-widget19__body">';
                        var description = events[key].description.slice(NaN, 120);
                        if (events[key].description.length > 120) {
                            description += "...";
                        }
                        div +=          description;
                        div += '    </div>';
                        div += '    </div >';
                        div += '    <div class="m-widget19__action">';
                        div += '        <button data-id="' + events[key].id + '" type="button" class="btn m-btn--pill btn-secondary m-btn m-btn--custom btn-read-more" data-toggle="modal" data-target="#detail_modal">';
                        div += '            Ver';
                        div += '        </button>&nbsp;';
                        div += '        <button data-id="' + events[key].id + '" type="button" class="btn m-btn--pill btn-success btn-inscription">';
                        div += '            Inscribirse';
                        div += '        </button>';
                        div += '    </div>'; 
                        div += '   </div >';
                        div += ' </div >';
                        div += ' </div >';
                        div += '</div >';
                        if (key % 3 == 2 || key == events.length-1) {
                            div += divCloseRow;
                        }
                         
                    }
                    $("#table-events tbody").append(div);
                    tablejs.paginate();
                } 
            });   
        }
    }

    return {
        init: function () {
            tablejs.init(); 
            loadSelect();
        }
    }
}();
 

$(function () {
    Events.init(); 
});

