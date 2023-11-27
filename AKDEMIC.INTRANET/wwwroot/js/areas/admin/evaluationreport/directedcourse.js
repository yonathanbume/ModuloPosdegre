var directedCourse = function () {

    var datatable = {
        object: null,
        options: getSimpleDataTableConfiguration({
            url: "/admin/generacion-actas/get-dirigidos".proto().parseURL(),
            data: function (data) {
                delete data.columns;
                data.teacherId = $("#teacher_select").val();
            },
            pageLength: 50,
            orderable: [],
            columns: [
                {
                    data: "course",
                    title: "Curso"
                },
                {
                    data: "teacher",
                    title: "Profesor"
                },
                {
                    data: "printQuantity",
                    title: "Impresiones Realizadas"
                },
                {
                    data: "status",
                    title: "Estado",
                    render: function (data) {
                        var status = {
                            "Recibido": { "title": "Recibido", "class": " m-badge--success" },
                            "Generado": { "title": "Generado", "class": " m-badge--warning" },
                            "Pendiente": { "title": "Pendiente", "class": " m-badge--metal" }
                        };
                        return '<span class="m-badge ' + status[data].class + ' m-badge--wide">' + status[data].title + "</span>";
                    }
                },
                {
                    data: null,
                    title: "Opciones",
                    orderable: false,
                    render: function (data) {
                        var url = `/admin/generacion-actas/acta-final-curso-dirigido/${data.courseId}/${data.teacherId}`.proto().parseURL();

                        if (!data.wasGenerated) {
                            return `<button data-url=${url} class="btn btn-primary btn-sm m-btn m-btn--icon btn-report" download><span><i class="la la-file-text-o"></i><span> Generar </span></span></button>`;
                        } else {
                            return `<button data-url=${url} class="btn btn-brand btn-sm m-btn m-btn--icon btn-report" download><span><i class="la la-file-text-o"></i><span> Imprimir </span></span></button>`;
                        }

                    }
                }
            ]
        }),
        reload: function () {
            this.object.ajax.reload();
        },
        events: {
            onReport: function(){
                $("#data-table")
                    .on("click", ".btn-report", function () {
                        var url = $(this).data("url");
                        var $btn = $(this);
                        $btn.addLoader();
                        $.fileDownload(url, {
                            httpMethod: 'GET', successCallback: function () {
                                $btn.removeLoader();
                                toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                datatable.reload();
                            },
                            failCallback: function () {
                                $btn.removeLoader();
                                toastr.error("Error al generar el archivo", "Error!");
                                datatable.reload();
                            }
                        });
                    });
            },
            init: function () {
                this.onReport();
            }
        },
        init: function () {
            datatable.object = $("#data-table").DataTable(datatable.options);
            this.events.init();
        }
    };

    var select2 = {
        teachers: {
            events: {
                onChange: function () {
                    $("#teacher_select").on("change", function () {
                        datatable.reload();
                    });
                },
                init: function () {
                    this.onChange();
                }
            },
            init: function () {
                $("#teacher_select").select2({
                    ajax: {
                        delay: 300,
                        url: (`/profesores/get/v2`).proto().parseURL(),
                    },
                    allowClear: true,
                    minimumInputLength: 2,
                    placeholder: "Seleccione profesor"
                });
                this.events.init();
            }
        },
        init: function () {
            this.teachers.init();
        }
    };

    return {
        init: function () {
            select2.init();
            datatable.init();
        }
    };
}();

$(function () {
    directedCourse.init();
});