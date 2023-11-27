using System;
using System.Threading.Tasks;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.INSTITUTIONAL_WELFARE)]
    [Area("Admin")]
    [Route("admin/reporte_alertas_atencion")]
    public class ReportAlertAtendedController : BaseController
    {
        private readonly IInstitutionalAlertService _institutionalAlertService;

        public ReportAlertAtendedController(IInstitutionalAlertService institutionalAlertService) : base()
        {
            _institutionalAlertService = institutionalAlertService;
        }

        /// <summary>
        /// Vista reporte de atención de alertas
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene la cantidad de alertas agrupadas por dependencia
        /// </summary>
        /// <param name="did">Identificador de la dependencia</param>
        /// <returns>Objeto que contiene los datos para el reporte</returns>
        [HttpGet("chart/{did}")]
        public async Task<IActionResult> GetChartReport(Guid did)
        {
            var result = await _institutionalAlertService.GetInstitutionalAlertsCountGroupByStatus(did);
            return Ok(result);
        }

    }
}
