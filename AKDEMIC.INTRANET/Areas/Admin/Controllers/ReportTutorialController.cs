using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/reporte_tutoria")]
    public class ReportTutorialController : BaseController
    {
        private readonly ITutorialService _tutorialService;
        private readonly ITutorialStudentService _tutorialStudentService;
        public ReportTutorialController(
            ITutorialService tutorialService,
            ITutorialStudentService tutorialStudentService) : base()
        {
            _tutorialService = tutorialService;
            _tutorialStudentService = tutorialStudentService;
        }

        /// <summary>
        /// Vista donde se muestra el reporte de tutorías
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de las tutorías programadas
        /// </summary>
        /// <returns>Objeto que contiene el listado de tutorías programadas</returns>
        [HttpGet("tutorias_programadas/get")]
        public async Task<IActionResult> GetProgramTutorials()
        {
            var result = await _tutorialService.GetProgramTutorials();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de tutorías efectudas filtradas por un rango de fecha
        /// </summary>
        /// <param name="startDate">Fecha de inicio</param>
        /// <param name="endDate">Fecha fin</param>
        /// <returns>Objeto que contiene el listado de tutorías efectuadas</returns>
        [HttpGet("tutorias_efectuadas/get")]
        public async Task<IActionResult> GetDoneTutorials([FromQuery] string startDate, [FromQuery] string endDate)
        {
            if (!String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate))
            {
                var StartDate = ConvertHelpers.DatepickerToUtcDateTime(startDate).Date;
                var EndDate = ConvertHelpers.DatepickerToUtcDateTime(endDate).Date;
                var result = await _tutorialService.GetDoneTutorialsStartAndEndDate(StartDate, EndDate);
                return Ok(result);
            }
            else
            {
                return Ok();
            }
        }

        /// <summary>
        /// Vista detalle de la tutoría
        /// </summary>
        /// <param name="eid">Identificador de la tutoría</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("tutorias_efectuadas/{eid}/detail")]
        public IActionResult DoneTutorialDetail(Guid eid)
        {
            return View(eid);
        }

        /// <summary>
        /// Obtiene el listado de alumnos asignados a la tutoría
        /// </summary>
        /// <param name="eid">Identificador de la tutoría</param>
        /// <returns>Objeto que contiene el listado de alumnos asignados</returns>
        [HttpGet("tutorias_efectuadas/{eid}/alumnos")]
        public async Task<IActionResult> StudentTutorialDone(Guid eid)
        {
            var result = await _tutorialStudentService.GetStudentTutorialDone(eid);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene la cantidad de asistencias y faltas de la tutoría
        /// </summary>
        /// <param name="eid">Identificador de la tutoría</param>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("chart/{eid}")]
        public async Task<IActionResult> GetChartReport(Guid eid)
        {
            var result = await _tutorialStudentService.GetChartReport(eid);

            return Ok(result);
        }
    }
}
