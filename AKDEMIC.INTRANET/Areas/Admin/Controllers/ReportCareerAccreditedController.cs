using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.REPORT_QUERIES)]
    [Area("Admin")]
    [Route("admin/reporte-escuelas-acreditadas")]
    public class ReportCareerAccreditedController : BaseController
    {
        private readonly ICareerAccreditationService _careerAccreditationService;

        public ReportCareerAccreditedController(
            ICareerAccreditationService careerAccreditationService,
            IDataTablesService dataTablesService
        ) : base(dataTablesService)
        {
            _careerAccreditationService = careerAccreditationService;
        }

        /// <summary>
        /// Vista donde se muestra el reporte de acreditaciones por escuela profesional
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de acreditaciones para ser usado en tablas
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns>Objeto que contiene el listado de acreditaciones</returns>
        [HttpGet("datatable/get")]
        public async Task<IActionResult> CareerAccreditedDatatable(Guid? careerId = null)
        {
            //Reporte de escuelas acreditadas hasta la fecha
            var today = DateTime.UtcNow.AddHours(-5);
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _careerAccreditationService.GetCareerAccreditationDatatable(sentParameters, careerId, today);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene la cantidad de acreditaciones por escuela profesional
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns>Objeto que contiene los datos para el reporte</returns>
        [HttpGet("chart/get")]
        public async Task<IActionResult> CareerAccreditedChart(Guid? careerId = null)
        {
            var today = DateTime.UtcNow.AddHours(-5);
            var result = await _careerAccreditationService.GetCareerAccreditationChart(careerId, today);

            return Ok(result);
        }
    }
}
