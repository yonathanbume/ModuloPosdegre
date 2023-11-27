var examweek = function () {
    var datatable = {
        examweek: {
            object : null,
            options: {
                ajax: {
                    url: "/admin/semana-examenes/get",
                    type: "GEt"
                },
                columns: [
                    {
                        data: "term",
                        title : "Periodo"
                    },
                    {
                        data: "week",
                        title : "Semana"
                    },
                    {
                        data: "weekStart",
                        title : "Sem. Inicio"
                    },
                    {
                        data: "weekEnd",
                        title :"Sem. Fin"
                    }
                ]
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#datatable").DataTable(this.options);
            }
        },
        init: function () {
            this.examweek.init();
        }
    };

    var select = {
        term: {
            load: function () {
                $.ajax({
                    url: "/periodos/get",
                    type: "GET"
                })
                    .done(function (e) {
                        $("[name='TermId']").select2({
                            data: e.items,
                            placeholder: "Seleccionar periodo",
                            dropdownParent: modal.examweek.object
                        });

                        $("[name='TermId']").val(e.selected).trigger("change");
                    });
            },
            events: {
                onChange: function () {
                    $("[name='TermId']").on("change", function () {
                        var termId = $(this).val();
                        select.week.load(termId);
                    });
                },
                init: function () {
                    this.onChange();
                }
            },
            init: function () {
                this.events.init();
                this.load();
            }
        },
        week: {
            load: function (termId) {
                $("[name='Week']").empty();
                $.ajax({
                    url: `/admin/semana-examenes/get-semanas-detalle?termId=${termId}`,
                    type: "GET"
                })
                    .done(function (e) {
                        $("[name='Week']").select2({
                            data: e,
                            dropdownParent: modal.examweek.object,
                            placeholder: "Seleccionar semana"
                        });
                    })
                    .fail(function () {
                        toastr.error("Error al obtener las semanas", "Error!");
                    });
            }
        },
        type: {
            load: function () {
                $("[name='Type']").select2({
                    placeholder: "Seleccionar tipo",
                    dropdownParent: modal.examweek.object
                });
            },
            init: function () {
                this.load();
            }
        },
        init: function () {
            this.type.init();
            this.term.init();
        }
    };

    var modal = {
        examweek: {
            object: $("#add_exam_week_modal"),
            form: {
                object: $("#add_exam_week_form").validate({
                    submitHandler: function (formElement, e) {
                        swal({
                            type: "warning",
                            title: "Este proceso es irreversible.",
                            text: "¿Seguro que desea continuar?.",
                            confirmButtonText: "Aceptar",
                            showCancelButton: true,
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise(() => {
                                    var formData = new FormData(formElement);
                                    modal.examweek.object.find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                                    $.ajax({
                                        url: `/admin/semana-examenes/agregar`,
                                        type: "POST",
                                        data: formData,
                                        contentType: false,
                                        processData: false
                                    })
                                        .done(function (e) {
                                            datatable.examweek.reload();
                                            modal.examweek.object.modal("hide");
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: "Semana de examen agregada satisfactoriamente.",
                                                confirmButtonText: "Aceptar"
                                            });
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
                                            modal.examweek.object.find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                                        });
                                });
                            }
                        });
                    }
                })
            }
        }
    };

    return {
        init: function () {
            datatable.init();
            select.init();
        }
    };
}();

$(() => {
    examweek.init();
});