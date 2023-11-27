var reportStudentersonalInformation = function () {
    var datatable = null;
    var options = getSimpleDataTableConfiguration({
        url: `/admin/reporte_estudiante_fichas/get/`.proto().parseURL(),
        pageLength: 10,
        orderable: [],
        columns: [
            {
                data: 'username',
                title: 'Código',
            },
            {
                data: 'name',
                title: 'Nombre',
            },
            {
                data: 'paternalsurname',
                title: 'Apellido Paterno',
            },
            {
                data: 'maternalsurname',
                title: 'Apellido Materno',

            },
            {
                data: 'email',
                title: 'Email',
            },
            {
                data: null,
                title: "Opciones",
                render: function (row) {
                    return `<button data-id="${row.id}" class="btn btn-primary btn-sm m-btn m-btn--icon btn-detail"<span><i class="la la-eye"> </i> </span>Ver Ficha </span></span></button>`;
                }
            }

        ]
    });
    var loadCareersSelect = function () {
        var objectAll = $("<option/>", { value: _app.constants.guid.empty, text: "Todos" });
        $.ajax({
            type: 'GET',
            url: `/carreras/get`.proto().parseURL()
        }).done(function (data) {
            $("#select2Careers").select2({
                data: data.items
            });
            $('#select2Careers').prepend(objectAll).val(_app.constants.guid.empty).trigger('change');
        });
    }
    var getDeparments = function () {
        var objectAll = $("<option/>", { value: _app.constants.guid.empty, text: "Todos" });
        $.ajax({
            type: 'GET',
            url: ('/departamentos/get').proto().parseURL()
        }).done(function (data) {
            $("#select2Departments").select2({
                data: data.items
            });
            $('#select2Departments').prepend(objectAll).val(_app.constants.guid.empty).trigger('change');
        });
    }
    var getProvinces = function () {
        var objectAll = $("<option/>", { value: _app.constants.guid.empty, text: "Todos" });
        $("#select2Departments").on('change', function () {
            var Id = $(this).val();
            if (Id === _app.constants.guid.empty) {
                $("#select2Provinces").empty();
                $("#select2Provinces").prepend(objectAll);
                $("#select2Provinces").select2().trigger('change');
            } else {
                $.ajax({
                    type: 'GET',
                    url: ('/departamentos/' + Id + '/provincias/get').proto().parseURL()
                }).done(function (data) {
                    $("#select2Provinces").empty();
                    $("#select2Provinces").select2({
                        data: data.items
                    }).trigger('change');
                });
            }
        });
    }
    var getDistricts = function () {
        var objectAll = $("<option/>", { value: _app.constants.guid.empty, text: "Todos" });
        $("#select2Provinces").on('change', function () {
            var Id = $(this).val();
            var IdDepartamento = $("#select2Departments").val();
            if (IdDepartamento === _app.constants.guid.empty) {
                $("#select2Districts").empty();
                $("#select2Districts").prepend(objectAll);
                $("#select2Districts").select2().trigger('change');
            } else {
                $.ajax({
                    type: 'GET',
                    url: ('/departamentos/' + IdDepartamento + '/provincias/' + Id + '/distritos/get').proto().parseURL()
                }).done(function (data) {
                    $("#select2Districts").empty();
                    $("#select2Districts").select2({
                        data: data.items
                    });

                });
            }
        });
    }
    var getAdmissionTypes = function () {
        var objectAll = $("<option/>", { value: _app.constants.guid.empty, text: "Todos" });
        $.ajax({
            type: 'GET',
            url: ('/admissionTypes/get').proto().parseURL()
        }).done(function (data) {
            $("#select2AdmissionType").select2({
                data: data.items,
                minimumResultsForSearch: -1
            });
            $('#select2AdmissionType').prepend(objectAll).val(_app.constants.guid.empty).trigger('change');
        });
    }
    var loadDatatable = function (caid, did, six, scid, upid, atid, starage, endage) {
        if (datatable !== null) {
            datatable.destroy();
            datatable = null;
        }
        var newoptions = options;
        newoptions.ajax.url = `/admin/reporte_alumnos_datos_personales/get/${caid}/${did}/${six}/${scid}/${upid}/${atid}/${starage}/${endage}`.proto().parseURL();
        datatable = $("#ajax_data").DataTable(newoptions);

        datatable.on('click', '.btn-detail', function () {
            var sid = $(this).data("id");
            window.location.href = `/admin/bienestar_institucional/ficha/${sid}/get`.proto().parseURL();
        });
    }
    var sexSelect2 = function () {
        var objectAll = $("<option/>", { value: _app.constants.guid.empty, text: "Todos" });
        $('#selectSex').prepend(objectAll).val(_app.constants.guid.empty).trigger('change');
        $("#selectSex").select2({
            minimumResultsForSearch: -1
        });
    }

    var SchoolTypeNameSelect2 = function () {
        var objectAll = $("<option/>", { value: _app.constants.guid.empty, text: "Todos" });
        $('#selectSchoolType').prepend(objectAll).val(_app.constants.guid.empty).trigger('change');
        $("#selectSchoolType").select2({
            minimumResultsForSearch: -1
        });
    }

    var UniversityPreparationSelect2 = function () {
        var objectAll = $("<option/>", { value: _app.constants.guid.empty, text: "Todos" });
        $('#selectUniversityPreparation').prepend(objectAll).val(_app.constants.guid.empty).trigger('change');
        $("#selectUniversityPreparation").select2({
            minimumResultsForSearch: -1
        });
    }

    var clear = function () {
        $("#m_reset").on('click', function () {
            $("#startAge").val("");
            $("#endAge").val("");
        });

    }
    var search = function () {
        $("#search").on('click', function () {
            var caid = $("#select2Careers").val() == _app.constants.guid.empty ? null : $("#select2Careers").val();
            var did = $("#select2Districts").val() == _app.constants.guid.empty ? null : $("#select2Districts").val();
            var six = $("#selectSex").is(":hidden") == true ? null : $("#selectSex").val();
            var scid = $("#selectSchoolType").is(":hidden") == true ? null : $("#selectSchoolType").val();
            var upid = $("#selectUniversityPreparation").is(":hidden") == true ? null : $("#selectUniversityPreparation").val();
            var atid = $("#select2AdmissionType").is(":hidden") == true || $("#select2AdmissionType").val() == _app.constants.guid.empty ? null : $("#select2AdmissionType").val();
            var starage = $("#startAge").is(":hidden") == true ? null : $("#startAge").val();
            var endage = $("#endAge").is(":hidden") == true ? null : $("#endAge").val();
            loadDatatable(caid, did, six, scid, upid, atid, starage, endage);
        });
    }

    var hideFilters = function () {
        $("#advancedfilters_content").css('display', 'none');
    }
    var checkboxEvent = function () {
        $("#advanced_filters").on('click', function () {
            if ($(this).is(":checked")) {
                $("#advancedfilters_content").css('display', 'flex');
                $("#selectSex").css('visibility', 'visible');
                $("#selectSchoolType").css('visibility', 'visible');
                $("#selectUniversityPreparation").css('visibility', 'visible');
                $("#select2AdmissionType").css('visibility', 'visible');
                $("#startAge").css('visibility', 'visible');
                $("#endAge").css('visibility', 'visible');
            }
            else {
                $("#advancedfilters_content").css('display', 'none');
                $("#selectSex").css('visibility', 'hidden');
                $("#selectSchoolType").css('visibility', 'hidden');
                $("#selectUniversityPreparation").css('visibility', 'hidden');
                $("#select2AdmissionType").css('visibility', 'hidden');
                $("#startAge").css('visibility', 'hidden');
                $("#endAge").css('visibility', 'hidden');
            }
        });
    }

    return {
        load: function () {
            loadCareersSelect();
            getDeparments();
            getProvinces();
            getDistricts();
            getAdmissionTypes();
            clear();
            search();
            checkboxEvent();
            sexSelect2();
            SchoolTypeNameSelect2();
            UniversityPreparationSelect2();
        }
    }
}();



$(function () {
    $("#advancedfilters_content").css('display', 'none');
    reportStudentersonalInformation.load();
})