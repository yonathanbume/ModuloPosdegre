using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Areas.Student.Models.ClassScheduleViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.REPOSITORY.Data;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/horario-ciclo")]
    public class ClassScheduleController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly AkdemicContext _context;
        private readonly IClassScheduleService _classScheduleService;

        public ClassScheduleController(IUserService userService,
            ITermService termService, IStudentService studentService,
            AkdemicContext context,
            IClassScheduleService classScheduleService) : base(userService, termService)
        {
            _studentService = studentService;
            _context = context;
            _classScheduleService = classScheduleService;
        }

        /// <summary>
        /// Vista donde se muestra el horario del ciclo del alumno logueado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            return View(model);
        }

        /// <summary>
        /// Método que obtiene el listado de clases del alumno logueado
        /// </summary>
        /// <param name="start">Fecha de inicio</param>
        /// <param name="end">Fecha fin</param>
        /// <returns>Listado de clases</returns>
        [Route("get")]
        public async Task<IActionResult> GetTeacherSchedules(DateTime start, DateTime end)
        {
            var term = await GetActiveTerm();
            if (term == null)
                term = new ENTITIES.Models.Enrollment.Term();

            var userId = GetUserId();
            var student = await _studentService.GetStudentByUser(userId);

            var classSchedules = await _context.ClassSchedules
                .Where(cs => cs.Section.StudentSections.Any(x => x.StudentId == student.Id && (!cs.SectionGroupId.HasValue || cs.SectionGroupId == x.SectionGroupId)) && cs.Section.CourseTerm.TermId == term.Id)
                .ToListAsync();

            var classSchedulesID = classSchedules.Select(x => x.Id).ToList();
            var resultDB = await _context.Classes.Where(x => classSchedulesID.Contains(x.ClassScheduleId))
                .Select(x => new
                {
                    id = x.ClassScheduleId,
                    title = $"{x.ClassSchedule.Section.CourseTerm.Course.Code} {x.ClassSchedule.Section.CourseTerm.Course.Name} ({x.ClassSchedule.Section.Code})",
                    description = x.ClassSchedule.Classroom.Description,
                    allDay = false,
                    x.StartTime,
                    x.EndTime
                })
                .ToListAsync();

            var result = resultDB
                .Where(x => x.StartTime.ToDefaultTimeZone().Date >= start.Date && x.EndTime.ToDefaultTimeZone().Date <= end.Date)
                .Select(x => new
                {
                    x.id,
                    x.title,
                    x.description,
                    x.allDay,
                    start = $"{x.StartTime.ToDefaultTimeZone():yyyy-MM-dd}T{x.StartTime.ToDefaultTimeZone():HH:mm:ss}",
                    end = $"{x.EndTime.ToDefaultTimeZone():yyyy-MM-dd}T{x.EndTime.ToDefaultTimeZone():HH:mm:ss}"
                })
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el detalle de la clase
        /// </summary>
        /// <param name="id">Identificador de la clase</param>
        /// <returns>Detalle de la clase</returns>
        [Route("{id}/get")]
        public async Task<IActionResult> GetTeacherSchedule(Guid id)
        {
            var term = await GetActiveTerm();
            var userId = _userService.GetUserIdByClaim(User);
            var today = (int)(DateTime.UtcNow.ToDefaultTimeZone().DayOfWeek);
            var student = await _studentService.GetStudentByUser(userId);
            var classSchedule = await _classScheduleService.GetWithTeacherSchedules(id);
            var result = new
            {
                classroom = classSchedule.Classroom.Description,
                teachers = classSchedule.TeacherSchedules.Select(ts => ts.Teacher.User.FullName),
                day = ConstantHelpers.WEEKDAY.VALUES[classSchedule.WeekDay],
                start = classSchedule.StartTime.ToLocalDateTimeFormatUtc(),
                end = classSchedule.EndTime.ToLocalDateTimeFormatUtc(),
                course = classSchedule.Section.CourseTerm.Course.FullName,
                section = classSchedule.Section.Code,
                type = ConstantHelpers.SESSION_TYPE.VALUES[classSchedule.SessionType]
            };
            return Ok(result);
        }
    }
}
