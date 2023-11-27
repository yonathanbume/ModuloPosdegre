var InitApp = function () {

    var datatable = {
        reports: {
            object: null,
            options: getSimpleDataTableConfiguration({
                url: "/admin/generacion-actas/get".proto().parseURL(),
                data: function (data) {
                    delete data.columns;
                    data.typeSearch = $("#cbx_filters").bootstrapSwitch('state') ? 1 : 2;
                    data.termId = $("#term_select").val();

                    //Por Docente
                    data.academicDepartmentId = $("#department_select").val();
                    data.teacherId = $("#teacher_select").val();
                    data.teacherCode = $("#teacher_code").val();

                    //Por Curso
                    data.careerId = $("#career_select").val();
                    data.curriculum = $("#curriculum_select").val();
                    data.courseSearch = $("#course_search").val();
                    data.academicYear = $("#academic_year_select").val();

                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        data: "career",
                        title: "Escuela"
                    },
                    {
                        data: "course",
                        title: "Curso"
                    },
                    {
                        data: "section",
                        title: "Sección",
                        width: 100
                    },
                    {
                        data: "teachers",
                        title: "Profesor(es)"
                    },
                    {
                        data: "printQuantity",
                        title: "Impresiones Realizadas",
                        width: 100
                    },
                    {
                        data: "status",
                        title: "Estado",
                        width: 100,
                        render: function (data) {
                            var status = {
                                "Recibido": { "title": "Recibido", "class": " m-badge--success" },
                                "Generado": { "title": "Generado", "class": " m-badge--warning" },
                                "Pendiente": { "title": "Pendiente", "class": " m-badge--metal" }
                            };
                            return '<span class="m-badge ' + status[data].class + ' m-badge--wide">' + status[data].title + "</span>";
                        }
                    },
                    {
                        data: null,
                        title: "Detalle",
                        render: function (data) {
                            var buttons = `<button data-id=${data.id} class="btn btn-primary btn-sm m-btn m-btn--icon btn-preview-details"><i class="la la-eye"></i></button> `;
                            return buttons;

                        }
                    },
                    {
                        data: null,
                        title: "Opciones",
                        orderable: false,
                        width: 220,
                        render: function (data) {
                            var buttons = ``;
                            var url = `/admin/generacion-actas/acta-final/${data.id}`.proto().parseURL();
                            var url2 = `/admin/generacion-actas/acta-final-detallada/${data.id}`.proto().parseURL();
                            var url3 = `/admin/generacion-actas/acta-final-registro/${data.id}`.proto().parseURL();

                            if (data.hasSustituteExams) {
                                buttons = `<button class="btn btn-primary btn-sm m-btn m-btn--icon btn-sustitute-exams" download><span><i class="la la-file-text-o"></i><span> Generar </span></span></button>`;
                            }
                            if (!data.wasGenerated) {
                                if (data.complete) {
                                    buttons = `<button data-url=${url} class="btn btn-primary btn-sm m-btn m-btn--icon btn-report" download><span><i class="la la-file-text-o"></i><span> Generar </span></span></button>`;
                                }
                                else {
                                    buttons = `<button data-url=${url} class="btn btn-primary btn-sm m-btn m-btn--icon btn-report-incomplete" download><span><i class="la la-file-text-o"></i><span> Generar </span></span></button>`;
                                }
                            } else {
                                buttons = `<button data-url=${url} class="btn btn-brand btn-sm m-btn m-btn--icon btn-report2" download><span><i class="la la-file-text-o"></i><span> Imprimir </span></span></button>`;
                                buttons += `<button data-url=${url3} class="ml-1 btn btn-brand btn-sm m-btn m-btn--icon btn-report-registerv2" download><span><i class="la la-file-text-o"></i><span> Registro</span></span></button>`;
                            }
                            if(data.isUNAMAD){
                                buttons += ` <button data-url=${url2} class="btn btn-brand btn-sm m-btn m-btn--icon btn-detailed-report" download><span><i class="la la-file-pdf"></i><span> Detallado </span></span></button>`;
                            }

                            return buttons;
                        }
                    }
                ]
            }),
            init: function () {
                this.object = $("#data-table").DataTable(this.options);

                $("#data-table")
                    .on("click", ".btn-report", function () {
                        var url = $(this).data("url");
                        var $btn = $(this);
                        $btn.addLoader();
                        $.fileDownload(url, {
                            httpMethod: 'GET', successCallback: function () {
                                $btn.removeLoader();
                                toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                datatable.reports.reload();
                            },
                            failCallback: function (responseHtml, url) {
                                $btn.removeLoader();
                                responseHtml = responseHtml.replace(`<pre style="word-wrap: break-word; white-space: pre-wrap;">`, "");
                                responseHtml = responseHtml.replace(`</pre>`, "");
                                toastr.error(responseHtml, "Error");
                            }
                        });
                    });

                $("#data-table")
                    .on("click", ".btn-report2", function () {
                        var url = $(this).data("url");
                        var $btn = $(this);
                        $btn.addLoader();
                        $.fileDownload(url, {
                            httpMethod: 'GET', 
                            successCallback: function () {
                                $btn.removeLoader();
                                toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                datatable.reports.reload();
                            },
                            failCallback: function (responseHtml, url) {
                                $btn.removeLoader();
                                responseHtml = responseHtml.replace(`<pre style="word-wrap: break-word; white-space: pre-wrap;">`, "");
                                responseHtml = responseHtml.replace(`</pre>`, "");
                                toastr.error(responseHtml, "Error");
                            }
                        });
                    });

                $("#data-table")
                    .on("click", ".btn-report-registerv2", function () {
                        var url = $(this).data("url");
                        var $btn = $(this);
                        $btn.addLoader();
                        $.fileDownload(url, {
                            httpMethod: 'GET',
                            successCallback: function () {
                                $btn.removeLoader();
                                toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                datatable.reports.reload();
                            },
                            failCallback: function (responseHtml, url) {
                                $btn.removeLoader();
                                responseHtml = responseHtml.replace(`<pre style="word-wrap: break-word; white-space: pre-wrap;">`, "");
                                responseHtml = responseHtml.replace(`</pre>`, "");
                                toastr.error(responseHtml, "Error");
                            }
                        });
                    });

                $("#data-table")
                    .on("click", ".btn-detailed-report", function () {
                        var url = $(this).data("url");
                        var $btn = $(this);
                        $btn.addLoader();
                        $.fileDownload(url, {
                            httpMethod: 'GET', successCallback: function () {
                                $btn.removeLoader();
                                toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                datatable.reports.reload();
                            },
                            failCallback: function (responseHtml, url) {
                                $btn.removeLoader();
                                responseHtml = responseHtml.replace(`<pre style="word-wrap: break-word; white-space: pre-wrap;">`, "");
                                responseHtml = responseHtml.replace(`</pre>`, "");
                                toastr.error(responseHtml, "Error");
                            }
                        });
                    });
                $("#data-table")
                    .on("click", ".btn-report-incomplete", function () {
                        var url = $(this).data("url");
                        var $btn = $(this);
                        swal({
                            title: "Evaluaciones sin notas registradas",
                            text: "Existen evaluaciones sin notas registradas, ¿desea generar el acta de todos modos?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Si",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            cancelButtonText: "Cancelar"
                        }).then(function (result) {
                            if (result.value) {
                                $btn.addLoader();

                                $.fileDownload(url, {
                                    httpMethod: 'GET', successCallback: function () {
                                        $btn.removeLoader();
                                        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                        datatable.reports.reload();
                                    },
                                    failCallback: function (responseHtml, url) {
                                        $btn.removeLoader();
                                        responseHtml = responseHtml.replace(`<pre style="word-wrap: break-word; white-space: pre-wrap;">`, "");
                                        responseHtml = responseHtml.replace(`</pre>`, "");
                                        toastr.error(responseHtml, "Error");
                                    }
                                });
                            }
                        });
                    });

                $("#data-table")
                    .on("click", ".btn-sustitute-exams", function () {
                        swal({
                            title: "Información",
                            text: "Existen examenes sustitorios pendientes, no se puede generar el acta.",
                            type: "warning",
                            confirmButtonText: "Aceptar",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        });
                    });

                $("#data-table")
                    .on('click', ".btn-preview-details", function () {
                        var id = $(this).data('id');    
                        $("#preview-datatable").html('');
                        $("#previewView").modal('show');

                        mApp.block("#preview-datatable");
      
                        $.ajax({
                            url: `/admin/generacion-actas/obtener-vista-previa/${id}`.proto().parseURL(),
                            type: "GET",
                            dataType: "html",
                            contextType: "application/json"                
                        })
                            .done(function (data) {
                                $("#preview-datatable").html(data);
                            })
                            .fail(function (data) {
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            }).always(function () {
                                mApp.unblock("#preview-datatable");
                            });
               
                    });

                $("#teacher_code").doneTyping(function () {
                    datatable.reports.reload();
                });

                $("#course_search").doneTyping(function () {
                    datatable.reports.reload();
                });
            },
            reload: function () {
                this.object.ajax.reload();
            }
        }        
    };

    var select2 = {
        academicYear: {
            init: function () {
                $("#academic_year_select").select2({
                    minimumResultsForSearch: -1,
                    placeholder: "Seleccione un ciclo",
                    disabled: true
                });
                select2.academicYear.events.init();
            },
            load: function (careerId, curriculumId) {
                $("#academic_year_select").empty();

                if (curriculumId === _app.constants.guid.empty || curriculumId === " " || curriculumId === undefined || curriculumId === null) {

                    if (careerId === _app.constants.guid.empty || careerId === " " || careerId === undefined || careerId == null) {
                        $("#academic_year_select").select2({
                            disabled: true,
                            placeholder: "Seleccionar..."
                        });
                    }
                    else {
                        $.ajax({
                            url: `/carrera/${careerId}/niveles/get`.proto().parseURL()
                        }).done(function (data) {
                            data.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                            $("#academic_year_select").select2({
                                minimumResultsForSearch: -1,
                                data: data.items,
                                disabled: false
                            });
                        });
                    }
                } else {
                    $.ajax({
                        url: `/planes-estudio/${curriculumId}/niveles/get`.proto().parseURL()
                    }).done(function (data) {
                        data.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                        $("#academic_year_select").select2({
                            minimumResultsForSearch: -1,
                            data: data.items,
                            disabled: false
                        });
                    });
                }

            },
            events: {
                onChange: function(){
                    $("#academic_year_select").on("change", function () {
                        datatable.reports.reload();
                    });
                },
                init: function () {
                    this.onChange();
                }
            }
        },
        career: {
            events: {
                load: function () {
                    $.ajax({
                        url: ("/carreras/get").proto().parseURL()
                    }).done(function (data) {
                        $("#career_select").select2({
                            data: data.items,
                        });
                    });

                },
                onChange: function () {
                    $("#career_select").on("change", function () {
                        var careerId = $(this).val();
                        select2.curriculum.events.load(careerId);
                        select2.academicYear.load(careerId, null);
                        datatable.reports.reload();
                    });
                },
                init: function () {
                    this.load();
                    this.onChange();
                }
            },
            init: function () {
                select2.career.events.init();
            }
        },
        curriculum: {
            events: {
                load: function (careerId) {
                    $("#curriculum_select").empty();
                    if (careerId === _app.constants.guid.empty || careerId === " " || careerId === undefined) {
                        $("#curriculum_select").select2({
                            disabled: true,
                            placeholder : "Seleccionar plan de estudio"
                        });
                    } else {
                        var termId = $("#term_select").val();
                        $.ajax({
                            url: (`/planes-estudio/${careerId}/activos/${termId}/get`).proto().parseURL()
                        }).done(function (data) {
                            data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });

                            $("#curriculum_select").select2({
                                data: data.items,
                                disabled: false
                            });
                        });
                    }
                },
                onChange: function () {
                    $("#curriculum_select").on("change", function () {
                        select2.academicYear.load(null, $(this).val());
                        datatable.reports.reload();
                    })
                },
                init: function () {
                    this.load();
                    this.onChange();
                }
            },
            init: function () {
                select2.curriculum.events.init();
            }
        },
        academicDepartments: {
            events: {
                load: function () {
                    $.ajax({
                        url: ("/departamentos-academicos/get").proto().parseURL()
                    }).done(function (data) {
                        $(".department-select").select2({
                            data: data
                        });
                    });

                },
                onChange: function () {
                    $(".department-select").on("change", function () {
                        var departmentId = $(this).val();
                        if (departmentId === _app.constants.guid.empty) {
                            $("#teacher_code").attr("disabled", true);
                            $("#teacher_select").attr("disabled", true);

                            select2.teacher.events.clean();
                            $("#teacher_code").val("");

                        } else {
                            $("#teacher_code").attr("disabled", false);
                            $("#teacher_select").attr("disabled", false);
                        }

                        datatable.reports.reload();
                    });
                },
                init: function () {
                    this.load();
                    this.onChange();
                }
            },
            init: function () {
                select2.academicDepartments.events.init();
            }
        },

        teacher: {
            events: {
                load: function () {
                    $("#teacher_select").select2({
                        ajax: {
                            delay: 1000,
                            url: (`/profesores/get/v2`).proto().parseURL(),
                            data: function (params) {
                                return {
                                    q: params.term,
                                    academicDepartmentId: $("#department_select").val()
                                };
                            }
                        },
                        allowClear: true,
                        minimumInputLength: 1,
                        placeholder: "Seleccione docente"
                    });
                },
                onChange: function () {
                    $("#teacher_select").on("change", function () {

                        datatable.reports.reload();

                        if ($(this).val() === "" || $(this).val() === null) {
                            $("#print_block_register").addClass("d-none");
                            $("#print_block").addClass("d-none");
                        } else {
                            $("#print_block_register").removeClass("d-none");
                            $("#print_block").removeClass("d-none");
                        }
                    });
                },
                clean: function () {
                    $("#teacher_select").val("").trigger("change");
                },
                init: function () {
                    select2.teacher.events.load();
                    select2.teacher.events.onChange();
                }
            },
            init: function () {
                select2.teacher.events.init();
            }
        },
        term: {
            events: {
                load: function () {
                    $.ajax({
                        url: ("/periodos/get").proto().parseURL()
                    }).done(function (data) {
                        $("#term_select").select2({
                            data: data.items
                        });

                        $("#term_select").val(data.selected).trigger("change");

                        $("#term_select").on("change", function () {
                            var careerId = $("#career_select").val();
                            select2.curriculum.events.load(careerId);
                            datatable.reports.reload();
                        });
                    });

                },
                init: function () {
                    this.load();
                }
            },
            init: function () {
                this.events.init();
            }
        },
        init: function () {
            select2.career.init();
            select2.teacher.init();
            select2.term.init();
            select2.curriculum.init();
            this.academicDepartments.init();
            select2.academicYear.init();
        }
    };

    var signature = {
        modal: $("#signature_modal"),
        imageUrl: null,
        hasChanged : false,
        form: {
            object: $("#form_signature"),
            validate: function () {
                signature.form.object.validate({
                    rules: {
                        Signature: {
                            required: true
                        }
                    },
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        signature.form.submit(formElement);
                    }
                });
            },
            submit: function (formElement) {
                var formData = new FormData($(formElement)[0]);
                signature.form.object.find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                $.ajax({
                    url: "/admin/generacion-actas/subir-firma",
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false
                })
                    .done(function (e) {
                        signature.modal.modal("hide");
                        swal({
                            type: "success",
                            title: "Completado",
                            text: "Firma actualizada satisfactoriamente.",
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
                    })
                    .always(function () {
                        signature.form.object.find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                    });
            },
            init: function () {
                this.validate();
            }
        },
        events: {
            getSignature: function () {
                signature.modal.find("button[type='submit']").attr("disabled", true);
                signature.modal.find(":input").attr("disabled", true);

                $.ajax({
                    url: "/admin/generacion-actas/get-firma",
                    type: "GET"
                })
                    .done(function (e) {
                        if (e !== null && e !== undefined) {
                            var url = `/imagenes/${e.urlSignature}`;
                            signature.imageUrl = url;
                            signature.modal.find(".custom-file-label").text(`C:\\fakepath\\firma.jpg`);
                            $("#img_signature").attr("src", url);
                        }
                        else {
                            signature.modal.find(".custom-file-label").text(``);
                            signature.modal.find("[name='file']").val("");
                            $("#img_signature").attr("src", "");
                        }
                    })
                    .always(function () {
                        signature.modal.find("button[type='submit']").attr("disabled", false);
                        signature.modal.find(":input").attr("disabled", false);
                    });
            },
            onChange: function () {
                signature.form.object.find("[name='file']").on("change", function () {
                    var input = $(this);
                    var oFReader = new FileReader();
                    var file = input[0].files[0];

                    if (file === null || file === undefined)
                        return;

                    oFReader.readAsDataURL(file);
                    oFReader.onload = function (oFREvent) {
                        $("#img_signature").attr("src", oFREvent.target.result);
                    };

                    signature.hasChanged = true;
                });
            },
            onHide: function () {
                signature.modal.on('hide.bs.modal', function (e) {
                    if (signature.hasChanged) {
                        signature.events.getSignature();
                        signature.hasChanged = false;
                    }
                });
            },
            init: function () {
                this.getSignature();
                this.onHide();
                this.onChange();
            }
        },
        init: function () {
            signature.form.init();
            signature.events.init();
        }
    };


    var events = {
        onDownloadBlock: function () {
            $("#print_block").on("click", function () {
                var textmessage =  "Solo se tomaran en cuenta las actas que posean evaluaciones registradas y no tengan ningún examen sustitorio pendiente. ¿Seguro que desea generar las actas de todos modos?";
                if($(this).hasClass("isUNAMAD")){
                   textmessage = "¿Seguro que desea generar las actas en bloque?"; 
                }
                //if (career === null || career === "" || career === undefined || career == _app.constants.guid.empty || career == "Todos") {
                //    toastr.error("Se debe seleccionar una escuela", "Error");
                //    return;
                //}

                var $btn = $(this);
                //var url = `/admin/generacion-actas/imprimir-bloque-acta-final/${career}`;
                var url = `/admin/generacion-actas/imprimir-bloque-acta-final/v2`;
                swal({
                    title: "Impresión del Bloque",
                    text: textmessage,
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Si",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar"
                }).then(function (result) {
                    if (result.value) {
                        $btn.addLoader();
                        $.fileDownload(url, {
                            httpMethod: 'GET',
                            data: {
                                teacherId: $("#teacher_select").val(),
                                termId: $("#term_select").val()
                            },
                            successCallback: function () {
                                $btn.removeLoader();
                                toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                datatable.reports.reload();
                            }
                        }).fail(function () {
                            $btn.removeLoader();
                            toastr.error("No se encontraron actas que cumplan con los requisitos indicados previamente.");
                        });
                    }
                });

            });

            $("#print_block_register").on("click", function () {
                var teacher = $("#teacher_select").val();
                var termId = $("#term_select").val();

                if (teacher === "" || termId === "") {
                    toastr.info("Es necesario seleccionar un docente.");
                    return;
                }

                var $btn = $(this);
                var url = `/admin/generacion-actas/acta-final-registro/bloque/${termId}/${teacher}`;
                swal({
                    title: "Impresión de Registro en Bloque",
                    text: "Solo se tomará en cuenta las actas emitidas.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Si",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar"
                }).then(function (result) {
                    if (result.value) {
                        $btn.addLoader();
                        $.fileDownload(url, {
                            httpMethod: 'GET',
                            successCallback: function () {
                                $btn.removeLoader();
                                toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                datatable.reports.reload();
                            }
                        }).fail(function () {
                            $btn.removeLoader();
                            toastr.error("No se encontraron actas que cumplan con los requisitos indicados previamente.");
                        });
                    }
                });

            });
        },
        onChangeFilters: function () {
            if ($("#cbx_filters").is(":checked")) {
                $(".bootstrap-switch-label").text("Por Curso");
            } else {
                $(".bootstrap-switch-label").text("Por Docente");
            }

            $("#cbx_filters").on("switchChange.bootstrapSwitch", function () {
                var value = $(this).bootstrapSwitch('state')

                if (value) {
                    $(".bootstrap-switch-label").text("Por Curso");
                    $("#div_by_teacher").removeClass("d-none");
                    $("#div_by_course").addClass("d-none");

                } else {
                    $(".bootstrap-switch-label").text("Por Docente");
                    $("#div_by_teacher").addClass("d-none");
                    $("#div_by_course").removeClass("d-none");
                }
                datatable.reports.reload();
            })
        },
        init: function () {
            events.onChangeFilters();
            events.onDownloadBlock();
        }
    };

    return {
        init: function () {
            select2.init();
            signature.init();
            datatable.reports.init();
            events.init();
        }
    };
}();

$(function () {
    InitApp.init();
});