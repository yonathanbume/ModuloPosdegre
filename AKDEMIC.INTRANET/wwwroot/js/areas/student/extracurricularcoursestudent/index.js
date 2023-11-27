var ActivitiesTable = function () {
    var id = "#courses-datatable";
    var datatable;
    var datatableDetailEvaluation;
    var datatableDetailSection;

    var options = {
        search: {
            input: $("#search")
        },
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ("/alumno/mis-cursos-extracurriculares/get").proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "course",
                title: "Curso"
            },
            {
                field: "group",
                title: "Grupo"
            },
            {
                field: "state",
                title: "Estado"
            },
            {
                field: "score",
                title: "Nota"
            }
        ]
    };

    var loadDatatable = function () {
        if (datatable !== undefined)
            datatable.destroy();
        options.data.source.read.url = ("/alumno/mis-cursos-extracurriculares/get").proto().parseURL();
        datatable = $(id).mDatatable(options);
    };
    return {
        init: function () {
            loadDatatable();
        },
        reload: function () {
            datatable.reload();
        }
    }
}();

$(function () {
    ActivitiesTable.init();
});