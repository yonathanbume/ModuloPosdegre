using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Doctor.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.INSTITUTIONAL_WELFARE)]
    [Area("Admin")]
    [Route("admin/reporte_alerta_dependencias")]
    public class ReportAlertDependencyController : BaseController
    {
        private readonly IInstitutionalAlertService _institutionalAlertService;

        public ReportAlertDependencyController(IInstitutionalAlertService institutionalAlertService)
        {
            _institutionalAlertService = institutionalAlertService;
        }

        /// <summary>
        /// Vista reporte de atención de alertas por dependencia
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene la cantidad de alertas agrupadas por dependencia
        /// </summary>
        /// <returns>Objeto que contiene los datos para el reporte</returns>
        [HttpGet("chart")]
        public async Task<IActionResult> GetChartReport()
        {
            var result = await _institutionalAlertService.GetInstitutionalAlertGroupByDependency();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene la cantidad de alertas agrupadas por dependencia
        /// </summary>
        /// <param name="pid">Identificador de la dependencia</param>
        /// <returns>Objeto que contiene los datos para el reporte</returns>
        [HttpGet("{pid}/get")]
        public async Task<IActionResult> GetAlersbytDependency(Guid pid)
        {
            var institutionAlerts = await _institutionalAlertService.GetInstitutionalAlertsByDependecyId(pid);
            var result = institutionAlerts.Select(x => new
            {
                type = x.Type,
                description = x.Description,
                registerdate = x.RegisterDate.ToShortDateString(),
                status = x.Status

            }).ToList();

            return Ok(result);
        }
    }
}
