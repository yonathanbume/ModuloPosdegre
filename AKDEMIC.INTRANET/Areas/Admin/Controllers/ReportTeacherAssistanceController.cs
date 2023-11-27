using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.ReportTeacherAssistanceViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using ClosedXML.Excel;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Helpers.ConstantHelpers;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," +
        ConstantHelpers.ROLES.REPORT_QUERIES + "," +
        ConstantHelpers.ROLES.DEAN + "," +
        ConstantHelpers.ROLES.DEAN_SECRETARY + "," +
        ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," +
        ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        ConstantHelpers.ROLES.VICERRECTOR)]
    [Area("Admin")]
    [Route("admin/reporte_asistencia_docentes")]
    public class ReportTeacherAssistanceController : BaseController
    {
        private readonly IViewRenderService _viewRenderService;
        private readonly ITeacherService _teacherService;
        private readonly IClassService _classService;
        private readonly IConverter _dinkConverter;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ITermService _termService;
        private readonly ITeacherScheduleService _teacherScheduleService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportTeacherAssistanceController(
            AkdemicContext context,
            IViewRenderService viewRenderService,
            IDataTablesService dataTablesService,
            ITeacherService teacherService,
            IClassService classService,
            IConverter dinkConverter,
            IWebHostEnvironment hostingEnvironment,
            ITermService termService,
            ITeacherScheduleService teacherScheduleService,
            IWebHostEnvironment webHostEnvironment
        ) : base(context, dataTablesService)
        {
            _viewRenderService = viewRenderService;
            _teacherService = teacherService;
            _classService = classService;
            _dinkConverter = dinkConverter;
            _hostingEnvironment = hostingEnvironment;
            _termService = termService;
            _teacherScheduleService = teacherScheduleService;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Vista de reporte de asistencia de docentes
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de docentes para ser usado en tablas
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <param name="academicDepartmentId">identificador del departamento académico</param>
        /// <returns>Objeto que contiene el listado de docentes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetStudentFilter(string search, Guid? academicDepartmentId, Guid? termId)
        {
            var parameters = _dataTablesService.GetSentParameters();

            if (!termId.HasValue)
            {
                var activeTerm = await _termService.GetActiveTerm();
                termId = activeTerm?.Id;
            }

            var result = await _teacherService.GetAllByTermandCareer2(parameters, termId ?? Guid.Empty, null, null, null, search, academicDepartmentId, User);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de faltas del docente
        /// </summary>
        /// <param name="academicDepartmentId">Identificador del departamento académico</param>
        /// <param name="endDate">fecha fin</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de faltas</returns>
        [HttpGet("reporte-faltas-get")]
        public async Task<IActionResult> GetReportMissing(Guid? academicDepartmentId, string endDate, string search, Guid? termId)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var endDateTime = ConvertHelpers.DatepickerToUtcDateTime(endDate);

            if (!termId.HasValue)
            {
                var term = await _termService.GetActiveTerm();
                termId = term?.Id;
            }

            var result = await _teacherService.GetTeacherClassesWithoutAttendance(parameters, termId ?? Guid.Empty, academicDepartmentId, search, endDateTime, User);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de clases asignadas al docente
        /// </summary>
        /// <param name="teacherId">Identificador del docente</param>
        /// <param name="endDate">Fecha fin</param>
        /// <returns>Listado de clases</returns>
        [HttpGet("get-detalle-falta")]
        public async Task<IActionResult> GetReportMissingDetailed(Guid? termId, string teacherId, string endDate)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var endDateTime = ConvertHelpers.DatepickerToUtcDateTime(endDate);

            if (!termId.HasValue)
            {
                var activeTerm = await _termService.GetActiveTerm();
                termId = activeTerm?.Id;
            }

            var result = await _teacherScheduleService.GetTeacherClassesDetailedWithoutAttendance(parameters, termId ?? Guid.Empty, endDateTime, teacherId);
            return Ok(result);
        }

        /// <summary>
        /// Vista detalle de las clases asignadas al docente
        /// </summary>
        /// <param name="id">Identificador del docente</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            var model = await _context.Teachers
                .Where(x => x.UserId == id)
                .Select(x => new TeacherViewModel
                {
                    TeacherId = x.UserId,
                    UserName = x.User.UserName,
                    Name = x.User.FullName,
                    AcademicDepartment = x.AcademicDepartment.Name,
                }).FirstOrDefaultAsync();

            return View(model);
        }

        /// <summary>
        /// Obtiene el listado de clases del horario del docente
        /// </summary>
        /// <param name="id">Identificador del docente</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <param name="start">Fecha de inicio</param>
        /// <param name="end">Fecha fin</param>
        /// <returns>Objeto que contiene el listado de clases</returns>
        [HttpGet("get-horario/{id}/{termId}")]
        public async Task<IActionResult> GetClasses(string id, Guid termId, DateTime start, DateTime end)
        {
            var dateStart = start.ToUniversalTime();
            var dateEnd = end.ToUniversalTime();

            return Ok(await _classService.GetAllByTermIdTeacherIdAndDateRange(termId, id, dateStart, dateEnd));
        }

        /// <summary>
        /// Obtiene el listado de secciones asignadas al docente
        /// </summary>
        /// <param name="teacherId">Identificador del docente</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Listado de secciones</returns>
        [HttpGet("secciones/get")]
        public async Task<IActionResult> GetSectionAbsenceDetail(string teacherId, Guid termId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            var query = _context.TeacherSections
                .Where(x => x.TeacherId == teacherId && x.Section.CourseTerm.TermId == termId)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var teacherClasses = await _context.Classes
                .Where(x => x.Section.CourseTerm.TermId == termId && x.ClassSchedule.TeacherSchedules.Any(y => y.TeacherId == teacherId))
                //.Select()
                .ToListAsync();

            var dataDB = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    course = $"{x.Section.CourseTerm.Course.Code}-{x.Section.CourseTerm.Course.Name}",
                    section = x.Section.Code,
                    sectionId = x.SectionId
                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.course,
                    x.section,
                    totalClasses = teacherClasses.Where(t => t.SectionId == x.sectionId).Count(),
                    dictatedClasses = teacherClasses.Where(t => t.SectionId == x.sectionId && t.IsDictated).Count(),
                    rescheduledClasses = teacherClasses.Where(t => t.SectionId == x.sectionId && t.IsRescheduled).Count(),
                })
                .ToList();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return Ok(result);
        }

        /// <summary>
        /// Genera el reporte de secciones asignadas al docente
        /// </summary>
        /// <param name="teacherId">Identificador del docente</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Archivo en formato EXCEL</returns>
        [HttpGet("secciones/get/reporte-excel")]
        public async Task<IActionResult> GetReportExcel(string teacherId, Guid termId)
        {
            var query = _context.TeacherSections
               .Where(x => x.TeacherId == teacherId && x.Section.CourseTerm.TermId == termId)
               .AsNoTracking();

            var teacherClasses = await _context.Classes
                .Where(x => x.Section.CourseTerm.TermId == termId && x.ClassSchedule.TeacherSchedules.Any(y => y.TeacherId == teacherId))
                .ToListAsync();

            var dataDB = await query
                .Select(x => new
                {
                    course = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                    sectionId = x.SectionId
                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.course,
                    x.section,
                    totalClasses = teacherClasses.Where(t => t.SectionId == x.sectionId).Count(),
                    dictatedClasses = teacherClasses.Where(t => t.SectionId == x.sectionId && t.IsDictated).Count(),
                    rescheduledClasses = teacherClasses.Where(t => t.SectionId == x.sectionId && t.IsRescheduled).Count()
                })
                .ToList();

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Curso");
            dt.Columns.Add("Sección");
            dt.Columns.Add("Clases Dictadas");
            dt.Columns.Add("Clases Reprogramadas");
            dt.Columns.Add("Clases Totales");

            foreach (var item in data)
                dt.Rows.Add(item.course, item.section, item.dictatedClasses, item.rescheduledClasses, item.totalClasses);

            dt.AcceptChanges();

            //var img = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png");

            var fileName = $"Reporte de Asistencia.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Reporte de Asistencia", null);

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        /// <summary>
        /// Genera el reporte de secciones asignadas al docente
        /// </summary>
        /// <param name="teacherId">Identificador del docente</param>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("secciones/get/reporte-pdf")]
        public async Task<IActionResult> GetReportPdf(string teacherId, Guid termId)
        {
            var query = _context.TeacherSections
               .Where(x => x.TeacherId == teacherId && x.Section.CourseTerm.TermId == termId)
               .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var teacherClasses = await _context.Classes
                .Where(x => x.Section.CourseTerm.TermId == termId && x.ClassSchedule.TeacherSchedules.Any(y => y.TeacherId == teacherId))
                .ToListAsync();

            var dataDB = await query
                .Select(x => new
                {
                    course = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                    sectionId = x.SectionId
                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new ReportSectionAssistanceViewModel
                {
                    Course = x.course,
                    Section = x.section,
                    TotalClasses = teacherClasses.Where(t => t.SectionId == x.sectionId).Count(),
                    DictatedClasses = teacherClasses.Where(t => t.SectionId == x.sectionId && t.IsDictated).Count(),
                    RescheduledClasses = teacherClasses.Where(t => t.SectionId == x.sectionId && t.IsRescheduled).Count()
                })
                .ToList();

            var userTeacher = await _context.Users.Where(x => x.Id == teacherId).FirstOrDefaultAsync();
            var academicDepartment = await _context.Teachers.Where(x => x.UserId == teacherId).Select(x => x.AcademicDepartment.Name).FirstOrDefaultAsync();
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();

            var model = new TeacherViewModel
            {
                Name = userTeacher.FullName,
                UserName = userTeacher.UserName,
                Term = term.Name,
                AcademicDepartment = academicDepartment,
                Sections = data
            };

            model.Image = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png");

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                DocumentTitle = $"REPORTE DE ASISTENCIA"
            };
            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/ReportTeacherAssistance/ReportSectionPdf.cshtml", model);
            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8" },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };
            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var fileByte = _dinkConverter.Convert(pdf);
            return File(fileByte, "application/pdf");
        }

        /// <summary>
        /// Genera el reporte detallado de las clases asignadas al docente
        /// </summary>
        /// <param name="academicDepartmentId">Identificador del departamento académico</param>
        /// <param name="endDate">Fecha fin</param>
        /// <returns>Archivo en formato EXCEL</returns>
        [HttpGet("detallado-excel")]
        public async Task<IActionResult> GetDetailedReportExcel(Guid? academicDepartmentId, string endDate, string startDate, Guid termId)
        {
            var term = await _termService.Get(termId);

            var startDateTime = ConvertHelpers.DatepickerToUtcDateTime(startDate);
            var endDateTime = ConvertHelpers.DatepickerToUtcDateTime(endDate);

            var query = _context.TeacherSections
                .Where(x => x.Section.CourseTerm.TermId == term.Id)
                .AsNoTracking();

            var teacherClassesQuery = _context.Classes
                .Where(x => x.Section.CourseTerm.TermId == term.Id && x.EndTime <= endDateTime.Date && x.EndTime >= startDateTime.Date)
                .AsNoTracking();

            if (academicDepartmentId.HasValue)
            {
                query = query.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);
                teacherClassesQuery = teacherClassesQuery.Where(x => x.Section.TeacherSections.Any(y => y.Teacher.AcademicDepartmentId == academicDepartmentId));
            }

            var teacherClasses = await teacherClassesQuery
                .Select(x => new
                {
                    x.Id,
                    x.SectionId,
                    x.IsDictated,
                    x.IsRescheduled,
                    teachersId = x.ClassSchedule.TeacherSchedules.Select(y => y.TeacherId).ToList()
                })
                .ToListAsync();

            var dataDB = await query
               .Select(x => new
               {
                   teacherId = x.TeacherId,
                   username = x.Teacher.User.UserName,
                   teacher = x.Teacher.User.FullName,
                   courseCode = x.Section.CourseTerm.Course.Code,
                   course = x.Section.CourseTerm.Course.Name,
                   academicYears = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear).ToList(),
                   section = x.Section.Code,
                   sectionId = x.SectionId,
                   academicDepartment = x.Teacher.AcademicDepartment.Name
               })
               .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.username,
                    x.teacher,
                    x.courseCode,
                    x.course,
                    x.academicDepartment,
                    x.section,
                    academicYear = string.Join(" ,", x.academicYears.Select(y => ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[y]).ToList()),
                    totalClasses = teacherClasses.Where(t => t.SectionId == x.sectionId && t.teachersId.Any(y => y == x.teacherId)).Count(),
                    dictatedClasses = teacherClasses.Where(t => t.SectionId == x.sectionId && t.IsDictated && t.teachersId.Any(y => y == x.teacherId)).Count(),
                    rescheduledClasses = teacherClasses.Where(t => t.SectionId == x.sectionId && t.IsRescheduled && t.teachersId.Any(y => y == x.teacherId)).Count()
                })
                .ToList();

            var img = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png");

            var fileName = $"Reporte de Asistencia.xlsx";

            using (var wb = new XLWorkbook())
            {
                var ws = wb.AddWorksheet("Reporte Detallado");

                var mergeRangeColumn = 'H';

                if (!string.IsNullOrEmpty(img))
                {
                    //using (var memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(img)))
                    //{
                    //    ws.AddPicture(memoryStream).MoveTo(ws.Cell("A1")).Scale(0.5);
                    //}
                }

                ws.Cell(2, 1).Value = CORE.Helpers.GeneralHelpers.GetInstitutionName().ToUpper();
                ws.Cell(2, 1).Style.Font.FontSize = 12;
                ws.Cell(2, 1).Style.Font.Bold = true;
                ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range($"A2:{mergeRangeColumn}2").Merge();
                ws.Cell(3, 1).Value = "Reporte de Asistencia Detallado";

                ws.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(3, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range($"A3:{mergeRangeColumn}3").Merge();

                ws.Cell(5, 1).Value = "Departamento Académico";
                ws.Cell(5, 1).Style.Font.Bold = true;
                if (academicDepartmentId.HasValue)
                {
                    var academicDepartment = await _context.AcademicDepartments.Where(x => x.Id == academicDepartmentId).FirstOrDefaultAsync();
                    ws.Cell(5, 2).Value = academicDepartment.Name;
                }
                else
                {
                    ws.Cell(5, 2).Value = "Todos";
                }
                var percentage = data.Select(x => x.totalClasses).Sum() > 0 ? $"{Math.Round(data.Select(x => x.dictatedClasses).Sum() * 100M / data.Select(x => x.totalClasses).Sum(), 2, MidpointRounding.AwayFromZero)}%" : "0%";

                ws.Cell(6, 1).Value = "Rango de Fechas";
                ws.Cell(6, 1).Style.Font.Bold = true;
                ws.Cell(6, 2).Value = $"{startDate} - {endDate}";
                ws.Cell(7, 1).Value = "Porcentaje de Cumplimiento";
                ws.Cell(7, 1).Style.Font.Bold = true;
                ws.Cell(7, 2).Value = percentage;

                ws.Cell(9, 1).Value = "Departamento Académico";
                ws.Cell(9, 1).Style.Font.FontSize = 11;
                ws.Cell(9, 1).Style.Font.Bold = true;
                ws.Cell(9, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 1).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 1).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 2).Value = "Nombre del Docente";
                ws.Cell(9, 2).Style.Font.FontSize = 11;
                ws.Cell(9, 2).Style.Font.Bold = true;
                ws.Cell(9, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 2).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 2).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 3).Value = "Curso";
                ws.Cell(9, 3).Style.Font.FontSize = 11;
                ws.Cell(9, 3).Style.Font.Bold = true;
                ws.Cell(9, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 3).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 3).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 4).Value = "Semestre";
                ws.Cell(9, 4).Style.Font.FontSize = 11;
                ws.Cell(9, 4).Style.Font.Bold = true;
                ws.Cell(9, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 4).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 4).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 5).Value = "Sección";
                ws.Cell(9, 5).Style.Font.FontSize = 11;
                ws.Cell(9, 5).Style.Font.Bold = true;
                ws.Cell(9, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 5).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 5).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 6).Value = "Clases Dictadas";
                ws.Cell(9, 6).Style.Font.FontSize = 11;
                ws.Cell(9, 6).Style.Font.Bold = true;
                ws.Cell(9, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 6).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 6).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 7).Value = "Clases Reprogramadas";
                ws.Cell(9, 7).Style.Font.FontSize = 11;
                ws.Cell(9, 7).Style.Font.Bold = true;
                ws.Cell(9, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 7).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 7).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 8).Value = "Clases Totales";
                ws.Cell(9, 8).Style.Font.FontSize = 11;
                ws.Cell(9, 8).Style.Font.Bold = true;
                ws.Cell(9, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 8).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 8).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 9).Value = "% Cumplimiento";
                ws.Cell(9, 9).Style.Font.FontSize = 11;
                ws.Cell(9, 9).Style.Font.Bold = true;
                ws.Cell(9, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 9).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 9).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 9).Style.Fill.BackgroundColor = XLColor.LightGray;


                var index = 10;
                foreach (var item in data.GroupBy(x => x.username).ToList())
                {
                    var toAdd = item.Count();
                    var toMergue = index + toAdd - 1;
                    ws.Cell(index, 1).Value = item.Select(x => x.academicDepartment).FirstOrDefault();
                    ws.Range(index, 1, toMergue, 1).Merge();
                    ws.Range(index, 1, toMergue, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(index, 1, toMergue, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(index, 1, toMergue, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(index, 1, toMergue, 1).Style.Border.OutsideBorderColor = XLColor.Black;


                    ws.Cell(index, 2).Value = item.Select(x => x.teacher).FirstOrDefault();
                    ws.Range(index, 2, toMergue, 2).Merge();
                    ws.Range(index, 2, toMergue, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(index, 2, toMergue, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(index, 2, toMergue, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(index, 2, toMergue, 2).Style.Border.OutsideBorderColor = XLColor.Black;

                    foreach (var detail in data.Where(x => x.username == item.Key))
                    {
                        ws.Cell(index, 3).Value = $"{detail.courseCode}-{detail.course}";
                        ws.Cell(index, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(index, 3).Style.Border.OutsideBorderColor = XLColor.Black;

                        ws.Cell(index, 4).Value = detail.academicYear;
                        ws.Cell(index, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(index, 4).Style.Border.OutsideBorderColor = XLColor.Black;

                        ws.Cell(index, 5).Value = detail.section;
                        ws.Cell(index, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(index, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                        ws.Cell(index, 6).Value = detail.dictatedClasses;
                        ws.Cell(index, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(index, 6).Style.Border.OutsideBorderColor = XLColor.Black;

                        ws.Cell(index, 7).Value = detail.rescheduledClasses;
                        ws.Cell(index, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(index, 7).Style.Border.OutsideBorderColor = XLColor.Black;

                        ws.Cell(index, 8).Value = detail.totalClasses;
                        ws.Cell(index, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(index, 8).Style.Border.OutsideBorderColor = XLColor.Black;

                        if (detail.totalClasses == 0)
                        {
                            ws.Cell(index, 9).Value = $"Sin clases progamadas";
                        }
                        else
                        {
                            ws.Cell(index, 9).Value = $"{Math.Round((detail.dictatedClasses * 100M / (decimal)detail.totalClasses), 2, MidpointRounding.AwayFromZero)}%";
                        }

                        ws.Cell(index, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(index, 9).Style.Border.OutsideBorderColor = XLColor.Black;

                        index++;
                    }
                }

                ws.Cells().Style.Alignment.WrapText = true;
                ws.Columns().AdjustToContents();
                ws.Rows().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }

        }

        /// <summary>
        /// Genera el reporte consolidado de las clases asignadas al docente
        /// </summary>
        /// <param name="academicDepartmentId">Identificador del departamento académico</param>
        /// <param name="endDate">Fecha fin</param>
        /// <returns>Archivo en formato EXCEL</returns>
        [HttpGet("consolidado-excel")]
        public async Task<IActionResult> GetConsolidatedReportExcel(Guid? academicDepartmentId, string endDate, string startDate, Guid termId)
        {
            var term = await _termService.Get(termId);

            var startDateTime = ConvertHelpers.DatepickerToUtcDateTime(startDate);
            var endDateTime = ConvertHelpers.DatepickerToUtcDateTime(endDate);

            var query = _context.TeacherSections
                .Where(x => x.Section.CourseTerm.TermId == term.Id)
                .AsNoTracking();

            var teacherClassesQuery = _context.Classes
                .Where(x => x.Section.CourseTerm.TermId == term.Id && x.EndTime <= endDateTime && x.EndTime >= startDateTime)
                .AsNoTracking();

            if (academicDepartmentId.HasValue)
            {
                query = query.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);
                teacherClassesQuery = teacherClassesQuery.Where(x => x.Section.TeacherSections.Any(y => y.Teacher.AcademicDepartmentId == academicDepartmentId));
            }

            var teacherClasses = await teacherClassesQuery
                .Select(x => new
                {
                    x.Id,
                    x.SectionId,
                    x.IsDictated,
                    x.IsRescheduled,
                    teachersId = x.ClassSchedule.TeacherSchedules.Select(y => y.TeacherId).ToList()
                })
                .ToListAsync();

            var pedagogical_hour_time_configuration = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME).FirstOrDefaultAsync();

            if (pedagogical_hour_time_configuration == null)
            {
                pedagogical_hour_time_configuration = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME,
                    Value = ConstantHelpers.Configuration.Enrollment.DEFAULT_VALUES[ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME]
                };
            }

            var peddagogicalHourTimeValue = Convert.ToInt32(pedagogical_hour_time_configuration.Value);

            var dataDB = await query
               .Select(x => new
               {
                   teacherId = x.TeacherId,
                   username = x.Teacher.User.UserName,
                   teacher = x.Teacher.User.FullName,
                   courseCode = x.Section.CourseTerm.Course.Code,
                   course = x.Section.CourseTerm.Course.Name,
                   academicYears = x.Section.CourseTerm.Course.AcademicYearCourses.Select(y => y.AcademicYear).ToList(),
                   section = x.Section.Code,
                   sectionId = x.SectionId,
                   academicDepartment = x.Teacher.AcademicDepartment.Name,
                   schedules = x.Section.ClassSchedules.Where(y => y.TeacherSchedules.Any(z => z.TeacherId == x.TeacherId)).Select(y => new
                   {
                       y.StartTime,
                       y.EndTime
                   })
                   .ToList()
               })
               .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.username,
                    x.teacher,
                    x.courseCode,
                    x.course,
                    x.academicDepartment,
                    x.section,
                    academicYear = string.Join(" ,", x.academicYears.Select(y => ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[y]).ToList()),
                    totalClasses = teacherClasses.Where(t => t.SectionId == x.sectionId && t.teachersId.Any(y => y == x.teacherId)).Count(),
                    dictatedClasses = teacherClasses.Where(t => t.SectionId == x.sectionId && t.IsDictated && t.teachersId.Any(y => y == x.teacherId)).Count(),
                    rescheduledClasses = teacherClasses.Where(t => t.SectionId == x.sectionId && t.IsRescheduled && t.teachersId.Any(y => y == x.teacherId)).Count(),
                    hours = Math.Round(x.schedules.Sum(y => y.EndTime.ToLocalTimeSpanUtc().Subtract(y.StartTime.ToLocalTimeSpanUtc()).TotalMinutes / peddagogicalHourTimeValue), 1, MidpointRounding.AwayFromZero)
                })
                .ToList();

            var img = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png");

            var fileName = $"Reporte de Asistencia.xlsx";

            using (var wb = new XLWorkbook())
            {
                var ws = wb.AddWorksheet("Reporte Consolidado");

                var mergeRangeColumn = 'I';

                //if (!string.IsNullOrEmpty(img))
                //{
                //    using (var memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(img)))
                //    {
                //        ws.AddPicture(memoryStream).MoveTo(ws.Cell("A1")).Scale(0.5);
                //    }
                //}

                ws.Cell(2, 1).Value = CORE.Helpers.GeneralHelpers.GetInstitutionName().ToUpper();
                ws.Cell(2, 1).Style.Font.FontSize = 12;
                ws.Cell(2, 1).Style.Font.Bold = true;
                ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range($"A2:{mergeRangeColumn}2").Merge();

                ws.Cell(3, 1).Value = "Reporte de Asistencia Consolidado";
                ws.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(3, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range($"A3:{mergeRangeColumn}3").Merge();

                ws.Cell(5, 1).Value = "Departamento Académico";
                ws.Cell(5, 1).Style.Font.Bold = true;
                if (academicDepartmentId.HasValue)
                {
                    var academicDepartment = await _context.AcademicDepartments.Where(x => x.Id == academicDepartmentId).FirstOrDefaultAsync();
                    ws.Cell(5, 2).Value = academicDepartment.Name;
                }
                else
                {
                    ws.Cell(5, 2).Value = "Todos";
                }
                var percentage = data.Select(x => x.totalClasses).Sum() > 0 ? $"{Math.Round(data.Select(x => x.dictatedClasses).Sum() * 100M / data.Select(x => x.totalClasses).Sum(), 2, MidpointRounding.AwayFromZero)}%" : "0.0";
                ws.Cell(6, 1).Value = "Rango de Fechas";
                ws.Cell(6, 1).Style.Font.Bold = true;
                ws.Cell(6, 2).Value = $"{startDate} - {endDate}";
                ws.Cell(7, 1).Value = "Porcentaje de Cumplimiento";
                ws.Cell(7, 1).Style.Font.Bold = true;
                ws.Cell(7, 2).Value = percentage;

                ws.Cell(9, 1).Value = "Departamento Académico";
                ws.Cell(9, 1).Style.Font.FontSize = 11;
                ws.Cell(9, 1).Style.Font.Bold = true;
                ws.Cell(9, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 1).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 1).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 2).Value = "Nombre del Docente";
                ws.Cell(9, 2).Style.Font.FontSize = 11;
                ws.Cell(9, 2).Style.Font.Bold = true;
                ws.Cell(9, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 2).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 2).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 3).Value = "Curso";
                ws.Cell(9, 3).Style.Font.FontSize = 11;
                ws.Cell(9, 3).Style.Font.Bold = true;
                ws.Cell(9, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 3).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 3).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 4).Value = "Semestre";
                ws.Cell(9, 4).Style.Font.FontSize = 11;
                ws.Cell(9, 4).Style.Font.Bold = true;
                ws.Cell(9, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 4).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 4).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 5).Value = "Sección";
                ws.Cell(9, 5).Style.Font.FontSize = 11;
                ws.Cell(9, 5).Style.Font.Bold = true;
                ws.Cell(9, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 5).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 5).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 6).Value = "Horas Pedagógicas";
                ws.Cell(9, 6).Style.Font.FontSize = 11;
                ws.Cell(9, 6).Style.Font.Bold = true;
                ws.Cell(9, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 6).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 6).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 7).Value = "Sesiones de \n Aprendizaje \n Totales";
                ws.Cell(9, 7).Style.Font.FontSize = 11;
                ws.Cell(9, 7).Style.Font.Bold = true;
                ws.Cell(9, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 7).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 7).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 8).Value = "Sesiones de \n Aprendizaje \n Dictadas";
                ws.Cell(9, 8).Style.Font.FontSize = 11;
                ws.Cell(9, 8).Style.Font.Bold = true;
                ws.Cell(9, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 8).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 8).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Cell(9, 9).Value = "Porcentaje de \n cumplimiento de \n Actividades lectivas";
                ws.Cell(9, 9).Style.Font.FontSize = 11;
                ws.Cell(9, 9).Style.Font.Bold = true;
                ws.Cell(9, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(9, 9).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell(9, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Cell(9, 9).Style.Border.OutsideBorderColor = XLColor.Black;
                ws.Cell(9, 9).Style.Fill.BackgroundColor = XLColor.LightGray;

                var index = 10;
                foreach (var item in data.GroupBy(x => x.username).ToList())
                {
                    var toAdd = item.Count();
                    var toMergue = index + toAdd - 1;
                    ws.Cell(index, 1).Value = item.Select(x => x.academicDepartment).FirstOrDefault();
                    ws.Range(index, 1, toMergue, 1).Merge();
                    ws.Range(index, 1, toMergue, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(index, 1, toMergue, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(index, 1, toMergue, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(index, 1, toMergue, 1).Style.Border.OutsideBorderColor = XLColor.Black;

                    ws.Cell(index, 2).Value = item.Select(x => x.teacher).FirstOrDefault();
                    ws.Range(index, 2, toMergue, 2).Merge();
                    ws.Range(index, 2, toMergue, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(index, 2, toMergue, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(index, 2, toMergue, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(index, 2, toMergue, 2).Style.Border.OutsideBorderColor = XLColor.Black;

                    ws.Cell(index, 7).Value = item.Select(x => x.totalClasses).Sum();
                    ws.Range(index, 7, toMergue, 7).Merge();
                    ws.Range(index, 7, toMergue, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(index, 7, toMergue, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(index, 7, toMergue, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(index, 7, toMergue, 7).Style.Border.OutsideBorderColor = XLColor.Black;

                    ws.Cell(index, 8).Value = item.Select(x => x.dictatedClasses).Sum();
                    ws.Range(index, 8, toMergue, 8).Merge();
                    ws.Range(index, 8, toMergue, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(index, 8, toMergue, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(index, 8, toMergue, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(index, 8, toMergue, 8).Style.Border.OutsideBorderColor = XLColor.Black;


                    if (item.Select(x => x.totalClasses).Sum() == 0)
                    {
                        ws.Cell(index, 9).Value = $"{0M}%";
                    }
                    else
                    {
                        ws.Cell(index, 9).Value = $"{Math.Round((item.Select(x => x.dictatedClasses).Sum() * 100M / item.Select(x => x.totalClasses).Sum()), 2, MidpointRounding.AwayFromZero)}%";
                    }
                    ws.Range(index, 9, toMergue, 9).Merge();
                    ws.Range(index, 9, toMergue, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(index, 9, toMergue, 9).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(index, 9, toMergue, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(index, 9, toMergue, 9).Style.Border.OutsideBorderColor = XLColor.Black;

                    var dataDetailed = data.Where(x => x.username == item.Key).ToList();
                    foreach (var detail in dataDetailed)
                    {
                        ws.Cell(index, 3).Value = $"{detail.courseCode}-{detail.course}";
                        ws.Cell(index, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(index, 3).Style.Border.OutsideBorderColor = XLColor.Black;

                        ws.Cell(index, 4).Value = detail.academicYear;
                        ws.Cell(index, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(index, 4).Style.Border.OutsideBorderColor = XLColor.Black;

                        ws.Cell(index, 5).Value = detail.section;
                        ws.Cell(index, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(index, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                        ws.Cell(index, 6).Value = detail.hours;
                        ws.Cell(index, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(index, 6).Style.Border.OutsideBorderColor = XLColor.Black;
                        index++;
                    }
                }

                ws.Cells().Style.Alignment.WrapText = true;
                ws.Columns().AdjustToContents();
                ws.Rows().AdjustToContents();


                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }

        }

        /// <summary>
        /// Genera el reporte de faltas de las clases asignadas al docente
        /// </summary>
        /// <param name="academicDepartmentId">Identificador del departamento académico</param>
        /// <param name="endDate">Fecha fin</param>
        /// <returns>Archivo en formato EXCEL</returns>
        [HttpGet("reporte-faltas-excel")]
        public async Task<IActionResult> GetReportMissingExcel(Guid? academicDepartmentId, string endDate, Guid termId)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            var endDateTime = ConvertHelpers.DatepickerToUtcDateTime(endDate);
            var query = _context.TeacherSections.Where(x => x.Section.CourseTerm.TermId == term.Id && x.Section.ClassSchedules.Any(y => y.Classes.Any(z => !z.IsDictated && z.EndTime <= endDateTime))).AsNoTracking();

            if (academicDepartmentId.HasValue && academicDepartmentId != null)
                query = query.Where(x => x.Teacher.AcademicDepartmentId == academicDepartmentId);

            var dataDB = await query
                .Select(x => new
                {
                    teacherId = x.TeacherId,
                    teacher = x.Teacher.User.FullName,
                    courseCode = x.Section.CourseTerm.Course.Code,
                    course = x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                    sectionId = x.Section.Id
                })
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.teacher,
                    x.courseCode,
                    x.course,
                    x.section,
                    clases = string.Join(", ", _context.Classes.Where(y => y.ClassSchedule.TeacherSchedules.Any(z => z.TeacherId == x.teacherId) && y.SectionId == x.sectionId && !y.IsDictated && y.EndTime <= endDateTime).Select(y => $"{y.StartTime.ToLocalDateTimeFormat()}-{y.EndTime.ToLocalDateTimeFormat()}").ToList())
                })
                .ToList();

            var dt = new DataTable
            {
                TableName = "Reporte de clases no dictadas"
            };

            dt.Columns.Add("Nombre del Docente");
            dt.Columns.Add("Curso");
            dt.Columns.Add("Sección");
            dt.Columns.Add("Clases No Dictadas");

            foreach (var item in data)
                dt.Rows.Add(item.teacher, $"{item.courseCode}-{item.course}", item.section, item.clases);

            dt.AcceptChanges();

            var img = Path.Combine(_hostingEnvironment.WebRootPath, @"images/themes/" + CORE.Helpers.GeneralHelpers.GetTheme() + @"/logo-report.png");

            string fileName = $"Reporte de Clases No Dictadas.xlsx";
            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                //ws.AddHeaderToWorkSheet("Reporte de Clases No Dictadas", img);
                ws.AddHeaderToWorkSheet("Reporte de Clases No Dictadas", null);

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }

        }

    }
}
