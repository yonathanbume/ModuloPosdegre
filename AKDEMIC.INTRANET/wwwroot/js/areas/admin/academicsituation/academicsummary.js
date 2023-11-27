﻿var Summary = function () {

    var options = {
        data: {
            type: "remote",
            source: {
                read: {
                    method: "GET",
                    url: ""
                }
            },
            pageSize: 10,
            saveState: {
                cookie: true,
                webstorage: true
            }
        },
        pagination: false,
        columns: [
            {
                field: "course",
                title: "Curso",
                width: 200,
                sortable: false
            },
            {
                field: "academicYear",
                title: "Ciclo",
                textAlign: "center",
                width: 80,
                sortable: false
            },
            {
                field: "credits",
                title: "Créditos",
                textAlign: "center",
                width: 80,
                sortable: false
            },
            {
                field: "finalGrade",
                title: "Promedio Final",
                textAlign: "center",
                width: 120,
                sortable: false
            },
            {
                field: "try",
                title: "N° de Veces",
                textAlign: "center",
                width: 120,
                sortable: false
            }
        ]
    }

    var load = {
        all: function () {
            $(".summary-detail-datatable").each(function (index, datatable) {
                var pid = $(datatable).data("pid");
                var studentId = location.pathname.match(/(.{0,8}-.{0,4}-.{0,4}-.{0,4}-.{0,12})/).pop();
                options.data.source.read.url = `/alumnos/${studentId}/historial/periodo/${pid}/get`.proto().parseURL();
                datatable.id = `summary-detail-${index}`;
                $(`#${datatable.id}`).mDatatable(options);
            });

            $(".btn-export").on("click",
                function () {
                    var $btn = $(this);
                    $btn.addLoader();
                    $.fileDownload(`${location.pathname}/reporte`.proto().parseURL())
                        .always(function() {
                            $btn.removeLoader();
                        }).done(function() {
                            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                        }).fail(function() {
                            toastr.error("No se pudo descargar el archivo", "Error");
                        });
                    return false;
                });
        }
    }

    var init = function () {
        load.all();
    }

    return {
        init: function () {
            init();
        }
    }
}();

$(function () {
    Summary.init();
});