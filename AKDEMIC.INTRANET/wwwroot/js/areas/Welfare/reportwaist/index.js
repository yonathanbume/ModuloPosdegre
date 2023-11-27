var reportWaist = function () {

    var datatable = null;

    var options = {
        ajax: {
            url: "/welfare/reporte_cintura/detalle",
            type: "GET",
            data: function (data) {
                data.n_minimo = $("#min").val();
                data.n_maximo = $("#max").val();
            }
        },
        columns: [
            {
                data: 'username',
                title: 'Código',
            },
            {
                data: 'fullname',
                title: 'Nombres Completos',
            },
            {
                data: 'email',
                title: 'Email',
            },
            {
                data: 'facultyname',
                title: 'Escuela Profesional',
            },
            {
                data: 'sex',
                title: 'Sexo',
                render: function (row) {
                    if (row.sex == "Masculino") {
                        return `<span class="m-badge  m-badge--success m-badge--wide">Masculino</span>`
                    } else {
                        return `<span class="m-badge  m-badge--primary m-badge--wide">Femenino</span>`
                    }

                }
            }
        ]
    };

    var loadDatatable = function () {
        if (datatable === null) {
            datatable = $("#ajax_data").DataTable(options);
        } else {
            datatable.ajax.reload();
        }
    };

    var validate = function () {
        $("#form_waist").validate({
            rules: {
                min: { required: true },
                max: { required: true }
            },
            submitHandler: function (form, event) {
                event.preventDefault();
                var n_minimo = $("#min").val();
                var n_maximo = $("#max").val();
                $.ajax({
                    type: 'GET',
                    url: `/welfare/reporte_cintura/${n_minimo}/${n_maximo}`.proto().parseURL(),
                    success: function (data) {
                        $("#val_m").html(data.man);
                        $("#val_f").html(data.woman);
                    }
                });
                loadDatatable();
            }

        });
    };

    return {
        load: function () {
            loadDatatable();
            validate();

        }
    };
}();

$(function () {
    reportWaist.load();
})