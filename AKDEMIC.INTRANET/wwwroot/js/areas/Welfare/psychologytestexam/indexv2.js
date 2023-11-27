var psycologyTestv2 = function () {
    
    
    var datatable;
    var vrt=true;
    var private = {
        objects: {}
    };
    var arr = [];
    var number = 0;

    var options = getDataTableConfigurationPortal({
        url: `/welfare/psicologia_exam/list`,
        columns: [
            {
                title: "#",
                render: function (data , type , row) {
                    number++;
                    return number;
                }
            },
            {
                data: "question",
                name: "question",
                title: "Pregunta"
            },
            {
                data: "state",
                name: "state",
                title: "Respuesta",
                render: function (data, type, row , index) {
                    if (vrt) {
                        console.log(data);
                        console.log(type);
                        console.log(row);                        
                        console.log(index);
                        vrt = false;
                    }
                    var statetrue = (row.state) ? "checked" : "";
                    var statefalse = (!row.state) ? "checked" : "";
                    var template = "";
                    template += "<input name='model.ListAnswer[" + index.row + "].PsychologyTestQuestionId' type='hidden' value='" + row.id + "'><div class='m-radio-inline'>" +
                        "<label class='m-checkbox' >" +
                        "<input type='radio' name='model.ListAnswer[" + index.row + "].Answer' value='1' " + statetrue + "> Sí <span></span>" +
                        "</label>" +
                        "<label class='m-checkbox'>" +
                        "<input type='radio' name='model.ListAnswer[" + index.row + "].Answer' value='0' " + statefalse + "> No <span></span>" +
                        "</label>" +
                        "</div>";

                    return template;
                }
            },
            
        ]
    });

    var events = function () {
        $("#add-form").on('submit', function (e) {
            e.preventDefault();
            var form = $(this).serializeArray();   
            var id = $("#medicalappointmentid").val();
            var studentid = $("#studentid").val();
            form.push({ name: "model.psychologycalrecordid", value: $("#psychologycalrecordid").val() });            

            $.ajax({
                type: 'POST',
                url: `/welfare/psicologia_exam/crear/post`.proto().parseURL(),
                data: form,
                success: function () {
                    mApp.block(".m-portlet", { type: "loader", message: "Cargando..." });
                    location.href = `/doctor/horario-citas/detalle-historico/${id}/${studentid}`.proto().parseURL();
                }
            });
        });
    }


    var load = {
        init: function () {
            datatable = $("#examsubject-datatable").DataTable(options);
            events();
        }
    }




    return {
        init: function () {
            load.init();

        }
    }


}();

$(function () {
    psycologyTestv2.init();
});