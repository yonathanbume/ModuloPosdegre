var procedureDependency = (function () {
    var private = {
        datatable: {
            load: {
                get: function () {
                    private.datatable.objects["procedure-dependency-datatable-get"] = $("#procedure-dependency-datatable-get").mDatatable({
                        data: {
                            source: {
                                read: {
                                    method: "GET",
                                    url: `/dependencia/tramites/dependencias/${procedureId}/get`.proto().parseURL(),
                                },
                            },
                        },
                        columns: [
                            {
                                field: "",
                                title: "#",
                                width: 20,
                                template: function (row, index, datatable) {
                                    var currentPage = datatable.getCurrentPage();
                                    var pageSize = datatable.getPageSize();
                                    var template = (index + 1) + ((currentPage - 1) * pageSize);

                                    return template;
                                }
                            },
                            {
                                field: "dependency.name",
                                title: "Nombre de la Dependencia"
                            }
                        ]
                    });
                }
            },
            objects: {}
        }
    };

    return {
        datatable: {
            getObject: function (key) {
                return private.datatable.objects[key];
            }
        },
        init: function () {
            private.datatable.load.get();
        },
    };
})();

var userProcedureDerivation = (function () {
    var private = {
        ajax: {
            objects: {}
        },
        datatable: {
            load: {
                documentFiles: function () {
                    private.datatable.objects["user-procedure-derivation-modal-document-files-datatable-document-files"] = $("#user-procedure-derivation-modal-document-files-datatable-document-files").DataTable({
                        data: private.other.filteredUserProcedureDerivationFiles,
                        columns: [
                            {
                                data: "fileName",
                                title: "Nombre del Archivo"
                            },
                            {
                                data: "size",
                                title: "Tamaño",
                                render: function (data, type, row, meta) {
                                    var render = _app.modules.unit.bytesToSize(row.size);

                                    return render;
                                }
                            },
                            {
                                orderable: false,
                                render: function (data, type, row, meta) {
                                    var render = `
										<a href="${row.path}" class="btn btn-primary m-btn m-btn--icon m-btn--icon-only">
											<i class="la la-download"></i>
										</a>
									`;

                                    return render;
                                },
                                title: "Opciones"
                            }
                        ],
                        lengthChange: false,
                        lengthMenu: [3],
                        processing: false,
                        serverSide: false
                    });
                }
            },
            finish: {
                documentFiles: function () {
                    private.datatable.objects["user-procedure-finish-modal-document-files-datatable-document-files"] = $("#user-procedure-finish-modal-document-files-datatable-document-files").DataTable({
                        data: private.other.filteredUserProcedureFinishFiles,
                        columns: [
                            {
                                data: "fileName",
                                title: "Nombre del Archivo"
                            },
                            {
                                data: "size",
                                title: "Tamaño",
                                render: function (data, type, row, meta) {
                                    var render = _app.modules.unit.bytesToSize(row.size);

                                    return render;
                                }
                            },
                            {
                                orderable: false,
                                render: function (data, type, row, meta) {
                                    var pdfPdf = `/pdf/${row.path}`.proto().parseURL();
                                    var render = `
										<a href="${pdfPdf}" target="_blank" class="btn btn-primary m-btn m-btn--icon m-btn--icon-only">
											<i class="la la-download"></i>
										</a>
									`;

                                    return render;
                                },
                                title: "Opciones"
                            }
                        ],
                        lengthChange: false,
                        lengthMenu: [3],
                        processing: false,
                        serverSide: false
                    });
                }
            },
            objects: {}
        },
        modal: {
            objects: {}
        },
        other: {
            filteredUserProcedureDerivationFiles: null,
            userProcedureDerivationId: null,
            userProcedureDerivationFiles: null,
            filteredUserProcedureFinishFiles: null,
            userProcedureFinishId: null,
            userProcedureFinishFiles: null
        },
    };

    return {
        ajax: {
            getObject: function (key) {
                return private.ajax.objects[key];
            },
            load: {
                userProcedureDerivation: function () {
                    private.ajax.objects["user-procedure-derivation-ajax-user-procedure-derivation"] = $.ajax({
                        type: "GET",
                        url: `/tramites/usuarios/derivaciones/todos/${id}/get`.proto().parseURL()
                    })
                        .done(function (data, textStatus, jqXHR) {
                            var dataLength = data.derivationList.length;
                            var template = "";
                            template += `
									<div class="m-timeline-3">
										<div class="m-timeline-3__items">
								`;

                            if (dataLength > 0) {
                               
                                if (data.itsFinished) {
                                    for (var i = 0; i < data.derivationFinishedList.lengh; i++) {
                                        var dataRow = data.derivationFinishedList[i];

                                        template +=
                                            `<div class="m-timeline-3__item m-timeline-3__item--${["danger", "info", "primary", "success", "warning"].proto().random()}">
												<span class="m-timeline-3__item-time">${dataRow.parsedCreatedAt}</span>
												<div class="m-timeline-3__item-desc">
													<span class="m-timeline-3__item-text">Finalizado por la dependencia ${dataRow.dependency.name} - ${dataRow.user.fullName}</span><br>
													<span class="m-timeline-3__item-user-name">
														<button type="button" title="documentos"  class="btn btn-secondary m-btn btn-sm m-btn--icon" onclick="userProcedureDerivation.modal.finish.documentFiles(this, event, '${dataRow.proto().encode()}')"><i class="la la-files-o"></i></button>
														<button type="button" title="observaciones" class="btn btn-secondary m-btn btn-sm m-btn--icon" onclick="userProcedureDerivation.modal.finish.observation(this, event, '${dataRow.proto().encode()}')"><i class="la la-comment"></i></button>
													</span>
												</div>
										</div>`;
                                    }
                                }

                                for (var i = 0; i < data.derivationList.length; i++) {
                                    var dataRow = data.derivationList[i];

                                    template += `
											<div class="m-timeline-3__item m-timeline-3__item--${["danger", "info", "primary", "success", "warning"].proto().random()}">
												<span class="m-timeline-3__item-time">${dataRow.parsedCreatedAt}</span>
												<div class="m-timeline-3__item-desc">
													<span class="m-timeline-3__item-text">Derivado a : ${dataRow.dependency.name} por ${dataRow.user.name} (${dataRow.dependencyFrom.name})</span><br>
													<span class="m-timeline-3__item-user-name">
														<button type="button" title="documentos"  class="btn btn-secondary m-btn btn-sm m-btn--icon" onclick="userProcedureDerivation.modal.load.documentFiles(this, event, '${dataRow.proto().encode()}')"><i class="la la-files-o"></i></button>
														<button type="button" title="observaciones" class="btn btn-secondary m-btn btn-sm m-btn--icon" onclick="userProcedureDerivation.modal.load.observation(this, event, '${dataRow.proto().encode()}')"><i class="la la-comment"></i></button>
													</span>
												</div>
											</div>
									`;
                                }

                                template += `
										</div>
									</div>
								`;

                            }
                            else {


                                if (data.itsFinished) {
                                    for (var i = 0; i < data.derivationFinishedList.length; i++) {
                                        var dataRow = data.derivationFinishedList[i];

                                        template +=
                                            `<div class="m-timeline-3__item m-timeline-3__item--${["danger", "info", "primary", "success", "warning"].proto().random()}">
												<span class="m-timeline-3__item-time">${dataRow.parsedCreatedAt}</span>
												<div class="m-timeline-3__item-desc">
													<span class="m-timeline-3__item-text">Finalizado por la dependencia ${dataRow.dependency.name} - ${dataRow.user.fullName}</span><br>
													<span class="m-timeline-3__item-user-name">
														<button type="button" title="documentos"  class="btn btn-secondary m-btn btn-sm m-btn--icon" onclick="userProcedureDerivation.modal.finish.documentFiles(this, event, '${dataRow.proto().encode()}')"><i class="la la-files-o"></i></button>
														<button type="button" title="observaciones" class="btn btn-secondary m-btn btn-sm m-btn--icon" onclick="userProcedureDerivation.modal.finish.observation(this, event, '${dataRow.proto().encode()}')"><i class="la la-comment"></i></button>
													</span>
												</div>
										</div>`;
                                    }
                                } else {
                                    template += `
									<div class="alert alert-dismissible fade show   m-alert m-alert--outline m-alert--air" role="alert">
										<div class="m-alert__text">
											No hay datos disponibles
										</div>
									</div>
								`;
                                } 
                                template += `
										</div>
									</div>
								`;
                            }

                            document.getElementById("user-procedure-derivation-content").innerHTML = template;
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        });
                },
                userProcedureDerivationFile: function () {
                    private.ajax.objects["user-procedure-derivation-ajax-user-procedure-derivation"] = $.ajax({
                        type: "GET",
                        url: `/tramites/usuarios/archivos/${id}/get`.proto().parseURL()
                    })
                        .done(function (data, textStatus, jqXHR) {
                            private.other.userProcedureDerivationFiles = data;
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        });
                }
            },
            finish: {
                userProcedureFinish: function () {
                    private.ajax.objects["user-procedure-finish-ajax-user-procedure-finish"] = $.ajax({
                        type: "GET",
                        url: `/tramites/usuarios/finalizado/${id}/get`.proto().parseURL()
                    })
                        .done(function (data, textStatus, jqXHR) {
                            var dataLength = data.length;
                            var template = "";

                            if (dataLength > 0) {
                                template += `
									<div class="m-timeline-3">
										<div class="m-timeline-3__items">
								`;

                                for (var i = 0; i < data.length; i++) {
                                    var dataRow = data[i];

                                    template += `
											<div class="m-timeline-3__item m-timeline-3__item--${["danger", "info", "primary", "success", "warning"].proto().random()}">
												<span class="m-timeline-3__item-time">${dataRow.parsedCreatedAt}</span>
												<div class="m-timeline-3__item-desc">
													<span class="m-timeline-3__item-text">Finalizado por la dependencia ${dataRow.dependency.name} - ${dataRow.user.fullName}</span><br>
													<span class="m-timeline-3__item-user-name">
														<button type="button" title="documentos" class="btn btn-secondary m-btn  btn-sm m-btn--icon" onclick="userProcedureDerivation.modal.finish.documentFiles(this, event, '${dataRow.proto().encode()}')"><i class="la la-files-o"></i></button>
														<button type="button" title="observaciones" class="btn btn-secondary m-btn btn-sm m-btn--icon" onclick="userProcedureDerivation.modal.finish.observation(this, event, '${dataRow.proto().encode()}')"><i class="la la-comment"></i></button>
													</span>
												</div>
											</div>
									`;
                                }

                                template += `
										</div>
									</div>
								`;
                            } else {
                                template += `
									<div class="alert alert-dismissible fade show   m-alert m-alert--outline m-alert--air" role="alert">
										<div class="m-alert__text">
											No hay datos disponibles
										</div>
									</div>
								`;
                            }

                            document.getElementById("user-procedure-finish-content").innerHTML = template;
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        });
                },
                userProcedureFinishFile: function () {
                    private.ajax.objects["user-procedure-finish-ajax-user-procedure-finish"] = $.ajax({
                        type: "GET",
                        url: `/tramites/usuarios/get-observation-file/${id}`.proto().parseURL()
                    })
                        .done(function (data, textStatus, jqXHR) {
                            private.other.userProcedureFinishFiles = data;
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        });
                }
            }
        },
        datatable: {
            getObject: function (key) {
                return private.datatable.objects[key];
            },
            reset: function (object, data) {
                object.clear();
                object.rows.add(data);
                object.draw();
            }
        },
        init: function () {
            private.datatable.load.documentFiles();
            userProcedureDerivation.ajax.load.userProcedureDerivation();
            userProcedureDerivation.ajax.load.userProcedureDerivationFile();

            private.datatable.finish.documentFiles();
            //userProcedureDerivation.ajax.finish.userProcedureFinish();
            userProcedureDerivation.ajax.finish.userProcedureFinishFile();
        },
        modal: {
            getObject: function (key) {
                return private.modal.objects[key];
            },
            load: {
                documentFiles: function (element, event, data) {
                    data = data.proto().decode();
                    private.other.userProcedureDerivationId = data.id;

                    userProcedureDerivation.other.generateUserProcedureDerivationFiles();
                    userProcedureDerivation.datatable.reset(userProcedureDerivation.datatable.getObject("user-procedure-derivation-modal-document-files-datatable-document-files"), userProcedureDerivation.other.getFilteredUserProcedureDerivationFiles());

                    private.modal.objects["user-procedure-derivation-modal-document-files"] = $("#user-procedure-derivation-modal-document-files").modal("show");
                },
                observation: function (element, event, data) {
                    var observationTemplate = "";
                    data = data.proto().decode();

                    observationTemplate += `<p>${data.observation}</p>`;

                    document.getElementById("user-procedure-derivation-modal-observation-content").innerHTML = observationTemplate;
                    private.modal.objects["user-procedure-derivation-modal-observation"] = $("#user-procedure-derivation-modal-observation").modal("show");
                }
            },
            finish: {
                documentFiles: function (element, event, data) {
                    data = data.proto().decode();
                    private.other.userProcedureFinishId = data.id;

                    userProcedureDerivation.other.generateUserProcedureFinishFiles();
                    userProcedureDerivation.datatable.reset(userProcedureDerivation.datatable.getObject("user-procedure-finish-modal-document-files-datatable-document-files"), userProcedureDerivation.other.getFilteredUserProcedureFinishFiles());

                    private.modal.objects["user-procedure-finish-modal-document-files"] = $("#user-procedure-finish-modal-document-files").modal("show");
                },
                observation: function (element, event, data) {
                    var observationTemplate = "";
                    data = data.proto().decode();

                    observationTemplate += `<p>${data.observationStatus}</p>`;

                    document.getElementById("user-procedure-finish-modal-observation-content").innerHTML = observationTemplate;
                    private.modal.objects["user-procedure-finish-modal-observation"] = $("#user-procedure-finish-modal-observation").modal("show");
                }
            }
        },
        other: {
            getFilteredUserProcedureDerivationFiles: function () {
                return private.other.filteredUserProcedureDerivationFiles;
            },
            getUserProcedureDerivationId: function () {
                return private.other.userProcedureDerivationId;
            },
            getUserProcedureDerivationFiles: function () {
                return private.other.userProcedureDerivationFiles;
            },
            generateUserProcedureDerivationFiles: function () {
                private.other.filteredUserProcedureDerivationFiles = [];

                for (var i = 0; i < private.other.userProcedureDerivationFiles.length; i++) {
                    var userProcedureDerivationFile = private.other.userProcedureDerivationFiles[i];

                    if (userProcedureDerivationFile.userProcedureDerivationId == private.other.userProcedureDerivationId) {
                        private.other.filteredUserProcedureDerivationFiles.push(userProcedureDerivationFile);
                    }
                }
            },
            getFilteredUserProcedureFinishFiles: function () {
                return private.other.filteredUserProcedureFinishFiles;
            },
            getUserProcedureFinishId: function () {
                return private.other.userProcedureFinishId;
            },
            getUserProcedureFinishFiles: function () {
                return private.other.userProcedureFinishFiles;
            },
            generateUserProcedureFinishFiles: function () {
                private.other.filteredUserProcedureFinishFiles = [];

                for (var i = 0; i < private.other.userProcedureFinishFiles.length; i++) {
                    var userProcedureFinishFile = private.other.userProcedureFinishFiles[i];

                    if (userProcedureFinishFile.userProcedureId == private.other.userProcedureFinishId) {
                        private.other.filteredUserProcedureFinishFiles.push(userProcedureFinishFile);
                    }
                }
            }
        }
    };
})();

