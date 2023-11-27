var InitApp = function () {

    var datatable = {
        students: {
            object: null,
            options: getSimpleDataTableConfiguration({
                url: null,
                data: function (data) {
                    data.search = $("#search").val();
                    data.term = $("#select_term").val();
                    data.faculty = $("#faculty_select").val();
                    data.career = $("#career_select").val();
                },
                pageLength: 10,
                columns: [
                    {
                        data: "code"
                    },
                    {
                        data: "name"
                    },
                    {
                        data: "faculty"
                    },
                    {
                        data: "career"
                    },
                    {
                        data: "academicYear"
                    },
                    {
                        data: null,
                        render: function (data) {
                            var url = `/admin/notas/detalle/${data.id}`.proto().parseURL();
                            //return `<button class="btn btn-secondary btn-sm m-btn m-btn--icon btn-detail" data-id="${data}"><span><i class="la la-edit"></i><span> Ver Detalle </span></span></a>`;
                            return `<a class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail" href="${url}"><span><i class="la la-edit"></i><span> Ver Detalle </span></span></a>`;
                        }
                    }
                ]
            }),
            load: function () {
                console.log(datatable.students.options.ajax.data);
                if (datatable.students.object !== undefined && datatable.students.object !== null) {
                    var url = `/admin/notas/alumnos/get`.proto().parseURL();
                    datatable.students.object.ajax.url(url).load();
                } else {
                    datatable.students.options.ajax.url = `/admin/notas/alumnos/get`.proto().parseURL();
                    datatable.students.object = $("#students_table").DataTable(datatable.students.options);
                }
            },
            init: function (term) {
                datatable.students.options.ajax.url = `/admin/notas/alumnos/get`.proto().parseURL();
                datatable.students.object = $("#students_table").DataTable(datatable.students.options);

                $("#students_table").on("click", ".btn-detail", function (e) {
                    console.log("test");
                    var id = $(this).data("id");
                    //form.edit.load(id);


                });
            }
        },
        grades: {
            object: null,
            options: getSimpleDataTableConfiguration({
                url: null,
                data: function (data) {
                    data.search = $("#search").val();
                    data.term = $("#select_term").val();
                    data.faculty = $("#faculty_select").val();
                    data.career = $("#career_select").val();
                },
                pageLength: 10,
                columns: [
                    {
                        data: "code"
                    },
                    {
                        data: "name"
                    },
                    {
                        data: "faculty"
                    },
                    {
                        data: "career"
                    },
                    {
                        data: "academicYear"
                    },
                    {
                        data: "id",
                        render: function (data) {
                            return `<button class="btn btn-secondary btn-sm m-btn m-btn--icon btn-detail" data-id="${data}"><span><i class="la la-edit"></i><span> Ver Detalle </span></span></a>`;
                        }
                    }
                ]
            }),
            load: function () {
                console.log(datatable.students.options.ajax.data);
                if (datatable.students.object !== undefined && datatable.students.object !== null) {
                    var url = `/admin/notas/alumnos/get`.proto().parseURL();
                    datatable.students.object.ajax.url(url).load();
                } else {
                    datatable.students.options.ajax.url = `/admin/notas/alumnos/get`.proto().parseURL();
                    datatable.students.object = $("#students_table").DataTable(datatable.students.options);
                }
            }
        }
    }
    var select = {
        term: {
            init: function () {
                $.ajax({
                    url: ("/periodos/get").proto().parseURL()
                }).done(function (data) {
                    $("#select_term").select2({
                        data: data.items
                    });

                    if (data.selected !== null) $("#select_term").val(data.selected).trigger("change.select2");

                    datatable.students.init($("#select_term").val());
                    select.term.events();
                });
            },
            events: function () {
                $("#select_term").on("change", function (e) {
                    var term = $("#select_term").val();

                    datatable.students.load(term);
                });
            }
        },
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
                    var term = $("#select_term").val();
                    var faculty = $("#faculty_select").val();

                    if (faculty === _app.constants.guid.empty) {
                        datatable.students.load(term);

                        $("#career_select").empty();
                        $("#career_select").select2({
                            placeholder: "Seleccione una facultad",
                            disabled: true
                        });

                    } else {
                        datatable.students.load(term, faculty);
                        select.career.load($("#faculty_select").val());
                    }
                });
            }
        },
        career: {
            init: function () {
                $("#career_select").select2({
                    placeholder: "Seleccione una escuela",
                    disabled: true
                });
                select.career.events();
            },
            load: function (faculty) {
                $.ajax({
                    url: (`/facultades/${faculty}/carreras/v2/get`).proto().parseURL()
                }).done(function (data) {
                    $("#career_select").empty();
                    $("#career_select").select2({
                        placeholder: "Seleccione una escuela",
                        data: data.items,
                        minimumResultsForSearch: -1,
                        disabled: false
                    });
                });
            },
            events: function () {
                $("#career_select").on("change", function () {
                    console.log("hit select");
                    var term = $("#select_term").val();
                    var faculty = $("#faculty_select").val();
                    var career = $("#career_select").val();

                    if (career === _app.constants.guid.empty) datatable.students.load(term, faculty);
                    else datatable.students.load(term, faculty, career);
                });


            }
        },
        init: function () {
            select.term.init();
            select.faculty.init();
            select.career.init();
        }
    }
    var inputs = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.students.load();
            });
        }
    }

    var detail = {
        show: function (id) {
            $("#edit_term_modal").modal("toggle");

            $("#edit_term_modal").on("shown.bs.modal", function (e) {

                $(".datepicker-input").prop("disabled", true);
                $(".select2-terms").prop("disabled", true);

                mApp.block("#edit_term_modal .modal-content", { type: "loader", message: "Cargando..." });
                $.ajax({
                    url: `/admin/periodosadmision/${id}/get`.proto().parseURL()
                }).done(function (result) {
                    var formElements = $("#edit-form").get(0).elements;
                    formElements["Id"].value = result.id;
                    formElements["Fields_TermId"].value = result.termId;
                    formElements["Fields_StartDate"].value = result.startDate;
                    formElements["Fields_EndDate"].value = result.endDate;
                    formElements["Fields_InscriptionStartDate"].value = result.inscriptionStartDate;
                    formElements["Fields_InscriptionEndDate"].value = result.inscriptionEndDate;
                    formElements["Fields_PublicationDate"].value = result.publicationDate;
                    mApp.unblock("#edit_term_modal .modal-content");
                });
            });
        }
    }

    return {
        init: function () {
            select.init();
            inputs.init();
        }
    }
}();

$(function () {
    InitApp.init();
});
