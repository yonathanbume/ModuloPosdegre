var diplomas = function () {

    var excel = {
        init: function () {
            this.report.init();
            this.template.init();
            this.upload_template.init();
        },
        report: {
            init: function () {
                excel.report.events();
            },
            events: function () {
                $("#btnDownloadExcel").on("click", function () {
                    excel.report.download();
                })
            },
            download: function () {
                $("#btnDownloadExcel").addLoader();
                $.fileDownload(`/admin/entrega-de-diplomas/reporte-excel`.proto().parseURL(), {
                    httpMethod: "GET",
                })
                    .done(function () {
                        $("#btnDownloadExcel").removeLoader();
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    })
                    .fail(function (e) {
                        $("#btnDownloadExcel").removeLoader();
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    });
            }
        },
        template: {
            init: function () {
                excel.template.events();
            },
            events: function () {
                $("#btnUploadTemplate").on("click", function () {
                    excel.template.download();
                })
            },
            download: function () {
                $("#btnUploadTemplate").addLoader();
                $.fileDownload(`/admin/entrega-de-diplomas/template/reporte-excel`.proto().parseURL(), {
                    httpMethod: "GET",
                })
                    .done(function () {
                        $("#btnUploadTemplate").removeLoader();
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    })
                    .fail(function (e) {
                        $("#btnUploadTemplate").removeLoader();
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    });
            }
        },
        upload_template: {
            init: function () {
                excel.upload_template.events();
            },
            events: function () {
                $("#upload-excel-form").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        $("#btn-upload-excel").addLoader();
                        var formData = new FormData($(formElement).get(0));
                        $.ajax({
                            type: "POST",
                            url: $(formElement).attr("action"),
                            data: formData,
                            contentType: false,
                            processData: false,
                            success: function () {
                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                window.location.href = window.location.href;
                            },
                            error: function () {
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            },
                            complete: function () {
                                $("#btn-upload-excel").removeLoader();
                            }
                        });
                    }
                });
            }
        }
    };



    var private = {
        objects: {}
    };

    var searchByDate = function () {
        $("#btn-search-filters").on('click', function (e) {
            e.preventDefault();
            private.objects["tbl-datatable-registry-patterns"].draw();
        });
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
        },
        diploma_status: function () {
            $("#diplomaStatus").select2();
 
            $("#diplomaStatus").on('change', function () {
                private.objects["tbl-datatable-registry-patterns"].draw();
            });
        }

    };
    var inputs = {
        init: function () {
            $("#searchValue").doneTyping(function () {
                private.objects["tbl-datatable-registry-patterns"].draw();
            });
            $("#searchBookNumber").doneTyping(function () {
                private.objects["tbl-datatable-registry-patterns"].draw();
            });
            $("#searchBookNumber").on('blur', function () {
                if ($(this).val() == '') {
                    $("#d-zone").hide();
                } else {
                    $("#d-zone").show();
                }

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
            url: `/admin/entrega-de-diplomas/lista-registro-padrones`.proto().parseURL(),
            data: function (values) {
                delete values.columns;
                values.searchValue = $("#searchValue").val();            
                values.seachBookNumber = $("#searchBookNumber").val();
                values.careerId = $("#careerId").val();
                values.facultyId = $("#facultyId").val();
                values.academicProgramId = $("#program_academicId").val();
                values.diplomaStatus = $("#diplomaStatus").val();
            }
        },
        columns: [
            {
                data: "request_name",
                name: "request_name",
  
            },
            {
                data: "type",
                name: "type",
     
            },
            {
                data: "userName",
                name: "Usuario",

            },
            {
                data: "user",
                name: "user",
 
            },
            {
                data: "dni",
                name: "dni",
        
            },
            {
                data: "careerName",
                name: "carrerName",
     
            },
            {
                data: "diplomadelivery",
                name: "diplomadelivery",
                orderable: false,   
                render: function (data, type, row, meta) {
                    var tmp = "";
                    if (row.diplomadeliveryNumber === 0 ) {
                        tmp += `<span class="m-badge m-badge--info m-badge--wide">${row.diplomadelivery}</span>`;
                    }
                    if (row.diplomadeliveryNumber === 1) {
                        tmp += `<span class="m-badge  m-badge--brand m-badge--wide">${row.diplomadelivery}</span>`;
                    }
                    if (row.diplomadeliveryNumber === 2) {
                        tmp += `<span class="m-badge  m-badge--success m-badge--wide">${row.diplomadelivery}</span>`;
                    }
                   
                    return tmp;
                },
      
            },
            {
                title: "Opciones",
                orderable: false,
                render: function (data, type, row, meta) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-brand btn-sm m-btn m-btn--icon btn-change-status"><i class="fa fa-sync"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm m-btn m-btn--icon btn-information"><i class="fa fa-eye"></i></button>`;
                    return tmp;
                },

            }
        ]

    };

    var dataTable = {
        init: function () {
            private.objects["tbl-datatable-registry-patterns"] = $("#tbl-datatable-registry-patterns").DataTable(options);
            private.objects["tbl-datatable-registry-patterns"].on("click", ".btn-change-status",
                function (e) {
                    e.preventDefault();        
                    var id = $(this).data("id");          
                    $("#change-status-form input[name='registryPatternId']").val(id);
                    $.ajax({
                        type: 'GET',
                        url: `/admin/entrega-de-diplomas/obtener-estado-de-entrega/${id}`,
                        success: function (data) {
                            $("#change-status-form select[name='deliveryDiploma']").val(data).trigger("change");
                            $("#modal-status").modal('show');      
                        }
                    });
                               
                });         
            private.objects["tbl-datatable-registry-patterns"].on("click", ".btn-information",
                function (e) {
                    e.preventDefault();
                    var id = $(this).data("id");

                    $.ajax({
                        type: 'GET',
                        url: `/admin/entrega-de-diplomas/informacion/${id}`,
                        success: function (data) {
                            $("#more-info-form input[name='fullname']").val(data.fullname);
                            $("#more-info-form input[name='code']").val(data.code);
                            $("#more-info-form input[name='phone']").val(data.phone);
                            $("#more-info-form input[name='email1']").val(data.email1);
                            $("#more-info-form input[name='email2']").val(data.email2);
                            $("#modal-info").modal('show');
                        }
                    });
                    
                });         
        }
    };

    var form = function () {
        $("#change-status-form").validate({           
            submitHandler: function (form, e) {
                e.preventDefault();
                $("#btn-submit").addLoader();
                $.ajax({
                    type: "POST",
                    url: `/admin/entrega-de-diplomas/cambiar-estado`.proto().parseURL(),
                    data: $(form).serialize(),
                    success: function () {
                        $("#modal-status").modal('hide');
                        private.objects["tbl-datatable-registry-patterns"].draw();
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    },
                    error: function () {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    },
                    complete: function () {
                        $("#btn-submit").removeLoader();
                    }
                });
                $("#change-status-form input[name='deliveryDiploma']").val('');
            }
        });
    };

    return {
        load: function () {
            select.faculty();
            select.diploma_status();
            dataTable.init();
            inputs.init();
            searchByDate();
            form();
            excel.init();
        }
    };
}();

$(function () {
    diplomas.load();
});


