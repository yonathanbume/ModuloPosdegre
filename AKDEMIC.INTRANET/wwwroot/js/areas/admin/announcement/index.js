var announcement = (function () {
    var result = {
        datatable: {
            list: {},
            load: {
                get: function () {
                    announcement.datatable.list["ajax_data"] = $("#ajax_data").mDatatable({
                        data: {
                            source: {
                                read: {
                                    method: "GET",
                                    url: ("/admin/anuncios/get").proto().parseURL()
                                }
                            }
                        },
                        columns: [
                            {
                                field: 'title',
                                title: 'Título',
                                width: 350
                            },
                            {
                                field: 'startdate',
                                title: 'Fecha inicio',
                                width: 200
                            },
                            {
                                field: 'enddate',
                                title: 'Fecha fin',
                                width: 200
                            },
                            {
                                field: 'options',
                                title: 'Opciones',
                                width: 400,
                                sortable: false,
                                filterable: false,
                                template: function (row) {

                                    var template = "";
                                    //template += `<a class="btn btn-info btn-sm m-btn m-btn--icon" href =`;
                                    //template += `/admin/anuncios/detalles/get/${row.id}`.proto().parseURL();
                                    //template += `> <span><i class="la la-eye"></i><span>Ver Detalle</span></span></a>`;                                    
                                    template += ` <a class="btn btn-primary btn-sm m-btn m-btn--icon" href=`;
                                    template += `/admin/anuncios/editar/get/${row.id}`.proto().parseURL();
                                    template += `><span><i class="la la-edit"></i><span>Editar</span></span></a>`;
                                    template += ` <button data-id ="${row.id}" class="delete btn btn-danger btn-sm m-btn m-btn--icon "   > <span><i class="la la-trash"></i><span>Eliminar</span></span></button> `;
                                    return template;
                                }
                            }
                        ]
                    });
                },
                addform: function () {
                    window.location.href = ('/admin/anuncios/agregar/get').proto().parseURL();
                },
                add: function () {
                    $("#add-form").validate({

                        rules: {
                            Title: { required: true },
                            Description: { required: true },
                            StartDate: { required: true },
                            EndDate: { required: true },

                        },
                        submitHandler: function (form, event) {
                            var formData = new FormData($(form).get(0));
                            event.preventDefault();
                            $("#btnSave").addLoader();
                            $.ajax({
                                type: "POST",
                                url: ("/admin/anuncios/crear/post").proto().parseURL(),
                                data: formData,
                                contentType: false,
                                processData: false,
                                success: function () {
                                    window.location.href = `/admin/anuncios`.proto().parseURL();
                                },
                                complete: function () {
                                    $("#btnSave").removeLoader();
                                }

                            });
                        }
                    });
                },
                delete: function () {
                    $("#ajax_data").on('click', '.delete', function (e) {
                        e.preventDefault();
                        var Id = $(this).data("id");
                        swal({
                            title: "¿Está seguro?",
                            text: "El anuncio será eliminado permanentemente",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Si, eliminarlo",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            cancelButtonText: "Cancelar"
                        }).then(function (result) {
                            if (result.value) {
                                $.ajax({
                                    url: `/admin/anuncios/eliminar/post/${Id}`.proto().parseURL(),
                                    type: "POST",
                                    success: function () {
                                        announcement.datatable.list["ajax_data"].reload();
                                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                    },
                                    error: function () {
                                        toastr.error("El anuncio tiene información relacionada", _app.constants.toastr.title.error);
                                    }
                                });
                            }
                        });
                    });
                },
                edit: function () {
                    $("#edit-form").validate({

                        rules: {
                            Title: { required: true },
                            Description: { required: true },
                            StartDate: { required: true },
                            EndDate: { required: true },

                        },
                        submitHandler: function (form, event) {
                            var formData = new FormData($(form).get(0));
                            var Id = $("#Id").val();
                            event.preventDefault();
                            $("#btnSave").addLoader();
                            $.ajax({
                                type: "POST",
                                url: `/admin/anuncios/editar/post/${Id}`.proto().parseURL(),
                                data: formData,
                                contentType: false,
                                processData: false,
                                success: function () {
                                    window.location.href = `/admin/anuncios`.proto().parseURL();
                                },
                                error: function () {
                                    toastr.error(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                                },
                                complete: function () {
                                    $("#btnSave").removeLoader();
                                }

                            });
                        }
                    });
                },
                picture: function () {
                    $("#Picture").on("change",
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
                },
                roles: {
                    init: function () {
                        $.ajax({
                            url: ("/roles_todos/get").proto().parseURL()
                        }).done(function (data) {

                            $(".select2-roles").select2({
                                placeholder: "Roles",
                                minimumInputLength: 0,
                                data: data.items
                            });

                        });
                    }
                },
                startEndDate: {
                    init: function () {
                        $("#StartDate").datepicker({
                            format: _app.constants.formats.datepicker
                        }).one("changeDate", function (e) {
                            $(this).valid();
                            $("#EndDate").datepicker("setStartDate", e.date);
                        });;
                        $("#EndDate").datepicker({
                            format: _app.constants.formats.datepicker
                        }).one("changeDate", function (e) {
                            $("#StartDate").datepicker("setEndDate", e.date);
                        });;
                    }
                }
            }
        }
    }
    return result;
})();

$(function () {
    announcement.datatable.load.get();
    announcement.datatable.load.add();
    announcement.datatable.load.edit();
    announcement.datatable.load.delete();
    announcement.datatable.load.picture();
    announcement.datatable.load.roles.init();
    announcement.datatable.load.startEndDate.init();
});

