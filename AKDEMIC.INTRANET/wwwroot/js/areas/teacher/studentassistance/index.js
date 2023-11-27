var Assistances = function () {
    var select2 = {
        init: function () {
            this.section.init();
        },
        section: {
            init: function () {

                $("#section_select").on("change", function () {
                    var value = $(this).val();
                    mApp.block(".m-portlet__body", {
                        message : "Cargando datos..."
                    });

                    $.ajax({
                        url: `/profesor/asistencia/get-toma-asistencia?sectionId=${value}`,
                        type : "GET"
                    })
                        .done(function (e) {
                            $("#main_container").html(e);

                            mApp.unblock(".m-portlet__body");
                        })
                })

                $("#section_select").select2({
                    minimumResultsForSearch: -1,
                    width: "500px"
                }).trigger("change");
            }
        }
    };

    return {
        init: function () {
            select2.init();
        }
    }
}();

$(function () {
    Assistances.init();
});