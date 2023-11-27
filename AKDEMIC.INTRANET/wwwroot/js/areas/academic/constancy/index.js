var InitApp = function () {
    var select = {
        init: function () {
            select.types.init();
            select.students.init();
            select.academic_record.init();
        },
        academic_record: {
            init: function () {
                $.ajax({
                    url: ("/registros_academicos/get").proto().parseURL()
                })
                    .done(function (data) {
                        $("#academic_record_select").select2({
                            placeholder: "Seleccionar usuario",
                            data: data
                        });
                    }); 
            }
        },
        types: {
            init: function () {
                $.ajax({
                    url: ("/academico/constancias/tipos/get").proto().parseURL()
                }).done(function (data) {

                    $("#type_select").select2({
                        data: data.items
                    }).on("change", function (){
                        if($(this).val() == 18 && ($("#student_select").val() == "" || $("#student_select").val() == null)){
                            $("#term_select").select2({
                                disabled: true
                            }); 
                            $(".term-select").show();                           
                        }else if($(this).val() == 18 && $("#student_select").val() != "" && $("#student_select").val() != null) {
                            $("#term_select").select2({
                                disabled: false
                            }); 
                            $(".term-select").show();  
                        }else{
                            $("#term_select").select2('data', null);
                            $(".term-select").hide();
                        }
                    });
                })
            }
        },
        students: {
            init: function () {
                $("#student_select").select2({
                    width: "100%",
                    placeholder: "Buscar...",
                    ajax: {
                        url: "/academico/alumnos/buscar".proto().parseURL(),
                        dataType: "json",
                        data: function (params) {
                            return {
                                term: params.term,
                                page: params.page
                            };
                        },
                        processResults: function (data, params) {
                            return {
                                results: data.items
                            };
                        },
                        cache: true
                    },
                    escapeMarkup: function (markup) {
                        return markup;
                    },
                    minimumInputLength: 3
                }).on("change", function (){
                    if($("#type_select").val() == 18){
                        studentId = $(this).val();
                        $.ajax({
                            url: (`/periodos-por-estudiante/get?studentId=${studentId}`).proto().parseURL()
                        }).done(function (data) {
                            $("#term_select").select2({
                                data: data.items, disabled: false
                            });    
                        });
                    }
                });
            }
        }
    };

    var datatable = {
        history: {
            object: null,
            options: getSimpleDataTableConfiguration({
                serverSide: false,
                url: "/academico/constancias/historial/get".proto().parseURL(),
                data: function (data) {
                    delete data.columns;
                    data.studentId = $("#student_select").val();
                    data.type = $("#type_select").val();
                    data.academicrecordId = $("#academic_record_select").val();
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Fecha emisión",
                        data: "date"
                    },
                    {
                        title: "Nro. constancia",
                        data: "number"
                    },
                    {
                        title: "Observaciones",
                        data: "observations"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        render: function (data) {

                            var url = `/academico/constancias/imprimir/${data.id}`.proto().parseURL();

                            return `<div class="table-options-section"> 
                                <a href="${url}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail" <span=""><i class="la la-file"></i>Imprimir</a>
                            </div>`;
                        }
                    }
                ]
            }),
            load: function () {
                if (this.object !== undefined && this.object !== null)
                    this.object.ajax.reload();
                else
                    this.object = $("#data-table").DataTable(this.options);
            }
        }
    };

    var events = {
        onHistoric: function () {
            $("#btn-history").click(function () {
                if ($("#student_select").val() == "" || $("#student_select").val() == null) {
                    toastr.error("Debe completar todos los campos", _app.constants.toastr.title.error);
                    return false;
                }
                datatable.history.load();
            });
        },
        onRegisterRequest: function () {
            $("#btn-create").click(function () {
                var type = $("#type_select").val();
                var student = $("#student_select").val();
                var academic_record = $("#academic_record_select").val();

                if (student == "" || student == null) {
                    toastr.error("Debe completar todos los campos", _app.constants.toastr.title.error);
                    return false;
                }
                if (type == 18 && (type == "" || type == null)) {
                    toastr.error("Debe completar todos los campos", _app.constants.toastr.title.error);
                    return false;
                }
                if (academic_record === "" || academic_record === null) {
                    toastr.error("Debe completar todos los campos", _app.constants.toastr.title.error);
                    return false;
                }

                var formData = new FormData();
                formData.append("StudentId", student);
                formData.append("RecordType", type);
                formData.append("AcademicRecordStaffId", academic_record);

                swal({
                    title: "Proceso de Solicitud",
                    text: "¿Seguro que desea enviar la solicitud?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise(() => {
                            $.ajax({
                                url: "/academico/constancias/generar",
                                type: "POST",
                                data: formData,
                                processData: false,
                                contentType:false
                            })
                                .done(function () {
                                    datatable.history.load();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "La solicitud ha sido enviado con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                })
                                .fail(function () {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "Error al enviar la solicitud"
                                    });
                                });
                        });
                    }
                });
            });
        },
        init: function () {
            events.onHistoric();
            events.onRegisterRequest();
        }
    };

    //var form = {
    //    create: {
    //        object: $("#create-form").validate({
    //            submitHandler: function (e) {
    //                mApp.block("#create-modal .modal-content");
    //                $.ajax({
    //                    url: $(e).attr("action"),
    //                    type: "POST",
    //                    data: $(e).serialize()
    //                }).done(function (result) {
    //                    $(".modal").modal("hide");
    //                    $(".m-alert").addClass("m--hide");

    //                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
    //                    datatable.history.load();
    //                    form.create.clear();

    //                    var url = `/academico/constancias/imprimir/${result}`.proto().parseURL();
    //                    $.fileDownload(url);
    //                })
    //                    .fail(function (error) {
    //                        if (error.responseText !== null && error.responseText !== "") $("#createAlertText").html(error.responseText);
    //                        else $("#createAlertText").html(_app.constants.ajax.message.error);

    //                        $("#createAlert").removeClass("m--hide").show();
    //                    })
    //                    .always(function () {
    //                        mApp.unblock("#create-modal .modal-content");
    //                    });
    //            }
    //        }),
    //        clear: function () {
    //            form.create.object.resetForm();
    //        }
    //    }
    //};

    return {
        init: function () {
            select.init();
            events.init();
        }
    };
}();

$(function () {
    InitApp.init();
}); 