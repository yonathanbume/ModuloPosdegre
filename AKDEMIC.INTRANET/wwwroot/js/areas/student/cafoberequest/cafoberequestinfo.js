var InitApp = function () {
    var datatable = {
        cafobeRequest: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/estudiante/apoyo-economico/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.searchValue = $("#search").val();
                        data.type = $("#type").val();
                        data.status = $("#status").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Periodo Académico",
                        data: "termName"
                    },
                    {
                        title: "Tipo",
                        data: "typeText"
                    },
                    {
                        title: "Estado",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";

                            if (data.status == 0) {
                                template += `<span class="m-badge m-badge--primary m-badge--wide">${data.statusText}</span>`
                            } else if (data.status == 1) {
                                template += `<span class="m-badge m-badge--success m-badge--wide">${data.statusText}</span>`
                            } else if (data.status == 2) {
                                template += `<span class="m-badge m-badge--danger m-badge--wide">${data.statusText}</span>`
                            } else if (data.status == 3) {
                                template += `<span class="m-badge m-badge--warning m-badge--wide">${data.statusText}</span>`
                            } else {
                                template += `<span class="m-badge m-badge--secondary m-badge--wide">${data.statusText}</span>`
                            }
                            return template;
                        }
                    },
                    {
                        title: "Observación",
                        data: "observation"
                    },
                    {
                        title: "Monto",
                        data: "supportAmount"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var tmp = "";
                            tmp += `<button data-id="${data.id}" data-type="${data.type}" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit"  title="Editar"><i class="la la-edit"></i></button> `;
                            return tmp;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#data-table").on('click', '.btn-edit', function () {
                    var id = $(this).data('id');
                    var type = $(this).data('type');

         

                    switch (type) {
                        case 1: console.log(type);
                            location.href = `/estudiante/apoyo-economico/alto-rendimiento/editar/${id}`.proto().parseURL();
                            break;
                        case 2: location.href = `/estudiante/apoyo-economico/maternidad/editar/${id}`.proto().parseURL();
                            break;

                        case 3: location.href = `/estudiante/apoyo-economico/salud/editar/${id}`.proto().parseURL();
                            break;

                        case 4: location.href = `/estudiante/apoyo-economico/defuncion/editar/${id}`.proto().parseURL();
                            break;

                        case 5: location.href = `/estudiante/apoyo-economico/oftalmologico/editar/${id}`.proto().parseURL();
                            break;
                        case 6: location.href = `/estudiante/apoyo-economico/estimulo/editar/${id}`.proto().parseURL();
                            break;
                    }
                });

                //$("#data-table").on('click', '.btn-delete', function () {
                //    var id = $(this).data("id");
                //    modal.delete(id);
                //});
            }
        },
        init: function () {
            this.cafobeRequest.init();
        }
    };


    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.cafobeRequest.reload();
            });
        }
    };

    var select = {
        init: function () {
            this.terms.init();
            this.careers.init();
            this.types.init();
            this.status.init();
        },
        status: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $("#status").select2();
            },
            events: function () {
                $("#status").on("change", function () {
                    datatable.cafobeRequest.reload();
                });
            }
        },
        types: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $("#type").select2();
            },
            events: function () {
                $("#type").on("change", function () {
                    datatable.cafobeRequest.reload();
                });
            }
        },
        terms: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: ("/periodos/get/v2").proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#terms").select2({
                        data: result.items,
                    });

                });
            },
            events: function () {
                $("#terms").on("change", function () {
                    datatable.cafobeRequest.reload();
                });
            }
        },
        careers: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: ("/carreras/v2/get").proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#careers").select2({
                        data: result.items,
                    });

                });
            },
            events: function () {
                $("#careers").on("change", function () {
                    datatable.cafobeRequest.reload();
                });
            }
        }
    };


    return {
        init: function () {
            datatable.init();
            search.init();
            select.init();
        }
    }
}();

$(function () {
    InitApp.init();
})