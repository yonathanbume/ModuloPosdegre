var index = function () {
    var datatable = {
        object: null,
        options: getSimpleDataTableConfiguration({
            url: "/registrosacademicos/reporte-ingresantes/get",
            data: function (data) {
                data.careerId = $("#select2-careers").val();
                data.admissionTypeId = $("#select2-addmision-type").val();
                data.applicationTermId = $("#select2-term").val();
            },
            pageLength: 50,
            columns: [
                {
                    title: "Código",
                    data: "code"
                },
                {
                    title: "Nombre completo",
                    data: "name"
                },
                {
                    title: "Documento",
                    data: "document"
                },
                {
                    title: "Escuela Profesional",
                    data: "career"
                },
                {
                    title : "Facultad",
                    data : "faculty"
                },
                {
                    title: "Modalidad",
                    data: "admissionType"
                },
            ]
        }),
        reload: function () {
            datatable.object.ajax.reload();
        },  
        init: function () {
            datatable.object = $("#report").DataTable(datatable.options);
        }
    };

    var events = {
        onDownloadExcel: function () {
            $("#download_excel").on("click", function () {
                var url = `/registrosacademicos/reporte-ingresantes/reporte-excel?careerId=${$("#select2-careers").val()}` +
                    `&admissionTypeId=${$("#select2-addmision-type").val()}&applicationTermId=${$("#select2-term").val()}`;
                window.open(url, "_blank");
                //var $btn = $(this);
                //$btn.addLoader();
                //$.fileDownload(url, {
                //    httpMethod: 'GET', successCallback: function () {
                //        $btn.removeLoader();
                //        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                //    }
                //});
            });
        },
        init: function () {
            this.onDownloadExcel();
        }
    }

    var select2 = {
        career: {
            load: function () {
                $.ajax({
                    url: "/carreras/registroacademico/get"
                })
                    .done(function (e) {
                        $("#select2-careers").select2({
                            placeholder: "Seleccionar carrera",
                            data: e
                        }).on("change", function () {
                            datatable.reload();
                        });
                    });
            },
            init: function () {
                this.load();
            }
        },
        admissionType: {
            load: function () {
                $.ajax({
                    url: "/admissionTypes/get"
                })
                    .done(function (e) {
                        $("#select2-addmision-type").select2({
                            placeholder: "Seleccionar modalidad",
                            data: e.items
                        }).on("change", function () {
                            datatable.reload();
                        });
                    });
            },
            init: function () {
                this.load();
            }
        },
        term: {
            load: function () {
                $.ajax({
                    url: "/periodos-finalizados/get"
                })
                    .done(function (e) {
                        $("#select2-term").select2({
                            placeholder: "Seleccionar periodo",
                            data: e.items
                        }).on("change", function () {
                            datatable.reload();
                        });

                        datatable.init();
                    });
            },
            init: function () {
                this.load();
            }
        },
        init: function () {
            this.career.init();
            this.term.init();
            this.admissionType.init();
        }
    };

    return {
        init: function () {
            select2.init();
            events.init();
        }
    };
}();

$(function () {
    index.init();
});

