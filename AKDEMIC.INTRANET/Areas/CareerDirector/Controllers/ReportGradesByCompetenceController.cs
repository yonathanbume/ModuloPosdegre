// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.GradeReportByCompetences;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using ClosedXML.Excel;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.CAREER_DIRECTOR + "," + ConstantHelpers.ROLES.REPORT_QUERIES)]
    [Area("CareerDirector")]
    [Route("director-carrera/notas-por-competencias")]
    public class ReportGradesByCompetenceController : BaseController
    {
        private readonly IStudentSectionService _studentSectionService;
        private readonly IDataTablesService _dataTablesService;
        private readonly IConverter _dinkConverter;
        private readonly IViewRenderService _viewRenderService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ReportGradesByCompetenceController(
             IDataTablesService dataTablesService,
             IStudentSectionService studentSectionService,
             IWebHostEnvironment hostingEnvironment,
             IViewRenderService viewRenderService,
             IConverter dinkConverter)
        {
            _studentSectionService = studentSectionService;
            _dataTablesService = dataTablesService;
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Vista donde se muestra el reporte de notas por competencias
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el reporte de notas por competencia
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="competenceId">Identificador de la competencia</param>
        /// <returns>Reporte de notas</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetReport(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, Guid competenceId)
        {
            var result = await _studentSectionService.GetGradesByCompetences(termId, facultyId, careerId, curriculumId, competenceId);
            return Ok(result);
        }

        /// <summary>
        /// Vista donde se muestra el nivel de logro por competencia
        /// </summary>
        /// <returns>Vista detalle</returns>
        [HttpGet("nivel-de-logro")]
        public ActionResult AchievementLevel()
        {
            return View();
        }

        /// <summary>
        /// Obtiene la vista parcial donde se detalla el reporte por competencia 
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <returns>Vista parcial</returns>
        [HttpGet("nivel-de-logro-datos/{termId}/{facultyId}/{careerId}/{curriculumId}")]
        public async Task<IActionResult> _AchievementLevelReport(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId)
        {
            var result = await _studentSectionService.AchievementLevelCompetences(termId, facultyId, careerId, curriculumId);

            return PartialView(result);
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
        /// <returns>Datos por cantidad</returns>
        [HttpGet("obtener-datos-por-cantidad/{termId}/{facultyId}/{careerId}/{curriculumId}/{competenceId}/{type}")]
        public async Task<IActionResult> GetStudentByCompetence(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, Guid competenceId, int type)
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

            var result = await _studentSectionService.AchievementLevelCompetenceDetail(termId, facultyId, careerId, curriculumId, competenceId, RangeLevelList);
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
        /// <returns>Datos por cantidad de estudiante</returns>
        [HttpGet("obtener-datos-por-cantidad-por-estudiante/{termId}/{facultyId}/{careerId}/{curriculumId}/{competenceId}/{type}/{studentId}")]
        public async Task<IActionResult> GetStudentByCompetenceDetail(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId, Guid competenceId, int type, Guid studentId)
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

            var result = await _studentSectionService.AchievementLevelCompetenceStudentDetail(termId, facultyId, careerId, curriculumId, competenceId, studentId, RangeLevelList);
            return Ok(result);
        }

        /// <summary>
        /// Genera el reporte del nivel de logro por competencia
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="facultyId">identificador de la facultad</param>
        /// <param name="careerId">identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("descargar/pdf/{termId}/{facultyId}/{careerId}/{curriculumId}")]
        public async Task<IActionResult> DownloadPDF(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId)
        {

            var model = await _studentSectionService.AchievementLevelCompetences(termId, facultyId, careerId, curriculumId);
            var viewToString = "";
            viewToString = await _viewRenderService.RenderToStringAsync($"/Areas/CareerDirector/Views/ReportGradesByCompetence/Pdf.cshtml", model);
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

                return File(fileByte, "application/pdf", "reporte_nivel_de_logro.pdf");


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
        /// <returns>Archivo en formato EXCEL</returns>
        [HttpGet("reporte-excel/{termId}/{facultyId}/{careerId}/{curriculumId}")]
        public async Task<IActionResult> DownloadReportExcel(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId)
        {
            var data = await _studentSectionService.AchievementLevelCompetences(termId, facultyId, careerId, curriculumId);

            var fileName = $"Nivel de logro por competencias.xlsx";

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

        /// <summary>
        /// Obtiene el reporte del nivel de logro filtrado por los siguientes parámetros
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="facultyId">identificador de la facultad</param>
        /// <param name="careerId">identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <returns>Objeto que contiene los datos del reporte</returns>
        [HttpGet("reporte")]
        public async Task<IActionResult> GetReport(Guid termId, Guid facultyId, Guid careerId, Guid curriculumId)
        {
            var result = await _studentSectionService.GetReportAchievementLevel(termId, facultyId, careerId, curriculumId);
            return Ok(result);
        }
    }
}
