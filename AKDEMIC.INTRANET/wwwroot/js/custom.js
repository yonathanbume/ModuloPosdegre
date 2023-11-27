var _app = typeof _app !== "undefined" ? _app : {};

// ----------
// Config
// ----------
_app.constants = {
    academicOrder: {
        none: {
            value: 1,
            text: "-"
        },
        upperThird: {
            value: 2,
            text: "Tercio Superior"
        },
        upperFifth: {
            value: 3,
            text: "Quinto Superior"
        },
        upperTenth: {
            value: 4,
            text: "Décimo Superior"
        },
        upperHalf: {
            value: 5,
            text: "Medio Superior"
        }
    },
    ajax: {
        message: {
            error: "Ocurrió un problema al procesar su consulta",
            success: "Tarea realizada satisfactoriamente"
        },
        status: {
            beforeSend: 0,
            success: 1,
            error: 2,
            complete: 3
        }
    },
    assistance: {
        assisted: {
            value: 1,
            text: "Asistido"
        },
        absence: {
            value: 0,
            text: "Falta"
        },
        late: {
            value: 2,
            text: "Tardanza"
        }
    },
    broadcastMedium: {
        internet: {
            value: 1,
            text: "Internet"
        },
        socialNetworks: {
            value: 2,
            text: "Redes Sociales"
        },
        familyAndFriends: {
            value: 3,
            text: "Familia y/o amigos"
        },
        other: {
            value: 4,
            text: "Otro"
        }
    },
    console: {
        message: {
            deprecatedNow: "Método deprecado. Será eliminado en el siguiente commit.",
            deprecatedSoon: "Método deprecado. Será eliminado en los próximos commits."
        }
    },
    date: {
        days: ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"],
        months: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"]
    },
    finishedSecondaryEducation: {
        yes: {
            value: 1,
            text: "Sí"
        },
        notYet: {
            value: 2,
            text: "No, cursando 5° año"
        },
        anotherCase: {
            value: 3,
            text: "Otros casos"
        }
    },
    formats: {
        datepicker: "dd/mm/yyyy",
        datepickerJsMoment: "DD/MM/YYYY",
        datetimepickerJsMoment: "DD/MM/YYYY h:mm A",
        datetimepicker: "dd/mm/yyyy H:ii P",
        timepickerJsMoment: "h:mm A",
        momentJsDate: "DD-MM-YYYY",
        momentJsDateTime: "DD-MM-YYYY h:mm A"
    },
    guid: {
        empty: "00000000-0000-0000-0000-000000000000"
    },
    http: {
        statusCode: {
            ok: 200,
            badRequest: 400,
            forbidden: 403,
            notFound: 404,
            imATeapot: 418,
            internalServerError: 500
        }
    },
    hubs: {
        akdemic: {
            clientProxy: {
                method: "",
                url: ""
            }
        }
    },
    maritalStatus: {
        single: {
            value: 1,
            text: "Soltero"
        },
        married: {
            value: 2,
            text: "Casado"
        },
        divorced: {
            value: 3,
            text: "Divorciado"
        },
        widowed: {
            value: 4,
            text: "Viudo"
        }
    },
    representativeType: {
        none: {
            value: 1,
            text: "Ninguno"
        },
        mother: {
            value: 2,
            text: "Madre"
        },
        father: {
            value: 3,
            text: "Padre"
        },
        other: {
            value: 4,
            text: "Otro"
        }
    },
    request: {
        approved: {
            text: "Aceptado",
            value: 4
        },
        disapproved: {
            text: "No Procede",
            value: 5
        },
        inProcess: {
            text: "En Proceso",
            value: 3
        }
    },
    secondaryEducationType: {
        public: {
            value: 1,
            text: "Estatal"
        },
        private: {
            value: 2,
            text: "Particular"
        },
        parochial: {
            value: 3,
            text: "Parroquial"
        },
        other: {
            value: 4,
            text: "Otro"
        }
    },
    sex: {
        female: {
            value: 2,
            text: "Femenino"
        },
        male: {
            value: 1,
            text: "Masculino"
        }
    },
    status: {
        active: 1,
        inactive: 0
    },
    survey: {
        text_question: 1,
        multiple_selection_question: 2,
        unique_selection_question: 3

    },
    survey_states: {
        notsent: 1,
        sent: 2,
        inprocess: 3,
        finished: 4
    },
    swal: {
        title: {
            delete: "¿Desea eliminar el registro?"
        }
    },
    termStates: {
        inactive: {
            value: 0,
            text: "Inactivo"
        },
        active: {
            value: 1,
            text: "Activo"
        },
        finished: {
            value: 2,
            text: "Finalizado"
        }
    },
    toastr: {
        message: {
            error: {
                create: "No se pudo crear el registro",
                delete: "No se pudo eliminar el registro",
                get: "No se pudo obtener el registro",
                update: "No se pudo actualizar el registro",
                task: "Ocurrió un problema al procesar su consulta"
            },
            success: {
                create: "Registro creado exitósamente",
                delete: "Registro eliminado exitósamente",
                update: "Registro actualizado exitósamente",
                task: "Tarea realizada satisfactoriamente"
            },
            info: {
                grades: "Han actualizado una de tus notas",
                surveys: "Haz recibido una encuesta"
            },
            mail: {
                send: "Correo enviado exitósamente, revise su bandeja"
            }
        },
        title: {
            error: "Error",
            success: "Éxito",
            info: "Información"
        },
        notificationType: {
            surveys: 1,
            grades: 2
        }
    },
    url: {
        root: ""
    }
};

