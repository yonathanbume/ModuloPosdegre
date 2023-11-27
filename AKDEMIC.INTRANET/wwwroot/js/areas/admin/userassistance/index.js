var UserAssistance = function () {
    var formValidate;
    var userDatatable = null;
    //var users = [];
    var X = XLSX;
    var XW = {
        /* worker message */
        msg: "xlsx",
        /* worker scripts */
        worker: "./xlsxworker.js"
    };

    var users = [];
    var global_wb;

    var datatable = {
        destroy: function() {
            userDatatable.destroy();
            userDatatable = null;
            $("#m-users-datatable").hide();
        },
        init: function(paramUsers) {
            if (userDatatable !== null) {
                this.destroy();
            }
            userDatatable = $("#m-users-datatable").DataTable({
                serverSide: false,
                paging: false,
                ordering: false,
                info: false,
                filter: true,
                data: paramUsers,
                columns: [
                    {
                        title: "Usuario",
                        data: "UserName"
                    },
                    {
                        title: "Rol",
                        data: "Role"
                    },
                    {
                        title: "Fecha",
                        data: "Date",
                        render: function (data, type, row) {
                            return new Date(data).toLocaleDateString();
                        }
                    },
                    {
                        title: "Entrada (Turno Mañana)",
                        data: "FirstEntryTime",
                        render: function(data, type, row) {
                            return new Date(data).toLocaleString();
                        }
                    },
                    {
                        title: "Salida (Turno Mañana)",
                        data: "FirstExitTime",
                        render: function (data, type, row) {
                            return new Date(data).toLocaleString();
                        }
                    },
                    {
                        title: "Marcado de Entrada (Turno Mañana)",
                        data: "FirstEntry",
                        render: function (data, type, row) {
                            return data ? new Date(data).toLocaleString() : "";
                        }
                    },
                    {
                        title: "Marcado de Salida (Turno Mañana)",
                        data: "FirstExit",
                        render: function (data, type, row) {
                            return data ? new Date(data).toLocaleString() : "";
                        }
                    },
                    {
                        title: "Entrada (Turno Tarde)",
                        data: "SecondEntryTime",
                        render: function (data, type, row) {
                            return new Date(data).toLocaleString();
                        }
                    },
                    {
                        title: "Salida (Turno Tarde)",
                        data: "SecondExitTime",
                        render: function (data, type, row) {
                            return new Date(data).toLocaleString();
                        }
                    },
                    {
                        title: "Marcado de Entrada (Turno Tarde)",
                        data: "SecondEntry",
                        render: function (data, type, row) {
                            return data ? new Date(data).toLocaleString() : "";
                        }
                    },
                    {
                        title: "Marcado de Salida (Turno Tarde)",
                        data: "SecondExit",
                        render: function (data, type, row) {
                            return data ? new Date(data).toLocaleString() : "";
                        }
                    }
                ]
            });
            $("#m-users-datatable").show();
        }
    }

    var processWb = (function () {
        var OUT = document.getElementById("m-users-datatable");

        var toJson = function(workbook) {
            var result = {};
            workbook.SheetNames.forEach(function (sheetName) {
                var roa = X.utils.sheet_to_json(workbook.Sheets[sheetName], { header: 1 });
                if (roa.length) result[sheetName] = roa;
            });
            return result;
        };

        var normalizeJson = function(rawJson) {
            var list = [];
            for (var key in rawJson) {
                if (rawJson.hasOwnProperty(key)) {
                    for (var row in rawJson[key]) {
                        if (row !== "0" && rawJson[key][row][0] !== "") {
                            var user = {
                                UserName: rawJson[key][row][0],
                                Role: rawJson[key][row][2],
                                Date: rawJson[key][row][3],
                                FirstEntryTime: rawJson[key][row][4],
                                FirstExitTime: rawJson[key][row][5],
                                FirstEntry: rawJson[key][row][6],
                                FirstExit: rawJson[key][row][7],
                                SecondEntryTime: rawJson[key][row][8],
                                SecondExitTime: rawJson[key][row][9],
                                SecondEntry: rawJson[key][row][10],
                                SecondExit: rawJson[key][row][11]
                            };
                            list.push(user);
                        }
                    }
                    break;
                }
            }
            return list;
        };
        
        return function (wb) {
            global_wb = wb;
            var rawOutput = "";
            rawOutput = toJson(wb);
            users = normalizeJson(rawOutput);
            if (users.length > 0) {
                form.change.toSubmit();
                datatable.init(users);
            } else {
                toastr.error("El archivo se encuentra vacío.", _app.constants.toastr.title.error);
            }
            mApp.unblock(".m-portlet");
        };
    })();

    var doFile = (function () {
        var rABS = typeof FileReader !== "undefined" && (FileReader.prototype || {}).readAsBinaryString;
        domrabs = true;
        var use_worker = typeof Worker !== "undefined";
        domwork = false;
        
        var xw = function(data, cb) {
            var worker = new Worker(XW.worker);
            worker.onmessage = function (e) {
                switch (e.data.t) {
                case "ready": break;
                case "e": console.error(e.data.d); break;
                case XW.msg: cb(JSON.parse(e.data.d)); break;
                }
            };
            worker.postMessage({ d: data, b: rABS ? "binary" : "array" });
        };

        return function(file) {
            rABS = domrabs;
            use_worker = domwork;
            var reader = new FileReader();
            reader.onload = function (e) {
                var data = e.target.result;
                if (!rABS) data = new Uint8Array(data);
                if (use_worker) xw(data, processWb);
                else processWb(X.read(data, { type: rABS ? "binary" : "array" }));
            };
            if (rABS) reader.readAsBinaryString(file);
            else reader.readAsArrayBuffer(file);
        };
    })();

    var events = {
        init: function() {
            $("#input-file").on("change",
                function (e) {
                    var tgt = e.target || window.event.srcElement,
                        files = tgt.files;
                    if (files[0]) {
                        if (files[0].size > 15728640) {
                            swal(
                                "Archivo demasiado grande",
                                "Por favor, asegúrese de que el peso de su archivo no exceda los 15MB.",
                                "error"
                            );
                            form.change.toUpload();
                            return;
                        }
                        mApp.block(".m-portlet", { type: "loader", message: "Cargando..." });
                        $(".custom-file-label").text(files[0].name);
                        doFile(files[0]);
                    } else {
                        form.change.toUpload();
                    }
                });

            $(".btn-template").on("click",
                function () {
                    var $btn = $(this);
                    $btn.addLoader();
                    $.fileDownload("/admin/asistencia-personal/plantilla/get".proto().parseURL())
                        .always(function () {
                            $btn.removeLoader();
                        }).done(function () {
                            toastr.success("Archivo descargado satisfactoriamente", "Éxito");
                        }).fail(function () {
                            toastr.error("No se pudo descargar el archivo", "Error");
                        });
                    return false;
                });

            $("#btnCancel").on("click",
                function () {
                    form.change.toUpload();
                });
        }
    };

    var form = {
        submit: function (formElement) {
            $("#user-assistance input").attr("disabled", true);
            $("#btnSave").addLoader();
            $.ajax({
                data: {
                    model: users
                },
                type: "POST",
                url: $(formElement).attr("action")
            })
                .always(function () {
                    $("#user-assistance input").attr("disabled", false);
                    $("#btnSave").removeLoader();
                    form.change.toUpload();
                })
                .done(function (result) {
                    formValidate.resetForm();
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                })
                .fail(function (e) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (e.responseText != null) $("#alert-text").html(e.responseText);
                    else $("#alert-text").html(_app.constants.toastr.message.error.task);
                    $("#m-form_alert").removeClass("m--hide").show();
                });
        },
        change: {
            toSubmit: function() {
                $("#btnSave").prop("disabled", false);
                $("#btnCancel").prop("disabled", false).show();
            },
            toUpload: function() {
                $("#btnSave").prop("disabled", true);
                $("#btnCancel").prop("disabled", true).hide();
                $(".custom-file-label").text("Escoja un archivo");
                formValidate.resetForm();
                if (userDatatable !== null)
                    datatable.destroy();
            }
        }
    };

    var validate = {
        init: function() {
            formValidate = $("#user-assistance").validate({
                submitHandler: function (formElement) {
                    if (users.length === 0) {
                        swal(
                            "Ningún Archivo Subido",
                            "Por favor, asegúrese de subir un archivo Excel con la asistencia del Personal.",
                            "error"
                        );
                        return;
                    }
                    form.submit(formElement);
                }
            });
        }
    };

    return {
        init: function () {
            $("#m-users-datatable").hide();
            validate.init();
            events.init();
        }
    }
}();

$(function() {
    UserAssistance.init();
});