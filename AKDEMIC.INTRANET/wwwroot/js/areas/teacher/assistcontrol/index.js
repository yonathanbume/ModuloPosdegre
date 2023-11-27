var reportAssistControl = function () {
    var datatable = null;
    var loadDatatable = function () {

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        options.data.source.read.url = `/profesor/reporte_asistencia/cursos?termId=${$("#select_term").val()}`;
        datatable = $(".m-datatable-courses").mDatatable(options);

        datatable.on('click', '.btn-detail', function () {
            var tid = $(this).data('id');
            location.href = `/profesor/reporte_asistencia/secciones/detalles/${tid}`.proto().parseURL();
        });
        datatable.on('click', '.btn-assistance', function () {
            var tid = $(this).data('id');
            location.href = `/profesor/reporte_asistencia/asistencias/${tid}`.proto().parseURL();
        });
    }
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/profesor/reporte_asistencia/cursos`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'code',
                title: 'Código',
                width: 150
            },
            {
                field: 'name',
                title: 'Nombre'
            },
            {
                field: 'sectioncode',
                title: 'Código de Sección',
                width: 150
            },
            {
                field: "options",
                title: "Opciones",
                width: 450,
                sortable: false,
                filterable: false,
                template: function (row) {
                    var template = "";
                    template += `<button data-id="${row.idsection}" class="btn btn-info btn-sm m-btn m-btn--icon btn-assistance"<span><i class="la la-list"> </i> </span>Tomar Asist.</span></span></button>`;
                    template += ` <button data-id="${row.idsection}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span>Reporte Asistencia</span></span></button>`;
                    //template+= ` <button data-id="${row.idsection}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-history"<span><i class="la la-list"> </i> </span>Historial Asistencia</span></span></button>`;
                    template += ` <a href='/profesor/secciones/${row.idsection}/registro-avance/pdf' target='_blank' data-id="${row.idsection}" class="btn btn-primary btn-sm m-btn m-btn--icon"<span><i class="la la-file-pdf-o"></i></span>Avance</span></span></a>`;
                    template += ` <a href='/profesor/secciones/${row.idsection}/matriculados/pdf' target='_blank' data-id="${row.idsection}" class="btn btn-primary btn-sm m-btn m-btn--icon"<span><i class="la la-eye"> </i> </span>Listado</span></span></a>`;


                    if (row.sectionGroups != null && row.sectionGroups.length > 0) {
                        console.log(row.sectionGroups);

                        $.each(row.sectionGroups, function (index, value) {
                            template += ` <a href='/profesor/secciones/${row.idsection}/matriculados/pdf?sectionGroupId=${value.id}' target='_blank' data-id="${row.idsection}" class="btn btn-primary btn-sm m-btn m-btn--icon"<span><i class="la la-eye"> </i> </span>${value.code}</span></span></a>`;
                        });
                    }

                    return template;
                }
            }
        ]

    }

    var select = {
        term: {
            load: function () {
                $.ajax({
                    url: `/periodos/get`,
                    type: "GET"
                })
                    .done(function (e) {
                        $("#select_term").select2({
                            data: e.items,
                            placeholder: "Seleccionar periodo"
                        });

                        $("#select_term").on("change", function () {
                            loadDatatable();
                        })

                        $("#select_term").val(e.selected).trigger("change");
                    })
            },
            init: function () {
                select.term.load();
            }
        },
        init: function () {
            select.term.init();
        }
    }

    return {
        load: function () {
            select.init();
        }
    }
}();

$(function () {
    reportAssistControl.load();
})