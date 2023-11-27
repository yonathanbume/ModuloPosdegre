var InitApp = function () {
    var datatable = {
        reports: {
            object: null,
            reload: function () {
                var data = $("#component_select").select2("data")[0];
                var units = data.number;

                let columns = [
                    {
                        data: "course",
                        title: "Curso"
                    },
                    {
                        data: "section",
                        title: "Sección"
                    },
                    {
                        data: "term",
                        title: "Periodo"
                    }
                ];

                for (let i = 0; i < units; i++) {
                    var title = `Unidad ${i + 1}`;
                    columns.push(
                        {
                            data: null,
                            title: title,
                            render: function (data) {
                                let index = i;

                                var html = `<span class="m-badge m-badge--wide m-badge--success"> A tiempo </span>`;

                                console.log(data.units[index]);

                                if (data.units[index] == undefined || data.units[index].complete == false) {
                                    html = `<span class="m-badge m-badge--wide m-badge--danger"> Falta </span>`;
                                } else if (data.units[i].waslate == false) {
                                    html = `<span class="m-badge m-badge--wide m-badge--warning"> Tarde </span>`;
                                }

                                return html;
                            }
                        }
                    );
                }


                this.delete();

                this.object = $("#data-table").DataTable({
                    dom: 'Bfrtip',
                    buttons: [
                        {
                            text: "<i class='la la-file-excel-o'></i><span>Excel</span>",
                            action: function () {
                                $.fileDownload(`/academico/registro-notas/reporte-excel/{${$("#component_select").val()}}/{${$("#term_select").val()}}/{${$("#faculty_select").val()}}`.proto().parseURL(),
                                    {
                                        httpMethod: 'GET',

                                        successCallback: function () {
                                            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                        }
                                    }
                                ).done(function () {
                                    //console.log("entro");
                                })
                                    .fail(function () { alert('Falló la descarga del archivo!'); })

                                    .always(function () {
                                        console.log("always");
                                    });
                            }
                        }
                    ],
                    ajax: {
                        url: "/academico/registro-notas/get".proto().parseURL(),
                        type: "GET",
                        dataType: "JSON",
                        data: function (data) {
                            delete data.columns;

                            data.search = $("#search").val();
                            data.term = $("#term_select").val();
                            data.faculty = $("#faculty_select").val();
                            //data.school = $("#school_select").val();
                            //data.career = $("#career_select").val();
                            data.component = $("#component_select").val();
                        }
                    },
                    columns: columns,
                });
            },
            delete: function () {
                if (this.object !== null && this.object !== undefined && $.fn.dataTable.isDataTable("#data-table")) {
                    this.object.clear().destroy();
                    $("#table-container").html(`<table class="table table-striped table-bordered" id="data-table"></table>`);
                }               
            }
        }
    };

    var select = {
        faculty: {
            init: function () {
                $.ajax({
                    url: ("/facultades/v2/get").proto().parseURL()
                }).done(function (data) {
                    $("#faculty_select").select2({
                        data: data.items,
                        minimumResultsForSearch: -1
                    });

                    select.faculty.events();
                });
            },
            events: function () {
                $("#faculty_select").on("change", function () {
                    var facultyId = $("#faculty_select").val();

                    //if (facultyId === _app.constants.guid.empty) {
                    //    $("#school_select").empty();
                    //    $("#school_select").select2({
                    //        placeholder: "Seleccione una facultad",
                    //        disabled: true
                    //    });

                    //    $("#career_select").empty();
                    //    $("#career_select").select2({
                    //        placeholder: "Seleccione una escuela",
                    //        disabled: true
                    //    });
                    //} else {
                    //    select.school.load($("#faculty_select").val());
                    //}

                    datatable.reports.reload();
                });
            }
        },
        career: {
            init: function () {
                $("#career_select").select2({
                    placeholder: "Seleccione una escuela"
                });
                select.career.events();
            },
            load: function (school) {
                $.ajax({
                    url: `/escuelas/${school}/carreras/v2/get`.proto().parseURL()
                }).done(function (data) {
                    $("#career_select").empty();
                    $("#career_select").select2({
                        placeholder: "Seleccione una carrera",
                        data: data.items,
                        minimumResultsForSearch: -1,
                        disabled: false
                    });
                });
            },
            events: function () {
                $("#career_select").on("change", function () {
                    datatable.students.reload();
                });
            }
        },
        component: {
            init: function () {
                $.ajax({
                    url: "/academico/registro-notas/componentes/get".proto().parseURL()
                }).done(function (data) {
                    $("#component_select").select2({
                        data: data.items,
                        minimumResultsForSearch: -1
                    });

                    select.component.events();
                    datatable.reports.reload();
                });
            },
            ajax: $.ajax({
                url: "/academico/registro-notas/componentes/get".proto().parseURL()
            }),
            load: function (data) {
                console.log(data);
                $("#component_select").select2({
                    data: data.items,
                    minimumResultsForSearch: -1
                });
                select.component.events();
            },
            events: function () {
                $("#component_select").on("change", function () {
                    datatable.reports.reload();
                });
            }
        },
        term: {
            ajax: $.ajax({
                url: "/periodos/get".proto().parseURL()
            }),
            load: function (data) {
                $("#term_select").select2({
                    data: data.items,
                    minimumResultsForSearch: -1
                });
                select.term.events();
            },
            events: function () {
                $("#term_select").on("change", function () {
                    datatable.reports.reload();
                });
            }
        },
        init: function () {
            select.faculty.init();
            //select.school.init();
            //select.career.init();
            //this.component.init();

            $.when(
                select.term.ajax,
                select.component.ajax
            ).then(function (data1, data2) {
                select.term.load(data1[0]);
                select.component.load(data2[0]);

                datatable.reports.reload();
            });
        }
    };

    var search = {
        init: function () {
            $("#search").donetyping(function () {
                datatable.students.reload();
            });
        }
    };

    return {
        init: function () {
            select.init();
        }
    };
}();

$(function () {
    InitApp.init();
});