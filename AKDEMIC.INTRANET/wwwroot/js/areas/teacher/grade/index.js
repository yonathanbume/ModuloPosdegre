var initDatatable = function () {

    var datatable = {
        object: null,
        options: {
            data: {
                source: {
                    read: {
                        method: "GET",
                        url: null
                    }
                }
            },
            columns: [
                {
                    field: "course",
                    title: "Curso"
                },
                {
                    field: "section",
                    title: "Sección"
                },
                {
                    field: "credits",
                    title: "Cred."
                },
                {
                    field: "career",
                    title: "Escuela Profesional"
                },
                {
                    field: "term",
                    title: "Periodo"
                },
                {
                    field: "options",
                    title: "Opciones",
                    sortable: false,
                    filterable: false,
                    template: function (row) {
                        var tmp = "";
                        tmp += '<a href="/profesor/notas/detalle/';
                        tmp += row.sectionId;
                        tmp += '" class="btn btn-primary btn-sm m-btn m-btn--icon btn-notes" data-course="';
                        tmp += row.courseId;
                        tmp += '" data-section="';
                        tmp += row.sectionId;
                        tmp += '"><span><i class="la la-list"></i><span> Detalle </span></span></a> ';

                        if (AuxiliaryEvaluationReport) {
                            tmp += `<a target="_blank" title='Registro auxiliar de evaluaciones PDF' class="btn btn-primary m-btn btn-sm m-btn--icon m-btn--icon-only" href="/profesor/notas/secciones/${row.sectionId}/registro-auxiliar-evaluacion"><i class="la la-file-pdf-o"></i></a> `;
                            tmp += `<a target="_blank" title='Registro auxiliar de evaluaciones EXCEL' class="btn btn-primary m-btn btn-sm m-btn--icon m-btn--icon-only" href="/profesor/notas/secciones/${row.sectionId}/registro-auxiliar-evaluacion-excel"><i class="la la-file-excel-o"></i></a>`;
                        }
                        return tmp;
                    }
                }
            ]
        },
        load: function (term) {
            if (datatable.object !== undefined && datatable.object !== null) datatable.object.destroy();
            datatable.options.data.source.read.url = `/profesor/notas/secciones/get?term=${term}`.proto().parseURL();
            datatable.object = $(".m-datatable").mDatatable(datatable.options);
        }
    };

    var select = {
        init: function () {
            $.ajax({
                url: ("/ultimos-periodos/get?yearDifference=3").proto().parseURL()
            }).done(function (data) {
                $("#select_term").select2({
                    data: data.items
                });

                datatable.load($("#select_term").val());

                $("#select_term").on("change", function (e) {
                    datatable.load($("#select_term").val());
                });
            });
        }
    };

    return {
        // public functions
        init: function() {
            //datatable = $(".m-datatable").mDatatable(options);
            select.init();
        }
    };
}();

jQuery(document).ready(function () {
    initDatatable.init();
});