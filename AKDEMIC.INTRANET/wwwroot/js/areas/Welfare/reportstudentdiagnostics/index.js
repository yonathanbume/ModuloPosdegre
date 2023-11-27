﻿var reportstudentdiagnostics = function () {
    var datatable = null;
    var datatableStudent = null;

    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/welfare/reporte-estudiantes-tipo-diagnostico/detalle`.proto().parseURL()
                }
            }
        },
        columns: [

            {
                field: 'name',
                title: 'Descripción del diagnóstico psicológico',
                width: 600
            },
            {
                field: 'count',
                title: 'N° de estudiantes',
                width: 200
            },

        ],
        search: {
            input: $("#search")
        }
    };

    var options_students = {
        
        data: {
            source: {
                read: {
                    method: "GET",
                    url: `/welfare/reporte-estudiantes-tipo-diagnostico/estudiantes-por-diagnostico`.proto().parseURL()
                }
            }
        },
        columns: [

            {
                field: 'userName',
                title: 'Código',
                width: 200
            },
            {
                field: 'fullname',
                title: 'Nombres Completos',
                width: 250
            },
            {
                field: 'faculty',
                title: 'Escuela Profesional',
                width: 200
            },
            {
                field: 'email',
                title: 'Email',
                width: 200
            },
            {
                field: 'diagnostic',
                title: 'Diagnóstico',
                width: 500
            }

        ],
        search: {
            input: $("#search2")
        }

    };


    var loadDatatable = function () {

        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        datatable = $(".m-datatable").mDatatable(options);

    };

    var loadDatatableStudents = function () {

        if (datatableStudent !== null) {
            datatableStudent.destroy();
            datatableStudent = null;
        }
        datatableStudent = $(".m-datatable-students").mDatatable(options_students);

    };
    


    return {
        load: function () {

            loadDatatable();
            loadDatatableStudents();
        }
    };
}();

$(function () {
    reportstudentdiagnostics.load();
})