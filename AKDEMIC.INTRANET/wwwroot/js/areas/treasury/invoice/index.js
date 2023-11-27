var UserProcedureTable = function () {
    var datatable;
    var options = {
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
                field: "id",
                title: "#",
                sortable: false,
                width: 40,
                textAlign: "center",
                selector: { class: "m-checkbox--solid m-checkbox--brand", name: "UserProcedures" }
            },
            {
                field: "concept",
                title: "Concepto"
            },
            {
                field: "status",
                title: "Estado"
            },
            {
                field: "totalamount",
                title: "Monto Total",
                textAlign: "right",
                template: function (row) {
                    return "S/. " + row.totalamount;
                }
            },
            {
                field: "paidamount",
                title: "Monto Pagado",
                textAlign: "right",
                template: function (row) {
                    return "S/. " + row.paidamount;
                }
            },
            {
                field: "pendingamount",
                title: "Monto Pendiente",
                textAlign: "right",
                template: function (row) {
                    return "S/. " + row.pendingamount;
                }
            }
        ]
    }

    var validaUsuario = function () {
        var code = $("#code_user").val();

        if (code === "") {
            UserProcedureTable.detroy();
            swal("Debe ingresar un usuario", "Debe ingresar un código de usuario primero.", "warning");
            return;
        }

        mApp.blockPage();

        $.ajax({
            type: "GET",
            url: ("/pagos/usuario/get/" + code).proto().parseURL()
        }).done(function () {
            $("#user").val(code);
            UserProcedureTable.init(code);
        }).fail(function () {
            UserProcedureTable.detroy();
            swal("Usuario incorrecto", "El código de usuario ingresado no existe.", "error");
        }).always(function () {
            mApp.unblockPage();
        });
    }

    var codeEvents = function() {
        $("#code_search").on("click", function () {
            validaUsuario();
        });
        $("#code_user").on("keypress", function (e) {
            if (e.keyCode === 13) validaUsuario();
        });
    }
    
    return {
        init: function (code) {
            $("#empty-message").hide();
            $("#btn-new").show();

            if (datatable !== undefined && $.trim($(".m-datatable").html()).length) datatable.destroy();
            
            options.data.source.read.url = ("/pagos/usuario/tramites/get/" + code).proto().parseURL();

            datatable = $("#ajax_data").mDatatable(options);
            datatable.on("m-datatable--on-check m-datatable--on-uncheck m-datatable--on-layout-updated", function (e) {
                var checkedNodes = datatable.rows(".m-datatable__row--active").nodes();
                var count = checkedNodes.length;

                $("#m_datatable_selected_number").html(count);
                if (count > 0) {
                    $("#m_datatable_group_action_form").collapse("show");
                } else {
                    $("#m_datatable_group_action_form").collapse("hide");
                }
            });
        },
        reload: function () {
            datatable.reload();
        },
        detroy: function () {
            if (datatable !== undefined && $.trim($(".m-datatable").html()).length) datatable.destroy();

            $("#empty-message").show();
            $("#btn-new").hide();
        },
        codeEvents: function() {
            codeEvents();
        }
    }
}();

var ProcedureListTable = function () {

    var datatable;
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: "/pagos/tramites/get".proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: "category",
                title: "Categoría"
            },
            {
                field: "name",
                title: "Nombre"
            },
            {
                field: "totalamount",
                title: "Monto total"
            },
            {
                field: "options",
                title: "Opciones",
                template: function(row) {
                    return '<button onclick=ProcedureListTable.initProcedure("' + row.id + '") class="btn btn-outline-info btn-sm m-btn m-btn--icon"><span><i class="la la-file-text"></i><span> Iniciar </span></span></button>';
                }

            }
        ],
        translate: {
            records: {
                noRecords: "No existen trámites disponibles"
            }
        }
    }

    var initProcedure = function (id) {
        swal({
            confirmButtonText: "Si",
            cancelButtonText: "No",
            showCancelButton: true,
            title: "¿Desea solicitar el trámite?",
            text: "Se iniciará el trámite para el usuario seleccionado.",
            type: "warning",
            showLoaderOnConfirm: true,
            preConfirm: () => {
                return new Promise((resolve) => {
                    $.ajax({
                        url: "/pagos/usuario/tramite/registrar".proto().parseURL(),
                        type: "POST",
                        data: {
                            ProcedureId: id,
                            UserName: $("#user").val()
                        },
                        success: function (result) {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#procedure_modal").modal("hide");

                            UserProcedureTable.reload();
                        },
                        error: function (errormessage) {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        },
                        complete: function () {
                            swal.close();
                        }
                    });
                });
            },
            allowOutsideClick: () => !swal.isLoading()
        });
    }

    return {
        init: function () {
            datatable = $("#procedures_table").mDatatable(options);
        },
        destroy: function () {
            datatable.destroy();
        },
        initProcedure: function(id) {
            initProcedure(id);
        }

    };

}();

jQuery(document).ready(function () {
    UserProcedureTable.codeEvents();
    ProcedureListTable.init();
});