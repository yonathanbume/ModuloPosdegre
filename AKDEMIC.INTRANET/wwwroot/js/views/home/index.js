var announcementHome = (function () {
    var result = {
        viewMore: function () {

            $(".btn-download").on('click', function () {
                var $btn = $(this);
                var id = $btn.data('id');
                window.open(`/descargar-comunicado/pdf/${id}`.proto().parseURL(), '_blank');
            });

            if ($(".carousel-item.active input[name='txtAll']").length) {
                var strViewMore = $(".carousel-item.active input[name='txtAll']")["0"].value;
                $(".carousel-item.active p")["0"].innerHTML = strViewMore;   
            }
           
        }
    }
    return result.viewMore();
})();