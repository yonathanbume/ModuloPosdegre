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
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.REPORT_QUERIES + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE)]
    [Area("Admin")]
    [Route("admin/reporte-ponderado-desaprobado")]
    public class ReportDisapprovedStudentController : BaseController
    {
        private readonly IAcademicSummariesService _academicSummariesService;
        public ReportDisapprovedStudentController(
            IDataTablesService dataTablesService,
            IAcademicSummariesService academicSummariesService) : base(dataTablesService)
        {
            _academicSummariesService = academicSummariesService;
        }

        /// <summary>
        /// Vista donde se muestra el reporte de estudiantes con ponderado semestral desaprobado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de estudiantes con ponderado semestral desaprobado
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de estudiantes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetStatusStudent(Guid? termId, Guid? careerId, Guid? facultyId, string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _academicSummariesService.GetDisapprovedStudents(sentParameters, termId, careerId, facultyId, search);
            return Ok(result);
        }

        [HttpGet("get-excel")]
        public async Task<IActionResult> GetStatusStudentDatatable(Guid? termId, Guid? careerId, Guid? facultyId)
        {
            var result = await _academicSummariesService.GetDisapprovedStudentsTemplate(termId, careerId, facultyId);

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Usuario");
            dt.Columns.Add("Estudiante");
            dt.Columns.Add("Periodo Académico");
            dt.Columns.Add("Facultad");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Puntaje");

            foreach (var item in result)
                dt.Rows.Add(item.UserName, item.FullName, item.Term, item.Faculty, item.Career, item.Score);

            dt.AcceptChanges();

            string fileName = $"Estudiantes con promedio desaprobado.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Estudiantes con promedio desaprobado", null);

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
