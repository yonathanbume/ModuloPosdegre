// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Teacher.Models.DeferredExamViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("profesor/examenes-aplazados")]
    public class DeferredExamController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly ISectionService _sectionService;
        private readonly ICourseTermService _courseTermService;
        private readonly IDeferredExamService _deferredExamService;
        private readonly ICourseService _courseService;
        private readonly IConfigurationService _configurationService;
        private readonly ITermService _termService;
        private readonly IStudentService _studentService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly IDeferredExamStudentService _deferredExamStudentService;

        public DeferredExamController(
            IDataTablesService dataTablesService,
            ISectionService sectionService,
            ICourseTermService courseTermService,
            IDeferredExamService deferredExamService,
            ICourseService courseService,
            IConfigurationService configurationService,
            ITermService termService,
            IAcademicHistoryService academicHistoryService,
            IDeferredExamStudentService deferredExamStudentService,
            IStudentService studentService
            )
        {
            _dataTablesService = dataTablesService;
            _sectionService = sectionService;
            _courseTermService = courseTermService;
            _deferredExamService = deferredExamService;
            _courseService = courseService;
            _configurationService = configurationService;
            _termService = termService;
            _academicHistoryService = academicHistoryService;
            _deferredExamStudentService = deferredExamStudentService;
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            var enabled_deferred_exam = bool.Parse(await _configurationService.GetValueByKey(AKDEMIC.CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.ENABLED_DEFERRED_EXAM));

            if (!enabled_deferred_exam)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            return View();
        }

        [HttpGet("get-datatable")]
        public async Task<IActionResult> GetDatatable(Guid termId, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _deferredExamService.GetDeferredExamDatatable(parameters, termId, null, null, null, search, User);
            return Ok(result);
        }

        [HttpGet("{id}/estudiantes")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var entity = await _deferredExamService.Get(id);
            var section = await _sectionService.Get(entity.SectionId);
            var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
            var course = await _courseService.GetAsync(courseTerm.CourseId);
            var term = await _termService.Get(courseTerm.TermId);

            var model = new DeferredExamViewModel
            {
                Course = course.FullName,
                Term = term.Name,
                Id = entity.Id,
                Section = section.Code
            };

            return View(model);
        }

        [HttpGet("get-alumnos-asignados")]
        public async Task<IActionResult> GetAssignedStudents(Guid id, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _deferredExamStudentService.GetDeferredExamStudentsDatatable(parameters, id, search);
            return Ok(result);
        }

        [HttpPost("calificar")]
        public async Task<IActionResult> AssignScore(AssignScoreViewModel model)
        {
            var deferredExamStudent = await _deferredExamStudentService.Get(model.Id);
            var deferredExam = await _deferredExamService.Get(deferredExamStudent.DeferredExamId);
            var section = await _sectionService.Get(deferredExam.SectionId);
            var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
            var term = await _termService.Get(courseTerm.TermId);
            var student = await _studentService.Get(deferredExamStudent.StudentId);

            if (term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                return BadRequest("Solo se pueden calificar examenes aplazados del periodo activo.");

            if (deferredExamStudent.Status == ConstantHelpers.DEFERRED_EXAM_STATUS.QUALIFIED)
                return BadRequest("El registro se encuentra con estado calificado.");

            deferredExamStudent.GradePublicationDate = DateTime.UtcNow;
            deferredExamStudent.Status = ConstantHelpers.DEFERRED_EXAM_STATUS.QUALIFIED;
            deferredExamStudent.Grade = model.Grade;

            var academicHistory = new AcademicHistory
            {
                CourseId = courseTerm.CourseId,
                StudentId = deferredExamStudent.StudentId,
                TermId = courseTerm.TermId,
                Approved = model.Grade >= term.MinGrade,
                Grade = model.Grade,
                Type = ConstantHelpers.AcademicHistory.Types.DEFERRED,
                CurriculumId = student.CurriculumId
            };

            await _academicHistoryService.Insert(academicHistory);
            return Ok();
        }
    }
}
