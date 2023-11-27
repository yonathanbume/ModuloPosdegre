var InitApp = function () {
    var datatable = {
        reports: {
            object: null,
            options: {
                ajax: {
                    url: "/alumno/mis-solicitudes/get-records".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                },
                pageLength: 10,
                columns: [
                    {
                        title: "Fec. Solicitud",
                        data: "createdAt",
                        orderable : false
                    },
                    {
                        data: "type",
                        title: "Tipo Solicitud",
                        orderable: false
                    },
                    {
                        data: "code",
                        title: "Código",
                        orderable: false
                    },
                    {
                        data: "status",
                        title: "Estado",
                        orderable: false
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            return `<button data-id='${data.id}' class="btn btn-primary btn-sm m-btn m-btn--icon btn-observations" <span=""><i class="la la-eye"></i>Observaciones</button>`;
                        }
                    }
                ]
            },
            reload: function () {
                this.object.ajax.reload();
            },
            events: {
                onObservations: function () {
                    $("#data-table").on("click", ".btn-observations", function () {
                        var id = $(this).data("id");
                        $("#recordHistoryId").val(id);
                        $("#observation_modal").modal("show");
                        datatableObservation.reload();
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
                    data: "date",
                    orderable: false
                },
                {
                    title: "Observación",
                    data: "observation",
                    orderable: false
                },
                {
                    title: "Generado con Estado",
                    data: "status",
                    orderable: false
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


    return {
        init: function () {
            datatable.init();
            datatableObservation.init();
        }
    }
}();

$(function () {
    InitApp.init();
})