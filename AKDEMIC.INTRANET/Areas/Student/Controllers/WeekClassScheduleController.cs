using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.INTRANET.Areas.Student.Models.WeekClassSchedule;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/horario-semana")]
    public class WeekClassScheduleController : BaseController
    {
        private readonly IGradeRecoveryExamService _gradeRecoveryExamService;
        private readonly IStudentService _studentService;
        private readonly AkdemicContext _context;
        private readonly IClassService _classService;

        public WeekClassScheduleController(IUserService userService,
            ITermService termService,
            IGradeRecoveryExamService gradeRecoveryExamService,
            IStudentService studentService,
            AkdemicContext context,
            IClassService classService) : base(userService, termService)
        {
            _gradeRecoveryExamService = gradeRecoveryExamService;
            _studentService = studentService;
            _context = context;
            _classService = classService;
        }

        /// <summary>
        /// Vista donde se muestra el horario de la semana del alumno logueado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de clases asignadas al alumno logueado
        /// </summary>
        /// <returns>Listado de clases</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetClasses(DateTime start, DateTime end)
        {
            var term = await GetActiveTerm();
            if (term == null)
                term = new ENTITIES.Models.Enrollment.Term();
            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);

            var classSchedules = await _context.ClassSchedules
                .Where(cs => cs.Section.StudentSections.Any(x => x.StudentId == student.Id && (!cs.SectionGroupId.HasValue || cs.SectionGroupId == x.SectionGroupId)) && cs.Section.CourseTerm.TermId == term.Id)
                .ToListAsync();

            var classSchedulesID = classSchedules.Select(x => x.Id).ToList();
            var resultDB = await _context.Classes.Where(x => classSchedulesID.Contains(x.ClassScheduleId))
                .Select(x => new
                {
                    id = x.Id,
                    title = $"{x.ClassSchedule.Section.CourseTerm.Course.Code} {x.ClassSchedule.Section.CourseTerm.Course.Name} ({x.ClassSchedule.Section.Code})",
                    description = x.ClassSchedule.Classroom.Description,
                    allDay = false,
                    x.StartTime,
                    x.EndTime
                })
                .ToListAsync();

            var result = resultDB
                .Where(x => x.StartTime.ToDefaultTimeZone().Date >= start.Date && x.EndTime.ToDefaultTimeZone().Date <= end.Date)
                .Select(x => new WeekClassScheduleViewModel
                {
                    Id = x.id,
                    Title = x.title,
                    Description = x.description,
                    AllDay = x.allDay,
                    Start = $"{x.StartTime.ToDefaultTimeZone():yyyy-MM-dd}T{x.StartTime.ToDefaultTimeZone():HH:mm:ss}",
                    End = $"{x.EndTime.ToDefaultTimeZone():yyyy-MM-dd}T{x.EndTime.ToDefaultTimeZone():HH:mm:ss}"
                }).ToList();

            //Examenes de recuperacion de nota
            var gradeRecoveryExams = await _gradeRecoveryExamService.GetGradeRecoveryByStudent(student.Id, term.Id);
            if (gradeRecoveryExams.Any())
            {
                var gradeRecoveryExamsFormat = gradeRecoveryExams
                    .Select(x => new WeekClassScheduleViewModel
                    {
                        Id = x.Id,
                        Title = $"Exámen de Recuperación de Nota - {x.Section.CourseTerm.Course.FullName}",
                        Description = x.Classroom.Description,
                        AllDay = false,
                        Start = $"{x.StartTime.ToDefaultTimeZone():yyyy-MM-dd}T{x.StartTime.ToDefaultTimeZone():HH:mm:ss}",
                        End = $"{x.EndTime.ToDefaultTimeZone():yyyy-MM-dd}T{x.EndTime.ToDefaultTimeZone():HH:mm:ss}"
                    })
                    .ToList();

                result.AddRange(gradeRecoveryExamsFormat);
            }

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el detalle de la clase
        /// </summary>
        /// <param name="id">Identificador de la clase</param>
        /// <returns>Detalle de la clase</returns>
        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetClass(Guid id)
        {
            var term = await GetActiveTerm();
            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);
            var studentClass = await _classService.GetWithTeacherSchedules(id);
            var result = new
            {
                classroom = studentClass.Classroom.Description,
                teachers = studentClass.ClassSchedule.TeacherSchedules.Select(cs => cs.Teacher.User.FullName),
                date = studentClass.StartTime.ToLocalDateFormat(),
                start = studentClass.StartTime.ToLocalTimeFormat(),
                end = studentClass.EndTime.ToLocalTimeFormat(),
                course = studentClass.ClassSchedule.Section.CourseTerm.Course.FullName,
                section = studentClass.ClassSchedule.Section.Code,
                type = ConstantHelpers.SESSION_TYPE.VALUES[studentClass.ClassSchedule.SessionType]
            };
            return Ok(result);
        }
    }
}