// ----------
// Defaults
// ----------
jQuery(document).ready(function () {
    var submenu = $(".m-menu__item--active").parent().parent().parent();
    if (submenu.hasClass("m-menu__item--submenu")) {
        submenu.addClass("m-menu__item--open m-menu__item--expanded");
    }
});

// ----------
// Chart.js
// ----------
Chart.defaults.global.defaultFontColor = "#9699a2";
Chart.defaults.global.maintainAspectRatio = false;
Chart.defaults.global.tooltips.mode = "point";

Chart.plugins.register({
    afterDraw: function (chart, easingValue, options) {
        if (chart.data.datasets.length === 0) {
            var chartCtx = chart.ctx;

            chart.clear();
            chartCtx.save();

            chartCtx.textAlign = "center";
            chartCtx.textBaseline = "middle";

            chartCtx.fillText("No hay datos disponibles", chart.width / 2, chart.height / 2);
            chartCtx.restore();
        }
    }
});

Chart.plugins.register({
    beforeDraw: function (chart, easingValue, options) {
        if (chart.config.options.elements.center) {
            var centerX = (chart.chartArea.left + chart.chartArea.right) / 2;
            var centerY = (chart.chartArea.top + chart.chartArea.bottom) / 2;
            var chartCtx = chart.chart.ctx;
            var datasets = chart.data.datasets;
            var elementsCenter = chart.config.options.elements.center;
            var elementsCenterColor = elementsCenter.color || "#dadbe2";
            var elementsCenterFont = elementsCenter.font || "Arial";
            var elementsCenterFontSize = elementsCenter.fontSize;
            var elementsCenterMaxFontSize = elementsCenter.maxFontSize;
            var elementsCenterMode = elementsCenter.mode;
            var elementsCenterPrefix = elementsCenter.prefix;
            var elementsCenterSidePadding = elementsCenter.sidePadding || 20;
            var elementsCenterSuffix = elementsCenter.suffix;
            var elementsCenterText = elementsCenter.text;

            chartCtx.restore();

            if (elementsCenterMode) {
                switch (elementsCenterMode.toLowerCase()) {
                    case "all":
                        var total = 0;

                        for (var i = 0; i < datasets.length; i++) {
                            var datasetData = datasets[i].data;

                            for (var j = 0; j < datasetData.length; j++) {
                                var dataValue = datasetData[j];
                                total += dataValue;
                            }

                            elementsCenterText = total;
                        }

                        break;
                    case "dataset":
                        var datasetIndex = chart.config.options.elements.center.datasetIndex;

                        if (datasets.length > 0 && datasetIndex != null) {
                            var datasetData = datasets[datasetIndex].data;
                            var total = 0;

                            for (var i = 0; i < datasetData.length; i++) {
                                var dataValue = datasetData[i];
                                total += dataValue;
                            }

                            elementsCenterText = total;
                        }

                        break;
                    default:
                        break;
                }
            }

            if (elementsCenterPrefix != null) {
                elementsCenterText = elementsCenterPrefix + elementsCenterText;
            }

            if (elementsCenterSuffix != null) {
                elementsCenterText += elementsCenterSuffix;
            }

            var fontSize = 0;
            chartCtx.font = "30px " + elementsCenterFont;
            elementsCenterText += "";

            if (elementsCenterFontSize != null) {
                fontSize = elementsCenterFontSize;
            } else {
                var calculatedSizePadding = elementsCenterSidePadding / 100 * chart.innerRadius * 1.5;
                var elementHeight = chart.innerRadius * 1.5;
                var elementWidth = chart.innerRadius * 1.5 - calculatedSizePadding;
                var textWidth = chartCtx.measureText(elementsCenterText).width;
                var widthRatio = elementWidth / textWidth;

                var fontRatio = Math.floor(30 * widthRatio);

                if (elementsCenterMaxFontSize != null) {
                    fontSize = Math.min(elementHeight, elementsCenterMaxFontSize, fontRatio);
                } else {
                    fontSize = Math.min(elementHeight, fontRatio);
                }
            }

            chartCtx.fillStyle = elementsCenterColor;
            chartCtx.font = fontSize + "px " + elementsCenterFont;
            chartCtx.textAlign = "center";
            chartCtx.textBaseline = "middle";

            chartCtx.fillText(elementsCenterText, centerX, centerY);
            chartCtx.save();
        }
    }
});

window.onbeforeprint = function () {
    for (var id in Chart.instances) {
        Chart.instances[id].resize();
    }
}

