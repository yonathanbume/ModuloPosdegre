var reportCourseDetail = function () {
    var datatable = {
        object: null,
        options: {
            ajax: {
                url: `/admin/reporte_asistencia_curso/reporte_curso_detalles`.proto().parseURL(),
                data: function (data) {
                    data.sectionId = $("#Id").val();
                }
            },
            pageLength: 100,
            orderable: [],
            columns: [
                {
                    data: 'fullname',
                    title: 'Nombre Completo',
                },
                {
                    data: 'absences',
                    title: 'Faltas',
                },
                {
                    data: 'dictated',
                    title: 'Clases dictadas',
                },
                {
                    data: 'maxAbsences',
                    title: 'Máx.Faltas',
                },
                {
                    data: null,
                    title: "Opciones",
                    orderable: false,
                    render: function (row) {
                        if (row.disabled) {
                            return '-';
                        }
                        return '<button type="button" class="btn btn-danger m-btn btn-sm m-btn--icon btn-dpi" data-id="' + row.id + '">Deshabilitar</button>';
                    }
                }
            ],
            dom: 'Bfrtip',
            buttons: [
                {
                    text: 'Excel',
                    action: function (e, dt, node, config) {
                        window.open(`/admin/reporte_asistencia_curso/reporte-excel/${$("#Id").val()}`, "_blank");
                    }
                }
            ]
        },
        reloadTable: function () {
            if (datatable.object === null) {
                datatable.init();
            } else {
                datatable.object.ajax.reload();
            }
        },
        init: function () {
            datatable.object = $("#report-datatable").DataTable(datatable.options);

            $("#report-datatable").on("click", ".btn-dpi", function () {
                var id = $(this).data("id");
                swal({
                    title: "¿Está seguro?",
                    text: "El estudiante será deshabilitado del curso.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Si, confirmar",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar"
                }).then(function (result) {
                    if (result.value) {
                        $.ajax({
                            url: `/admin/reporte_asistencia_curso/reporte_curso_vista/deshabilita-estudiante/${id}`,
                            type: "POST",
                            data: {
                                directedcourseid: id
                            },
                            success: function () {
                                swal({
                                    title: _app.constants.toastr.title.success,
                                    text: "El estudiante fue deshabilitado",
                                    type: "success",
                                    confirmButtonText: "Entendido"
                                });

                                datatable.reloadTable();
                            },
                            error: function () {
                                swal({
                                    title: _app.constants.toastr.title.error,
                                    text: _app.constants.toastr.message.error.task,
                                    type: "error",
                                    confirmButtonText: "Entendido"
                                });
                            }
                        });
                    }
                });
            });
        }
    };

    return {
        load: function () {
            datatable.init();
        }
    }
}();

$(function () {
    reportCourseDetail.load();
})