var PsicologyPage = function () {
    var pid = $("#Id").val();
    var oview = $("#DoctorView").val();
    var cpdid = $("#CurrentPsychologicalDiagnostic").val();
    var datatable_consult_reason = null;
    var datatable_personal_history = null;
    var datatable_family_history = null;
    var datatable_observations = null;
    var datatable_diagnostic_impressions = null;
    var datatable_historical_diagnostic = null;
    var datatable_test_psychology = null;
    var datatable_test_psycholoy_detail = null;
    var datatable_current_test = null;
    var datatable_current_test_historical_point = null;

    var Source_consult_reason = [];
    var Source_personal_history = [];
    var Source_family_history = [];
    var Source_observations = [];
    var Source_diagnostic_impressions = [];

    var GetDatesConsultReason = function(){
        $.ajax({
            type : 'GET',
            url : `/admin/bienestar_institucional/${pid}/ficha_psicologica_motivo_consulta/get`.proto().parseURL(),
            success : function(data){                    
                      Source_consult_reason = data;                             
                      options_consult_reason.data.source = Source_consult_reason;
                      loadDatatable_consult_reason();
            }
        });
    }

    var GetDatesPersonalHistory = function(){
        $.ajax({
            type : 'GET',
            url : `/admin/bienestar_institucional/${pid}/ficha_psicologica_antecedentes_personales/get`.proto().parseURL(),
            success : function(data){                    
                      Source_personal_history = data;                             
                      options_personal_history.data.source = Source_personal_history;
                      loadDatatable_personal_history();
            }
        });
    }

    var GetDatesFamilyHistory = function(){
        $.ajax({
            type : 'GET',
            url : `/admin/bienestar_institucional/${pid}/ficha_psicologica_historia_familiar/get`.proto().parseURL(),
            success : function(data){                    
                      Source_family_history = data;                             
                      options_family_history.data.source = Source_family_history;
                      loadDatatable_family_history();
            }
        });
    }

    var GetDatesObservations = function(){
        $.ajax({
            type : 'GET',
            url : `/admin/bienestar_institucional/${pid}/ficha_psicologica_observaciones/get`.proto().parseURL(),
            success : function(data){                    
                      Source_observations = data;                             
                      options_observations.data.source = Source_observations;
                      loadDatatable_observations();
            }
        });
    }

    var GetDatesDiagnosticImpressions = function(){
        $.ajax({
            type : 'GET',
            url : `/admin/bienestar_institucional/${pid}/ficha_psicologica_impresiones_diagnosticas/get`.proto().parseURL(),
            success : function(data){                    
                      Source_diagnostic_impressions = data;                             
                      options_diagnostic_impressions.data.source = Source_diagnostic_impressions;
                      loadDatatable_diagnostic_impressions();
            }
        });
    }




var options_test_pyschology_detail =
{
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
            field: 'description',
            title: 'Descripción',
            width: 500
        },
        {
            field: "answer",
            title: "Respuesta",
            width: 150,
            sortable: false,
            filterable: false,
            template: function (row) {
                if (row.answer == true) {
                    return `<span class="m-badge  m-badge--success m-badge--wide">Si</span>`;
                } else {
                    return `<span class="m-badge  m-badge--danger m-badge--wide">No</span>`;
                }
            }
        }
    ]
};
var options_consult_reason = {
        data: {
            type: "local",
            source: Source_consult_reason
        },
        columns: [
            {
                field: 'description',
                title: 'Descripción',
                width: 700
            },
            {
                field: 'datehistorical',
                title: 'Fecha',
                width: 100
            },
            
            {
                field: "options",
                title: "Opciones",
                width: 200 ,
                template: function (row ,index) {
                    var tmp = '';                    
                    if (row.editable == true)
                    {                    
                    tmp += `<button onclick="PsicologyPage.removeRow_consult_reason(${index})" type="button" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete"<span><i class="la la-trash"> </i> </span> Eliminar </span></span></button>`;
                    return tmp;                    
                    }
                    else
                    {
                    return tmp;
                    }
                    
                }

                
            }
        ]
    }  

