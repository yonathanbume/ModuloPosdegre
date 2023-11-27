var academicCalendarDate = (function () {
    var private = {
        ajax: {
            objects: {}
        }
    };

    return {
        ajax: {
            getObject: function (key) {
                return private.ajax.objects[key];
            },
            load: {
                calendar: function () {
                    private.ajax.objects["academic-calendar-date-ajax-calendar"] = $.ajax({
                        type: "GET",
                        url: "/calendario-academico/get".proto().parseURL()
                    })
                        .done(function (data, textStatus, jqXHR) {
                            var dataLength = data.length;
                            var academicCalendarDateTemplate = "";
                            var academicCalendarDateTemplate2 = "";

                            if (dataLength > 0) {
                                academicCalendarDateTemplate += `
                                    <h1 class="m-0">${data[0].term.name}</h1>
                                `;
                                academicCalendarDateTemplate2 += `
                                    <table class="table table-bordered m-table m-table--border-secondary">
                                        <tbody>
                                `;

                                for (var i = 0; i < dataLength; i++) {
                                    var dataRow = data[i];
                                    var endFormattedDateMoment = moment(dataRow.endFormattedDate, _app.constants.formats.datetimepickerJsMoment);
                                    var endFormattedDateMomentIsValid = endFormattedDateMoment.isValid();
                                    var endFormattedDateDay = null;
                                    var endFormattedDateDayName = null;
                                    var endFormattedDateMonthName = null;
                                    var startFormattedDateMoment = moment(dataRow.startFormattedDate, _app.constants.formats.datetimepickerJsMoment);
                                    var startFormattedDateDate = startFormattedDateMoment.date();
                                    var startFormattedDateDayName = _app.constants.date.days[startFormattedDateMoment.day()];
                                    var startFormattedDateMonthName = _app.constants.date.months[startFormattedDateMoment.month()];

                                    if (endFormattedDateMomentIsValid) {
                                        endFormattedDateDate = endFormattedDateMoment.date();
                                        endFormattedDateDayName = _app.constants.date.days[endFormattedDateMoment.day()];
                                        endFormattedDateMonthName = _app.constants.date.months[endFormattedDateMoment.month()];
                                    }

                                    academicCalendarDateTemplate2 += `
                                            <tr class="m-table__row${i % 2 == 0 ? "--primary" : "--secondary"}">
                                                <th class="m--font-transform-u text-right" style="width: 33%; vertical-align: middle;"><h6 class="m-0">
                                `;

                                    if (endFormattedDateMomentIsValid) {
                                        academicCalendarDateTemplate2 += `del ${startFormattedDateDayName} ${startFormattedDateDate} de ${startFormattedDateMonthName}<div class="m--space-5"></div>al ${endFormattedDateDayName} ${endFormattedDateDate} de ${endFormattedDateMonthName}`;
                                    } else {
                                        academicCalendarDateTemplate2 += `${startFormattedDateDayName} ${startFormattedDateDate} de ${startFormattedDateMonthName}`;
                                    }

                                    academicCalendarDateTemplate2 += `
                                                </h6></th>
                                                <td class="m--font-bold">${dataRow.name}</td>
                                            </tr>
                                `;
                                }

                                academicCalendarDateTemplate2 += `
                                        </tbody>
                                    </table>
                                    <h6>(*) El calendario puede estar sujeto a cambios los cuales se avisarán oportunamente.</h6>
                                `;
                            } else {
                                academicCalendarDateTemplate += `
                                    <h1 class="m-0">${new Date().getFullYear()}</h1>
                                `;
                                academicCalendarDateTemplate2 += `
                                    <div class="m-alert m-alert--icon m-alert--air m-alert--square alert alert-primary alert-dismissible fade show" role="alert">
                                        <div class="m-alert__icon">
								            <i class="la la-warning"></i>
							            </div>
<hr></hr></hr>
                                        <div class="m-alert__text">
							  	            <strong>Advertencia</strong> No hay datos disponibles.
                                        </div>
						            </div>
                                `;
                            }

                            document.getElementById("academic-calendar-date-content").innerHTML = academicCalendarDateTemplate;
                            document.getElementById("academic-calendar-date-content-2").innerHTML = academicCalendarDateTemplate2;
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            var responseText = jqXHR.responseText;

                            if (responseText != "" && jqXHR.status == 400) {
                                toastr.error(responseText, _app.constants.toastr.title.error);
                            } else {
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            }
                        });
                }
            }
        },
        init: function () {
            academicCalendarDate.ajax.load.calendar();
        }
    };
})();

$(function () {
    academicCalendarDate.init();
});
