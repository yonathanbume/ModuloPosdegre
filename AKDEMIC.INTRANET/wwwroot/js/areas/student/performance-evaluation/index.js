const Datatable = function () {
    const id = "#sections-datatable";
    let datatable;
    $.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
        $("#sections-datatable_wrapper").hide();
    };

    const options = {
        ajax: {
            data: function (data, settings) {
                data["searchValue"] = $("#search").val();
            },
            url: `/alumno/evaluacion-docente/get`.proto().parseURL(),
            type: "GET",
            complete: function (e) {
                try {
                    JSON.parse(e.responseText);
                } catch (x) {
                    $("#datatable_msg_txt").html(e.responseText);
                    $("#datatable_msg").removeClass("m--hide").show();
                }
            }
        },
        ordering: false,
        columns: [
            {
                data: "career",
                title: "Escuela Profesional"
            },
            {
                data: "course",
                title: "Curso"
            },
            {
                data: "section",
                title: "Sección"
            },
            {
                data: "teacher",
                title: "Docente"
            },
            {
                data: "options",
                title: "Opciones",
                className: "text-center",
                orderable: false,
                render: function (data, type, row) {
                    if (row.value != null) {
                        return "<button data-id='" + row.userId + "' class='btn btn-primary btn-sm m-btn m-btn--icon' disabled>Calificado</button>";
                    }
                    else {
                        return "<button data-id='" + row.userId + "' data-section='" + row.sectionId + "' class='btn btn-primary btn-sm m-btn m-btn--icon btn-qualify'>Calificar</button>";
                    }
                }
            }
        ]
    };

    var events = {
        getPendingSurveys: function () {
            $.ajax({
                url: "/alumno/evaluacion-docente/get-cantidad-encuestas-pendientes",
                type: "GET"
            })
                .done(function (e) {
                    if (e === 0) {
                        $("#completed_check").removeClass("d-none");
                    } else {
                        $("#alert_pending_surveys").removeClass("d-none");
                        $("#quantity_pending").text(e);
                    }
                })
        },
        init: function () {
            this.getPendingSurveys();
            $(id).on("click", ".btn-qualify", function () {
                var dataId = $(this).data("id");
                var sectionId = $(this).data("section");
                window.location.href = `/alumno/evaluacion-docente/${dataId}/encuesta/${sectionId}/seccion`;
            });
        }
    };

    var loadDatatable = function () {
        $("#search").doneTyping(function () {
            datatable.ajax.reload();
        });
        datatable = $(id).DataTable(options);
        events.init();
    };

    return {
        init: function () {
            loadDatatable();
        },
        reload: function () {
            datatable.ajax.reload();
        }
    };
}();

$(function () {
    Datatable.init();
});

