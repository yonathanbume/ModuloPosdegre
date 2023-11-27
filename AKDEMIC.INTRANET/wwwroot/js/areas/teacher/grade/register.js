var datatable;
var section = $("#section").val();

var initDatatable = function () {

    var form = {
        init: function () {
            $("#form-validation").validate({
                submitHandler: function (form) {
                    console.log($(this.submitButton));

                    mApp.block(".m-portlet");
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: $(form).serialize(),
                        success: function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        },
                        error: function (e) {
                            if (e.responseText != null && e.responseText != "") {
                                toastr.error(e.responseText, _app.constants.toastr.title.error);
                                return;
                            }
                            toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        },
                        complete: function () {
                            mApp.unblock(".m-portlet");
                        }
                    });
                }
            });
        }
    }

    var events = {
        init: function () {
            $("#publish-btn").on("click", function (e) {
                e.preventDefault();

                if ($("#form-validation").valid()) {

                    swal({
                        title: "Publicar notas",
                        text: "Una vez publicadas no podrán ser modificadas. ¿Está seguro?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, publicar",
                        confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                        cancelButtonText: "Cancelar"
                    }).then(function (result) {
                        if (result.value) {
                            mApp.block(".m-portlet");
                            //$("#form-validation").submit();

                            $.ajax({
                                url: "/profesor/notas/registrar/publicar".proto().parseURL(),
                                type: "POST",
                                data: $("#form-validation").serialize(),
                                success: function () {
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                                    swal({
                                        type: "success",
                                        title: "Notas publicadas",
                                        text: "Las notas han sido publicadas y seran visibles para los estudiantes",
                                        confirmButtonText: "Aceptar"
                                    }).then(function (isConfirm) {
                                        if (isConfirm) {
                                            window.location.href = `/profesor/notas/detalle/${$("#section").val()}`;
                                        }
                                    });
                                },
                                error: function (e) {
                                    mApp.unblock(".m-portlet");
                                    toastr.error(e.responseText, _app.constants.toastr.title.error);
                                }
                            });
                        }
                    });
                }
            });
        }
    }
    return {
        init: function () {
            form.init();
            events.init();
        }
    };
}();

jQuery(document).ready(function () {
    initDatatable.init();
});

