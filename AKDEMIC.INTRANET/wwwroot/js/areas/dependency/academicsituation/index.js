var AcademicSituation = function () {
    var studentDatatable = null;

    var options = {
        responsive: true,
        processing: true,
        serverSide: true,
        ajax: {
            url: "/dependencia/situacion-academica/get".proto().parseURL(),
            data: function (d) {
                d.fid = $(".select2-faculties").val();
                d.cid = $(".select2-careers").val();
                d.academicOrder = $(".select2-orders ").val();
            }
        },
        columns: [
            { title: "Nombre y Apellidos", data: "name" },
            { title: "Carrera", data: "career" },
            { title: "Órden de Mérito", data: "lastOrder" },
            { title: "Promedio Ponderado", data: "lastGrade" },
            {
                title: "Clasificación",
                data: null,
                render: function (data, type, row) {
                    if (row.lastMeritOrder !== null && row.lastMeritOrder !== -1) {
                        var order = {
                            0: {
                                text: _app.constants.academicOrder.upperThird.text,
                                value: _app.constants.academicOrder.upperThird.value,
                                class: "m-badge--metal"
                            },
                            1: {
                                text: _app.constants.academicOrder.upperFifth.text,
                                value: _app.constants.academicOrder.upperFifth.value,
                                class: "m-badge--accent"
                            },
                            2: {
                                text: _app.constants.academicOrder.upperTenth.text,
                                value: _app.constants.academicOrder.upperTenth.value,
                                class: "m-badge--focus"
                            }
                        };
                        return "<span class='m-badge " +
                            order[row.lastMeritOrder].class +
                            " m-badge--wide'>" +
                            order[row.lastMeritOrder].text +
                            "</span>";
                    } else {
                        return "Ninguno";
                    }
                }
            },
            { title: "Créditos Aprobados", data: "approvedCredits" },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    return `<button data-id="${row.id}" class="btn btn-info btn-sm m-btn m-btn--icon btn-record" title="Record Académico"><span><i class="flaticon-interface-7"></i><span>Record</span></span></button> <button data-id="${row.id}" class="btn btn-brand btn-sm m-btn m-btn--icon btn-situation" title="Situación Académica"><span><i class="flaticon-statistics"></i><span>Situación</span></span></button>`;
                }
            }
        ]
    };

    var select2 = {
        init: function () {
            this.faculties.init();
            this.careers.initEvents();
            this.orders.init();
        },
        faculties: {
            init: function () {
                $.ajax({
                    url: "/facultades/get".proto().parseURL()
                }).done(function (result) {
                    $(".select2-faculties").select2({
                        data: result.items,
                        placeholder: "Facultad"
                    }).on("change", function () {
                        select2.careers.init($(this).val());
                    }).trigger("change");
                });
            }
        },
        careers: {
            initEvents: function() {
                $(".select2-careers").on("change",
                    function () {
                        datatable.init();
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
                    });
                    if (result.items.length > 1) {
                        $(".select2-careers").prop("disabled", false);
                    }
                    $(".select2-careers").trigger("change");
                });
            }
        },
        orders: {
            init: function() {
                $(".select2-orders").select2().on("change", function () {
                    datatable.init();
                });
            }
        }
    }

    var datatable = {
        init: function (facultyId, careerId, academicOrder) {
            if (studentDatatable === null) {
                studentDatatable = $(".students-datatable").DataTable(options);
                this.initEvents();
            }
            else {
                studentDatatable.ajax.reload();
            }
        },
        initEvents: function () {
            studentDatatable.on("click",
                ".btn-situation",
                function () {
                    var id = $(this).data("id");
                    location.href = `/dependencia/situacion-academica/${id}/situacion`.proto().parseURL();
                });

            studentDatatable.on("click",
                ".btn-record",
                function () {
                    var id = $(this).data("id");
                    location.href = `/dependencia/situacion-academica/${id}/historial`.proto().parseURL();
                });
        }
    }

    return {
        init: function () {
            select2.init();
        }
    }
}();

$(function () {
    AcademicSituation.init();
});