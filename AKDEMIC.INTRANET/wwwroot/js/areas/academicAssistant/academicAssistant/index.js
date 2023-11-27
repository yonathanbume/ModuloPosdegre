var certificate = function () {
    var sectionDatatable = null;

    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/academic-assistant/academic-assistant/sections`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'code',
                title: 'Código de sección'
            },
            {
                field: 'name',
                title: 'Curso'
            },
            {
                field: 'career',
                title: 'Carrera'
            },
            {
                field: 'faculty',
                title: 'Facultad'
            },
            {
                field: "options",
                title: "Opciones",
                sortable: false,
                filterable: false,
                template: function (row) {
                    return `<button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-scores"<span><i class="la la-eye"></i></span>Ver notas</span></span></button>`;
                }
            }
        ]
    };

    var events = {
        init: function () {
            sectionDatatable.on("click", ".btn-scores", function () {
                var id = $(this).data('id');
                location.href = `/academic-assistant/academic-assistant/sectionScores/${id}`.proto().parseURL();
            });
        }
    };

    var select2 = {
        init: function () {
            this.faculties.init();
            this.careers.initEvents();
            this.courses.initEvents();
        },
        faculties: {
            init: function () {
                $.ajax({
                    url: "/facultades/get"
                }).done(function (result) {
                    $(".select2-faculties").select2({
                        data: result.items
                    }).on("change", function () {
                        select2.careers.init($(this).val());
                        }).trigger("change");
                });
            }
        },
        careers: {
            initEvents: function () {
                $(".select2-careers").on("change",
                    function () {
                        datatable.init($(".select2-faculties").val(), $(this).val());
                    });
            },
            init: function (facultyId) {
                $(".select2-careers").prop("disabled", true);
                $.ajax({
                    url: `/carreras/get?fid=${facultyId}`.proto().parseURL()
                }).done(function (result) {
                    $(".select2-careers").empty();
                    result.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });
                    $(".select2-careers").select2({
                        data: result.items,
                        placeholder: "Carrera"
                    })
                    if (result.items.length > 1) 
                        $(".select2-careers").prop("disabled", false);
                    $(".select2-careers").trigger("change");
                });
            }
        },
        courses: {
            initEvents: function () {
                $(".select2-courses").on("change",
                    function () {
                        datatable.init($(".select2-careers").val(), $(this).val());
                    });
            },
            init: function (careerId) {
                $(".select2-courses").prop("disabled", true);
                $.ajax({
                    url: `/cursos/get?cid=${careerId}`.proto().parseURL()
                }).done(function (result) {
                    $(".select2-courses").empty();
                    result.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });
                    $(".select2-courses").select2({
                        data: result.items,
                        placeholder: "Curso"
                    });
                    if (result.items.length > 1) 
                        $(".select2-courses").prop("disabled", false);
                    $(".select2-courses").trigger("change");
                });
            }
        }
    };

    var datatable = {
        init: function (facultyId, careerId) {
            if (sectionDatatable !== null) {
                sectionDatatable.destroy();
                sectionDatatable = null;
            }
            options.data.source.read.url = `/academic-assistant/academic-assistant/sections?fid=${facultyId}&cid=${careerId}`.proto().parseURL();
            sectionDatatable = $(".m-datatable").mDatatable(options);
            events.init();
        }
    }

    return {
        init: function () {
            select2.init();
        }
    };
}();

$(function () {
    certificate.init();
});