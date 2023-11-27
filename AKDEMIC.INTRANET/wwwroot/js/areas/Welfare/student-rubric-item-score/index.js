InitApp = function () {

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
    var options = {
        columnDefs: [
            { "orderable": false, "targets": [1] }
        ],
        ajax: {
            type: "GET",
            dataType: 'JSON',
            url: `/welfare/evaluacion-socieconomica/obtener-estudiantes`.proto().parseURL(),
            data: function (values) {
                values.searchValue = $("#search").val();
                values.careerId = $("#careerId").val();
                values.facultyId = $("#facultyId").val();
                values.academicProgramId = $("#program_academicId").val();
            }
        },
        columns: [
            { data: "code", title: "Código" },
            { data: "name", title: "Nombre Completos" },
            { data: "career", title: "Escuela Profesional" },            
            {
                data: null,
                title: "Opciones",
                render: function (data, type, row, meta) {
                    var tmp = `<button type="button" data-id="${data.id}" class="btn btn-sm btn-brand evaluate"> Evaluar </button>`;
                    return tmp;
                }
            }
        ]
    };

    var select = {
        career: function (facultyId) {
            $.ajax({
                type: 'GET',
                url: `/welfare/evaluacion-socieconomica/carreras/${facultyId}/get`.proto().parseURL()
            }).done(function (data) {
                $("#careerId").select2({
                    data: data.items
                });
            });

            $("#careerId").on('change', function () {
                var careerId = $(this).val();
                select.program_academic(careerId);
            });
            $("#careerId").trigger('change');
        },
        faculty: function () {
            $.ajax({
                type: 'GET',
                url: `/welfare/evaluacion-socieconomica/facultades/get`.proto().parseURL()
            }).done(function (data) {
                $("#facultyId").select2({
                    data: data.items
                });
            });
            //$('#careerId').append(`<option value="${null}"> 
            //                           Todos
            //                      </option selected>`);

            $("#facultyId").on('change', function () {
                var facultyId = $(this).val();
                select.career(facultyId);
            });
            $("#facultyId").trigger('change');

        },
        program_academic: function (careerId) {
            $.ajax({
                type: 'GET',
                url: `/welfare/evaluacion-socieconomica/programaacademicos/${careerId}/get`.proto().parseURL()
            }).done(function (data) {
                $("#program_academicId").select2({
                    data: data.items
                });
                private.objects["tbl-data"].draw();
            }); 
            $("#program_academicId").on('change', function () {
                private.objects["tbl-data"].draw();
            });


            //$('#careerId').append(`<option value="${null}"> 
            //                           Todos
            //                      </option selected>`);

            //$("#careerId").on('change', function () {
            //    private.objects["tbl-datatable-registry-patterns"].draw();
            //});
        }

    };

    var events = {
        datatable_init: function () {
            private.objects["tbl-data"].on("click", ".evaluate",
                function () {
                    var id = $(this).data('id');
                    window.location.href = `/welfare/evaluacion-socieconomica/calificacion/${id}`.proto().parseURL();
                });           
            
        }

    };
    var dataTable = {
        init: function () {
            private.objects["tbl-data"] = $("#rubric-item-score-datatable").DataTable(options);
            events.datatable_init();
        }
    };



    return {
        init: function () {
            inputs.init();
            dataTable.init();
            select.faculty();
            select.career();
            select.program_academic();
            
            
        }
    }
}();

$(function () {
    InitApp.init();
});