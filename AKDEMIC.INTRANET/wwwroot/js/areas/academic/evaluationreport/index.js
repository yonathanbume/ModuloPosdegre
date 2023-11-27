var InitApp = function () {
    var datatable = {
        reports: {
            object: null,
            options: getSimpleDataTableConfiguration({
                url: "/academico/recepcion-ficha/get".proto().parseURL(),
                data: function (data) {
                    delete data.columns;
                    data.search = $("#search").val();
                    data.faculty = $("#faculty_select").val();
                    data.school = $("#school_select").val();
                    data.career = $("#career_select").val();
                    data.status = $("#status_select").val();
                    data.termId = $("#term_select").val();

                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        data: "code",
                        title: "Código de Acta"
                    },
                    {
                        data: "courseName",
                        title: "Curso"
                    },
                    {
                        data: "typeText",
                        title: "Tipo"
                    },
                    {
                        data: "lastGenerated",
                        title: "Ultima emisión"
                    },
                    {
                        data: "status",
                        title: "Estado"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        orderable: false,
                        render: function (data) {
                            if (!data.isReceived) {
                                return `<button data-id=${data.id} class="btn btn-primary btn-sm m-btn m-btn--icon btn-receive"><span><i class="la la-check"></i><span> Recibir </span></span></a>`;
                            }
                            return "---";

                        }
                    }
                ]
            }),
            init: function () {
                this.options.order = [5,"desc"];
                this.object = $("#data-table").DataTable(this.options);

                $("#data-table")
                    .on("click", ".btn-receive", function () {
                        var id = $(this).data("id");
                        swal({
                            title: "Confirmar recepción de acta",
                            text: "Una vez recepcionada el acta las notas no podrán ser cambiadas.",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Si",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            cancelButtonText: "Cancelar"
                        }).then(function (result) {
                            if (result.value) {
                                $.ajax({
                                    url: "/academico/recepcion-ficha/recibir".proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        id: id
                                    },
                                    success: function () {
                                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                        datatable.reports.reload();
                                    },
                                    error: function () {
                                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                    }
                                });
                            }
                        });
                    });
            },
            reload: function () {
                this.object.ajax.reload();
            }
        }
    };


    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.reports.reload();
            });
        }
    };

    var events = {
        onDownloadExcel: function () {
            $("#export_excel").on("click", function () {
                var button = $(this);
                $(button).addClass("m-loader m-loader--right m-loader--ligth").attr("disabled", true);
                var termId = $("#term_select").val();
                var status = $("#status_select").val();
                var url = `/academico/recepcion-ficha/reporte-excel?termId=${termId}&status=${status}`;
                $.fileDownload(url,
                    {
                        httpMethod: 'GET',
                        successCallback: function () {
                            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                            $(button).removeClass("m-loader m-loader--right m-loader--ligth").attr("disabled", false);
                        }
                    }
                )
            });
        },
        onDownloadPDF: function () {
            $("#export_pdf").on("click", function () {
                var button = $(this);
                var termId = $("#term_select").val();
                var status = $("#status_select").val();
                var url = `/academico/recepcion-ficha/reporte-pdf?termId=${termId}&status=${status}`;
                $(button).addClass("m-loader m-loader--right m-loader--ligth").attr("disabled", true);
                $.fileDownload(url,
                    {
                        httpMethod: 'GET',
                        successCallback: function () {
                            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                        }
                    }
                )
                    .always(function () {
                        $(button).removeClass("m-loader m-loader--right m-loader--ligth").attr("disabled", false);
                    });
            });
        },
        init: function () {
            this.onDownloadPDF();
            this.onDownloadExcel();
        }
    };

    var select2 = {
        status: {
            init: function () {
                $("#status_select").select2({
                    minimumResultsForSearch: -1
                }).on("change", function () {
                    datatable.reports.reload();
                });
            }
        },
        term: {
            init: function () {
                $.ajax({
                    url: "/periodos/get",
                    type : "GET"
                })
                    .done(function (e) {
                        console.log(e);
                        $("#term_select").select2({
                            data: e.items,
                            placeholder: "Seleccionar periodo"
                        });

                        $("#term_select").val(e.selected).trigger("change");

                        $("#term_select").on("change", function () {
                            datatable.reports.reload();
                        });
                    })
              
            }
        },
        init: function () {
            select2.status.init();
            select2.term.init();
        }
    }

    return {
        init: function () {
            select2.init();
            search.init();
            datatable.reports.init();
            events.init();
        }
    };
}();

$(function () {
    InitApp.init();
});