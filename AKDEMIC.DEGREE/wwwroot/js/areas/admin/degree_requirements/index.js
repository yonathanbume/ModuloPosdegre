var degreesRequirements = function () {
    var datatable = null;

    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ""
                }
            }
        },
        columns: [
            {
                field: 'name',
                title: 'Descripción',
                width: 500
            },
            {
                field: 'code',
                title: 'Código',
                width: 100
            },            
            {
                field: 'options',
                title: 'Opciones',
                sortable: false,
                filterable: false,
                template: function (row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit" ><span><i class="flaticon-edit"></i><span>Editar</span></span></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete"><span><i class="flaticon-delete-1"></i><span>Eliminar</span></span></button>`;
                    return tmp;
                }
            }
        ]
    }

    var select2Degrees = function () {
        $.ajax({
            type: "GET",
            url: `/admin/gestion-de-requerimientos/grados-titulos`.proto().parseURL()
        }).done(function (data) {
            $(".DegreeId").select2({
                data: data.items
            });
            $(".select-degrees").select2({
                data: data.items
            }).trigger('change');
            
        });
        $(".select-degrees").on('change', function () {
            var did = $(this).val();
            $(".DegreeId").val(did).trigger('change');
            loadDatatable(did);
        });
        
    }
    var modals = function () {
        $(".btn-add").on('click', function () {                
            $('#add_requeriment_modal').modal('show');
            $("#DegreeId-1").trigger('change');
            $(".ProcedureId").select2({
                allowClear: true,
                placeholder: "Buscar por descripción del trámite TUPA",
                ajax: {
                    type : "GET",
                    url: ("/admin/gestion-de-requerimientos/tramites-TUPA/buscar").proto().parseURL(),
                    delay: 1000,
                    data: function (params) {
                        return {
                            searchValue: params.term
                        };
                    },
                    processResults: function (result) {
                        return {
                            results: $.map(result, function (item) {
                                return {
                                    text: item.text,
                                    id: item.id
                                }
                            })
                        };
                    }
                },
                escapeMarkup: function (markup) {
                    return markup;
                },
                minimumInputLength: 1
            });
            $(".ProcedureId").on('change', function () {
                var tid = $(this).val();
                if (tid != "") {
                    $.ajax({
                        type: "GET",
                        url: `/admin/gestion-de-requerimientos/tramites-TUPA/${tid}`.proto().parseURL()
                    }).done(function (data) {
                        $("#add-form input[name = 'Name']").val(data.name).prop("disabled", true);
                        $("#add-form input[name = 'Code']").val(data.code).prop("disabled", true);                     
                    });
                }
                
                
            });
            $(".ProcedureId").on("select2:unselecting", function () {
                $("#add-form input[name = 'Name']").val("").prop("disabled", false);
                $("#add-form input[name = 'Code']").val("").prop("disabled", false);          
            });
        });
    }

    var events = function () {

        $("#add-form").validate({
            submitHandler: function (form, event) {
                $.ajax({
                    type: "POST",
                    url: `/admin/gestion-de-requerimientos/agregar`.proto().parseURL(),
                    data: $(form).serialize(),
                    success: function () {
                        
                        $('#add_requeriment_modal').modal('hide');
                        $("#add-form").resetForm();
                        $("#add-form input[name = 'Name']").val("").prop("disabled", false);
                        $("#add-form input[name = 'Code']").val("").prop("disabled", false);  
                        datatable.reload();
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    },
                    error: function () {
                        toastr.success(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                    }
                });
            }
        });

        $("#edit-form").validate({
            submitHandler: function (form, event) {
                var formData = $(form).serializeArray();
                formData.push({ name: "DegreeId", value: $("#edit-form input[name ='did']").val() });

                $.ajax({
                    type: "POST",
                    url: `/admin/gestion-de-requerimientos/editar`.proto().parseURL(),
                    data: $.param(formData),
                    success: function () {
                        $('#edit_requeriment_modal').modal('hide');
                        datatable.reload();
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.update);
                    },
                    error: function () {
                        toastr.success(_app.constants.toastr.message.error.update, _app.constants.toastr.title.error);
                    }
                });
            }
        });
    }
    var eventsDatatable = function () {
        datatable.on('click', '.btn-edit', function (e) {
            e.preventDefault();
            
            var did = $(this).data('id');
            $.ajax({
                type: "GET",
                url: `/admin/gestion-de-requerimientos/obtener/${did}`.proto().parseURL()                

            }).done(function (data) {             
                $('#edit-form').trigger('reset'); 
                $('#edit_requeriment_modal').modal('show');                
                $("#edit-form select[id ='DegreeId-2']").val(data.degreeid).trigger('change').prop("disabled",true);
                $("#edit-form input[name ='Id']").val(data.id);
                $("#edit-form input[name ='did']").val(data.degreeid);
                //if (data.procedureId != null) {
                //    $("#ProcedureId-2").append(`<option value="${result.procedureId}" selected="selected">${result.procedure}</option>`);
                //} else {
                //    $("#ProcedureId-2").html("");
                //}
                //$('#edit-form input[name="Name"]').val(data.name);
                $("#ProcedureId-2").select2({
                    allowClear: true,
                    placeholder: "Buscar por descripción del trámite TUPA",
                    ajax: {
                        type: "GET",
                        url: ("/admin/gestion-de-requerimientos/tramites-TUPA/buscar").proto().parseURL(),
                        delay: 1000,
                        data: function (params) {
                            return {
                                searchValue: params.term
                            };
                        },
                        processResults: function (result) {
                            return {
                                results: $.map(result, function (item) {
                                    return {
                                        text: item.text,
                                        id: item.id
                                    }
                                })
                            };
                        }
                    },
                    escapeMarkup: function (markup) {
                        return markup;
                    },
                    minimumInputLength: 1
                });

                $("#ProcedureId-2").on('change', function () {

                    var tid = $(this).val();
                    if (tid != null) {
                        if (tid != "") {
                            $.ajax({
                                type: "GET",
                                url: `/admin/gestion-de-requerimientos/tramites-TUPA/${tid}`.proto().parseURL()
                            }).done(function (data) {
                                if (data != undefined) {
                                    $("#edit-form input[name ='Name']").val(data.name).prop("disabled", true);
                                    $("#edit-form input[name ='Code']").val(data.code).prop("disabled", true);
                                }

                            });
                        }
                        
                    }
                    


                });
                $("#ProcedureId-2").on("select2:unselecting", function () {
                    $("#edit-form input[name = 'Name']").val("").prop("disabled", false);
                    $("#edit-form input[name = 'Code']").val("").prop("disabled", false);
                });
                if (data.procedureid != null) {
                    $("#ProcedureId-2").append(`<option value="${data.procedureid}" selected="selected">${data.name}</option>`);
                } else {
                    $("#ProcedureId-2").html("");
                }
                if (data.level == false) {
                    $('#edit-form input[name="Name"]').val(data.name).prop("disabled", false);
                    $('#edit-form input[name="Code"]').val(data.code).prop("disabled", false);
                    $('#edit-form input[name="Name"]').val(data.name);
                    $('#edit-form input[name="Code"]').val(data.code);
                   
                } else {
                    $('#edit-form input[name="Name"]').val(data.name).prop("disabled", true);
                    $('#edit-form input[name="Code"]').val(data.code).prop("disabled", true);
                    //$('#edit-form select[id="ProcedureId-2"]').val(data.degreeid);
                  
                    
                }
            });

        });
        datatable.on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var did = $(this).data('id');
            swal({
                type: "warning",
                title: "¿Desea eliminar el registro?",
                showCancelButton: true,
                showLoaderOnConfirm: true,
                allowOutsideClick: () => !swal.isLoading(),
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            type: "POST",
                            url: `/admin/gestion-de-requerimientos/eliminar/${did}`.proto().parseURL()
                        }).always(function () {
                            swal.close();
                        })
                            .done(function (result) {
                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                datatable.reload();
                            })
                            .fail(function (jqXHR, textStatus, errorThrown) {
                                var responseText = jqXHR.responseText;

                                if (responseText != "" && jqXHR.status == 400) {
                                    toastr.error(responseText, _app.constants.toastr.title.error);
                                } else {
                                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                }
                            });
                    })
                }
            });
        });
    }

    var loadDatatable = function (did) {
        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        options.data.source.read.url = `/admin/gestion-de-requerimientos/todos/${did}`.proto().parseURL();
        datatable = $(".m-datatable").mDatatable(options);
        eventsDatatable();

    }

    return {
        load: function () {
            select2Degrees();
            modals();
            events();
        }
    }
}();

$(function () {
    degreesRequirements.load();
});


