var SimulationSubjectTable = function () {
    var id = "#psychologytestquestion-datatable";
    var datatable;
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ("/welfare/campania/lista").proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "topic",
                title: "Tema",
                width: 200
            },
            {
                field: "place",
                title: "Lugar",
                width: 200
            },
            {
                field: "currentDate",
                title: "Fecha",
                width: 200
            },
            {
                field: "",
                title: "Horario",
                sortable: false,
                filterable: false,
                template: function(row) {
                    return `${row.dateInitTime} - ${row.dateFinishTime}`;
                },
                width: 200
            },
            {
                field: "id",
                title: "Opciones",
                sortable: false,
                filterable: false,
                template: function (row) {
                    var template = "";
                    template +=
                        "<button class='btn btn-primary m-btn btn-sm m-btn--icon btn-edit' data-id='" + row.id + "'><i class='la la-edit'></i> Editar</button>";
                    template +=
                        " <button class='btn btn-danger m-btn btn-sm m-btn--icon btn-delete' data-id='" + row.id + "'><i class='la la-trash'></i> Eliminar</button>";

                    return template;
                }
            }
        ]
    };
    var events = {
        init: function () {
            datatable.on("click",
                ".btn-delete",
                function () {
                    var dataId = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "La campaña será eliminada",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, eliminarla",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        allowOutsideClick: () => !swal.isLoading(),
                        preConfirm: () => {
                            return new Promise(() => {
                                $.ajax({
                                    url: (`/welfare/campania/eliminar/${dataId}`).proto().parseURL(),
                                    type: "GET",
                                    success: function () {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "La pregunta ha sido eliminada con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                    },
                                    error: function () {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Al parecer la pregunta tiene información relacionada"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });
        }

    };
  
    return {
        init: function () {
            datatable = $(id).mDatatable(options);
            events.init();
        },
        reload: function () {
            datatable.reload();

        }
    }
}();

$(function () {
    SimulationSubjectTable.init();
});