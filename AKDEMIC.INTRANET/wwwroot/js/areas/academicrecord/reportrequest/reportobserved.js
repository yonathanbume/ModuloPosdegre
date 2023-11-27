var report = function () {

    var datatable = {
        object: null,
        options: {
            ajax: {
                url: "/registrosacademicos/reporte-solicitudes/tramites-observados-datatable",
                data: function (e) {
                    e.careerId = $("#careerId").val();
                    e.start = $("#start").val();
                    e.end = $("#end").val();
                    e.type = $("#type").val();
                }
            },
            columns: [
                {
                    data: "username",
                    title: "Cod. Estudiante",
                },
                {
                    data: "student",
                    title: "Nombre Completo"
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
        type: {
            load: function () {
                $("#type").select2({
                    placeholder: "Seleccionar Tipo",
                    allowClear: true
                });
            },
            events: {
                onChange: function () {
                    $("#type").on("change", function () {
                        datatable.reload();
                    });
                },
                init: function () {
                    this.onChange();
                }
            },
            init: function () {
                this.load();
                this.events.init();
            }
        },
        init: function () {
            select.career.init();
            select.type.init();
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