var options_personal_history = {
        data: {
            type: "local",
            source: Source_personal_history
        },
        columns: [
            {
                field: 'description',
                title: 'Descripción',
                width: 700
            },
            {
                field: 'datehistorical',
                title: 'Fecha',
                width: 100
            },
            
            {
                field: "options",
                title: "Opciones",
                width: 200 ,
                template: function (row ,index) {
                    var tmp = '';                    
                    if (row.editable == true)
                    {                    
                    tmp += `<button onclick="PsicologyPage.removeRow_personal_history(${index})" type="button" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete"<span><i class="la la-trash"> </i> </span> Eliminar </span></span></button>`;
                    return tmp;                    
                    }
                    else
                    {
                    return tmp;
                    }
                    
                }
                
            }
        ]
    }  

var options_family_history = {
        data: {
            type: "local",
            source: Source_family_history
        },
        columns: [
            {
                field: 'description',
                title: 'Descripción',
                width: 700
            },
            {
                field: 'datehistorical',
                title: 'Fecha',
                width: 100
            },
            
            {
                field: "options",
                title: "Opciones",
                width: 200 ,
                template: function (row ,index) {
                    var tmp = '';                    
                    if (row.editable == true)
                    {                    
                    tmp += `<button onclick="PsicologyPage.removeRow_family_history(${index})" type="button" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete"<span><i class="la la-trash"> </i> </span> Eliminar </span></span></button>`;
                    return tmp;                    
                    }
                    else
                    {
                    return tmp;
                    }
                    
                }
                
            }
        ]
    }  

var options_observations = {
        data: {
            type: "local",
            source: Source_observations
        },
        columns: [
            {
                field: 'description',
                title: 'Descripción',
                width: 700
            },
            {
                field: 'datehistorical',
                title: 'Fecha',
                width: 100
            },
            
            {
                field: "options",
                title: "Opciones",
                width: 200 ,
                template: function (row ,index) {
                    var tmp = '';                    
                    if (row.editable == true)
                    {                    
                    tmp += `<button onclick="PsicologyPage.removeRow_observation(${index})" type="button" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete"<span><i class="la la-trash"> </i> </span> Eliminar </span></span></button>`;
                    return tmp;                    
                    }
                    else
                    {
                    return tmp;
                    }
                    
                }
                
            }
        ]
    }  

var options_diagnostic_impressions = {
        data: {
            type: "local",
            source: Source_diagnostic_impressions
        },
        columns: [
            {
                field: 'description',
                title: 'Descripción',
                width: 300
            },
            {
                field: 'datehistorical',
                title: 'Fecha',
                width: 100
            },
            
            {
                field: "options",
                title: "Opciones",
                width: 200 ,
                template: function (row ,index) {
                    var tmp = '';                    
                    if (row.editable == true)
                    {                    
                    tmp += `<button onclick="PsicologyPage.removeRow_diagnostic_impression(${index})" type="button" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete"<span><i class="la la-trash"> </i> </span> Eliminar </span></span></button>`;
                    return tmp;                    
                    }
                    else
                    {
                    return tmp;
                    }
                    
                }
                
            }
        ]
    }  
var options_test_pyschology = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/doctor/horario-citas/test-psicologico/${pid}`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'name',
                title: 'Descripción',
                width: 300
            },         
            {
                field: "options",
                title: "Opciones",
                width: 250,
                sortable: false,
                filterable: false,
                template: function (row) {
                    var template = '';
                    template += `<button data-date="${row.date}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-test-detail"<span><i class="la flaticon-eye"> </i> </span> Ver Detalle</span></span></button> `;
                    template += `  <button data-date="${row.date}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-test-detail-points"<span><i class="la flaticon-eye"> </i> </span> Ver Puntuación</span></span></button> `;
                    return template;
                }
            }
        ]
    };
var options_current_test =
    {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/doctor/horario-citas/test-actual/${pid}`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'name',
                title: 'Descripción de la categoría',
                width: 600
            },
            {
                field: 'count',
                title: 'Puntuación',
                width: 100
            }
            
        ]
    };

    var options_current_test_historical_point =
    {
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
                field: 'name',
                title: 'Descripción de la categoría',
                width: 600
            },
            {
                field: 'count',
                title: 'Puntuación',
                width: 100
            }

        ]
    };


var options_historical_diagnostics = {
     data:
    {
         source:
         {
             read: {
                 type: 'GET',
                 url: `/admin/bienestar_institucional/diagnostico-historico/${pid}/get`.proto().parseURL()
             }
         }    
    },
        columns: [
            {
                field: 'description',
                title: 'Descripción',
                width: 600
            },
            {
                field: 'code',
                title: 'Código',
                width: 150
            },
            {
                field: 'datehistorical',
                title: 'Fecha',
                width: 100
            },

           
        ]
    }  


