var InitApp = function () {
    var arrayReceipt = [];
    var createForm = null;
    var createReceipt = null;

    var hasReceipt = $("#hasReceipt").val().toLowerCase() == 'true';
    var hasPicture = $("#hasPicture").val().toLowerCase() == 'true';

    var requerid = {
        init: function () {
            if (hasReceipt)
                $("#valReceipt").prop('required', true);
            else
                $("#valReceipt").prop('required', false);

            if (hasPicture)
                $("#Image").prop('required', true);
            else
                $("#Image").prop('required', false);
        }
    };

    var datepicker = {
        init: function () {
            var date = new Date();
            date.setDate(date.getDate());
            $(".date-picker").datepicker({
            });
        }
    }

    var form = {
        init: function () {
            $("#createForm").validate({
                submitHandler: function (e) {
                    mApp.blockPage();
                    e.submit();
                }
            });
        },
        create: {
            submit: function (formElement) {
                mApp.block(".m-content", { type: "loader", message: "Cargando" });

                var data = $(formElement).serialize();
                var formData = new FormData($(formElement).get(0));

                $("#procedure-modal-request-form").find(".btn-submit").addLoader();
                $("#procedure-modal-request-form").attr("disabled", true);

                if (arrayReceipt.length > 0) {
                    formData.append("PaymentReceipt.Datetime", arrayReceipt[0].datetime);
                    formData.append("PaymentReceipt.Sequence", arrayReceipt[0].sequence);
                    formData.append("PaymentReceipt.Amount", arrayReceipt[0].amount);
                }

                var insertFiles = [];
                for (var j = 0; j < filesAdded.length; j++) {
                    if (!filesAdded[j].isdatabase)
                        insertFiles.push({ file: filesAdded[j].file, name: filesAdded[j].name });
                }

                for (var j = 0; j < insertFiles.length; j++) {
                    formData.append("DocumentFiles", insertFiles[j].file);
                }

                
                $.ajax({
                    type: formElement.method,
                    url: formElement.action,
                    data: formData,
                    contentType: false,
                    processData: false
                })
                    .done(function (result) {
                        window.location.href = `/tramites/usuarios?tab=2`.proto().parseURL();
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        var responseText = jqXHR.responseText;

                        if (responseText != "" && jqXHR.status == 400) {
                            toastr.error(responseText, _app.constants.toastr.title.error);
                        } else {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        }

                        $("#procedure-modal-request-form").attr("disabled", false);
                        mApp.unblock(".m-content");
                    });
            }
        },
        receipt: {
            show: function () {
                $("#receipt-modal").modal("show");
                $("#receipt-modal").one("hidden.bs.modal",
                    function () {

                    });
            },
            submit: function (formElement) {

                var data = $(formElement).serialize();
                $(formElement).find("#btnAddReceipt").addLoader();
                $(formElement).find("input").prop("disabled", true);

                var formData = new FormData($(formElement).get(0));
                var formElements = formElement.elements;

                $.ajax({
                    url: $(formElement).attr("action"),
                    type: "POST",
                    data: data
                })
                    .always(function () {
                        $(formElement).find("#btnAddReceipt").removeLoader();
                        $(formElement).find("input").prop("disabled", false);
                    })
                    .done(function () {
                        arrayReceipt = [];
                        arrayReceipt.push(
                            {
                                datetime: formElements["PaymentReceipt_Datetime"].value,
                                sequence: formElements["PaymentReceipt_Sequence"].value,
                                amount: formElements["PaymentReceipt_Amount"].value
                            });

                        $("#valReceipt").html("");
                        $("#valReceipt").val(`${arrayReceipt[0].datetime}  -  N° ${arrayReceipt[0].sequence}  -  S/. ${arrayReceipt[0].amount}`);

                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        $("#receipt-modal").modal("hide");

                        $("#add_reference_msg").addClass("m--hide").hide();
                    }).fail(function (e) {
                        toastr.error(e.responseText, _app.constants.toastr.title.error);
                        if (e.responseText != null) $("#add_form_msg_txt").html(e.responseText);
                        else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        $("#add_reference_msg").removeClass("m--hide").show();
                    });

            }
        },
        image: {
            show: function () {
                $("#image-modal").modal("show");
                $("#image-modal").one("hidden.bs.modal",
                    function () {

                    });
            },
            save: function () {
                $("#btnCropSave").addLoader();

                $("#btnAddReceipt").removeLoader();


                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                $("#image-modal").modal("hide");
            }
        }
    };

    var upload = {
        init: function () {
            $('#Image').on('change', function () {
                var reader = new FileReader();
                reader.onload = function (event) {

                    var image = $('#img-offi')[0];
                    var container = $('.cropper-container cropper-bg').prevObject;
                    //$("#upload-offi div").html("");
                    $("#upload-offi div").remove();
                    var cropper = new Cropper(image);
                    cropper.destroy();
                    cropper.reset();
                    cropper.clear();
                    cropper = null;


                    container.remove();

                    $('.preview').css({
                        width: '100%', //width,  sets the starting size to the same as orig image  
                        overflow: 'hidden',
                        height: 300,
                        maxWidth: 300,
                        maxHeight: 300
                    });


                    $("#img-offi").attr("src", event.target.result);
                    $("#cropper-hide").attr("src", event.target.result);


                    cropper = new Cropper(image, {
                        dragMode: 'move',
                        preview: '.preview',
                        aspectRatio: 12 / 12,
                        minContainerWidth: 300,
                        maxContainerWidth: 300,
                        minContainerHeight: 300,
                        maxContainerHeight: 300,
                        background: false,
                        viewMode: 1, aspectRatio: 1,
                        responsive: true,
                        autoCrop: true,
                        ready: function () {
                            $('#crop_button').trigger('click');
                        },
                    });

                    $('#btnCropSave').on('click', function () {

                        $("#btnCropSave").addLoader();

                        var croppedimage = cropper.getCroppedCanvas().toDataURL("image/png");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        $("#image-modal").modal("hide");

                        $("#imageFile").prop('required', false);
                        $("#img-offi-preview").attr("src", croppedimage)
                        $("#urlCropImg").val(croppedimage)

                        $("#btnCropSave").removeLoader();


                       // window.location.href = `/tramites/usuarios?tab=2`.proto().parseURL();

                    });

                }
                $("#Image").next().html("Reemplazar imagen");
                reader.readAsDataURL(this.files[0]);
            });

        }
    }

    var events = {
        init: function () {
            $(".btn-receipt").on("click",
                function () {
                    form.receipt.show();
                });
            $(".btn-image").on("click",
                function () {
                    form.image.show();
                });
        }
    };

    var validate = {
        init: function () {
            createForm = $("#procedure-modal-request-form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.create.submit(formElement);
                }
            });
            createReceipt = $("#create-receipt-form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.receipt.submit(formElement);
                }
            });

        }
    };

    return {
        init: function () {
            datepicker.init();
            validate.init();
            events.init();
            form.init();
            upload.init();
            requerid.init();
        }
    };
}();

