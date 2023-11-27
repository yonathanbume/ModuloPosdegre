var reportAssistControl = function () {
    var datatable = null;
    var loadDatatable = function () {

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }

        datatable = $(".m-datatable-class").mDatatable(options);
        datatable.on('click', '.btn-detail', function () {
            var tid = $(this).data('id');
            location.href = `/profesor/asistencia/${tid}`.proto().parseURL();
        });
    };
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/profesor/reporte_asistencia/asistencias/${$("#SectionId").val()}/get`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'weekNumber',
                title: 'Núm. Semana',
            },
            {
                field: 'formattedStart',
                title: 'Fecha',
            },
            {
                field: "options",
                title: "Opciones",
                width: 200,
                sortable: false,
                filterable: false,
                template: function (row) {
                    var template = "";
                    template += `<button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-list"> </i> </span>Tomar asistencia</span></span></button>`;
                    return template;
                }
            }
        ]
    };

    return {
        load: function () {
            loadDatatable();
        }
    };
}();

var InitApp = function () {
    var datatable = {
        classes: {
            object: null,
            options: {
                ajax: {
                    url: `/profesor/reporte_asistencia/asistencias/${$("#SectionId").val()}/get`,
                    type: "GET",
                    data: function (data) {
                        delete data.columns;
                        data.search = $("#search").val();
                        data.date = $("#datepicker").val();
                    },
                    dataSrc: ""
                },
                serverSide: false,
                columns: [
                    {
                        data: "number",
                        title: "N°",
                        orderable: false
                    },
                    {
                        data: 'type',
                        title: 'Tipo de Sesión',
                        orderable : false
                    },
                    {
                        data: "date",
                        title: "Fecha",
                        orderable: false
                    },
                    {
                        data: "start",
                        title: "Inicio",
                        orderable: false
                    },
                    {
                        data: "end",
                        title: "Fin",
                        orderable: false
                    },
                    {
                        data: "classroom",
                        title: "Aula",
                        orderable: false
                    },
                    {
                        data: null,
                        title: "Opciones",
                        width: "170px",
                        orderable: false,
                        render: function (data) {
                            var tpm = "";
                            if (data.needReschedule) {
                                tpm += `<button disabled class="btn btn-primary btn-sm m-btn m-btn--icon"><i class="la la-list"></i>Necesita Reprogramación</button>`;
                            } else {
                                var url = `/profesor/reporte_asistencia/asistencias/tomar-asistencia/${data.id}`.proto().parseURL();
                                tpm += `<a class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail" href="${url}"><i class="la la-list"> </i> Tomar asistencia  </a>`;
                            }

                            return tpm;
                        }
                    }
                ]
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
            },
            reload: function () {
                this.object.ajax.reload();
            }
        }
    };

    var datepicker = {
        init: function () {
            $("#datepicker").datepicker({
                endDate: "0d"
            });

            $("#datepicker")
                .on("changeDate", function (e) {
                    datatable.classes.reload();
                });

            $("#clear-datepicker").click(function () {

                $("#datepicker").datepicker("setDate", null);

            });
        }
    };

    return {
        init: function () {
            datatable.classes.init();
            datepicker.init();
            $('.btn-history').on('click', function () {
                var tid = $(this).data('id');
                location.href = `/profesor/reporte_asistencia/historial/${tid}`.proto().parseURL();
            });
        }
    };
}();

$(function () {
    //reportAssistControl.load();
    InitApp.init();
});