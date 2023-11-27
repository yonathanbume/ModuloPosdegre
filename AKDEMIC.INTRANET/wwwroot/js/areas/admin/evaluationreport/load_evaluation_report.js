var index = function () {

    var datatable = {
        evaluation_report: {
            data: [],
            object: null,
            options: {
                lengthMenu: [10, 25, 50],
                orderMulti: false,
                pagingType: "full_numbers",
                processing: true,
                responsive: true,
                serverSide: false,
                data: [],
                rowCallback: function (row, data, index) {

                    if (!data.Valid) {
                        $(row).closest("tr").css("background-color", "rgb(255 90 90 / 46%)");
                    } else {
                        $(row).closest("tr").css("background-color", "");

                    }
                },
                columns: [
                    {
                        title: "Periodo",
                        data: "Term"
                    },
                    {
                        title: "Código",
                        data: "Code"
                    },
                    {
                        title: "Fec. Recepción",
                        data: "ReceptionDate"
                    },
                    {
                        title: "Observaciones",
                        data: "Observations"
                    }
                ]
            },
            init: function () {
                datatable.evaluation_report.object = $("#data-table").DataTable(datatable.evaluation_report.options);
            },
        },
        init: function () {
            datatable.evaluation_report.init();
        }
    }

    var events = {
        onLoadFile: function () {
            $("#btn_load_file").on("click", function () {
                var $btn = $(this);

                var files = $("#input_file").prop("files");
                if (files.length < 1) {
                    toastr.info("Debe ingresar un archivo.", "Información");
                    return;
                }

                var formData = new FormData();
                formData.append('file', files[0]);

                mApp.block("#data-table_wrapper", {
                    message: "Cargando actas..."
                });

                $btn.addLoader();

                datatable.evaluation_report.data = [];
                datatable.evaluation_report.object.rows().remove().draw(false);

                $.ajax({
                    url: `/admin/generacion-actas/cargar-actas-historicas/cargar-excel`,
                    contentType: false,
                    processData: false,
                    type: "POST",
                    data: formData
                })
                    .done(function (data) {

                        for (var i = 0; i < data.length; i++) {
                            var evaluation_report = {
                                Term: data[i].term,
                                TermId: data[i].termId,
                                Code: data[i].code,
                                ReceptionDate: data[i].receptionDate,
                                Valid: data[i].valid,
                                Observations: data[i].observations,
                                EvaluationReportId: data[i].evaluationReportId,
                                New: data[i].new
                            };

                            datatable.evaluation_report.data.push(evaluation_report);
                            datatable.evaluation_report.object.clear().rows.add(datatable.evaluation_report.data).draw();
                            datatable.evaluation_report.object.columns.adjust().draw();
                        }
                    })
                    .fail(function (e) {

                    })
                    .always(function () {
                        $btn.removeLoader();
                        mApp.unblock("#data-table_wrapper");
                    })
            })
        },
        onProccessEvaluationReports: function () {
            $("#btn_proccess").on("click", function () {
                var $btn = $(this);

                if (datatable.evaluation_report.data.length < 1) {
                    swal({
                        type: "info",
                        title: "Información",
                        text: "No se encontraron registros.",
                        confirmButtonText: "Ok",
                    })
                }
                else {

                    var anyInvalid = datatable.evaluation_report.data.some(x => x.Valid == false);

                    var formData = new FormData();

                    for (var i = 0; i < datatable.evaluation_report.data.length; i++) {
                        formData.append(`EvaluationReports[${i}].EvaluationReportId`, datatable.evaluation_report.data[i].EvaluationReportId);
                        formData.append(`EvaluationReports[${i}].Code`, datatable.evaluation_report.data[i].Code);
                        formData.append(`EvaluationReports[${i}].Term`, datatable.evaluation_report.data[i].Term);
                        formData.append(`EvaluationReports[${i}].TermId`, datatable.evaluation_report.data[i].TermId);
                        formData.append(`EvaluationReports[${i}].ReceptionDate`, datatable.evaluation_report.data[i].ReceptionDate);
                        formData.append(`EvaluationReports[${i}].Observations`, datatable.evaluation_report.data[i].Observations);
                        formData.append(`EvaluationReports[${i}].Valid`, datatable.evaluation_report.data[i].Valid);
                        formData.append(`EvaluationReports[${i}].New`, datatable.evaluation_report.data[i].New);
                    }

                    if (anyInvalid) {
                        swal({
                            title: "¿Seguro que desea continuar?",
                            text: "Se encontraron actas que no han sido procesadas correctamente",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Sí, continuar",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            cancelButtonText: "Cancelar",
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise((resolve) => {
                                    $btn.addLoader();

                                    $.ajax({
                                        url: `/admin/generacion-actas/cargar-actas-historicas`,
                                        type: "POST",
                                        contentType: false,
                                        processData: false,
                                        data: formData
                                    })
                                        .done(function (e) {
                                            swal({
                                                type: "success",
                                                title: "Actas cargadas!",
                                                text: e,
                                                confirmButtonText: "Ok",
                                                allowOutsideClick: false
                                            })
                                            datatable.evaluation_report.data = [];
                                            datatable.evaluation_report.object.rows().remove().draw(false);
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
                                        .always(function (e) {
                                            $btn.removeLoader();
                                        })
                                });
                            }
                        });
                    } else {

                        $btn.addLoader();

                        $.ajax({
                            url: `/admin/generacion-actas/cargar-actas-historicas`,
                            type: "POST",
                            contentType: false,
                            processData: false,
                            data: formData
                        })
                            .done(function (e) {
                                swal({
                                    type: "success",
                                    title: "Actas cargadas!",
                                    text: e,
                                    confirmButtonText: "Ok",
                                    allowOutsideClick: false
                                })
                                datatable.evaluation_report.data = [];
                                datatable.evaluation_report.object.rows().remove().draw(false);
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
                            .always(function (e) {
                                $btn.removeLoader();
                            })
                    }


             
                }
            })
        },
        init: function () {
            this.onLoadFile();
            this.onProccessEvaluationReports();
        }
    }

    return {
        init: function () {
            events.init();
            datatable.init();
        }
    }
}();

$(() => {
    index.init();
})