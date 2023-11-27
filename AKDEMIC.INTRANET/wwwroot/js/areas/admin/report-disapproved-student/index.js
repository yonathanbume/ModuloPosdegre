var Datatable = function () {
    var id = "#datatable";
    var datatable;

    const options = {
        "searching": false,
        ajax: {
            data: function (data, settings) {
                data.search = $("#search").val();
                data.termId = $("#termId").val();
                data.careerId = $("#careerId").val();
                data.facultyId = $("#facultyId").val();

                for (var i = 0; i < data.columns.length; i++) {
                    delete data.columns[i].name;
                    delete data.columns[i].search;
                }
            },
            url: "/admin/reporte-ponderado-desaprobado/get".proto().parseURL(),
            type: "GET"
        },
        columns: [
            {
                title: "Código",
                data: "code"
            },
            {
                title: "Estudiante",
                data: "student"
            },
            {
                title: "Periodo Académico",
                data: "term"
            },
            {
                title: "Facultad",
                data: "faculty"
            },
            {
                title: "Escuela Profesional",
                data: "career"
            },
            {
                title: "Puntaje",
                data: "score"
            }
        ],
        dom: 'Bfrtip',
        buttons: [
            {
                text: 'Excel',
                action: function (e, dt, node, config) {
                    var url = `/admin/reporte-ponderado-desaprobado/get-excel?termId=${$("#termId").val()}&careerId=${$("#careerId").val()}&facultyId=${$("#facultyId").val()}`;
                    window.open(url, "_blank");
                }
            }
        ]
    };

    var select = function () {

        $.when(
            $.ajax({
                url: `/carreras/v2/get`.proto().parseURL()
            })
        ).then(function (data) {
            $("#careerId").select2({
                placeholder: "Seleccione una escuela",
                //minimumResultsForSearch: -1,
                allowClear: true,
                data: data.items
            });
            $('#careerId').val('').trigger();
        });

        $.when(
            $.ajax({
                url: `/facultades/v2/get`.proto().parseURL()
            })
        ).then(function (data) {
            $("#facultyId").select2({
                placeholder: "Seleccione una facultad",
                //minimumResultsForSearch: -1,
                allowClear: true,
                data: data.items
            });
            $("#facultyId").on("change", function (e) {
                var id = $(this).val();
                $('#careerId').empty().select2();
                $.when(
                    $.ajax({
                        url: `/facultades/${id}/carreras/v2/get`
                    })
                ).then(function (data) {
                    $("#careerId").select2({
                        placeholder: "Seleccione una escuela",
                        //minimumResultsForSearch: -1,
                        allowClear: true,
                        data: data.items
                    });
                });
            });
        });

        $.when(
            $.ajax({
                url: `/periodos-finalizados/get`.proto().parseURL()
            })
        ).then(function (data) {
            $("#termId").select2({
                placeholder: "Seleccione una periodo académico",
                //minimumResultsForSearch: -1,
                allowClear: true,
                data: data.items
            });
        });

        $('#search').doneTyping(function () {
            datatable.ajax.reload();
        });
        $('#facultyId').change(function () {
            datatable.ajax.reload();
        });
        $('#careerId').change(function () {
            datatable.ajax.reload();
        });
        $('#termId').change(function () {
            datatable.ajax.reload();
        });
    };

    var loadDatatable = function () {
        datatable = $(id).DataTable(options);
    };
    return {
        init: function () {
            loadDatatable();
            select();
        }
    };
}();

$(function () {
    Datatable.init();
});