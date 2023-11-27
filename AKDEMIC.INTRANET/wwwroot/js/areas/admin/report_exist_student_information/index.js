var reportExistStudentInformation = function () {
    var datatable = null;
    var options = getSimpleDataTableConfiguration({
        url: `/admin/reporte_estudiante_fichas/get/`.proto().parseURL(),
        pageLength: 10,
        orderable: [],
        columns: [
            {
                data: 'username',
                title: 'Código',
            },
            {
                data: 'name',
                title: 'Nombre',
            },
            {
                data: 'paternalsurname',
                title: 'Apellido Paterno',
                width: 150
            },
            {
                data: 'maternalsurname',
                title: 'Apellido Materno',
            },
            {
                data: 'email',
                title: 'Email',
            },
            {
                data: null,
                title: 'Ficha asignada',
                render: function (row) {
                    if (row.existfile == true) {
                        var tmp = `<span class="m-badge  m-badge--success m-badge--wide">Asignado</span>`;
                        return tmp;
                    }
                    else {
                        var tmp = `<span class="m-badge  m-badge--danger m-badge--wide">No presenta</span>`;
                        return tmp;
                    }
                }
            }


        ]
    });
    //var loadTermSelect = function () {
    //    $.ajax({
    //        url: `/periodos/get`.proto().parseURL()
    //    }).done(function (data) {
    //        $("#select2Terms").select2({
    //            data: data.items
    //        }).val(data.items.selected);
    //    });
    //}
    var loadCareersSelect = function () {
        $("#select2Careers").select2({
            ajax: {
                url: `/admin/reporte_estudiante_fichas/carreras/get`.proto().parseURL(),
                delay: 300,
            },
            minimumInputLength: 0,
            placeholder: 'Carreras',
            allowClear: true
        });

        // $.ajax({
        //     url: `/carreras/get`.proto().parseURL()
        // }).done(function (data) {
        //     $("#select2Careers").select2({
        //         data: data.items
        //     });
        // });
    }


    var loadDatatable = function (cid) {
        if (datatable != null) {
            datatable.destroy();
            datatable = null;
        }
        var newoptions = options;
        newoptions.ajax.url = `/admin/reporte_estudiante_fichas/get/${cid}`.proto().parseURL();
        //options.data.source.read.url = `/admin/reporte_estudiante_fichas/get/${cid}`.proto().parseURL();
        datatable = $("#ajax_data").DataTable(newoptions);

    }


    var search = function () {
        $("#search").on('click', function () {
            var cid = $("#select2Careers").val();
            loadDatatable(cid);
        });
    }


    return {
        load: function () {
            //loadTermSelect();
            loadCareersSelect();
            search();

        }
    }
}();



$(function () {

    reportExistStudentInformation.load();

})