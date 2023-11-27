var Events = function () {

    var CurrentEventId = null;
    var table = $('#table-events');

    var divInitRow = '<tr class=""><td><div class="row row-event">';
    var divCloseRow = '</tr></td></div>';

    var loadSelect = function () {
        return $.ajax({
            url: ("/inscripcion_eventos/getEventTypes").proto().parseURL()
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
                    text: "El usuario será inscrito",
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
                                url: "/inscripcion_eventos/agregar",
                                type: "POST",
                                data: {
                                    id: id
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El usuario ha sido inscrito con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                    location.reload();
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
                CurrentEventId = id;
                $.ajax({
                    url: "/inscripcion_eventos/getEvent",
                    type: "POST",
                    data: {
                        id: id
                    },
                    success: function (result) {
                        datatable.files.reload();
                        var modal = $("#detail_modal");
                        modal.find(".event-name").text(result.eventName);
                        modal.find(".event-date").text(result.eventDate);
                        modal.find(".event-image").attr("src", `/imagenes/${result.pathPicture}`);
                        modal.find(".event-range").text(`Desde : ${result.registrationStartDate} Hasta : ${result.registrationEndDate}`);
                        modal.find(".event-type").text(result.eventTypeName);
                        modal.find(".event-creator").text(result.creator);
                        modal.find(".event-description").text(result.description);

                        if (result.urlVideo != null && result.urlVideo != "") {
                            $(".url_container").removeClass("d-none");
                            modal.find(".event-urlvideo").attr("href",urlVideo);

                        } else {
                            $(".url_container").addClass("d-none");
                        }
                    },
                    error: function (errormessage) {
                    }
                });
            });
        },
        init: function () {
            $.ajax({
                url: ('/inscripcion_eventos/getEvents/' + $("#select_eventtype").val()).proto().parseURL(),
                type: 'POST',
                success: function (events) {
                    $("#table-events tbody").append(events);
                    tablejs.paginate();
                }
            });
        }
    }

    var datatable = {
        files: {
            object : null,
            options: {
                ajax: {
                    url: `/inscripcion_eventos/get-archivos`,
                    type: "GET",
                    data: function (data) {
                        data.eventId = CurrentEventId;
                    }
                },
                columns: [
                    {
                        data: "name",
                        title :"Nombre"
                    },
                    {
                        data: null,
                        title: "Archivo",
                        render: function (row) {
                            var tpm = `<a href="/documentos/${row.urlFile}" target="_blank" class="btn btn-primary btn-sm m-btn m-btn--icon" title="Descargar"><span><i class="la la-download"></i><span>Descargar</span></span></a> `;
                            return tpm;
                        }
                    }
                ]
            },
            reload: function () {
                if (datatable.files.object == null) {
                    datatable.files.object = $("#table_files").DataTable(datatable.files.options);
                } else {
                    datatable.files.reload();
                }
            }
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