//// ----------
//// DataTables
//// ----------
//$.extend($.fn.dataTable.defaults, {
//    serverSide: true,
//    processing: true,
//    dom: "<'top'i>rt<'bottom'lp><'clear'>",
//    language: {
//        //"sProcessing": "<div class='m-blockui' style='display: inline; background: none; box-shadow: none;'><span>Cargando...</span><span><div class='m-loader  m-loader--brand m-loader--lg'></div></span></div>",
//        "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>",
//        "lengthMenu": "",
//        "zeroRecords": "No existen registros",
//        "sEmptyTable": "Ningún dato disponible en esta tabla",
//        //"sInfo": "Mostrando _START_ - _END_ de _TOTAL_ registros",
////        "sInfo": "Mostrando _START_ - _END_ de _TOTAL_ registros",
//        "sInfoEmpty": "Mostrando 0 - 0 de 0 registros",
//        "infoEmpty": "",
//        //"sInfoFiltered": "(filtrado de _MAX_ registros)",
//        "infoFiltered": "_MAX_ / _TOTAL_",
//        "sInfoPostFix": "",
//        "sSearch": "Buscar:",
//        "sUrl": "",
//        "sInfoThousands": ",",
//        "sLoadingRecords": "Cargando...",
//        "oPaginate": {
//            "sFirst": "<i class='la la-angle-double-left'></i>",
//            "sLast": "<i class='la la-angle-double-right'></i>",
//            "sNext": "<i class='la la-angle-right'></i>",
//            "sPrevious": "<i class='la la-angle-left'></i>"
//        },
//        "paginate": {
//            "next": ">>",
//            "previous": "<<"
//        },
//        "oAria": {
//            "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
//            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
//        }
//    },
//    lengthMenu: [10, 25, 50],
//    orderMulti: false,
//    pagingType: "full_numbers",
//    responsive: true,
//    filter: false,
//    pageLength: 50,
//    lengthChange: false,
//    paging: true,
//    order: [],
//    info: true
//    //ordering: true, //default
//});

