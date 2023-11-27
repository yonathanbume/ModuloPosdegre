var detail = function () {

    var Id = $("#Id").val();

    var datatable = {
        students: {
            object: null,
            options: {
                ajax: {
                    url: `/profesor/examenes-aplazados/get-alumnos-asignados`,
                    type: "GET",
                    data: function (data) {
                        data.id = Id;
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        data: "username",
                        title: "Usuario"
                    },
                    {
                        data: "fullName",
                        title: "Nombre Completo"
                    },
                    {
                        data: "grade",
                        title: "Nota Obtenida",
                        orderable: false
                    },
                    {
                        data: null,
                        title: "Opciones",
                        orderable: false,
                        render: function (row) {
                            var tpm = "-";
                            if (row.termStatus == 1 && row.status == 1) {
                                tpm = `<button data-id="${row.id}" data-student="${row.fullName}" title="Calificar" class="btn-save-grade btn btn-primary btn-sm m-btn  m-btn m-btn--icon"><span><i class="la la-edit"></i><span>Calificar</span></span></button>`;
                            }
                            return tpm;
                        }
                    }
                ]
            },
            events: {
                onSaveGrade: function () {
                    $("#ajax_data").on("click", ".btn-save-grade", function () {
                        var id = $(this).data("id");
                        var student = $(this).data("student");

                        modal.saveGrade.events.show(id, student);
                    })
                },
                init: function () {
                    this.onSaveGrade();
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
        }
    };

    var modal = {
        saveGrade: {
            object: $("#modal_save_grade"),
            form: {
                object: $("#form_save_grade").validate({
                    submitHandler: function (form, e) {
                        e.preventDefault();
                        swal({
                            title: '¿Está seguro?',
                            text: `La nota(${$(form).find("[name='Grade']").val()}) será asignada.`,
                            type: 'info',
                            showCancelButton: true,
                            confirmButtonText: 'Sí, asignar',
                            confirmButtonClass: 'btn btn-danger m-btn m-btn--custom',
                            cancelButtonText: 'Cancelar',
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $.ajax({
                                        url: `/profesor/examenes-aplazados/calificar`,
                                        data: $(form).serialize(),
                                        type: "POST",
                                    })
                                        .done(function (e) {
                                            datatable.students.reload();
                                            modal.saveGrade.object.modal("hide");
                                            swal({
                                                type: 'success',
                                                title: 'Completado',
                                                text: 'La nota ha sido asignada',
                                                confirmButtonText: 'Excelente'
                                            });
                                        })
                                        .fail(function (e) {
                                            swal({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                                confirmButtonText: "Entendido",
                                                text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                            });
                                        })
                                })
                            }
                        });
                    }
                })
            },
            events: {
                show: function (id, studentName) {
                    modal.saveGrade.object.find("[name='Id']").val(id);
                    $("#StudentName").text(studentName);
                    modal.saveGrade.object.modal("show");
                },
                onHidden: function () {
                    modal.saveGrade.object.on('hidden.bs.modal', function (e) {
                        modal.saveGrade.object.resetForm();
                    })
                },
                init: function () {
                    this.onHidden();
                }
            },
            init: function () {
                modal.saveGrade.events.init();
            }
        },
        init: function () {
            modal.saveGrade.init();
        }
    }

    var events = {
        onSearch: function () {
            $("#search").doneTyping(function () {
                datatable.students.reload();
            })
        },
        init: function () {
            this.onSearch();
        }
    }


    return {
        init: function () {
            events.init();
            modal.init();
            datatable.init();
        }
    }
}();

$(() => {
    detail.init();
});