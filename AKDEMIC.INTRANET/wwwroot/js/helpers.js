// ----------
// Datatable
// ----------

$.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
    toastr.error("Ocurrió un problema durante el proceso", "Error");
};

function getOldDataTableConfiguration(configuration) {
    return {
        search: {
            input: configuration.input
        },
        data: {
            type: "remote",
            source: {
                read: {
                    method: "GET",
                    url: (configuration.url).proto().parseURL(),
                    params: configuration.params,
                    map: function (raw) {
                        // sample data mapping

                        var dataSet = raw;
                        if (typeof raw.data !== 'undefined') {
                            console.log(raw.data);
                            dataSet = raw.data;
                        }
                        return dataSet;
                    }
                }
            },
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true
        },
        pagination: true,
        toolbar: {
            items: {
                pagination: {
                    pageSizeSelect: [10, 15, 20, 25, 30]
                },
                info: true
            }
        },
        columns: configuration.columns
    }
}

function getDataTableConfiguration(configuration) {
    return {
        dom: '<"top"i>rt<"bottom"flp><"clear">',
        responsive: true,
        processing: true,
        serverSide: true,
        filter: false,
        lengthChange: true,
        ordering: true,
        orderMulti: false,
        pagingType: "full_numbers",
        columnDefs: [
            { "orderable": false, "targets": configuration.orderable }
        ],
        language: {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>",
            //"lengthMenu": _app.sharedLocalizer.dataTable.tableLengthMenu,
            //"infoEmpty": _app.sharedLocalizer.dataTable.tableInfoEmpty,
            //"zeroRecords": _app.sharedLocalizer.dataTable.tableInfoEmpty,
            //"info": _app.sharedLocalizer.dataTable.tableInfo,
            "lengthMenu": "Mostrar _MENU_ registros por página",
            "infoEmpty": "No se encontraron registros",
            "zeroRecords": "No se encontraron registros",
            "info": "Mostrando la página _PAGE_ de _PAGES_ - Se encontraron _TOTAL_ registros",
            "infoFiltered": "_MAX_ / _TOTAL_",
            "paginate": {
                //"next": _app.sharedLocalizer.dataTable.next,
                //"previous": _app.sharedLocalizer.dataTable.previous,
                //"first": _app.sharedLocalizer.dataTable.first,
                //"last": _app.sharedLocalizer.dataTable.last
                "next": "Siguiente",
                "previous": "Anterior",
                "first": "Inicial",
                "last": "Final"
            }
        },
        ajax: {
            url: (configuration.url).proto().parseURL(),
            type: "GET",
            dataType: "JSON",
            data: configuration.data
        },
        columns: configuration.columns
    };
}

function getSimpleDataTableConfiguration(configuration) {
    return {
        dom: configuration.dom !== undefined ? configuration.dom : 'rt<"bottom"flp><"clear">',
        responsive: true,
        processing: true,
        serverSide: configuration.serverSide !== undefined ? configuration.serverSide : true,
        filter: false,
        lengthChange: false,
        pageLength: configuration.pageLength !== undefined ? configuration.pageLength : 5,
        ordering: configuration.ordering !== undefined ? configuration.ordering : true,
        orderMulti: false,
        order: [],
        columnDefs: [
            { "orderable": false, "targets": configuration.orderable }
            //{ "width": "10%", "targets": 3 }
        ],
        language: {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>",
            "lengthMenu": "",
            "infoEmpty": "",
            "zeroRecords": "No existen registros",
            //"info": "",
            "infoFiltered": "_MAX_ / _TOTAL_",
            "paginate": {
                "next": ">>",
                "previous": "<<"
            }
        },
        ajax: {
            url: configuration.url,
            type: "GET",
            dataType: "JSON",
            data: configuration.data
        },
        columns: configuration.columns
    };
}


// ----------
// SweetAlert2
// ----------

function showDeleteSwal(configuration) {
    swal({
        type: "warning",
        title: "¿Está seguro?",
        text: "El registro será eliminado permanentemente",
        showCancelButton: true,
        confirmButtonText: "Sí, elmininar",
        cancelButtonText: "Cancelar",
        showLoaderOnConfirm: true,
        allowOutsideClick: () => !swal.isLoading(),
        preConfirm: configuration.promise
    });
}

function showErrorSwal(text = _app.sharedLocalizer.general.relatedInfo) {
    text = (text == "" || text == null || text == undefined) ? _app.sharedLocalizer.general.relatedInfo : text;
    swal({
        type: "error",
        title: "Error",
        text: text,
        confirmButtonText: _app.sharedLocalizer.general.ok
    });
}

//function showSuccessSwal(text = _app.sharedLocalizer.general.processSuccess) {
function showSuccessSwal(text = null) {
    //text = (text == "" || text == null || text == undefined) ? _app.sharedLocalizer.general.processSuccess : text;
    swal({
        type: "success",
        title: "Eliminado",
        text: "Se elimino correctamente el registro",
        confirmButtonText: "Cerrar"
    });
}

// ----------
// Toastr
// ----------

function showToastrFail(text = _app.sharedLocalizer.general.processError) {
    text = (text == "" || text == null || text == undefined) ? _app.sharedLocalizer.general.processError : text;
    toastr.error(text, _app.constants.toastr.title.error);
}

