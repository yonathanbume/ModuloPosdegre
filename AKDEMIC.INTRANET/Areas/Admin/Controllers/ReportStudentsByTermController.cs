using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        ConstantHelpers.ROLES.DEAN + "," +
        ConstantHelpers.ROLES.DEAN_SECRETARY + "," +
        ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        ConstantHelpers.ROLES.REPORT_QUERIES + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE)]
    [Area("Admin")]
    [Route("admin/reporte-semestres-matriculados")]
    public class ReportStudentsByTermController : BaseController
    {
        private readonly IAcademicSummariesService _academicSummariesService;
        public ReportStudentsByTermController(
            IDataTablesService dataTablesService,
            IAcademicSummariesService academicSummariesService) : base(dataTablesService)
        {
            _academicSummariesService = academicSummariesService;
        }

        /// <summary>
        /// Vista del reporte de semestres matriculados por estudiante
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de alumnos donde se detalla la cantidad de periodos matriculados para ser usado en tablas
        /// </summary>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de alumnos</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetStatusStudent(Guid? careerId, Guid? facultyId, string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _academicSummariesService.GetTermsByStudent(sentParameters, careerId, facultyId, search, User);
            return Ok(result);
        }

        [HttpGet("get-excel")]
        public async Task<IActionResult> GetStatusStudentExcel(Guid? careerId, Guid? facultyId)
        {
            var result = await _academicSummariesService.GetTermsByStudentTemplate(careerId, facultyId, null, User);

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Usuario");
            dt.Columns.Add("Nombre Completo");
            dt.Columns.Add("Facultad");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Periodos Matriculados");

            foreach (var item in result)
                dt.Rows.Add(item.UserName, item.FullName, item.Faculty, item.Career, item.Terms);

            dt.AcceptChanges();

            string fileName = $"Ciclos matriculados por estudiantes.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Ciclos matriculados por estudiante", null);

                ws.Rows().AdjustToContents();
                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }

        }
    }
}