// ----------
// DataTables
// ----------
$.extend($.fn.dataTable.defaults, {
    dom: "<'top'i>rt<'bottom'lp><'clear'>",
    language: {
        "sProcessing": "<div class='m-blockui' style='display: inline; background: none; box-shadow: none;'><span>Cargando...</span><span><div class='m-loader  m-loader--brand m-loader--lg'></div></span></div>",
        "sLengthMenu": "Mostrar _MENU_ registros",
        "sZeroRecords": "No se encontraron resultados",
        "sEmptyTable": "Ningún dato disponible en esta tabla",
        "sInfo": "Mostrando _START_ - _END_ de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando 0 - 0 de 0 registros",
        "sInfoFiltered": "(filtrado de _MAX_ registros)",
        "sInfoPostFix": "",
        "sSearch": "Buscar:",
        "sUrl": "",
        "sInfoThousands": ",",
        "sLoadingRecords": "Cargando...",
        "oPaginate": {
            "sFirst": "<i class='la la-angle-double-left'></i>",
            "sLast": "<i class='la la-angle-double-right'></i>",
            "sNext": "<i class='la la-angle-right'></i>",
            "sPrevious": "<i class='la la-angle-left'></i>"
        },
        "oAria": {
            "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    },
    lengthMenu: [10, 25, 50],
    orderMulti: false,
    pagingType: "full_numbers",
    processing: true,
    responsive: true,
    serverSide: true,
    filter: false,
    pageLength: 50,
    lengthChange: false,
    paging: true,
    order: [],
    info: true
});

// ----------
// Datepicker
// ----------
var dayNames = ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"];
var monthNames = [
    "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Setiembre", "Octubre", "Noviembre",
    "Diciembre"
];
var dayNamesShort = ["Dom", "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb"];
var monthNamesShort = ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"];

$.fn.datepicker.dates.es = {
    clear: "Borrar",
    days: dayNames,
    daysMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
    daysShort: dayNamesShort,
    format: "dd/mm/yyyy",
    months: monthNames,
    monthsShort: monthNamesShort,
    monthsTitle: "Meses",
    today: "Hoy",
    weekStart: 1
};

$.fn.datepicker.defaults.autoclose = true;
$.fn.datepicker.defaults.clearBtn = true;
$.fn.datepicker.defaults.language = "es";
$.fn.datepicker.defaults.templates = {
    leftArrow: "<i class=\"la la-angle-left\"></i>",
    rightArrow: "<i class=\"la la-angle-right\"></i>"
};
$.fn.datepicker.defaults.todayHighlight = true;

// ----------
// jQuery
// ----------
(function ($) {
    $.fn.addFormHelp = function (content, selector = ":last") {
        var element = $(this.parent().children(selector));

        if (element.length > 0) {
            element.after("<span class=\"m-form__help\">" + content + "</span>");
        } else {
            return null;
        }

        return this;
    };

    $.fn.removeFormHelp = function (selector = "*") {
        var element = this.parent().children(".m-form__help");

        if (element.length > 0) {
            element.filter(selector).remove();
        } else {
            return null;
        }

        return this;
    };

    $.fn.addLoader = function () {
        this.addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
        return this;
    };

    $.fn.removeLoader = function () {
        this.removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
        return this;
    };

    $.fn.addProgressBar = function (selector = ":last") {
        var element = $(this.parent().children(selector));

        if (element.length > 0) {
            element.after("<div class=\"progress\"><div class=\"progress-bar progress-bar-striped progress-bar-animated bg-success\" role=\"progressbar\" aria-valuenow=\"0\" aria-valuemin=\"0\" aria-valuemax=\"100\" style=\"width: 0%\"></div></div>");
        } else {
            return null;
        }

        return this;
    };

    $.fn.removeProgressBar = function (selector = "*") {
        var element = this.parent().children(".progress");

        if (element.length > 0) {
            element.filter(selector).remove();
        } else {
            return null;
        }

        return this;
    };
})(jQuery);

// ----------
// jQuery AJAX
// ----------
$.ajaxSetup({
    headers: {
        "X-CSRF-TOKEN": $("meta[name=\"csrf-token\"]").attr("content")
    }
});

// ----------
// jQuery Validation
// ----------
$.extend($.validator.messages, {
    accept: "Por favor, ingrese un archivo con un formato válido.",
    cifES: "Por favor, escriba un CIF válido.",
    creditcard: "Por favor, escriba un número de tarjeta válido.",
    date: "Por favor, escriba una fecha válida.",
    dateISO: "Por favor, escriba una fecha (ISO) válida.",
    digits: "Por favor, escriba sólo dígitos.",
    email: "Por favor, escriba un correo electrónico válido.",
    equalTo: "Por favor, escriba el mismo valor de nuevo.",
    extension: "Por favor, escriba un valor con una extensión permitida.",
    max: $.validator.format("Por favor, escriba un valor menor o igual a {0}."),
    maxlength: $.validator.format("Por favor, no escriba más de {0} caracteres."),
    min: $.validator.format("Por favor, escriba un valor mayor o igual a {0}."),
    minlength: $.validator.format("Por favor, no escriba menos de {0} caracteres."),
    nieES: "Por favor, escriba un NIE válido.",
    nifES: "Por favor, escriba un NIF válido.",
    number: "Por favor, escriba un número válido.",
    pattern: "Por favor, escriba un formato válido.",
    range: $.validator.format("Por favor, escriba un valor entre {0} y {1}."),
    rangelength: $.validator.format("Por favor, escriba un valor entre {0} y {1} caracteres."),
    remote: "Por favor, llene este campo.",
    required: "Este campo es obligatorio.",
    url: "Por favor, escriba una URL válida.",
    step: "Por favor, ingrese un número entero."
});

jQuery.validator.setDefaults({
    errorPlacement: function (error, element) {
        if (element.parent(".input-group").length) {
            error.insertAfter(element.parent()); // radio/checkbox?      
        }
        else if (element.parent(".m-input-icon").length) {
            error.insertAfter(element.parent());
        }
        else if (element.parent().parent(".m-radio-inline").length) {
            error.insertAfter(element.parent().parent());
        }
        else if (element.hasClass("m-select2")) {
            error.insertAfter(element.next("span")); // select2
        }
        else {
            error.insertAfter(element); // default
        }

    }
});

$.validator.addMethod("dates", function (value, element, params, message) {
    if (this.optional(element)) {
        return true;
    }

    var messagesIndices = [4, 5];

    var tmpDateObject = {};
    var tmpDateObjectKeys = [];

    var operator = params[0];
    var dateObject = params[1];
    var formatObject = params[2];
    var validates = params[3];

    if (validates == null) {
        params[3] = validates = [];
    }

    for (var dateObjectKey in dateObject) {
        var dateObjectValue = dateObject[dateObjectKey];

        if (dateObjectValue != null) {
            if (dateObjectValue.constructor == Object) {
                tmpDateObject[dateObjectKey] = {};

                if (dateObjectValue.date != null) {
                    tmpDateObject[dateObjectKey].date = dateObjectValue.date;
                }

                if (dateObjectValue.time != null) {
                    tmpDateObject[dateObjectKey].time = dateObjectValue.time;
                }
            } else {
                tmpDateObject[dateObjectKey] = dateObjectValue;
            }
        }
    }

    if (tmpDateObject.base == null) {
        tmpDateObject.base = value;
    } else {
        if (tmpDateObject.base.constructor == Object) {
            if (tmpDateObject.base.date == null && tmpDateObject.base.time == null) {
                tmpDateObject.base = value;
            } else {
                if (tmpDateObject.base.date == null) {
                    tmpDateObject.base.date = value;
                } else if (tmpDateObject.base.time == null) {
                    tmpDateObject.base.time = value;
                }

                var baseDateElement = $(tmpDateObject.base.date);
                var baseTimeElement = $(tmpDateObject.base.time);

                if (baseDateElement.length > 0) {
                    tmpDateObject.base.date = baseDateElement.val();
                }

                if (baseTimeElement.length > 0) {
                    tmpDateObject.base.time = baseTimeElement.val();
                }

                tmpDateObjectKeys.push("base");
            }
        }
    }

    if (tmpDateObject.target != null) {
        if (tmpDateObject.target.constructor == Object) {
            if (tmpDateObject.target.date != null) {
                var targetDateElement = $(tmpDateObject.target.date);

                if (targetDateElement.length > 0) {
                    tmpDateObject.target.date = targetDateElement.val();
                }
            }

            if (tmpDateObject.target.time != null) {
                var targetTimeElement = $(tmpDateObject.target.time);

                if (targetTimeElement.length > 0) {
                    tmpDateObject.target.time = targetTimeElement.val();
                }
            }

            tmpDateObjectKeys.push("target");
        }
    }

    for (var i = 0; i < tmpDateObjectKeys.length; i++) {
        var tmpDateObjectKey = tmpDateObjectKeys[i];
        var tmpDateObjectValue = tmpDateObject[tmpDateObjectKey];

        if (tmpDateObjectValue.date == null) {
            tmpDateObject[tmpDateObjectKey] = tmpDateObjectValue.time;
        } else if (tmpDateObjectValue.time == null) {
            tmpDateObject[tmpDateObjectKey] = tmpDateObjectValue.date;
        } else {
            tmpDateObject[tmpDateObjectKey] = tmpDateObjectValue.date + " " + tmpDateObjectValue.time;
        }
    }

    if (formatObject.base == null) {
        formatObject.base = formatObject.target;
    }

    if (formatObject.target == null) {
        formatObject.target = formatObject.base;
    }

    var baseMoment = moment(tmpDateObject.base, formatObject.base);
    var targetMoment = moment(tmpDateObject.target, formatObject.target);
    var parsedBaseDate = null;
    var parsedTargetDate = null;

    if (
        baseMoment.isValid() &&
        targetMoment.isValid() &&
        baseMoment._pf.unusedTokens.length <= 0 &&
        targetMoment._pf.unusedTokens.length <= 0) {
        parsedBaseDate = Date.parse(baseMoment.toDate());
        parsedTargetDate = Date.parse(targetMoment.toDate());
    } else {
        return true;
    }

    params[messagesIndices[1]] = tmpDateObject.target;

    for (var i = 0; i < validates.length; i++) {
        $(validates[i]).valid();
    }

    switch (operator) {
        case ">":
            if (parsedBaseDate <= parsedTargetDate) {
                params[messagesIndices[0]] = "mayor a ";
                return false;
            }
        case "<":
            if (parsedBaseDate >= parsedTargetDate) {
                params[messagesIndices[0]] = "menor a ";
                return false;
            }
        case "==":
            if (parsedBaseDate != parsedTargetDate) {
                params[messagesIndices[0]] = "igual a ";
                return false;
            }
        case ">=":
            if (parsedBaseDate < parsedTargetDate) {
                params[messagesIndices[0]] = "mayor o igual a ";
                return false;
            }
        case "<=":
            if (parsedBaseDate > parsedTargetDate) {
                params[messagesIndices[0]] = "menor o igual a ";
                return false;
            }
        default:
            return false;
    }

    return true;
}, $.validator.format("Por favor, escriba una fecha u hora {4}{5}"));

$.validator.addMethod("decimal", function (value, element, params, message) {
    if (this.optional(element)) {
        return true;
    }

    var errorMessage = "";
    var messagesIndices = [2];

    var decimalText = value + "";
    var decimalTextSplit = decimalText.split(/[.,]/);
    var decimalIntegral = decimalTextSplit[0];
    var decimalFractional = decimalTextSplit[1];

    var maxIntegralLength = params[0];
    var maxFractionalLength = params[1];

    if (decimalIntegral != null) {
        var decimalIntegralLength = decimalIntegral.length;

        if (maxIntegralLength != null && decimalIntegralLength > maxIntegralLength) {
            errorMessage += (errorMessage.length > 0 ? " y " : "");
            errorMessage += "entera (";
            errorMessage += maxIntegralLength;
            errorMessage += ")";
        }
    }

    if (decimalFractional != null) {
        var decimalFractionalLength = decimalFractional.length;

        if (maxFractionalLength != null && decimalFractionalLength > maxFractionalLength) {
            errorMessage += (errorMessage.length > 0 ? " y " : "");
            errorMessage += "decimal (";
            errorMessage += maxFractionalLength;
            errorMessage += ")";
        }
    }

    params[messagesIndices[0]] = errorMessage;

    if (errorMessage.length > 0) {
        return false;
    }

    return true;
}, "El número sobrepasa el máximo de dígitos permitidos en su parte {2}");

$.validator.addMethod("fileSizeBs", function (value, element, param) {
    return this.optional(element) || element.files[0].size <= param;
}, "El archivo debe pesar menos de {0} B.");

$.validator.addMethod("fileSizeKBs", function (value, element, param) {
    return this.optional(element) || element.files[0].size <= param * 1024;
}, "El archivo debe pesar menos de {0} KB.");

$.validator.addMethod("fileSizeMBs", function (value, element, param) {
    return this.optional(element) || element.files[0].size <= param * 1024 * 1024;
}, "El archivo debe pesar menos de {0} MB.");

$.validator.addMethod("timeRange", function (value, element, param) {
    if (this.optional(element)) {
        return true;
    }

    var element2 = param[0];
    var conditional = param[1];
    var format = param[2];

    console.log(param);
    console.log(element);
    console.log($(element));
    console.log($(element)[0].value);
    console.log($(element).val());
    console.log(value);


    var time1 = moment($(element).val(), format !== undefined ? format : "LT");
    var minutes1 = time1.hour() * 60 + time1.minute();
    console.log(minutes1);

    console.log(element2);
    var time2 = moment(element2.val(), format !== undefined ? format : "LT");
    console.log(time2);
    var minutes2 = time2.hour() * 60 + time2.minute();
    console.log(minutes2);

    if (conditional === null) conditional = ">";

    switch (conditional) {
        case ">": return minutes1 > minutes2;
        case "<": return minutes1 < minutes2;
        case ">=": return minutes1 >= minutes2;
        case "<=": return minutes1 <= minutes2;
        default:
            return false;
    }
}, "El rango de horas es inválido");

// ----------
// Markdown
// ----------
(function ($) {
    $.fn.markdown.messages["es"] = {
        "Bold": "Negrita",
        "Code": "Código",
        "emphasized text": "Texto Enfatizado",
        "enter image description here": "Ingrese una descripción de la imagen aquí",
        "enter image title here": "Ingrese un título para la imagen aquí",
        "enter link description here": "Ingrese una descripción del enlace aquí",
        "Heading": "Título",
        "heading text": "Texto de Cabecera",
        "Image": "Inserte una Imagen",
        "Insert Hyperlink": "Ingrese una URL",
        "Insert Image Hyperlink": "Inserta una URL para la imagen",
        "Italic": "Cursiva",
        "List": "Lista",
        "list text here": "Texto a listar aquí",
        "Ordered List": "Lista Enumerada",
        "Preview": "Previsualización",
        "Quote": "Cita",
        "strong text": "Texto Importante",
        "URL/Link": "URL/Enlace",
        "Unordered List": "Lista"
    };
})(jQuery);

$.fn.markdown.defaults.language = "es";

// ----------
// mLayout
// ----------
var mLayoutReturn = {
    initHeader: function () { }
};

// ----------
// Select2
// ----------
(function () {
    if (jQuery && jQuery.fn && jQuery.fn.select2 && jQuery.fn.select2.amd) {
        var e = jQuery.fn.select2.amd;

        return e.define("select2/i18n/es", [], function () {
            return {
                errorLoading: function () {
                    return "No se pudieron cargar los resultados";
                },
                inputTooLong: function (e) {
                    var t = e.input.length - e.maximum,
                        n = "Por favor, elimine " + t + " car";

                    return t == 1 ? n += "ácter" : n += "acteres", n;
                },
                inputTooShort: function (e) {
                    var t = e.minimum - e.input.length,
                        n = "Por favor, introduzca " + t + " car";

                    return t == 1 ? n += "ácter" : n += "acteres", n;
                },
                loadingMore: function () {
                    return "Cargando más resultados…";
                },
                maximumSelected: function (e) {
                    var t = "Sólo puede seleccionar " + e.maximum + " elemento";

                    return e.maximum != 1 && (t += "s"), t;
                },
                noResults: function () {
                    return "No se encontraron resultados";
                },
                searching: function () {
                    return "Buscando…";
                }
            };
        }), { define: e.define, require: e.require };
    }
})();

//$.fn.select2.defaults.set("ajax--delay", 1000);
$.fn.select2.defaults.set("language", "es");
$.fn.select2.defaults.set("placeholder", "---");
$.fn.select2.defaults.set("width", "100%");

// ----------
// SweetAlert2
// ----------
var swal = swal.mixin({
    allowOutsideClick: () => !swal.isLoading(),
    cancelButtonClass: "btn",
    cancelButtonColor: "#e73d4a",
    cancelButtonText: "No",
    confirmButtonClass: "btn",
    confirmButtonColor: "#27a4b0",
    confirmButtonText: "Sí",
    showCancelButton: false,
    showConfirmButton: true
});

// ----------
// Toastr
// ----------
toastr.options.closeButton = false;
toastr.options.progressBar = false;

// ----------
// Prototypes
// ----------

// ----------
// Array
// ----------
Object.defineProperty(Array.prototype, "proto", {
    value: function () {
        var self = this;
        var result = {
            copy: function () {
                var arr = self;
                var tmpArr = [];

                for (var i = 0; i < arr.length; i++) {
                    var arrItem = arr[i];

                    if (arrItem != null) {
                        if (
                            arrItem.constructor === Array ||
                            arrItem.constructor === Object
                        ) {
                            tmpArr[i] = arrItem.proto().copy();
                            continue;
                        } else if (arrItem.constructor === Date) {
                            tmpArr[i] = new Date(arrItem.getTime());
                            continue;
                        }
                    }

                    tmpArr[i] = arrItem;
                }

                return tmpArr;
            },
            merge: function () {
                var target = self;
                var sources = arguments;

                if (target == null) {
                    target = [];
                } else if (target.constructor !== Array) {
                    return target;
                }

                var arrTarget = target.proto().copy();

                for (var i = 0; i < sources.length; i++) {
                    var source = sources[i];

                    if (source == null) {
                        source = [];
                    } else if (source.constructor !== Array) {
                        continue;
                    }

                    var arrSource = source.proto().copy();

                    for (var j = 0; j < arrSource.length; j++) {
                        var arrSourceItem = arrSource[j];
                        var arrTargetItem = arrTarget[j];

                        if (arrTargetItem !== void 0) {
                            if (arrTargetItem == null && (arrSourceItem != null || arrSourceItem.constructor === Array)) {
                                arrTarget[j] = arrSourceItem;
                            } else if ((arrTargetItem != null || arrTargetItem.constructor === Array) && arrSourceItem == null) {
                                continue;
                            } else if (arrTargetItem.constructor === Array && arrSourceItem.constructor === Array) {
                                arrTarget[j] = arrTargetItem.concat(arrSourceItem);
                            } else if (arrTargetItem.constructor === Object && arrSourceItem.constructor === Object) {
                                arrTarget[j] = arrTargetItem.proto().merge(arrSourceItem);
                            } else if (arrTargetItem.constructor === Function && arrSourceItem.constructor === Function) {
                                arrTarget[j] = function () {
                                    arrTargetItem.apply(null, arguments);
                                    arrSourceItem.apply(null, arguments);
                                };
                            } else {
                                //TODO: Validate if it should replace or not
                                arrTarget[j] = arrSourceItem;
                            }
                        } else {
                            arrTarget.push(arrSourceItem);
                        }
                    }
                }

                return arrTarget;
            },
            random: function () {
                var arr = self;

                return arr[Math.floor(Math.random() * arr.length)];
            }
        };

        return result;
    },
    enumerable: false
});

// ----------
// Date
// ----------
Object.defineProperty(Date.prototype, "proto", {
    value: function () {
        var self = this;
        var result = {
            addDays: function (days) {
                var result = self;
                result = new Date(result.valueOf());

                result.setDate(result.getDate() + days);

                return result;
            },
            toDate: function () {
                var result = self;
                result = self.toDateString();

                return new Date(result);
            },
            toTime: function () {
                var result = self;
                result = self.toTimeString();

                return new Date("1970 " + result);
            }
        };

        return result;
    },
    enumerable: false
});

// ----------
// FormData
// ----------
Object.defineProperty(FormData.prototype, "proto", {
    value: function () {
        var self = this;
        var result = {
            appendArray: function (arr, name) {
                var result = self;

                for (var i = 0; i < arr.length; i++) {
                    var tmpName = name + "[" + i + "]";
                    var arrItem = arr[i];

                    if (arrItem.constructor == Array) {
                        result.proto().appendArray(arrItem, tmpName);
                    } else if (arrItem.constructor == Object) {
                        result.proto().appendObject(arrItem, tmpName);
                    } else {
                        result.append(tmpName, arrItem);
                    }
                }

                return result;
            },
            appendFiles: function (files, name, childs = null) {
                var result = self;
                var keys = [];

                if (childs != null) {
                    keys = childs.split(".");
                }

                for (var i = 0; i < files.length; i++) {
                    var file = files[i];

                    for (var j = 0; j < keys.length; j++) {
                        var key = keys[j];

                        if (file[key] != null) {
                            file = file[key];
                        }
                    }

                    result.append(name, file, file.name);
                }

                return result;
            },
            appendObject: function (obj, name) {
                var result = self;

                for (var key in obj) {
                    var tmpName = name + "." + key;
                    var objItem = obj[key];

                    if (objItem.constructor == Array) {
                        result.proto().appendArray(objItem, tmpName);
                    } else if (objItem.constructor == Object) {
                        result.proto().appendObject(objItem, tmpName);
                    } else {
                        result.append(tmpName, objItem);
                    }
                }

                return result;
            }
        };

        return result;
    },
    enumerable: false
});

// ----------
// Number
// ----------
Object.defineProperty(Number.prototype, "proto", {
    value: function (scale) {
        var self = this;
        var result = {
            round: function () {
                var num = self;
                var numString = num + "";
                var roundString = "";

                if (!numString.includes("e")) {
                    roundString = Math.round(num + "e+" + scale) + "e-" + scale;
                } else {
                    var numStrings = numString.split("e");
                    var numStringExponential = numStrings[1];
                    var numExponential = +numStringExponential;
                    var sign = "";

                    if (numExponential + scale > 0) {
                        sign = "+";
                    }

                    var numStringValue = numStrings[0];
                    var numValue = +numStringValue;
                    roundString = Math.round(numValue + "e" + sign + (numExponential + scale)) + "e-" + scale;
                }

                var round = +roundString;

                return round;
            },
            toRGB: function () {
                var num = self;
                var hash = num & 0x00FFFFFF;
                var hashBase = hash.toString(16);
                var rgb = "00000".substring(0, 6 - hashBase.length) + hashBase.toUpperCase();

                return rgb;
            }
        };

        return result;
    },
    enumerable: false
});

// ----------
// Object
// ----------
Object.defineProperty(Object.prototype, "proto", {
    value: function () {
        var self = this;
        var result = {
            copy: function () {
                var obj = self;
                var tmpObj = {};

                for (var key in obj) {
                    var objItem = obj[key];

                    if (objItem != null) {
                        if (
                            objItem.constructor === Array ||
                            objItem.constructor === Object
                        ) {
                            tmpObj[key] = objItem.proto().copy();
                            continue;
                        } else if (objItem.constructor === Date) {
                            tmpObj[key] = new Date(objItem.getTime());
                            continue;
                        }
                    }

                    tmpObj[key] = objItem;
                }

                return tmpObj;
            },
            encode: function () {
                var result = self;

                if (result) {
                    result = JSON.stringify(result);
                    result = encodeURIComponent(result);
                    result = result.replace(/%([0-9A-F]{2})/g, function (str, p1) {
                        return String.fromCharCode(parseInt(p1, 16));
                    });

                    result = window.btoa(result);
                }

                return result;
            },
            htmlElement: function (callback) {
                var elements = self;
                var element = null;

                switch (elements.constructor) {
                    case HTMLCollection:
                        for (element in elements) {
                            if (callback != null) {
                                callback(element);
                            } else {
                                break;
                            }
                        }

                        break;
                    case HTMLElement:
                        element = elements;
                        callback(element);

                        break;
                    case jQuery:
                        var elementsLength = elements.length;

                        for (var i = 0; i < elementsLength; i++) {
                            element = elements[i];

                            if (callback != null) {
                                callback(element);
                            } else {
                                break;
                            }
                        }

                        break;
                    default:
                        break;
                }

                return element;
            },
            isEmpty: function () {
                var obj = self;

                for (var key in obj) {
                    if (obj.hasOwnProperty(key)) {
                        return false;
                    }
                }

                return obj === {};
            },
            merge: function () {
                var target = self;
                var sources = arguments;

                if (target == null) {
                    target = {};
                } else if (target.constructor !== Object) {
                    return target;
                }

                var objTarget = target.proto().copy();

                for (var i = 0; i < sources.length; i++) {
                    var source = sources[i];

                    if (source == null) {
                        source = {};
                    } else if (source.constructor !== Object) {
                        continue;
                    }

                    var objSource = source.proto().copy();

                    for (var key in objSource) {
                        var objSourceItem = objSource[key];

                        if (objTarget.hasOwnProperty(key)) {
                            var objTargetItem = objTarget[key];

                            if (objTargetItem == null && (objSourceItem != null || objSourceItem.constructor === Array)) {
                                objTarget[key] = objSourceItem;
                            } else if ((objTargetItem != null || objTargetItem.constructor === Array) && objSourceItem == null) {
                                continue;
                            } else if (objTargetItem.constructor === Array && objSourceItem.constructor === Array) {
                                objTarget[key] = objTargetItem.concat(objSourceItem);
                            } else if (objTargetItem.constructor === Object && objSourceItem.constructor === Object) {
                                objTarget[key] = objTargetItem.proto().merge(objSourceItem);
                            } else if (objTargetItem.constructor === Function && objSourceItem.constructor === Function) {
                                objTarget[key] = function () {
                                    objTargetItem.apply(null, arguments);
                                    objSourceItem.apply(null, arguments);
                                };
                            } else {
                                //TODO: Validate if it should replace or not
                                objTarget[key] = objSourceItem;
                            }
                        } else {
                            objTarget[key] = objSourceItem;
                        }
                    }
                }

                return objTarget;
            },
            tryGet: function (childs) {
                if (childs == null) {
                    return null;
                }

                var obj = self;
                var keys = childs.split(".");

                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];

                    if (obj[key] != null) {
                        obj = obj[key];
                    } else {
                        return null;
                    }
                }

                return obj;
            }
        };

        return result;
    },
    enumerable: false
});

