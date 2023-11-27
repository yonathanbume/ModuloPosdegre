InitApp = function () {    

    
    var calificate = function () {

        $("#btn-submit").on('click', function () {
            var btn = $(this);
            btn.addLoader();
            var arrayElementList = [];
            arrayElementList = $(".calificate");
            //arrayElementList.push({ score:  }, { studentRubricItemId: });

            var objectList = new FormData();
            for (var i = 0; i < arrayElementList.length; i++) {
                objectList.append(`model[${i}].Score`, $(arrayElementList[i]).val());
                objectList.append(`model[${i}].StudentRubricItemId`, $(arrayElementList[i]).data('rubricitem'));
            }
            var id = $("#studentId").val();  
            $.ajax({
                type: 'POST',
                url: `/welfare/evaluacion-socieconomica/calificar/${id}`.proto().parseURL(),
                processData: false,
                contentType: false,
                data: objectList,
                success : function () {
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                },
                error: function () {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                },
                complete: function () {
                    btn.removeLoader();
                }
            });
        });

    };



    return {
        init: function () {
            calificate();
        }
    };
}();

$(function () {
    InitApp.init();
});




//var objectList = new FormData();
////console.log(arrayAcademicList);
////return false;
//for (var i = 0; i < arrayDegreeList.length; i++) {

//    if (arrayDegreeList[i].toSend === true) {
//        objectList.append(`ListRendered[${i}].DescriptionDegree`, arrayDegreeList[i].description);
//        objectList.append(`ListRendered[${i}].InstitutionDegree`, arrayDegreeList[i].institution);
//    }
//}
//$.ajax({
//    type: 'POST',
//    url: `/estudiante/perfil/guardar_grados_titulos/${id}`.proto().parseURL(),
//    processData: false,
//    contentType: false,
//    data: objectList,
//    success: function (dataListDegree) {
//        $("#EditDegree").modal('hide');
//        if (dataListDegree.length > 0) {
//            $("#DegreeContent").html('');
//            for (var i = 0; i < dataListDegree.length; i++) {
//                $("#DegreeContent").append(`
//                                <div style="margin-bottom:10px;">
//                                    <i class="fa fa-graduation-cap m--font-primary"></i>       
//                                    <text class="m--font-primary" style="font-weight:bolder; font-size:15px;">${dataListDegree[i].description}</text>
//                                    <br>
//                                    <text >Dictado por ${dataListDegree[i].institution}</text>
//                                    <br>
//                                </div>                          
//                        `);
//            }

//        } else {
//            $("#DegreeContent").html('');
//            $("#DegreeContent").append(`
//                                <div id="DegreeDefault"><label class= "pull-left" > Aún no ha agregado ningún grado o título</label ></div>                        
//                            `);
//        }

//        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
//    },
//    error: function () {
//        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
//    }

//});