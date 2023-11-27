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
                title: "Estudiantes"
            },
            {
                field: "score",
                title: "Nota",
                width: 100,
                textAlign: "center",
                sortable: false,
                filterable: false,
                template: function (row) {
                    return "<input data-studentid='" + row.studentId + "' class='form-control integer score' value='" + row.score + "' min='0' max='20' type='number'>";
                }
            }
        ]
    };

    var events = {
        init: function () {
            $(".score").on("change", function () {
                var value = parseFloat(this.value);
                if (isNaN(value))
                    this.value = "0.00";
                else if (value > 20)
                    this.value = "20.00";
                else if (value < 0)
                    this.value = "0.00";
                else
                    this.value = value.toFixed(2);
            });
            $(".score").on("focus", function () {
                $(this).select();
            });
            $(".btn-save").on("click", function () {
                var scoreStudents = [];
                $(".score").each(function () {
                    var StudentId = $(this).data("studentid");
                    var Score = this.value;
                    scoreStudents.push({ StudentId, Score });
                });
                if (scoreStudents.length > 0) {
                    var list = JSON.stringify(scoreStudents);
                    swal({
                        title: "¿Está seguro?",
                        text: "Se guardarán las notas ingresadas.",
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
                                    url: ("/docente/gruposextracurriculares/score/save").proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        score: list,
                                        groupId: groupId
                                    },
                                    success: function () {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "La notas de los estudiantes han sido guardadas con éxito",
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
                                            text: "Ocurrió un error al guardar las notas de los estudiantes"
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