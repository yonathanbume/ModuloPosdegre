var generateRelation = function () {

    var private = {
        objects: {}
    };
    var generateRelations = function () {
        $('.btn-generate').on('click', function () {
            $(this).addLoader();        
        var startDate = $("#StartDate").val();
        var endDate = $("#EndDate").val();
        var bookCode = $("#BookCode").val();
        if (startDate != "" && endDate != "") {
            window.open(`/admin/generacion-de-relacion-de-grados-titulos/generar?startDate=${startDate}&endDate=${endDate}&bookCode=${bookCode}`.proto().parseURL());
        }       
        $(this).removeLoader();
        });
    };
                
    
    var options = {
        //columnDefs: [
        //    { "orderable": false, "targets": [1] }
        //],
        ajax: {
            type: "GET",
            dataType: "JSON",
            url: `/admin/generacion-de-relacion-de-grados-titulos/listado`.proto().parseURL(),
            data: function (values) {
                values.startDate = $("#StartDate").val();
                values.endDate = $("#EndDate").val();
                values.bookCode = $("#BookCode").val();
                values.searchValue = $("#search").val();
            }
        },
        columns: [
            {
                data: "fullName",
                name: "fullName",
                title: "Estudiante"
            },
            {
                data: "career",
                name: "career",
                title: "Escuela Profesional"
            },
            {
                data: "gradeType",
                name: "gradeType",               
                title: "Grado Académico"
            }            
        ]

    };


    var dataTable = {
        init: function () {
            private.objects["tbl-datatable-registry-relation"] = $("#tbl-datatable-registry-relations").DataTable(options);
        }
    };


    var initializer = function () {
        $("#StartDate").datepicker();
        $("#EndDate").datepicker();
        $("#StartDate").on("changeDate", function (e) {
            $(this).valid();
            $("#EndDate").datepicker("setStartDate", e.date);
        });
        $("#EndDate").on("changeDate", function (e) {
            $(this).valid();
            $("#StartDate").datepicker("setEndDate", e.date);
        });
      
    };  



    var form = function () {
        $("#frmSearch").validate({
            submitHandler: function (form, event) {         
                if (private.objects["tbl-datatable-registry-relation"] != null) {
                    private.objects["tbl-datatable-registry-relation"].draw();
                }
                //$(this).addLoader();        
                //var startDate = $("#StartDate").val();
                //var endDate = $("#EndDate").val();
                //var bookCode = $("#BookCode").val();
                //window.open(`/admin/generacion-de-relacion-de-grados-titulos/generar?startDate=${startDate}&endDate=${endDate}&bookCode=${bookCode}`.proto().parseURL());
                dataTable.init();
                //window.open(`/admin/generacion-de-relacion-de-grados-titulos/generar`.proto().parseURL());
                //$(this).removeLoader();  
            }



        });
    };

    return {
        load: function () {
            initializer();
            //dataTable.init();
            generateRelations();
            form();

        }
    };

}();

$(function () {
    generateRelation.load();
});

