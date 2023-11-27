var reportBodyFatPercentaje = function () {
    
    var datatable = null;
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/welfare/reporte_porcentaje_grasa_corporal/estudiantes-porcentaje-grasa`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "userName",
                title: "Código",
                width: 200
            },
            {
                field: "fullname",
                title: "Nombres Completos",
                width: 200
            },
            {
                field: "email",
                title: "Email",
                width: 200
            },
            {
                field: "faculty",
                title: "Escuela Profesional",
                width: 200
            },
            {
                field: "categoryIMC",
                title: "Clasificación IMC",
                width: 200
            }
        ]
    };

    var loadDatatable = function () {

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        datatable = $(".m-datatable").mDatatable(options);
    };


    return {
        load: function () {            
            loadDatatable();
        }
    };
}();

$(function () {
    reportBodyFatPercentaje.load();
})