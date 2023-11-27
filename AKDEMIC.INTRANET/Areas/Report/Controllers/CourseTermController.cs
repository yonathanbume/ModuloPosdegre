// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Areas.Report.ViewModels.CourseTermViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
       ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
       ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
       ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," +
       ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
       ConstantHelpers.ROLES.DEAN + "," +
       ConstantHelpers.ROLES.DEAN_SECRETARY + "," +
       ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," +
       ConstantHelpers.ROLES.REPORT_QUERIES + "," + CORE.Helpers.ConstantHelpers.ROLES.VICERRECTOR)]
    [Area("Report")]
    [Route("reporte/curso-periodo")]
    public class CourseTermController : BaseController
    {
        private readonly ICourseTermService _courseTermService;
        private readonly ITeacherService _teacherService;
        private readonly IClassService _classService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly AkdemicContext _context;
        private readonly IViewRenderService _viewRenderService;
        private IWebHostEnvironment _hostingEnvironment;
        public IConverter _dinkConverter { get; }

        public CourseTermController(AkdemicContext context,
              IConverter dinkConverter,
            IViewRenderService viewRenderService,
            IWebHostEnvironment environment,
            IDataTablesService dataTablesService,
            ICourseTermService courseTermService,
            ITeacherService teacherService,
            IClassService classService,
            IStudentSectionService studentSectionService) : base(dataTablesService)
        {
            _context = context;
            _courseTermService = courseTermService;
            _teacherService = teacherService;
            _classService = classService;
            _studentSectionService = studentSectionService;
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = environment;
        }

        /// <summary>
        /// Vista principal del controlador
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de cursos para ser usado en tablas
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="curriculum">Identificador del plan de estudios</param>
        /// <param name="academicyear">Ciclo académico del plan</param>
        /// <returns>Objeto que contiene el listado de docentes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid termId, Guid curriculum, int academicyear)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _courseTermService.GetCourseTermReportDatatable(sentParameters, termId, curriculum, academicyear);

            return Ok(result);
        }

        /// <summary>
        /// Vista detalle del curso
        /// </summary>
        /// <param name="academicYearCourseId">Identificador del curso del plan de estudios</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("detalle/{academicYearCourseId}/{termId}")]
        public async Task<IActionResult> Detail(Guid academicYearCourseId, Guid termId)
        {
            var model = await _context.AcademicYearCourses
                .Where(x => x.Id == academicYearCourseId)
                .Select(x => new DetailViewModel
                {
                    AcademicYear = x.AcademicYear,
                    Career = x.Curriculum.Career.Name,
                    Curriculum = x.Curriculum.Code,
                    CurriculumId = x.CurriculumId,
                    CourseId = x.CourseId,
                    TermId = termId
                }).FirstOrDefaultAsync();

            return View(model);
        }
    }
}
