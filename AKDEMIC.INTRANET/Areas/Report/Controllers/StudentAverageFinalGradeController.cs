// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles =
        ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_SECRETARY + "," +
        ConstantHelpers.ROLES.CAREER_DIRECTOR_GENERAL + "," +
        CORE.Helpers.ConstantHelpers.ROLES.VICERRECTOR)]
    [Area("Report")]
    [Route("reporte/estudiantes-promedios-del-periodo")]
    public class StudentAverageFinalGradeController : BaseController
    {
        private readonly IStudentService _studentService;

        public StudentAverageFinalGradeController(
            IStudentService studentService,
            IDataTablesService dataTablesService
        ) : base(dataTablesService)
        {
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene una lista de los estudiantes con su promedio aritmético y promedio ponderado de los siguientes filtros
        /// </summary>
        /// <param name="searchValue">Texto de busqueda</param>
        /// <param name="termId">Identificador del perido académico</param>
        /// <param name="academicYear">Ciclo académico</param>
        /// <param name="facultyId">Identificador de la Facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns></returns>
        [HttpGet("datatable/get")]
        public async Task<IActionResult> GetDatatable(string searchValue, Guid termId, Guid facultyId, int? academicYear = -99, Guid? careerId = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            int? finalAcademicYear = null;
            if (academicYear != -99)
                finalAcademicYear = academicYear;
            var result = await _studentService.GetStudentsWithAverageFinalGrades(sentParameters, termId, facultyId, finalAcademicYear, careerId, searchValue, User);
            return Ok(result);
        }
    }
}
