var Event = (function () {

    var roles = {
        init: function () {
            $.ajax({
                url: "/admin/eventos/roles/get".proto().parseURL()
            }).done(function (data) {
                $(".select2-roles").select2({
                    placeholder: "Roles",
                    minimumInputLength: 0,
                    data: data.items
                });
            });
        }
    };

    var modal = {
        file: {
            object: $("#modal_file"),
            form: {
                object: $("#form_file").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        var fileEntity = $("#form_file").find("[name='File']")[0].files[0];

                        var file = {
                            id: null,
                            name: $("#form_file").find("[name='Name']").val(),
                            url: null,
                            file: fileEntity
                        };

                        datatable.file.localData.push(file);
                        datatable.file.object.row.add([
                            file.name,
                            `<button type="button" data-id="${file.id}" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete" title="Eliminar"><span><i class="la la-trash"></i><span>Eliminar</span></span></button>`
                        ]).draw(false);

                        toastr.success("Archivo Agregado", "Hecho!");
                        modal.file.object.modal("hide");
                    }
                })
            },
            events: {
                onHidden: function () {
                    modal.file.object.on('hidden.bs.modal', function (e) {
                        modal.file.form.object.resetForm();
                        $("#form_file").find("[name='File']").val(null).trigger("change");
                        modal.file.object.find(".custom-file-label").text("Seleccionar un archivo");
                    });
                },
                init: function () {
                    this.onHidden();
                }
            },
            init: function () {
                modal.file.events.init();
            }
        },
        init: function () {
            this.file.init();
        }
    }

    var datatable = {
        file: {
            object: null,
            localData: [],
            options: {
                lengthMenu: [10, 25, 50],
                orderMulti: false,
                pagingType: "full_numbers",
                processing: true,
                responsive: true,
                serverSide: false,
                columns: [
                    { title: "Nombre" },
                    { title: "Opciones" }
                ]
            },
            events: {
                onDelete: function () {
                    $("#table_files").on("click", ".btn-delete", function () {
                        var id = $(this).data("id");
                        var trParent = $(this).parent().parent();
                        swal({
                            type: "error",
                            title: "Elimnará el archivo.",
                            text: "¿Seguro que desea eliminar el archivo seleccionado?.",
                            confirmButtonText: "Aceptar",
                            showCancelButton: true
                        }).then(function (isConfirm) {
                            if (isConfirm.value) {
                                var file = datatable.file.localData.filter((v) => v.id === id)[0];
                                var indexOf = datatable.file.localData.indexOf(file);
                                datatable.file.localData.splice(indexOf, 1);
                                datatable.file.object.row(trParent).remove().draw(false);
                            }
                        });
                    });
                },
                init: function () {
                    this.onDelete();
                }
            },
            init: function () {
                datatable.file.object = $("#table_files").DataTable(datatable.file.options);
                datatable.file.events.init();
            }
        },
        init: function () {
            datatable.file.init();
        }
    }

    var loadPicture = function () {
        $("#File").on("change",
            function (e) {

                var tgt = e.target || window.event.srcElement,
                    files = tgt.files;
                console.log(files[0]);
                // FileReader support
                if (FileReader && files && files.length) {
                    var fr = new FileReader();
                    fr.onload = function () {
                        $("#current-picture").attr("src", fr.result);
                    }
                    fr.readAsDataURL(files[0]);
                }
                // Not supported
                else {
                    console.log("File Reader not supported.");
                }
            });
    };

    var initFormValidation = function () {
        formCreate = $("#create-form").validate({
            rules: {
                File: {
                    required: true
                },
                UrlVideo: {
                    url : true
                }
            },
            submitHandler: function (formElements, e) {
                e.preventDefault();
                var formData = new FormData($(formElements)[0]);

                for (var i = 0; i < datatable.file.localData.length; i++) {
                    formData.append(`EventFiles[${i}].Id`, datatable.file.localData[i].id);
                    formData.append(`EventFiles[${i}].Name`, datatable.file.localData[i].name);
                    formData.append(`EventFiles[${i}].File`, datatable.file.localData[i].file);
                }

                $(`#create-form input, #create-form select,#create-form textarea`).attr("disabled", true);
                $(".add").addLoader();
                $.ajax({
                    url: $(formElements).attr("action"),
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false
                }).always(function () {
                    $(`#create-form input, #create-form select,#create-form textarea`).attr("disabled", false);
                    $(".add").removeLoader();
                }).done(function () {
                    swal({
                        type: "success",
                        title: 'Éxito',
                        text: 'Evento agregado correctamente',
                        confirmButtonText: "Ok"
                    }).then(function () {
                        window.location = "/admin/eventos".proto().parseURL()
                    });
                }).fail(function (e) {
                    var text = "";
                    if (e.responseText !== null)
                        text = e.responseText;
                    else
                        text = _app.constants.toastr.message.error.task;

                    swal({
                        type: "error",
                        title: _app.constants.toastr.title.error,
                        text: text,
                        confirmButtonText: "Ok"
                    });
                });
            }
        });
    };
    var initFormDatepickers = function () {
        $("#cRegistrationStartDate").datepicker()
            .on("changeDate", function (e) {
                $("#cRegistrationEndDate").datepicker("setStartDate", e.date);
            });

        $("#cRegistrationEndDate").datepicker()
            .on("changeDate", function (e) {
                $("#cRegistrationStartDate").datepicker("setEndDate", e.date);
            });
        $("#cEventDate").datepicker();

        $("#Cost").keypress(function (event) {
            if (event.which > 31 && (event.which < 48 || event.which > 57) && event.which != 46) {
                event.preventDefault();
            }
        }).on('paste', function (event) {
            event.preventDefault();
        });

    };

    var initCall = function () {
        $(document).on('change', '.custom-file-input', function (event) {
            $("#File").valid();
            $(this).next('.custom-file-label').html(event.target.files[0].name);

        });

    };

    return {
        init: function () {
            loadPicture();
            roles.init();
            initFormValidation();
            initFormDatepickers();
            initCall();
            datatable.init();
            modal.init();
        }
    }


})();

$(function () {
    Event.init();
});