var loadDatatable_current_test = function () {
            
    if (datatable_current_test !== null) {
        datatable_current_test.destroy();
        datatable_current_test = null;
    }      
    datatable_current_test = $(".m-datatable_current_test").mDatatable(options_current_test);       
    
};




 var loadDatatable_test_detail = function (pid, date) {        
     $("#modal_test_psychologic_detail").modal('show');
        
     $("#modal_test_psychologic_detail").on("shown.bs.modal", function (e) {
         if (datatable_test_psycholoy_detail !== null) {
             datatable_test_psycholoy_detail.destroy();
             datatable_test_psycholoy_detail = null;
         }
         $("#modal_test_psychologic_detail > div > div > div h5").html("Detalle del test psicológico " + date);            
         options_test_pyschology_detail.data.source.read.url = `/doctor/horario-citas/test-psicologico-detalle?pid=${pid}&date=${date}`.proto().parseURL();            
         datatable_test_psycholoy_detail = $(".m-datatable_test_psicology_detail").mDatatable(options_test_pyschology_detail);
         datatable_test_psycholoy_detail.adjustCellsWidth();
         
     });  
    };

    var loadDatatable_test_detail_point = function (pid, date) {
        $("#modal_test_psychologic_detail_point").modal('show');

        $("#modal_test_psychologic_detail_point").on("shown.bs.modal", function (e) {
            if (datatable_current_test_historical_point !== null) {
                datatable_current_test_historical_point.destroy();
                datatable_current_test_historical_point = null;
            }
            
            options_current_test_historical_point.data.source.read.url = `/doctor/horario-citas/test-psicologico-puntuacion-historica?pid=${pid}&date=${date}`.proto().parseURL();
            datatable_current_test_historical_point = $(".m-datatable_test_psicology_detail_point").mDatatable(options_current_test_historical_point);
            datatable_current_test_historical_point.adjustCellsWidth();

        });
    };

var loadDatatable_consult_reason = function () {    
    datatable_consult_reason = $(".m-datatable_consult_reason").mDatatable(options_consult_reason);      
 }

var loadDatatable_personal_history = function () {           
    datatable_personal_history = $(".m-datatable_personal_history").mDatatable(options_personal_history);
 }

var loadDatatable_family_history = function () {       
    datatable_family_history = $(".m-datatable_family_history").mDatatable(options_family_history);
 }

var loadDatatable_observations = function () {       
    datatable_observations = $(".m-datatable_observations").mDatatable(options_observations);
 }

    var loadDatatable_diagnostic_impressions = function () {
        datatable_diagnostic_impressions = $(".m-datatable_diagnostic_impressions").mDatatable(options_diagnostic_impressions);
    };

    var loadDatatable_historical_diagnostic = function () {
        datatable_historical_diagnostic = $(".m-datatable_historical_diagnostics").mDatatable(options_historical_diagnostics);
    };

    var loadDatatable_test_pyschology = function () {
        datatable_test_psychology = $(".m-datatable_test_psicology").mDatatable(options_test_pyschology);
        datatable_test_psychology.on('click', '.btn-test-detail',
            function (e) {
                e.preventDefault();                
                var date = $(this).data("date");                
                loadDatatable_test_detail(pid, date);                                
                
            });
        datatable_test_psychology.on('click', '.btn-test-detail-points', function (e) {
            e.preventDefault();
            var datehistorical = $(this).data("date");
            loadDatatable_test_detail_point(pid, datehistorical);

        });
    };


var Modals = function (){
    $(".btn-1").on('click',function(){
        $("#modal_consult_reason").modal('show');
    });
    $(".btn-2").on('click',function(e){
        e.preventDefault();
        $("#modal_personal_history").modal('show');
    });
    $(".btn-3").on('click',function(e){
        e.preventDefault();
        $("#modal_family_history").modal('show');
    });
    $(".btn-4").on('click',function(e){
        e.preventDefault();
        $("#modal_observations").modal('show');
    });
    $(".btn-5").on('click',function(e){
        e.preventDefault();
        $("#modal_diagnostic_impressions").modal('show');
    });

    $("#IsRehabilitaded").bootstrapSwitch({
        onColor: 'primary',
        offColor: 'danger',
        onText: 'SI',
        offText: 'NO'        
    });
};


