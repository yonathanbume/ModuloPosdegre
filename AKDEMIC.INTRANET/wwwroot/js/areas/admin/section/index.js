var sections = function () {
    var Modal = {
        load: {
            getTeachers: function (element, event) {
                var id = $(element).data("id");
                var id_teacher = $(element).data("teacher");
                Modal.list["sections-ajax-edit"] = $.ajax({
                    type: 'GET', 
                    url: ('/admin/secciones/editar-seccion-profesor').proto().parseURL(),
                    success: function (data, textStatus, jqXHR) {
                        $("input[name='Id']").val(id);
                        $("input[name='IdTeacher']").val(id_teacher);
                        $("#Teacher-select2").select2({
                            data : data.items
                        });
                        $("#Teacher-select2").val(id_teacher).trigger('change');

                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        toastr.error(_app.constants.toastr.message.update, _app.constants.toastr.title.error);
                    }
                });
            },
            edit: function (element, event) {
                
                var formElements = element.elements;
                Modal.list["sections-ajax-edit"] = $.ajax({
                    data: {
                        SectionId: formElements["Id"].value,
                        TeacherId: formElements["TeacherId"].value                        
                    },

                    type: element.method,
                    url: element.action,
                    beforeSend: function (jqXHR, settings) {
                        $(element).addLoader();
                    },
                    complete: function (jqXHR, textStatus) {
                        $(element).removeLoader();
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        toastr.error(_app.constants.toastr.message.error.update, _app.constants.toastr.title.error);
                    },
                    success: function (data, textStatus, jqXHR) {

                        datatable.reload();
                        Modal.list["sections-modal-edit"].modal("hide");
                        toastr.success(_app.constants.toastr.message.success.update, _app.constants.toastr.title.success);
                    }
                });
            }
        },
        list: {}
    }
    var validate = {
        list: {},
        load: {
            edit: function () {
                validate.list["sections-modal-edit-form"] = $("#sections-modal-edit-form").validate({
                    submitHandler: function (form, event) {
                        Modal.load.edit(form, event);
                    }
                });
            }
        } 
    }

    var datatable;
    var options = {
        data: {
            source: {
                read: {
                    method: "GET",
                    url: ""
                }
            }
        },
        columns: [
            {
                field: "code",
                title: "Código",
                width: 80
            },
            {
                field: "teacher",
                title: "Profesor(a)",
                width: 120
            },
            {
                field: "vacancies",
                title: "Max.Vacantes",
                width: 140
            },
            {
                field: "options",
                title: "Opciones",
                width: 250,
                sortable: false,
                filterable: false,
                template: function (row) {
                    var url = `/admin/secciones/acta-final/${row.id}`.proto().parseURL();

                    if (row.termfinished != "Finished")
                        return '<button type="button" data-id="' + row.id + '" data-teacher="' + row.id_teacher + '" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit" onClick="sections.initModal(this, event)"><span><i class="la la-edit"></i><span>Editar Profesor</span></span></button> ' +
                    `<a href="${url}" class="btn btn-primary btn-sm m-btn m-btn--icon"><span><i class="la la-file"></i><span>Acta final</span></span></a>`;
                    else {
                        return '<button type="button" data-id="' + row.id + '" data-teacher="' + row.id_teacher + '" class="btn btn-info btn-sm m-btn m-btn--icon btn-edit" onClick="sections.initModal(this, event)" disabled><span><i class="la la-edit"></i><span>Editar Profesor</span></span></button> ' +
`<a href="${url}" class="btn btn-primary btn-sm m-btn m-btn--icon"><span><i class="la la-file"></i><span>Acta final</span></span></a>`;
                    }

                }                                                                                                                          
            }
        ]
    }
    var loadModal = function (element, event) {
        var sectionsModalEditForm = document.getElementById("sections-modal-edit-form");

        _app.modules.form.reset({
            element: sectionsModalEditForm
        });

        Modal.load.getTeachers(element, event);
        Modal.list["sections-modal-edit"] = $("#sections-modal-edit").modal("show");

    }
    var loadDatatable = function () {
        options.data.source.read.url = ("/admin/secciones/" + $("#select_career").val() + "/periodo/" + $("#select_term").val()).proto().parseURL();
        if (datatable !== undefined) datatable.destroy();
        datatable = $(".m-datatable").mDatatable(options);
    }

    //var loadCareerSelect = function () {
    //    return $.ajax({
    //        url: ("/admin/secciones/obtener-cursos").proto().parseURL()
    //    }).done(function (data) {
    //        $("#select_career").select2({
    //            data: data.results,
    //            //minimumResultsForSearch: -1
    //        }).on("change", function (e) {
    //            loadDatatable();
    //        });
    //    });
    //};

    var loadCareerSelect = function () {
        $('#select_career').select2({
            allowClear: true,
            placeholder: "Buscar..",
            ajax: {
                type: 'GET',
                url: ("/admin/secciones/obtener-cursos").proto().parseURL(),
                delay: 1000,
                data: function (params) {
                    return {
                        searchValue: params.term
                    };
                },
                processResults: function (data) {
                    return {
                        results: data.results
                    };
                    //return {
                    //    results: $.map(result, function (item) {
                    //        return {
                    //            text: item.text,
                    //            id: item.id
                    //        }
                    //    })
                    //};
                },
                escapeMarkup: function (markup) {
                    return markup;
                },
                minimumInputLength: 1
            }
        });

        $("#select_career").on('change', function () {
            loadDatatable();
        });
        //.on("change", function (e) {
        //    loadDatatable();
        //});
        //return $.ajax({
        //    url:
        //}).done(function (data) {
        //    $("#select_career").select2({
        //        data: data.results,
        //        //minimumResultsForSearch: -1
        //    }).on("change", function (e) {
        //        loadDatatable();
        //    });
        //});
    };
    var loadTermSelect = function () {
        return $.ajax({
            url: ("/periodos/get").proto().parseURL()
        }).done(function (data) {
            $("#select_term").select2({
                data: data.items
            }).val(data.selected).trigger('change');
            $("#select_term").on("change", function (e) {
                loadDatatable();
            });
        });
    }
    var initializer = function () {
        $.when(loadCareerSelect(), loadTermSelect()).done(function () {
            loadDatatable();
        });
    }
    return {
        init: function () {

            initializer();
            validate.load.edit();
        },
        initModal: function (element,event) {   
            loadModal(element, event);
        }
    }
}();

$(function () {
    sections.init();
    //sections.initModal(element,event);
})  
    
 