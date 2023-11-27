using System.Threading.Tasks;
using System;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Intranet.Implementations;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using AKDEMIC.INTRANET.Areas.Admin.Models.CorrectionExamViewModels;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.CORE.Extensions;
using System.IO;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using DocumentFormat.OpenXml.Bibliography;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.SERVICE.Services.Generals.Implementations;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
    ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/examen-subsanacion")]
    public class CorrectionExamController : BaseController
    {
        private readonly ICorrectionExamService _correctionExamService;
        private readonly IDataTablesService _dataTablesService;
        private readonly ICourseTermService _courseTermService;
        private readonly ICourseService _courseService;
        private readonly AkdemicContext _context;
        private readonly ITermService _termService;
        private readonly IUserService _userService;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly ISectionService _sectionService;
        private readonly IClassroomService _classroomService;

        public CorrectionExamController(
            ICorrectionExamService correctionExamService,
            IDataTablesService dataTablesService,
            ICourseTermService courseTermService,
            ICourseService courseService,
            AkdemicContext context,
            ITermService termService,
            IUserService userService,
            ICloudStorageService cloudStorageService,
            IStudentSectionService studentSectionService,
            ISectionService sectionService,
            IClassroomService classroomService
            )
        {
            _correctionExamService = correctionExamService;
            _dataTablesService = dataTablesService;
            _courseTermService = courseTermService;
            _courseService = courseService;
            _context = context;
            _termService = termService;
            _userService = userService;
            _cloudStorageService = cloudStorageService;
            _studentSectionService = studentSectionService;
            _sectionService = sectionService;
            _classroomService = classroomService;
        }

        public async Task<IActionResult> Index()
        {
            var lastTerm = await _termService.GetLastTerm(false);
            ViewBag.LastTermId = lastTerm.Id;
            ViewBag.LastTermName = lastTerm.Name;
            return View();
        }


        [HttpGet("get-datatable")]
        public async Task<IActionResult> GetDeferredExamDatatable(Guid? termId, Guid? careerId, Guid? curriculumId, int? academicYear, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _correctionExamService.GetDatatable(parameters, termId, careerId, curriculumId, academicYear, search);
            return Ok(result);
        }

        [HttpPost("agregar")]
        public async Task<IActionResult> Add(CorrectionExamViewModel model)
        {
            var section = await _sectionService.Get(model.SectionId);
            var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);

            if (await _correctionExamService.AnyBySection(model.SectionId))
                return BadRequest("Se encontró un examen creado para la sección seleccionada.");

            var studentSections = await _studentSectionService.GetAllSectionStudentsWithUserBySectionId(model.SectionId);

            if (!studentSections.Any())
                return BadRequest("No se encontraron alumnos habilitados para dar el examen.");

            var entity = new CorrectionExam
            {
                SectionId = model.SectionId,
                ClassroomId = model.ClassroomId,
                TeacherId = model.TeacherId,
                TermId = courseTerm.TermId
            };

            if (!string.IsNullOrEmpty(model.StartDate))
            {
                if (model.Duration <= 0)
                    return BadRequest("Debe ingresar la duración.");

                var startDate = ConvertHelpers.DatepickerToDatetime(model.StartDate);
                var startTime = ConvertHelpers.TimepickerToDateTime(model.StartTime);

                var startDateTime = startDate.Date.Add(startTime.TimeOfDay).ToUtcDateTime();
                var endDateTime = startDateTime.AddMinutes(model.Duration);

                entity.StartDateTime = startDateTime;
                entity.EndDateTime = endDateTime;
            }

            if (model.File != null)
            {
                entity.File = await _cloudStorageService
                    .UploadFile(
                    model.File.OpenReadStream(),
                    ConstantHelpers.CONTAINER_NAMES.CORRECTION_EXAM,
                    Path.GetExtension(model.File.FileName));
            }

            if (model.FileResolution != null)
            {
                entity.File = await _cloudStorageService
                    .UploadFile(
                    model.FileResolution.OpenReadStream(),
                    ConstantHelpers.CONTAINER_NAMES.CORRECTION_EXAM,
                    Path.GetExtension(model.FileResolution.FileName));
            }

            await _correctionExamService.Insert(entity);
            return Ok();
        }

        [HttpGet("{id}/gestionar")]
        public async Task<IActionResult> CorrectionExamDetail(Guid id)
        {
            var entity = await _correctionExamService.Get(id);
            var section = await _sectionService.Get(entity.SectionId);
            var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
            var course = await _courseService.GetAsync(courseTerm.CourseId);
            var term = await _termService.Get(courseTerm.TermId);
            var teacher = await _userService.Get(entity.TeacherId);

            var model = new CorrectionExamViewModel
            {
                Id = entity.Id,
                FileResolutionUrl = entity.FileResolution,
                FileUrl = entity.File,
                Section = section.Code,
                Teacher = $"{teacher.UserName} - {teacher.FullName}",
                Term = term.Name,
                Course = $"{course.Code} - {course.Name}",
                SectionId = section.Id,
            };

            if (entity.ClassroomId.HasValue)
            {
                var classroom = await _classroomService.Get(entity.ClassroomId.Value);
                model.Classroom = classroom.Code;
            }

            if (entity.StartDateTime.HasValue)
                model.StartDate = entity.StartDateTime.ToLocalTimeFormat();

            if (entity.EndDateTime.HasValue)
                model.EndDate = entity.EndDateTime.ToLocalTimeFormat();

            return View(model);
        }

        [HttpGet("get-alumnos/{correctionExamId}")]
        public async Task<IActionResult> GetStudentSectionsToCorrectionExam(Guid correctionExamId)
        {
            var correctionExam = await _context.CorrectionExams.Where(x => x.Id == correctionExamId).FirstOrDefaultAsync();
            var section = await _context.Sections.Where(x => x.Id == correctionExam.SectionId).FirstOrDefaultAsync();

            var studentSections = await _context.StudentSections
                .Where(x => x.SectionId == section.Id && x.Status == ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED)
                .Select(x => new
                {
                    x.StudentId,
                    x.Student.User.FullName,
                    x.Student.User.UserName,
                    x.FinalGrade,
                })
                .ToListAsync();

            var studentsAssigned = await _context.CorrectionExamStudents.Where(x => x.CorrectionExamId == correctionExam.Id).ToListAsync();

            var result = studentSections
                .Select(x => new
                {
                    Id = x.StudentId,
                    x.FullName,
                    x.UserName,
                    isAssigned = studentsAssigned.Any(y => y.StudentId == x.StudentId),
                    x.FinalGrade
                })
                .ToList();

            result = result.OrderBy(x => x.FullName).ToList();

            return Ok(result);
        }

        [HttpPost("asignar-estudiante")]
        public async Task<IActionResult> AssignStudent(Guid studentId, Guid correctionExamId)
        {
            var exam = await _correctionExamService.Get(correctionExamId);
            var studentSection = await _context.StudentSections.Where(x => x.StudentId == studentId && x.SectionId == exam.SectionId).FirstOrDefaultAsync();

            if (studentSection.Status != ConstantHelpers.STUDENT_SECTION_STATES.DISAPPROVED)
                return BadRequest("El estudiante no se encuentra con el estado desaprobado.");

            var correctionExamStudent = await _context.CorrectionExamStudents.Where(x => x.CorrectionExamId == correctionExamId && x.StudentId == studentId).FirstOrDefaultAsync();

            if (correctionExamStudent != null)
            {
                if (correctionExamStudent.Status == ConstantHelpers.CORRECTION_EXAM_STUDENT_STATUS.QUALIFIED)
                    return BadRequest("El examen se encuentra calificado");

                _context.CorrectionExamStudents.Remove(correctionExamStudent);
            }
            else
            {
                var entity = new CorrectionExamStudent
                {
                    StudentId = studentId,
                    CorrectionExamId = exam.Id,
                    Status = ConstantHelpers.CORRECTION_EXAM_STUDENT_STATUS.PENDING
                };

                await _context.CorrectionExamStudents.AddAsync(entity);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("get-alumnos-asignados-datatable")]
        public async Task<IActionResult> GetStudentSectionsToCorrectionExamDatatable(Guid correctionExamId, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _correctionExamService.GetStudentToCorrectionExam(parameters, correctionExamId, search);
            return Ok(result);
        }

        [HttpPost("eliminar-estudiante-asignado")]
        public async Task<IActionResult> DeleteAssignedStudent(Guid correctionExamStudentId)
        {
            var entity = await _context.CorrectionExamStudents.Where(x => x.Id == correctionExamStudentId).FirstOrDefaultAsync();

            if (entity.Status == ConstantHelpers.CORRECTION_EXAM_STUDENT_STATUS.QUALIFIED)
                return BadRequest("Ya se calificó al estudiante.");

            _context.CorrectionExamStudents.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