var form_consult_reason = function () {
        consult_reasion_Form = $("#form_consult_reason").validate({
            submitHandler: function () {
                var descriptionval = $("textarea[name=Description]").val();   
                var currentDate = new Date().toLocaleDateString('en-GB');
                var today = currentDate;
                   
            Source_consult_reason.push({ description: descriptionval, datehistorical : today , editable : true});
            datatable_consult_reason.originalDataSet = Source_consult_reason;
            datatable_consult_reason.reload();
                $("#modal_consult_reason").modal("hide");
                consult_reasion_Form.resetForm();
            }
        });
    };

var form_personal_history = function () {
        personal_history_Form = $("#form_personal_history").validate({
            submitHandler: function () {
                var descriptionval = $("#form_personal_history textarea[name=Description]").val();   
                var currentDate = new Date().toLocaleDateString('en-GB');
                var today = currentDate;
                   
            Source_personal_history.push({ description: descriptionval, datehistorical : today , editable : true});
            datatable_personal_history.originalDataSet = Source_personal_history;
            datatable_personal_history.reload();
                $("#modal_personal_history").modal("hide");
                personal_history_Form.resetForm();
            }
        });
    };

var form_family_history = function () {
        family_history_Form = $("#form_family_history").validate({
            submitHandler: function () {
                var descriptionval = $("#form_family_history textarea[name=Description]").val();   
                var currentDate = new Date().toLocaleDateString('en-GB');
                var today = currentDate;
                   
            Source_family_history.push({ description: descriptionval, datehistorical : today , editable : true});
            datatable_family_history.originalDataSet = Source_family_history;
            datatable_family_history.reload();
                $("#modal_family_history").modal("hide");
                family_history_Form.resetForm();
            }
        });
    };

var form_observations = function () {
        observations_Form = $("#form_observations").validate({
            submitHandler: function () {
                var descriptionval = $("#form_observations textarea[name=Description]").val();   
                var currentDate = new Date().toLocaleDateString('en-GB');
                var today = currentDate;
                   
            Source_observations.push({ description: descriptionval, datehistorical : today , editable : true});
            datatable_observations.originalDataSet = Source_observations;
            datatable_observations.reload();
                $("#modal_observations").modal("hide");
                observations_Form.resetForm();
            }
        });
    };


var form_diagnostic_impressions = function () {
        diagnostic_impressions_Form = $("#form_diagnostic_impressions").validate({
            submitHandler: function () {
                var descriptionval = $("#form_diagnostic_impressions textarea[name=Description]").val();   
                var currentDate = new Date().toLocaleDateString('en-GB');
                var today = currentDate;
                   
            Source_diagnostic_impressions.push({ description: descriptionval, datehistorical : today , editable : true});
            datatable_diagnostic_impressions.originalDataSet = Source_diagnostic_impressions;
            datatable_diagnostic_impressions.reload();
                $("#modal_diagnostic_impressions").modal("hide");
                diagnostic_impressions_Form.resetForm();
            }
        });
    };

