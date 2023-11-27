var Search = function () {

    var select = {
        search: {
            init: function () {
                $("#search").select2({
                    width: "100%",
                    placeholder: "Buscar...",
                    ajax: {
                        url: "/academico/alumnos/buscar".proto().parseURL(),
                        dataType: "json",
                        data: function (params) {
                            return {
                                term: params.term,
                                page: params.page
                            };
                        },
                        processResults: function (data, params) {
                            return {
                                results: data.items
                            };
                        },
                        cache: true
                    },
                    escapeMarkup: function (markup) {
                        return markup;
                    },
                    minimumInputLength: 3
                });

                $("#search").on("change", function () {
                    var id = $("#search").val();
                    window.location.href = `/academico/alumnos/informacion/${id}`.proto().parseURL();
                });
            }
        }
    };

    return {
        init: function () {
            select.search.init();
        }
    };
}();

$(function () {
    Search.init();
});