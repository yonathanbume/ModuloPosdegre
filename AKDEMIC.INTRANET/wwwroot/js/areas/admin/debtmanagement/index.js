var InitApp = function () {
    var datatable = {
        payments: {
            object: null,
            options: {
                data: {
                    source: {
                        read: {
                            method: "GET",
                            url: null
                        }
                    }
                },
                columns: [
                    {
                        field: "type",
                        title: "Tipo",
                        width: 100,
                        template: function (row) {
                            return `<span class="m-badge m-badge--brand m-badge--wide">${row.type}</span> <input hidden name="Type" value="${row.type}" />`;
                        }
                    },
                    {
                        field: "concept",
                        title: "Concepto"
                    },
                    {
                        field: "issueDate",
                        title: "Fecha emisión"
                    },
                    {
                        field: "totalamount",
                        title: "Monto Total",
                        textAlign: "right",
                        template: function (row) {
                            return "S/. " + row.totalamount.toFixed(2);
                        }
                    },
                    {
                        field: "user",
                        title: "Creado por"
                    },
                    {
                        field: "option",
                        title: " ",
                        width: 50,
                        //textAlign: "right",
                        template: function (row) {
                            if (row.isCreator) {
                                return `<button type="button" class="btn btn-danger m-btn btn-sm m-btn--icon m-btn--icon-only btn-delete" data-id="${row.id}"><i class="la la-trash"></i></button>`;
                            }
                            return ``;
                        }
                    }
                ]
            },
            load: function (code) {
                $("#empty-message").hide();
                $("#btn-new").show();

                if (datatable.payments.object !== undefined && $.trim($(".m-datatable").html()).length) datatable.payments.object.destroy();
                datatable.payments.options.data.source.read.url = `/admin/gestor-de-deudas/usuario/${code}/pendientes/get`.proto().parseURL();
                datatable.payments.object = $("#ajax_data").mDatatable(datatable.payments.options);

                $("#ajax_data").on('click', '.btn-delete', function (e) {
                    var id = $(this).data("id");

                    swal({
                        title: "¿Está seguro?",
                        //text: "Se exonerará el pago pendiente del usuario",
                        text: "Se eliminará el pago pendiente del usuario",
                        type: "warning",
                        showCancelButton: true,
                        //confirmButtonText: "Sí, exonerarlo",
                        confirmButtonText: "Sí, eliminarlo",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar"
                    }).then(function (result) {
                        if (result.value) {
                            $.ajax({
                                //url: "/admin/gestor-de-deudas/usuario/tramites/exonerar",
                                url: "/admin/gestor-de-deudas/usuario/conceptos/eliminar",
                                type: "POST",
                                data: {
                                    id: id
                                },
                                success: function () {
                                    datatable.payments.object.reload();
                                    //datatable.exonerated.reload();
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                },
                                error: function () {
                                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                }
                            });
                        }
                    });
                });
            },
            destroy: function () {
                if (datatable.payments.object !== undefined && $.trim($(".m-datatable").html()).length) datatable.payments.object.destroy();

                $("#empty-message").show();
                $("#btn-new").hide();
            }
        },
        concepts: {
            object: null,
            options: {
                serverSide: true,
                ajax: {
                    url: "/admin/gestor-de-deudas/conceptos/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.search = $("#search2").val();
                    }
                },
                pageLength: 8,
                orderable: [],
                columns: [
                    {
                        title: "Cuenta",
                        width: 80,
                        data: "classifier"
                    },
                    {
                        title: "Código",
                        width: 50,
                        data: "code"
                    },
                    {
                        title: "Descripción",
                        data: "description"
                    },
                    {
                        title: "Monto",
                        width: 70,
                        className: "text-right",
                        data: "amount",
                        render: function (data) {
                            return "S/. " + data.toFixed(2);
                        }
                    },
                    {
                        data: null,
                        width: 50,
                        orderable: false,
                        render: function (data) {
                            return `<div class="table-options-section">
                                <button data-description="${data.description}" data-id="${data.id}" class="btn btn-info btn-sm m-btn m-btn--icon m-btn--icon-only btn-confirmation" title=""><i class="la la-plus"></i></button>
                            </div>`;
                        }
                    }
                ]
            },
            init: function () {
                this.object = $("#concept-table").DataTable(this.options);

                $('#concept-table').on('click', '.btn-confirmation', function (e) {
                    var id = $(this).data("id");
                    var description = $(this).data("description");
                    datatable.concepts.confirmation(id, description);
                });
            },
            reload: function () {
                this.object.ajax.reload();
            },
            confirmation: function (id, description) {
                swal({
                    confirmButtonText: "Si",
                    input: 'text',
                    inputValue: description,
                    inputAttributes: {
                        autocapitalize: 'off'
                    },
                    inputValidator: (value) => {
                        if (!value) {
                            return 'Debes especificar una descripción'
                        }
                    },
                    cancelButtonText: "No",
                    showCancelButton: true,
                    title: "¿Desea generar el pago del concepto?",
                    text: "Se iniciará la solicitud del concepto seleccionado.Modifique el campo si así lo desea.",
                    type: "warning",
                    showLoaderOnConfirm: true,
                    preConfirm: (descriptionInput) => {
                        return new Promise((resolve) => {
                            console.log(descriptionInput);
                            $.ajax({
                                url: `/admin/gestor-de-deudas/usuario/${$("#user").val()}/conceptos/registrar`.proto().parseURL(),
                                type: "POST",
                                data: {
                                    conceptId: id,
                                    description: descriptionInput
                                },
                                success: function () {
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                    $("#concept_modal").modal("hide");

                                    $("#ajax_data").one("m-datatable--on-layout-updated", function () {
                                        $("[name='Payments']:last").click();
                                    });

                                    datatable.payments.object.reload();
                                },
                                error: function (e) {
                                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                },
                                complete: function () {
                                    swal.close();
                                }
                            });
                        });
                    },
                    allowOutsideClick: () => swal.isLoading()
                });
            }
        },
        exonerated: {
            object: null,          
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/admin/gestor-de-deudas/usuario/${$("#code").val()}/exonerados/get`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 10,
                orderable: [],
                columns: [
                    {
                        title: "Tipo",
                        data: "type"
                    },
                    {
                        title: "Concepto",
                        data: "concept"
                    },
                    {
                        title: "Fecha Emisión",
                        data: "issueDate"
                    },
                    {
                        title: "Fecha de Exoneración",
                        data: "paymentDate"
                    },
                    {
                        title: "Monto Total",
                        data: null,
                        render: function (data) {
                            return "S/. " + data.totalamount.toFixed(2);
                        }
                    },
                ],
            },
            load: function (code) {
                this.options.ajax.url = `/admin/gestor-de-deudas/usuario/${code}/exonerados/get`.proto().parseURL();
                if (this.object != null) {
                    this.object.destroy();
                }
                this.object = $("#exonerated-data-table").DataTable(this.options);
            },
            reload: function () {
                this.object.ajax.reload();
            }
        },
        init: function () {
            //datatable.concepts.init();
        }
    };

    var searchInput = {
        validate: function () {
            var code = $("#code_user").val();

            if (code === "") {
                datatable.payments.destroy();
                $("#info-portlet").addClass("m--hide");
                swal("Debe ingresar un usuario", "Debe ingresar un código de usuario primero.", "warning");
                return;
            }

            mApp.blockPage();

            $.ajax({
                type: "GET",
                url: `/admin/gestor-de-deudas/usuario/${code}/valida`.proto().parseURL()
            }).done(function (data) {
                $("#user").val(code);
                datatable.payments.load(code);
                datatable.exonerated.load(code);

                $("#info-portlet").removeClass("m--hide");
                $("#code").val(data.code);
                $("#fullname").val(data.name);
                $("#dni").val(data.dni);

                $("#user-code").val(code);
            }).fail(function () {
                datatable.payments.destroy();
                $("#info-portlet").addClass("m--hide");
                swal("Usuario incorrecto", "El código de usuario ingresado no existe.", "error");
            }).always(function () {
                mApp.unblockPage();
            });
        },
        init: function () {
            $("#code_search").on("click", function () {
                searchInput.validate();
            });
            $("#code_user").on("keypress", function (e) {
                if (e.keyCode === 13) searchInput.validate();
            });
        }
    };

    var events = {
        init: function () {
            $("#search2").doneTyping(function () {
                datatable.concepts.reload();
            });

            $('#concept_modal').on('shown.bs.modal', function () {
                $(document).off('focusin.modal');
            });
          
            $('#concept_modal').on('shown.bs.modal', function (e) {
                //datatable.concepts.object.columns.adjust().draw();

                $("#concept-select").val(null).trigger("change");
            });
        }
    };

    var select = {
        search: {
            init: function () {
                $("#user-select").select2({
                    dropdownParent: $("#users_modal"),
                    width: "100%",
                    placeholder: "Buscar...",
                    ajax: {
                        url: "/admin/gestor-de-deudas/buscar".proto().parseURL(),
                        dataType: "json",
                        data: function (params) {
                            return {
                                term: params.term,
                                page: params.page
                            };
                        },
                        processResults: function (data, params) {
                            return {
                                results: data.items
                            };
                        },
                        cache: true
                    },
                    escapeMarkup: function (markup) {
                        return markup;
                    },
                    minimumInputLength: 3
                });

                $("#btn-user").on("click", function () {
                    select.search.load();
                });
            },
            load: function () {
                var code = $("#user-select").val();

                if (code === "") {
                    swal("Seleccione un usuario", "Debe buscar y seleccionar un usuario primero", "warning");
                    return false;
                }
                $("#users_modal").modal("hide");

                $("#code_user").val(code);
                $("#user-code").val(code);
                searchInput.validate();
            }
        },
        concepts: {
            init: function () {

                $("#concept-select").select2({
                    width: "100%",
                    placeholder: "Buscar...",
                    ajax: {
                        delay: 1000,
                        url: "/conceptos/buscar".proto().parseURL(),
                        dataType: "json",
                        data: function (params) {
                            return {
                                term: params.term,
                                page: params.page
                            };
                        },
                        processResults: function (data) {
                            return {
                                results: //data.items
                                    $.map(data.items, function (item) {
                                        return {
                                            text: item.text,
                                            accountingplan: item.accountingplan,
                                            fixed: item.isFixed,
                                            taxed: item.isTaxed,
                                            price: item.price,
                                            id: item.id,
                                            accountId: item.accountId
                                        }
                                    })
                            }
                        },
                        cache: true
                    },
                    escapeMarkup: function (markup) {
                        return markup;
                    },
                    minimumInputLength: 3
                });

                $("#concept-select").change(function () {
                    var id = $("#concept-select").val();
                    console.log(id);

                    if (id != null && id != "") {
                        var data = $(this).select2('data');

                        $("#concept-id").val(data[0].id);
                        $("#concept-name").val(data[0].text);
                        $("#concept-price").val(data[0].price);
                    }
                });
            }
        }
    };

    var form = {
        create: {
            object: $("#concept-form").validate({
                submitHandler: function (e) {
                    mApp.block("#concept_modal .modal-content");

                    $.ajax({
                        url: $(e).attr("action"),
                        type: "POST",
                        data: $(e).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");

                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        datatable.payments.object.reload();
                        var userCode = $("#user-code").val();
                        form.create.clear();
                        $("#user-code").val(userCode);
                    }).fail(function (error) {
                        if (error.responseText !== null && error.responseText !== "") $("#create-alert-txt").html(error.responseText);
                        else $("#create-alert-txt").html(_app.constants.ajax.message.error);

                        $("#create-alert").removeClass("m--hide").show();
                    }).always(function () {
                        mApp.unblock("#concept_modal .modal-content");
                    });
                }
            }),
            clear: function () {
                this.object.resetForm();
                $("#concept-select").val(null).trigger("change");
            }
        }
    };

    return {
        init: function () {
            searchInput.init();
            datatable.init();
            events.init();
            select.search.init();
            select.concepts.init();
        },
        adjustConcepts: function () {
            //datatable.concepts.object.columns.adjust().draw();
        }
    };
}();

$(function () {
    InitApp.init();
});