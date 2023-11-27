using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Areas.Admin.Models.Report_courseViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.CORE.Services;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ClosedXML.Excel;
using AKDEMIC.CORE.Extensions;
using System.Linq;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_SECRETARY+ "," +
        ConstantHelpers.ROLES.DEAN + "," +
        ConstantHelpers.ROLES.DEAN_SECRETARY + "," +
        ConstantHelpers.ROLES.REPORT_QUERIES + "," +
        ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," +
        ConstantHelpers.ROLES.VICERRECTOR)]
    [Area("Admin")]
    [Route("admin/reporte_asistencia_curso")]
    public class ReportCourseController : BaseController
    {
        private readonly ICourseTermService _courseTermService;
        private readonly ITeacherSectionService _teacherSectionService;
        private readonly ITermService _termService;
        private readonly ISectionService _sectionService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ICurriculumService _curriculumService;
        private readonly ICareerService _careerService;
        private readonly ICourseService _courseService;
        private readonly IClassService _classService;
        private readonly IStudentSectionService _studentSectionService;

        public ReportCourseController(ICourseTermService courseTermService,
            IDataTablesService datatableService,
            ITeacherSectionService teacherSectionService,
            ITermService termService,
            ISectionService sectionService,
            IWebHostEnvironment environment,
            ICurriculumService curriculumService,
            ICareerService careerService,
            ICourseService courseService,
            IClassService classService,
            IStudentSectionService studentSectionService) : base(datatableService)
        {
            _courseTermService = courseTermService;
            _teacherSectionService = teacherSectionService;
            _termService = termService;
            _sectionService = sectionService;
            _hostingEnvironment = environment;
            _curriculumService = curriculumService;
            _careerService = careerService;
            _courseService = courseService;
            _classService = classService;
            _studentSectionService = studentSectionService;
        }

        /// <summary>
        /// Vista donde se muestra el reporte de asistencia por curso
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de cursos aperturados filtrados por los siguientes parámetros para ser usado en tablas.
        /// </summary>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <param name="carId">Identificador del curso</param>
        /// <param name="curId">Identificador del plan de estudio</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de cursos</returns>
        [HttpGet("curso/periodo")]
        public async Task<IActionResult> GetCourseTermData(Guid cid, Guid pid, Guid carId, Guid curId, string search = null)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var query = await _courseTermService.GetCourseTermSectionsDataTable(parameters, carId, curId, Guid.Empty, pid, search);
            return Ok(query);
        }

        /// <summary>
        /// Obtiene el listado de cursos aperturados para ser usado en select
        /// </summary>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <returns>Listado de cursos</returns>
        [HttpGet("cursos/periodo/{pid}")]
        public async Task<IActionResult> GetCourseTermSelect(Guid pid)
        {
            if (pid.Equals(Guid.Empty))
                return NotFound($"No se pudo encontrar un periodo con el id '{pid}'.");

            var result = await _courseTermService.GetCourseTermByTermSelect2ClientSide(pid);
            return Ok(result);
        }

        /// <summary>
        /// Genera el reporte general de asistencia
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="endDate">Fecha fin</param>
        /// <returns>Archivo en formato EXCEL</returns>
        [HttpGet("reporte-general-excel/{termId}/{careerId}/{curriculumId}")]
        public async Task<IActionResult> GetGeneralReportExcel(Guid termId, Guid careerId, Guid curriculumId, string endDate)
        {
            var term = await _termService.Get(termId);
            var career = await _careerService.Get(careerId);
            var curriculum = await _curriculumService.Get(curriculumId);

            var endDateTime = string.IsNullOrEmpty(endDate) ? DateTime.UtcNow : ConvertHelpers.DatepickerToUtcDateTime(endDate);
            var result = await _classService.GetReportClassAssistance(termId, careerId, curriculumId, endDateTime);

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Código");
            dt.Columns.Add("Curso");
            dt.Columns.Add("Semestre");
            dt.Columns.Add("Sección");
            dt.Columns.Add("Docentes");
            dt.Columns.Add("Sesiones de Aprendizaje Totales");
            dt.Columns.Add("Sesiones de Aprendizaje Dictadas");
            dt.Columns.Add("Porcentaje de Cumplimiento");

            foreach (var item in result)
                dt.Rows.Add(
                    item.CourseCode,
                    item.Course,
                    item.Semester,
                    item.Section,
                    item.Teachers,
                    item.Scheduled,
                    item.Taken,
                    $"{item.Percentage:F}%"
                    );

            dt.AcceptChanges();
            //var img = Path.Combine(_hostingEnvironment.WebRootPath, @"images/themes/" + CORE.Helpers.GeneralHelpers.GetTheme() + @"/logo-report.png");

            string fileName = $"Reporte asistencia estudiantes.xlsx";
            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.Row(1).InsertRowsAbove(7);
                var mergeRangeColumn = 'H';

                //using (var memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(img)))
                //{
                //    //ws.AddPicture(memoryStream).MoveTo(ws.Cell("A1")).Scale(0.5);
                //}

                ws.Cell(2, 1).Value = CORE.Helpers.GeneralHelpers.GetInstitutionName().ToUpper();
                ws.Cell(2, 1).Style.Font.FontSize = 12;
                ws.Cell(2, 1).Style.Font.Bold = true;
                ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range($"A2:{mergeRangeColumn}2").Merge();

                ws.Cell(3, 1).Value = $"Reporte de Asistencia ({term.Name})";
                ws.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(3, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range($"A3:{mergeRangeColumn}3").Merge();

                ws.Cell(4, 1).Value = "ESCUELA PROFESIONAL";
                ws.Cell(4, 1).Style.Font.Bold = true;
                ws.Cell(4, 1).Style.Font.FontSize = 11;
                ws.Cell(4, 2).Value = $"{career?.Name}";

                ws.Cell(5, 1).Value = "PLAN DE ESTUDIO";
                ws.Cell(5, 1).Style.Font.Bold = true;
                ws.Cell(5, 1).Style.Font.FontSize = 11;
                ws.Cell(5, 2).Value = $"'{curriculum.Code}";

                ws.Cell(6, 1).Value = "RANGO DE FECHAS";
                ws.Cell(6, 1).Style.Font.Bold = true;
                ws.Cell(6, 1).Style.Font.FontSize = 11;
                ws.Cell(6, 2).Value = $"'{term.ClassStartDate:dd/MM/yyy}-{endDateTime.ToLocalDateFormat()}";

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        /// <summary>
        /// Obtiene el listado de asistencias por sección
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Objeto que contiene el listado de clases</returns>
        [HttpGet("reporte_curso_detalles")]
        public async Task<IActionResult> GetReportCourseDetail(Guid sectionId)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var query = await _sectionService.GetReportClassDetailDataTable(parameters, sectionId);
            return Ok(query);
        }

        /// <summary>
        /// Genera el reporte de clases por sección
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Archivo en formato EXCEL</returns>
        [HttpGet("reporte-excel/{sectionId}")]
        public async Task<IActionResult> GetSectionReportExcel(Guid sectionId)
        {
            var section = await _sectionService.Get(sectionId);
            var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
            var course = await _courseService.GetAsync(courseTerm.CourseId);

            var data = await _studentSectionService.GetStudentAssistanceReportDataTable(sectionId);

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            var cont = 1;
            dt.Columns.Add("Nro");
            dt.Columns.Add("Estudiante");
            dt.Columns.Add("Faltas");
            dt.Columns.Add("Clases Dictadas");
            dt.Columns.Add("Máx Faltas");

            foreach (var item in data.Data)
                dt.Rows.Add(
                    cont++,
                    item.student,
                    item.absences,
                    item.dictated,
                    item.maxAbsences
                    );

            dt.AcceptChanges();

            //var img = Path.Combine(_hostingEnvironment.WebRootPath, @"images/themes/" + CORE.Helpers.GeneralHelpers.GetTheme() + @"/logo-report.png");

            string fileName = $"Reporte asistencia estudiantes.xlsx";
            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.Column(1).InsertColumnsBefore(1);
                ws.Row(1).InsertRowsAbove(6);
                var mergeRangeColumn = 'F';

                //using (var memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(img)))
                //{
                //    ws.AddPicture(memoryStream).MoveTo(ws.Cell("A1")).Scale(0.2);
                //}


                ws.Cell(2, 2).Value = CORE.Helpers.GeneralHelpers.GetInstitutionName().ToUpper();
                ws.Cell(2, 2).Style.Font.FontSize = 12;
                ws.Cell(2, 2).Style.Font.Bold = true;
                ws.Cell(2, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(2, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range($"B2:{mergeRangeColumn}2").Merge();

                ws.Cell(3, 2).Value = "Listado de Estudiantes";
                ws.Cell(3, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(3, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range($"B3:{mergeRangeColumn}3").Merge();

                ws.Cell(4, 2).Value = "CURSO";
                ws.Cell(4, 2).Style.Font.Bold = true;
                ws.Cell(4, 2).Style.Font.FontSize = 11;
                ws.Cell(4, 3).Value = $"{course.Code}-{course.Name}";

                ws.Cell(5, 2).Value = "SECCIÓN";
                ws.Cell(5, 2).Style.Font.Bold = true;
                ws.Cell(5, 2).Style.Font.FontSize = 11;
                ws.Cell(5, 3).Value = section.Code;

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        /// <summary>
        /// Vista donde se muestra el listado de alumnos matriculados detallando sus asistencias y faltas
        /// </summary>
        /// <param name="sectionId">Identificador de la sección</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("reporte_curso_vista/{sectionId}")]
        public async Task<IActionResult> ReportCourseView(Guid sectionId)
        {
            var section = await _sectionService.Get(sectionId);
            var courseTerm = await _courseTermService.GetCourseTermWithCourse(section.CourseTermId);
            var totalClasses = await _sectionService.GetTotalClassesBySection(sectionId);
            var teachers = await _teacherSectionService.GetAllBySection(sectionId);

            ReportCourseViewModel modelResult = new ReportCourseViewModel()
            {
                IdCourseTerm = courseTerm.Id,
                TotalClasses = totalClasses,
                Name = courseTerm.Course.Name,
                Code = courseTerm.Course.Code,
                Credits = courseTerm.Course.Credits,
                Section = section.Code,
                Id = section.Id,
                Teachers = string.Join(", ", teachers.Select(x => x.Teacher.User.FullName).ToList())
            };
            return View(modelResult);
        }

        [HttpPost("reporte_curso_vista/deshabilita-estudiante/{id}")]
        public async Task<IActionResult> DisableStudentSection(Guid id)
        {
            var studentSection = await _studentSectionService.Get(id);
            studentSection.Status = ConstantHelpers.STUDENT_SECTION_STATES.DPI;
            await _studentSectionService.Update(studentSection);
            return Ok();
        }

    }
}
