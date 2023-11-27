// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.DEGREE.Controllers;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using iTextSharp.text.pdf.security;
using AKDEMIC.CORE.Services;

namespace AKDEMIC.DEGREE.Areas.Report.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Report")]
    [Route("reporte/egresados")]
    public class GraduatedStudentController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IStudentService _studentService;

        public GraduatedStudentController(
            IStudentService studentService,
            IDataTablesService dataTablesService
        ) : base()
        {
            _studentService = studentService;
            _dataTablesService = dataTablesService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _studentService.ReportGraduates(false, null);
            return View(result);
        }

        [HttpGet("reporte-excel")]
        public async Task<IActionResult> GetExcelGraduates(int gradeType, Guid careerParameterId, int year, int admissionYear)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte_de_egresados");
                try
                {
                    await _studentService.DownloadExcelGraduates(worksheet, false, null, gradeType, careerParameterId, year, admissionYear);

                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                worksheet.RangeUsed().SetAutoFilter(false);

                Response.Cookies.Append("fileDownload", "true", new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(1) });

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte De Egresados.xlsx");
                }
            }
        }

        [HttpGet("listado")]
        public async Task<IActionResult> GraduatedList(int gradeType, Guid careerParameterId, int year, int admissionYear)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GraduatedListReport(sentParameters, false, null, gradeType, careerParameterId, year, admissionYear);
            return Ok(result);
        }
    }
}
