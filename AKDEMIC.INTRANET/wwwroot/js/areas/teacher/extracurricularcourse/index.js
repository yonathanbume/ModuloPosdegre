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
                    url: ("/docente/gruposextracurriculares/get").proto().parseURL()
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
                field: "options",
                title: "Opciones",
                textAlign: "center",
                width: 376,
                sortable: false,
                filterable: false,
                template: function (row) {
                    var template = "";
                    template += "<button data-id='" + row.id + "' data-name='" + row.course + " - " + row.group + "' class='btn btn-brand btn-sm m-btn m-btn--icon btn-assistance'><span><i class='la la-edit'></i><span>Asistencia</span></span></button> ";
                    template += "<button data-id='" + row.id + "' data-name='" + row.course + " - " + row.group + "' class='btn btn-accent btn-sm m-btn m-btn--icon btn-qualification'><span><i class='la la-edit'></i><span>Calificación</span></span></button> ";
                    template += "<button data-id='" + row.id + "' data-name='" + row.course + " - " + row.group + "' class='btn btn-dark btn-sm m-btn m-btn--icon btn-historic'><span><i class='la la-edit'></i><span>Historial Asistencia</span></span></button> ";
                    return template;
                }
            }
        ]
    };

    var events = {
        init: function () {
            $(".btn-assistance").on("click", function () {
                var id = $(this).data("id");
                var name = $(this).data("name");
                location.href = `/docente/gruposextracurriculares/assistance/${id}/${name}`.proto().parseURL();
            });
            $(".btn-qualification").on("click", function () {
                var id = $(this).data("id");
                var name = $(this).data("name");
                location.href = `/docente/gruposextracurriculares/qualification/${id}/${name}`.proto().parseURL();
            });
            $(".btn-historic").on("click", function () {
                var id = $(this).data("id");
                var name = $(this).data("name");
                location.href = `/docente/gruposextracurriculares/historic/${id}/${name}`.proto().parseURL();
            });
        }
    };

    var loadDatatable = function () {
        if (datatable !== undefined)
            datatable.destroy();
        datatable = $(id).mDatatable(options);
        $(datatable).on("m-datatable--on-layout-updated", function () {
            events.init();
        });
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