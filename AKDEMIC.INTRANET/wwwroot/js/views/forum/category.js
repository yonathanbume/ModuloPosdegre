var Topics = function () {
    var currentPage = 0;
    var numPerPage = 10;
    var table = $('#table-topics');

    var events = {
        init: function () {
            table.bind('repaginate', function () {
                table.find('tbody tr').hide().slice(currentPage * numPerPage, (currentPage + 1) * numPerPage).show();
            });

            $('#File').on('change', function () {
                let fileName = $(this).val();  
                $(this).next().next().html(fileName);
            });

            table.trigger('repaginate');
            var numRows = table.find('tbody tr').length;
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
        }
    }
    var validate = function(){
        $("#forums-modal-create-form").validate();

        $("#forums-modal-create").find("button[type=submit]").click(function () {
            mApp.block($("#forums-modal-create").find(".modal-content"));
        });
    }

    return {
        init: function () {
            validate();
            events.init();
        }
    }
}(); 

$(function () {
    Topics.init();
});

 