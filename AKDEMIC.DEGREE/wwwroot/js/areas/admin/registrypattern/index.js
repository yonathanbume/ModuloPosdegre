var registrypatterns = function () {

    var private = {
        objects: {}
    };
    var searchByDate = function () {
        $("#btn-search-dates").on('click', function (e) {
            e.preventDefault();
            var dateStartVal = $("#dateStartFilter").val();
            var dateEndVal = $("#dateEndFilter").val();
            if (dateStartVal === null || dateEndVal === null) {
                return false;
            } else {
                private.objects["tbl-datatable-registry-patterns"].draw();
            }


        });

        $("#btn-search-filters").on('click', function (e) {
            e.preventDefault();
            private.objects["tbl-datatable-registry-patterns"].draw();
        });
    };


    var datepicker = function () {
        $("#dateStartFilter").datepicker();
        $("#dateEndFilter").datepicker();



        $("#dateStartFilter").datepicker({
            clearBtn: true,
            orientation: "bottom",
            format: _app.constants.formats.datepicker
        }).on("changeDate", function (e) {
            $("#dateEndFilter").datepicker("setStartDate", moment(e.date).toDate());

        });

        $("#dateEndFilter").datepicker({
            clearBtn: true,
            orientation: "bottom",
            format: _app.constants.formats.datepicker
        }).on("changeDate", function (e) {
            $("#dateStartFilter").datepicker("setEndDate", e.date);


        });
    };

    var select = {
        career: function (facultyId) {
            $.ajax({
                type: 'GET',
                url: `/admin/padron-de-registro/carreras/${facultyId}/get`.proto().parseURL()
            }).done(function (data) {
                $("#careerId").html(null);
                $("#careerId").select2({
                    data: data.items
                });
            });

            $("#careerId").on('change', function () {
                var careerId = $(this).val();
                select.program_academic(careerId);
            });
            $("#careerId").trigger('change');
        },
        faculty: function () {
            $.ajax({
                type: 'GET',
                url: `/admin/padron-de-registro/facultades/get`.proto().parseURL()
            }).done(function (data) {
                $("#facultyId").select2({
                    data: data.items
                });
            });
            //$('#careerId').append(`<option value="${null}"> 
            //                           Todos
            //                      </option selected>`);

            $("#facultyId").on('change', function () {
                var facultyId = $(this).val();
                select.career(facultyId);
            });
            $("#facultyId").trigger('change');

        },
        program_academic: function (careerId) {
            $.ajax({
                type: 'GET',
                url: `/admin/padron-de-registro/programaacademicos/${careerId}/get`.proto().parseURL()
            }).done(function (data) {
                $("#program_academicId").html(null);
                $("#program_academicId").select2({
                    data: data.items
                });
            });
            //$('#careerId').append(`<option value="${null}"> 
            //                           Todos
            //                      </option selected>`);

            //$("#careerId").on('change', function () {
            //    private.objects["tbl-datatable-registry-patterns"].draw();
            //});
        },
        type: function () {
            $("#type").select2({
                minimumResultsForSearch: -1
            });
        },
        clasification: function () {
            $("#clasification").select2({
                minimumResultsForSearch: -1
            });
        }
    };
    var inputs = {
        init: function () {
            $("#search").doneTyping(function () {
                private.objects["tbl-datatable-registry-patterns"].draw();
            });

            $("#searchBookNumber").doneTyping(function () {
                private.objects["tbl-datatable-registry-patterns"].draw();
            }, 2);
        }
    };

    var options = {
        //columnDefs: [
        //    { "orderable": false, "targets": [1] }
        //],
        ajax: {
            type: "GET",
            dataType: "JSON",
            url: `/admin/padron-de-registro/lista-registro`.proto().parseURL(),
            data: function (values) {
                values.searchValue = $("#search").val();
                values.searchBookNumber = $("#searchBookNumber").val();
                values.careerId = $("#careerId").val();
                values.facultyId = $("#facultyId").val();
                values.academicProgramId = $("#program_academicId").val();
                values.dateStartFilter = $("#dateStartFilter").val();
                values.dateEndFilter = $("#dateEndFilter").val();
                values.type = $("#type").val();
                values.clasification = $("#clasification").val();

            }
        },
        columns: [
            {
                data: "request_name",
                name: "request_name"
            },
            {
                data: "type",
                name: "type"
            },
            {
                data: "user",
                name: "user"
            },
            {
                data: "dni",
                name: "dni"
            },
            {
                data: "careerName",
                name: "carrerName"
            },
            {
                orderable: false,
                render: function (data, type, row, meta) {
                    var tmp = "";
                    if (row.hasGeneratedCode) {
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm m-btn m-btn--icon btn-add"><span><i class="flaticon-eye"></i><span></span></span></button>`;
                    } else {
                        tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm m-btn m-btn--icon btn-add"><span><i class="flaticon-cogwheel-2"></i><span>Generar Padrón</span></span></button>`;
                    }

                    return tmp;
                },

            }
        ]

    };

    var dataTable = {
        init: function () {
            private.objects["tbl-datatable-registry-patterns"] = $("#tbl-datatable-registry-patterns").DataTable(options);
            private.objects["tbl-datatable-registry-patterns"].on("click", ".btn-add",
                function (e) {
                    e.preventDefault();
                    $(this).addLoader();
                    var cpid = $(this).data("id");
                    location.href = `/admin/padron-de-registro/${cpid}/gestion`.proto().parseURL();

                });

        }
    };

    var exportExcel = function () {

        $(".btn-excel").on('click', function (e) {
            e.preventDefault();
            $(".btn-excel").addLoader();
            $.fileDownload(`/admin/padron-de-registro/reporte-excel`.proto().parseURL(), {
                data: {
                    careerId: $("#careerId").val(),
                    facultyId: $("#facultyId").val(),
                    academicProgramId: $("#program_academicId").val(),
                    dni: $("#search").val(),
                    searchBookNumber: $("#searchBookNumber").val(),
                    dateStartFilter: $("#dateStartFilter").val(),
                    dateEndFilter: $("#dateEndFilter").val(),
                    type : $("#type").val(),
                    clasification : $("#clasification").val()
                }
            })
                .always(function () {
                    $(".btn-excel").removeLoader();
                }).done(function () {
                    $(".btn-excel").removeLoader();
                    toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                }).fail(function (e) {
                    $(".btn-excel").removeLoader();
                    toastr.error("No se encontraron padrones generados", "Error");
                });

        });

    };


    var exportExcelSunedu = function () {

        $(".btn-excel-sunedu").on('click', function (e) {
            e.preventDefault();
            $(".btn-excel-sunedu").addLoader();
            $.fileDownload(`/admin/padron-de-registro/reporte-excel-sunedu`.proto().parseURL(), {
                data: {
                    careerId: $("#careerId").val(),
                    facultyId: $("#facultyId").val(),
                    academicProgramId: $("#program_academicId").val(),
                    dni: $("#search").val(),
                    searchBookNumber: $("#searchBookNumber").val(),
                    dateStartFilter: $("#dateStartFilter").val(),
                    dateEndFilter: $("#dateEndFilter").val(),
                    type : $("#type").val(),
                    clasification : $("#clasification").val()
                }
            })
                .always(function () {
                    $(".btn-excel-sunedu").removeLoader();
                }).done(function () {
                    $(".btn-excel-sunedu").removeLoader();
                    toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                }).fail(function (e) {
                    $(".btn-excel-sunedu").removeLoader();
                    toastr.error("No se encontraron padrones generados", "Error");
                });

        });

    };

    return {
        load: function () {
            datepicker();
            searchByDate();
            select.faculty();
            select.type();
            select.clasification();
            dataTable.init();
            inputs.init();
            exportExcel();
            exportExcelSunedu();

        }
    };
}();

$(function () {
    registrypatterns.load();
});





