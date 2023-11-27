var needreschedule = function () {

    var datatable = {
        needreschedule: {
            object: null,
            options: {
                ajax: {
                    url: "/profesor/clases/reprogramaciones/clases-necesitan-reprogramacion/get",
                    type : "GET"
                },
                columns: [
                    {
                        data: "section",
                        title :"Sección"
                    },
                    {
                        data: "courseCode",
                        title :"Code"
                    },
                    {
                        data: "course",
                        title : "Curso"
                    },
                    {
                        data: "day",
                        title: "Fecha"
                    },
                    {
                        data: "schedule",
                        title : "Horario"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        render: function (row) {
                            var tpm = "";
                            tpm += `<button data-id='${row.id}' class="btn btn-primary btn-sm m-btn m-btn m-btn--icon generate-request-need-reschedule"><span><i class="la la-edit"></i><span>Solicitar</span></span></button>`;
                            return tpm;
                        }
                    }
                ]
            },
            events: {
                onRequest: function () {
                    $("#need_reschedule").on("click", ".generate-request-need-reschedule", function () {
                        var id = $(this).data("id");
                        modal.request.show(id);
                    });
                },
                init: function () {
                    this.onRequest();
                }
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#need_reschedule").DataTable(this.options);
                this.events.init();
            }
        },
        init: function () {
            datatable.needreschedule.init();
        }
    };

    var modal = {
        request: {
            object: $("#class_need_reschedule_modal"),
            show: function (id) {
                modal.request.object.modal("show");
                modal.request.object.find("[name='ClassId']").val(id);
            },
            form: {
                object: $("#class_need_reschedule_modal_form").validate({
                    submitHandler: function (e) {
                        var formData = new FormData($(e)[0]);
                        modal.request.object.find(":input").attr("disabled", true);

                        $.ajax({
                            url: "/profesor/clases/reprogramaciones/clases-necesitan-reprogramacion/crear",
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function () {
                            modal.request.object.modal("hide");
                            datatable.needreschedule.reload();
                            classReschedule.datatable.getObject("class-reschedule-datatable-get").reload();
                            swal({
                                type: "success",
                                title: "Completado",
                                text: "Solicitud realizada con éxito.",
                                confirmButtonText: "Entendido"
                            });
                        }).fail(function (e) {
                            swal({
                                type: "error",
                                title: "Error",
                                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                confirmButtonText: "Entendido",
                                text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                            });
                        }).always(function () {
                            modal.request.object.find(":input").attr("disabled", false);
                        });
                    }
                })
            },
            events: {
                onHidden: function () {
                    modal.request.object.on('hidden.bs.modal', function (e) {
                        modal.request.form.object.resetForm();
                    });
                },
                init: function () {
                    this.onHidden();
                }
            },
            init: function () {
                this.events.init();
            }
        },
        init: function () {
            this.request.init();
        }
    };

    var datepicker = {
        init: function () {
            $(".start-date-datepicker").datepicker({
                startDate: "0d"
            });
        }
    };

    return {
        init: function () {
            datatable.init();
            datepicker.init();
            modal.init();
        }
    };
}();

$(() => {
    needreschedule.init();
});