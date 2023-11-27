var dependency = (function () {
    var private = {
        ajax: {
            objects: {}
        },
        datatable: {
            load: {
                get: function () {
                    $("#search").doneTyping(function () {
                        private.datatable.objects["dependency-datatable-get"].draw();
                    });

                    private.datatable.objects["dependency-datatable-get"] = $("#dependency-datatable-get").DataTable({
                        ajax: {
                            data: function (data, settings) {
                                data["searchValue"] = $("#search").val();
                                data["userId"] = $("#dependency-header-form .user-select2").val();
                            },
                            url: "/admin/dependencias/datatable/get".proto().parseURL(),
                            type: "GET"
                        },
                        columns: [
                            {
                                data: "name",
                                name: "name",
                                title: "Nombre de la Dependencia"
                            },
                            {
                                data: "acronym",
                                name: "acronym",
                                title: "Acrónimo"
                            },
                            {
                                data: "user.fullName",
                                name: "user.fullName",
                                render: function (data, type, row, meta) {
                                    var render = "";
                                    if (row.user != null) {
                                        render += row.user.fullName;
                                    } else {
                                        render += "---";
                                    }

                                    return render;
                                },
                                title: "Director"
                            },
                            {
                                orderable: false,
                                render: function (data, type, row, meta) {
                                    var render = `
                                        <button class="btn btn-primary btn-sm m-btn m-btn--icon" onclick="dependency.modal.load.detail(this, event, '${row.proto().encode()}')"><span><i class="la la-eye"></i><span> Detalle </span></span></button> 
                                        <button class="btn btn-primary btn-sm m-btn m-btn--icon" onclick="dependency.modal.load.update(this, event, '${row.proto().encode()}')\"><span><i class=\"la la-edit\"></i><span> Editar </span></span></button> 
                                        <button class="btn btn-danger btn-sm m-btn m-btn--icon m-btn--icon-only" onclick="dependency.swal.load.delete(this, event, '${"/admin/dependencias/eliminar/post".proto().parseURL()}', '${row.proto().encode()}')\"><i class=\"la la-trash\"></i></button> 
                                    `;

                                    return render;
                                },
                                title: "Opciones"
                            }
                        ]
                    });
                }
            },
            objects: {}
        },
        modal: {
            objects: {}
        },
        select2: {
            load: {
                dependencyUser: function () {
                    $("#dependency-modal-create-form .dependency-user-select2, #dependency-modal-update-form .dependency-user-select2").each(function (index, element) {
                        var jQueryElement = $(element);
                        var jQueryElementModalParent = jQueryElement.parents(".modal");
                        var dropdownParent = jQueryElementModalParent.length > 0 ? jQueryElementModalParent : null;

                        private.select2.objects[`dependency-user-select2-${index}`] = jQueryElement.select2({
                            ajax: {
                                delay: 1000,
                                url: "/admin/usuarios/dependencias/select2/get".proto().parseURL()
                            },
                            allowClear: true,
                            dropdownParent: dropdownParent
                        });
                    });
                },
                user: function () {
                    $("#dependency-header-form .user-select2").each(function (index, element) {
                        var jQueryElement = $(element);
                        var jQueryElementModalParent = jQueryElement.parents(".modal");
                        var dropdownParent = jQueryElementModalParent.length > 0 ? jQueryElementModalParent : null;

                        private.select2.objects[`user-select2-${index}`] = jQueryElement.select2({
                            ajax: {
                                delay: 1000,
                                url: "/admin/usuarios/select2/get".proto().parseURL()
                            },
                            allowClear: true,
                            dropdownParent: dropdownParent
                        });
                    });

                    $("#dependency-header-form .user-select2").on("change select2:select", function (e) {
                        private.datatable.objects["dependency-datatable-get"].draw();
                    });
                }
            },
            objects: {}
        },
        swal: {
            objects: {}
        },
        validate: {
            load: {
                create: function () {
                    private.validate.objects["dependency-modal-create-form"] = $("#dependency-modal-create-form").validate({
                        submitHandler: function (form, event) {
                            event.preventDefault();
                            dependency.ajax.load.create(form, event);
                        }
                    });
                },
                update: function () {
                    private.validate.objects["dependency-modal-update-form"] = $("#dependency-modal-update-form").validate({
                        submitHandler: function (form, event) {
                            event.preventDefault();
                            dependency.ajax.load.update(form, event);
                        }
                    });
                }
            },
            objects: {}
        }
    };

    return {
        ajax: {
            load: {
                create: function (element, event) {
                    var formElements = element.elements;
                    var data = new FormData();

                    data.append("UserId", formElements["UserId"].value);
                    data.append("Acronym", formElements["Acronym"].value);
                    data.append("Name", formElements["Name"].value);
                    data.proto().appendFiles(formElements["SignatureFile"].files, "SignatureFile");
                    mApp.block(".modal-content");

                    private.ajax.objects["dependency-ajax-create"] = $.ajax({
                        contentType: false,
                        data: data,
                        processData: false,
                        type: element.method,
                        url: element.action,
                        xhr: function () {
                            var xhr = new window.XMLHttpRequest();

                            if (formElements["SignatureFile"].files.length > 0) {
                                $(formElements["SignatureFile"]).parent().addProgressBar();

                                xhr.upload.onprogress = function (event) {
                                    if (event.lengthComputable) {
                                        var completedPercentage = (event.loaded / event.total) * 100;

                                        $(element).find(".progress-bar").width(`${completedPercentage}%`);
                                    }
                                }
                            }

                            return xhr;
                        }
                    })
                        .always(function (data, textStatus, jqXHR) {
                            mApp.unblock(".modal-content");
                            $(formElements["SignatureFile"]).parent().removeProgressBar();
                        })
                        .done(function (data, textStatus, jqXHR) {
                            _app.modules.form.reset({
                                element: element
                            });

                            dependency.datatable.getObject("dependency-datatable-get").draw();
                            dependency.modal.getObject("dependency-modal-create").modal("hide");
                            toastr.success(_app.constants.toastr.message.success.create, _app.constants.toastr.title.success);
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            var responseText = jqXHR.responseText;

                            if (responseText != "" && jqXHR.status == 400) {
                                toastr.error(responseText, _app.constants.toastr.title.error);
                            } else {
                                toastr.error(_app.constants.toastr.message.error.create, _app.constants.toastr.title.error);
                            }
                        });
                },
                delete: function (data, url) {
                    $.ajax({
                        data: {
                            Id: data.id
                        },
                        type: "POST",
                        url: url
                    })
                        .always(function (data, textStatus, jqXHR) {
                            swal.close();
                        })
                        .done(function (data, textStatus, jqXHR) {
                            dependency.datatable.getObject("dependency-datatable-get").draw();
                            toastr.success(_app.constants.toastr.message.success.delete, _app.constants.toastr.title.success);
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.delete, _app.constants.toastr.title.error);
                        });
                },
                update: function (element, event) {
                    var formElements = element.elements;
                    var data = new FormData();

                    data.append("Id", formElements["Id"].value);
                    data.append("UserId", formElements["UserId"].value);
                    data.append("Acronym", formElements["Acronym"].value);
                    data.append("Name", formElements["Name"].value);
                    data.proto().appendFiles(formElements["SignatureFile"].files, "SignatureFile");
                    mApp.block(".modal-content");

                    private.ajax.objects["dependency-ajax-update"] = $.ajax({
                        contentType: false,
                        data: data,
                        processData: false,
                        type: element.method,
                        url: element.action,
                        xhr: function () {
                            var xhr = new window.XMLHttpRequest();

                            if (formElements["SignatureFile"].files.length > 0) {
                                $(formElements["SignatureFile"]).parent().addProgressBar();

                                xhr.upload.onprogress = function (event) {
                                    if (event.lengthComputable) {
                                        var completedPercentage = (event.loaded / event.total) * 100;

                                        $(element).find(".progress-bar").width(`${completedPercentage}%`);
                                    }
                                }
                            }

                            return xhr;
                        }
                    })
                        .always(function (data, textStatus, jqXHR) {
                            mApp.unblock(".modal-content");
                            $(formElements["SignatureFile"]).parent().removeProgressBar();
                        })
                        .done(function (data, textStatus, jqXHR) {
                            dependency.datatable.getObject("dependency-datatable-get").draw();
                            dependency.modal.getObject("dependency-modal-update").modal("hide");
                            toastr.success(_app.constants.toastr.message.success.update, _app.constants.toastr.title.success);
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            var responseText = jqXHR.responseText;

                            if (responseText != "" && jqXHR.status == 400) {
                                toastr.error(responseText, _app.constants.toastr.title.error);
                            } else {
                                toastr.error(_app.constants.toastr.message.error.update, _app.constants.toastr.title.error);
                            }
                        });
                }
            },
            getObject: function (key) {
                return private.ajax.objects[key];
            }
        },
        datatable: {
            getObject: function (key) {
                return private.datatable.objects[key];
            }
        },
        file: {
            load: {
                signatureFileCreate: function (element, event) {
                    var target = event.target || window.event.srcElement;
                    var targetFiles = target.files;
                    var targetFilesLength = targetFiles.length;
                    var fileReader = new FileReader();

                    if (FileReader && targetFiles && targetFilesLength > 0) {
                        fileReader.onload = function (event) {
                            var signatureHtml = "";
                            signatureHtml += `
                                <div class="m--space-20"></div>
                                <img src="${event.target.result}" alt="Signature" height="300" width="300">
                            `;

                            document.getElementById("dependency-modal-create-signature-content").innerHTML = signatureHtml;
                        }
                        fileReader.readAsDataURL(targetFiles[0]);
                    }
                },
                signatureFileUpdate: function (element, event) {
                    var target = event.target || window.event.srcElement;
                    var targetFiles = target.files;
                    var targetFilesLength = targetFiles.length;
                    var fileReader = new FileReader();

                    if (FileReader && targetFiles && targetFilesLength > 0) {
                        fileReader.onload = function (event) {
                            var signatureHtml = "";
                            signatureHtml += `
                                <div class="m--space-20"></div>
                                <img src="${event.target.result}" alt="Signature" height="300" width="300">
                            `;

                            document.getElementById("dependency-modal-update-signature-content").innerHTML = signatureHtml;
                        }
                        fileReader.readAsDataURL(targetFiles[0]);
                    }
                }
            }
        },
        init: function () {
            private.datatable.load.get();
            private.select2.load.dependencyUser();
            private.select2.load.user();
            private.validate.load.create();
            private.validate.load.update();
        },
        modal: {
            load: {
                create: function (element, event) {
                    var dependencyModalCreateForm = document.getElementById("dependency-modal-create-form");
                    dependencyModalCreateForm["SignatureFile"].value = null;
                    dependencyModalCreateForm["SignatureFile"].nextElementSibling.innerHTML = "Seleccione archivos";
                    document.getElementById("dependency-modal-create-signature-content").innerHTML = "";
                    private.modal.objects["dependency-modal-create"] = $("#dependency-modal-create").modal("show");
                },
                detail: function (element, event, data) {
                    var dependencyModalDetailForm = document.getElementById("dependency-modal-detail-form");
                    var signatureHtml = "";
                    data = data.proto().decode();

                    _app.modules.form.reset({
                        element: dependencyModalDetailForm
                    });

                    _app.modules.form.fill({
                        element: dependencyModalDetailForm,
                        data: {
                            Id: data.id,
                            UserName: data.user != null ? data.user.name : "---",
                            Acronym: data.acronym,
                            Name: data.name
                        }
                    });

                    if (data.signature != null) {
                        signatureHtml += `<img src="${data.signature}" alt="Signature" height="300" width="300">`;
                    } else {
                        signatureHtml += "<label>No hay una imágen disponible</label>";
                    }

                    document.getElementById("dependency-modal-detail-signature-content").innerHTML = signatureHtml;
                    private.modal.objects["dependency-modal-detail"] = $("#dependency-modal-detail").modal("show");
                },
                update: function (element, event, data) {
                    var dependencyModalUpdateForm = document.getElementById("dependency-modal-update-form");
                    var signatureHtml = "";
                    data = data.proto().decode();

                    _app.modules.form.reset({
                        element: dependencyModalUpdateForm
                    });

                    _app.modules.form.fill({
                        element: dependencyModalUpdateForm,
                        data: {
                            Id: data.id,
                            UserId: data.userId,
                            Acronym: data.acronym,
                            Name: data.name
                        }
                    });

                    $("#dependency-modal-update .dependency-user-select2").val(null).trigger("change");

                    if (data.user != null) {
                        $("#dependency-modal-update .dependency-user-select2").append(new Option(data.user.fullName, data.userId, true, true)).trigger("change");
                    }

                    if (data.signature != null) {
                        signatureHtml += `
                            <div class="m--space-20"></div>
                            <img src="${data.signature}" alt="Signature" height="300" width="300">
                        `;
                    }

                    document.getElementById("dependency-modal-update-signature-content").innerHTML = signatureHtml;
                    private.modal.objects["dependency-modal-update"] = $("#dependency-modal-update").modal("show");
                }
            },
            getObject: function (key) {
                return private.modal.objects[key];
            }
        },
        select2: {
            getObject: function (key) {
                return private.select2.objects[key];
            }
        },
        swal: {
            load: {
                delete: function (element, event, url, data) {
                    data = data.proto().decode();

                    private.swal.objects["dependency-swal-delete"] = swal({
                        preConfirm: function () {
                            return new Promise(function (resolve, reject) {
                                dependency.ajax.load.delete(data, url);
                            });
                        },
                        title: _app.constants.swal.title.delete,
                        type: "warning",
                        showCancelButton: true,
                        showLoaderOnConfirm: true
                    });
                }
            },
            getObject: function (key) {
                return private.swal.objects[key];
            }
        },
        validate: {
            getObject: function (key) {
                return private.validate.objects[key];
            }
        }
    };
}());

$(function () {
    dependency.init();
});
