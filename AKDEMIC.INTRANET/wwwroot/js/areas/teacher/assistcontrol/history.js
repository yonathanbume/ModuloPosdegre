
var InitApp = function () {
    var datatable = {
        classes: {
            object: null,
            options: {
                ajax: {
                    url: `/profesor/reporte_asistencia/historial/${$("#SectionId").val()}/get`,
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
                            console.log(data);
                            //var url = `/profesor/asistencia/${data.id}`.proto().parseURL();
                            var url = `/profesor/reporte_asistencia/historial/detalle/${data.id}`.proto().parseURL();
                            return `<a class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail" href="${url}"><i class="la la-eye"> </i> Ver detalle  </a>`;
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
        }
    };
}();

$(function () {
    //reportAssistControl.load();
    InitApp.init();
});