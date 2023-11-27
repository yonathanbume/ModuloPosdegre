using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Teacher.Models.StudentAssistanceViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("profesor/asistencia")]
    public class StudentAssistanceController : BaseController
    {
        private readonly AkdemicContext _context;
        private readonly IClassService _classService;
        private readonly ISectionService _sectionService;
        private readonly IHolidayService _holidayService;
        private readonly IClassroomService _classroomService;
        private readonly IClassStudentService _classStudentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEvaluationService _evaluationService;
        private readonly IWeeklyAttendanceReportService _weeklyAttendanceReportService;

        public StudentAssistanceController(
            AkdemicContext context,
            IUserService userService,
            ITermService termService,
            IClassService classService,
            ISectionService sectionService,
            IHolidayService holidayService,
            IClassroomService classroomService,
            IHttpContextAccessor httpContextAccessor,
            IEvaluationService evaluationService,
            IWeeklyAttendanceReportService weeklyAttendanceReportService,
            IClassStudentService classStudentService) : base(userService, termService)
        {
            _context = context;
            _classService = classService;
            _sectionService = sectionService;
            _holidayService = holidayService;
            _classroomService = classroomService;
            _classStudentService = classStudentService;
            _httpContextAccessor = httpContextAccessor;
            _evaluationService = evaluationService;
            _weeklyAttendanceReportService = weeklyAttendanceReportService;
        }

        /// <summary>
        /// Vista donde se toma la asistencia de la clase actual
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var sections = await _sectionService.GetSectionByClassRange(userId, DateTime.UtcNow, DateTime.UtcNow);

            if (sections == null || !sections.Any())
            {
                var nextC = (await _classService.GetAll(null, null, userId, null, DateTime.UtcNow)).OrderBy(x => x.StartTime).FirstOrDefault();

                var msg = "No se encuentra en un horario de clase para la toma de asistencia.";
                var subMsg = "";
                if (nextC != null)
                {
                    var timediff = nextC.StartTime - DateTime.UtcNow;
                    if (timediff.Days == 0)
                    {
                        if (timediff.Hours < 1) subMsg = $"{timediff.Minutes:##} {(timediff.Minutes > 1 ? "minutos" : "minuto")}";
                        else
                            subMsg = $"{timediff.Hours:##} {(timediff.Hours > 1 ? "horas" : "hora")} {(timediff.Minutes != 0 ? $"y {timediff.Minutes:##} {(timediff.Minutes > 1.00 ? "minutos" : "minuto")}" : "")}";
                    }
                    else
                    {
                        subMsg = $"{timediff.Days:##} {(timediff.Days > 1 ? "días" : "día")}";
                    }

                    msg += $" Su siguiente clase será en {subMsg}. Fecha: {nextC.StartTime.ToLocalDateTimeFormat()}.";
                }
                return View("Error", msg);
            }

            var model = sections
                .Select(x => new SectionViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Course = new CourseViewModel
                    {
                        Id = x.CourseTerm.Course.Id,
                        Code = x.CourseTerm.Course.Code,
                        Name = x.CourseTerm.Course.Name
                    }
                })
                .ToList();

            return View(model);
        }

        [HttpGet("get-toma-asistencia")]
        public async Task<IActionResult> TakeAttendancePartialView(Guid sectionId)
        {
            var section = await _sectionService.Get(sectionId);
            var userId = GetUserId();

            var model = new IndexViewModel();

            var classInRange = await _context.Classes
                .Where(x => x.SectionId == sectionId && x.ClassSchedule.TeacherSchedules.Any(y => y.TeacherId == userId) && x.StartTime < DateTime.UtcNow && DateTime.UtcNow < x.EndTime)
                .Include(x => x.ClassSchedule.Section.CourseTerm.Course)
                .FirstOrDefaultAsync();

            if (classInRange is null)
            {
                model.ErrorMessage = "La sección no tiene una clase en este horario";

                return PartialView(model);
            }

            var evaluations = await _evaluationService.GetEvaluationsByClass(classInRange.Id);

            model = new IndexViewModel()
            {
                Class = new ClassViewModel()
                {
                    Id = classInRange.Id,
                    WeekNumber = classInRange.WeekNumber,
                    ClassNumber = classInRange.ClassNumber,
                    ActivityId = classInRange.UnitActivityId,
                    VirtualClassId = classInRange.VirtualClassId,
                    SessionType = ConstantHelpers.SESSION_TYPE.VALUES[classInRange.ClassSchedule.SessionType],
                    Date = classInRange.StartTime.ToLocalDateFormat(),
                    EndTime = classInRange.EndTime.ToLocalDateTimeFormat(),
                    StartTime = classInRange.StartTime.ToLocalDateTimeFormat(),
                    Commentary = classInRange.Commentary,
                    Section = new SectionViewModel()
                    {
                        Id = section.Id,
                        Code = classInRange.ClassSchedule.Section.Code,
                        Course = new CourseViewModel()
                        {
                            Id = classInRange.ClassSchedule.Section.CourseTerm.CourseId,
                            Code = classInRange.ClassSchedule.Section.CourseTerm.Course.Code,
                            Name = classInRange.ClassSchedule.Section.CourseTerm.Course.Name
                        }
                    },
                    Evaluations = evaluations.Select(x => new EvaluationViewModel
                    {
                        EvaluationId = x.Id,
                        Description = x.Description,
                        Name = x.Name,
                        Percentage = x.Percentage,
                        Taken = classInRange.EvaluationId == x.Id
                    })
                   .ToList()
                }
            };

            return PartialView(model);
        }

        /// <summary>
        /// Vista donde se toma asistencia de una clase
        /// </summary>
        /// <param name="cid">Identificador de la clase</param>
        /// <returns>Vista</returns>
        [HttpGet("{cid}")]
        public async Task<IActionResult> Index2(Guid cid)
        {
            Class c = await _classService.GetWithTeacherSchedules(cid);

            IndexViewModel model = new IndexViewModel()
            {
                Class = new ClassViewModel()
                {
                    Id = c.Id,
                    WeekNumber = c.WeekNumber,
                    ClassNumber = c.ClassNumber,
                    ActivityId = c.UnitActivityId,
                    VirtualClassId = c.VirtualClassId,
                    Date = c.StartTime.ToLocalDateFormat(),
                    EndTime = c.EndTime.ToLocalDateTimeFormat(),
                    StartTime = c.StartTime.ToLocalDateTimeFormat(),
                    Section = new SectionViewModel()
                    {
                        Code = c.ClassSchedule.Section.Code,
                        Course = new CourseViewModel()
                        {
                            Id = c.ClassSchedule.Section.CourseTerm.CourseId,
                            Code = c.ClassSchedule.Section.CourseTerm.Course.Code,
                            Name = c.ClassSchedule.Section.CourseTerm.Course.Name
                        }
                    }
                }
            };
            return View("Index", model);
        }

        /// <summary>
        /// Obtiene el listado de asistencias para poder tomar sus asistencia
        /// </summary>
        /// <param name="classId">Identificador de la clase</param>
        /// <returns>Listado de alumnos</returns>
        [Route("get")]
        public async Task<IActionResult> GetStudentAssistance(Guid? classId)
        {
            var userId = GetUserId();
            var result = await _classStudentService.GetStudentAssistances(null, userId, classId);

            return Ok(result);
        }

        /// <summary>
        /// Método para registrar la asistencia de los alumnos
        /// </summary>
        /// <param name="classId">Identificador de la clase</param>
        /// <param name="assists">Objeto que contiene los datos de los alumnos</param>
        /// <returns>Código de estado HTTP</returns>
        [Route("registrar/post")]
        [HttpPost]
        public async Task<IActionResult> PostStudentAssistance(Guid classId, AssistanceViewModel assists)
        {
            if (classId.Equals(Guid.Empty))
            {
                return BadRequest($"No se pudo encontrar la clase con el Id {classId}.");
            }

            string userId = GetUserId();

            Class c = await _classService.Get(classId);

            var classStudents = await _context.ClassStudents.Where(x => x.ClassId == classId).ToListAsync();

            if (c is null)
            {
                return BadRequest($"No se pudo encontrar la clase.");
            }

            Classroom classroom = await _classroomService.Get(c.ClassroomId);

            if (classroom is null)
            {
                return BadRequest($"No se pudo encontrar el aula.");
            }

            if (assists.Evaluations != null && assists.Evaluations.Count(x => x.Taken) > 1)
            {
                return BadRequest("Solo es posible seleccionar una evaluación para asignar a la clase.");
            }

            Section section = await _sectionService.GetWithTeacherSections(c.ClassSchedule.SectionId);

            if (c == null)
            {
                return BadRequest($"No se pudo encontrar la clase con el Id {classId}.");
            }

            if (section.TeacherSections.All(ts => ts.TeacherId != userId))
            {
                return BadRequest("No está autorizado para esta sección.");
            }

            if (assists.List != null)
            {
                foreach (AssistanceDetailViewModel a in assists.List.Where(x => !x.DPI))
                {
                    if (classStudents.Any(y => y.StudentId == a.StudentId))
                    {
                        var classStudent = classStudents.Where(y => y.StudentId == a.StudentId).FirstOrDefault();
                        classStudent.IsAbsent = a.IsAbsent;
                    }
                    else
                    {
                        var classStudent = new ClassStudent
                        {
                            ClassId = classId,
                            IsAbsent = a.IsAbsent,
                            StudentId = a.StudentId
                        };

                        await _classStudentService.Insert(classStudent);
                    }
                }
            }

            c.VirtualClassId = assists.VirtualClassId;
            c.UnitActivityId = assists.ActivityId;
            c.IsDictated = true;
            c.DictatedDate = DateTime.UtcNow;
            c.Commentary = assists.Commentary;
            c.TeacherId = userId;

            if (assists.Evaluations != null && assists.Evaluations.Any(x => x.Taken))
            {
                c.EvaluationId = assists.Evaluations.Where(x => x.Taken).Select(x => x.EvaluationId).FirstOrDefault();
            }
            else
            {
                c.EvaluationId = null;
            }

            await _classService.Update(c);
            await _weeklyAttendanceReportService.SaveWeeklyAttendanceReport(section.Id, c.WeekNumber);
            await _classStudentService.AssignDPI(section.Id);
            return Ok();
        }
    }
}
