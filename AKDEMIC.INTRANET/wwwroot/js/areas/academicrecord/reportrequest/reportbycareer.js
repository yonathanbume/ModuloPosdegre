var report = function () {

    var datatable = {
        object: null,
        options: {
            ajax: {
                url: "/registrosacademicos/reporte-solicitudes/por-escuela-datatable",
                data: function (e) {
                    e.careerId = $("#careerId").val();
                    e.start = $("#start").val();
                    e.end = $("#end").val();
                }
            },
            columns: [
                {
                    data: "username",
                    title: "Cod. Estudiante",
                    orderable: false
                },
                {
                    data: "student",
                    title: "Nombre Completo",
                    orderable: false
                },
                {
                    data: "code",
                    title: "Código",
                    orderable :false
                },
                {
                    data: "subject",
                    title: "Tipo",
                    orderable: false
                },
                {
                    data: "status",
                    title: "Estado",
                    orderable: false
                }
            ]
        },
        reload: function () {
            datatable.object.ajax.reload();
        },
        init: function () {
            datatable.object = $("#datatable").DataTable(datatable.options);
        }
    };

    var select = {
        career: {
            load: function () {
                $.ajax({
                    url: "/carreras/v2/get"
                })
                    .done(function (e) {
                        $("#careerId").select2({
                            placeholder: "Seleccionar Carrera",
                            data: e.items,
                            allowClear: true
                        });
                    });
            },
            events: {
                onChange: function () {
                    $("#careerId").on("change", function () {
                        datatable.reload();
                    });
                },
                init: function () {
                    this.onChange();
                }
            },
            init: function () {
                this.events.init();
                this.load();
            }
        },
        init: function () {
            select.career.init();
        }
    };

    var datepicker = {
        start: {
            init: function () {
                $("#start").datepicker({
                    endDate: '+0d',
                    clearBtn: false,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                })
                    .on('changeDate', function (selected) {
                        $("#end").datepicker('setStartDate', $(this).val());
                        datatable.reload();
                    });
            }
        },
        end: {
            init: function () {
                $("#end").datepicker({
                    clearBtn: false,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker,
                    startDate: '+0d'
                })
                    .on('changeDate', function (selected) {
                        $("#start").datepicker('setEndDate', $(this).val());
                        datatable.reload();
                    });
            }
        },
        init: function () {
            datepicker.start.init();
            datepicker.end.init();
        }
    };

    return {
        init: function () {
            datepicker.init();
            datatable.init();
            select.init();
        }
    };
}();

$(function () {
    report.init();
});