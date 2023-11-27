var reportAssistControlDetails = function () {
    var datatable = null;
    var sid = $("#IdSection").val();
    var absencesTable=null;
    var loadDatatable = function () {

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }

        datatable = $(".m-datatable-students").mDatatable(options);
      
        $(".m-datatable-students").on("click", ".btn-details", function(){
            var studentid = $(this).data("studentid");
            $.ajax({
                url: `/profesor/reporte_asistencia/alumnos/get-inasistencias/${sid}/${studentid}`.proto().parseURL()
            }).done(function (result) {

                if(absencesTable!= null && absencesTable!= undefined){
                    absencesTable.clear();
                    absencesTable.destroy();
                }

                absencesTable= $("#absences-datatable").DataTable({
                    serverSide:false,
                    columns: [
                    {
                        title: 'Fecha'
                    },]
                });
                $.each(result, function(index,value){
                    absencesTable.row.add([
                        value
                    ]).draw(false);
                });
                var result3 = result;
            });
        });
    };
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/profesor/reporte_asistencia/alumnos/get/${sid}`.proto().parseURL()
                }
            }
        },
        columns: [
            {
                field: 'fullName',
                title: 'Nombres Completos',
                width: 150
            },
            {
                field: 'absences',
                title: 'Faltas',
                width: 150
            },
            {
                field: 'assisted',
                title: 'Asistencias',
                width: 150
            },
            {
                field: 'dictated',
                title: 'Clases Dictadas',
                width: 70
            },
            {
                field: 'options',
                title: 'Opciones',
                template: function(data){
                    return `<button data-toggle="modal" data-studentid="${data.studentId}" data-target="#assistances_modal" id="" class="btn btn-primary btn-sm m-btn m-btn m-btn--icon btn-details"><i class="la la-eye"></i></button>`;
                }
            }
        ]

    }

    return {
        load: function () {
           loadDatatable();
        }
    }
}();

var InitApp = function () {

    var absencesTable = null;

    var datatable = {
        students: {
            object: null,
            options: {
                serverSide: false,
                ajax: {
                    url: `/profesor/reporte_asistencia/alumnos/get/${$("#sectionId").val()}`.proto().parseURL(),
                    //data: function (data) {
                    //    data.sectionId = $("#Id").val();
                    //}
                    dataSrc: ""
                },
                columns: [
                    {
                        data: 'userName',
                        title: 'Código',
                        width: "150px",
                    },
                    {
                        data: 'fullName',
                        title: 'Nombre Completo',
                    },
                    {
                        data: 'assisted',
                        title: 'Asistencias',
                        orderable: false,
                    },
                    {
                        data: 'absences',
                        title: 'Faltas',
                        orderable: false,
                    },
                    {
                        data: 'absencesPercentage',
                        title: '% de Inasis.',
                        orderable: false,
                        render: function (row) {
                            return `${row} %`;
                        }
                    },
                    {
                        data: 'dictated',
                        title: 'Clases dictadas',
                        orderable: false,
                    },
                    {
                        data: 'totalClasses',
                        title: 'Total de Clases',
                        orderable: false,
                    },
                    {
                        data: null,
                        title: 'Opciones',
                        orderable: false,
                        render: function (data) {
                            return `<button data-toggle="modal" data-studentid="${data.studentId}" data-target="#assistances_modal" id="" class="btn btn-primary btn-sm m-btn m-btn m-btn--icon btn-details"><i class="la la-eye"></i></button>`;
                        }
                    }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        text: 'Excel',
                        action: function (e, dt, node, config) {
                            window.open(`/profesor/reporte_asistencia/reporte-excel/${$("#sectionId").val()}`, "_blank");
                        }
                    }
                ]
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);

                $("#data-table").on("click", ".btn-details", function () {
                    var studentid = $(this).data("studentid");
                    $.ajax({
                        url: `/profesor/reporte_asistencia/alumnos/get-inasistencias/${$("#sectionId").val()}/${studentid}`.proto().parseURL()
                    }).done(function (result) {

                        if (absencesTable != null && absencesTable != undefined) {
                            absencesTable.clear();
                            absencesTable.destroy();
                        }

                        absencesTable = $("#absences-datatable").DataTable({
                            serverSide: false,
                            pageLength: 10,
                            columns: [
                                {
                                    title: 'Fecha'
                                },]
                        });

                        $.each(result, function (index, value) {
                            absencesTable.row.add([
                                value
                            ]).draw(false);
                        });
                    });
                });
            }
        }
    };

    return {
        init: function () {
            datatable.students.init();
        }
    }
}();

$(function () {
    //reportAssistControlDetails.load();

    InitApp.init();
})