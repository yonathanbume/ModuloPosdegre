InitApp = function () {

    var datatable = {
        students: {
            object: null,
            options: {
                ajax: {
                    url: `/profesor/evaluacion-extraordinaria/get`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.search = $("#search").val();
                        data.termId = "";
                    }
                },
                columns: [
                    {
                        title: "Cod. Curso",
                        data: "courseCode"
                    },
                    {
                        title: "Curso",
                        data: "courseName"
                    },
                    {
                        title: "Escuela Profesional",
                        data : "career"
                    },
                    {
                        title: "Plan de Estudio",
                        data : "curriculum"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        orderable: false,
                        render: function (data) {
                            var tpm = `<a href='/profesor/evaluacion-extraordinaria/detalles/${data.id}' class="btn btn-info btn-sm m-btn m-btn--icon"><span><i class="la la-edit"></i><span> Gestionar </span></span></a>`;
                            return tpm;
                        }
                    }
                ]
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
            },
            reload: function () {
                this.object.ajax.reload();
            }
        }
    };

    var events = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.students.reload();
            });
        }
    };

    return {
        init: function () {
            datatable.students.init();
            events.init();
        }
    };
}();

$(function () {
    InitApp.init();
});