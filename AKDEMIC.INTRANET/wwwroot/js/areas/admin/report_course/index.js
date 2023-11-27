var reportCourse = function () {
    var datatable = {
        object: null,
        options: getSimpleDataTableConfiguration({
            url: `/admin/reporte_asistencia_curso/curso/periodo`.proto().parseURL(),
            data: function (data) {
                delete data.colums;
                data.pid = $("#select_terms").val();
                data.carId = $("#select_careers").val();
                data.curId = $("#select_curriculums").val();
                data.search = $("#search").val();
            },
            pageLength: 10,
            orderable: [],
            columns: [
                {
                    data: 'courseCode',
                    title: 'Código'
                },
                {
                    data: "courseName",
                    title : "Curso"
                },
                {
                    data: 'section',
                    title: 'Sección'
                },
                {
                    data: 'teachers',
                    title: 'Docentes'
                },
                {
                    data: null,
                    title: "Opciones",
                    render: function (row) {
                        return `<button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span>Ver Detalle </span></span></button>`;
                    }
                }
            ]
        }),
        reloadTable: function () {
            if (datatable.object === null) {
                datatable.init();
            } else {
                datatable.object.ajax.reload();
            }
        },
        events: {
            onDetails: function () {
                $("#report-datatable").on('click', '.btn-detail', function () {
                    var ctid = $(this).data("id");
                    location.href = `/admin/reporte_asistencia_curso/reporte_curso_vista/${ctid}`.proto().parseURL();
                });
            },
            search: function () {
                $("#search").doneTyping(function () {
                    datatable.reloadTable();
                });
            },
            init: function () {
                this.search();
                this.onDetails();
            }
        },
        init: function () {
            datatable.events.init();
            datatable.object = $("#report-datatable").DataTable(datatable.options);
        }
    };

    var loadCareersSelect = function () {
        $.ajax({
            url: `/carreras/v2/get`.proto().parseURL()
        }).done(function (data) {
            $("#select_careers").empty();
            $("#select_careers").select2({
                data: data.items,                
                placeholder: "Escuelas"
            });

            $("#select_careers").on('change', function () {
                loadCurriculumsSelect();
            });
            $("#select_careers").trigger("change");
        });
    };

    var loadCurriculumsSelect = function () {

        $("#select_curriculums").on('change', function () {
            datatable.reloadTable(); 
        });

        var cid = $("#select_careers").val();
        $.ajax({
            url: `/carreras/${cid}/planestudio/get`.proto().parseURL()
        }).done(function (data) {
            $("#select_curriculums").empty();
            $("#select_curriculums").select2({
                data: data.items,                
                placeholder: "Planes de estudio"
            }).trigger("change");
        });
  
    };

    var isFIrst = true;

    var loadTermsSelect = function () {
        $.ajax({
            url: `/periodos/get`.proto().parseURL()
        }).done(function (data) {
            $("#select_terms").select2({
                data: data.items
            }).val(data.selected);
        });
    }; 

    var events = {
        onExport: function () {
            $("#btn_excel_sections").on("click", function () {
                var termId = $("#select_terms").val();
                var careerId = $("#select_careers").val();
                var curriculumId = $("#select_curriculums").val();
                var endDate = $("#end_date").val();

                var url = `/admin/reporte_asistencia_curso/reporte-general-excel/${termId}/${careerId}/${curriculumId}?endDate=${endDate}`;
                window.open(url, "_blank");
            });
        },
        init: function () {
            this.onExport();
        }
    };

    var datepicker = {
        init: function () {
            $("#end_date").datepicker({
                clearBtn: false,
                orientation: "bottom",
                format: _app.constants.formats.datetimepicker
            });
        }
    }
    
    return {
        load: function () {
            events.init();
            loadTermsSelect();
            loadCareersSelect();
            datepicker.init();
        }
    };
}();

$(function () {
    reportCourse.load();    
})