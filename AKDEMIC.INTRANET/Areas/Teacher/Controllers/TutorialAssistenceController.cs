using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Teacher.Models.TutorialStudentAssistanceViewModels;
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
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("profesor/tutoria_asistencia")]
    public class TutorialAssistenceController : BaseController
    {
        private readonly ITutorialService _tutorialService;
        private readonly ITutorialStudentService _tutorialStudentService;
        public TutorialAssistenceController(
            UserManager<ApplicationUser> userManager,
            ITutorialService tutorialService,
            ITutorialStudentService tutorialStudentService) : base(userManager)
        {
            _tutorialService = tutorialService;
            _tutorialStudentService = tutorialStudentService;
        }

        /// <summary>
        /// Vista donde se listan las tutorias asignadas al docente logueado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de tutorias asignadas al docente
        /// </summary>
        /// <param name="startDate">Fecha de inicio</param>
        /// <param name="endDate">Fecha fin</param>
        /// <returns>Listado de tutorias</returns>
        [HttpGet("tutorias_profesor/get")]
        public async Task<IActionResult> GetTutorials([FromQuery] string startDate, [FromQuery] string endDate)
        {

            if (!String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate))
            {

                var teacherId = _userManager.GetUserId(User);
                var StartDate = ConvertHelpers.DatepickerToUtcDateTime(startDate).Date;
                var EndDate = ConvertHelpers.DatepickerToUtcDateTime(endDate).Date;
                var result = await _tutorialService.GetTutorialsByDatesAndTeacherId(StartDate, EndDate, teacherId);

                return Ok(result);
            }
            else
            {
                return Ok();
            }
        }

        /// <summary>
        /// Vista donde se guardan las asistencias de los alumnos a la tutoría
        /// </summary>
        /// <param name="eid">Identificador de la tutoría</param>
        /// <returns>Vista de asistencia</returns>
        [HttpGet("asistencia/{eid}")]
        public ActionResult Assistance(Guid eid)
        {
            return View(eid);
        }

        /// <summary>
        /// Obtiene el listado de alumnos asignados a la tutoría
        /// </summary>
        /// <param name="eid">Identificador de la tutoría</param>
        /// <returns>Listado de alumnos</returns>
        [HttpGet("asistencia_alumnos/{eid}")]
        public async Task<IActionResult> GetTutorialStudent(Guid eid)
        {
            var result = await _tutorialStudentService.GetTutorialStudentByTutorialId(eid);
            return Ok(result);
        }

        /// <summary>
        /// Método para guardar la asistencia de los alumnos
        /// </summary>
        /// <param name="data">Objeto que contiene los datos de la asistencia</param>
        /// <param name="eid">Identificador de la tutoría</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("asistencia/detalle/{eid}/post")]
        public async Task<IActionResult> AssistancePost(TutorialStudentViewModel data, Guid eid)
        {
            var LstStudentTutorials = await _tutorialStudentService.GetAll();
            if (data.List != null)
            {
                foreach (var item in LstStudentTutorials)
                {
                    item.Absent = false;
                }

                foreach (var item in data.List)
                {
                    var tutorialStudent = await _tutorialStudentService.GetByTutorialIdAndStudentId(item.TutorialId, item.Id);
                    tutorialStudent.Absent = item.Absent;
                    await SaveChangesAsync();
                    tutorialStudent = null;
                }
                var assistance = await _tutorialService.Get(eid);
                assistance.IsDictated = true;
                await SaveChangesAsync();
            }
            else
            {
                var assistance = await _tutorialService.Get(eid);
                assistance.IsDictated = true;
                await SaveChangesAsync();
            }
            return Ok();
        }
    }
}
