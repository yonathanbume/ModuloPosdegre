var TutoringAnnouncement = function () {

    var announcementsDatatable = null;
    var createForm = null;
    var editForm = null;

    var options = {
        responsive: true,
        processing: true,
        serverSide: true,
        ajax: {
            url: "/admin/comunicados/get".proto().parseURL(),
            data: function (d) {
                if (d != "-1") {
                    d.published = $("#status-filter").val();
                    d.search = $("#search").val().toUpperCase();
                }
            }
        },
        columns: [
            { title: "Asunto", data: "title" },
            {
                title: "Roles",
                data: "tutoringAnnouncementRoles",
                render: function (data, type, row) {
                    return row.allRoles ? "Todos" : [data.slice(0, -1).join(', '), data.slice(-1)[0]].join(data.length < 2 ? '' : ' y ');;
                }
            },
            {
                title: "Escuelas",
                data: "tutoringAnnouncementCareers",
                render: function (data, type, row) {
                    return row.allCareers ? "Todas" : [data.slice(0, -1).join(', '), data.slice(-1)[0]].join(data.length < 2 ? '' : ' y ');;
                }
            },
            { title: "Fecha de Publicación", data: "displayTime" },
            { title: "Fecha fin de Publicación", data: "endTime" },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit">`;
                    tmp += `<span><i class="la la-edit"></i><span>Editar</span></span></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete">`;
                    tmp += `<span><i class="la la-trash"></i><span>Eliminar</span></span></button>`;
                    return tmp;
                }
            }
        ]
    };

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.init();
            });
        }
    };

    var datatable = {
        init: function () {
            if (announcementsDatatable === null) {
                announcementsDatatable = $(".announcements-datatable").DataTable(options);
                this.initEvents();
            }
            else {
                announcementsDatatable.ajax.reload();
            }
        },
        initEvents: function () {
            announcementsDatatable.on("click", ".btn-edit", function () {
                let id = $(this).data("id");
                form.edit.load(id);
            });

            announcementsDatatable.on("click", ".btn-delete", function () {
                let id = $(this).data("id");
                swal({
                    title: "¿Está seguro?",
                    text: "El comunicado será eliminado permanentemente",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: `/admin/comunicados/eliminar/post`.proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: id
                                },
                                success: function (result) {
                                    announcementsDatatable.ajax.reload();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El comunicado ha sido eliminado con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "Ocurrió un error al intentar eliminar el comunicado"
                                    });
                                }
                            });
                        });
                    }
                });
            });
        }
    };

    var select2 = {
        init: function () {
            this.careers.init();
            this.roles.init();
            this.status.init();
        },
        careers: {
            init: function () {
                $.ajax({
                    url: "/carreras/v3/get".proto().parseURL()
                }).done(function (result) {
                    $(".select2-careers").select2({
                        data: result.items,
                        placeholder: "Carreras",
                        allowClear: true
                    });
                });
            }
        },
        roles: {
            init: function() {
                $.ajax({
                    url: "/roles-anuncios".proto().parseURL()
                }).done(function (result) {
                    $(".select2-roles").select2({
                        data: result.items,
                        placeholder: "Roles",
                        allowClear: true
                    });
                });
            }
        },
        status: {
            init: function () {
                $(".select2-status")
                    .on("change", function () {
                        datatable.init();
                    }).select2({
                        minimumResultsForSearch: -1
                    }).trigger("change");
            }
        }
    }

    var datepicker = {
        init: function () {
            var date = new Date();
            date.setDate(date.getDate());
            $(".input-datepicker").datetimepicker({
                format: _app.constants.formats.datetimepicker,
                showMeridian: true,
                startDate: date
            });
        }
    }

    var checkbox = {
        init: function () {
            $("#Add_AllRoles").change(function () {
                if (this.checked) {
                    $("#Add_TutoringAnnouncementRoleIds").parent().find(".form-control-label").removeClass("required-form-label");
                    $("#Add_TutoringAnnouncementRoleIds").val(null).trigger("change");
                    $("#Add_TutoringAnnouncementRoleIds").prop("disabled", true);
                }
                else {
                    $("#Add_TutoringAnnouncementRoleIds").parent().find(".form-control-label").addClass("required-form-label");
                    $("#Add_TutoringAnnouncementRoleIds").prop("disabled", false);
                }
            });

            $("#Add_AllCareers").change(function () {
                if (this.checked) {
                    $("#Add_TutoringAnnouncementCareerIds").parent().find(".form-control-label").removeClass("required-form-label");
                    $("#Add_TutoringAnnouncementCareerIds").val(null).trigger("change");
                    $("#Add_TutoringAnnouncementCareerIds").prop("disabled", true);
                }
                else {
                    $("#Add_TutoringAnnouncementCareerIds").parent().find(".form-control-label").addClass("required-form-label");
                    $("#Add_TutoringAnnouncementCareerIds").prop("disabled", false);
                }
            });

            $("#Edit_AllRoles").change(function () {
                if (this.checked) {
                    $("#Edit_TutoringAnnouncementRoleIds").parent().find(".form-control-label").removeClass("required-form-label");
                    $("#Edit_TutoringAnnouncementRoleIds").val(null).trigger("change");
                    $("#Edit_TutoringAnnouncementRoleIds").prop("disabled", true);
                }
                else {
                    $("#Edit_TutoringAnnouncementRoleIds").parent().find(".form-control-label").addClass("required-form-label");
                    $("#Edit_TutoringAnnouncementRoleIds").prop("disabled", false);
                }
            });

            $("#Edit_AllCareers").change(function () {
                if (this.checked) {
                    $("#Edit_TutoringAnnouncementCareerIds").parent().find(".form-control-label").removeClass("required-form-label");
                    $("#Edit_TutoringAnnouncementCareerIds").val(null).trigger("change");
                    $("#Edit_TutoringAnnouncementCareerIds").prop("disabled", true);
                }
                else {
                    $("#Edit_TutoringAnnouncementCareerIds").parent().find(".form-control-label").addClass("required-form-label");
                    $("#Edit_TutoringAnnouncementCareerIds").prop("disabled", false);
                }
            });

        }
    }

    var events = {
        init: function () {
            $("#add-modal").on("shown.bs.modal", function () {
                $("#Add_TutoringAnnouncementRoleIds").select2({
                    placeholder: "Roles"
                });
                $("#Add_TutoringAnnouncementCareerIds").select2({
                    placeholder: "Escuelas"
                });
            }).on("hidden.bs.modal", function () {
                form.add.reset();
            });
            $("#edit-modal").on("shown.bs.modal", function () {
                $("#Edit_TutoringAnnouncementRoleIds").select2({
                    placeholder: "Roles"
                });
                $("#Edit_TutoringAnnouncementCareerIds").select2({
                    placeholder: "Escuelas"
                });
            }).on("hidden.bs.modal", function () {
                form.edit.reset();
            });
            $('[data-toggle="m-tooltip"]').tooltip()
        }
    };

    var form = {
        add: {
            submit: function(formElement) {
                //var data = $(formElement).serialize();
                var formData = new FormData($(formElement).get(0));
                $(formElement).find(".btn-submit").addLoader();
                $(formElement).find("input, select, textarea").prop("disabled", true);

        

                $.ajax({
                    url: $(formElement).attr("action"),
                    type: "POST",
                    data: formData,              
                    contentType: false,
                    processData: false                    
                })
                    .always(function () {
                        $(formElement).find(".btn-submit").removeLoader();
                        $(formElement).find("input, select, textarea").prop("disabled", false);
                    })
                    .done(function () {
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        announcementsDatatable.ajax.reload();
                        $("#add-modal").modal("hide");
                    })
                    .fail(function (e) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (e.responseText != null) $("#createAlertText").html(e.responseText);
                        else $("#createAlertText").html(_app.constants.ajax.message.error);
                        $("#createAlert").removeClass("m--hide").show();
                    });
            },
            reset: function () {
                createForm.resetForm();
                $("#createAlert").addClass("m--hide").hide();
                $("#Add_TutoringAnnouncementRoleIds").prop("disabled", false);
                $("#Add_TutoringAnnouncementCareerIds").prop("disabled", false);
            }
        },
        edit: {
            load: function (id) {
                mApp.blockPage();
                $.ajax({
                    url: `/admin/comunicados/${id}/get`.proto().parseURL()
                })
                .always(function () {
                    mApp.unblockPage();
                })
                .done(function (result) {
                    var formElements = $("#edit-form").get(0).elements;
                    formElements["Edit_Id"].value = id;
                    formElements["Edit_Title"].value = result.title;
                    formElements["Edit_Message"].value = result.message;
                    $("#Edit_TutoringAnnouncementRoleIds").val(result.roles).trigger("change");
                    $("#Edit_TutoringAnnouncementCareerIds").val(result.careers).trigger("change");
                    $("#Edit_DisplayTime").val(result.displayTime).trigger("change");
                    $("#Edit_EndTime").val(result.endTime).trigger("change");
                    $("#Edit_AllRoles").prop("checked", result.allRoles).trigger("change");
                    $("#Edit_AllCareers").prop("checked", result.allCareers).trigger("change");
                    $("#edit-modal").modal("show");
                })
                .fail(function () {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            },
            submit: function (formElement) {

                var formData = new FormData($(formElement).get(0));
                $(formElement).find(".btn-submit").addLoader();
                $(formElement).find("input, select, textarea").prop("disabled", true);
                $.ajax({             
                    type: "POST",
                    url: $(formElement).attr("action"),
                    data: formData,
                    contentType: false,
                    processData: false   
                })
                    .always(function () {
                        $(formElement).find(".btn-submit").removeLoader();
                        $(formElement).find("input, select, textarea").prop("disabled", false);
                    })
                    .done(function () {
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        announcementsDatatable.ajax.reload();
                        $("#edit-modal").modal("hide");
                    })
                    .fail(function (e) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (e.responseText != null) $("#editAlertText").html(e.responseText);
                        else $("#editAlertText").html(_app.constants.ajax.message.error);
                        $("#editAlert").removeClass("m--hide").show();
                    });
            },
            reset: function () {
                editForm.resetForm();
                $("#editAlert").addClass("m--hide").hide();
                $("#Edit_TutoringAnnouncementRoleIds").prop("disabled", false);
                $("#Edit_TutoringAnnouncementCareerIds").prop("disabled", false);
            }
        }
    };

    var validate = {
        init: function () {
            createForm = $("#add-form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.add.submit(formElement);
                }
            });

            editForm = $("#edit-form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.edit.submit(formElement);
                }
            });
        }
    };

    return {
        init: function () {
            search.init();
            select2.init();
            datepicker.init();
            checkbox.init();
            events.init();
            validate.init();
        }
    }
}();

$(function () {
    TutoringAnnouncement.init();
});