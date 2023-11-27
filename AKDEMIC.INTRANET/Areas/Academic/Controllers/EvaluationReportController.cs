using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Areas.Academic.ViewModels.EvaluationReportViewModel;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using ClosedXML.Excel;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Academic.Controllers
{
    [Area("Academic")]
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.ACADEMIC_RECORD + "," + ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF)]
    [Route("academico/recepcion-ficha")]
    public class EvaluationReportController : BaseController
    {
        private readonly IEvaluationReportService _evaluationReportService;
        private readonly IViewRenderService _viewRenderService;
        private readonly IConverter _converter;
        private readonly ITermService _termService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EvaluationReportController(
            IEvaluationReportService evaluationReportService,
            IDataTablesService dataTablesService,
            IViewRenderService viewRenderService,
            IConverter converter,
            ITermService termService,
            IWebHostEnvironment hostingEnvironment) : base(dataTablesService)
        {
            _evaluationReportService = evaluationReportService;
            _viewRenderService = viewRenderService;
            _converter = converter;
            _termService = termService;
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// Retorna la vista incial de la recepción de actas emitidas
        /// </summary>
        /// <returns></returns>
        public IActionResult Index() => View();
        /// <summary>
        /// Retorna la lista completa de actas emitidas filtradas por el usuario
        /// </summary>
        /// <param name="faculty">Id de la facultad</param>
        /// <param name="school">Id de la escuela</param>
        /// <param name="career">Id de la escuela profesional</param>
        /// <param name="status">Estado del acta</param>
        /// <param name="search">Término a filtrar</param>
        /// <param name="termId">Id del periodo académico</param>
        /// <returns>Objeto con la lista de actas</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid faculty, Guid school, Guid career,byte? status, string search,Guid? termId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _evaluationReportService.GetAllEvaluationReportDatatable(sentParameters, faculty, school, career, search,User,status,termId);
            return Ok(result);
        }
        /// <summary>
        /// Método para cambiar el estado del acta a recibida, guardando la fecha y hora
        /// </summary>
        /// <param name="id">Id del acta</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("recibir")]
        public async Task<IActionResult> Receive(Guid id)
        {
            var evaluationReport = await _evaluationReportService.GetEvaluationReportById(id);

            evaluationReport.ReceptionDate = DateTime.UtcNow;
            evaluationReport.Status = ConstantHelpers.Intranet.EvaluationReport.RECEIVED;

            await _evaluationReportService.UpdateEvaluationReport(evaluationReport);

            return Ok();
        }
        /// <summary>
        /// Genera el reporte de actas emitidas en formato Excel
        /// </summary>
        /// <param name="termId">Id del periodo académico</param>
        /// <param name="status">Estado del acta</param>
        /// <returns>Documento Excel</returns>
        [HttpGet("reporte-excel")]
        public async Task<IActionResult> DownloadReportExcel(Guid termId, byte? status)
        {
            var term = await _termService.Get(termId);
            var data = await _evaluationReportService.GetEvaluationReportExcel(termId, status);
            data = data.OrderBy(x => x.Career).ThenBy(x => x.Curriculum);
            var dt = new DataTable
            {
                TableName = "Reporte de Actas Emitidas"
            };

            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Curso");
            dt.Columns.Add("Sección");
            dt.Columns.Add("Fec. Emisión");
            dt.Columns.Add("Profesor(es)");
            dt.Columns.Add("Estado");
            dt.Columns.Add("Fecha de Recepción");

            foreach (var item in data)
                dt.Rows.Add(
                    item.Career,
                    item.Course,
                    item.Section,
                    item.CreatedAt,
                    item.Teacher,
                    item.Status,
                    item.ReceptionDate);

            dt.AcceptChanges();

            //var img = Path.Combine(_hostingEnvironment.WebRootPath, $@"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-report.png");
            var fileName = $"Resumen de Actas Emitidas {term.Name}.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Listado de Actas Emitidas", null);
                ws.Rows().AdjustToContents();
                ws.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        /// <summary>
        /// Genera el reporte de actas emitidas en formato PDF
        /// </summary>
        /// <param name="termId">Id del periodo académico</param>
        /// <param name="status">Estado del acta</param>
        /// <returns>Documento PDF</returns>
        [HttpGet("reporte-pdf")]
        public async Task<IActionResult> DownloadReportPDF(Guid termId, byte? status)
        {
            var data = await _evaluationReportService.GetEvaluationReportExcel(termId, status);
            data = data.OrderBy(x => x.Career).ThenBy(x => x.Curriculum);
            var term = await _termService.Get(termId);

            var model = new EvaluationPdfViewModel
            {
                Term = term.Name,
                Img = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                Items = new List<Item>()
            };

            foreach (var item in data)
            {
                model.Items.Add(new Item
                {
                    Career = item.Career,
                    CareerId = item.CareerId,
                    Curriculum = item.Curriculum,
                    CurriculumId = item.CurriculumId,
                    Code = item.Code,
                    CreatedAt = item.CreatedAt,
                    Course = item.Course,
                    Section = item.Section,
                    LastGeneratedDate = item.LastGenerated,
                    Status = item.Status,
                    Teachers = item.Teacher,
                    ReceptionDate = item.ReceptionDate
                });
            }

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Landscape,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                DocumentTitle = "Resumen de Actas Emitidas"
            };

            var view = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/EvaluationReport/EvaluationPdf.cshtml", model);

            var mainObjectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = view,
                WebSettings = { DefaultEncoding = "utf-8" },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { mainObjectSettings }
            };

            byte[] fileByte = _converter.Convert(pdf);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf", "Resumen de Actas Emitidas.pdf");
        }
    }
}
