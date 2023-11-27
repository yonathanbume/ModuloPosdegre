using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Report")]
    [Route("reporte/estudiantes-desaprobados")]
    public class DisapprovedStudentController : BaseController
    {
        private readonly IStudentSectionService _studentSectionService;

        public DisapprovedStudentController(IDataTablesService dataTablesService,
            IStudentSectionService studentSectionService) : base(dataTablesService)
        {
            _studentSectionService = studentSectionService;
        }

        public IActionResult Index() => View();


        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid termId, Guid? facultyId, Guid? careerId, byte? studentTry)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentSectionService.GetDisapprovedStudentsDetailedReportDatatable(sentParameters, termId, facultyId, careerId, User, studentTry);
            return Ok(result);
        }

        [HttpGet("reporte-excel")]
        public async Task<IActionResult> ExcelReport(Guid termId, Guid? facultyId, Guid? careerId, byte? studentTry)
        {
            var data = await _studentSectionService.GetDisapprovedStudentsDetailedReportData(termId, facultyId, careerId, User, studentTry);
            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Plan");
            dt.Columns.Add("Código Estudiante");
            dt.Columns.Add("Apellidos y Nombres");
            dt.Columns.Add("Ciclo Curso");
            dt.Columns.Add("Código Curso");
            dt.Columns.Add("Asignatura");
            dt.Columns.Add("Nro veces desaprobado");
            dt.Columns.Add("Semestre");

            foreach (var item in data)
                dt.Rows.Add(item.Career, item.Curriculum, item.Username, item.Fullname, item.Year, item.Code, item.Course, item.Try, item.Term);

            dt.AcceptChanges();

            var fileName = $"Reporte de Estudiantes Desaprobados.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Listado de Estudiantes Matriculados en Cursos Desaprobados", null, 9);

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
