var InitApp = function () {

    var select2 = {
        init: function () {
            this.campus.init();
            this.academicYears.init();
            this.careers.init();
            this.terms.init();

            //$("#cycle_select").select2();
            $("#type_select").select2();
        },
        terms: {
            init: function () {
                $.ajax({
                    url: `/reporte/cuadro-merito/periodos/get`.proto().parseURL()
                }).done(function (result) {
                    $("#term_select").select2({
                        data: result.items
                    });

                    if (result.selected !== null) {
                        $("#term_select").val(result.selected);
                    }

                    $("#term_select").on("change", function () {
                        var term = $("#term_select").val();
                        select2.careers.load(term);
                    });

                    $("#term_select").trigger('change', [true]);
                });
            }
        },
        careers: {
            init: function () {
                $("#career_select").select2({
                    placeholder: "Seleccione una escuela",
                    disabled: true
                });

                $("#career_select").on("change", function () {
                    var term = $("#term_select").val();
                    var career = $("#career_select").val();
                    select2.academicYears.load(term, career);
                    select2.campus.load(term, career);
                    select2.years.load(term, career);
                });
            },
            load: function (term) {
                if (term == null) {
                    return false;
                }
                $.ajax({
                    url: `/reporte/cuadro-merito/periodos/${term}/carreras/get`.proto().parseURL()
                }).done(function (data) {
                    $("#career_select").empty();
                    $("#career_select").select2({
                        placeholder: "Seleccione una escuela",
                        data: data.items,
                        minimumResultsForSearch: 10,
                        disabled: false
                    });
                    $("#career_select").trigger('change', [true]);
                });
            }
        },
        academicYears: {
            init: function () {
                $("#cycle_select").select2({
                    placeholder: "Seleccione un ciclo",
                    disabled: true
                });
            },
            load: function (term, career) {
                $.ajax({
                    url: `/reporte/cuadro-merito/periodos/${term}/carrera/${career}/ciclos/get`.proto().parseURL()
                }).done(function (data) {
                    $("#cycle_select").empty();
                    $("#cycle_select").select2({
                        placeholder: "Seleccione una ciclo",
                        data: data.items,
                        minimumResultsForSearch: 10,
                        disabled: false
                    });
                });
            }
        },
        campus: {
            init: function () {
                console.log('test');
                $("#campus_select").select2({
                    placeholder: "Seleccione una sede",
                    disabled: true
                });
            },
            load: function (term, career) {
                $.ajax({
                    url: `/reporte/cuadro-merito/periodos/${term}/carrera/${career}/sedes/get`.proto().parseURL()
                }).done(function (data) {
                    $("#campus_select").empty();
                    $("#campus_select").select2({
                        placeholder: "Seleccione una sede",
                        data: data.items,
                        minimumResultsForSearch: 10,
                        disabled: false
                    });
                });
            }
        },
        years: {
            init: function () {
                console.log('test');
                $("#year_select").select2({
                    placeholder: "Seleccione un año",
                    disabled: true
                });
            },
            load: function (term, career) {
                $.ajax({
                    url: `/reporte/cuadro-merito/periodos/${term}/carrera/${career}/anios/get`.proto().parseURL()
                }).done(function (data) {
                    $("#year_select").empty();
                    $("#year_select").select2({
                        placeholder: "Seleccione un año",
                        data: data.items,
                        minimumResultsForSearch: 10,
                        disabled: false
                    });
                });
            }
        }
    };

    var events = {
        init: function () {
            $(".btn-report").on("click", function () {
                if ($("#term_select").val() == null || $("#career_select").val() == null) {
                    toastr.error("Debe seleccionar todos los campos", "Error");
                    return false;
                }

                if ($("#cycle_select").length && $("#cycle_select").val() == null) {
                    toastr.error("Debe seleccionar todos los campos", "Error");
                    return false;
                }

                if ($("#campus_select").length && $("#campus_select").val() == null) {
                    toastr.error("Debe seleccionar todos los campos", "Error");
                    return false;
                }

                var button = $(this);
                button.addClass("m-loader m-loader--right m-loader--primary").attr("disabled", true);

                $.fileDownload(`/reporte/cuadro-merito/calcular-orden-merito`.proto().parseURL(),
                    {
                        httpMethod: 'GET',
                        data: {
                            termId: $("#term_select").val(),
                            careerId: $("#career_select").val(),
                            academicYear: $("#cycle_select").val(),
                            type: $("#type_select").val(),
                            campusId: $("#campus_select").val(),
                            year: $("#year_select").val(),
                        }
                    }
                ).done(function () {
                    toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                })
                    .fail(function (responseHtml) {
                        toastr.error($(responseHtml).text(), "Error");
                    })
                    .always(function () {
                        button.removeClass("m-loader m-loader--right m-loader--primary").attr("disabled", false);
                    });
            })
        }
    };
    return {
        init: function () {
            events.init();
            select2.init();
        }
    }
}();

$(function () {
    InitApp.init();
})