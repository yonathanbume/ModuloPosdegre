var SimulationSubjectTable = function () {
    var id = "#examsubject-datatable";
    var datatable;
    var number = 0;
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ("/welfare/psicologia_exam/list").proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "options",
                title: "#",
                width: 200,
                template: function (row) {
                    number++;
                    return number;
                }
            },
            {
                field: "question",
                title: "Pregunta",
                width: 200
            },
            {
                field: "state",
                title: "Respuesta",
                sortable: false,
                filterable: false,
                template: function (row, index) {
                    var statetrue = (row.state) ? "checked" : "";
                    var statefalse = (!row.state) ? "checked" : "";
                    var template = "";
                    template += "<input name='model.ListAnswer[" + index + "].PsychologyTestQuestionId' type='hidden' value='"+row.id+"'><div class='m-radio-inline'>" +
                        "<label class='m-checkbox' >" +
                        "<input type='radio' name='model.ListAnswer[" + index + "].Answer' value='1' " + statetrue + "> Sí <span></span>" +
                        "</label>" +
                        "<label class='m-checkbox'>" +
                        "<input type='radio' name='model.ListAnswer[" + index + "].Answer' value='0' " + statefalse + "> No <span></span>" +
                        "</label>" +
                        "</div>";

                    return template;
                }
            }
        ]
    };
    var events = function () {
        $("#add-form").on('submit', function(e){
            e.preventDefault();
            var form = $(this).serializeArray();
            var id = $("#medicalappointmentid").val();
            var studentid = $("#studentid").val();
            form.push({ name: "model.psychologycalrecordid", value: $("#psychologycalrecordid").val() });
           // form.push({ name: "model.medicalappointmentid", value: $("#medicalappointmentid").val() });
           // form.push({ name: "model.studentid", value: $("#studentid").val() });

            $.ajax({
                type : 'POST',
                url: `/welfare/psicologia_exam/crear/post`.proto().parseURL(),
                data: form,
                success: function () {
                    mApp.block(".m-portlet", { type: "loader", message: "Cargando..." });    
                    location.href = `/doctor/horario-citas/detalle-historico/${id}/${studentid}`.proto().parseURL();                    
                }
            });
        });
    }

    return {
        init: function () {
            datatable = $(id).mDatatable(options);
            events();
        },
        reload: function () {
            datatable.reload();

        }
    }
}();

$(function () {
    SimulationSubjectTable.init();
});