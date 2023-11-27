var index = function () {

    var datatable = {
        correctionExam: {
            object: null,
            options: {
                ajax: {
                    url: `/admin/generacion-actas/get-examenes-subsanacion-datatable`,
                    type: "GET",
                    data: function (data) {
                        data.termId = $("#term_select").val();
                        data.careerId = $("#career_select").val();
                        data.curriculumId = $("#curriculum_select").val();
                        data.search = $("#course_search").val();

                    }
                },
                columns: [
                    {
                        title: "Cod. Curso",
                        data: "courseCode"
                    },
                    {
                        title: "Curso",
                        data: "courseName"
                    },
                    {
                        title: "Estado",
                        data: null,
                        render: function (row) {
                            if (row.evaluationReport !== null) {
                                if (row.evaluationReport.status == 1) {
                                    return '<span class="m-badge m-badge--warning m-badge--wide">Generado</span>';
                                }
                                else if (row.evaluationReport.status == 2) {
                                    return '<span class="m-badge m-badge--success m-badge--wide"> Recibido</span>';
                                }
                                else if (row.evaluationReport.status == 3) {
                                    return '<span class="m-badge m-badge--metal m-badge--wide"> Pendiente </span>';
                                }

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
                        title: "Opciones",
                        data: null,
                        render: function (row) {
                            var tpm = "";
                            if (row.evaluationReport != null) {
                                tpm += `<button data-id="${row.id}" class="btn btn-secondary btn-sm m-btn m-btn--icon btn-generate"><span><i class="la la-file-text-o"></i><span>Imprimir</span></span></button>`;
                            } else {
                                tpm += `<button data-id="${row.id}" class="btn btn-brand btn-sm m-btn m-btn--icon btn-generate"><span><i class="la la-file-text-o"></i><span>Generar</span></span></button>`;
                            }

                            return tpm;
                        }
                    }
                ]
            },
            reload: function () {
                datatable.correctionExam.object.ajax.reload();
            },
            events: {
                onPreview: function () {
                    $("#data-table").on("click", ".btn-preview-details", function () {
                        var id = $(this).data('id');
                        $("#preview-datatable").html('');
                        $("#previewView").modal('show');

                        mApp.block("#preview-datatable");

                        $.ajax({
                            url: `/admin/generacion-actas/examenes-subsanacion/vista-previa?correctionExamId=${id}`.proto().parseURL(),
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
                    })
                },
                onReport: function () {
                    $("#data-table").on("click", ".btn-generate", function () {
                        var id = $(this).data("id");
                        var $btn = $(this);
                        $btn.addLoader();
                        $.fileDownload(`/admin/generacion-actas/examenes-subsanacion/imprimir-acta?id=${id}`, {
                            httpMethod: 'GET', successCallback: function () {
                                $btn.removeLoader();
                                toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                datatable.correctionExam.reload();
                            },
                            failCallback: function (responseHtml, url) {
                                $btn.removeLoader();
                                responseHtml = responseHtml.replace(`<pre style="word-wrap: break-word; white-space: pre-wrap;">`, "");
                                responseHtml = responseHtml.replace(`</pre>`, "");
                                toastr.error(responseHtml, "Error");
                            }
                        });
                    });
                },
                init: function () {
                    this.onReport();
                    this.onPreview();
                }
            },
            init: function () {
                datatable.correctionExam.object = $("#data-table").DataTable(datatable.correctionExam.options);
                datatable.correctionExam.events.init();
            }
        },
        init: function () {
            datatable.correctionExam.init();
        }
    }

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
                        datatable.correctionExam.reload();
                    })
                });
            }
        },
        career: {
            events: {
                load: function () {
                    $.ajax({
                        url: ("/carreras/get").proto().parseURL()
                    }).done(function (data) {
                        $("#career_select").select2({
                            data: data.items,
                            allowClear: true,
                            placeholder: "Seleccionar una escuela profesional"
                        });
                    });

                },
                onChange: function () {
                    $("#career_select").on("change", function () {
                        var careerId = $(this).val();
                        select2.curriculum.events.load(careerId);
                        datatable.correctionExam.reload();
                    });
                },
                init: function () {
                    this.load();
                    this.onChange();
                }
            },
            init: function () {
                select2.career.events.init();
            }
        },
        curriculum: {
            events: {
                load: function (careerId) {
                    $("#curriculum_select").empty();
                    if (careerId === _app.constants.guid.empty || careerId === " " || careerId === undefined || careerId == null) {
                        $("#curriculum_select").select2({
                            disabled: true,
                            placeholder: "Seleccionar plan de estudio"
                        });
                    } else {
                        var termId = $("#term_select").val();
                        $.ajax({
                            url: (`/planes-estudio/${careerId}/activos/${termId}/get`).proto().parseURL()
                        }).done(function (data) {
                            data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });

                            $("#curriculum_select").select2({
                                data: data.items,
                                disabled: false
                            });
                        });
                    }
                },
                onChange: function () {
                    $("#curriculum_select").on("change", function () {
                        datatable.correctionExam.reload();
                    })
                },
                init: function () {
                    this.load();
                    this.onChange();
                }
            },
            init: function () {
                select2.curriculum.events.init();
            }
        },
        init: function () {
            select2.terms.init();
            select2.career.init();
            select2.curriculum.init();
        }
    };

    var events = {
        onSearch: function () {
            $("#course_search").doneTyping(function () {
                datatable.correctionExam.reload();
            });
        },
        init: function () {
            this.onSearch();
        }
    }


    return {
        init: function () {
            select2.init();
            events.init();
        }
    }
}();

$(() => {
    index.init();
})