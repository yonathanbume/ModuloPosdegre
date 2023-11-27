var enrollmentReservation = (function () {
    var private = {
        ajax: {
            objects: {}
        },
        datatable: {
            load: {
                get: function () {
                    private.datatable.objects["enrollment-reservation-datatable-get"] = $("#enrollment-reservation-datatable-get").mDatatable({
                        data: {
                            source: {
                                read: {
                                    method: "GET",
                                    url: "/admin/matricula/reservas/get".proto().parseURL()
                                }
                            }
                        },
                        columns: [
                            {
                                field: "user.fullName",
                                title: "Nombre del Solicitante"
                            },
                            {
                                field: "total",
                                title: "Total",
                                template: function (row) {
                                    var template = "";
                                    template += "S/. ";
                                    template += row.total.toFixed(2);

                                    return template;
                                }
                            },
                            {
                                field: "paidAmount",
                                title: "Monto Pagado",
                                template: function (row) {
                                    var template = "";
                                    template += "S/. ";
                                    template += row.paidAmount.toFixed(2);

                                    return template;
                                }
                            },
                            {
                                field: "paid",
                                title: "Pagado",
                                template: function (row) {
                                    var template = "";

                                    if (row.paid) {
                                        template += "Sí";
                                    } else {
                                        template += "No";
                                    }

                                    return template;
                                }
                            },
                            {
                                field: "term.name",
                                title: "Periodo",
                                template: function (row) {
                                    var template = "";

                                    if (row.term != null) {
                                        template += row.term.name;
                                    } else {
                                        template += "---";
                                    }

                                    return template;
                                }
                            },
                            {
                                field: "parsedCreatedAt",
                                title: "Fecha de Solicitud"
                            },
                            {
                                field: "options",
                                title: "Opciones",
                                sortable: false,
                                filterable: false,
                                template: function (row) {
                                    var template = "";
                                    template += "<button class=\"btn btn-danger btn-sm m-btn m-btn--icon\" onclick=\"enrollmentReservation.swal.load.deny(this, event, '";
                                    template += "/admin/matricula/reservas/aceptar/post".proto().parseURL();
                                    template += "', '";
                                    template += row.proto().encode();
                                    template += "')\"><i class=\"la la-close\"></i></button> ";
                                    template += "<button class=\"btn btn-success btn-sm m-btn m-btn--icon\" onclick=\"enrollmentReservation.swal.load.accept(this, event, '";
                                    template += "/admin/matricula/reservas/denegar/post".proto().parseURL();
                                    template += "', '";
                                    template += row.proto().encode();
                                    template += "')\"><i class=\"la la-check\"></i></button> ";

                                    return template;
                                }
                            }
                        ]
                    });
                }
            },
            objects: {}
        },
        select2: {
            load: {
                user: function () {
                    $("#enrollment-reservation-header-form .user-select2").on("select2:select", function (event) {
                        var data = $(this).select2('data')[0];
                        var enrollmentReservationDatatableGet = enrollmentReservation.datatable.getObject("enrollment-reservation-datatable-get");

                        enrollmentReservationDatatableGet.setDataSourceParam("userId", data.id);
                        enrollmentReservationDatatableGet.load();
                    });
                }
            }
        },
        swal: {
            objects: {}
        }
    }

    return {
        ajax: {
            getObject: function (key) {
                return private.ajax.objects[key];
            },
            load: {
                accept: function (data, url) {
                    private.ajax.objects["enrollment-reservation-ajax-accept"] = $.ajax({
                        data: {
                            Id: data.id
                        },
                        type: "POST",
                        url: url
                    })
                        .always(function (data, textStatus, jqXHR) {
                            swal.close();
                        })
                        .done(function (data, textStatus, jqXHR) {
                            enrollmentReservation.datatable.getObject("enrollment-reservation-datatable-get").reload();
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        });
                },
                deny: function (data, url) {
                    private.ajax.objects["enrollment-reservation-ajax-deny"] = $.ajax({
                        data: {
                            Id: data.id
                        },
                        type: "POST",
                        url: url
                    })
                        .always(function (data, textStatus, jqXHR) {
                            swal.close();
                        })
                        .done(function (data, textStatus, jqXHR) {
                            enrollmentReservation.datatable.getObject("enrollment-reservation-datatable-get").reload();
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        });
                },
                user: function () {
                    private.ajax.objects["enrollment-reservation-ajax-user"] = $.ajax({
                        type: "GET",
                        url: "/admin/usuarios/2/get".proto().parseURL()
                    })
                        .done(function (data, textStatus, jqXHR) {
                            $("#enrollment-reservation-header-form .user-select2").proto().htmlElement(function (element) {
                                var jQueryElement = $(element);
                                var jQueryElementVal = jQueryElement.val();

                                if (jQueryElement.hasClass("select2-hidden-accessible")) {
                                    jQueryElement.select2("destroy");
                                    jQueryElement.html("");
                                }

                                _app.modules.select.fill({
                                    data: data,
                                    element: element,
                                    name: "fullName",
                                    nullable: true
                                });

                                var jQueryElementModalParent = jQueryElement.parents(".modal");
                                var select2Options = {};

                                if (jQueryElementModalParent.length > 0) {
                                    select2Options.dropdownParent = jQueryElementModalParent;
                                }

                                jQueryElement.select2(select2Options);
                            });
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.get, _app.constants.toastr.title.error);
                        });
                }
            }
        },
        datatable: {
            getObject: function (key) {
                return private.datatable.objects[key];
            }
        },
        init: function () {
            private.datatable.load.get();
            private.select2.load.user();
            enrollmentReservation.ajax.load.user();
        },
        swal: {
            getObject: function (key) {
                return private.swal.objects[key];
            },
            load: {
                accept: function (element, event, url, data) {
                    data = data.proto().decode();

                    private.swal.objects["enrollment-reservation-swal-accept"] = swal({
                        preConfirm: function () {
                            return new Promise(function (resolve, reject) {
                                enrollmentReservation.ajax.load.accept(data, url);
                            });
                        },
                        title: "¿Desea aceptar la reserva de matrícula de " + data.user.fullName + "?",
                        type: "warning",
                        showCancelButton: true,
                        showLoaderOnConfirm: true
                    });
                },
                deny: function (element, event, url, data) {
                    data = data.proto().decode();

                    private.swal.objects["enrollment-reservation-swal-deny"] = swal({
                        preConfirm: function () {
                            return new Promise(function (resolve, reject) {
                                enrollmentReservation.ajax.load.deny(data, url);
                            });
                        },
                        title: "¿Desea denegar la reserva de matrícula de " + data.user.fullName + "?",
                        type: "warning",
                        showCancelButton: true,
                        showLoaderOnConfirm: true
                    });
                }
            }
        }
    };
})();

$(function () {
    enrollmentReservation.init();
});
