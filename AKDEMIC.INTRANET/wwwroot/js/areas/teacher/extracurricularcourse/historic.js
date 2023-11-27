var ActivitiesTable = function () {
    var id = "#datatable";
    var groupId = $("#GroupId").val();
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
                    url: (`/docente/gruposextracurriculares/section/${groupId}/assistances`).proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "date",
                title: "Fecha"
            },
            {
                field: "options",
                title: "Opciones",
                textAlign: "center",
                sortable: false,
                filterable: false,
                template: function (row) {
                    return "<button data-id='" + row.id + "' data-date='" + row.date + "' class='btn btn-brand btn-sm m-btn m-btn--icon btn-assistance'><span><i class='la la-eye'></i><span>Ver asistencia</span></span></button>";
                }
            }
        ]
    };

    var events = {
        init: function () {
            $(".btn-assistance").on("click", function () {
                var id = $(this).data("id");
                var group = $("#Group").val();
                location.href = `/docente/gruposextracurriculares/historic/assistance/${id}/${group}`.proto().parseURL();
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