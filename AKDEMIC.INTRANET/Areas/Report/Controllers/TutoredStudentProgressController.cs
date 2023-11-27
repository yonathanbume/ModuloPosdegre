// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN
        + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Report")]
    [Route("reporte/rendimiento-tutorados")]
    public class TutoredStudentProgressController : BaseController
    {
        private readonly ITutoringStudentService _tutoringStudentService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IDataTablesService _dataTablesService;

        public TutoredStudentProgressController(IDataTablesService dataTablesService, ITutoringStudentService tutoringStudentService,
            IStudentSectionService studentSectionService)
        {
            _dataTablesService = dataTablesService;
            _tutoringStudentService = tutoringStudentService;
            _studentSectionService = studentSectionService;
        }

        /// <summary>
        /// Vista donde se muestra el rendimiento de estudiantes tutorados
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de alumnos tutorados
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <param name="onlyDisapproved">¿Solo desaprobados?</param>
        /// <returns>Listado de alumnos</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid termId, Guid? careerId, string search, bool onlyDisapproved)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _tutoringStudentService.GetTutoringStudentsProgressReportDatatable(sentParameters, termId, careerId, search, onlyDisapproved);

            return Ok(result);
        }


        /// <summary>
        /// Obtiene el listado de los cursos desaprobados
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Listado de cursos desaprobados</returns>
        [HttpGet("cursos-desaprobados/get")]
        public async Task<IActionResult> GetDisapprovedCourses(Guid studentId, Guid termId)
        {
            var data = await _studentSectionService.GetGradesByStudentAnTerm(studentId, termId);

            var result = data
                .Where(x => x.Status == ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED)
                .OrderBy(x => x.Course)
                .Select(x => new
                {
                    x.Course,
                    x.Try,
                    x.Credits,
                    x.FinalGrade
                }).ToList();

            return Ok(result);
        }
    }
}
