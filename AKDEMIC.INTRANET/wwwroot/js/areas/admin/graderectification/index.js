var InitApp = function () {

    var createForm = null;

    var datatable = {
        requests: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/correccion-rectificacion-notas/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        data: "term",
                        title: "Periodo"
                    },
                    {
                        data: "teacher",
                        title: "Docente"
                    },
                    {
                        data: "course",
                        title: "Curso"
                    },
                    {
                        data: "code",
                        title: "Código"
                    },
                    {
                        data: "student",
                        title: "Estudiante"
                    },
                    {
                        data: "evaluation",
                        title: "Evaluación"
                    },
                    {
                        data: null,
                        title: "Nota Ant.",
                        render: function (data, type, row) {
                            var result = row.gradePrevious

                            return result;
                        }
                    },
                    {
                        data: null,
                        title: "Nota Nue.",
                        render: function (data, type, row) {
                            var result = data.gradeNew != 0 ? data.gradeNew : "--"

                            return result;
                        }
                    },
                    {
                        data: "state",
                        title: "Estado",
                    }
                ]
            },
            init: function () {
                this.object = $("#ajax_data").DataTable(this.options);

                //this.object.on("click", ".btn-detail", function () {
                //    var object = $(this).data("object");
                //    object = object.proto().decode();

                //    $("#observations").val(object.observations);

                //    $("#prev-grade").val(object.prevGrade);
                //    $("#new-grade").val(object.grade);
                //    $("#date").val(object.date);
                //    $("#teacher").val(object.teacher);

                //    if (object.file != null && object.file != "") {
                //        var url = `/admin/correccion-notas/archivo/${object.file}`.proto().parseURL();
                //        $("#file-url").prop("href", url);
                //        $("#file-container").show();
                //    }
                //    else {
                //        $("#file-container").hide();
                //    }

                //    $("#request_modal").modal("show");
                //});
            },
            reload: function () {
                this.object.ajax.reload();
            }
        }
    };


    var form = {
        create: {
            submit: function (formElement) {
                var data = $(formElement).serialize();
                $("#create-form").find(".btn-submit").addLoader();
                $("#create-form").attr("disabled", true);
                $.ajax({
                    contentType: false,
                    processData: false,
                    data: data,
                    type: "POST",
                    url: "/admin/correccion-rectificacion-notas/crear/post".proto().parseURL()
                })
                    .always(function () {
                        $("#create-form").find(".btn-submit").removeLoader();
                        $("#create-form select").attr("disabled", false);
                    })
                    .done(function () {
                        toastr.success(_app.constants.toastr.message.success.task,
                            _app.constants.toastr.title.success);
                        $("#request_modal").modal("hide");
                        datatable.requests.reload();
                    })
                    .fail(function (e) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (e.responseText != null) $("#createAlertText").html(e.responseText);
                        else $("#createAlertText").html(_app.constants.ajax.message.error);
                        $("#createAlert").removeClass("m--hide").show();
                    });
            }
        },
        
    };

    var event = {
        init: function () {
            $("#request_modal").on("hidden.bs.modal", function () {
                form.reset.tutors();
            });

            $(".type1").hide();
            $(".type").hide();
            $(".select2-evaluation").prop('required', true);

            $('input[type=radio][name=Type]').change(function () {
                if (this.value == '1') {
                    $("#SectionId").empty();
                    select.course.init(this.value);
                    $(".type").show();
                    $(".type1").show();
                    $(".select2-evaluation").prop('required', true);
                }
                else if (this.value == '2') {
                    $("#SectionId").empty();
                    select.course.init(this.value);
                    $(".type1").hide();
                    $(".type").show();
                    $(".select2-evaluation").prop('required', false);
                }
            });
        }
    }

    var select = {
        init: function () {
            this.term.init();
        },
        term: {
            init: function () {
                $.ajax({
                    url: "/periodos-pasados/get".proto().parseURL()
                }).done(function (data) {
                    $("#TermId").select2({
                        data: data.items
                    });

                    $("#TermId").on("change", function () {
                        if ($("#TermId").val() !== null) {
                            $(".select2-teachers").empty();
                            $("#SectionId").empty();
                            $("#EvaluationId").empty();

                            $('input[type=radio][name=Type]').prop('checked', false);
                            $(".type1").hide();
                            $(".type").hide();
                            $(".select2-evaluation").prop('required', true);

                            select.teacher.init();
                        }
                    });

                    $("#TermId").trigger("change");
                }).fail(function () {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            }
        },
        teacher: {
            init: function () {
                select.teacher.load();
                select.teacher.onChange();
            },
            load: function () {
                $(".select2-teachers").select2({
                    ajax: {
                        data: function (params) {
                            console.log(params);
                            var data = {
                                q: params.term || params.q || "",
                                query: params.q || params.term || "",
                                type: params._type,
                                page: params.page || 1
                            };
                            return data;
                        },
                        delay: 300,
                        type: "GET",
                        url: `/periodo/${$("#TermId").val()}/profesor/get`.proto().parseURL()
                    },
                    placeholder: "Seleccione Docente",
                    allowClear: true,
                    minimumInputLength: 3,
                    dropdownParent: $(".select2-teachers").closest(".modal-body"),
                });
            },
            onChange: function () {
                $(".select2-teachers").on("change", function () {
                    if ($(".select2-teachers").val() !== null) {
                        $("#SectionId").empty();
                        $('input[type=radio][name=Type]').prop('checked', false);
                        $(".type1").hide();
                        $(".type").hide();
                        $(".select2-evaluation").prop('required', true);
                    }
                });

                $(".select2-teachers").trigger("change");
            },
            clean: function () {
                $(".select2-teachers").val("").trigger("change");
            }
        },
        course: {
            init: function (type) {
                $.ajax({
                    url: `/cursos/docente/${$("#TeacherId").val()}/periodo/${$("#TermId").val()}/tipo/${type}/get`.proto().parseURL(),
                    
                }).done(function (data) {
                    $("#SectionId").select2({
                        data: data,
                        placeholder: "Seleccione Curso",
                        dropdownParent: $("#SectionId").closest(".modal-body"),
                    });

                    $("#SectionId").on("change", function () {
                        if ($("#SectionId").val() !== null) {
                            $("#EvaluationId").empty();

                            if (type == 1)
                                select.evaluation.init();
                            else
                                select.studentEvaluation.init();
                        }
                    });

                    $("#SectionId").trigger("change");
                }).fail(function () {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            }
        },
        evaluation: {
            init: function () {
                $.ajax({
                    url: `/cursos/${$("#SectionId").val()}/periodo/${$("#TermId").val()}/evaluacion/get`.proto().parseURL(),

                }).done(function (data) {
                    $("#EvaluationId").select2({
                        data: data,
                        placeholder: "Seleccione Evaluación",
                        dropdownParent: $("#EvaluationId").closest(".modal-body"),
                    });

                    $("#EvaluationId").on("change", function () {
                        if ($("#EvaluationId").val() !== null) {
                            $("#StudentIds").empty()
                            select.studentEvaluation.init();
                        }
                    });
                    $("#EvaluationId").trigger("change");

                }).fail(function () {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            }
        },
        studentEvaluation: {
            init: function () {
                select.studentEvaluation.load();
                select.studentEvaluation.onChange();
            },
            load: function () {
                $("#StudentIds").select2({
                    ajax: {
                        data: function (params) {
                            console.log(params);
                            var data = {
                                q: params.term || params.q || "",
                                query: params.q || params.term || "",
                                type: params._type,
                                page: params.page || 1
                            };
                            return data;
                        },
                        delay: 300,
                        type: "GET",
                        url: `/alumnos/seccion/${$("#SectionId").val()}/evaluacion/${$("#EvaluationId").val()}/get`.proto().parseURL()
                    },
                    placeholder: "Seleccione Alumno",
                    allowClear: true,
                    dropdownParent: $("#StudentIds").closest(".modal-body"),
                });
            },
            onChange: function () {
                $("#StudentIds").on("change", function () {
                    if ($("#StudentIds").val() !== null) {

                    }
                });

                $("#StudentIds").trigger("change");
            },
            clean: function () {
                $("#StudentIds").val("").trigger("change");
            }
        }
    };

    var validate = {
        init: function () {
            createForm = $("#create-form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.create.submit(formElement);
                }
            });
        }
    };

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.requests.reload();
            });
        }
    }

    return {
        init: function () {
            select.init();
            event.init();
            search.init();
            validate.init();
            datatable.requests.init();
        },
        reloadTable: function () {
            datatable.requests.reload();
        }
    };
}();

$(function () {
    InitApp.init();
});

