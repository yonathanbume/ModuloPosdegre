using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Areas.Report.ViewModels.GraduatedStudentViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using ClosedXML.Excel;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.INTRANET_ADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_RECORD)]
    [Area("Report")]
    [Route("reporte/estudiantes-egresados")]
    public class GraduatedStudentController : BaseController
    {
        private readonly AkdemicContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConverter _converter;
        private readonly IViewRenderService _viewRenderService;
        private readonly IStudentService _studentService;

        public GraduatedStudentController(IDataTablesService dataTablesService,
            AkdemicContext context,
            IWebHostEnvironment webHostEnvironment,
            IConverter converter,
            IViewRenderService viewRenderService,
            IStudentService studentService) : base(dataTablesService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _converter = converter;
            _viewRenderService = viewRenderService;
            _studentService = studentService;
        }

        /// <summary>
        /// Vista donde se muestra el listado de estudiantes egresados
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de estudiantes egresados
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de estudiantes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetStudents(Guid termId, Guid? careerId, string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetGraduatedsDataTable(sentParameters, User, null, termId, careerId, null, search);
            return Ok(result);
        }

        [HttpGet("excel")]
        public async Task<IActionResult> GetExcelReport(Guid termId, Guid? careerId)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();

            var query = _context.Students
                  .Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED)
                  .OrderBy(x => x.User.FullName)
                  .AsNoTracking();

            query = query.Where(x => x.GraduationTermId == termId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId.Value);

            var result = await query
                .Select(x => new
                {
                    x.User.UserName,
                    x.User.Dni,
                    Career = (x.Career == null ? "" : x.Career.Name),
                    FullName = x.User.FullName,
                    AdmissionTerm = x.AdmissionTerm == null ? "" : x.AdmissionTerm.Name,
                    GraduationTerm = x.GraduationTerm == null ? "" : x.GraduationTerm.Name,
                    WeightedAverageGrade = x.AcademicSummaries.Any()
                    ? x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number).Select(y => y.WeightedAverageCumulative).FirstOrDefault().ToString("0.00")
                    : "-",
                })
                .ToListAsync();

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Usuario");
            dt.Columns.Add("Nombre Completo");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Periodo de Admisión");
            dt.Columns.Add("Periodo de Egreso");
            dt.Columns.Add("Nota Promedio");

            foreach (var item in result)
                dt.Rows.Add(
                    item.UserName,
                    item.FullName,
                    item.Career,
                    item.AdmissionTerm,
                    item.GraduationTerm,
                    item.WeightedAverageGrade
                    );

            dt.AcceptChanges();

            var img = Path.Combine(_webHostEnvironment.WebRootPath, @"images/themes/" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png");

            var fileName = $"Listado de Alumnos Egresados.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet($"Listado de Alumnos Egresados {term.Name}", null);
                ws.Rows().AdjustToContents();
                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdfReport(Guid termId, Guid? careerId)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();

            var query = _context.Students
                  .Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED)
                  .OrderBy(x => x.User.FullName)
                  .AsNoTracking();

            query = query.Where(x => x.GraduationTermId == termId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId.Value);

            var result = await query
                .Select(x => new
                {
                    x.User.UserName,
                    x.User.Dni,
                    Career = (x.Career == null ? "" : x.Career.Name),
                    FullName = x.User.FullName,
                    AdmissionTerm = x.AdmissionTerm == null ? "" : x.AdmissionTerm.Name,
                    GraduationTerm = x.GraduationTerm == null ? "" : x.GraduationTerm.Name,
                    WeightedAverageGrade = x.AcademicSummaries.Any()
                    ? x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number).Select(y => y.WeightedAverageCumulative).FirstOrDefault().ToString("0.00")
                    : "-",
                })
                .ToListAsync();

            var model = new ReportPdfViewModel
            {
                Term = term.Name,
                Logo = Path.Combine(_webHostEnvironment.WebRootPath, $@"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-report.png"),
                Students = result.Select(x=> new GraduatedStudentViewModel
                {
                    AdmissionTerm = x.AdmissionTerm,
                    AverageGrade = x.WeightedAverageGrade,
                    Career = x.Career,
                    FullName = x.FullName,
                    GraduatedTerm = x.GraduationTerm,
                    UserName = x.UserName
                })
                .ToList()
            };

            var globalSettings2 = new DinkToPdf.GlobalSettings
            {
                DPI = 290,
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 }
            };
            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Report/Views/GraduatedStudent/PdfReport.cshtml", model);

            var objectSettings2 = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8" },
            };

            var pdf2 = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings2,
                Objects = { objectSettings2 }
            };
            var fileByte2 = _converter.Convert(pdf2);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileByte2, "application/pdf", $"Estudiantes Egresados.pdf");
        }
    }
}
