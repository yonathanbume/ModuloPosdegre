var InitApp = function () {
    var datatable = {
        reports: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/alumno/mis-solicitudes/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    //data: function (data) {
                    //    data.searchValue = $("#search").val();
                    //}
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Usuario Encargado",
                        data: "academicRecord",
                    },
                    {
                        data: "createdAt",
                        title : "Fec. Solicitud"
                    },
                    {
                        data: "code",
                        title: "Codigo"
                    },
                    {
                        data: "subject",
                        title: "Tipo"
                    },
                    {
                        data: "status",
                        title: "Estado"
                    },
                    {
                        title: "Observaciones",
                        data: null,
                        render: function (data) {
                            return `<button data-id='${data.id}' class="btn btn-primary btn-sm m-btn m-btn--icon btn-observations" <span=""><i class="la la-eye"></i>Observaciones</button>`;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            events: {
                onObservations: function () {
                    $("#data-table").on("click", ".btn-observations", function () {
                        var id = $(this).data("id");
                        $("#recordHistoryId").val(id);
                        datatableObservation.reload();
                        $("#observation_modal").modal("show");
                    });
                },
                init: function () {
                    this.onObservations();
                }
            },
            init: function () {
                this.events.init();
                this.object = $("#data-table").DataTable(this.options);
            },

        },
        init: function () {
            this.reports.init();
        }
    };

    var datatableObservation = {
        object: null,
        options: getSimpleDataTableConfiguration({
            url: "/alumno/mis-solicitudes/get-observaciones".proto().parseURL(),
            data: function (data) {
                data.recordHistoryId = $("#recordHistoryId").val();
            },
            columns: [
                {
                    title: "Fecha",
                    data: "date"
                },
                {
                    title: "Observación",
                    data: "observation"
                },
                {
                    title: "Generado con Estado",
                    data: "status"
                }
            ]
        }),
        reload: function () {
            datatableObservation.object.ajax.reload();
        },
        init: function () {
            datatableObservation.object = $("#table_observations").DataTable(datatableObservation.options);
        }
    };
    //var search = {
    //    init: function () {
    //        $("#search").doneTyping(function () {
    //            datatable.reports.reload();
    //        });
    //    }
    //};
    return {
        init: function () {
            datatable.init();
            datatableObservation.init();
            //search.init();
        }
    }
}();

$(function () {
    InitApp.init();
})