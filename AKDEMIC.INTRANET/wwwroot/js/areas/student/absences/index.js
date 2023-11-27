var Absences = function () {
    var datatable = null;
    var detailDatatable = null;

    // Round
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

    var options = {
        data: {
            type: "remote",
            source: {
                read: {
                    method: "GET",
                    url: `/alumno/inasistencias/periodo/${$("#select-term").val()}/get`.proto().parseURL()
                }
            },
            pageSize: 10,
            saveState: {
                cookie: true,
                webstorage: true
            }
        },
        columns: [
            {
                field: "courseName",
                title: "Curso",
                width: 200,
                sortable: false
            },
            {
                field: "classCount",
                title: "Total",
                textAlign: "center",
                width: 80,
                sortable: false
            },
            {
                field: "dictated",
                title: "Dictadas",
                textAlign: "center",
                width: 80,
                sortable: false
            },
            {
                field: "assisted",
                title: "Asistidas",
                textAlign: "center",
                width: 80,
                sortable: false
            },
            {
                field: "absences",
                title: "Faltas",
                textAlign: "center",
                width: 80,
                sortable: false
            },
            {
                field: "absencePercentage",
                title: "% Faltas",
                textAlign: "center",
                sortable: false,
                width: 80,
                template: function (row) {
                    return row.absencesPercentage + " %";
                }
            },
            {
                field: "options",
                title: "Opciones",
                width: 150,
                template: function (row) {
                    var tmp = "";
                    tmp += "<button class=\"btn btn-primary m-btn btn-sm m-btn--icon btn-detail\" ";
                    tmp += `data-id=\"${row.sectionId}\" `;
                    tmp += "><span><i class=\"la la-list\"></i><span> Ver Detalle</span></span></button>";

                    if (row.isActive) {
                        tmp += ` <button class="btn btn-secondary m-btn btn-sm m-btn--icon m-btn--icon-only btn-request" data-id="${row.sectionId}" title="Solicitar justificación"><i class="la la-eraser"></i></button>`;
                    }
                    return tmp;
                }
            }
        ]
    };

    var optionDetails = {
        dom: 'Bfrtip',
        buttons: [
            {
                extend: "excel",
                className: "btn m-btn m-btn--icon",
                text: "<i class='la la-file-excel-o'></i><span>Excel</span>",
                titleAttr: "Excel"
            }
            //,
            //{
            //    extend: "pdf",
            //    className: "btn m-btn m-btn--icon",
            //    text: "<i class='la la-file-pdf-o'></i><span>PDF</span>",
            //    titleAttr: "PDF"
            //}
        ],
        responsive: true,
        processing: true,
        serverSide: false,
        ajax: {
            url: `/alumno/inasistencias/seccion/${$("#assistance-filter").val()}/get`.proto().parseURL(),
            dataSrc: ""
        },
        columns: [
            { title: "Semana", data: "week" },
            { title: "Sesión", data: "sessionNumber" },
            { title: "Fecha", data: "date" },
            { title: "Día", data: "weekDay" },
            { title: "Inicio", data: "startTime" },
            { title: "Fin", data: "endTime" },
            //{ title: "Estado", data: "status" },
            {
                title: "Estado",
                data: null,
                orderable: false,
                render: function (data, type, row) {
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
    };

    var events = {
        init: function () {
            $("#select-term").on("change", function (e) {
                loadDatatable();
            });
        },
        datatable: {
            init: function () {
                datatable.on("click", ".btn-detail", function () {
                    var sectionId = $(this).data("id");
                    detail.show(sectionId);
                });

                datatable.on("click", ".btn-request", function () {
                    var sectionId = $(this).data("id");
                    form.create.load(sectionId);
                });
            }
        }
    }

    var detail = {
        show: function (sectionId) {
            $("#absences-detail-modal").modal("toggle");
            $("#SectionId").val(sectionId);
            $("#assistance-filter")
                .select2({
                    minimumResultsForSearch: -1
                });
            $("#assistance-filter").val(_app.constants.assistance.absence.value).trigger("change");

            $("#assistance-filter").on("change", function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
                loadDetailDatatable(sectionId);
            });

            $("#absences-detail-modal").one("shown.bs.modal", function (e) {
                $("#assistance-filter").trigger("change");
            });

            $("#absences-detail-modal").one("hidden.bs.modal", function (e) {
                if (detailDatatable !== null) {
                    detailDatatable.destroy();
                    detailDatatable = null;
                }
                $("#assistance-filter").off("change");
            });
        }
    }

    var loadDetailDatatable = function (sectionId) {
        if (detailDatatable !== null) {
            detailDatatable.destroy();
            $("#absences-detail-datatable").empty();
            detailDatatable = null;
        }
        optionDetails.ajax.url = `/alumno/inasistencias/seccion/${sectionId}/get?filter=${$("#assistance-filter").val()}`.proto().parseURL();
        detailDatatable = $("#absences-detail-datatable").DataTable(optionDetails);
    }

    var loadDatatable = function () {
        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        options.data.source.read.url = `/alumno/inasistencias/periodo/${$("#select-term").val()}/get`.proto().parseURL();
        datatable = $(".m-datatable-absences").mDatatable(options);
        events.datatable.init();
    }


    var form = {
        create: {
            object: $("#create-form").validate({
                submitHandler: function (e) {
                    mApp.block("#create-modal .modal-content");

                    var formData = new FormData($(e)[0]);

                    $.ajax({
                        url: $(e).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        datatable.justifications.reload();
                        form.create.clear();
                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "") $("#create-form-alert-txt").html(error.responseText);
                        else $("#create-form-alert-txt").html(_app.constants.ajax.message.error);

                        $("#create-form-alert").removeClass("m--hide").show();
                    }).always(function () {
                        mApp.unblock("#create-modal .modal-content");
                    });
                }
            }),
            clear: function () {
                form.create.object.resetForm();
            },
            load: function (sectionId) {
                select.classes.load(sectionId);
                $("#create-modal").modal("toggle");
            },
            clear: function () {
                form.create.object.resetForm();

            }
        }
    };

    var select = {
        init: function () {
            this.classes.init();
        },
        classes: {
            init: function () {
                $(".select2-classes").select2();
            },
            load: function (sectionId) {
                $.ajax({
                    url: `/alumno/secciones/${sectionId}/inasistencias/get`.proto().parseURL()
                }).done(function (result) {
                    $(".select2-classes").empty();

                    $(".select2-classes").select2({
                        data: result.items,
                        placeholder: "Inasistencias",
                        dropdwonParent: $("#create-modal"),
                        minimumResultsForSearch: 10
                    });
                });
            }
        }
    }

    return {
        init: function () {
            loadDatatable();
            events.init();
            select.init();
        }
    }

}();

$(function () {
    Absences.init();
});
