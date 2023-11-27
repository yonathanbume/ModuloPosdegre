var Home = (function () {
    $("#goToRegistry").on('click', function () {
        window.location.href = `/admin/padron-de-registro`.proto().parseURL();
    });

    $("#goToDiploma").on('click', function () {
        window.location.href = `/admin/diplomas`.proto().parseURL();
    });

    $("#goToReportBachelorTitle").on('click', function () {
        window.location.href = `/admin/reporte-alumnos-bachilleres-y-titulados`.proto().parseURL();
    });


})();


