var ActivitiesTable = function () {
    var id = "#datatable";
    var assistanceGroupId = $("#AssistanceGroupId").val();
    var datatable;
    var datatableDetailEvaluation;
    var datatableDetailSection;

    var options = {
        pagination: false,
        search: {
            input: $("#search")
        },
        data: {
            source: {
                read: {
                    method: "GET",
                    url: (`/docente/gruposextracurriculares/section/${assistanceGroupId}/assistances/students`).proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "student",
                title: "Estudiante"
            },
            {
                field: "assistance",
                title: "Asistencia",
                textAlign: "center",
                sortable: false,
                filterable: false,
                template: function (row) {
                    if (row.state)
                        return "<span class='m-badge m-badge--success m-badge--wide'>Asistió</span>";
                    else
                        return "<span class='m-badge m-badge--danger m-badge--wide'>No asistió</span>";
                }
            }
        ]
    };

    var loadDatatable = function () {
        if (datatable !== undefined)
            datatable.destroy();
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