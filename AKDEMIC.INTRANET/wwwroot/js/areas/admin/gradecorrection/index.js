var InitApp = function () {

    var datatable = {
        requests: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/correccion-notas/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.id = $("#select_term").val();
                        data.searchValue = $("#search").val();
                        data.state = $("#state-select").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        data: "course",
                        title: "Curso"
                    },
                    {
                        data: "section",
                        title: "Sección"
                    },
                    {
                        data: "code",
                        title: "Código"
                    },
                    {
                        data: "student",
                        title: "Estudiante"
                    },
                    {
                        data: "evaluation",
                        title: "Evaluación"
                    },
                    {
                        data: "grade",
                        title: "Nueva Nota"
                    },
                    {
                        data: null,
                        title: "Estado",
                        orderable: false,
                        render: function (row) {
                            if (row.termActive) {
                                switch (row.state) {
                                    case 1:
                                        return `<span class="m-badge m-badge--metal m-badge--wide">Pendiente</span>`;
                                    case 2:
                                        return `<span class="m-badge m-badge--primary m-badge--wide">Aprobado</span>`;
                                    case 3:
                                        return  `<span class="m-badge m-badge--danger m-badge--wide">Rechazado</span>`;
                                    case 4:
                                        return `<span class="m-badge m-badge--warning m-badge--wide">Solicitado</span>`;
                                    default:
                                        return "-";
                                }
                            } else {
                                switch (row.state) {
                                    case 2:
                                        return "<span class='m-badge m-badge--success m-badge--wide'> Aprobado </span>";
                                    case 3:
                                        return "<span class='m-badge m-badge--danger m-badge--wide'> Rechazado </span>";
                                    default:
                                        return "<span class='m-badge m-badge--metal m-badge--wide'> Vencido </span>";
                                }
                            }
                         
                        }
                    },
                    {
                        data: null,
                        title: " ",
                        width: 100,
                        orderable: false,
                        render: function (row) {
                            var html = `<button class="btn btn-info m-btn btn-sm m-btn--icon m-btn--icon-only btn-detail" data-object="${row.proto().encode()}" title="Ver detalle"><i class="la la-eye"></i></button> `;

                            if (row.state == 1 && row.termActive) {
                                html += `<button class="btn btn-success m-btn btn-sm m-btn--icon m-btn--icon-only approve" data-id="${row.id}" title="Aprobar"><i class="la la-check"></i></button> `;
                                html += `<button class="btn btn-danger m-btn btn-sm m-btn--icon m-btn--icon-only disapprove" data-id="${row.id}" title="Rechazar"><i class="la la-remove"></i></button>`;
                            }

                            return html;
                        }
                    }
                ]
            },
            init: function () {
                this.object = $("#ajax_data").DataTable(this.options);

                this.object.on("click", ".approve", function () {
                    var id = $(this).data("id");
                    form.approve.load(id, this);
                });
                this.object.on("click", ".disapprove", function () {
                    var id = $(this).data("id");
                    form.disapprove.load(id, this);
                });
                this.object.on("click", ".btn-detail", function () {
                    var object = $(this).data("object");
                    object = object.proto().decode();


                    $("#observations").val(object.observations);

                    $("#prev-grade").val(object.prevGrade);
                    $("#new-grade").val(object.grade);
                    $("#date").val(object.date);
                    $("#teacher").val(object.teacher);
                    $("#preview_notTaken").attr("checked", object.notTaken);


                    if (object.state == 2 || object.state == 3) {
                        $("#updatedby").val(object.approvedBy);
                    } else {
                        $("#updatedby").val("");
                    }

                    if (object.file != null && object.file != "") {
                        var url = `/admin/correccion-notas/archivo/${object.file}`.proto().parseURL();
                        $("#file-url").prop("href", url);
                        $("#file-container").show();
                    }
                    else {
                        $("#file-container").hide();
                    }

                    $("#request_modal").modal("show");
                });
            },
            reload: function () {
                this.object.ajax.reload();
            }
        }
    };

    var form = {
        approve: {
            load: function (id, element) {
                swal({
                    title: "Confirmar aprobación de la solicitud",
                    text: "Se aprobará la solicitud indicada y se cambiará la nota de manera automática",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Si, aprobarla",
                    confirmButtonClass: "btn btn-success",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: "/admin/correccion-notas/post",
                                type: "POST",
                                data: {
                                    Id: id,
                                    Status: true
                                },
                                success: function () {
                                    //datatable.requests.reload();

                                    $(element).parent().parent().children("td")[6].innerHTML = "<span class='m-badge m-badge--success m-badge--wide'> Aprobado </span>";
                                    $(element).parent().parent().children("td").eq(7).children("button.disapprove").remove();
                                    $(element).parent().parent().children("td").eq(7).children("button.approve").remove();          

                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                },
                                error: function (e) {
                                    toastr.error(e.responseText, _app.constants.toastr.title.error);
                                },
                                complete: function () {
                                    swal.close();
                                }
                            });
                        });
                    }
                });
            }
        },
        disapprove: {
            load: function (id, element) {
                swal({
                    title: "Rechazar la solicitud",
                    text: "Se rechazará la solicitud indicada ¿Desea continuar?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Si, rechazar",
                    confirmButtonClass: "btn btn-danger",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: "/admin/correccion-notas/post",
                                type: "POST",
                                data: {
                                    Id: id,
                                    Status: false
                                },
                                success: function () {
                                    //datatable.requests.reload();

                                    $(element).parent().parent().children("td")[6].innerHTML = "<span class='m-badge m-badge--danger m-badge--wide'> Rechazado </span>";
                                    $(element).parent().parent().children("td").eq(7).children("button.approve").remove();    
                                    $(element).parent().parent().children("td").eq(7).children("button.disapprove").remove();                                          

                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                },
                                error: function () {
                                    toastr.error(e.responseText, _app.constants.toastr.title.error);
                                },
                                complete: function () {
                                    swal.close();
                                }
                            });
                        });
                    }
                });
            }
        }
    };

    var select = {
        term: {
            init: function () {
                $.ajax({
                    url: "/periodos/get".proto().parseURL()
                }).done(function (data) {
                    $("#select_term").select2({
                        data: data.items
                    });

                    if (data.selected !== null) {
                        $("#select_term").val(data.selected);
                        $("#select_term").trigger("change.select2");
                    }

                    datatable.requests.init();

                    $("#select_term").on("change", function () {
                        if ($("#select_term").val() !== null) {
                            datatable.requests.reload();
                        }
                    });
                }).fail(function () {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                });
            }
        },
        states: {
            init: function () {
                $("#state-select").select2();

                $("#state-select").on("change", function () {
                    datatable.requests.reload();
                });
            }
        }
    };

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.requests.reload();
            });
        }
    }

    return {
        init: function () {
            select.term.init();
            select.states.init();

            search.init();
        },
        reloadTable: function () {
            datatable.requests.reload();
        }
    };
}();

$(function () {
    InitApp.init();
});

