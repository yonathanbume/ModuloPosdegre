var InitApp = function () {

    var datatable = {
        sections: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/gestion-duplicado-aula/get".proto().parseURL(),
                    type: "GET",
                    data: function (data) {
                        delete data.columns;
                        data.termId = $("#term-select").val();
                        data.careerId = $("#career-select").val();
                    }
                },
                columns: [
                    {
                        data: "career",
                        title: "Escuela profesional",
                        orderable: false
                    },
                    {
                        data: "academicYearA",
                        title: "Plan de estudio A",
                        orderable: false
                    },
                    {
                        data: "courseA",
                        title: "Curso A",
                        orderable: false
                    },
                    {
                        data: "sectionA",
                        title: "Sección A",
                        orderable: false
                    },
                    {
                        data: "academicYearB",
                        title: "Plan de estudio B",
                        orderable: false
                    },
                    {
                        data: "courseB",
                        title: "Curso B",
                        orderable: false
                    },
                    {
                        data: "sectionB",
                        title: "Sección B",
                        orderable: false
                    },
                    {
                        data: null,
                        title: "Opciones",
                        orderable: false,
                        width: "240px",
                        render: function (data) {
                            var template = "";
                            template += `<button data-idA="${data.sectionAId}" data-idB="${data.sectionBId}" class="btn btn-sm btn-danger delete"> <i class="fa fa-trash"></i> </button>`;
                            return template;
                        }
                    }
                ]
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);

                $('#data-table').on('click', '.delete', function (e) {
                    var sectionAid = $(this).data("ida");
                    var sectionBid = $(this).data("idb");
                    swal({
                        title: "¿Desea eliminar la sección?",
                        text: "Una vez eliminado no podrá recuperarlo",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Si, eliminar",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar"
                    }).then(function (result) {
                        if (result.value) {
                            mApp.blockPage();
                            $.ajax({
                                url: `/admin/gestion-duplicado-aula/eliminar/${sectionAid}/${sectionBid}`.proto().parseURL(),
                                type: "POST",
                            }).done(function () {
                                mApp.unblockPage();
                                datatable.sections.reload();

                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            }).fail(function () {
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            });

                        }
                    });
                });
            },
            reload: function () {
                if (this.object === null)
                    this.init();
                else
                    this.object.ajax.reload();
            }
        }
    };

    var validate = {
        add: function () {
            $("#add-form").validate({
                submitHandler: function (form) {
                    var btn = $(form).find('button[type="submit"]');
                    btn.addLoader();
                    $.ajax({
                        type: "POST",
                        url: `/admin/gestion-duplicado-aula/guardar`.proto().parseURL(),
                        data: $(form).serialize(),
                        success: function () {
                            $("#AddModal").modal('hide');
                            datatable.sections.reload();
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#add-form")[0].reset();
                        },
                        error: function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                        },
                        complete: function () {
                            btn.removeLoader();
                        }
                    });

                }
            });
        }
    };

    var select = {
        term: {
            init: function () {
                $.ajax({
                    url: "/periodos/get".proto().parseURL()
                }).done(function (data) {

                    $("#term-select").select2({
                        data: data.items
                    });

                    if (data.selected !== null) {
                        $("#term-select").val(data.selected);
                        $("#term-select").trigger("change.select2");
                    }

                    datatable.sections.init();

                    $("#term-select").on("change", function (e) {
                        datatable.sections.reload();
                    });
                });
            }
        },
        career: {
            init: function () {
                $.ajax({
                    url: "/carreras/get".proto().parseURL()
                }).done(function (data) {
                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $("#career-select").select2({
                        data: data.items
                    });

                    $("#career-select").on("change", function (e) {
                       datatable.sections.reload();
                    });
                });
            }
        },
        careers: {
            init: function () {
                $("#careerId_a").empty();
                $.ajax({
                    url: `/carreras/get`
                }).done(function (result) {
                    //result.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $("#careerId_a").select2({
                        //minimumResultsForSearch: -1,
                        data: result.items
                    });

                    $("#careerId_a").on("change", function () {

                        var id = $(this).val();

                        if (id === _app.constants.guid.empty) {
                            select.plansA.empty();
                            select.plansB.empty();
                        } else {
                            select.plansA.load($(this).val());
                            select.plansB.load($(this).val());
                        }
                    });

                    $("#careerId_a").trigger("change");
                });
            }
        },
        plansA: {
            init: function () {
                $("#academicYearAId_a").select2({
                    placeholder: "Seleccione un plan",
                    disabled: true
                });

                $("#academicYearAId_a").on("change", function () {
                    var id = $(this).val();

                    if (id === _app.constants.guid.empty) {
                        select.courseA.empty();
                    } else {
                        select.courseA.load($(this).val());
                    }
                });
                $("#academicYearAId_a").trigger("change");
            },
            load: function (careerId) {
                $("#academicYearAId_a").empty();
                $.ajax({
                    url: `/carreras/${careerId}/planestudio/get`.proto().parseURL()
                }).done(function (data) {

                    //data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });
                    $("#academicYearAId_a").select2({
                        data: data.items,
                        disabled: false
                    }).trigger("change");

                });
            },
            empty: function () {
                $("#academicYearAId_a").empty();
                $("#academicYearAId_a").select2({
                    placeholder: "Seleccione un plan",
                    disabled: true
                });
            }
        },
        courseA: {
            init: function () {
                $("#courseAId_a").select2({
                    placeholder: "Seleccione un Curso",
                    disabled: true
                });

                $("#courseAId_a").on("change", function () {
                    var id = $(this).val();

                    if (id === _app.constants.guid.empty) {
                        select.sectionA.empty();
                    } else {
                        select.sectionA.load($(this).val());
                    }
                });
                $("#courseAId_a").trigger("change");
            },
            load: function (curriculumId) {
                $("#courseAId_a").empty();
                $.ajax({
                    url: `/curriculum/${curriculumId}/cursos/get`.proto().parseURL()
                }).done(function (data) {

                    //data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });

                    $("#courseAId_a").select2({
                        data: data.items,
                        disabled: false
                    }).trigger("change");

                });
            },
            empty: function () {
                $("#courseAId_a").empty();
                $("#courseAId_a").select2({
                    placeholder: "Seleccione un Curso",
                    disabled: true
                });
            }
        },
        sectionA: {
            init: function () {
                $("#sectionAId_a").select2({
                    placeholder: "Seleccione una Seccion",
                    disabled: false
                });

                $("#sectionAId_a").on("change", function () {
                    var id = $(this).val();

                });
                $("#sectionAId_a").trigger("change");
            },
            load: function (curriculumId) {
                $("#sectionAId_a").empty();
                $.ajax({
                    url: `/cursos/${$("#courseAId_a").val()}/secciones/get`.proto().parseURL()
                }).done(function (data) {

                    //data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });

                    $("#sectionAId_a").select2({
                        data: data.items,
                        disabled: false
                    }).trigger("change");

                });
            },
            empty: function () {
                $("#sectionAId_a").empty();
                $("#sectionAId_a").select2({
                    placeholder: "Seleccione una sección",
                    disabled: false
                });
            }
        },
        plansB: {
            init: function () {
                $("#academicYearBId_a").select2({
                    placeholder: "Seleccione un plan",
                    disabled: true
                });

                $("#academicYearBId_a").on("change", function () {
                    var id = $(this).val();

                    if (id === _app.constants.guid.empty) {
                        select.courseB.empty();
                    } else {
                        select.courseB.load($(this).val());
                    }
                });
                $("#academicYearBId_a").trigger("change");
            },
            load: function (careerId) {
                $("#academicYearBId_a").empty();
                $.ajax({
                    url: `/carreras/${careerId}/planestudio/get`.proto().parseURL()
                }).done(function (data) {

                    //data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });
                    $("#academicYearBId_a").select2({
                        data: data.items,
                        disabled: false
                    }).trigger("change");

                });
            },
            empty: function () {
                $("#academicYearBId_a").empty();
                $("#academicYearBId_a").select2({
                    placeholder: "Seleccione un plan",
                    disabled: true
                });
            }
        },
        courseB: {
            init: function () {
                $("#courseBId_a").select2({
                    placeholder: "Seleccione un Curso",
                    disabled: true
                });

                $("#courseBId_a").on("change", function () {
                    var id = $(this).val();

                    if (id === _app.constants.guid.empty) {
                        select.sectionB.empty();
                    } else {
                        select.sectionB.load($(this).val());
                    }
                });
                $("#courseBId_a").trigger("change");
            },
            load: function (curriculumId) {
                $("#courseBId_a").empty();
                $.ajax({
                    url: `/curriculum/${curriculumId}/cursos/get`.proto().parseURL()
                }).done(function (data) {

                    //data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });

                    $("#courseBId_a").select2({
                        data: data.items,
                        disabled: false
                    }).trigger("change");

                });
            },
            empty: function () {
                $("#courseBId_a").empty();
                $("#courseBId_a").select2({
                    placeholder: "Seleccione un Curso",
                    disabled: true
                });
            }
        },
        sectionB: {
            init: function () {
                $("#sectionBId_a").select2({
                    placeholder: "Seleccione una Seccion",
                    disabled: false
                });

                $("#sectionBId_a").on("change", function () {
                    var id = $(this).val();

                });
                $("#sectionBId_a").trigger("change");
            },
            load: function (curriculumId) {
                $("#sectionBId_a").empty();
                $.ajax({
                    url: `/cursos/${$("#courseBId_a").val()}/secciones/get`.proto().parseURL()
                }).done(function (data) {

                    //data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });

                    $("#sectionBId_a").select2({
                        data: data.items,
                        disabled: false
                    }).trigger("change");

                });
            },
            empty: function () {
                $("#sectionBId_a").empty();
                $("#sectionBId_a").select2({
                    placeholder: "Seleccione una sección",
                    disabled: false
                });
            }
        },

        init: function () {
            this.term.init();
            this.career.init();
            this.careers.init();
            this.plansA.init();
            this.courseA.init();
            this.sectionA.init();
            this.plansB.init();
            this.courseB.init();
            this.sectionB.init();
        }
    };

    return {
        init: function () {
            select.init();
            validate.add();
        }
    };
}();

$(function () {
    InitApp.init();
});