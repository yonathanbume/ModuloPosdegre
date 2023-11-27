//``

var workerPositionTable = function () {
    var baseUrl = "/admin/cargos";

    var private = {
        objects: {}
    };

    var options = getDataTableConfiguration({
        url: `${baseUrl}/listar`,
        data: function(data) {
            data.search = $("#search").val();
        },
        columns: [
            {
                 data: "description",
                 name: "description",
                 title: "Description"
            },
            {
                data: "age",
                name: "age",
                title: "Año"
            },
            {
                data: "category",
                name: "category",
                title: "Categoria"
            },
            {
                data: null,
                title: "Opciones",
                render: function(data) {
                    return `<div class="table-options-section">
                                <button data-id="${data.id}" class="btn btn-info btn-sm m-btn m-btn--icon btn-detail" title="Detalle"><i class="la la-eye"></i></button> 
                                <button data-id="${data.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-edit table-options-double-margin" title="Editar"><i class="la la-edit"></i></button> 
                                <button data-id="${data.id}" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete" title="Eliminar"><i class="la la-trash"></i></button>
                            </div>`;
                }
            }
        ]
    });

    var inputs = {
        init: function() {
            $("#search").doneTyping(function() {
                private.objects["tbl-data"].draw();
            });
        }
    }

    var events = {
        init: function () {
            private.objects["tbl-data"].on("click",".btn-detail",function() {
                var id = $(this).data("id");
                location.href = `${baseUrl}/${id}/detalle`.proto().parseURL();
            });

            private.objects["tbl-data"].on("click",".btn-edit",function() {
                var id = $(this).data("id");
                location.href = `${baseUrl}/${id}/editar`.proto().parseURL();
            });

            private.objects["tbl-data"].on("click",".btn-delete",function() {
                var id = $(this).data("id");
                showDeleteSwal({
                    text: "Se elimino correctamente",
                    promise: () => {
                        return new Promise((resolver) => {
                            $.ajax({
                                    type: "POST",
                                    url: `${baseUrl}/${id}/eliminar`.proto().parseURL()
                                })
                                .always(function(parameters) {
                                    private.objects["tbl-data"].draw();
                                }).done(function(parameters) {
                                    showSuccessSwal();
                                }).fail(function(e) {
                                    showErrorSwal(e.responseText(e));
                                });
                        });
                    }
                });
            });
        }
    }

    var datatable = {
        init: function() {
            private.objects["tbl-data"] = $("#tbl-data").DataTable(options);
            events.init();
        }
    }

    return {
        init: function() {
            inputs.init();
            datatable.init();
        }
    }


}();

$(function () {
    workerPositionTable.init();
});