var index = function () {

    var datatable = {
        resignation: {
            object: null,
            options: {
                ajax: {
                    url: `/reporte/estudiantes-retiro-ciclo/get`,
                    type: "GET",
                    data: function (data) {
                        data.termId = $("#term_select").val();
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        data: "userName",
                        title: "Usuario"
                    },
                    {
                        data: "fullName",
                        title: "Nombre Completo"
                    },
                    {
                        data: "career",
                        title: "Escuela profesional"
                    },
                    {
                        data: "createdAt",
                        title: "Fec. Retiro"
                    }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Excel',
                        action: function (e, dt, node, config) {
                            var url = `/reporte/estudiantes-retiro-ciclo/get-excel?termId=${$("#term_select").val()}`;
                            window.open(url, "_blank");
                        }
                    }
                ]
            },
            reload: function () {
                if (datatable.resignation.object === null) {
                    datatable.resignation.object = $("#students_table").DataTable(datatable.resignation.options);
                }
                else {
                    datatable.resignation.object.ajax.reload();
                }
            }
        },
    }

    var select = {
        term: {
            load: function () {
                $.ajax({
                    url: `/periodos/get`,
                    type: "GET"
                })
                    .done(function (e) {
                        $("#term_select").select2({
                            data: e.items,
                            placeholder: "Seleccionar periodo"
                        });

                        if (e.selected != null) {
                            $("#term_select").val(e.selected).trigger("change");
                        }
                    })
            },
            events: {
                onChange: function () {
                    $("#term_select").on("change", function () {
                        datatable.resignation.reload();
                    })
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
            select.term.init();
        }
    }

    var events = {
        search: function () {
            $("#search").doneTyping(function (e) {
                datatable.resignation.reload();
            })
        },
        init: function () {
            events.search();
        }
    }

    return {
        init: function () {
            events.init();
            select.init();
        }
    }
}();

$(() => {
    index.init();
});