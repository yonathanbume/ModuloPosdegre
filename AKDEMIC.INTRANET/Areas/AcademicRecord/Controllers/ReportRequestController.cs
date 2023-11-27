using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.ACADEMIC_RECORD + "," + ConstantHelpers.ROLES.REPORT_QUERIES)]
    [Area("AcademicRecord")]
    [Route("registrosacademicos/reporte-solicitudes")]
    public class ReportRequestController : BaseController
    {
        private readonly IRecordHistoryService _recordHistoryService;
        private readonly IInternalProcedureService _internalProcedureService;
        private readonly IDataTablesService _dataTablesService;

        public ReportRequestController(IRecordHistoryService recordHistoryService,
            IInternalProcedureService internalProcedureService,
            IDataTablesService dataTablesService)
        {
            _recordHistoryService = recordHistoryService;
            _internalProcedureService = internalProcedureService;
            _dataTablesService = dataTablesService;
        }


        /// <summary>
        /// Vista de reporte de solicitudes por tipo y estado
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("por-tipo-estado")]
        public IActionResult ReportByTypeAndStatus()
            => View();

        /// <summary>
        /// Obtiene el reporte de solicitudes por tipo filtradas por año
        /// </summary>
        /// <param name="year">Año</param>
        /// <returns>Reporte de solicitudes</returns>
        [HttpGet("por-tipo-chart")]
        public async Task<IActionResult> ReportByTypeChart(int year)
            => Ok(await _recordHistoryService.GetReportQuantityByYearToAcademicRecord(year));

        /// <summary>
        /// Obtiene el reporte de solicitudes por estado filtradas por año
        /// </summary>
        /// <param name="year">Año</param>
        /// <returns>Reporte de solicitudes</returns>
        [HttpGet("por-estado-chart")]
        public async Task<IActionResult> ReportByStatusChart(int year)
            => Ok(await _recordHistoryService.GetReportStatusByYearToAcademicRecord(year));

        /// <summary>
        /// Vista del reporte de solicitudes finalizadas vs pendientes
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("finalizados-pendientes")]
        public IActionResult ReportFinishedPending()
            => View();

        /// <summary>
        /// Obtiene el reporte de solicitudes finalizadas vs pendientes filtradas por mes
        /// </summary>
        /// <param name="month">Mes</param>
        /// <returns>Reporte de solicitudes</returns>
        [HttpGet("finalizados-pendientes-chart")]
        public async Task<IActionResult> ReportFinishedPendingChart(int month)
            => Ok(await _recordHistoryService.GetReportFinishedVsPendingByMonth(month));

        /// <summary>
        /// Vista del reporte de solicitudes por escuela profesional
        /// </summary>
        /// <returns>Vista repote</returns>
        [HttpGet("por-escuela")]
        public IActionResult ReportByCareer()
            => View();

        /// <summary>
        /// Obtiene el listado de solicitudes agrupadas por escuela profesional para ser usado en tablas
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="start">Fecha de inicio</param>
        /// <param name="end">Fecfha fin</param>
        /// <returns>Listado de solicitudes</returns>
        [HttpGet("por-escuela-datatable")]
        public async Task<IActionResult> ReportByCareerDatatable(Guid? careerId, string start, string end)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var data = await _internalProcedureService.GetInternalProceduresToAcademicRecordByUserDatatable(parameters, User, null, careerId, start, end);
            return Ok(data);
        }

        /// <summary>
        /// Vista del reporte de solicitudes agrupadas por usuarios asignado
        /// </summary>
        /// <returns>Vista reporte</returns>
        [HttpGet("por-usuario-encargado")]
        public IActionResult ReportByAcademicRecord()
            => View();

        /// <summary>
        /// Obtiene el reporte de solicitudes agrupadas por usuario asignado
        /// </summary>
        /// <returns>Reporte de solicitudes</returns>
        [HttpGet("por-usuario-encargado-chart")]
        public async Task<IActionResult> ReportByAcademicRecordChart()
        {
            var result = await _recordHistoryService.GetReportByAcademicRecordChart();
            return Ok(result);
        }

        /// <summary>
        /// Vista reporte de trámties observados
        /// </summary>
        /// <returns></returns>
        [HttpGet("tramites-observados")]
        public IActionResult ReportObserved()
            => View();

        /// <summary>
        /// Obtiene el listado de trámites observados para ser usado en tablas
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="type">Tipo</param>
        /// <param name="start">Fecha de inicio</param>
        /// <param name="end">Fecha fin</param>
        /// <returns>Listado de trámites</returns>
        [HttpGet("tramites-observados-datatable")]
        public async Task<IActionResult> ReportObservedDatatable(Guid? careerId, int? type, string start, string end)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var data = await _internalProcedureService.GetInternalProceduresToAcademicRecordByUserDatatable(parameters, User, null, careerId, start, end, type, true);
            return Ok(data);
        }

    }
}
