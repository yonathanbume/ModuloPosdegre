var AcademicSituation = function () {
    var select2 = {
        init: function () {
            this.terms.init();
            this.faculties.init();
            this.careers.init();
            this.orders.init();
        },
        terms: {
            init: function () {
                $.ajax({
                    url: "/periodos/get".proto().parseURL()
                }).done(function (data) {
                    $(".select2-terms").select2({
                        data: data.items
                    });
                  
                    if (data.selected !== null) {
                        $(".select2-terms").val(data.selected).trigger("change");
                    }

                    datatable.students.init();

                    $(".select2-terms").on("change", function (e) {
                        datatable.students.reload();
                    });
                });
            }
        },
        faculties: {
            init: function () {
                $.ajax({
                    url: "/facultades/get"
                }).done(function (result) {

                    result.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $(".select2-faculties").select2({
                        data: result.items
                    });

                    $(".select2-faculties").on("change", function () {
                        var facultyId = $(".select2-faculties").val();

                        if (facultyId === _app.constants.guid.empty) {
                            $(".select2-careers").empty();

                            $(".select2-careers").select2({
                                placeholder: "Seleccione una facultad",
                                disabled: true
                            });
                        } else {
                            select2.careers.load(facultyId);
                        }

                        datatable.students.reload();
                    });
                });
            }
        },
        careers: {
            init: function () {

                $(".select2-careers").select2({
                    placeholder: "Seleccione una facultad",
                    disabled: true
                });

                $(".select2-careers").on("change", function () {
                    datatable.students.reload();
                });
            },
            load: function (facultyId) {
                $(".select2-careers").empty();
                $.ajax({
                    url: `/carreras/get?fid=${facultyId}`.proto().parseURL()
                }).done(function (result) {

                    result.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $(".select2-careers").select2({
                        data: result.items,
                        placeholder: "Seleccione una carrera",
                        disabled: false
                    });
                });
            }
        },
        orders: {
            init: function () {
                $(".select2-orders").select2().on("change", function () {
                    datatable.students.reload();
                });
            }
        }
    };

    var datatable = {
        students: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/situacion-academica/acumulado/get".proto().parseURL(),
                    data: function (d) {
                        d.fid = $(".select2-faculties").val();
                        d.cid = $(".select2-careers").val();
                        d.tid = $(".select2-terms").val();
                        d.academicOrder = $(".select2-orders").val();
                        d.searchValue = $("#search").val();
                    }
                },
                order: [[3, "asc"]],
                columns: [
                    {
                        title: "Código",
                        data: "code",
                        width: "100px"
                    },
                    {
                        title: "Estudiante",
                        data: "name"
                    },
                    {
                        title: "Escuela Profesional",
                        data: "career",
                        width: "250px" },
                    {
                        title: "Orden de Mérito",
                        data: "order",
                        width: "120px"
                    },
                    {
                        title: "PPA",
                        data: "grade",
                        width: "80px"
                    },
                    {
                        title: "Clasificación",
                        data: null,
                        width: "120px",
                        render: function (data, type, row) {
                            if (row.meritType !== null && row.meritType > 1) {
                                var order = {
                                    0: {
                                        text: _app.constants.academicOrder.none.text,
                                        value: _app.constants.academicOrder.none.value,
                                        class: "m-badge--info"
                                    },
                                    1: {
                                        text: _app.constants.academicOrder.none.text,
                                        value: _app.constants.academicOrder.none.value,
                                        class: "m-badge--info"
                                    },
                                    2: {
                                        text: _app.constants.academicOrder.upperThird.text,
                                        value: _app.constants.academicOrder.upperThird.value,
                                        class: "m-badge--metal"
                                    },
                                    3: {
                                        text: _app.constants.academicOrder.upperFifth.text,
                                        value: _app.constants.academicOrder.upperFifth.value,
                                        class: "m-badge--accent"
                                    },
                                    4: {
                                        text: _app.constants.academicOrder.upperTenth.text,
                                        value: _app.constants.academicOrder.upperTenth.value,
                                        class: "m-badge--focus"
                                    },
                                    5: {
                                        text: _app.constants.academicOrder.upperHalf.text,
                                        value: _app.constants.academicOrder.upperHalf.value,
                                        class: "m-badge--warning"
                                    }
                                };

                                return "<span class='m-badge " +
                                    order[row.meritType].class +
                                    " m-badge--wide'>" +
                                    order[row.meritType].text +
                                    "</span>";
                            } else {
                                return "Ninguno";
                            }
                        }
                    },
                    {
                        title: "Créditos Aprob.",
                        width: "120px",
                        data: "approvedCredits"
                    }
                ]
            },
            init: function () {
                this.object = $(".students-datatable").DataTable(this.options);
            },
            reload: function () {
                this.object.ajax.reload();
            }
        }
    };

    var events = {
        onDownloadExcel: function () {
            $("#download_excel").on("click", function () {
                var url = `/admin/situacion-academica/reporte-excel?termId=${$(".select2-terms").val()}` +
                    `&facultyId=${$(".select2-faculties").val()}&careerId=${$(".select2-careers").val()}&academicOrder=${$(".select2-orders").val()}`;
                var $btn = $(this);
                $btn.addLoader();
                $.fileDownload(url, {
                    httpMethod: 'GET', successCallback: function () {
                        $btn.removeLoader();
                        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                    }
                });
            });
        },
        init: function () {
            this.onDownloadExcel();
        }
    };

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.students.reload();
            });
        }
    };

    return {
        init: function () {
            select2.init();
            search.init();
            events.init();
        }
    };
}();

$(function () {
    AcademicSituation.init();
});