var reportDetails = function () {
    var datatable = {
        object: null,
        sectionId: null,
        options: getSimpleDataTableConfiguration({
            url: `/admin/reporte_asistencia/periodo/get`.proto().parseURL(),
            data: function (data) {
                delete data.columns;
                data.pid = $("#select2-terms").val();
                data.sid = $("#Student_StudentId").val();
            },
            pageLength: 10,
            orderable: [],
            columns: [
                {
                    data: "courseName",
                    title: "Curso",
                },
                {
                    data: "classCount",
                    title: "Total",
                },
                {
                    data: "dictated",
                    title: "Dictadas",
                },
                {
                    data: "assisted",
                    title: "Asistidas",
                },
                {
                    data: "absences",
                    title: "Faltas",
                },
                {
                    data: null,
                    title: "% Faltas",
                    render: function (row) {
                        var absencePercentage = row.absences / row.classCount * 100;
                        var number = roundNumber(absencePercentage, 2);
                        return isNaN(number) ? 0 : number + " %"; //absencePercentage.proto().round(2) + " %";
                    }
                },
                {
                    data: null,
                    title: "Opciones",
                    render: function (row) {
                        var tmp = "";
                        tmp += "<button class=\"btn btn-primary m-btn btn-sm m-btn--icon btn-detail\" ";
                        tmp += `data-id=\"${row.sectionId}\" `;
                        tmp += "><span><i class=\"la la-eye\"></i><span> Ver Detalle</span></span></button>";
                        return tmp;
                    }
                }
            ]
        }),
        reloadTable: function () {
            if (datatable.object === null) {
                datatable.init();
            } else {
                datatable.object.ajax.reload();
            }
        },
        events: {
            onDetails: function () {
                $("#datatable_absences").on('click', '.btn-detail', function () {
                    var sectionId = $(this).data("id");
                    datatable.sectionId = sectionId;

                    $("#absences-detail-modal").modal("toggle");
                    $("#SectionId").val(sectionId);
                    $("#assistance-filter").select2();
                    $("#assistance-filter").val(_app.constants.assistance.absence.value).trigger("change");

                    $("#assistance-filter").on("change", function (e) {
                        e.preventDefault();
                        e.stopImmediatePropagation();
                        detailsDatatable.reloadTable();
                    });

                    $("#absences-detail-modal").one("shown.bs.modal", function (e) {
                        $("#assistance-filter").trigger("change");
                    });

                    $("#absences-detail-modal").one("hidden.bs.modal", function (e) {
                        if (detailsDatatable.object !== null) {
                            detailsDatatable.object.destroy();
                            detailsDatatable.object = null;
                        }
                        $("#assistance-filter").off("change");
                    });
                });
            },
            init: function () {
                this.onDetails();
            }
        },
        init: function () {
            datatable.events.init();
            datatable.object = $("#datatable_absences").DataTable(datatable.options);
        }
    };
    var detailsDatatable = {
        object: null,
        options: getSimpleDataTableConfiguration({
            url: `/admin/reporte_asistencia/seccion/alumno/get`.proto().parseURL(),
            data: function (data) {
                delete data.columns;
                data.filter = $("#assistance-filter").val();
                data.sid = datatable.sectionId;
                data.aid = $("#Student_StudentId").val();
            },
            pageLength: 10,
            orderable: [],
            columns: [
                {
                    data: "week",
                    title: "Semana",
                },
                {
                    data: "sessionNumber",
                    title: "Sesión",
                },
                {
                    data: "date",
                    title: "Fecha",
                },
                {
                    data: "weekDay",
                    title: "Día",
                },
                {
                    data: "startTime",
                    title: "Inicio",
                },
                {
                    data: "endTime",
                    title: "Fin",
                },
                {
                    data: null,
                    title: "Estado",
                    render: function (row) {
                        var tmp = "";
                        var key = row.isAbsent ? 0 : 1;
                        var status = {
                            0: { text: _app.constants.assistance.absence.text, value: _app.constants.assistance.absence.value, icon: "la la-user-times", class: "m-badge--danger" },
                            1: { text: _app.constants.assistance.assisted.text, value: _app.constants.assistance.assisted.value, icon: "flaticon-user-ok", class: "m-badge--success" }
                        };
                        return `<span class="m-badge ${status[key].class} m-badge--wide"><i class="${status[key].icon}"></i> ${status[key].text}</span>`;
                    }
                }
            ]
        }),
        reloadTable: function () {
            if (detailsDatatable.object === null) {
                detailsDatatable.init();
            } else {
                detailsDatatable.object.ajax.reload();
            }
        },
        init: function () {
            //detailsDatatable.events.init();
            detailsDatatable.object = $("#absences-detail-datatable").DataTable(detailsDatatable.options);
        }
    };
    var loadTermSelect = function () {
      
         $.ajax({
             type: "GET",
             url: `/admin/reporte_asistencia/periodos/get`.proto().parseURL()
         }).done(function (data) {
             $("#select2-terms").select2({
                 data: data,
                 placeholder: "Periodo Academico"
             }).val($("#ActiveTerm").val()).trigger('change');
         });
         $("#select2-terms").on('change', function () {
             datatable.reloadTable();
         });
    }

    function roundNumber(num, scale) {
        if (!("" + num).includes("e")) {
            return +(Math.round(num + "e+" + scale) + "e-" + scale);
        } else {
            var arr = ("" + num).split("e");
            var sig = "";
            if (+arr[1] + scale > 0) {
                sig = "+";
            }
            return +(Math.round(+arr[0] + "e" + sig + (+arr[1] + scale)) + "e-" + scale);
        }
    }

    return {
        load: function () {
            loadTermSelect();
        }
    }
}();
$(function () {
    reportDetails.load();
})