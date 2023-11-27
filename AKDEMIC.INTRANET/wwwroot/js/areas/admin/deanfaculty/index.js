var DeanFacultyManagement = function () {
    var select = function () {
        $("#DeanId").select2({
            dropdownParent: $("#edit_term_modal"),
            width: "100%",
            ajax: {
                url: "/admin/gestion-de-decanos/obtener-usuarios".proto().parseURL(),
                dataType: "json",
                data: function (params) {
                    return {
                        term: params.term,
                        page: params.page
                    };
                },
                processResults: function (data, params) {
                    return {
                        results: data.items.results
                    };
                },
                cache: true
            },
            escapeMarkup: function (markup) {
                return markup;
            }
        });
        $("#SecretaryId").select2({
            dropdownParent: $("#edit_term_modal"),
            width: "100%",
            ajax: {
                delay: 1000,
                url: (`/administrativos`).proto().parseURL(),
                data: function (params) {
                    var query = {
                        term: params.term,
                        page: params.page || 1
                    };
                    return query;
                }
            },
            allowClear: true,
            minimumInputLength: 0,
            placeholder: "Seleccione..."
        });
        $("#AdministrativeAssistantId").select2({
            dropdownParent: $("#edit_term_modal"),
            width: "100%",
            ajax: {
                delay: 1000,
                url: (`/administrativos`).proto().parseURL(),
                data: function (params) {
                    var query = {
                        term: params.term,
                        page: params.page || 1
                    };
                    return query;
                }
            },
            allowClear: true,
            minimumInputLength: 0,
            placeholder: "Seleccione..."
        });
    };
    var inputs = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.reload();
            });
        }
    };
    var datatable = {
        object: null,
        options: {
            ajax: {
                url: "/admin/gestion-de-decanos/obtener-decanos".proto().parseURL(),
                data: function (data) {
                    delete data.columns;
                    data.searchValue = $("#search").val();
                },
            },
            columns: [
                { data: "name", title: "Facultad" },
                { data: "deanGrade", title: "Grado" },
                { data: "dean.fullName", title: "Decano" },
                { data: "secretary.fullName", title: "Secretario" },
                { data: "administrativeAssistant.fullName", title: "Asistente administrativo" },

                {
                    data: null, title: "Opciones",
                    render: function (data, type, row, meta) {
                        var tpm = "";
                        if (data.deanResolutionFile != null && data.deanResolutionFile != "") {
                            tpm += ` <a href='/file/${data.deanResolutionFile}' target='_blank' class="btn btn-primary btn-sm m-btn m-btn--icon"><span><i class="la la-download"></i><span>Resolución</span></span></a>`;
                        }
                        tpm += ` <button data-id="${data.id}" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit" title="Asignar"><span><i class="la la-eye"></i><span>Asignar</span></span></button>`;
                        tpm += ` <button data-id="${data.id}" class="btn btn-success btn-sm m-btn m-btn--icon btn-history" title="Historial de Encargados"><span><i class="la la-history"></i><span>Historial</span></span></button>`;
                        return tpm;
                        //return `<button data-toggle="modal" data-target="#EditDeanFacultyModal" data-id="${data.id}" class="btn btn-sm btn-info edit"> <i class= "fa fa-edit"></i> </button>
                        //<button data-id="${data.id}" class="btn btn-sm btn-danger delete"> <i class="fa fa-trash"></i> </button>`;
                    }
                }
            ]
        },
        init: function () {
            this.object = $("#tbl-data").DataTable(this.options);

            $('#tbl-data').on('click', '.btn-edit', function (e) {
                var id = $(this).data("id");

                form.edit.load(id);
            });

            $('#tbl-data').on("click",
                ".btn-history",
                function () {
                    var id = $(this).data("id");
                    history.load(id);
                });
        },
        reload: function () {
            this.object.ajax.reload();
        }
    };
    var form = {
        edit: {
            object: $("#edit-form").validate({
                submitHandler: function (e) {
                    mApp.block("#edit_term_modal .modal-content");
                    var formData = new FormData(e);

                    $.ajax({
                        url: $(e).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    })
                        .done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");

                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                            datatable.reload();
                            form.edit.clear();
                        })
                        .fail(function (error) {
                            if (error.responseText !== null && error.responseText !== "") $("#edit-form-alert-txt").html(error.responseText);
                            else $("#edit-form-alert-txt").html(_app.constants.ajax.message.error);

                            $("#edit-form-alert").removeClass("m--hide").show();
                        })
                        .always(function () {
                            mApp.unblock("#edit_term_modal .modal-content");
                        });
                }
            }),
            load: function (id) {
                $.ajax({
                    url: `/admin/gestion-de-decanos/obtener-decano/${id}`,
                    type: "GET",
                })
                    .done(function (result) {
                        $('#Id').val(id);
                        if (result.dean != undefined && result.dean != null) {
                            var newOption = new Option(result.dean.fullName, result.deanId, false, false);
                            $('#DeanId').append(newOption);
                            $('#DeanId').val(result.deanId).trigger('change');
                        }
                        if (result.secretary != undefined && result.secretary != null) {
                            var newOption2 = new Option(result.secretary.fullName, result.secretaryId, false, false);
                            $("#SecretaryId").append(newOption2);
                            $("#SecretaryId").val(result.secretaryId).trigger('change');
                        }
                        if (result.administrativeAssistant != undefined && result.administrativeAssistant != null) {
                            var newOption = new Option(result.administrativeAssistant.fullName, result.administrativeAssistantId, false, false);
                            $('#AdministrativeAssistantId').append(newOption);
                            $('#AdministrativeAssistantId').val(result.administrativeAssistantId).trigger('change');
                        }

                        if ((result.dean == undefined || result.dean == null)){
                            $('#DeanId').val(null).trigger('change');
                        }
                        if ((result.secretary == undefined || result.secretary == null)) {
                            $('#SecretaryId').val(null).trigger('change');
                        }
                        if ((result.administrativeAssistant == undefined || result.administrativeAssistant == null)) {
                            $('#AdministrativeAssistantId').val(null).trigger('change');
                        }

                        $("#DeanGrade").val(result.deanGrade);
                        $("#edit-form").find("[name='Resolution']").val(result.deanResolution);
                        $("#edit_term_modal").modal("toggle");
                    });
            },
            clear: function () {
                form.edit.object.resetForm();
                $(".custom-file-label").text("Seleccione un archivo");
                $("[name='ResolutionFile']").val(null).trigger("change");
            }
        }
    };
    var history = {
        load: function (id) {
            $("#history_modal").modal("toggle");
            var url = `/admin/gestion-de-decanos/historial?id=${id}`.proto().parseURL();
            var newoptions = history.options;
            newoptions.ajax.url = url;
            if (history.object != null) {
                history.object.destroy();
            }
            history.object = $("#history_table").DataTable(newoptions);
        },
        object: null,
        options: {
            serverSide: false,
            filter: false,
            lengthChange: false,
            //pageLength: 50,
            paging: false,
            ajax: {
                url: `/admin/carreras/historial?id={id}`.proto().parseURL(),
                type: "GET"
            },
            columns: [
                {
                    data: 'date',
                    title: 'Fecha inicio',
                },
                {
                    data: 'dateEnd',
                    title: 'Fecha fin',
                },
                {
                    data: 'dean',
                    title: 'Decano',
                },
                {
                    data: 'secretary',
                    title: 'Secretario',
                },
                {
                    data: 'resolution',
                    title: 'Resolución',
                },
                {
                    data: null,
                    title: 'Archivo Res.',
                    render: function (data) {
                        var tpm = "";

                        if (data.resolutionFile != null && data.resolutionFile != '') {
                            tpm += ` <a href='/file/${data.resolutionFile}' target='_blank' class="btn btn-primary btn-sm m-btn m-btn--icon"><span><i class="la la-download"></i><span>Descargar</span></span></a>`;
                        }
                        else
                            tpm = "Sin archivo";
                        return tpm;
                    }
                },
            ]
        },
    };

    return {
        init: function () {
            datatable.init();
            inputs.init();
            //validate.add();
            //validate.edit();
            select();
        }
    };
}();

$(function () {
    DeanFacultyManagement.init();
});