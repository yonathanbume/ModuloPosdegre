var InitApp = function () {
    var evaluationReportId = $("#EvaluationReportId").val();
    var datatable = {
        studentsreport: {
            object: null,
            options: getSimpleDataTableConfiguration({
                url: `/admin/generacion-actas/acta/${evaluationReportId}/datatable`.proto().parseURL(),
                data: function (data) {
                    data.serachValue = $("#search").val();
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Cod",
                        data: "code"
                    },
                    {
                        title: "Estudiante",
                        data: "student"
                    },
                    {
                        title: "Nota",
                        data: "finalGrade"
                    }                    
                ]
            }),
            reload: function () {
                datatable.studentsreport.object.ajax.reload();
            },
            init: function () {
                datatable.studentsreport.object = $("#data-table").DataTable(datatable.studentsreport.options);
            }
        },
        init: function(){
            this.studentsreport.init();
        }
    };

    var events = {
        onSearchStudent: function () {
            $("#search").doneTyping(function () {
                datatable.studentsreport.reload();
            })
        },
        init: function () {
            this.onSearchStudent();
        }
    };

    return {
        init: function () {
            datatable.init();
            events.init();
        }
    }
}();

$(function () {
    InitApp.init();
})