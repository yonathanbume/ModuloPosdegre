var Absences = function () {
    var datatable;
    var formValidate = $("#add-form").validate();

    var options = {
        data: {
            type: "remote",
            source: {
                read: {
                    method: "GET",
                    url: ("/admin/justificacion-inasistencias/personal/get").proto().parseURL(),
                },
            }
        },
        columns: [
            {
                field: "user",
                title: "Personal",
                width: 200
            },
            {
                field: "date",
                title: "Fecha de Inasistencia",
                width: 200
            },
            {
                field: "registerDate",
                title: "Fecha de la Solicitud",
                textAlign: "center",
                width: 220
            },
            {
                field: "status",
                title: "Estado",
                textAlign: "center",
                width: 150,
                template: function (row) {
                    var status = {
                        3: { text: _app.constants.request.inProcess.text, value: _app.constants.request.inProcess.value, class: "m-badge--metal" },
                        4: { text: _app.constants.request.approved.text, value: _app.constants.request.approved.value, class: "m-badge--success" },
                        5: { text: _app.constants.request.disapproved.text, value: _app.constants.request.disapproved.value, class: "m-badge--danger" }
                    };
                    return "<span class=\"m-badge " + status[row.status].class + " m-badge--wide\">" + status[row.status].text + "</span>";
                }
            },
            {
                field: "details",
                title: "Detalle",
                width: 120,
                template: function (row) {
                    return "<button class='btn btn-default m-btn btn-sm m-btn--icon btn-detail' data-id='" + row.id + "'><span><i class='la la-eye'></i><span>Ver Detalle</span></span></button>";
                }
            },
            {
                field: "options",
                title: "Opciones",
                width: 180,
                template: function (row) {
                    var tmp = "";
                    if (row.status === _app.constants.request.inProcess.value) {
                        tmp += '<button class="btn btn-success m-btn btn-sm m-btn--icon m-btn--icon-only approve" data-id="' + row.id + '" title="Aprobar"><i class="la la-check"></i></button> ';
                        tmp += '<button class="btn btn-danger m-btn btn-sm m-btn--icon m-btn--icon-only disapprove" data-id="' + row.id + '" title="Rechazar"><i class="la la-remove"></i></button>';
                    }
                    return tmp;
                }
            }
        ]
    }

    var events = {
        init: function () {
            datatable.on("click", ".btn-detail", function () {
                var id = $(this).data("id");
                detail.show(id);
            });

            datatable.on("click", ".approve", function () {
                var id = $(this).data("id");
                swal({
                    title: "¿Está seguro?",
                    text: "La solicitud será aprobada",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, continuar",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: "/admin/justificacion-inasistencias/personal/post",
                                type: "POST",
                                data: {
                                    id: id,
                                    approved: true
                                },
                                success: function (result) {
                                    datatable.reload();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "La solicitud ha sido aprobada con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "Ocurrió un error al intentar modificar la solicitud"
                                    });
                                }
                            });
                        });
                    }
                });
            });

            datatable.on("click", ".disapprove", function () {
                var id = $(this).data("id");
                swal({
                    title: "¿Está seguro?",
                    text: "La solicitud será desaprobada",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, continuar",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: "/admin/justificacion-inasistencias/personal/post",
                                type: "POST",
                                data: {
                                    id: id,
                                    approved: false
                                },
                                success: function (result) {
                                    datatable.reload();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "La solicitud ha sido desaprobada con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "Ocurrió un error al intentar modificar la solicitud"
                                    });
                                }
                            });
                        });
                    }
                });
            });
        }
    }

    var detail = {
        show: function (id) {
            mApp.blockPage();
            $.ajax({
                url: ("/admin/justificacion-inasistencias/personal/" + id + "/get").proto().parseURL()
            }).done(function (result) {
                var formElements = $("#detail-form").get(0).elements;
                formElements["DetailUser"].value = result.user;
                formElements["DetailDate"].value = result.date;
                formElements["DetailRegisterDate"].value = result.registerDate;
                var status = {
                    3: { text: _app.constants.request.inProcess.text },
                    4: { text: _app.constants.request.approved.text },
                    5: { text: _app.constants.request.disapproved.text }
                };
                formElements["DetailStatus"].value = status[result.status].text;
                formElements["DetailJustification"].value = result.justification;
                if (result.file) {
                    $("#fileDiv").show();
                    $("#fileUrl").attr("href", `/admin/justificacion-inasistencias/personal/${id}/archivo/descargar`.proto().parseURL());
                } else {
                    $("#fileDiv").hide();
                }
                mApp.unblockPage();
                $("#detail_modal").modal("toggle");
                $("#detail_modal").on("hidden.bs.modal", function () {
                    detail.reset();
                });
            });
        },
        reset: function () {
            var formElements = $("#detail-form").get(0).elements;
            formElements["DetailStudent"].value = null;
            formElements["DetailCourse"].value = null;
            formElements["DetailClass"].value = null;
            formElements["DetailRegisterDate"].value = null;
            formElements["DetailStatus"].value = null;
            formElements["DetailJustification"].value = null;
        }
    }

    return {
        Table: {
            init: function () {
                datatable = $(".m-datatable").mDatatable(options);
                events.init();
            }
        }
    }
}();

$(function () {
    Absences.Table.init();
})