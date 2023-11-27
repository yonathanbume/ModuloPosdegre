var InitApp = function () {

    var datatable = {
        students: {
            serverSide: false,
            object: null,
            options: {
                ajax: {
                    url: "/reporte/pagos-exonerados/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.termId = $("#term_select").val();
                        data.careerId = $("#career_select").val();
                        data.type = $("#type_select").val();
                    }
                },
                columns: [
                    {
                        title: "Código",
                        data: "user"
                    },
                    {
                        title: "Nombre completo",
                        data: "fullname"
                    },
                    {
                        title: "Carrera",
                        data: "career",
                        orderable: false
                    },
                    {
                        title: "Tipo Pago",
                        data: "type",
                        orderable: false
                    },
                    {
                        title: "Concepto",
                        data: "concept"
                    },
                    //{
                    //    title: "Monto",
                    //    data: "totalamount"
                    //},
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Excel',
                        action: function () {
                            var button = $(this)[0].node;
                            $(button).addClass("m-loader m-loader--right m-loader--primary").attr("disabled", true);

                            $.fileDownload(`/reporte/pagos-exonerados/reporte-excel`.proto().parseURL(),
                                {
                                    httpMethod: 'GET',
                                    data: {
                                        termId: $("#term_select").val(),
                                        careerId: $("#career_select").val(),
                                        type: $("#type_select").val()
                                    },
                                    successCallback: function () {
                                        toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                                    }
                                }
                            ).done(function () {

                            })
                                .fail(function () { alert('Falló la descarga del archivo!'); })

                                .always(function () {
                                    $(button).removeClass("m-loader m-loader--right m-loader--primary").attr("disabled", false);
                                });
                        }
                    }
                ]
            },
            init: function () {
                datatable.students.object = $("#data-table").DataTable(datatable.students.options);
            },
            reload: function () {
                datatable.students.object.ajax.reload();
            }
        }
    };

    var select = {
        term: {
            init: function () {
                $.ajax({
                    url: "/periodos/get".proto().parseURL()
                }).done(function (data) {
                    $("#term_select").select2({
                        data: data.items
                    });

                    if (data.selected !== null) {
                        $("#term_select").val(data.selected);
                        $("#term_select").trigger("change.select2");
                    }

                    datatable.students.init();

                    $("#term_select").on("change", function (e) {
                        datatable.students.reload();
                    });
                });
            }
        },
        career: {
            init: function () {
                $.ajax({
                    url: ("/carreras/get").proto().parseURL()
                }).done(function (data) {

                    data.items.unshift({ id: _app.constants.guid.empty, text: "Todas" });

                    $("#career_select").select2({
                        placeholder: "Seleccione una escuela",
                        data: data.items,
                        minimumResultsForSearch: 10
                    });

                    $("#career_select").on("change", function () {
                        datatable.students.reload();
                    });
                });

            }
        },
        type: {
            init: function () {
                $("#type_select").select2({
                    placeholder: "Seleccione un tipo",
                    minimumResultsForSearch: -1
                });

                $("#type_select").on("change", function () {
                    datatable.students.reload();
                });
            }
        },
        init: function () {
            select.type.init();
            select.career.init();
            select.term.init();
        }
    };

    return {
        init: function () {
            select.init();
        }
    };
}();

$(function () {
    InitApp.init();
});