var report = function () {

    var datatable = {
        evaluationreports: {
            object: null,
            options: {
                ajax: {
                    url: `/admin/generacion-actas/buscador/datatable`,
                    data: function (data) {
                        delete data.columns;
                        data.careerId = $("#career_select").val();
                        data.code = $("#evaluation_report_code").val();
                        data.termId = $("#term_select").val();
                        data.curriculumId = $("#curriculum_select").val();
                        data.resolutionNumber = $("#resolution_number").val();
                        data.typeSelect = $("#type_select").val();
                        data.courseSearch = $("#course_input").val();
                        data.onlyReceived = true;
                    }
                },
                columns: [
                    {
                        title: "Periodo",
                        data: "term"
                    },
                    {
                        title: "Curso",
                        data: "course"
                    },
                    {
                        title: "Acta",
                        data: "code"
                    },
                    {
                        title: "Fec. Recepción",
                        data: "receptionDate"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            downloadUrl = `/admin/generacion-actas/acta/${data.id}/pdf`.proto().parseURL();
                            var template = "";
                            //Descargar
                            template += "<button ";
                            template += "class='btn btn-primary ";
                            template += "m-btn btn-sm m-btn--icon btn-download' ";
                            template += ` data-url=${downloadUrl}>`;
                            template += "<span><i class='la la-file-text-o'></i><span>Descargar</span></span></button> ";

                            if (data.type == 1) {
                                template += "<button ";
                                template += "class='btn btn-primary ";
                                template += "m-btn btn-sm m-btn--icon btn-download-excel' ";
                                template += ` data-id="${data.id}">`;
                                template += "<span><i class='la la-file-excel-o'></i><span>EXCEL</span></span></button> ";
                            }

                            return template;
                        }
                    }
                ]
            },
            reload: function () {
                if (datatable.evaluationreports.object === null) {
                    datatable.evaluationreports.object = $("#data-table").DataTable(datatable.evaluationreports.options);
                } else {
                    datatable.evaluationreports.object.ajax.reload();
                }
            },
            events: function () {
                $("#data-table").on('click', '.btn-download', function () {
                    var $btn = $(this);
                    var url = $btn.data("url");
                    $btn.addLoader();
                    $.fileDownload(url, {
                        httpMethod: "GET",
                    })
                        .done(function () {
                            $btn.removeLoader();
                            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                        })
                        .fail(function (e) {
                            $btn.removeLoader();
                            toastr.error("Sucedio un Error", "Error");
                        });
                });

                $("#data-table").on('click', '.btn-download-excel', function () {
                    var $btn = $(this);
                    var id = $btn.data("id");
                    var url = `/admin/generacion-actas/acta/${id}/excel`;
                    $btn.addLoader();
                    $.fileDownload(url, {
                        httpMethod: "GET",
                    })
                        .done(function () {
                            $btn.removeLoader();
                            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                        })
                        .fail(function (e) {
                            $btn.removeLoader();
                            toastr.error("Sucedio un Error", "Error");
                        });
                });

                $("#data-table").on('click', '.btn-detail', function () {
                    var $btn = $(this);
                    var url = $btn.data("url");
                    window.location.href = url;
                });
            },
            init: function () {
                datatable.evaluationreports.events();
            }
        },
        init: function () {
            datatable.evaluationreports.init();
        }
    };

    var select2 = {
        init: function () {
            this.type.init();
            this.terms.init();
            this.careers.init();
            this.curriculums.init();
        },
        type: {
            init: function () {
                $("#type_select").select2({
                    minimumResultsForSearch: -1,
                    placeholder: "Seleccionar tipo de busqueda"
                });
                select2.type.events();
            },
            events: function () {
                $("#type_select").on('change', function () {
                    //Por Numero - 1
                    //Por Curso - 2

                    $("#resolution_code").val("");
                    $("#career_select").val(null).trigger("change");
                    $("#course_input").val("");

                    if ($(this).val() == 1) {
                        //Limpiamos campos
                        $("#code_option").addClass("d-none");
                        $("#course_option").removeClass("d-none");
                        return;
                    }

                    $("#btn-search").prop('disabled', false);
                    if ($(this).val() == 2) {
                        $("#code_option").removeClass("d-none");
                        $("#course_option").addClass("d-none");
                    }
                });
            }
        },
        terms: {
            init: function () {
                select2.terms.load();
                select2.terms.events();
            },
            load: function () {
                $.ajax({
                    url: ("/periodos/get").proto().parseURL()
                }).done(function (result) {
                    $("#term_select").select2({
                        data: result.items
                    }).trigger("change");
                });
            },
            events: function () {
                $("#term_select").on("change", function () {
                    datatable.evaluationreports.reload();
                });
            }
        },
        careers: {
            init: function () {
                select2.careers.load();
                select2.careers.events();
            },
            load: function () {
                $.ajax({
                    url: ("/carreras/v2/get").proto().parseURL()
                }).done(function (result) {
                    $("#career_select").select2({
                        placeholder: "Seleccionra escuela",
                        data: result.items
                    });
                });
            },
            events: function () {
                $("#career_select").on('change', function (e, state) {
                    //we check state if exists and is true then event was triggered
                    if (typeof state != 'undefined' && state) {
                        $("#curriculum_select").prop('disabled', true);
                        $("#course_select").prop('disabled', true);
                        return false;
                    }
                    select2.curriculums.load();
                    $('#course_select').html("<option value='0' disabled selected>Seleccione un Curso</option>");
                    $("#course_select").prop('disabled', true);

                })
            }
        },
        curriculums: {
            init: function () {
                $("#curriculum_select").select2();
                select2.curriculums.events();
            },
            load: function () {
                $("#curriculum_select").prop('disabled', false);
                $('#curriculum_select').html("<option value='0' disabled selected>Seleccione un Plan</option>");
                $.ajax({
                    url: (`/carreras/${$("#career_select").val()}/planestudio/get`).proto().parseURL()
                }).done(function (result) {
                    $("#curriculum_select").select2({
                        data: result.items
                    }).trigger("change");
                });
            },
            events: function () {
                $("#curriculum_select").on('change', function (e, state) {
                    datatable.evaluationreports.reload();
                });
            }
        }
    };

    var events = {
        onSerachByCode: function () {
            $("#search_by_code").on("click", function () {
                var value = $("#evaluation_report_code").val();

                if (value === null || value === undefined || value === "") {
                    toastr.info("Debe ingresar el código del acta.", "Información");
                    return;
                }

                datatable.evaluationreports.reload();
            });
        },
        onSearchCourse: function () {
            $("#course_input").doneTyping(function () {
                datatable.evaluationreports.reload();
            });
        },
        init: function () {
            events.onSerachByCode();
            events.onSearchCourse();
        }
    };


    return {
        init: function () {
            select2.init();
            events.init();
            datatable.init();
        }
    };
}();

$(() => {
    report.init();
});
