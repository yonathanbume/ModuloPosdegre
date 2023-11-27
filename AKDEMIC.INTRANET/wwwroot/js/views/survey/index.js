var InitApp = function () {
    var table = $('#table-surveys');
    var divInitRow = '<tr class=""><td><div class="row row-survey">';
    var divCloseRow = '</tr></td></div>';

    var tablejs = {
        paginate: function () {
            var currentPage = 0;
            var numPerPage = 3;
            table.bind('repaginate', function () {
                table.find('tbody tr').hide().slice(currentPage * numPerPage, (currentPage + 1) * numPerPage).show();
            });

            table.trigger('repaginate');
            var numRows = table.find('tbody tr').length;
            console.log(numRows);
            var numPages = Math.ceil(numRows / numPerPage);
            var pager = $('<div class="pager"></div>');
            for (var page = 0; page < numPages; page++) {
                $('<span class="page-number"></span>').text(page + 1).bind('click', {
                    newPage: page
                }, function (event) {
                    currentPage = event.data['newPage'];
                    table.trigger('repaginate');
                    $(this).addClass('active').siblings().removeClass('active');
                }).appendTo(pager).addClass('clickable');
            }
            pager.insertAfter(table).find('span.page-number:first').addClass('active');


        },
        init: function () {

            $.ajax({
                url: ('/encuestas/getsurveybyuser').proto().parseURL(),
                type: 'GET',
                success: function (surveys) {
                    var div = '';
                    for (var key = 0; key < surveys.length; key++) {
                        if (key % 3 === 0) {
                            div += divInitRow;
                        }
                        div += '<div class="col-xl-4">';
                        div += '<div class="m-portlet m-portlet--bordered-semi m-portlet--full-height bottom-rigth-radius">';
                        div += '<div class="m-portlet__head m-portlet__head--fit">';
                        div += '<div class="m-portlet__head-caption">';
                        div += '<div class="m-portlet__head-action">';
                        div += '<h5>';
                        div += surveys[key].name;
                        div += '</h5>';
                        div += '<label class="date-style"> Publicado:' + surveys[key].publicationDate;
                        div += '</label>';
                        div += '</div>';
                       
                        div += '</div>';
                        div += '</div>';
                        div += '<div class="m-portlet__body survey-detail" style="color:#ffffff;">';
                        div += '<div style="height:150px;">';
                        div += '<div class="padding-description">';
                        div += '<label>';
                        var description = surveys[key].description.slice(NaN, 120);
                        if (surveys[key].description.length > 120) {
                            description += "...";
                        }
                        div += description;
                        div += '</label>';
                        div += '</div>';
                        div += '<div class="padding-description">';
                        div += '<label class="date-style"> Finaliza el:';
                        div += surveys[key].finishDate;
                        div += '</label>';
                        div += '</div>';


                        div += '<div class="curve">';
                        div += '</div>';



                        div += '<div>';
                        div += '<a class="btn-detail" href="' + ("/encuestas/responder/" + surveys[key].id).proto().parseURL() + '">';
                        div += '<i class="la la-arrow-right icon-arrow">';
                        div += '</i>';
                        div += '</a>';
                        div += '</div>';
                        div += '</div>';
                        div += '</div>';
                        div += '</div>';
                        div += '</div>';

                        if (key % 3 === 2 || key === surveys.length - 1) {
                            div += divCloseRow;
                        }

                    }
                    $("#table-surveys tbody").append(div);
                    tablejs.paginate();
                }
            });
        }
    };
    return {
        init: function () {
            tablejs.init(); 
        }
    }
}();

$(function () {
    InitApp.init();
});
