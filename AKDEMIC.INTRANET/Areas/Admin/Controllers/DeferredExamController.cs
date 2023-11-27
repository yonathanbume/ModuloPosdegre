// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.DeferredExamViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/examen-aplazado")]
    public class DeferredExamController : BaseController
    {
        private readonly IDeferredExamService _deferredExamService;
        private readonly IDataTablesService _dataTablesService;
        private readonly AkdemicContext _context;
        private readonly IClassroomService _classroomService;
        private readonly ISectionService _sectionService;
        private readonly ICourseTermService _courseTermService;
        private readonly ICourseService _courseService;
        private readonly IConfigurationService _configurationService;
        private readonly IDeferredExamStudentService _deferredExamStudentService;

        public DeferredExamController(
            IDeferredExamService deferredExamService,
            IDataTablesService dataTablesService,
            AkdemicContext context,
            IClassroomService classroomService,
            ISectionService sectionService,
            ICourseTermService courseTermService,
            ICourseService courseService,
            IConfigurationService configurationService,
            IDeferredExamStudentService deferredExamStudentService
            )
        {
            _deferredExamService = deferredExamService;
            _dataTablesService = dataTablesService;
            _context = context;
            _classroomService = classroomService;
            _sectionService = sectionService;
            _courseTermService = courseTermService;
            _courseService = courseService;
            _configurationService = configurationService;
            _deferredExamStudentService = deferredExamStudentService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("get-datatable")]
        public async Task<IActionResult> GetDeferredExamDatatable(Guid? termId, Guid? careerId, Guid? curriculumId, int? academicYear, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _deferredExamService.GetDeferredExamDatatable(parameters, termId, careerId, curriculumId, academicYear, search);
            return Ok(result);
        }

        [HttpPost("agregar")]
        public async Task<IActionResult> Add(DeferredExamViewModel model)
        {
            if (await _deferredExamService.AnyBySection(model.SectionId))
                return BadRequest("Se encontró un examen creado para la sección seleccionada.");

            var studentSectionsAvailable = await _deferredExamService.GetStudentSectionsAvailableToDeferredExam(model.SectionId);

            if (!studentSectionsAvailable.Any())
                return BadRequest("No se encontraron alumnos habilitados para dar el examen.");

            var entity = new DeferredExam
            {
                SectionId = model.SectionId,
                ClassroomId = model.ClassroomId,
            };

            if (!string.IsNullOrEmpty(model.StartDate))
            {
                if (model.Duration <= 0)
                    return BadRequest("Debe ingresar la duración.");

                var startDate = ConvertHelpers.DatepickerToDatetime(model.StartDate);
                var startTime = ConvertHelpers.TimepickerToDateTime(model.StartTime);

                var startDateTime = startDate.Date.Add(startTime.TimeOfDay).ToUtcDateTime();
                var endDateTime = startDateTime.AddMinutes(model.Duration);

                entity.StartTime = startDateTime;
                entity.EndTime = endDateTime;
            }

            await _deferredExamService.Insert(entity);
            return Ok();
        }

        [HttpGet("{id}/gestionar")]
        public async Task<IActionResult> DeferredExamDetail(Guid id)
        {
            var entity = await _deferredExamService.Get(id);
            var section = await _sectionService.Get(entity.SectionId);
            var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
            var course = await _courseService.GetAsync(courseTerm.CourseId);

            var model = new DeferredExamViewModel
            {
                Id = entity.Id,
                ClassroomId = entity.ClassroomId,
                Course = course.FullName,
                Section = section.Code
            };

            if (entity.ClassroomId.HasValue)
            {
                var classroom = await _classroomService.Get(entity.ClassroomId.Value);
                model.Classroom = classroom.Code;
            }

            if (entity.StartTime.HasValue)
                model.StartDate = entity.StartTime.ToLocalDateTimeFormat();

            if (entity.EndTime.HasValue)
                model.EndDate = entity.EndTime.ToLocalDateTimeFormat();

            return View(model);

        }

        [HttpGet("get-alumnos/{deferredExamId}")]
        public async Task<IActionResult> GetStudentSectionsToDeferredExam(Guid deferredExamId)
        {
            var minAvgDeferredExam = Convert.ToDecimal(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.MIN_AVG_DEFERRED_EXAM));

            var deferredExam = await _context.DeferredExams.Where(x => x.Id == deferredExamId).FirstOrDefaultAsync();
            var section = await _context.Sections.Where(x => x.Id == deferredExam.SectionId).FirstOrDefaultAsync();
            var term = await _context.Sections.Where(x => x.Id == section.Id).Select(x => x.CourseTerm.Term).FirstOrDefaultAsync();
            var evaluations = await _context.Evaluations.Where(x => x.CourseTermId == section.CourseTermId).ToListAsync();

            var studentSections = await _context.StudentSections
                .Where(x => x.SectionId == section.Id)
                //.Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN && x.Grades.Count() >= evaluations.Count())
                //.Where(x => x.FinalGrade >= minAvgDeferredExam && x.FinalGrade < term.MinGrade)
                .Select(x => new
                {
                    x.StudentId,
                    x.Student.User.FullName,
                    x.Student.User.UserName,
                    x.FinalGrade,
                    x.Status,
                    gradesCount = x.Grades.Count()
                })
                .ToListAsync();

            var studentsAssigned = await _context.DeferredExamStudents.Where(x => x.DeferredExamId == deferredExam.Id).ToListAsync();

            var result = studentSections
                .Select(x => new
                {
                    Id = x.StudentId,
                    x.FullName,
                    x.UserName,
                    x.Status,
                    x.gradesCount,
                    isAssigned = studentsAssigned.Any(y => y.StudentId == x.StudentId),
                    x.FinalGrade
                })
                .ToList();

            result = result
                .Where(x =>
                x.isAssigned ||
                (x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN && x.gradesCount >= evaluations.Count() && x.FinalGrade >= minAvgDeferredExam && x.FinalGrade < term.MinGrade)
                ).ToList();

            result = result.OrderBy(x => x.FullName).ToList();

            return Ok(result);
        }

        [HttpPost("asignar-estudiante")]
        public async Task<IActionResult> AssignStudent(Guid studentId, Guid deferredExamId)
        {
            var exam = await _context.DeferredExams.Where(x => x.Id == deferredExamId).FirstOrDefaultAsync();
            var minAvgDeferredExam = Convert.ToDecimal(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.MIN_AVG_DEFERRED_EXAM));

            var section = await _context.Sections.Where(x => x.Id == exam.SectionId)
                .Select(x => new
                {
                    x.Id,
                    x.CourseTermId,
                    x.CourseTerm.Term.MinGrade
                })
                .FirstOrDefaultAsync();

            var deferredExamStudent = await _context.DeferredExamStudents.Where(x => x.DeferredExamId == exam.Id && x.StudentId == studentId).FirstOrDefaultAsync();
            var evaluations = await _context.Evaluations.Where(x => x.CourseTermId == section.CourseTermId).ToListAsync();

            var studentSection = await _context.StudentSections
               .Where(x => x.SectionId == exam.SectionId && x.StudentId == studentId)
               .Select(x => new
               {
                   x.StudentId,
                   x.Student.User.FullName,
                   x.Student.User.UserName,
                   x.FinalGrade,
                   x.Status,
                   Grades = x.Grades.ToList()
               })
               .FirstOrDefaultAsync();

            if (deferredExamStudent != null)
            {
                if (deferredExamStudent.Status == ConstantHelpers.DEFERRED_EXAM_STATUS.QUALIFIED)
                    return BadRequest("El examen se encuentra calificado.");

                _context.DeferredExamStudents.Remove(deferredExamStudent);
            }
            else
            {
                if (studentSection.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                    return BadRequest("El estudiante se encuentra con estado retirado.");

                if (studentSection.FinalGrade < minAvgDeferredExam)
                    return BadRequest("El estudiante no cumple con la nota mínima para poder dar el examen.");

                if (studentSection.FinalGrade >= section.MinGrade)
                    return BadRequest("El estudiante se encuentra con un promedio aprobatorio.");

                foreach (var eva in evaluations)
                {
                    if (!studentSection.Grades.Any(y => y.EvaluationId == eva.Id))
                        return BadRequest($"El alumno {studentSection.FullName} tiene pendiente la evaluación {eva.Name}");
                }

                var entity = new DeferredExamStudent
                {
                    StudentId = studentId,
                    DeferredExamId = deferredExamId,
                    Status = ConstantHelpers.DEFERRED_EXAM_STATUS.PENDING
                };

                await _context.DeferredExamStudents.AddAsync(entity);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("get-alumnos-asignados-datatable")]
        public async Task<IActionResult> GetAssignedStudentsDatatable(Guid deferredExamId, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _deferredExamStudentService.GetDeferredExamStudentsDatatable(parameters, deferredExamId, search);
            return Ok(result);
        }

        [HttpPost("{id}/actualizar-estudiantes-asignados")]
        public async Task<IActionResult> UpdateAssignedStudents(Guid id)
        {
            var entity = await _deferredExamService.Get(id);
            var studentsAssigned = await _deferredExamStudentService.GetAll(entity.Id);

            var studentSectionsAvailable = await _deferredExamService.GetStudentSectionsAvailableToDeferredExam(entity.SectionId);

            var toAdd = new List<DeferredExamStudent>();

            foreach (var item in studentSectionsAvailable)
            {
                if (!studentsAssigned.Any(y => y.StudentId == item.StudentId))
                {
                    var newStudent = new DeferredExamStudent
                    {
                        StudentId = item.StudentId,
                        Status = ConstantHelpers.DEFERRED_EXAM_STATUS.PENDING,
                        DeferredExamId = entity.Id
                    };

                    toAdd.Add(newStudent);
                }
            }

            var toDelete = studentsAssigned.Where(x => x.Status != ConstantHelpers.DEFERRED_EXAM_STATUS.QUALIFIED && !studentSectionsAvailable.Any(y => y.StudentId == x.StudentId)).ToList();

            await _deferredExamStudentService.DeleteRange(toDelete);
            await _deferredExamStudentService.InsertRange(toAdd);

            return Ok();
        }

        [HttpPost("{id}/eliminar-estudiante-asignado")]
        public async Task<IActionResult> DeleteAssignedStudent(Guid deferredExamStudentId)
        {
            var entity = await _deferredExamStudentService.Get(deferredExamStudentId);

            if (entity.Status == ConstantHelpers.DEFERRED_EXAM_STATUS.QUALIFIED)
                return BadRequest("Ya se calificó al estudiante.");

            await _deferredExamStudentService.Delete(entity);
            return Ok();
        }
    }
}