var filesAdded = [];
var deletedFiles = [];

var filesTable = function () {
    var private = {
        objects: {}
    };

    var options = {
        serverSide: false,
        processing: false,
        lengthChange: false,
        lengthMenu: [5],
        columnDefs: [
            { "orderable": false, "targets": [2] }
        ],
        data: private.objects["lst-files"],
        columns: [
            { data: "name" },
            {
                data: null,
                render: function (result) {
                    var bytes = result.size;
                    return _app.modules.file.getFormattedFileSize(bytes);
                }
            },
            {
                data: null,
                render: function (result) {
                    return `<div class="table-options-section">
								<button data-id="${result.id}"  data-isdatabase="${result.isdatabase}" data-name="${result.name}" class ="btn btn-danger btn-sm m-btn m-btn--icon btn-delete" title="Eliminar"><i class ="la la-trash"></i></button>
							</div>`;
                }
            }
        ]
    };

    var dataTable = {
        init: function () {
            private.objects["lst-files"] = [];
            private.objects["tbl-files"] = $("#tbl-files").DataTable(options);
            this.events();
        },
        clear: function () {
            filesAdded = private.objects["lst-files"];
            private.objects["tbl-files"].clear();
            private.objects["tbl-files"].rows.add(private.objects["lst-files"]);
            private.objects["tbl-files"].draw();
        },
        events: function () {
            private.objects["tbl-files"].on("click", ".btn-delete", function () {
                var id = $(this).data("id");
                let isdatabase = $(this).data("isdatabase");
                let name = $(this).data("name");
                if (isdatabase)
                    deletedFiles.push({ Id: id, name: name });

                var index = -1;
                for (var i = 0; i < private.objects["lst-files"].length; i += 1) {
                    if (private.objects["lst-files"][i]["id"] === id) {
                        index = i;
                        break;
                    }
                }

                if (index > -1) {
                    private.objects["lst-files"].splice(index, 1);
                    dataTable.clear();

                    if (private.objects["lst-files"].length < 1) {
                        $("#DocumentFiles").val("");
                        $("#DocumentFiles").next().html("Seleccione los documentos");
                    }
                }
            });
        }
    };

    var fileInput = {
        init: function () {
            $("#DocumentFiles").on("change", function (e) {
                var tgt = e.target || window.event.srcElement,
                    files = tgt.files;
                // FileReader support
                if (FileReader && files && files.length) {
                    var arrayOfFiles = [];
                    $.each(files, function (index, item) {
                        //  arrayOfFiles.push({
                        private.objects["lst-files"].push({
                            id: index,
                            name: item.name,
                            size: item.size,
                            file: item,
                            isdatabase: false
                        });
                    });
                    //private.objects["lst-files"] = arrayOfFiles;
                    //filesAdded = private.objects["lst-files"];
                    dataTable.clear();
                    $("#DocumentFiles").next().html("Documentos seleccionados correctamente");
                } else {
                    // private.objects["lst-files"] = [];
                    //   filesAdded = [];
                    dataTable.clear();
                    $("#DocumentFiles").next().html("Seleccione los documentos");
                }
            });
        }
    };
    return {
        init: function () {
            fileInput.init();
            dataTable.init();
        }
    };
}();

$(function () {
    InitApp.init();
    filesTable.init();
});