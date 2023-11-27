var reportArms = function () {

    var datatable = null;

    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ""
                }
            }
        },
        columns: [
            {
                field: 'username',
                title: 'Código',
                width: 100
            },
            {
                field: 'fullname',
                title: 'Nombres Completos',
                width: 200
            },            
            {
                field: 'email',
                title: 'Email',
                width: 180
            },
            {
                field: 'facultyname',
                title: 'Escuela Profesional',
                width: 150
            },           
            {
                field: 'sex',
                title: 'Sexo',
                template: function (row) {
                    if (row.sex == "Masculino") {
                        return `<span class="m-badge  m-badge--success m-badge--wide">Masculino</span>`
                    } else {
                        return `<span class="m-badge  m-badge--primary m-badge--wide">Femenino</span>`
                    }

                }
            }
        ]
    }

    var loadDatatable = function (n_minimo,n_maximo) {    
        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        options.data.source.read.url = `/welfare/reporte_circunferencia_brazo/detalle/${n_minimo}/${n_maximo}`.proto().parseURL();
        datatable = $(".m-datatable").mDatatable(options);

    }
    

    var validate = function(){
        $("#form_arms").validate({
                rules : {
                    min : { required: true },
                    max : { required: true }
                },                 
                submitHandler: function (form, event) {
                    event.preventDefault();
                    var n_minimo = $("#min").val();
                    var n_maximo = $("#max").val();
                    $.ajax({
                        type : 'GET',
                        url : `/welfare/reporte_circunferencia_brazo/${n_minimo}/${n_maximo}`.proto().parseURL(),
                        success : function (data){
                                $("#val_m").html(data.man);
                                $("#val_f").html(data.woman);
                        }
                    });
                    loadDatatable(n_minimo, n_maximo);
                }

            });
    }

    return {
        load: function () {
                                 
            loadDatatable(-1, -1);
            validate(); 
        }
    }
}();

$(function () {
    reportArms.load();
})