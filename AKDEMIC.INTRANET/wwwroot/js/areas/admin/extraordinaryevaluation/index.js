var index = function () {
    var datatable = {
        evaluations: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/evaluacion-extraordinaria/get-evaluaciones-extraordinarias",
                    type: "GET",
                    data: function (data) {
                        data.searchValue = $("#search").val();
                        data.type = $("#Type_filter").val();
                    }
                },
                columns: [
                    {
                        title: "Código",
                        data: "courseCode"
                    },
                    {
                        title: "Curso",
                        data: "courseName"
                    },
                    {
                        title: "Escuela",
                        data: "career"
                    },
                    {
                        title: "Plan de Estudio",
                        data: "curriculum"
                    },
                    {
                        title: "N° mat.",
                        data: "students"
                    },
                    {
                        title: "Docente Presidente",
                        data: "teacher"
                    },
                    {
                        title: "Tipo",
                        data: "type"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        render: function (row) {
                            var tpm = "";
                            tpm += `<a href="/admin/evaluacion-extraordinaria/detalles/${row.id}" title="Gestionar" class="btn btn-info m-btn m-btn--icon btn-sm m-btn--icon-only"><i class="la la-cog"></i></a>`;
                            tpm += `<button data-id="${row.id}" title="Editar" class="ml-1 btn btn-primary m-btn m-btn--icon btn-sm m-btn--icon-only btn-edit"><i class="la la-edit"></i></button>`;
                            tpm += `<button data-id="${row.id}" title="Eliminar" class="ml-1 btn btn-danger m-btn m-btn--icon btn-sm m-btn--icon-only btn-delete"><i class="la la-trash"></i></button>`;
                            return tpm;
                        }
                    }
                ]
            },
            events: {
                onDelete: function () {
                    $("#datatable").on("click", ".btn-delete", function () {
                        var id = $(this).data("id");
                        swal({
                            type: "warning",
                            title: "Eliminará la evaluación seleccionada.",
                            text: "¿Seguro que desea eliminarla?.",
                            confirmButtonText: "Aceptar",
                            showCancelButton: true,
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise(() => {
                                    $.ajax({
                                        type: "POST",
                                        url: `/admin/evaluacion-extraordinaria/eliminar-evaluacion-extraordinaria?id=${id}`
                                    })
                                        .done(function (data) {
                                            datatable.evaluations.reload();
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: "Evaluación eliminada con éxito.",
                                                confirmButtonText: "Aceptar"
                                            });
                                        })
                                        .fail(function (e) {
                                            swal({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                                confirmButtonText: "Aceptar",
                                                text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                            });
                                        });
                                });
                            }
                        });
                    });
                },
                onEdit: function () {
                    $("#datatable").on("click", ".btn-edit", function () {
                        var id = $(this).data("id");
                        modal.evaluation.edit.events.show(id);
                    });
                },
                init: function () {
                    this.onEdit();
                    this.onDelete();
                }
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#datatable").DataTable(this.options);
            }
        },
        init: function () {
            this.evaluations.init();
            this.evaluations.events.init();
        }
    };

    var modal = {
        evaluation: {
            add: {
                object: $("#add_evaluation_modal"),
                form: {
                    object: $("#add_evaluation_form").validate({
                        submitHandler: function (formElement, e) {
                            e.preventDefault();
                            var fomData = new FormData(formElement);
                            $("#add_evaluation_form").find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                            $.ajax({
                                url: "/admin/evaluacion-extraordinaria/agregar-evaluacion-extraordinaria",
                                type: "POST",
                                data: fomData,
                                contentType: false,
                                processData: false
                            })
                                .done(function (e) {
                                    datatable.evaluations.reload();
                                    $("#add_evaluation_modal").modal("hide");
                                    swal({
                                        type: "success",
                                        title: "Hecho!",
                                        text: "Evaluación editada satisfactoriamente.",
                                        confirmButtonText: "Aceptar"
                                    }).then(function (isConfirm) {
                                        if (isConfirm) {
                                            window.location.href = `/admin/evaluacion-extraordinaria/detalles/${e}`;
                                        }
                                    });
                                })
                                .fail(function (e) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Aceptar",
                                        text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                    });
                                })
                                .always(function () {
                                    $("#add_evaluation_form").find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                                });
                        }
                    })
                },
                events: {
                    onHidden: function () {
                        modal.evaluation.add.object.on('hidden.bs.modal', function (e) {
                            modal.evaluation.add.form.object.resetForm();
                            select.courses.clear();
                            select.teacher.clear();
                            select.career.clear();
                            modal.evaluation.add.object.find("[name='ResolutionFile']").val(null);
                            modal.evaluation.add.object.find(".custom-file-label").text("Seleccionar archivo");
                            select.type.clear(modal.evaluation.add.object);
                            select.curriculum.clear();
                        });
                    },
                    init: function () {
                        this.onHidden();
                    }
                },
                init: function () {
                    this.events.init();
                }
            },
            edit: {
                object: $("#edit_evaluation_modal"),
                form: {
                    object: $("#edit_evaluation_form").validate({
                        submitHandler: function (formElement, e) {
                            e.preventDefault();
                            var fomData = new FormData(formElement);
                            $("#edit_evaluation_form").find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                            $.ajax({
                                url: "/admin/evaluacion-extraordinaria/editar-evaluacion-extraordinaria",
                                type: "POST",
                                data: fomData,
                                contentType: false,
                                processData: false
                            })
                                .done(function (e) {
                                    datatable.evaluations.reload();
                                    modal.evaluation.edit.object.modal("hide");
                                    swal({
                                        type: "success",
                                        title: "Hecho!",
                                        text: "Evaluación agregada satisfactoriamente.",
                                        confirmButtonText: "Aceptar"
                                    })
                                })
                                .fail(function (e) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Aceptar",
                                        text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                    });
                                })
                                .always(function () {
                                    $("#edit_evaluation_form").find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                                });
                        }
                    })
                },
                events: {
                    show: function (id) {
                        modal.evaluation.edit.object.modal("show");
                        modal.evaluation.edit.object.find(":input").attr("disabled", true);

                        $.ajax({
                            url: `/admin/evaluacion-extraordinaria/get-evaluacion-extraordinaria?id=${id}`
                        })
                            .done(function (e) {
                                $("#edit_file_url").addClass("d-none");
                                modal.evaluation.edit.object.find("[name='Id']").val(e.id);
                                modal.evaluation.edit.object.find("[name='Course']").val(e.course);

                                if (e.teacherId != null) {
                                    select.teacher.setValue(modal.evaluation.edit.object.find("[name='TeacherId']"), e.teacher, e.teacherId);
                                } else {
                                    modal.evaluation.edit.object.find("[name='TeacherId']").val(null).trigger("change");
                                }

                                select.teacher.setValues(modal.evaluation.edit.object.find("[name='Committee']"), e.committee);
                                modal.evaluation.edit.object.find("[name='Resolution']").val(e.resolution);
                                modal.evaluation.edit.object.find(":input").attr("disabled", false);

                                modal.evaluation.edit.object.find("[name='Type']").val(e.type).trigger("change");

                                if (e.resolutionFileUrl !== null && e.resolutionFileUrl !== undefined) {
                                    $("#edit_file_url").removeClass("d-none");
                                    modal.evaluation.edit.object.find("[name='FileUrl']").attr("href", `/documentos/${e.resolutionFileUrl}`)
                                }
                            });
                    },
                    onHidden: function () {
                        modal.evaluation.edit.object.on('hidden.bs.modal', function (e) {
                            modal.evaluation.edit.form.object.resetForm();
                            modal.evaluation.edit.object.find("[name='ResolutionFile']").val(null);
                            modal.evaluation.edit.object.find(".custom-file-label").text("Seleccionar archivo");

                        });
                    },
                    init: function () {
                        this.onHidden();
                    }
                },
                init: function () {
                    this.events.init();
                }
            }
        },
        init: function () {
            this.evaluation.add.init();
            this.evaluation.edit.init();
        }
    };

    var select = {
        courses: {
            load: function (curriculumId) {
                modal.evaluation.add.object.find("[name='CourseId']").empty();

                if (curriculumId === null) {
                    modal.evaluation.add.object.find("[name='CourseId']").select2({
                        placeholder: "Seleccionar curso",
                        dropdownParent: $('#add_evaluation_form')
                    }).trigger("change");
                } else {
                    $.ajax({
                        url: `/curriculum/${curriculumId}/cursos/get`,
                        type: "get"
                    })
                        .done(function (e) {
                            modal.evaluation.add.object.find("[name='CourseId']").select2({
                                data: e.items,
                                placeholder: "Seleccionar curso",
                                dropdownParent: $('#add_evaluation_form')
                            }).trigger("change");
                        });
                }
            },
            clear: function () {
                modal.evaluation.add.object.find("[name='CourseId']").val(null).trigger("change");
            },
            init: function () {
                modal.evaluation.add.object.find("[name='CourseId']").select2({
                    placeholder: "Seleccionar cursos",
                    dropdownParent: $('#add_evaluation_form')
                });
            }
        },
        teacher: {
            load: function () {
                modal.evaluation.add.object.find("[name='TeacherId']").select2({
                    ajax: {
                        delay: 300,
                        url: (`/profesores/get`).proto().parseURL(),
                        processResults: function (data) {
                            return {
                                results: data.items.results,
                                pagination : data.items.pagination
                            };
                        },
                        cache: true
                    },
                    allowClear: true,
                    minimumInputLength: 2,
                    placeholder: "Seleccione profesor",
                    dropdownParent: $('#add_evaluation_form')
                });

                modal.evaluation.add.object.find("[name='Committee']").select2({
                    ajax: {
                        delay: 300,
                        url: (`/profesores/get`).proto().parseURL(),
                        processResults: function (data) {
                            return {
                                results: data.items.results,
                                pagination: data.items.pagination
                            };
                        },
                        cache: true
                    },
                    allowClear: true,
                    minimumInputLength: 1,
                    placeholder: "Seleccione el comité",
                    dropdownParent: $('#add_evaluation_form')
                });

                modal.evaluation.edit.object.find("[name='TeacherId']").select2({
                    ajax: {
                        delay: 300,
                        url: (`/profesores/get`).proto().parseURL(),
                        processResults: function (data) {
                            return {
                                results: data.items.results,
                                pagination: data.items.pagination
                            };
                        },
                        cache: true
                    },
                    allowClear: true,
                    minimumInputLength: 2,
                    placeholder: "Seleccione profesor",
                    dropdownParent: $('#edit_evaluation_form')
                });

                modal.evaluation.edit.object.find("[name='Committee']").select2({
                    ajax: {
                        delay: 300,
                        url: (`/profesores/get`).proto().parseURL(),
                        processResults: function (data) {
                            return {
                                results: data.items.results,
                                pagination: data.items.pagination
                            };
                        },
                        cache: true
                    },
                    allowClear: true,
                    minimumInputLength: 1,
                    placeholder: "Seleccione el comité",
                    dropdownParent: $('#edit_evaluation_form')
                });
            },
            setValue: function (object, text, id) {
                if (object.find("option[value='" + id + "']").length) {
                    object.val(id).trigger('change');
                } else {
                    var newOption = new Option(text, id, true, true);
                    object.append(newOption).trigger('change');
                } 
            },
            setValues: function (object,data) {
                var values = [];
                $.each(data, function (i, value) {
                    if (!object.find("option[value='" + value.id + "']").length) {
                        var newOption = new Option(value.text, value.id);
                        object.append(newOption);
                    } 
                    values.push(value.id);
                });
                object.val(values).trigger("change");
            },
            clear: function () {
                modal.evaluation.add.object.find("[name='TeachersCommitee']").val(null).trigger("change");
                modal.evaluation.add.object.find("[name='TeacherId']").val(null).trigger("change");
            },
            init: function () {
  
                select.teacher.load();
            }
        },
        career: {
            load: function () {
                $.ajax({
                    url: (`/carreras/v3/get`).proto().parseURL(),
                    type: "get"
                })
                    .done(function (e) {
                        modal.evaluation.add.object.find("[name='CareerId']").select2({
                            data: e.items,
                            allowClear: true,
                            placeholder: "Seleccionar Escuela",
                            dropdownParent: $('#add_evaluation_form')
                        }).trigger("change");
                    });
                //modal.evaluation.add.object.find("[name='CareerId']").select2({
                //    ajax: {
                //        delay: 300,
                //        url: (`/carreras/v3/get`).proto().parseURL(),
                //    },
                //    allowClear: true,
                //    placeholder: "Seleccione Escuela",
                //    dropdownParent: $('#add_evaluation_form'),
                //    processResults: function (data) {
                //        return {
                //            results: data.items
                //        };
                //    }
                //});
            },
            events: {
                onChange: function () {
                    modal.evaluation.add.object.find("[name='CareerId']").on("change", function () {
                        var id = $(this).val();
                        select.curriculum.load(id);
                    });
                },
                init: function () {
                    this.onChange();
                }
            },
            clear: function () {
                modal.evaluation.add.object.find("[name='CareerId']").val(null).trigger("change");
            },
            init: function () {
                select.career.load();
                select.career.events.init();
            }
        },
        curriculum: {
            load: function (careerId) {
                modal.evaluation.add.object.find("[name='CurriculumId']").empty();

                if (careerId === null) {
                    modal.evaluation.add.object.find("[name='CurriculumId']").select2({
                        placeholder: "Seleccionar plan de estudio",
                        dropdownParent: $('#add_evaluation_form')
                    }).trigger("change");
                } else {
                    $.ajax({
                        url: `/carreras/${careerId}/planestudio/get`,
                        type: "get"
                    })
                        .done(function (e) {
                            modal.evaluation.add.object.find("[name='CurriculumId']").select2({
                                data: e.items,
                                placeholder: "Seleccionar plan de estudio",
                                dropdownParent: $('#add_evaluation_form')
                            }).trigger("change");
                        });
                }
            },
            clear: function () {
                modal.evaluation.add.object.find("[name='CurriculumId']").empty();
                modal.evaluation.add.object.find("[name='CurriculumId']").select2({
                    placeholder: "Seleccionar plan de estudio",
                    dropdownParent: $('#add_evaluation_form')
                });
            },
            events: {
                onChange: function () {
                    modal.evaluation.add.object.find("[name='CurriculumId']").on("change", function () {
                        var id = $(this).val();
                        select.courses.load(id);
                    });
                },
                init: function () {
                    this.onChange();
                }
            },
            init: function () {
                modal.evaluation.add.object.find("[name='CurriculumId']").select2({
                    placeholder: "Seleccionar plan de estudio"
                });
                this.events.init();
            }
        },
        type: {
            load: function () {
                $("[name='Type']").select2({
                    minimumResultsForSearch: -1,
                    placeholder: "Seleccionar tipo"
                });


                $("#add_evaluation_modal").find("[name='Type']").on("change", function () {
                    var value = $(this).val();
                    if (value == "1" || value == null) {
                        $(".add_select_teacherId").text("Seleccione el Profesor Presidente");
                        $(".add_select_committee").text("Seleccione el comité");
                    } else {
                        $(".add_select_teacherId").text("Seleccione el Profesor Principal");
                        $(".add_select_committee").text("Seleccione profesores complementarios");
                    }
                })

                $("#Type_filter").select2({
                    minimumResultsForSearch: -1,
                    placeholder: "Seleccionar tipo"
                });
            },
            clear: function (object) {
                object.find("[name='Type']").val(null).trigger("change");
            },
            events: {
                onChange: function () {
                    $("#Type_filter").on("change", function () {
                        datatable.evaluations.reload();
                    });
                },
                init: function () {
                    this.onChange();
                }
            },
            init: function () {
                this.load();
                this.events.init();
            }
        },
        init: function () {
            this.career.init();
            this.curriculum.init();
            this.teacher.init();
            this.courses.init();
            this.type.init();
        }
    };

    var events = {
        onSearch: function () {
            $("#search").doneTyping(function () {
                datatable.evaluations.reload();
            });
        },
        init: function () {
            this.onSearch();
        }
    };

    return {
        init: function () {
            datatable.init();
            select.init();
            modal.init();
            events.init();
        }
    };
}();

$(() => {
    index.init();
});