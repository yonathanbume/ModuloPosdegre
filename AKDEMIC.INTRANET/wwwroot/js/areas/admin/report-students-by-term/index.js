var Datatable = function () {
    var id = "#datatable";
    var datatable;

    const options = {
        "searching": false,
        ajax: {
            data: function (data, settings) {
                data.search = $("#search").val();
                data.careerId = $("#careerId").val();
                data.facultyId = $("#facultyId").val();

                for (var i = 0; i < data.columns.length; i++) {
                    delete data.columns[i].name;
                    delete data.columns[i].search;
                }
            },
            url: "/admin/reporte-semestres-matriculados/get".proto().parseURL(),
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
                title: "Facultad",
                data: "faculty"
            },
            {
                title: "Escuela Profesional",
                data: "career"
            },
            {
                title: "Periodos matriculados",
                data: "terms"
            }
        ],
        dom: 'Bfrtip',
        buttons: [
            {
                text: 'Excel',
                action: function (e, dt, node, config) {
                    var url = `/admin/reporte-semestres-matriculados/get-excel?careerId=${$("#careerId").val()}&facultyId=${$("#facultyId").val()}`;
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
                        minimumResultsForSearch: -1,
                        allowClear: true,
                        data: data.items
                    });
                });
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