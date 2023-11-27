//== Class definition

var pageInit = function () {
    //== Private functions
    var datatable;

    var SectionId = $("#SectionId").val();

    var options = {
        data: {
            type: "local"
        },
        sortable: false,
        pagination: false,
        type: "local",
        columns: columns
    };

    var tableInitializer = function () {
        //datatable = $(".m-datatable").mDatatable(options);       

        datatable = $("#student-table").DataTable({
            scrollX: true,
            responsive: false,
            ordering: false,
            filter: false,
            lengthChange: false,
            paging: false,
            info: false
        });

    };

    var selectInitializer = function () {
        $("#selEvaluation").select2({
            placeholder: "Seleccione una evaluación",
            minimumResultsForSearch: -1
        });

        $("#select_courseUnit").select2({
            placeholder: "Seleccione una unidad",
            minimumResultsForSearch: -1
        })

        $("#btnRegister").click(function () {
            var url = $("#btnRegister").data("url");
            url = url.slice(0, -1);

            var evId = $("#selEvaluation").val();

            if (evId === "") {
                swal("Evaluación no seleccionada", "Seleccione una evaluación para registrar las notas", "warning");
            } else {
                window.location.href = url + evId;
            }
        });

        $("#btnRegister_unit").click(function () {
            var id = $("#select_courseUnit").val();
            var url = `/profesor/notas/registrar/${SectionId}/unidad/${id}`;

            if (id == "" || id == null) {
                swal("Unidad no seleccionada", "Seleccione una unidad para registrar las notas", "warning");
            } else {
                window.location.href = url;
            }
        });
    };

    var events = function () {
        $('.btn-evaluation').on('click', function () {
            var sectionId = $(this).data("sectionid");
            var $btn = $(this);
            $btn.addLoader();

            $.fileDownload(`/profesor/notas/acta-final/${sectionId}`.proto().parseURL(), {
                httpMethod: 'GET', successCallback: function (url) {
                    $btn.removeLoader();
                    toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                },
                failCallback: function (responseHtml, url) {
                    toastr.error("No se pudo descargar el archivo", "Error");
                }
            });
        });
        $('.btn-evaluation-2').on('click', function () {
            var sectionId = $(this).data("sectionid");
            var $btn = $(this);
            $btn.addLoader();

            $.fileDownload(`/profesor/notas/acta-final/${sectionId}`.proto().parseURL(), {
                httpMethod: 'GET', successCallback: function (url) {
                    $btn.removeLoader();
                    toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                },
                failCallback: function (responseHtml, url) {
                    toastr.error("No se pudo descargar el archivo", "Error");
                }
            });
        });
        $('.btn-evaluation-detalle').on('click', function () {
            var sectionId = $(this).data("sectionid");
            var $btn = $(this);
            $btn.addLoader();

            $.fileDownload(`/profesor/notas/acta-final-detallada/${sectionId}`.proto().parseURL(), {
                httpMethod: 'GET', successCallback: function (url) {
                    $btn.removeLoader();
                    toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                },
                failCallback: function (responseHtml, url) {
                    toastr.error("No se pudo descargar el archivo", "Error");
                }
            });
        });
    };

    return {
        //== Public functions
        init: function () {
            // init
            tableInitializer();
            selectInitializer();
            events();
        }
    };
}();

jQuery(document).ready(function () {
    pageInit.init();
});