using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = 
        ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        ConstantHelpers.ROLES.REPORT_QUERIES)]
    [Area("Admin")]
    [Route("admin/reporte_aprobados_desaprobados")]
    public class ReportApprovedController : BaseController
    {
        private readonly ICareerService _careerService;
        private readonly IAcademicSummariesService _academicSummariesService;

        public ReportApprovedController(ICareerService careerService, IDataTablesService dataTablesService,
            IAcademicSummariesService academicSummariesService) : base(dataTablesService)
        {
            _careerService = careerService;
            _academicSummariesService = academicSummariesService;
        }

        /// <summary>
        /// Vista donde se muestra el reporte de alumnos aprobados vs desaprobados
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales
        /// </summary>
        /// <returns>Listado de escuelas profesionales</returns>
        [HttpGet("getCarreras")]
        public async Task<IActionResult> GetCareers()
        {
            var result = await _careerService.GetCareerSelect2ClientSide();
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de estudiantes y su estado (aprobado o desaprobado) para ser usado en tablas
        /// </summary>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <param name="name">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de estudiantes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetStatusStudent(Guid? cid, Guid pid, string name)
        {
            if (cid == Guid.Empty) cid = null;
            var paramters = _dataTablesService.GetSentParameters();
            var result = await _academicSummariesService.GetAcademicSummaryDatatable(paramters, cid, pid, name);
            return Ok(result);
        }

        [HttpGet("get-excel")]
        public async Task<IActionResult> GetStatusStudentExcel(Guid? careerId, Guid? termId)
        {
            var result = await _academicSummariesService.GetAcademicSummaryTemplate(careerId, termId);

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Usuario");
            dt.Columns.Add("Nombre Completo");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Programa Académico");
            dt.Columns.Add("Nota Final");
            dt.Columns.Add("Estado");

            foreach (var item in result)
                dt.Rows.Add(item.Code, item.Name, item.Career, item.AcademicProgram, item.Finalgrade, (item.Approbed ? "Aprobado" : "Desaprobado"));

            dt.AcceptChanges();

            string fileName = $"Listado de estudiantes.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Listado de estudiantes", null);

                ws.Rows().AdjustToContents();
                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        /// <summary>
        /// Obtiene la cantidad de alumnos aprobados vs desaprobados
        /// </summary>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("chart/{cid}/{pid}")]
        public async Task<IActionResult> GetChartReport(Guid? cid, Guid pid)
        {
            if (cid == Guid.Empty) cid = null;
            var result = await _academicSummariesService.GetAcademicSummariesReportAsData(cid, pid);
            return Ok(result);
        }

    }
}
