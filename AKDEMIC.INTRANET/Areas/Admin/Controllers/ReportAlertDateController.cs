using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.INSTITUTIONAL_WELFARE)]
    [Area("Admin")]
    [Route("admin/reporte_alertas_fechas")]
    public class ReportAlertDateController : BaseController
    {
        private readonly IInstitutionalAlertService _institutionalAlertService;

        public ReportAlertDateController(IInstitutionalAlertService institutionalAlertService)
        {
            _institutionalAlertService = institutionalAlertService;
        }
        
        /// <summary>
        /// Vista donde se muestra el report de alertas por fechas
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene la cantidad de alertas agrupadas por dependencia y limitadas por fechas
        /// </summary>
        /// <param name="StartDate">Fecha de inicio</param>
        /// <param name="EndDate">Fecha fin</param>
        /// <returns>Objeto que contiene los datos para el reporte</returns>
        [HttpGet("chart")]
        public async Task<IActionResult> GetChartReport(string StartDate, string EndDate)
        {
            try
            {
                if (string.IsNullOrEmpty(StartDate) || string.IsNullOrEmpty(EndDate))
                    return BadRequest("Ingresar fechas");

                var startDatetime = CORE.Helpers.ConvertHelpers.DatepickerToUtcDateTime(StartDate); 
                var endDatetime = CORE.Helpers.ConvertHelpers.DatepickerToUtcDateTime(EndDate);
                var result = await _institutionalAlertService.GetInstitutionalAlertGroupByDependency(startDatetime, endDatetime);
                return Ok(result);
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}
