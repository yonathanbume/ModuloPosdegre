// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Report")]
    [Route("reporte/cursos-desaparobados-estudiante")]
    public class DisapprovedCourseByStudentController : BaseController
    {
        private readonly IAcademicHistoryService _academicHistoryService;

        public DisapprovedCourseByStudentController(IDataTablesService dataTablesService,
            IAcademicHistoryService academicHistoryService) : base(dataTablesService)
        {
            _academicHistoryService = academicHistoryService;
        }

        public IActionResult Index() => View();

        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid termId, Guid? facultyId, Guid? careerId, byte? courses)
        {
            var result = await _academicHistoryService.GetDisapprovedCoursesByStudentDataDatatable(termId, facultyId, careerId, courses);
            return Ok(result);
        }

        [HttpGet("get-cursos")]
        public async Task<IActionResult> GetStudentDisapprovedCourses(Guid termId, Guid studentId)
        {
            var data = await _academicHistoryService.GetAcademicHistoriesByStudent(studentId, termId, false, false);

            var result = data
                .Where(x => x.SectionId.HasValue)
                .Select(x => new
                {
                    code = x.Course.Code,
                    name = x.Course.Name,
                    grade = x.Grade
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("reporte-excel")]
        public async Task<IActionResult> ExcelReport(Guid termId, Guid? facultyId, Guid? careerId, byte? courses)
        {
            var data = await _academicHistoryService.GetDisapprovedCoursesByStudentData(termId, facultyId, careerId, courses);
            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Código Estudiante");
            dt.Columns.Add("Nombre Completo");
            dt.Columns.Add("Facultad");
            dt.Columns.Add("Escuela");
            dt.Columns.Add("Periodo");
            dt.Columns.Add("Cantidad de Cursos Desaprobados");

            foreach (var item in data)
                dt.Rows.Add(item.UserName, item.Name, item.Faculty, item.Career, item.Term, item.DisapprovedCourses);

            dt.AcceptChanges();

            var fileName = $"Reporte - Cursos Desaprobados por Estudiante.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Cantidad de Cursos Desaprobados por Estudiante", null, 6);
                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
