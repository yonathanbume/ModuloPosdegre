using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.RankingViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using ClosedXML.Excel;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_SECRETARY + "," +
        ConstantHelpers.ROLES.DEAN + "," +
        ConstantHelpers.ROLES.DEAN_SECRETARY + "," +
        ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," +
        ConstantHelpers.ROLES.REPORT_QUERIES + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE)]
    [Area("CareerDirector")]
    [Route("director-carrera/notas-por-curso")]
    public class ReportGradesController : BaseController
    {
        private readonly IAcademicSummariesService _academicSummariesService;
        private readonly IStudentSectionService _studentSectionService;
        private IWebHostEnvironment _hostingEnvironment;
        public ReportGradesController(
            IDataTablesService dataTablesService,
            IAcademicSummariesService academicSummariesService,
            IWebHostEnvironment environment,
            IStudentSectionService studentSection) : base(dataTablesService)
        {
            _academicSummariesService = academicSummariesService;
            _studentSectionService = studentSection;
            _hostingEnvironment = environment;
        }

        /// <summary>
        /// Vista donde se muestra el reporte de notas por curso
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el reporte de notas por curso
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="courseId">Identificador del curso</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="academicYear">Ciclo académico</param>
        /// <returns>Reporte de notas</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetStatusStudent(Guid termId, Guid courseId, Guid curriculumId, byte academicYear)
        {
            var result = await _studentSectionService.GetGradesByCourses(termId, null, null, curriculumId, null, courseId, academicYear);
            return Ok(result);
        }

        /// <summary>
        /// Genera un reporte de notas por curso
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="courseId">Identificador del curso</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="academicYear">Ciclo académico</param>
        /// <returns>Archivo en formato EXCEL</returns>
        [Route("reporte-excel")]
        public async Task<IActionResult> DownloadExcelReport(Guid termId, Guid courseId, Guid curriculumId, byte academicYear)
        {
            var data = await _studentSectionService.GetGradesByCourses(termId, null, null, curriculumId, null, courseId, academicYear);
            var dt = new DataTable
            {
                TableName = "Reporte de notas por curso"
            };

            dt.Columns.Add("#");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Curso");
            dt.Columns.Add("Grupo");
            dt.Columns.Add("Nota");

            for (int i = 0; i < data.Datatable.Count(); i++)
            {
                dt.Rows.Add(
                    i + 1,
                    data.Datatable[i].Name,
                    data.Datatable[i].Course,
                    data.Datatable[i].SectionCode,
                    data.Datatable[i].Grade
                    );
            }

            dt.AcceptChanges();

            var img = Path.Combine(_hostingEnvironment.WebRootPath, $@"images/themes/{CORE.Helpers.GeneralHelpers.GetTheme()}/logo-report.png");
            var fileName = $"Reporte de notas por curso.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Listado de Estudiantes", null);
                //ws.AddHeaderToWorkSheet("Listado de Estudiantes", img);

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }

        }

    }
}
