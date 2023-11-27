var posts = function () {

    var validate = function () {
        $("#new-post").validate();
    }

    var currentPage = 0;
    var numPerPage = 5;
    var table = $('#table-topics');
    var events = {
        init: function () {
            $('.btn-delete-post').on('click', function () {
                var dataId = $(this).data("postid");
                swal({
                    title: "¿Está seguro?",
                    text: "El post será eliminado",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminar",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    allowOutsideClick: () => !swal.isLoading(),
                    preConfirm: () => {
                        return new Promise(() => {
                            $.ajax({
                                url: ("/foro/post/eliminar/post").proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: dataId
                                },
                                success: function (text) { 
                                    if (text === '') {
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El post ha sido eliminada con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                        location.reload();
                                    }
                                    else {
                                        var categoryId = $("#categoryId").val();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: text,
                                            confirmButtonText: "Excelente"
                                        });
                                        window.location.href = (`/foro/categoria/${categoryId}`).proto().parseURL();
                                    }
                                },
                                error: function () {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "Al parecer el post tiene información relacionada"
                                    });
                                }
                            });
                        });
                    }
                });
            });

            table.bind('repaginate', function () {
                table.find('tbody tr').hide().slice(currentPage * numPerPage, (currentPage + 1) * numPerPage).show();
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


            $("body").on("click", ".message-comments-toggler", function () {
                var id = $(this).data("toggle-id");
                var icon = $(this).find("i");
                $(icon).toggleClass("la-angle-down");
                $(icon).toggleClass("la-angle-up");
                $(`.message-comments-level-2.${id}`).collapse("toggle");
            });

            $("body").on("click", ".message-reply-button", function () {
                var id = $(this).data('id');
                console.log(id);
                var fullName = $(this).data('fullname');

                $(".citation-div").collapse("show");
                $(".citation-target").text(fullName);
                $('#PostCitedId').val(id);
            });

            $("body").on("click", ".delete-citation-target", function () {
                $(".citation-div").collapse("hide");
                $(".citation-target").text("");
                $('#PostCitedId').val('');
            });

            $(".fileselect-trigger").click(function () {
                $("#new-post #File").trigger("click");
            });

            $("#new-post #File").change(function () {
                var fileName = $(this).val();

                if (fileName !== null && fileName.length > 0) {
                    $(".fileselect-trigger > span").removeClass("d-none");
                } else {
                    $(".fileselect-trigger > span").addClass("d-none");
                }
            });


            $("#new-post").find("button[type=submit]").click(function () {
                mApp.block($("#new-post"));
            });

        }
       
    };     

    return {
        init: function(){
            events.init();  
        },
        deleteCitation: function () {
            $('.deleteCitation').remove();
            $('#citedText').text('');
            $('#PostCitedId').val('')
            $('#PostCitedId').text('')
        },
        addCitation: function (element) {
            var postId = $(element).data('postid');
            var fullName = $(element).data('fullname');
            var removeButton = '<button style={margin-left:5px;} class="deleteCitation btn btn-default" onclick="posts.deleteCitation()" type="button"><i class="fa fa-trash"></i></button>';
            $('#citedText').text(fullName);
            if ($('#PostCitedId').val() == '') {
                $('#citedText').after(removeButton);
            }
            $('#PostCitedId').val(postId);
            $('#PostCitedId').text(postId);
        }

    }

}();

var comments = function () {
    var result = {
        ajax: {
            list: {},
            load: {
                create: function (element, event) {

                    var formElements = element.elements;    
                    var fdata = new FormData(form); 
                    comments.ajax.list["posts-ajax-create"] = $.ajax({ 
                        data: {
                             fdata
                        },
                        type: element.method,
                        enctype: 'multipart/form-data',
                        url: element.action,
                        processData: false,  
                        beforeSend: function (jqXHR, settings) {
                            $(element).addLoader();
                        },
                        complete: function (jqXHR, textStatus) {
                            $(element).removeLoader();
                        },
                        success: function (data, textStatus, jqXHR) {
                            toastr.success(_app.constants.toastr.message.success.create, _app.constants.toastr.title.success);
                            location.reload();
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                        }
                    });
                }
            }
        },
        validate: {
            list: {},
            load: {
                create: function () {
                    comments.validate.list["new-post"] = $("#new-post").validate({
                        submitHandler: function (form, event) {
                            comments.ajax.load.create(form, event);
                        }
                    });
                }
            }
        }
    }
    return result;
}();

$(function () { 
    posts.init();
    comments.validate.load.create();
});