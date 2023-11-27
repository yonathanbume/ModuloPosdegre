var CurriculumProgress = function () {
    var options = {
        data: {
            type: "remote",
            source: {
                read: {
                    method: "GET",
                    url: ""
                }
            },
            pageSize: 10,
            saveState: {
                cookie: true,
                webstorage: true,
            }
        },
        pagination: false,
        columns: [
            {
                field: "course",
                title: "Curso",
                width: 200,
                sortable: false
            },
            {
                field: "credits",
                title: "Créditos",
                textAlign: "center",
                width: 80,
                sortable: false
            },
            {
                field: "try",
                title: "N° de Veces",
                textAlign: "center",
                width: 120,
                sortable: false
            },
            {
                field: "grade",
                title: "Promedio Final",
                textAlign: "center",
                width: 120,
                sortable: false,
                template: function (data) {
                    if (data.validated) {
                        return `<span class="m--font-info"> ${data.grade} </span>`;
                    }
                    return data.grade;
                }
            },
            {
                field: "term",
                title: "Ciclo",
                textAlign: "center",
                width: 80,
                sortable: false
            },
            {
                field: "status",
                title: "Estado",
                textAlign: "center",
                width: 100,
                sortable: false,
                template: function (row) {
                    return row.status ? "Cumplido" : "Pendiente";
                }
            }
        ]
    }

    var datatables = {
        init: {
            all: function () {
                $(".academic-year-datatable").each(function (index, datatable) {
                    var ayid = $(datatable).data("ayid");
                    var studentId = location.pathname.match(/(.{0,8}-.{0,4}-.{0,4}-.{0,4}-.{0,12})/).pop();
                    options.data.source.read.url = `/alumnos/${studentId}/situacion/nivel/${ayid}/get`.proto().parseURL();
                    datatable.id = `academic-year-${index}`;
                    datatable.dataset.number = index;
                    $("#" + datatable.id).mDatatable(options);
                });
            }
        },
        events: {
            load: function () {
                $(".academic-year-datatable").on("m-datatable--on-init", function () {
                    var totalCredits = 0;
                    var allApproved = true;

                    $(`#${this.id} td[data-field="credits"]`).each(function () {
                        var value = $(this).text();
                        if (!isNaN(value) && value.length > 0 && !isNaN(parseInt(value))) {
                            totalCredits += parseInt(value);
                        }
                    });

                    $(`#${this.id} td[data-field="status"`).each(function () {
                        var value = $(this).text();
                        if (value.length > 0) {
                            allApproved = allApproved && (value === "Cumplido");
                        }
                    });

                    var num = $(this).data("number");
                    var oldTitle = $(`#m-accordion-title-${num}`).text();
                    $(`#m-accordion-title-${num}`).html(`${oldTitle}&emsp;<i class="fa fa-angle-double-right"></i>&emsp;${totalCredits} créditos`);
                    var iconClass = (allApproved ? "fa fa-check" : "fa fa-exclamation-triangle");
                    $(`#m-accordion-icon-${num}`).html(`<i class="${iconClass}"></i>`);
                    $(`#m-accordion-icon-${num}`).addClass(allApproved ? "text-success" : "text-warning");
                });
            }
        }
    };

    return {
        init: function () {
            datatables.init.all();
            datatables.events.load();
        }
    };
}();

var CurriculumProgressElectives = function () {
    var options = {
        data: {
            type: "remote",
            source: {
                read: {
                    method: "GET",
                    url: "/alumno/progreso/electivos/get".proto().parseURL()
                }
            },
            pageSize: 10,
            saveState: {
                cookie: true,
                webstorage: true,
            }
        },
        pagination: false,
        columns: [
            {
                field: "course",
                title: "Curso",
                width: 200,
                sortable: false
            },
            {
                field: "academicYear",
                title: "Nivel",
                width: 70,
                textAlign: "center",
                sortable: false
            },
            {
                field: "credits",
                title: "Créditos",
                textAlign: "center",
                width: 80,
                sortable: false
            },
            {
                field: "try",
                title: "N° de Veces",
                textAlign: "center",
                width: 100,
                sortable: false
            },
            {
                field: "grade",
                title: "Promedio Final",
                textAlign: "center",
                width: 120,
                sortable: false
            },
            {
                field: "term",
                title: "Ciclo",
                textAlign: "center",
                width: 80,
                sortable: false
            },
            {
                field: "status",
                title: "Estado",
                textAlign: "center",
                width: 100,
                sortable: false,
                template: function (row) {
                    return row.status ? "Cumplido" : "-";
                }
            }
        ]
    };

    var datatable = {
        init: {
            all: function () {
                var studentId = location.pathname.match(/(.{0,8}-.{0,4}-.{0,4}-.{0,4}-.{0,12})/).pop();
                options.data.source.read.url = `/alumnos/${studentId}/situacion/electivos/get`.proto().parseURL();
                $(".elective-courses-datatable").mDatatable(options);
            }
        }
    };

    return {
        init: function () {
            datatable.init.all();
        }
    };
}();

$(function () {
    CurriculumProgress.init();
    CurriculumProgressElectives.init();
});