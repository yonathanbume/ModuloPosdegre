var InitApp = function () {
    var datatable = {
        students: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/bienestar_institucional/students-datatable".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.termId = $("#termId").val();
                        data.facultyId = $("#facultyId").val();
                        data.careerId = $("#careerId").val();
                        data.admissionTypeId = $("#admissionTypeId").val();
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        data: 'userName',
                        title: 'Usuario'
                    },
                    {
                        data: 'fullname',
                        title: 'Nombres Completos'
                    },
                    {
                        data: 'email',
                        title: 'Correo electrónico'
                    },
                    {
                        data: 'faculty',
                        title: 'Facultad'
                    },
                    {
                        data: 'career',
                        title: 'Escuela Profesional'
                    },
                    {
                        title: 'Lleno Ficha',
                        data: null,
                        render: function (data) {
                            var template = "";
                            if (data.hasStudentInformationTerm) {
                                template = '<span class="m--font-bolder m--font-success">SI</span>';
                            } else {
                                template = '<span class="m--font-bolder m--font-danger">NO</span>';
                            }

                            return template;
                        }
                    },
                    {
                        data: null,
                        title: 'Opciones',
                        render: function (data) {
                            var template = "";
                            var detailUrl = `/admin/bienestar_institucional/ficha-estudiante/${data.id}/periodo/${$("#termId").val()}`.proto().parseURL();
                            var constancyUrl = `/admin/bienestar_institucional/constancia/${data.id}/periodo/${$("#termId").val()}`.proto().parseURL();
                            //Detail
                            template += `<a href="${detailUrl}" style="margin-bottom:3px" `;
                            template += "class='btn btn-primary ";
                            template += "m-btn btn-sm m-btn--icon' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-eye'></i><span>Ficha</span></span></a> ";

                            if (data.hasStudentInformationTerm) {
                                //Detail
                                template += `<a href="${constancyUrl}" `;
                                template += "class='btn btn-primary ";
                                template += "m-btn btn-sm m-btn--icon' ";
                                template += " data-id='" + data.id + "'>";
                                template += "<span><i class='la la-download'></i><span>Constancia</span></span></a> ";
                            }

                            return template;

                        }

                    }
                ],
            },
            reload: function () {
                if (this.object == null) {
                    this.object = $("#data-table").DataTable(this.options);
                } else {
                    this.object.ajax.reload();
                }
            },
        },
    };

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.students.reload();
            });
        }
    };
    var select = {
        init: function () {
            this.faculties.init();
            this.careers.init();
            this.admissionTypes.init();
            this.terms.init();
        },
        terms: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: `/periodos/get/v2`.proto().parseURL()
                }).done(function (result) {
                    $("#termId").select2({
                        data: result.items
                    });
                    if (result.selected !== null) {
                        $("#termId").val(result.selected).trigger('change');
                    }
                });
            },
            events: function () {
                $("#termId").on('change', function () {
                    datatable.students.reload();
                });
            }
        },
        faculties: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: ("/facultades/get").proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#facultyId").select2({
                        data: result.items,
                    });
                });
            },
            events: function () {
                $("#facultyId").on("change", function () {
                    let facultyId = $(this).val();
                    select.careers.load(facultyId);
                    datatable.students.reload();
                });
            }
        },
        careers: {
            init: function () {
                $("#careerId").select2();
                this.events();
            },
            clear: function () {
                $("#careerId").empty();
                $("#careerId").html(`<option value=0 selected>Todos</option>`);
            },
            load: function (id) {
                this.clear();
                $.ajax({
                    url: (`/facultades/${id}/carreras/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#careerId").select2({
                        data: result.items,
                    });
                });
            },
            events: function () {
                $("#careerId").on("change", function () {
                    datatable.students.reload();
                });
            }
        },
        admissionTypes: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: ("/admissionTypes/get").proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#admissionTypeId").select2({
                        data: result.items,
                    });
                });
            },
            events: function () {
                $("#admissionTypeId").on("change", function () {
                    datatable.students.reload();
                });
            }
        }

    };

    var report = {
        excel: {
            init: function () {
                $("#btn-reporte-excel").click(function () {
                    var $btn = $(this);
                    $btn.addLoader();
                    $.fileDownload("/admin/bienestar_institucional/reporte-excel/get".proto().parseURL(), {
                        httpMethod: "GET",
                        data: {
                            termId: $("#termId").val(),
                            facultyId: $("#facultyId").val(),
                            careerId: $("#careerId").val(),
                            admissionTypeId: $("#admissionTypeId").val(),
                            searchValue: $("#search").val(),
                        },
                        successCallback: function () {
                            $btn.removeLoader();
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        },
                        failCallback: function () {
                            $btn.removeLoader();
                            toastr.success(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        }
                    });
                });
            }
        },
        init: function () {
            this.excel.init();
        }
    }

    return {
        init: function () {
            search.init();
            select.init();
            report.init();
        }
    }
}();

$(function () {
    InitApp.init();
});