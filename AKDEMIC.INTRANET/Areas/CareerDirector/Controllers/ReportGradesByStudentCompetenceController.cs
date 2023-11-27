using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.GradeReportByCompetences;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using ClosedXML.Excel;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.CAREER_DIRECTOR)]
    [Area("CareerDirector")]
    [Route("director-carrera/reporte-alumnos-competencias")]
    public class ReportGradesByStudentCompetenceController : Controller
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IConverter _dinkConverter;
        private readonly IViewRenderService _viewRenderService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IAcademicSummariesService _academicSummariesService;
        private readonly AkdemicContext _context;

        public ReportGradesByStudentCompetenceController(
             IDataTablesService dataTablesService,
             IWebHostEnvironment hostingEnvironment,
             IViewRenderService viewRenderService,
             IAcademicSummariesService academicSummariesService,
             AkdemicContext context,
             IConverter dinkConverter)
        {
            _dataTablesService = dataTablesService;
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = hostingEnvironment;
            _academicSummariesService = academicSummariesService;
            _context = context;
        }

        /// <summary>
        /// Vista donde se muestra el reporte de alumnos por competencia
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Vista donde se muestra el nivel de logro por competencia y ciclo académico
        /// </summary>
        /// <returns>Vista detalle</returns>
        [HttpGet("nivel-de-logro")]
        public ActionResult AchievementLevelAcademicYear()
        {
            return View();
        }

        /// <summary>
        /// Obtiene la vista parcial del reporte de estudiantes por competencia
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="academicYear">Ciclo académico</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("get/{termId}/{facultyId}/{careerId}/{curriculumId}/{academicYear}")]
        public async Task<IActionResult> _ReportStudentByCompetences(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, byte academicYear)
        {
            var competences = await _context.Competencies.ToListAsync();
            var result = await _academicSummariesService.GetReportStudentByCompetences(termId, facultyId, careerId, curriculumId, academicYear);
            var resultFinal = new Final();

            resultFinal.List = result;
            resultFinal.ListCompetencies = competences;

            return PartialView(resultFinal);
        }

        /// <summary>
        /// Obtiene el reporte de notas por curso de los estudiantes filtrados por los siguientes parámetros
        /// </summary>
        /// <param name="termId">identificador del periodo académico</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">identificador del plan de estudio</param>
        /// <param name="competenceId">identificador de la competencia</param>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <param name="academicYear">Ciclo académico</param>
        /// <returns></returns>
        [HttpGet("obtener-datos-por-cantidad-por-estudiante/{termId}/{facultyId}/{careerId}/{curriculumId}/{competenceId}/{studentId}/{academicYear}")]
        public async Task<IActionResult> GetStudentByCompetenceDetail(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, Guid competenceId, Guid studentId, byte academicYear)
        {
            var result = await _academicSummariesService.ReportGradesByStudentCourses(termId, facultyId, careerId, curriculumId, competenceId, studentId, academicYear);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene la vista parcial donde se detalla los historiales académicos filtrados por los siguientes parámetros
        /// </summary>
        /// <param name="termId">identificador del periodo académico</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="academicYear">Ciclo académico</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("nivel-de-logro-datos/{termId}/{facultyId}/{careerId}/{curriculumId}/{academicYear}")]
        public async Task<IActionResult> _AchievementLevelAcademicYear(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, byte academicYear)
        {
            var result = await _academicSummariesService.AchievementLevelAcademicYearCompetences(termId, facultyId, careerId, curriculumId, academicYear);

            return PartialView(result.OrderBy(x => x.CompetenceName).ToList());
        }

        /// <summary>
        /// Obtiene los datos por cantidad filtrados por los siguientes parámetros
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="facultyId">identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="competenceId">Identificador de la competencia</param>
        /// <param name="type">Tipo</param>
        /// <param name="academicYear">Ciclo académico</param>
        /// <returns>Datos por cantidad</returns>
        [HttpGet("obtener-datos-por-cantidad/{termId}/{facultyId}/{careerId}/{curriculumId}/{competenceId}/{type}/{academicYear}")]
        public async Task<IActionResult> GetStudentByCompetence(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, Guid competenceId, int type, byte academicYear)
        {
            var RangeLevelList = new List<RangeLevel>();

            switch (type)
            {
                case 1:
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Deficiente",
                        Min = 0,
                        Max = 10,
                        Total = 0,
                        Type = 1
                    });
                    break;
                case 2:
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Regular",
                        Min = 11,
                        Max = 13,
                        Total = 0,
                        Type = 2
                    });
                    break;
                case 3:
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Bueno",
                        Min = 14,
                        Max = 16,
                        Total = 0,
                        Type = 3
                    });
                    break;
                case 4:
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Excelente",
                        Min = 17,
                        Max = 20,
                        Total = 0,
                        Type = 4
                    });
                    break;

            }

            var result = await _academicSummariesService.AchievementLevelAcademicYearCompetenceDetail(termId, facultyId, careerId, curriculumId, competenceId, academicYear, RangeLevelList);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene los datos por cantidad por estudiante filtrados por los siguientes parámetros
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="competenceId">Identificador de la competencia</param>
        /// <param name="type">Tipo</param>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <param name="academicYear">Ciclo académico</param>
        /// <returns>Datos por cantidad de estudiante</returns>
        [HttpGet("obtener-datos-por-cantidad-por-estudiante/{termId}/{facultyId}/{careerId}/{curriculumId}/{competenceId}/{type}/{academicYear}/{studentId}")]
        public async Task<IActionResult> GetStudentByCompetenceDetail(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, Guid competenceId, int type, byte academicYear, Guid studentId)
        {
            var RangeLevelList = new List<RangeLevel>();

            switch (type)
            {
                case 1:
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Deficiente",
                        Min = 0,
                        Max = 10,
                        Total = 0,
                        Type = 1
                    });
                    break;
                case 2:
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Regular",
                        Min = 11,
                        Max = 13,
                        Total = 0,
                        Type = 2
                    });
                    break;
                case 3:
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Bueno",
                        Min = 14,
                        Max = 16,
                        Total = 0,
                        Type = 3
                    });
                    break;
                case 4:
                    RangeLevelList.Add(new RangeLevel
                    {
                        Name = "Excelente",
                        Min = 17,
                        Max = 20,
                        Total = 0,
                        Type = 4
                    });
                    break;

            }

            var result = await _academicSummariesService.AchievementLevelAcademicYearStudentCompetenceDetail(termId, facultyId, careerId, curriculumId, competenceId, studentId, academicYear, RangeLevelList);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el reporte de alumnos por competencias filtrado por los siguientes parámetros
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="academicYear">Ciclo académico</param>
        /// <returns>Reporte de alumnos</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetReport(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, byte academicYear)
        {
            var result = await _academicSummariesService.GetReportAchievementLevelAcademicYear(termId, facultyId, careerId, curriculumId, academicYear);
            return Ok(result);
        }

        /// <summary>
        /// Genera el reporte del nivel de logro por competencia
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="facultyId">identificador de la facultad</param>
        /// <param name="careerId">identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="academicYear">Ciclo académico</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("descargar/pdf/{termId}/{facultyId}/{careerId}/{curriculumId}/{academicYear}")]
        public async Task<IActionResult> DownloadPDF(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, byte academicYear)
        {

            var model = await _academicSummariesService.AchievementLevelAcademicYearCompetences(termId, facultyId, careerId, curriculumId, academicYear);
            var viewToString = "";
            viewToString = await _viewRenderService.RenderToStringAsync($"/Areas/CareerDirector/Views/ReportGradesByStudentCompetence/Pdf.cshtml", model);
            try
            {
                var objectSettings = new DinkToPdf.ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = viewToString,
                    WebSettings = { DefaultEncoding = "utf-8",
                                    //UserStyleSheet = cssPath
                    },
                    FooterSettings = {
                        FontName = "Arial",
                        FontSize = 9,
                        Line = false,
                        Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                        Center = "",
                        Right = "Pág: [page]/[toPage]"
                    }
                };

                var globalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 30, Left = 10, Right = 10 }
                };

                var pdf = new DinkToPdf.HtmlToPdfDocument
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                var fileByte = _dinkConverter.Convert(pdf);

                HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

                return File(fileByte, "application/pdf", "reporte_nivel_de_logro_por_competencia_y_ciclo.pdf");


            }
            catch (Exception e)
            {
                return BadRequest("No se pudo descargar el archivo");
            }
        }

        /// <summary>
        /// Genera el reporte del nivel de logro por competencia
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="facultyId">identificador de la facultad</param>
        /// <param name="careerId">identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="academicYear">Ciclo académico</param>
        /// <returns>Archivo en formato EXCEL</returns>
        [HttpGet("reporte-excel/{termId}/{facultyId}/{careerId}/{curriculumId}/{academicYear}")]
        public async Task<IActionResult> DownloadReportExcel(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, byte academicYear)
        {
            var data = await _academicSummariesService.AchievementLevelAcademicYearCompetences(termId, facultyId, careerId, curriculumId, academicYear);

            var fileName = $"Nivel de logro por competencias y ciclo academico.xlsx";

            using (var wb = new XLWorkbook())
            {
                var worksheet = wb.Worksheets.Add("Reporte");
                worksheet.Cell(1, 1).Value = $"Competencia";
                worksheet.Cell(1, 1).Style.Font.FontSize = 11;
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightGray;


                worksheet.Range("B1:C1").Merge();
                worksheet.Cell(1, 2).Value = $"Deficiente";
                worksheet.Cell(1, 2).Style.Font.FontSize = 11;
                worksheet.Cell(1, 2).Style.Font.Bold = true;
                worksheet.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightGray;


                worksheet.Range("D1:E1").Merge();
                worksheet.Cell(1, 4).Value = $"Regular";
                worksheet.Cell(1, 4).Style.Font.FontSize = 11;
                worksheet.Cell(1, 4).Style.Font.Bold = true;
                worksheet.Cell(1, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, 4).Style.Fill.BackgroundColor = XLColor.LightGray;



                worksheet.Range("F1:G1").Merge();
                worksheet.Cell(1, 6).Value = $"Bueno";
                worksheet.Cell(1, 6).Style.Font.FontSize = 11;
                worksheet.Cell(1, 6).Style.Font.Bold = true;
                worksheet.Cell(1, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, 6).Style.Fill.BackgroundColor = XLColor.LightGray;



                worksheet.Range("H1:I1").Merge();
                worksheet.Cell(1, 8).Value = $"Excelente";
                worksheet.Cell(1, 8).Style.Font.FontSize = 11;
                worksheet.Cell(1, 8).Style.Font.Bold = true;
                worksheet.Cell(1, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, 8).Style.Fill.BackgroundColor = XLColor.LightGray;


                worksheet.Range("J1:K1").Merge();
                worksheet.Cell(1, 10).Value = $"TOTAL";
                worksheet.Cell(1, 10).Style.Font.FontSize = 11;
                worksheet.Cell(1, 10).Style.Font.Bold = true;
                worksheet.Cell(1, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, 10).Style.Fill.BackgroundColor = XLColor.LightGray;

                var row = 2;
                var colum = 2;

                for (int i = 0; i <= 4; i++)
                {
                    worksheet.Cell(row, colum).Value = $"N°";
                    worksheet.Cell(row, colum).Style.Font.FontSize = 11;
                    worksheet.Cell(row, colum).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    colum++;
                    worksheet.Cell(row, colum).Value = $"%";
                    worksheet.Cell(row, colum).Style.Font.FontSize = 11;
                    worksheet.Cell(row, colum).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    colum++;
                }


                var rowData = 3;
                var columData = 1;
                foreach (var item in data.OrderBy(x => x.CompetenceName).ToList())
                {
                    worksheet.Cell(rowData, columData).Value = $"{item.CompetenceName}";
                    worksheet.Cell(rowData, columData).Style.Font.FontSize = 11;
                    worksheet.Cell(rowData, columData).Style.Font.Bold = true;
                    worksheet.Cell(rowData, columData).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    columData++;

                    worksheet.Cell(rowData, columData).Value = $"{item.RangeLevels.Where(x => x.Name == "Deficiente").FirstOrDefault().Total}";
                    worksheet.Cell(rowData, columData).Style.Font.FontSize = 10;
                    worksheet.Cell(rowData, columData).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    columData++;

                    worksheet.Cell(rowData, columData).Value = (item.RangeLevels.Sum(x => x.Total) > 0 ? Math.Round((((decimal)item.RangeLevels.Where(x => x.Name == "Deficiente").FirstOrDefault().Total) / (decimal)(item.RangeLevels.Sum(x => x.Total)) * 100), 2) : 0).ToString();
                    worksheet.Cell(rowData, columData).Style.Font.FontSize = 10;
                    worksheet.Cell(rowData, columData).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    columData++;

                    worksheet.Cell(rowData, columData).Value = $"{item.RangeLevels.Where(x => x.Name == "Regular").FirstOrDefault().Total}";
                    worksheet.Cell(rowData, columData).Style.Font.FontSize = 10;
                    worksheet.Cell(rowData, columData).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    columData++;

                    worksheet.Cell(rowData, columData).Value = (item.RangeLevels.Sum(x => x.Total) > 0 ? Math.Round((((decimal)item.RangeLevels.Where(x => x.Name == "Regular").FirstOrDefault().Total) / (decimal)(item.RangeLevels.Sum(x => x.Total)) * 100), 2) : 0).ToString();
                    worksheet.Cell(rowData, columData).Style.Font.FontSize = 10;
                    worksheet.Cell(rowData, columData).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    columData++;

                    worksheet.Cell(rowData, columData).Value = $"{item.RangeLevels.Where(x => x.Name == "Bueno").FirstOrDefault().Total}";
                    worksheet.Cell(rowData, columData).Style.Font.FontSize = 10;
                    worksheet.Cell(rowData, columData).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    columData++;

                    worksheet.Cell(rowData, columData).Value = (item.RangeLevels.Sum(x => x.Total) > 0 ? Math.Round((((decimal)item.RangeLevels.Where(x => x.Name == "Bueno").FirstOrDefault().Total) / (decimal)(item.RangeLevels.Sum(x => x.Total)) * 100), 2) : 0).ToString();
                    worksheet.Cell(rowData, columData).Style.Font.FontSize = 10;
                    worksheet.Cell(rowData, columData).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    columData++;

                    worksheet.Cell(rowData, columData).Value = $"{item.RangeLevels.Where(x => x.Name == "Excelente").FirstOrDefault().Total}";
                    worksheet.Cell(rowData, columData).Style.Font.FontSize = 10;
                    worksheet.Cell(rowData, columData).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    columData++;

                    worksheet.Cell(rowData, columData).Value = (item.RangeLevels.Sum(x => x.Total) > 0 ? Math.Round((((decimal)item.RangeLevels.Where(x => x.Name == "Excelente").FirstOrDefault().Total) / (decimal)(item.RangeLevels.Sum(x => x.Total)) * 100), 2) : 0).ToString();
                    worksheet.Cell(rowData, columData).Style.Font.FontSize = 10;
                    worksheet.Cell(rowData, columData).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    columData++;

                    worksheet.Cell(rowData, columData).Value = $"{item.RangeLevels.Sum(x => x.Total)}";
                    worksheet.Cell(rowData, columData).Style.Font.FontSize = 10;
                    worksheet.Cell(rowData, columData).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    columData++;

                    worksheet.Cell(rowData, columData).Value = $"100%";
                    worksheet.Cell(rowData, columData).Style.Font.FontSize = 10;
                    worksheet.Cell(rowData, columData).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rowData++;
                    columData = 1;
                }

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
