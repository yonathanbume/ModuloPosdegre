var report = function () {    
    var datatable = {
        object: null,
        options: {
            serverSide: true,
            filter: false,
            lengthChange: false,
            ajax: {
                url: "/admin/reporte_asistencia/get".proto().parseURL(),
                type: "GET",
                dataType: "JSON",
                data: function (data) {
                    data.cid = $("#select_career").val();
                    data.fid = $("#select_faculties").val();
                    data.search = $("#search").val();
                }
            },
            pageLength: 50,
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
        },
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
                    location.href = `/admin/reporte_asistencia/${sid}`.proto().parseURL();
                });
            },
            search: function () {
                var timing = 0;
                $("#search").keyup(function () {
                    clearTimeout(timing);
                    timing = setTimeout(function () {
                        datatable.reloadTable();
                    }, 500);
                });
            },
            init: function () {
                this.onDetails();
                this.search();
            }
        },
        init: function () {
            datatable.events.init();
            datatable.object = $("#datatable_report").DataTable(datatable.options);
        }
    };
    
    var loadFacultiesSelect = function () {
        $.ajax({
            url: `/admin/reporte_asistencia/getfaculties`.proto().parseURL()
        }).done(function (data) {
            if (data.items.length > 0) {
                $("#select_faculties").select2({
                    data: data.items
                }).trigger("change");
            }
            else {
                $("#select_faculties").select2();
                $("#select_career").select2();
                showToastrFail("Debe tener al menos una facultad");
            }
          
        });
    }

    var loadCareerSelect = function () {
        $("#select_faculties").on('change', function () {
            var fid = $("#select_faculties").val();
            $.ajax({
                url: `/admin/reporte_asistencia/getcareers/${fid}`.proto().parseURL()
            }).done(function (data) {
                $("#select_career").empty();
                data.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                $("#select_career").select2({
                    data: data.items,                    
                    placeholder: "Carrera"
                }).trigger("change");
            });
        });
        $("#select_career").on('change', function () {
            datatable.reloadTable();
        });
    }

    var getReportDetail = function () {
        $(".m-datatable").on('click', '.btn-detail', function () {
            var sid = $(this).data("id");
            location.href = `/admin/reporte_asistencia/${sid}`.proto().parseURL();
        });
    };

    var events = {
        onChangeCareer: function () {
            $("#btn_export").on("click", function () {
                var facultyId = $("#select_faculties").val();
                var careerId = $("#select_career").val();
                var url = `/admin/reporte_asistencia/reporte-asistencia-consolidado?facultyId=${facultyId}&careerId=${careerId}`;
                window.open(url, "_blank");
            });
        },
        init: function () {
            this.onChangeCareer();
        }
    };

    return {
        load: function () {
            loadFacultiesSelect();
            loadCareerSelect();
            events.init();
        }
    }
}();

$(function () {
    report.load();    
})