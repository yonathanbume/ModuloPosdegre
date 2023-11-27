var InitApp = function () {
    var datatable = {
        students: {
            object: null,
            options: {
                bInfo: true,
                columnDefs: [
                    { "orderable": false, "targets": [] }
                ],
                ajax: {
                    url: "/reporte/estado-pagos/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.termId = $("#term_select").val();
                        data.facultyId = $("#faculty_select").val();
                        data.careerId = $("#career_select").val();
                        data.type = $("#type_select").val();
                        data.status = $("#status_select").val();
                        data.search = $("#search").val();
                    }
                },
                //order: [[1, "asc"]],
                columns: [                   
                    {
                        title: "Código",
                        data: "code",
                        width: "50px"
                    },
                    {
                        title: "Estudiante",
                        data: "name"
                    },
                    {
                        title: "Escuela Profesional",
                        data: "career",
                        width: "250px"
                    },
                    {
                        title: "Plan",
                        data: "curriculum"
                    },
                    {
                        title: "Créditos",
                        data: "credits"
                    },
                    {
                        title: "Estado",
                        data: "paid",
                        render: function (data) {
                            if (data)
                                return '<span class="m-badge m-badge--success m-badge--wide">Pagado</span>';
                            else
                                return '<span class="m-badge m-badge--danger m-badge--wide">Por pagar</span>';
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            return `<button class="btn btn-primary m-btn btn-sm m-btn--icon btn-payments" data-object="${data.proto().encode()}"><span><i class="la la-eye"></i><span>Ver pagos</span></span></button>`;
                        }
                    }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Excel',
                        action: function () {
                            var button = $(this)[0].node;
                            $(button).addClass("m-loader m-loader--right m-loader--primary").attr("disabled", true);

                            $.fileDownload(`/reporte/estado-pagos/reporte-excel`.proto().parseURL(),
                                {
                                    httpMethod: 'GET',
                                    data: {
                                        termId: $("#term_select").val(),
                                        facultyId: $("#faculty_select").val(),
                                        careerId: $("#career_select").val(),
                                        type: $("#type_select").val(),
                                        status: $("#status_select").val(),
                                        search: $("#search").val()
                                    },
                                    successCallback: function () {
                                        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                    }
                                }
                            ).done(function () {

                            })
                                .fail(function () { alert('Falló la descarga del archivo!'); })

                                .always(function () {
                                    $(button).removeClass("m-loader m-loader--right m-loader--primary").attr("disabled", false);
                                });
                        }
                    }
                ]
            },
            init: function () {
                this.object = $("#students_table").DataTable(this.options);

                $("#students_table").on("click", ".btn-payments", function (e) {
                    var object = $(this).data("object");
                    object = object.proto().decode();

                    $("#studentId").val(object.id);
                    $("#student-code").text(object.code);
                    $("#student-name").text(object.name);

                    datatable.payments.load();
                });
            },
            load: function () {
                this.object.ajax.reload();
            }
        },
        payments: {
            object: null,
            options: {
                info:false,
                paging: false,
                ordering: false,
                serverSide: false,
                ajax: {
                    url: "/reporte/estado-pagos/pagos/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.termId = $("#term_select").val();
                        data.studentId = $("#studentId").val();
                        data.type = $("#type_select").val();
                    },
                    dataSrc: ""
                },
                columns: [
                    {
                        title: "Tipo",
                        data: "type"
                    },
                    {
                        title: "Concepto",
                        data: "description"
                    },
                    {
                        title: "Estado",
                        data: "status",
                        width: "70px"
                    },
                    {
                        title: "Monto",
                        data: "total"
                    }
                ]
            },
            load: function () {
                if (this.object !== undefined && this.object !== null) {
                    this.object.clear().draw();
                    this.object.ajax.reload();
                }
                else this.object = $("#payments-table").DataTable(this.options);

                this.object.columns.adjust();

                $("#payments-modal").modal("show");
            },
        }
    };

    var select = {
        term: {
            init: function () {
                $.ajax({
                    url: "/periodos/get".proto().parseURL()
                }).done(function (data) {
                    $("#term_select").select2({
                        data: data.items
                    });

                    if (data.selected !== null) {
                        $("#term_select").val(data.selected);
                        $("#term_select").trigger("change.select2");
                    }

                    datatable.students.init();

                    $("#term_select").on("change", function (e) {
                        datatable.students.load();
                    });
                });
            }
        },
        faculty: {
            init: function () {
                $.ajax({
                    url: ("/facultades/get").proto().parseURL()
                }).done(function (data) {

                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $("#faculty_select").select2({
                        data: data.items,
                        minimumResultsForSearch: 10
                    });

                    select.career.events();
                    select.faculty.events();
                });
            },
            events: function () {
                $("#faculty_select").on("change", function () {
                    var facultyId = $("#faculty_select").val();

                    if (facultyId === _app.constants.guid.empty) {
                        datatable.students.load();

                        $("#career_select").empty();
                        $("#career_select").select2({
                            placeholder: "Seleccione una Escuela",
                            disabled: true
                        });
                    } else {
                        console.log(facultyId);
                        datatable.students.load();
                        select.career.load($("#faculty_select").val());
                    }
                });
            }
        },
        career: {
            init: function () {
                $("#career_select").select2({
                    placeholder: "Seleccione una Escuela"
                });
            },
            load: function (faculty) {
                $.ajax({
                    url: `/facultades/${faculty}/carreras/get`.proto().parseURL()
                }).done(function (data) {
                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $("#career_select").empty();
                    $("#career_select").select2({
                        placeholder: "Seleccione una Escuela",
                        data: data.items,
                        minimumResultsForSearch: 10,
                        disabled: false
                    });
                });
            },
            events: function () {
                $("#career_select").on("change", function () {
                    datatable.students.load();
                });
            }
        },
        status: {
            init: function () {
                $("#status_select").select2({
                    minimumResultsForSearch: 10
                }).on("change", function () {
                    datatable.students.load();
                });
            }
        },
        type: {
            init: function () {
                $("#type_select").select2({
                    minimumResultsForSearch: 10
                }).on("change", function () {
                    datatable.students.load();
                });
            }
        },
        init: function () {
            select.term.init();
            select.faculty.init();
            select.career.init();
            select.status.init();
            select.type.init();
        }
    };

    var events = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.students.load();
            });
        }
    }

    return {
        init: function () {
            select.init();
            events.init();
        }
    };
}();

$(function () {
    InitApp.init();
});