var InitApp = function () {

    var private = {
        ajax: {
            objects: {}
        },
        other: {
            dependencyNames: null,
            documentFiles: null,
            documentFiles2: null,
            filteredProcedureDependencies: null,
            procedureDependencies: null,
            procedureId: null
        },
        swal: {
            objects: {}
        },
        datatable: {
            load: {
                documentFiles: function () {
                    private.datatable.objects["active-user-procedure-modal-derive-form-datatable-document-files"] = $("#active-user-procedure-modal-derive-form-datatable-document-files").DataTable({
                        data: private.other.documentFiles,
                        columns: [
                            {
                                data: "fileName",
                                title: "Nombre del Archivo"
                            },
                            {
                                data: "size",
                                title: "Tamaño",
                                render: function (data, type, row, meta) {
                                    var render = _app.modules.unit.bytesToSize(data);

                                    return render;
                                }
                            },
                            {
                                orderable: false,
                                render: function (data, type, row, meta) {
                                    var render = `
										<button type="button" class="btn btn-danger m-btn btn-sm m-btn--icon" onclick="InitApp.swal.load.deleteDocumentFile(this, event, '${row.proto().encode()}')"><i class="la la-trash"></i></button>
									`;

                                    return render;
                                },
                                title: "Opciones"
                            }
                        ],
                        lengthChange: false,
                        lengthMenu: [3],
                        processing: false,
                        serverSide: false
                    });
                }
            },
            status: {
                documentFiles2: function () {
                    private.datatable.objects["status-user-procedure-modal-derive-form-datatable-document-files"] = $("#status-user-procedure-modal-derive-form-datatable-document-files").DataTable({
                        data: private.other.documentFiles2,
                        columns: [
                            {
                                data: "fileName",
                                title: "Nombre del Archivo"
                            },
                            {
                                data: "size",
                                title: "Tamaño",
                                render: function (data, type, row, meta) {
                                    var render = _app.modules.unit.bytesToSize(data);

                                    return render;
                                }
                            },
                            {
                                orderable: false,
                                render: function (data, type, row, meta) {
                                    var render = `
										<button type="button" class="btn btn-danger m-btn btn-sm m-btn--icon" onclick="InitApp.swal.load.deleteDocumentFile2(this, event, '${row.proto().encode()}')"><i class="la la-trash"></i></button>
									`;

                                    return render;
                                },
                                title: "Opciones"
                            }
                        ],
                        lengthChange: false,
                        lengthMenu: [3],
                        processing: false,
                        serverSide: false
                    });
                }
            },
            objects: {}
        },
    };
    var id = $("#Id").val();
    var firstView = 1;
    var datatable = {
        req: {
            object: null,
            options: {
                responsive: true,
                serverSide: true,
                pageLength: 10,
                ordering: true,
                orderMulti: false,
                //columnDefs: [
                //	{
                //		type: 'date-dd-MMM-yyyy',
                //		targets: [2],
                //	}
                //],
                order: [[0, 'desc']],
                language: {
                    "lengthMenu": "",
                    "infoEmpty": "",
                    "zeroRecords": "No existen registros",
                    "info": "",
                    "infoFiltered": "_MAX_ / _TOTAL_",
                    "paginate": {
                        "next": ">>",
                        "previous": "<<"
                    }
                },
                ajax: {
                    url: `/dependencia/tramites/usuarios/get-req/${id}`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        //data.Headline = $("#tbHeadline").val();
                        //data.Status = $("#selectStatus option:selected").val();
                    }
                },
                columns: [
                    {
                        title: "Nombre de Archivo",
                        data: "filename"
                    },
                    {
                        title: "Tamaño",
                        data: null,
                        render: function (result) {
                            var bytes = result.filesize;
                            return _app.modules.file.getFormattedFileSize(bytes);
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        render: function (data) {
                            var pdfPdf = `/pdf/${data.path}`.proto().parseURL();
                            return `<a href="${pdfPdf}" class="btn btn-primary btn-sm m-btn m-btn--icon" target="_blank"><i class="la la-download"></i></a>`
                        }
                    }
                ]
            },
            init: function () {
                datatable.req.object = $("#data-table").DataTable(datatable.req.options);

            },
            reload: function () {
                datatable.req.object.ajax.reload();
            }
        },
        fileObservation: {
            object: null,
            options: {
                responsive: true,
                serverSide: true,
                pageLength: 10,
                ordering: true,
                orderMulti: false,
                //columnDefs: [
                //	{
                //		type: 'date-dd-MMM-yyyy',
                //		targets: [2],
                //	}
                //],
                order: [[0, 'desc']],
                language: {
                    "lengthMenu": "",
                    "infoEmpty": "",
                    "zeroRecords": "No existen registros",
                    "info": "",
                    "infoFiltered": "_MAX_ / _TOTAL_",
                    "paginate": {
                        "next": ">>",
                        "previous": "<<"
                    }
                },
                ajax: {
                    url: `/dependencia/tramites/usuarios/get-observation-file/${id}`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        //data.Headline = $("#tbHeadline").val();
                        //data.Status = $("#selectStatus option:selected").val();
                    }
                },
                columns: [
                    {
                        title: "Nombre de Archivo",
                        data: "filename"
                    },
                    {
                        title: "Tamaño",
                        data: null,
                        render: function (result) {
                            var bytes = result.filesize;
                            return _app.modules.file.getFormattedFileSize(bytes);
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        render: function (data) {
                            var pdfPdf = `/pdf/${data.path}`.proto().parseURL();
                            return `<a href="${pdfPdf}" class="btn btn-primary btn-sm m-btn m-btn--icon" target="_blank"><i class="la la-download"></i></a>`
                        }
                    }
                ]
            },
            init: function () {
                datatable.req.object = $("#data-table").DataTable(datatable.req.options);

            },
            reload: function () {
                datatable.req.object.ajax.reload();
            }
        },
        init: function () {
            datatable.req.init();
            datatable.fileObservation.init();
        }
    };

    var form = {
        derive: {
            show: function () {
                $("#active-user-procedure-modal-derive").modal("show");
                $("#active-user-procedure-modal-derive").one("hidden.bs.modal");
            },
            submit: function (element, event) {
                var formElements = element.elements;
                var data = new FormData();
                var documentFiles = InitApp.other.getDocumentFiles();

                $("#active-user-procedure-modal-derive-form").find(".btn-submit").addLoader();
                $("#active-user-procedure-modal-derive-form").attr("disabled", true);

                data.append("DependencyId", formElements["UserProcedureDerivationViewModel.DependencyId"].value);
                data.append("UserProcedureId", id);
                data.append("Observation", formElements["UserProcedureDerivationViewModel.Observation"].value);
                data.append("UserDependencyId", formElements["UserProcedureDerivationViewModel.UserDependencyId"].value);
                data.append("ActivityProcedureId", formElements["UserProcedureDerivationViewModel.ActivityProcedureId"].value);
                data.proto().appendFiles(documentFiles, "DocumentFiles", "file");

                private.ajax.objects["active-user-procedure-ajax-derive"] = $.ajax({
                    contentType: false,
                    data: data,
                    processData: false,
                    type: element.method,
                    url: element.action,
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();

                        if (documentFiles.length > 0) {
                            $(formElements["DocumentFiles"]).parent().addProgressBar();

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
                        $("#active-user-procedure-modal-derive-form").find(".btn-submit").removeLoader();
                        $("#active-user-procedure-modal-derive-form select").attr("disabled", false);
                        $(formElements["DocumentFiles"]).parent().removeProgressBar();
                    })
                    .done(function (data, textStatus, jqXHR) {
                        $("#active-user-procedure-modal-derive").modal("hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        window.location.href = '/dependencia/tramites/usuarios';
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        var responseText = jqXHR.responseText;

                        if (responseText != "" && jqXHR.status == 400) {
                            toastr.error(responseText, _app.constants.toastr.title.error);
                        } else {
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        }
                    });
            },
        },
        status:
        {
            finish: {
                show: function () {
                    $("#statusModal").modal("show");
                    $(".finish").show();
                    $("#statusModal").one("hidden.bs.modal");
                },
                submit: function (element, event) {
                    var formElements = element.elements;
                    var data = new FormData();
                    var documentFiles = InitApp.other.getDocumentFiles2();

                    $("#status-user-procedure-modal-form").find(".btn-submit").addLoader();
                    $("#status-user-procedure-modal-form").attr("disabled", true);

                    data.append("UserProcedureId", id);
                    data.append("Status", status);
                    data.append("Observation", formElements["Observation"].value);
                    if (status == 9)
                        data.proto().appendFiles(documentFiles, "DocumentFiles", "file");

                    private.ajax.objects["status-user-procedure-ajax"] = $.ajax({
                        contentType: false,
                        data: data,
                        processData: false,
                        type: element.method,
                        url: element.action,
                        xhr: function () {
                            var xhr = new window.XMLHttpRequest();
                            if (status == 9) {
                                if (documentFiles.length > 0) {
                                    $(formElements["DocumentFiles"]).parent().addProgressBar();

                                    xhr.upload.onprogress = function (event) {
                                        if (event.lengthComputable) {
                                            var completedPercentage = (event.loaded / event.total) * 100;

                                            $(element).find(".progress-bar").width(`${completedPercentage}%`);
                                        }
                                    }
                                }
                            }


                            return xhr;
                        }
                    })
                        .always(function (data, textStatus, jqXHR) {
                            $("#status-user-procedure-modal-form").find(".btn-submit").removeLoader();
                            $("#status-user-procedure-modal-form select").attr("disabled", false);
                            $(formElements["DocumentFiles"]).parent().removeProgressBar();
                        })
                        .done(function (data, textStatus, jqXHR) {
                            $("#statusModal").modal("hide");
                            if (status == 9)
                                $("#btnSend").hide();
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            location.reload();
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            var responseText = jqXHR.responseText;

                            if (responseText != "" && jqXHR.status == 400) {
                                toastr.error(responseText, _app.constants.toastr.title.error);
                            } else {
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            }
                        });
                },
            },
            observation: {
                show: function () {
                    $("#statusModal").modal("show");
                    $(".finish").hide();
                    $("#statusModal").one("hidden.bs.modal");
                },
                submit: function (element, event) {
                    var formElements = element.elements;
                    var data = new FormData();

                    $("#status-user-procedure-modal-form").find(".btn-submit").addLoader();
                    $("#status-user-procedure-modal-form").attr("disabled", true);

                    data.append("UserProcedureId", id);
                    data.append("Status", status);

                    private.ajax.objects["status-user-procedure-ajax"] = $.ajax({
                        contentType: false,
                        data: data,
                        processData: false,
                        type: element.method,
                        url: element.action,
                    })
                        .always(function (data, textStatus, jqXHR) {
                            $("#status-user-procedure-modal-form").find(".btn-submit").removeLoader();
                            $("#status-user-procedure-modal-form select").attr("disabled", false);
                            $(formElements["DocumentFiles"]).parent().removeProgressBar();
                        })
                        .done(function (data, textStatus, jqXHR) {
                            $("#statusModal").modal("hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            location.reload();
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            var responseText = jqXHR.responseText;

                            if (responseText != "" && jqXHR.status == 400) {
                                toastr.error(responseText, _app.constants.toastr.title.error);
                            } else {
                                toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                            }
                        });
                },
            }
        }
    }
    var status = null;
    var select2 = {
        init: function () {
            this.dependency.init();
            this.status.init();
            this.status.change();
            this.activity.init();
        },
        dependency: {
            init: function () {
                $(".dependency-select2").prop("disabled", true);
                $.ajax({
                    url: "/dependencias/get".proto().parseURL()
                }).done(function (result) {
                    $(".dependency-select2").select2({
                        data: result.items
                    });
                    $(".dependency-select2").on("change", function () {
                        select2.userDependency.init(this.value);
                    }).trigger("change");
                    $(".dependency-select2").prop("disabled", false);
                });
            }
        },
        userDependency: {
            init: function (id) {
                $(".dependency-user-select2").attr("disabled", true);
                $.ajax({
                    url: `/usuarios/get/${id}`.proto().parseURL()
                }).done(function (result) {

                    $(".dependency-user-select2").select2({
                        data: result.items
                    });
                    $(".dependency-user-select2").on("change", function () {

                    }).trigger("change");
                    $(".dependency-user-select2").attr("disabled", false);
                });
            }
        },
        status: {
            init: function () {
                $.ajax({
                    url: "/status/get".proto().parseURL()
                }).done(function (result) {
                    $(".status-select2").select2({
                        data: result.items
                    });

                    //if (firstView != 1) {
                    $(".status-select2").on("change", function () {

                        if (this.value == 9) {
                            form.status.finish.show();
                            status = 9;
                        }

                        if (this.value == 7) {
                            form.status.observation.show();
                            status = 7;
                        }
                    });
                    //}
                });

            },
            change: function () {



            }
        },
        activity: {
            init: function () {
                $(".activity-select2").prop("disabled", true);
                $.ajax({
                    url: "/tareas/get".proto().parseURL()
                }).done(function (result) {
                    $(".activity-select2").select2({
                        data: result.items
                    });
                    $(".activity-select2").on("change", function () {
                    }).trigger("change");
                    $(".activity-select2").prop("disabled", false);
                });
            }
        },
    };

    var validate = {
        init: function () {
            createForm = $("#active-user-procedure-modal-derive-form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.derive.submit(formElement);
                }
            });
            createForm = $("#status-user-procedure-modal-form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.status.finish.submit(formElement);
                }
            });
        }
    };

    var events = {
        init: function () {
            $("#btnSend").on("click",
                function () {
                    form.derive.show();
                });

            $("#Status").on("click",
                function () {
                    firstView = 2;
                });
        }
    };

    return {
        init: function () {
            private.datatable.load.documentFiles();
            private.datatable.status.documentFiles2();
            datatable.req.init();
            events.init();
            validate.init();
            select2.init();
        },
        file: {
            load: {
                documentFiles: function (element, event) {
                    var target = event.target || window.event.srcElement;
                    var targetFiles = target.files;
                    var targetFilesLength = targetFiles.length;
                    private.other.documentFiles = [];

                    if (FileReader && targetFiles && targetFilesLength > 0) {
                        for (var i = 0; i < targetFilesLength; i++) {
                            var targetFile = targetFiles[i];

                            private.other.documentFiles.push({
                                id: i,
                                file: targetFile,
                                fileName: targetFile.name,
                                type: targetFile.type,
                                size: targetFile.size
                            })
                        }

                        element.value = null;
                        element.nextElementSibling.innerHTML = `${targetFilesLength} archivo${targetFilesLength != 1 ? "s" : ""} seleccionado${targetFilesLength != 1 ? "s" : ""}`;
                    }

                    InitApp.datatable.reset(InitApp.datatable.getObject("active-user-procedure-modal-derive-form-datatable-document-files"), InitApp.other.getDocumentFiles());
                    event.stopImmediatePropagation();
                }
            },
            status: {
                documentFiles: function (element, event) {
                    var target = event.target || window.event.srcElement;
                    var targetFiles = target.files;
                    var targetFilesLength = targetFiles.length;
                    private.other.documentFiles2 = [];

                    if (FileReader && targetFiles && targetFilesLength > 0) {
                        for (var i = 0; i < targetFilesLength; i++) {
                            var targetFile = targetFiles[i];

                            private.other.documentFiles2.push({
                                id: i,
                                file: targetFile,
                                fileName: targetFile.name,
                                type: targetFile.type,
                                size: targetFile.size
                            })
                        }

                        element.value = null;
                        element.nextElementSibling.innerHTML = `${targetFilesLength} archivo${targetFilesLength != 1 ? "s" : ""} seleccionado${targetFilesLength != 1 ? "s" : ""}`;
                    }

                    InitApp.datatable.reset(InitApp.datatable.getObject("status-user-procedure-modal-derive-form-datatable-document-files"), InitApp.other.getDocumentFiles2());
                    event.stopImmediatePropagation();
                }
            }
        },
        other: {
            getDocumentFiles: function () {
                return private.other.documentFiles;
            },
            getDocumentFiles2: function () {
                return private.other.documentFiles2;
            },
        },
        datatable: {
            getObject: function (key) {
                return private.datatable.objects[key];
            },
            reset: function (object, data) {
                object.clear();
                object.rows.add(data);
                object.draw();
            }
        },
        swal: {
            getObject: function (key) {
                return private.swal.objects[key];
            },
            load: {
                deleteDocumentFile: function (element, event, data) {
                    data = data.proto().decode();

                    private.swal.objects["active-user-procedure-modal-derive-form-swal-delete-document-file"] = swal({
                        preConfirm: function () {
                            return new Promise(function (resolve, reject) {
                                var documentFiles = InitApp.other.getDocumentFiles();

                                for (var i = 0; i < documentFiles.length; i++) {
                                    if (documentFiles[i].id == data.id) {
                                        private.other.documentFiles.splice(i, 1);
                                        break;
                                    }
                                };

                                var activeUserProcedureModalDeriveForm = document.getElementById("active-user-procedure-modal-derive-form");
                                var documentFilesLength = documentFiles.length;

                                if (documentFilesLength <= 0) {
                                    activeUserProcedureModalDeriveForm["DocumentFiles"].nextElementSibling.innerHTML = "Seleccione archivos";
                                } else {
                                    activeUserProcedureModalDeriveForm["DocumentFiles"].nextElementSibling.innerHTML = `${documentFilesLength} archivo${documentFilesLength != 1 ? "s" : ""} seleccionado${documentFilesLength != 1 ? "s" : ""}`;
                                }

                                InitApp.datatable.reset(InitApp.datatable.getObject("active-user-procedure-modal-derive-form-datatable-document-files"), documentFiles);
                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                swal.close();
                            });
                        },
                        title: `¿Desea eliminar el archivo "${data.fileName}?"`,
                        type: "warning",
                        showCancelButton: true,
                        showLoaderOnConfirm: true
                    });
                },
                deleteDocumentFile2: function (element, event, data) {
                    data = data.proto().decode();

                    private.swal.objects["status-user-procedure-modal-derive-form-swal-delete-document-file"] = swal({
                        preConfirm: function () {
                            return new Promise(function (resolve, reject) {
                                var documentFiles = InitApp.other.getDocumentFiles2();

                                for (var i = 0; i < documentFiles.length; i++) {
                                    if (documentFiles[i].id == data.id) {
                                        private.other.documentFiles2.splice(i, 1);
                                        break;
                                    }
                                };

                                var activeUserProcedureModalDeriveForm = document.getElementById("status-user-procedure-modal-form");
                                var documentFilesLength = documentFiles.length;

                                if (documentFilesLength <= 0) {
                                    activeUserProcedureModalDeriveForm["DocumentFiles"].nextElementSibling.innerHTML = "Seleccione archivos";
                                } else {
                                    activeUserProcedureModalDeriveForm["DocumentFiles"].nextElementSibling.innerHTML = `${documentFilesLength} archivo${documentFilesLength != 1 ? "s" : ""} seleccionado${documentFilesLength != 1 ? "s" : ""}`;
                                }

                                InitApp.datatable.reset(InitApp.datatable.getObject("status-user-procedure-modal-derive-form-datatable-document-files"), documentFiles);
                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                swal.close();
                            });
                        },
                        title: `¿Desea eliminar el archivo "${data.fileName}?"`,
                        type: "warning",
                        showCancelButton: true,
                        showLoaderOnConfirm: true
                    });
                },
                derive: function (element, event) {
                    var formElements = element.elements;
                    var dependencyId = formElements["DependencyId"];

                    private.swal.objects["active-user-procedure-swal-derive"] = swal({
                        preConfirm: function () {
                            return new Promise(function (resolve, reject) {
                                InitApp.form.derive.submit(element, event);
                            });
                        },
                        title: `¿Está seguro de derivar el trámite "${formElements["UserProcedureProcedureName"].value}" a la dependencia "${dependencyId.options[dependencyId.selectedIndex].text}"?`,
                        type: "warning",
                        showCancelButton: true,
                        showLoaderOnConfirm: true
                    });
                }
            },
        }
    };
}();

$(function () {
    procedureDependency.init();
    userProcedureDerivation.init();
    InitApp.init();

    $(".nav-link.m-tabs__link").on("shown.bs.tab", function (event) {
        procedureDependency.datatable.getObject("procedure-dependency-datatable-get").adjustCellsWidth();
    });
});