using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.CORE.Helpers;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_SECRETARY + "," +
        ConstantHelpers.ROLES.DEAN + "," +
        ConstantHelpers.ROLES.DEAN_SECRETARY + "," +
        ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," +
        ConstantHelpers.ROLES.REPORT_QUERIES)]
    [Area("Admin")]
    [Route("admin/reporte_aprobados_desaprobados_cursos")]
    public class ReportApprovedCourseController : BaseController
    {
        private readonly ISelect2Service _select2Service;
        private readonly ICourseTermService _courseTermService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly IStudentSectionService _studentSectionService;

        public ReportApprovedCourseController(ISelect2Service select2Service, ICourseTermService courseTermService,
            IAcademicHistoryService academicHistoryService, IDataTablesService dataTablesService, IStudentSectionService studentSectionService) : base(dataTablesService)
        {
            _select2Service = select2Service;
            _courseTermService = courseTermService;
            _academicHistoryService = academicHistoryService;
            _studentSectionService = studentSectionService;
        }

        /// <summary>
        /// Vista donde se muestra el reporte de aprobados vs desaprobados
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de cursos aperturados para ser usado en select
        /// </summary>
        /// <param name="cid">Identificador del periodo académico</param>
        /// <returns>Listado de cursos</returns>
        [HttpGet("getCourses/{cid}")]
        public async Task<IActionResult> GetCourses(Guid cid)
        {
            var courses = await _courseTermService.GetCourseTermByTermSelect2ClientSide(cid);
            return Ok(courses);
        }

        /// <summary>
        /// Obtiene el listado de resumenes académicos de los estudiantes para ser usado en tablas
        /// </summary>
        /// <param name="cid">Identificador del plan de estudio</param>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <param name="carId">Identificador de la escuela profesional</param>
        /// <param name="curId">Identificador del curso </param>
        /// <param name="name">Texto de búsqueda</param>
        /// <returns>Listado de resumenes académicos</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetStudents(Guid cid, Guid pid, Guid carId, Guid curId, string name)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _academicHistoryService.GetAcademicHistoryDatatable(parameters, carId, curId, cid, pid, name);
            return Ok(result);
        }

        [HttpGet("get-excel")]
        public async Task<IActionResult> GetStudentsExcel(Guid careerId, Guid curriculumId, Guid termId, Guid courseId)
        {
            var result = await _academicHistoryService.GetAcademicHistoryTemplate(careerId, curriculumId, courseId, termId);

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Nombre Completo");
            dt.Columns.Add("Usuario");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Programa Académico");
            dt.Columns.Add("Veces");
            dt.Columns.Add("Nota");
            dt.Columns.Add("Estado");

            foreach (var item in result)
                dt.Rows.Add(item.Name, item.Code, item.Career, item.AcademicProgram, item.Intents, item.Grade, (item.Approbed ? "Aprobado" : "Desaprobado"));

            dt.AcceptChanges();

            string fileName = $"Listado de estudiantes.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Listado de estudiantes", null);

                ws.Rows().AdjustToContents();
                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }

        }

        /// <summary>
        /// Obtiene el reporte de los resumenes académicos
        /// </summary>
        /// <param name="carId">Identificador de la escuela profesional</param>
        /// <param name="curId">Identificador del curso</param>
        /// <param name="cid">Identificador del plan de estudio</param>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene los datos de los resumenes académicos</returns>
        [HttpGet("chart/{carId}/{curId}/{cid}/{pid}")]
        public async Task<IActionResult> GetChartReport(Guid carId, Guid curId, Guid cid, Guid pid)
        {
            var result = await _academicHistoryService.GetAcademicHistoriesReportAsData(carId, curId, cid, pid);
            return Ok(result);
        }

        /// <summary>
        /// Vista donde se muestra el reporte de aprobados vs desaprobados por sección
        /// </summary>
        /// <returns>Vista del reporte por sección</returns>
        [HttpGet("seccion")]
        public IActionResult SectionReport()
        {
            return View();
        }


        [HttpGet("seccion/datatable/get")]
        public async Task<IActionResult> GetSectionStudentsDatable(Guid termId, Guid? careerId = null, int year = -1)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            int? yearValue = null;
            if (year != -1)
                yearValue = year;
            var result = await _studentSectionService.GetStudentSectionsIntranetReportDatatable(sentParameters, termId, careerId, yearValue);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de secciones con los alumnos aprobados y desaprobados según los filtros
        /// </summary>
        /// <param name="cid">Identificador del plan de estudio</param>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curId">Identificador del curso </param>
        /// <param name="name">Texto de búsqueda</param>
        /// <returns>Listado de resumenes académicos</returns>
        [HttpGet("seccion/get")]
        public async Task<IActionResult> GetSectionStudents(Guid termId, Guid careerId, Guid curriculumId, int? year)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentSectionService.GetStudentsApprovedDisapprovedBySectionDatatable(sentParameters, termId, careerId, curriculumId, year);
            return Ok(result);
        }

        [HttpGet("seccion/reporte-excel")]
        public async Task<IActionResult> GetSectionStudentsExcel(Guid termId, Guid careerId, Guid curriculumId, int? year)
        {
            var data = await _studentSectionService.GetStudentsApprovedDisapprovedBySectionData(termId, careerId, curriculumId, year);
            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Semestre");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Plan de Estudios");
            dt.Columns.Add("Código Curso");
            dt.Columns.Add("Nombre Asignatura");
            dt.Columns.Add("Sección");
            dt.Columns.Add("Créditos");
            dt.Columns.Add("Ciclo");
            dt.Columns.Add("Nro Matriculados");
            dt.Columns.Add("Nro Aprobados");
            dt.Columns.Add("Nro Desaprobados");
            dt.Columns.Add("Docente (Acta Final)");

            foreach (var item in data)
                dt.Rows.Add(item.Term, item.Career, item.Curriculum, item.Code, item.Name, item.Section, item.Credits, item.AcademicYear, item.Enrolled, item.Approved, item.Disapproved, item.Teacher);

            dt.AcceptChanges();


            var fileName = $"Reporte de Estudiantes Aprobados por Seccion.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Estudiantes aprobados y desaprobados por sección", null, 12);

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