// ----------
// String
// ----------
Object.defineProperty(String.prototype, "proto", {
    value: function () {
        var self = this;
        var result = {
            decode: function () {
                var result = self;

                if (result) {
                    var tmpResult = "";
                    result = window.atob(result);

                    for (var i = 0; i < result.length; i++) {
                        tmpResult += "%" + ("00" + result[i].charCodeAt(0).toString(16)).slice(-2);
                    }

                    var tmpResult2 = "";

                    for (var i = 0; i < tmpResult.length; i++) {
                        tmpResult2 += tmpResult[i];
                    }

                    result = decodeURIComponent(tmpResult2);
                    result = JSON.parse(result);
                }

                return result;
            },
            ellipsis: function (start, end) {
                var str = self;
                var ellipsis = "";

                if (start > 0) {
                    ellipsis = str.substring(start, str.length);
                    ellipsis = "..." + ellipsis;
                }

                if (end < str.length) {
                    ellipsis = str.substring(0, end);
                    ellipsis = ellipsis + "...";
                }

                return ellipsis;
            },
            format: function () {
                var str = self;

                for (argument in arguments) {
                    str = str.replace("{" + argument + "}", arguments[argument]);
                }

                return str;
            },
            hash: function () {
                var str = self;
                var hash = 0;

                for (var i = 0; i < str.length; ++i) {
                    hash = str.charCodeAt(i) + ((hash << 5) - hash);
                }

                return hash;
            },
            parseBaseURL: function (baseUrl) {
                var str = self;
                var url = window.location.protocol;
                url += "//";
                url += window.location.host;
                url += baseUrl;
                url += str;

                return url;
            },
            parseURL: function () {
                var str = self;
                var url = window.location.protocol;
                url += "//";
                url += window.location.host;
                url += _app.constants.url.root;
                url += str;

                return url;
            },
            toRGB: function () {
                var str = self;
                var hash = str.proto().hash();
                var rgb = hash.proto().toRGB();

                return rgb;
            }
        };

        return result;
    },
    enumerable: false
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
                    `Por favor, introduzca ${quantityNeeded} ${(quantityNeeded === 1 ? 'caracter' : 'caracteres')}` : "";

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
                console.log(`keycode: ${e.keyCode}`);
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


