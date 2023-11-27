var extracurricularcoursegroupTable = function () {
    var id = "#extracurricularcoursegroup-datatable";
    var datatable;
    var options = {
        search: {
            input: $("#search")
        },
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ("/admin/gruposextracurriculares/get").proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "code",
                title: "Código",
            },
            {
                field: "name",
                title: "Curso",
            }, {
                field: "teacher",
                title: "Docente",
            },
            {
                field: "options",
                title: "Opciones",
                textAlign: "center",
                sortable: false, // disable sort for this column
                filterable: false, // disable or enable filtering
                template: function (row) {
                    var template = "";
                    template += "<button data-toggle='modal' data-target='#edit_extracurricularcoursegroup_modal' data-id='" +
                        row.id +
                        "' class='btn btn-info btn-sm m-btn m-btn--icon btn-edit'><span><i class='la la-edit'></i><span>Editar</span></span></button> ";
                    template += "<button data-id='" + row.id + "' class='btn btn-danger btn-sm m-btn--icon btn-delete' title='Eliminar'><i class='la la-trash'></i></button> ";
                    template += "<button data-id='" + row.id + "' class='btn btn-primary btn-sm m-btn--icon btn-assign' title='Alumnos'><i class='la la-user'></i></button>";
                    return template;
                }
            }
        ]
    }
    var events = {
        init: function () {
            datatable.on('click', '.btn-delete', function () {
                var dataId = $(this).data("id");
                swal({
                    title: "¿Está seguro?",
                    text: "El grupo será eliminado",
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
                                url: ("/admin/gruposextracurriculares/eliminar/post").proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: dataId
                                },
                                success: function () {
                                    datatable.reload();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El grupo ha sido eliminado con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function () {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "Al parecer el grupo tiene información relacionada"
                                    });
                                }
                            });
                        });
                    }
                });
            });
            datatable.on('click', '.btn-assign', function () {
                var dataId = $(this).data("id");
                studentsTable.init(dataId);
            });
            datatable.on('click', '.btn-edit', function () {
                var dataId = $(this).data("id");
                Form.Edit.show(dataId);
            });
     
            $('.letters').keypress(function (event) {
                var inputValue = event.which;
                // allow letters and whitespaces only.
                if (!(inputValue >= 65 && inputValue <= 120) && (inputValue != 32 && inputValue != 0)) {
                    event.preventDefault();
                }
            });

        }
    }
    var loadDatatable = function () {
        //var pid = $(".select2-term").val();
        //var aid = $("#areacareer-filter").val();
        //var aydid = $("#academicyear-filter").val();
        if (datatable !== undefined)
            datatable.destroy();
        options.data.source.read.url = ("/admin/gruposextracurriculares/get").proto().parseURL();
        datatable = $(id).mDatatable(options);
        $(datatable).on("m-datatable--on-layout-updated", function () {
            events.init();
        });
    };
    return {
        init: function () {
            //   filters.init();
            loadDatatable();
            //fileInput.init();
        },
        reload: function () {
            datatable.reload();
        }
    }
}();

