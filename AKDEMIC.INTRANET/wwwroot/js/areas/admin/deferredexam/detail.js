var detail = function () {

    var DeferredExamId = $("#Id").val();


    var datatable = {
        studentsToAssign: {
            object: null,
            options: {
                pageLength: 5,
                serverSide: false,
                ajax: {
                    url: `/admin/examen-aplazado/get-alumnos/${DeferredExamId}`,
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
                            url: `/admin/examen-aplazado/asignar-estudiante?studentId=${id}&deferredExamId=${DeferredExamId}`,
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
                    url: `/admin/examen-aplazado/get-alumnos-asignados-datatable`,
                    data: function (data) {
                        data.deferredExamId = DeferredExamId;
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
                                    return `<span class="m-badge m-badge--primary m-badge--wide">Calificado</span>`;
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
                    //{
                    //    data: null,
                    //    title: "Opciones",
                    //    render: function (row) {
                    //        var tpm = "";
                    //        if (row.status == 1) {
                    //            tpm = `<button data-id="${row.id}" class="btn btn-danger m-btn m-btn--icon btn-sm m-btn--icon-only btn-delete-assigned-student"><i class="la la-trash"></i></button>`;
                    //        }

                    //        return tpm;
                    //    }
                    //}
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
                                        url: `/admin/examen-aplazado/{0}/eliminar-estudiante-asignado?deferredExamStudentId=${id}`,
                                        type: "POST",
                                        success: function () {
                                            datatable.students.reload();
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
        onUpdateStudents: function() {
            $("#btn-update-students").on("click", function () {
                var $btn = $(this);
                $btn.addLoader();

                $.ajax({
                    url: `/admin/examen-aplazado/${DeferredExamId}/actualizar-estudiantes-asignados`,
                    type : "POST"
                })
                    .done(function () {
                        datatable.students.reload();
                        swal({
                            type: "success",
                            title: "Completado",
                            text: "Alumnos actualizados con éxito",
                            confirmButtonText: "Excelente"
                        });
                    })
                    .fail(function (e) {
                        swal({
                            type: "error",
                            title: "Error",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            confirmButtonText: "Entendido",
                            text: e.responseText
                        });
                    })
                    .always(function () {
                        $btn.removeLoader();
                    })


            })
        },
        onHidden: function () {
            $('#assigned_students_modal').on('hidden.bs.modal', function (e) {
                datatable.studentsToAssign.reload();
            })
        },
        init: function () {
            events.onSearch();
            events.onUpdateStudents();
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
