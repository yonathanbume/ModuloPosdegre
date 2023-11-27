using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/tutorias")]
    public class TutorialController : BaseController
    {
        public TutorialController(AkdemicContext context, UserManager<ApplicationUser> userManager) : base(context, userManager) { }

        /// <summary>
        /// Vista donde se muestra el horario de las tutorías inscritas y programadas
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de tutorías
        /// </summary>
        /// <param name="start">Fecha de inicio</param>
        /// <param name="end">Fecha fin</param>
        /// <param name="onlyRegistered">¿Solo inscritos?</param>
        /// <returns>Listado de tutorías</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetTutorials(DateTime start, DateTime end, bool onlyRegistered = false)
        {
            var dateStart = start.ToUniversalTime();
            var dateEnd = end.ToUniversalTime();
            var query = _context.Tutorials
                .Where(t => t.Section.StudentSections.Any(x => x.Student.UserId == _userManager.GetUserId(User)))
                .Where(t => t.StartTime >= dateStart && t.EndTime <= dateEnd).AsQueryable();
            if (onlyRegistered)
                query = query.Where(t => t.TutorialStudents.Any(ts => ts.Student.UserId == _userManager.GetUserId(User)));
            var result = await query.Select(t => new
            {
                id = t.Id,
                title = string.Format("{0}-{1} ({2})", t.Section.CourseTerm.Course.Code,
                    t.Section.CourseTerm.Course.Name, t.Section.Code),
                description = t.Classroom.Description,
                registered = t.TutorialStudents.Any(ts => ts.Student.UserId == _userManager.GetUserId(User)),
                allDay = false,
                start = t.StartTime.ToDefaultTimeZone().ToString("yyyy-MM-dd HH:mm:ss"),
                end = t.EndTime.ToDefaultTimeZone().ToString("yyyy-MM-dd HH:mm:ss")
            }).ToListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el detalle de la tutoría
        /// </summary>
        /// <param name="id">Identificador de la tutoría</param>
        /// <returns>Detalle de la tutoría</returns>
        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetTutorial(Guid id)
        {
            var result = await _context.Tutorials
                .Where(t => t.Id == id && t.Section.StudentSections.Any(x => x.Student.UserId == _userManager.GetUserId(User)))
                .Select(t => new
                {
                    id = t.Id,
                    section = $"{t.Section.CourseTerm.Course.Name} - {t.Section.Code}",
                    classroom = t.Classroom.Description,
                    date = t.StartTime.ToLocalDateFormat(),
                    start = t.StartTime.ToLocalTimeFormat(),
                    end = t.EndTime.ToLocalTimeFormat(),
                    teacher = t.Teacher.FullName,
                    registered = t.TutorialStudents.Any(ts => ts.Student.UserId == _userManager.GetUserId(User)),
                    canRegister = t.StartTime > DateTime.UtcNow
                }).FirstOrDefaultAsync();
            return Ok(result);
        }

        /// <summary>
        /// Mëtodo para inscribirse en una tutoría programada
        /// </summary>
        /// <param name="id">Identificador de la tutoría</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("inscripcion")]
        public async Task<IActionResult> Inscription(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest($"No se pudo encontrar una tutoría con el id '{id}'.");
            var tutorial = await _context.Tutorials.FindAsync(id);
            var student = await _context.Students.FirstOrDefaultAsync(x => x.UserId == _userManager.GetUserId(User));
            var tutorialStudent = new TutorialStudent
            {
                TutorialId = id,
                StudentId = student.Id
            };
            if (tutorial.StartTime < DateTime.UtcNow)
                return BadRequest("No se puede inscribir a una tutoría pasada.");
            await _context.TutorialStudents.AddAsync(tutorialStudent);
            await SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Método para anular inscripción de una tutoría
        /// </summary>
        /// <param name="id">Identificador de la tutoría</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("anular-inscripcion")]
        public async Task<IActionResult> InscriptionDisable(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest($"No se pudo encontrar una tutoría con el id '{id}'.");
            var tutorialStudent = await _context.TutorialStudents.Include(x => x.Tutorial).FirstOrDefaultAsync(x =>
                x.TutorialId == id && x.Student.UserId == _userManager.GetUserId(User));
            if (tutorialStudent == null)
                return BadRequest("No se encuentra inscrito en la tutoría.");
            if (tutorialStudent.Tutorial.StartTime < DateTime.UtcNow)
                return BadRequest("No se puede desinscribir de una tutoría pasada.");
            _context.TutorialStudents.Remove(tutorialStudent);
            await SaveChangesAsync();
            return Ok();
        }
    }
}