var loadHistoricalDiagnosticsSelect = function () {
    
        $.ajax({
            url: `/diagnostics-psicologicos/get`.proto().parseURL()
        }).done(function (data) {            
            if (cpdid == _app.constants.guid.empty) {
                $("#select-historical-diagnostics").select2({  
                    
                    data: data.items,
                    placeholder: {
                        id: "-1",
                        text: " No tiene un diagnóstico actualizado",
                        selected: 'selected'
                    },

                    
                }).val(-1).trigger("change");;
            } else {
                $("#select-historical-diagnostics").select2({
                    data: data.items
                }).val(cpdid).trigger("change");
            }
            
        });       

    }

    var extensions = function () {
        $(".btn-mc").on('click', function (e) {
            e.preventDefault();
            $("#modal_consult_reason").modal('show');
        });

        $(".btn-ap").on('click', function (e) {
            e.preventDefault();
            $("#modal_personal_history").modal('show');
        });

        $(".btn-hf").on('click', function (e) {
            e.preventDefault();
            $("#modal_family_history").modal('show');
        });

        $(".btn-oc").on('click', function (e) {
            e.preventDefault();
            $("#modal_observations").modal('show');
        });

        $(".btn-di").on('click', function (e) {
            e.preventDefault();
            $("#modal_diagnostic_impressions").modal('show');
        });

        
    };


    var SaveAll = function () {
        $("#medical-form").validate({
            submitHandler: function (form, event) {
                var lst1 = [];
                var lst2 = [];
                var lst3 = [];
                var lst4 = [];
                var lst5 = [];
                $.each(Source_consult_reason, function (i, item) {
                    if (item.editable !== false)
                        lst1.push(item);
                });
                $.each(Source_personal_history, function (i, item) {
                    if (item.editable !== false)
                        lst2.push(item);
                });
                $.each(Source_family_history, function (i, item) {
                    if (item.editable !== false)
                        lst3.push(item);
                });
                $.each(Source_observations, function (i, item) {
                    if (item.editable !== false)
                        lst4.push(item);
                });
                $.each(Source_diagnostic_impressions, function (i, item) {
                    if (item.editable !== false)
                        lst5.push(item);
                });

                var list_consult_reason = lst1;
                var list_personal_history = lst2;
                var list_family_history = lst3;
                var list_observations = lst4;
                var list_diagnostic_impressions = lst5;

                var temp1 = JSON.stringify(list_consult_reason);
                var temp2 = JSON.stringify(list_personal_history);
                var temp3 = JSON.stringify(list_family_history);
                var temp4 = JSON.stringify(list_observations);
                var temp5 = JSON.stringify(list_diagnostic_impressions);


                var formtoController = $("#medical-form").serializeArray();
                formtoController.push({ name: "listmedicalconsultreason", value: list_consult_reason });
                formtoController.push({ name: "listmedicalpersonalhistory", value: list_personal_history });
                formtoController.push({ name: "listmedicalfamilyhistory", value: list_family_history });
                formtoController.push({ name: "listmedicalobservation", value: list_observations });
                formtoController.push({ name: "listmedicaldiagnosticimpression", value: list_diagnostic_impressions });
                var rehabilitaded = false;
                if ($("#IsRehabilitaded")[0].checked === true) {
                    rehabilitaded = true;
                }
                $.ajax({
                    type: 'POST',
                    url: `/admin/bienestar_institucional/GuardarTodo`.proto().parseURL(),
                    data: { listmedicalconsultreason: temp1, listmedicalpersonalhistory: temp2, listmedicalfamilyhistory: temp3, listmedicalobservation: temp4, listmedicaldiagnosticimpression: temp5, Id: $("#Id").val(), medicalappointmet: $("#MedicalAppointmet").val(), doctorview: $("#DoctorView").val(), currenthistoricaldiagnostic: $("#select-historical-diagnostics").val(), isRehabilitaded: rehabilitaded },
                    complete: function () {
                        if (oview === "false") {
                            location.href = `/admin/bienestar_institucional`.proto().parseURL();
                        }
                        else {
                            location.href = `/doctor/horario-citas`.proto().parseURL();
                        }

                    }
                });
            }
        });

    };

    return {
        load: function () {
            GetDatesConsultReason();           
            GetDatesPersonalHistory();
            GetDatesFamilyHistory();
            GetDatesObservations();
            GetDatesDiagnosticImpressions();
            Modals();
            form_consult_reason();     
            form_personal_history();       
            form_family_history();
            form_observations();
            form_diagnostic_impressions();
            loadDatatable_historical_diagnostic();
            loadHistoricalDiagnosticsSelect();
            loadDatatable_test_pyschology();   
            loadDatatable_current_test();
            extensions();
            SaveAll();
        },
        removeRow_consult_reason: function (id) {
            Source_consult_reason.splice(id, 1);
            datatable_consult_reason.originalDataSet = Source_consult_reason;
            datatable_consult_reason.reload();
        },
        removeRow_personal_history: function (id) {
            Source_personal_history.splice(id, 1);
            datatable_personal_history.originalDataSet = Source_personal_history;
            datatable_personal_history.reload();
        },
        removeRow_family_history: function (id) {
            Source_family_history.splice(id, 1);
            datatable_family_history.originalDataSet = Source_family_history;
            datatable_family_history.reload();
        },
        removeRow_observation: function (id) {
            Source_observations.splice(id, 1);
            datatable_observations.originalDataSet = Source_observations;
            datatable_observations.reload();
        },
        removeRow_diagnostic_impression: function (id) {
            Source_diagnostic_impressions.splice(id, 1);
            datatable_diagnostic_impressions.originalDataSet = Source_diagnostic_impressions;
            datatable_diagnostic_impressions.reload();
        }
    }
}();

$(function () {

    PsicologyPage.load();

});

























