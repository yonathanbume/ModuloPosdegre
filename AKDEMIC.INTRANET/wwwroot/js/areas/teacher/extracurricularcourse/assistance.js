var ActivitiesTable = function () {
    var id = "#datatable";
    var groupId = $("#GroupId").val();
    var datatable;
    var datatableDetailEvaluation;
    var datatableDetailSection;

    var options = {
        pagination: false,
        search: {
            input: $("#search")
        },
        data: {
            source: {
                read: {
                    method: "GET",
                    url: (`/docente/gruposextracurriculares/section/${groupId}/students`).proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "student",
                title: "Estudiane"
            },
            {
                field: "assistance",
                title: "Asistencia",
                textAlign: "center",
                sortable: false,
                filterable: false,
                template: function (row) {
                    return "<span data-id='" + row.studentId + "' class=\"m-switch m-switch--outline m-switch--icon m-switch--success\"><label><input type=\"checkbox\" name=\"\"><span></span></label></span>";
                }
            }
        ]
    };

    var events = {
        init: function () {
            $(".btn-save").on("click", function () {
                var assistanceStudents = [];
                $(".m-switch").each(function () {
                    var StudentId = $(this).data("id");
                    var Assistance = this.children[0].children[0].checked;
                    assistanceStudents.push({ StudentId, Assistance });
                });
                if (assistanceStudents.length > 0) {
                    var list = JSON.stringify(assistanceStudents);
                    swal({
                        title: "¿Está seguro?",
                        text: "La asistencia de los estudiantes se guardará y no podrá ser modificada.",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, guardar.",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        allowOutsideClick: () => !swal.isLoading(),
                        preConfirm: () => {
                            return new Promise(() => {
                                $.ajax({
                                    url: ("/docente/gruposextracurriculares/assistance/save").proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        assistance: list,
                                        groupId: groupId
                                    },
                                    success: function () {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "La asistencia de los estudiantes han sido guardadas con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                        $(location).attr("href", `/docente/gruposextracurriculares`.proto().parseURL());
                                    },
                                    error: function () {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al guardar las asistencias de los estudiantes"
                                        });
                                    }
                                });
                            });
                        }
                    });
                }
            });
        }
    };

    var loadDatatable = function () {
        if (datatable !== undefined)
            datatable.destroy();
        datatable = $(id).mDatatable(options);
        $(datatable).on("m-datatable--on-layout-updated", function () {
            events.init();
        });
    };
    return {
        init: function () {
            loadDatatable();
        },
        reload: function () {
            datatable.reload();
        }
    }
}();

$(function () {
    ActivitiesTable.init();
});