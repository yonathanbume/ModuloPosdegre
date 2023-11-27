var staff = function () {
    var datatable = {
        object: null,
        options: getSimpleDataTableConfiguration({
            url: "/registrosacademicos/personal/get".proto().parseURL(),
            data: function (data) {
                data.search = $("#search").val();
            },
            pageLength: 5,
            orderable: [],
            columns: [
                {
                    title: "Dni",
                    data : "dni"
                },
                {
                    title: "Nombre",
                    data: "fullName"
                },
                {
                    title: "Usuario",
                    data: "userName",
                    orderable: false
                },
                {
                    title: "Email",
                    data: "email",
                    orderable: false
                },
                {
                    title: "Celular",
                    data: "phoneNumber",
                    orderable: false
                },
                {
                    title: "Opciones",
                    data: null,
                    render: function (row) {
                        var tmp = "";
                        tmp += "<button data-id='" + row.id + "' class='ml-1 btn btn-secondary btn-sm m-btn--icon btn-edit'><i class='la la-edit'></i></button>";
                        tmp += '<button class="btn btn-primary btn-sm m-btn m-btn--icon btn-assign-career" data-id="' + row.id + '"><span><i class="la la-edit"></i><span>Asignar</span></span></a>'
                        return tmp;
                    }
                }
            ]
        }),
        reload: function () {
            datatable.object.ajax.reload();
        },
        events: {
            onAssign: function () {
                $("#users_datatable").on("click", ".btn-assign-career", function () {
                    var userId = $(this).data("id");
                    modal.assigned.events.show(userId);
                });
            },
            onEdit: function () {
                $("#users_datatable").on("click", '.btn-edit', function () {
                    var id = $(this).data("id");
                    window.location.href = `/registrosacademicos/personal/editar/${id}`.proto().parseURL();
                });
            },
            onSearch: function () {
                $("#search").doneTyping(function () {
                    datatable.reload();
                });
            },
            init: function () {
                this.onAssign();
                this.onEdit();
                this.onSearch();
            }
        },
        init: function () {
            datatable.options.initComplete = function () {
                select2.academicDepartment.init();
            };

            datatable.object = $("#users_datatable").DataTable(datatable.options);
            datatable.events.init();
        }
    };

    var modal = {
        assigned: {
            object: $("#assigned_deparments"),
            events: {
                show: function (userId) {
                    modal.assigned.form.object.find("[name='UserId']").val(userId);
                    modal.assigned.object.modal("show");
                    modal.assigned.events.getCareers(userId);
                    modal.assigned.form.object.find("button[type='submit']").attr("disabled", true);
                },
                hide: function () {
                    modal.assigned.object.modal("hide");
                },
                onHidden: function () {
                    modal.assigned.object.on('hidden.bs.modal', function (e) {
                        $("#Career").val([]).trigger("change");
                        modal.assigned.form.object.find("[name='UserId']").val("");
                        $("#Career").attr("disabled", true);
                    });
                },
                getCareers: function (userId) {
                    $.ajax({
                        url: `/registrosacademicos/personal/${userId}/get-departamentos-asignados`,
                        type: "GET"
                    })
                        .done(function (e) {
                            $("#AcademicDepartments").val(e).trigger("change");
                        })
                        .always(function () {
                            $("#AcademicDepartments").attr("disabled", false);
                            modal.assigned.form.object.find("button[type='submit']").attr("disabled", false);
                        });
                },
                init: function () {
                    this.onHidden();
                }
            },
            form: {
                object: $("#assigned_deparments_form"),
                events: {
                    validate: function () {
                        modal.assigned.form.object.validate({
                            submitHandler: function (formElement, e) {
                                e.preventDefault();
                                modal.assigned.form.events.submit(formElement);
                            }
                        });
                    },
                    submit: function (formElement) {
                        modal.assigned.form.object.find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                        var formData = new FormData(formElement);
                        $.ajax({
                            url: "/registrosacademicos/personal/asignar-departamentos",
                            data: formData,
                            type: "POST",
                            contentType: false,
                            processData: false
                        })
                            .done(function () {
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "Departamentos asignados correctamente.",
                                    confirmButtonText: "Aceptar"
                                });
                                modal.assigned.events.hide();
                            })
                            .fail(function (e) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Aceptar",
                                    text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                });
                            })
                            .always(function () {
                                modal.assigned.form.object.find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                            });
                    },
                    init: function () {
                        modal.assigned.form.events.validate();
                    }
                },
                init: function () {
                    modal.assigned.form.events.init();
                }
            },
            init: function () {
                modal.assigned.events.init();
                modal.assigned.form.init();
            }
        },
        init: function () {
            modal.assigned.init();
        }
    };

    var select2 = {
        academicDepartment: {
            init: function () {
                $.ajax({
                    url: "/departamentos-academicos/get",
                    type: "GET"
                }).done(function (e) {
                    $("#AcademicDepartments").select2({
                        placeholder: "Seleccionar departamento académico...",
                        data: e
                    });
                });
            }
        },
    };

    return {
        init: function () {
            datatable.init();
            modal.init();
        }
    };
}();

$(function () {
    staff.init();
});