var detail = function () {

    var CorrectionExamId = $("#Id").val();

    var datatable = {
        studentsToAssign: {
            object: null,
            options: {
                pageLength: 5,
                serverSide: false,
                ajax: {
                    url: `/admin/examen-subsanacion/get-alumnos/${CorrectionExamId}`,
                    method: 'GET',
                    dataSrc: ""
                },
                columns: [
                    {
                        data: "userName",
                        title: "Código"
                    },
                    {
                        data: "fullName",
                        title: "Estudiante"
                    },
                    {
                        data: "finalGrade",
                        title: "Nota Final"
                    },
                    {
                        data: null,
                        title: "Asignar",
                        render: function (row) {
                            var tmp = "";
                            var checked = row.isAssigned ? "checked" : "";

                            tmp += `<span class="m-switch m-switch--outline m-switch--icon m-switch--primary">
                                <label>
                                    <input type="checkbox" ${checked} class="students_check" data-id="${row.id}">
                                    <span></span>
                                </label>
                            </span>`;
                            return tmp;
                        }
                    }
                ]
            },
            events: {
                onAssignStudent: function () {
                    $("#students_table").on("click", ".students_check", function () {
                        var $enti = $(this);
                        var id = $(this).data("id");

                        mApp.block("#students_table", {
                            message: "Guardando datos..."
                        });

                        $.ajax({
                            url: `/admin/examen-subsanacion/asignar-estudiante?studentId=${id}&correctionExamId=${CorrectionExamId}`,
                            type : "POST"
                        })
                            .done(function (e) {
                                toastr.success("Registro actualizado con éxito", "Hecho!");
                            })
                            .fail(function (e) {
                                toastr.error(e.responseText, "Error!");
                                var currentValue = $enti.is(":checked");
                                $enti.prop("checked", !currentValue);
                            })
                            .always(function () {
                                datatable.students.reload();
                                mApp.unblock("#students_table");
                            })
                    })
                },
                init: function () {
                    this.onAssignStudent();
                }
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#students_table").DataTable(this.options);
                datatable.studentsToAssign.events.init();
            }
        },
        students: {
            object: null,
            options: {
                ajax: {
                    url: `/admin/examen-subsanacion/get-alumnos-asignados-datatable`,
                    data: function (data) {
                        data.correctionExamId = CorrectionExamId;
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        data: "username",
                        title : "Usuario"
                    },
                    {
                        data: "fullName",
                        title :"Nombre Completo"
                    },
                    {
                        data: "status",
                        title: "Estado",
                        render: function (row) {
                            switch (row) {
                                case 1:
                                    return `<span class="m-badge m-badge--metal m-badge--wide">Pendiente</span>`;
                                case 2:
                                    return `<span class="m-badge m-badge--primary m-badge--wide">Aprobado</span>`;
                                default:
                                    return `-`;
                            }
                        }
                    },
                    {
                        data: "grade",
                        title: "Nota Obtenida",
                        render: function (row) {
                            if (row == null) {
                                return `-`;
                            }
                            else {
                                return row;
                            }
                        }
                    },
                    {
                        data: null,
                        title: "Opciones",
                        render: function (row) {
                            var tpm = "";
                            if (row.status == 1) {
                                tpm = `<button data-id="${row.id}" class="btn btn-danger m-btn m-btn--icon btn-sm m-btn--icon-only btn-delete-assigned-student"><i class="la la-trash"></i></button>`;
                            }

                            return tpm;
                        }
                    }
                ]
            },
            events: {
                onDelete: function () {
                    $("#ajax_data").on("click", ".btn-delete-assigned-student", function () {
                        var id = $(this).data("id");

                        swal({
                            title: "Eliminar estudiante",
                            text: "¿Seguro que desea eliminar al estudiante?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Si",
                            confirmButtonClass: "btn btn-success",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: `/admin/examen-subsanacion/eliminar-estudiante-asignado?correctionExamStudentId=${id}`,
                                        type: "POST",
                                        success: function () {
                                            datatable.students.reload();
                                            datatable.studentsToAssign.reload();
                                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                        },
                                        error: function (e) {
                                            toastr.error(e.responseText, _app.constants.toastr.title.error);
                                        },
                                        complete: function () {
                                            swal.close();
                                        }
                                    });
                                });
                            }
                        });

                    })
                },
                init: function () {
                    this.onDelete();
                }
            },
            reload: function () {
                datatable.students.object.ajax.reload();
            },
            init: function () {
                datatable.students.object = $("#ajax_data").DataTable(datatable.students.options);
                datatable.students.events.init();
            }
        },
        init: function () {
            datatable.students.init();
            datatable.studentsToAssign.init();
        }
    }

    var events = {
        onSearch: function () {
            $("#search").doneTyping(function () {
                datatable.students.reload();
            })
        },
        onHidden: function () {
            $('#assigned_students_modal').on('hidden.bs.modal', function (e) {
                datatable.studentsToAssign.reload();
            })
        },
        init: function () {
            events.onSearch();
            events.onHidden();
        }
    }

    return {
        init: function () {
            datatable.init();
            events.init();
        }
    }
}();

$(() => {
    detail.init();
});
