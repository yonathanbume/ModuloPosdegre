var registrypattern_review = function () {

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
        career: function () {
            $.ajax({
                type: 'GET',
                url: `/admin/padron-de-registro/carreras/get`.proto().parseURL()
            }).done(function (data) {
                $("#careerId").select2({
                    data: data.items
                });
            });
            $('#careerId').append(`<option value="${null}"> 
                                       Todos
                                  </option selected>`);

            $("#careerId").on('change', function () {
                private.objects["tbl-datatable-registry-patterns"].draw();
            });
        }

    };
    var inputs = {
        init: function () {
            $("#search").doneTyping(function () {
                private.objects["tbl-datatable-registry-patterns"].draw();
            });
        }
    };

    var options = {
        //columnDefs: [
        //    { "orderable": false, "targets": [1] }
        //],
        ajax: {
            type: "GET",
            dataType: "JSON",
            url: `/admin/gestion-aprobacion-registros-padrones/lista-registros-aprobados`.proto().parseURL(),
            data: function (values) {
                values.searchValue = $("#search").val();
                values.searchBookNumber = $("#searchBookNumber").val();
                values.careerId = $("#careerId").val();
                values.dateStartFilter = $("#dateStartFilter").val();
                values.dateEndFilter = $("#dateEndFilter").val();

            }
        },
        columns: [            
            {
                data: "type",
                name: "type",
                title: "Tipo"
            },
            {
                data: "user",
                name: "user",
                title: "Estudiante"
            },
            {
                data: "dni",
                name: "dni",
                title: "DNI"
            },
            {
                data: "careerName",
                name: "carrerName",
                title: "Escuela Profesional"
            },
            {
                title: "Opciones",
                orderable: false,
                render: function (data, type, row, meta) {
                    return `<button data-id="${row.id}" class="btn btn-danger btn-sm m-btn m-btn--icon btn-change-state"><span><i class="flaticon-close"></i><span>Denegar</span></span></button>`;
                },

            }
        ]

    };

    var dataTable = {
        init: function () {
            private.objects["tbl-datatable-registry-patterns"] = $("#tbl-datatable-registry-patterns").DataTable(options);           
            private.objects["tbl-datatable-registry-patterns"].on("click", ".btn-change-state",
                function (e) {
                    e.preventDefault();
                    $(this).addLoader();
                    var cpid = $(this).data("id");
                    $.ajax({
                        type: 'POST',
                        url: `/admin/gestion-aprobacion-registros-padrones/cambiar-estado/${cpid}`.proto().parseURL(),
                        success: function () {
                            private.objects["tbl-datatable-registry-patterns"].draw();
                            toastr.success("Tarea realizada satisfactoriamente", "Éxito");
                        }
                    })                    

                });      
        }
    };

    var approvedAll = function () {

        $(".btn-approved").on('click', function (e) {
            e.preventDefault();
            $(".btn-approved").addLoader();
            $.ajax({
                type: 'POST',
                url: `/admin/gestion-aprobacion-registros-padrones/aprobacion`.proto().parseURL(),
                data: {
                    searchValue : $("#search").val(),
                    searchBookNumber : $("#searchBookNumber").val(),
                    careerId: $("#careerId").val(),
                    dateStartFilter : $("#dateStartFilter").val(),
                    dateEndFilter : $("#dateEndFilter").val()
                },
                success: function () {
                    private.objects["tbl-datatable-registry-patterns"].draw();
                    toastr.success("Tarea realizada satisfactoriamente", "Éxito");
                },
                complete: function myfunction() {
                    $(".btn-approved").removeLoader();
                }
            })
            

        });

    };    
    

    return {
        load: function () {
            datepicker();
            searchByDate();
            select.career();
            dataTable.init();
            inputs.init();
            approvedAll();

        }
    };
}();

$(function () {
    registrypattern_review.load();
});





