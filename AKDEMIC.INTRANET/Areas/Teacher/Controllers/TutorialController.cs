using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Teacher.Models.TutorialViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN)]
    [Area("Teacher")]
    [Route("profesor/tutorias")]
    public class TutorialController : BaseController
    {
        private readonly ITutorialService _tutorialService;
        private readonly ITutorialStudentService _tutorialStudentService;
        private readonly IClassService _classeService;
        public TutorialController(UserManager<ApplicationUser> userManager,
            ITutorialService tutorialService,
            ITutorialStudentService tutorialStudentService,
            IClassService classeService) : base(userManager)
        {
            _tutorialService = tutorialService;
            _tutorialStudentService = tutorialStudentService;
            _classeService = classeService;
        }

        /// <summary>
        /// Vista donde se programan las tutorías
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de tutorías registradas
        /// </summary>
        /// <param name="start">Fecha de inicio</param>
        /// <param name="end">Fecha fin</param>
        /// <returns>Listado de tutorías</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetTutorials(DateTime start, DateTime end)
        {
            var userId = _userManager.GetUserId(User);
            var dateStart = start.ToUniversalTime();
            var dateEnd = end.ToUniversalTime();
            var result = await _tutorialService.GetTutorials(dateStart, dateEnd, userId);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene los datos de tutoría
        /// </summary>
        /// <param name="id">Identificador de la tutoría</param>
        /// <returns>Datos de la tutoría</returns>
        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetTutorial(Guid id)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _tutorialService.GetTutorialByIdAndUserId(id, userId);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de alumnos asignados a la tutoría
        /// </summary>
        /// <param name="id">Identificador de la tutoría</param>
        /// <returns>Listado de alumnos</returns>
        [HttpGet("{id}/alumnos/get")]
        public async Task<IActionResult> GetStudents(Guid id)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _tutorialStudentService.GetStudents(id, userId);
            return Ok(result);
        }

        /// <summary>
        /// Mëtodo para crear una tutoría
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de nueva tutoría</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("crear/post")]
        public async Task<IActionResult> CreateTutorial([Bind(Prefix = "Add")] TutorialViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var tutorial = await _tutorialService.AddAsync();
            tutorial.TeacherId = _userManager.GetUserId(User);
            tutorial.ClassroomId = model.ClassroomId;
            tutorial.SectionId = model.SectionId;
            return await SaveTutorial(model, tutorial);
        }

        /// <summary>
        /// Método para editar una tutoría
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados de la tutoría</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar/post")]
        public async Task<IActionResult> EditTutorial([Bind(Prefix = "Edit")] TutorialViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = _userManager.GetUserId(User);
            var tutorial = await _tutorialService.GetTutorialByIdAndUserIdEdit(model.Id.Value, userId);
            if (tutorial == null)
                return BadRequest($"No se pudo encontrar la tutoría con id '{model.Id}'.");
            if (tutorial.StartTime < DateTime.UtcNow)
                return BadRequest("No se puede modificar una tutoría pasada.");
            tutorial.ClassroomId = model.ClassroomId;
            tutorial.SectionId = model.SectionId;
            return await SaveTutorial(model, tutorial);
        }

        /// <summary>
        /// Método para guardar la tutoría
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la tutoría</param>
        /// <param name="tutorial">Entidad tutoría</param>
        /// <returns>Código de estado HTTP</returns>
        private async Task<IActionResult> SaveTutorial(TutorialViewModel model, Tutorial tutorial)
        {
            tutorial.StartTime = ConvertHelpers.DatetimepickerToUtcDateTime($"{model.Date} {model.Start}");
            tutorial.EndTime = ConvertHelpers.DatetimepickerToUtcDateTime($"{model.Date} {model.End}");

            // Hard Validation
            var localStartTime = tutorial.StartTime.ToDefaultTimeZone();
            var localEndTime = tutorial.EndTime.ToDefaultTimeZone();
            var totalStartMins = localStartTime.Minute + localStartTime.Hour * 60;
            var totalEndMins = localEndTime.Minute + localEndTime.Hour * 60;
            if ((totalStartMins > 0 && totalStartMins < 420) || (totalEndMins > 0 && totalEndMins < 420))
                return BadRequest("Horas inválidas");

            if (tutorial.EndTime.ToDefaultTimeZone().Hour == 0)
                tutorial.EndTime = tutorial.EndTime.AddDays(1);
            if (tutorial.StartTime.ToDefaultTimeZone().Hour == 0)
                tutorial.StartTime = tutorial.StartTime.AddDays(1);

            if (tutorial.EndTime <= tutorial.StartTime)
                return BadRequest("La hora de fin debe ser mayor a la hora de inicio.");
            if (tutorial.StartTime < DateTime.UtcNow)
                return BadRequest("La fecha y hora programada es inválida.");

            if (await _classeService.GetExistClassRoom(tutorial.Id, tutorial.ClassroomId, tutorial.StartTime, tutorial.EndTime) ||
                await _tutorialService.GetExistClassRoom(tutorial.Id, tutorial.ClassroomId, tutorial.StartTime, tutorial.EndTime))
                return BadRequest("El Aula seleccionada se encuentra ocupada en el rango de horas seleccionado.");

            var conflictedClass = await _classeService.GetConflictedClass(tutorial.Id, tutorial.TeacherId, tutorial.StartTime, tutorial.EndTime);

            var conflictedTutorial = await _tutorialService.GetConflictedTutorial(tutorial.Id, tutorial.TeacherId, tutorial.StartTime, tutorial.EndTime);

            if (conflictedClass != null)
                return BadRequest($"Ya cuenta con una clase programada que se cruza con el horario seleccionado. Sección {conflictedClass.ClassSchedule.Section.Code} del curso {conflictedClass.ClassSchedule.Section.CourseTerm.Course.FullName}.");
            if (conflictedTutorial != null)
                return BadRequest($"Ya cuenta con una tutoría programada que se cruza con el horario seleccionado. Sección {conflictedTutorial.Section.Code} del curso {conflictedTutorial.Section.CourseTerm.Course.FullName}.");
            await SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Método para editar el rango horario de la tutoría
        /// </summary>
        /// <param name="id">Identificador de la tutoría</param>
        /// <param name="date">Fecha </param>
        /// <param name="start">Hora inicio</param>
        /// <param name="end">Hora fin</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editartiempo/post")]
        public async Task<IActionResult> EditTimeTutorial(Guid id, string date, string start, string end)
        {
            if (id == Guid.Empty)
                return BadRequest($"No se pudo encontrar una tutoría con el Id '{id}'.");
            var tutorial = await _tutorialService.GetTutorialByIdAndUserIdEdit(id, _userManager.GetUserId(User));
            if (tutorial == null)
                return BadRequest($"No se pudo encontrar la tutoría con id '{id}'.");
            if (tutorial.StartTime < DateTime.UtcNow)
                return BadRequest("No se puede modificar una tutoría pasada.");
            return await SaveTutorial(new TutorialViewModel { Date = date, End = end, Start = start }, tutorial);
        }

        /// <summary>
        /// Método para eliminar una tutoría
        /// </summary>
        /// <param name="id">Identificador de la tutoría</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar/post")]
        public async Task<IActionResult> DeleteTutorial(Guid id)
        {
            var tutorial = await _tutorialService.GetWithDataById(id);
            if (tutorial.Section.CourseTerm.Term.Status.Equals(ConstantHelpers.TERM_STATES.FINISHED))
                return BadRequest("No puede eliminar una Tutoría para un Periodo Académico finalizado.");
            if (tutorial.StartTime < DateTime.UtcNow)
                return BadRequest("No puede eliminar una Tutoría pasada.");
            _tutorialStudentService.RemoveRange(tutorial.TutorialStudents);
            _tutorialService.Remove(tutorial);
            await SaveChangesAsync();
            return Ok();
        }
    }
}
