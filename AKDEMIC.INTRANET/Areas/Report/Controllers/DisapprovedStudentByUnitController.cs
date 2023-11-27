// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Report")]
    [Route("reporte/desaprobados-unidad")]
    public class DisapprovedStudentByUnitController : BaseController
    {
        private readonly IStudentSectionService _studentSectionService;
        private readonly AkdemicContext _context;

        public DisapprovedStudentByUnitController(IDataTablesService dataTablesService,
            IStudentSectionService studentSectionService, AkdemicContext context) : base(dataTablesService)
        {
            _studentSectionService = studentSectionService;
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid careerId, Guid curriculumId, byte academicYear, byte unit)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentSectionService.GetDisapprovedStudentsByUnitReportDatatable(sentParameters, careerId, curriculumId, academicYear, unit);
            return Ok(result);
        }

        [HttpGet("get-unidades")]
        public async Task<IActionResult> GetUnits(Guid curriculumId, byte academicYear)
        {
            var units = await _context.Evaluations
                .Where(x => x.CourseUnitId.HasValue && x.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE
                && x.CourseTerm.Sections.Any(y => y.StudentSections.Any(z => z.Student.CurriculumId == curriculumId && z.Student.CurrentAcademicYear == academicYear)))
                .Select(x => x.CourseUnit.Number)
                .ToListAsync();

            units = units.Distinct().OrderBy(x => x).ToList();

            return Ok(new { items = units });
        }
    }
}
