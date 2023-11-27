var report = function () {
    var datatable = {
        object: null,
        options: getSimpleDataTableConfiguration({
            url: `/director-carrera/reporte_asistencia/carrera/facultad`,
            data: function (data) {
                data.cid = $("#select_career").val();
                data.fid = $("#select_faculties").val();
                data.pid = $("#select_program").val();
                data.search = $("#search").val();
            },
            pageLength: 10,
            orderable: [],
            columns: [
                {
                    data: 'paternalSurname',
                    title: 'Apellido Paterno',
                },
                {
                    data: 'maternalSurname',
                    title: 'Apellido Materno',
                },
                {
                    data: 'names',
                    title: 'Nombres',
                },
                {
                    data: 'userName',
                    title: 'Usuario',
                },
                {
                    data: 'email',
                    title: 'Correo electrónico',
                },
                {
                    data: 'phoneNumber',
                    title: 'Teléfono',
                },
                {
                    data: null,
                    title: "Opciones",
                    render: function (row) {
                        return `<button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail" <span><i class="la la-eye"> </i> </span>Ver Reporte </span></span></button>`;
                    }
                }]
        }),
        reloadTable: function () {
            if (datatable.object === null) {
                datatable.init();
            } else {
                datatable.object.ajax.reload();
            }
        },
        events: {
            onDetails: function () {
                $("#datatable_report").on('click', '.btn-detail', function () {
                    var sid = $(this).data("id");
                    location.href = `/director-carrera/reporte_asistencia/${sid}`.proto().parseURL();
                });
            },
            init: function () {
                this.onDetails();
            }
        },
        init: function () {
            datatable.events.init();
            datatable.object = $("#datatable_report").DataTable(datatable.options);
        }
    };

    var loadFacultiesSelect = function () {
        $("#select_faculties").select2({
            ajax: {
                url: `/director-carrera/reporte_asistencia/getfaculties`.proto().parseURL(),
                delay: 300,
                processResults: function (data) {
                    return {
                        results: data.results
                    };
                }
            },
            minimumInputLength: 0,
            placeholder: 'Seleccione facultad',
            allowClear: true
        }).on('change', function () {
            $("#select_career").val('').trigger("change");
            var fid = $("#select_faculties").val();
            $("#select_career").select2({
                ajax: {
                    url: `/director-carrera/reporte_asistencia/getcareers/${fid}`.proto().parseURL(),
                    delay: 300,
                    processResults: function (data) {
                        return {
                            results: data.results
                        };
                    }
                },
                minimumInputLength: 0,
                placeholder: 'Seleccione carrera',
                allowClear: true
            }).on('change', function () {
                $("#select_program").val('').trigger("change");
                var cid = $("#select_career").val();
                $("#select_program").select2({
                    ajax: {
                        url: `/director-carrera/reporte_asistencia/getprograms/${cid}`,
                        delay: 300,
                        processResults: function (data) {
                            if (data.results.length > 1) {
                                $("#div_select_program").removeClass("d-none");
                            } else {
                                $("#div_select_program").addClass("d-none");
                            }
                            return {
                                results: data.results
                            };
                        }
                    },
                    minimumInputLength: 0,
                    placeholder: 'Seleccione carrera',
                    allowClear: true
                }).on('change', function () {
                    datatable.reloadTable();
                });
                datatable.reloadTable();
            });
            datatable.reloadTable();
        });
        $("#search").doneTyping(function () {
            datatable.reloadTable();
        });
    };

    return {
        load: function () {
            loadFacultiesSelect();
        }
    }
}();

$(function () {
    report.load();
})