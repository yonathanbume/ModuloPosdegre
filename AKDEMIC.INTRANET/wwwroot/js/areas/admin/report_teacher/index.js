var InitApp = function () {
    var select = {
        term: {
            init: function () {
                this.events();
                this.load();
            },
            load: function () {
                $.ajax({
                    url: "/periodos/get".proto().parseURL()
                }).done(function (data) {
                    $("#term-select").select2({
                        data: data.items
                    });

                    if (data.selected !== null) {
                        $("#term-select").val(data.selected).trigger("change");
                    }
                });
            },
            events: function (){
                $("#term-select").on("change", function (e) {
                    datatable.sections.reload();
                });
            }
        },
        academicDepartment: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: `/departamentos-academicos/get`.proto().parseURL()
                }).done(function (data) {
                    $("#academicDepartment-select").select2({
                        placeholder: "Sin asignar",
                        data: data,
                        allowClear: true
                    });
                });
            },
            events: function () {
                $("#academicDepartment-select").on("change", function () {
                    datatable.sections.reload();
                });
            }
        },
        init: function () {
            this.term.init();
            this.academicDepartment.init();
        }
    };

    var datatable = {
        sections: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/reporte-docentes/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.termId = $("#term-select").val();
                        data.academicDepartmentId = $("#academicDepartment-select").val();
                        data.search = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        data: "academicDepartment",
                        title: "Dep. Académico",
                        orderable : false,
                        render: function (row) {
                            var tpm = row;
                            if (row === "" || row === null) {
                                tpm = "Sin asignar";
                            }
                            return tpm;
                        }
                    },
                    {
                        data: "username",
                        title: "Usuario"
                    },
                    {
                        data: "name",
                        title: "Docente",
                        orderable: false
                    },
                    {
                        data: "sections",
                        title: "Secciones Asignadas",
                        orderable: false
                    },
                    {
                        data: null,
                        title: "Opciones",
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var url = `/admin/reporte-docentes/detalle/${data.id}/${$("#term-select").val()}`.proto().parseURL();
                            template += `<a class="btn btn-brand m-btn btn-sm m-btn--icon btn-manage" href="${url}"><span><i class="la la-gear"></i><span>Detalle</span></span></a> `;
                            return template;
                        }
                    }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        title: 'Docentes por periodo académico',
                        exportOptions: {
                            columns: [0, 1, 2, 3]
                        }
                    }
                ]
            },
            reload: function () {
                if (this.object == null) {
                    this.object = $("#data-table").DataTable(this.options);
                } else {
                    this.object.ajax.reload();
                }
            }
        }
    };

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.sections.reload();
            });
        }
    }

    return {
        init: function () {
            select.init();
            search.init();
        }
    }
}();

$(function () {
    InitApp.init();
});

