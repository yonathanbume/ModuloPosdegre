var studentsTable = function () {
    var id = "#students-datatable";
    var datatablestudents;
    var options = {
        search: {
            input: $("#search-students")
        },
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ("/admin/gruposextracurriculares/alumnos/get").proto().parseURL()
                }
            }
        },
        columns: [
           {
                field: "name",
                title: "Nombre"
            }
        ]
    }
    var events = {
        init: function () {
            $(".btn-assignsection").on("click",
            function () {
                var dataId = $(this).data("id");
                var userprocedureId = $("#UserProcedureId").val();
                swal({
                    title: "¿Está seguro?",
                    text: "Se consumirá el uso de esta solicitud.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarla",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise(() => {
                            $.ajax({
                                url: ("/admin/solicitudesmatricula/matricular/post").proto().parseURL(),
                                type: "POST",
                                data: {
                                    sid: dataId,
                                    pid: userprocedureId
                                },
                                success: function (e) {
                                    datatablestudents.reload();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El estudiante ha sido matriculado con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                    $("#section_modal").modal("toggle");
                                },
                                error: function (e) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: e.responseText
                                    });
                                }
                            });
                        });
                    }
                });
            });

        }
    }
    var loadDatatable = function (groupid) {
       // $("#GroupId").val(groupid);
        $("#students_modal").modal('show');
        if (datatablestudents !== undefined)
            datatablestudents.destroy();
        options.data.source.read.url = ("/admin/gruposextracurriculares/alumnos/get?gid=" + groupid).proto().parseURL();
        datatablestudents = $(id).mDatatable(options);
        $(datatablestudents).on("m-datatable--on-layout-updated", function () {
            events.init();
        });
    };
    return {
        init: function (groupid) {
            loadDatatable(groupid);
        },
        reload: function () {
            datatablestudents.reload();
        }
    }
}();