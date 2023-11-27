var index = function () {
    var datatable = {
        curriculum: {
            object: null,
            options: {
                ajax: {
                    url: "/admin/planes-de-estudios/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.search = "";
                        data.faculty = $("#faculty_select").val();
                        data.career = $("#career_select").val();
                        data.academicProgram = $("#academicProgram_select").val();
                    }
                },
                columns: [
                    {
                        data: "faculty",
                        title: "Facultad"
                    },
                    {
                        data: "name",
                        title: "Carrera"
                    },
                    {
                        data: "academicProgram",
                        title: "Programa"
                    },
                    {
                        data: "curriculumCode",
                        title: "Plan Estudio"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        width: 200,
                        orderable: false,
                        render: function (data) {
                            var tmp = "";
                            var url = `/admin/planes-de-estudios/detalle/${data.id}`.proto().parseURL();
                            tmp+= `<a href="${url}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail" <span><i class="la la-eye"> </i> </span> Ver plan de estudio </span></span></a> `;
                            if (data.isActive) tmp +=`<button class="btn btn-primary m-btn btn-sm m-btn--icon m-btn--icon-only btn-evaluation" data-id="${data.id}"><i class="la la-download"></i></button>`;
                            return tmp;
                        }
                    }
                ]
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);

                $("#data-table").on('click', '.btn-evaluation', function () {
                    var sectionId = $(this).data("id");
                    var $btn = $(this);
                    window.location.href = `/admin/planes-de-estudios/pdf/${sectionId}`.proto().parseURL();
                });

                $("#search").doneTyping(function () {
                    datatable.curriculum.reload();
                });
            },
            reload: function () {
                this.object.ajax.reload();
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
                        //minimumResultsForSearch: -1
                    });

                    select.faculty.events();
                    select.career.init($("#faculty_select").val());
                });
            },
            events: function () {
                $("#faculty_select").on("change", function () {
                    datatable.curriculum.reload();
                    $("#career_select").empty();
                    select.career.init($("#faculty_select").val());
                });
            }
        },
        career: {
            init: function (faculty) {
                $.ajax({
                    url: ("/admin/planes-de-estudios/carreras/" + faculty + "/get").proto().parseURL()
                }).done(function (data) {

                    data.unshift({ id: _app.constants.guid.empty, text: "Todas" });
                    $("#career_select").select2({
                        data: data,
                        //minimumResultsForSearch: -1
                    }).trigger("change");

                    select.career.events();
                    select.academicProgram.init($("#career_select").val());
                });
            },
            events: function () {
                $("#career_select").on("change", function () {
                    datatable.curriculum.reload();
                    $("#academicProgram_select").empty();
                    select.academicProgram.init($("#career_select").val());
                });
            }
        },
        academicProgram: {
            init: function (careerId) {
                if (careerId == _app.constants.guid.empty || careerId == "" || careerId == null) {
                    $("#academicProgram_select").select2({ disabled: true });
                } else {
                    $.ajax({
                        url: (`/especialidadporcarrera/${careerId}/get`).proto().parseURL()
                    }).done(function (data) {
                        data.items.unshift({ id: _app.constants.guid.empty, text: "Todos" });

                        $("#academicProgram_select").select2({
                            data: data.items,
                            disabled: false
                            //minimumResultsForSearch: -1
                        }).trigger("change");

                        select.academicProgram.events();
                    });
                }
            },
            events: function () {
                $("#academicProgram_select").on("change", function () {
                    datatable.curriculum.reload();
                });
            }
        },
        init: function () {
            select.faculty.init();
        },
    };

    return {
        init: function () {
            datatable.curriculum.init();
            select.init();
        }
    };
}();

$(function () {
    index.init();
});