var reportTutorial = function () {    
    var datatable_done = null;

    var options_done = {        
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ``                
                }
            }
        },
        columns: [
            {
                field: 'teacher',
                title: 'Profesor',
                width: 200
            },
            {
                field: 'classroom',
                title: 'Salón',
                width: 90
            },
            {
                field: 'section',
                title: 'Sección',
                width: 70
            },
            {
                field: 'date',
                title: 'Fecha',
                width: 70
            },
            {
                field: 'startTime',
                title: 'Hora Inicio',
                width: 90
            },
            {
                field: 'endtime',
                title: 'Hora fin',
                width: 90
            },
            {
                field: "options",
                title: "Opciones",
                width: 150,
                template: function (row) {
                    return `<button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span>Ver detalle </span></span></button>`;
                }
            }
        ]
    }  
    var loadDatatable_done = function (startDate,endDate) {

        if (datatable_done !== null) {
            datatable_done.destroy();
            datatable_done = null;
        }        
        
        options_done.data.source.read.url = `/admin/reporte_tutoria/tutorias_efectuadas/get?startDate=${startDate}&endDate=${endDate}`.proto().parseURL();        
        datatable_done = $(".m-datatable_tutorial_done").mDatatable(options_done);
        datatable_done.on('click', '.btn-detail', function () {
            var eid = $(this).data("id");
            location.href = `/admin/reporte_tutoria/tutorias_efectuadas/${eid}/detail`.proto().parseURL();            
        });

    }   
    var searchTutorials = function () {
        $("#btn-search").on("click", function () {    

            var startDate = $("#m_datepicker_init").datepicker({ dateFormat: 'dd/mm/yyyy' }).val();
            var endDate = $("#m_datepicker_end").datepicker({ dateFormat: 'dd/mm/yyyy' }).val();            
            loadDatatable_done(startDate,endDate);
        });
    }
      
    
    var datetimepick = function () {
        $("#m_datepicker_init").datepicker();
        $("#m_datepicker_end").datepicker();

        $("#m_datepicker_init").datepicker()        
        .on("changeDate", function (e) {
            
            $("#m_datepicker_end").datepicker('setStartDate', e.date);
        });


        $("#m_datepicker_end").datepicker()
        .on("changeDate", function (e) {
            
            $("#m_datepicker_init").datepicker("setEndDate", e.date);
        }); 
    }
    return {
        load: function () {
            datetimepick();      
            searchTutorials();  
            
        }
    }
}();

$(function () {

    reportTutorial.load();
    $("#btn-search").trigger('click');
})

























