var Grades = function () {

    var report = {
        studentInformation: {
            download: function () {
                $("#btnStudentInformationConstancy").addLoader();
                $.fileDownload(`/alumno/notas/constancia-fichasocioeconomica`.proto().parseURL(), {
                    httpMethod: "GET",
                }).done(function () {
                    $("#btnStudentInformationConstancy").removeLoader();
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                }).fail(function (e) {
                    $("#btnStudentInformationConstancy").removeLoader();
                    var responseText = e;
                    if (responseText != "undefined") {
                        var segunda = responseText.substr(responseText.indexOf('>') + 1, responseText.length);
                        var text = segunda.substr(0, segunda.length - 6);
                        toastr.error(text, _app.constants.toastr.title.error);
                    } else {
                        toastr.error(e_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }

                });
            },
            init: function () {
                $("#btnStudentInformationConstancy").on("click", function () {
                    report.studentInformation.download();
                });
            }
        },
        init: function () {
            this.studentInformation.init();
        }
    };

    var init = function () {
        $('#select-term').on('change', function (e) {
            $("#lblAcademicYear").text('-');
            $("#lblSectionCount").text('-');
            $("#lblCreditSum").text('-');

            load();
        });
        if ($('#select-term').val()) {
            load();
        }
    }

    var load = function () {
        mApp.block('.m-portlet', { type: 'loader', message: 'Cargando...' });
        $.ajax({
            type: "GET",
            url: ('/alumno/notas/periodo/' + $('#select-term').val() + '/get').proto().parseURL()
        }).done(function (data) {
            $('#enrolled-courses-container').html(data);
            $("[data-toggle='m-tooltip']").tooltip();
            $("[rel='m-tooltip']").tooltip();
            $("#lblAcademicYear").text($('#summaryAcademicYear').val());
            $("#lblSectionCount").text($('#summarySectionCount').val());
            $("#lblCreditSum").text(parseFloat($('#summaryCreditSum').val()).toFixed(1));
            mApp.unblock('.m-portlet');
        });
    }

    var events = {
        onDownloadSyllabus: function () {
            $("#enrolled-courses-container").on("click", ".btn_download_syllabus", function () {
                var studentSectionid = $(this).data("studentsectionid");
                var syllabusTeacherId = $(this).data("syllabusteacherid");
                var $btn = $(this);

                var url = `/alumno/notas/descargar-silabo?syllabusTeacherId=${syllabusTeacherId}&studentSectionId=${studentSectionid}`;

                $btn.addLoader();

                $.fileDownload(url)
                    .always(function () {
                        $btn.removeLoader();
                    }).done(function () {
                        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                    }).fail(function () {
                        toastr.error("No se pudo descargar el archivo", "Error");
                    });
            })
        },
        onDownloadEnrollmentReport: function () {
            $(".download-report").on("click", function () {

                if ($('#select-term').val()) {
                    var $btn = $(this);
                    $btn.addLoader();

                    var url = `/alumno/notas/ficha-matricula/${$('#select-term').val()}`;

                    $.fileDownload(url)
                        .done(function () {
                            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                        })
                        .fail(function () {
                            toastr.error("No se pudo descargar el archivo", "Error");
                        })
                        .always(function () {
                            $btn.removeLoader();
                        });
                }
                else
                    toastr.error("Debe seleccionar un periodo matrículado", "Error");
            })
        },
        init: function () {
            this.onDownloadSyllabus();
            this.onDownloadEnrollmentReport();
        }
    }

    return {
        init: function () {
            init();
            events.init();
            report.init();
        }
    }
}();

$(function () {
    Grades.init();
});