var InitApp = function () {
    var datatable = {
        students: {
            object: null,
            options: {
                responsive: true,
                processing: true,
                serverSide: false,
                filter: false,
                lengthChange: false,
                paging: false,
                pageLength: 50,
                ordering: false,
                orderMulti: false,
                columnDefs: [
                    { "orderable": false, "targets": [] }
                ],
                language: {
                    "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>",
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
                    url: `/profesor/notas/registrar/getalumnos/${section}/${$("#evaluation").val()}`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON"
                },
                columns: [
                    {
                        data: null,
                        title: "#",
                        render: function (data, type, row, meta) {
                            var tpm = meta.row + 1;
                            return tpm;
                        }
                    },
                    {
                        data: "code",
                        title: "Código",
                        width: 70,
                        textAlign: "center"
                    },
                    {
                        data: "student",
                        title: "Estudiante"
                    },
                    {
                        data: null,
                        title: "No rindió",
                        width: 70,
                        className: "text-center",
                        render: function (data, type, row, meta) {
                            if (data.status) {
                                return `RET <input hidden data-id="${data.id}" type="checkbox" class="chck" name="grades[${meta.row}].NotTaken" value="true" checked>`;
                            }

                            if (!data.pending) {
                                if (data.attended) {
                                    return "<label class='m-checkbox' style='padding-left: 18px'><input disabled type='checkbox' class='chck' value='true'>&nbsp;<span></span></label>";
                                }
                                else {
                                    return "<label class='m-checkbox' style='padding-left: 18px'><input disabled checked type='checkbox' class='chck' value='true'>&nbsp;<span></span></label>";
                                }
                            }

                            if (data.attended) {
                                return "<label class='m-checkbox' style='padding-left: 18px'><input data-id='" + data.id + "' type='checkbox' class='chck' name='grades[" + meta.row + "].NotTaken' value='true'>&nbsp;<span></span></label>";
                            }
                            else {
                                return "<label class='m-checkbox' style='padding-left: 18px'><input checked data-id='" + data.id + "' type='checkbox' class='chck' name='grades[" + meta.row + "].NotTaken' value='true'>&nbsp;<span></span></label>";
                            }
                        }
                    },
                    {
                        data: null,
                        title: "Nota",
                        className: "text-center",
                        render: function (data, type, row, meta) {
                            var label = "";
                            if (data.status)//retirado
                            {
                                label = `<input hidden name="grades[${meta.row}].Id" value="${data.id}">
                                    <input hidden class="input-student-id" value="${data.studentId}">
                                    <div class="form-group m-form__group"> RET
                                        <input hidden type="number" id="input_${data.id}" class="form-control form-control-sm text-right" name="grades[${meta.row}].Grade" required value="${data.grade}">
                                    </div>`;
                            }
                            else if (!data.pending){
                                label = `<input hidden type="number" id="input_${data.id}" class="form-control form-control-sm text-right" name="grades[${meta.row}].Grade" required value="${data.grade}"><input hidden name="grades[${meta.row}].Id" value="${data.id}"><div class="form-group m-form__group"> ${data.grade} </div>`;
                            }
                            else {
                                label = `<input hidden name="grades[${meta.row}].Id" value="${data.id}">
                                    <input hidden class="input-student-id" value="${data.studentId}">
                                    <div class="form-group m-form__group">
                                        <input tabindex="1" type="number" min="0" max="20" placeholder="0" id="input_${data.id}" class="form-control form-control-sm text-right grade-input" name="grades[${meta.row}].Grade" required value="${data.grade}" data-number=${meta.row} style="width:100px; font-size: 1rem; margin: auto;">
                                    </div>`;
                            }
                            return label;
                        }
                    }
                ],
                fnDrawCallback: function () {
                    events.focusInputGrade(0);
                }
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);

                $("#data-table")
                    .on("change", ".chck", function () {
                        var id = $(this).data("id");

                        if (this.checked) {
                            $(`#input_${id}`).attr("disabled", true);
                            $(`#input_${id}`).val(0);
                            $(`#input_${id}`).parent().removeClass("has-danger");;
                        } else {
                            $(`#input_${id}`).removeAttr("disabled");
                        }
                    });

                $("#data-table")
                    .on("click focusin", ".grade-input", function () {
                        if (this.value == 0) {
                            this.value = '';
                        }
                    });

                $("#data-table")
                    .on("focusout", ".grade-input", function () {
                        if (this.value == '') {
                            this.value = '0';
                        }
                    });
            },
            reload: function () {
                this.object.ajax.reload();
            }
        }
    };

    var events = {
        focusInputGrade: function (number) {
            $("#data-table").find(`[name='grades[${number}].Grade']`).focus();
        },
        onChangeFocusInput: function () {
            $("#data-table").on("keypress", "input[type=number]", function (e) {
                var key = e.charCode || e.keyCode || 0;
                if (key == 13) {
                    var input = $(this).data("number");
                    var nextInput = (parseInt(input) + 1);
                    events.focusInputGrade(nextInput);
                }
            })
        },
        init: function () {
            $(".grade-input").on('click focusin', function () {
                console.log("enmtro");
                if (this.value == 0) {
                    this.value = '';
                }
            });
            $(".grade-input").on('focusout', function () {
                if (this.value == '') {
                    this.value = '0';
                }
            });

            this.onChangeFocusInput();
        }
    }
    return {
        init: function () {
            datatable.students.init();
            events.init();
        }
    };
}();

$(function () {
    InitApp.init();

    document.getElementById("form-validation").onkeypress = function (e) {
        var key = e.charCode || e.keyCode || 0;
        if (key == 13) {
            e.preventDefault();
        }
    };
});