function showToastrSuccess(text = _app.sharedLocalizer.general.processSuccess) {
    text = (text == "" || text == null || text == undefined) ? _app.sharedLocalizer.general.processSuccess : text;
    toastr.success(text, _app.sharedLocalizer.swal.completed);
}
function getDataTableConfigurationPortal(configuration) {
    return {
        dom: '<"top"i>rt<"bottom"flp><"clear">',
        responsive: true,
        processing: false,
        serverSide: false,
        filter: false,
        lengthChange: false,
        ordering: false,
        orderMulti: false,
        pagingType: "full_numbers",
        bPaginate: false,
        info: false,
        columnDefs: configuration.columndefs,
        language: {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>",
            "lengthMenu": "Mostrar _MENU_ registros por página",
            "infoEmpty": "No se encontraron registros",
            "zeroRecords": "No se encontraron registros",
            "info": "Mostrando la página _PAGE_ de _PAGES_ - Se encontraron _TOTAL_ registros",
            "infoFiltered": "_MAX_ / _TOTAL_",
            "paginate": {
                "next": "Siguiente",
                "previous": "Anterior",
                "first": "Inicial",
                "last": "Final"
            }
        },
        ajax: {
            url: (configuration.url).proto().parseURL(),
            type: "GET",
            dataType: "JSON",
            data: configuration.data
        },
        columns: configuration.columns,
        drawCallback: configuration.draw

    };
}
// ----------
// Loader
// ----------

function showPageLoaderForLoading(targetClass = ".m-content") {
    mApp.block(targetClass, { type: "loader", message: _app.sharedLocalizer.general.loaderLoading });
}

function showPageLoaderForSaving(targetClass = ".m-content") {
    //mApp.block(targetClass, { type: "loader", message: _app.sharedLocalizer.general.loaderSaving });
    mApp.block(targetClass, { type: "loader", message: "Se completo el proceso" });
}

function hidePageLoader(targetClass = ".m-content") {
    mApp.unblock(targetClass);
}

function showModalLoaderForLoading(targetClass = ".modal-content") {
    mApp.block(targetClass, { type: "loader", message: _app.sharedLocalizer.general.loaderLoading });
}

function showModalLoaderForSaving(targetClass = ".modal-content") {
    mApp.block(targetClass, { type: "loader", message: _app.sharedLocalizer.general.loaderSaving });
}

function hideModalLoader(targetClass = ".modal-content") {
    mApp.unblock(targetClass);
}

// ----------
// Status
// ----------

function getStatusMessage(value) {
    return value == 1 ? _app.sharedLocalizer.general.active : _app.sharedLocalizer.general.inactive;
}

// ----------
// Input Autocomplete
// ----------

$(document).on('focus', ':input', function () {
    $(this).attr('autocomplete', 'off');
});

// ----------
// Input after typing
// ----------


$.fn.extend({
    doneTyping: function (callback, timeout) {
        //timeout = timeout || 1e3;
        timeout = timeout || 800;
        let minimunLenght = 4;
        var timeoutReference,
            timeoutReferenceValidate,
            validate = function (el) {
                var input = $(el).parent().parent().find(".general_search_input");
                var quantityNeeded = minimunLenght - $(el).val().length;
                var text = ((quantityNeeded !== minimunLenght && quantityNeeded > 0) || quantityNeeded > minimunLenght) ?
                    `Por favor, introduzca ${quantityNeeded} ${(quantityNeeded === 1 ?'caracter':'caracteres')}` : "";

                if (input.length <= 0) {
                    $(el).parent().parent().append(`<span class='m-form__help general_search_input'></span>`);
                    input = $(el).parent().parent().find(".general_search_input");
                    input.css({ "color": "#7b7e8a", "font-weight": "300", "font-size": ".85rem", "padding-top": "7px", "display": "inline-block" });
                }

                var isValid = (quantityNeeded === minimunLenght || quantityNeeded <= 0) ? true : false;
                input.text(text); isValid ? $(input).addClass("d-none") : $(input).removeClass("d-none");
                return isValid;
            },
            doneTyping = function (el) {
                if (!timeoutReference) return;
                timeoutReference = null;
                if ($(el).val().length >= minimunLenght || $(el).val().length === 0)
                    callback.call(el);
            };

        return this.each(function (i, el) {
            var $el = $(el);
            // Chrome Fix (Use keyup over keypress to detect backspace)
            // thank you @palerdot
            $el.is(':input') && $el.on('keyup keypress paste', function (e) {
                // This catches the backspace button in chrome, but also prevents
                // the event from triggering too preemptively. Without this line,
                // using tab/shift+tab will make the focused element fire the callback.
                if (e.type === 'keyup' && e.keyCode !== 8) return;
                if (timeoutReferenceValidate) clearTimeout(timeoutReferenceValidate);
                timeoutReferenceValidate = setTimeout(function () {
                    //validate that the input has the necessary number of characters
                    if (!validate($el)) return;
                    if (timeoutReference) clearTimeout(timeoutReference);
                    timeoutReference = setTimeout(function () {
                        // if we made it here, our timeout has elapsed. Fire the
                        // callback
                        doneTyping(el);
                    }, timeout);
                }, timeout / 5);
                // Check if timeout has been set. If it has, "reset" the clock and
                // start over again.

            }).on('blur', function () {
                // If we can, fire the event since we're leaving the field
                doneTyping(el);
            });
        });
    }
});


