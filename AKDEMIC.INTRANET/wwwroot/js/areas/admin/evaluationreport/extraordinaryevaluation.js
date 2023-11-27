var evaluation = function () {

    var datatable = {
        object: null,
        options: {
            ajax: {
                url: "/admin/generacion-actas/evaluacion-extraordinaria/get",
                type: "GET",
                data: function (data) {
                    data.termId = $("#term_select").val();
                    data.type = $("#type_filter").val();
                }
            },
            columns: [
                {
                    title: "Cod. Curso",
                    data : "courseCode"
                },
                {
                    title: "Curso",
                    data : "courseName"
                },
                {
                    title: "Profesor",
                    data : "teacher"
                },
                {
                    title: "Estado",
                    data: null,
                    render: function (row) {
                        if (row.evaluationReport !== null) {
                            var status = {
                                "Recibido": { "title": "Recibido", "class": " m-badge--success" },
                                "Generado": { "title": "Generado", "class": " m-badge--warning" },
                                "Pendiente": { "title": "Pendiente", "class": " m-badge--metal" }
                            };
                            return '<span class="m-badge ' + status[row.evaluationReport.status].class + ' m-badge--wide">' + status[row.evaluationReport.status].title + "</span>";
                        } else {
                            return '<span class="m-badge m-badge--metal m-badge--wide">Pendiente</span>';
                        }
                    }
                },
                {
                    data: null,
                    title: "Detalle",
                    render: function (data) {
                        var buttons = `<button data-id=${data.id} class="btn btn-primary btn-sm m-btn m-btn--icon btn-preview-details"><i class="la la-eye"></i></button> `;
                        return buttons;
                    }
                },
                {
                    title: "Cant. Impresiones",
                    data: null,
                    render: function (row) {
                        if (row.evaluationReport !== null) {
                            return row.evaluationReport.printQuantity;
                        } else {
                            return '0';
                        }
                    }
                },
                {
                    title : "Opciones",
                    data: null,
                    render: function (row) {
                        var tpm = "";
                        if (row.evaluationReport != null) {
                            tpm += `<button data-id="${row.id}" class="btn btn-secondary btn-sm m-btn m-btn--icon btn-generate"><span><i class="la la-file-text-o"></i><span>Imprimir</span></span></button>`;
                            //tpm += `<button data-id="${row.id}" class="ml-1 btn btn-secondary btn-sm m-btn m-btn--icon btn-generate-v2"><span><i class="la la-file-text-o"></i><span>Registro</span></span></button>`;

                        } else {
                            if (row.complete) {
                                tpm += `<button data-id="${row.id}" class="btn btn-brand btn-sm m-btn m-btn--icon btn-generate"><span><i class="la la-file-text-o"></i><span>Generar</span></span></button>`;
                            } else {
                                tpm += `<button data-id="${row.id}" class="btn btn-brand btn-sm m-btn m-btn--icon btn-generate-incomplete"><span><i class="la la-file-text-o"></i><span>Generar</span></span></button>`;
                            }
                        }

                     
                        return tpm;
                    }
                }
            ]
        },
        reload: function () {
            this.object.ajax.reload();
        },
        events: {
            onGenerate: function () {
                $("#data-table").on("click", ".btn-generate-incomplete", function () {
                    var id = $(this).data("id");
                    var url = `/admin/generacion-actas/evaluacion-extraordinaria/imprimir/acta?evaluationId=${id}`;
                    var $btn = $(this);
                    swal({
                        title: "Evaluaciones sin notas registradas",
                        text: "Existen evaluaciones sin notas registradas, ¿desea generar el acta de todos modos?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Si",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar"
                    }).then(function (result) {
                        if (result.value) {
                            $btn.addLoader();

                            $.fileDownload(url, {
                                httpMethod: 'GET', successCallback: function () {
                                    $btn.removeLoader();
                                    toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                    datatable.reload();
                                },
                                failCallback: function (responseHtml, url) {
                                    toastr.error("No se pudo descargar el archivo", "Error");
                                }
                            });
                        }
                    });
                });

                $("#data-table").on("click", ".btn-generate", function () {
                    var id = $(this).data("id");
                    var url = `/admin/generacion-actas/evaluacion-extraordinaria/imprimir/acta?evaluationId=${id}`;
                    var $btn = $(this);
                    $btn.addLoader();

                    $.fileDownload(url, {
                        httpMethod: 'GET', successCallback: function () {
                            $btn.removeLoader();
                            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                            datatable.reload();
                        },
                        failCallback: function (responseHtml, url) {
                            toastr.error("No se pudo descargar el archivo", "Error");
                        }
                    });
                });

                $("#data-table").on("click", ".btn-generate-v2", function () {
                    var id = $(this).data("id");
                    var url = `/admin/generacion-actas/evaluacion-extraordinaria/imprimir/acta-registro?evaluationId=${id}`;
                    var $btn = $(this);
                    $btn.addLoader();

                    $.fileDownload(url, {
                        httpMethod: 'GET', successCallback: function () {
                            $btn.removeLoader();
                            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                            datatable.reload();
                        },
                        failCallback: function (responseHtml, url) {
                            toastr.error("No se pudo descargar el archivo", "Error");
                        }
                    });
                });

                $("#data-table").on('click', ".btn-preview-details", function () {
                        var id = $(this).data('id');
                        $("#preview-datatable").html('');
                        $("#previewView").modal('show');

                        mApp.block("#preview-datatable");

                        $.ajax({
                            url: `/admin/generacion-actas/evaluacion-extraordinaria/vista-previa?evaluationId=${id}`.proto().parseURL(),
                            type: "GET",
                            dataType: "html",
                            contextType: "application/json"
                        })
                            .done(function (data) {
                                $("#preview-datatable").html(data);
                            })
                            .fail(function (data) {
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            }).always(function () {
                                mApp.unblock("#preview-datatable");
                            });

                    });
            },
            init: function () {
                this.onGenerate();
            }
        },
        init: function () {
            datatable.object = $("#data-table").DataTable(this.options);
            datatable.events.init();
        }
    };

    var select2 = {
        terms: {
            init: function () {
                select2.terms.load();
            },
            load: function () {
                $.ajax({
                    url: ("/periodos/get").proto().parseURL(),
                    success: function (result) {
                        $("#term_select").select2({
                            data: result.items
                        });   
                    }
                }).done(function () {
                    datatable.init();             
                    $("#term_select").on("change", function () {
                        datatable.reload();
                    })
                });
            }
        },
        type: {
            init : function() {
                $("#type_filter").select2();

                $("#type_filter").on("change", function () {
                    datatable.reload();

                });
            }
        },
        init: function () {
            select2.terms.init();
            select2.type.init();
        }
    };

    return {
        init: function () {
            select2.init();
        }        
    };
}();

$(() => {
    evaluation.init();
});