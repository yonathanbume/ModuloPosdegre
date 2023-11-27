var InitApp = function () {
    var datatable = {
        reprogram: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/clases/reprogramaciones/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.startSearchDate = $("#startSearchDate").val();
                        data.endSearchDate = $("#endSearchDate").val();
                        data.search = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        data: "fullName",
                        title: "Nombre del Solicitante"
                    },
                    {
                        data: "course",
                        title: "Curso"
                    },
                    {
                        data: "startTime",
                        title: "Inicio (Actual)"
                    },
                    {
                        data: "createdAt",
                        title: "Fecha de Solicitud"
                    },
                    {
                        data: "isPermanent",
                        title: "¿Es Permanente?"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        render: function (data) {
                            var template = "";
                            template = "<button data-target='#info_modal' data-toggle='modal' class='btn btn-info m-btn btn-sm m-btn--icon btn-detail' data-id='" + data.id + "'><i class='la la-eye'></i></button>";
                            ////Accept
                            //template += "<button ";
                            //template += "class='btn btn-info ";
                            //template += "m-btn btn-sm m-btn--icon btn-aceptar' ";
                            //template += " data-userfullname='" + data.fullName + "' data-id='" + data.id + "'>";
                            //template += "<i class='la la-check'></i></button>";
                            ////Deny
                            //template += "<button ";
                            //template += "class='btn btn-danger btn-denegar ";
                            //template += "m-btn btn-sm  m-btn--icon-only' ";
                            //template += " data-userfullname='" + data.fullName + "' data-id='" + data.id + "'>";
                            //template += "<i class='la la-close'></i></button>";
                            return template;
                        }
                    }
                ]
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#data-table").on('click', '.btn-detail', function () {
                    var id = $(this).data("id");
                    options.showmodal(id);
                });
                $('.btn-aceptar').on('click', function () {
                    var id = $("#info_modal #Id").val();
                    var userfullname = $("#info_modal #teacher").val();
                    options.accept(id, userfullname);
                });

                $('.btn-denegar').on('click', function () {
                    var id = $("#info_modal #Id").val();
                    var userfullname = $("#info_modal #teacher").val();
                    options.deny(id, userfullname);
                });
            }
        },
        init: function () {
            this.reprogram.init();
        }
    };
    var beforeStartDate = "";
    var beforeEndDate = "";
    var datepicker = {
        init: function () {
            $("#startSearchDate").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            }).on("changeDate", function (e) {
                $("#endSearchDate").datepicker("setStartDate", moment(e.date).toDate());
                beforeStartDate = $("#startSearchDate").val();
                datatable.reprogram.reload();
            });

            $("#endSearchDate").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            }).on("changeDate", function (e) {
                $("#startSearchDate").datepicker("setEndDate", e.date);
                beforeEndDate = $("#endSearchDate").val();
                datatable.reprogram.reload();
            });

            $("#startSearchDate").datepicker().on("hide", function () {
                var actualDate = $("#startSearchDate").val();

                var testStart = $("#startSearchDate").datepicker('getDate');

                if (testStart == null) {
                    $("#endSearchDate").datepicker("setStartDate", -Infinity);
                }


                if (actualDate != beforeStartDate) {
                    datatable.reprogram.reload();
                    beforeStartDate = actualDate;
                }

            });

            $("#endSearchDate").datepicker().on("hide", function (e) {
                var actualDate = $("#endSearchDate").val();
                var testEnd = $("#endSearchDate").datepicker('getDate');

                if (testEnd == null) {
                    $("#startSearchDate").datepicker("setEndDate", Infinity);
                }

                if (actualDate != beforeEndDate) {
                    datatable.reprogram.reload();
                    beforeEndDate = actualDate;
                }
            });
        }
    };
    var options = {
        showmodal: function (id) {
            $("#info_modal input").prop("disabled", true);
            $.ajax({
                url: (`/admin/clases/reprogramaciones/get/${id}`).proto().parseURL(),
                type: "GET",
                contentType: false,
                processData: false,
                success: function (result) {
                    $("#info_modal #Id").val(result.id);
                    $("#info_modal #teacher").val(result.fullName);
                    $("#info_modal #course").val(result.course);
                    $("#info_modal #section").val(result.section);
                    $("#info_modal #startDateActual").val(result.startTime);
                    $("#info_modal #endDateActual").val(result.endTime);
                    $("#info_modal #startDate").val(result.startDateTime);
                    $("#info_modal #endDate").val(result.endDateTime);
                    $("#info_modal #isPermanent").val(result.isPermanent);
                },
                error: function (errormessage) {

                }
            });
        },
        accept: function (id, userfullname) {
            swal({
                title: "¿Está seguro?",
                text: "¿Desea aceptar la solicitud de reprogramación de clase de " + userfullname + " ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, aceptarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        var formData = new FormData();
                        formData.append("Id", id);
                        $.ajax({
                            url: ("/admin/clases/reprogramaciones/aceptar/post").proto().parseURL(),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false,
                            success: function (result) {
                                $("#info_modal").modal("hide");
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "Se ha aceptado la solicitud de reprogramación de clase de " + userfullname + " con exito.",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.reprogram.reload());
                            },
                            error: function (e) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },
        deny: function (id, userfullname) {
            swal({
                title: "¿Está seguro?",
                text: "¿Desea denegar la solicitud de reprogramación de clase de " + userfullname + " ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, aceptarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/admin/clases/reprogramaciones/denegar/post").proto().parseURL(),
                            type: "POST",
                            data: {
                                id: id
                            },
                            success: function (result) {
                                $("#info_modal").modal("hide");
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "Se ha denegado la solicitud de reprogramación de clase de " + userfullname + " con exito.",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.reprogram.reload());
                            },
                            error: function (e) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },
    };

    var events = {
        onSearch: function () {
            $("#search").doneTyping(function () {
                datatable.reprogram.reload();
            })
        },
        init: function () {
            this.onSearch();
        }
    }
    return {
        init: function () {
            datatable.init();
            datepicker.init();
            events.onSearch();
        }
    }
}();

$(function () {
    InitApp.init();
})



