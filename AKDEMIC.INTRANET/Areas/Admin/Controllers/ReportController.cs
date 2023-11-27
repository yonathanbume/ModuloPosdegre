using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Areas.Student.Models.AbsencesViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ClosedXML.Excel;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        ConstantHelpers.ROLES.DEAN + "," +
        ConstantHelpers.ROLES.DEAN_SECRETARY + "," +
        ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_SECRETARY + "," +
        ConstantHelpers.ROLES.REPORT_QUERIES + "," +
        ConstantHelpers.ROLES.VICERRECTOR)]
    [Area("Admin")]
    [Route("admin/reporte_asistencia")]
    public class ReportController : BaseController
    {
        private readonly ICareerService _careerService;
        private readonly IFacultyService _facultyService;
        private readonly IStudentService _studentService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IClassStudentService _classStudentService;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IWeeklyAttendanceReportService _weeklyAttendanceReportService;

        public ReportController(ICareerService careerService, IFacultyService facultyService,
            IDataTablesService dataTablesService, IStudentService studentService, ITermService termService, IStudentSectionService studentSectionService,
            IClassStudentService classStudentService, IUserService userService,
            IWebHostEnvironment webHostEnvironment,
            IWeeklyAttendanceReportService weeklyAttendanceReportService) : base(termService, dataTablesService)
        {
            _careerService = careerService;
            _facultyService = facultyService;
            _studentService = studentService;
            _studentSectionService = studentSectionService;
            _classStudentService = classStudentService;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            _weeklyAttendanceReportService = weeklyAttendanceReportService;
        }

        /// <summary>
        /// Vista donde se muestra el reporte de asistencia de estudiantes
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de estudiantes filtrados por los siguientes parámetros para ser usado en tablas
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <param name="fid">Identificador de la facultad</param>
        /// <returns>Listado de estudiantes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetStudentFilter(string search, Guid? cid, Guid? fid)
        {
            if (!fid.HasValue)
                return BadRequest("Seleccionar una facultad");

            if (cid == Guid.Empty) cid = null;

            var parameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetStudentFilterDatatable(parameters, cid, fid, null, search, User);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de escuelas profesionales para ser usado en select
        /// </summary>
        /// <param name="fid">Identificador de la facultad</param>
        /// <returns>Listado de escuelas profesionas</returns>
        [HttpGet("getcareers/{fid}")]
        public async Task<IActionResult> GetCareers(Guid fid)
        {
            var result = await _careerService.GetCareerSelect2ClientSide(null, fid, false, null, User);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de facultades para ser usado en select
        /// </summary>
        /// <returns>Listado de facultades</returns>
        [HttpGet("getfaculties")]
        public async Task<IActionResult> GetFaculties()
        {
            var result = await _facultyService.GetFacultiesSelect2ClientSide(false, User);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Vista donde se muestra el detalle del estudiante
        /// </summary>
        /// <param name="sid">Identificador del estudiante</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("{sid}")]
        public async Task<IActionResult> GetReportView(Guid sid)
        {
            var student = await _studentService.Get(sid);
            student.User = await _userService.Get(student.UserId);
            student.Career = await _careerService.Get(student.CareerId);

            var term = await _termService.GetActiveTerm();
            if (term == null)
                term = new ENTITIES.Models.Enrollment.Term();
            IndexViewModel model = new IndexViewModel()
            {
                Student = new StudentViewModel()
                {
                    StudentId = student.Id,
                    FullName = student.User.FullName,
                    UserName = student.User.UserName,
                    Career = new CareerViewModel()
                    {
                        Name = student.Career.Name
                    }
                },
                ActiveTerm = term.Id,
                AttendanceMinPercentage = term.AbsencePercentage,
                Terms = (await _termService.GetAll()).Select(x => new TermViewModel()
                {
                    Name = x.Name,
                    Id = x.Id
                }),
            };

            return View(model);
        }

        /// <summary>
        /// Obtiene el listado de cursos matriculados
        /// </summary>
        /// <param name="pid">Identificador del periodo académico</param>
        /// <param name="sid">identificador del estudiante</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de cursos matriculados</returns>
        [HttpGet("periodo/get")]
        public async Task<IActionResult> GetStudentCourses(Guid pid, Guid sid, string search)
        {
            //if (pid.Equals(Guid.Empty))
            //    return BadRequest($"No se pudo encontrar un Periodo con el id {pid}.");
            if (sid.Equals(Guid.Empty))
                return BadRequest($"No se pudo encontrar un estudiante con el id {sid}.");

            var parameters = _dataTablesService.GetSentParameters();
            var result = await _studentSectionService.GetStudentSectionDatatable(parameters, sid, pid, search);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de clases del alumno para ser usado en tablas
        /// </summary>
        /// <param name="sid">Identificador de la sección</param>
        /// <param name="aid">Identificador del estudiante</param>
        /// <param name="filter">Asistencia o falta</param>
        /// <returns>Listado de clases</returns>
        [HttpGet("seccion/alumno/get")]
        public async Task<IActionResult> GetSectionAbsenceDetail(Guid sid, Guid aid, [FromQuery] int filter)
        {
            if (sid.Equals(Guid.Empty))
                return BadRequest($"No se pudo encontrar una Sección con el id {sid}.");
            if (aid.Equals(Guid.Empty))
                return BadRequest($"No se pudo encontrar el estudiante con el id {aid}.");

            var parameters = _dataTablesService.GetSentParameters();
            var result = await _classStudentService.GetSectionAbsenceDetailDataTable(parameters, sid, aid, filter);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de periodos académicos para ser usado en select
        /// </summary>
        /// <returns>Listado de periodos académicos</returns>
        [HttpGet("periodos/get")]
        public async Task<IActionResult> GetTerms()
        {
            var result = await _termService.GetTermsSelect2ClientSide();
            return Ok(result);
        }

        /// <summary>
        /// Genera el reporte consolidado de asistencia
        /// </summary>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <returns>Archivo en formato EXCEL</returns>
        [HttpGet("reporte-asistencia-consolidado")]
        public async Task<IActionResult> GetReportAssistance(Guid facultyId, Guid? careerId)
        {
            var data = await _weeklyAttendanceReportService.GetSectionReportData(facultyId, careerId, User);
            var img = Path.Combine(_webHostEnvironment.WebRootPath, @"images/themes/" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png");
            var fileName = $"Reporte de Cumplimiento.xlsx";
            using (var wb = new XLWorkbook())
            {
                var ws = wb.AddWorksheet("Reporte Cumplimiento");

                //if (!string.IsNullOrEmpty(img))
                //{
                //    using (var memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(img)))
                //    {
                //        ws.AddPicture(memoryStream).MoveTo(ws.Cell("A1")).Scale(0.5);
                //    }
                //}
                var maxColumnsTitle = data.OrderByDescending(x => x.WeekReport.Count()).Select(x => x.WeekReport.Count()).FirstOrDefault() + 6;
                ws.Cell(2, 1).Value = CORE.Helpers.GeneralHelpers.GetInstitutionName().ToUpper();
                ws.Cell(2, 1).Style.Font.FontSize = 12;
                ws.Cell(2, 1).Style.Font.Bold = true;
                ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range(2, 1, 2, maxColumnsTitle).Merge();

                if (careerId.HasValue && careerId != Guid.Empty)
                {
                    var career = await _careerService.Get(careerId.Value);
                    ws.Cell(3, 1).Value = $"Reporte de Cumplimiento de Asistencia de Estudiantes ({career.Name})";
                }
                else
                {
                    ws.Cell(3, 1).Value = "Reporte de Cumplimiento de Asistencia de Estudiantes";

                }
                ws.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(3, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range(3, 1, 3, maxColumnsTitle).Merge();


                ws.Cell(6, 1).Value = "Curso";
                ws.Cell(6, 1).Style.Font.Bold = true;
                ws.Cell(6, 1).Style.Font.FontSize = 11;
                ws.Cell(6, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(6, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range(6, 1, 7, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(6, 1, 7, 1).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Range(6, 1, 7, 1).Merge();

                ws.Cell(6, 2).Value = "Sección";
                ws.Cell(6, 2).Style.Font.FontSize = 11;
                ws.Cell(6, 2).Style.Font.Bold = true;
                ws.Cell(6, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(6, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range(6, 2, 7, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(6, 2, 7, 2).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Range(6, 2, 7, 2).Merge();

                ws.Cell(6, 3).Value = "Semestre";
                ws.Cell(6, 3).Style.Font.FontSize = 11;
                ws.Cell(6, 3).Style.Font.Bold = true;
                ws.Cell(6, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(6, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range(6, 3, 7, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(6, 3, 7, 3).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Range(6, 3, 7, 3).Merge();

                ws.Cell(6, 4).Value = "Estudiantes Matriculados";
                ws.Cell(6, 4).Style.Font.FontSize = 11;
                ws.Cell(6, 4).Style.Font.Bold = true;
                ws.Cell(6, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(6, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range(6, 4, 7, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(6, 4, 7, 4).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Range(6, 4, 7, 4).Merge();

                var maxColumns = data.OrderByDescending(x => x.WeekReport.Count()).Select(x => x.WeekReport.Count()).FirstOrDefault();
                var toMergue = 5 + maxColumns;
                ws.Cell(6, 5).Value = "Asistencia por Semana";
                ws.Cell(6, 5).Style.Font.FontSize = 11;
                ws.Cell(6, 5).Style.Font.Bold = true;
                ws.Cell(6, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(6, 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range(6, 5, 6, toMergue).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(6, 5, 6, toMergue).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Range(6, 5, 6, (toMergue - 1 < 5 ? 5 : toMergue - 1)).Merge();

                for (int i = 5; i < toMergue; i++)
                {
                    ws.Cell(7, i).Value = $"Semana {i - 4}";
                    ws.Cell(7, i).Style.Font.FontSize = 11;
                    ws.Cell(7, i).Style.Font.Bold = true;
                    ws.Cell(7, i).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(7, i).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(7, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell(7, i).Style.Border.OutsideBorderColor = XLColor.Black;
                }

                ws.Cell(6, toMergue).Value = "Promedio de Asistencia";
                ws.Cell(6, toMergue).Style.Font.FontSize = 11;
                ws.Cell(6, toMergue).Style.Font.Bold = true;
                ws.Cell(6, toMergue).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(6, toMergue).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range(6, toMergue, 7, toMergue).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(6, toMergue, 7, toMergue).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Range(6, toMergue, 7, toMergue).Merge();

                toMergue++;
                ws.Cell(6, toMergue).Value = "Porcentaje Total de Asistencia";
                ws.Cell(6, toMergue).Style.Font.FontSize = 11;
                ws.Cell(6, toMergue).Style.Font.Bold = true;
                ws.Cell(6, toMergue).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(6, toMergue).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range(6, toMergue, 7, toMergue).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(6, toMergue, 7, toMergue).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Range(6, toMergue, 7, toMergue).Merge();

                var index = 8;
                foreach (var item in data)
                {
                    ws.Cell(index, 1).Value = item.Course;
                    ws.Cell(index, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(index, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(index, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell(index, 1).Style.Border.OutsideBorderColor = XLColor.Black;

                    ws.Cell(index, 2).Value = item.Section;
                    ws.Cell(index, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(index, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(index, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell(index, 2).Style.Border.OutsideBorderColor = XLColor.Black;

                    ws.Cell(index, 3).Value = $"'{item.AcademicYear}";
                    ws.Cell(index, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(index, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(index, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell(index, 3).Style.Border.OutsideBorderColor = XLColor.Black;

                    ws.Cell(index, 4).Value = item.EnrolledStudents;
                    ws.Cell(index, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(index, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(index, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell(index, 4).Style.Border.OutsideBorderColor = XLColor.Black;

                    var subIndex = 5;
                    for (int i = 0; i < maxColumns; i++)
                    {
                        if (item.WeekReport.ElementAtOrDefault(i) != null)
                        {
                            //ws.Cell(index, subIndex).Value = item.WeekReport[i].AverageAttendances;
                            ws.Cell(index, subIndex).Value = $"{item.WeekReport[i].AttendancePercentage}%";
                        }

                        ws.Cell(index, subIndex).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(index, subIndex).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        ws.Cell(index, subIndex).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(index, subIndex).Style.Border.OutsideBorderColor = XLColor.Black;
                        subIndex++;
                    }

                    var subToMergue = 5 + maxColumns;
                    ws.Cell(index, subToMergue).Value = item.AttendanceAverage;
                    ws.Cell(index, subToMergue).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(index, subToMergue).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(index, subToMergue).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell(index, subToMergue).Style.Border.OutsideBorderColor = XLColor.Black;

                    subToMergue++;
                    ws.Cell(index, subToMergue).Value = $"{item.AttendancePercentage}%";
                    ws.Cell(index, subToMergue).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(index, subToMergue).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Cell(index, subToMergue).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell(index, subToMergue).Style.Border.OutsideBorderColor = XLColor.Black;
                    index++;
                }

                ws.Columns().AdjustToContents();
                ws.Rows().AdjustToContents();
                //ws.Cells().Style.Alignment.WrapText = true;

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
