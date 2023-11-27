var TopicConsult = function () {
    var saveData = function () {
        $("#topic-consult-form").validate({
            rules: {
                NextAppointmentDate: { required : true }
            },          
            submitHandler: function (form, event) {

                var data = $(form).serializeArray();
                data.push({ name: "MedicalIndication", value: $("#MedicalIndication").val() });
                data.push({ name: "Description", value: $("#Description").val()});
                $.ajax({
                    type: 'POST',
                    url: ("/topico/horario-citas/guardar").proto().parseURL(),
                    data: $.param(data),
                    success: function (data) {
                        location.href = `/topico/horario-citas`.proto().parseURL();     
                    },
                    error: function (data) {
                        toastr.error(data.responseText, _app.constants.toastr.title.error);
                    }
                });

            }
        });

    }

    var datepickers = function () {
        $("#HistoricalDate").datepicker({
            format: _app.constants.formats.datepicker
        }).one("changeDate", function (e) {
            $(this).valid();
            $("#NextAppointmentDate").datepicker("setStartDate", e.date);
        });;
        $("#NextAppointmentDate").datepicker({
            format: _app.constants.formats.datepicker
        }).one("changeDate", function (e) {
            $("#HistoricalDate").datepicker("setEndDate", e.date);
        });;
    }

    return {
        init: function () {            
            saveData();
            datepickers();
        }
    }
}();

$(function () {
    TopicConsult.init();
});