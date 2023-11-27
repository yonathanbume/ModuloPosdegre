var diplomas = function () {


    var private = {
        objects: {}
    };

    var searchByDate = function () {        

        $("#btn-search-filters").on('click', function (e) {
            e.preventDefault();
            private.objects["tbl-datatable-registry-patterns"].draw();
        });
    };
    var decodeHTMLEntities = function (str) {
        var element = document.createElement('div');

        if (str && typeof str === 'string') {
            // strip script/html tags
            str = str.replace(/<script[^>]*>([\S\s]*?)<\/script>/gmi, '');
            str = str.replace(/<\/?\w(?:[^"'>]|"[^"]*"|'[^']*')*>/gmi, '');
            element.innerHTML = str;
            str = element.textContent;
            element.textContent = '';
        }

        return str;
    };


    var select = {
        career: function (facultyId) {
            $.ajax({
                type: 'GET',
                url: `/admin/padron-de-registro/carreras/${facultyId}/get`.proto().parseURL()
            }).done(function (data) {
                $("#careerId").empty();
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
                $("#program_academicId").empty();
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
        }

    };
    var inputs = {
        init: function () {
            $("#searchValue").doneTyping(function () {
                private.objects["tbl-datatable-registry-patterns"].draw();
            });

            $("#searchDiplomaNumber").doneTyping(function () {
                private.objects["tbl-datatable-registry-patterns"].draw();
            },2);
            
            $("#searchBookNumber").doneTyping(function () {
                if ($(this).val() == '') {
                    $("#d-zone").hide();
                } else {
                    $("#d-zone").show();
                }
                private.objects["tbl-datatable-registry-patterns"].draw();
            }, 2);

            $("#officeNumber")
                .doneTyping(function () {
                    if ($(this).val() == '') {
                        $("#d-zone").hide();
                    } else {
                        $("#d-zone").show();
                    }
                    private.objects["tbl-datatable-registry-patterns"].draw();
                });
            //$("#searchBookNumber").on('blur', function () {
            //   
           
            //});
            //$("#searchBookNumber").doneTyping(function () {
            //    private.objects["tbl-datatable-registry-patterns"].draw();
            //});
        }
    };

    var options = {
        //columnDefs: [
        //    { "orderable": false, "targets": [1] }
        //],
        ajax: {
            type: "GET",
            dataType: "JSON",
            url: `/admin/diplomas/lista-registro-padrones`.proto().parseURL(),
            data: function (values) {
                values.searchDiplomaNumber = $("#searchDiplomaNumber").val();
                values.searchValue = $("#searchValue").val();
                values.seachBookNumber = $("#searchBookNumber").val();
                values.careerId = $("#careerId").val();
                values.facultyId = $("#facultyId").val();
                values.academicProgramId = $("#program_academicId").val();
                values.officeNumber = $("#officeNumber").val();
                
            }
        },
        columns: [
            {
                data: "request_name",
                name: "request_name",
                title: "Concepto"
            },
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
                data: "currentAcademicYear",
                name: "currentAcademicYear",
                title: "Ciclo"
            },
            {
                title: "Opciones",
                orderable: false,
                render: function (data, type, row, meta) {
                    var tmp = "";
                        tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm m-btn m-btn--icon btn-add"><span><i class="flaticon-eye"></i><span>Ver Diploma</span></span></button>`;
                        tmp += `<div style="display:inline-block"><a data-id="${row.id}" class="btn btn-sm m-btn m-btn--icon btn-download" title="Descargar" style="width:50px!important;vertical-align: middle;border-style:none;"><img src="/images/demo/pdf.svg"/></a></div>`;                  
                    
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
                    var id = $(this).data("id");
                    location.href = `/admin/diplomas/${id}/diploma`.proto().parseURL();

                });
            private.objects["tbl-datatable-registry-patterns"].on("click", ".btn-download",
                function () {
                    var id = $(this).data("id");
                    $.fileDownload(`/admin/diplomas/descargar/Pdf/${id}`.proto().parseURL())
                        .done(function () {
                            $(".btn-excel").removeLoader();
                            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                        })
                        .fail(function (e) { 
                            $(".btn-excel").removeLoader();                 
                            var response = decodeHTMLEntities(e);
                            toastr.error(response, "Error");
                        });
                });
        }
    };

    var exportZip = function () {
      
        $("#btn-zip").on('click', function (e) {
            e.preventDefault();
            var bookNumber = $("#searchBookNumber").val();
            if (bookNumber !== '') {                              
                $("#btn-zip").addLoader();
                $.fileDownload(`/admin/diplomas/diploma-zip?searchBookNumber=${bookNumber}`.proto().parseURL())
                    .always(function () {
                        $("#btn-zip").removeLoader();
                    }).done(function () {
                        $("#btn-zip").removeLoader();
                        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                    }).fail(function () {
                        $("#btn-zip").removeLoader();
                        toastr.error("No se pudo descargar el archivo", "Error");
                    });
            };

       

        });
     
       
    };
    var events = {
        init: function () {

        },
        downloadPdf: {

        }
    }

    return {
        load: function () {
            select.faculty();   
            dataTable.init();
            inputs.init();
            searchByDate();
            exportZip();
            //exportExcel();

        }
    };
}();

$(function () {
    diplomas.load();
});


