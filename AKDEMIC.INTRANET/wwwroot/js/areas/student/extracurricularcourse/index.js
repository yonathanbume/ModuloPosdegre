var ActivitiesTable = function () {
    var id = "#courses-datatable";
    var datatable;
    var datatableDetailEvaluation;
    var datatableDetailSection;

    var options = {
        search: {
            input: $("#search")
        },
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ("/alumno/cursos-extracurriculares/get").proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "course",
                title: "Curso"
            },
            {
                field: "group",
                title: "Grupo"
            },
            {
                field: "price",
                title: "Precio"
            },
            {
                field: "credits",
                title: "Crédits"
            },
            {
                field: "options",
                title: "Opciones",
                textAlign: "center",
                width: 125,
                sortable: false,
                filterable: false,
                template: function (row) {
                    var template = "";
                    template += "<button data-id='" + row.id + "' data-price='" + row.price + "' class='btn btn-accent btn-sm m-btn m-btn--icon btn-enrollment'><span><i class='la la-edit'></i><span>Matricularse</span></span></button>";
                    return template;
                }
            }
        ]
    };

    var events = {
        init: function () {
            $(".btn-enrollment").on("click", function () {
                var dataId = $(this).data("id");
                var dataPrice = $(this).data("price");
                swal({
                    title: "¿Está seguro?",
                    text: "Se generará un pago de S/." + dataPrice + " para la matrícula",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, generar",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise(() => {
                            $.ajax({
                                url: ("/alumno/cursos-extracurriculares/enrollment").proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: dataId
                                },
                                success: function () {
                                    datatable.reload();
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "Se ha generado el pago de la matrícula con éxito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function (error) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: error.responseText
                                    });
                                }
                            });
                        });
                    }
                });
            });
        }
    };

    var loadDatatable = function () {
        if (datatable !== undefined)
            datatable.destroy();
        options.data.source.read.url = ("/alumno/cursos-extracurriculares/get").proto().parseURL();
        datatable = $(id).mDatatable(options);
        $(datatable).on("m-datatable--on-layout-updated", function () {
            events.init();
        });
    };
    return {
        init: function () {
            loadDatatable();
        },
        reload: function () {
            datatable.reload();
        }
    }
}();

$(function () {
    ActivitiesTable.init();
});