var Tutorial = function () {

    var eid = $("#Id").val();
    var datatable_done = null;    
    var datatable_student = null;    
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
                    return `<button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span> Asistencia </span></span></button>`;
                }
            }
        ] }
    var options_student = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/profesor/tutoria_asistencia/asistencia_alumnos/${eid}`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "assistance",
                title: "Inasistencia",
                width: 90,
                textAlign: "center",
                template: function (row,index) {
                    var tmp = ``;
                    tmp += `<label class='m-checkbox m-checkbox--single m-checkbox--solid m-checkbox--brand'>`;
                    tmp += `<input data-id='${row.id}' type='checkbox' name='${index}' value='true' ${row.assistance
                        ? "checked"
                        : ""}/>`;
                    tmp += `<input name='${index}' type='hidden' value='false' />`;
                    tmp += "<span></span></label>";
                    return tmp;
                }              
            },
            {
                field: 'name',
                title: 'Nombres Completos',
                width: 250  
            },
            {
                field: 'email',
                title: 'Email',
                width: 250
            }
        ]
    }


    var events = function () {

            $("#btn-save").on('click', function () {
                var arraycheckeds = [];           
                $("input:checkbox:checked").each(function () {     
                    var json_object = {
                        tutorialId : eid,
                        id : $(this).data("id"),
                        absent : $(this).val()            
                    };                
                    arraycheckeds.push(json_object);  
                });                        
            
                $.ajax({
                    type: "POST",
                    url: `/profesor/tutoria_asistencia/asistencia/detalle/${eid}/post`.proto().parseURL(),
                    data: {
                        data: {
                            List: arraycheckeds
                        }
                    },                
                    success: function () {
                        toastr.success(_app.constants.toastr.message.success.update, _app.constants.toastr.title.success);
                        datatable.reload();
                    },
                    error: function () {
                        toastr.error(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                    }
                });
            
            });
        }

    var loadDatatable_done = function (startDate, endDate) {

        if (datatable_done !== null) {
            datatable_done.destroy();
            datatable_done = null;
        }
        options_done.data.source.read.url = `/profesor/tutoria_asistencia/tutorias_profesor/get?startDate=${startDate}&endDate=${endDate}`.proto().parseURL();
        datatable_done = $(".m-datatable_tutorial_done").mDatatable(options_done);
        datatable_done.on('click', '.btn-detail', function () {
            var eid = $(this).data("id");
            location.href = `/profesor/tutoria_asistencia/asistencia/${eid}`.proto().parseURL();
        });
        
    }
    var loadDatatable_student = function () {

        if (datatable_student !== null) {
            datatable_student.destroy();
            datatable_student = null;
        }
        
        datatable_student = $(".m-datatable_student").mDatatable(options_student);

        
    }
    var searchTutorials = function () {
        $("#btn-search").on("click", function () {

            var startDate = $("#m_datepicker_init").datepicker({ dateFormat: 'dd/mm/yyyy' }).val();
            var endDate = $("#m_datepicker_end").datepicker({ dateFormat: 'dd/mm/yyyy' }).val();
            loadDatatable_done(startDate, endDate);
        });
    }
     var datetimepick = function () {
        $("#m_datepicker_init").datepicker().on('changeDate', function () {
            var startDate = $("#m_datepicker_end").val();
            $('#m_datepicker_end').datepicker('setEndDate', startDate);
        });

        $("#m_datepicker_end").datepicker().on('changeDate', function () {
            var endtDate = $("#m_datepicker_init").val();
            $('#m_datepicker_end').datepicker('setStartDate', endtDate);
        });
    }
    return {
        load: function () {
            datetimepick();
            searchTutorials();            
            loadDatatable_student();
            events();
        }
    }
}();

$(function () {

    Tutorial.load();
    $("#btn-search").trigger('click');
})

























