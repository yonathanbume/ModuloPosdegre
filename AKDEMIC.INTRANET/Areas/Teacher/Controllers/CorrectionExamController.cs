// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Teacher.Models.CorrectionExamViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Implementations;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("profesor/examen-subsanacion")]
    public class CorrectionExamController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly ICorrectionExamService _correctionExamService;
        private readonly IUserService _userService;
        private readonly ISectionService _sectionService;
        private readonly ICourseTermService _courseTermService;
        private readonly ICourseService _courseService;
        private readonly ITermService _termService;
        private readonly IStudentService _studentService;
        private readonly AkdemicContext _context;
        private readonly IAcademicHistoryService _academicHistoryService;

        public CorrectionExamController(
            IDataTablesService dataTablesService,
            ICorrectionExamService correctionExamService,
            IUserService userService,
            ISectionService sectionService,
            ICourseTermService courseTermService,
            ICourseService courseService,
            ITermService termService,
            AkdemicContext context,
            IAcademicHistoryService academicHistoryService,
            IStudentService studentService
            )
        {
            _dataTablesService = dataTablesService;
            _correctionExamService = correctionExamService;
            _userService = userService;
            _sectionService = sectionService;
            _courseTermService = courseTermService;
            _courseService = courseService;
            _termService = termService;
            _context = context;
            _academicHistoryService = academicHistoryService;
            _studentService = studentService;
        }

        public IActionResult Index()
            => View();

        [HttpGet("get-datatable")]
        public async Task<IActionResult> GetDatatable(Guid termId, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var user = await _userService.GetUserByClaim(User);
            var result = await _correctionExamService.GetTeacherDatatable(parameters, termId, user.Id, search);
            return Ok(result);
        }

        [HttpGet("{id}/estudiantes")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var entity = await _correctionExamService.Get(id);
            var section = await _sectionService.Get(entity.SectionId);
            var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
            var course = await _courseService.GetAsync(courseTerm.CourseId);
            var term = await _termService.Get(courseTerm.TermId);

            var model = new CorrectionExamViewModel
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
            var result = await _correctionExamService.GetStudentToCorrectionExam(parameters, id, search);
            return Ok(result);
        }

        [HttpPost("calificar")]
        public async Task<IActionResult> AssignScore(AssignScoreViewModel model)
        {
            var correctionExamStudent = await _context.CorrectionExamStudents
                .Where(x => x.Id == model.Id)
                .Include(x => x.Student)
                .FirstOrDefaultAsync();

            var correctionExam = await _correctionExamService.Get(correctionExamStudent.CorrectionExamId);
            var section = await _sectionService.Get(correctionExam.SectionId);
            var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
            var term = await _termService.Get(courseTerm.TermId);
            var user = await _userService.GetUserByClaim(User);

            if (user.Id != correctionExam.TeacherId)
                return BadRequest("El docente no está asignado al examen.");

            if (term.Status != ConstantHelpers.TERM_STATES.FINISHED)
                return BadRequest("Solo se pueden calificar examenes de periodos finalizados.");

            if (correctionExamStudent.Status == ConstantHelpers.CORRECTION_EXAM_STUDENT_STATUS.QUALIFIED)
                return BadRequest("El registro se encuentra con estado calificado.");

            correctionExamStudent.Status = ConstantHelpers.CORRECTION_EXAM_STUDENT_STATUS.QUALIFIED;
            correctionExamStudent.Grade = model.Grade;
            correctionExamStudent.GradePublicationDate = DateTime.UtcNow;

            var academicHistory = new AcademicHistory
            {
                CourseId = courseTerm.CourseId,
                StudentId = correctionExamStudent.StudentId,
                TermId = courseTerm.TermId,
                Approved = model.Grade >= term.MinGrade,
                Grade = model.Grade,
                Type = ConstantHelpers.AcademicHistory.Types.CORRECTION,
                CurriculumId = correctionExamStudent.Student.CurriculumId
            };

            await _academicHistoryService.Insert(academicHistory);
            return Ok();
        }
    }
}
