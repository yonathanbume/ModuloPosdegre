var InitApp = function () {

    var datatable = {
        students: {
            object: null,
            options: getSimpleDataTableConfiguration({
                url: "/director-carrera/estudiantes-invictos/get".proto().parseURL(),
                data: function (data) {
                    data.search = $("#search").val();
                },
                pageLength: 10,
                orderable: [],
                columns: [
                    {
                        title: "Nombre",
                        data: "user.fullName"
                    },
                    {
                        title: "Cod. Estudiante",
                        data: "user.userName"
                    },
                    {
                        title: "Carrera",
                        data: "career.name"
                    },
                    {
                        title: "Prog. Académico",
                        data: "academicProgram.name"
                    },
                    //{
                    //    data: null,
                    //    title:"Opciones",
                    //    render: function (data) {
                    //        //return `<div class="table-options-section">
                    //        //    <button onclick=InitApp.confirmation("${data.id}") data-id="${data.id}" class="btn btn-info btn-sm m-btn m-btn--icon m-btn--icon-only btn-detail" title=""><i class="la la-plus"></i></button>
                    //        //</div>`;
                    //        return "-";
                    //    }
                    //}
                ]
            }),
            events: {
                search: {
                    timing:0,
                    onChange: function () {
                        $("#search").keyup(function (){
                            clearTimeout(datatable.students.events.search.timing);
                            datatable.students.events.search.timing = setTimeout(function () {
                                datatable.students.object.ajax.reload();
                            },500);
                        });
                    },
                    init: function () {
                        this.onChange();
                    }
                },
                init: function () {
                    this.search.init();
                }
            },
            init: function () {
                datatable.students.object = $("#students_table").DataTable(datatable.students.options);
                this.events.init();
            }
        }
    };

    return {
        init: function () {
            datatable.students.init();
        }
    };
}();

$(function () {
    InitApp.init();
});