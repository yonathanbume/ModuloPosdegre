var ClassReschedulingRequest = function () {
    var datatable;
    var options = {
        data: {
            type: "remote",
            source: {
                read: {
                    method: "GET",
                    url: ("/profesor/reprogramacion-clase/get").proto().parseURL(),
                }
            }
        },
        columns: [
            {
                field: "course",
                title: "Curso",
                width: 200,
                template: "{{course}} (Sección {{section}})"
            },
            {
                field: "classDate",
                title: "Clase",
                width: 180,
                template: "{{classDate}} De {{starTime}} a {{endTime}}"
            },
            {
                field: "newDate",
                title: "Nuevo Horario",
                width: 180,
                template: "{{date}} De {{newStartTime}} a {{newEndTime}}"
            },
            {
                field: "registerDate",
                title: "Fecha de la Solicitud",
                textAlign: "center",
                width: 180
            },
            {
                field: "approved",
                title: "Estado",
                textAlign: "center",
                width: 150,
                template: function (row) {
                    var tmp = "";
                    var status = {
                        3: { text: _app.constants.request.inProcess.text, value: _app.constants.request.inProcess.value, class: 'm-badge--metal' },
                        4: { text: _app.constants.request.approved.text, value: _app.constants.request.approved.value, class: 'm-badge--success' },
                        5: { text: _app.constants.request.disapproved.text, value: _app.constants.request.disapproved.value, class: 'm-badge--danger' }
                    };
                    return '<span class="m-badge ' + status[row.status].class + ' m-badge--wide">' + status[row.status].text + '</span>';
                }
            },
            {
                field: "options",
                title: "Opciones",
                width: 180,
                template: function (row) {
                    var tmp = "";
                    tmp += "<button class='btn btn-default btn-sm m-btn--icon btn-detail' data-id='" + row.id + "'><span><i class='la la-eye'></i><span>Ver Detalle</span></span></button> ";
                    if (row.canDelete) {
                        tmp += "<button class='btn btn-danger btn-sm m-btn--icon btn-delete' data-id='" + row.id + "'><i class='la la-trash'></i></button>";
                    }
                    return tmp;
                }
            }
        ]
    }

    return {
        init: function () {            
            datatable = $(".m-datatable").mDatatable(options);
        }
    }
}();

$(function () {
    ClassReschedulingRequest.init();
});