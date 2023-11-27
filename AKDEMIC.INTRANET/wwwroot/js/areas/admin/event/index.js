var EventsTable = function () {
    var datatableStudents;
    var optionStudents = {
        data: {
            type: 'remote',
            source: {
                read: {
                    method: 'POST',
                    url: ('/admin/eventos/getEnrolled?id=' + $("#eventyId").val()).proto().parseURL()
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
                field: 'name',
                title: 'Nombre',
                width: 300
            }
        ]
    };

    var private = {
        objects: {}
    };

    var inputs = {
        init: function () {
            $("#search").doneTyping(function () {
                private.objects["tbl-data"].draw();
            });
        }
    };

    var datatable;


    var options = {
        columnDefs: [
            { "orderable": false, "targets": [1] }
        ],
        ajax: {
            type: "GET",
            dataType: "JSON",
            url: ('/admin/eventos/get').proto().parseURL(),

            data: function (values) {
                delete values.columns;
                values.searchValue = $("#search").val();
                values.eventTypeId = $("#eventType").val();
            }
        },
        columns: [

            { data: "nameEvent", title: "Nombre del Evento" },
            { data: "organizer", title: "Organizador" },
            { data: "type", title: "Tipo de Evento" },
            { data: "description", title: "Descripcion" },
            { data: "place", title: "Lugar" },
            { data: "eventDate", title: "Fecha del Evento" },
            { data: "registrationStartDate", title: "Inicio de Inscripciones" },
            { data: "registrationEndDate", title: "Fin de Inscripciones" },
            { data: "eventDate", title: "Fecha del Evento" },
            {
                data: null,
                title: "Opciones",
                width: "12%",
                render: function (data, type, row, meta) {
                    var tmp = "";
                    tmp += `<button data-id="${data.id}" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit"  title="Editar"><i class="la la-edit"></i></button> `;
                    tmp += `<button data-id="${data.id}" class="btn btn-success btn-sm m-btn m-btn--icon btn-enrolled"  title="Inscritos"><i class="la la-users"></i></button> `;
                    tmp += `<button data-id="${data.id}" class="btn btn-brand btn-sm m-btn m-btn--icon btn-asistence" title="Asistencia"><i class="flaticon-list-2"></i></button>`;
                    return tmp;

                }
            }
        ]
    };


    var initFormValidation = function () {
        formCreate = $("#create-form").validate();
    };
    var initFormDatepickers = function () {
        $("#cRegistrationStartDate").datepicker()
            .on("changeDate", function (e) {
                $("#cRegistrationEndDate").datepicker("setStartDate", e.date);
            });

        $("#cRegistrationEndDate").datepicker()
            .on("changeDate", function (e) {
                $("#cRegistrationStartDate").datepicker("setEndDate", e.date);
            });
        $("#cEventDate").datepicker();
    };
    var initModalEvents = function () {
        $("#create_modal").on("hidden.bs.modal",
            function () {
                $("#create_msg").addClass("m--hide");
                formCreate.resetForm();
            });
    };

    var events = {
        init: function () {

            private.objects["tbl-data"].on('click', '.btn-asistence', function () {
                var id = $(this).data('id');
                location.href = `/admin/eventos/asistencia/${id}`.proto().parseURL();
            });
            private.objects["tbl-data"].on('click', '.btn-edit', function () {
                var id = $(this).data('id');
                location.href = `/admin/eventos/editar/${id}`.proto().parseURL();
            });
            private.objects["tbl-data"].on('click', '.btn-enrolled', function () {

                var aux = $(this);
                var id = aux.data('id');
                location.href = `/admin/eventos/registrados/${id}`.proto().parseURL();
            });

        }
    };

    var select2 = function () {
        $("#eventType").on('change', function () {
            private.objects["tbl-data"].draw();
            events.init();
        })

        $.ajax({
            url: '/admin/eventos/tipo-eventos',
            data: {
                isAll: true
            }

        }).done(function (_data) {
            $("#eventType").select2({
                data: _data
            });

            private.objects["tbl-data"] = $("#ajax_data").DataTable(options);
            events.init();

        });

    };

    return {
        init: function () {
            inputs.init();
            select2();



            //datatableStudents = $(".m-datatable-students").mDatatable(optionStudents);          

            //initFormValidation();
            //initFormDatepickers();
            //initModalEvents();
        },
        reloadTable: function () {
            datatable.reload();
        }
    }
}();
//var DefaultAjaxFunctions = function () {
//    var beginAjaxCall = function () {
//        $(".btn-submit").each(function (index, element) {
//            $(this).addLoader();
//        });
//    };
//    var endAjaxCall = function () {
//        $(".btn-submit").each(function (index, element) {
//            $(this).removeLoader();
//        });
//    };
//    var ajaxSuccess = function () {
//        $("#create_modal").modal("hide");
//        $("#edit_modal").modal("hide");
//        EventsTable.reloadTable();
//        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
//    };
//    var createFailure = function (e) { 
//        toastr.success(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
//    };

//    return {
//        beginAjaxCall: function () {
//            beginAjaxCall();
//        },
//        endAjaxCall: function () {
//            endAjaxCall();
//        },
//        ajaxSuccess: function () {
//            ajaxSuccess();
//        },
//        createFailure: function (e) {
//            createFailure(e);
//        }
//    };
//}();


$(function () {
    EventsTable.init();
});