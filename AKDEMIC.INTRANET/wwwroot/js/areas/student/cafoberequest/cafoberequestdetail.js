var InitApp = function () {
    var datatable = {
        cafobeRequest: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/estudiante/rendicion-cuenta/get".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.searchValue = $("#search").val();
                        data.type = $("#type").val();
                        data.status = $("#status").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Periodo Académico",
                        data: "termName"
                    },
                    {
                        title: "Tipo",
                        data: "typeText"
                    },
                    {
                        title: "Estado",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";

                            if (data.status == 0) {
                                template += `<span class="m-badge m-badge--primary m-badge--wide">${data.statusText}</span>`
                            } else if (data.status == 1) {
                                template += `<span class="m-badge m-badge--success m-badge--wide">${data.statusText}</span>`
                            } else if (data.status == 2) {
                                template += `<span class="m-badge m-badge--danger m-badge--wide">${data.statusText}</span>`
                            } else if (data.status == 3) {
                                template += `<span class="m-badge m-badge--warning m-badge--wide">${data.statusText}</span>`
                            } else {
                                template += `<span class="m-badge m-badge--secondary m-badge--wide">${data.statusText}</span>`
                            }
                            return template;
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var tmp = "";
                            //Delete

                            tmp += `<button type="button" data-id="${data.id}"class="btn btn-primary btn-sm m-btn m-btn--icon btn-edit" title="Rendir Cuenta"><span><span><i class="la la-edit"></i></span><span>Rendir Cuenta</span></span></button>`;
                            return tmp;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#data-table").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");

                    modal.load(id);
                });

            }

        },
        init: function () {
            this.cafobeRequest.init();
        }
    };


    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.cafobeRequest.reload();
            });
        }
    };

    var select = {
        init: function () {
            this.types.init();
        },
        types: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $("#type").select2();
            },
            events: function () {
                $("#type").on("change", function () {
                    datatable.cafobeRequest.reload();
                });
            }
        }
    };

    var modal = {
        create: {
            object: $("#add-form").validate({
                submitHandler: function (form, e) {
                    $("#btnSave").addLoader();
                    e.preventDefault();
                    let formData = new FormData($(form)[0]);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        datatable.cafobeRequest.reload();
                        modal.create.clear();
                        $("#btnSave").removeLoader();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnSave").removeLoader();
                    }).always(function () {

                    });
                }
            }),
            show: function () {
                $("#addModal").modal("toggle");
                $("#btnSave").removeLoader();

            },
            clear: function () {
                modal.create.object.resetForm();
            },
            events: function () {
                $("#addModal").on("hidden.bs.modal", function () {
                    modal.create.clear();
                    $("#FileDetailLabel").html("Seleccione Archivo");
                });
            }
        },
        load: function (id) {
            console.log("hola");
            modal.create.show();
            $.ajax({
                url: (`/estudiante/rendicion-cuenta/get/${id}`).proto().parseURL(),
                type: "GET",

            }).done(function (result) {
                console.log(result);
                $("#add-form input[name=CafobeRequestId]").val(result.cafobeRequestId);
                $("#add-form input[name=StatusText]").val(result.statusText);
                $("#add-form input[name=Comentary]").val(result.comentary);
            }).fail(function (error) {
                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                $("#add_form_msg").removeClass("m--hide").show();
            }).always(function () {

            });
        },
        init: function () {
            this.create.events();
        }

    };


    return {
        init: function () {
            datatable.init();
            search.init();
            select.init();
            modal.init();
        }
    }
}();

$(function () {
    InitApp.init();
})