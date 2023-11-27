using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Areas.Student.Models.AbsencesViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/inasistencias")]
    public class AbsencesController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IClassStudentService _classStudentService;
        private readonly AkdemicContext _context;

        public AbsencesController(IUserService userService,
            ITermService termService,
            IStudentSectionService studentSectionService,
            IClassStudentService classStudentService,
            AkdemicContext context,
            IStudentService studentService)
            : base(userService, termService)
        {
            _studentService = studentService;
            _studentSectionService = studentSectionService;
            _classStudentService = classStudentService;
            _context = context;
        }

        /// <summary>
        /// Vista donde se detalla las asistencias y faltas del alumno logueado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            var term = await GetActiveTerm();
            if (term == null)
                term = new Term();
            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);
            var terms = await _termService.GetAll();
            var model = new IndexViewModel()
            {
                Student = new StudentViewModel()
                {
                    FullName = student.User.FullName,
                    UserName = student.User.UserName,
                    Career = new CareerViewModel()
                    {
                        Name = student.Career.Name
                    }
                },
                ActiveTerm = term.Id,
                AttendanceMinPercentage = 100 - (int)term.AbsencePercentage,
                Terms = terms
                .OrderByDescending(x => x.Name)
                .Select(x => new TermViewModel
                {
                    Name = x.Name,
                    Id = x.Id
                }).ToList()
            };
            return View(model);
        }

        /// <summary>
        /// Obtiene los cursos matriculados del alumno logueado
        /// </summary>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene los datos de los cursos matriculados</returns>
        [Route("periodo/{pid}/get")]
        public async Task<IActionResult> GetStudentCourses(Guid pid)
        {
            if (pid.Equals(Guid.Empty))
                return BadRequest($"No se pudo encontrar un Periodo con el id {pid}.");

            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);
            var term = await _termService.Get(pid);

            var absencePercentage = term.AbsencePercentage;

            var allClassesBySubGroup = await _context.Classes
                .Where(x=>x.Section.StudentSections.Any(y=>y.StudentId == student.Id))
                .Select(x => new
                {
                    x.SectionId,
                    x.ClassSchedule.SectionGroupId
                })
                .ToListAsync();

            var allClassStudents = await _context.ClassStudents
                .Where(x=>x.StudentId == student.Id)
                .Select(x => new
                {
                    x.Class.SectionId,
                    x.ClassId,
                    x.IsAbsent,
                    x.Class.ClassSchedule.SectionGroupId
                })
                .ToListAsync();

            var model = await _context.StudentSections.Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == term.Id)
                .OrderBy(x => x.Section.CourseTerm.Course.Code)
                .ThenBy(x => x.Section.CourseTerm.Course.Name)
                .Select(x => new StudentClassReportViewModel
                {
                    SectionId = x.SectionId,
                    SectionGroupId = x.SectionGroupId,
                    CourseName = $"{x.Section.CourseTerm.Course.Code} - {x.Section.CourseTerm.Course.Name}",
                    IsActive = x.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE
                })
                .ToListAsync();

            foreach (var item in model)
            {
                var clasesBySubGroup = allClassesBySubGroup.Where(x => x.SectionId == item.SectionId)
                    .GroupBy(x => x.SectionGroupId)
                    .Select(x => new
                    {
                        x.Key,
                        count = x.Count()
                    })
                    .ToList();

                var classStudents = allClassStudents.Where(x => x.SectionId == item.SectionId)
                    .Select(x => new
                    {
                        x.ClassId,
                        x.IsAbsent,
                        x.SectionGroupId
                    })
                    .ToList();


                item.Absences = classStudents.Where(x => (!x.SectionGroupId.HasValue || x.SectionGroupId == item.SectionGroupId) && x.IsAbsent).Count();
                item.Assisted = classStudents.Where(x => (!x.SectionGroupId.HasValue || x.SectionGroupId == item.SectionGroupId) && !x.IsAbsent).Count();
                item.MaxAbsences = (int)Math.Floor((decimal)clasesBySubGroup.Where(x => !x.Key.HasValue || x.Key == item.SectionGroupId).Sum(x => x.count) * (decimal)absencePercentage / 100M);
                item.Dictated = classStudents.Where(x => (!x.SectionGroupId.HasValue || x.SectionGroupId == item.SectionGroupId)).Count();
                item.ClassCount = clasesBySubGroup.Where(x => !x.Key.HasValue || x.Key == item.SectionGroupId).Sum(x => x.count);
                item.AbsencesPercentage = item.ClassCount == 0 ? 0 : Math.Round(((decimal)item.Absences / (decimal)item.ClassCount * 100M), 1, MidpointRounding.AwayFromZero);
            }

            return Ok(model);
        }

        /// <summary>
        /// Obtiene el listado de clases de una sección donde se detalla su asistencia o falta del alumno logueado
        /// </summary>
        /// <param name="sid">identificador de la sección</param>
        /// <param name="filter">Todos, Asistencia o falta</param>
        /// <returns>Objeto que contiene el listado de clases</returns>
        [Route("seccion/{sid}/get")]
        public async Task<IActionResult> GetSectionAbsenceDetail(Guid sid, [FromQuery] int filter)
        {
            if (sid.Equals(Guid.Empty))
                return BadRequest($"No se pudo encontrar una Sección con el id {sid}.");

            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);
            var data = await _classStudentService.GetAll(sid, student.Id, null, DateTime.UtcNow, filter != -1 ? (bool?)ConstantHelpers.ASSISTANCE_STATES.INVERSE_VALUES[filter] : null);
            var result = data.OrderByDescending(x => x.Class.StartTime)
                .Select(x => new
                {
                    classId = x.Class.Id,
                    week = x.Class.WeekNumber,
                    sessionNumber = x.Class.ClassNumber,
                    date = $"{x.Class.StartTime.ToDefaultTimeZone():dd/MM/yyyy}",
                    weekDay = ConstantHelpers.WEEKDAY.VALUES[x.Class.ClassSchedule.WeekDay],
                    startTime = x.Class.StartTime.ToDefaultTimeZone().ToString(ConstantHelpers.FORMATS.TIME, System.Globalization.CultureInfo.InvariantCulture),
                    endTime = x.Class.EndTime.ToDefaultTimeZone().ToString(ConstantHelpers.FORMATS.TIME, System.Globalization.CultureInfo.InvariantCulture),
                    isAbsent = x.IsAbsent
                }).ToList();
            return Ok(result);
        }
    }
}
