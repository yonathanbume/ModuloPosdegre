var isInformationTupa = ($("#informationtupa").val() == 'True') || ($("#informationtupa").val() == 'true');

var procedure = (function () {
    var private = {
        ajax: {
            objects: {}
        },
        datatable: {
            load: {
                get: function () {
                    private.datatable.objects["procedure-datatable-get"] = $("#procedure-datatable-get").mDatatable({
                        data: {
                            source: {
                                read: {
                                    method: "GET",
                                    url: "/tramites/get".proto().parseURL()
                                }
                            }
                        },
                        columns: [
                            {
                                field: "name",
                                title: "Nombre del Trámite"
                            },
                            {
                                field: "duration",
                                title: "Duración",
                                template: function (row) {
                                    var template = "";
                                    template += row.duration;
                                    template += row.duration !== 1 ? " días" : " día";

                                    return template;
                                }
                            },
                            {
                                field: "procedureRequirementsCostSum",
                                title: "Costo",
                                template: function (row) {
                                    var template = "";
                                    template += "S/. ";
                                    template += row.procedureRequirementsCostSum.toFixed(2);

                                    return template;
                                }
                            },
                            {
                                field: "uit",
                                title: "UITs",
                                template: function (row) {
                                    var template = "";

                                    if (uit != null) {
                                        template += (row.procedureRequirementsCostSum / uit.value).toFixed(2);
                                        template += " UITs";
                                    } else {
                                        template += "---";
                                    }

                                    return template;
                                }
                            },
                            {
                                field: "options",
                                title: "Opciones",
                                sortable: false,
                                filterable: false,
                                template: function (row) {
                                    var template = "";
                                    //template += "<button class=\"btn btn-primary btn-sm m-btn m-btn--icon\" onclick=\"procedure.modal.load.request(this, event, '";
                                    //template += row.proto().encode();
                                    //template += "')\"><span><i class=\"la la-book\"></i><span> Solicitar </span></span></button> ";
                                    template += `<a class="btn btn-primary btn-sm m-btn m-btn--icon" href="${"/tramites/usuarios/solicitar-tramite".proto().parseURL()}/${row.id}">`;
                                    template += `<span><i class="la la-book"></i><span>Ver Detalle</span></span></a> `;
                                    return template;
                                }
                            }
                        ]
                    });
                }
            },
            objects: {}
        },
        modal: {
            objects: {}
        },
        validate: {
            load: {
                request: function () {
                    private.validate.objects["procedure-modal-request-form"] = $("#procedure-modal-request-form").validate({
                        submitHandler: function (form, event) {
                            event.preventDefault();
                            procedure.ajax.load.request(form, event);
                        }
                    });
                }
            },
            objects: {}
        }
    };

    return {
        ajax: {
            getObject: function (key) {
                return private.ajax.objects[key];
            },
            load: {
                request: function (element, event) {
                    var formElements = element.elements;

                    mApp.block(".modal-content");

                    private.ajax.objects["procedure-ajax-request"] = $.ajax({
                        data: {
                            ProcedureId: formElements["ProcedureId"].value,
                            Comment: formElements["Comment"].value
                        },
                        type: element.method,
                        url: element.action
                    })
                        .always(function (data, textStatus, jqXHR) {
                            mApp.unblock(".modal-content");
                        })
                        .done(function (data, textStatus, jqXHR) {
                            _app.modules.form.reset({
                                element: element
                            });

                            activeUserProcedure.datatable.getObject("active-user-procedure-datatable-get").reload();
                            procedure.modal.getObject("procedure-modal-request").modal("hide");
                            toastr.success(_app.constants.toastr.message.success.create, _app.constants.toastr.title.success);
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            var responseText = jqXHR.responseText;

                            if (responseText != "" && jqXHR.status == 400) {
                                toastr.error(responseText, _app.constants.toastr.title.error);
                            } else {
                                toastr.error(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                            }
                        });
                },

            }
        },
        datatable: {
            getObject: function (key) {
                return private.datatable.objects[key];
            }
        },
        init: function () {
            private.datatable.load.get();
            private.validate.load.request();

        },
        modal: {
            getObject: function (key) {
                return private.modal.objects[key];
            },
            load: {
                request: function (element, event, data) {
                    var procedureModalRequestForm = document.getElementById("procedure-modal-request-form");
                    data = data.proto().decode();

                    _app.modules.form.reset({
                        element: procedureModalRequestForm
                    });

                    _app.modules.form.fill({
                        element: procedureModalRequestForm,
                        data: {
                            ProcedureId: data.id
                        }
                    });

                    private.modal.objects["procedure-modal-request"] = $("#procedure-modal-request").modal("show");
                }
            }
        },
        validate: {
            getObject: function (key) {
                return private.validate.objects[key];
            }
        }
    };
})();

var activeUserProcedure = (function () {
    var private = {
        datatable: {
            load: {
                get: function () {
                    private.datatable.objects["active-user-procedure-datatable-get"] = $("#active-user-procedure-datatable-get").mDatatable({
                        data: {
                            source: {
                                read: {
                                    method: "GET",
                                    url: "/tramites/usuarios/activos/get".proto().parseURL()
                                }
                            }
                        },
                        columns: [
                            {
                                field: "procedure.name",
                                title: "Nombre del Trámite"
                            },
                            {
                                field: "dependency.name",
                                title: "Dependencia Actual",
                                template: function (row) {
                                    var template = "";

                                    if (row.dependency != null) {
                                        template += row.dependency.name;
                                    } else {
                                        template += "---";
                                    }

                                    return template;
                                }
                            },
                            {
                                field: "status",
                                title: "Estado",
                                template: function (row) {
                                    var template = "";
                                    template += userProcedureStatusValues[row.status];

                                    return template;
                                }
                            },
                            {
                                field: "parsedCreatedAt",
                                title: "Fecha de Solicitud",
                                template: function (row) {
                                    var createdAtDate = new Date(row.createdAt);
                                    var createdAtDuration = createdAtDate.proto().addDays(row.procedure.duration);
                                    var createdAtDurationDate = createdAtDuration.proto().toDate();
                                    var date = new Date().proto().toDate();
                                    var template = "";
                                    template = "<span class=\"m--font-"

                                    if (createdAtDurationDate < date) {
                                        template += "danger";
                                    } else if (createdAtDurationDate.valueOf() == date.valueOf()) {
                                        template += "warning";
                                    } else {
                                        template += "success";
                                    }

                                    template += "\">";
                                    template += row.parsedCreatedAt;
                                    template += "</span>";

                                    return template;
                                }
                            },
                            {
                                field: "options",
                                title: "Opciones",
                                sortable: false,
                                filterable: false,
                                template: function (row) {
                                    var template = "";
                                    if (row.status == 1) {
                                        var url = `/tramites/usuarios/tramite-solicitado/${row.id}`;
                                        template += `<a class="btn btn-info btn-sm m-btn m-btn--icon" href="${url}/"><span><i class="la la-book"></i><span>Continuar</span></span></a>`;
                                    }
                                    else {
                                        template += "<button class=\"btn btn-primary btn-sm m-btn m-btn--icon\" onclick=\"activeUserProcedure.redic.detail(this, event, '";
                                        template += row.proto().encode();
                                        template += "')\"><span><i class=\"la la-book\"></i><span> Ver Detalle </span></span></button> ";
                                        if (row.status == 10) {
                                            template += `<a class="btn btn-primary btn-sm m-btn m-btn--icon" href="${"/tramites/usuarios/continuar-tramite".proto().parseURL()}/${row.id}">`;
                                            template += `<span><i class="la la-book"></i><span>Continuar Trámite</span></span></a> `;
                                        }
                                    }                                   

                                    return template;
                                }
                            }
                        ],
                        search: {
                            input: $("#search-2")
                        }
                    });
                }
            },
            objects: {}
        },
        init: function () {
            private.datatable.load.get();
        }
    };

    return {
        datatable: {
            getObject: function (key) {
                return private.datatable.objects[key];
            }
        },
        init: function () {
            return private.init();
        },
        redic: {
            detail: function (element, event, data) {
                data = data.proto().decode();
                var id = data.id;
                window.location.href = `/tramites/usuarios/detalle-tramite/${id}`;
            }
        }
    };
})();

var historicUserProcedure = (function () {
    var private = {
        datatable: {
            load: {
                get: function () {
                    private.datatable.objects["historic-user-procedure-datatable-get"] = $("#historic-user-procedure-datatable-get").mDatatable({
                        data: {
                            source: {
                                read: {
                                    method: "GET",
                                    url: "/tramites/usuarios/historicos/get".proto().parseURL()
                                }
                            }
                        },
                        columns: [
                            {
                                field: "procedure.name",
                                title: "Nombre del Trámite"
                            },
                            {
                                field: "dependency.name",
                                title: "Dependencia Actual",
                                template: function (row) {
                                    var template = "";

                                    if (row.dependency != null) {
                                        template += row.dependency.name;
                                    } else {
                                        template += "---";
                                    }

                                    return template;
                                }
                            },
                            {
                                field: "procedure.duration",
                                title: "Duración",
                                template: function (row) {
                                    var template = "";
                                    template += row.procedure.duration;
                                    template += row.procedure.duration !== 1 ? " días" : " día";

                                    return template;
                                }
                            },
                            {
                                field: "status",
                                title: "Estado",
                                template: function (row) {
                                    var template = "";
                                    if (row.status == 7) {
                                        template += '<span style="width: 222px;color: #d48b6f;font-weight: bold;">' + userProcedureStatusValues[row.status] + '</span>'
                                    }
                                    else {
                                        template += userProcedureStatusValues[row.status];
                                    }


                                    return template;
                                }
                            },
                            {
                                field: "parsedCreatedAt",
                                title: "Fecha de Solicitud"
                            },
                            {
                                field: "parsedUpdatedAt",
                                title: "Fecha de Finalización"
                            },
                            {
                                field: "options",
                                title: "Opciones",
                                sortable: false,
                                filterable: false,
                                overflow: "visible",
                                template: function (row) {
                                    var template = "";
                                    //template += "<button class=\"btn btn-primary btn-sm m-btn m-btn--icon\" onclick=\"historicUserProcedure.modal.load.observation(this, event, '";
                                    //template += row.proto().encode();
                                    //template += "')\"><span><i class=\"la la-comment\"></i><span> Observación </span></span></button> ";
                                    template += "<button class=\"btn btn-primary btn-sm m-btn m-btn--icon\" onclick=\"historicUserProcedure.modal.redic.detail(this, event, '";
                                    template += row.proto().encode();
                                    template += "')\"><span><i class=\"la la-book\"></i><span> Ver Detalle </span></span></button> ";
                                    return template;
                                }
                            }
                        ],
                        search: {
                            input: $("#search-3")
                        }
                    });
                }
            },
            objects: {}
        },
        init: function () {
            private.datatable.load.get();
        },
        modal: {
            objects: {}
        }
    };

    return {
        datatable: {
            getObject: function (key) {
                return private.datatable.objects[key];
            }
        },
        init: function () {
            return private.init();
        },
        modal: {
            getObject: function (key) {
                return private.modal.objects[key];
            },
            load: {
                observation: function (element, event, data) {
                    var observationTemplate = "";
                    data = data.proto().decode();

                    observationTemplate += "<p>";
                    observationTemplate += data.observation != null ? data.observation : "No hay datos disponibles.";
                    observationTemplate += "</p>";

                    document.getElementById("historic-user-procedure-modal-observation-content").innerHTML = observationTemplate;
                    private.modal.objects["historic-user-procedure-modal-observation"] = $("#historic-user-procedure-modal-observation").modal("show");
                }
            },
            redic: {
                detail: function (element, event, data) {
                    data = data.proto().decode();
                    var id = data.id;
                    window.location.href = `/tramites/usuarios/detalle-tramite/${id}`;
                }
            }
        },
    };
})();

$(function () {
    procedure.init();
    activeUserProcedure.init();
    historicUserProcedure.init();

    $(".nav-link.m-tabs__link").on("shown.bs.tab", function (event) {
        procedure.datatable.getObject("procedure-datatable-get").adjustCellsWidth();
        activeUserProcedure.datatable.getObject("active-user-procedure-datatable-get").adjustCellsWidth();
        historicUserProcedure.datatable.getObject("historic-user-procedure-datatable-get").adjustCellsWidth();
    });
});