var Form = function () {
    var modalCreateId = "#add_extracurricularcoursegroup_modal";
    var formCreateId = "#add-extracurricularcoursegroup-form";
    var formCreateValidate;

    var modalEditId = "#edit_extracurricularcoursegroup_modal";
    var formEditId = "#edit-extracurricularcoursegroup-form";
    var formEditValidate;
    var events = {
        init: function () {
            $(".btn-add").on("click",
                function () {

                    $(modalCreateId).one("hidden.bs.modal", function () {
                        form.reset.create();
                    });
                });

        }
    }
    var form = {
        submit: {
            create: function (formElements) {
                var data = $(formElements).serialize();
                $(`${modalCreateId} input, ${modalCreateId} select`).attr("disabled", true);
                $("#btnCreate").addLoader();
                $.ajax({
                    url: $(formElements).attr("action"),
                    type: "POST",
                    data: data
                }).always(function () {
                    $(`${modalCreateId} input, ${modalCreateId} select`).attr("disabled", false);
                    $("#btnCreate").removeLoader();
                }).done(function () {
                    $(modalCreateId).modal("toggle");
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    extracurricularcoursegroupTable.reload();
                }).fail(function (e) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (e.responseText != null) $("#add_form_msg_txt").html(e.responseText);
                    else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#add_form_msg").removeClass("m--hide").show();
                });
            },
            edit: function (formElements) {
                var data = $(formElements).serialize();
                $(`${modalEditId} input, ${modalEditId} select`).attr("disabled", true);
                $("#btnEdit").addLoader();
                $.ajax({
                    url: $(formElements).attr("action"),
                    type: "POST",
                    data: data
                }).always(function () {
                    $(`${modalEditId} input, ${modalEditId} select`).attr("disabled", false);
                    $("#btnEdit").removeLoader();
                }).done(function () {
                    $(modalEditId).modal("toggle");
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    extracurricularcoursegroupTable.reload();
                }).fail(function (e) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (e.responseText != null) $("#edit_form_msg_txt").html(e.responseText);
                    else $("#edit_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#edit_form_msg").removeClass("m--hide").show();
                });
            }
        },
        reset: {
            create: function () {
                $("#add_form_msg").addClass("m--hide").hide();
                $("#Add_TeacherId").val(null).trigger("change.select2");
                $("#Add_ExtracurricularCourseId").val(null).trigger("change.select2");
                formCreateValidate.resetForm();
            },
            edit: function () {
                $("#edit_form_msg").addClass("m--hide").hide();
                $("#Edit_TeacherId").val(null).trigger("change.select2");
                $("#Edit_ExtracurricularCourseId").val(null).trigger("change.select2");
                formEditValidate.resetForm();
            }
        },
        show: {
            edit: function (id) {
                //mApp.blockPage();
                $.ajax({
                    url: `/admin/gruposextracurriculares/${id}/get`.proto().parseURL()
                }).done(function (result) {
                    var formElements = $(formEditId).get(0).elements;
                    formElements["Edit_Id"].value = result.id;
                    formElements["Edit_Code"].value = result.code;
                    //$("#Edit_TeacherId").val(result.teacherid).trigger("change.select2");
                    //$("#Edit_TeacherId").select2('data', { id: result.teacherid, text: result.teacherName });
                    $("#Edit_TeacherId").empty().append(`<option value="${result.teacherid}">${result.teacherName}</option>`).val(`${result.teacherid}`).trigger('change');
                    $("#Edit_ExtracurricularCourseId").val(result.extracurricularcourseid).trigger("change.select2");
                    //mApp.unblockPage();
                    //$(modalEditId).one("hidden.bs.modal", function () {
                    //    form.reset.edit();
                    //});
                }).fail(function (error) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            }
        }
    }
    var select = {
        modal: {
            init: function () {
                //$.when(
                //    $.ajax({
                //        url: "/cursosextracurriculares/get".proto().parseURL()
                //    }),
                //    $.ajax({
                //        url: ("/profesores/get").proto().parseURL()
                //    })
                //).then(function (data1, data2) {

                $.ajax({
                    url: "/cursosextracurriculares/get".proto().parseURL(),
                    success: function (data) {
                        $("#Add_ExtracurricularCourseId").select2({
                            placeholder: "Seleccione un curso",
                            dropdownParent: $(modalCreateId),
                            data: data.items
                        });

                        $("#Edit_ExtracurricularCourseId").select2({
                            placeholder: "Seleccione un curso",
                            dropdownParent: $(modalEditId),
                            data: data.items
                        });
                    }
                });

                //    //$("#Add_TeacherId").select2({
                //    //    placeholder: "Seleccione un docente",
                //    //    dropdownParent: $(modalCreateId),
                //    //    minimumResultsForSearch: 1,
                //    //    data: data2[0].items
                //    //});

                //    //$("#Edit_TeacherId").select2({
                //    //    placeholder: "Seleccione un docente",
                //    //    dropdownParent: $(modalEditId),
                //    //    minimumResultsForSearch: 1,
                //    //    data: data2[0].items
                //    //});
                //});


                //$("#Add_TeacherId").select2({
                //    minimumInputLength: 2,
                //    tags: [],
                //    ajax: {
                //        url: ("/profesores/get").proto().parseURL(),
                //        dataType: 'json',
                //        type: "GET",
                //        quietMillis: 50,
                //        data: function (term) {
                //            return {
                //                term: term
                //            };
                //        },
                //        results: function (data) {
                //            return {
                //                results: $.map(data, function (item) {
                //                    return {
                //                        text: item.text,                             
                //                        id: item.id
                //                    }
                //                })
                //            };
                //        }
                //    }
                //});

                //$("#Edit_TeacherId").select2({
                //    minimumInputLength: 2,
                //    tags: [],
                //    ajax: {
                //        url: ("/profesores/get").proto().parseURL(),
                //        dataType: 'json',
                //        type: "GET",
                //        quietMillis: 50,
                //        data: function (term) {
                //            return {
                //                term: term
                //            };
                //        },
                //        results: function (data) {
                //            return {
                //                results: $.map(data, function (item) {
                //                    return {
                //                        text: item.text,
                //                        id: item.id
                //                    }
                //                })
                //            };
                //        }
                //    }
                //});


                $("#Add_TeacherId").select2({
                    ajax: {
                        url: "/profesores/get",
                        type: 'GET',
                        datatype: 'json',
                        delay: 250,
                        data: function (params) {
                            return {
                                q: params.term, // search term
                                page: params.page
                            };
                        },
                        processResults: function (data, params) {
                            // parse the results into the format expected by Select2
                            // since we are using custom formatting functions we do not need to
                            // alter the remote JSON data, except to indicate that infinite
                            // scrolling can be used
                            params.page = params.page || 1;

                            return {
                                results: data.items.results,
                                pagination: {
                                    more: (params.page * 30) < data.total_count
                                }
                            };
                        },
                        cache: true
                    },
                    placeholder: 'Buscar docentes',
                    minimumInputLength: 3,
                    //templateResult: formatRepo,
                    //templateSelection: formatRepoSelection
                });


                $("#Edit_TeacherId").select2({
                    ajax: {
                        url: "/profesores/get",
                        type: 'GET',
                        datatype: 'json',
                        delay: 250,
                        data: function (params) {
                            return {
                                q: params.term, // search term
                                page: params.page
                            };
                        },
                        processResults: function (data, params) {
                            // parse the results into the format expected by Select2
                            // since we are using custom formatting functions we do not need to
                            // alter the remote JSON data, except to indicate that infinite
                            // scrolling can be used
                            params.page = params.page || 1;

                            return {
                                results: data.items.results,
                                pagination: {
                                    more: (params.page * 30) < data.total_count
                                }
                            };
                        },
                        cache: true
                    },
                    placeholder: 'Buscar docentes',
                    minimumInputLength: 3,
                    //templateResult: formatRepo,
                    //templateSelection: formatRepoSelection
                });
            }
        }
    }


    var validate = {
        init: function () {
            this.create.init();
            this.edit.init();
        },
        create: {
            init: function () {
                formCreateValidate = $(formCreateId).validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        form.submit.create(formElement);
                    }
                });
            }
        },
        edit: {
            init: function () {
                formEditValidate = $(formEditId).validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        form.submit.edit(formElement);
                    }
                });
            }
        }
    }
    return {
        init: function () {
            validate.init();
            events.init();
            select.modal.init();
        },
        Edit: {
            show: function (id) {
                form.show.edit(id);
            }
        }
    }
}();
$(function () {
    extracurricularcoursegroupTable.init();
    Form.init();
});