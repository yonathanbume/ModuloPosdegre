var index = function () {

    var datatable = {
        available_procedures: {
            object: null,
            options: {
                ajax: {
                    url: `/tramites/get-tramites-disponibles/datatable`,
                    data: function (data) {
                        data.search = $("#search_available_procedures").val()
                    }
                },
                columns: [
                    {
                        data: "name",
                        orderable : false,
                        title :"Nombre"
                    },
                    {
                        data: "amount",
                        orderable: false,
                        title : "Monto"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        orderable: false,
                        render: function (row) {
                            var tpm = "";
                            tpm += `<a href="/tramites/detalles/${row.id}" class="btn btn-primary m-btn btn-sm m-btn m-btn--icon"><span><i class="la la-eye"></i><span>Detalle</span></span></a>`;
                            return tpm;
                        }
                    }
                ]
            },
            reload: function () {
                datatable.available_procedures.object.ajax.reload();
            },
            init: function () {
                datatable.available_procedures.object = $("#table_available_procedures").DataTable(datatable.available_procedures.options);
            }
        },
        procedures_in_process: {
            object: null,
            options: {
                ajax: {
                    url: `/tramites/get-tramites-en-proceso/datatable`,
                },
                columns: [
                    {
                        data: "createdAt",
                        orderable: false,
                        title: "Fec. Solicitud"
                    },
                    {
                        data: "procedure",
                        orderable: false,
                        title: "Trámite"
                    },
                    {
                        data: "status",
                        orderable: false,
                        title: "Estado"
                    },
                    {
                        data: null,
                        orderable: false,
                        render: function (row) {
                            var tpm = ``;
                            tpm += `<a href="/tramites/detalles-solicitud/${row.id}" class="btn btn-primary m-btn btn-sm m-btn m-btn--icon"><span><i class="la la-eye"></i><span>Detalle</span></span></a> `;
                            tpm += `<button data-id="${row.id}" class="btn-delete btn btn-danger m-btn m-btn--icon btn-sm m-btn--icon-only"><i class="la la-trash" title="Eliminar"></i></button>`;

                            return tpm;
                        }
                    }
                ]
            },
            events: {
                onDelete: function () {
                    $("#table_procedures_in_process").on("click", ".btn-delete", function () {
                        var id = $(this).data("id");
                        swal({
                            title: "¿Está seguro?",
                            text: "La solicitud será eliminada permanentemente",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, eliminarla",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: `/tramites/eliminar-tramite?id=${id}`,
                                        type: "POST"
                                    })
                                        .done(function (e) {
                                            datatable.procedures_in_process.object.ajax.reload();
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: "La solicitud ha sido eliminada con éxito",
                                                confirmButtonText: "Excelente"
                                            });
                                        })
                                        .fail(function (e) {
                                            swal({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                                confirmButtonText: "Entendido",
                                                text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                            });
                                        });
                                });
                            }
                        });
                    })
                },
                init: function () {
                    this.onDelete();
                }
            },
            reload: function () {
                datatable.procedures_in_process.object.ajax.reload();
            },
            init: function () {
                datatable.procedures_in_process.object = $("#table_procedures_in_process").DataTable(datatable.procedures_in_process.options);
                datatable.procedures_in_process.events.init();
            }
        },
        completed_procedures: {
            object: null,
            options: {
                ajax: {
                    url: `/tramites/get-tramites-finalizados/datatable`,
                },
                columns: [
                    {
                        data: "createdAt",
                        title: "Fec. Solicitud",
                        orderable: false,
                    },
                    {
                        data: "procedure",
                        title: "Trámites",
                        orderable: false,
                    },
                    {
                        data: "total",
                        title: "Costo Total",
                        orderable: false,
                    },
                    {
                        data: "status",
                        title: "Estado",
                        orderable: false,
                    },
                    {
                        data: null,
                        orderable: false,
                        render: function (row) {
                            var tpm = ``;

                            tpm += `<a href="/tramites/detalles-solicitud/${row.id}" class="btn btn-primary m-btn btn-sm m-btn m-btn--icon"><span><i class="la la-eye"></i><span>Detalle</span></span></a> `;

                            if (row.recordHistoryId != "" && row.recordHistoryId != null) {

                                if (row.recordHistoryFileUrl != null && row.recordHistoryFileUrl != "") {
                                    tpm += `<a target='_blank' href="/documentos/${row.recordHistoryFileUrl}" class="btn btn-primary btn-sm m-btn  m-btn m-btn--icon"><span><i class="la la-download"></i><span>Descargar</span></span></a>`;
                                } else {
                                    tpm += `<a target='_blank' href="/registrosacademicos/solicitudes/imprimir/${row.recordHistoryId}?userProcedureId=${row.id}" class="btn btn-primary btn-sm m-btn  m-btn m-btn--icon"><span><i class="la la-download"></i><span>Descargar</span></span></a>`;
                                }
                            }

                            return tpm;
                        }
                    }
                ]
            },
            reload: function () {
                datatable.completed_procedures.object.ajax.reload();
            },
            init: function () {
                datatable.completed_procedures.object = $("#table_completed_procedures").DataTable(datatable.completed_procedures.options);
            }
        },
        init: function () {
            datatable.available_procedures.init();
            datatable.procedures_in_process.init();
            datatable.completed_procedures.init();
        }
    }

    var events = {
        onSearch: function () {
            $("#search_available_procedures").doneTyping(function () {
                datatable.available_procedures.reload();
            })
        },
        init: function () {
            events.onSearch();
        }
    }

    return {
        init: function () {
            datatable.init();
            events.init();
        }
    }
}();

$(() => {
    index.init();
});