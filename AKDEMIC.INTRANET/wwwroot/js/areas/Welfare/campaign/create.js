var SimulationSubjectTable = function () {
    var array = [];
    var Source_consult_reason = [];
    var id = "#psychologytestquestion-datatable";
    var datatable;
    var options = {
        data: {
            type: "local",
            source: Source_consult_reason
        },
        columns: [
            {
                field: 'FullName',
                title: 'Nombres y Apellidos'
            },
            {
                field: 'Faculty',
                title: 'Escuela profesional'
            },
            {
                field: 'Cicle',
                title: 'Ciclo'
            },
            {
                field: 'Code',
                title: 'código'
            },

            {
                field: "options",
                title: "Firma",
                template: function (row, index) {
                    var tmp = '';
                    return tmp;

                }

            }
        ]
    }  ;

    var events = {
        init: function () {
            datatable.on("click",
                ".btn-delete",
                function () {
                    var dataId = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "La campaña será eliminada",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, eliminarla",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        allowOutsideClick: ( ) => !swal.isLoading(),
                        preConfirm: () => {
                            return new Promise(() => {
                                $.ajax({
                                    url: ("/welfare/preguntas-categoria/eliminar/post").proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        id: dataId
                                    },
                                    success: function () {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "La pregunta ha sido eliminada con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                    },
                                    error: function () {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Al parecer la pregunta tiene información relacionada"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });

            $("#btn-add").on("click",
                function() {

                });
        }

    };

    var modal = {
        initAdd: function () {
            $(".alumno").on("click",
                function () {
                    //$.ajax({
                    //    type: "GET",
                    //    url: "/welfare/campania/student".proto().parseURL()
                    //}).done(function (result) {
                    //    $("#StudentSelect").select2({
                    //        data: result.items,
                    //        placeholder: "Seleccione el alumno",
                    //        allowClear: true
                    //    });
                    //    $("#StudentSelect").val(null).trigger("change");
                    //    });

                    $('#StudentSelect').select2({
                        allowClear: true,
                        placeholder: "Buscar..",
                        ajax: {
                            type: 'GET',
                            url: ("/alumnosv2/get").proto().parseURL(),
                            delay: 1000,
                            data: function (params) {
                                return {
                                    searchValue: params.term
                                };
                            },
                            processResults: function (data) {
                                return {
                                    results: data.results
                                };
                                //return {
                                //    results: $.map(result, function (item) {
                                //        return {
                                //            text: item.text,
                                //            id: item.id
                                //        }
                                //    })
                                //};
                            },
                            escapeMarkup: function (markup) {
                                return markup;
                            },
                            minimumInputLength: 1
                        }
                    });
                    $("#StudentSelect").val(null).trigger("change");
                    $("#add_student_modal").modal("show");
                });

            

            $(".alumno-externo").on("click",
                function () {
                    $("#add_student_extern_modal").modal("show");
                });

            $(".btn-add").on("click", function () {
                array.push({
                    FullName: $("#namestudenthidden").val(),
                    Cicle: $("#cicleu").val(),
                    Code: $("#codeu").val(),
                    Faculty: $("#facultyu").val(),
                    GuIdStudent: $("#StudentSelect").val(),
                    type: 1
                });
                datatable.originalDataSet = array;
                datatable.reload();
                $("#add_student_modal").modal("hide");
            });

            $(".btn-addExternal").on("click", function () {
                array.push({
                    FullName: $("#studentexternal").val(),
                    Cicle: $("#Cicle").val(),
                    Code: $("#Code").val(),
                    Faculty: $("#Faculty").val(),
                    type: 2
                });
                datatable.originalDataSet = array;
                datatable.reload();
                $("#add_student_extern_modal").modal("hide");
                
            });

            $("#StudentSelect").on("change", function () {
                var vl = $(this).val();
                
                $.ajax({
                    type: "GET",
                    url: `/welfare/campania/studentselect/${vl}`.proto().parseURL()
                }).done(function (result) {
                    console.log(result);
                    $("#cicleu").val(result.cicle);
                    $("#facultyu").val(result.faculty);
                    $("#codeu").val(result.code);
                    $("#namestudenthidden").val(result.fullname);
                });
            });

            $("#add-form").validate({
                submitHandler: function () {
                    var form = $("#add-form").serializeArray();
                    form.push({ name: "Participants", value: JSON.stringify(array)})
                    $.ajax({
                        type: 'POST',
                        url: "/welfare/campania/crear/post".proto().parseURL(),
                        data: $.param(form),
                        success: function () {
                            datatable.reload();
                            $("#add_student_modal").modal("hide");
                            $("#add_student_extern_modal").modal("hide");
                            location.href = "/welfare/campania".proto().parseURL()
                            //toastr.success(_app.constants.toastr.message.success.update, _app.constants.toastr.message.success.title);
                        }

                    });
                }
            });
        }
    };

    var select = {
        init: function () {
            $.ajax({
                type: "GET",
                url: "/welfare/campania/encargados".proto().parseURL()
            }).done(function(result) {
                $("#userrole").select2({
                    data: result.items
                });
            });


        }
    }

    var dates = {
        init: function() {
            $(".datepicker").datepicker();
            $(".timepicker").timepicker();
        }
    }
  
    return {
        init: function () {
            datatable = $(id).mDatatable(options);
            events.init();
            select.init();
            dates.init();
            modal.initAdd();
        },
        reload: function () {
            datatable.reload();

        }
    }
}();

$(function () {
    SimulationSubjectTable.init();
});