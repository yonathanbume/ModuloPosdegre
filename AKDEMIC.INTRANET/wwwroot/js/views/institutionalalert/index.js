var InstitutionalAlertsTable = function () {
    var datatable;
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ("/alertaInstitucional/getInstitutionalAlerts").proto().parseURL()
                }
            }
        },
        columns: [ 
            {
                field: "applicant",
                title: "Solicitante"
            },
            {
                field: "dependency",
                title: "Dependencia"
            }, 
            {
                field: "register",
                title: "Fecha Registro"
            },
            {  
                field: "state",
                title: "Estado", 
                template: function (row) {   
                  if (row.status === "Activo") { 
                        return '<span class="m-badge m-badge--success m-badge--wide"> Activo </span>';
                    }
                    else if(row.status== "Inactivo"){
                       
                        return '<span class="m-badge m-badge--metal m-badge--wide"> Inactivo </span>';
                    }
                  
                }
            }, 
            {
                field: "options",
                title: "Opciones",
                sortable: false,
                filterable: false,
                template: function (row) {
                    return '<a href="' + ("/alertaInstitucional/responder/" + row.id).proto().parseURL() + '" class="btn btn-secondary btn-sm m-btn m-btn--icon"><span><i class="la la-edit"></i><span> Responder </span></span></a>'; 
                }
            }
        ]
    }; 
 
    return {
        init: function () {
            datatable = $(".m-datatable").mDatatable(options); 
             
        }
    }
}();


jQuery(document).ready(function () {
    InstitutionalAlertsTable.init(); 
});