var InitApp = function () {

    var AcademicYear = $("#AcademicYear").val();
    var CourseId = $("#CourseId").val();
    var CurriculumId = $("#CurriculumId").val();
    var TermId = $("#TermId").val();

    var datatable = {
        assistance: {
            object: null,
            options: {
                serverSide: false,
                ajax: {
                    url: `/admin/reporte-docentes/detalle/asistencia/get`.proto().parseURL(),
                    type: "GET",
                    data: function (data) {
                        data.sectionId = $("#section_select2").val();
                    }
                },
                columns: [
                    {
                        data: "student",
                        title: "Estudiante"
                    },
                    {
                        data: "absences",
                        title: "Faltas"
                    },
                    {
                        data: "assisted",
                        title: "Asistidas"
                    },
                    {
                        data: "dictated",
                        title: "Dictadas"
                    }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Excel',
                        action: function (e, dt, node, config) {
                            window.location.href = `/admin/reporte-docentes/seccion/${$("#section_select2").val()}/control-asistencia-excel`.proto().parseURL();
                            //window.location.href = `/admin/reporte-docentes/detalle/${$("#section_select2").val()}/asistencia/excel`.proto().parseURL();
                        }
                    },
                    {
                        text: 'Pdf',
                        action: function (e, dt, node, config) {
                            window.location.href = `/admin/reporte-docentes/detalle/${$("#section_select2").val()}/asistencia/pdf?teacherId=${$("#TeacherId").val()}`.proto().parseURL();
                        }
                    }
                ]
            },
            reload: function () {
                if ($("#section_select2").val() !== null) {
                    if (this.object === null) {
                        datatable.assistance.object = $("#assistance-table").DataTable(this.options);
                    } else {
                        datatable.assistance.object.ajax.reload();
                    }
                }
            }
        },
        grade: {
            reload: function () {
                $("#grades_partial").html("");
                mApp.block("#grades_partial", {
                    message: "Cargando datos..."
                });

                $.ajax({
                    url: `/admin/reporte-docentes/detalle/notas/get?sectionId=${$('#section_select2').val()}`,
                    type: "GET",
                    dataType: "HTML"
                })
                    .done(function (e) {
                        $("#grades_partial").html(e);
                    });
            }
        }
    };

    var select2 = {
        academicyear: {
            load: function () {
                $.ajax({
                    url: `/planes-estudio/${CurriculumId}/niveles/get`,
                    type: "GET"
                })
                    .done(function (data) {
                        $("#academicyear-select").select2({
                            placeholder: "Seleccionar ciclo",
                            data: data.items
                        });

                        $("#academicyear-select").val(AcademicYear).trigger("change");
                        console.log(AcademicYear);

                        select2.academicyear.events();
                    });
            },
            events: function () {
                $("#academicyear-select").on("change", function () {
                    var year = $(this).val();
                    select2.course.load(year);
                });
            },
            init: function () {
                this.load();
            }
        },
        course: {
            init: function () {
                $.ajax({
                    url: `/planes-estudio/${CurriculumId}/niveles/${AcademicYear}/get`,
                    type: "GET"
                })
                    .done(function (data) {
                        $("#course_select2").select2({
                            placeholder: "Seleccionar cursos",
                            data: data.items
                        });

                        $("#course_select2").val(CourseId).trigger("change");

                        select2.course.events();
                    });
            },
            load: function (year) {
                $.ajax({
                    url: `/planes-estudio/${CurriculumId}/niveles/${year}/get`,
                    type: "GET"
                })
                    .done(function (data) {
                        $("#course_select2").empty();
                        $("#course_select2").select2({
                            placeholder: "Seleccionar cursos",
                            data: data.items
                        }).trigger("change");
                    });
            },
            events: function () {
                $("#course_select2").on("change", function () {
                    var id = $(this).val();
                    select2.sections.load(id);
                });
            }
        },
        sections: {
            init: function () {
                this.events.init();
                this.load(CourseId);
            },
            load: function (courseId) {
                $("#section_select2").empty();

                $.ajax({
                    url: `/cursos/${courseId}/secciones/get?termId=${TermId}`,
                    type: "GET"
                })
                    .done(function (data) {
                        $("#section_select2").select2({
                            placeholder: "Seleccionar secciones",
                            data: data.items
                        }).trigger("change");
                    });
            },
            events: {
                onChange: function () {
                    $("#section_select2").on("change", function () {
                        var value = $(this).val();
                        if (value !== null && value !== "") {
                            datatable.assistance.reload();
                            datatable.grade.reload();
                        }
                    });
                },
                init: function () {
                    this.onChange();
                }
            }
        },
        init: function () {
            select2.academicyear.init();
            select2.course.init();
            select2.sections.init();
        }
    };



    var events = {
        firstTime: true,
        cleanGrades: function () {
            $("#clean_grades").on("click", function () {
                var sectionId = $("#section_select2").val();
                var course = $("#course_select2").select2('data')[0].text;
                var section = $("#section_select2").text();

                console.log(course);
                console.log(section);

                swal({
                    type: "warning",
                    text: `¿Seguro que desea eliminar las asistencias del curso ${course} - ${section}? Esta acción no es recuperable.`,
                    title: `Eliminar asistencias`,
                    confirmButtonText: "Aceptar",
                    showCancelButton: true,
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise(() => {
                            $.ajax({
                                type: "POST",
                                url: `/admin/reporte-docentes/detalle/${sectionId}/limpiar-asistencias`
                            })
                                .done(function (e) {
                                    datatable.assistance.reload();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: e,
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
                        });
                    }
                });
            })
        },
        init: function () {
            this.cleanGrades();
        }
    };

    return {
        init: function () {
            select2.init();
            events.init();
        }
    };
}();

$(function () {
    InitApp.init();
});