var InitApp = function () {
    var select2 = {
        init: function () {
            this.terms.init();
            this.faculties.init();
            this.careers.init();
            this.orders.init();

            this.curriculum.init();
            this.academicyear.init();
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

                    //datatable.students.init();

                    //$(".select2-terms").on("change", function (e) {
                    //    datatable.students.reload();
                    //});
                });
            }
        },
        faculties: {
            init: function () {
                $.ajax({
                    url: "/facultades/get"
                }).done(function (result) {

                    //result.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

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

                        //datatable.students.reload();
                    });
                    $(".select2-faculties").trigger("change");
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
                    var careerId = $(".select2-careers").val();
                    if (careerId === _app.constants.guid.empty) {
                        $("#curriculum-select").empty();

                        $("#curriculum-select").select2({
                            placeholder: "Seleccione una escuela",
                            disabled: true
                        });
                    } else {
                        select2.curriculum.load(careerId);
                    }

                    //datatable.students.reload();
                });
            },
            load: function (facultyId) {
                $(".select2-careers").empty();
                $.ajax({
                    url: `/carreras/get?fid=${facultyId}`.proto().parseURL()
                }).done(function (result) {

                    //result.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $(".select2-careers").select2({
                        data: result.items,
                        placeholder: "Seleccione una carrera",
                        disabled: false
                    });
                    $(".select2-careers").trigger("change");
                });
            }
        },
        orders: {
            init: function () {
                $(".select2-orders").select2().on("change", function () {
                    //datatable.students.reload();
                });
            }
        },

        curriculum: {
            init: function () {
                $("#curriculum-select").select2({
                    placeholder: "Seleccione un plan",
                    disabled: true
                });

                $("#curriculum-select").on("change", function () {
                    var curriculumId = $("#curriculum-select").val();

                    if (curriculumId === _app.constants.guid.empty) {
                        select2.academicyear.empty();
                    } else {
                        select2.academicyear.load(curriculumId);
                    }

                    //datatable.students.reload();
                });
            },
            load: function (career) {
                $.ajax({
                    url: `/carreras/${career}/planestudio/get`.proto().parseURL(),
                }).done(function (data) {
                    $("#curriculum-select").empty();

                    //data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });

                    $("#curriculum-select").select2({
                        placeholder: "Seleccione un plan de estudios",
                        data: data.items,
                        disabled: false
                    });
                    $("#curriculum-select").trigger("change");
                });
            },
            empty: function () {
                $("#curriculum-select").empty();
                $("#curriculum-select").select2({
                    placeholder: "Seleccione un plan",
                    disabled: true
                });
            }
        },
        academicyear: {
            init: function () {
                $("#academicyear-select").select2({
                    placeholder: "Seleccione un ciclo",
                    disabled: true
                });

                $("#academicyear-select").on("change", function () {
                    //datatable.students.reload();
                });
            },
            load: function (curriculumid) {
                $.ajax({
                    url: `/planes-estudio/${curriculumid}/niveles/get`.proto().parseURL(),
                }).done(function (data) {
                    $("#academicyear-select").empty();

                    //data.items.unshift({ id: -1, text: "Todos" });

                    $("#academicyear-select").select2({
                        placeholder: "Seleccione un ciclo",
                        data: data.items,
                        disabled: false,
                        minimumResultsForSearch: -1
                    });
                });
            },
            empty: function () {
                $("#academicyear-select").empty();
                $("#academicyear-select").select2({
                    placeholder: "Seleccione un curso",
                    disabled: true
                });
            }
        }
    };

    var datatable = {
        students: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/situacion-academica/get".proto().parseURL(),
                    data: function (d) {
                        d.fid = $(".select2-faculties").val();
                        d.cid = $(".select2-careers").val();
                        d.tid = $(".select2-terms").val();
                        d.curriculumId = $("#curriculum-select").val();
                        d.year = $("#academicyear-select").val();
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
                        width: "250px"
                    },
                    {
                        title: "Orden de Mérito",
                        data: "order",
                        width: "120px"
                    },
                    {
                        title: "Prom. Ponderado",
                        data: "grade",
                        width: "120px"
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
            },
            load: function () {
                if (datatable.students.object == null) {
                    datatable.students.init();
                } else {
                    datatable.students.reload();
                }
            }
        }
    };

    //var events = {
    //    onDownloadExcel: function () {
    //        $("#download_excel").on("click", function () {
    //            var url = `/admin/situacion-academica/reporte-excel?termId=${$(".select2-terms").val()}` +
    //                `&facultyId=${$(".select2-faculties").val()}&careerId=${$(".select2-careers").val()}&academicOrder=${$(".select2-orders").val()}`;
    //            var $btn = $(this);
    //            $btn.addLoader();
    //            $.fileDownload(url, {
    //                httpMethod: 'GET', successCallback: function () {
    //                    $btn.removeLoader();
    //                    toastr.success("Archivo descargado satisfactoriamente", "Éxito");
    //                }
    //            });
    //        });
    //    },
    //    init: function () {
    //        this.onDownloadExcel();
    //    }
    //};

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.students.reload();
            });
        }
    };

    var events = {
        init: function () {
            $("#btn-apply").on('click', function () {
                var termId = $(".select2-terms").val();
                var facultyId = $(".select2-faculties").val();
                var careerId = $(".select2-careers").val();
                var curriculumId = $("#curriculum-select").val();
                var year = $("#academicyear-select").val();

                if (termId == null || termId == _app.constants.guid.empty
                    || facultyId == null || facultyId == _app.constants.guid.empty
                    || careerId == null || careerId == _app.constants.guid.empty
                    || curriculumId == null || curriculumId == _app.constants.guid.empty
                    || year == -1) {
                    toastr.error("Todos los campos son obligatorios", _app.constants.toastr.title.error);
                    return;
                } else {
                    datatable.students.load();
                }
            });


            $("#print_pdf").on("click", function () {
                var termId = $(".select2-terms").val();
                var facultyId = $(".select2-faculties").val();
                var careerId = $(".select2-careers").val();
                var curriculumId = $("#curriculum-select").val();
                var year = $("#academicyear-select").val();

                if (termId == null || termId == _app.constants.guid.empty
                    || facultyId == null || facultyId == _app.constants.guid.empty
                    || careerId == null || careerId == _app.constants.guid.empty
                    || curriculumId == null || curriculumId == _app.constants.guid.empty
                    || year == -1) {
                    toastr.error("Todos los campos son obligatorios", _app.constants.toastr.title.error);
                    return;
                } else {
                    var $btn = $(this);
                    $btn.addLoader();

                    $.fileDownload("/admin/situacion-academica/reporte-pdf".proto().parseURL(), {
                        httpMethod: 'GET',
                        data: {
                            fid: $(".select2-faculties").val(),
                            cid: $(".select2-careers").val(),
                            tid: $(".select2-terms").val(),
                            curriculumId: $("#curriculum-select").val(),
                            year: $("#academicyear-select").val(),
                            academicOrder: $(".select2-orders").val()
                        },
                        successCallback: function () {
                            $btn.removeLoader();
                            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                        }
                    });
                }                
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
    InitApp.init();
});