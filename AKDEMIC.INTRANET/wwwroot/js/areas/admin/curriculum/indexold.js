var curriculum = function () {
    var datatable = null;
    var loadFacultiesSelect = function () {
        $.ajax({
            url: `/admin/reporte_asistencia/getfaculties`.proto().parseURL()
        }).done(function (data) {
            $("#select_faculties").select2({
                data: data.items
            }).trigger("change");          
            });

        $("#select_faculties").on('change', function () {
            var fid = $(this).val();
            loadDatatable(fid);
        });
                 
    }
   
    var getDetail = function () {
        $(".m-datatable").on('click', '.btn-detail', function () {
            var cid = $(this).data("id");
            mApp.block(".m-portlet", { type: "loader", message: "Cargando..." });    
            location.href = `/admin/planes-de-estudios/${cid}/get`.proto().parseURL();
        });
    }
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
                field: 'name',
                title: 'Carrera',
                width: 150
            },
            {
                field: "options",
                title: "Opciones",
                width: 200,
                sortable: false,
                filterable: false,
                template: function (row) {
                    return `<button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail" <span><i class="la la-eye"> </i> </span> Ver plan de estudio </span></span></button>`;
                }
            }
        ]
    }
    var loadDatatable = function (fid) {   

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        options.data.source.read.url = `/admin/planes-de-estudios/carreras/${fid}/get`.proto().parseURL();
        datatable = $(".m-datatable").mDatatable(options);
        getDetail();

    }
    return {
        load: function () {
            loadFacultiesSelect();            
        }
    }
}();

$(function () {
    curriculum.load();
});