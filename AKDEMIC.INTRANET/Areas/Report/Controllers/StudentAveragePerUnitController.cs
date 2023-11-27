// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles =
        ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR
    )]
    [Area("Report")]
    [Route("reporte/promedio-unidad-estudiantes")]
    public class StudentAveragePerUnitController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IConfigurationService _configurationService;

        public StudentAveragePerUnitController(
            IDataTablesService dataTablesService,
            IStudentSectionService studentSectionService,
            IConfigurationService configurationService
            )
        {
            _dataTablesService = dataTablesService;
            _studentSectionService = studentSectionService;
            _configurationService = configurationService;
        }

        public async Task<IActionResult> Index()
        {
            if(bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT)))
            {
                return View();
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet("get-datatable")]
        public async Task<IActionResult> GetDatatable(Guid termId, Guid careerId, int unitNumber, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _studentSectionService.GetStudentAveragePerUnitDatatable(parameters, termId, careerId, unitNumber, search);
            return Ok(result);
        }
    }
}
